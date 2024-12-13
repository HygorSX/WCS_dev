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
               ".1.3.6.1.4.1.236.11.5.1.1.1.4.0", // Serial Number
               // Inserir Fabricante Manualmente
               ".1.3.6.1.4.1.236.11.5.11.53.31.1.4.0", // Modelo Da Impressora
               ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Total Impressões
               ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto
               ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Preto
               ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Fusor
               ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Fusor
               ".1.3.6.1.4.1.236.11.5.11.53.61.5.2.1.7.1.1", //Serial Tonner Preto
               // ".1.3.6.1.2.1.43.16.5.1.2.1.1"// Printer Status
            };
        }




        public static List<string> GetColorOidsSam()
        {
            return new List<string>
            {
               ".1.3.6.1.4.1.236.11.5.1.1.1.4.0", // Serial Number
               // Inserir Fabricante Manualmente
               ".1.3.6.1.4.1.236.11.5.11.53.31.1.4.0", // Modelo Da Impressora
               ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Total Impressões
               ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto
               ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Preto
               ".1.3.6.1.4.1.236.11.5.11.53.61.5.2.1.7.1.1", // Serial Tonner Preto
               // ".1.3.6.1.2.1.43.16.5.1.2.1.1",// Printer Status
               ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Fusor
               ".1.3.6.1.2.1.43.11.1.1.8.1.2" // Capacidade Fusor
            };
        }

        public static Printers AnalisarDadosMonoSam(string[] resultado, Printers eps)
        {

            eps.SerialUniImage = resultado[0];
            eps.DeviceManufacturer = "SAMSUNG";
            eps.DeviceModel = resultado[1];

            eps.QuantidadeImpressaoTotal = int.Parse(resultado[2]);

            eps.PorcentagemBlack = Math.Abs(int.Parse(resultado[3]) * 100) / int.Parse(resultado[4]);
            eps.PorcentagemFusor = Math.Abs(int.Parse(resultado[5]) * 100) / int.Parse(resultado[6]);

            eps.SerialTonnerPreto = resultado[7];

            eps.PrinterStatus = "";

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

            eps.PorcentagemBlack = Math.Abs(int.Parse(resultado[3]) * 100) / int.Parse(resultado[4]);
            eps.PorcentagemFusor = Math.Abs(int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]);

            eps.SerialTonnerPreto = resultado[5];

            eps.PrinterStatus = "";

            eps.SerialImpressora = resultado[0];

            eps.PrinterModel = resultado[1];

            eps.Tipo = "COLOR";

            return eps;
        }
    }
}
