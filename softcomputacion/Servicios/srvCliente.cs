using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvCliente
    {
        public List<cliente> ObtenerClientes(int idCliente = 0 , string nombre = "", string apellido = "")
        {
            try
            {
                List<cliente> lstCliente = new List<cliente>();
                nombre = nombre.ToUpper();
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    if (idCliente != 0)
                    {
                        lstCliente = bd.cliente.Where(x => x.idCliente == idCliente).ToList();
                       
                        return lstCliente;
                    }
                    if (nombre == "") lstCliente = bd.cliente.ToList();
                    else lstCliente = bd.cliente.Where(x => x.nombre.Contains(nombre)).ToList();
                    if (apellido != "") lstCliente = lstCliente.Where(x => x.apellido == apellido).ToList();                    
                    
                    return lstCliente.OrderBy(x => x.apellido).ThenBy(x=> x.nombre).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}