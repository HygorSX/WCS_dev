using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.Data;
using WCS.Utilities;
using WCS;
using WCS_2._0.Models;

namespace WCS_2._0.Controllers
{
    public class RicohController
    {
        public static void EnviarDadosRicoh(Printers ricoh)
        {
            using (var db = new PrinterMonitoringContext())
            {
                try
                {
                    var existingPrinter = db.PrinterMonitoring
                        .FirstOrDefault(p => p.Patrimonio == ricoh.Patrimonio);

                    if (existingPrinter == null)
                    {

                        db.PrinterMonitoring.Add(ricoh);
                        //existingPrinter.InstituicaoId = ricoh.InstituicaoId;
                        //existingPrinter.Localizacao = ricoh.Localizacao;
                        Console.WriteLine(existingPrinter);
                        db.SaveChanges();


                        Console.ForegroundColor = ConsoleColor.Green;
                        Utils.Log("Dados da impressora RICOH enviados com sucesso para o banco de dados. - \n");
                    }
                    else
                    {
                        existingPrinter.Ip = ricoh.Ip;
                        existingPrinter.Ativa = ricoh.Ativa;
                        existingPrinter.AbrSecretaria = ricoh.AbrSecretaria;
                        existingPrinter.Secretaria = ricoh.Secretaria;
                        existingPrinter.Depto = ricoh.Depto;
                        existingPrinter.InstituicaoId = ricoh.InstituicaoId;
                        existingPrinter.Localizacao = ricoh.Localizacao;
                        existingPrinter.Patrimonio = ricoh.Patrimonio;

                        db.SaveChanges();

                        SaveChangesInLogs(db, existingPrinter.Id, ricoh);
                    }
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

        private static void SaveChangesInLogs(PrinterMonitoringContext db, int printerId, Printers ricoh)
        {
            if (!HasChanges(db, printerId, ricoh))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"A impressora {ricoh.Patrimonio} já foi salva hoje.\n");
                return; 
            }

            var newLog = new PrinterStatusLogs
            {
                PrinterId = printerId,
                QuantidadeImpressaoTotal = ricoh.QuantidadeImpressaoTotal,
                PorcentagemBlack = ricoh.PorcentagemBlack,
                PorcentagemCyan = ricoh.PorcentagemCyan,
                PorcentagemYellow = ricoh.PorcentagemYellow,
                PorcentagemMagenta = ricoh.PorcentagemMagenta,
                PorcentagemFusor = ricoh.PorcentagemFusor,
                PorcentagemBelt = ricoh.PorcentagemBelt,
                PorcentagemUnidadeImagem = ricoh.PorcentagemUnidadeImagem,
                PrinterStatus = ricoh.PrinterStatus,
                DataHoraDeBusca = DateTime.Now,
            };

            db.PrinterStatusLogs.Add(newLog);
            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Utils.Log("Alterações na impressora RICOH registradas com sucesso no banco de dados. - ");
        }

        private static bool HasChanges(PrinterMonitoringContext db, int printerId, Printers ricoh)
        {
            if (db.PrinterStatusLogs.Any(log => log.PrinterId == printerId && log.DataHoraDeBusca.Date == DateTime.Now.Date))
            {
                return false; 
            }

            var lastLog = db.PrinterStatusLogs
                .Where(l => l.PrinterId == printerId)
                .OrderByDescending(l => l.DataHoraDeBusca)
                .FirstOrDefault();

            if (lastLog == null) return true;

            return lastLog.QuantidadeImpressaoTotal != ricoh.QuantidadeImpressaoTotal ||
                   lastLog.PorcentagemBlack != ricoh.PorcentagemBlack ||
                   lastLog.PorcentagemCyan != ricoh.PorcentagemCyan ||
                   lastLog.PorcentagemYellow != ricoh.PorcentagemYellow ||
                   lastLog.PorcentagemMagenta != ricoh.PorcentagemMagenta ||
                   lastLog.PorcentagemFusor != ricoh.PorcentagemFusor ||
                   lastLog.PorcentagemBelt != ricoh.PorcentagemBelt ||
                   lastLog.PorcentagemUnidadeImagem != ricoh.PorcentagemUnidadeImagem ||
                   lastLog.PrinterStatus != ricoh.PrinterStatus;
        }
    }
}
