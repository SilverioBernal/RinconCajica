using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmSportModality : SportModality
    {
        public string nombreDeporte { get; set; }

        public List<Sport> lsDeportes { get; set; }

        public vmSportModality()
        {
            lsDeportes = new List<Sport>();
        }
    }
}