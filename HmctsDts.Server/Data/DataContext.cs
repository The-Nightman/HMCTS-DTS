using HmctsDts.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace HmctsDts.Server.Data;


public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}