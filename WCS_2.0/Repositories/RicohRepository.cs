using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS;

namespace WCS_2._0.Repositories
{
    public class RicohRepository
    {
        public static List<string> GetMonoOidsRic()
        {
            return new List<string>
            {
                ".1.3.6.1.2.1.43.5.1.1.17.1", // Serial Number Unidade De Imagem
                ".1.3.6.1.2.1.43.8.2.1.14.1.1", // Fabricante Da Impressora
                ".1.3.6.1.2.1.43.5.1.1.16.1", // Modelo Da Impressora
                ".1.3.6.1.2.1.43.10.2.1.4.1.1", // Total Impressões
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Toner
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner
                ".1.3.6.1.2.1.43.16.5.1.2.1.1", // Status Da Impressora
            };
        }

        public static List<string> GetColorOidsRic()
        {
            return new List<string>
            {

                ".1.3.6.1.2.1.43.5.1.1.17.1", //0 Serial Number Unidade De Imagem
                ".1.3.6.1.2.1.43.8.2.1.14.1.1", //1 Fabricante da Impressora
                ".1.3.6.1.2.1.43.5.1.1.16.1", //2 Modelo Da Impressora - EDITÁVEL
                ".1.3.6.1.2.1.43.10.2.1.4.1.1", //3 Total Impressões
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", //4 Nível Preto
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", //5 Capacidade Toner Preto
                ".1.3.6.1.2.1.43.11.1.1.9.1.3", //6 Nível Ciano
                ".1.3.6.1.2.1.43.11.1.1.8.1.3", //7 Capacidade Toner Ciano
                ".1.3.6.1.2.1.43.11.1.1.9.1.4", //8 Level Magenta
                ".1.3.6.1.2.1.43.11.1.1.8.1.4", //9 Capacidade Magenta
                ".1.3.6.1.2.1.43.11.1.1.9.1.5", //10 Level Yellow
                ".1.3.6.1.2.1.43.11.1.1.8.1.5", //11 Capacidade Yellow
                ".1.3.6.1.2.1.43.16.5.1.2.1.1", //12 Status Da Impressora
            };
        }


        public static Printers AnalisarDadosMonoRic(string[] resultado, Printers lex)
        {

            lex.SerialUniImage = resultado[0];
            lex.DeviceManufacturer = resultado[1];
            lex.DeviceModel = resultado[2];

            lex.QuantidadeImpressaoTotal = int.Parse(resultado[3]);

            lex.PorcentagemBlack = (Math.Abs(int.Parse(resultado[4])) * 100) / Math.Abs(int.Parse(resultado[5]));

            lex.PrinterStatus = resultado[6];

            lex.SerialImpressora = resultado[0];

            lex.PrinterModel = resultado[2];

            lex.Tipo = "MONO";

            return lex;
        }

        public static Printers AnalisarDadosColorRic(string[] resultado, Printers lex)
        {

            lex.SerialUniImage = resultado[0];
            lex.DeviceManufacturer = resultado[1];
            lex.DeviceModel = resultado[2];

            lex.QuantidadeImpressaoTotal = int.Parse(resultado[3]);

            lex.PorcentagemBlack = (Math.Abs(int.Parse(resultado[4])) * 100) / Math.Abs(int.Parse(resultado[5]));
            lex.PorcentagemCyan = (Math.Abs(int.Parse(resultado[6])) * 100) / Math.Abs(int.Parse(resultado[7]));
            lex.PorcentagemMagenta = (Math.Abs(int.Parse(resultado[8])) * 100) / Math.Abs(int.Parse(resultado[9]));
            lex.PorcentagemYellow = (Math.Abs(int.Parse(resultado[10])) * 100) / Math.Abs(int.Parse(resultado[11]));

            lex.PrinterStatus = resultado[12];

            lex.SerialImpressora = resultado[0];

            lex.PrinterModel = resultado[2];

            lex.Tipo = "COLOR";

            return lex;
        }
    }
}
