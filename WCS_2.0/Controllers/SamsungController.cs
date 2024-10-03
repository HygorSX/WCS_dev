using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WCS.Data;
using WCS.Utilities;
using WCS;
using WCS_2._0.Models;

namespace WCS_2._0.Controllers
{
    public class SamsungController
    {
        public static void EnviarDadosSamsung(Printers samsung)
        {
            using (var db = new PrinterMonitoringContext())
            {
                try
                {
                    var existingPrinter = db.PrinterMonitoringTESTE
                        .FirstOrDefault(p => p.Ip == samsung.Ip);

                    if (existingPrinter == null)
                    {
                        // Adiciona a impressora se ainda não existir na tabela
                        db.PrinterMonitoringTESTE.Add(samsung);
                        db.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Utils.Log("Dados da impressora SAMSUNG enviados com sucesso para o banco de dados. - ");
                    }
                    else
                    {
                        // Se a impressora já existir, registre as alterações no log
                        SaveChangesInLogs(db, existingPrinter.Id, samsung);
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

        private static void SaveChangesInLogs(PrinterMonitoringContext db, int printerId, Printers samsung)
        {
            if (!HasChanges(db, printerId, samsung))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"A impressora {samsung.Patrimonio} já foi salva hoje.\n");
                return; // Não faz nada se já foi salva hoje
            }

            var newLog = new PrinterStatusLogs
            {
                PrinterId = printerId,
                QuantidadeImpressaoTotal = samsung.QuantidadeImpressaoTotal,
                PorcentagemBlack = samsung.PorcentagemBlack,
                PorcentagemCyan = samsung.PorcentagemCyan,
                PorcentagemYellow = samsung.PorcentagemYellow,
                PorcentagemMagenta = samsung.PorcentagemMagenta,
                PorcentagemFusor = samsung.PorcentagemFusor,
                PorcentagemBelt = samsung.PorcentagemBelt,
                PorcentagemKitManutencao = samsung.PorcentagemKitManutencao,
                PrinterStatus = samsung.PrinterStatus,
                DataHoraDeBusca = DateTime.Now,
            };

            db.PrinterStatusLogs.Add(newLog);
            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Utils.Log("Alterações na impressora SAMSUNG registradas com sucesso no banco de dados. - ");
        }

        private static bool HasChanges(PrinterMonitoringContext db, int printerId, Printers samsung)
        {
            // Verifica se já existe um log para hoje
            if (db.PrinterStatusLogs.Any(log => log.PrinterId == printerId && log.DataHoraDeBusca.Date == DateTime.Now.Date))
            {
                return false; // Retorna false se já foi coletado hoje
            }

            // Se não houver log para hoje, verifica se os dados mudaram
            var lastLog = db.PrinterStatusLogs
                .Where(l => l.PrinterId == printerId)
                .OrderByDescending(l => l.DataHoraDeBusca)
                .FirstOrDefault();

            if (lastLog == null) return true; // Se não houver log anterior, considera que há mudanças

            return lastLog.QuantidadeImpressaoTotal != samsung.QuantidadeImpressaoTotal ||
                   lastLog.PorcentagemBlack != samsung.PorcentagemBlack ||
                   lastLog.PorcentagemCyan != samsung.PorcentagemCyan ||
                   lastLog.PorcentagemYellow != samsung.PorcentagemYellow ||
                   lastLog.PorcentagemMagenta != samsung.PorcentagemMagenta ||
                   lastLog.PorcentagemFusor != samsung.PorcentagemFusor ||
                   lastLog.PorcentagemBelt != samsung.PorcentagemBelt ||
                   lastLog.PorcentagemKitManutencao != samsung.PorcentagemKitManutencao ||
                   lastLog.PrinterStatus != samsung.PrinterStatus;
        }
    }
}