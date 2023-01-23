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
                aula.AlunoId = context.Conclusoes.Where(a => a.ConclusaoId == aula.ConclusaoId).First().AlunoId;//adicionar manualmente o alunoid do curso
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
            var TodasAulas = from c in context.Aulas select new { c.AulaId, c.Titulo, c.Ordem, c.CursoId, c.TrueFalse, c.ConclusaoId, c.Conclusao, c.AlunoId };
            var aulaDetails = new AulaDetails();
            aulaDetails.AulaId = id.Value;
            aulaDetails.Ordem = aula.Ordem;
            aulaDetails.Titulo = aula.Titulo;
            aulaDetails.Duracao = aula.Duracao;
            aulaDetails.CursoId = aula.CursoId;
            aulaDetails.Curso = aula.Curso;
            aulaDetails.Curso.Categoria = aula.Curso.Categoria;
            aulaDetails.Curso.Descricao = aula.Curso.Descricao;
            aulaDetails.TrueFalse = aula.TrueFalse;
            aulaDetails.Conclusao = aula.Conclusao;
            aulaDetails.ConclusaoId = aula.ConclusaoId;
            aulaDetails.AlunoId = aula.AlunoId;

            var CursosUsuarios = new CursosUsuarios();
            CursosUsuarios.AlunoId = id;
            CursosUsuarios.CursosUsuariosId = id;
            aulaDetails.curso_usuario = CursosUsuarios;

            var ListaAulas = new List<Aula>();
            foreach (var item in TodasAulas)
            {
                if (item.CursoId == aula.CursoId && item.AlunoId == aula.AlunoId)
                {
                    ListaAulas.Add(new Aula { AulaId = item.AulaId, Titulo = item.Titulo, Ordem = item.Ordem, CursoId = item.CursoId, TrueFalse = item.TrueFalse, AlunoId = item.AlunoId });
                }
            }
            aulaDetails.Aulas = ListaAulas;
            return View(aulaDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AulaIndividual(AulaDetails auladetails)//post
        {
            if (ModelState.IsValid)
            {
                Aula aula = context.Aulas.Where(p => p.AulaId == auladetails.AulaId).Include(c => c.Curso).First();//achou a aula correspondente

                aula.TrueFalse = auladetails.TrueFalse;

                foreach (var item in context.Conclusoes)
                {
                    if (aula.ConclusaoId == item.ConclusaoId)
                    {
                        aula.Conclusao = item;
                    }
                }
                aula.ConclusaoId = context.Conclusoes.Where(c => c.AlunoId == aula.Conclusao.AlunoId && c.Concluido == aula.TrueFalse).First().ConclusaoId;
                aula.AlunoId = context.Conclusoes.Where(c => c.AlunoId == aula.Conclusao.AlunoId && c.Concluido == aula.TrueFalse).First().AlunoId;

                context.Entry(aula).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("MeusCursosIndex/" + aula.AlunoId, "Cursos");
            }
            return View(auladetails);
        }

        /*
        public ActionResult MeusCursosIndex(long? alunoid)
        {
            //aluno -> pedido ok -> curso ok
    
            List<Curso> concluidoo = new List<Curso>();
            List<Curso> emm_andamento = new List<Curso>();

            //IQueryable<Curso> todos = context.Cursos.Include.(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome);

            IQueryable<Curso> todos_cursos;
            IQueryable<Aula> todas_aulas = context.


            foreach (Aula item in contex)
            

            foreach (Curso item in todos)
            {
                int concluidos = 0;
                int percorrer_list = item.Qtd_Aulas;//2
                int list_final = 0;//0 1 2 

                foreach (Aula subitem in item.Aulas)
                {
                    list_final++;//1 2 3
                    if (subitem.TrueFalse == true)
                    {
                        concluidos++;//1
                    }
                    if (list_final == percorrer_list || list_final == item.Aulas.Count)//tem todas as aulas para serem analisadas x se só tem uma
                    {
                        if (concluidos < percorrer_list)
                        {
                            emm_andamento.Add(item);
                        }
                        if (concluidos == percorrer_list)
                        {
                            concluidoo.Add(item);
                        }
                    }
                }
            }
            MeusCursosIndex index = new MeusCursosIndex();
            index.em_andamento = emm_andamento;
            index.concluido = concluidoo;
            return View(index);


        }
        */
    }
}