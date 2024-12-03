using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS_2._0.Models
{
    public class Equipamento
    {
        public int Id { get; set; }
        public int InstituicaoId { get; set; }
        public string Tipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string IP { get; set; }
        public int Patrimonio { get; set; }
        public string Secretaria { get; set; }
        public string AbrSecretaria { get; set; }
        public string Depto { get; set; }
        public List<Suprimento> Suprimentos { get; set; }
    }
}
