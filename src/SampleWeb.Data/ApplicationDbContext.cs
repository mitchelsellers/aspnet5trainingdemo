using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SampleWeb.Data.Models;

namespace SampleWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Helpful snippets
            //For Migrations
            //dotnet ef --startup-project ../SampleWeb migrations add Initial

            //Apply changes
            //dotnet ef --startup-project ../SampleWeb database update
        }

        public DbSet<TrainingEvent> TrainingEvents { get; set; }
        public DbSet<TrainingClass> TrainingClasses { get; set; }
    }
}
