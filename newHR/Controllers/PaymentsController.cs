using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class PaymentsController : Controller
    {
        DBContext db = new DBContext();
        // GET: Payments
        public ActionResult Index()
        {
            return View();
        }

        // GET: Payments/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var sql = @"insert into Deductions(Date,Amount,PaymentDeductionId,RecurringOneTimeId,TotalNet,EmployeeId,Remarkes,DaysNumber)
		values(@Date,@Amount,@PaymentDeductionId,@RecurringOneTimeId,@TotalNet,(select id from employees where FileNumber= @EmployeeId),@Remarkes,@DaysNumber)
declare @deductId int =(select SCOPE_IDENTITY() )
;with data 
as(
		select 1 i,@Date Date,case when @TotalNet=0 then @Amount/@RecurringOneTimeId else @Amount end Amount,@PaymentDeductionId PaymentDeductionId,@RecurringOneTimeId RecurringOneTimeId,@TotalNet TotalNet,(select id from employees where FileNumber= @EmployeeId) EmployeeId,@Remarkes Remarkes,@DaysNumber DaysNumber,@deductId deductId
        union all
		select i+1,dateadd(month,1,Date),Amount,PaymentDeductionId,RecurringOneTimeId,TotalNet,EmployeeId,Remarkes,DaysNumber,deductId
		from data
		where i<RecurringOneTimeId
)
insert into   Payments( Date,	Amount,	PaymentDeductionId,	RecurringOneTimeId,	TotalNet,	EmployeeId,	Remarkes,   DaysNumber,	DeductionId)
select Date,	Amount,	PaymentDeductionId,	RecurringOneTimeId,	TotalNet,	EmployeeId,	Remarkes,	DaysNumber,	deductId from data";
                SqlCommand com = new SqlCommand(sql);
                com.Parameters.Add("@Date",SqlDbType.Date).Value = collection["date"];
                com.Parameters.Add("@Amount",SqlDbType.Float).Value = collection["amount"];
                com.Parameters.Add("@PaymentDeductionId",SqlDbType.Int).Value = collection["type"];
                com.Parameters.Add("@RecurringOneTimeId",SqlDbType.Int).Value = collection["times"];
                com.Parameters.Add("@TotalNet",SqlDbType.Int).Value = collection["totalNet"];
                com.Parameters.Add("@EmployeeId",SqlDbType.Int).Value = collection["fileNo"];
                com.Parameters.Add("@Remarkes", SqlDbType.VarChar).Value = (collection["remarks"] == null ? "null" : collection["remarks"]);
                com.Parameters.Add("@DaysNumber", SqlDbType.Float).Value = (collection["daysNumber"] == null ? "null" : collection["daysNumber"]);
                db.exec(com);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Payments/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int id)
        {
            
            SqlCommand cmd = new SqlCommand("delete from deductions where id=" + id + " delete from payments where DeductionId=" + id);
            db.exec(cmd);
            return RedirectToAction("Index");
        }

        // POST: Payments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
