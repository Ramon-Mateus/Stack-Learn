using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Stack_Learn.Context;
using Stack_Learn.Models;
using Modelos.Models;

namespace Stack_Learn.Controllers
{
    public class AulasController : Controller
    {

        private EFContext context = new EFContext();

        public ActionResult AulaAssistir()//get, pegar curso id
        {
            ViewBag.ID = Request.QueryString["idCurso"];
            long id = ViewBag.ID;
            //IQueryable<Aula> daquele_curso = context.Aulas.
            return View(context.Aulas.Where(p => p.CursoId == id).Include(c => c.Curso).Include(y => y.Conclusao).OrderBy(n => n.Titulo));
        }
        public ActionResult Index()
        {
            return View(context.Aulas.Include(c => c.Curso).Include(y => y.Conclusao).OrderBy(n => n.Titulo));
        }

        public ActionResult Create()
        {
            IEnumerable<SelectListItem> ConclusaoList = context.Conclusoes.AsEnumerable().Select(x => new SelectListItem
            {
                Value = x.ConclusaoId.ToString(),
                Text = string.Format("{0} {1}", x.Concluido, x.AlunoId)
            });
            ViewBag.CursoId = new SelectList(context.Cursos.OrderBy(b => b.Nome), "CursoId", "Nome");
            ViewBag.ConclusaoId = ConclusaoList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aula aula)
        {
            if (aula.ConclusaoId != 0)
            {
                aula.TrueFalse = context.Conclusoes.Where(a => a.ConclusaoId == aula.ConclusaoId).First().Concluido;//adicionar manualmente a situação do curso
            }
            
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
            ViewBag.ConclusaoId = new SelectList(context.Conclusoes, "ConclusaoId", "Concluido", aula.ConclusaoId);
            return View(aula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Aula aula)
        {
            if (ModelState.IsValid)
            {
                aula.TrueFalse = context.Conclusoes.Where(a => a.ConclusaoId == aula.ConclusaoId).First().Concluido;//adicionar manualmente a situação do curso
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
            Aula aula = context.Aulas.Where(p => p.AulaId == id).Include(c => c.Curso).Include(y => y.Conclusao).First();
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
            Aula aula = context.Aulas.Where(p => p.AulaId == id).Include(c => c.Curso).Include(y => y.Conclusao).First();
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
        public ActionResult AulaIndividual(long? id)//get
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aula aula = context.Aulas.Where(p => p.AulaId == id).Include(c => c.Curso).First();
            foreach (var item in context.Categorias)
            {
                if (aula.Curso.CategoriaId == item.CategoriaId)
                {
                    aula.Curso.Categoria = item;
                }
            }
            if (aula == null)
            {
                return HttpNotFound();
            }
            var TodasAulas = from c in context.Aulas select new { c.AulaId, c.Titulo, c.Ordem, c.CursoId, c.TrueFalse };
            var aulaDetails = new AulaDetails();
            aulaDetails.AulaId = id.Value;
            aulaDetails.Ordem = aula.Ordem;
            aulaDetails.Titulo = aula.Titulo;
            aulaDetails.Duracao = aula.Duracao;
            aulaDetails.CursoId = aula.CursoId;
            aulaDetails.Curso = aula.Curso;
            aulaDetails.Curso.Categoria = aula.Curso.Categoria;
            aulaDetails.TrueFalse = aula.TrueFalse;
            var ListaAulas = new List<Aula>();
            foreach (var item in TodasAulas)
            {
                if (item.CursoId == aula.CursoId)
                {
                    ListaAulas.Add(new Aula { AulaId = item.AulaId, Titulo = item.Titulo, Ordem = item.Ordem, CursoId = item.CursoId, TrueFalse=item.TrueFalse});
                }
            }
            aulaDetails.Aulas = ListaAulas;
            return View(aulaDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AulaIndividual(AulaDetails auladetails)
        {
            if (ModelState.IsValid)
            {
                Aula aula = context.Aulas.Where(p => p.AulaId == auladetails.AulaId).Include(c => c.Curso).First();//achou a aula correspondente
                
                aula.TrueFalse = auladetails.TrueFalse;
                
                context.SaveChanges();
                return RedirectToAction("MeusCursosIndex", "Cursos");
            }
            return View(auladetails);
        }
    }
}
