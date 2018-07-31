using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvProveedor
    {
        private proveedor GuardarProveedor(proveedor oProveedor)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oProveedor.nombreEmpresa = oProveedor.nombreEmpresa.ToUpper();
                    bd.Entry(oProveedor).State = System.Data.Entity.EntityState.Added;
                    bd.SaveChanges();
                    return oProveedor;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private proveedor ModificarProveedor(proveedor oProveedor)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oProveedor.nombreEmpresa = oProveedor.nombreEmpresa.ToUpper();
                    bd.Entry(oProveedor).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                    return oProveedor;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public proveedor GuardarModificarProveedor(proveedor oProveedor)
        {
            try
            {
                if (oProveedor.idProveedor==0)
                {
                    GuardarProveedor(oProveedor);
                }
                else
                {
                    ModificarProveedor(oProveedor);
                }
                return oProveedor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public proveedor ObtenerProveedor(int idProveedor)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    return bd.proveedor.Where(x => x.idProveedor == idProveedor && x.fechaBaja == null).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<proveedor> ObtenerProveedores()
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    return bd.proveedor.Where(x=> x.fechaBaja==null).OrderBy(x => x.nombreEmpresa).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<proveedor> ObtenerProveedores(string nombreEmpresa)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    if (nombreEmpresa == "")
                    {
                        return bd.proveedor.ToList();
                    }
                    return bd.proveedor.Where(x => x.nombreEmpresa.Contains(nombreEmpresa.ToUpper()) && x.fechaBaja == null).OrderBy(x => x.nombreEmpresa).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public proveedor EliminarProveedor(int idProveedor)
        {
            try
            {
                proveedor oProveedor = ObtenerProveedor(idProveedor);
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oProveedor.fechaBaja = DateTime.Now;
                    ModificarProveedor(oProveedor);
                }
                return oProveedor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}