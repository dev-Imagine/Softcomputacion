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
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    detalleVenta oDetalle = new detalleVenta();
                    srvProducto sProducto = new srvProducto();
                    oDetalle.idProducto = idProducto;
                    oDetalle.producto = sProducto.ObtenerProducto(idProducto);
                    string nombre = oDetalle.producto.nombre;
                    oDetalle.costoIndividual = Convert.ToDecimal(precio);
                    oDetalle.costoGrupal = Convert.ToDecimal(precio * cantidad);
                    oDetalle.cantidad = cantidad;
                    oVenta.detalleVenta.Add(oDetalle);
                    return oVenta;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}