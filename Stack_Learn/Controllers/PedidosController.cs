using Stack_Learn.Context;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Stack_Learn.Areas.Seguranca.Models;
using Stack_Learn.Infraestrutura;

namespace Stack_Learn.Controllers
{
    public class PedidosController : Controller
    {
        private EFContext context = new EFContext();
        private GerenciadorUsuario GerenciadorUsuario
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<GerenciadorUsuario>();
            }
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Index()
        {
            return View(context.Pedidos.Include(a=>a.Aluno).OrderBy(c => c.PedidoId));
        }
        [Authorize(Roles = "ADM")]
        public ActionResult IndexCanalha()
        {
            return View(context.Pedidos.Include(a => a.Aluno).OrderBy(c => c.PedidoId));
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Create()
        {
            ViewBag.AlunoId = new SelectList(context.Alunos.OrderBy(b => b.Nome), "AlunoId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pedido pedido)
        {
            context.Pedidos.Add(pedido);
            context.SaveChanges();
            return RedirectToAction("IndexCanalha");
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = context.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlunoId = new SelectList(context.Alunos.OrderBy(b => b.Nome), "AlunoId", "Nome", pedido.Aluno);
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                context.Entry(pedido).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pedido);
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = context.Pedidos.Where(f => f.PedidoId == id).Include(a=>a.Aluno).First();
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = context.Pedidos.Where(p => p.PedidoId == id).Include(c => c.Aluno).First();
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Pedido pedido = context.Pedidos.Find(id);
            context.Pedidos.Remove(pedido);
            context.SaveChanges();
            TempData["Message"] = "Pedidos " + pedido.PedidoId + " foi removido";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Aluno")]
        public ActionResult CarrrinhoCompra(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = context.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlunoId = new SelectList(context.Alunos.OrderBy(b => b.Nome), "AlunoId", "Nome", pedido.Aluno);
            Console.WriteLine(pedido.Cursos);
            pedido.Valor_Total = 0;
            if(pedido.Cursos.Count() > 0)
            {
                foreach (Curso curso in pedido.Cursos)
                {
                    pedido.Valor_Total = pedido.Valor_Total + curso.Preco;
                }
            }
            var CursosUsuarios = new CursosUsuarios();
            CursosUsuarios.AlunoId = pedido.AlunoId;
            CursosUsuarios.CursosUsuariosId = pedido.AlunoId;
            pedido.curso_usuario = CursosUsuarios;

            return View(pedido);
        }


        [HttpPost]
        public ActionResult CarrrinhoCompra(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                /*
                Aluno 1, pedido pago -> aula 1 false 1, aula 2 false 1, aula 3 false 1
                aulaDetails.AulaId = id.Value;
                aulaDetails.Ordem = aula.Ordem;
                aulaDetails.Titulo = aula.Titulo;
                aulaDetails.Duracao = aula.Duracao;
                aulaDetails.CursoId = aula.CursoId;
                aulaDetails.Curso = aula.Curso;
                aulaDetails.Curso.Categoria = aula.Curso.Categoria;
                */

                Pedido pedido2 = context.Pedidos.Find(pedido.PedidoId);
                pedido2.Pago = true;
                pedido2.Data_Pagamento = DateTime.Now;
                context.Entry(pedido2).State = EntityState.Modified;
                Conclusao conclusao_false = new Conclusao();
                foreach (var conclusao in context.Conclusoes.Include(c => c.Aluno))
                {
                    if (conclusao.Concluido == false && conclusao.AlunoId == pedido2.AlunoId)
                    {
                        conclusao_false = conclusao;
                    }
                }
                foreach (var curso in pedido2.Cursos)
                {
                    foreach (var aula in curso.Aulas)
                    {
                        Aula aula_adicionar = new Aula();
                        aula_adicionar = aula;
                        aula_adicionar.AlunoId = pedido2.AlunoId;
                        aula_adicionar.Conclusao = conclusao_false;
                        aula_adicionar.ConclusaoId = conclusao_false.ConclusaoId;
                        context.Aulas.Add(aula_adicionar);
                    }
                }
                pedido2.Cursos.Clear();

                context.SaveChanges();
                return RedirectToAction("../Home/PaginaInicial");
            }
            return View(pedido);
        }

        [HttpPost]
        public ActionResult ExcluirCurso(long item_CursoId)
        {
            Curso curso = context.Cursos.Where(i => i.CursoId == item_CursoId).First();
            var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
            Usuario user = GerenciadorUsuario.FindById(userid);
            Pedido pedido = context.Pedidos.Where(p => p.PedidoId == user.AlunoId).First();
            long id = pedido.PedidoId;
            pedido.Cursos.Remove(curso);
            context.SaveChanges();
            return RedirectToAction("CarrrinhoCompra", new { id = id });
        }
    }
}