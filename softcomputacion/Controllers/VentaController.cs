using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;
using softcomputacion.Servicios;
using PagedList;
using System.Web.Mvc;

namespace softcomputacion.Controllers
{
    public class VentaController : Controller
    {
        
        public ActionResult NuevaVenta(int nroPagina = 1, int tamañoPagina = 6, bool paginacion = false)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvEstado sEstado = new srvEstado();
                srvProducto sProducto = new srvProducto();
                srvCategoria sCategoria = new srvCategoria();
                List<producto> lstProductos = (List<producto>)Session["lstProducto"];
                if (lstProductos == null || lstProductos.Count == 0 || paginacion == false)
                {
                    Session["lstProducto"] = new List<producto>();
                    lstProductos = new List<producto>();
                }
                if (Session["venta"] == null)
                {
                    Session["venta"] = new venta();
                    //Session["lstProducto"] = new List<producto>();
                    //lstProductos = new List<producto>();
                }
                ViewBag.lstCategorias = sCategoria.ObtenerCategorias();
                ViewBag.lstEstados = sEstado.ObtenerEstados();
                ViewBag.filtros = ";;;";
                ProductoController ProductoController = new ProductoController();
                ViewBag.ValorUSD = ProductoController.GetValorUsd();
                PagedList<producto> model = new PagedList<producto>(lstProductos.ToList(), nroPagina, tamañoPagina);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }

        }
        [HttpPost]
        public ActionResult NuevaVenta(string nombreProducto = "", int idEstado = 0, int idCategoria = 0, int idSubCategoria = 0, int idProducto = 0)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                if (Session["venta"] == null)
                {
                    Session["venta"] = new venta();
                }
                srvEstado sEstado = new srvEstado();
                srvProducto sProducto = new srvProducto();
                srvCategoria sCategoria = new srvCategoria();
                List<producto> lstProductos = sProducto.ObtenerProductos(nombreProducto, idCategoria, idSubCategoria, idEstado, idProducto);
                Session["lstProducto"] = lstProductos;
                ViewBag.lstCategorias = sCategoria.ObtenerCategorias();
                ViewBag.lstEstados = sEstado.ObtenerEstados();
                ViewBag.filtros = Convert.ToString(nombreProducto + ";" + idCategoria + ";" + idSubCategoria + ";" + idEstado);
                ProductoController ProductoController = new ProductoController();
                ViewBag.ValorUSD = ProductoController.GetValorUsd();
                PagedList<producto> model = new PagedList<producto>(lstProductos.ToList(), 1, 6);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }

        }
        public ActionResult VistaVenta(int idVenta)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                
                srvVenta sVenta = new srvVenta();                
                venta oVenta = sVenta.ObtenerVenta(idVenta);
                if (oVenta.idCliente==0 || oVenta.idCliente == null)
                {
                    oVenta.idCliente = 0;
                    oVenta.cliente = new cliente();
                    oVenta.cliente.nombre = "CONSUMIDOR ";
                    oVenta.cliente.apellido = "FINAL";
                }
                return View(oVenta);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }
        public ActionResult ListarVentas(int nroPagina = 1, int tamañoPagina = 10, bool paginacion = false)
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
                List<venta> lstVentas= (List<venta>)Session["lstVentas"];
                if (lstVentas == null || lstVentas.Count == 0 || paginacion == false)
                {
                    srvVenta sVenta = new srvVenta();
                    lstVentas = sVenta.ObtenerVentas(Convert.ToDateTime("01/01/1000"), Convert.ToDateTime("01/01/3000"), oUsuario.idUsuario);
                    Session["lstVentas"] = lstVentas;
                }
                ViewBag.filtros = ";;";
                PagedList<venta> ModelVentas = new PagedList<venta>(lstVentas.ToList(), nroPagina, tamañoPagina);
                return View(ModelVentas);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }
        [HttpPost]
        public ActionResult ListarVentas(string fechaDesde, string fechaHasta, int idUsuario = 0, int idVenta=0)
        {
            ViewBag.filtros = fechaDesde + ";" + fechaHasta + ";" + idUsuario;
            if (fechaDesde == "") fechaDesde = "01/01/1000";
            if (fechaHasta == "") fechaHasta = "01/01/3000";
            try
            {                
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvVenta sVenta = new srvVenta();
                List<venta> lstVentas;
                srvUsuario sUsuario = new srvUsuario();
                ViewBag.Ususarios = sUsuario.ObtenerUsuarios();
                lstVentas = sVenta.ObtenerVentas(Convert.ToDateTime(fechaDesde), Convert.ToDateTime(fechaHasta),idUsuario, idVenta);
                Session["lstVentas"] = lstVentas;
                PagedList<venta> ModelVentas = new PagedList<venta>(lstVentas.ToList(), 1, 10);
                return View(ModelVentas);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }

        //Vistas Parciales
        [HttpPost]
        public PartialViewResult _CarritoVenta(int idProducto, string precio, string cantidad)
        {
            decimal precioD = Convert.ToDecimal(precio);
            int cantidadI = Convert.ToInt32(cantidad);
            try
            {
                venta oVenta = new venta();
                if (Session["venta"] == null)
                {
                    Session["venta"] = new venta();
                }
                else
                {
                    //Session["venta"] = Session["venta"];
                    oVenta = (venta)Session["venta"];
                }
                srvVenta sVenta = new srvVenta();
                oVenta = sVenta.agregarDetalle(oVenta, idProducto, precioD, cantidadI);
                Session["venta"] = oVenta;
                return PartialView();
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Metodos
        public string borrarDetalle(int idProducto)
        {
            try
            {
                venta oVenta = new venta();
                oVenta = (venta)Session["venta"];
                detalleVenta oDetalle = new detalleVenta();
                oDetalle = oVenta.detalleVenta.Where(x => x.idProducto == idProducto).First();                
                oVenta.detalleVenta.Remove(oDetalle);
                return "true";
            }
            catch (Exception)
            {
                return "false";
            }
        }
        [HttpPost]
        public ActionResult GenerarVenta()
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                venta oVenta = (venta)Session["venta"];
                oVenta.cliente = null;
                oVenta.idUsuario = oUsuario.idUsuario;
                srvVenta sVenta = new srvVenta();                
                if (oVenta.detalleVenta.Count == 0)
                {
                    return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
                }
                if (oVenta.idCliente == 0)
                {
                    oVenta.idCliente = null;
                }
                oVenta = sVenta.guardarVenta(oVenta);                
                Session["venta"] = null;
                return RedirectToAction("VistaVenta", new { idVenta = oVenta.idVenta });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }

        public JsonResult SeleccionarClienteVenta(cliente oCliente)
        {
            try
            {
                if (oCliente.idCliente==0)
                {
                    throw new Exception();
                }
                venta oVenta = (venta)Session["venta"];
                oVenta.idCliente = oCliente.idCliente;
                oVenta.cliente = oCliente;
            }
            catch (Exception)
            {
                oCliente.nombre = "FINAL";
                oCliente.apellido = "CONSUMIDOR";
                oCliente.idCliente = 0;
                venta oVenta = (venta)Session["venta"];
                oVenta.idCliente = 0;
                oVenta.cliente = oCliente;
            }
            return Json(oCliente);
        }
    }
}