using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.Repositories
{
    public class LexmarkRepository
    {
        public static List<string> GetMonoOidsLex()
        {
            return new List<string>
            {
                ".1.3.6.1.4.1.641.6.4.4.1.1.6.1.2", // Serial Number Unidade De Imagem
                ".1.3.6.1.2.1.43.8.2.1.14.1.1", // Fabricante Da Impressora
                ".1.3.6.1.2.1.43.5.1.1.16.1", // Modelo Da Impressora
                ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.4", // Total Impressões
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Toner
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner
                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Kit Manutenção
                ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Kit Manutenção
                ".1.3.6.1.2.1.43.16.5.1.2.1.1", // Status Da Impressora
                ".1.3.6.1.4.1.641.2.1.2.1.6.1", // Serial Number Impressora 
                ".1.3.6.1.4.1.641.6.2.3.1.4.1" // Modelo Da Impressora - NÃO EDITÁVEL
            };
        }

        public static List<string> GetColorOidsLex()
        {
            return new List<string>
            {

                ".1.3.6.1.2.1.43.5.1.1.17.1", // Serial Number Unidade De Imagem
                ".1.3.6.1.2.1.43.8.2.1.14.1.1", // Fabricante da Impressora
                ".1.3.6.1.2.1.43.5.1.1.16.1", // Modelo Da Impressora - EDITÁVEL
                ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.4", // Total Impressões
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
                ".1.3.6.1.2.1.43.16.5.1.2.1.1", // Status Da Impressora
                ".1.3.6.1.4.1.641.2.1.2.1.6.1", // Serial Number Impressora
                ".1.3.6.1.4.1.641.6.2.3.1.4.1" // Modelo Da Impressora - NÃO EDITÁVEL
            };
        }





        public static Printers AnalisarDadosMonoLex(string[] resultado, Printers lex)
        {

            lex.SerialUniImage = resultado[0];
            lex.DeviceManufacturer = resultado[1];
            lex.DeviceModel = resultado[2];

            lex.QuantidadeImpressaoTotal = int.Parse(resultado[3]);

            lex.PorcentagemBlack = (int.Parse(resultado[4]) * 100) / int.Parse(resultado[5]);
            lex.PorcentagemKitManutencao = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]);

            lex.PrinterStatus = resultado[8];

            lex.SerialImpressora = resultado[9];

            lex.PrinterModel = resultado[10];

            lex.Tipo = "MONO";

            return lex;
        }

        public static Printers AnalisarDadosColorLex(string[] resultado, Printers lex)
        {

            lex.SerialUniImage = resultado[0];
            lex.DeviceManufacturer = resultado[1];
            lex.DeviceModel = resultado[2];

            lex.QuantidadeImpressaoTotal = int.Parse(resultado[3]);

            lex.PorcentagemBlack = (int.Parse(resultado[4]) * 100) / int.Parse(resultado[5]);
            lex.PorcentagemCyan = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]);
            lex.PorcentagemMagenta = (int.Parse(resultado[8]) * 100) / int.Parse(resultado[9]);
            lex.PorcentagemYellow = (int.Parse(resultado[10]) * 100) / int.Parse(resultado[11]);
            lex.PorcentagemFusor = (int.Parse(resultado[12]) * 100) / int.Parse(resultado[13]);
            lex.PorcentagemBelt = (int.Parse(resultado[14]) * 100) / int.Parse(resultado[15]);

            lex.PrinterStatus = resultado[16];

            lex.SerialImpressora = resultado[17];

            lex.PrinterModel = resultado[18];

            lex.Tipo = "COLOR";

            return lex;
        }
    }
}
