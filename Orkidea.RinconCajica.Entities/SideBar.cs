//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orkidea.RinconCajica.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class SideBar
    {
        public SideBar()
        {
            this.Page = new HashSet<Page>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public string contenidoPublico { get; set; }
        public string contenidoPrivado { get; set; }
    
        public virtual ICollection<Page> Page { get; set; }
    }
}
