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
            
                return View();
            
        }
        public ActionResult Log()
        {
            
                return View();
           
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
        public ActionResult AK_LogIn(string username = "", string password = "")
        {
            string clientIP = Request.UserHostAddress;
            string sql = @"SELECT u.Id userId,*
                           from AK_Users u 
                           where u.UserName='" + username + "' and u.password='" + password + "'";
            string output = "[{\"status\" : \"error\" ,\"message\" : \"user name or password is error\"}]";
            var ds = static_class.getbysql(sql);
            if (ds.Tables["status"].Rows[0][0].ToString() == "success")
            {
                //output = JsonConvert.SerializeObject(ds, Formatting.Indented);
                if (ds.Tables["data"].Rows.Count > 0)
                {
                    Guid g = Guid.NewGuid();
                    string GuidString = Convert.ToBase64String(g.ToByteArray());
                    GuidString = GuidString.Replace("=", "");
                    GuidString = GuidString.Replace("+", "");
                    DataSet update;
                    if (ds.Tables["data"].Rows[0]["login_permit"].ToString() == "")
                    {
                        update = static_class.updatebysql(
                                @"update AK_Users set login_at ='" + DateTime.Now +
                                "',login_to ='" + DateTime.Now.AddMinutes(24 * 60) +
                                "',token ='" + GuidString +
                                "' where id=" + ds.Tables["data"].Rows[0]["Id"]
                                );
                    }
                    else
                    {
                        update = static_class.updatebysql(
                                @"update AK_Users set login_at='" + DateTime.Now +
                                "',login_to='" + DateTime.Now.AddMinutes(Convert.ToInt32(ds.Tables["data"].Rows[0]["login_permit"])) +
                                "',token ='" + GuidString +
                                "' where id=" + ds.Tables["data"].Rows[0]["Id"]
                                );
                    }
                    static_class.insertbysql(
                                @"INSERT INTO dbo.AK_logins
                                (
                                  userid
                                 ,username
                                 ,login_from
                                 ,token
                                )
                                VALUES
                                ('" +
                                 ds.Tables["data"].Rows[0]["Id"] + "','" +
                                 ds.Tables["data"].Rows[0]["UserName"] + "','" +
                                 Request.UserHostAddress + "','" +
                                 GuidString + "'" +
                                 ");"
                                );

                    HttpCookie token = new HttpCookie("token", GuidString);
                    HttpCookie userName = new HttpCookie("UserName", ds.Tables["data"].Rows[0]["UserName"].ToString());
                    token.Expires = DateTime.Now.AddDays(1);
                    userName.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(token);
                    Response.Cookies.Add(userName);
                    output = JsonConvert.SerializeObject(update.Tables[0]);
                }
            }

            //return Content(/*, "application/json"*/); 
            return Content(output, "application/json");
        }
        public ActionResult AK_LogOut()
        {
            Response.Cookies["userName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["token"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("log", "home");
        }
        public ActionResult test()
        {
            DataSet ds;
            try
            {
                ds = static_class.is_Authrize("shifts", Request.Cookies.Get("token").Value, "p_access");
            }
            catch (Exception)
            {
                return RedirectToAction("Log", "Home");
            }
            if (ds.Tables[0].Rows[0]["status"].ToString() == "error")
            {
                return View("_NotAuthorized");
            }
            else
            {
                ViewBag.perms = static_class.o_Authrizes(Request.Cookies.Get("token").Value);//.AsEnumerable().ToList();
                return View();
            }
            // return Content(JsonConvert.SerializeObject(ds), "application/json");
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
            SqlCommand cmd = new SqlCommand(@"select COUNT(*)cnt,'emp' name from Employees e ,Personals p where e.PersonalId=p.Id and p.StatusId=1
union ALL
select count(*)cnt, 'depts' name from Departements
 union ALL
                                 select count(*)cnt, 'users' name from Users2
                                  union all
                                                                  select count(*)cnt, 'mac' name from Mac
                                                                   ");
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
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

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