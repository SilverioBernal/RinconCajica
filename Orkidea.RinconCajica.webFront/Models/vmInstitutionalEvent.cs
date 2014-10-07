using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmInstitutionalEvent : InstitutionalEvent
    {
        public string tmpInicio { get; set; }
        public string tmpFin { get; set; }
    }
}