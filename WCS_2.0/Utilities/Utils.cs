using Newtonsoft.Json;
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
using WCS_2._0.Models;
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
                    if (marca == "LEXMARK")
                    {
                        LexmarkController.EnviarDadosLexmark(imp);
                        //LexmarkRepository.EscreverDadosMono(imp, sw);
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
                    else if (marca == "RICOH")
                    {
                        RicohController.EnviarDadosRicoh(imp);
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
                    else if (marca == "RICOH")
                    {
                        RicohController.EnviarDadosRicoh(imp);
                    }
                }
            }
        }



        public static void Log(string message)
        {
            Console.Write(message);
            // Adicionar log para um arquivo ou sistema de log se necessário
        }

        public static List<Equipamento> GetImpressoras()
        {
            var url = "https://www1.barueri.sp.gov.br/citestoque/EquipamentoSecretaria/GetListaImpServicePrinter"; // Substitua pela URL real da sua API
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Realiza a requisição GET na API de forma síncrona
                    var response = client.GetStringAsync(url).Result;

                    // Deserializa a resposta da API para um formato de lista de objetos
                    var impressoras = JsonConvert.DeserializeObject<List<Equipamento>>(response);

                    return impressoras;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Erro ao acessar a API: {ex.Message}");
                    return new List<Equipamento>(); // Retorna uma lista vazia em caso de erro
                }
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

        //-------------------------------------------------------------------------------------
        //sendo usado em outra classe
        //
        //função que envia o email
        // Patrimonio - Modelo - Secretaria - Departamento - % Toner

        public static bool EnviarEmailTonnerMinimo(List<Printers> imp)
        {
            string Date = DateTime.Now.ToString("dd-MMMM-yyyy HH:mm:ss");
            string emailPrincipal = "cit.rmanso@barueri.sp.gov.br";
            string msn = $@"
                        <html>
                        <body>
                            <h3>Relatório de Impressoras - Tonner Preto Abaixo de 20%</h3>
                            <p>Prezado,</p>
                            <p>Segue abaixo a relação das impressoras com nível de toner preto abaixo de 20%, conforme registrado em <strong>{Date}</strong>.</p>
                            <br/>
                            <table style='border-collapse: collapse; width: 100%;'>
                                <thead>
                                    <tr>
                                        <th style='border: 1px solid #dddddd; padding: 8px;'>Patrimônio</th>
                                        <th style='border: 1px solid #dddddd; padding: 8px;'>Modelo</th>
                                        <th style='border: 1px solid #dddddd; padding: 8px;'>% Toner Preto</th>
                                        <th style='border: 1px solid #dddddd; padding: 8px;'>% Unidade De Imagem</th>
                                        <th style='border: 1px solid #dddddd; padding: 8px;'>Local</th>
                                    </tr>
                                </thead>
                                <tbody>";

                                    foreach (var impressora in imp)
                                    {
                                        msn += $@"
                                    <tr>
                                        <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{impressora.Patrimonio}</td>
                                        <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{impressora.PrinterModel}</td>
                                        <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{impressora.PorcentagemBlack}%</td>
                                        <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{impressora.PorcentagemUnidadeImagem}%</td>
                                        <td style='border: 1px solid #dddddd; padding: 8px; text-align: center;'>{impressora.AbrSecretaria} - {impressora.Depto}</td>   
                                    </tr>";
                                    }

                                    msn += @"
                                </tbody>
                            </table>
                            <br/>
                            <p>Atenciosamente,<br/>
                                Equipe de Suporte Técnico<br/>
                                CIT Barueri</p>
                            <hr/>
                            <p><small>Este é um email automático. Por favor, não responda a este email.</small></p>
                        </body>
                        </html>";

            Email email = new Email();
            email.ToEmail = emailPrincipal;
            email.CcEmail = "cit.vinicius@barueri.sp.gov.br;cit.arjona@barueri.sp.gov.br";
            email.Subject = "Tonners Abaixo de 20%";
            //email.CcEmail = emailCopia;

            email.Body = msn;
            try
            {
                email.Send(email);
                return true;

            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
