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
    public class BizSportCategory
    {
        /*CRUD SportCategorys*/

        /// <summary>
        /// Retrieve SportCategorys list without parameters
        /// </summary>
        /// <returns></returns>
        public List<SportCategory> GetSportCategoryList()
        {

            List<SportCategory> lstSportCategory = new List<SportCategory>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstSportCategory = ctx.SportCategory.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstSportCategory;
        }

        /// <summary>
        /// Retrieve SportCategory information based in the primary key
        /// </summary>
        /// <param name="SportCategoryTarget"></param>
        /// <returns></returns>
        public SportCategory GetSportCategorybyKey(SportCategory SportCategoryTarget)
        {
            SportCategory oSportCategory = new SportCategory();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oSportCategory = ctx.SportCategory.Where(x => x.id.Equals(SportCategoryTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSportCategory;
        }

        /// <summary>
        /// Create or update a SportCategory
        /// </summary>
        /// <param name="SportCategoryTarget"></param>
        public void SaveSportCategory(SportCategory SportCategoryTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the SportCategory exists
                    SportCategory oSportCategory = GetSportCategorybyKey(SportCategoryTarget);

                    if (oSportCategory != null)
                    {
                        // if exists then edit 
                        ctx.SportCategory.Attach(oSportCategory);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oSportCategory, SportCategoryTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.SportCategory.Add(SportCategoryTarget);
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
        /// Delete a SportCategory
        /// </summary>
        /// <param name="SportCategoryTarget"></param>
        public void DeleteSportCategory(SportCategory SportCategoryTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    SportCategory oSportCategory = GetSportCategorybyKey(SportCategoryTarget);

                    if (oSportCategory != null)
                    {
                        // if exists then edit 
                        ctx.SportCategory.Attach(oSportCategory);
                        ctx.SportCategory.Remove(oSportCategory);
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
