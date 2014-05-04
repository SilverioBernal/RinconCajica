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
    public class BizSport
    {
        /*CRUD Sports*/

        /// <summary>
        /// Retrieve Sports list without parameters
        /// </summary>
        /// <returns></returns>
        public List<Sport> GetSportList()
        {

            List<Sport> lstSport = new List<Sport>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstSport = ctx.Sport.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstSport;
        }

        /// <summary>
        /// Retrieve Sport information based in the primary key
        /// </summary>
        /// <param name="SportTarget"></param>
        /// <returns></returns>
        public Sport GetSportbyKey(Sport SportTarget)
        {
            Sport oSport = new Sport();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oSport = ctx.Sport.Where(x => x.id.Equals(SportTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSport;
        }

        /// <summary>
        /// Create or update a Sport
        /// </summary>
        /// <param name="SportTarget"></param>
        public void SaveSport(Sport SportTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the Sport exists
                    Sport oSport = GetSportbyKey(SportTarget);

                    if (oSport != null)
                    {
                        // if exists then edit 
                        ctx.Sport.Attach(oSport);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oSport, SportTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Sport.Add(SportTarget);
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
        /// Delete a Sport
        /// </summary>
        /// <param name="SportTarget"></param>
        public void DeleteSport(Sport SportTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    Sport oSport = GetSportbyKey(SportTarget);

                    if (oSport != null)
                    {
                        // if exists then edit 
                        ctx.Sport.Attach(oSport);
                        ctx.Sport.Remove(oSport);
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
