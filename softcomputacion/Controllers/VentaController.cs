using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;
using softcomputacion.Servicios;
using PagedList;
using System.Net.Http;
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
                ViewBag.lstCategorias = sCategoria.ObtenerCategorias();
                ViewBag.lstEstados = sEstado.ObtenerEstados();
                ViewBag.filtros = ";;;";
                ViewBag.ValorUSD = GetValorUsd();
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
                srvEstado sEstado = new srvEstado();
                srvProducto sProducto = new srvProducto();
                srvCategoria sCategoria = new srvCategoria();
                List<producto> lstProductos = sProducto.ObtenerProductos(nombreProducto, idCategoria, idSubCategoria, idEstado, idProducto);
                Session["lstProducto"] = lstProductos;
                ViewBag.lstCategorias = sCategoria.ObtenerCategorias();
                ViewBag.lstEstados = sEstado.ObtenerEstados();
                ViewBag.filtros = Convert.ToString(nombreProducto + ";" + idCategoria + ";" + idSubCategoria + ";" + idEstado);
                ViewBag.ValorUSD = GetValorUsd();
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
                return View(oVenta);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }
        public ActionResult ListarVentas()
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
                srvVenta sVenta = new srvVenta();
                List<venta> lstVentas = sVenta.ObtenerVentasUsuario(oUsuario.idUsuario);
                ViewBag.filtros = ";;";
                return View(lstVentas);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }
        [HttpPost]
        public ActionResult ListarVentas(int idUsuario = 0)
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
                List<venta> lstVentas;
                srvUsuario sUsuario = new srvUsuario();
                ViewBag.Ususarios = sUsuario.ObtenerUsuarios();
                lstVentas = sVenta.ObtenerVentasUsuario(idUsuario);
                ViewBag.filtros = ";"+";"+ idUsuario;
                return View(lstVentas);
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
        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server)]
        public double GetValorUsd()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://ws.geeklab.com.ar");
                var responseTask = client.GetAsync("dolar/get-dolar-json.php");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    //  {\"libre\":\"28.41\",\"blue\":\"28.65\"}
                    string stResult = readTask.Result.Substring(10, 5).Replace(".", ",");
                    return Convert.ToDouble(stResult);
                }
                else //web api sent error response 
                {
                    return 0;
                }
            }
        }

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
                oVenta.idUsuario = oUsuario.idUsuario;
                srvVenta sVenta = new srvVenta();                
                if (oVenta.detalleVenta.Count == 0)
                {
                    return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
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
    }
}