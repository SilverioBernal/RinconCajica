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
    public class BizPage
    {
        /*CRUD Pages*/

        /// <summary>
        /// Retrieve Pages list without parameters
        /// </summary>
        /// <returns></returns>
        public List<Page> GetPageList()
        {

            List<Page> lstPage = new List<Page>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstPage = ctx.Page.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstPage;
        }        

        /// <summary>
        /// Retrieve Page information based in the primary key
        /// </summary>
        /// <param name="PageTarget"></param>
        /// <returns></returns>
        public Page GetPagebyKey(Page PageTarget)
        {
            Page oPage = new Page();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oPage = ctx.Page.Where(x => x.id.Equals(PageTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oPage;
        }

        /// <summary>
        /// Create or update a Page
        /// </summary>
        /// <param name="PageTarget"></param>
        public void SavePage(Page PageTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the Page exists
                    Page oPage = GetPagebyKey(PageTarget);

                    if (oPage != null)
                    {
                        // if exists then edit 
                        ctx.Page.Attach(oPage);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oPage, PageTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Page.Add(PageTarget);
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
        /// Delete a Page
        /// </summary>
        /// <param name="PageTarget"></param>
        public void DeletePage(Page PageTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    Page oPage = GetPagebyKey(PageTarget);

                    if (oPage != null)
                    {
                        // if exists then edit 
                        ctx.Page.Attach(oPage);
                        ctx.Page.Remove(oPage);
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
