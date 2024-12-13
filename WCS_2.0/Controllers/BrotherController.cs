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
    public class BrotherController
    {
        public static void EnviarDadosBrother(Printers brother)
        {
            using (var db = new PrinterMonitoringContext())
            {
                try
                {
                    var existingPrinter = db.PrinterMonitoring
                        .FirstOrDefault(p => p.Patrimonio == brother.Patrimonio);

                    if (existingPrinter == null)
                    {

                        db.PrinterMonitoring.Add(brother);
                        //existingPrinter.SerialTonnerPreto = brother.SerialTonnerPreto;
                        //existingPrinter.InstituicaoId = brother.InstituicaoId;
                        //existingPrinter.Localizacao = brother.Localizacao;

                        Console.WriteLine(existingPrinter);
                        db.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Utils.Log("Dados da impressora BROTHER enviados com sucesso para o banco de dados. - ");
                    }

                    else
                    {
                        existingPrinter.Ip = brother.Ip;
                        existingPrinter.Ativa = brother.Ativa;
                        existingPrinter.AbrSecretaria = brother.AbrSecretaria;
                        existingPrinter.Secretaria = brother.Secretaria;
                        existingPrinter.Depto = brother.Depto;
                        existingPrinter.InstituicaoId = brother.InstituicaoId;
                        existingPrinter.Localizacao = brother.Localizacao;
                        existingPrinter.Patrimonio = brother.Patrimonio;

                        db.SaveChanges();

                        SaveChangesInLogs(db, existingPrinter.Id, brother);
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

        private static void SaveChangesInLogs(PrinterMonitoringContext db, int printerId, Printers brother)
        {
            if (!HasChanges(db, printerId, brother))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"A impressora {brother.Patrimonio} já foi salva hoje.\n");
                return; 
            }

            var newLog = new PrinterStatusLogs
            {
                PrinterId = printerId,
                QuantidadeImpressaoTotal = brother.QuantidadeImpressaoTotal,
                PorcentagemBlack = brother.PorcentagemBlack,
                PorcentagemCyan = brother.PorcentagemCyan,
                PorcentagemYellow = brother.PorcentagemYellow,
                PorcentagemMagenta = brother.PorcentagemMagenta,
                PorcentagemFusor = brother.PorcentagemFusor,
                PorcentagemBelt = brother.PorcentagemBelt,
                PorcentagemUnidadeImagem = brother.PorcentagemUnidadeImagem,
                PrinterStatus = brother.PrinterStatus,
                DataHoraDeBusca = DateTime.Now,
            };

            db.PrinterStatusLogs.Add(newLog);
            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Utils.Log("Alterações na impressora BROTHER registradas com sucesso no banco de dados. -\n ");
        }

        private static bool HasChanges(PrinterMonitoringContext db, int printerId, Printers brother)
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

            return lastLog.QuantidadeImpressaoTotal != brother.QuantidadeImpressaoTotal ||
                   lastLog.PorcentagemBlack != brother.PorcentagemBlack ||
                   lastLog.PorcentagemCyan != brother.PorcentagemCyan ||
                   lastLog.PorcentagemYellow != brother.PorcentagemYellow ||
                   lastLog.PorcentagemMagenta != brother.PorcentagemMagenta ||
                   lastLog.PorcentagemFusor != brother.PorcentagemFusor ||
                   lastLog.PorcentagemBelt != brother.PorcentagemBelt ||
                   lastLog.PorcentagemUnidadeImagem != brother.PorcentagemUnidadeImagem ||
                   lastLog.PrinterStatus != brother.PrinterStatus;
        }
    }
}