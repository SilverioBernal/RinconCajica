using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmFileUpload
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}