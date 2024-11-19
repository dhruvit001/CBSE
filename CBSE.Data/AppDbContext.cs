using CBSE.Entities;
using Microsoft.EntityFrameworkCore;
namespace CBSE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Marks> Marks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }

        // Configuring relationships, if necessary
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student and School relationship
            modelBuilder.Entity<Student>()
                .HasOne(s => s.School)
                .WithMany(sc => sc.Students)
                .HasForeignKey(s => s.SchoolId);

            modelBuilder.Entity<User>().
                HasOne(u => u.Roles)
                .WithMany(d => d.Users)
                .HasForeignKey(e => e.RoleId);
                

            // Configure Marks and Student relationship
            modelBuilder.Entity<Marks>()
                .HasOne(m => m.Student)
                .WithMany(s => s.Marks)
                .HasForeignKey(m => m.StudentId);
        }
    }

}
