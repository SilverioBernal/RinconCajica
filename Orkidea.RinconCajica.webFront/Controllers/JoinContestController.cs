using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.webFront.Models;
using Recaptcha;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class JoinContestController : Controller
    {
        BizFrontUser bizFrontUser = new BizFrontUser();
        BizClubPartner bizClubPartner = new BizClubPartner();
        BizJoinContest bizJoinContest = new BizJoinContest();
        BizSportSchedule bizSportSchedule = new BizSportSchedule();

        [Authorize]
        public ActionResult join(int id)
        {
            ViewBag.menu = "Deportes";
            ViewBag.socio = false;
            ViewBag.mensaje = "";

            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            SportSchedule sportSchedule = bizSportSchedule.GetSportSchedulebyKey(new SportSchedule() { id = id });
            string[] categorias = sportSchedule.categoria.Split(',');

            vmJoinContest joinContest = new vmJoinContest { idTorneo = id, nombreTorneo = sportSchedule.competencia.ToUpper(), urlPoster = sportSchedule.poster };

            foreach (string item in categorias)
                joinContest.lsCategorias.Add(item, item);

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                string userCode = userRole[0];

                try
                {
                    Partner clubPartner = bizClubPartner.GetClubPartnerbyUser(int.Parse(userCode));

                    joinContest.identificacion = clubPartner.docto;
                    joinContest.idSocio = clubPartner.docid;
                    joinContest.nombre = clubPartner.nombre;
                    joinContest.telefonoCelular = clubPartner.celular;
                    joinContest.telefonoFijo = clubPartner.teleco;
                    joinContest.tipoIdentificacion = clubPartner.doccl;
                    joinContest.email = clubPartner.correo;

                    ViewBag.socio = true;
                }
                catch (Exception)
                {

                    ViewBag.socio = false;
                }
            }

            if (sportSchedule.privado && joinContest.idSocio == null)
                return RedirectToAction("index", "Home");

            return View(joinContest);
        }

        [Authorize]
        [HttpPost, RecaptchaControlMvc.CaptchaValidator]
        public ActionResult join(int id, JoinContest refJoinContest, bool captchaValid)
        {
            if (!captchaValid)
            {

                ViewBag.menu = "Deportes";
                ViewBag.socio = false;
                ViewBag.mensaje = "Código de verificación incorrecto";

                System.Security.Principal.IIdentity context = HttpContext.User.Identity;

                SportSchedule sportSchedule = bizSportSchedule.GetSportSchedulebyKey(new SportSchedule() { id = id });

                string[] categorias = sportSchedule.categoria.Split(',');

                vmJoinContest joinContest = new vmJoinContest { idTorneo = id, nombreTorneo = sportSchedule.competencia.ToUpper(), urlPoster = sportSchedule.poster };

                foreach (string item in categorias)
                    joinContest.lsCategorias.Add(item, item);

                if (context.IsAuthenticated)
                {
                    System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                    string[] userRole = ci.Ticket.UserData.Split('|');

                    string userCode = userRole[0];

                    try
                    {
                        ClubPartner clubPartner = bizClubPartner.GetClubPartnerbyKey(new ClubPartner() { docid = int.Parse(userCode) });

                        joinContest.identificacion = clubPartner.docto;
                        joinContest.idSocio = clubPartner.docid;
                        joinContest.nombre = clubPartner.nombre;
                        joinContest.telefonoCelular = clubPartner.celular;
                        joinContest.telefonoFijo = clubPartner.telefn;
                        joinContest.tipoIdentificacion = clubPartner.doccl;

                        ViewBag.socio = true;
                    }
                    catch (Exception)
                    {

                        ViewBag.socio = false;
                    }
                }

                return View(joinContest);
            }
            else
            {

                JoinContest newJoinContest = new JoinContest()
                {
                    idTorneo = id,
                    fechaInscripcion = DateTime.Now,
                    categoria = refJoinContest.categoria,
                    email = refJoinContest.email,
                    identificacion = refJoinContest.identificacion,
                    idSocio = refJoinContest.idSocio,
                    nombre = refJoinContest.nombre,
                    telefonoCelular = refJoinContest.telefonoCelular,
                    telefonoFijo = refJoinContest.telefonoFijo
                };

                string rootPath = Server.MapPath("~");                

                bizJoinContest.SaveJoinContest(newJoinContest, rootPath);
                return RedirectToAction("joined");
            }

        }

        public ActionResult joined()
        {
            return View();
        }

        //public JsonResult join(JoinContest joinContest)
        //{
        //    string res;

        //    try
        //    {
        //        bizJoinContest.SaveJoinContest(joinContest);
        //        res = "Ok";
        //    }
        //    catch (Exception)
        //    {
        //        res = "Fail";
        //    }

        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
    }
}
