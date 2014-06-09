using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orkidea.RinconCajica.Business;
using Orkidea.RinconCajica.Entities;
using Orkidea.RinconCajica.webFront.Models;

namespace Orkidea.RinconCajica.webFront.Controllers
{
    public class SportScheduleController : Controller
    {
        BizSport bizSport = new BizSport();
        BizSportBranch bizSportBranch = new BizSportBranch();
        //BizSportCategory bizSportCategory = new BizSportCategory();
        BizSportModality bizSportModality = new BizSportModality();
        BizSportSchedule bizSportSchedule = new BizSportSchedule();

        //
        // GET: /SportSchedule/
        [Authorize]
        public ActionResult Index()
        {
            List<SportSchedule> lsSportSchedule = bizSportSchedule.GetSportScheduleList();
            List<vmSportSchedule> lsCalendario = new List<vmSportSchedule>();

            List<SportModality> lsModalidad = bizSportModality.GetSportModalityList();
            //List<SportCategory> lsCategoria = bizSportCategory.GetSportCategoryList();
            List<SportBranch> lsRama = bizSportBranch.GetSportBranchList();
            List<Sport> lsDeporte = bizSport.GetSportList();

            foreach (SportSchedule item in lsSportSchedule)
            {
                lsCalendario.Add(new vmSportSchedule()
                {
                    id = item.id,
                    idDeporte = item.idDeporte,
                    inicio = item.inicio,
                    fin = item.fin,
                    competencia = item.competencia,
                    categoria = !string.IsNullOrEmpty(item.categoria) ? item.categoria : "",
                    idModalidad = item.idModalidad != null && item.idModalidad != 0 ? item.idModalidad : null,
                    idRama = item.idRama != null && item.idRama != 0 ? item.idRama : null,
                    aireacion = item.aireacion,
                    visible = item.visible,                    
                    nombreDeporte = lsDeporte.Where(x => x.id.Equals(item.idDeporte)).Select(x => x.nombre).FirstOrDefault(),
                    nombreModalidad = item.idModalidad != null && item.idModalidad != 0 ? lsModalidad.Where(x => x.id.Equals(item.idModalidad)).Select(x => x.nombre).FirstOrDefault() : null,
                    nombreRama = item.idRama != null && item.idRama != 0 ? lsRama.Where(x => x.id.Equals(item.idRama)).Select(x => x.nombre).FirstOrDefault() : null,
                });
            }
            ViewBag.menu = "SportSchedule";
            return View(lsCalendario);
        }

        //public ActionResult IndexSportSchedulePage()
        //{
        //    BizPage bizPage = new BizPage();

        //    //ViewBag.id = id;
        //    Page page = bizPage.GetPagebyKey(new Page() { id = "Deportes" });

        //    string sideBarContent = "";

        //    //if (page.idSideBar != null && page.idSideBar != 0)            
        //    //    sideBarContent = bizSideBar.GetSideBarByKey(new SideBar() { id = (int)page.idSideBar }).contenido;


        //    vmPage _vmPage = new vmPage()
        //    {
        //        id = page.id,
        //        contenidoPublico = page.contenidoPublico,
        //        contenidoPrivado = page.contenidoPrivado,
        //        idSideBar = page.idSideBar,
        //        titulo = page.titulo,
        //        sideBarContent = sideBarContent
        //    };
        //    return View(_vmPage);
        //}

        //public ActionResult IndexSportSchedule()
        //{
        //    List<SportSchedule> lsSportSchedule = bizSportSchedule.GetSportScheduleList();
        //    List<vmSportSchedule> lsCalendario = new List<vmSportSchedule>();

        //    List<SportModality> lsModalidad = bizSportModality.GetSportModalityList();
        //    //List<SportCategory> lsCategoria = bizSportCategory.GetSportCategoryList();
        //    List<SportBranch> lsRama = bizSportBranch.GetSportBranchList();
        //    List<Sport> lsDeporte = bizSport.GetSportList();

