using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.Controllers;
using WCS.Repositories;

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



        public static void SalvarResultadosEmArquivo(Printers lexmark, bool isMono, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine($"Hello World!! - {DateTime.Now}\n");

                if (isMono)
                {
                    LexmarkController.EnviarDadosLexmark(lexmark);
                    LexmarkRepository.EscreverDadosMono(lexmark, sw);
                }
                else
                {
                    LexmarkController.EnviarDadosLexmark(lexmark);
                    LexmarkRepository.EscreverDadosColor(lexmark, sw);
                }
            }
        }



        public static bool IdentificarTipoImpressora(string type)
        {
            string cleanedInput = type.Replace("String =", "").Trim();
            string[] words = cleanedInput.Split(' ');

            if (words.Length >= 2)
            {
                string secondWord = words[1];
                if (secondWord.StartsWith("M"))
                {
                    return true; // Mono
                }
                else if(secondWord.StartsWith("C"))
                {
                    return false; // Color
                }
                else
                {
                    Log("A segunda palavra não começa com 'M' ou 'C'.");
                }
            }
            else
            {
                Log("A string não contém palavras suficientes.");
            }

            return true; // Default to false (Color)
        }



        public static void Log(string message)
        {
            Console.WriteLine(message);
            // Adicionar log para um arquivo ou sistema de log se necessário
        }
    }
}
