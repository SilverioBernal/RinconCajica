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
    public class BizSideBar
    {
        /*CRUD SideBars*/

        /// <summary>
        /// Retrieve SideBars list without parameters
        /// </summary>
        /// <returns></returns>
        public List<SideBar> GetSideBarList()
        {

            List<SideBar> lstSideBar = new List<SideBar>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstSideBar = ctx.SideBar.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstSideBar;
        }

        /// <summary>
        /// Retrieve SideBar information based in the primary key
        /// </summary>
        /// <param name="SideBarTarget"></param>
        /// <returns></returns>
        public SideBar GetSideBarByKey(SideBar SideBarTarget)
        {
            SideBar oSideBar = new SideBar();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oSideBar = ctx.SideBar.Where(x => x.id.Equals(SideBarTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSideBar;
        }

        /// <summary>
        /// Create or update a SideBar
        /// </summary>
        /// <param name="SideBarTarget"></param>
        public void SaveSideBar(SideBar SideBarTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the SideBar exists
                    SideBar oSideBar = GetSideBarByKey(SideBarTarget);

                    if (oSideBar != null)
                    {
                        // if exists then edit 
                        ctx.SideBar.Attach(oSideBar);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oSideBar, SideBarTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.SideBar.Add(SideBarTarget);
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
        /// Delete a SideBar
        /// </summary>
        /// <param name="SideBarTarget"></param>
        public void DeleteSideBar(SideBar SideBarTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    SideBar oSideBar = GetSideBarByKey(SideBarTarget);

                    if (oSideBar != null)
                    {
                        // if exists then edit 
                        ctx.SideBar.Attach(oSideBar);
                        ctx.SideBar.Remove(oSideBar);
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
