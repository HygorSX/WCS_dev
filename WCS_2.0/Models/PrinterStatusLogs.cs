using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS;
using WCS.Utilities;

namespace WCS_2._0.Models
{
    public class PrinterStatusLogs : Utils
    {
        public int Id { get; set; }
        public int PrinterId { get; set; }
        public int QuantidadeImpressaoTotal { get; set; }
        public int PorcentagemBlack { get; set; }
        public int PorcentagemCyan { get; set; }
        public int PorcentagemYellow { get; set; }
        public int PorcentagemMagenta { get; set; }
        public int PorcentagemFusor { get; set; }
        public int PorcentagemBelt { get; set; }
        public int PorcentagemKitManutencao { get; set; }
        private string _printerStatus { get; set; }
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

        public Printers Printer { get; set; }

    }
}
