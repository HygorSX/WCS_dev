using Microsoft.EntityFrameworkCore;

namespace WCS.Data
{
    public class PrinterMonitoringContext : DbContext
    {
        public DbSet<Lexmark> PrinterMonitoringTESTE { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=192.168.222.243; Database=wfs; User Id=sa; Password=sa; Connect Timeout=30; Encrypt=False; TrustServerCertificate=True; ApplicationIntent=ReadWrite; MultiSubnetFailover=False");
        }
    }
}
