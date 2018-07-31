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
        // GET: Venta
        public ActionResult venta()
        {
            usuario oUsuario = (usuario)Session["Usuario"];
            if (oUsuario == null || oUsuario.idTipoUsuario != 2)
            {
                Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Venta(int nroPagina = 1, int tamañoPagina = 10, bool paginacion = false)
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
        public ActionResult Venta(string nombreProducto = "", int idEstado = 0, int idCategoria = 0, int idSubCategoria = 0)
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
                List<producto> lstProductos = sProducto.ObtenerProductos(nombreProducto, idCategoria, idSubCategoria, idEstado);
                Session["lstProducto"] = lstProductos;
                ViewBag.lstCategorias = sCategoria.ObtenerCategorias();
                ViewBag.lstEstados = sEstado.ObtenerEstados();
                ViewBag.filtros = Convert.ToString(nombreProducto + ";" + idCategoria + ";" + idSubCategoria + ";" + idEstado);
                ViewBag.ValorUSD = GetValorUsd();
                PagedList<producto> model = new PagedList<producto>(lstProductos.ToList(), 1, 10);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }

        }


        //Vistas Parciales

        [HttpPost]
        public PartialViewResult _CarritoVenta(int idProducto, double precio, int cantidad)
        {
            try
            {
                venta oVenta = new venta();
                if (Session["venta"] == null)
                {
                    Session["venta"] = new venta();
                }
                else
                {
                    Session["venta"] = Session["venta"];
                    oVenta = (venta)Session["venta"];
                }
                srvVenta sVenta = new srvVenta();
                oVenta = sVenta.agregarDetalle(oVenta, idProducto, precio, cantidad);
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


    }
}