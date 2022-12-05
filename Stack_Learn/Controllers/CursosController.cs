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
    public class CursosController : Controller
    {

        private EFContext context = new EFContext();


        public ActionResult Index()
        {
            return View(context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome));
        }
        public ActionResult Lista()
        {
            return View(context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome));
        }
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(context.Categorias.OrderBy(b => b.Nome),"CategoriaId", "Nome");
            ViewBag.ProfessorId = new SelectList(context.Professores.OrderBy(b => b.Nome),"ProfessorId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Curso curso)
        {
            context.Cursos.Add(curso);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = context.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(context.Categorias.OrderBy(b => b.Nome), "CategoriaId","Nome", curso.CategoriaId);
            ViewBag.ProfessorId = new SelectList(context.Professores.OrderBy(b => b.Nome), "ProfessorId","Nome", curso.ProfessorId);
            return View(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Curso curso)
        {
            if (ModelState.IsValid)
            {
                context.Entry(curso).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(curso);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = context.Cursos.Where(p => p.CursoId == id).Include(c => c.Categoria).Include(f => f.Professor).First();
            if (curso == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categoria = curso.CategoriaId;
            return View(curso);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = context.Cursos.Where(p => p.CursoId == id).Include(c => c.Categoria).Include(f => f.Professor).First();
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Curso curso = context.Cursos.Find(id);
            context.Cursos.Remove(curso);
            context.SaveChanges();
            TempData["Message"] = "Curso " + curso.Nome.ToUpper() + " foi removido";
            return RedirectToAction("Index");
        }
    }
}
