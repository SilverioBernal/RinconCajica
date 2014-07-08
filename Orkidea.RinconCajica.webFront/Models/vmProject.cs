using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmProject
    {
        public int id { get; set; }
        [Display(Name = "Nombre proyecto")]
        [Required(ErrorMessage = "Required")]
        public string nombre { get; set; }
    }
}