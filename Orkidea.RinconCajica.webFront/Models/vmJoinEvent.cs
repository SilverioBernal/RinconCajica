using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmJoinEvent : JoinEvent
    {
        public string etiqueta1 { get; set; }
        public string etiqueta2 { get; set; }
        public string etiqueta3 { get; set; }
        public string urlPoster { get; set; }        
        public string nombreEvento { get; set; }
    }
}