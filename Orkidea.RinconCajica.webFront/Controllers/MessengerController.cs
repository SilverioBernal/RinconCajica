using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class MessengerController : Controller
    {
        BizMessenger bizMessenger = new BizMessenger();
        //
        // GET: /Messenger/

        public ActionResult Index()
        {
            return View(bizMessenger.GetMessengerList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Messenger mensajero)
        {
            try
            {
                // TODO: Add insert logic here
                bizMessenger.SaveMessenger(mensajero);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Fail");
            }
        }

        public ActionResult Edit(int id)
        {
            return View(bizMessenger.GetMessengerbyKey(new Messenger() { id = id}));
        }

        [HttpPost]
        public ActionResult Edit(int id, Messenger mensajero)
        {
            try
            {
                // TODO: Add insert logic here
                mensajero.id = id;
                bizMessenger.SaveMessenger(mensajero);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Fail");
            }
        }

        public ActionResult Delete(int id)
        {
            bizMessenger.DeleteMessenger(new Messenger() { id = id });
            return RedirectToAction("Index");
        }

        public ActionResult GetAsyncMessenger()
        {
            return Json(bizMessenger.GetMessengerList().Where(x => x.activo).ToList(), JsonRequestBehavior.AllowGet);            
        }

        public ActionResult Fail()
        {
            return View();
        }
    }
}
