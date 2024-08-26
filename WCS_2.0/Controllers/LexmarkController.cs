using Microsoft.EntityFrameworkCore;
using WCS.Data;
using WCS.Utilities;

namespace WCS.Controllers
{
    public class LexmarkController
    {
        public static void EnviarDadosLexmark(Printers lexmark)
        {
            using (var db = new PrinterMonitoringContext())
            {
                try
                {
                    db.PrinterMonitoringTESTE.Add(lexmark);
                    db.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Utils.Log("Dados da impressora LEXMARK enviados com sucesso para o banco de dados. - ");
                }
                catch (DbUpdateException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
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
                    Console.ForegroundColor = ConsoleColor.DarkRed;
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
