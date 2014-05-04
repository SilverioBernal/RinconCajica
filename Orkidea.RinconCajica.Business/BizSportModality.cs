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
    public class BizSportModality
    {
        /*CRUD SportModalitys*/

        /// <summary>
        /// Retrieve SportModalitys list without parameters
        /// </summary>
        /// <returns></returns>
        public List<SportModality> GetSportModalityList()
        {

            List<SportModality> lstSportModality = new List<SportModality>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstSportModality = ctx.SportModality.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstSportModality;
        }

        /// <summary>
        /// Retrieve SportModality information based in the primary key
        /// </summary>
        /// <param name="SportModalityTarget"></param>
        /// <returns></returns>
        public SportModality GetSportModalitybyKey(SportModality SportModalityTarget)
        {
            SportModality oSportModality = new SportModality();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oSportModality = ctx.SportModality.Where(x => x.id.Equals(SportModalityTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSportModality;
        }

        /// <summary>
        /// Create or update a SportModality
        /// </summary>
        /// <param name="SportModalityTarget"></param>
        public void SaveSportModality(SportModality SportModalityTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the SportModality exists
                    SportModality oSportModality = GetSportModalitybyKey(SportModalityTarget);

                    if (oSportModality != null)
                    {
                        // if exists then edit 
                        ctx.SportModality.Attach(oSportModality);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oSportModality, SportModalityTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.SportModality.Add(SportModalityTarget);
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
        /// Delete a SportModality
        /// </summary>
        /// <param name="SportModalityTarget"></param>
        public void DeleteSportModality(SportModality SportModalityTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    SportModality oSportModality = GetSportModalitybyKey(SportModalityTarget);

                    if (oSportModality != null)
                    {
                        // if exists then edit 
                        ctx.SportModality.Attach(oSportModality);
                        ctx.SportModality.Remove(oSportModality);
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
