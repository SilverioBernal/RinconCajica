﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class SideBarController : Controller
    {
        BizSideBar bizSideBar = new BizSideBar();

        //
        // GET: /SideBar/
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.menu = "SideBar";
            return View(bizSideBar.GetSideBarList());
        }

        //
        // GET: /SideBar/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.menu = "SideBar";
            return View();
        }

        //
        // POST: /SideBar/Create
        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(SideBar sideBar)
        {
            try
            {
                // TODO: Add insert logic here
                if (string.IsNullOrEmpty(sideBar.contenidoPublico))
                    sideBar.contenidoPublico = " ";

                if (string.IsNullOrEmpty(sideBar.contenidoPrivado))
                    sideBar.contenidoPrivado = " ";

                bizSideBar.SaveSideBar(sideBar);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SideBar/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            SideBar sidebar = bizSideBar.GetSideBarByKey(new SideBar() { id = id });
            ViewBag.menu = "SideBar";
            return View(sidebar);
        }

        //
        // POST: /SideBar/Edit/5
        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(int id, SideBar sideBar)
        {
            try
            {
                // TODO: Add update logic here
                sideBar.id = id;

                if (string.IsNullOrEmpty(sideBar.contenidoPublico))
                    sideBar.contenidoPublico = " ";

                if (string.IsNullOrEmpty(sideBar.contenidoPrivado))
                    sideBar.contenidoPrivado = " ";

                bizSideBar.SaveSideBar(sideBar);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SideBar/Delete/5        
        [Authorize]
        public ActionResult Delete(int id)
        {
            bizSideBar.DeleteSideBar(new SideBar() { id = id });
            
            return RedirectToAction("Index");
        }

        //
        // POST: /SideBar/Delete/5

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
