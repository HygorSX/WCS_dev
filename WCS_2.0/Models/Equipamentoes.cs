using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS_2._0.Models
{
    public class Equipamentoes
    {
        public int Id { get; set; }
        public int EquipamentoMarcaId { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public int Ativo { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
