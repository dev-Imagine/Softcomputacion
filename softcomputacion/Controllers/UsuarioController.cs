using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using softcomputacion.Models;
using softcomputacion.Servicios;

namespace softcomputacion.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        [HttpGet]
        public ActionResult ListarUsuario()
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvUsuario sUsuario = new srvUsuario();
                ViewBag.lstUsuarios = sUsuario.ObtenerUsuarios();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los proveedores." });
            }
        }

        //Vistas parciales

        // Metodos
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GuardarModificarUsuario(usuario oUsuario)
        {
            try
            {
                usuario oSession = (usuario)Session["Usuario"];
                if (oSession == null || oSession.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvUsuario sUsuario= new srvUsuario();
                sUsuario.GuardarModificarUsuario(oUsuario);
                return RedirectToAction("ListarUsuario");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar guardar o modificar el usuario." });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult EliminarUsuario(int idUsuario)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    throw new Exception();
                }
                srvUsuario sUsuario = new srvUsuario();
                sUsuario.EliminarUsuario(idUsuario);
                return Json("True");
            }
            catch (Exception)
            {
                return Json("False");
            }
        }

    }
}