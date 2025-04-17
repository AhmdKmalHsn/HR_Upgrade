using newHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace newHR.Controllers
{
    public class SSEController : Controller
    {
        // GET: SSE
        public ActionResult Index()
        {
            return View();
        }
        DBContext db = new DBContext();
        public ActionResult SSE(string userid)
        {
            /*= "sameh";
            DateTime n = new DateTime();*/
            var sb = new StringBuilder();
            var data = db.getData("select count(*)cnt from Requests where (Auth1User ='" + userid + "' and Auth1Status = 0) or (Auth2User ='" + userid + "' and Auth2Status = 0 and Auth1Status = 1)");
            // var emptyData = db.getData("select 0 cnt");
            JavaScriptSerializer ser = new JavaScriptSerializer();
            if (!string.IsNullOrEmpty(StreamModel.dataHolder))
            {
                var serializedObject = ser.Serialize(new { result = data.Rows[0][0]/*StreamModel.dataHolder*/, status = "success" });
                sb.AppendFormat("data: {0}\n\n", serializedObject);
            }
            else
            {
                var serializedObject = ser.Serialize(new { result = data.Rows[0][0] /*emptyData.Rows[0][0]*/, status = "not found" });
                sb.AppendFormat("data: {0}\n\n", serializedObject);
            }
            return Content(sb.ToString(), "text/event-stream");
        }
        public ActionResult SSE2()
        {
            var data = db.getData(@"
                select e.FileNumber,a.* from Attendances a join Employees e on a.EmployeeId=e.id
where (TimeFrom='' or TimeTo='') and DateFrom<dateadd(day,-3,GETDATE())
and
(
    ( DATEPART(day,DateFrom)<26 and  datefrom>=cast(cast(DATEPART(YEAR,GETDATE())as varchar(4))+'-'+cast(DATEPART(MONTH,dateadd(month,-1,GETDATE())) as varchar(2))+'-'+'26' as datetime))
    or
    ( DATEPART(day,DateFrom)>=26 and  datefrom>=cast(cast(DATEPART(YEAR,GETDATE())as varchar(4))+'-'+cast(DATEPART(MONTH,GETDATE()) as varchar(2))+'-'+'26' as datetime))
)
                ");
            var sb = new StringBuilder();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            if (!string.IsNullOrEmpty(StreamModel.dataHolder))
            {
                var serializedObject = ser.Serialize(new { result = data, status = "success" });
                sb.AppendFormat("data: {0}\n\n", serializedObject);
            }
            else
            {
                //var serializedObject = ser.Serialize(new { result = data, status = "not found" });
                //sb.AppendFormat("data: {0}\n\n", serializedObject);
            }
            return Content(sb.ToString(), "text/event-stream");
        }

        public ActionResult SSE_CNT()
        {
            var data = db.getData(@"
                select count(*) cnt from (
select e.FileNumber,a.* from Attendances a join Employees e on a.EmployeeId=e.id
where (TimeFrom='' or TimeTo='') and DateFrom<dateadd(day,-3,GETDATE())
and
(
    ( DATEPART(day,DateFrom)<26 and  datefrom>=cast(cast(DATEPART(YEAR,GETDATE())as varchar(4))+'-'+cast(DATEPART(MONTH,dateadd(month,-1,GETDATE())) as varchar(2))+'-'+'26' as datetime))
    or
    ( DATEPART(day,DateFrom)>=26 and  datefrom>=cast(cast(DATEPART(YEAR,GETDATE())as varchar(4))+'-'+cast(DATEPART(MONTH,GETDATE()) as varchar(2))+'-'+'26' as datetime))
)
)q
                ");
            var sb = new StringBuilder();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            if (!string.IsNullOrEmpty(StreamModel.dataHolder))
            {
                var serializedObject = ser.Serialize(new { result = data, status = "success" });
                sb.AppendFormat("data: {0}\n\n", serializedObject);
            }
            else
            {
                //var serializedObject = ser.Serialize(new { result = data, status = "not found" });
                //sb.AppendFormat("data: {0}\n\n", serializedObject);
            }
            return Content(sb.ToString(), "text/event-stream");
        }

    }

}