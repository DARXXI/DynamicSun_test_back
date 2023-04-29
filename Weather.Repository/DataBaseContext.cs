using System.Data;
using System.Reflection;
using Weather.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace Weather.Repository
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        public DbSet<Weather.Domain.Entities.Weather> Weather { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.LoadFrom(Path.Combine(path, "Weather.Repository.dll")));
        }
    }
}