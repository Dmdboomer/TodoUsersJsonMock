using System.Data;
using Microsoft.Data.SqlClient;
using TodoApp.Models;

namespace TodoApp.Data;

public class DatabaseService
{
    private readonly string _connectionString;
    private readonly string _sqlScriptsPath;

    public DatabaseService(string connectionString, string sqlScriptsPath = "sql")
    {
        _connectionString = connectionString;
        _sqlScriptsPath = sqlScriptsPath;
    }
    private async Task<string> ReadSqlFileAsync(string fileName)
    {
        var filePath = Path.Combine(_sqlScriptsPath, fileName);
        return await File.ReadAllTextAsync(filePath);
    }

    public async Task CreateTablesAsync()
    {
        var sql = await ReadSqlFileAsync("01_create_tables.sql");
        
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    public async Task InsertUsersAsync(List<User> users)
    {
        const string sql = @"
            MERGE Users AS target
            USING (VALUES (@Id, @Name, @Username, @Email, @Phone, @Website, @CompanyName, @City)) 
            AS source (Id, Name, Username, Email, Phone, Website, CompanyName, City)
            ON target.Id = source.Id
            WHEN MATCHED THEN
                UPDATE SET Name = source.Name, Username = source.Username, Email = source.Email, 
                          Phone = source.Phone, Website = source.Website, 
                          CompanyName = source.CompanyName, City = source.City
            WHEN NOT MATCHED THEN
                INSERT (Id, Name, Username, Email, Phone, Website, CompanyName, City)
                VALUES (source.Id, source.Name, source.Username, source.Email, source.Phone, 
                        source.Website, source.CompanyName, source.City);";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        foreach (var user in users)
        {
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Phone", user.Phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Website", user.Website ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CompanyName", user.CompanyName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@City", user.City ?? (object)DBNull.Value);
            
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task InsertTodosAsync(List<Todo> todos)
    {
        const string sql = @"
            MERGE Todos AS target
            USING (VALUES (@Id, @UserId, @Title, @Completed)) 
            AS source (Id, UserId, Title, Completed)
            ON target.Id = source.Id
            WHEN MATCHED THEN
                UPDATE SET UserId = source.UserId, Title = source.Title, Completed = source.Completed
            WHEN NOT MATCHED THEN
                INSERT (Id, UserId, Title, Completed)
                VALUES (source.Id, source.UserId, source.Title, source.Completed);";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        foreach (var todo in todos)
        {
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", todo.Id);
            command.Parameters.AddWithValue("@UserId", todo.UserId);
            command.Parameters.AddWithValue("@Title", todo.Title);
            command.Parameters.AddWithValue("@Completed", todo.Completed);
            
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task ExecuteQueryAsync()
    {
        var sql = await ReadSqlFileAsync("02_query.sql");

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        Console.WriteLine("\nUser Todo Completion Report:");
        Console.WriteLine("UserID | Name | Completed | Total | Completion %");
        Console.WriteLine("------------------------------------------------");

        while (await reader.ReadAsync())
        {
            Console.WriteLine($"{reader["UserID"]} | {reader["Name"]} | {reader["CompletedCount"]} | {reader["TotalCount"]} | {reader["CompletionPct"]:P2}");
        }
    }

    public async Task CreateViewAsync()
    {
        var sql = await ReadSqlFileAsync("03_create_views.sql");

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}