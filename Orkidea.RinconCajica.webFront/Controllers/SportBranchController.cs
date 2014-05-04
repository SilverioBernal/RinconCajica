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
    public class SportBranchController : Controller
    {
        BizSportBranch bizSportBranch = new BizSportBranch();
        BizSport bizSport = new BizSport();
        //
        // GET: /SportBranch/
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

            List<SportBranch> lsCategorias = bizSportBranch.GetSportBranchList();
            List<Sport> lsDeportes = bizSport.GetSportList();

            List<vmSportBranch> lsIndexCategorias = new List<vmSportBranch>();

            foreach (SportBranch item in lsCategorias)
            {
                lsIndexCategorias.Add(new vmSportBranch()
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
        // GET: /SportBranch/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SportBranch/Create
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
            vmSportBranch SportBranch = new vmSportBranch();

            SportBranch.lsDeportes.AddRange(lsDeportes);

            return View(SportBranch);
        }

        //
        // POST: /SportBranch/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(vmSportBranch newSportBranch)
        {
            try
            {
                // TODO: Add insert logic here
                SportBranch SportBranch = new SportBranch()
                {
                    idDeporte = newSportBranch.idDeporte,
                    nombre = newSportBranch.nombre
                };

                bizSportBranch.SaveSportBranch(SportBranch);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportBranch/Edit/5
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

            SportBranch currentSportBranch = bizSportBranch.GetSportBranchbyKey(new SportBranch() { id = id });
            List<Sport> lsDeporte = bizSport.GetSportList();

            vmSportBranch SportBranch = new vmSportBranch()
            {
                id = currentSportBranch.id,
                idDeporte = currentSportBranch.idDeporte,
                nombre = currentSportBranch.nombre,
                nombreDeporte = lsDeporte.Where(x => x.id.Equals(currentSportBranch.idDeporte)).Select(x => x.nombre).FirstOrDefault(),
                lsDeportes = lsDeporte
            };

            return View(SportBranch);
        }

        //
        // POST: /SportBranch/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, vmSportBranch updatedSportBranch)
        {
            try
            {
                // TODO: Add update logic here

                SportBranch SportBranch = new SportBranch()
                {
                    id = id,
                    idDeporte = updatedSportBranch.idDeporte,
                    nombre = updatedSportBranch.nombre
                };

                bizSportBranch.SaveSportBranch(SportBranch);


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportBranch/Delete/5
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

            bizSportBranch.DeleteSportBranch(new SportBranch() { id = id });
            return RedirectToAction("Index");
        }
    }
}
