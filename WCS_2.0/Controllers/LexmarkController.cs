using Microsoft.EntityFrameworkCore;
using WCS.Data;
using WCS.Utilities;

namespace WCS.Controllers
{
    public class LexmarkController
    {
        public static void EnviarDadosLexmark(Lexmark lexmark)
        {
            using (var db = new PrinterMonitoringContext())
            {
                try
                {
                    db.PrinterMonitoringTESTE.Add(lexmark);
                    //db.Entry(lexmark).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    Utils.Log("Dados da impressora enviados com sucesso para o banco de dados.");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Erro ao enviar dados da impressora para o banco de dados: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Detalhes da exceção interna: " + ex.InnerException.Message);
                    }
                    if (ex.InnerException?.InnerException != null)
                    {
                        Console.WriteLine("Mais detalhes da exceção interna: " + ex.InnerException.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro inesperado: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Detalhes da exceção interna: " + ex.InnerException.Message);
                    }
                }
            }
        }
    }
}
