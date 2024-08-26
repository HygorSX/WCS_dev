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
using System.Text;
using System.Threading.Tasks;
using WCS.Controllers;
using WCS.Repositories;
using WCS.Utilities;
using WCS_2._0.Controllers;
using WCS_2._0.Repositories;

namespace WCS
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var infoImpressoras = Utils.GetImpressoras();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            foreach (var impressora in infoImpressoras)
            {
                //if(impressora.Marca != "LEXMARK" && impressora.Marca != "EPSON") { continue; }
                if (impressora.Marca != "SAMSUNG") { continue; }
                if (TestePing(impressora.IP))
                {
                    bool isMono = Utils.VerificarMono(impressora.Suprimentos);
                    var snmpResults = ObterDadosSnmp(impressora.IP, isMono, impressora.Marca);
                    if(snmpResults.Count != 0)
                    {
                        var impressoraData = AnalisarResultadosSnmp(snmpResults, isMono, impressora.Marca);
                        Utils.SalvarResultados(impressoraData, isMono, $"C:\\WFS\\Test-{impressora.Id}.txt", impressora.Marca);
                        await Console.Out.WriteAsync($"{impressora.IP}\n");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        await Console.Out.WriteLineAsync($"Você não tem permissão para acessar as informações desta impressora! - {impressora.IP} - {impressora.Id} - {impressora.Marca}\n");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    await Console.Out.WriteLineAsync($"Não foi possível entrar em contato com a impressora - {impressora.IP} - {impressora.Id} - {impressora.Marca}\n");
                }
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
            //List<string> oids = isMono ? LexmarkRepository.GetMonoOidsLex() : LexmarkRepository.GetColorOidsLex();

          
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
                else
                {
                    Console.WriteLine("COLOR - Impressora Não Listada");
                    oids = null;
                }
            }
            else // Se não for mono, então é colorido
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
                else
                {
                    Console.WriteLine("COLOR - Impressora Não Listada");
                    oids = null;
                }
            }

            return ObterDadosSnmp(ip, oids);
        }



        private static Printers AnalisarResultadosSnmp(Dictionary<Oid, AsnType> snmpResults, bool isMono, dynamic marca)
        {
            Printers printer = new Printers();
            string[] resultado = snmpResults.Values.Select(v => v.ToString()).ToArray();
            //lexmark.Id = 0;
            if (isMono)
            {
                if(marca == "LEXMARK")
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
            }
            else
            {
                if (marca == "LEXMARK")
                {
                    printer = LexmarkRepository.AnalisarDadosColorLex(resultado, printer);
                }
                else if(marca == "EPSON")
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
            }

            return printer;
        }



        private static bool TestePing(string ip)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(ip);
                return pingReply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                Utils.Log($"Erro no ping: {ex.Message}");
                return false;
            }
        }
    }
}