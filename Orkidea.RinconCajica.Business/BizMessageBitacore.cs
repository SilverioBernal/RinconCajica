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
    public class BizMessageBitacore
    {
        /*CRUD MessageBitacores*/

        /// <summary>
        /// Retrieve MessageBitacores list without parameters
        /// </summary>
        /// <returns></returns>
        public List<MessageBitacore> GetMessageBitacoreList()
        {

            List<MessageBitacore> lstMessageBitacore = new List<MessageBitacore>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstMessageBitacore = ctx.MessageBitacore.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstMessageBitacore;
        }

        /// <summary>
        /// Retrieve MessageBitacore information based in the primary key
        /// </summary>
        /// <param name="MessageBitacoreTarget"></param>
        /// <returns></returns>
        public MessageBitacore GetMessageBitacorebyKey(MessageBitacore MessageBitacoreTarget)
        {
            MessageBitacore oMessageBitacore = new MessageBitacore();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oMessageBitacore = ctx.MessageBitacore.Where(x => 
                        x.tipoRegistro == MessageBitacoreTarget.tipoRegistro &&
                        x.idRegistro.Equals(MessageBitacoreTarget.idRegistro)
                        ).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oMessageBitacore;
        }

        /// <summary>
        /// Create or update a MessageBitacore
        /// </summary>
        /// <param name="MessageBitacoreTarget"></param>
        public void SaveMessageBitacore(MessageBitacore MessageBitacoreTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the MessageBitacore exists
                    MessageBitacore oMessageBitacore = GetMessageBitacorebyKey(MessageBitacoreTarget);

                    if (oMessageBitacore != null)
                    {
                        // if exists then edit 
                        ctx.MessageBitacore.Attach(oMessageBitacore);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oMessageBitacore, MessageBitacoreTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.MessageBitacore.Add(MessageBitacoreTarget);
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
        /// Delete a MessageBitacore
        /// </summary>
        /// <param name="MessageBitacoreTarget"></param>
        public void DeleteMessageBitacore(MessageBitacore MessageBitacoreTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    MessageBitacore oMessageBitacore = GetMessageBitacorebyKey(MessageBitacoreTarget);

                    if (oMessageBitacore != null)
                    {
                        // if exists then edit 
                        ctx.MessageBitacore.Attach(oMessageBitacore);
                        ctx.MessageBitacore.Remove(oMessageBitacore);
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
