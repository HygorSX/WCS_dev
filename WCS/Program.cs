using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WCS
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TestarConexao();

            string ip = "192.168.211.30";
            bool isMono = true;

            if (TestePing(ip))
            {
                var snmpResults = GetSnmpData(ip, isMono);
                var lexmarkData = ParseSnmpResults(snmpResults, isMono);

                SaveResultsToFile(lexmarkData, isMono, "C:\\WFS\\Test.txt");
            }
        }

        public static async Task TestarConexao()
        {
            SqlConnection conn = Conexao.ObterConexao();
            if (conn == null)
            {
                Log("Não foi possível obter a conexão. Veja o log de erros.");
            }
            else
            {
                Log("A conexão foi obtida com sucesso.");
            }

            Conexao.fecharConexao();
        }

        public static bool TestePing(string ip)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(ip);
                return pingReply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                Log($"Erro no ping: {ex.Message}");
                return false;
            }
        }

        private static Dictionary<Oid, AsnType> GetSnmpData(string ip, bool isMono)
        {
            SimpleSnmp snmp = new SimpleSnmp(ip, "public");
            Pdu pdu = new Pdu();

            var oids = isMono ? GetMonoOids() : GetColorOids();
            foreach (var oid in oids)
            {
                pdu.VbList.Add(oid);
            }

            return snmp.Get(SnmpVersion.Ver1, pdu);
        }

        private static List<string> GetMonoOids()
        {
            return new List<string>
            {
                ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.4",
                ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.8",
                ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.10",
                ".1.3.6.1.4.1.641.6.4.4.1.1.16.1.2",
                ".1.3.6.1.4.1.641.6.4.4.1.1.16.1.1",
                ".1.3.6.1.4.1.641.6.2.3.1.5.1",
                ".1.3.6.1.4.1.641.6.4.4.1.1.6.1.2",
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Level Toner
                ".1.3.6.1.2.1.43.5.1.1.16.1", // Device Model
                ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Kit Manutenção
                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Level Kit Manutenção
                ".1.3.6.1.2.1.43.17.6.1.5.1.1" // Printer Status
            };
        }

        private static List<string> GetColorOids()
        {
            return new List<string>
            {
                ".1.3.6.1.2.1.43.5.1.1.17.1", // Serial Number
                ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Page Counter
                ".1.3.6.1.2.1.43.8.2.1.14.1.1", // Device Manufacturer
                ".1.3.6.1.2.1.43.5.1.1.16.1", // Device Model
                ".1.3.6.1.2.1.43.11.1.1.9.1.4", // Level Black
                ".1.3.6.1.2.1.43.11.1.1.8.1.4", // Capacidade Toner Black
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner Cyan
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Level Cyan
                ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Magenta
                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Level Magenta
                ".1.3.6.1.2.1.43.11.1.1.8.1.3", // Capacidade Yellow
                ".1.3.6.1.2.1.43.11.1.1.9.1.3", // Level Yellow
                ".1.3.6.1.2.1.43.11.1.1.8.1.6", // Printer Fusor Capacity
                ".1.3.6.1.2.1.43.11.1.1.9.1.6", // Printer Fusor Level
                ".1.3.6.1.2.1.43.11.1.1.8.1.7", // Printer Belt Capacity
                ".1.3.6.1.2.1.43.11.1.1.9.1.7", // Printer Belt Level
                ".1.3.6.1.2.1.43.16.5.1.2.1.1" // Printer Status
            };
        }

        private static Lexmark ParseSnmpResults(Dictionary<Oid, AsnType> snmpResults, bool isMono)
        {
            Lexmark l = new Lexmark();
            string[] resultado = snmpResults.Values.Select(v => v.ToString()).ToArray();

            if (isMono)
            {
                l.Porta = resultado[0];
                l.TotalPaginas = int.Parse(resultado[1]);
                l.TotalCopias = int.Parse(resultado[2]) + int.Parse(resultado[3]);
                l.TotalUnidadeImagem = (int.Parse(resultado[3]) * 100) / 60000;
                l.Serial = resultado[6];
                l.BlackCapacity = int.Parse(resultado[7]);
                l.BlackLevel = int.Parse(resultado[8]);
                l.PorcentagemBlack = (l.BlackLevel * 100) / l.BlackCapacity;
                l.DeviceModel = resultado[9];
                l.CapacidadeKitManutenção = int.Parse(resultado[10]);
                l.LevelKitManutenção = int.Parse(resultado[11]);
                l.PorcentagemKitManutenção = (l.LevelKitManutenção * 100) / l.CapacidadeKitManutenção;
            }
            else
            {
                l.Serial = resultado[0];
                l.DeviceManufacturer = resultado[2];
                l.DeviceModel = resultado[3];
                l.BlackCapacity = int.Parse(resultado[4]);
                l.BlackLevel = int.Parse(resultado[5]);
                l.PorcentagemBlack = (l.BlackLevel * 100) / l.BlackCapacity;
                l.CyanCapacity = int.Parse(resultado[6]);
                l.CyanLevel = int.Parse(resultado[7]);
                l.PorcentagemCyan = (l.CyanLevel * 100) / l.CyanCapacity;
                l.MagentaCapacity = int.Parse(resultado[8]);
                l.MagentaLevel = int.Parse(resultado[9]);
                l.PorcentagemMagenta = (l.MagentaLevel * 100) / l.MagentaCapacity;
                l.YellowCapacity = int.Parse(resultado[10]);
                l.YellowLevel = int.Parse(resultado[11]);
                l.PorcentagemYellow = (l.YellowLevel * 100) / l.YellowCapacity;
            }

            return l;
        }

        private static void SaveResultsToFile(Lexmark lexmark, bool isMono, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("Hello World!! - " + DateTime.Now + "\n");

                if (isMono)
                {
                    sw.WriteLine("Porta - " + lexmark.Porta);
                    sw.WriteLine("Total - " + lexmark.TotalPaginas);
                    sw.WriteLine("Total Copias - " + lexmark.TotalCopias);
                    sw.WriteLine("Total Unidade de Imagem - " + lexmark.TotalUnidadeImagem);
                    sw.WriteLine("Serial - " + lexmark.Serial);
                    sw.WriteLine("Capacidade Toner Black - " + lexmark.BlackCapacity);
                    sw.WriteLine("Nivel Toner Black - " + lexmark.BlackLevel);
                    sw.WriteLine("Porcentagem Toner Black - " + lexmark.PorcentagemBlack + "%");
                    sw.WriteLine("Device Model - " + lexmark.DeviceModel);
                    sw.WriteLine("Capacidade Kit Manutenção - " + lexmark.CapacidadeKitManutenção);
                    sw.WriteLine("Nivel Kit Manutenção - " + lexmark.LevelKitManutenção);
                    sw.WriteLine("Porcentagem Kit Manutenção - " + lexmark.PorcentagemKitManutenção + "%");
                }
                else
                {
                    sw.WriteLine("Serial - " + lexmark.Serial);
                    sw.WriteLine("Device Manufacturer - " + lexmark.DeviceManufacturer);
                    sw.WriteLine("Device Model - " + lexmark.DeviceModel);
                    sw.WriteLine("Capacidade Toner Black - " + lexmark.BlackCapacity);
                    sw.WriteLine("Nivel Toner Black - " + lexmark.BlackLevel);
                    sw.WriteLine("Porcentagem Toner Black - " + lexmark.PorcentagemBlack + "%");
                    sw.WriteLine("Capacidade Toner Cyan - " + lexmark.CyanCapacity);
                    sw.WriteLine("Nivel Toner Cyan - " + lexmark.CyanLevel);
                    sw.WriteLine("Porcentagem Toner Cyan - " + lexmark.PorcentagemCyan + "%");
                    sw.WriteLine("Capacidade Toner Magenta - " + lexmark.MagentaCapacity);
                    sw.WriteLine("Nivel Toner Magenta - " + lexmark.MagentaLevel);
                    sw.WriteLine("Porcentagem Toner Magenta - " + lexmark.PorcentagemMagenta + "%");
                    sw.WriteLine("Capacidade Toner Yellow - " + lexmark.YellowCapacity);
                    sw.WriteLine("Nivel Toner Yellow - " + lexmark.YellowLevel);
                    sw.WriteLine("Porcentagem Toner Yellow - " + lexmark.PorcentagemYellow + "%");
                }
            }
        }

        private static void Log(string message)
        {
            Console.WriteLine(message);
            // Adicionar log para um arquivo ou sistema de log se necessário
        }
    }
}
