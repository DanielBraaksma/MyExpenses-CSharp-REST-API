using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyExpenses.API.Data
{
    public class ExpensesAuthDbContext: IdentityDbContext
    {
        public ExpensesAuthDbContext(DbContextOptions<ExpensesAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var userRoleId = "fababfac-d9ce-487b-a3c0-3088a63d0784";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
