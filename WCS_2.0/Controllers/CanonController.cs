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
    public class CanonController
    {
        public static void EnviarDadosCanon(Printers canon)
        {
            using (var db = new PrinterMonitoringContext())
            {
                try
                {
                    var existingPrinter = db.PrinterMonitoring
                        .FirstOrDefault(p => p.Patrimonio == canon.Patrimonio);

                    if (existingPrinter == null)
                    {

                        db.PrinterMonitoring.Add(canon);
                        //existingPrinter.InstituicaoId = canon.InstituicaoId;
                        //existingPrinter.Localizacao = canon.Localizacao;
                        //existingPrinter.SerialTonnerPreto = canon.SerialTonnerPreto;

                        Console.WriteLine(existingPrinter);
                        db.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Utils.Log("Dados da impressora CANON enviados com sucesso para o banco de dados. -\n ");
                    }
                    else
                    {
                        existingPrinter.Ip = canon.Ip;
                        existingPrinter.Ativa = canon.Ativa;
                        existingPrinter.AbrSecretaria = canon.AbrSecretaria;
                        existingPrinter.Secretaria = canon.Secretaria;
                        existingPrinter.Depto = canon.Depto;
                        existingPrinter.InstituicaoId = canon.InstituicaoId;
                        existingPrinter.Localizacao = canon.Localizacao;
                        existingPrinter.Patrimonio = canon.Patrimonio;

                        db.SaveChanges();

                        SaveChangesInLogs(db, existingPrinter.Id, canon);
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

        private static void SaveChangesInLogs(PrinterMonitoringContext db, int printerId, Printers canon)
        {
            if (!HasChanges(db, printerId, canon))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"A impressora {canon.Patrimonio} já foi salva hoje.\n");
                return; 
            }

            var newLog = new PrinterStatusLogs
            {
                PrinterId = printerId,
                QuantidadeImpressaoTotal = canon.QuantidadeImpressaoTotal,
                PorcentagemBlack = canon.PorcentagemBlack,
                PorcentagemCyan = canon.PorcentagemCyan,
                PorcentagemYellow = canon.PorcentagemYellow,
                PorcentagemMagenta = canon.PorcentagemMagenta,
                PorcentagemFusor = canon.PorcentagemFusor,
                PorcentagemBelt = canon.PorcentagemBelt,
                PorcentagemUnidadeImagem = canon.PorcentagemUnidadeImagem,
                PrinterStatus = canon.PrinterStatus,
                DataHoraDeBusca = DateTime.Now,
            };

            db.PrinterStatusLogs.Add(newLog);
            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Utils.Log("Alterações na impressora CANON registradas com sucesso no banco de dados. - ");
        }

        private static bool HasChanges(PrinterMonitoringContext db, int printerId, Printers canon)
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

            return lastLog.QuantidadeImpressaoTotal != canon.QuantidadeImpressaoTotal ||
                   lastLog.PorcentagemBlack != canon.PorcentagemBlack ||
                   lastLog.PorcentagemCyan != canon.PorcentagemCyan ||
                   lastLog.PorcentagemYellow != canon.PorcentagemYellow ||
                   lastLog.PorcentagemMagenta != canon.PorcentagemMagenta ||
                   lastLog.PorcentagemFusor != canon.PorcentagemFusor ||
                   lastLog.PorcentagemBelt != canon.PorcentagemBelt ||
                   lastLog.PorcentagemUnidadeImagem != canon.PorcentagemUnidadeImagem ||
                   lastLog.PrinterStatus != canon.PrinterStatus;
        }
    }
}
