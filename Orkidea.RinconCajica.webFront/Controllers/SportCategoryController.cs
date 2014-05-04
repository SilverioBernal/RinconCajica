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
    public class SportCategoryController : Controller
    {
        BizSportCategory bizSportCategory = new BizSportCategory();
        BizSport bizSport = new BizSport();
        //
        // GET: /SportCategory/
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

            List<SportCategory> lsCategorias = bizSportCategory.GetSportCategoryList();
            List<Sport> lsDeportes = bizSport.GetSportList();

            List<vmSportCategory> lsIndexCategorias = new List<vmSportCategory>();

            foreach (SportCategory item in lsCategorias)
            {
                lsIndexCategorias.Add(new vmSportCategory()
                {
                    id = item.id,
                    idDeporte = item.idDeporte,
                    nombre = item.nombre,
                    nombreDeporte = lsDeportes.Where(x => x.id.Equals(item.idDeporte)).Select(x => x.nombre).FirstOrDefault()
                });
            }

            return View(lsIndexCategorias);
        }

        //
        // GET: /SportCategory/Details/5
        
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SportCategory/Create
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

            List<Sport> lsDeportes = bizSport.GetSportList();
            vmSportCategory sportCategory = new vmSportCategory();

            sportCategory.lsDeportes.AddRange(lsDeportes);

            return View(sportCategory);
        }

        //
        // POST: /SportCategory/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(vmSportCategory newSportCategory)
        {
            try
            {
                // TODO: Add insert logic here
                SportCategory sportCategory = new SportCategory()
                {
                    idDeporte = newSportCategory.idDeporte,
                    nombre = newSportCategory.nombre
                };

                bizSportCategory.SaveSportCategory(sportCategory);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportCategory/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
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

            SportCategory currentSportCategory = bizSportCategory.GetSportCategorybyKey(new SportCategory() { id = id });
            List<Sport> lsDeporte = bizSport.GetSportList();

            vmSportCategory sportCategory = new vmSportCategory()
            {
                id = currentSportCategory.id,
                idDeporte = currentSportCategory.idDeporte,
                nombre = currentSportCategory.nombre,
                nombreDeporte = lsDeporte.Where(x => x.id.Equals(currentSportCategory.idDeporte)).Select(x => x.nombre).FirstOrDefault(),
                lsDeportes = lsDeporte
            };

            return View(sportCategory);
        }

        //
        // POST: /SportCategory/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, vmSportCategory updatedSportCategory)
        {
            try
            {
                // TODO: Add update logic here

                SportCategory sportCategory = new SportCategory()
                {
                    id = id,
                    idDeporte = updatedSportCategory.idDeporte,
                    nombre = updatedSportCategory.nombre
                };

                bizSportCategory.SaveSportCategory(sportCategory);


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportCategory/Delete/5
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

            bizSportCategory.DeleteSportCategory(new SportCategory() { id = id });
            return RedirectToAction("Index");
        }

        //
        // POST: /SportCategory/Delete/5

    }
}
