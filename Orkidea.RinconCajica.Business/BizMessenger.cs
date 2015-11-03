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
    public class BizMessenger
    {
        /*CRUD Messenger*/

        /// <summary>
        /// Retrieve process list 
        /// </summary>
        /// <param name="schoolTarget"></param>
        /// <returns></returns>
        public List<Messenger> GetMessengerList()
        {

            List<Messenger> lstMessenger = new List<Messenger>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstMessenger = ctx.Messenger.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstMessenger;
        }

        /// <summary>
        /// Retrieve course information based in the primary key
        /// </summary>
        /// <param name="MessengerTarget"></param>
        /// <returns></returns>
        public Messenger GetMessengerbyKey(Messenger MessengerTarget)
        {
            Messenger oMessenger = new Messenger();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oMessenger =
                        ctx.Messenger.Where(x => x.id.Equals(MessengerTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oMessenger;
        }

        /// <summary>
        /// Create or update a course
        /// </summary>
        /// <param name="Messenger"></param>
        public void SaveMessenger(Messenger Messenger)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the student exists
                    Messenger oMessenger = GetMessengerbyKey(Messenger);

                    if (oMessenger != null)
                    {
                        // if exists then edit 
                        ctx.Messenger.Attach(oMessenger);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oMessenger, Messenger);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Messenger.Add(Messenger);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="MessengerTarget"></param>
        public void DeleteMessenger(Messenger MessengerTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    Messenger oMessenger = GetMessengerbyKey(MessengerTarget);

                    if (oMessenger != null)
                    {
                        // if exists then edit 
                        ctx.Messenger.Attach(oMessenger);
                        ctx.Messenger.Remove(oMessenger);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
