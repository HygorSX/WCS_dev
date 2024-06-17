using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WCS.Data;

namespace WCS
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TestarConexao();

            string ip = "192.168.211.30";
            if (TestePing(ip))
            {
                bool isMono = VerificarTipoImpressora(ip);
                var snmpResults = ObterDadosSnmp(ip, isMono);
                var lexmarkData = AnalisarResultadosSnmp(snmpResults, isMono);
                SalvarResultadosEmArquivo(lexmarkData, isMono, "C:\\WFS\\Test.txt");
            }
        }





        private static async Task TestarConexao()
        {
            SqlConnection conn = Conexao.ObterConexao();
            Log(conn == null ? "Não foi possível obter a conexão. Veja o log de erros." : "A conexão foi obtida com sucesso.");
            Conexao.fecharConexao();
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
                Log($"Erro no ping: {ex.Message}");
                return false;
            }
        }



        private static bool VerificarTipoImpressora(string ip)
        {
            string typeOid = ".1.3.6.1.2.1.43.5.1.1.16.1";
            var snmpResults = ObterDadosSnmp(ip, new List<string> { typeOid });
            string deviceModel = snmpResults.FirstOrDefault().Value?.ToString();
            return IdentificarTipoImpressora(deviceModel);
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
                Log("Erro ao obter dados SNMP.");
                return new Dictionary<Oid, AsnType>();
            }

            return snmpResults;
        }



        private static Dictionary<Oid, AsnType> ObterDadosSnmp(string ip, bool isMono)
        {
            List<string> oids = isMono ? GetMonoOids() : GetColorOids();
            return ObterDadosSnmp(ip, oids);
        }



        private static List<string> GetMonoOids()
        {
            return new List<string>
            {
                //Acrescentar e revisar as oids

                ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.4",
                ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.8",
                ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.10",
                ".1.3.6.1.4.1.641.6.4.4.1.1.16.1.2",
                ".1.3.6.1.4.1.641.6.4.4.1.1.16.1.1",
                ".1.3.6.1.4.1.641.6.2.3.1.5.1",
                ".1.3.6.1.4.1.641.6.4.4.1.1.6.1.2",
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Toner
                ".1.3.6.1.2.1.43.5.1.1.16.1", // Device Model
                ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Kit Manutenção
                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Kit Manutenção
                ".1.3.6.1.2.1.43.16.5.1.2.1.1" // Status Da Impressora
            };
        }



        private static List<string> GetColorOids()
        {
            return new List<string>
            {
                //Acrescentar e revisar as oids

                ".1.3.6.1.2.1.43.5.1.1.17.1", // Serial Number
                ".1.3.6.1.2.1.43.8.2.1.14.1.1", // Fabricante da Impressora
                ".1.3.6.1.2.1.43.5.1.1.16.1", // Modelo Da Impressora 
                //Nome da Impressora
                ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Total Páginas
                //Total Cópias
                ".1.3.6.1.2.1.43.11.1.1.9.1.4", // Nível Preto
                ".1.3.6.1.2.1.43.11.1.1.8.1.4", // Capacidade Toner Preto
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Ciano
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner Ciano
                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Level Magenta
                ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Magenta
                ".1.3.6.1.2.1.43.11.1.1.9.1.3", // Level Yellow
                ".1.3.6.1.2.1.43.11.1.1.8.1.3", // Capacidade Yellow
                ".1.3.6.1.2.1.43.11.1.1.9.1.6", // Nível Do fusor da impressora
                ".1.3.6.1.2.1.43.11.1.1.8.1.6", // Capacidade do fusor da impressora
                ".1.3.6.1.2.1.43.11.1.1.9.1.7", // Nível Printer Belt
                ".1.3.6.1.2.1.43.11.1.1.8.1.7", // Capacidade Printer Belt
                ".1.3.6.1.2.1.43.16.5.1.2.1.1" // Status Da Impressora
            };
        }



        private static Lexmark AnalisarResultadosSnmp(Dictionary<Oid, AsnType> snmpResults, bool isMono)
        {
            Lexmark lexmark = new Lexmark();
            string[] resultado = snmpResults.Values.Select(v => v.ToString()).ToArray();

            if (isMono)
            {
                lexmark = AnalisarDadosMono(resultado);
            }
            else
            {
                lexmark = AnalisarDadosColor(resultado);
            }

            return lexmark;
        }



        private static Lexmark AnalisarDadosMono(string[] resultado)
        {
            return new Lexmark
            {
                //Corrigir formato dos dados

                //Porta = resultado[0],
                TotalPaginas = int.Parse(resultado[1]),
                TotalCopias = int.Parse(resultado[2]) + int.Parse(resultado[3]),
                //TotalUnidadeImagem = (int.Parse(resultado[3]) * 100) / 60000,
                Serial = resultado[6],
                //BlackCapacity = int.Parse(resultado[7]),
                //BlackLevel = int.Parse(resultado[8]),
                PorcentagemBlack = (int.Parse(resultado[8]) * 100) / int.Parse(resultado[7]),
                DeviceModel = resultado[9],
                //CapacidadeKitManutenção = int.Parse(resultado[10]),
                //LevelKitManutenção = int.Parse(resultado[11]),
                PorcentagemKitManutenção = (int.Parse(resultado[11]) * 100) / int.Parse(resultado[10]),
                PrinterStatus = resultado[12]
            };
        }



        private static Lexmark AnalisarDadosColor(string[] resultado)
        {
            return new Lexmark
            {
                Serial = resultado[0],
                DeviceManufacturer = resultado[1],
                DeviceModel = resultado[2],
                DeviceName = resultado[3],

                TotalPaginas = int.Parse(resultado[4]),
                TotalCopias = int.Parse(resultado[5]),

                PorcentagemBlack = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]),
                PorcentagemCyan = (int.Parse(resultado[8]) * 100) / int.Parse(resultado[9]),
                PorcentagemMagenta = (int.Parse(resultado[10]) * 100) / int.Parse(resultado[11]),
                PorcentagemYellow = (int.Parse(resultado[12]) * 100) / int.Parse(resultado[13]),
                PorcentagemFusor = (int.Parse(resultado[14]) * 100) / int.Parse(resultado[15]),
                PorcentagemBelt = (int.Parse(resultado[16]) * 100) / int.Parse(resultado[17]),

                PrinterStatus = resultado[17]
            };
        }



        private static void SalvarResultadosEmArquivo(Lexmark lexmark, bool isMono, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine($"Hello World!! - {DateTime.Now}\n");

                if (isMono)
                {
                    EscreverDadosMono(lexmark, sw);
                    EnviarDadosImpressora(lexmark);
                }
                else
                {
                    EscreverDadosColor(lexmark, sw);
                    EnviarDadosImpressora(lexmark);
                }
            }
        }



        private static void EscreverDadosMono(Lexmark lexmark, StreamWriter sw)
        {
            sw.WriteLine($"Serial - {lexmark.Serial}");
            sw.WriteLine($"Fabricante Da Impressora - {lexmark.DeviceManufacturer}");
            sw.WriteLine($"Modelo Da Impressora - {lexmark.DeviceModel}");
            sw.WriteLine($"Nome Da Impressora - {lexmark.DeviceName}");
            sw.WriteLine($"Total Páginas - {lexmark.TotalPaginas}");
            sw.WriteLine($"Total Copias - {lexmark.TotalCopias}");
            sw.WriteLine($"Impressão Total - {lexmark.QuantidadeImpressaoTotal}");
            sw.WriteLine($"Porcentagem Toner Black - {lexmark.PorcentagemBlack}%");
            sw.WriteLine($"Porcentagem Kit Manutenção - {lexmark.PorcentagemKitManutenção}%");
            sw.WriteLine($"Status - {lexmark.PrinterStatus}");
        }



        private static void EscreverDadosColor(Lexmark lexmark, StreamWriter sw)
        {
            sw.WriteLine($"Serial - {lexmark.Serial}");
            sw.WriteLine($"Fabricante Da Impressora - {lexmark.DeviceManufacturer}");
            sw.WriteLine($"Modelo Da Impressora - {lexmark.DeviceModel}");
            //sw.WriteLine($"Nome Da Impressora - {lexmark.DeviceName}");
            //sw.WriteLine($"Total Páginas - {lexmark.TotalPaginas}");
            //sw.WriteLine($"Total Cópias - {lexmark.TotalCopias}");
            //sw.WriteLine($"Impressão Total - {lexmark.QuantidadeImpressaoTotal}");
            sw.WriteLine($"Porcentagem Toner Black - {lexmark.PorcentagemBlack}%");
            sw.WriteLine($"Porcentagem Toner Cyan - {lexmark.PorcentagemCyan}%");
            sw.WriteLine($"Porcentagem Toner Magenta - {lexmark.PorcentagemMagenta}%");
            sw.WriteLine($"Porcentagem Toner Yellow - {lexmark.PorcentagemYellow}%");
            sw.WriteLine($"Porcentagem Do Fusor - {lexmark.PorcentagemFusor}%");
            sw.WriteLine($"Porcentagem Do Belt - {lexmark.PorcentagemBelt}%");
            sw.WriteLine($"Porcentagem Do Kit De Manutenção - {lexmark.PorcentagemKitManutenção}%");
            sw.WriteLine($"Status - {lexmark.PrinterStatus}");
        }



        private static void EnviarDadosImpressora(Lexmark lexmark)
        {
            using (var db = new PrinterMonitoringContext())
            {
                var printer = new Lexmark
                {
                    Serial = lexmark.Serial,
                    DeviceManufacturer = lexmark.DeviceManufacturer,
                    DeviceModel = lexmark.DeviceModel,
                    //DeviceName = lexmark.DeviceName
                    TotalPaginas = lexmark.TotalPaginas,
                    //TotalCopias = lexmark.TotalCopias
                    //QuantidadeImpressaoTotal = lexmark.QuantidadeImpressaoTotal
                    PorcentagemBlack = lexmark.PorcentagemBlack,
                    PorcentagemCyan = lexmark.PorcentagemCyan,
                    PorcentagemMagenta = lexmark.PorcentagemMagenta,
                    PorcentagemYellow = lexmark.PorcentagemYellow,
                    PorcentagemFusor = lexmark.PorcentagemFusor,
                    PorcentagemBelt = lexmark.PorcentagemBelt,
                    PorcentagemKitManutenção = lexmark.PorcentagemKitManutenção,
                    PrinterStatus = lexmark.PrinterStatus,
                };

                db.PrinterMonitorings.Add(printer);
                db.SaveChanges();
            }
        }



        private static void Log(string message)
        {
            Console.WriteLine(message);
            // Adicionar log para um arquivo ou sistema de log se necessário
        }




        private static bool IdentificarTipoImpressora(string type)
        {
            string cleanedInput = type.Replace("String =", "").Trim();
            string[] words = cleanedInput.Split(' ');

            if (words.Length >= 2)
            {
                string secondWord = words[1];
                if (secondWord.StartsWith("M"))
                {
                    return true; // Mono
                }
                else if (secondWord.StartsWith("C"))
                {
                    return false; // Color
                }
                else
                {
                    Log("A segunda palavra não começa com 'M' ou 'C'.");
                }
            }
            else
            {
                Log("A string não contém palavras suficientes.");
            }

            return true; // Default to false (Color)
        }
    }
}