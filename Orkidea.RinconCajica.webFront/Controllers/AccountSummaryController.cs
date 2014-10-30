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
    public class AccountSummaryController : Controller
    {
        BizAccountSummary bizAccountSummary = new BizAccountSummary();

        //
        // GET: /AccountSummary/

        public ActionResult Index()
        {
            List<AccountSummary> lsAccountSummary = bizAccountSummary.GetAccountSummaryList();
            ViewBag.menu = "AccountSummary";
            return View(lsAccountSummary);
        }

        //
        // GET: /AccountSummary/Details/5

        public ActionResult Details(string id)
        {
            List<AccountSummary> lsAccountSummary = bizAccountSummary.GetAccountSummaryList(id);
            return View(lsAccountSummary);
        }

        public ActionResult upload()
        {
            vmAccountSummary accountSummaryFiles = new vmAccountSummary() {  ano= DateTime.Now.Year, mes = DateTime.Now.Month};
            ViewBag.menu = "AccountSummary";
            return View(accountSummaryFiles);
        }

        [HttpPost]
        public ActionResult upload(vmAccountSummary model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string physicalPath = HttpContext.Server.MapPath("~") + "\\UploadedFiles\\AccountSummaries\\";

            AccountSummary fileUploadModel = new AccountSummary();
            foreach (var item in model.File)
            {
                //byte[] uploadFile = new byte[item.InputStream.Length];
                //item.InputStream.Read(uploadFile, 0, uploadFile.Length);

                string fileExtension = Path.GetExtension(item.FileName);
                string fileName = Guid.NewGuid().ToString() + fileExtension;

                fileUploadModel.archivo = fileName;                

                item.SaveAs(physicalPath + fileName);

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


        public ActionResult Delete(int id)
        {
            AccountSummary As = bizAccountSummary.GetAccountSummarybyKey(new AccountSummary() { id = id });
            bizAccountSummary.DeleteAccountSummary(As);

            return RedirectToAction("Index");
        }
    }
}
