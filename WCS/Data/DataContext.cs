using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Lexmark> PrinterMonitorings { get; set; }

        public DataContext() : base("name=PrinterMonitoringContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your models if needed
        }
    }
}
