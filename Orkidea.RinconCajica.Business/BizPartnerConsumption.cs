﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orkidea.RinconCajica.DataAccessEF;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.Utilities;
using System.Threading;
using System.Data.Entity.Validation;

namespace Orkidea.RinconCajica.Business
{
    public class BizPartnerConsumption
    {
        /*CRUD PartnerConsumption*/

        /// <summary>
        /// Retrieve PartnerConsumption list 
        /// </summary>
        /// <param name="schoolTarget"></param>
        /// <returns></returns>
        public List<ConsumptionGlobal> GetPartnerConsumptionList()
        {

            List<ConsumptionGlobal> oPartnerConsumption = new List<ConsumptionGlobal>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    oPartnerConsumption = ctx.Database.SqlQuery<ConsumptionGlobal>("Select distinct a.Fecha, a.Nufactura, a.Sufijo, a.Total_fac, a.Docid_pagador, b.accion, b.nombre from PartnerConsumption a inner join ClubPartner b on a.Docid_pagador = b.docid order by b.nombre, Fecha, Sufijo, NuFactura").ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return oPartnerConsumption;
        }

        public List<ConsumptionGlobal> GetPartnerConsumptionList2M()
        {

            List<ConsumptionGlobal> oPartnerConsumption = new List<ConsumptionGlobal>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    oPartnerConsumption = ctx.Database.SqlQuery<ConsumptionGlobal>("Select distinct a.Fecha, a.Nufactura, a.Sufijo, a.Total_fac, a.Docid_pagador, b.accion, b.nombre from PartnerConsumption a inner join ClubPartner b on a.Docid_pagador = b.docid where datediff(m,a.fecha, getdate()) <= 2 order by Fecha desc,b.nombre, Sufijo, NuFactura").ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return oPartnerConsumption;
        }

        /// <summary>
        /// Retrieve course information based in the primary key
        /// </summary>
        /// <param name="partnerConsumptionTarget"></param>
        /// <returns></returns>
        public PartnerConsumption GetPartnerConsumptionbyKey(PartnerConsumption partnerConsumptionTarget)
        {
            PartnerConsumption oPartnerConsumption = new PartnerConsumption();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oPartnerConsumption =
                        ctx.PartnerConsumption.Where(x =>
                            x.Sufijo == partnerConsumptionTarget.Sufijo &&
                            x.Nufactura == partnerConsumptionTarget.Nufactura &&
                            x.Producto == partnerConsumptionTarget.Producto &&
                            x.Propina == partnerConsumptionTarget.Propina
                            ).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oPartnerConsumption;
        }

        /// <summary>
        /// Retrieve course information based in the invoice
        /// </summary>
        /// <param name="partnerConsumptionTarget"></param>
        /// <returns></returns>
        public List<PartnerConsumption> GetPartnerConsumptionbyInvoice(PartnerConsumption partnerConsumptionTarget)
        {
            List<PartnerConsumption> oPartnerConsumptionList = new List<PartnerConsumption>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oPartnerConsumptionList =
                        ctx.PartnerConsumption.Where(x => x.Nufactura == partnerConsumptionTarget.Nufactura && x.Sufijo == partnerConsumptionTarget.Sufijo).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return oPartnerConsumptionList;
        }

        /// <summary>
        /// Retrieve course information based in the partner
        /// </summary>
        /// <param name="partnerConsumptionTarget"></param>
        /// <returns></returns>
        public List<ConsumptionResume> GetPartnerConsumptionbyPartner(ClubPartner clubPartnerTarget)
        {
            BizClubPartner bizClubPärtner = new BizClubPartner();
            List<ConsumptionResume> oPartnerConsumption = new List<ConsumptionResume>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    ClubPartner oPartner = ctx.ClubPartner.Where(x => x.accion == clubPartnerTarget.accion && x.rel_tit == "T").FirstOrDefault();

                    oPartnerConsumption = ctx.Database.SqlQuery<ConsumptionResume>("Select distinct Fecha, Nufactura, Sufijo, Total_fac from PartnerConsumption where Docid_pagador = @p0 order by Sufijo, NuFactura", oPartner.docid).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return oPartnerConsumption;
        }

        /// <summary>
        /// Create or update a course
        /// </summary>
        /// <param name="partnerConsumption"></param>
        public void SavePartnerConsumption(PartnerConsumption partnerConsumption)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.PartnerConsumption.Add(partnerConsumption);
                    ctx.SaveChanges();
                }

            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex) { throw ex; }
        }

        public void SaveAsyncPartnerConsumption(List<PartnerConsumption> partnerConsumptions)
        {

            try
            {

                Parallel.ForEach(partnerConsumptions, (partnerConsumption) =>
                                {                                   
                                    SavePartnerConsumption(partnerConsumption);
                                });

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="partnerConsumptionTarget"></param>
        public void DeletePartnerConsumption(PartnerConsumption partnerConsumptionTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    PartnerConsumption oPartnerConsumption = GetPartnerConsumptionbyKey(partnerConsumptionTarget);

                    if (oPartnerConsumption != null)
                    {
                        // if exists then edit 
                        ctx.PartnerConsumption.Attach(oPartnerConsumption);
                        ctx.PartnerConsumption.Remove(oPartnerConsumption);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
