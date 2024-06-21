using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS_2._0.Models
{
    public class EquipamentoSecretarias
    {
        public int Id { get; set; }
        public int Patrimonio { get; set; }
        public string SalaAtendimento { get; set; }
        public int Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public int DeptoId { get; set; }
        public int EquipamentoId { get; set; }
        public string Ip { get; set; }
        public int EquipamentoInstituicaoId { get; set; }
        public int Novo { get; set; }
        public int Estoque { get; set; }
        public int Inservivel { get; set; }
    }
}
