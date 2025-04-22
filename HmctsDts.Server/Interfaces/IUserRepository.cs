using HmctsDts.Server.Entities;

namespace HmctsDts.Server.Interfaces;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserByStaffId(string staffId);
}