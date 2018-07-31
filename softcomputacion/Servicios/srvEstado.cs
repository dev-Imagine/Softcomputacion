using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvEstado
    {
        public List<estado> ObtenerEstados()
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    return bd.estado.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string ObtenerColorEstado(int idEstado)
        {
            switch (idEstado)
            {
                case 1:
                    return "#28a745";
                case 2:
                    return "#ffc107";
                case 3:
                    return "#dc3545";
                default:
                    return "#000";
            }
        }
    }
}