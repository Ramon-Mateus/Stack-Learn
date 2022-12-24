using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Stack_Learn.Context;
using Modelos.Models;
using System.IO;
using Stack_Learn.Models;

namespace Stack_Learn.Controllers
{
    public class CursosController : Controller
    {

        private EFContext context = new EFContext();

        private void PopularViewBag(Curso curso = null)
        {
            if (curso == null)
            {
                ViewBag.CategoriaId = new SelectList(context.Categorias.OrderBy(b => b.Nome), "CategoriaId", "Nome", curso.CategoriaId);
                ViewBag.ProfessorId = new SelectList(context.Professores.OrderBy(b => b.Nome), "ProfessorId", "Nome", curso.ProfessorId);
            }
            else
            {
                ViewBag.CategoriaId = new SelectList(context.Categorias.OrderBy(b => b.Nome), "CategoriaId", "Nome", curso.CategoriaId);
                ViewBag.ProfessorId = new SelectList(context.Professores.OrderBy(b => b.Nome), "ProfessorId", "Nome", curso.ProfessorId);
            }


        }
        private byte[] SetLogotipo(HttpPostedFileBase logotipo)
        {
            var bytesLogotipo = new byte[logotipo.ContentLength];
            logotipo.InputStream.Read(bytesLogotipo, 0, logotipo.ContentLength);
            return bytesLogotipo;
        }
        public FileContentResult GetLogotipo(long id)
        {
            Curso curso = context.Cursos.Where(p => p.CursoId == id).Include(c => c.Categoria).Include(f => f.Professor).First();
            if (curso != null)
            {
                if (curso.NomeArquivo != null)
                {
                    var bytesLogotipo = new byte[curso.TamanhoArquivo];
                    FileStream fileStream = new FileStream(Server.MapPath("~/App_Data/" + curso.NomeArquivo), FileMode.Open, FileAccess.Read);
                    fileStream.Read(bytesLogotipo, 0, (int)curso.TamanhoArquivo);
                    return File(bytesLogotipo, curso.LogotipoMimeType);
                }
            }
            return null;
        }
        public ActionResult DownloadArquivo(long id)
        {

            Curso curso = context.Cursos.Where(p => p.CursoId == id).Include(c => c.Categoria).Include(f => f.Professor).First();
            FileStream fileStream = new FileStream(Server.MapPath("~/App_Data/" + curso.NomeArquivo), FileMode.Open, FileAccess.Read);
            return File(fileStream.Name, curso.LogotipoMimeType, curso.NomeArquivo);

        }
        private ActionResult GravarCurso(Curso curso, HttpPostedFileBase logotipo, string chkRemoverImagem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (chkRemoverImagem != null)
                    {
                        curso.Logotipo = null;
                    }
                    if (logotipo != null)
                    {
                        curso.LogotipoMimeType = logotipo.ContentType;
                        curso.Logotipo = SetLogotipo(logotipo);
                        curso.NomeArquivo = logotipo.FileName;
                        curso.TamanhoArquivo = logotipo.ContentLength;

                        string strFileName = Server.MapPath("~/App_Data/") + Path.GetFileName(logotipo.FileName);
                        logotipo.SaveAs(strFileName);
                    }
                    if (curso.CursoId == null)
                    {
                        context.Cursos.Add(curso);
                    }
                    else
                    {
                        context.Entry(curso).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                PopularViewBag(curso);
                return View(curso);
            }
            catch
            {
                PopularViewBag(curso);
                return View(curso);
            }
        }

        public ActionResult Index()
        {
            return View(context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome));
        }
        public ActionResult Lista()//get, sem post
        {
            return View(context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome));
        }
        public ActionResult MeusCursosIndex()
        {
            List<Curso> concluidoo = new List<Curso>();
            List<Curso> emm_andamento = new List<Curso>();

            IQueryable<Curso> todos = context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome);

            foreach (Curso item in todos)
            {
                int concluidos = 0;
                int percorrer_list = item.Qtd_Aulas ;//10
                int list_final = 0;

                foreach(Aula subitem in item.Aulas)
                {
                    list_final++;
                    if (subitem.TrueFalse == true)
                    {
                        concluidos++;//1
                    }
                    if(list_final == percorrer_list - 1 || list_final == item.Aulas.Count)//tem todas as aulas para serem analisadas x se só tem uma
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
        public ActionResult Create()
        {
            //para quem comprar, vir com todos os cursos falso
            ViewBag.CategoriaId = new SelectList(context.Categorias.OrderBy(b => b.Nome),"CategoriaId", "Nome");
            ViewBag.ProfessorId = new SelectList(context.Professores.OrderBy(b => b.Nome),"ProfessorId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Curso curso)
        {
            
            if (curso.Professor != null)
            {
                curso.NomeProfessor = curso.Professor.Nome;
            }
            else
            {
                curso.NomeProfessor = "Sem Professor";
            }
            curso.PedidoId = null;
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
        public ActionResult Edit(Curso curso, HttpPostedFileBase logotipo = null, string chkRemoverImagem = null)
        {
            return GravarCurso(curso, logotipo, chkRemoverImagem);
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
            CursoDetails objeto = new CursoDetails();
            objeto.curso = curso;
            objeto.cursos = context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome);
            return View(objeto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(long id)
        {
            Curso curso = context.Cursos.Find(id);
            Pedido pedido = context.Pedidos.Where(p => p.PedidoId == 1).First();
            curso.PedidoId = pedido.PedidoId;
            pedido.Cursos.Add(curso);
   
            context.SaveChanges();
            return RedirectToAction("Index");
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
