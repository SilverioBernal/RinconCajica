using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmSportSchedule : SportSchedule
    {
        //BizHomeSlider bizHomeSlider = new BizHomeSlider();

        public string nombreDeporte { get; set; }
        public string nombreRama { get; set; }
        //public string nombreCategoria { get; set; }
        public string nombreModalidad { get; set; }
        public string tmpInicio { get; set; }
        public string tmpFin { get; set; }
        public List<Sport> lsDeportes { get; set; }
        public List<SportBranch> lsRamas { get; set; }
        //public List<SportCategory> lsCategorias { get; set; }
        public List<SportModality> lsModalidades { get; set; }
        //public List<HomeSlider> lsHomeSlider { get; set; }
        public List<SportSchedule> lsOtrosEventos { get; set; }

        public vmSportSchedule()
        {
            //lsHomeSlider = new List<HomeSlider>();
            //lsHomeSlider = bizHomeSlider.GetHomeSliderList();
            lsDeportes = new List<Sport>();            
            lsRamas = new List<SportBranch>();
            //lsCategorias = new List<SportCategory>();
            lsModalidades = new List<SportModality>();
            lsOtrosEventos = new List<SportSchedule>();
        }
    }
}