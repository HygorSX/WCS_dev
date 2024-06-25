using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using WCS_2._0.Repositories;

namespace WCS
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var infoImpressoras = Utils.GetImpressoras();
            foreach (var impressora in infoImpressoras)
            {
                //if(impressora.Marca != "LEXMARK") { continue; }
                if (TestePing(impressora.IP))
                {
                    bool isMono = Utils.VerificarMono(impressora.Suprimentos);
                    var snmpResults = ObterDadosSnmp(impressora.IP, isMono, impressora.Marca);
                    if(snmpResults.Count != 0)
                    {
                        var impressoraData = AnalisarResultadosSnmp(snmpResults, isMono, impressora.Marca);
                        Utils.SalvarResultadosEmArquivo(impressoraData, isMono, $"C:\\WFS\\Test-{impressora.Id}.txt", impressora.Marca);
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"Você não tem permissão para acessar as informações desta impressora! - {impressora.IP} - {impressora.Id}");
                    }
                }
                else
                {
                    await Console.Out.WriteLineAsync($"Não foi possível entrar em contato com a impressora - {impressora.IP} - {impressora.Id}");
                }
            }
        }



        private static bool VerificarTipoImpressora(string ip)
        {
            string typeOid = ".1.3.6.1.4.1.641.6.2.3.1.4.1";
            var snmpResults = ObterDadosSnmp(ip, new List<string> { typeOid });
            string deviceModel = snmpResults.FirstOrDefault().Value?.ToString();
            return Utils.IdentificarTipoImpressora(deviceModel);
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
                Utils.Log("Erro ao obter dados SNMP.");
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
                else
                {
                    Console.WriteLine("MONO - Impressora Não Listada");
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