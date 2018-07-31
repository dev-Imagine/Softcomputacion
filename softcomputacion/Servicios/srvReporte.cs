using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;


namespace softcomputacion.Servicios
{
    public class srvReporte
    {

        //Reporte de productos.
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

    }
}