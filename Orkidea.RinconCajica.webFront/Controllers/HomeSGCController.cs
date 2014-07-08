using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class HomeSGCController : Controller
    {
        BizNewsPaper bizNewsPaper = new BizNewsPaper();

        const int recordsPerPage = 3;

        [Authorize]
        public ActionResult Index(int? id)
        {
            var page = id ?? 0;

            if (Request.IsAjaxRequest())
            {
                List<NewsPaper> listOfProducts = GetPaginatedNews(page);
                return PartialView("_NewsPaper", listOfProducts);
            }

            return View("Index", bizNewsPaper.GetNewsList().Take(recordsPerPage));
        }       

        private List<NewsPaper> GetPaginatedNews(int page = 1)
        {
            var skipRecords = page * recordsPerPage;

            List<NewsPaper> listOfProducts = bizNewsPaper.GetNewsList().Skip(skipRecords).Take(recordsPerPage).ToList();

            return listOfProducts;
        }
    }
}
