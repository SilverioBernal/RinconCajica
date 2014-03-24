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
    public class PageController : Controller
    {
        BizPage bizPage = new BizPage();
        //
        // GET: /Page/

        public ActionResult Index()
        {
            List<Page> lstPage = bizPage.GetPageList();
            return View(lstPage);
        }

        //
        // GET: /Page/Details/5

        public ActionResult Details(string id)
        {
            ViewBag.id = id;
            Page page = bizPage.GetPagebyKey(new Page() { id = id });
            vmPage _vmPage = new vmPage() { id = page.id, contenidoPrincipal = page.contenidoPrincipal, contenidoSecundario = page.contenidoSecundario, titulo = page.titulo };
            return View(_vmPage);
        }

        //
        // GET: /Page/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Page/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /Page/Edit/5

        public ActionResult Edit(string id)
        {
            return View(bizPage.GetPagebyKey(new Page() { id = id }));
        }

        //
        // POST: /Page/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string id, Page page)
        {
            try
            {
                // TODO: Add update logic here

                page.id = id;
                bizPage.SavePage(page);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        ////
        //// GET: /Page/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Page/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
