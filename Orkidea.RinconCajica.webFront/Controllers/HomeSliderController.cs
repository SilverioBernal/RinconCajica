using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class HomeSliderController : Controller
    {
        BizHomeSlider bizHomeSlider = new BizHomeSlider();

        //
        // GET: /HomeSlider/

        public ActionResult Index()
        {
            List<HomeSlider> lsHomeSlider = new List<HomeSlider>();

            try
            {
                lsHomeSlider.AddRange(bizHomeSlider.GetHomeSliderList());
            }
            catch (Exception )
            {
                                
            }
            return View(lsHomeSlider);
        }

        ////
        //// GET: /HomeSlider/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //
        // GET: /HomeSlider/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HomeSlider/Create

        [HttpPost]
        public ActionResult Create(HomeSlider homeSlider)
        {
            try
            {
                // TODO: Add insert logic here
                homeSlider.id = Guid.NewGuid();

                bizHomeSlider.SaveHomeSlider(homeSlider);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /HomeSlider/Edit/5

        public ActionResult Edit(Guid id)
        {            
            return View(bizHomeSlider.GetHomeSliderbyKey(new HomeSlider() { id = id }));
        }

        //
        // POST: /HomeSlider/Edit/5

        [HttpPost]
        public ActionResult Edit(Guid id, HomeSlider homeSlider)
        {
            try
            {
                // TODO: Add insert logic here
                homeSlider.id = id;

                bizHomeSlider.SaveHomeSlider(homeSlider);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /HomeSlider/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /HomeSlider/Delete/5

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
