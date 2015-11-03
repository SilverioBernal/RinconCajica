using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.webFront.Models;
using Orkidea.RinconCajica.Utilities;
using System.Threading;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class PartnerConsumptionController : Controller
    {
        BizPartnerConsumption bizPartnerConsumption = new BizPartnerConsumption();

        //
        // GET: /PartnerConsumption/

        public ActionResult Index()
        {
            List<ConsumptionGlobal> lsConsumos = bizPartnerConsumption.GetPartnerConsumptionList2M();
            return View(lsConsumos);
            //return View();
        }

        //
        // GET: /PartnerConsumption/Details/5

        public ActionResult InvoiceList()
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

            if (rol != "S")
                return RedirectToAction("index", "Home");

            List<ConsumptionResume> lsConsumos = bizPartnerConsumption.GetPartnerConsumptionbyPartner(new ClubPartner() { accion = idAccion });

            ViewBag.accion = idAccion;

            return View(lsConsumos);
        }

        public JsonResult InvoiceDetails(string id)
        {
            string[] idFactura = id.Split('|');
            string sufijo = idFactura[0];
            string factura = idFactura[1];

            List<PartnerConsumption> detalleFactura = bizPartnerConsumption.GetPartnerConsumptionbyInvoice(new PartnerConsumption() { Sufijo = sufijo, Nufactura = factura });

            return Json(detalleFactura, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string id)
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

            string[] idFactura = id.Split('|');
            string sufijo = idFactura[0];
            string factura = idFactura[1];

            List<PartnerConsumption> detalleFactura = bizPartnerConsumption.GetPartnerConsumptionbyInvoice(new PartnerConsumption() { Sufijo= sufijo, Nufactura= factura });

            ViewBag.accion = idAccion;

            return View(detalleFactura);
        }

        //
        // GET: /PartnerConsumption/Create
        [Authorize]
        public ActionResult Create()
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
            return View(new vmFileUpload() {  nombre = "x"});
        }

        //
        // POST: /PartnerConsumption/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(vmFileUpload model)
        {
            if (ModelState.IsValid)
            {
                //string physicalPath = HttpContext.Server.MapPath("~") + "\\UploadedFiles\\PartnerConsumption\\";
                //string fileExtension = Path.GetExtension(model.File.FileName);
                //string fileName = Guid.NewGuid().ToString() + fileExtension;

                //model.File.SaveAs(physicalPath + fileName);

                //AzureStorageHelper.uploadFile(model.File.InputStream, fileName, "partnerComsumption");

                
                //List<PartnerConsumption> lsPartnerConsumption = new List<PartnerConsumption>();



                using (StreamReader reader = new StreamReader(model.File.InputStream))
                {

                    List<PartnerConsumption> consumos = new List<PartnerConsumption>();

                    int lines = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lines == 0)
                            lines++;
                        else
                        {
                            
                                string[] oStreamDataValues = line.Split('\t');

                                PartnerConsumption partnerConsumption = new PartnerConsumption()
                                {
                                    Punto_venta_a = oStreamDataValues[0].Trim(),
                                    Producto = oStreamDataValues[1].Trim(),
                                    Punto_venta_b = oStreamDataValues[2].Trim(),
                                    Codigo_producto = oStreamDataValues[3].Trim(),
                                    Cantidad = oStreamDataValues[4].Trim(),
                                    Valor_producto = decimal.Parse(oStreamDataValues[5]),
                                    Fecha = ArmaFecha(oStreamDataValues[6]),
                                    Docid_consumidor = string.IsNullOrEmpty(oStreamDataValues[7]) ? "" : oStreamDataValues[7].Trim(),
                                    Nufactura = oStreamDataValues[8].Trim(),
                                    Sufijo = oStreamDataValues[9].Trim(),
                                    Propina = decimal.Parse(oStreamDataValues[10]),
                                    Total_fac = decimal.Parse(oStreamDataValues[11]),
                                    Consumidor = string.IsNullOrEmpty(oStreamDataValues[12]) ? "" : oStreamDataValues[12].Trim(),
                                    Docid_pagador = oStreamDataValues[13].Trim(),
                                    Pagador = oStreamDataValues[14].Trim()
                                };

                                consumos.Add(partnerConsumption);

                                bizPartnerConsumption.SaveAsyncPartnerConsumption(consumos);//.SavePartnerConsumption(partnerConsumption);
                            
                        }
                    }
                }
            }

            ViewBag.menu = "medios";

            return RedirectToAction("Index");
        }

        public ActionResult UploadConsupmtionFile()
        {
            return View();
        }

        public JsonResult AsyncGuardaConsumo(PartnerConsumption consumo)
        {           
            string res = "Fail";
            try
            {
                string[] tmpSufijo = consumo.Sufijo.Split('|');
                
                consumo.Sufijo = tmpSufijo[0];
                consumo.Fecha = ArmaFecha(tmpSufijo[1]);
                bizPartnerConsumption.SavePartnerConsumption(consumo);
                res = "OK";
            }
            catch (Exception ex)
            {
                res = string.Format("{0} Factura: {1}, Fecha: {2}, producto: {3}",ex.Message, consumo.Nufactura, consumo.Fecha.ToString("yyyy-MM-dd"), consumo.Codigo_producto);
            }

            return Json(res, JsonRequestBehavior.AllowGet); 
        }


        private DateTime ArmaFecha (string fecha)
        {            
            string[] oFecha = fecha.Split('/');

            return new DateTime(int.Parse(oFecha[2]), int.Parse(oFecha[1]), int.Parse(oFecha[0]));
        }

        //
        // GET: /PartnerConsumption/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /PartnerConsumption/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PartnerConsumption/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PartnerConsumption/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
