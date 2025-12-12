using ciberpaz_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ciberpaz_api.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<Section> Sections { get; set; }

    }
}
