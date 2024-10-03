﻿using Microsoft.EntityFrameworkCore;
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
                    var existingPrinter = db.PrinterMonitoringTESTE
                        .FirstOrDefault(p => p.Ip == lexmark.Ip);

                    if (existingPrinter == null)
                    {
                        // Adiciona a impressora se ainda não existir na tabela
                        db.PrinterMonitoringTESTE.Add(lexmark);
                        db.SaveChanges();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Utils.Log("Dados da impressora LEXMARK enviados com sucesso para o banco de dados. - ");
                    }
                    else
                    {
                        // Se a impressora já existir, registre as alterações no log
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
            var lastLog = db.PrinterStatusLogs
                .Where(l => l.PrinterId == printerId)
                .OrderByDescending(l => l.DataHoraDeBusca)
                .FirstOrDefault();

            if (lastLog == null || HasChanges(lastLog, lexmark))
            {
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
                    PorcentagemKitManutencao = lexmark.PorcentagemKitManutencao,
                    PrinterStatus = lexmark.PrinterStatus,
                    DataHoraDeBusca = DateTime.Now,
                };

                db.PrinterStatusLogs.Add(newLog);
                db.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Green;
                Utils.Log("Alterações na impressora LEXMARK registradas com sucesso no banco de dados. - ");
            }
        }

        private static bool HasChanges(PrinterStatusLogs lastLog, Printers lexmark)
        {
            return lastLog.QuantidadeImpressaoTotal != lexmark.QuantidadeImpressaoTotal ||
                   lastLog.PorcentagemBlack != lexmark.PorcentagemBlack ||
                   lastLog.PorcentagemCyan != lexmark.PorcentagemCyan ||
                   lastLog.PorcentagemYellow != lexmark.PorcentagemYellow ||
                   lastLog.PorcentagemMagenta != lexmark.PorcentagemMagenta ||
                   lastLog.PorcentagemFusor != lexmark.PorcentagemFusor ||
                   lastLog.PorcentagemBelt != lexmark.PorcentagemBelt ||
                   lastLog.PorcentagemKitManutencao != lexmark.PorcentagemKitManutencao ||
                   lastLog.PrinterStatus != lexmark.PrinterStatus;
        }
    }
}
