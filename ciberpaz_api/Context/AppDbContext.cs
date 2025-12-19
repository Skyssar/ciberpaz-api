using ciberpaz_api.Models;
using Microsoft.EntityFrameworkCore;
using ciberpaz_api.DTOs;

namespace ciberpaz_api.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<Home> Homes { get; set; }
        public DbSet<ViewItem> ViewItems { get; set; }
        public DbSet<AppView> AppViews { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Multimedia> Multimedias { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Home>().ToTable("home");

            // VIEW ITEM
            modelBuilder.Entity<ViewItem>(entity =>
            {
                entity.ToTable("viewitem");

                entity.Property(v => v.Embed)
                      .HasDefaultValue(false);

                // Relación 1 (Home) -> N (ViewItem)
                entity.HasOne(v => v.Home)
                      .WithMany(h => h.ViewItems)
                      .HasForeignKey(v => v.HomeId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });
        

            modelBuilder.Entity<AppView>().ToTable("appviews");
            modelBuilder.Entity<Section>().ToTable("sections");
            modelBuilder.Entity<Paragraph>().ToTable("paragraphs");
            modelBuilder.Entity<Multimedia>().ToTable("multimedias");
            modelBuilder.Entity<Event>().ToTable("events");
            modelBuilder.Entity<Course>().ToTable("courses");
            modelBuilder.Entity<Volunteer>().ToTable("volunteers");
        }
        public DbSet<ciberpaz_api.DTOs.HomeDto> HomeDto { get; set; } = default!;
    }
}
