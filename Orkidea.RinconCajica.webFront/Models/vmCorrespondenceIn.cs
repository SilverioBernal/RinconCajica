using Orkidea.RinconCajica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmCorrespondenceIn : CorrespondenceIn
    {
        public string nombreDependencia { get; set; }
        public string nombreRemitente { get; set; }
        public string nombreUsuarioRecibe { get; set; }
    }
}