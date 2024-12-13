using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WCS.Utilities;

namespace WCS
{
    public class Printers : Utils
    {
        [Key]
        public int Id { get; set; }
        public string SerialUniImage { get; set; }
        public string DeviceManufacturer { get; set; }
        public string DeviceModel { get; set; }
        public int QuantidadeImpressaoTotal { get; set; }
        public int PorcentagemKitManutencao { get; set; }
        public int PorcentagemBlack { get; set; }
        private string _printerStatus { get; set; } 
        public int PorcentagemCyan { get; set; }
        public int PorcentagemYellow { get; set; }
        public int PorcentagemMagenta { get; set; }
        public int PorcentagemFusor { get; set; }
        public int PorcentagemBelt { get; set; }
        public int PorcentagemUnidadeImagem { get; set; }
        public string SerialImpressora { get; set; }
        public string SerialTonnerPreto { get; set; } = string.Empty;
        public string PrinterModel { get; set; }
        public string Tipo { get; set; }
        public int Patrimonio { get; set; }
        public string Ip { get; set; }
        public string Secretaria { get; set; }
        public string AbrSecretaria { get; set; }
        public string Depto { get; set; }
        public int InstituicaoId { get; set; }
        public int EmailAbaixo20pc { get; set; }
        public DateTime DataHoraDeBusca { get; set; } = DateTime.Now;
        public string Localizacao { get; set; }
        public int Ativa { get; set; }

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
    }
}
