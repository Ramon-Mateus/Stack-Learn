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
            return View(context.Pedidos.OrderBy(c => c.PedidoId));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pedido pedido)
        {
            context.Pedidos.Add(pedido);
            context.SaveChanges();
            return RedirectToAction("Index");
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
            Pedido pedido = context.Pedidos.Where(f => f.PedidoId == id).First();
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
            Pedido pedido = context.Pedidos.Find(id);
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
    }
}