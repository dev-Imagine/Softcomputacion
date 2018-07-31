using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace softcomputacion.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string stError ="")
        {
            ViewBag.stMensajeError = stError;
            return View();
        }
    }
}