using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS
{
    public class Lexmark
    {
        public string Porta { get; set; }
        public string Serial { get; set; }
        public int Total { get; set; }
        public int QuantidadeImpressaoTotal { get; set; }
        public int TotalCopias { get; set; }
        public string NumSerieUniImagem { get; set; }
        public int TotalUnidadeImagem { get; set; }
        public string DeviceManufacturer { get; set; }
        public int DeviceUpTime { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceDescription { get; set; }
        public string DeviceName { get; set; }
        public int BlackLevel { get; set; }
        public int BlackCapacity { get; set; }
        public int PorcentagemBlack { get; set; }
        public int CapacidadeKitManutenção { get; set; }
        public int LevelKitManutenção { get; set; }
        public int PorcentagemKitManutenção { get; set; }
        public String MacAddress { get; set; }

        //=================== Color =======================

        private string _printerStatus { get; set; }
        public int CyanLevel { get; set; }
        public int CyanCapacity { get; set; }
        public int PorcentagemCyan { get; set; }
        public int YellowLevel { get; set; }
        public int YellowCapacity { get; set; }
        public int PorcentagemYellow { get; set; }
        public int MagentaLevel { get; set; }
        public int MagentaCapacity { get; set; }
        public int PorcentagemMagenta { get; set; }
        public int PrinterFusorLevel { get; set; }
        public int PrinterFusorCapacity { get; set; }
        public int PorcentagemFusor { get; set; }
        public int PrinterBeltLevel { get; set; }
        public int PrinterBeltCapacity { get; set; }
        public int PorcentagemBelt { get; set; }

        public string PrinterStatus
        {

            get
            {
                return HexConvert(_printerStatus);
            }

            set
            {
                _printerStatus = value;
            }

        }
        private string HexConvert(string hex)
        {
            string hexString = hex;

            // Removendo os espaços em branco
            hexString = hexString.Replace(" ", "");

            // Convertendo a string hexadecimal em um array de bytes
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            // Decodificando os bytes em uma string usando a codificação UTF-8
            string decodedString = Encoding.UTF8.GetString(bytes);

            return(decodedString);
        }
    }
}
