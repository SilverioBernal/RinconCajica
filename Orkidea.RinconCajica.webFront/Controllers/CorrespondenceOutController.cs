using AzureUtilities;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.Utilities;
using Orkidea.RinconCajica.webFront.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class CorrespondenceOutController : Controller
    {
        BizCorrespondenceOut bizCorrespondenceOut = new BizCorrespondenceOut();
        BizDependence bizDependence = new BizDependence();
        BizSupplier bizSupplier = new BizSupplier();
        BizFrontUser bizFrontUser = new BizFrontUser();
        //
        // GET: /CorrespondenceOut/

        public ActionResult Index()
        {
            return View(bizCorrespondenceOut.GetCorrespondenceOutList());
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(vmCorrespondenceOut correo)
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

                string fileExtension = Path.GetExtension(correo.File.FileName);
                string fileName = Guid.NewGuid().ToString() + fileExtension;

                AzureStorageHelper.uploadFile(correo.File.InputStream, fileName, "uploadedFiles");

                CorrespondenceOut correoEnviado = new CorrespondenceOut() { 
                    destinatario = correo.destinatario,
                    direccion = correo.direccion,
                    destino = correo.destino,
                    fecha = TimeZoneOrgHelper.GetZoneNowDateTime("4.1711", "-74.00639"),
                    imagen = fileName,
                    origen = correo.origen,
                    tipoDocumento = correo.tipoDocumento,
                    usuarioEnvia = user
                };

                bizCorrespondenceOut.SaveCorrespondenceOut(correoEnviado);
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

            return View(GetCorrespondenceOut(desde, hasta));
        }

        public ActionResult HistoryImgReport(string id)
        {
            string[] parametros = id.Split('|');

            DateTime desde = DateTime.Parse(parametros[0]);
            DateTime hasta = DateTime.Parse(parametros[1]);

            ViewBag.desde = desde.ToString("yyyy-MM-dd");
            ViewBag.hasta = hasta.ToString("yyyy-MM-dd");

            return View(GetCorrespondenceOut(desde, hasta));
        }

        public ActionResult GetImage(string archivo)
        {
            string mimeType = "";
            int cuentaPuntos = 0;

            string[] nombreArchivo = archivo.Split('.');
            cuentaPuntos = nombreArchivo.Length;

            mimeType = BizMimeType.GetMimeType(nombreArchivo[cuentaPuntos - 1]).mimetype1;

            //dynamically generate a file
            System.IO.MemoryStream ms;
            ms = AzureStorageHelper.getFile(archivo, "uploadedFiles");

            // return the file
            return File(ms.ToArray(), mimeType);
        }

        public JsonResult GetAsyncDocumentType() 
        {
            List<string> lsTipoDoc = new List<string>();
            string[] tiposDoc = ConfigurationManager.AppSettings["tipoDocumento"].ToString().Split('|');            

            foreach (string item in tiposDoc)
                lsTipoDoc.Add(item);

            return Json(lsTipoDoc, JsonRequestBehavior.AllowGet);
        }

        private List<vmCorrespondenceOut> GetCorrespondenceOut(DateTime desde, DateTime hasta)
        {
            List<CorrespondenceOut> correos = bizCorrespondenceOut.GetCorrespondenceOutList(desde, hasta).ToList();
            List<Dependence> dependencias = bizDependence.GetDependenceList();
            List<Supplier> proveedores = bizSupplier.GetSupplierList();
            List<FrontUser> usuarios = bizFrontUser.GetFrontUserList();
            List<vmCorrespondenceOut> recibidos = new List<vmCorrespondenceOut>();

            foreach (CorrespondenceOut item in correos)
            {
                recibidos.Add(new vmCorrespondenceOut()
                {
                    id = item.id,
                    nombreOrigen = dependencias.Where(x => x.id.Equals(item.origen)).Select(x => x.nombre).FirstOrDefault(),
                    nombreDestino = proveedores.Where(x => x.id.Equals(item.destino)).Select(x => x.nombre).FirstOrDefault(),
                    nombreUsuarioEnvia = usuarios.Where(x => x.id.Equals(item.usuarioEnvia)).Select(x => x.usuario).FirstOrDefault(),
                    fecha = item.fecha,
                    direccion = item.direccion,
                    destinatario = item.destinatario,
                    tipoDocumento = item.tipoDocumento,
                    imagen = item.imagen
                });
            }

            return recibidos;
        }
    }
}
