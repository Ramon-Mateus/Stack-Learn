using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Stack_Learn.Areas.Seguranca.Models;
using Stack_Learn.Infraestrutura;

namespace Stack_Learn.Areas.Seguranca.Controllers
{
    public class AdminController : Controller
    {
        // Definição da Propriedade GerenciadorUsuario
        private GerenciadorUsuario GerenciadorUsuario
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<GerenciadorUsuario>();
            }
        }
        // GET: Seguranca/Admin
        //[Authorize(Roles = "ADM")]
        public ActionResult Index()
        {
            return View(GerenciadorUsuario.Users);
        }
        //[Authorize(Roles = "ADM")]
        public ActionResult Create()
        {
            return View();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [HttpPost]
        public ActionResult Create(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                Usuario user = new Usuario
                {
                    UserName = model.Nome,
                    Email = model.Email
                };
                IdentityResult result = GerenciadorUsuario.Create(user, model.Senha);
                if (result.Succeeded)
                { return RedirectToAction("Index"); }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = GerenciadorUsuario.FindById(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            // inicia o objeto usuário para visão
            var uvm = new UsuarioViewModel();
            uvm.Id = usuario.Id;
            uvm.Nome = usuario.UserName;
            uvm.Email = usuario.Email;
            return View(uvm);
        }
        [HttpPost]
        public ActionResult Edit(UsuarioViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = GerenciadorUsuario.FindById(uvm.Id);
                usuario.UserName = uvm.Nome;
                usuario.Email = uvm.Email;
                usuario.PasswordHash = GerenciadorUsuario.PasswordHasher.HashPassword(uvm.Senha);
                IdentityResult result = GerenciadorUsuario.Update(usuario);
                if (result.Succeeded)
                { return RedirectToAction("Index"); }
                else
                { AddErrorsFromResult(result); }
            }
            return View(uvm);
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(
                HttpStatusCode.BadRequest);
            }
            Usuario usuario = GerenciadorUsuario.FindById(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
        [HttpPost]
        public ActionResult Delete(Usuario usuario)
        {
            Usuario user = GerenciadorUsuario.FindById(usuario.Id);
            if (user != null)
            {
                IdentityResult result = GerenciadorUsuario.Delete(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
        [Authorize(Roles = "ADM")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(
                HttpStatusCode.BadRequest);
            }
            Usuario usuario = GerenciadorUsuario.FindById(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
    }
}