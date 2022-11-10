using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Stack_Learn.Context;
using Modelos.Models;

namespace Stack_Learn.Controllers
{
    public class HomeController : Controller
    {

        private EFContext context = new EFContext();
        // GET: Home
        public ActionResult PaginaInicial()
        {
            return View(context.Cursos.Include(c => c.Categoria).Include(f => f.Professor).OrderBy(n => n.Nome));
        }
    }
}