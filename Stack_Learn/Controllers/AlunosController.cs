using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Stack_Learn.Context;
using Modelos.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Stack_Learn.Areas.Seguranca.Models;
using Stack_Learn.Infraestrutura;
using Stack_Learn.Models;

namespace Stack_Learn.Controllers
{
    public class AlunosController : Controller
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
            return View(context.Alunos.OrderBy(c => c.Nome));
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aluno aluno)
        {
            context.Alunos.Add(aluno);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Aluno")]
        public ActionResult Edit(long? id)
        {
            var CursosUsuarios = new CursosUsuarios();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aluno aluno = context.Alunos.Find(id);
            CursosUsuarios.AlunoId = id;
            CursosUsuarios.CursosUsuariosId = id;
            aluno.curso_usuario = CursosUsuarios;
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                context.Entry(aluno).State = EntityState.Modified;
                Usuario usuario = GerenciadorUsuario.FindById(aluno.Id_do_usuario);
                usuario.UserName = aluno.Login;
                usuario.Email = aluno.Email;
                usuario.PasswordHash = GerenciadorUsuario.PasswordHasher.HashPassword(aluno.Senha);
                GerenciadorUsuario.Update(usuario);
                context.SaveChanges();
                return RedirectToAction("../Home/PaginaInicial");
            }
            return View(aluno);
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aluno aluno = context.Alunos.Find(id);
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aluno aluno = context.Alunos.Find(id);
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Aluno aluno = context.Alunos.Find(id);
            context.Alunos.Remove(aluno);
            context.SaveChanges();
            Usuario user = GerenciadorUsuario.FindById(aluno.Id_do_usuario);
            GerenciadorUsuario.Delete(user);
            TempData["Message"] = "Aluno(a) " + aluno.Nome.ToUpper() + " foi removido(a)";
            return RedirectToAction("Index");
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(Aluno aluno)
        {
            
            if (ModelState.IsValid)
            {
                context.Alunos.Add(aluno);
                context.SaveChanges();
                Usuario user = new Usuario
                {
                    UserName = aluno.Login,
                    Email = aluno.Email,
                    AlunoId = aluno.AlunoId
                };
                IdentityResult result = GerenciadorUsuario.Create(user, aluno.Senha);
                GerenciadorUsuario.AddToRole(user.Id, "Aluno");
                aluno.Id_do_usuario = user.Id;
                Pedido pedido = new Pedido();
                pedido.AlunoId = aluno.AlunoId;
                context.Pedidos.Add(pedido);
                Conclusao conclusao_false = new Conclusao
                {
                    Aluno = aluno,
                    AlunoId = aluno.AlunoId,
                    Concluido = false
                };
                context.Conclusoes.Add(conclusao_false);
                Conclusao conclusao_true = new Conclusao
                {
                    Aluno = aluno,
                    AlunoId = aluno.AlunoId,
                    Concluido = true
                };
                context.Conclusoes.Add(conclusao_true);
                context.SaveChanges();

                if (result.Succeeded)
                { return RedirectToAction("Index"); }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return RedirectToAction("Index");
        }
    }
}