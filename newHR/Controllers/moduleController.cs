using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class moduleController : Controller
    {
        // GET: module
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Setting()
        {
            return View();
        }
        public ActionResult Module()
        {
            return View();
        }
        public ActionResult Module_list()
        {
            return View();
        }
    }
}