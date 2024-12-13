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
                    var existingPrinter = db.PrinterMonitoring
                        .FirstOrDefault(p => p.Patrimonio == samsung.Patrimonio);

                    if (existingPrinter == null)
                    {

                        db.PrinterMonitoring.Add(samsung);
                        //existingPrinter.InstituicaoId = samsung.InstituicaoId;
                        //
                        //existingPrinter.SerialTonnerPreto = samsung.SerialTonnerPreto;
                        //existingPrinter.Localizacao = samsung.Localizacao;
                        Console.WriteLine(existingPrinter);
                        db.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Utils.Log("Dados da impressora SAMSUNG enviados com sucesso para o banco de dados. - \n");
                    }
                    else
                    {
                        existingPrinter.Ip = samsung.Ip;
                        existingPrinter.Ativa = samsung.Ativa;
                        existingPrinter.AbrSecretaria = samsung.AbrSecretaria;
                        existingPrinter.Secretaria = samsung.Secretaria;
                        existingPrinter.Depto = samsung.Depto;
                        existingPrinter.InstituicaoId = samsung.InstituicaoId;
                        existingPrinter.Localizacao = samsung.Localizacao;
                        existingPrinter.Patrimonio = samsung.Patrimonio;

                        db.SaveChanges();

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
                return; 
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
                PorcentagemUnidadeImagem = samsung.PorcentagemUnidadeImagem,
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
            if (db.PrinterStatusLogs.Any(log => log.PrinterId == printerId && log.DataHoraDeBusca.Date == DateTime.Now.Date))
            {
                return false; 
            }

            var lastLog = db.PrinterStatusLogs
                .Where(l => l.PrinterId == printerId)
                .OrderByDescending(l => l.DataHoraDeBusca)
                .FirstOrDefault();

            if (lastLog == null) return true;

            return lastLog.QuantidadeImpressaoTotal != samsung.QuantidadeImpressaoTotal ||
                   lastLog.PorcentagemBlack != samsung.PorcentagemBlack ||
                   lastLog.PorcentagemCyan != samsung.PorcentagemCyan ||
                   lastLog.PorcentagemYellow != samsung.PorcentagemYellow ||
                   lastLog.PorcentagemMagenta != samsung.PorcentagemMagenta ||
                   lastLog.PorcentagemFusor != samsung.PorcentagemFusor ||
                   lastLog.PorcentagemBelt != samsung.PorcentagemBelt ||
                   lastLog.PorcentagemUnidadeImagem != samsung.PorcentagemUnidadeImagem ||
                   lastLog.PrinterStatus != samsung.PrinterStatus;
        }
    }
}