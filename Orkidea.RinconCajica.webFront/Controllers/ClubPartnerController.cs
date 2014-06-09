using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class ClubPartnerController : Controller
    {
        //
        // GET: /ClubPartner/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ClubPartner/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ClubPartner/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ClubPartner/Create

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
        // GET: /ClubPartner/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ClubPartner/Edit/5

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
        // GET: /ClubPartner/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ClubPartner/Delete/5

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
