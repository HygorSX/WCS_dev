using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS;

namespace WCS_2._0.Repositories
{
    public class CanonRepository
    {
        public static List<string> GetMonoOidsCan()
        {
            return new List<string>
            {
               ".1.3.6.1.4.1.1602.1.2.1.8.1.3.1.1", // Serial Number
               // Inserir Fabricante Manualmente
               ".1.3.6.1.2.1.25.3.2.1.3.1", // Modelo Da Impressora
               ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Total Impressões
               ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto Mate - Será incluído no campo fusor
               ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Preto Mate - Será incluído no campo fusor
               ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Preto 
               ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Preto
               //".1.3.6.1.4.1.236.11.5.11.53.61.5.2.1.7.1.1", //Serial Tonner Preto
               ".1.3.6.1.2.1.43.16.5.1.2.1.1"// Printer Status
            };
        }




        public static List<string> GetColorOidsCan()
        {
            return new List<string>
            {
                ".1.3.6.1.4.1.1602.1.2.1.8.1.3.1.1", // Serial Number
                // Inserir Fabricante Manualmentes
                ".1.3.6.1.2.1.25.3.2.1.3.1", // Modelo Da Impressora 
                ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Impressões Totais
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto Mate - Será incluído no campo fusor
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Preto Mate - Será incluído no campo fusor
                ".1.3.6.1.2.1.43.11.1.1.9.1.3", // Nível Amarelo
                ".1.3.6.1.2.1.43.11.1.1.8.1.3", // Capacidade Toner Amarelo
                ".1.3.6.1.2.1.43.11.1.1.9.1.4", // Level Magenta
                ".1.3.6.1.2.1.43.11.1.1.8.1.5", // Capacidade Magenta
                ".1.3.6.1.2.1.43.11.1.1.9.1.5", // Level Ciano
                ".1.3.6.1.2.1.43.11.1.1.8.1.5", // Capacidade Ciano
                ".1.3.6.1.2.1.43.16.5.1.2.1.1",// Status Da Impressora

                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Preto 
               ".1.3.6.1.2.1.43.11.1.1.8.1.2" // Capacidade Preto
            };
        }

        public static Printers AnalisarDadosMonoCan(string[] resultado, Printers can)
        {

            can.SerialUniImage = resultado[0];
            can.DeviceManufacturer = "CANON";
            can.DeviceModel = resultado[1];

            can.QuantidadeImpressaoTotal = Math.Abs(int.Parse(resultado[2]));

            can.PorcentagemFusor = Math.Abs((int.Parse(resultado[3]) * 100) / int.Parse(resultado[4]));
            can.PorcentagemBlack = Math.Abs((int.Parse(resultado[5]) * 100) / int.Parse(resultado[6]));

            can.PrinterStatus = resultado[7];

            can.SerialImpressora = resultado[0];
            can.PrinterModel = resultado[1];
            can.Tipo = "MONO";

            return can;
        }

        public static Printers AnalisarDadosColorCan(string[] resultado, Printers can)
        {

            can.SerialUniImage = resultado[0];
            can.DeviceManufacturer = "CANON";
            can.DeviceModel = resultado[1];

            can.QuantidadeImpressaoTotal = Math.Abs(int.Parse(resultado[2]));

            can.PorcentagemFusor = Math.Abs((int.Parse(resultado[3]) * 100) / int.Parse(resultado[4]));
            can.PorcentagemBlack = Math.Abs((int.Parse(resultado[12]) * 100) / int.Parse(resultado[13]));
            can.PorcentagemCyan = Math.Abs((int.Parse(resultado[9]) * 100) / int.Parse(resultado[10]));
            can.PorcentagemMagenta = Math.Abs((int.Parse(resultado[7]) * 100) / int.Parse(resultado[8]));
            can.PorcentagemYellow = Math.Abs((int.Parse(resultado[5]) * 100) / int.Parse(resultado[6]));

            can.PrinterStatus = resultado[11];
            can.SerialImpressora = resultado[0];
            can.PrinterModel = resultado[1];

            can.Tipo = "COLOR";

            return can;
        }
    }
}
