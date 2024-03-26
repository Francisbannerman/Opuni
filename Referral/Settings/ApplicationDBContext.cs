using Microsoft.EntityFrameworkCore;
using Referral.Model;

namespace Referral.Settings;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Client> Clients { get; set; }
}