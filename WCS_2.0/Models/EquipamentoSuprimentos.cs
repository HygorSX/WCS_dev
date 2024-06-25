using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS_2._0.Models
{
    public class EquipamentoSuprimentos
    {
        public int Id { get; set; }
        public int Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public int ProdutoId { get; set; }
        public int EquipamentoId { get; set; }
        public int Quantidade { get; set; }
    }
}
