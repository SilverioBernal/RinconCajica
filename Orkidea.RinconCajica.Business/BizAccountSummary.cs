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
    public class BizAccountSummary
    {
        /// <summary>
        /// Retrieve AccountSummarys list without parameters
        /// </summary>
        /// <returns></returns>
        public List<AccountSummary> GetAccountSummaryList()
        {

            List<AccountSummary> lstAccountSummary = new List<AccountSummary>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstAccountSummary = ctx.AccountSummary.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstAccountSummary;
        }

        public List<AccountSummary> GetAccountSummaryList(string idAccion)
        {

            List<AccountSummary> lstAccountSummary = new List<AccountSummary>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstAccountSummary = ctx.AccountSummary.Where(x => x.accion == idAccion).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstAccountSummary;
        }

        /// <summary>
        /// Retrieve AccountSummary information based in the primary key
        /// </summary>
        /// <param name="AccountSummaryTarget"></param>
        /// <returns></returns>
        public AccountSummary GetAccountSummarybyKey(AccountSummary AccountSummaryTarget)
        {
            AccountSummary oAccountSummary = new AccountSummary();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oAccountSummary = ctx.AccountSummary.Where(x => x.id.Equals(AccountSummaryTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oAccountSummary;
        }

        /// <summary>
        /// Retrieve AccountSummary information based in the primary key
        /// </summary>
        /// <param name="AccountSummaryTarget"></param>
        /// <returns></returns>
        public AccountSummary GetAccountSummarybyKeyValues(AccountSummary AccountSummaryTarget)
        {
            AccountSummary oAccountSummary = new AccountSummary();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oAccountSummary = ctx.AccountSummary.Where(
                        x => x.accion == AccountSummaryTarget.accion && x.ano.Equals(AccountSummaryTarget.ano) && x.mes.Equals(AccountSummaryTarget.mes)
                        ).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oAccountSummary;
        }

        /// <summary>
        /// Create or update a AccountSummary
        /// </summary>
        /// <param name="AccountSummaryTarget"></param>
        public int SaveAccountSummary(AccountSummary AccountSummaryTarget)
        {
            int res = -1;
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the AccountSummary exists
                    AccountSummary oAccountSummary = GetAccountSummarybyKey(AccountSummaryTarget);

                    if (oAccountSummary != null)
                    {
                        //verify if the AccountSummary exists by key values 
                        oAccountSummary = GetAccountSummarybyKeyValues(AccountSummaryTarget);

                        if (oAccountSummary != null)
                        {
                            // if exists then edit                         
                            ctx.AccountSummary.Attach(oAccountSummary);
                            EntityFrameworkHelper.EnumeratePropertyDifferences(oAccountSummary, AccountSummaryTarget);
                            ctx.SaveChanges();
                            res = oAccountSummary.id;
                        }
                    }
                    else
                    {
                        // else  create                                               
                        ctx.AccountSummary.Add(AccountSummaryTarget);
                        ctx.SaveChanges();
                        res = AccountSummaryTarget.id;
                    }
                }

                return res;

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
        /// Delete a AccountSummary
        /// </summary>
        /// <param name="AccountSummaryTarget"></param>
        public void DeleteAccountSummary(AccountSummary AccountSummaryTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    AccountSummary oAccountSummary = GetAccountSummarybyKey(AccountSummaryTarget);

                    if (oAccountSummary != null)
                    {
                        // if exists then edit 
                        ctx.AccountSummary.Attach(oAccountSummary);
                        ctx.AccountSummary.Remove(oAccountSummary);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("REFERENCE constraint"))
                {
                    throw new Exception("No se puede eliminar este extracto porque existe información asociada a este.");
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
