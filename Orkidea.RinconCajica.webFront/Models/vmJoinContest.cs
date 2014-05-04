using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmJoinContest : JoinContest
    {
        public SortedDictionary<string, string> lsCategorias { get; set; }
        public SortedDictionary<string, string> lsTipoDoc { get; set; }
        public string rama { get; set; }
        public string modalidad { get; set; }
        public string nombreTorneo { get; set; }
        public string urlPoster { get; set; }

        public vmJoinContest()
        {
            lsCategorias = new SortedDictionary<string, string>();
            lsTipoDoc = new SortedDictionary<string, string>();
            lsTipoDoc.Add(" ", "- Seleccione uno -");
            lsTipoDoc.Add("CC", "Cédula de ciudadanía");
            lsTipoDoc.Add("TI", "Tarjeta de identidad");
            lsTipoDoc.Add("PA", "Pasaporte");
            lsTipoDoc.Add("CE", "Cédula de Exttranjería");
            lsTipoDoc.Add("XX", "Otro");
        }
    }
}