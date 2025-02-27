using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using TOLED.Web.Data.Models;

namespace TOLED.Web.Data
{
    public class ToledDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<PPImage> Images { get; set; }
        public DbSet<PPDevice> Devices { get; set; }
        public DbSet<ImageCollection> ImageCollections { get; set; }

        private string DbPath { get; set; }

        public ToledDbContext()
        {
        }

        public ToledDbContext(DbContextOptions<ToledDbContext> options) : base(options)  
        {
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=toled.db");
    }

    public static class PPImageExtentions
    {
        internal static Size ParseSize(this string size)
        {
            var parts = size.Split(",");
            return new Size(int.Parse(parts[0]), int.Parse(parts[1]));
        }
    }
}
