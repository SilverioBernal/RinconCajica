using AzureUtilities;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.webFront.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class MessagingController : Controller
    {
        BizMessaging bizMessaging = new BizMessaging();
        BizFrontUser bizFrontUser = new BizFrontUser();
        BizDependence bizDependence = new BizDependence();
        BizSupplier bizSupplier = new BizSupplier();
        BizMessenger bizMessenger = new BizMessenger();

        //
        // GET: /Messaging/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Messaging mensaje)
        {
            try
            {
                // TODO: Add insert logic here
                #region Partner identification
                System.Security.Principal.IIdentity context = HttpContext.User.Identity;

                int user = 0;

                if (context.IsAuthenticated)
                {
                    System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                    string[] userRole = ci.Ticket.UserData.Split('|');
                    user = int.Parse(userRole[0]);
                }
                #endregion

                mensaje.fecha = TimeZoneOrgHelper.GetZoneNowDateTime("4.1711", "-74.00639");
                mensaje.usuarioEnvia = user;
                bizMessaging.SaveMessaging(mensaje);                
                return RedirectToAction("Success");
            }
            catch
            {
                return RedirectToAction("Fail");
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Fail()
        {
            return View();
        }

        [Authorize]
        public ActionResult IndexOpenMessages()
        {
            List<Messaging> openMessages = bizMessaging.GetOpenMessagingList();
            List<FrontUser> users = bizFrontUser.GetFrontUserList();
            List<Dependence> dependences = bizDependence.GetDependenceList();
            List<Messenger> mensajeros = bizMessenger.GetMessengerList();
            //List<Supplier> suppliers = bizSupplier.GetSupplierList();
            List<vmMessaging> messages = new List<vmMessaging>();

            foreach (Messaging item in openMessages)
            {
                vmMessaging message = new vmMessaging() 
                {
                    id = item.id,
                    nombreDependenciaOrigen = dependences.Where(x => x.id.Equals(item.origen)).Select(x => x.nombre).First(),
                    descripcion = item.descripcion,
                    fecha = item.fecha,
                    destinatario = item.destinatario,
                    fechaLimite = item.fechaLimite,
                    nombreAutor = users.Where(x => x.id.Equals(item.usuarioEnvia)).Select(x=> x.usuario).First(),
                    direccion = item.direccion,
                    prioridad = item.prioridad,                    
                    mensajero = item.mensajero
                };

                if (item.dependenciaDestino != null)
                    message.nombreDependenciaDestino = dependences.Where(x => x.id.Equals(item.dependenciaDestino)).Select(x => x.nombre).First();

                if (item.mensajero != null)
                    message.nombreMensajero = mensajeros.Where(x => x.id.Equals(item.mensajero)).Select(x => x.nombre).First();

                messages.Add(message);
            }

            return View(messages);
        }

        [Authorize]
        public ActionResult AttachMessenger(int id)
        {
            List<FrontUser> users = bizFrontUser.GetFrontUserList();
            List<Dependence> dependences = bizDependence.GetDependenceList();
            Messaging openMessage = bizMessaging.GetMessagingbyKey(new Messaging() { id = id });

            vmMessaging message = new vmMessaging() 
            {
                id = openMessage.id,
                nombreDependenciaOrigen = dependences.Where(x => x.id.Equals(openMessage.origen)).Select(x => x.nombre).First(),
                descripcion = openMessage.descripcion,
                fecha = openMessage.fecha,
                destinatario = openMessage.destinatario,
                fechaLimite = openMessage.fechaLimite,
                nombreAutor = users.Where(x => x.id.Equals(openMessage.usuarioEnvia)).Select(x => x.usuario).First(),
                direccion = openMessage.direccion,
                prioridad = openMessage.prioridad,                    
            };
            return View(message);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AttachMessenger(int id, Messaging mensaje)
        {            
            Messaging openMessage = bizMessaging.GetMessagingbyKey(new Messaging() { id = id });
            openMessage.mensajero = mensaje.mensajero;
            bizMessaging.SaveMessaging(openMessage);

            return RedirectToAction("IndexOpenMessages");
        }

        [Authorize]
        public ActionResult CloseMessage(int id)
        {
            Messaging openMessage = bizMessaging.GetMessagingbyKey(new Messaging() { id = id });
            openMessage.fechaRealizado = TimeZoneOrgHelper.GetZoneNowDateTime("4.1711", "-74.00639");
            bizMessaging.SaveMessaging(openMessage);

            return RedirectToAction("IndexOpenMessages");
        }

        [Authorize]
        public ActionResult History()
        {
            return View();
        }

        public ActionResult HistoryReport(string id)
        {
            List<FrontUser> users = bizFrontUser.GetFrontUserList();
            List<Dependence> dependences = bizDependence.GetDependenceList();
            List<Messenger> mensajeros = bizMessenger.GetMessengerList();

            string[] parametros = id.Split('|');

            DateTime desde = DateTime.Parse(parametros[0]);
            DateTime hasta = DateTime.Parse(parametros[1]);

            ViewBag.desde = desde.ToString("yyyy-MM-dd");
            ViewBag.hasta = hasta.ToString("yyyy-MM-dd");

            List<Messaging> openMessages = bizMessaging.GetMessagingList(desde, hasta);

            List<vmMessaging> messages = new List<vmMessaging>();

            foreach (Messaging item in openMessages)
            {
                vmMessaging message = new vmMessaging()
                {
                    id = item.id,
                    nombreDependenciaOrigen = dependences.Where(x => x.id.Equals(item.origen)).Select(x => x.nombre).First(),
                    descripcion = item.descripcion,
                    fecha = item.fecha,
                    destinatario = item.destinatario,
                    fechaLimite = item.fechaLimite,
                    nombreAutor = users.Where(x => x.id.Equals(item.usuarioEnvia)).Select(x => x.usuario).First(),
                    direccion = item.direccion,
                    prioridad = item.prioridad,
                    mensajero = item.mensajero,
                    fechaRealizado = item.fechaRealizado
                };

                if (item.dependenciaDestino != null)
                    message.nombreDependenciaDestino = dependences.Where(x => x.id.Equals(item.dependenciaDestino)).Select(x => x.nombre).First();

                if (item.mensajero != null)
                    message.nombreMensajero = mensajeros.Where(x => x.id.Equals(item.mensajero)).Select(x => x.nombre).First();

                messages.Add(message);
            }

            return View(messages);
        }
        public JsonResult GetAsyncPriority()
        {
            List<string> lsPriority = new List<string>();

            lsPriority.Add("Alta");
            lsPriority.Add("Baja");

            return Json(lsPriority, JsonRequestBehavior.AllowGet);
        }

    }
}
