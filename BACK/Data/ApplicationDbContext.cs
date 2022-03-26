using BACK.Models;
using Microsoft.EntityFrameworkCore;

namespace BACK.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }
        public DbSet<Card> Cards { get; set; }
    }
}
