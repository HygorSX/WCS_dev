using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS;

namespace WCS_2._0.Repositories
{
    public class BrotherRepository
    {
        public static List<string> GetMonoOidsBth()
        {
            return new List<string>
            {
               ".1.3.6.1.2.1.43.5.1.1.17.1", // Serial Number
               // Inserir Fabricante Manualmente
               ".1.3.6.1.2.1.25.3.2.1.3.1", // Modelo Da Impressora
               ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Impressões Coloridas
               ".1.3.6.1.4.1.1248.1.2.2.27.1.1.3.1.1", // Impressões Mono
               ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto
               ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Preto
               // Nível Kit Manutenção 
               // Capacidade Kit Manutenção
               // Printer Status
            };
        }




        public static List<string> GetColorOidsBth()
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

        public static Printers AnalisarDadosMonoBth(string[] resultado, Printers bth)
        {

            bth.SerialUniImage = resultado[0];
            bth.DeviceManufacturer = "BROTHER";
            bth.DeviceModel = resultado[1];

            bth.QuantidadeImpressaoTotal = int.Parse(resultado[2]) + int.Parse(resultado[3]);

            bth.PorcentagemBlack = (int.Parse(resultado[4]) * 100) / int.Parse(resultado[5]);
            //eps.PorcentagemKitManutenção = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]);

            //eps.PrinterStatus = resultado[8];
            bth.PrinterStatus = "";

            bth.SerialImpressora = "";

            bth.PrinterModel = "";

            bth.Tipo = "MONO";

            return bth;
        }

        public static Printers AnalisarDadosColorBth(string[] resultado, Printers bth)
        {

            bth.SerialUniImage = resultado[0];
            bth.DeviceManufacturer = "BROTHER";
            bth.DeviceModel = resultado[1];

            bth.QuantidadeImpressaoTotal = int.Parse(resultado[2]) + int.Parse(resultado[3]);

            bth.PorcentagemBlack = (int.Parse(resultado[4]) * 100) / int.Parse(resultado[5]);
            bth.PorcentagemCyan = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]);
            bth.PorcentagemMagenta = (int.Parse(resultado[8]) * 100) / int.Parse(resultado[9]);
            bth.PorcentagemYellow = (int.Parse(resultado[10]) * 100) / int.Parse(resultado[11]);
            //eps.PorcentagemFusor = (int.Parse(resultado[12]) * 100) / int.Parse(resultado[13]);
            //eps.PorcentagemBelt = (int.Parse(resultado[14]) * 100) / int.Parse(resultado[15]);
            bth.PrinterStatus = "";
            //eps.PrinterStatus = resultado[16];
            bth.SerialImpressora = resultado[0];

            bth.PrinterModel = resultado[1];

            bth.Tipo = "COLOR";

            return bth;
        }
    }
}
