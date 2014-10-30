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
    public class BizMessageActor
    {
        /*CRUD MessageActors*/

        /// <summary>
        /// Retrieve MessageActors list without parameters
        /// </summary>
        /// <returns></returns>
        public List<MessageActor> GetMessageActorList()
        {

            List<MessageActor> lstMessageActor = new List<MessageActor>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstMessageActor = ctx.MessageActor.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstMessageActor;
        }

        /// <summary>
        /// Retrieve MessageActors list without parameters
        /// </summary>
        /// <returns></returns>
        public List<MessageActor> GetMessageActorListByType(string tipo)
        {

            List<MessageActor> lstMessageActor = new List<MessageActor>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstMessageActor = ctx.MessageActor.Where(x => x.tipo == tipo).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstMessageActor;
        }

        /// <summary>
        /// Retrieve MessageActor information based in the primary key
        /// </summary>
        /// <param name="messageActorTarget"></param>
        /// <returns></returns>
        public MessageActor GetMessageActorbyKey(MessageActor messageActorTarget)
        {
            MessageActor oMessageActor = new MessageActor();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oMessageActor = ctx.MessageActor.Where(x => x.id.Equals(messageActorTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oMessageActor;
        }

        /// <summary>
        /// Create or update a MessageActor
        /// </summary>
        /// <param name="messageActorTarget"></param>
        public void SaveMessageActor(MessageActor messageActorTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the MessageActor exists
                    MessageActor oMessageActor = GetMessageActorbyKey(messageActorTarget);

                    if (oMessageActor != null)
                    {
                        // if exists then edit 
                        ctx.MessageActor.Attach(oMessageActor);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oMessageActor, messageActorTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.MessageActor.Add(messageActorTarget);
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
        /// Delete a MessageActor
        /// </summary>
        /// <param name="messageActorTarget"></param>
        public void DeleteMessageActor(MessageActor messageActorTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    MessageActor oMessageActor = GetMessageActorbyKey(messageActorTarget);

                    if (oMessageActor != null)
                    {
                        // if exists then edit 
                        ctx.MessageActor.Attach(oMessageActor);
                        ctx.MessageActor.Remove(oMessageActor);
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
