using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Runtime.Remoting.Contexts;

namespace newHR.Controllers
{
    public class HomeController : Controller
    {
        HttpCookie UserId = new HttpCookie("UserId");
        HttpCookie UserName = new HttpCookie("UserName");
        HttpCookie Name = new HttpCookie("name");
        HttpCookie Vacation = new HttpCookie("Vacation");
        HttpCookie Mission = new HttpCookie("Mission");
        HttpCookie Attendance = new HttpCookie("Attendance");
        HttpCookie OverTime = new HttpCookie("OverTime");
        HttpCookie Salary = new HttpCookie("Salary");
        HttpCookie ClosingPeriod = new HttpCookie("ClosingPeriod");
        HttpCookie FingerMachine = new HttpCookie("FingerMachine");
        HttpCookie Reports_Abs = new HttpCookie("Reports_Abs");
        HttpCookie Reports_Salary_Details = new HttpCookie("Reports_Salary_Details");
        HttpCookie Logs = new HttpCookie("Logs");
        HttpCookie TempShifts = new HttpCookie("TempShifts");

        public ActionResult Index()
        {
            //repeat state 

            return static_class.GetView(this,"Home");
            
        }
        /**************************************/
        public ActionResult LogIn()
        {

            string sql = @"SELECT u.Id userId,UserName,isnull(HaveApprove,0)HaveApprove,isnull(ApproveId,0)ApproveId,r.* 
                           from Users2 u join Roles2 r on r.id=u.RoleId
                           where u.UserName=@UserName and u.password=@Password
                           ";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Request["UserName"];
                com.Parameters.Add("@Password", SqlDbType.VarChar).Value = Request["Password"];
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {


                    UserId.Value = rdr["userId"].ToString();
                    UserName.Value = rdr["UserName"].ToString();
                    Name.Value = rdr["name"].ToString();
                    Vacation.Value = rdr["Vacation"].ToString();
                    Mission.Value = rdr["Mission"].ToString();
                    Attendance.Value = rdr["Attendance"].ToString();
                    OverTime.Value = rdr["OverTime"].ToString();
                    Salary.Value = rdr["Salary"].ToString();
                    ClosingPeriod.Value = rdr["ClosingPeriod"].ToString();
                    FingerMachine.Value = rdr["FingerMachine"].ToString();
                    Reports_Abs.Value = rdr["Reports_Abs"].ToString();
                    Reports_Salary_Details.Value = rdr["Reports_Salary_Details"].ToString();
                    Logs.Value = rdr["Logs"].ToString();
                    TempShifts.Value = rdr["TempShifts"].ToString();
                    UserId.Expires =
                    UserName.Expires =
                    Name.Expires =
                    Vacation.Expires =
                    Mission.Expires =
                    Attendance.Expires =
                    OverTime.Expires =
                    Salary.Expires =
                    ClosingPeriod.Expires =
                    FingerMachine.Expires =
                    Reports_Abs.Expires =
                    Reports_Salary_Details.Expires =
                    Logs.Expires =
                    TempShifts.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Clear();
                    Response.Cookies.Add(UserId);
                    Response.Cookies.Add(UserName);
                    Response.Cookies.Add(Name);
                    Response.Cookies.Add(Vacation);
                    Response.Cookies.Add(Mission);
                    Response.Cookies.Add(Attendance);
                    Response.Cookies.Add(OverTime);
                    Response.Cookies.Add(Salary);
                    Response.Cookies.Add(ClosingPeriod);
                    Response.Cookies.Add(FingerMachine);
                    Response.Cookies.Add(Reports_Abs);
                    Response.Cookies.Add(Reports_Salary_Details);
                    Response.Cookies.Add(Logs);
                    Response.Cookies.Add(TempShifts);


                    if (rdr.HasRows) return RedirectToAction("/index");
                }

            }
            return View("Log");

        }
        public ActionResult meals()
        {
            DataSet ds = static_class.is_Authrize("meals", Request.Cookies.Get("token").Value, "p_access");
            if (ds.Tables[0].Rows[0]["status"].ToString() == "error")
                return View("_NotAuthorized"); //Content(JsonConvert.SerializeObject(ds.Tables[0]), "application/json");

            else
                return View(ds);
        }
        public ActionResult LogOut()
        {
            Response.Cookies["userId"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["name"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Vacation"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Mission"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Attendance"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["OverTime"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Salary"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["ClosingPeriod"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["FingerMachine"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Reports_Abs"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Reports_Salary_Details"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Logs"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["TempShifts"].Expires = DateTime.Now.AddDays(-1);

            return RedirectToAction("log");
        }

        DBContext db = new DBContext();
        public JsonResult dashboard()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT
  COUNT(e.Id) cnt
 ,'emp' name
FROM Employees e LEFT JOIN Personals p ON e.PersonalId=p.Id
WHERE e.PersonalId = p.Id
AND p.StatusId = 1
UNION ALL
SELECT
  COUNT(*) cnt
 ,'depts' name
FROM Departements
UNION ALL
SELECT
  COUNT(*) cnt
 ,'users' name
FROM ak_Users
UNION ALL
SELECT
  COUNT(*) cnt
 ,'branches' name
 FROM  Locations l");
            return Json(db.toJSON(db.getData(cmd)), JsonRequestBehavior.AllowGet);
        }

        /* upload image */

        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname = file.FileName;

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public ActionResult license()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}