using System.Text.Json.Serialization;

namespace UserService.Application.Contracts.Models;

public class UserModel
{
    public Guid Id { get; set; }

    public required string UserName { get; set; }

    public required string Email { get; set; }

    public string? Photo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? Roles { get; set; } = null;
}
