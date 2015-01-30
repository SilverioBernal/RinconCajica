using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orkidea.RinconCajica.DataAccessEF;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.Utilities;

namespace Orkidea.RinconCajica.Business
{
    public class BizFrontUser
    { /*CRUD FrontUsers*/

        /// <summary>
        /// Retrieve FrontUsers list without parameters
        /// </summary>
        /// <returns></returns>
        public List<FrontUser> GetFrontUserList()
        {

            List<FrontUser> lstFrontUser = new List<FrontUser>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstFrontUser = ctx.FrontUser.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstFrontUser;
        }

        /// <summary>
        /// Retrieve FrontUser information based in the primary key
        /// </summary>
        /// <param name="FrontUserTarget"></param>
        /// <returns></returns>
        public FrontUser GetFrontUserbyKey(FrontUser FrontUserTarget)
        {
            FrontUser oFrontUser = new FrontUser();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oFrontUser = ctx.FrontUser.Where(x => x.id.Equals(FrontUserTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oFrontUser;
        }

        public FrontUser GetFrontUserbyPartner(int idPartner)
        {
            FrontUser oFrontUser = new FrontUser();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    int? idUsuario = ctx.ClubPartner.Where(x => x.doccl.Equals(idPartner)).Select(x => x.idUsuario).FirstOrDefault();

                    if (idUsuario != null)
                        oFrontUser = ctx.FrontUser.Where(x => x.id.Equals(idUsuario)).FirstOrDefault();
                }   
            }
            catch (Exception ex) { throw ex; }

            return oFrontUser;
        }

        /// <summary>
        /// Create or update a FrontUser
        /// </summary>
        /// <param name="FrontUserTarget"></param>
        public int SaveFrontUser(FrontUser FrontUserTarget, string rootPath)
        {
            int res = -1;
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the FrontUser exists
                    FrontUser oFrontUser = GetFrontUserbyKey(FrontUserTarget);
                    Cryptography oCrypto = new Cryptography();

                    string accion;

                    if (oFrontUser != null)
                    {
                        // if exists then edit                         
                        ctx.FrontUser.Attach(oFrontUser);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oFrontUser, FrontUserTarget);
                        ctx.SaveChanges();
                        res = oFrontUser.id;
                        accion = "modificación";
                    }
                    else
                    {
                        // else assing password and create                        
                        FrontUserTarget.contrasena = oCrypto.Encrypt(PasswordHelper.Generate());
                        ctx.FrontUser.Add(FrontUserTarget);
                        ctx.SaveChanges();
                        res = FrontUserTarget.id;

                        accion = "creación";

                        // send notification
                        List<System.Net.Mail.MailAddress> to = new List<System.Net.Mail.MailAddress>();

                        if (ConfigurationManager.AppSettings["testMail"].ToString() == "N")
                            to.Add(new System.Net.Mail.MailAddress(FrontUserTarget.email));
                        else
                            to.Add(new System.Net.Mail.MailAddress("silverio.bernal@orkidea.co"));


                        Dictionary<string, string> dynamicValues = new Dictionary<string, string>();
                        dynamicValues.Add("[usuario]", FrontUserTarget.usuario);
                        dynamicValues.Add("[clave]", oCrypto.Decrypt(FrontUserTarget.contrasena));
                        dynamicValues.Add("[urlSitio]", ConfigurationManager.AppSettings["UrlApp"].ToString());

                        MailingHelper.SendMail(to, string.Format("Notificación de {0} de usuario", accion),
                            rootPath + ConfigurationManager.AppSettings["emailNewUserNotificationTemplateHTML"].ToString(),
                            rootPath + ConfigurationManager.AppSettings["emailNewUserNotificationTemplateText"].ToString(),
                            rootPath + ConfigurationManager.AppSettings["emailLogoPath"].ToString(), dynamicValues);
                    }
                }

