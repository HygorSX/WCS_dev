using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS_2._0.Models
{
    public class Suprimento
    {
        public string Descricao { get; set; }
        public int QuantMinima { get; set; }
        public string Observacao { get; set; }
        public int QtdeEstoque { get; set; }
        public int Status { get; set; }
        public int Tecnico { get; set; }
        public int? TipoAtaId { get; set; }
        public int GrupoProdutosId { get; set; }
        public int Rendimento { get; set; }
        public string Unidade { get; set; }
        public int Id { get; set; }
        public int Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
