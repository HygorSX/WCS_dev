using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WCS.Data;
using WCS.Utilities;
using WCS;
using WCS_2._0.Models;

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
                    var existingPrinter = db.PrinterMonitoringTESTE
                        .FirstOrDefault(p => p.Ip == epson.Ip);

                    if (existingPrinter != null)
                    {

                        //db.PrinterMonitoringTESTE.Add(epson);
                        //existingPrinter.SerialTonnerPreto = epson.SerialTonnerPreto;
                        existingPrinter.InstituicaoId = epson.InstituicaoId;
                        Console.WriteLine(existingPrinter);
                        db.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Utils.Log("Dados da impressora EPSON enviados com sucesso para o banco de dados. - ");
                    }
                    else
                    {
                        // Se a impressora já existir, registre as alterações no log
                        SaveChangesInLogs(db, existingPrinter.Id, epson);
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

        private static void SaveChangesInLogs(PrinterMonitoringContext db, int printerId, Printers epson)
        {
            if (!HasChanges(db, printerId, epson))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"A impressora {epson.Patrimonio} já foi salva hoje.\n");
                return; // Não faz nada se já foi salva hoje
            }

            var newLog = new PrinterStatusLogs
            {
                PrinterId = printerId,
                QuantidadeImpressaoTotal = epson.QuantidadeImpressaoTotal,
                PorcentagemBlack = epson.PorcentagemBlack,
                PorcentagemCyan = epson.PorcentagemCyan,
                PorcentagemYellow = epson.PorcentagemYellow,
                PorcentagemMagenta = epson.PorcentagemMagenta,
                PorcentagemFusor = epson.PorcentagemFusor,
                PorcentagemBelt = epson.PorcentagemBelt,
                PorcentagemUnidadeImagem = epson.PorcentagemUnidadeImagem,
                PrinterStatus = epson.PrinterStatus,
                DataHoraDeBusca = DateTime.Now,
            };

            db.PrinterStatusLogs.Add(newLog);
            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Utils.Log("Alterações na impressora EPSON registradas com sucesso no banco de dados. - ");
        }

        private static bool HasChanges(PrinterMonitoringContext db, int printerId, Printers epson)
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

            return lastLog.QuantidadeImpressaoTotal != epson.QuantidadeImpressaoTotal ||
                   lastLog.PorcentagemBlack != epson.PorcentagemBlack ||
                   lastLog.PorcentagemCyan != epson.PorcentagemCyan ||
                   lastLog.PorcentagemYellow != epson.PorcentagemYellow ||
                   lastLog.PorcentagemMagenta != epson.PorcentagemMagenta ||
                   lastLog.PorcentagemFusor != epson.PorcentagemFusor ||
                   lastLog.PorcentagemBelt != epson.PorcentagemBelt ||
                   lastLog.PorcentagemUnidadeImagem != epson.PorcentagemUnidadeImagem ||
                   lastLog.PrinterStatus != epson.PrinterStatus;
        }
    }
}
