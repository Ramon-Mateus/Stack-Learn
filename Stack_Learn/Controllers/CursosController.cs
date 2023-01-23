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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Stack_Learn.Areas.Seguranca.Models;
using Stack_Learn.Infraestrutura;

namespace Stack_Learn.Controllers
{
    public class CursosController : Controller
    {

        private EFContext context = new EFContext();

        private GerenciadorUsuario GerenciadorUsuario
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<GerenciadorUsuario>();
            }
        }

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
        [Authorize(Roles = "ADM")]
        public ActionResult Index()
        {
            return View(context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome));
        }
        public ActionResult Lista()//get, sem post
        {
            var Cursos_Usuarios = new CursosUsuarios();
            if (System.Web.HttpContext.Current.User.Identity.Name.ToString() != "")
            {
                var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
                Usuario user = GerenciadorUsuario.FindById(userid);
                Cursos_Usuarios.AlunoId = user.AlunoId;
            }
            var Cursos_totais = new List<Curso>();
            foreach (var item in context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome))
            {
                Cursos_totais.Add(item);
            }
            Cursos_Usuarios.Cursos = Cursos_totais;

            var Categorias_totais = new List<Categoria>();
            foreach (var item in context.Categorias.OrderBy(c => c.Nome))
            {
                Categorias_totais.Add(item);
            }
            Cursos_Usuarios.Categorias = Categorias_totais;
            return View(Cursos_Usuarios);
        }
        [Authorize(Roles = "Aluno")]
        public ActionResult MeusCursosIndex(long? id)
        {
            //aluno -> pedido ok -> curso ok
            long? AlunoId = id;//sempre o primeiro
            List<Curso> concluidoo = new List<Curso>();
            List<Curso> emm_andamento = new List<Curso>();

            IQueryable<Curso> todos = context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome);

            foreach (Curso item in todos)
            {
                int concluidos = 0;
                int percorrer_list = item.Qtd_Aulas ;//2
                int list_final = 0;//0 1 2 

                foreach(Aula subitem in item.Aulas)
                {
                    if(subitem.AlunoId == AlunoId)
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
            }

            MeusCursosIndex index = new MeusCursosIndex();
            index.em_andamento = emm_andamento;
            index.concluido = concluidoo;
            index.MeusCursosIndexId = AlunoId;
            return View(index);


       }
        [Authorize(Roles = "ADM")]
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
            context.Cursos.Add(curso);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "ADM")]
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
            
            var Cursos_Usuarios = new CursosUsuarios();
            if (System.Web.HttpContext.Current.User.Identity.Name.ToString() != "")
            {
                var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
                Usuario user = GerenciadorUsuario.FindById(userid);
                Cursos_Usuarios.AlunoId = user.AlunoId;
                objeto.curso.Aulas.Clear();
                foreach (var aula in context.Aulas.Where(a => a.CursoId == objeto.curso.CursoId))
                {
                    if (aula.AlunoId == user.AlunoId)
                    {
                        objeto.curso.Aulas.Add(aula);
                    }
                }
            }
            objeto.cursos_usuarios = Cursos_Usuarios;
            return View(objeto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(long id)
        {
            Curso curso = context.Cursos.Find(id);
            Professor professor = context.Professores.Find(curso.ProfessorId);
            curso.NomeProfessor = professor.Nome;
            if (System.Web.HttpContext.Current.User.Identity.Name.ToString() != "")
            {
                var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
                Usuario user = GerenciadorUsuario.FindById(userid);
                Pedido pedido = context.Pedidos.Where(p => p.PedidoId == user.AlunoId).First();
                pedido.Cursos.Add(curso);
                context.SaveChanges();
                return RedirectToAction("../Pedidos/CarrrinhoCompra/"+pedido.PedidoId);
            }
            else
            {
                return RedirectToAction("../Seguranca/Account/Login");
            }

            
   
            
        }
        [Authorize(Roles = "ADM")]
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
            return RedirectToAction("Index");
        }
    }
}
