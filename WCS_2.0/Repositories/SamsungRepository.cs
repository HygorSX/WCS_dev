using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS;

namespace WCS_2._0.Repositories
{
    public class SamsungRepository
    {
        public static List<string> GetMonoOidsSam()
        {
            return new List<string>
            {
               ".1.3.6.1.2.1.43.5.1.1.16.1", // Serial Number
               // Inserir Fabricante Manualmente
               ".1.3.6.1.2.1.25.3.2.1.3.1", // Modelo Da Impressora
               ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Total Impressões
               ".1.3.6.1.4.1.236.11.5.1.1.3.22.0", // Nível Preto
               // Nível Kit Manutenção 
               // Capacidade Kit Manutenção
               ".1.3.6.1.2.1.43.16.5.1.2.1.1"// Printer Status
            };
        }




        public static List<string> GetColorOidsSam()
        {
            return new List<string>
            {
               ".1.3.6.1.2.1.43.5.1.1.16.1", // Serial Number
               // Inserir Fabricante Manualmente
               ".1.3.6.1.2.1.25.3.2.1.3.1", // Modelo Da Impressora
               ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Total Impressões
               ".1.3.6.1.4.1.236.11.5.1.1.3.22.0", // Nível Preto
               // Nível Kit Manutenção 
               // Capacidade Kit Manutenção
               ".1.3.6.1.2.1.43.16.5.1.2.1.1"// Printer Status
            };
        }

        public static Printers AnalisarDadosMonoSam(string[] resultado, Printers eps)
        {

            eps.SerialUniImage = resultado[0];
            eps.DeviceManufacturer = "SAMSUNG";
            eps.DeviceModel = resultado[1];

            eps.QuantidadeImpressaoTotal = int.Parse(resultado[2]);

            eps.PorcentagemBlack = int.Parse(resultado[3]);
            //eps.PorcentagemKitManutenção = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]);

            //eps.PrinterStatus = resultado[8];
            eps.PrinterStatus = resultado[4];

            eps.SerialImpressora = resultado[0];

            eps.PrinterModel = resultado[1];

            eps.Tipo = "MONO";

            return eps;
        }

        public static Printers AnalisarDadosColorSam(string[] resultado, Printers eps)
        {
            eps.SerialUniImage = resultado[0];
            eps.DeviceManufacturer = "SAMSUNG";
            eps.DeviceModel = resultado[1];

            eps.QuantidadeImpressaoTotal = int.Parse(resultado[2]);

            eps.PorcentagemBlack = int.Parse(resultado[3]);
            //eps.PorcentagemKitManutenção = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]);

            //eps.PrinterStatus = resultado[8];
            eps.PrinterStatus = resultado[4];

            eps.SerialImpressora = resultado[0];

            eps.PrinterModel = resultado[1];

            eps.Tipo = "COLOR";

            return eps;
        }
    }
}
