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
    public class BizMessaging
    {
        /*CRUD Messaging*/

        /// <summary>
        /// Retrieve process list 
        /// </summary>
        /// <param name="schoolTarget"></param>
        /// <returns></returns>
        public List<Messaging> GetMessagingList()
        {

            List<Messaging> lstMessaging = new List<Messaging>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstMessaging = ctx.Messaging.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstMessaging;
        }

        public List<Messaging> GetMessagingList(DateTime desde, DateTime hasta)
        {

            List<Messaging> lstMessaging = new List<Messaging>();
            hasta = hasta.AddDays(1);
            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstMessaging = ctx.Messaging.Where(x=> x.fecha >= desde && x.fecha < hasta).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstMessaging;
        }

        public List<Messaging> GetOpenMessagingList()
        {

            List<Messaging> lstMessaging = new List<Messaging>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    lstMessaging = ctx.Messaging.Where(x => x.fechaRealizado == null).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstMessaging;
        }

        /// <summary>
        /// Retrieve course information based in the primary key
        /// </summary>
        /// <param name="MessagingTarget"></param>
        /// <returns></returns>
        public Messaging GetMessagingbyKey(Messaging MessagingTarget)
        {
            Messaging oMessaging = new Messaging();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oMessaging =
                        ctx.Messaging.Where(x => x.id.Equals(MessagingTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oMessaging;
        }

        /// <summary>
        /// Create or update a course
        /// </summary>
        /// <param name="Messaging"></param>
        public void SaveMessaging(Messaging Messaging)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the student exists
                    Messaging oMessaging = GetMessagingbyKey(Messaging);

                    if (oMessaging != null)
                    {
                        // if exists then edit 
                        ctx.Messaging.Attach(oMessaging);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oMessaging, Messaging);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Messaging.Add(Messaging);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="MessagingTarget"></param>
        public void DeleteMessaging(Messaging MessagingTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    Messaging oMessaging = GetMessagingbyKey(MessagingTarget);

                    if (oMessaging != null)
                    {
                        // if exists then edit 
                        ctx.Messaging.Attach(oMessaging);
                        ctx.Messaging.Remove(oMessaging);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
