using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvMetodoPago
    {
        public List<metodoPago> ObtenerMetodosPago()
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    return bd.metodoPago.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}