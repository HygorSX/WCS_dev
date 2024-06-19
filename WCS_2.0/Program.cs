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

            string[] ip = { "192.168.222.30", "192.168.223.24", "192.168.222.26" };
            //string[] ip = { "192.168.222.30" };

            for(int i = 0; i < ip.Length; i++) 
            {
                if (TestePing(ip[i]))
                {
                    bool isMono = VerificarTipoImpressora(ip[i]);
                    var snmpResults = ObterDadosSnmp(ip[i], isMono);
                    var lexmarkData = AnalisarResultadosSnmp(snmpResults, isMono);
                    Utils.SalvarResultadosEmArquivo(lexmarkData, isMono, $"C:\\WFS\\Test{i}.txt");
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



        private static Dictionary<Oid, AsnType> ObterDadosSnmp(string ip, bool isMono)
        {
            List<string> oids = isMono ? LexmarkRepository.GetMonoOids() : LexmarkRepository.GetColorOids();
            return ObterDadosSnmp(ip, oids);
        }



        private static Lexmark AnalisarResultadosSnmp(Dictionary<Oid, AsnType> snmpResults, bool isMono)
        {
            Lexmark lexmark = new Lexmark();
            string[] resultado = snmpResults.Values.Select(v => v.ToString()).ToArray();
            //lexmark.Id = 0;
            if (isMono)
            {
                lexmark = LexmarkRepository.AnalisarDadosMono(resultado, lexmark);
            }
            else
            {
                lexmark = LexmarkRepository.AnalisarDadosColor(resultado, lexmark);
            }

            return lexmark;
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