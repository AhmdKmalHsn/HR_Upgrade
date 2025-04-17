var ajax, ajaxC;
var result = [],
  success = 0,
  failed = 0,
  element = 0;
//احضار البيانات مع للعامل
function getTable() {
  console.clear();
  var sqlC1 = `select filenumber,
       knownas name,
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
 join Payments p on EmployeeId=(select id from Employees where FileNumber=@fno) and p.Date=month_cte.dateDay 
group by PaymentDeductionId
)did right join PaymentDeductions on did.PaymentDeductionId=PaymentDeductions.Id
`;
  ajaxC = readSQL(sqlC1);
  //console.log(readSQL(sqlD));
  FN = $("#FN").val();

  html = "";

  for (var i = 0; i < ajaxC.length; i++) {
    html += `<option style="text-align:right" value="${ajaxC[i].ShiftId}">${ajaxC[i].ShiftName}</option>`;
  }
  $("#shft").html(html);
  if (EMPS.includes("" + $("#FN").val())) {
    var data = getdata($("#FN").val(), $("#M").val(), $("#Y").val(), 1);
    console.log(data);
  } else {
    $("#loader").css("display", "none");
    alert("غير مسموح!");
  }
}

function getdata(fileNo, M, Y, Display) {
  //if(EMPS.includes(fileNo))

  var sqlC1 = `select filenumber,
       sh.DailyHours,
       knownas name,
       DepartementId,
       (select name from Departements where  id=DepartementId)dept,
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
	   VacationDeferredDate,
	   p.StatusId,p.DateOfBirth,p.JoiningDate,isnull(p.TimeShit,0)timeshift,
	   ins.EmployeeFixedSalary,
	   ins.EmployeeFixedSalary*(select ofFixedSalartForEmployeeContrribution from InsurancePrecentages)/100 insurance

        from employees e left join BasicBayWorks b on e.Id=b.EmployeeId
                  left join Shifts sh on sh.ShiftId=b.ShiftId
				 left join (select id ,EmployeeId,isnull(EmployeeFixedSalary,0) EmployeeFixedSalary from InsuranceDetails i1 where id=(select max(id)id from InsuranceDetails i2 where i1.EmployeeId=i2.EmployeeId))ins on ins.EmployeeId=e.id
                 left join Personals p on p.id=e.PersonalId
        where FileNumber=${fileNo}`;
  let sqlD1 = `declare @Y varchar(4)='${Y}',@M varchar(2)='${M}',@fno int ='${fileNo}'
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
        join Payments p on EmployeeId=(select id from Employees where FileNumber=@fno) and p.Date=month_cte.dateDay 
        group by PaymentDeductionId
        )did right join PaymentDeductions on did.PaymentDeductionId=PaymentDeductions.Id
        `;
  let ajaxC1 = readSQL(sqlC1);
  let ajaxD1 = readSQL(sqlD1);
  console.log(ajaxD1);
  ajax = $.ajax({
    type: "POST",
    dataType: "JSON",
    url: "/AttTable/SalaryFileNoV3",
    data: { fileNo: fileNo, M: M, Y: Y },
    success: function (res) {
      if(res.Result == false){
          window.location = '/home/license'
      } else {
         success++;
         console.log("data from server=>");
	     console.log(res);
      if (Display == 1) display(calc(res, ajaxC1, ajaxD1));
      else result.push(calc(res, ajaxC1, ajaxD1));
      }
     
    },
    fail: function (res) {
      //return { ajaxC, res}
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
    //console.log(Arr[index]);
  }
  const timer = setInterval(function () {
    if (Arr.length == failed + success) {
      clearInterval(timer);
      console.log(result);
      result = result.filter((w) => EMPS.includes(w.all.filenumber));
      console.log(result);
      displayAllMonth(result);
    }
  }, 1000);
}

function calc(res, x, y) {
  var v = JSON.parse(res);
  console.log(v);
  var days = [],
    total;

  var attSum = 0,
    absSum = 0,
    vacSum = 0,
    lateSum = 0,
    lateDeduSum = 0,
    leaveSum = 0,
    overSum = 0,
    errandSum = 0,
    specialSum = 0,
    absCount = 0;
  //console.log(v.findIndex(W=>W.lateHours < 1 && W.lateHours > 0))
  if (v.findIndex((W) => W.lateHours < 1 && W.lateHours > 0) > -1) {
    try {
      v[
        v.findIndex((W) =>
        (W.lateHours < 1 &&
          W.lateHours > 0 &&
          (W.LateChecker == "false" && W.LateChecker2 == "false")
        )
        )
      ]["lateHours"] = "انذار";
    }
    catch (e) { }  
  }
 
  for (var i = 0; i < v.length; i++) {
    if (v[i]["dateDay"].substr(0, 10) >= x[0].JoiningDate) {
      if (v[i]["official"] == 1) 
      {
        v[i]["att"] = 1;
        v[i]["vacDay"] = "اجازة رسمية";
        abs = 0;
        v[i]["lateHours"] = 0;
        v[i]["leaveHours"] = 0;
      }
    }
    else
    {
      
    }
  }
  for (var i = 0; i < v.length; i++) {
    //if(v[i]["official"]==1){ v[i]["att"]=1;}
    if (v[i]["weekend"] == 1) {
      v[i]["att"] = 1;
      abs = 0;
      v[i]["lateHours"] = 0;
      v[i]["leaveHours"] = 0;
    }
    if (v[i]["weekend"] == 0) {
      //html += '<tr>';
    } else {
      switch (i) {
        case 0:
          {
            if ((v[i + 1]["att"] == null ? 0 : v[i + 1]["att"]) == 0) {
              
              if (v[i]["dateDay"].substr(0, 10) < x[0].JoiningDate) v[i]["att"] = 0;
            }
          }
          break;
        case v.length - 1:
          { //الجمعة اخر يوم 
            /*if (v[i]["dateDay"].substr(0, 10) > new Date().toISOString().substring(0,10)) v[i]["att"] = 0;
            if ((v[i - 1]["att"] == null ? 0 : v[i - 1]["att"]) == 0) {
              if (v[i]["dateDay"].substr(0, 10) < x[0].JoiningDate) v[i]["att"] = 0;
            }*/
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
    if (v[i]["LateChecker"] == "true" || v[i]["LateChecker2"] == "true")
      v[i]["lateHours"] = 0;
    if (v[i]["leaveChecker"] == "true" || v[i]["leaveChecker2"] == "true")
      v[i]["leaveHours"] = 0;

    days.push({
      weekend: v[i]["weekend"],
      DayName: weekDays.find((element) => element.EN == v[i]["DayName"]).Ar,
      dateDay: v[i]["dateDay"].substr(0, 10),
      TimeFrom: v[i]["TimeFrom"] == null ? "" : v[i]["TimeFrom"],
      TimeTo: v[i]["TimeTo"] == null ? "" : v[i]["TimeTo"],
      att: v[i]["att"] == null ? "" : v[i]["att"] <= 0 ? "" : v[i]["att"],
      abs: abs > 0 ? abs : "",
      vacDay:
        v[i]["vacDay"] == null ? "" : v[i]["vacDay"] <= 0 ? "" : v[i]["vacDay"],
      lateHours:
        v[i]["lateHours"] == null
          ? ""
          : v[i]["lateHours"] <= 0
          ? ""
          : v[i]["lateHours"],
      leaveHours:
        v[i]["leaveHours"] == null
          ? ""
          : v[i]["leaveHours"] <= 0
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
    });
    if (v.length > 30) {
      if (i != v.findIndex((w) => w.weekend == 1))
        attSum += v[i]["att"] == null ? 0 : v[i]["att"];
    } else {
      attSum += v[i]["att"] == null ? 0 : v[i]["att"];
    }
    absSum += parseFloat(abs);
    
  
    
    if (parseFloat(abs) > 0 && v[i].dateDay.substr(0,10) >= x[0].JoiningDate) absCount++;
    vacSum +=
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
  //console.log(x[0])
  //console.log(x[0].timeshift);
  //console.log(x[0].timeshift==1)
  if (x[0].timeshift == 1) {
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
        if (v[K]["vacDay"] > 0 && v[K]["vacDay"] <=1 ) {
          vacSum += Object.is(parseFloat(v[K]["vacDay"]), NaN)? 0: parseFloat(v[K]["vacDay"]);
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
        else if( v[K]["official"] == 1){
          attSum += 1;
          days.push({
            weekend: v[K]["weekend"],
            DayName: weekDays.find((element) => element.EN == v[K]["DayName"]).Ar,
            dateDay: v[K]["dateDay"].substr(0, 10),
            TimeFrom: v[K]["TimeFrom"] == null ? "" : v[K]["TimeFrom"],
            TimeTo: v[K]["TimeTo"] == null ? "" : v[K]["TimeTo"],
            att: 1,
            abs: "",
            vacDay:v[K]["vacDay"],
            lateHours: "",
            leaveHours: "",
            overTimeHour: "",
            errand: "",
            special: "",
          });

        } else {
          attSum += (v[K]["weekend"] == 1 ? 1 : 0);
          absSum += (v[K]["weekend"] == 0 ? 1 : 0);
          //vacSum += 1;
          days.push({
            weekend: v[K]["weekend"],
            DayName: weekDays.find((element) => element.EN == v[K]["DayName"]).Ar,
            dateDay: v[K]["dateDay"].substr(0, 10),
            TimeFrom: v[K]["TimeFrom"] == null ? "" : v[K]["TimeFrom"],
            TimeTo: v[K]["TimeTo"] == null ? "" : v[K]["TimeTo"],
            att: v[K]["weekend"] == 1 ? 1 : "",
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
    if((attSum+absSum+vacSum)==31){
      if(days[days.findIndex(w=>w.weekend==1)].att==1)attSum--;
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
      if (days[s]["TimeFrom"] !=""  && days[s]["TimeTo"] != "") {
        days[s]["att"] = 1;
        days[s]["abs"] = "";
      }
      days[s]["lateHours"] = v[s]["lateHours"] == null ? "" : v[s]["lateHours"];
      days[s]["leaveHours"] = v[s]["leaveHours"] == null ? "" : v[s]["leaveHours"];
      attSum += (isNaN(parseFloat(days[s]["att"])) ? 0:parseFloat(days[s]["att"]) );
    }
  }
  
  total = {
    attSum: attSum,
    absSum: absSum,
    absCount: absCount,
    vacSum: vacSum,
    lateSum: lateSum,
    leaveSum: leaveSum,
    overSum: overSum,
    errandSum: errandSum,
    specialSum:specialSum,
    dValue: dValue,
    balance: v[0]["balance"],
  };
  const all = {
    filenumber: x[0].filenumber,
    dailyHours: x[0].DailyHours,
    name: x[0].name,
    DepartementId: x[0].DepartementId,
    dept: x[0].dept,
    timeShift: x[0].timeshift,
    DateOfBirth: x[0].DateOfBirth.substr(0,10),
    JoiningDate: x[0].JoiningDate.substr(0,10),
    OvertimeRate: x[0].overtimeRate,
    balance: v[0].balance,
    monthlySalary: x[0].monthlySalary,
    regular: x[0].regular,
    expensive: x[0].expensive,
    skill: x[0].skill,
    management: x[0].manage,
    insurance: x[0].insurance,
    clothes: y[1].Amount + y[8].Amount,
    sanctions: y[2].Amount + y[3].Amount,
    other: y[6].Amount + y[7].Amount + y[9].Amount,
    loans: y[0].Amount + y[10].Amount,
  };
  console.log("amount=>")
  console.log(y);
  return { all: all, total: total, days: days };
}

function salary(res) {
  /*basics*/
  var fileNo = res.all.filenumber,
    name = res.all.name,
    att = parseFloat(res.total.attSum).toFixed(2),
    vac = parseFloat(res.total.vacSum).toFixed(2),
    abs = parseFloat(res.total.absSum).toFixed(2),
    basicM = parseFloat(res.all.monthlySalary),
    basicD = parseFloat(res.all.monthlySalary / 30) /*.toFixed(2)*/,
    basicH = parseFloat(
      res.all.monthlySalary / 30 / res.all.dailyHours /*.toFixed(2)*/
    ),
    /*calcus*/
    basicV = (
      (parseFloat(res.total.attSum) + parseFloat(res.total.vacSum)) *
      basicD
    ).toFixed(2),
    regular = 0,
    expensive = (
      ((parseFloat(res.total.attSum) + parseFloat(res.total.vacSum)) *
        res.all.expensive) /
      30
    ).toFixed(2),
    management = (
      ((parseFloat(res.total.attSum) + parseFloat(res.total.vacSum)) *
        res.all.management) /
      30
    ).toFixed(2),
    skill = (
      ((parseFloat(res.total.attSum) + parseFloat(res.total.vacSum)) *
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
      parseFloat(res.all.other)
    ).toFixed(2),
    clothes = parseFloat(res.all.clothes),
    loans = parseFloat(res.all.loans);
/*
  if (parseFloat(res.total.absSum) > 0) {
    if (parseFloat(res.total.vacSum) + parseFloat(res.total.absSum) > 1.5) {
      regular = parseFloat(res.all.regular) * 0.0;
    }
    if (parseFloat(res.total.vacSum) + parseFloat(res.total.absSum) == 1.5) {
      regular = parseFloat(res.all.regular) * 0.0;
    }
    if (parseFloat(res.total.vacSum) + parseFloat(res.total.absSum) < 1.5) {
      regular = parseFloat(res.all.regular) * 1.0;
    }
  } else {
    regular = parseFloat(res.all.regular) * 1.0;
  }
  */

  switch (true) {
    case res.total.absSum == 0:
      regular = parseFloat(res.all.regular) * 1.0;
      break;
    case res.total.absSum > 0 && res.total.absSum <= 1:
      regular = parseFloat(res.all.regular) * 0.8;
      break;
    case res.total.absSum > 1 && res.total.absSum <= 2:
      regular = parseFloat(res.all.regular) * 0.4;
      break;

    default:
      regular = 0;
      break;
  }
  if (res.days[0]["dateDay"].substr(0, 10) <res.all.JoiningDate.substr(0, 10))
   {
    regular = 0;
  }
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
    regular: regular,
    total1: total1,
    total2: total2,
    total3: total3,
  };
}
