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
        [Authorize]
        public ActionResult Index()
        {
            #region User identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            string rol = "";

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                rol = userRole[1];
            }
            #endregion

            if (rol != "A")
                return RedirectToAction("index", "Home");

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
        [Authorize]
        public ActionResult Create()
        {
            #region User identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            string rol = "";

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                rol = userRole[1];
            }
            #endregion

            if (rol != "A")
                return RedirectToAction("index", "Home");

            return View();
        }

        //
        // POST: /HomeSlider/Create
        [Authorize]
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
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            #region User identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            string rol = "";

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                rol = userRole[1];
            }
            #endregion

            if (rol != "A")
                return RedirectToAction("index", "Home");

            return View(bizHomeSlider.GetHomeSliderbyKey(new HomeSlider() { id = id }));
        }

        //
        // POST: /HomeSlider/Edit/5
        [Authorize]
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
        [Authorize]
        public ActionResult Delete(int id)
        {
            #region User identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            string rol = "";

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                rol = userRole[1];
            }
            #endregion

            if (rol != "A")
                return RedirectToAction("index", "Home");

            return View();
        }

        //
        // POST: /HomeSlider/Delete/5
        [Authorize]
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
