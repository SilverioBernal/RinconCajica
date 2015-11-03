using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class DependenceController : Controller
    {
        BizDependence bizDependence = new BizDependence();
        //
        // GET: /Dependence/

        public ActionResult Index()
        {
            List<Dependence> dependences = bizDependence.GetDependenceList();
            return View(dependences);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Dependence dependencia)
        {
            try
            {
                // TODO: Add insert logic here
                bizDependence.SaveDependence(dependencia);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Dependence dependence = bizDependence.GetDependencebyKey(new Dependence() { id = id });

            return View(dependence);
        }

        [HttpPost]
        public ActionResult Edit(int id, Dependence dependencia)
        {
            try
            {
                // TODO: Add insert logic here
                dependencia.id = id;
                bizDependence.SaveDependence(dependencia);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            bizDependence.DeleteDependence(new Dependence() { id = id });

            return RedirectToAction("Index");
        }

        public JsonResult GetAsyncDependences()
        {            
            List<Dependence> dependences = bizDependence.GetDependenceList();

            return Json(dependences, JsonRequestBehavior.AllowGet);
        }

    }
}
