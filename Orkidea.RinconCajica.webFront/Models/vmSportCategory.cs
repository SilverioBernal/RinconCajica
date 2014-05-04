using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmSportCategory : SportCategory
    {
        public string nombreDeporte { get; set; }

        public List<Sport> lsDeportes { get; set; }

        public vmSportCategory()
        {
            lsDeportes = new List<Sport>();
        }
    }
}