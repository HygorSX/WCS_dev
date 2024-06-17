﻿using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WCS
{
    public class Lexmark
    {
        //public string Porta { get; set; } 
        //public string NumSerieUniImagem { get; set; }
        //public string DeviceDescription { get; set; }
        //public int BlackLevel { get; set; }
        //public int BlackCapacity { get; set; }
        //public int CapacidadeKitManutenção { get; set; }
        //public int LevelKitManutenção { get; set; } 
        //public int TotalUnidadeImagem { get; set; }

        //==================== ALL ====================
        public string Serial { get; set; } //
        public string DeviceManufacturer { get; set; } //
        public string DeviceModel { get; set; } //
        public int QuantidadeImpressaoTotal { get; set; }//
        public int PorcentagemKitManutenção { get; set; } //
        public int PorcentagemBlack { get; set; } //
        public String MacAddress { get; set; } //
        private string _printerStatus { get; set; } //


        //=================== Color =======================

        //public int CyanLevel { get; set; }
        //public int CyanCapacity { get; set; }
        //public int YellowLevel { get; set; }
        //public int YellowCapacity { get; set; }
        //public int MagentaLevel { get; set; }
        //public int MagentaCapacity { get; set; }
        //public int PrinterFusorLevel { get; set; }
        //public int PrinterFusorCapacity { get; set; }
        //public int PrinterBeltLevel { get; set; }
        //public int PrinterBeltCapacity { get; set; }

        public int PorcentagemCyan { get; set; } // 
        public int PorcentagemYellow { get; set; } //
        public int PorcentagemMagenta { get; set; } //
        public int PorcentagemFusor { get; set; } //
        public int PorcentagemBelt { get; set; } //

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

        private bool IsHexa(string hex)
        {
            string[] arrHex = hex.Split(' ');

            for(int i = 0; i < arrHex.Count(); i++)
            {
                try
                {
                    ulong int_hex = Convert.ToUInt64(arrHex[i], 16);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        private string HexConvert(string hex)
        {
            if (IsHexa(hex))
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

                return (decodedString);
            }
            else
            {
                return hex;
            }
        }
    }
}
