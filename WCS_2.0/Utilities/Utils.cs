using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.Controllers;
using WCS.Data;
using WCS.Repositories;
using WCS_2._0.Controllers;
using WCS_2._0.Repositories;

namespace WCS.Utilities
{
    public class Utils
    {
        public bool IsHexa(string hex)
        {
            string[] arrHex = hex.Split(' ');

            for (int i = 0; i < arrHex.Count(); i++)
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



        public string HexConvert(string hex)
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



        public static void SalvarResultados(Printers imp, bool isMono, string filePath, dynamic marca)

        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine($"Hello World!! - {DateTime.Now}\n");

                if (isMono)
                {
                    if(marca == "LEXMARK")
                    {
                        LexmarkController.EnviarDadosLexmark(imp);
                        //LexmarkRepository.EscreverDadosMono(imp, sw);
                    }
                    else if(marca == "EPSON")
                    {
                        EpsonController.EnviarDadosEpson(imp);
                    }
                    else if(marca == "SAMSUNG")
                    {
                        SamsungController.EnviarDadosSamsung(imp);
                    }
                    else if(marca == "BROTHER")
                    {
                        BrotherController.EnviarDadosBrother(imp);
                    }
                }
                else
                {
                    if (marca == "LEXMARK")
                    {
                        LexmarkController.EnviarDadosLexmark(imp);
                        //LexmarkRepository.EscreverDadosColor(imp, sw);
                    }
                    else if (marca == "EPSON")
                    {
                        EpsonController.EnviarDadosEpson(imp);
                    }
                    else if (marca == "SAMSUNG")
                    {
                        SamsungController.EnviarDadosSamsung(imp);
                    }
                    else if (marca == "BROTHER")
                    {
                        BrotherController.EnviarDadosBrother(imp);
                    }
                }
            }
        }



        public static void Log(string message)
        {
            Console.Write(message);
            // Adicionar log para um arquivo ou sistema de log se necessário
        }

        public static dynamic GetImpressoras()
        {
            using (var db = new PrinterMonitoringContext())
            {
                var select_tipos = db.EquipamentoTipoes
                        .Join(db.EquipamentoMarcas, et => et.Id, em => em.EquipamentoTipoId, (et, em) => new { et, em })
                        .Join(db.Equipamentoes, join1 => join1.em.Id, eq => eq.EquipamentoMarcaId, (join1, eq) => new { join1, eq })
                        .Join(db.EquipamentoSecretarias, join2 => join2.eq.Id, es => es.EquipamentoId, (join2, es) => new { join2, es })
                        .Where(busca => busca.join2.join1.et.Id > 0)
                        .Where(busca => busca.join2.join1.et.Id == 1) // Tipo 1: Impressoras
                        .Where(busca => busca.es.Estoque == 0 && busca.es.Inservivel == 0) // Onde estoque e inservivel for 0 (false) a impressora está em uso 
                        .Where(busca => busca.es.Ip != "") // Trás impressoras que possuem IP
                        .Select(s => new {
                            Id = s.es.Id,
                            Tipo = s.join2.join1.et.Nome,
                            Marca = s.join2.join1.em.Nome,
                            Modelo = s.join2.eq.Nome,
                            IP = s.es.Ip,
                            Patrimonio = s.es.Patrimonio,
                            Suprimentos = db.EquipamentoSuprimentos
                                            .Join(db.Equipamentoes, es => es.EquipamentoId, eq => eq.Id, (es, eq) => new { es, eq })
                                            .Join(db.Produtoes, join1 => join1.es.ProdutoId, p => p.Id, (join1, p) => new { join1, p })
                                            .Where(busca => busca.join1.eq.Id == s.join2.eq.Id)
                                            .Select(s2 => s2.p)
                                            .ToList()
                        }).ToList();

                return select_tipos;
            }
        }

        public static bool VerificarMono(dynamic suprimentos)
        {
            foreach (var suprimento in suprimentos)
            {
                if (suprimento.Descricao.Contains("CYAN") || suprimento.Descricao.Contains("CIANO") ||
                    suprimento.Descricao.Contains("YELLOW") || suprimento.Descricao.Contains("AMARELO") ||
                    suprimento.Descricao.Contains("MAGENTA") || suprimento.Descricao.Contains("MAGENTA"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
