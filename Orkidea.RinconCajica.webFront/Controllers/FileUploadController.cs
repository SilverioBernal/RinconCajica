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
    public class FileUploadController : Controller
    {
        BizFileUpload bizFileUpload = new BizFileUpload();
        //
        // GET: /FileUpload/

        public ActionResult Index()
        {
            List<FileUpload> lsHomeSlider = new List<FileUpload>();

            try
            {
                lsHomeSlider.AddRange(bizFileUpload.GetFileUploadList());
            }
            catch (Exception)
            {

            }
            return View(lsHomeSlider);
        }

        //
        // GET: /FileUpload/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //
        // GET: /FileUpload/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FileUpload/Create

        [HttpPost]
        public ActionResult Create(vmFileUpload model)
        {
            if (ModelState.IsValid)
            {
                string physicalPath = HttpContext.Server.MapPath("~") + "\\UploadedFiles\\";
                string fileExtension = Path.GetExtension(model.File.FileName);
                string fileName = Guid.NewGuid().ToString() + fileExtension;

                model.File.SaveAs(physicalPath + fileName);

                bizFileUpload.SaveFileUpload(new FileUpload() { id = Guid.NewGuid(), fileName = fileName });                
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /FileUpload/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /FileUpload/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /FileUpload/Delete/5

        public ActionResult Delete(Guid id)
        {            
            bizFileUpload.DeleteFileUpload(new FileUpload() { id = id});
            return RedirectToAction("Index");
        }

        //
        // POST: /FileUpload/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
