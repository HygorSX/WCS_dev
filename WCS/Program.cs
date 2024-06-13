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
        static void Main(string[] args)
        {
            TestarConexao().Wait();
            // O método .Wait() é usado para aguardar a conclusão de uma
            // operação assíncrona antes de continuar a execução do código.

            SqlConnection conn = Conexao.ObterConexao();
            

            string ip = "192.168.211.30";
           bool mono = true;
           bool encontrou = TestePing(ip);
           int cont = 0;
            
            if (encontrou)
            {
                SimpleSnmp snmp = new SimpleSnmp(ip, "public");
                Lexmark lexmark = new Lexmark();



                //.1.3.6.1.4.1.2699.1.2.1.3.1.1.6.1.1

                Pdu pdu = new Pdu();
                if (mono)
                {
                    pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.2.1.1.4.1.4");
                    pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.2.1.1.4.1.8");
                    pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.2.1.1.4.1.10");
                    pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.4.1.1.16.1.2");
                    pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.4.1.1.16.1.1");
                    pdu.VbList.Add(".1.3.6.1.4.1.641.6.2.3.1.5.1");
                    pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.4.1.1.6.1.2");
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.1"); //Capacidade Toner
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.1"); //Level Toner
                    pdu.VbList.Add(".1.3.6.1.2.1.43.5.1.1.16.1"); //Device Model
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.2"); //Capacidade Kit Manutenção
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.2"); //Level Kit Manutenção
                    pdu.VbList.Add(".1.3.6.1.2.1.43.17.6.1.5.1.1"); //Printer Status

                }
                else
                {
                    pdu.VbList.Add(".1.3.6.1.2.1.43.5.1.1.17.1"); //Serial Number
                    pdu.VbList.Add(".1.3.6.1.2.1.43.10.2.1.4.1.1"); //Page Counter
                    pdu.VbList.Add(".1.3.6.1.2.1.43.8.2.1.14.1.1"); //Device Manufacturer
                    pdu.VbList.Add(".1.3.6.1.2.1.43.5.1.1.16.1"); //Device Model
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.4"); //Level Black
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.4"); //Capacidade Toner Black
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.1"); //Capacidade Toner Cyan
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.1"); //Level Cyan
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.2"); //Capacidade Magenta
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.2"); //Level Magenta
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.3"); //Capacidade Yellow
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.3"); //Level Yellow
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.6"); //Printer Fusor Capacity
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.6"); //Printer Fusor Level
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.7"); //Printer Belt Capacity
                    pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.7"); //Printer Belt Level
                    pdu.VbList.Add(".1.3.6.1.2.1.43.16.5.1.2.1.1"); //Printer Status
                }

                //pdu.VbList.Add("system.sysUpTime.0"); //Tempo de funcionamento
                //pdu.VbList.Add("system.sysDescr.0"); //Device Description
                //pdu.VbList.Add("system.sysName.0"); //Name Device*/

                snmp.Set(SnmpVersion.Ver1, pdu);

                //Dictionary<Oid, AsnType> result = snmp.Get(SnmpVersion.Ver1,
                //new string[] { ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.4" });
                //Dictionary<Oid, AsnType> resultDVM = snmp.Get(SnmpVersion.Ver1, new string[] { ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.8" });

                Dictionary<Oid, AsnType> resultDVM = snmp.Get(SnmpVersion.Ver1, pdu);

                String[] resultado = new string[pdu.VbList.Count()];

                if (resultDVM != null)
                {
                    foreach (KeyValuePair<Oid, AsnType> kvp in resultDVM)
                    {

                        resultado[cont] = kvp.Value.ToString();
                        cont++;
                        

                    }
                }
                if (mono)
                {
                    lexmark.Porta = resultado[0];
                    lexmark.Total = int.Parse(resultado[1]);
                    lexmark.TotalCopias = int.Parse(resultado[2]) + int.Parse(resultado[3]);
                    lexmark.TotalUnidadeImagem = (int.Parse(resultado[3]) * 100) / 60000;
                    lexmark.Serial = resultado[6];
                    lexmark.BlackCapacity = int.Parse(resultado[7]);
                    lexmark.BlackLevel = int.Parse(resultado[8]);
                    lexmark.PorcentagemBlack = (lexmark.BlackLevel * 100) / lexmark.BlackCapacity;
                    lexmark.DeviceModel = resultado[9];
                    lexmark.CapacidadeKitManutenção = int.Parse(resultado[10]);
                    lexmark.LevelKitManutenção = int.Parse(resultado[11]);
                    lexmark.PorcentagemKitManutenção = (lexmark.LevelKitManutenção * 100) / lexmark.CapacidadeKitManutenção;
                }
                else
                {
                    //lexmark.Total = int.Parse(resultado[1]);
                    //lexmark.TotalUnidadeImagem = (int.Parse(resultado[3]) * 100) / 60000;
                    //lexmark.Porta = resultado[0];
                    //lexmark.TotalCopias = int.Parse(resultado[2]) + int.Parse(resultado[3]);
                    //lexmark.CapacidadeKitManutenção = int.Parse(resultado[10]);
                    //lexmark.LevelKitManutenção = int.Parse(resultado[11]);

                    lexmark.Serial = resultado[0];
                    lexmark.DeviceManufacturer = resultado[2];
                    lexmark.DeviceModel = resultado[3];

                    lexmark.BlackCapacity = int.Parse(resultado[4]);
                    lexmark.BlackLevel = int.Parse(resultado[5]);
                    lexmark.PorcentagemBlack = (lexmark.BlackLevel * 100) / lexmark.BlackCapacity;

                    lexmark.CyanCapacity = int.Parse(resultado[6]);
                    lexmark.CyanLevel = int.Parse(resultado[7]);
                    lexmark.PorcentagemCyan = (lexmark.CyanLevel * 100) / lexmark.CyanCapacity;

                    lexmark.MagentaCapacity = int.Parse(resultado[8]);
                    lexmark.MagentaLevel = int.Parse(resultado[9]);
                    lexmark.PorcentagemMagenta = (lexmark.MagentaLevel * 100) / lexmark.MagentaCapacity;

                    lexmark.YellowCapacity = int.Parse(resultado[10]);
                    lexmark.YellowLevel = int.Parse(resultado[11]);
                    lexmark.PorcentagemYellow = (lexmark.YellowLevel * 100) / lexmark.YellowCapacity;

                    lexmark.PrinterFusorCapacity = int.Parse(resultado[12]);
                    lexmark.PrinterFusorLevel = int.Parse(resultado[13]);
                    lexmark.PorcentagemFusor = (lexmark.PrinterFusorLevel * 100) / lexmark.PrinterFusorCapacity;

                    lexmark.PrinterBeltCapacity = int.Parse(resultado[14]);
                    lexmark.PrinterBeltLevel = int.Parse(resultado[15]);
                    lexmark.PorcentagemBelt = (lexmark.PrinterBeltLevel * 100) / lexmark.PrinterBeltCapacity;

                    lexmark.PrinterStatus = (resultado[16]);

                }

                //lexmark.DeviceUpTime = int.Parse(resultado[9]);
                //lexmark.DeviceDescription = resultado[11];   
                //lexmark.DeviceName = resultado[14];  

                StreamWriter sw = new StreamWriter("C:\\WFS\\Test.txt");
                //Write a line of text
                sw.WriteLine("Hello World!! - " + DateTime.Now + "\n");
                sw.WriteLine("Serial - " + lexmark.Serial);
                //Write a second line of text
                sw.WriteLine("Total = " + lexmark.Total + "\n");
                sw.WriteLine("Porta = " + lexmark.Porta + "\n");
                sw.WriteLine("Total de Cópias = " + lexmark.TotalCopias + "\n");
                sw.WriteLine("Total de Toner = " + (lexmark.PorcentagemBlack + "% \n"));
                sw.WriteLine("Capacidade Toner Preto = " + (lexmark.BlackCapacity + " \n"));
                sw.WriteLine("Total unidade de imagem: " + lexmark.TotalUnidadeImagem + "% \n");
                sw.WriteLine("Número de série uni. imagem 6 = " + (lexmark.NumSerieUniImagem + " \n"));
                sw.WriteLine("Modelo Dispositivo = " + (lexmark.DeviceModel + " \n"));
                sw.WriteLine("Capacidade do Kit de Manutenção = " + (lexmark.CapacidadeKitManutenção + " \n"));
                sw.WriteLine("Level do Kit de Manutenção = " + (lexmark.LevelKitManutenção + " \n"));
                sw.WriteLine("Porcentagem do Kit de Manutenção = " + (lexmark.PorcentagemKitManutenção + " \n"));

                if (!mono)
                {
                    //sw.WriteLine("Porta = " + lexmark.Porta + "\n");

                    //Write a second line of text
                    sw.WriteLine("Serial - " + lexmark.Serial);
                    sw.WriteLine("Modelo Dispositivo = " + (lexmark.DeviceModel + " \n"));
                    sw.WriteLine("Fabricante do Dispositivo = " + (lexmark.DeviceManufacturer + " \n"));

                    sw.WriteLine("Capacidade Toner Preto = " + (lexmark.BlackCapacity + " \n"));
                    sw.WriteLine("Total de Toner Preto = " + (lexmark.PorcentagemBlack + "% \n"));

                    sw.WriteLine("Capacidade Toner Ciano = " + (lexmark.CyanCapacity + " \n"));
                    sw.WriteLine("Total de Toner Ciano = " + (lexmark.PorcentagemCyan + "% \n"));

                    sw.WriteLine("Capacidade Toner Magenta = " + (lexmark.MagentaCapacity + " \n"));
                    sw.WriteLine("Total de Toner Magenta = " + (lexmark.PorcentagemMagenta + "% \n"));

                    sw.WriteLine("Capacidade Toner Amarelo = " + (lexmark.YellowCapacity + " \n"));
                    sw.WriteLine("Total de Toner Amarelo = " + (lexmark.PorcentagemYellow + "% \n"));

                    sw.WriteLine("Capacidade Fusor = " + (lexmark.PrinterFusorCapacity + " \n"));
                    sw.WriteLine("Total Fusor = " + (lexmark.PorcentagemFusor + " \n"));

                    sw.WriteLine("Capacidade Belt = " + (lexmark.PrinterBeltCapacity + " \n"));
                    sw.WriteLine("Total Belt = " + (lexmark.PorcentagemBelt + " \n"));

                    sw.WriteLine("Printer Status = " + (lexmark.PrinterStatus + " \n"));

                }


                //Close the file
                sw.Close();
            }

        }

        public static async Task TestarConexao()
        {
            SqlConnection conn = Conexao.ObterConexao();
            if (conn == null)
            {
                Console.Write("Não foi possível obter a conexão. Veja o log de erros.");
            }
            else
            {
                Console.Write("A conexão foi obtida com sucesso.");
            }

            // não precisamos mais da conexão? vamos fechá-la
            Conexao.fecharConexao();

        }


        public static bool TestePing(string ip)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(ip);
                string status = pingReply.Status.ToString();
                if (status == "Success")
                {
                    // Fazer algo quando o ping for bem-sucedido
                    return true;
                }
                else
                {
                    // Fazer algo quando o ping falhar
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
