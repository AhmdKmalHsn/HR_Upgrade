var ajax, ajaxC, dataDay;
var result = [],
  success = 0,
  failed = 0,
  element = 0;
//احضار البيانات مع للعامل
function getTable() {
  //console.clear();
  var sqlC1 = `select filenumber,
       knownas name,
       isnull(e.color,'')'emp_color',
       DepartementId,
       (select name from Departements where  id=DepartementId)dept,
       TotalSalary,
	   Overtime,
	   SkillIncentive,
	   ExpensiveLivingConditons,
	   RegularityIncentive,
	   IncentiveIncentiveForAbsence,
	   NumberOfVacationInYear,
	   NumberOfVacationInManth,
	   NumberOfStageVacations,
	   VacationDeferredDate,
	   p.StatusId,p.DateOfBirth,p.JoiningDate,isnull(p.TimeShit,0)timeshift,
	   sh.NoOfShifts+1'NoOfShifts',
	   sh.ShiftId,
	   sh.ShiftName,
	   sh.DailyHours,
	   ins.EmployeeFixedSalary,
	   ins.EmployeeFixedSalary*(select ofFixedSalartForEmployeeContrribution from InsurancePrecentages)/100 insurance
from employees e left join BasicBayWorks b on e.Id=b.EmployeeId
                 left join Shifts sh on
                 (
				 (sh.ShiftId=b.ShiftId and (select NoOfShifts from Shifts where ShiftId=b.ShiftId) =0) or
				 (sh.Shift2=b.Shiftid and (select NoOfShifts from Shifts where ShiftId=b.ShiftId) =1) or
				 (sh.Shift3=b.ShiftId and (select NoOfShifts from Shifts where ShiftId=b.ShiftId) =2)
				 )
				 left join (select id ,EmployeeId,EmployeeFixedSalary from InsuranceDetails i1 where id=(select max(id)id from InsuranceDetails i2 where i1.EmployeeId=i2.EmployeeId))ins on ins.EmployeeId=e.id
                 left join Personals p on p.id=e.PersonalId
where FileNumber=${$("#FN").val()}`;
  $("#loader").css("display", "block");

  let sqlD = `declare @Y varchar(4)='${$("#Y").val()}',@M varchar(2)='${$(
    "#M"
  ).val()}',@fno int ='${$("#FN").val()}'
declare @f date ,@t date;
set @f=
(
select 
case when start <15 then 
cast((@y+'-'+@m+'-'+cast(Start as varchar)) as date) 
else
dateadd(month,-1,cast((@y+'-'+@m+'-'+cast(Start as varchar)) as date))
end
from Months
)
set @t=(select dateadd(day,-1,dateadd(month,1,@f)));
with month_cte as
( 
select @f dateDay
union All
select dateadd(day,1,dateDay)
from month_cte
where dateDay<@t
)
select isnull((did.Amount),0)Amount,PaymentDeductions.Id,Name from 
(select sum(amount)Amount,PaymentDeductionId
from month_cte 
 join deductions p on EmployeeId=(select id from Employees where FileNumber=@fno) and p.Date=month_cte.dateDay 
group by PaymentDeductionId
)did right join PaymentDeductions on did.PaymentDeductionId=PaymentDeductions.Id
`;
  ajaxC = readSQL(sqlC1);
  console.log(ajaxC);
  FN = $("#FN").val();

  html = "";

  for (var i = 0; i < ajaxC.length; i++) {
    html += `<option style="text-align:right" value="${ajaxC[i].ShiftId}">${ajaxC[i].ShiftName}</option>`;
  }
  $("#shft").html(html);
  if (EMPS.includes("" + $("#FN").val())) {
    data = getdata($("#FN").val(), $("#M").val(), $("#Y").val(), 1);
    //console.log(data);
  } else {
    $("#loader").css("display", "none");
    alert("غير مسموح!");
  }
}

