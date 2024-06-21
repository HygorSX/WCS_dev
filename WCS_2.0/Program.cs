using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace WCS
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var infoImpressoras = Utils.GetImpressoras();
            foreach (var impressora in infoImpressoras)
            {
                if(impressora.Marca != "LEXMARK") { continue; }
                if (TestePing(impressora.IP))
                {
                    bool isMono = Utils.VerificarMono(impressora.Suprimentos);
                    var snmpResults = ObterDadosSnmp(impressora.IP, isMono);
                    if(snmpResults.Count != 0)
                    {
                        var lexmarkData = AnalisarResultadosSnmp(snmpResults, isMono);
                        Utils.SalvarResultadosEmArquivo(lexmarkData, isMono, $"C:\\WFS\\Test-{impressora.Id}.txt");
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

            /*string[] ip = { "192.168.222.30", "192.168.223.24", "192.168.222.26" };

            for(int i = 0; i < ip.Length; i++) 
            {
                if (TestePing(ip[i]))
                {
                    bool isMono = VerificarTipoImpressora(ip[i]);
                    var snmpResults = ObterDadosSnmp(ip[i], isMono);
                    var lexmarkData = AnalisarResultadosSnmp(snmpResults, isMono);
                    Utils.SalvarResultadosEmArquivo(lexmarkData, isMono, $"C:\\WFS\\Test{i}.txt");
                }
            }*/
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



        private static Dictionary<Oid, AsnType> ObterDadosSnmp(string ip, bool isMono)
        {
            List<string> oids = isMono ? LexmarkRepository.GetMonoOidsLex() : LexmarkRepository.GetColorOidsLex();
            return ObterDadosSnmp(ip, oids);
        }



        private static Printers AnalisarResultadosSnmp(Dictionary<Oid, AsnType> snmpResults, bool isMono)
        {
            Printers printer = new Printers();
            string[] resultado = snmpResults.Values.Select(v => v.ToString()).ToArray();
            //lexmark.Id = 0;
            if (isMono)
            {
                printer = LexmarkRepository.AnalisarDadosMonoLex(resultado, printer);
            }
            else
            {
                printer = LexmarkRepository.AnalisarDadosColorLex(resultado, printer);
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