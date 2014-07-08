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
    public class SportModalityController : Controller
    {
        BizSportModality bizSportModality = new BizSportModality();
        BizSport bizSport = new BizSport();
        //
        // GET: /SportModality/
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

            List<SportModality> lsCategorias = bizSportModality.GetSportModalityList();
            List<Sport> lsDeportes = bizSport.GetSportList();

            List<vmSportModality> lsIndexCategorias = new List<vmSportModality>();

            foreach (SportModality item in lsCategorias)
            {
                lsIndexCategorias.Add(new vmSportModality()
                {
                    id = item.id,
                    idDeporte = item.idDeporte,
                    nombre = item.nombre,
                    nombreDeporte = lsDeportes.Where(x => x.id.Equals(item.idDeporte)).Select(x => x.nombre).FirstOrDefault()
                });
            }
            ViewBag.menu = "SportModality";
            return View(lsIndexCategorias);
        }

        //
        // GET: /SportModality/Create
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
            vmSportModality SportModality = new vmSportModality();

            SportModality.lsDeportes.AddRange(lsDeportes);
            ViewBag.menu = "SportModality";
            return View(SportModality);
        }

        //
        // POST: /SportModality/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(vmSportModality newSportModality)
        {
            try
            {
                // TODO: Add insert logic here
                SportModality SportModality = new SportModality()
                {
                    idDeporte = newSportModality.idDeporte,
                    nombre = newSportModality.nombre
                };

                bizSportModality.SaveSportModality(SportModality);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportModality/Edit/5
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

            SportModality currentSportModality = bizSportModality.GetSportModalitybyKey(new SportModality() { id = id });
            List<Sport> lsDeporte = bizSport.GetSportList();

            vmSportModality SportModality = new vmSportModality()
            {
                id = currentSportModality.id,
                idDeporte = currentSportModality.idDeporte,
                nombre = currentSportModality.nombre,
                nombreDeporte = lsDeporte.Where(x => x.id.Equals(currentSportModality.idDeporte)).Select(x => x.nombre).FirstOrDefault(),
                lsDeportes = lsDeporte
            };
            ViewBag.menu = "SportModality";
            return View(SportModality);
        }

        //
        // POST: /SportModality/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, vmSportModality updatedSportModality)
        {
            try
            {
                // TODO: Add update logic here

                SportModality SportModality = new SportModality()
                {
                    id = id,
                    idDeporte = updatedSportModality.idDeporte,
                    nombre = updatedSportModality.nombre
                };

                bizSportModality.SaveSportModality(SportModality);


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportModality/Delete/5
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

            bizSportModality.DeleteSportModality(new SportModality() { id = id });
            return RedirectToAction("Index");
        }
    }
}
