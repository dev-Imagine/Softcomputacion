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
                    lstVenta = sVenta.ObtenerVentasReporte(fechaDesde, fechaHasta);
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
                        //calculo la ganancia neta de la venta
                        decimal neto = 0;
                        foreach (detalleVenta oDetalle in oVenta.detalleVenta)
                        {
                            neto += oDetalle.precioCostoGrupal;
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
                            oReporte.ingresoNeto = oVenta.costoTotal - neto;
                            oReporte.abonadoTotal = Convert.ToDecimal(oVenta.entregado);
                            lstReporteStock.Add(oReporte);
                            oReporte = new ReporteESstock();
                        }
                        else
                        {
                            lstReporteStock.Where(x => x.mes == mes && x.año == año).FirstOrDefault().ventaTotal = lstReporteStock.Where(x => x.mes == mes).FirstOrDefault().ventaTotal + oVenta.costoTotal;
                            lstReporteStock.Where(x => x.mes == mes && x.año == año).FirstOrDefault().ingresoNeto = lstReporteStock.Where(x => x.mes == mes).FirstOrDefault().ingresoNeto + (oVenta.costoTotal - neto);                            
                            lstReporteStock.Where(x => x.mes == mes && x.año == año).FirstOrDefault().abonadoTotal = lstReporteStock.Where(x => x.mes == mes && x.año == año).FirstOrDefault().abonadoTotal + Convert.ToDecimal(oVenta.entregado);
                            lstReporteStock.Where(x => x.mes == mes && x.año == año).FirstOrDefault().cantVentaTotal++;
                        }                        
                    }
                }
                return lstReporteStock;
            }
            catch (Exception ex)
            {
                throw ex;
                //return new List<ReporteESstock>();
            }
        }
        public List<cliente> obtenerClientesDeudores()
        {
            try
            {
                List<venta> lstVentas;
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    lstVentas = bd.venta.Where(x => (x.idEstado == 9 || x.idEstado == 10) && x.entregado < x.costoTotal && x.idCliente !=0 && x.idCliente != null).ToList();
                    List<cliente> lstClientes = new List<cliente>();
                    foreach (venta oVenta in lstVentas)
                    {
                        if (lstClientes.Where(x => x.idCliente == oVenta.idCliente).Count() == 0)
                        {
                            lstClientes.Add(oVenta.cliente);
                        }
                        lstClientes.Where(x => x.idCliente == oVenta.idCliente).FirstOrDefault().venta.Add(oVenta);
                    }
                    return lstClientes.OrderByDescending(x => x.venta.Sum(z => z.costoTotal - z.entregado)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReporteRankingGastoCliente> obtenerRankingCostoCliente(string fechaDesde, string fechaHasta)
        {
            try
            {
                DateTime dtFechaDesde = Convert.ToDateTime(fechaDesde).Date;
                DateTime dtFechaHasta = Convert.ToDateTime(fechaHasta).AddHours(23.59).AddSeconds(59);
                List<ReporteRankingGastoCliente> lstReporte = new List<ReporteRankingGastoCliente>();
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    ReporteRankingGastoCliente oReporte = new ReporteRankingGastoCliente();
                    oReporte.cantidadVentas = 0;
                    oReporte.gastoTotal = 0;
                    oReporte.nombreCliente = "";
                    srvCliente sCliente = new srvCliente();
                    List<cliente> lstCliente = new List<cliente>();
                    lstCliente = bd.cliente.ToList();
                    foreach (cliente oCLie in lstCliente)
                    {
                        oReporte.nombreCliente = oCLie.apellido + " " + oCLie.nombre;
                        foreach (venta oVenta in oCLie.venta.Where(x=> x.fechaEmision >= dtFechaDesde &&  x.fechaEmision <= dtFechaHasta && (x.idEstado == 10 || x.idEstado == 11)))
                        {
                            oReporte.cantidadVentas = oReporte.cantidadVentas + 1;
                            oReporte.gastoTotal = Convert.ToDecimal(oReporte.gastoTotal) + Convert.ToDecimal(oVenta.entregado);
                        }
                        lstReporte.Add(oReporte);
                        oReporte = new ReporteRankingGastoCliente();
                    }


                    return lstReporte;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}