namespace HmctsDts.Server.Entities;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string StaffId { get; set; }
    public required byte[] Hash { get; set; }
    public required byte[] Salt { get; set; }
}