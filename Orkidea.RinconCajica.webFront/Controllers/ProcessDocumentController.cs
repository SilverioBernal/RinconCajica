using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.webFront.Models;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class ProcessDocumentController : Controller
    {
        BizProcessDocument processDocumentBiz = new BizProcessDocument();

        //
        // GET: /ProcessDocument/
        [Authorize]
        public ActionResult Index()
        {
            BizProcess processBiz = new BizProcess();
            BizDocumentType docTypeBiz = new BizDocumentType();

            List<Process> processList = processBiz.GetProcessList();
            List<DocumentType> documentTypeList = docTypeBiz.GetDocumentTypeList();


            List<ProcessDocument> lstProcessDocument = processDocumentBiz.GetProcessDocumentList().OrderBy(x => x.idProceso).ToList();

            List<vmProcessDocument> lstProcessDocumentRes = new List<vmProcessDocument>();

            foreach (ProcessDocument item in lstProcessDocument)
            {
                vmProcessDocument processDoc = new vmProcessDocument()
                {
                    id = item.id,
                    descripcion = item.descripcion,
                    idProceso = item.idProceso,
                    idTipoDocumento = item.idTipoDocumento,
                    nombre = item.nombre,
                    ruta = item.ruta
                };

                processDoc.desProceso = processList.Where(x => x.id.Equals(item.idProceso)).FirstOrDefault().nombre;//processBiz.GetProcessbyKey(new Process() { id = item.idProceso }).nombre;
                processDoc.desTipo = documentTypeList.Where(x => x.id.Equals(item.idTipoDocumento)).FirstOrDefault().nombre;//docTypeBiz.GetDocumentTypeByKey(new DocumentType() { id = item.idTipoDocumento }).nombre;

                lstProcessDocumentRes.Add(processDoc);
            }
            return View(lstProcessDocumentRes);
        }

        [Authorize]
        public ActionResult IndexByProcess(int idProceso, int idTipoDocumento)
        {
            ProcessDocument processDocument = new ProcessDocument() { idProceso = idProceso, idTipoDocumento = idTipoDocumento };

            List<ProcessDocument> lstProcessDocument =
                processDocumentBiz.GetProcessDocumentListByProcessDocumentType(processDocument)
                .OrderBy(x => x.nombre).ToList();

            return View(lstProcessDocument);
        }

        //
        // GET: /ProcessDocument/Create
        [Authorize]
        public ActionResult Create()
        {
            BizProcess processBiz = new BizProcess();
            BizDocumentType documentTypeBiz = new BizDocumentType();

            vmProcessDocument processDocument = new vmProcessDocument();
            processDocument.lstProcess = processBiz.GetProcessList().OrderBy(x => x.nombre).ToList();
            processDocument.lstDocType = documentTypeBiz.GetDocumentTypeList().OrderBy(x => x.nombre).ToList();

            return View(processDocument);
        }

        //
        // POST: /ProcessDocument/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(vmProcessDocument oProcessDocument, IEnumerable<HttpPostedFileBase> files)
        {

            try
            {
                string physicalPath = HttpContext.Server.MapPath("~") + "\\UploadedFiles\\";
                string fileExtension = Path.GetExtension(oProcessDocument.File.FileName);
                string fileName = Guid.NewGuid().ToString() + fileExtension;

                oProcessDocument.File.SaveAs(physicalPath + fileName);

                processDocumentBiz.SaveProcessDocument(new ProcessDocument()
                {
                    descripcion = "",
                    idTipoDocumento = oProcessDocument.idTipoDocumento,
                    idProceso = oProcessDocument.idProceso,
                    nombre = oProcessDocument.nombre,
                    ruta = fileName
                });


                ViewBag.menu = "medios";

                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message + ex.StackTrace + ex.InnerException.Message;
                BizProcess processBiz = new BizProcess();
                BizDocumentType documentTypeBiz = new BizDocumentType();

                vmProcessDocument processDocument = new vmProcessDocument()
                {
                    descripcion = oProcessDocument.descripcion,
                    desProceso = oProcessDocument.descripcion,
                    idProceso = oProcessDocument.idProceso,
                    idTipoDocumento = oProcessDocument.idTipoDocumento,
                    nombre = oProcessDocument.nombre,
                    ruta = oProcessDocument.ruta
                };

                processDocument.lstProcess = processBiz.GetProcessList().OrderBy(x => x.nombre).ToList();
                processDocument.lstDocType = documentTypeBiz.GetDocumentTypeList().OrderBy(x => x.nombre).ToList();

                return View(processDocument);
            }
        }

        //
        // GET: /ProcessDocument/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            BizProcess processBiz = new BizProcess();
            BizDocumentType documentTypeBiz = new BizDocumentType();
            ProcessDocument oProcessDocument = processDocumentBiz.GetProcessDocumentbyKey(new ProcessDocument() { id = id });
            Process process = processBiz.GetProcessbyKey(new Process() { id = oProcessDocument.idProceso });
            DocumentType documentType = documentTypeBiz.GetDocumentTypeByKey(new DocumentType() { id = oProcessDocument.idTipoDocumento });

            vmProcessDocument processDocument = new vmProcessDocument()
            {
                id = id,
                descripcion = oProcessDocument.descripcion,
                desProceso = process.nombre,
                idProceso = oProcessDocument.idProceso,
                idTipoDocumento = oProcessDocument.idTipoDocumento,
                desTipo = documentType.nombre,
                nombre = oProcessDocument.nombre,
                ruta = oProcessDocument.ruta
            };

            processDocument.lstProcess = processBiz.GetProcessList().OrderBy(x => x.nombre).ToList();
            processDocument.lstDocType = documentTypeBiz.GetDocumentTypeList().OrderBy(x => x.nombre).ToList();

            return View(processDocument);
        }

        //
        // POST: /ProcessDocument/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, ProcessDocument oProcessDocument, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                if (string.IsNullOrEmpty(oProcessDocument.descripcion))
                    oProcessDocument.descripcion = "";

                if (files != null)
                {
                    BizFileType fileTypeBiz = new BizFileType();
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file.FileName != null)
                        {
                            string physicalPath = HttpContext.Server.MapPath("~") + "UploadedFiles" + "\\";
                            string fileName = Guid.NewGuid().ToString() + fileTypeBiz.GetFileTypebyKey(new FileType() { tipoMIME = file.ContentType }).extension;

                            using (Stream output = System.IO.File.OpenWrite(physicalPath + fileName))
                            using (Stream input = file.InputStream)
                            {
                                input.CopyTo(output);

                                oProcessDocument.ruta = fileName;
                            }
                        }
                    }
                    oProcessDocument.id = id;
                    processDocumentBiz.SaveProcessDocument(oProcessDocument);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                BizProcess processBiz = new BizProcess();
                BizDocumentType documentTypeBiz = new BizDocumentType();

                vmProcessDocument processDocument = new vmProcessDocument()
                {
                    id = id,
                    descripcion = oProcessDocument.descripcion,
                    desProceso = oProcessDocument.descripcion,
                    idProceso = oProcessDocument.idProceso,
                    idTipoDocumento = oProcessDocument.idTipoDocumento,
                    nombre = oProcessDocument.nombre,
                    ruta = oProcessDocument.ruta
                };

                processDocument.lstProcess = processBiz.GetProcessList().OrderBy(x => x.nombre).ToList();
                processDocument.lstDocType = documentTypeBiz.GetDocumentTypeList().OrderBy(x => x.nombre).ToList();

                return View(processDocument);
            }
        }
       
        //
        // POST: /ProcessDocument/Delete/5
        [Authorize]        
        public ActionResult Delete(int id, ProcessDocument oProcessDocument)
        {
            try
            {
                oProcessDocument.id = id;
                processDocumentBiz.DeleteProcessDocument(oProcessDocument);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
