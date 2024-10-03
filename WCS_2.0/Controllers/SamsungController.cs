﻿using Microsoft.EntityFrameworkCore;
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
            var lastLog = db.PrinterStatusLogs
                .Where(l => l.PrinterId == printerId)
                .OrderByDescending(l => l.DataHoraDeBusca)
                .FirstOrDefault();

            if (lastLog == null || HasChanges(lastLog, samsung))
            {
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
        }

        private static bool HasChanges(PrinterStatusLogs lastLog, Printers samsung)
        {
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