                return res;

            }
            catch (DbEntityValidationException e)
            {
                StringBuilder oError = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    oError.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));

                    foreach (var ve in eve.ValidationErrors)
                    {
                        oError.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                string msg = oError.ToString();
                throw new Exception(msg);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Create or update a FrontUser
        /// </summary>
        /// <param name="FrontUserTarget"></param>
        public int SaveFrontUser(FrontUser FrontUserTarget, bool actualizaContrasena)
        {
            int res = -1;
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the FrontUser exists
                    FrontUser oFrontUser = GetFrontUserbyKey(FrontUserTarget);
                    Cryptography oCrypto = new Cryptography();

                    string accion = "";

                    if (oFrontUser != null)
                    {
                        // if exists then edit                         
                        FrontUserTarget.contrasena = oCrypto.Encrypt(PasswordHelper.Generate());

                        ctx.FrontUser.Attach(oFrontUser);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oFrontUser, FrontUserTarget);
                        ctx.SaveChanges();
                        res = oFrontUser.id;
                        accion = "modificación";
                    }

                    // send notification
                    List<System.Net.Mail.MailAddress> to = new List<System.Net.Mail.MailAddress>();

                    if (ConfigurationManager.AppSettings["testMail"].ToString() == "N")
                        to.Add(new System.Net.Mail.MailAddress(FrontUserTarget.email));
                    else
                        to.Add(new System.Net.Mail.MailAddress("silverio.bernal@orkidea.co"));


                    Dictionary<string, string> dynamicValues = new Dictionary<string, string>();
                    dynamicValues.Add("[usuario]", FrontUserTarget.usuario);
                    dynamicValues.Add("[clave]", oCrypto.Decrypt(FrontUserTarget.contrasena));
                    dynamicValues.Add("[urlSitio]", ConfigurationManager.AppSettings["UrlApp"].ToString());

                    MailingHelper.SendMail(to, string.Format("Notificación de {0} de usuario", accion),
                        ConfigurationManager.AppSettings["emailNewUserNotificationTemplateHTML"].ToString(),
                        ConfigurationManager.AppSettings["emailNewUserNotificationTemplateText"].ToString(),
                        ConfigurationManager.AppSettings["emailLogoPath"].ToString(), dynamicValues);

                }

                return res;

            }
            catch (DbEntityValidationException e)
            {
                StringBuilder oError = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    oError.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));

                    foreach (var ve in eve.ValidationErrors)
                    {
                        oError.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                string msg = oError.ToString();
                throw new Exception(msg);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Delete a FrontUser
        /// </summary>
        /// <param name="FrontUserTarget"></param>
        public void DeleteFrontUser(FrontUser FrontUserTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    FrontUser oFrontUser = GetFrontUserbyKey(FrontUserTarget);

                    if (oFrontUser != null)
                    {
                        // if exists then edit 
                        ctx.FrontUser.Attach(oFrontUser);
                        ctx.FrontUser.Remove(oFrontUser);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("REFERENCE constraint"))
                {
                    throw new Exception("No se puede eliminar este grado porque existe información asociada a este.");
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public FrontUser GetFrontUserbyName(FrontUser FrontUserTarget)
        {
            FrontUser oFrontUser = new FrontUser();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oFrontUser = ctx.FrontUser.Where(x => x.usuario == FrontUserTarget.usuario).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oFrontUser;
        }

        public string UserNameSuggest(int idSocio)
        {
            string res = "";

            BizClubPartner bizClubPartner = new BizClubPartner();
            ClubPartner clubPartner = bizClubPartner.GetClubPartnerbyKey(new ClubPartner() { docid = idSocio });

            string usuariosSugeridos = UserNameHelper.userNameGenerator(clubPartner.nombre, true);

            string[] usuariosSugeridosArray = usuariosSugeridos.Split('|');

            List<FrontUser> lsUsuarios = GetFrontUserList();

            for (int i = 0; i < usuariosSugeridosArray.Count(); i++)
            {
                string usuarioActual = usuariosSugeridosArray[i];

                int repetidos = lsUsuarios.Where(x => x.usuario.Contains(usuarioActual)).Count();

                if (repetidos != 0)
                {
                    res += usuariosSugeridosArray[i] + (repetidos + 1).ToString() + "|";

                }
                else
                {
                    res += usuariosSugeridosArray[i] + "|";
                }
            }
            return res.Substring(0, (res.Length - 1)); ;
        }
    }
}