function getdata(fileNo, M, Y, Display) {
  //if(EMPS.includes(fileNo))

  var sqlC1 = `select filenumber,
  sh.DailyHours,
  isnull(e.color,'') 'emp_color', 
  knownas name,
  sh.ShiftId,
  DepartementId,
  (select name from Departements where  id=DepartementId)dept,
  (select Name from JobTitles where Id= e.JobTitleId)'job',
  TotalSalary monthlySalary ,
Overtime overtimeRate,
SkillIncentive skill,
ExpensiveLivingConditons expensive,
RegularityIncentive regular,
IncentiveIncentiveForAbsence manage,
NumberOfVacationInYear,
NumberOfVacationInManth,
NumberOfStageVacations,
isnull(ProductivityDays,0) ProDays,
isnull(ProductivityFixed,0) ProFixed,
 isnull(b.withinSalary,0) withinSalary,
VacationDeferredDate,
p.StatusId,p.DateOfBirth,p.JoiningDate,isnull(p.TimeShit,0)timeshift,
ins.EmployeeFixedSalary,
case when ins.Percentage > 0 then ins.EmployeeFixedSalary*Percentage else ins.EmployeeFixedSalary*(select ofFixedSalartForEmployeeContrribution from InsurancePrecentages)/100 end insurance
   from employees e left join BasicBayWorks b on e.Id=b.EmployeeId
             left join Shifts sh on sh.ShiftId=b.ShiftId
    left join (select id ,EmployeeId,isnull(EmployeeFixedSalary,0) EmployeeFixedSalary,i1.Percentage  from InsuranceDetails i1 where id=(select max(id)id from InsuranceDetails i2 where i1.EmployeeId=i2.EmployeeId))ins on ins.EmployeeId=e.id
            left join Personals p on p.id=e.PersonalId
   where FileNumber=${fileNo}`;
  let sqlD1 = `declare @Y varchar(4)='${Y}',@M varchar(2)='${M}',@fno int ='${fileNo}' 
  declare @f date ,@t date;
  set @f=
  (
  select 
  case when start <15 then cast((@y+'-'+@m+'-'+cast(Start as varchar)) as date) 
        else dateadd(month,-1,cast((@y+'-'+@m+'-'+cast(Start as varchar)) as date))
  end
  from Months
  )
  set @t=(select dateadd(day,-1,dateadd(month,1,@f)));
  with month_cte as
  ( 
  select @f dateDay
  union All
  select dateadd(day,1,dateDay)
  from month_cte
  where dateDay<@t
  ), 
  cte AS
    (
      SELECT id,Date,cast(Amount as float)/(case when isnull(RecurringOneTimeId,0)=0 then 1 else RecurringOneTimeId end) 'Amount',PaymentDeductionId,RecurringOneTimeId,TotalNet,DaysNumber
      FROM Deductions where  EmployeeId=(select id from Employees where FileNumber=@fno)
      UNION ALL
  
      SELECT  cte.id,dateadd(month,1,cte.Date),cte.Amount,cte.PaymentDeductionId,(cte.RecurringOneTimeId- 1),cte.TotalNet,cte.DaysNumber
    
      FROM cte INNER JOIN Deductions t
        ON cte.[ID] = t.[ID]
      WHERE cte.RecurringOneTimeId > 1
  ) 
  select isnull((did.Amount),0)Amount,isnull(DaysNumber,0)Days,PaymentDeductions.Id,Name
  from 
  (select sum(amount)Amount, sum(DaysNumber)DaysNumber,PaymentDeductionId
  from month_cte left join cte on cte.Date=month_cte.dateDay 
  where PaymentDeductionId is not null
  group by PaymentDeductionId
  )did right join PaymentDeductions on did.PaymentDeductionId=PaymentDeductions.Id
        `;
  let ajaxC1 = readSQL(sqlC1);
  let ajaxD1 = readSQL(sqlD1);
  //console.log(ajaxD1);
  //console.log(ajaxC1);
  ajax = $.ajax({
    type: "POST",
    dataType: "JSON",
    url: "/AttTable/SalaryFileNoV3",
    data: { fileNo: fileNo, M: M, Y: Y },
    success: function (res) {
      
      console.log("pure res => ",JSON.parse(res));
      if (res.code == 1) {
       // window.location = '/home/yourloginpage.html'
      }
      success++;
      
      dataDay = JSON.parse(res);
      dataDay = dataDay.filter((item, i, ar) => ar.findIndex((w) => w.dateDay == item.dateDay) === i);//remove duplicated days
      console.log("filter => ",dataDay);
      if (Display == 1) 
      {  
        display(calc(dataDay, ajaxC1, ajaxD1));
      }
      else result.push(calc(dataDay, ajaxC1, ajaxD1));
    },
    fail: function (res) {
      console.log(res);
      failed++;
    },
  });
}

