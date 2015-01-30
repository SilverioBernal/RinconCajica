using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.webFront.Models;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class MessageActorController : Controller
    {
        BizMessageActor bizMessageActor = new BizMessageActor();
        BizMessageBitacore bizMessageBitacore = new BizMessageBitacore();

        //
        // GET: /MessageActor/

        public ActionResult IndexDependencias()
        {
            List<MessageActor> lsMessageActor = bizMessageActor.GetMessageActorListByType("D");
            return View(lsMessageActor);
        }

        public ActionResult IndexProveedores()
        {
            List<MessageActor> lsMessageActor = bizMessageActor.GetMessageActorListByType("P");
            return View(lsMessageActor);
        }

        public ActionResult IndexRecibidoHoy()
        {
            List<MessageBitacore> lsRecibido = bizMessageBitacore.GetMessageBitacoreList("I", DateTime.Now);
            List<vmMessageBitacore> lsRecibidoHoy = new List<vmMessageBitacore>();

            foreach (MessageBitacore item in lsRecibido)
            {
                lsRecibidoHoy.Add(new vmMessageBitacore(item));
            }

            return View(lsRecibidoHoy.OrderBy(x => x.fecha).ToList());
        }

        public ActionResult ReporteRecibido()
        {
            return View();
        }

        public ActionResult exportaRecibido(string id)
        {
            string[] parametros = id.Split('|');

            DateTime desde = DateTime.Parse(parametros[0]);
            DateTime hasta = DateTime.Parse(parametros[1]);
            string tipo = parametros[2];

            List<MessageBitacore> lsRecibido = bizMessageBitacore.GetMessageBitacoreList(tipo, desde, hasta);
            List<vmMessageBitacore> lsReport = new List<vmMessageBitacore>();
            List<vmMessageBitacoreInputReport> lsReportExport = new List<vmMessageBitacoreInputReport>();

            foreach (MessageBitacore item in lsRecibido)
            {
                lsReport.Add(new vmMessageBitacore(item));
            }

            foreach (vmMessageBitacore item in lsReport)
            {
                lsReportExport.Add(new vmMessageBitacoreInputReport()
                {
                    descripcionCorta = item.descripcionCorta,
                    descripcionLarga = item.descripcionLarga,
                    destino = item.strDestino,
                    fecha = item.fecha,
                    origen = item.strOrigen,
                    idRegistro = item.idRegistro
                });
            }

            string fileName = Regex.Replace(
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Correo_recibido_desde_" + parametros[0] + "_hasta_" + parametros[1]
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

            var stream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(lsReportExport.GetType());

            serializer.Serialize(stream, lsReportExport);
            stream.Position = 0;

            return File(stream, "application/vnd.res-excel", fileName + ".xls");
        }

        public ActionResult GeneraReporteGeneral()
        {
            vmMessageBitacore messageBitacore = new vmMessageBitacore();
            return View(messageBitacore);
        }

        public ActionResult ReporteGeneral(string id)
        {
            string[] parametros = id.Split('|');

            MessageBitacore mb = new MessageBitacore();
            List<MessageBitacore> lsMB = new List<MessageBitacore>();
            List<vmMessageBitacoreReport> lsMessageBitacore = new List<vmMessageBitacoreReport>();
            List<MessageActor> lsMessageActor = bizMessageActor.GetMessageActorList();

            if (!string.IsNullOrEmpty(parametros[0]))
                mb.fecha = DateTime.Parse(parametros[0]);

            if (!string.IsNullOrEmpty(parametros[1]))
                mb.prioridad = byte.Parse(parametros[1]);
            //else
                //mb.prioridad = 1;

            if (!string.IsNullOrEmpty(parametros[2]))
                mb.descripcionCorta = parametros[2];

            if (!string.IsNullOrEmpty(parametros[3]))
                mb.fechaLimite = DateTime.Parse(parametros[3]);

            if (!string.IsNullOrEmpty(parametros[4]))
                mb.entregaPersonal = bool.Parse(parametros[4]);

            lsMB = bizMessageBitacore.GetMessageBitacoreList(mb);

            foreach (MessageBitacore item in lsMB)
            {
                vmMessageBitacoreReport mbr = new vmMessageBitacoreReport()
                {
                    descripcionCorta = item.descripcionCorta,
                    descripcionLarga = item.descripcionLarga,
                    destino = lsMessageActor.Where(x => x.id.Equals(item.destino)).Select(x => x.descripcion).FirstOrDefault(),
                    direccion = item.direccion,
                    entregaPersonal = item.entregaPersonal,
                    enviado = item.enviado,
                    fecha = item.fecha,
                    fechaLimite = item.fechaLimite,
                    idRegistro = item.idRegistro,
                    origen = lsMessageActor.Where(x => x.id.Equals(item.origen)).Select(x => x.descripcion).FirstOrDefault(),
                    prioridad = ObtienePrioridad(item.prioridad),
                    tipoRegistro = item.tipoRegistro
                };
                
                lsMessageBitacore.Add(mbr);
            }

            return View(lsMessageBitacore);
        }

        [Authorize]
        public ActionResult Upload()
        {
            #region User identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            string rol = "";
            string idSocio = "";
            string titular = "";
            string idAccion = "";

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                rol = userRole[1];

                if (rol == "S")
                {
                    idSocio = userRole[2];
                    titular = userRole[3];
                    idAccion = userRole[4];
                }
            }
            #endregion

            if (rol != "A")
                return RedirectToAction("index", "Home");

            ViewBag.menu = "medios";
            return View(new vmFileUpload() { nombre = "x" });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Upload(vmFileUpload model)
        {
            if (ModelState.IsValid)
            {
                string physicalPath = HttpContext.Server.MapPath("~") + "\\UploadedFiles\\PartnerConsumption\\";
                string fileExtension = Path.GetExtension(model.File.FileName);
                string fileName = Guid.NewGuid().ToString() + fileExtension;

                model.File.SaveAs(physicalPath + fileName);

                string[] oStreamDataValues = null;
                List<PartnerConsumption> lsPartnerConsumption = new List<PartnerConsumption>();

                using (StreamReader reader = new StreamReader(physicalPath + fileName))
                {

                    int lines = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lines == 0)
                            lines++;
                        else
                        {
                            oStreamDataValues = line.Split('\t');

                            MessageActor messageActor = new MessageActor()
                            {
                                descripcion = oStreamDataValues[0].Trim(),
                                nit = oStreamDataValues[1].Trim(),
                                tipo = "P"
                            };

                            bizMessageActor.SaveMessageActor(messageActor);
                        }
                    }
                }
            }

            ViewBag.menu = "medios";

            return RedirectToAction("IndexProveedores");
        }

        //
        // GET: /MessageActor/Create

        public ActionResult Create()
        {
            MessageActor messageActor = new MessageActor() { tipo = "D" };
            return View(messageActor);
        }

        //
        // POST: /MessageActor/Create

        [HttpPost]
        public ActionResult Create(MessageActor messageActor)
        {
            try
            {
                bizMessageActor.SaveMessageActor(messageActor);

                return RedirectToAction("IndexDependencias");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MessageActor/Edit/5

        public ActionResult Edit(int id)
        {
            MessageActor messageActor = bizMessageActor.GetMessageActorbyKey(new MessageActor() { id = id });
            return View(messageActor);
        }

        //
        // POST: /MessageActor/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, MessageActor messageActor)
        {
            try
            {
                // TODO: Add update logic here
                messageActor.id = id;
                bizMessageActor.SaveMessageActor(messageActor);

                return RedirectToAction("IndexDependencias");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MessageActor/Delete/5

        public ActionResult Delete(int id)
        {
            MessageActor ma = bizMessageActor.GetMessageActorbyKey(new MessageActor() { id = id });

            bizMessageActor.DeleteMessageActor(new MessageActor() { id = id });

            if (ma.tipo == "D")
                return RedirectToAction("IndexDependencias");
            else
                return RedirectToAction("IndexProveedores");
        }

        public ActionResult RecibeDocumento()
        {
            vmMessageBitacore messageBitacore = new vmMessageBitacore() { tipoRegistro = "I", fecha = DateTime.Now };
            return View(messageBitacore);
        }

        [HttpPost]
        public ActionResult RecibeDocumento(MessageBitacore messageBitacore)
        {
            bizMessageBitacore.SaveMessageBitacore(messageBitacore);
            return RedirectToAction("RecibeDocumento");
        }

        public ActionResult EnviaDocumento()
        {
            vmMessageBitacore messageBitacore = new vmMessageBitacore() { tipoRegistro = "O", fecha = DateTime.Now };
            return View(messageBitacore);
        }

        public JsonResult EnviarDocumento(MessageBitacore messageBitacore)
        {
            string res = "";

            try
            {
                messageBitacore.fecha = DateTime.Now;
                res = bizMessageBitacore.SaveMessageBitacore(messageBitacore).ToString();

            }
            catch (Exception ex)
            {
                res = string.Format("Error - no se pudo guardar debido a un error inesperado {0}", ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Enviado(string id)
        {
            string res = "";

            try
            {
                string[] parametro = id.Split('|');
                long idDoc = long.Parse(parametro[0]);
                string tipo = parametro[1] == "In" ? "I" : "O";

                MessageBitacore mb = bizMessageBitacore.GetMessageBitacorebyKey(new MessageBitacore() { idRegistro = idDoc, tipoRegistro = tipo });
                mb.enviado = true;
                if (bizMessageBitacore.SaveMessageBitacore(mb) != 0)
                    res = "OK";

            }
            catch (Exception ex)
            {
                res = string.Format("Error - no se pudo guardar debido a un error inesperado {0}", ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private string ObtienePrioridad(byte idprioridad)
        {
            string strRet = "";
            switch (idprioridad)
            {
                case 1:
                    strRet = "Normal";
                    break;
                case 2:
                    strRet = "Alta";
                    break;
                default:
                    strRet = "Normal";
                    break;
            }

            return strRet;
        }
    }
}
