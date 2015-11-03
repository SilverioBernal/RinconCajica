using Orkidea.RinconCajica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmMessaging : Messaging
    {
        public string nombreAutor { get; set; }
        public string nombreMensajero { get; set; }
        public string nombreDependenciaOrigen { get; set; }
        public string nombreDependenciaDestino { get; set; }
    }
}