function getArray(Arr, M, Y) {
  (result = []), (success = 0);
  failed = 0;
  for (let index = 0; index < Arr.length; index++) {
    getdata(Arr[index], M, Y, 0);
  }
  const timer = setInterval(function () {
    if (Arr.length == failed + success) {
      clearInterval(timer);
      //console.log(result);
      result = result.filter((w) => EMPS.includes(w.all.filenumber));
      //console.log(result);
      displayAllMonth(result);
    }
  }, 1000);
}

function calc(v, x, y) {
  //var v = JSON.parse(res);
  console.log("dataday =>");
  console.log(v);
  console.log("personal data =>");
  console.log(x);
  console.log("deductions =>");
  console.log(y);
  var days = [],
    total;
  var attSum = 0,
    absSum = 0,
    vacSum = 0,
    vacSumAll = 0,
    lateSum = 0,
    lateDeduSum = 0,
    leaveSum = 0,
    overSum = 0,
    errandSum = 0,
    specialSum = 0,
    absCount = 0;
  //console.log(v.findIndex(W=>W.lateHours < 1 && W.lateHours > 0))
  if (v.findIndex((W) => (W.official != 1 &&W.weekend == 0 || (W.weekend == 1 && W.weekend_cancel == 1)) && W.lateHours < 1 && W.lateHours > 0) > -1) {
    try {
      v[
        v.findIndex((W) =>
        (
          (W.weekend == 0 || (W.weekend == 1 && W.weekend_cancel == 1)) &&
          W.official != 1 &&
          W.lateHours < 1 &&
          W.lateHours > 0 &&
          (W.LateChecker == "false" && W.LateChecker2 == "false")
        )
        )
      ]["lateHours"] = "انذار";
    }
    catch (e) { }
  }
  /* اجازة رسمية علي الجميع*/
  for (var i = 0; i < v.length; i++) {
    if (v[i].sumAtt <= v[i].worktime / 4 && v[i]['attValue_flag'] != 1) v[i]["att"] = 0;

    {
      if (v[i]["official"] == 1 && v[i]['attValue_flag'] != 1 && v[i]['indcator'] == 'active') {
        v[i]["att"] = 1;
        v[i]["vacDay"] = "اجازة رسمية";
        abs = 0;
        v[i]["lateHours"] = 0;
        v[i]["leaveHours"] = 0;
      }
    }
  }
  /*كل الشهر*/
  for (var i = 0; i < v.length; i++) {

    if (v[i]["weekend"] == 1 && v[i]['attValue_flag'] != 1 && v[i]['weekend_cancel'] != 1) {
      v[i]["att"] = 1;
      abs = 0;
      v[i]["lateHours"] = 0;
      v[i]["leaveHours"] = 0;
    }
    if (v[i]["weekend"] == 1 && v[i]['weekend_cancel'] != 1) {
      if (v[i]['attValue_flag'] != 1) {
        switch (i) {
          case 0:
            { //الجمعة اول يوم 
              if ((v[i + 1]["att"] == null ? 0 : v[i + 1]["att"]) == 0) {
                if (v[i]["dateDay"].substr(0, 10) < x[0].JoiningDate) v[i]["att"] = 0;
              }
            } break;
          case v.length - 1:
            { //الجمعة اخر يوم 
              console.log(v[i]["dateDay"].substr(0, 10), new Date().toJSON().substring(0, 10));

              //if ((v[i]["att"] == null ? 0 : v[i]["att"]) == 0) {//احمد عيد قالي مالكش دعوة باليوم اللي قبله
              if (v[i]["dateDay"].substr(0, 10) > new Date().toJSON().substring(0, 10)) v[i]["att"] = 0;
              //}
            }
            break;
          default:
            {
              if (
                (v[i + 1]["att"] == null ? 0 : v[i + 1]["att"]) == 0 &&
                (v[i - 1]["att"] == null ? 0 : v[i - 1]["att"]) == 0
              ) {
                v[i]["att"] = 0;
              }
            }
            break;
        }
      }
    }

    var abs = (
      1 -
      v[i]["att"] -
      (Object.is(parseFloat(v[i]["vacDay"]), NaN)
        ? 0
        : parseFloat(v[i]["vacDay"]))
    ).toFixed(2);
    if (abs < 0) {
      v[i]["att"] = parseFloat(v[i]["att"]) + parseFloat(abs);
      abs = 0;
    }
    if (abs > 1) {
      abs = 1;
    }
    if (v[i]["LateChecker"] == "true" || v[i]["LateChecker2"] == "true") {
      v[i]["lateHours"] = 0;
    }

    if (v[i]["leaveChecker"] == "true" || v[i]["leaveChecker2"] == "true") {
      v[i]["leaveHours"] = 0;
    }
    if (v[i]["vacDay"] == 1) {
      v[i]["lateHours"] = 0;
      v[i]["leaveHours"] = 0;
    }
    days.push({
      ShiftId: v[i]["ShiftId"],
      shift_id_flag: v[i]["shift_id_flag"],
      attValue_flag: v[i]["attValue_flag"],
      late_flag: v[i]["late_flag"],
      leave_flag: v[i]["leave_flag"],
      weekend: v[i]["weekend"],
      DayName: weekDays.find((element) => element.EN == v[i]["DayName"]).Ar,
      dateDay: v[i]["dateDay"].substr(0, 10),
      TimeFrom: v[i]["TimeFrom"] == null ? "" : v[i]["TimeFrom"],
      TimeTo: v[i]["TimeTo"] == null ? "" : v[i]["TimeTo"],
      att: v[i]["att"] == null ? "" : v[i]["att"] <= 0 ? "" : v[i]["att"],
      abs: abs > 0 ? abs : "",
      vacDay: v[i]["vacDay"] == null ? "" : v[i]["vacDay"] <= 0 ? "" : v[i]["vacDay"],
      lateHours:
        v[i]["lateHours"] == null
          ? ""
          : (v[i]["lateHours"] <= 0 || (v[i]["weekend"] == 1 && v[i]["weekend_cancel"] == 0))//cancel delay friday
            ? ""
            : v[i]["lateHours"],
      leaveHours:
        v[i]["leaveHours"] == null
          ? ""
          : (v[i]["leaveHours"] <= 0 || (v[i]["weekend"] == 1 && v[i]["weekend_cancel"] == 0))//cancel leave friday
            ? ""
            : v[i]["leaveHours"],
      overTimeHour:
        v[i].overTimeC < -0.66 && v[i]["overTimeHour"] == null
          ? v[i]["overTimeHour"] == null
            ? "&#9889"
            : v[i]["overTimeHour"] <= 0
              ? ""
              : v[i]["overTimeHour"]
          : v[i]["overTimeHour"] == null
            ? ""
            : v[i]["overTimeHour"] <= 0
              ? ""
              : v[i]["overTimeHour"],
      errand: v[i]["errand"] == null ? "" : "&#10003",
      special: v[i]["special"] == null ? "" : v[i]["special"],
      vacDayTypeId: v[i]["AbsenceTypeId"] == null ? "" : v[i]["AbsenceTypeId"],
      vacDayType: v[i]["AbsenceType"] == null ? "" : v[i]["AbsenceType"],
    });
    if (v.length > 30) {
      if (i != v.findIndex((w) => w.weekend == 1 && w.weekend_cancel != 1)) {
        attSum +=
          (
            v[i]["att"] == null
              ? 0
              : (
                v[i]["att"] < 0
                  ? 0
                  : v[i]["att"]
              )
          );
        //console.log("else attSum =>" + attSum);
      }
      else {
        //console.log("else attSum =>" + attSum);
        if (v[i]["vacDay"] >= 0) {
          vacSum -= v[i]["vacDay"];
          vacSumAll -= v[i]["vacDay"];
        }//ak_modification 2024-05-04
      }
    }
    else {
      //console.log("else attSum =>" + attSum);
      attSum +=
        (
          v[i]["att"] == null
            ? 0
            : (
              v[i]["att"] < 0
                ? 0
                : v[i]["att"]
            )
        );
    }
    absSum += parseFloat(abs);

    if (parseFloat(abs) > 0 && v[i].dateDay.substr(0, 10) >= x[0].JoiningDate) absCount++;
    vacSum +=
      v[i]["vacDay"] == null
        ? 0
        : Object.is(parseFloat(v[i]["vacDay"]), NaN)
          ? 0
          : (parseFloat(v[i]["vacDay"]) && v[i]["AbsenceTypeId"] == 6)
            ? 0
            : parseFloat(v[i]["vacDay"]);

    vacSumAll +=
      v[i]["vacDay"] == null
        ? 0
        : Object.is(parseFloat(v[i]["vacDay"]), NaN)
          ? 0
          : parseFloat(v[i]["vacDay"]);

    lateSum +=
      v[i]["lateHours"] == null
        ? 0
        : Object.is(parseFloat(v[i]["lateHours"]), NaN)
          ? 0
          : parseFloat(v[i]["lateHours"]);
    if (lateSum > 2) {
      lateSum -= v[i]["lateHours"];
      lateDeduSum++;
    } else if (lateSum == 2 && parseFloat(v[i]["lateHours"]) > 0) {
      //lateDeduSum++;
    }

    leaveSum += v[i]["leaveHours"] == null ? 0 : v[i]["leaveHours"];
    overSum += v[i]["overTimeHour"] == null ? 0 : v[i]["overTimeHour"];
    errandSum += v[i]["errand"] == null ? 0 : v[i]["errand"];
    specialSum += v[i]["special"] == null ? 0 : v[i]["special"];
  }
  var dValue = 0;
  switch (lateDeduSum % 3) {
    case 0:
      dValue = parseInt(lateDeduSum / 3) * 1.75;
      break;
    case 1:
      dValue = parseInt(lateDeduSum / 3) * 1.75 + 0.25;
      break;
    case 2:
      dValue = parseInt(lateDeduSum / 3) * 1.75 + 0.75;
      break;
  }
  /*
    if (x[0].timeshift == 3) {
      days = [];
      attSum = 0,
        absSum = 0,
        vacSum = 0,
        lateSum = 0,
        lateDeduSum = 0,
        leaveSum = 0,
        overSum = 0,
        errandSum = 0,
        specialSum = 0,
        absCount = 0;
      for (var K = 0; K < v.length; K++) {
        if (v[K]["TimeFrom"] != null && v[K]["TimeTo"] != null) {
          attSum += 1;
          //console.log("attsum=>" + attSum);
          days.push({
            weekend: v[K]["weekend"],
            DayName: weekDays.find((element) => element.EN == v[K]["DayName"]).Ar,
            dateDay: v[K]["dateDay"].substr(0, 10),
            TimeFrom: v[K]["TimeFrom"] == null ? "" : v[K]["TimeFrom"],
            TimeTo: v[K]["TimeTo"] == null ? "" : v[K]["TimeTo"],
            att: 1,
            abs: "",
            vacDay:
              v[K]["vacDay"] == null
                ? ""
                : v[K]["vacDay"] <= 0
                  ? ""
                  : v[K]["vacDay"],
            lateHours: "",
            leaveHours: "",
            overTimeHour: "",
            errand: "",
            special: "",
          });
        } else {
        if (v[K]["vacDay"] > 0 && v[K]["vacDay"] <= 1) 
          {
            vacSum += Object.is(parseFloat(v[K]["vacDay"]), NaN) ? 0 : parseFloat(v[K]["vacDay"]);
            days.push({
              weekend: v[K]["weekend"],
              DayName: weekDays.find((element) => element.EN == v[K]["DayName"]).Ar,
              dateDay: v[K]["dateDay"].substr(0, 10),
              TimeFrom: v[K]["TimeFrom"] == null ? "" : v[K]["TimeFrom"],
              TimeTo: v[K]["TimeTo"] == null ? "" : v[K]["TimeTo"],
              att: "",//official,
              abs: "",
              vacDay:
                v[K]["vacDay"] == null
                  ? ""
                  : v[K]["vacDay"] <= 0
                    ? ""
                    : v[K]["vacDay"],
              lateHours: "",
              leaveHours: "",
              overTimeHour: "",
              errand: "",
              special: "",
            });
          }
          else if (v[K]["official"] == 1) 
            {
            attSum += 1;
            days.push({
              weekend: v[K]["weekend"],
              DayName: weekDays.find((element) => element.EN == v[K]["DayName"]).Ar,
              dateDay: v[K]["dateDay"].substr(0, 10),
              TimeFrom: v[K]["TimeFrom"] == null ? "" : v[K]["TimeFrom"],
              TimeTo: v[K]["TimeTo"] == null ? "" : v[K]["TimeTo"],
              att: 1,
              abs: "",
              vacDay: v[K]["vacDay"],
              lateHours: "",
              leaveHours: "",
              overTimeHour: "",
              errand: "",
              special: "",
            });
  
          } 
          else 
          {
            attSum += ((v[K]["weekend"] == 1 && v[K]["weekend_cancel"] != 1)? 1 : 0);
            absSum += ((v[K]["weekend"] == 0 )? 1 : 0);
            //vacSum += 1;
            days.push({
              weekend: v[K]["weekend"],
              DayName: weekDays.find((element) => element.EN == v[K]["DayName"]).Ar,
              dateDay: v[K]["dateDay"].substr(0, 10),
              TimeFrom: v[K]["TimeFrom"] == null ? "" : v[K]["TimeFrom"],
              TimeTo: v[K]["TimeTo"] == null ? "" : v[K]["TimeTo"],
              att: (v[K]["weekend"] == 1 && v[K]["weekend_cancel"] != 1) ? 1 : "",
              abs: v[K]["weekend"] == 0 ? 1 : "",
              vacDay: "",
              lateHours: "",
              leaveHours: "",
              overTimeHour: "",
              errand: "",
              special: "",
            });
          }
        }
      }
      //console.log("ahmed kamal=>" + (attSum+absSum+vacSum))
      if ((attSum + absSum + vacSum) == 31) {
        if (days[days.findIndex(w => w.weekend == 1)].att == 1) attSum--;
      }
    }
    if (x[0].timeshift == 2) {
      attSum = 0
      absSum = 0
      vacSum = 0
      lateSum = 0
      lateDeduSum = 0
      leaveSum = 0
      overSum = 0
      errandSum = 0
      specialSum = 0
      absCount = 0
      for (var s = 0; s < days.length; s++) {
        if (days[s]["TimeFrom"] != "" && days[s]["TimeTo"] != "") {
          days[s]["att"] = 1;
          days[s]["abs"] = "";
        }
        days[s]["lateHours"] = v[s]["lateHours"] == null ? "" : v[s]["lateHours"];
        days[s]["leaveHours"] = v[s]["leaveHours"] == null ? "" : v[s]["leaveHours"];
        attSum += (isNaN(parseFloat(days[s]["att"])) ? 0 : parseFloat(days[s]["att"]));
      }
    }
  */
  total = {
    attSum: attSum,
    absSum: absSum,
    absCount: absCount,
    vacSum: vacSum,
    vacSumAll: vacSumAll,
    lateSum: lateSum,
    leaveSum: leaveSum,
    overSum: overSum,
    errandSum: errandSum,
    specialSum: specialSum,
    dValue: dValue,
    balance: v[0]["balance"],
  };

  const all = {
    filenumber: x[0].filenumber,
    dailyHours: x[0].DailyHours,
    shiftId: x[0].ShiftId,
    name: x[0].name,
    color: x[0].emp_color,
    DepartementId: x[0].DepartementId,
    dept: x[0].dept,
    job: x[0].job,
    proDays: x[0].ProDays,
    proFixed: x[0].ProFixed,
    withinSalary: x[0].withinSalary,
    timeShift: x[0].timeshift,
    DateOfBirth: x[0].DateOfBirth.substr(0, 10),
    JoiningDate: x[0].JoiningDate.substr(0, 10),
    OvertimeRate: x[0].overtimeRate,
    balance: v[0].balance,
    monthlySalary: x[0].monthlySalary,
    regular: x[0].regular,
    expensive: x[0].expensive,
    skill: x[0].skill,
    management: x[0].manage,
    insurance: x[0].insurance,
    clothes: (y[1].Amount + y[8].Amount).toFixed(2),
    sanctions: (y[2].Amount).toFixed(2),//خصم
    sanctions2: (y[3].Days).toFixed(2),//جزاء ايام
    other: (y[6].Amount + y[7].Amount + y[9].Amount).toFixed(2),
    loans: (y[0].Amount + y[10].Amount).toFixed(2),
  };

  return { all: all, total: total, days: days };
}

