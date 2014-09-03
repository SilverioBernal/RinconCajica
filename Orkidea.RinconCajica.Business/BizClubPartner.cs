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
    public class BizClubPartner
    {
        /*CRUD ClubPartners*/

        /// <summary>
        /// Retrieve ClubPartners list without parameters
        /// </summary>
        /// <returns></returns>
        public List<ClubPartner> GetClubPartnerList()
        {

            List<ClubPartner> lstClubPartner = new List<ClubPartner>();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstClubPartner = ctx.ClubPartner.ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return lstClubPartner;
        }

        /// <summary>
        /// Retrieve ClubPartner information based in the primary key
        /// </summary>
        /// <param name="ClubPartnerTarget"></param>
        /// <returns></returns>
        public ClubPartner GetClubPartnerbyKey(ClubPartner ClubPartnerTarget)
        {
            ClubPartner oClubPartner = new ClubPartner();

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oClubPartner = ctx.ClubPartner.Where(x => x.docid.Equals(ClubPartnerTarget.docid)).FirstOrDefault();
                }
            }
            catch (Exception ex) { throw ex; }

            return oClubPartner;
        }

        public Partner GetClubPartnerbyUser(int user)
        {
            ClubPartner oClubPartner = new ClubPartner();
            Partner res;
            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oClubPartner = ctx.ClubPartner.Where(x => x.idUsuario == user).FirstOrDefault();

                    res = new Partner()
                    {
                        accion = oClubPartner.accion,
                        acta = oClubPartner.acta,
                        carnet = oClubPartner.carnet,
                        celular = oClubPartner.celular,
                        ciudad = oClubPartner.ciudad,
                        cod_clie = oClubPartner.cod_clie,
                        cod_est = oClubPartner.cod_est,
                        correo = oClubPartner.correo,
                        doccl = oClubPartner.doccl,
                        docid = oClubPartner.docid,
                        docto = oClubPartner.docto,
                        doreco = oClubPartner.doreco,
                        estado = oClubPartner.estado,
                        fec_nac = oClubPartner.fec_nac,
                        fecmatr = oClubPartner.fecmatr,
                        idUsuario = oClubPartner.idUsuario,
                        looker = oClubPartner.looker,
                        nombre = oClubPartner.nombre,
                        rel_tit = oClubPartner.rel_tit,
                        sexo = oClubPartner.sexo,
                        teleco = oClubPartner.teleco,
                        telefn = oClubPartner.telefn,
                        direcc = oClubPartner.direcc
                    };

                    //if (res.rel_tit == "T")
                    //{
                    //    res.lsGrupoFamiliar = GetClubPartnerFamylyGroup(oClubPartner);
                    //}
                }
            }
            catch (Exception ex) { throw ex; }

            return res;
        }

        public Partner GetPartnerbyKey(string idPartner)
        {
            ClubPartner oClubPartner = new ClubPartner();
            Partner res;

            int user = int.Parse(idPartner);

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    oClubPartner = ctx.ClubPartner.Where(x => x.docid == user).FirstOrDefault();

                    res = new Partner()
                    {
                        accion = oClubPartner.accion,
                        acta = oClubPartner.acta,
                        carnet = oClubPartner.carnet,
                        celular = oClubPartner.celular,
                        ciudad = oClubPartner.ciudad,
                        cod_clie = oClubPartner.cod_clie,
                        cod_est = oClubPartner.cod_est,
                        correo = oClubPartner.correo,
                        doccl = oClubPartner.doccl,
                        docid = oClubPartner.docid,
                        docto = oClubPartner.docto,
                        doreco = oClubPartner.doreco,
                        estado = oClubPartner.estado,
                        fec_nac = oClubPartner.fec_nac,
                        fecmatr = oClubPartner.fecmatr,
                        idUsuario = oClubPartner.idUsuario,
                        looker = oClubPartner.looker,
                        nombre = oClubPartner.nombre,
                        rel_tit = oClubPartner.rel_tit,
                        sexo = oClubPartner.sexo,
                        teleco = oClubPartner.teleco,
                        telefn = oClubPartner.telefn,
                        direcc = oClubPartner.direcc
                    };

                    if (res.rel_tit == "T")
                    {
                        res.lsGrupoFamiliar = GetClubPartnerFamylyGroup(new ClubPartner { docid = res.docid });
                    }
                }
            }
            catch (Exception ex) { throw ex; }

            return res;
        }

        public Partner GetPartnerbyKey(string idPartner, int idTitular)
        {
            ClubPartner partnerTarget = new ClubPartner();
            Partner titularPartner = GetPartnerbyKey(idTitular.ToString());
            Partner res;

            int user = int.Parse(idPartner);

            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    partnerTarget = ctx.ClubPartner.Where(x => x.docid == user).FirstOrDefault();

                    res = new Partner()
                    {
                        accion = partnerTarget.accion,
                        acta = partnerTarget.acta,
                        carnet = partnerTarget.carnet,
                        celular = partnerTarget.celular,
                        ciudad = partnerTarget.ciudad,
                        cod_clie = partnerTarget.cod_clie,
                        cod_est = partnerTarget.cod_est,
                        correo = partnerTarget.correo,
                        doccl = partnerTarget.doccl,
                        docid = partnerTarget.docid,
                        docto = partnerTarget.docto,
                        doreco = partnerTarget.doreco,
                        estado = partnerTarget.estado,
                        fec_nac = partnerTarget.fec_nac,
                        fecmatr = partnerTarget.fecmatr,
                        idUsuario = partnerTarget.idUsuario,
                        looker = partnerTarget.looker,
                        nombre = partnerTarget.nombre,
                        rel_tit = partnerTarget.rel_tit,
                        sexo = partnerTarget.sexo,
                        teleco = partnerTarget.teleco,
                        telefn = partnerTarget.telefn,
                        direcc = partnerTarget.direcc
                    };

                    if (titularPartner.rel_tit == "T")
                    {
                        res.lsGrupoFamiliar = GetClubPartnerFamylyGroup(new ClubPartner { cod_clie = titularPartner.cod_clie });
                    }
                }
            }
            catch (Exception ex) { throw ex; }

            return res;
        }

        private List<Partner> GetClubPartnerFamylyGroup(ClubPartner ClubPartnerTarget)
        {
            List<ClubPartner> lstClubPartner = new List<ClubPartner>();
            List<Partner> res = new List<Partner>();
            try
            {
                using (var ctx = new RinconEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    lstClubPartner = ctx.ClubPartner.Where(x => x.cod_clie == ClubPartnerTarget.cod_clie && x.rel_tit != "T").ToList();

                    foreach (ClubPartner item in lstClubPartner)
                    {
                        res.Add(new Partner()
                        {
                            accion = item.accion,
                            acta = item.acta,
                            carnet = item.carnet,
                            celular = item.celular,
                            ciudad = item.ciudad,
                            cod_clie = item.cod_clie,
                            cod_est = item.cod_est,
                            correo = item.correo,
                            doccl = item.doccl,
                            docid = item.docid,
                            docto = item.docto,
                            doreco = item.doreco,
                            estado = item.estado,
                            fec_nac = item.fec_nac,
                            fecmatr = item.fecmatr,
                            idUsuario = item.idUsuario,
                            looker = item.looker,
                            nombre = item.nombre,
                            rel_tit = item.fec_ret,
                            sexo = item.sexo,
                            teleco = item.teleco,
                            telefn = item.telefn,
                            direcc = item.direcc
                        });
                    }
                }
            }
            catch (Exception ex) { throw ex; }

            return res;
        }

        /// <summary>
        /// Create or update a ClubPartner
        /// </summary>
        /// <param name="ClubPartnerTarget"></param>
        public void SaveClubPartner(Partner ClubPartnerTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the ClubPartner exists
                    ClubPartner modifiedClubPartner = GetClubPartnerbyKey(new ClubPartner() { docid = ClubPartnerTarget.docid });
                    ClubPartner originalClubPartner = GetClubPartnerbyKey(new ClubPartner() { docid = ClubPartnerTarget.docid });

                    modifiedClubPartner.doreco = ClubPartnerTarget.doreco;
                    modifiedClubPartner.teleco = ClubPartnerTarget.teleco;
                    modifiedClubPartner.telefn = ClubPartnerTarget.telefn;
                    modifiedClubPartner.fec_nac = ClubPartnerTarget.fec_nac;
                    modifiedClubPartner.fecmatr = ClubPartnerTarget.fecmatr;
                    modifiedClubPartner.ciudad = ClubPartnerTarget.ciudad;
                    modifiedClubPartner.correo = ClubPartnerTarget.correo;
                    modifiedClubPartner.celular = ClubPartnerTarget.celular;
                    modifiedClubPartner.direcc = ClubPartnerTarget.direcc;
                    modifiedClubPartner.fechaActualizacion = ClubPartnerTarget.fechaActualizacion;



                    if (modifiedClubPartner != null)
                    {
                        // if exists then edit 
                        ctx.ClubPartner.Attach(originalClubPartner);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(originalClubPartner, modifiedClubPartner);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.ClubPartner.Add(modifiedClubPartner);
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


        public void SaveClubPartner(ClubPartner ClubPartnerTarget)
        {

            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the ClubPartner exists
                    ClubPartner originalClubPartner = GetClubPartnerbyKey(ClubPartnerTarget);


                    if (originalClubPartner != null)
                    {
                        // if exists then edit 
                        ctx.ClubPartner.Attach(originalClubPartner);
                        EntityFrameworkHelper.EnumeratePropertyDifferences(originalClubPartner, ClubPartnerTarget);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        // else create
                        ctx.ClubPartner.Add(ClubPartnerTarget);
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
        /// Delete a ClubPartner
        /// </summary>
        /// <param name="ClubPartnerTarget"></param>
        public void DeleteClubPartner(ClubPartner ClubPartnerTarget)
        {
            try
            {
                using (var ctx = new RinconEntities())
                {
                    //verify if the school exists
                    ClubPartner oClubPartner = GetClubPartnerbyKey(ClubPartnerTarget);

                    if (oClubPartner != null)
                    {
                        // if exists then edit 
                        ctx.ClubPartner.Attach(oClubPartner);
                        ctx.ClubPartner.Remove(oClubPartner);
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
