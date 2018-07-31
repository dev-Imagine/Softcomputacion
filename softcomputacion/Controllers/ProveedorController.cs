using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using softcomputacion.Models;
using softcomputacion.Servicios;

namespace softcomputacion.Controllers
{
    public class ProveedorController : Controller
    {
        [HttpGet]
        public ActionResult ListaProveedores()
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null ||oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvProveedor sProveedor = new srvProveedor();
                ViewBag.lstProveedores = sProveedor.ObtenerProveedores();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los proveedores." });
            }
        }
        [HttpPost]
        public ActionResult ListaProveedores(string nombreEmpresa)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvProveedor sProveedor = new srvProveedor();
                ViewBag.lstProveedores = sProveedor.ObtenerProveedores(nombreEmpresa);
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los proveedores." });
            }
        }

        //vistas parciales
        public PartialViewResult _frmProveedor(proveedor oProveedor)
        {
            if (oProveedor == null || oProveedor.idProveedor == 0)
            {
                oProveedor = new proveedor();
            }
            return PartialView(oProveedor);
        }
        //metodos
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GuardarModificarProveedor(proveedor oProveedor)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvProveedor sProveedor = new srvProveedor();
                sProveedor.GuardarModificarProveedor(oProveedor);
                return RedirectToAction("ListaProveedores");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar guardar o modificar el proveedor." });
            }
        }
        [HttpPost,ValidateAntiForgeryToken]
        public JsonResult EliminarProveedor(int idProveedor)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    throw new Exception();
                }
                srvProveedor sProveedor = new srvProveedor();
                sProveedor.EliminarProveedor(idProveedor);
                return Json("True");
            }
            catch (Exception)
            {
                return Json("False");
            }
        }
    }
}