function salary(res) {
  /*basics*/
  var fileNo = res.all.filenumber,
    name = res.all.name,
    att = parseFloat(res.total.attSum).toFixed(2),
    vac = parseFloat(res.total.vacSumAll).toFixed(2),
    abs = parseFloat(res.total.absSum).toFixed(2);
  if (res.days[0]["dateDay"].substr(0, 10) >= res.all.JoiningDate.substr(0, 10)) {
    if (res.days.length < 30) att = parseFloat(att) + (30 - res.days.length);
  }
  /*console.log(att);
  console.log((att+vac));*/
  var basicM = parseFloat(res.all.monthlySalary),
    basicD = parseFloat(res.all.monthlySalary / 30) /*.toFixed(2)*/,
    basicH = parseFloat(
      res.all.monthlySalary / 30 / res.all.dailyHours /*.toFixed(2)*/
    ),
    /*calcus*/

    basicV = (
      (parseFloat(att) + parseFloat(res.total.vacSumAll)) *
      basicD
    ).toFixed(2),
    regular = 0,
    expensive = (
      ((parseFloat(att) + parseFloat(res.total.vacSumAll)) *
        res.all.expensive) /
      30
    ).toFixed(2),
    management = (
      ((parseFloat(att) + parseFloat(res.total.vacSumAll)) *
        res.all.management) /
      30
    ).toFixed(2),
    skill = (
      ((parseFloat(att) + parseFloat(res.total.vacSumAll)) *
        res.all.skill) /
      30
    ).toFixed(2),
    over = (basicH * res.all.OvertimeRate * res.total.overSum).toFixed(2),
    insu = res.all.insurance,
    delays = (
      basicH *
      (parseFloat(res.total.lateSum) +
        parseFloat(res.total.leaveSum) +
        parseFloat(res.total.specialSum))
    ).toFixed(2),
    sanctions = (
      parseFloat(basicD * res.total.dValue) +
      parseFloat(res.all.sanctions) +
      parseFloat(res.all.sanctions2) * basicD +
      parseFloat(res.all.other)
    ).toFixed(2),
    clothes = parseFloat(res.all.clothes),
    loans = parseFloat(res.all.loans);

  if (parseFloat(res.total.absSum) > 0) {
    if (parseFloat(res.total.vacSum) + parseFloat(res.total.absSum) > 1.75) {
      regular = 0.0;
    } else {
      regular = parseFloat(res.all.regular);
    }
  }
  else {
    regular = parseFloat(res.all.regular);
  }

  /*if (res.days[0]["dateDay"].substr(0, 10) < res.all.JoiningDate.substr(0, 10)) {
    regular = 0;
  }*/
  regular = parseFloat(regular.toFixed(2));

  var total1 = (
    parseFloat(basicV) +
    parseFloat(regular.toFixed(2)) +
    parseFloat(expensive) +
    parseFloat(management) +
    parseFloat(skill) +
    parseFloat(over)
  ).toFixed(2),
    total2 = (
      parseFloat(insu) +
      parseFloat(delays) +
      parseFloat(sanctions) +
      parseFloat(clothes) +
      parseFloat(loans)
    ).toFixed(2),
    total3 = (total1 - total2).toFixed(2);
  return {
    fileNo: fileNo,
    name: name,
    att: att,
    vac: vac,
    abs: abs,
    basicM: basicM,
    basicD: basicD,
    basicH: basicH,
    /*calcus*/
    basicV: basicV,
    regular: regular,
    expensive: expensive,
    management: management,
    skill: skill,
    over: over,
    insu: insu,
    delays: delays,
    sanctions: sanctions,
    clothes: clothes,
    loans: loans,
    product: (basicM / 30 * parseFloat(res.all.proDays) + parseFloat(res.all.proFixed)) * (parseFloat(res.total.attSum) + (parseFloat(res.total.vacSumAll) >= 1.75 ? 1.75 : parseFloat(res.total.vacSumAll))) / 30,
    //regular: regular,
    total1: total1,
    total2: total2,
    total3: total3,
  };
}

