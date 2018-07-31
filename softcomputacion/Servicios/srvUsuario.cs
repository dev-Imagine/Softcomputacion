using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvUsuario
    {
        public List<usuario> ObtenerUsuarios()
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    List<usuario> lstUsuario = bd.usuario.Where(x => x.fechaBaja == null).OrderBy(x => x.apellido).ToList();
                    string st = "";
                    foreach (usuario oUsuario in lstUsuario)
                    {
                        st = oUsuario.tipoUsuario.nombre;
                    }
                    return lstUsuario;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public usuario ObtenerUsuario(usuario oUsuario)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oUsuario = bd.usuario.Where(x => x.idUsuario == oUsuario.idUsuario).FirstOrDefault();
                    string st = oUsuario.tipoUsuario.nombre;
                    return oUsuario;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public usuario ObtenerUsuario(int idUsuario)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    usuario oUsuario = bd.usuario.Where(x => x.idUsuario == idUsuario).FirstOrDefault();
                    string st = oUsuario.tipoUsuario.nombre;
                    return oUsuario;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public usuario ObtenerUsuario(string dni, string contraseña)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    usuario oUsuario = bd.usuario.Where(x => x.dni == dni && x.contraseña == contraseña).FirstOrDefault();
                    string st = oUsuario.tipoUsuario.nombre;
                    return oUsuario;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private usuario GuardarUsuario(usuario oUsuario)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oUsuario.nombre = oUsuario.nombre.ToUpper();
                    oUsuario.apellido = oUsuario.apellido.ToUpper();

                    bd.Entry(oUsuario).State = System.Data.Entity.EntityState.Added;
                    bd.SaveChanges();
                    return oUsuario;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private usuario ModificarUsuario(usuario oUsuario)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oUsuario.nombre = oUsuario.nombre.ToUpper();
                    oUsuario.apellido = oUsuario.apellido.ToUpper();

                    bd.Entry(oUsuario).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                    return oUsuario;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public usuario GuardarModificarUsuario(usuario oUsuario)
        {
            try
            {
                if (oUsuario.idUsuario == 0)
                {
                    GuardarUsuario(oUsuario);
                }
                else
                {
                    ModificarUsuario(oUsuario);
                }
                return oUsuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public usuario EliminarUsuario(int idUsuario)
        {
            try
            {
                usuario oUsuario = ObtenerUsuario(idUsuario);
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    oUsuario.fechaBaja = DateTime.Now;
                    ModificarUsuario(oUsuario);
                }
                return oUsuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}