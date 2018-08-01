using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvProducto
    {
        private producto GuardarProducto(producto oProducto)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oProducto.nombre = oProducto.nombre.ToUpper();
                    bd.Entry(oProducto).State = System.Data.Entity.EntityState.Added;
                    bd.SaveChanges();
                    return oProducto;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private producto ModificarProducto(producto oProducto)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oProducto.nombre = oProducto.nombre.ToUpper();
                    oProducto.estado = bd.estado.Where(x => x.idEstado == oProducto.idEstado).FirstOrDefault();
                    bd.Database.ExecuteSqlCommand("DELETE FROM proveedorXproducto WHERE idProducto = @id", new System.Data.SqlClient.SqlParameter("id", oProducto.idProducto));
                    foreach (proveedorXproducto oPxp in oProducto.proveedorXproducto.ToList())
                    {
                        bd.Entry(oPxp).State = System.Data.Entity.EntityState.Added;
                    }
                    oProducto.proveedorXproducto.Clear();
                    bd.Entry(oProducto).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                    return oProducto;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public producto GuardarModificarProducto(producto oProducto)
        {
            try
            {
                oProducto.nombre = oProducto.nombre.ToUpper();
                oProducto.estado = null;
                if (oProducto.stockActual < 0 || oProducto.stockIdeal < 0 || oProducto.stockMinimo < 0)
                {
                    throw new Exception();
                }
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
                if (oProducto.idProducto == 0)
                {
                    GuardarProducto(oProducto);
                }
                else
                {
                    ModificarProducto(oProducto);
                }
                return oProducto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<producto> ObtenerProductos(string nombre = "", int idCategoria = 0, int idSubCategoria=0, int idEstado=0, int idProducto = 0)
        {
            try
            {
                List<producto> lstProductos = new List<producto>();
                nombre = nombre.ToUpper();
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    if (idProducto != 0)
                    {
                        lstProductos = bd.producto.Where(x => x.idProducto == idProducto).ToList();
                        foreach (producto opro in lstProductos)
                        {
                            int id;
                            id = opro.categoria.idCategoria;
                            id = opro.subcategoria.idSubCategoria;
                            id = opro.subcategoria.categoria.idCategoria;
                            id = opro.estado.idEstado;
                            id = opro.proveedorXproducto.Count();

                        }
                        return lstProductos;
                    }
                    if (nombre == "") lstProductos = bd.producto.ToList();
                    else lstProductos = bd.producto.Where(x => x.nombre.Contains(nombre)).ToList();
                    if (idCategoria != 0) lstProductos = lstProductos.Where(x => x.idCategoria == idCategoria).ToList();
                    if (idSubCategoria != 0) lstProductos = lstProductos.Where(x => x.idSubCategoria == idSubCategoria).ToList();
                    if (idEstado != 0) lstProductos = lstProductos.Where(x => x.idEstado == idEstado).ToList();
                    foreach (producto opro in lstProductos)
                    {
                        int id;
                        id = opro.categoria.idCategoria;
                        id = opro.subcategoria.idSubCategoria;
                        id = opro.subcategoria.categoria.idCategoria;
                        id = opro.estado.idEstado;
                        id = opro.proveedorXproducto.Count();

                    }
                    return lstProductos.OrderBy(x => x.categoria.nombre + " - " + x.nombre).ToList();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public producto ObtenerProducto(int idProducto)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    producto oProducto = bd.producto.Where(X => X.idProducto == idProducto).FirstOrDefault();
                    string st = oProducto.subcategoria.nombre;
                    st = oProducto.categoria.nombre;
                    st = oProducto.estado.nombre;
                    foreach (proveedorXproducto oPxp in oProducto.proveedorXproducto.ToList())
                    {
                        st = oPxp.proveedor.nombreEmpresa;
                    }
                    return oProducto;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void QuitarProveedorProducto(int idPxp)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    proveedorXproducto oPxp = bd.proveedorXproducto.Where(x => x.idProveedorXproducto == idPxp).FirstOrDefault();
                    bd.proveedorXproducto.Remove(oPxp);
                    bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}