        //    foreach (SportSchedule item in lsSportSchedule)
        //    {
        //        lsCalendario.Add(new vmSportSchedule()
        //        {
        //            id = item.id,
        //            idDeporte = item.idDeporte,
        //            inicio = item.inicio,
        //            fin = item.fin,
        //            competencia = item.competencia,
        //            //idCategoria = item.idCategoria != null && item.idCategoria != 0 ? item.idCategoria : null,
        //            idModalidad = item.idModalidad != null && item.idModalidad != 0 ? item.idModalidad : null,
        //            idRama = item.idRama != null && item.idRama != 0 ? item.idRama : null,
        //            aireacion = item.aireacion,
        //            visible = item.visible,
        //            //nombreCategoria = item.idCategoria != null && item.idCategoria != 0 ? lsCategoria.Where(x => x.id.Equals(item.idCategoria)).Select(x => x.nombre).FirstOrDefault() : null,
        //            nombreDeporte = lsDeporte.Where(x => x.id.Equals(item.idDeporte)).Select(x => x.nombre).FirstOrDefault(),
        //            nombreModalidad = item.idModalidad != null && item.idModalidad != 0 ? lsModalidad.Where(x => x.id.Equals(item.idModalidad)).Select(x => x.nombre).FirstOrDefault() : null,
        //            nombreRama = item.idRama != null && item.idRama != 0 ? lsRama.Where(x => x.id.Equals(item.idRama)).Select(x => x.nombre).FirstOrDefault() : null,
        //            poster = item.poster
        //        });
        //    }

        //    ViewBag.menu = "Deportes";

        //    return View(lsCalendario);
        //}

        public ActionResult IndexNextSchedule(string id)
        {
            ViewBag.menu = "Deportes";
            ViewBag.titulo = id;

            int sport = bizSport.GetSportList().Where(x => x.nombre.ToUpper() == id.ToUpper()).Select(x => x.id).FirstOrDefault();

            List<SportSchedule> lsSportSchedule = bizSportSchedule.GetSportScheduleList().Where(x => x.idDeporte.Equals(sport) && x.inicio >= DateTime.Now && x.visible).ToList();
            List<vmSportSchedule> lsCalendario = new List<vmSportSchedule>();

            List<SportModality> lsModalidad = bizSportModality.GetSportModalityList();
            //List<SportCategory> lsCategoria = bizSportCategory.GetSportCategoryList();
            List<SportBranch> lsRama = bizSportBranch.GetSportBranchList();
            List<Sport> lsDeporte = bizSport.GetSportList();

            foreach (SportSchedule item in lsSportSchedule)
            {
                lsCalendario.Add(new vmSportSchedule()
                {
                    id = item.id,
                    idDeporte = item.idDeporte,
                    inicio = item.inicio,
                    fin = item.fin,
                    competencia = item.competencia,
                    //idCategoria = item.idCategoria != null && item.idCategoria != 0 ? item.idCategoria : null,
                    idModalidad = item.idModalidad != null && item.idModalidad != 0 ? item.idModalidad : null,
                    idRama = item.idRama != null && item.idRama != 0 ? item.idRama : null,
                    aireacion = item.aireacion,
                    visible = item.visible,
                    //nombreCategoria = item.idCategoria != null && item.idCategoria != 0 ? lsCategoria.Where(x => x.id.Equals(item.idCategoria)).Select(x => x.nombre).FirstOrDefault() : null,
                    nombreDeporte = lsDeporte.Where(x => x.id.Equals(item.idDeporte)).Select(x => x.nombre).FirstOrDefault(),
                    nombreModalidad = item.idModalidad != null && item.idModalidad != 0 ? lsModalidad.Where(x => x.id.Equals(item.idModalidad)).Select(x => x.nombre).FirstOrDefault() : null,
                    nombreRama = item.idRama != null && item.idRama != 0 ? lsRama.Where(x => x.id.Equals(item.idRama)).Select(x => x.nombre).FirstOrDefault() : null,
                    poster = item.poster
                });
            }

            return View(lsCalendario);
        }


        //
        // GET: /SportSchedule/Create
        [Authorize]
        public ActionResult Create()
        {
            List<SportModality> lsModalidad = new List<SportModality>();
            //List<SportCategory> lsCategoria = new List<SportCategory>();
            List<SportBranch> lsRama = new List<SportBranch>();
            List<Sport> lsDeporte = new List<Sport>();

            lsModalidad.Add(new SportModality());
            //lsCategoria.Add(new SportCategory());
            lsRama.Add(new SportBranch());

            lsModalidad.AddRange(bizSportModality.GetSportModalityList());
            //lsCategoria.AddRange(bizSportCategory.GetSportCategoryList());
            lsRama.AddRange(bizSportBranch.GetSportBranchList());
            lsDeporte.AddRange(bizSport.GetSportList());


            vmSportSchedule nuevoEvento = new vmSportSchedule()
            {
                //lsCategorias = lsCategoria,
                lsDeportes = lsDeporte,
                lsModalidades = lsModalidad,
                lsRamas = lsRama
            };
            ViewBag.menu = "SportSchedule";
            return View(nuevoEvento);
        }

