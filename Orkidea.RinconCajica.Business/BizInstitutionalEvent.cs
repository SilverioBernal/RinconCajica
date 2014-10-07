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
    public class BizInstitutionalEvent
    {
        /*CRUD InstitutionalEvents*/

        /// <summary>
        /// Retrieve InstitutionalEvents list without parameters
        /// </summary>
        /// <returns></returns>
        public List<InstitutionalEvent> GetInstitutionalEventList()
        {

            List<InstitutionalEvent> lstInstitutionalEvent = new List<InstitutionalEvent>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstInstitutionalEvent = ctx.InstitutionalEvent.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstInstitutionalEvent;
        }

        /// <summary>
        /// Retrieve InstitutionalEvent information based in the primary key
        /// </summary>
        /// <param name="InstitutionalEventTarget"></param>
        /// <returns></returns>
        public InstitutionalEvent GetInstitutionalEventbyKey(InstitutionalEvent InstitutionalEventTarget)
        {
            InstitutionalEvent oInstitutionalEvent = new InstitutionalEvent();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oInstitutionalEvent = ctx.InstitutionalEvent.Where(x => x.id.Equals(InstitutionalEventTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oInstitutionalEvent;
        }

        /// <summary>
        /// Create or update a InstitutionalEvent
        /// </summary>
        /// <param name="InstitutionalEventTarget"></param>
        public void SaveInstitutionalEvent(InstitutionalEvent InstitutionalEventTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the InstitutionalEvent exists
                    InstitutionalEvent oInstitutionalEvent = GetInstitutionalEventbyKey(InstitutionalEventTarget);

                    if (oInstitutionalEvent != null)
                    {
                        // if exists then edit 
                        ctx.InstitutionalEvent.Attach(oInstitutionalEvent);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oInstitutionalEvent, InstitutionalEventTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.InstitutionalEvent.Add(InstitutionalEventTarget);
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
        /// Delete a InstitutionalEvent
        /// </summary>
        /// <param name="InstitutionalEventTarget"></param>
        public void DeleteInstitutionalEvent(InstitutionalEvent InstitutionalEventTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    InstitutionalEvent oInstitutionalEvent = GetInstitutionalEventbyKey(InstitutionalEventTarget);

                    if (oInstitutionalEvent != null)
                    {
                        // if exists then edit 
                        ctx.InstitutionalEvent.Attach(oInstitutionalEvent);
                        ctx.InstitutionalEvent.Remove(oInstitutionalEvent);
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
