using System;
using System.Collections.Generic;
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
        public void SaveJoinContest(JoinContest JoinContestTarget)
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
