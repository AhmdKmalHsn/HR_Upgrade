using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class testController : Controller
    {
        // GET: test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult is_authen()
        {
            return Content(""+static_class.is_Authenticated(Request.Cookies.Get("token").Value));
        }
    }
}