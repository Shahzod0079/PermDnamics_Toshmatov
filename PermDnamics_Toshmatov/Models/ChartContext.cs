using System;
using Microsoft.EntityFrameworkCore;

namespace PermDnamics_Toshmatov.Models
{
    public class ChartContext : DbContext
    {
        public DbSet<ChartSave> ChartSaves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=charts.db");
    }

    public class ChartSave
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }  
        public DateTime SaveDate { get; set; }
    }
}