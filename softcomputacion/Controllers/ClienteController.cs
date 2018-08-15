using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using softcomputacion.Models;
using softcomputacion.Servicios;

namespace softcomputacion.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult ListarClientes(int nroPagina = 1, int tamañoPagina = 10, bool paginacion = false)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                List<cliente> lstCliente = (List<cliente>)Session["lstCliente"];
                if (lstCliente == null || lstCliente.Count == 0 || paginacion == false)
                {
                    srvCliente sCliente = new srvCliente();
                    lstCliente = new List<cliente>();
                    Session["lstCliente"] = lstCliente;
                }
                ViewBag.filtros = ";";
                PagedList<cliente> ModelClientes = new PagedList<cliente>(lstCliente.ToList(), nroPagina, tamañoPagina);
                return View(ModelClientes);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }
        [HttpPost]
        public ActionResult ListarClientes(string nombreApellido = "", string dni="")
        {
            ViewBag.filtros = nombreApellido + ";" + dni;
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvCliente sCliente = new srvCliente();
                List<cliente> lstClientes = sCliente.BuscarClientes(nombreApellido, dni);
                
                
                Session["lstClientes"] = lstClientes;
                PagedList<cliente> ModelClientes = new PagedList<cliente>(lstClientes.ToList(), 1, 10);
                return View(ModelClientes);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
        }
        [HttpPost]
        public ActionResult ListaVentasCliente(int idCliente)
        {
            try
            {
                usuario oUsuario = (usuario)Session["Usuario"];
                if (oUsuario == null)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                srvCliente sCliente = new srvCliente();
                cliente oCliente = sCliente.ObtenerCliente(idCliente);
                if (oCliente == null || oCliente.idCliente == 0)
                {
                    throw new Exception();
                }
                ViewBag.oCliente = oCliente;
                List <venta> lstVentas = sCliente.ObtenerVentas(idCliente);
                return View(lstVentas);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar obtener los datos del servidor." });
            }
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