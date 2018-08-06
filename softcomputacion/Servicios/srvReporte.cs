using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;


namespace softcomputacion.Servicios
{
    public class srvReporte
    {

        //Guardar de historial de productos.
        public string GuardarHistorialStock(int idProducto, int cantidad, string tipo)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    historialStock oHistorial = new historialStock();
                    oHistorial.cantidad = cantidad;
                    oHistorial.idProducto = idProducto;
                    oHistorial.tipo = tipo;
                    oHistorial.fechaHora = System.DateTime.Now;
                    bd.Entry(oHistorial).State = System.Data.Entity.EntityState.Added;
                    bd.SaveChanges();
                    return "True";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Obtener reporte entrada y salida stock
        public List<ReporteESstock> obtenerReporteESstock(DateTime fechaDesde, DateTime fechaHasta, int idProducto)
        {
            try
            {
                List<ReporteESstock> lstReporteStock = new List<ReporteESstock>();
                
                List<historialStock> lstHistorialStock = new List<historialStock>();

                fechaHasta = fechaHasta.AddHours(23.59).AddSeconds(59);
                bool busquedaPorId = false;
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    if (idProducto != 0)
                    {
                        lstHistorialStock = bd.historialStock.Where(x => x.idProducto == idProducto && x.fechaHora >= fechaDesde && x.fechaHora <= fechaHasta).ToList();
                        busquedaPorId = true;
                    }
                    else
                    {
                        lstHistorialStock = bd.historialStock.Where(x=> x.fechaHora >= fechaDesde && x.fechaHora <= fechaHasta).ToList();
                    }
                    int mes = 0;
                    int año = 0;
                    foreach (historialStock oHistorial in lstHistorialStock)
                    {
                        if (mes == 0 || mes != oHistorial.fechaHora.Month)
                        {
                            mes= oHistorial.fechaHora.Month;
                            año = oHistorial.fechaHora.Year;
                        }                         
                        if (busquedaPorId == false)
                        {
                                if (lstReporteStock.Where(x => x.idProducto == oHistorial.idProducto && x.mes == mes && x.año == año).Count() == 0 )
                                {
                                    ReporteESstock oReporte = new ReporteESstock();
                                    oReporte.entrada = 0;
                                    oReporte.salida = 0;
                                    oReporte.mes = mes;
                                    oReporte.idProducto = oHistorial.idProducto;
                                    oReporte.año = oHistorial.fechaHora.Year;
                                    oReporte.productoNombre = oHistorial.producto.categoria.nombre + " " + oHistorial.producto.subcategoria.nombre + " " + oHistorial.producto.nombre;
                                    if (oHistorial.tipo == "entrada")
                                    {
                                        oReporte.entrada = oHistorial.cantidad;
                                    }
                                    else
                                    {
                                        oReporte.salida = oHistorial.cantidad;
                                    }
                                    lstReporteStock.Add(oReporte);
                                    oReporte = new ReporteESstock();
                                }
                                else
                                {
                                    
                                    if (oHistorial.tipo == "entrada")
                                    {
                                        lstReporteStock.Where(x => x.idProducto == oHistorial.idProducto).FirstOrDefault().entrada = lstReporteStock.Where(x => x.idProducto == oHistorial.idProducto).FirstOrDefault().entrada + oHistorial.cantidad;
                                    }
                                    else
                                    {
                                        lstReporteStock.Where(x => x.idProducto == oHistorial.idProducto).FirstOrDefault().salida = lstReporteStock.Where(x => x.idProducto == oHistorial.idProducto).FirstOrDefault().salida + oHistorial.cantidad;
                                    }

                                }
                        }
                        else
                        {
                            if (mes == 0 || mes != oHistorial.fechaHora.Month)
                            {
                                mes = oHistorial.fechaHora.Month;
                                año = oHistorial.fechaHora.Year;
                            }
                            if (lstReporteStock.Where(x => x.mes == mes && x.año == año).Count() == 0)
                            {
                                ReporteESstock oReporte = new ReporteESstock();
                                oReporte.entrada = 0;
                                oReporte.salida = 0;
                                oReporte.mes = mes;
                                oReporte.idProducto = oHistorial.idProducto;
                                oReporte.año = oHistorial.fechaHora.Year;
                                oReporte.productoNombre = oHistorial.producto.categoria.nombre + " " + oHistorial.producto.subcategoria.nombre + " " + oHistorial.producto.nombre;
                                if (oHistorial.tipo == "entrada")
                                {
                                    oReporte.entrada = oHistorial.cantidad;
                                }
                                else
                                {
                                    oReporte.salida = oHistorial.cantidad;
                                }
                                lstReporteStock.Add(oReporte);
                                oReporte = new ReporteESstock();
                            }
                            else
                            {

                                if (oHistorial.tipo == "entrada")
                                {
                                    lstReporteStock.Where(x => x.idProducto == oHistorial.idProducto).FirstOrDefault().entrada = lstReporteStock.Where(x => x.idProducto == oHistorial.idProducto).FirstOrDefault().entrada + oHistorial.cantidad;
                                }
                                else
                                {
                                    lstReporteStock.Where(x => x.idProducto == oHistorial.idProducto).FirstOrDefault().salida = lstReporteStock.Where(x => x.idProducto == oHistorial.idProducto).FirstOrDefault().salida + oHistorial.cantidad;
                                }

                            }
                        }
                    }
                }
                return lstReporteStock;
            }
            catch (Exception)
            {
                return new List<ReporteESstock>();
            }
        }



        public List<ReporteESstock> obtenerReporteVentas(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                List<ReporteESstock> lstReporteStock = new List<ReporteESstock>();
                List<venta> lstVenta = new List<venta>();
                srvVenta sVenta = new srvVenta();          
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    lstVenta = sVenta.ObtenerVentasUsuario(fechaDesde, fechaHasta, 0);
                    int mes = 0;
                    int año = 0;
                    foreach (venta oVenta in lstVenta)
                    {
                        if (mes == 0 || mes != oVenta.fechaEmision.Month)
                        {
                            mes = oVenta.fechaEmision.Month;
                            año = oVenta.fechaEmision.Year;
                        }
                        
                        
                        if (mes == 0 || mes != oVenta.fechaEmision.Month)
                        {
                            mes = oVenta.fechaEmision.Month;
                        }
                        if (lstReporteStock.Where(x => x.mes == mes && x.año == año).Count() == 0)
                        {
                            ReporteESstock oReporte = new ReporteESstock();
                            oReporte.entrada = 0;
                            oReporte.salida = 0;
                            oReporte.mes = mes;
                            oReporte.año = oVenta.fechaEmision.Year;
                            oReporte.ventaTotal = oVenta.costoTotal;
                            oReporte.cantVentaTotal = 1;
                            lstReporteStock.Add(oReporte);
                            oReporte = new ReporteESstock();
                        }
                        else
                        {
                            lstReporteStock.Where(x => x.mes == mes && x.año == año).FirstOrDefault().ventaTotal = lstReporteStock.Where(x => x.mes == mes).FirstOrDefault().ventaTotal + oVenta.costoTotal;
                            lstReporteStock.Where(x => x.mes == mes && x.año == año).FirstOrDefault().cantVentaTotal++;
                        }                        
                    }
                }
                return lstReporteStock;
            }
            catch (Exception)
            {
                return new List<ReporteESstock>();
            }
        }
    }
}