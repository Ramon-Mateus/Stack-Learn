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
    public class PedidosController : Controller
    {
        private EFContext context = new EFContext();


        public ActionResult Index()
        {
            return View(context.Pedidos.Include(a=>a.Aluno).OrderBy(c => c.PedidoId));
        }

        public ActionResult IndexCanalha()
        {
            return View(context.Pedidos.Include(a => a.Aluno).OrderBy(c => c.PedidoId));
        }

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
        [ValidateAntiForgeryToken]
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
                pedido.Pago = true;
                pedido.Data_Pagamento = DateTime.Now;
                context.Entry(pedido).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("IndexCanalha");
            }
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirCurso(long item_CursoId)
        {
            Curso curso = context.Cursos.Where(i => i.CursoId == item_CursoId).First();
            Pedido pedido = context.Pedidos.Where(i => i.PedidoId == curso.PedidoId).First();
            long id = pedido.PedidoId;
            pedido.Cursos.Remove(curso);
            context.SaveChanges();
            return RedirectToAction("CarrrinhoCompra", new { id=id });
        }
    }
}