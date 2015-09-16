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
            List<ClubPartner> socios = bizClubPartner.GetClubPartnerList();
            List<FrontUser> usuarios = bizFrontUser.GetFrontUserList();

            foreach (ClubPartner socio in socios)
            {
                if (socio.idUsuario != null)
                    socio.faxres = usuarios.Where(x => x.id.Equals(socio.idUsuario)).Select(x => x.usuario).FirstOrDefault();
            }

            return View(socios);
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
            Partner partner = new Partner();
            return View(partner);
        }

        //
        // POST: /ClubPartner/Create

        [HttpPost]
        public ActionResult Create(Partner partner)
        {
            try
            {
                // TODO: Add update logic here                
                partner.fechaActualizacion = DateTime.Now;
                bizClubPartner.SaveClubPartner(partner);

                return RedirectToAction("Index");

            }
            catch
            {                
                return View(partner);
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
            int serializatedSocio = int.Parse(id, System.Globalization.NumberStyles.HexNumber);
            // busqueda de socio por usuario
            if (titular)
                partner = bizClubPartner.GetPartnerbyKey(serializatedSocio.ToString(), int.Parse(idSocio));
            else
                partner = bizClubPartner.GetPartnerbyKey(id);

            return View(partner);
        }

        //
        // POST: /ClubPartner/Edit/5

        [HttpPost]
        public ActionResult Edit(string id, Partner partner)
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
                int serializatedSocio = int.Parse(id, System.Globalization.NumberStyles.HexNumber);
                // TODO: Add update logic here
                partner.docid = serializatedSocio;
                partner.fechaActualizacion = DateTime.Now;
                bizClubPartner.SaveClubPartner(partner);


                return RedirectToAction("Edit", new { id = id });

            }
            catch
            {
                return View();
            }
        }

        public ActionResult delete(int id)
        {            
            bizClubPartner.DeleteClubPartner(new ClubPartner() { docid = id });

            return RedirectToAction("Index");
        }
    }
}
