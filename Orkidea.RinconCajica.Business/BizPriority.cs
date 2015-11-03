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
    public class BizPriority
    {
        /*CRUD Priority*/

        /// <summary>
        /// Retrieve process list 
        /// </summary>
        /// <param name="schoolTarget"></param>
        /// <returns></returns>
        public List<Priority> GetPriorityList()
        {

            List<Priority> lstPriority = new List<Priority>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstPriority = ctx.Priority.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstPriority;
        }

        /// <summary>
        /// Retrieve course information based in the primary key
        /// </summary>
        /// <param name="PriorityTarget"></param>
        /// <returns></returns>
        public Priority GetPrioritybyKey(Priority PriorityTarget)
        {
            Priority oPriority = new Priority();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oPriority =
                        ctx.Priority.Where(x => x.id.Equals(PriorityTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oPriority;
        }

        /// <summary>
        /// Create or update a course
        /// </summary>
        /// <param name="Priority"></param>
        public void SavePriority(Priority Priority)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the student exists
                    Priority oPriority = GetPrioritybyKey(Priority);

                    if (oPriority != null)
                    {
                        // if exists then edit 
                        ctx.Priority.Attach(oPriority);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oPriority, Priority);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Priority.Add(Priority);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="PriorityTarget"></param>
        public void DeletePriority(Priority PriorityTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    Priority oPriority = GetPrioritybyKey(PriorityTarget);

                    if (oPriority != null)
                    {
                        // if exists then edit 
                        ctx.Priority.Attach(oPriority);
                        ctx.Priority.Remove(oPriority);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
