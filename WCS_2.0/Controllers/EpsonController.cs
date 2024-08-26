using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.Data;
using WCS.Utilities;
using WCS;

namespace WCS_2._0.Controllers
{
    public class EpsonController
    {
        public static void EnviarDadosEpson(Printers epson)
        {
            using (var db = new PrinterMonitoringContext())
            {
                try
                {
                    db.PrinterMonitoringTESTE.Add(epson);
                    db.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Utils.Log("Dados da impressora EPSON enviados com sucesso para o banco de dados. - ");
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
