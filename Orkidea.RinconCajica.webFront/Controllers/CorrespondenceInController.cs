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
    public class CorrespondenceInController : Controller
    {
        BizCorrespondenceIn bizCorrespondenceIn = new BizCorrespondenceIn();
        BizDependence bizDependence = new BizDependence();
        BizSupplier bizSupplier = new BizSupplier();
        BizFrontUser bizFrontUser = new BizFrontUser();
        //
        // GET: /CorrespondenceIn/

        public ActionResult Index()
        {            
            return View(GetCorrespondenceIn());
        }

        public ActionResult IndexRoller()
        {
            return View(GetCorrespondenceIn());
        }

        public ActionResult Rolling(string id) 
        {
            List<vmCorrespondenceIn> correosPendientes = GetCorrespondenceIn();
            List<vmCorrespondenceIn> correosPatinados = new List<vmCorrespondenceIn>();
            List<CorrespondenceIn> correos = bizCorrespondenceIn.GetNotRollerCorrespondenceInList().ToList();
            string[] ids = id.Split('-');

            foreach (string item in ids)
            {
                if (correos.Where(x => x.id.Equals(int.Parse(item))).Count() != 0)
                {
                    CorrespondenceIn patinado = correos.Where(x => x.id.Equals(int.Parse(item))).First();
                    patinado.fechaPatinado = TimeZoneOrgHelper.GetZoneNowDateTime("4.1711", "-74.00639");
                    bizCorrespondenceIn.SaveCorrespondenceIn(patinado);

                    correosPatinados.Add(correosPendientes.Where(x => x.id.Equals(int.Parse(item))).First());
                }
                    
            }

            return View(correosPatinados);
        }

        //
        // GET: /CorrespondenceIn/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CorrespondenceIn/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(CorrespondenceIn correo)
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

                correo.fecha = TimeZoneOrgHelper.GetZoneNowDateTime("4.1711", "-74.00639");
                correo.usuarioRecibe = user;
                bizCorrespondenceIn.SaveCorrespondenceIn(correo);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(correo);
            }
        }

        public ActionResult History()
        {
            return View();
        }

        public ActionResult HistoryReport(string id)
        {
            string[] parametros = id.Split('|');

            DateTime desde = DateTime.Parse(parametros[0]);
            DateTime hasta = DateTime.Parse(parametros[1]);

            ViewBag.desde = desde.ToString("yyyy-MM-dd");
            ViewBag.hasta = hasta.ToString("yyyy-MM-dd");

            return View(GetCorrespondenceIn(desde, hasta));
        }

        private List<vmCorrespondenceIn> GetCorrespondenceIn()
        {
            List<CorrespondenceIn> correos = bizCorrespondenceIn.GetNotRollerCorrespondenceInList().ToList();
            List<Dependence> dependencias = bizDependence.GetDependenceList();
            List<Supplier> proveedores = bizSupplier.GetSupplierList();
            List<FrontUser> usuarios = bizFrontUser.GetFrontUserList();
            List<vmCorrespondenceIn> recibidos = new List<vmCorrespondenceIn>();

            foreach (CorrespondenceIn item in correos)
            {
                recibidos.Add(new vmCorrespondenceIn()
                {
                    id = item.id,
                    nombreDependencia = dependencias.Where(x => x.id.Equals(item.dependencia)).Select(x => x.nombre).FirstOrDefault(),
                    nombreRemitente = proveedores.Where(x => x.id.Equals(item.remitente)).Select(x => x.nombre).FirstOrDefault(),
                    nombreUsuarioRecibe = usuarios.Where(x => x.id.Equals(item.usuarioRecibe)).Select(x => x.usuario).FirstOrDefault(),
                    fecha = item.fecha,
                    descripcion = item.descripcion
                });
            }

            return recibidos;
        }

        private List<vmCorrespondenceIn> GetCorrespondenceIn(DateTime desde, DateTime hasta)
        {
            List<CorrespondenceIn> correos = bizCorrespondenceIn.GetCorrespondenceInList(desde, hasta).ToList();
            List<Dependence> dependencias = bizDependence.GetDependenceList();
            List<Supplier> proveedores = bizSupplier.GetSupplierList();
            List<FrontUser> usuarios = bizFrontUser.GetFrontUserList();
            List<vmCorrespondenceIn> recibidos = new List<vmCorrespondenceIn>();

            foreach (CorrespondenceIn item in correos)
            {
                recibidos.Add(new vmCorrespondenceIn()
                {
                    id = item.id,
                    nombreDependencia = dependencias.Where(x => x.id.Equals(item.dependencia)).Select(x => x.nombre).FirstOrDefault(),
                    nombreRemitente = proveedores.Where(x => x.id.Equals(item.remitente)).Select(x => x.nombre).FirstOrDefault(),
                    nombreUsuarioRecibe = usuarios.Where(x => x.id.Equals(item.usuarioRecibe)).Select(x => x.usuario).FirstOrDefault(),
                    fecha = item.fecha,
                    descripcion = item.descripcion
                });
            }

            return recibidos;
        }
    }
}
