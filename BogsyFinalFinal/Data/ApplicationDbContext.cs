using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BogsyFinalFinal.Models;

namespace BogsyFinalFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BogsyFinalFinal.Models.Customers> Customers { get; set; } = default!;
        public DbSet<BogsyFinalFinal.Models.Rentals> Rentals { get; set; } = default!;
        public DbSet<BogsyFinalFinal.Models.Videos> Videos { get; set; } = default!;
    }
}
