using softcomputacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace softcomputacion.Servicios
{
    public class srvReparacion
    {
        public List<reparacion> ObtenerReparaciones(int idReparacion = 0, int idUsuario = 0, int idEstado = 0, string stCliente = "")
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    
                    List<reparacion> lstReparacion;
                    if (idReparacion != 0)
                    {
                        lstReparacion = bd.reparacion.Where(x => x.idReparacion == idReparacion).ToList();
                    }
                    else
                    {
                        if (idUsuario == 0)
                        {
                            lstReparacion = bd.reparacion.ToList();
                        }
                        else
                        {
                            lstReparacion = bd.reparacion.Where(x => x.idUsuario == idUsuario).ToList();
                        }
                        if(stCliente.Length > 0)
                        {
                            lstReparacion = lstReparacion.Where(x => (x.cliente.nombre + x.cliente.apellido).Contains(stCliente)).ToList();
                        }
                        if (idEstado > 0)
                        {
                            lstReparacion = lstReparacion.Where(x => x.idEstado == idEstado).ToList();
                        }

                    }

                    string temp = "";
                    foreach (reparacion oReparacion in lstReparacion.ToList())
                    {
                        temp = oReparacion.cliente.nombre;
                        temp = oReparacion.usuario.nombre;
                        temp = oReparacion.estado.nombre;
                        oReparacion.producto.Count();
                    }
                    return lstReparacion.OrderByDescending(x => x.fecha).ToList();
                }
            }
            catch (Exception)
            {
                return new List<reparacion>();
            }
        }
    }
}