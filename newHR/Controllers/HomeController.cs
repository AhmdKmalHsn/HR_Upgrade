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

namespace newHR.Controllers
{
    public class HomeController : Controller
    {
        HttpCookie UserId = new HttpCookie("UserId");
        HttpCookie User = new HttpCookie("UserName");
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
        public ActionResult Log()
        {
            return View();
        }
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
                    /*Session["userId"] = rdr["userId"].ToString(); 
                    Session["User"] = rdr["UserName"].ToString();
                    Session["name"] = rdr["name"].ToString();
                    Session["Vacation"] = rdr["Vacation"].ToString();
                    Session["Mission"] = rdr["Mission"].ToString();
                    Session["Attendance"] = rdr["Attendance"].ToString();
                    Session["OverTime"] = rdr["OverTime"].ToString();
                    Session["Salary"] = rdr["Salary"].ToString();
                    Session["ClosingPeriod"] = rdr["ClosingPeriod"].ToString();
                    Session["FingerMachine"] = rdr["FingerMachine"].ToString();
                    Session["Reports_Abs"] = rdr["Reports_Abs"].ToString();
                    Session["Reports_Salary_Details"] = rdr["Reports_Salary_Details"].ToString();
                    Session["Logs"] = rdr["Logs"].ToString();
                    Session["TempShifts"] = rdr["TempShifts"].ToString();*/
                    //HttpCookie User = new HttpCookie("user");


                    UserId.Value = rdr["userId"].ToString();
                    User.Value = rdr["UserName"].ToString();
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
                    User.Expires =
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
                    Response.Cookies.Add(User);
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
    }
}