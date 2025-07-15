//    path: Assets/Attendance3.js
//    parameters
var ajax,
  dataDay;
var result = [],
  success = 0,
  failed = 0,
  element = 0;
function getTable() 
{
  $("#loader").css("display", "block");
  FN = $("#FN").val();
  if (EMPS.includes("" + $("#FN").val())) {
    data = getdata($("#FN").val(), $("#M").val(), $("#Y").val(), 1);
  } else {
    $("#loader").css("display", "none");
    alert("غير مسموح!");
  }
}
//اول دالة بتتنادى وبعد كده هي بتنادي علي باقي الدوال
function getdata(fileNo, M, Y, Display) {
  //if(EMPS.includes(fileNo))

  var sqlC1 = `select filenumber,
  sh.DailyHours,
  isnull(e.color,'') 'emp_color', 
  e.Notes,
  e.BankCode,
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
  isnull(ProductivityDays,0) ProDays,
  isnull(ProductivityFixed,0) ProFixed,
  isnull(b.withinSalary,0) withinSalary,
  p.StatusId,
  p.DateOfBirth,
  p.JoiningDate,
  isnull(p.TimeShit,0)timeshift,
  ins.EmployeeFixedSalary,
  case when ins.Percentage > 0 then ins.EmployeeFixedSalary*Percentage else ins.EmployeeFixedSalary*(select ofFixedSalartForEmployeeContrribution from InsurancePrecentages)/100 end insurance
   from employees e left join BasicBayWorks b on e.Id=b.EmployeeId
             left join Shifts sh on sh.ShiftId=b.ShiftId
    left join (select id ,EmployeeId,isnull(EmployeeFixedSalary,0) EmployeeFixedSalary,i1.Percentage  from InsuranceDetails i1 where id=(select max(id)id from InsuranceDetails i2 where i1.EmployeeId=i2.EmployeeId))ins on ins.EmployeeId=e.id
            left join Personals p on p.id=e.PersonalId
   where FileNumber=${fileNo}`;
  //حساب الخصومات والاستقطاعات 
  let sqlD1 = `declare @Y varchar(4)='${Y}',@M varchar(2)='${M}',@fno int ='${fileNo}'
  --declare @Y varchar(4)='2023',@M varchar(2)='12',@fno int ='1102'
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
  ), 
  cte AS
    (
      SELECT id,Date,cast(Amount as float)/(case when isnull(RecurringOneTimeId,0)=0 then 1 else RecurringOneTimeId end) 'Amount',PaymentDeductionId,RecurringOneTimeId,TotalNet,DaysNumber
      FROM Deductions 
      WHERE  EmployeeId=(select id from Employees where FileNumber=@fno)
        UNION ALL
      SELECT  cte.id,dateadd(month,1,cte.Date),cte.Amount,cte.PaymentDeductionId,(cte.RecurringOneTimeId- 1),cte.TotalNet,cte.DaysNumber
      FROM cte INNER JOIN Deductions t ON cte.[ID] = t.[ID]
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
  let ajaxEmpData = readSQL(sqlC1);
  let ajaxDeduction = readSQL(sqlD1);

  ajax = $.ajax({
    type: "POST",
    dataType: "JSON",
    url: "/AttTable/SalaryFileNoV3",
    data: { fileNo: fileNo, M: M, Y: Y },
    success: function (res) {
      success++;
      if (Display == 1) {
        dataDay = JSON.parse(res);
        let unique = dataDay.filter((item, i, ar) => ar.findIndex((w) => w.dateDay == item.dateDay) === i);//remove duplicated days
        console.log("unique pure => ", unique);
        display(calc(unique, ajaxEmpData, ajaxDeduction));
      }
      else result.push(calc(unique, ajaxEmpData, ajaxDeduction));
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
      result = result.filter((w) => EMPS.includes(w.all.filenumber));
      displayAllMonth(result);
    }
  }, 1000);
}

function calc(v, x, y) {

  console.log("Pure =>");
  console.log(v);
  console.log("EmpData =>");
  x=x[0];
  console.log(x);
  console.log("Deduction =>");
  console.log(y);
  var days = [],
    total;
  var attSum = 0,
    absSum = 0,
    vacSum = 0,//all vacation days
    vacSumAll = 0,//from balance
    vacSumMinus1=0,//علشان تشيل يوم من الاساسي
    vacSumMinus2=0,//علشان تشيل يوم من الاساسي والحوافز
    lateSum = 0,
    lateDeduSum = 0,
    leaveSum = 0,
    overSum = 0,
    errandSum = 0,
    specialSum = 0,
    absCount = 0;
  //console.log(v.findIndex(W=>W.lateHours < 1 && W.lateHours > 0))
  if (v.findIndex((W) => (W.official != 1 && W.weekend == 0 || (W.weekend == 1 && W.weekend_cancel == 1)) && W.lateHours < 1 && W.lateHours > 0) > -1) {
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
                if (v[i]["dateDay"].substr(0, 10) < x.JoiningDate) v[i]["att"] = 0;
              }
            } break;
          case v.length - 1:
            { //الجمعة اخر يوم 

              if (v[i]["dateDay"].substr(0, 10) > new Date().toJSON().substring(0, 10)) v[i]["att"] = 0;

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
      vacDayTypeId2: v[i]["AbsenceTypeId2"] == null ? "" : v[i]["AbsenceTypeId2"],
      vacDayType2: v[i]["AbsenceType2"] == null ? "" : v[i]["AbsenceType2"],
    });

    //calculate the totals
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
          vacSumAll -= v[i]["vacDayBal"];
        }
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
    console.log(v[i]["dateDay"],v[i]["att"],attSum);
    absSum += parseFloat(abs);

    if (parseFloat(abs) > 0 && v[i].dateDay.substr(0, 10) >= x.JoiningDate) absCount++;
    
          if(v[i]["AbsenceTypeId"]==13){
            vacSumMinus1+=(v[i]["vacDay"]-v[i]["vacDayBal"]);
            
          }
          if(v[i]["AbsenceTypeId2"]==13){
            vacSumMinus1+=(v[i]["vacDay"]-v[i]["vacDayBal"]);
            
          }
          if(v[i]["AbsenceTypeId"]==17){
            vacSumMinus2+=(v[i]["vacDay"]-v[i]["vacDayBal"]);
            
          }
          if(v[i]["AbsenceTypeId2"]==17){
            vacSumMinus2+=(v[i]["vacDay"]-v[i]["vacDayBal"]);
           
          }
    vacSum +=
      v[i]["vacDay"] == null
        ? 0
        /*: Object.is(parseFloat(v[i]["vacDay"]), NaN)
          ? 0
          : (parseFloat(v[i]["vacDay"]) && v[i]["AbsenceTypeId"] == 6)
            ? 0*/
          : Object.is(parseFloat(v[i]["vacDay"]), NaN)
          ? 0
            : parseFloat(v[i]["vacDay"]);

    vacSumAll +=
      v[i]["vacDayBal"] == null
        ? 0
        : Object.is(parseFloat(v[i]["vacDayBal"]), NaN)
          ? 0
          : parseFloat(v[i]["vacDayBal"]);

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

  total = {
    attSum: attSum+((vacSum-vacSumAll)>0?(vacSum-vacSumAll):0),//الاستثناءات تكمل  الحضور
    absSum: absSum,
    absCount: absCount,
    vacSum: vacSum,
    vacSumAll: vacSumAll,
    vacSumMinus1: vacSumMinus1,
    vacSumMinus2: vacSumMinus2,
    lateSum: lateSum,
    leaveSum: leaveSum,
    overSum: overSum,
    errandSum: errandSum,
    specialSum: specialSum,
    dValue: dValue,
    balance: v[0]["balance"],
  };

  const all = {
    filenumber: x.filenumber,
    dailyHours: x.DailyHours,
    shiftId: x.ShiftId,
    name: x.name,
    name2: x.Notes,
    bank_code: x.BankCode,
    color: x.emp_color,
    DepartementId: x.DepartementId,
    dept: x.dept,
    job: x.job,
    proDays: x.ProDays,
    proFixed: x.ProFixed,
    withinSalary: x.withinSalary,
    timeShift: x.timeshift,
    DateOfBirth: x.DateOfBirth.substr(0, 10),
    JoiningDate: x.JoiningDate.substr(0, 10),
    OvertimeRate: x.overtimeRate,
    balance: v[0].balance,
    monthlySalary: x.monthlySalary,
    regular: x.regular,
    expensive: x.expensive,
    skill: x.skill,
    management: x.manage,
    insurance: x.insurance,
    clothes: (y[1].Amount + y[8].Amount).toFixed(2),
    rewards: (y[4].Amount).toFixed(2),
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
    name2 = res.all.name2,
    bank_code = res.all.bank_code,
    att = parseFloat(res.total.attSum).toFixed(2),
    vac = parseFloat(res.total.vacSumAll).toFixed(2),
    abs = parseFloat(res.total.absSum).toFixed(2);
  if (res.days[0]["dateDay"].substr(0, 10) >= res.all.JoiningDate.substr(0, 10)) {
    if (res.days.length < 30) att = parseFloat(att) + (30 - res.days.length);
  }

  var basicM = parseFloat(res.all.monthlySalary),
      basicD = parseFloat(res.all.monthlySalary / 30) /*.toFixed(2)*/,
      basicH = parseFloat(
      res.all.monthlySalary / 30 / res.all.dailyHours /*.toFixed(2)*/
      ),
    /*calcus*/

    basicV = (
      (parseFloat(att) + parseFloat(res.total.vacSumAll)-parseFloat(res.total.vacSumMinus1)-parseFloat(res.total.vacSumMinus2)) *
      basicD
    ).toFixed(2),
    regular = 0,
    expensive = (
      ((parseFloat(att) + parseFloat(res.total.vacSumAll)-parseFloat(res.total.vacSumMinus2)) *
        res.all.expensive) /
      30
    ).toFixed(2),
    management = (
      ((parseFloat(att) + parseFloat(res.total.vacSumAll)-parseFloat(res.total.vacSumMinus2)) *
        res.all.management) /
      30
    ).toFixed(2),
    skill = (
      ((parseFloat(att) + parseFloat(res.total.vacSumAll)-parseFloat(res.total.vacSumMinus2)) *
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
    rewards = parseFloat(res.all.rewards);
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

  regular = parseFloat(regular.toFixed(2));

  var total1 = (
    parseFloat(basicV) +
    parseFloat(regular.toFixed(2)) +
    parseFloat(expensive) +
    parseFloat(management) +
    parseFloat(skill) +
    parseFloat(over) +
    parseFloat(rewards)
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
    rewards: rewards,
    product: (basicM / 30 * parseFloat(res.all.proDays) + parseFloat(res.all.proFixed)) * (parseFloat(res.total.attSum) + (parseFloat(res.total.vacSumAll) >= 1.75 ? 1.75 : parseFloat(res.total.vacSumAll))) / 30,
    //regular: regular,
    total1: total1,
    total2: total2,
    total3: total3,
  };
}
