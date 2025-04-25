

namespace WAD.Models;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        return obj is User user && Id == user.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
