using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class SupplierController : Controller
    {
        BizSupplier bizSupplier = new BizSupplier();

        //
        // GET: /Supplier/

        public ActionResult Index()
        {
            return View(bizSupplier.GetSupplierList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Supplier proveedor)
        {
            try
            {
                // TODO: Add insert logic here
                bizSupplier.SaveSupplier(proveedor);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Supplier supplier = bizSupplier.GetSupplierbyKey(new Supplier() { id = id });
            return View(supplier);
        }

        [HttpPost]
        public ActionResult Edit(int id, Supplier proveedor)
        {
            try
            {
                // TODO: Add insert logic here
                proveedor.id = id;
                bizSupplier.SaveSupplier(proveedor);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            bizSupplier.DeleteSupplier(new Supplier() { id = id });
            return RedirectToAction("Index");
        }
        
        public JsonResult GetAsyncSuppliers()
        {
            
            List<Supplier> suppliers = bizSupplier.GetSupplierList();

            return Json(suppliers, JsonRequestBehavior.AllowGet);
        }
    }
}
