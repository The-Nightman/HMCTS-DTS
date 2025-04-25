using HmctsDts.Server.Entities;
using HmctsDts.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HmctsDts.Server.Data.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task CreateUser(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByStaffId(string staffId)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.StaffId == staffId);
    }
}