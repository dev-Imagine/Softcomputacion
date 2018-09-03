using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvDetallePago
    {
        public List<detallePago> obtenerDetallePagoCliente(int idCliente) {

            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    string temp;
                    List<detallePago> lstDetallePago = new List<detallePago>();
                    lstDetallePago = bd.detallePago.Where(x => x.venta.cliente.idCliente == idCliente).ToList().OrderByDescending(x => x.fechaPago).ToList();
                    foreach (detallePago oDet in lstDetallePago)
                    {
                        temp = oDet.venta.cliente.nombre;
                        temp = oDet.metodoPago.nombre;
                    }
                    return lstDetallePago;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}