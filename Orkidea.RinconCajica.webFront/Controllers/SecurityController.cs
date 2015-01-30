using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.Utilities;
using Orkidea.RinconCajica.webFront.Models;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class SecurityController : Controller
    {
        BizFrontUser bizFrontUser = new BizFrontUser();

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            Cryptography oCrypto = new Cryptography();
            if (!String.IsNullOrEmpty(model.UserName) && !String.IsNullOrEmpty(model.Password))
            {

                FrontUser frontUserTarget = bizFrontUser.GetFrontUserbyName(new FrontUser { usuario = model.UserName });


                if (frontUserTarget == null)
                    return View(model);

                string contraseñaDesencriptada = oCrypto.Decrypt(frontUserTarget.contrasena);


                if (model.Password.Equals(contraseñaDesencriptada))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    int id = frontUserTarget.id;
                    string idRole = frontUserTarget.idRol;
                    string idSocio = "";
                    string idAccion = "";
                    if (idRole == "S")
                    {
                        BizClubPartner bizClubPartner = new BizClubPartner();
                        Partner partner = bizClubPartner.GetClubPartnerbyUser(id);

                        idSocio = partner.docid.ToString();
                        idAccion = partner.accion;
                    }


                    string userData = "";

                    if (idRole != "S")
                        //userData = id.ToString().Trim() + "|" + idRole.ToString().Trim() + "|";
                        userData = string.Format("{0}|{1}", id.ToString(), idRole.ToString().Trim());
                    else
                        userData = string.Format("{0}|{1}|{2}|{3}", id.ToString(), idRole.ToString().Trim(), idSocio, idAccion);
                        //userData = id.ToString().Trim() + "|" + idRole.ToString().Trim() + "|" + idSocio;

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), false, userData);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    HttpContext.Response.Cookies.Add(faCookie);

                    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                    if (authCookie != null)
                    {
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        CustomIdentity identity = new CustomIdentity(authTicket.Name, userData);
                        GenericPrincipal newUser = new GenericPrincipal(identity, new string[] { });
                        HttpContext.User = newUser;
                    }

                    return RedirectToLocal(returnUrl);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public JsonResult barLogin(LoginModel model)
        {
            string res = "Fail";

            Cryptography oCrypto = new Cryptography();
            if (!String.IsNullOrEmpty(model.UserName) && !String.IsNullOrEmpty(model.Password))
            {

                FrontUser frontUserTarget = bizFrontUser.GetFrontUserbyName(new FrontUser { usuario = model.UserName });


                if (frontUserTarget == null)
                    res = "Fail";

                string contraseñaDesencriptada = oCrypto.Decrypt(frontUserTarget.contrasena);


                if (model.Password.Equals(contraseñaDesencriptada))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    int id = frontUserTarget.id;
                    string idRole = frontUserTarget.idRol;

                    //string userData = id.ToString().Trim() + "|" + idRole.ToString().Trim();

                    string idSocio = "";
                    string titular = "";
                    string idAccion = "";
                    if (idRole == "S")
                    {
                        BizClubPartner bizClubPartner = new BizClubPartner();
                        Partner partner = bizClubPartner.GetClubPartnerbyUser(id);

                        idSocio = partner.docid.ToString();
                        titular = partner.rel_tit;
                        idAccion = partner.accion;
                    }


                    string userData = "";

                    if (idRole != "S")
                        userData = string.Format("{0}|{1}", id.ToString(), idRole.ToString().Trim());
                    else                    
                        userData = string.Format("{0}|{1}|{2}|{3}|{4}", id.ToString().Trim(), idRole.ToString().Trim(), idSocio, titular, idAccion);

                    //if (titular == "T")
                    //    userData = id.ToString().Trim() + "|" + idRole.ToString().Trim() + "|" + idSocio + "|" + titular;
                    //else
                    //    userData = id.ToString().Trim() + "|" + idRole.ToString().Trim() + "|" + idSocio + "|";

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), false, userData);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    HttpContext.Response.Cookies.Add(faCookie);

                    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                    if (authCookie != null)
                    {
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        CustomIdentity identity = new CustomIdentity(authTicket.Name, userData);
                        GenericPrincipal newUser = new GenericPrincipal(identity, new string[] { });
                        HttpContext.User = newUser;

                        res = "OK";
                    }
                }
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction
                ("Login");
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //[Authorize]
        //public JsonResult barLogout()
        //{
        //    string res = "fail";
        //    Session.Abandon();
        //    FormsAuthentication.SignOut();

        //    res = "OK";
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        [Authorize]
        public ActionResult SaveUser(FrontUser frontUser)
        {
            try
            {
                Cryptography oCrypto = new Cryptography();

                if (string.IsNullOrEmpty(frontUser.contrasena))
                    frontUser.contrasena = oCrypto.Encrypt(frontUser.usuario.Trim() + DateTime.Now.Year.ToString());
                
                string rootPath = Server.MapPath("~");    
                bizFrontUser.SaveFrontUser(frontUser, rootPath);

                //if (frontUser.idRol == 3 || frontUser.idRol == 4)
                //{
                //    return RedirectToAction("TeacherUserIndex", new { id = frontUser.idColegio });
                //}

                //if (frontUser.idRol == 5)
                //{
                //    return RedirectToAction("StudentUserIndex", new { id = frontUser.idColegio });
                //}

            }
            catch
            {
                return View();
            }

            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            #region School identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;
            int usuario = 0;

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                usuario = int.Parse(userRole[0]);
            }

            #endregion

            FrontUser frontUser = bizFrontUser.GetFrontUserbyName(new FrontUser() { usuario = HttpContext.User.Identity.Name });

            return View(frontUser);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(FrontUser frontUserTarget)
        {
            Cryptography oCrypto = new Cryptography();

            #region School identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;
            int usuario = 0;

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                usuario = int.Parse(userRole[0]);
            }
            #endregion

            FrontUser frontUser = bizFrontUser.GetFrontUserbyName(new FrontUser() { usuario = HttpContext.User.Identity.Name });

            frontUser.contrasena = oCrypto.Encrypt(frontUserTarget.contrasena);

            SaveUser(frontUser);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public JsonResult BarChangePassword(FrontUser frontUserTarget)
        {
            string res = "Fail";

            Cryptography oCrypto = new Cryptography();

            #region School identification
            System.Security.Principal.IIdentity context = HttpContext.User.Identity;
            int usuario = 0;

            if (context.IsAuthenticated)
            {
                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');
                usuario = int.Parse(userRole[0]);
            }
            #endregion

            FrontUser frontUser = bizFrontUser.GetFrontUserbyName(new FrontUser() { usuario = HttpContext.User.Identity.Name });

            frontUser.contrasena = oCrypto.Encrypt(frontUserTarget.contrasena);

            SaveUser(frontUser);

            res = "OK";
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //[AllowAnonymous]
        //public JsonResult ResetPassword(string usuario)
        //{
        //    string resultado = "No se pudo resetear el usuario.";
        //    try
        //    {
        //        Cryptography oCrypto = new Cryptography();

        //        Person person = personBiz.GetPersonByUserName(new Person() { usuario = usuario });
        //        person.password = oCrypto.Encrypt(person.documento);

        //        SaveUser(person);

        //        resultado = "Clave reseteada con éxito";
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return Json(resultado, JsonRequestBehavior.AllowGet);
        //}

    }
}
