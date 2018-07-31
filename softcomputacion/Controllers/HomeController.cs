using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using softcomputacion.Models;
using softcomputacion.Servicios;

namespace softcomputacion.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(bool DatosIncorrectos = false)
        {
            ViewBag.DatosIncorrectos = DatosIncorrectos;
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Ingresar(string dni, string contraseña)
        {
            srvUsuario sUsuario = new srvUsuario();
            usuario oUsuario = sUsuario.ObtenerUsuario(dni, contraseña);
            if (oUsuario != null && oUsuario.fechaBaja == null)
            {
                Session["Usuario"] = oUsuario;
                return RedirectToAction("ListarProducto", "Producto");
            }
            Session["Usuario"] = null;
            return RedirectToAction("Index", new { DatosIncorrectos = true});
        }
        public ActionResult CerrarSession()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}