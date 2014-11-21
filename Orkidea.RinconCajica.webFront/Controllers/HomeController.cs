using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.webFront.Models;

namespace Orkidea.RinconCajica.webFront.Controllers
{    
    public class HomeController : Controller
    {
        BizHomeSlider bizHomeSlider = new BizHomeSlider();
        public ActionResult Index()
        {
            List<HomeSlider> lsHomeSlider = new List<HomeSlider>();

            try
            {
                lsHomeSlider.AddRange(bizHomeSlider.GetHomeSliderList());
            }
            catch (Exception)
            {

            }
            return View(lsHomeSlider);         
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {            
            ViewBag.menu = "contacto";
            return View();
        }

        public JsonResult sendContactMessage(string name, string email, string message)
        {
            string res = "";

            BizCommon bizCommon = new BizCommon();

            try
            {
                bizCommon.sendContactMessage(email, name + " se ha contactado con el Club", message);

                res = "OK";
            }
            catch (Exception)
            {
                res = "fail";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loginSigmaGolf()
        {
            return View();
        }

        public ActionResult loginSigmaSports()
        {
            return View();
        }
    }
}
