using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS
{
    public class LexmmarkMX511 : Lexmark
    {

        private Pdu pdu = new Pdu();
        public Pdu Pdu_Lexmark(){


            pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.2.1.1.4.1.4");
            pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.2.1.1.4.1.8");
            pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.2.1.1.4.1.10");
            pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.4.1.1.16.1.2");
            pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.4.1.1.16.1.1");
            pdu.VbList.Add(".1.3.6.1.4.1.641.6.2.3.1.5.1");
            pdu.VbList.Add(".1.3.6.1.4.1.641.6.4.4.1.1.6.1.2");


            pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.1"); //Capacidade Toner
            pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.1"); //Level Toner
            pdu.VbList.Add("system.sysUpTime.0"); //Tempo de funcionamento
            pdu.VbList.Add(".1.3.6.1.2.1.43.5.1.1.16.1"); //Device Model
            pdu.VbList.Add("system.sysDescr.0"); //Device Description
            pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.8.1.2"); //Capacidade Kit Manutenção
            pdu.VbList.Add(".1.3.6.1.2.1.43.11.1.1.9.1.2"); //Level Kit Manutenção
            pdu.VbList.Add("system.sysName.0"); //Name Device*/
          
            return pdu;
     
        }
        public int Pdu_Count()
        {
            return pdu.VbList.Count();
        }
    }
}
