using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using softcomputacion.Models;
using softcomputacion.Servicios;

namespace softcomputacion.Controllers
{
    public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult ReporteStockES()
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.filtros = ";;;";
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
            
        }

        public ActionResult ReporteVenta()
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.filtros = ";";
                srvReporte sReporte = new srvReporte();
                ViewBag.DatosChart = "";
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
            
        }
        public ActionResult ReporteDeudaClientes()
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvReporte sReporte = new srvReporte();
                List<cliente> lstClientesDeudores = sReporte.obtenerClientesDeudores();
                
                return View(lstClientesDeudores);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }

        }
        //Post Busqueda.
        [HttpPost]
        public ActionResult ReporteStockES(DateTime? fechaDesde = null, DateTime? fechaHasta = null, int idProducto = 0)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.filtros = "";
                DateTime dt;
                if (fechaDesde == null)
                {
                    fechaDesde = Convert.ToDateTime("01/01/2000");
                }
                else
                {
                    dt = Convert.ToDateTime(fechaDesde);
                    ViewBag.filtros = ViewBag.filtros + clsMetodosGenericos.ObtenerMes(dt.Month) + " de " + dt.Year;
                }
                ViewBag.filtros = ViewBag.filtros + ";";
                if (fechaHasta == null)
                {
                    fechaHasta = Convert.ToDateTime("01/01/3000");
                }
                else
                {
                    dt = Convert.ToDateTime(fechaHasta);
                    ViewBag.filtros = ViewBag.filtros + clsMetodosGenericos.ObtenerMes(dt.Month) + " de " + dt.Year;
                    fechaHasta = dt.AddMonths(1).AddDays(-1);
                }
                ViewBag.filtros = ViewBag.filtros + ";" + idProducto;
                srvReporte sReporte = new srvReporte();
                List<ReporteESstock> model = sReporte.obtenerReporteESstock(Convert.ToDateTime(fechaDesde), Convert.ToDateTime(fechaHasta), idProducto);
                return View(model.OrderBy(x => x.año).ThenBy(x => x.mes).ToList());
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
            
        }
        [HttpPost]
        public ActionResult ReporteVenta(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.filtros = "";
                DateTime dt;
                if (fechaDesde == null)
                {
                    fechaDesde = Convert.ToDateTime("01/01/2000");
                }
                else
                {
                    dt = Convert.ToDateTime(fechaDesde);
                    ViewBag.filtros = ViewBag.filtros + clsMetodosGenericos.ObtenerMes(dt.Month) + " de " + dt.Year;
                }
                ViewBag.filtros = ViewBag.filtros + ";";
                if (fechaHasta == null)
                {
                    fechaHasta = Convert.ToDateTime("01/01/3000");
                }
                else
                {
                    dt =  Convert.ToDateTime(fechaHasta);
                    ViewBag.filtros = ViewBag.filtros + clsMetodosGenericos.ObtenerMes(dt.Month) + " de " + dt.Year;
                    fechaHasta = dt.AddMonths(1).AddDays(-1);
                }
                srvReporte sReporte = new srvReporte();
                List<ReporteESstock> model = sReporte.obtenerReporteVentas(Convert.ToDateTime(fechaDesde), Convert.ToDateTime(fechaHasta));
                model = model.OrderBy(x => x.año).ThenBy(x => x.mes).ToList();
                string stDatosChart = "";
                //        [['Year', 'Sales', 'Expenses'],
                //        ['2004',  1000,      400],
                //        ['2005',  1170,      460],
                //        ['2006',  660,       1120],
                //        ['2007',  1030,      540]]
                stDatosChart = "[";//"[['Meses', 'Total', 'Neto'],";
                foreach (var oData in model)
                {
                    stDatosChart = stDatosChart + "[";
                    stDatosChart = stDatosChart + "\"" + oData.mes.ToString().PadLeft(2, '0') + "/" + oData.año + "\"," + Convert.ToInt32(oData.ventaTotal) + "," + Convert.ToInt32(oData.ingresoNeto);
                    stDatosChart = stDatosChart + "],";
                }
                stDatosChart = stDatosChart.Remove(stDatosChart.Length - 1, 1);
                stDatosChart = stDatosChart + "]";
                ViewBag.DatosChart = stDatosChart;
                return View(model);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
           
        }

        //Metodos

        public JsonResult GuardarHistorialStock(int idProducto, int cantidad, string tipo)
        {
            try
            {
                srvReporte sReporte = new srvReporte();
                sReporte.GuardarHistorialStock(idProducto, cantidad, tipo);
                return Json("True");
            }
            catch (Exception)
            {
                return Json("Error para almacenar historial de Stock");
            }
        }
        
    }
}