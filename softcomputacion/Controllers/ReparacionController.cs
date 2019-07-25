using PagedList;
using softcomputacion.Models;
using softcomputacion.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace softcomputacion.Controllers
{
    public class ReparacionController : Controller
    {
        // GET: Reparacion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListarReparaciones(int nroPagina = 1, int tamañoPagina = 10, bool paginacion = false)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvUsuario sUsuario = new srvUsuario();
                ViewBag.Ususarios = sUsuario.ObtenerUsuarios();
                srvEstado sEstado = new srvEstado();
                ViewBag.Estados = sEstado.ObtenerEstados("REPARACION");
                List<reparacion> lstReparaciones = (List<reparacion>)Session["lstReparaciones"];
                if (lstReparaciones == null || lstReparaciones.Count == 0 || paginacion == false)
                {
                    srvReparacion sReparacion = new srvReparacion();
                    lstReparaciones = sReparacion.ObtenerReparaciones();
                    Session["lstReparaciones"] = lstReparaciones;
                }
                ViewBag.filtros = ";;;";
                PagedList<reparacion> ModelReparaciones = new PagedList<reparacion>(lstReparaciones.ToList(), nroPagina, tamañoPagina);
                return View(ModelReparaciones);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }
        [HttpPost]
        public ActionResult ListarReparaciones(int idReparacion = 0, int idUsuario = 0, int idEstado = 0, string stCliente = "")
        {
            try
            {
                ViewBag.filtros = idReparacion + ";" + idUsuario + ";" + idEstado + ";" + stCliente;
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvReparacion sReparacion = new srvReparacion();
                List<reparacion> lstReparaciones;
                srvUsuario sUsuario = new srvUsuario();
                ViewBag.Ususarios = sUsuario.ObtenerUsuarios();
                srvEstado sEstado = new srvEstado();
                ViewBag.Estados = sEstado.ObtenerEstados("REPARACION");
                lstReparaciones = sReparacion.ObtenerReparaciones(idReparacion, idUsuario, idEstado, stCliente);
                Session["lstReparaciones"] = lstReparaciones;
                PagedList<reparacion> ModelReparaciones = new PagedList<reparacion>(lstReparaciones.ToList(), 1, 10);
                return View(ModelReparaciones);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }
        public ActionResult Reparacion()
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.fecha = System.DateTime.Now.Date;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }

        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Reparacion(int idProducto)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvCategoria sCategoria = new srvCategoria();
                ViewBag.lstCategorias = sCategoria.ObtenerCategorias();
                srvProveedor sProveedor = new srvProveedor();
                ViewBag.lstProveedores = sProveedor.ObtenerProveedores();
                srvProducto sProducto = new srvProducto();
                ViewBag.oProducto = sProducto.ObtenerProducto(idProducto);
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }

        }
    }
}