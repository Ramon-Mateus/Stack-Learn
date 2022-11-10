using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Stack_Learn.Context;
using Modelos.Models;

namespace Stack_Learn.Controllers
{
    public class ProfessoresController : Controller
    {

        private EFContext context = new EFContext();

        
        public ActionResult Index()
        {
            return View(context.Professores.OrderBy(c => c.Nome));
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Professor professor)
        {
            context.Professores.Add(professor);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(Professor professor)
        {
            context.Professores.Add(professor);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = context.Professores.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Professor professor)
        {
            if (ModelState.IsValid)
            {
                context.Entry(professor).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(professor);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = context.Professores.Where(f => f.ProfessorId == id).
           Include("Cursos.Categoria").First();
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = context.Professores.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Professor professor = context.Professores.Find(id);
            context.Professores.Remove(professor);
            context.SaveChanges();
            TempData["Message"] = "Professor " + professor.Nome.ToUpper() + " foi removido";
            return RedirectToAction("Index");
        }
    }
}