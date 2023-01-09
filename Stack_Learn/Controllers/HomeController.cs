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
    public class HomeController : Controller
    {
        private GerenciadorUsuario GerenciadorUsuario
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<GerenciadorUsuario>();
            }
        }
        private EFContext context = new EFContext();
        // GET: Home
        public ActionResult PaginaInicial()
        {
            var Cursos_Usuarios = new CursosUsuarios();
            if (System.Web.HttpContext.Current.User.Identity.Name.ToString() != "")
            {
                var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
                Usuario user = GerenciadorUsuario.FindById(userid);
                Cursos_Usuarios.AlunoId = user.AlunoId;
            }
            var Cursos_totais = new List<Curso>();
            foreach (var item in context.Cursos.Include(c => c.Categoria).Include(f => f.Professor))
            {
                Cursos_totais.Add(item);
            }
            Cursos_Usuarios.Cursos = Cursos_totais;
            return View(Cursos_Usuarios);
        }
    }
}