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
        public static List<string> GetMonoOids()
        {
            return new List<string>
            {
                ".1.3.6.1.4.1.641.6.4.4.1.1.6.1.2", // Serial Number
                ".1.3.6.1.2.1.43.8.2.1.14.1.1", // Fabricante Da Impressora
                ".1.3.6.1.2.1.43.5.1.1.16.1", // Modelo Da Impressora
                ".1.3.6.1.4.1.641.6.4.2.1.1.4.1.4", // Total Impressões
                ".1.3.6.1.2.1.43.11.1.1.9.1.1", // Nível Toner
                ".1.3.6.1.2.1.43.11.1.1.8.1.1", // Capacidade Toner
                ".1.3.6.1.2.1.43.11.1.1.9.1.2", // Nível Kit Manutenção
                ".1.3.6.1.2.1.43.11.1.1.8.1.2", // Capacidade Kit Manutenção
                ".1.3.6.1.2.1.43.16.5.1.2.1.1" // Status Da Impressora
            };
        }

        public static List<string> GetColorOids()
        {
            return new List<string>
            {

                ".1.3.6.1.2.1.43.5.1.1.17.1", // Serial Number
                ".1.3.6.1.2.1.43.8.2.1.14.1.1", // Fabricante da Impressora
                ".1.3.6.1.2.1.43.5.1.1.16.1", // Modelo Da Impressora 
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
                ".1.3.6.1.2.1.43.16.5.1.2.1.1" // Status Da Impressora
            };
        }





        public static Lexmark AnalisarDadosMono(string[] resultado)
        {
            return new Lexmark
            {
                Serial = resultado[0],
                DeviceManufacturer = resultado[1],
                DeviceModel = resultado[2],

                QuantidadeImpressaoTotal = int.Parse(resultado[3]),

                PorcentagemBlack = (int.Parse(resultado[4]) * 100) / int.Parse(resultado[5]),
                PorcentagemKitManutenção = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]),

                PrinterStatus = resultado[8]
            };
        }

        public static Lexmark AnalisarDadosColor(string[] resultado)
        {
            return new Lexmark
            {
                Serial = resultado[0],
                DeviceManufacturer = resultado[1],
                DeviceModel = resultado[2],

                QuantidadeImpressaoTotal = int.Parse(resultado[3]),

                PorcentagemBlack = (int.Parse(resultado[4]) * 100) / int.Parse(resultado[5]),
                PorcentagemCyan = (int.Parse(resultado[6]) * 100) / int.Parse(resultado[7]),
                PorcentagemMagenta = (int.Parse(resultado[8]) * 100) / int.Parse(resultado[9]),
                PorcentagemYellow = (int.Parse(resultado[10]) * 100) / int.Parse(resultado[11]),
                PorcentagemFusor = (int.Parse(resultado[12]) * 100) / int.Parse(resultado[13]),
                PorcentagemBelt = (int.Parse(resultado[14]) * 100) / int.Parse(resultado[15]),

                PrinterStatus = resultado[16]
            };
        }



        public static void EscreverDadosMono(Lexmark lexmark, StreamWriter sw)
        {
            sw.WriteLine($"Número de Série: {lexmark.Serial}");
            sw.WriteLine($"Fabricante Da Impressora: {lexmark.DeviceManufacturer}");
            sw.WriteLine($"Modelo Da Impressora: {lexmark.DeviceModel}");
            sw.WriteLine($"Quantidade Total De Impressões: {lexmark.QuantidadeImpressaoTotal}");
            sw.WriteLine($"Porcentagem Toner Black: {lexmark.PorcentagemBlack}%");
            sw.WriteLine($"Porcentagem Kit Manutenção: {lexmark.PorcentagemKitManutenção}%");
            sw.WriteLine($"Status: {lexmark.PrinterStatus}");
        }

        public static void EscreverDadosColor(Lexmark lexmark, StreamWriter sw)
        {
            sw.WriteLine($"Número De Série: {lexmark.Serial}");
            sw.WriteLine($"Fabricante Da Impressora: {lexmark.DeviceManufacturer}");
            sw.WriteLine($"Modelo Da Impressora: {lexmark.DeviceModel}");
            sw.WriteLine($"Quantidade Total De Impressões: {lexmark.QuantidadeImpressaoTotal}");
            sw.WriteLine($"Porcentagem Toner Black: {lexmark.PorcentagemBlack}%");
            sw.WriteLine($"Porcentagem Toner Cyan: {lexmark.PorcentagemCyan}%");
            sw.WriteLine($"Porcentagem Toner Magenta: {lexmark.PorcentagemMagenta}%");
            sw.WriteLine($"Porcentagem Toner Yellow: {lexmark.PorcentagemYellow}%");
            sw.WriteLine($"Porcentagem Do Fusor: {lexmark.PorcentagemFusor}%");
            sw.WriteLine($"Porcentagem Do Belt: {lexmark.PorcentagemBelt}%");
            sw.WriteLine($"Status: {lexmark.PrinterStatus}");
        }
    }
}
