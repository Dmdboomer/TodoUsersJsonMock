using System.Text.Json;
using TodoApp.Models;

namespace TodoApp.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://jsonplaceholder.typicode.com";

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var response = await _httpClient.GetStringAsync($"{BaseUrl}/users");
        var jsonElements = JsonSerializer.Deserialize<JsonElement[]>(response)!;
        
        var users = new List<User>();
        foreach (var jsonElement in jsonElements)
        {
            var user = User.FromJson(jsonElement);
            user.Validate();
            users.Add(user);
        }
        
        return users;
    }

    public async Task<List<Todo>> GetTodosAsync()
    {
        var response = await _httpClient.GetStringAsync($"{BaseUrl}/todos");
        var jsonElements = JsonSerializer.Deserialize<JsonElement[]>(response)!;
        
        var todos = new List<Todo>();
        foreach (var jsonElement in jsonElements)
        {
            var todo = Todo.FromJson(jsonElement);
            todo.Validate();
            todos.Add(todo);
        }
        
        return todos;
    }
}