function getDayData(fn, d1, d2, sh)//حساب بيانات يوم 
{
  var sqlcom = `select * from Ak_Cuts_V2( '${d1}','${d2}',${fn},${sh}) order by datetime,Type desc`;
  return readSQL(sqlcom);
}//حساب بيانات يوم 
function reSet(data) //تصفية البيانات               
{
  var array2 = []
  if (data != null && data.length > 2) {

    for (var i = 0; i < data.length; i++) {
      if (data[i].type == "In") {
        if (i > 0) {
          if (data[i - 1].type != data[i].type) {
            array2.push(data[i]);
          }
        } else array2.push(data[i]);
      }
      if (data[i].type == "Out") {
        if (i < data.length - 1) {
          if (data[i].type != data[i + 1].type) {
            array2.push(data[i]);
          }
        } else array2.push(data[i]);
      }
    }
    if (array2.length > 0) if (array2[0].type == 'out') array2 = array2.slice(1)
    if (array2.length > 0) if (array2[array2.length - 1].type == 'in') array2 = array2.slice(0, array2.length - 1)

  }
  return array2;
}//تصفية البيانات    
function reCalc(array2)//حساب الفرق
{
  //if (array2.length > 0)if(array2[0].type=="In")array2=array2.slice(1)
  var absTime = 0;
  if (array2.length > 2) {
    for (var i = 0; i < array2.length - 1; i++) {
      if (array2[i].type == 'Out') {
        var d1 = new Date(array2[i].date.substr(0, 10) + ' ' + array2[i].time);
        var d2 = new Date(array2[i + 1].date.substr(0, 10) + ' ' + array2[i + 1].time);
        let c = (d2.getHours() * 60 + d2.getMinutes()) - (d1.getHours() * 60 + d1.getMinutes());
        absTime += (c < 0 ? (24 * 60 + c) : c);
      }
    }
  }
  return absTime;
}//حساب الفرق  
