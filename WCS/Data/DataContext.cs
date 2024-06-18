using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.Data
{
    public class PrinterMonitoringContext : DbContext
    {

        public PrinterMonitoringContext() : base("name=PrinterMonitoringContext")
        {
        }
        public DbSet<Lexmark> DbLexmark { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your models if needed
        }
    }
}
