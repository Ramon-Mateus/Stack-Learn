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

namespace Stack_Learn.Controllers
{
    public class ProfessoresController : Controller
    {

        private EFContext context = new EFContext();
        private GerenciadorUsuario GerenciadorUsuario
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<GerenciadorUsuario>();
            }
        }
        private GerenciadorUsuario UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<GerenciadorUsuario>();
            }
        }

        public ActionResult Index()
        {
            return View(context.Professores.OrderBy(c => c.Nome));
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Professor professor)
        {
            context.Professores.Add(professor);
            context.SaveChanges();
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
        public ActionResult Cadastro(Professor professor)
        {
            if (ModelState.IsValid)
            {
                context.Professores.Add(professor);
                context.SaveChanges();
                Usuario user = new Usuario
                {
                    UserName = professor.Login,
                    Email = professor.Email,
                    ProfessorId = professor.ProfessorId
                };
                IdentityResult result = GerenciadorUsuario.Create(user, professor.Senha);
                UserManager.AddToRole(user.Id, "Professor");
                professor.Id_do_usuario = user.Id;
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

        public ActionResult Edit(long? id)
        {
            var CursosUsuarios = new CursosUsuarios();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = context.Professores.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Professor professor)
        {
            if (ModelState.IsValid)
            {
                context.Entry(professor).State = EntityState.Modified;
                Usuario usuario = GerenciadorUsuario.FindById(professor.Id_do_usuario);
                usuario.UserName = professor.Login;
                usuario.Email = professor.Email;
                usuario.PasswordHash = GerenciadorUsuario.PasswordHasher.HashPassword(professor.Senha);
                GerenciadorUsuario.Update(usuario);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(professor);
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = context.Professores.Where(f => f.ProfessorId == id).Include("Cursos.Categoria").First();
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = context.Professores.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Professor professor = context.Professores.Find(id);
            context.Professores.Remove(professor);
            context.SaveChanges();
            Usuario user = GerenciadorUsuario.FindById(professor.Id_do_usuario);
            GerenciadorUsuario.Delete(user);
            TempData["Message"] = "Professor " + professor.Nome.ToUpper() + " foi removido";
            return RedirectToAction("Index");
        }
    }
}