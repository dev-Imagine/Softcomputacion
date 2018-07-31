using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvVenta
    {
        public venta agregarDetalle(venta oVenta, int idProducto, double precio, int cantidad)
        {
            try
            {
                using (BDModuloVentaEntities bd = new BDModuloVentaEntities())
                {
                    detalleVenta oDetalle = new detalleVenta();
                    oDetalle.idProducto = idProducto;
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