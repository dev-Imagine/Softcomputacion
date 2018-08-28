using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvCategoria
    {
        public List<categoria> ObtenerCategorias()
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    return bd.categoria.OrderBy(x => x.nombre).ToList();
                }
            }
            catch (Exception)
            {
                return new List<categoria>();
            }
        }
        public categoria ObtenerCategoria(int idCategoria)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    categoria oCategoria = bd.categoria.Where(x => x.idCategoria == idCategoria).FirstOrDefault();
                    oCategoria.subcategoria.Count();
                    return oCategoria;
                }
            }
            catch (Exception)
            {
                return new categoria();
            }
        }
        public categoria GuardarModificarCategoria (categoria oCategoria)
        {
            if (oCategoria.idCategoria ==0)
            {
                oCategoria = GuardarCategoria(oCategoria);
            }
            else
            {
                oCategoria = ModificarCategoria(oCategoria); //falta actualizar detalle
            }
            return oCategoria;
        }
        private categoria GuardarCategoria(categoria oCategoria)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {   
                    bd.Entry(oCategoria).State = System.Data.Entity.EntityState.Added;
                    bd.SaveChanges();
                    return oCategoria;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private categoria ModificarCategoria (categoria oCategoria)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    List<subcategoria> lstSubcategoria = oCategoria.subcategoria.ToList();
                    oCategoria.subcategoria.Clear();
                    bd.Entry(oCategoria).State = System.Data.Entity.EntityState.Modified;
                    foreach (subcategoria oSubCategoria in lstSubcategoria)
                    {
                        if (oSubCategoria.idSubCategoria == 0)
                        {
                            bd.Entry(oSubCategoria).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            bd.Entry(oSubCategoria).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    bd.SaveChanges();
                    return oCategoria;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EliminarSubcategoria(int idSubCategoria)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    subcategoria oSubcategoria = bd.subcategoria.Where(x => x.idSubCategoria == idSubCategoria).FirstOrDefault();
                    if (oSubcategoria.producto.Count == 0)
                    {
                        bd.subcategoria.Remove(oSubcategoria);
                        bd.SaveChanges();
                    }
                    else
                    {
                        throw new Exception();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}