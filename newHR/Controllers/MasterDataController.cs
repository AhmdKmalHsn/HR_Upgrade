//using Microsoft.AspNet.SignalR.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class MasterDataController : Controller
    {
        // GET: MasterData
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Employees()
        {
            return View();
        }
        public ActionResult BasicPayWorks()
        {
            return View();
        }
        public ActionResult JobHistory()
        {
            return View();
        }
        public ActionResult Shifts()
        {
            return View();
        }
        public ActionResult Shifts_index()
        {
            return View();
        }
        public ActionResult Shift()
        {
            return View();
        }
        public ActionResult Shift_entry()
        {
            return View();
        }
        public ActionResult OfficialVacations()
        {
            return View();
        }
        public ActionResult jobs()
        {
            return View();
        }
        public ActionResult insuAuth()
        {
            return View();
        }
        public ActionResult depts()
        {
            return View();
        }
        public ActionResult edus()
        {
            return View();
        }

        DBContext db = new DBContext();
        public JsonResult GetEmployees()//AK
        {
            #region query
            string sql = @"
select e.Id,
      e.KnownAs'name',
      e.FileNumber,
      e.Image,
      e.notes 'name2',
      e.BankCode,
      e.BankCode2,
      ShiftId'shift',
      TotalSalary'salary',
      Overtime,
      SkillIncentive'skill',
      ExpensiveLivingConditons'expensive',
      RegularityIncentive'regular',
      IncentiveIncentiveForAbsence'managment',
      ConstValue'const',
      NumberOfDays'days',
      isNull(b.withinSalary,0)'withinSalary',
      isNull(b.isProMonthly,0)'isProMonthly',
      isNull(b.isProWeekly,0)'isProWeekly',
      isNull(b.ProductivityDays,0)'ProductivityDays',
      isNull(b.ProductivityFixed,0)'ProductivityFixed',
      NumberOfStageVacations'deferredDays',
      VacationDeferredDate'deferredDate',
      d.id 'dept_id',
      d.Name 'dept',
      j.id 'job_id',
      j.Name 'job',
      datediff(year,p.DateOfBirth,GETDATE()) 'age' ,
      CAST(p.JoiningDate AS DATE)'JoiningDate',
      CAST(p.DateOfBirth AS DATE)'DateOfBirth',
      MaritalStatus,
      NumberOfChildren'kids',
      MobilePhone,
      HomePhone,
      IDNo,
      PassportNo,
      EducationStatusId EducationId,
      (select name from EducationalStatus where id=EducationStatusId)'Education',
      s.Name 'status',
      s.id 'status_id',
      i.EmployeeFixedSalary,
      CASE WHEN g.SocialInsuranceNumber IS NOT NULL THEN g.SocialInsuranceNumber 
      ELSE i.InsuranceNumber end 'InsuranceNumber',
      i.Percentage,
      re.name religion,
      cast (g.TerminationDate AS DATE) 'TerminationDate',
      g.TerminationReason,
      p.PersonalArea,	
      p.PersonalSubArea,
    	p.Division	,
      p.SubDivision,
      ad.Email,
      ad.HomeStreet,
      ad.HomeCityId,
      ad.HomeCountryId,
      p.EmployeeGroupId,
      p.EmployeeSubGroupId,
      b.LevelId,
      e.LocationId,
      hc.name 'HomeCity',
      hc1.name 'HomeCountry',
      eg.Name'EmployeeGroup',
      esg.Name'EmployeeSubGroup',
      l.LevelName 'Level',
      l1.Name 'Location'

      from Employees e left join BasicBayWorks b on b.EmployeeId=e.id
                      left join Departements d on d.Id=e.DepartementId
                      left join JobTitles j on j.Id=e.JobTitleId
                      left join Personals p on p.Id=e.PersonalId
                      left join XReligions re on p.xreligionId=re.Id
                      left join Finances f on f.id=e.FinanceId
                      left join Administrations a on a.Id=e.AdministrationId
                      left join Addresses ad on ad.id=e.AddressId
                      left join Generals g on g.id=e.GeneralId
                      left join Remarks r on r.id=e.RemarkId
                      left join status s on s.id=p.statusid
                      left join( select * from(select ROW_NUMBER()over (partition by employeeid order by InsuranceNumber desc,employeeId  ) n, * from InsuranceDetails)q where n=1 )i on i.EmployeeId=e.id
                      LEFT JOIN HomeCities hc ON ad.HomeCityId = hc.Id
                      LEFT JOIN HomeCountries hc1 ON ad.HomeCountryId = hc1.Id
                      LEFT JOIN EmployeeGroups eg ON p.EmployeeGroupId = eg.Id
                      LEFT JOIN EmployeeSubGroups esg ON p.EmployeeSubGroupId = esg.Id
                      LEFT JOIN Levels l ON b.LevelId=l.Id
                      LEFT JOIN Locations l1 ON e.LocationId=l1.Id
";
            #endregion
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            return new JsonResult() {
                Data = db.toJSON(db.getData(com)),
                JsonRequestBehavior =JsonRequestBehavior.AllowGet,
                MaxJsonLength=Int32.MaxValue
            };
            //return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        }
    }
}