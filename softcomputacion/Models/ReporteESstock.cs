using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace softcomputacion.Models
{
    public class ReporteESstock
    {
        public int mes { get; set; }
        public int año { get; set; }
        public int idProducto { get; set; }
        public string productoNombre { get; set; }
        public int salida { get; set; }
        public int entrada { get; set; }
        public decimal ventaTotal { get; set; }
        public int cantVentaTotal { get; set; }
        public decimal ingresoNeto { get; set; }   
        public decimal abonadoTotal { get; set; }

    }
}