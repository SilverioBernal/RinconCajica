//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orkidea.RinconCajica.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectDocument
    {
        public int id { get; set; }
        public int idProyecto { get; set; }
        public int idTipoDocumento { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
    
        public virtual DocumentType DocumentType { get; set; }
        public virtual Project Project { get; set; }
    }
}
