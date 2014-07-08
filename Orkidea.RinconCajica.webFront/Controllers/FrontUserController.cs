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
    public class FrontUserController : Controller
    {
        //
        // GET: /FrontUser/
        BizFrontUser bizFrontUser = new BizFrontUser();
        public ActionResult Index()
        {
            List<FrontUser> lsFrontUser = bizFrontUser.GetFrontUserList().OrderBy(x => x.usuario).ToList();
            return View(lsFrontUser);
        }

        //
        // GET: /FrontUser/Create

        public ActionResult Create()
        {
            vmFrontUser user = new vmFrontUser(true);
            return View(user);
        }

        //
        // POST: /FrontUser/Create

        [HttpPost]
        public ActionResult Create(vmFrontUser user)
        {
            try
            {
                int id = bizFrontUser.SaveFrontUser(new FrontUser()
                {
                    usuario = user.usuario,
                    email = user.email,
                    idRol = user.idRol
                });

                BizClubPartner bizClubPartner = new BizClubPartner();
                ClubPartner clubPartner = bizClubPartner.GetClubPartnerbyKey(new ClubPartner() { id = user.idSocio });

                if (user.email.Trim() != clubPartner.correo.Trim())
                    clubPartner.correo = user.email;

                clubPartner.idUsuario = id;
                bizClubPartner.SaveClubPartner(clubPartner);


                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /FrontUser/Edit/5

        public ActionResult Edit(int id)
        {
            vmFrontUser user = new vmFrontUser(true);

            FrontUser frontUser = bizFrontUser.GetFrontUserbyKey(new FrontUser() { id = id });

            user.id = id;
            user.idRol = frontUser.idRol;
            user.usuario = frontUser.usuario;
            user.email = frontUser.email;

            if (frontUser.idRol == "S")
            {
                BizClubPartner bizClubPartner = new BizClubPartner();
                ClubPartner clubPartner = bizClubPartner.GetClubPartnerbyUser(id);

                if (clubPartner != null)
                    user.idSocio = clubPartner.id;
            }

            return View(user);
        }

        //
        // POST: /FrontUser/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, vmFrontUser user)
        {
            try
            {
                FrontUser currentUser = bizFrontUser.GetFrontUserbyKey(new FrontUser() { id = id });

                if (user.email.Trim() != currentUser.email)
                {
                    currentUser.email = user.email;
                    bizFrontUser.SaveFrontUser(currentUser);

                    BizClubPartner bizClubPartner = new BizClubPartner();
                    ClubPartner clubPartner = bizClubPartner.GetClubPartnerbyKey(new ClubPartner() { id = user.idSocio });

                    if (user.email.Trim() != clubPartner.correo.Trim())
                    {
                        clubPartner.correo = user.email;
                        bizClubPartner.SaveClubPartner(clubPartner);
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /FrontUser/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                FrontUser currentUser = bizFrontUser.GetFrontUserbyKey(new FrontUser() { id = id });

                if (currentUser != null)
                {
                    bizFrontUser.DeleteFrontUser(currentUser);

                    BizClubPartner bizClubPartner = new BizClubPartner();
                    ClubPartner clubPartner = bizClubPartner.GetClubPartnerbyUser(id);

                    if (clubPartner!=null)
                    {
                        clubPartner.idUsuario = null;
                        bizClubPartner.SaveClubPartner(clubPartner); ;
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public JsonResult sugerirUsuario(int id)
        {
            string res = "";

            try
            {
                res = bizFrontUser.UserNameSuggest(id);
            }
            catch (Exception)
            {
                res = "";
            }

            return Json(res.Split('|'), JsonRequestBehavior.AllowGet);
        }

        public JsonResult obtenerCorreoSocio(int id)
        {
            string res = "";

            try
            {
                BizClubPartner bizClubPartner = new BizClubPartner();
                ClubPartner clubPartner = bizClubPartner.GetClubPartnerbyKey(new ClubPartner() { id = id });
                res = clubPartner.correo;
            }
            catch (Exception)
            {
                res = "";
            }

            return Json(res.Split('|'), JsonRequestBehavior.AllowGet);
        }
    }
}
