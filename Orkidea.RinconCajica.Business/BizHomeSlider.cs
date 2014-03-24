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
    public class BizHomeSlider
    {
        /*CRUD HomeSliders*/

        /// <summary>
        /// Retrieve HomeSliders list without parameters
        /// </summary>
        /// <returns></returns>
        public List<HomeSlider> GetHomeSliderList()
        {

            List<HomeSlider> lstHomeSlider = new List<HomeSlider>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstHomeSlider = ctx.HomeSlider.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstHomeSlider;
        }

        public List<HomeSlider> GetHomeSliderList(bool active)
        {

            List<HomeSlider> lstHomeSlider = new List<HomeSlider>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstHomeSlider = ctx.HomeSlider.Where(x => x.activo.Equals(active)).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstHomeSlider;
        }

        /// <summary>
        /// Retrieve HomeSlider information based in the primary key
        /// </summary>
        /// <param name="HomeSliderTarget"></param>
        /// <returns></returns>
        public HomeSlider GetHomeSliderbyKey(HomeSlider HomeSliderTarget)
        {
            HomeSlider oHomeSlider = new HomeSlider();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oHomeSlider = ctx.HomeSlider.Where(x => x.id.Equals(HomeSliderTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oHomeSlider;
        }

        /// <summary>
        /// Create or update a HomeSlider
        /// </summary>
        /// <param name="HomeSliderTarget"></param>
        public void SaveHomeSlider(HomeSlider HomeSliderTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the HomeSlider exists
                    HomeSlider oHomeSlider = GetHomeSliderbyKey(HomeSliderTarget);

                    if (oHomeSlider != null)
                    {
                        // if exists then edit 
                        ctx.HomeSlider.Attach(oHomeSlider);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oHomeSlider, HomeSliderTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.HomeSlider.Add(HomeSliderTarget);
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
        /// Delete a HomeSlider
        /// </summary>
        /// <param name="HomeSliderTarget"></param>
        public void DeleteHomeSlider(HomeSlider HomeSliderTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    HomeSlider oHomeSlider = GetHomeSliderbyKey(HomeSliderTarget);

                    if (oHomeSlider != null)
                    {
                        // if exists then edit 
                        ctx.HomeSlider.Attach(oHomeSlider);
                        ctx.HomeSlider.Remove(oHomeSlider);
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
