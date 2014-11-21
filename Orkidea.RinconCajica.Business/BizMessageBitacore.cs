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

        public List<MessageBitacore> GetMessageBitacoreList(string tipo, DateTime fechaSolicitada)
        {

            List<MessageBitacore> lstMessageBitacore = new List<MessageBitacore>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    DateTime hoy = fechaSolicitada.Date;
                    DateTime mañana = hoy.AddDays(1);

                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstMessageBitacore = ctx.MessageBitacore.Where(x => x.tipoRegistro== tipo && x.fecha>= hoy && x.fecha < mañana).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstMessageBitacore;
        }

        public List<MessageBitacore> GetMessageBitacoreList(string tipo, DateTime fechaDesde, DateTime fechaHasta)
        {
            List<MessageBitacore> lstMessageBitacore = new List<MessageBitacore>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    DateTime desde = fechaDesde.Date;
                    DateTime hasta = fechaHasta.Date.AddDays(1);

                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstMessageBitacore = ctx.MessageBitacore.Where(x => x.tipoRegistro == tipo && x.fecha >= desde && x.fecha < hasta).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstMessageBitacore;
        }

        public List<MessageBitacore> GetMessageBitacoreList(MessageBitacore messageBitacore)
        {
            List<MessageBitacore> lstMessageBitacore = new List<MessageBitacore>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstMessageBitacore = ctx.MessageBitacore.Where(x => x.entregaPersonal.Equals(messageBitacore.entregaPersonal)).ToList();

                    if (messageBitacore.fecha.Year != 1)
                    {
                        DateTime fechaDesde = messageBitacore.fecha;
                        DateTime fechaHasta = messageBitacore.fecha;

                        lstMessageBitacore = lstMessageBitacore.Where(x => x.fecha >= fechaDesde && x.fecha < fechaHasta).ToList();
                    }

                    if (messageBitacore.descripcionCorta != null)
                    {
                        lstMessageBitacore = lstMessageBitacore.Where(x => x.descripcionCorta == messageBitacore.descripcionCorta).ToList();
                    }

                    if (messageBitacore.prioridad != 0)
                    {
                        lstMessageBitacore = lstMessageBitacore.Where(x => x.prioridad.Equals(messageBitacore.prioridad)).ToList();
                    }

                    if (messageBitacore.fechaLimite != null)
                    {
                        DateTime fechaDesde = (DateTime)messageBitacore.fechaLimite;
                        DateTime fechaHasta = (DateTime)messageBitacore.fechaLimite;

                        lstMessageBitacore = lstMessageBitacore.Where(x => x.fechaLimite >= fechaDesde && x.fechaLimite < fechaHasta).ToList();
                    }                    
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
        public long SaveMessageBitacore(MessageBitacore MessageBitacoreTarget)
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
                        MessageBitacoreTarget.idRegistro = consecutivo(MessageBitacoreTarget.tipoRegistro);

                        ctx.MessageBitacore.Add(MessageBitacoreTarget);
                        ctx.SaveChanges();
                    }

                    return MessageBitacoreTarget.idRegistro;
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

        private long consecutivo(string tipo)
        {
            long res = 0;
            long maximo = 0;
            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    maximo = ctx.MessageBitacore.Where(x => x.tipoRegistro == tipo).Max(x => x.idRegistro);
                    
                    
                }
            }
            catch (Exception)
            {
                maximo = 0;                
            }

            return res = maximo + 1; ;
        }
    }
}
