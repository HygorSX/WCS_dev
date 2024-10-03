using Microsoft.EntityFrameworkCore;
using WCS_2._0.Models;

namespace WCS.Data
{
    public class PrinterMonitoringContext : DbContext
    {
        public DbSet<Printers> PrinterMonitoringTESTE { get; set; }
        public DbSet<PrinterStatusLogs> PrinterStatusLogs { get; set; }
        public DbSet<Equipamentoes> Equipamentoes { get; set; }
        public DbSet<EquipamentosMarcas> EquipamentoMarcas { get; set; }
        public DbSet<EquipamentoSecretarias> EquipamentoSecretarias { get; set; }
        public DbSet<EquipamentoSuprimentos> EquipamentoSuprimentos { get; set; }
        public DbSet<EquipamentoTipoes> EquipamentoTipoes { get; set; }
        public DbSet<Produtoes> Produtoes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=192.168.222.243; Database=wfs2; User Id=sa; Password=sa; Connect Timeout=30; Encrypt=False; TrustServerCertificate=True; ApplicationIntent=ReadWrite; MultiSubnetFailover=False");
        }
    }
}
