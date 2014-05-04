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
    public class BizSportBranch
    {
        /*CRUD SportBranchs*/

        /// <summary>
        /// Retrieve SportBranchs list without parameters
        /// </summary>
        /// <returns></returns>
        public List<SportBranch> GetSportBranchList()
        {

            List<SportBranch> lstSportBranch = new List<SportBranch>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstSportBranch = ctx.SportBranch.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstSportBranch;
        }

        /// <summary>
        /// Retrieve SportBranch information based in the primary key
        /// </summary>
        /// <param name="SportBranchTarget"></param>
        /// <returns></returns>
        public SportBranch GetSportBranchbyKey(SportBranch SportBranchTarget)
        {
            SportBranch oSportBranch = new SportBranch();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oSportBranch = ctx.SportBranch.Where(x => x.id.Equals(SportBranchTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSportBranch;
        }

        /// <summary>
        /// Create or update a SportBranch
        /// </summary>
        /// <param name="SportBranchTarget"></param>
        public void SaveSportBranch(SportBranch SportBranchTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the SportBranch exists
                    SportBranch oSportBranch = GetSportBranchbyKey(SportBranchTarget);

                    if (oSportBranch != null)
                    {
                        // if exists then edit 
                        ctx.SportBranch.Attach(oSportBranch);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oSportBranch, SportBranchTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.SportBranch.Add(SportBranchTarget);
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
        /// Delete a SportBranch
        /// </summary>
        /// <param name="SportBranchTarget"></param>
        public void DeleteSportBranch(SportBranch SportBranchTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    SportBranch oSportBranch = GetSportBranchbyKey(SportBranchTarget);

                    if (oSportBranch != null)
                    {
                        // if exists then edit 
                        ctx.SportBranch.Attach(oSportBranch);
                        ctx.SportBranch.Remove(oSportBranch);
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
