using TodoApp.Services;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp;

class Program
{
    static async Task Main(string[] args)
    {

        String server = "localhost";
        String database = "TodoApp";

        // Connection string
        var connectionString = $"Server={server};Database={database};Trusted_Connection=true;TrustServerCertificate=true;";
        
        var apiService = new ApiService();
        var dbService = new DatabaseService(connectionString);

        try
        {
            Console.WriteLine("Fetching data from JSONPlaceholder API...");
            
            // Task 1: Fetch data
            var users = await apiService.GetUsersAsync();
            var todos = await apiService.GetTodosAsync();
            
            Console.WriteLine($"Users: {users.Count}");
            Console.WriteLine($"Todos: {todos.Count}");
            
            // Task 3 & 4: Create tables and insert data
            Console.WriteLine("\nCreating database tables...");
            await dbService.CreateTablesAsync();
            
            Console.WriteLine("Inserting users...");
            await dbService.InsertUsersAsync(users);
            
            Console.WriteLine("Inserting todos...");
            await dbService.InsertTodosAsync(todos);

            // Task 5: Execute join query
            await dbService.ExecuteQueryAsync();

            // Task 6: Create views
            Console.WriteLine("Creating Views");
            await dbService.CreateViewAsync();
            
            Console.WriteLine("\nAll tasks completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}