using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmFrontUser
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string idRol { get; set; }
        public int idSocio { get; set; }
        public string email { get; set; }

        public SortedList<string, string> lstRol { get; set; }
        public IDictionary<int, string> lstSocio { get; set; }


        public vmFrontUser()
        {
            
        }

        public vmFrontUser(bool instanciaDatos)
        {
            lstRol = new SortedList<string, string>();
            lstRol.Add("", "-Seleccione uno-");
            lstRol.Add("A", "Administrador");
            lstRol.Add("S", "Socio");
            lstRol.Add("E", "Empleado operativo");
            lstRol.Add("C", "Calidad");
            lstRol.Add("P", "Porteria");
            lstRol.Add("M", "Administracion Correspondencia y Mensajería");
            lstRol.Add("W", "Empleado administrativo");

            lstSocio = new Dictionary<int, string>();

            if (instanciaDatos)
            {
                BizClubPartner bizClubParner = new BizClubPartner();
                List<ClubPartner> lstSocios = bizClubParner.GetClubPartnerList().Where(x => !string.IsNullOrEmpty(x.nombre)).OrderBy(x => x.nombre).ToList();

                lstSocio.Add(0, "-Seleccione uno-");

                foreach (ClubPartner item in lstSocios)
                {
                    lstSocio.Add(item.docid, item.nombre);
                }
            }
        }
    }
}