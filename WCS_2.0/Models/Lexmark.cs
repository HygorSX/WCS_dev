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
    public class Lexmark : Utils
    {
        //public String MacAddress { get; set; } 
        [Key]
        public int Id { get; set; }
        public string Serial { get; set; }
        public string DeviceManufacturer { get; set; }
        public string DeviceModel { get; set; }
        public int QuantidadeImpressaoTotal { get; set; }
        public int PorcentagemKitManutenção { get; set; }
        public int PorcentagemBlack { get; set; }
        private string _printerStatus { get; set; } 
        public int PorcentagemCyan { get; set; }
        public int PorcentagemYellow { get; set; }
        public int PorcentagemMagenta { get; set; }
        public int PorcentagemFusor { get; set; }
        public int PorcentagemBelt { get; set; }
        //public string PrinterStatus { get; set; }
        public DateTime DataHoraDeBusca { get; set; } = DateTime.Now;

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
