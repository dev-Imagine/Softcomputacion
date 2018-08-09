using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using softcomputacion.Models;
using softcomputacion.Servicios;

namespace softcomputacion.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }

        //Metodos
        [HttpPost]
        public string llenarSessionCliente(string NombreApellido)
        {
            try
            {
                srvCliente sCliente = new srvCliente();
                List<cliente> lstCliente = sCliente.BuscarClientes(NombreApellido);
                Session["Clientes"] = lstCliente;
                if (lstCliente.Count == 0)
                {
                    return "0";
                }
                else
                {
                    return "1";
                }
            }
            catch (Exception)
            {
                return "0";
            }
        }

        //VistasParciales
        [HttpPost]
        public PartialViewResult _PopUpBuscarCliente()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult _ListaCliente()
        {
            return PartialView();
        }

    }
}