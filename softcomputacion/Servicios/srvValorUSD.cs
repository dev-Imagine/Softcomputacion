using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using softcomputacion.Models;

namespace softcomputacion.Servicios
{
    public class srvValorUSD
    {
        public double ObtenerValorUsd()
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    valorUSD valor = bd.valorUSD.FirstOrDefault();
                    return Convert.ToDouble(valor.valorUSD1);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public double ActualizarValorUsd(double valorUSD)
        {
            try
            {
                using (BDSoftComputacionEntities bd = new BDSoftComputacionEntities())
                {
                    valorUSD oValor = bd.valorUSD.FirstOrDefault();
                    oValor.valorUSD1 = Convert.ToDecimal(valorUSD);
                    bd.Entry(oValor).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                    return Convert.ToDouble(oValor.valorUSD1);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}