using Stack_Learn.Context;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Stack_Learn.Controllers
{
    public class AvaliacoesController : Controller
    {
        private EFContext context = new EFContext();


        public ActionResult Index()
        {
            return View(context.Avaliacoes.Include(c => c.Curso).Include(a=>a.Aluno).OrderBy(c => c.Nota));
        }

        public ActionResult Create()
        {

            ViewBag.IdCurso = Request.QueryString["CursoId"];
            ViewBag.IdAluno = Request.QueryString["AlunoId"];

            ViewBag.NomeCurso = context.Cursos.OrderBy(b => b.Nome);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Avaliacao avaliacao)
        {

            /*
            avaliacao.AlunoNome = context.Alunos.Where(a => a.AlunoId == avaliacao.AlunoId).First().Nome;//adicionar manualmente o nome do aluno, se editar não vai editar aqui também :/
            avaliacao.Curso.Nome = context.Cursos.Where(a => a.CursoId == avaliacao.CursoId).First().Nome;//adicionar manualmente o nome do curso, objetos vem nulos
            avaliacao.curso_usuario.AlunoId = avaliacao.AlunoId;
            
            if (ModelState.IsValid)
            {
                context.Entry(avaliacao).State = EntityState.Modified;
                avaliacao.AlunoNome = context.Alunos.Where(a => a.AlunoId == avaliacao.AlunoId).First().Nome;//adicionar manualmente o nome do aluno, se editar não vai editar aqui também :/
                avaliacao.Curso.Nome = context.Cursos.Where(a => a.CursoId == avaliacao.CursoId).First().Nome;//adicionar manualmente o nome do curso, objetos vem nulos
                avaliacao.curso_usuario.AlunoId = avaliacao.AlunoId;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            //se estiver vazio, volta de boa
            Avaliacao avaliacaooo = context.Avaliacoes.Find(avaliacao.AvaliacaoId);
            context.Avaliacoes.Remove(avaliacaooo);
            context.SaveChanges();
            return View("MeusCursosIndex");
            */
            avaliacao.AlunoNome = context.Alunos.Where(a => a.AlunoId == avaliacao.AlunoId).First().Nome;//adicionar manualmente o nome do aluno, se editar não vai editar aqui também :/
            avaliacao.Data_hora = DateTime.Now;
            context.Avaliacoes.Add(avaliacao);
            context.SaveChanges();
            return RedirectToAction("../Cursos/MeusCursosIndex/" + avaliacao.AlunoId);
        }


        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = context.Avaliacoes.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            ViewBag.CursoId = new SelectList(context.Cursos.OrderBy(b => b.Nome), "CursoId", "Nome", avaliacao.Curso);
            ViewBag.AlunoId = new SelectList(context.Alunos.OrderBy(b => b.Nome), "AlunoId", "Nome", avaliacao.Aluno);
            return View(avaliacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Avaliacao avaliacao)
        {
            if (ModelState.IsValid)
            {
                avaliacao.AlunoNome = context.Alunos.Where(a => a.AlunoId == avaliacao.AlunoId).First().Nome;//adicionar manualmente o nome do aluno
                context.Entry(avaliacao).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(avaliacao);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = context.Avaliacoes.Where(p => p.AvaliacaoId == id).Include(c => c.Curso).Include(f => f.Aluno).First();
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            return View(avaliacao);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = context.Avaliacoes.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            return View(avaliacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Avaliacao avaliacao = context.Avaliacoes.Find(id);
            context.Avaliacoes.Remove(avaliacao);
            context.SaveChanges();
            TempData["Message"] = "Avaliacao " + avaliacao.Nota + " foi removida";
            return RedirectToAction("Index");
        }
    }
}