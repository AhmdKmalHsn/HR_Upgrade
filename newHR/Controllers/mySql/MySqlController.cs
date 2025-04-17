using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers.mySql
{
    public class MySqlController : Controller
    {
        // GET: MySql
        string connString = "Server=myserver;User ID=mylogin;Password=mypass;Database=mydatabase";

        
    public ActionResult Index()
        {
            MySqlConnection connection = new MySqlConnection(connString);
            connection.OpenAsync();
            return View();
        }
    }
}