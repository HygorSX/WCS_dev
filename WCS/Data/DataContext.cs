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
            this.Configuration.AutoDetectChangesEnabled = true;
        }
        public DbSet<Lexmark> PrinterMonitorings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Lexmark
            modelBuilder.Entity<Lexmark>()
                .HasKey(l => l.Id); // Definindo a chave primária

            // Outras configurações podem ser adicionadas aqui
        }
    }
}
