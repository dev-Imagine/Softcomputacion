using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvCliente
    {
        public List<cliente> BuscarClientes(string NombreApellido, string dni ="")
        {
            try
            {
                List<cliente> lstCliente = new List<cliente>();
                NombreApellido = NombreApellido.ToUpper();
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    if (dni.Trim().Length != 0)
                    {
                        lstCliente = bd.cliente.Where(x => x.dni == dni).ToList();
                        lstCliente = lstCliente.Where(x => x.nombre.Contains(NombreApellido) || x.apellido.Contains(NombreApellido)).ToList();
                    }
                    else
                    {
                        lstCliente = bd.cliente.Where(x => x.nombre.Contains(NombreApellido) || x.apellido.Contains(NombreApellido)).ToList();
                    }
                    
                    return lstCliente.OrderBy(x => x.apellido).ThenBy(x => x.nombre).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<venta> ObtenerVentas(int idCliente)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    List<venta> lstVenta = bd.venta.Where(x => x.idCliente == idCliente).OrderByDescending(x => x.fechaEmision).ToList();
                    string st = "";
                    foreach (venta oVenta in lstVenta)
                    {
                        foreach (detalleVenta oDetalle in oVenta.detalleVenta.ToList())
                        {
                            st = oDetalle.producto.nombre;
                        }
                        st = oVenta.estado.nombre;
                    }
                    return lstVenta;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public cliente ObtenerCliente(int idCliente)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    return bd.cliente.Where(x=> x.idCliente == idCliente).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        private cliente GuardarCliente(cliente oCliente)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oCliente.nombre = oCliente.nombre.ToUpper();
                    oCliente.apellido = oCliente.apellido.ToUpper();
                    bd.Entry(oCliente).State = System.Data.Entity.EntityState.Added;
                    bd.SaveChanges();
                    return oCliente;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private cliente ModificarCliente(cliente oCliente)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oCliente.nombre = oCliente.nombre.ToUpper();
                    oCliente.apellido = oCliente.apellido.ToUpper();
                    bd.Entry(oCliente).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                    return oCliente;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public cliente GuardarModificarCliente(cliente oCliente)
        {
            try
            {
                if (oCliente.idCliente == 0)
                {
                    oCliente.saldo = 0;
                    GuardarCliente(oCliente);
                }
                else
                {
                    ModificarCliente(oCliente);
                }
                return oCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}