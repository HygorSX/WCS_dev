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
               ".1.3.6.1.4.1.1248.1.2.2.1.1.1.5.1", // Serial Number
               // Inserir Fabricante Manualmente
               ".1.3.6.1.4.1.1248.1.1.3.1.3.8.0", // Modelo Da Impressora
               ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Impressões Coloridas
               ".1.3.6.1.4.1.1248.1.2.2.27.1.1.3.1.1", // Impressões Mono
               ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto
               ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Preto
               // Nível Kit Manutenção 
               // Capacidade Kit Manutenção
               // Printer Status
            };
        }




        public static List<string> GetColorOidsEps()
        {
            return new List<string>
            {
                ".1.3.6.1.4.1.1248.1.2.2.1.1.1.5.1", // Serial Number
                // Inserir Fabricante Manualmentes
                ".1.3.6.1.2.1.25.3.2.1.3.1", // Modelo Da Impressora 
                ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Impressões Coloridas
                ".1.3.6.1.4.1.1248.1.2.2.27.1.1.3.1.1", // Impressões Mono
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner Preto
                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Ciano
                ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Toner Ciano
                ".1.3.6.1.2.1.43.11.1.1.9.1.3", // Level Magenta
                ".1.3.6.1.2.1.43.11.1.1.8.1.3", // Capacidade Magenta
                ".1.3.6.1.2.1.43.11.1.1.9.1.4", // Level Yellow
                ".1.3.6.1.2.1.43.11.1.1.8.1.4", // Capacidade Yellow
                // Nível Do fusor da impressora
                // Capacidade do fusor da impressora
                // Nível Printer Belt
                // Capacidade Printer Belt
                // Status Da Impressora
            };
        }

        public static Printers AnalisarDadosMonoEps(string[] resultado, Printers eps)
        {

            eps.SerialUniImage = resultado[0];
            eps.DeviceManufacturer = "EPSON";
            eps.DeviceModel = resultado[1];

            eps.QuantidadeImpressaoTotal = Math.Abs(int.Parse(resultado[2])) + Math.Abs(int.Parse(resultado[3]));

            eps.PorcentagemBlack = (Math.Abs(int.Parse(resultado[4])) * 100) / Math.Abs(int.Parse(resultado[5]));
            //eps.PorcentagemKitManutenção = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]);

            //eps.PrinterStatus = resultado[8];
            eps.PrinterStatus = "";

            eps.SerialImpressora = "";

            eps.PrinterModel = "";

            eps.Tipo = "MONO";

            return eps;
        }

        public static Printers AnalisarDadosColorEps(string[] resultado, Printers eps)
        {

            eps.SerialUniImage = resultado[0];
            eps.DeviceManufacturer = "EPSON";
            eps.DeviceModel = resultado[1];

            eps.QuantidadeImpressaoTotal = Math.Abs(int.Parse(resultado[2]));

            eps.PorcentagemBlack = (Math.Abs(int.Parse(resultado[4])) * 100) / Math.Abs(int.Parse(resultado[5]));
            eps.PorcentagemCyan = (Math.Abs(int.Parse(resultado[6])) * 100) / Math.Abs(int.Parse(resultado[7]));
            eps.PorcentagemMagenta = (Math.Abs(int.Parse(resultado[8])) * 100) / Math.Abs(int.Parse(resultado[9]));
            eps.PorcentagemYellow = (Math.Abs(int.Parse(resultado[10])) * 100) / Math.Abs(int.Parse(resultado[11]));
            //eps.PorcentagemFusor = (int.Parse(resultado[12]) * 100) / int.Parse(resultado[13]);
            //eps.PorcentagemBelt = (int.Parse(resultado[14]) * 100) / int.Parse(resultado[15]);
            eps.PrinterStatus = "";
            //eps.PrinterStatus = resultado[16];
            eps.SerialImpressora = resultado[0];

            eps.PrinterModel = resultado[1];

            eps.Tipo = "COLOR";

            return eps;
        }
    }
}
