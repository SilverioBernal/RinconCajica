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
    public class ProjectController : Controller
    {
        BizProject projectBiz = new BizProject();

        //
        // GET: /Project/
        [Authorize]
        public ActionResult Index()
        {
            List<Project> lstProject = projectBiz.GetProjectList();
            return View(lstProject);
        }

        [Authorize]
        public ActionResult List()
        {
            List<Project> lstProject = projectBiz.GetProjectList();
            return View(lstProject);
        }

        //
        // GET: /Project/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        public ActionResult ProjectDocumentTypeList(int id)
        {
            BizProjectDocument pdBiz = new BizProjectDocument();

            List<DocumentType> pdList = pdBiz.GetProjectDocumentTypeListByProject(new Project() { id = id });
            ViewBag.idProject = id;
            return View(pdList);
        }

        //
        // GET: /Project/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Project/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Project project)
        {
            try
            {
                projectBiz.SaveProject(project);

                return RedirectToAction("List");
            }
            catch
            {
                vmProject oProject = new vmProject() { nombre = project.nombre };
                return View(oProject);
            }
        }

        //
        // GET: /Project/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Project project = projectBiz.GetProjectbyKey(new Project() { id = id });
            vmProject oProject = new vmProject() { nombre = project.nombre };
            return View(oProject);
        }

        //
        // POST: /Project/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, Project project)
        {
            try
            {
                project.id = id;
                projectBiz.SaveProject(project);

                return RedirectToAction("List");
            }
            catch
            {
                vmProject oProject = new vmProject() { nombre = project.nombre };
                return View(oProject);
            }
        }

        //
        // GET: /Project/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            projectBiz.DeleteProject(new Project() { id = id });
            return RedirectToAction("List");
        }

        //
        // POST: /Project/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
