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
    public class BizCorrespondenceIn
    {
        /*CRUD CorrespondenceIn*/

        /// <summary>
        /// Retrieve process list 
        /// </summary>
        /// <param name="schoolTarget"></param>
        /// <returns></returns>
        public List<CorrespondenceIn> GetCorrespondenceInList()
        {

            List<CorrespondenceIn> lstCorrespondenceIn = new List<CorrespondenceIn>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstCorrespondenceIn = ctx.CorrespondenceIn.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstCorrespondenceIn;
        }

        public List<CorrespondenceIn> GetCorrespondenceInList(DateTime desde, DateTime hasta)
        {

            List<CorrespondenceIn> lstCorrespondenceIn = new List<CorrespondenceIn>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    hasta = hasta.AddDays(1);
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstCorrespondenceIn = ctx.CorrespondenceIn.Where(x => x.fecha >= desde && x.fecha < hasta).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstCorrespondenceIn;
        }

        public List<CorrespondenceIn> GetNotRollerCorrespondenceInList()
        {

            List<CorrespondenceIn> lstCorrespondenceIn = new List<CorrespondenceIn>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstCorrespondenceIn = ctx.CorrespondenceIn.Where(x => x.fechaPatinado == null).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstCorrespondenceIn;
        }

        /// <summary>
        /// Retrieve course information based in the primary key
        /// </summary>
        /// <param name="CorrespondenceInTarget"></param>
        /// <returns></returns>
        public CorrespondenceIn GetCorrespondenceInbyKey(CorrespondenceIn CorrespondenceInTarget)
        {
            CorrespondenceIn oCorrespondenceIn = new CorrespondenceIn();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oCorrespondenceIn =
                        ctx.CorrespondenceIn.Where(x => x.id.Equals(CorrespondenceInTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oCorrespondenceIn;
        }

        /// <summary>
        /// Create or update a course
        /// </summary>
        /// <param name="CorrespondenceIn"></param>
        public void SaveCorrespondenceIn(CorrespondenceIn CorrespondenceIn)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the student exists
                    CorrespondenceIn oCorrespondenceIn = GetCorrespondenceInbyKey(CorrespondenceIn);

                    if (oCorrespondenceIn != null)
                    {
                        // if exists then edit 
                        ctx.CorrespondenceIn.Attach(oCorrespondenceIn);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oCorrespondenceIn, CorrespondenceIn);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.CorrespondenceIn.Add(CorrespondenceIn);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="CorrespondenceInTarget"></param>
        public void DeleteCorrespondenceIn(CorrespondenceIn CorrespondenceInTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    CorrespondenceIn oCorrespondenceIn = GetCorrespondenceInbyKey(CorrespondenceInTarget);

                    if (oCorrespondenceIn != null)
                    {
                        // if exists then edit 
                        ctx.CorrespondenceIn.Attach(oCorrespondenceIn);
                        ctx.CorrespondenceIn.Remove(oCorrespondenceIn);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
