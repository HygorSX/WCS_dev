using Microsoft.EntityFrameworkCore;
using WCS.Data;
using WCS.Utilities;
using WCS_2._0.Models;
using System;
using System.Linq;

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
                    var existingPrinter = db.PrinterMonitoring
                        .FirstOrDefault(p => p.Patrimonio == lexmark.Patrimonio);

                    if (existingPrinter == null)
                    {

                        db.PrinterMonitoring.Add(lexmark);
                        //existingPrinter.SerialTonnerPreto = lexmark.SerialTonnerPreto;
                        //existingPrinter.InstituicaoId = lexmark.InstituicaoId;
                        //existingPrinter.Localizacao = lexmark.Localizacao;
                        //existingPrinter.AbrSecretaria = lexmark.AbrSecretaria;
                        //existingPrinter.Secretaria = lexmark.Secretaria;
                        //existingPrinter.Depto = lexmark.Depto;
                        //existingPrinter.Ip = lexmark.Ip;
                        Console.WriteLine(existingPrinter);
                        db.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Utils.Log("Dados da impressora LEXMARK enviados com sucesso para o banco de dados. - \n");
                    }
                    else
                    {
                        existingPrinter.Ip = lexmark.Ip;
                        existingPrinter.Ativa = lexmark.Ativa;
                        existingPrinter.AbrSecretaria = lexmark.AbrSecretaria;
                        existingPrinter.Secretaria = lexmark.Secretaria;
                        existingPrinter.Depto = lexmark.Depto;
                        existingPrinter.InstituicaoId = lexmark.InstituicaoId;
                        existingPrinter.Localizacao = lexmark.Localizacao;
                        existingPrinter.Patrimonio = lexmark.Patrimonio;

                        db.SaveChanges();

                        SaveChangesInLogs(db, existingPrinter.Id, lexmark);
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

        private static void SaveChangesInLogs(PrinterMonitoringContext db, int printerId, Printers lexmark)
        {
            if (!HasChanges(db, printerId, lexmark))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"A impressora {lexmark.Patrimonio} já foi salva hoje ou não existe alterações.\n");
                return; 
            }

            var lastLog = db.PrinterStatusLogs
                .Where(l => l.PrinterId == printerId)
                .OrderByDescending(l => l.DataHoraDeBusca)
                .FirstOrDefault();

            var newLog = new PrinterStatusLogs
            {
                PrinterId = printerId,
                QuantidadeImpressaoTotal = lexmark.QuantidadeImpressaoTotal,
                PorcentagemBlack = lexmark.PorcentagemBlack,
                PorcentagemCyan = lexmark.PorcentagemCyan,
                PorcentagemYellow = lexmark.PorcentagemYellow,
                PorcentagemMagenta = lexmark.PorcentagemMagenta,
                PorcentagemFusor = lexmark.PorcentagemFusor,
                PorcentagemBelt = lexmark.PorcentagemBelt,
                PorcentagemUnidadeImagem = lexmark.PorcentagemUnidadeImagem,
                PorcentagemKitManutencao = lexmark.PorcentagemKitManutencao,
                PrinterStatus = lexmark.PrinterStatus,
                SerialTonnerPreto = lexmark.SerialTonnerPreto,
                DataHoraDeBusca = DateTime.Now,
            };

            db.PrinterStatusLogs.Add(newLog);
            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Utils.Log("Alterações na impressora LEXMARK registradas com sucesso no banco de dados. - ");
        }

        private static bool HasChanges(PrinterMonitoringContext db, int printerId, Printers lexmark)
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

            return lastLog.QuantidadeImpressaoTotal != lexmark.QuantidadeImpressaoTotal ||
                   lastLog.PorcentagemBlack != lexmark.PorcentagemBlack ||
                   lastLog.PorcentagemCyan != lexmark.PorcentagemCyan ||
                   lastLog.PorcentagemYellow != lexmark.PorcentagemYellow ||
                   lastLog.PorcentagemMagenta != lexmark.PorcentagemMagenta ||
                   lastLog.PorcentagemFusor != lexmark.PorcentagemFusor ||
                   lastLog.PorcentagemBelt != lexmark.PorcentagemBelt ||
                   lastLog.PorcentagemUnidadeImagem != lexmark.PorcentagemUnidadeImagem ||
                   lastLog.PorcentagemKitManutencao != lexmark.PorcentagemKitManutencao ||
                   lastLog.SerialTonnerPreto != lexmark.SerialTonnerPreto ||
                   lastLog.PrinterStatus != lexmark.PrinterStatus;
        }
    }
}
