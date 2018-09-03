using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace softcomputacion.Models
{
    public class ReporteRankingGastoCliente
    {
        public string nombreCliente { get; set; }
        public decimal gastoTotal { get; set; }
        public int cantidadVentas { get; set; }
    }
}