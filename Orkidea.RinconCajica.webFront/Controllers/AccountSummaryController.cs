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

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class AccountSummaryController : Controller
    {
        BizAccountSummary bizAccountSummary = new BizAccountSummary();

        //
        // GET: /AccountSummary/
        [Authorize]
        public ActionResult Index()
        {
            List<AccountSummary> lsAccountSummary = bizAccountSummary.GetAccountSummaryList();
            ViewBag.menu = "AccountSummary";
            return View(lsAccountSummary);
        }

        //
        // GET: /AccountSummary/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            string accion = Serialization.FromHexString(id);
            List<AccountSummary> lsAccountSummary = bizAccountSummary.GetAccountSummaryList(accion);
            return View(lsAccountSummary);
        }
        
        [Authorize]
        public ActionResult AccountSummaryDetail(string archivo)
        {
            string mimeType = "";
            int cuentaPuntos = 0;

            //FileUpload fu = bizFileUpload.GetFileUploadByKey(new FileUpload() { id = id });

            string[] nombreArchivo = archivo.Split('.');
            cuentaPuntos = nombreArchivo.Length;

            mimeType = BizMimeType.GetMimeType(nombreArchivo[cuentaPuntos - 1]).mimetype1;

            //dynamically generate a file
            System.IO.MemoryStream ms;
            ms = AzureStorageHelper.getFile(archivo, "accountSummaries");

            // return the file
            return File(ms.ToArray(), mimeType);
        }
        
        [Authorize]
        public ActionResult upload()
        {
            vmAccountSummary accountSummaryFiles = new vmAccountSummary() {  ano= DateTime.Now.Year, mes = DateTime.Now.Month};
            ViewBag.menu = "AccountSummary";
            return View(accountSummaryFiles);
        }

        [Authorize]
        [HttpPost]
        public ActionResult upload(vmAccountSummary model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //string physicalPath = HttpContext.Server.MapPath("~") + "\\UploadedFiles\\AccountSummaries\\";

            AccountSummary fileUploadModel = new AccountSummary();
            foreach (var item in model.File)
            {
                //byte[] uploadFile = new byte[item.InputStream.Length];
                //item.InputStream.Read(uploadFile, 0, uploadFile.Length);

                string fileExtension = Path.GetExtension(item.FileName);
                string fileName = Guid.NewGuid().ToString() + fileExtension;

                fileUploadModel.archivo = fileName;                

                //item.SaveAs(physicalPath + fileName);
                AzureStorageHelper.uploadFile(item.InputStream, fileName, "accountSummaries");

                AccountSummary AS = new AccountSummary()
                {
                    accion = item.FileName.Substring(4, (item.FileName.Length - (4 + item.FileName.Length - item.FileName.IndexOf('.')))),
                    archivo = fileName,
                    ano = model.ano,
                    mes = model.mes,
                };

                bizAccountSummary.SaveAccountSummary(AS);                
            }
            return RedirectToAction("Index");

        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            AccountSummary As = bizAccountSummary.GetAccountSummarybyKey(new AccountSummary() { id = id });
            bizAccountSummary.DeleteAccountSummary(As);

            return RedirectToAction("Index");
        }
    }
}
