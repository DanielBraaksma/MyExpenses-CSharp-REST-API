using Microsoft.EntityFrameworkCore;
using MyExpenses.API.Models.Domain;

namespace MyExpenses.API.Data
{
    public class BillsDbContext : DbContext
    {
        public BillsDbContext(DbContextOptions<BillsDbContext> dbContextOptions) : base(dbContextOptions)
        {
             
        }

        public DbSet<Bill> Bills{ get; set; }
    }
}
