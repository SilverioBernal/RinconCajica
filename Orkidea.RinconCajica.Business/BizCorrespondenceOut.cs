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
    public class BizCorrespondenceOut
    {
        /*CRUD CorrespondenceOut*/

        /// <summary>
        /// Retrieve process list 
        /// </summary>
        /// <param name="schoolTarget"></param>
        /// <returns></returns>
        public List<CorrespondenceOut> GetCorrespondenceOutList()
        {

            List<CorrespondenceOut> lstCorrespondenceOut = new List<CorrespondenceOut>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstCorrespondenceOut = ctx.CorrespondenceOut.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstCorrespondenceOut;
        }

        public List<CorrespondenceOut> GetCorrespondenceOutList(DateTime desde, DateTime hasta)
        {

            List<CorrespondenceOut> lstCorrespondenceOut = new List<CorrespondenceOut>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    hasta = hasta.AddDays(1);
                    lstCorrespondenceOut = ctx.CorrespondenceOut.Where(x => x.fecha >= desde && x.fecha < hasta).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstCorrespondenceOut;
        }

        /// <summary>
        /// Retrieve course information based in the primary key
        /// </summary>
        /// <param name="CorrespondenceOutTarget"></param>
        /// <returns></returns>
        public CorrespondenceOut GetCorrespondenceOutbyKey(CorrespondenceOut CorrespondenceOutTarget)
        {
            CorrespondenceOut oCorrespondenceOut = new CorrespondenceOut();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oCorrespondenceOut =
                        ctx.CorrespondenceOut.Where(x => x.id.Equals(CorrespondenceOutTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oCorrespondenceOut;
        }

        /// <summary>
        /// Create or update a course
        /// </summary>
        /// <param name="CorrespondenceOut"></param>
        public void SaveCorrespondenceOut(CorrespondenceOut CorrespondenceOut)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the student exists
                    CorrespondenceOut oCorrespondenceOut = GetCorrespondenceOutbyKey(CorrespondenceOut);

                    if (oCorrespondenceOut != null)
                    {
                        // if exists then edit 
                        ctx.CorrespondenceOut.Attach(oCorrespondenceOut);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oCorrespondenceOut, CorrespondenceOut);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.CorrespondenceOut.Add(CorrespondenceOut);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="CorrespondenceOutTarget"></param>
        public void DeleteCorrespondenceOut(CorrespondenceOut CorrespondenceOutTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    CorrespondenceOut oCorrespondenceOut = GetCorrespondenceOutbyKey(CorrespondenceOutTarget);

                    if (oCorrespondenceOut != null)
                    {
                        // if exists then edit 
                        ctx.CorrespondenceOut.Attach(oCorrespondenceOut);
                        ctx.CorrespondenceOut.Remove(oCorrespondenceOut);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
