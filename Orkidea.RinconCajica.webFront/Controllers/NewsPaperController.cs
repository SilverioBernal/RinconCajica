using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.webFront.Models;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class NewsPaperController : Controller
    {
        BizNewsPaper newsPaperBiz = new BizNewsPaper();

        //
        // GET: /NewsPaper/
        [Authorize]
        public ActionResult Index()
        {
            List<NewsPaper> lstNewsPaper = newsPaperBiz.GetNewsList();
            return View(lstNewsPaper);
        }

        //
        // GET: /NewsPaper/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /NewsPaper/Create

        public ActionResult Create()
        {
            vmNews news = new vmNews() { fecha = DateTime.Now };
            return View(news);
        }

        //
        // POST: /NewsPaper/Create
        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(NewsPaper newsContent)
        {
            try
            {
                newsPaperBiz.SaveNews(newsContent);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /NewsPaper/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            NewsPaper newsContent = newsPaperBiz.GetNewsByKey(new NewsPaper() { id = id });
            vmNews news = new vmNews() { contenido = newsContent.contenido, fecha = newsContent.fecha, titulo = newsContent.titulo };

            return View(news);
        }

        //
        // POST: /NewsPaper/Edit/5
        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(int id, NewsPaper newsContent)
        {
            try
            {
                newsContent.id = id;
                newsPaperBiz.SaveNews(newsContent);

                return RedirectToAction("Index");
            }
            catch
            {
                vmNews news = new vmNews() { contenido = newsContent.contenido, fecha = newsContent.fecha, titulo = newsContent.titulo };

                return View(news);
            }
        }

        //
        // GET: /NewsPaper/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            NewsPaper newsContent = newsPaperBiz.GetNewsByKey(new NewsPaper() { id = id });
            newsPaperBiz.DeleteNews(newsContent);            
            return RedirectToAction("index");
        }
       
    }
}
