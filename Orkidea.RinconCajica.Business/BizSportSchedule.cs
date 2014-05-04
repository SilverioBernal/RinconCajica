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
    public class BizSportSchedule
    {
        /*CRUD SportSchedules*/

        /// <summary>
        /// Retrieve SportSchedules list without parameters
        /// </summary>
        /// <returns></returns>
        public List<SportSchedule> GetSportScheduleList()
        {

            List<SportSchedule> lstSportSchedule = new List<SportSchedule>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstSportSchedule = ctx.SportSchedule.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstSportSchedule;
        }

        /// <summary>
        /// Retrieve SportSchedule information based in the primary key
        /// </summary>
        /// <param name="SportScheduleTarget"></param>
        /// <returns></returns>
        public SportSchedule GetSportSchedulebyKey(SportSchedule SportScheduleTarget)
        {
            SportSchedule oSportSchedule = new SportSchedule();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oSportSchedule = ctx.SportSchedule.Where(x => x.id.Equals(SportScheduleTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSportSchedule;
        }

        /// <summary>
        /// Create or update a SportSchedule
        /// </summary>
        /// <param name="SportScheduleTarget"></param>
        public void SaveSportSchedule(SportSchedule SportScheduleTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the SportSchedule exists
                    SportSchedule oSportSchedule = GetSportSchedulebyKey(SportScheduleTarget);

                    if (oSportSchedule != null)
                    {
                        // if exists then edit 
                        ctx.SportSchedule.Attach(oSportSchedule);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oSportSchedule, SportScheduleTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.SportSchedule.Add(SportScheduleTarget);
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
        /// Delete a SportSchedule
        /// </summary>
        /// <param name="SportScheduleTarget"></param>
        public void DeleteSportSchedule(SportSchedule SportScheduleTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    SportSchedule oSportSchedule = GetSportSchedulebyKey(SportScheduleTarget);

                    if (oSportSchedule != null)
                    {
                        // if exists then edit 
                        ctx.SportSchedule.Attach(oSportSchedule);
                        ctx.SportSchedule.Remove(oSportSchedule);
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
