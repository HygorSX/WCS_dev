using System;
using System.Threading.Tasks;
using WCS.Data;
using WCS.Utilities;

namespace WCS.Controllers
{
    public class LexmarkController
    {
        public static void EnviarDadosLexmark(Lexmark lexmark)
        {
            using (var db = new PrinterMonitoringContext())
            {
                try
                {
                    db.PrinterMonitorings.Add(lexmark);
                    db.Entry(lexmark).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    Utils.Log("Dados da impressora enviados com sucesso para o banco de dados.");
                }
                catch (Exception ex)
                {
                    Utils.Log($"Erro ao enviar dados da impressora para o banco de dados: {ex.Message}");
                }
            }
        }
    }
}
