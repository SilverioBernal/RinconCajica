using Orkidea.RinconCajica.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmCorrespondenceOut : CorrespondenceOut
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
        public string nombreOrigen { get; set; }
        public string nombreDestino { get; set; }
        public string nombreUsuarioEnvia { get; set; }
    }
}