        //
        // POST: /SportSchedule/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(vmSportSchedule nuevoEvento)
        {
            try
            {
                // TODO: Add insert logic here

                SportSchedule Evento = new SportSchedule()
                {
                    //idCategoria = !nuevoEvento.aireacion ? nuevoEvento.idCategoria : null,
                    idDeporte = nuevoEvento.idDeporte,
                    idModalidad = !nuevoEvento.aireacion ? nuevoEvento.idModalidad : null,
                    idRama = !nuevoEvento.aireacion ? nuevoEvento.idRama : null,
                    inicio = nuevoEvento.inicio,
                    fin = nuevoEvento.fin,
                    aireacion = nuevoEvento.aireacion,
                    competencia = nuevoEvento.competencia,
                    poster = nuevoEvento.poster,
                    visible = nuevoEvento.visible
                };

                bizSportSchedule.SaveSportSchedule(Evento);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportSchedule/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            List<SportModality> lsModalidad = new List<SportModality>();
            //List<SportCategory> lsCategoria = new List<SportCategory>();
            List<SportBranch> lsRama = new List<SportBranch>();
            List<Sport> lsDeporte = new List<Sport>();

            lsModalidad.Add(new SportModality());
            //lsCategoria.Add(new SportCategory());
            lsRama.Add(new SportBranch());

            lsModalidad.AddRange(bizSportModality.GetSportModalityList());
            //lsCategoria.AddRange(bizSportCategory.GetSportCategoryList());
            lsRama.AddRange(bizSportBranch.GetSportBranchList());
            lsDeporte.AddRange(bizSport.GetSportList());

            SportSchedule evento = bizSportSchedule.GetSportSchedulebyKey(new SportSchedule() { id = id });
            vmSportSchedule sportSchedule = new vmSportSchedule()
            {
                id = evento.id,
                categoria = evento.categoria,
                idDeporte = evento.idDeporte,
                idModalidad = evento.idModalidad,
                idRama = evento.idRama,
                competencia = evento.competencia,
                aireacion = evento.aireacion,
                inicio = evento.inicio,
                fin = evento.fin,
                //nombreCategoria = evento.idCategoria != null && evento.idCategoria != 0 ? lsCategoria.Where(x => x.id.Equals(evento.idCategoria)).Select(x => x.nombre).FirstOrDefault() : null,
                nombreDeporte = lsDeporte.Where(x => x.id.Equals(evento.idDeporte)).Select(x => x.nombre).FirstOrDefault(),
                nombreModalidad = evento.idModalidad != null && evento.idModalidad != 0 ? lsModalidad.Where(x => x.id.Equals(evento.idModalidad)).Select(x => x.nombre).FirstOrDefault() : null,
                nombreRama = evento.idRama != null && evento.idRama != 0 ? lsRama.Where(x => x.id.Equals(evento.idRama)).Select(x => x.nombre).FirstOrDefault() : null,
                poster = evento.poster,
                //lsCategorias = lsCategoria,
                lsDeportes = lsDeporte,
                lsModalidades = lsModalidad,
                lsRamas = lsRama,
                visible = evento.visible
            };
            ViewBag.menu = "SportSchedule";
            return View(sportSchedule);
        }

        //
        // POST: /SportSchedule/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, vmSportSchedule evento)
        {
            try
            {
                // TODO: Add insert logic here

                SportSchedule Evento = new SportSchedule()
                {
                    id = id,
                    categoria = !evento.aireacion ? evento.categoria : "",
                    idDeporte = evento.idDeporte,
                    idModalidad = !evento.aireacion ? evento.idModalidad : null,
                    idRama = !evento.aireacion ? evento.idRama : null,
                    inicio = evento.inicio,
                    fin = evento.fin,
                    aireacion = evento.aireacion,
                    competencia = evento.competencia,
                    visible = evento.visible,
                    poster = evento.poster
                };

                bizSportSchedule.SaveSportSchedule(Evento);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SportSchedule/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                bizSportSchedule.DeleteSportSchedule(new SportSchedule() { id = id });
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}
