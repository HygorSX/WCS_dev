using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS_2._0.Models
{
    public class EquipamentoTipoes
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? Ativo { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}
