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

        /// <summary>
        /// Create or update a FrontUser
        /// </summary>
        /// <param name="FrontUserTarget"></param>
        public void SaveFrontUser(FrontUser FrontUserTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the FrontUser exists
                    FrontUser oFrontUser = GetFrontUserbyKey(FrontUserTarget);

                    if (oFrontUser != null)
                    {
                        // if exists then edit 
                        ctx.FrontUser.Attach(oFrontUser);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oFrontUser, FrontUserTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.FrontUser.Add(FrontUserTarget);
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
    }
}
