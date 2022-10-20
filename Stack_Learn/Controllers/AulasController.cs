using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Stack_Learn.Context;
using Stack_Learn.Models;

namespace Stack_Learn.Controllers
{
    public class AulasController : Controller
    {

        private EFContext context = new EFContext();


        public ActionResult Index()
        {
            return View(context.Aulas.OrderBy(n => n.Titulo));
        }
        
        public ActionResult Create()
        {
            ViewBag.CursoId = new SelectList(context.Cursos.OrderBy(b => b.Nome), "CursoId", "Nome");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aula aula)
        {
            context.Aulas.Add(aula);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aula aula = context.Aulas.Find(id);
            if (aula == null)
            {
                return HttpNotFound();
            }
            ViewBag.CursoId = new SelectList(context.Cursos.OrderBy(b => b.Nome), "CursoId", "Nome", aula.CursoId);
            return View(aula);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Aula aula)
        {
            if (ModelState.IsValid)
            {
                context.Entry(aula).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aula);
        }
        
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aula aula = context.Aulas.Where(p => p.AulaId == id).Include(c => c.Curso).First();
            if (aula == null)
            {
                return HttpNotFound();
            }
            return View(aula);
        }
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aula aula = context.Aulas.Where(p => p.AulaId == id).Include(c => c.Curso).First();
            if (aula == null)
            {
                return HttpNotFound();
            }
            return View(aula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Aula aula = context.Aulas.Find(id);
            context.Aulas.Remove(aula);
            context.SaveChanges();
            TempData["Message"] = "Aula " + aula.Titulo.ToUpper() + " foi removido";
            return RedirectToAction("Index");
        }
    }
}
