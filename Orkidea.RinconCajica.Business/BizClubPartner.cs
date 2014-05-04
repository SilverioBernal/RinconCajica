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
    public class BizClubPartner
    {
        /*CRUD ClubPartners*/

        /// <summary>
        /// Retrieve ClubPartners list without parameters
        /// </summary>
        /// <returns></returns>
        public List<ClubPartner> GetClubPartnerList()
        {

            List<ClubPartner> lstClubPartner = new List<ClubPartner>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstClubPartner = ctx.ClubPartner.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstClubPartner;
        }

        /// <summary>
        /// Retrieve ClubPartner information based in the primary key
        /// </summary>
        /// <param name="ClubPartnerTarget"></param>
        /// <returns></returns>
        public ClubPartner GetClubPartnerbyKey(ClubPartner ClubPartnerTarget)
        {
            ClubPartner oClubPartner = new ClubPartner();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oClubPartner = ctx.ClubPartner.Where(x => x.id.Equals(ClubPartnerTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oClubPartner;
        }

        public ClubPartner GetClubPartnerbyUser(int user)
        {
            ClubPartner oClubPartner = new ClubPartner();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oClubPartner = ctx.ClubPartner.Where(x => x.idUsuario==user).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oClubPartner;
        }

        /// <summary>
        /// Create or update a ClubPartner
        /// </summary>
        /// <param name="ClubPartnerTarget"></param>
        public void SaveClubPartner(ClubPartner ClubPartnerTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the ClubPartner exists
                    ClubPartner oClubPartner = GetClubPartnerbyKey(ClubPartnerTarget);

                    if (oClubPartner != null)
                    {
                        // if exists then edit 
                        ctx.ClubPartner.Attach(oClubPartner);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oClubPartner, ClubPartnerTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.ClubPartner.Add(ClubPartnerTarget);
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
        /// Delete a ClubPartner
        /// </summary>
        /// <param name="ClubPartnerTarget"></param>
        public void DeleteClubPartner(ClubPartner ClubPartnerTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    ClubPartner oClubPartner = GetClubPartnerbyKey(ClubPartnerTarget);

                    if (oClubPartner != null)
                    {
                        // if exists then edit 
                        ctx.ClubPartner.Attach(oClubPartner);
                        ctx.ClubPartner.Remove(oClubPartner);
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
