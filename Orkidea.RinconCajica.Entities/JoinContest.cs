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
    
    public partial class JoinContest
    {
        public int id { get; set; }
        public int idTorneo { get; set; }
        public Nullable<int> idSocio { get; set; }
        public string tipoIdentificacion { get; set; }
        public string identificacion { get; set; }
        public string nombre { get; set; }
        public string telefonoFijo { get; set; }
        public string telefonoCelular { get; set; }
        public string email { get; set; }
        public string categoria { get; set; }
        public System.DateTime fechaInscripcion { get; set; }
    
        public virtual SportSchedule SportSchedule { get; set; }
    }
}
