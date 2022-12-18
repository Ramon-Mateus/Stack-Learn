using Stack_Learn.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Modelos.Models;
using System.IO;
using Stack_Learn.Models;

namespace Stack_Learn.Controllers
{
    public class ConclusoesController : Controller
    {
        private EFContext context = new EFContext();

        
        public ActionResult Index()
        {
            return View(context.Conclusoes.Include(c => c.Aluno));
        }

        
        public ActionResult Create()
        {
            ViewBag.AlunoId = new SelectList(context.Alunos.OrderBy(b => b.Nome), "AlunoId", "Nome");
            //ViewBag.AulaId = new SelectList(context.Aulas, "AulaId", "ordem");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Conclusao conclusao)
        {
            context.Conclusoes.Add(conclusao);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conclusao conclusao = context.Conclusoes.Find(id);
            if (conclusao == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlunoId = new SelectList(context.Alunos.OrderBy(b => b.Nome), "AlunoId", "Nome", conclusao.Aluno);
            //ViewBag.AulaId = new SelectList(context.Aulas, "AulaId", "ordem", conclusao.Aula);
            return View(conclusao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Conclusao conclusao)
        {
            if (ModelState.IsValid)
            {
                context.Entry(conclusao).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(conclusao);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conclusao conclusao = context.Conclusoes.Where(p => p.ConclusaoId == id).Include(c => c.Aluno).First();
            if (conclusao == null)
            {
                return HttpNotFound();
            }
            return View(conclusao);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conclusao conclusao = context.Conclusoes.Where(p => p.ConclusaoId == id).Include(c => c.Aluno).First();
            if (conclusao == null)
            {
                return HttpNotFound();
            }
            return View(conclusao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Conclusao conclusao = context.Conclusoes.Find(id);
            context.Conclusoes.Remove(conclusao);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}