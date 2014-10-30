using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class MessageActorController : Controller
    {
        BizMessageActor bizMessageActor = new BizMessageActor();

        //
        // GET: /MessageActor/

        public ActionResult IndexDependencias()
        {
            List<MessageActor> lsMessageActor = bizMessageActor.GetMessageActorListByType("D");
            return View(lsMessageActor);
        }

        //
        // GET: /MessageActor/Details/5

        public ActionResult Details(int id)
        {
            return View();
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
            bizMessageActor.DeleteMessageActor(new MessageActor() { id = id });
            return RedirectToAction("IndexDependencias");
        }

        //
        // POST: /MessageActor/Delete/5        
    }
}
