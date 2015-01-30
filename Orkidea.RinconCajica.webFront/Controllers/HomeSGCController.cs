using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using System.Configuration;

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

        [Authorize]
        public ActionResult Suggestion(int? id)
        {
            return View();
        }

        [Authorize]
        public JsonResult SendSuggestion(string id)
        {
            string res = "";
            string[] message = id.Split('|');

            #region Partner identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;

            int user = 0;
            string rol = "U";
            string idSocio = "";
            bool titular = false;

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                user = int.Parse(userRole[0]);
                rol = userRole[1];

                if (rol == "S")
                {
                    idSocio = userRole[2];
                    titular = userRole[3] == "T" ? true : false;
                }
            }


            BizFrontUser bizFrontUser = new BizFrontUser();

            FrontUser frontUser = bizFrontUser.GetFrontUserbyKey(new FrontUser() { id = user });
            #endregion

            BizCommon bizCommon = new BizCommon();

            try
            {
                string rootPath = Server.MapPath("~");                
                //bizCommon.sendContactMessage(frontUser.email,idSocio,  idSocio + " le envió una sugerencia al Club - " + message[0], message[1]);

                bizCommon.sendContactMessage(frontUser.email, idSocio, string.Format("El usuario {0} se ha contactado con el Club a traves de la intranet", message[0]), message[1], rootPath);

                res = "OK";
            }
            catch (Exception)
            {
                res = "fail";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private List<NewsPaper> GetPaginatedNews(int page = 1)
        {
            var skipRecords = page * recordsPerPage;

            List<NewsPaper> listOfProducts = bizNewsPaper.GetNewsList().Skip(skipRecords).Take(recordsPerPage).ToList();

            return listOfProducts;
        }


    }
}
