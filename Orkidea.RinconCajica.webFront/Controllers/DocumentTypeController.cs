using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class DocumentTypeController : Controller
    {
        BizDocumentType bizDocumentType = new BizDocumentType();

        //
        // GET: /DocumentType/
        [Authorize]
        public ActionResult Index()
        {
            List<DocumentType> lstDocType = bizDocumentType.GetDocumentTypeList();
            return View(lstDocType);
        }

        //
        // GET: /DocumentType/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DocumentType/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(DocumentType documentType)
        {
            try
            {
                bizDocumentType.SaveDocumentType(documentType);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DocumentType/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            DocumentType documentType = bizDocumentType.GetDocumentTypeByKey(new DocumentType() { id = id });
            return View(documentType);
        }

        //
        // POST: /DocumentType/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, DocumentType documentType)
        {
            try
            {
                documentType.id = id;
                bizDocumentType.SaveDocumentType(documentType);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /DocumentType/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                bizDocumentType.DeleteDocumentType(new DocumentType() { id = id });

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }     
    }
}
