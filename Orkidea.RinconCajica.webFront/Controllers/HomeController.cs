using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.webFront.Models;
using System.Text;
using System.Configuration;

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
                string rootPath = Server.MapPath("~");                

                bizCommon.sendContactMessage(email, name,  string.Format("{0} se ha contactado con el Club a traves de la página de contacto", name), message, rootPath);

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
