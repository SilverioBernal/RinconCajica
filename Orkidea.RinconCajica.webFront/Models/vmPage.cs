using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Models
{
    public class vmPage : Page
    {
        BizHomeSlider bizHomeSlider = new BizHomeSlider();

        public List<HomeSlider> lsHomeSlider { get; set; }


        public vmPage()
        {
            lsHomeSlider = new List<HomeSlider>();
            lsHomeSlider = bizHomeSlider.GetHomeSliderList();
        }
    }
}