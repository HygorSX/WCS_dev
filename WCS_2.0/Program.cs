using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WCS.Controllers;
using WCS.Data;
using WCS.Repositories;
using WCS.Utilities;
using WCS_2._0.Repositories;

namespace WCS
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TimeSpan _startHour = new TimeSpan(11, 59, 0);
            TimeSpan _endHour = new TimeSpan(13, 0, 0);
            var now = DateTime.Now.TimeOfDay;
            var stopwatch = new Stopwatch();
            List<Printers> printers = new List<Printers>();

            stopwatch.Start();

            var infoImpressoras = Utils.GetImpressoras();

            Parallel.ForEach(infoImpressoras, impressora =>
            {
                if (TestePing((string)impressora.IP))
                {
                    bool isMono = Utils.VerificarMono(impressora.Suprimentos);
                    var snmpResults = ObterDadosSnmp(impressora.IP, isMono, impressora.Marca);

                    if (snmpResults.Count != 0)
                    {
                        Printers impressoraData = AnalisarResultadosSnmp(snmpResults, isMono, impressora.Marca);
                        impressoraData.Ativa = 1;

                        impressoraData.Patrimonio = impressora.Patrimonio;
                        impressoraData.Secretaria = impressora.Secretaria;
                        impressoraData.AbrSecretaria = impressora.AbrSecretaria;
                        impressoraData.Depto = impressora.Depto;
                        impressoraData.Ip = impressora.IP;
                        impressoraData.InstituicaoId = impressora.InstituicaoId;
                        impressoraData.Localizacao = impressora.Localizacao;

                        lock (printers)
                        {
                            if (impressoraData.PorcentagemBlack <= 20)
                            {
                                printers.Add(impressoraData);
                            }
                        }

                        Utils.SalvarResultados(impressoraData, isMono, $"C:\\WFS\\Test-{impressora.Id}.txt", impressora.Marca);
                    }
                    else
                    {
                        AtualizarStatusImpressoraInativa(impressora.IP, impressora.Id, impressora.Marca);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Out.WriteLineAsync($"Essa impressora pode estar cadastrada de maneira errada! - {impressora.IP} - {impressora.Id} - {impressora.Marca}\n");
                    }
                }
                else
                {
                    AtualizarStatusImpressoraInativa(impressora.IP, impressora.Id, impressora.Marca);
                }
            });

            if (printers.Count > 0 && (now >= _startHour && now < _endHour))
            {
                Utils.EnviarEmailTonnerMinimo(printers);
            }

            stopwatch.Stop();
            Console.WriteLine($"Tempo de execução: {stopwatch.Elapsed}");
        }




        private static Dictionary<Oid, AsnType> ObterDadosSnmp(string ip, List<string> oids)
        {
            SimpleSnmp snmp = new SimpleSnmp(ip, "public");
            Pdu pdu = new Pdu(PduType.Get);

            foreach (var oid in oids)
            {
                pdu.VbList.Add(oid);
            }

            Dictionary<Oid, AsnType> snmpResults = snmp.Get(SnmpVersion.Ver1, pdu);
            if (snmpResults == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Utils.Log("Erro ao obter dados SNMP.\n");
                return new Dictionary<Oid, AsnType>();
            }

            return snmpResults;
        }



        private static Dictionary<Oid, AsnType> ObterDadosSnmp(string ip, bool isMono, dynamic marca)
        {

            List<string> oids;

            if (isMono)
            {
                if (marca == "LEXMARK")
                {
                    oids = LexmarkRepository.GetMonoOidsLex();
                }
                else if (marca == "EPSON")
                {
                    oids = EpsonRepository.GetMonoOidsEps();
                }
                else if (marca == "SAMSUNG")
                {
                    oids = SamsungRepository.GetMonoOidsSam();
                }
                else if (marca == "BROTHER")
                {
                    oids = BrotherRepository.GetMonoOidsBth();
                }
                else if (marca == "RICOH")
                {
                    oids = RicohRepository.GetMonoOidsRic();
                }
                else if (marca == "CANON")
                {
                    oids = CanonRepository.GetMonoOidsCan();
                }
                else
                {
                    Console.WriteLine("COLOR - Impressora Não Listada");
                    oids = null;
                }
            }
            else 
            {
                if (marca == "LEXMARK")
                {
                    oids = LexmarkRepository.GetColorOidsLex();
                }
                else if (marca == "EPSON")
                {
                    oids = EpsonRepository.GetColorOidsEps();
                }
                else if (marca == "SAMSUNG")
                {
                    oids = SamsungRepository.GetColorOidsSam();
                }
                else if (marca == "BROTHER")
                {
                    oids = BrotherRepository.GetColorOidsBth();
                }
                else if (marca == "RICOH")
                {
                    oids = RicohRepository.GetColorOidsRic();
                }
                else if (marca == "CANON")
                {
                    oids = CanonRepository.GetColorOidsCan();
                }
                else
                {
                    Console.WriteLine("COLOR - Impressora Não Listada");
                    oids = null;
                }
            }

            if (oids == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Nenhum OID encontrado para a marca: {marca}");
                return new Dictionary<Oid, AsnType>();
            }

            return ObterDadosSnmp(ip, oids);
        }



        private static Printers AnalisarResultadosSnmp(Dictionary<Oid, AsnType> snmpResults, bool isMono, dynamic marca)
        {
            Printers printer = new Printers();
            string[] resultado = snmpResults.Values.Select(v => v.ToString()).ToArray();

            if (isMono)
            {
                if (marca == "LEXMARK")
                {
                    printer = LexmarkRepository.AnalisarDadosMonoLex(resultado, printer);
                }
                else if (marca == "EPSON")
                {
                    printer = EpsonRepository.AnalisarDadosMonoEps(resultado, printer);
                }
                else if (marca == "SAMSUNG")
                {
                    printer = SamsungRepository.AnalisarDadosMonoSam(resultado, printer);
                }
                else if (marca == "BROTHER")
                {
                    printer = BrotherRepository.AnalisarDadosMonoBth(resultado, printer);
                }
                else if (marca == "RICOH")
                {
                    printer = RicohRepository.AnalisarDadosMonoRic(resultado, printer);
                }
                else if (marca == "CANON")
                {
                    printer = CanonRepository.AnalisarDadosMonoCan(resultado, printer);
                }
            }
            else
            {
                if (marca == "LEXMARK")
                {
                    printer = LexmarkRepository.AnalisarDadosColorLex(resultado, printer);
                }
                else if (marca == "EPSON")
                {
                    printer = EpsonRepository.AnalisarDadosColorEps(resultado, printer);
                }
                else if (marca == "SAMSUNG")
                {
                    printer = SamsungRepository.AnalisarDadosColorSam(resultado, printer);
                }
                else if (marca == "BROTHER")
                {
                    printer = BrotherRepository.AnalisarDadosColorBth(resultado, printer);
                }
                else if (marca == "RICOH")
                {
                    printer = RicohRepository.AnalisarDadosColorRic(resultado, printer);
                }
                else if (marca == "RICOH")
                {
                    printer = CanonRepository.AnalisarDadosColorCan(resultado, printer);
                }
            }

            return printer;
        }

        public static void AtualizarStatusImpressoraInativa(string ip, int id, string marca)
        {
            using (var dbContext = new PrinterMonitoringContext()) 
            {
                var impressoraExistente = dbContext.PrinterMonitoring
                                                   .FirstOrDefault(p => p.Ip == ip);
                if (impressoraExistente != null)
                {
                    impressoraExistente.Ativa = 0;
                    dbContext.SaveChanges(); 

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Out.WriteLineAsync($"Impressora inativa e SALVA - {ip} - {id} - {marca}\n");
                }
            }
        }

        private static bool TestePing(string ip)
        {
            const int timeout = 1000;
            const int tentativas = 2;
            int falhas = 0;

            try
            {
                using (Ping ping = new Ping())
                {
                    for (int i = 0; i < tentativas; i++)
                    {
                        PingReply reply = ping.Send(ip, timeout);
                        if (reply.Status == IPStatus.Success)
                        {
                            return true;
                        }
                        falhas++;
                    }
                }
                return falhas == tentativas;
            }
            catch (Exception ex)
            {
                Utils.Log($"Erro no ping: {ex.Message}");
                return false;
            }
        }
    }
}