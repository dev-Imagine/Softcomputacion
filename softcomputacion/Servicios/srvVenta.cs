using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvVenta
    {
        public venta agregarDetalle(venta oVenta, int idProducto, decimal precio, int cantidad)
        {
            try
            {
                detalleVenta oDetalle = new detalleVenta();
                srvProducto sProducto = new srvProducto();
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    if (oVenta.detalleVenta.Where(x => x.idProducto == idProducto).Count() == 1)
                    {
                        oVenta.detalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().cantidad = oVenta.detalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().cantidad + cantidad;
                        int cantidadExistente = oVenta.detalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().cantidad;
                        oVenta.detalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().costoGrupal = Convert.ToDecimal(precio * cantidadExistente);
                    }
                    else {
                        oDetalle.idProducto = idProducto;
                        oDetalle.producto = sProducto.ObtenerProducto(idProducto);
                        string nombre = oDetalle.producto.nombre;
                        oDetalle.costoIndividual = Convert.ToDecimal(precio);
                        oDetalle.costoGrupal = Convert.ToDecimal(precio * cantidad);
                        oDetalle.cantidad = cantidad;
                        oVenta.detalleVenta.Add(oDetalle);
                    }
                    
                    
                    return oVenta;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public venta guardarVenta(venta oVenta)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    srvProducto sProducto = new srvProducto();
                    srvUsuario sUsuario = new srvUsuario();
                    producto oProducto = new producto();
                    decimal totalCosto = 0;
                    int totalCantidadProductos = 0;
                    foreach (detalleVenta oDetalle in oVenta.detalleVenta)
                    {
                        totalCosto = totalCosto + oDetalle.costoGrupal;
                        totalCantidadProductos = totalCantidadProductos + oDetalle.cantidad;
                        oProducto = bd.producto.Where(x => x.idProducto == oDetalle.idProducto).FirstOrDefault();
                        oProducto.stockActual = oProducto.stockActual - oDetalle.cantidad;
                        if (oProducto.stockActual == 0)
                        {
                            oProducto.idEstado = 3;
                        }
                        else
                        {
                            if (oProducto.stockActual <= oProducto.stockMinimo)
                            {
                                oProducto.idEstado = 2;
                            }
                            else
                            {
                                oProducto.idEstado = 1;
                            }
                        }
                        oProducto.estado = bd.estado.Where(x => x.idEstado == oProducto.idEstado).FirstOrDefault();
                        bd.Entry(oProducto).State = System.Data.Entity.EntityState.Modified;
                        oDetalle.producto = null;
                    }
                    oVenta.fechaEmision = System.DateTime.Now;
                    oVenta.cantidadProductosTotal = totalCantidadProductos;
                    oVenta.costoTotal = totalCosto;
                    oVenta.cliente = null;
                    bd.Entry(oVenta).State = System.Data.Entity.EntityState.Added;
                    bd.SaveChanges();
                    return oVenta;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public venta ObtenerVenta(int idVenta)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    venta oVenta = bd.venta.Where(x => x.idVenta == idVenta).FirstOrDefault();
                    string temp ="";
                    if (oVenta.idCliente !=0 && oVenta.idCliente !=null)
                    {
                        temp = oVenta.cliente.apellido;
                    }
                    
                    temp = oVenta.usuario.nombre;
                    foreach (detalleVenta oDetalle  in oVenta.detalleVenta)
                    {
                        temp = oDetalle.producto.nombre;
                        temp = oDetalle.producto.subcategoria.nombre;
                        temp = oDetalle.producto.categoria.nombre;
                    }
                    return oVenta;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<venta> ObtenerVentasUsuario(DateTime fechaDesde, DateTime fechaHasta, int idUsuario = 0)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    fechaHasta = fechaHasta.AddHours(23.59).AddSeconds(59);
                    List<venta> lstVenta;
                    if (idUsuario==0)
                    {
                        lstVenta = bd.venta.Where(x => x.fechaEmision >= fechaDesde && x.fechaEmision <= fechaHasta).ToList();
                    }
                    else
                    {
                        lstVenta = bd.venta.Where(x => x.idUsuario == idUsuario && x.fechaEmision >= fechaDesde && x.fechaEmision <= fechaHasta).ToList();
                    }
                    string temp = "";
                    foreach (venta oVenta in lstVenta.ToList())
                    {
                        temp = oVenta.usuario.nombre;
                    }
                    return lstVenta;
                }
            }
            catch (Exception)
            {
                return new List<venta>();
            }
        }
    }
}