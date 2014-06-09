using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class FrontUserController : Controller
    {
        //
        // GET: /FrontUser/
        BizFrontUser bizFrontUser = new BizFrontUser(); 
        public ActionResult Index()
        {
            List<FrontUser> lsFrontUser = bizFrontUser.GetFrontUserList().OrderBy(x => x.usuario).ToList();
            return View(lsFrontUser);
        }

        //
        // GET: /FrontUser/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /FrontUser/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FrontUser/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /FrontUser/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /FrontUser/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /FrontUser/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /FrontUser/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
