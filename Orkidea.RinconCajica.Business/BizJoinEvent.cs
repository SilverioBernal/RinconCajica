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
    public class BizJoinEvent
    {
        /*CRUD JoinEvents*/

        /// <summary>
        /// Retrieve JoinEvents list without parameters
        /// </summary>
        /// <returns></returns>
        public List<JoinEvent> GetJoinEventList()
        {

            List<JoinEvent> lstJoinEvent = new List<JoinEvent>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstJoinEvent = ctx.JoinEvent.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstJoinEvent;
        }

        public List<JoinEvent> GetJoinEventList(JoinEvent JoinEventTarget)
        {

            List<JoinEvent> lstJoinEvent = new List<JoinEvent>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstJoinEvent = ctx.JoinEvent.Where(x => x.idEvento.Equals(JoinEventTarget.idEvento)).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstJoinEvent;
        }

        /// <summary>
        /// Retrieve JoinEvent information based in the primary key
        /// </summary>
        /// <param name="JoinEventTarget"></param>
        /// <returns></returns>
        public JoinEvent GetJoinEventbyKey(JoinEvent JoinEventTarget)
        {
            JoinEvent oJoinEvent = new JoinEvent();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oJoinEvent = ctx.JoinEvent.Where(x => x.id.Equals(JoinEventTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oJoinEvent;
        }

        /// <summary>
        /// Create or update a JoinEvent
        /// </summary>
        /// <param name="JoinEventTarget"></param>
        public void SaveJoinEvent(JoinEvent JoinEventTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the JoinEvent exists
                    JoinEvent oJoinEvent = GetJoinEventbyKey(JoinEventTarget);

                    if (oJoinEvent != null)
                    {
                        // if exists then edit 
                        ctx.JoinEvent.Attach(oJoinEvent);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oJoinEvent, JoinEventTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.JoinEvent.Add(JoinEventTarget);
                        ctx.SaveChanges();

                        BizInstitutionalEvent bizIE = new BizInstitutionalEvent();
                        InstitutionalEvent IE = bizIE.GetInstitutionalEventbyKey(new InstitutionalEvent() { id = JoinEventTarget.idEvento });

                        // send notification
                        List<System.Net.Mail.MailAddress> to = new List<System.Net.Mail.MailAddress>();

                        if (ConfigurationManager.AppSettings["testMail"].ToString() == "N")
                            to.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailAdmin"].ToString()));
                        else
                            to.Add(new System.Net.Mail.MailAddress("silverio.bernal@orkidea.co"));


                        Dictionary<string, string> dynamicValues = new Dictionary<string, string>();
                        dynamicValues.Add("[socio]", string.Format("{0}-{1}", JoinEventTarget.idSocio.ToString(), JoinEventTarget.nombre));
                        dynamicValues.Add("[evento]", IE.nombreEvento);
                        dynamicValues.Add("[urlSitio]", ConfigurationManager.AppSettings["UrlApp"].ToString());

                        MailingHelper.SendMail(to, "Notificación de creacion de usuario",
                            ConfigurationManager.AppSettings["emailJoinEventNotificationTemplateHTML"].ToString(),
                            ConfigurationManager.AppSettings["emailJoinEventNotificationTemplateText"].ToString(),
                            ConfigurationManager.AppSettings["emailLogoPath"].ToString(), dynamicValues);
                    }
                }

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
        /// Delete a JoinEvent
        /// </summary>
        /// <param name="JoinEventTarget"></param>
        public void DeleteJoinEvent(JoinEvent JoinEventTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    JoinEvent oJoinEvent = GetJoinEventbyKey(JoinEventTarget);

                    if (oJoinEvent != null)
                    {
                        // if exists then edit 
                        ctx.JoinEvent.Attach(oJoinEvent);
                        ctx.JoinEvent.Remove(oJoinEvent);
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
    }
}
