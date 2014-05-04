using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
        BizSideBar bizSideBar = new BizSideBar();

        //
        // GET: /Page/
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

            ViewBag.paginasNoBorrables = ConfigurationManager.AppSettings["paginasNoBorrables"].ToString();
            List<Page> lstPage = bizPage.GetPageList();
            return View(lstPage);
        }

        //
        // GET: /Page/Details/5

        public ActionResult Details(string id)
        {
            string deportes = ConfigurationManager.AppSettings["deportes"].ToString() ;

            if (deportes.Contains(id))
                ViewBag.menu = "Deportes";
            else
                ViewBag.menu = id;

            Page page = bizPage.GetPagebyKey(new Page() { id = id });

            string sideBarContent = "";

            if (page.idSideBar != null && page.idSideBar != 0)
            {
               sideBarContent =  bizSideBar.GetSideBarByKey(new SideBar() { id = (int)page.idSideBar }).contenido;

            }
            vmPage _vmPage = new vmPage()
            {
                id = page.id,
                contenidoPublico = page.contenidoPublico,
                contenidoPrivado = page.contenidoPrivado,
                idSideBar = page.idSideBar,
                titulo = page.titulo,
                sideBarContent = sideBarContent
            };
            return View(_vmPage);
        }

        //
        // GET: /Page/Create
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

            vmPage page = new vmPage();
            return View(page);
        }

        //
        // POST: /Page/Create
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(vmPage pageTarget)
        {
            // TODO: Add insert logic here
            try
            {

                Page page = new Page()
                {
                    contenidoPrivado = pageTarget.contenidoPrivado,
                    contenidoPublico = pageTarget.contenidoPublico,
                    idSideBar = pageTarget.idSideBar,
                    titulo = pageTarget.titulo
                };

                page.id = Regex.Replace(
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pageTarget.titulo
                    .ToLower()
                    .Replace(@"@", "a")
                    .Replace('á', 'a')
                    .Replace('é', 'e')
                    .Replace('í', 'i')
                    .Replace('ó', 'o')
                    .Replace('ú', 'u')
                    .Replace('ñ', 'n')
                    .Replace('ü', 'u')
                    ), @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));

                bizPage.SavePage(page);

                return RedirectToAction("Index");
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return View();
            }
        }

        //
        // GET: /Page/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
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

            Page page = bizPage.GetPagebyKey(new Page() { id = id });

            vmPage _vmPage = new vmPage()
            {
                id = id,
                contenidoPrivado = page.contenidoPrivado,
                contenidoPublico = page.contenidoPublico,
                idSideBar = page.idSideBar,
                titulo = page.titulo
            };

            return View(_vmPage);
        }

        //
        // POST: /Page/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string id, vmPage _page)
        {
            try
            {
                // TODO: Add update logic here
                Page page = new Page()
                {
                    contenidoPrivado = _page.contenidoPrivado,
                    contenidoPublico = _page.contenidoPublico,
                    idSideBar = _page.idSideBar == 0 ? null : _page.idSideBar,
                    titulo = _page.titulo
                };

                page.id = id;
                bizPage.SavePage(page);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Page/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
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

            try
            {
                bizPage.DeletePage(new Page() { id = id });

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

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

