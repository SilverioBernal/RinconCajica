
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.RinconCajica.Entities
{
    public class ConsumptionResume
    {
        public System.DateTime Fecha { get; set; }        
        public string Nufactura { get; set; }
        public string Sufijo { get; set; }
        public decimal Total_fac { get; set; }
    }
}
