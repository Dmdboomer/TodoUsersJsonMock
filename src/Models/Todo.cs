using System.Text.Json;

namespace TodoApp.Models;

public class Todo
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }

    public static Todo FromJson(JsonElement json)
    {
        return new Todo
        {
            Id = json.GetProperty("id").GetInt32(),
            UserId = json.GetProperty("userId").GetInt32(),
            Title = json.GetProperty("title").GetString() ?? string.Empty,
            Completed = json.GetProperty("completed").GetBoolean()
        };
    }

    public void Validate()
    {
        if (Id <= 0) throw new ArgumentException("Todo Id is required");
        if (UserId <= 0) throw new ArgumentException("User Id is required");
        if (string.IsNullOrEmpty(Title)) throw new ArgumentException("Title is required");
    }
}