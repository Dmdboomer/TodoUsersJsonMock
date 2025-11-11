using System.Text.Json;

namespace TodoApp.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? CompanyName { get; set; }
    public string? City { get; set; }

    public static User FromJson(JsonElement json)
    {
        return new User
        {
            Id = json.GetProperty("id").GetInt32(),
            Name = json.GetProperty("name").GetString() ?? string.Empty,
            Username = json.GetProperty("username").GetString() ?? string.Empty,
            Email = json.GetProperty("email").GetString() ?? string.Empty,
            Phone = json.GetProperty("phone").GetString(),
            Website = json.GetProperty("website").GetString(),
            CompanyName = json.GetProperty("company").GetProperty("name").GetString(),
            City = json.GetProperty("address").GetProperty("city").GetString()
        };
    }

    public void Validate()
    {
        if (Id <= 0) throw new ArgumentException("User Id is required");
        if (string.IsNullOrEmpty(Name)) throw new ArgumentException("User Name is required");
        if (string.IsNullOrEmpty(Username)) throw new ArgumentException("Username is required");
        if (string.IsNullOrEmpty(Email)) throw new ArgumentException("Email is required");
    }
}