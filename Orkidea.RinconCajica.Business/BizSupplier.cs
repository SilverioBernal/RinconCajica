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
    public class BizSupplier
    {
        /*CRUD Supplier*/

        /// <summary>
        /// Retrieve process list 
        /// </summary>
        /// <param name="schoolTarget"></param>
        /// <returns></returns>
        public List<Supplier> GetSupplierList()
        {

            List<Supplier> lstSupplier = new List<Supplier>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstSupplier = ctx.Supplier.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstSupplier;
        }

        /// <summary>
        /// Retrieve course information based in the primary key
        /// </summary>
        /// <param name="SupplierTarget"></param>
        /// <returns></returns>
        public Supplier GetSupplierbyKey(Supplier SupplierTarget)
        {
            Supplier oSupplier = new Supplier();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oSupplier =
                        ctx.Supplier.Where(x => x.id.Equals(SupplierTarget.id)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oSupplier;
        }

        /// <summary>
        /// Create or update a course
        /// </summary>
        /// <param name="Supplier"></param>
        public void SaveSupplier(Supplier Supplier)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the student exists
                    Supplier oSupplier = GetSupplierbyKey(Supplier);

                    if (oSupplier != null)
                    {
                        // if exists then edit 
                        ctx.Supplier.Attach(oSupplier);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(oSupplier, Supplier);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.Supplier.Add(Supplier);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="SupplierTarget"></param>
        public void DeleteSupplier(Supplier SupplierTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    Supplier oSupplier = GetSupplierbyKey(SupplierTarget);

                    if (oSupplier != null)
                    {
                        // if exists then edit 
                        ctx.Supplier.Attach(oSupplier);
                        ctx.Supplier.Remove(oSupplier);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
