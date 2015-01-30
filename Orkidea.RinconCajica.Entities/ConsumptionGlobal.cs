using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.RinconCajica.Entities
{
    public class ConsumptionGlobal : ConsumptionResume
    {
        public string Docid_pagador { get; set; }
        public string accion { get; set; }
        public string nombre { get; set; }
    }
}
