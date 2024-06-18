using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.Data;

namespace WCS.Controllers
{
    public class LexmarkController
    {
        public static void EnviarDadosLexmark(Lexmark lexmark)
        {
            using (var db = new PrinterMonitoringContext())
            {
                db.DbLexmark.Add(lexmark);
                db.SaveChanges();
            }
        }
    }
}
