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
               ".1.3.6.1.4.1.1248.1.2.2.27.1.1.3.1.1", // Impressões Mono - NÂO FUNCIONA
               ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto
               ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Preto
               ".1.3.6.1.4.1.2435.2.4.3.1240.3.2.1.5.1" // Printer Status
            };
        }




        public static List<string> GetColorOidsBth()
        {
            return new List<string>
            {
                ".1.3.6.1.2.1.43.5.1.1.17.1", // Serial Number
                // Inserir Fabricante Manualmentes
                ".1.3.6.1.2.1.25.3.2.1.3.1", // Modelo Da Impressora 
                ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Impressões Coloridas
                ".1.3.6.1.4.1.1248.1.2.2.27.1.1.3.1.1", // Impressões Mono - NÃO FUNCIONA
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Preto
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner Preto
                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Ciano
                ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Toner Ciano
                ".1.3.6.1.2.1.43.11.1.1.9.1.3", // Level Magenta
                ".1.3.6.1.2.1.43.11.1.1.8.1.3", // Capacidade Magenta
                ".1.3.6.1.2.1.43.11.1.1.9.1.4", // Level Yellow
                ".1.3.6.1.2.1.43.11.1.1.8.1.4", // Capacidade Yellow
                ".1.3.6.1.4.1.2435.2.4.3.1240.3.2.1.5.1" // Status Da Impressora
            };
        }

        public static Printers AnalisarDadosMonoBth(string[] resultado, Printers bth)
        {

            bth.SerialUniImage = resultado[0];
            bth.DeviceManufacturer = "BROTHER";
            bth.DeviceModel = resultado[1];

            bth.QuantidadeImpressaoTotal = Math.Abs(int.Parse(resultado[2]));

            bth.PorcentagemBlack = Math.Abs((int.Parse(resultado[4]) * 100) / int.Parse(resultado[5]));

            bth.PrinterStatus = resultado[6];

            bth.SerialImpressora = resultado[0];

            bth.PrinterModel = resultado[1];

            bth.Tipo = "MONO";

            return bth;
        }

        public static Printers AnalisarDadosColorBth(string[] resultado, Printers bth)
        {

            bth.SerialUniImage = resultado[0];
            bth.DeviceManufacturer = "BROTHER";
            bth.DeviceModel = resultado[1];

            bth.QuantidadeImpressaoTotal = Math.Abs(int.Parse(resultado[2]));

            bth.PorcentagemBlack = Math.Abs((int.Parse(resultado[4]) * 100) / int.Parse(resultado[5]));
            bth.PorcentagemCyan = Math.Abs((int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]));
            bth.PorcentagemMagenta = Math.Abs((int.Parse(resultado[8]) * 100) / int.Parse(resultado[9]));
            bth.PorcentagemYellow = Math.Abs((int.Parse(resultado[10]) * 100) / int.Parse(resultado[11]));

            bth.PrinterStatus = resultado[12];
            bth.SerialImpressora = resultado[0];
            bth.PrinterModel = resultado[1];

            bth.Tipo = "COLOR";

            return bth;
        }
    }
}
