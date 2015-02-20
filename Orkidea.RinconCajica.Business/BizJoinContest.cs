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
    public class BizJoinContest
    {
        /*CRUD JoinContests*/

        /// <summary>
        /// Retrieve JoinContests list without parameters
        /// </summary>
        /// <returns></returns>
        public List<JoinContest> GetJoinContestList()
        {

            List<JoinContest> lstJoinContest = new List<JoinContest>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstJoinContest = ctx.JoinContest.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstJoinContest;
        }

        public List<JoinContest> GetJoinContestList(bool active)
        {

            List<JoinContest> lstJoinContest = new List<JoinContest>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstJoinContest = ctx.JoinContest.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstJoinContest;
        }

        public List<JoinContest> GetJoinContestList(JoinContest joinContestTarget)
        {

            List<JoinContest> lstJoinContest = new List<JoinContest>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstJoinContest = ctx.JoinContest.Where(x => x.idTorneo.Equals(joinContestTarget.idTorneo)).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstJoinContest;
        }

        /// <summary>
        /// Retrieve JoinContest information based in the primary key
        /// </summary>
        /// <param name="JoinContestTarget"></param>
        /// <returns></returns>
        public JoinContest GetJoinContestbyKey(JoinContest JoinContestTarget)
        {
            JoinContest oJoinContest = new JoinContest();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oJoinContest = ctx.JoinContest.Where(x => x.id.Equals(JoinContestTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oJoinContest;
        }

        /// <summary>
        /// Create or update a JoinContest
        /// </summary>
        /// <param name="JoinContestTarget"></param>
        public void SaveJoinContest(JoinContest JoinContestTarget, string rootPath)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the JoinContest exists
                    JoinContest oJoinContest = GetJoinContestbyKey(JoinContestTarget);

                    if (oJoinContest != null)
                    {
                        // if exists then edit 
                        ctx.JoinContest.Attach(oJoinContest);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oJoinContest, JoinContestTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.JoinContest.Add(JoinContestTarget);
                        ctx.SaveChanges();

                        BizSportSchedule bizSS = new BizSportSchedule();
                        SportSchedule ss = bizSS.GetSportSchedulebyKey(new SportSchedule() { id = JoinContestTarget.idTorneo });

                        // send notification
                        List<System.Net.Mail.MailAddress> to = new List<System.Net.Mail.MailAddress>();

                        if (ConfigurationManager.AppSettings["testMail"].ToString() == "N")
                            to.Add(new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["emailAdmin"].ToString()));
                        else
                            to.Add(new System.Net.Mail.MailAddress("silverio.bernal@orkidea.co"));


                        Dictionary<string, string> dynamicValues = new Dictionary<string, string>();
                        dynamicValues.Add("[socio]", string.Format("{0}-{1}", JoinContestTarget.idSocio.ToString(), JoinContestTarget.nombre));
                        dynamicValues.Add("[torneo]", ss.competencia);
                        dynamicValues.Add("[urlSitio]", ConfigurationManager.AppSettings["UrlApp"].ToString());

                        MailingHelper.SendMail(to, "Inscripcion a torneo",
                            rootPath + ConfigurationManager.AppSettings["emailjoinContestNotificationTemplateHTML"].ToString(),
                            rootPath + ConfigurationManager.AppSettings["emailjoinContestNotificationTemplateText"].ToString(),
                            rootPath + ConfigurationManager.AppSettings["emailLogoPath"].ToString(), dynamicValues);
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
        /// Delete a JoinContest
        /// </summary>
        /// <param name="JoinContestTarget"></param>
        public void DeleteJoinContest(JoinContest JoinContestTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    JoinContest oJoinContest = GetJoinContestbyKey(JoinContestTarget);

                    if (oJoinContest != null)
                    {
                        // if exists then edit 
                        ctx.JoinContest.Attach(oJoinContest);
                        ctx.JoinContest.Remove(oJoinContest);
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
