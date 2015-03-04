using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmProcessDocument
    {
        public int id { get; set; }

        [Display(Name = "Proceso")]
        [Required(ErrorMessage = "Required")]
        public int idProceso { get; set; }

        [Display(Name = "Subproceso")]
        [Required(ErrorMessage = "Required")]
        public int idTipoDocumento { get; set; }

        //[Display(Name = "Nombre del documento")]
        //[Required(ErrorMessage = "Required")]
        //public string nombre { get; set; }

        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Display(Name = "Archivo adjunto")]
        [Required(ErrorMessage = "Required")]
        public string ruta { get; set; }

        [Display(Name = "Proceso")]
        public string desProceso { get; set; }

        [Display(Name = "Subproceso")]
        public string desTipo { get; set; }

        [Required]
        public HttpPostedFileBase File { get; set; }

        public List<Process> lstProcess { get; set; }
        public List<DocumentType> lstDocType { get; set; }

        public vmProcessDocument()
        {
            lstDocType = new List<DocumentType>();
            lstProcess = new List<Process>();
        }
    }
}