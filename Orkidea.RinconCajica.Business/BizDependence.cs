using Orkidea.RinconCajica.DataAccessEF;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orkidea.RinconCajica.Business
{
    public class BizDependence
    {
        /*CRUD Dependence*/

        /// <summary>
        /// Retrieve process list 
        /// </summary>
        /// <param name="schoolTarget"></param>
        /// <returns></returns>
        public List<Dependence> GetDependenceList()
        {

            List<Dependence> lstDependence = new List<Dependence>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstDependence = ctx.Dependence.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstDependence;
        }

        /// <summary>
        /// Retrieve course information based in the primary key
        /// </summary>
        /// <param name="DependenceTarget"></param>
        /// <returns></returns>
        public Dependence GetDependencebyKey(Dependence DependenceTarget)
        {
            Dependence oDependence = new Dependence();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oDependence =
                        ctx.Dependence.Where(x => x.id.Equals(DependenceTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oDependence;
        }

        /// <summary>
        /// Create or update a course
        /// </summary>
        /// <param name="Dependence"></param>
        public void SaveDependence(Dependence Dependence)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the student exists
                    Dependence oDependence = GetDependencebyKey(Dependence);

                    if (oDependence != null)
                    {
                        // if exists then edit 
                        ctx.Dependence.Attach(oDependence);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oDependence, Dependence);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Dependence.Add(Dependence);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="DependenceTarget"></param>
        public void DeleteDependence(Dependence DependenceTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    Dependence oDependence = GetDependencebyKey(DependenceTarget);

                    if (oDependence != null)
                    {
                        // if exists then edit 
                        ctx.Dependence.Attach(oDependence);
                        ctx.Dependence.Remove(oDependence);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
