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
        public ActionResult ReporteProductos()
        {
            return View();
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