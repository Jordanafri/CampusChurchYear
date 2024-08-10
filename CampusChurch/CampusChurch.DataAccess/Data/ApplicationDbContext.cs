using CampusChurch.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CampusChurch.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Series> Series { get; set; }
        public DbSet<Sermon> Sermons { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Ensure relationships are correctly set up
            modelBuilder.Entity<Sermon>()
                .HasOne(s => s.Series)
                .WithMany()
                .HasForeignKey(s => s.SeriesId);
        }
    }
}
