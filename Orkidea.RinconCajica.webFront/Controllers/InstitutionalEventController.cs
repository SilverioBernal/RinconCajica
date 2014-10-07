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
    public class InstitutionalEventController : Controller
    {
        BizInstitutionalEvent bizInstitutionalEvent = new BizInstitutionalEvent();
        //
        // GET: /SportSchedule/
        [Authorize]
        public ActionResult Index()
        {
            List<InstitutionalEvent> lsInstitutionalEvent = bizInstitutionalEvent.GetInstitutionalEventList().OrderBy(x => x.inicio).ToList();

            ViewBag.menu = "Eventos";
            return View(lsInstitutionalEvent);
        }

        //
        // GET: /SportSchedule/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.menu = "Eventos";
            return View();
        }

        //
        // POST: /SportSchedule/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(vmInstitutionalEvent nuevoEvento)
        {
            try
            {
                // TODO: Add insert logic here
                InstitutionalEvent evento = new InstitutionalEvent()
                {
                    nombreEvento = nuevoEvento.nombreEvento,
                    inicio = DateTime.Parse(nuevoEvento.tmpInicio)
                };

                if (!string.IsNullOrEmpty(nuevoEvento.tmpFin))
                    evento.fin = DateTime.Parse(nuevoEvento.tmpFin);

                if (!string.IsNullOrEmpty(nuevoEvento.etiqueta1))
                    evento.etiqueta1 = nuevoEvento.etiqueta1;

                if (!string.IsNullOrEmpty(nuevoEvento.etiqueta2))
                    evento.etiqueta2 = nuevoEvento.etiqueta2;

                if (!string.IsNullOrEmpty(nuevoEvento.etiqueta3))
                    evento.etiqueta3 = nuevoEvento.etiqueta3;

                if (!string.IsNullOrEmpty(nuevoEvento.poster))
                    evento.poster = nuevoEvento.poster;

                if (!string.IsNullOrEmpty(nuevoEvento.urlPagina))
                    evento.urlPagina = nuevoEvento.urlPagina;

                bizInstitutionalEvent.SaveInstitutionalEvent(evento);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportSchedule/Create
        [Authorize]
        public ActionResult Edit(int id)
        {
            InstitutionalEvent eventoModificado = bizInstitutionalEvent.GetInstitutionalEventbyKey(new InstitutionalEvent() { id = id });

            vmInstitutionalEvent evento = new vmInstitutionalEvent()
            {
                id = id,
                nombreEvento = eventoModificado.nombreEvento,
                tmpInicio = eventoModificado.inicio.ToString("yyyy-MM-dd")
            };

            if (eventoModificado.fin != null)
                evento.tmpFin = ((DateTime)eventoModificado.fin).ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(eventoModificado.etiqueta1))
                evento.etiqueta1 = eventoModificado.etiqueta1;

            if (!string.IsNullOrEmpty(eventoModificado.etiqueta2))
                evento.etiqueta2 = eventoModificado.etiqueta2;

            if (!string.IsNullOrEmpty(eventoModificado.etiqueta3))
                evento.etiqueta3 = eventoModificado.etiqueta3;

            if (!string.IsNullOrEmpty(eventoModificado.poster))
                evento.poster = eventoModificado.poster;

            if (!string.IsNullOrEmpty(eventoModificado.urlPagina))
                evento.urlPagina = eventoModificado.urlPagina;

            ViewBag.menu = "Eventos";
            return View(evento);
        }

        //
        // POST: /SportSchedule/Create
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, vmInstitutionalEvent eventoModificado)
        {
            try
            {
                // TODO: Add insert logic here
                InstitutionalEvent evento = new InstitutionalEvent()
                {
                    id = id,
                    nombreEvento = eventoModificado.nombreEvento,
                    inicio = DateTime.Parse(eventoModificado.tmpInicio),
                    visible = eventoModificado.visible
                };

                if (!string.IsNullOrEmpty(eventoModificado.tmpFin))
                    evento.fin = DateTime.Parse(eventoModificado.tmpFin);

                if (!string.IsNullOrEmpty(eventoModificado.etiqueta1))
                    evento.etiqueta1 = eventoModificado.etiqueta1;

                if (!string.IsNullOrEmpty(eventoModificado.etiqueta2))
                    evento.etiqueta2 = eventoModificado.etiqueta2;

                if (!string.IsNullOrEmpty(eventoModificado.etiqueta3))
                    evento.etiqueta3 = eventoModificado.etiqueta3;

                if (!string.IsNullOrEmpty(eventoModificado.poster))
                    evento.poster = eventoModificado.poster;

                if (!string.IsNullOrEmpty(eventoModificado.urlPagina))
                    evento.urlPagina = eventoModificado.urlPagina;

                bizInstitutionalEvent.SaveInstitutionalEvent(evento);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportSchedule/Create
        [Authorize]
        public ActionResult Delete(int id)
        {
            bizInstitutionalEvent.DeleteInstitutionalEvent(new InstitutionalEvent() { id = id });
            ViewBag.menu = "Eventos";
            return RedirectToAction("Index");
        }

        public ActionResult join(int id)
        {
            ViewBag.menu = "Eventos";
            ViewBag.socio = false;
            ViewBag.mensaje = "";

            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            InstitutionalEvent institutionalEvent = bizInstitutionalEvent.GetInstitutionalEventbyKey(new InstitutionalEvent() { id = id });

            vmJoinEvent joinEvent = new vmJoinEvent()
            {
                idEvento = id,
                nombreEvento = institutionalEvent.nombreEvento,
                urlPoster = institutionalEvent.poster,
                etiqueta1 = institutionalEvent.etiqueta1,
                etiqueta2 = institutionalEvent.etiqueta2,
                etiqueta3 = institutionalEvent.etiqueta3
            };

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                string userCode = userRole[0];

                try
                {
                    BizClubPartner bizClubPartner = new BizClubPartner();
                    Partner clubPartner = bizClubPartner.GetClubPartnerbyUser(int.Parse(userCode));

                    joinEvent.identificacion = clubPartner.docto;
                    joinEvent.idSocio = clubPartner.docid;
                    joinEvent.nombre = clubPartner.nombre;
                    joinEvent.telefonoCelular = clubPartner.celular;
                    joinEvent.telefonoFijo = clubPartner.teleco;
                    joinEvent.tipoIdentificacion = clubPartner.doccl;
                    joinEvent.email = clubPartner.correo;

                    ViewBag.socio = true;
                }
                catch (Exception)
                {

                    ViewBag.socio = false;
                }
            }

            if (joinEvent.idSocio == null)
                return RedirectToAction("index", "Home");

            return View(joinEvent);
        }

        [Authorize]
        [HttpPost, RecaptchaControlMvc.CaptchaValidator]
        public ActionResult join(int id, JoinEvent refJoinEvent, bool captchaValid)
        {
            if (!captchaValid)
            {

                ViewBag.menu = "Eventos";
                ViewBag.socio = false;
                ViewBag.mensaje = "Código de verificación incorrecto";

                System.Security.Principal.IIdentity context = HttpContext.User.Identity;

                InstitutionalEvent institutionalEvent = bizInstitutionalEvent.GetInstitutionalEventbyKey(new InstitutionalEvent() { id = id });

                vmJoinEvent joinEvent = new vmJoinEvent()
                {
                    idEvento = id,
                    nombreEvento = institutionalEvent.nombreEvento,
                    urlPoster = institutionalEvent.poster,
                    etiqueta1 = institutionalEvent.etiqueta1,
                    etiqueta2 = institutionalEvent.etiqueta2,
                    etiqueta3 = institutionalEvent.etiqueta3
                };

                if (context.IsAuthenticated)
                {
                    System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                    string[] userRole = ci.Ticket.UserData.Split('|');

                    string userCode = userRole[0];

                    try
                    {
                        BizClubPartner bizClubPartner = new BizClubPartner();
                        Partner clubPartner = bizClubPartner.GetClubPartnerbyUser(int.Parse(userCode));

                        joinEvent.identificacion = clubPartner.docto;
                        joinEvent.idSocio = clubPartner.docid;
                        joinEvent.nombre = clubPartner.nombre;
                        joinEvent.telefonoCelular = clubPartner.celular;
                        joinEvent.telefonoFijo = clubPartner.teleco;
                        joinEvent.tipoIdentificacion = clubPartner.doccl;
                        joinEvent.email = clubPartner.correo;

                        ViewBag.socio = true;
                    }
                    catch (Exception)
                    {

                        ViewBag.socio = false;
                    }
                }

                if (joinEvent.idSocio == null)
                    return RedirectToAction("index", "Home");

                return View(joinEvent);
            }
            else
            {
                BizJoinEvent bizJoinEvent = new BizJoinEvent();

                JoinEvent newJoinEvent = new JoinEvent()
                {
                    idEvento = id,
                    fechaInscripcion = DateTime.Now,
                    email = refJoinEvent.email,
                    identificacion = refJoinEvent.identificacion,
                    idSocio = refJoinEvent.idSocio,
                    nombre = refJoinEvent.nombre,
                    telefonoCelular = refJoinEvent.telefonoCelular,
                    telefonoFijo = refJoinEvent.telefonoFijo,
                    campoLibre1 = refJoinEvent.campoLibre1,
                    campoLibre2 = refJoinEvent.campoLibre2,
                    campoLibre3 = refJoinEvent.campoLibre3,
                };

                bizJoinEvent.SaveJoinEvent(newJoinEvent);
                return RedirectToAction("joined");
            }

        }

        public ActionResult joined()
        {
            return View();
        }

        public ActionResult IndexNextSchedule()
        {
            ViewBag.menu = "Eventos";

            DateTime desde = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            List<InstitutionalEvent> lsSportSchedule = bizInstitutionalEvent.GetInstitutionalEventList().Where(x => x.inicio >= desde).OrderBy(x => x.inicio).ToList();

            return View(lsSportSchedule);
        }

        public ActionResult IndexOlderSchedule(string id)
        {
            ViewBag.menu = "Eventos";

            DateTime desde = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            List<InstitutionalEvent> lsSportSchedule = bizInstitutionalEvent.GetInstitutionalEventList().Where(x => x.inicio < desde).OrderBy(x => x.inicio).ToList();

            return View(lsSportSchedule);            
        }

    }
}
