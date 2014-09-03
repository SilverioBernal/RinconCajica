using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class ClubPartnerController : Controller
    {
        BizClubPartner bizClubPartner = new BizClubPartner();
        BizFrontUser bizFrontUser = new BizFrontUser();

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

        public ActionResult Edit(string id)
        {
            #region Partner identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            int user = 0;
            string rol = "U";
            string idSocio = "";
            bool titular = false;

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                user = int.Parse(userRole[0]);
                rol = userRole[1];

                if (rol == "S")
                {
                    idSocio = userRole[2];
                    titular = userRole[3] == "T" ? true : false;
                }
            }

            #endregion

            Partner partner = new Partner();

            // busqueda de socio por usuario
            if (titular)
                partner = bizClubPartner.GetPartnerbyKey(id, int.Parse(idSocio));
            else
                partner = bizClubPartner.GetPartnerbyKey(id);

            return View(partner);
        }

        //
        // POST: /ClubPartner/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Partner partner)
        {
            #region Partner identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            int user = 0;
            string rol = "U";
            string idSocio = "";
            bool titular = false;

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                user = int.Parse(userRole[0]);
                rol = userRole[1];

                if (rol == "S")
                {
                    idSocio = userRole[2];
                    titular = userRole[3] == "T" ? true : false;
                }
            }

            #endregion
            try
            {
                // TODO: Add update logic here
                partner.docid = id;
                partner.fechaActualizacion = DateTime.Now;
                bizClubPartner.SaveClubPartner(partner);


                return RedirectToAction("Edit", new { id = idSocio });

            }
            catch
            {
                return View();
            }
        }
    }
}
