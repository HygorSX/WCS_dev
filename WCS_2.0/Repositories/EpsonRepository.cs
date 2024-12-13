using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS;

namespace WCS_2._0.Repositories
{
    public class EpsonRepository
    {
        public static List<string> GetMonoOidsEps()
        {
            return new List<string>
            {
               //".1.3.6.1.2.1.43.5.1.1.17.1", // Serial Number
               // Inserir Fabricante Manualmente
               ".1.3.6.1.4.1.1248.1.1.3.1.3.8.0", // Modelo Da Impressora
               ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Impressões Totais
               ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto
               ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Preto
               ".1.3.6.1.2.1.43.16.5.1.2.1.1"// Printer Status
            };
        }




        public static List<string> GetColorOidsEps()
        {
            return new List<string>
            {
                //".1.3.6.1.2.1.43.5.1.1.17.1", // Serial Number
                // Inserir Fabricante Manualmentes
                ".1.3.6.1.4.1.1248.1.1.3.1.3.8.0", // Modelo Da Impressora 
                ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Impressões Totais
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner Preto
                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Ciano
                ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Toner Ciano
                ".1.3.6.1.2.1.43.11.1.1.9.1.3", // Level Magenta
                ".1.3.6.1.2.1.43.11.1.1.8.1.3", // Capacidade Magenta
                ".1.3.6.1.2.1.43.11.1.1.9.1.4", // Level Yellow
                ".1.3.6.1.2.1.43.11.1.1.8.1.4", // Capacidade Yellow
                ".1.3.6.1.2.1.43.16.5.1.2.1.1"// Status Da Impressora
            };
        }

        public static Printers AnalisarDadosMonoEps(string[] resultado, Printers eps)
        {

            eps.SerialUniImage = "";
            eps.DeviceManufacturer = "EPSON";
            eps.DeviceModel = resultado[0];

            eps.QuantidadeImpressaoTotal = Math.Abs(int.Parse(resultado[1]));

            eps.PorcentagemBlack = (Math.Abs(int.Parse(resultado[2])) * 100) / Math.Abs(int.Parse(resultado[3]));

            eps.PrinterStatus =resultado[4];

            eps.SerialImpressora = "";

            eps.PrinterModel = resultado[0];

            eps.Tipo = "MONO";

            return eps;
        }

        public static Printers AnalisarDadosColorEps(string[] resultado, Printers eps)
        {

            eps.SerialUniImage = "";
            eps.DeviceManufacturer = "EPSON";
            eps.DeviceModel = resultado[0];

            eps.QuantidadeImpressaoTotal = Math.Abs(int.Parse(resultado[1]));

            eps.PorcentagemBlack = (Math.Abs(int.Parse(resultado[2])) * 100) / Math.Abs(int.Parse(resultado[3]));
            eps.PorcentagemCyan = (Math.Abs(int.Parse(resultado[4])) * 100) / Math.Abs(int.Parse(resultado[5]));
            eps.PorcentagemMagenta = (Math.Abs(int.Parse(resultado[6])) * 100) / Math.Abs(int.Parse(resultado[7]));
            eps.PorcentagemYellow = (Math.Abs(int.Parse(resultado[8])) * 100) / Math.Abs(int.Parse(resultado[9]));
            //eps.PorcentagemFusor = (int.Parse(resultado[12]) * 100) / int.Parse(resultado[13]);
            //eps.PorcentagemBelt = (int.Parse(resultado[14]) * 100) / int.Parse(resultado[15]);
            eps.PrinterStatus = resultado[10];
            //eps.PrinterStatus = resultado[16];
            eps.SerialImpressora = "";

            eps.PrinterModel = resultado[0];

            eps.Tipo = "COLOR";

            return eps;
        }
    }
}
