var ajax, ajaxC;
//احضار البيانات مع للعامل 
function getTable()
{
    var sqlC = `select filenumber,
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
    $("#loader").css("display","block");
    ajaxC = readSQL(sqlC);
    console.log(ajaxC[0].name);
    html = '';
    for (var i = 0; i < ajaxC.length; i++) {
        html += `<option style="text-align:right" value="${ajaxC[i].ShiftId}">${ajaxC[i].ShiftName}</option>`;
    }
    $("#shft").html(html);

    ajax = $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/AttTable/SalaryFileNo',
        data: { fileNo: $("#FN").val(), M: $("#M").val(), Y: $("#Y").val() },
    });
    $.when(ajax).done(function (res) {
        $("#loader").css("display", "none");
        display(res);
    });
}

function getdata(fileNo, M, Y)
{

    var sqlC = `select filenumber,
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
	   ins.EmployeeFixedSalary,
	   ins.EmployeeFixedSalary*(select ofFixedSalartForEmployeeContrribution from InsurancePrecentages)/100 insurance

from employees e left join BasicBayWorks b on e.Id=b.EmployeeId
				 left join (select id ,EmployeeId,EmployeeFixedSalary from InsuranceDetails i1 where id=(select max(id)id from InsuranceDetails i2 where i1.EmployeeId=i2.EmployeeId))ins on ins.EmployeeId=e.id
                 left join Personals p on p.id=e.PersonalId

where FileNumber=${fileNo}`;
    ajaxC = readSQL(sqlC);
    //console.log(ajaxC[0].name);
    ajax = $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/AttTable/SalaryFileNo',
        data: { fileNo: fileNo, M: M, Y: Y },
        success: function (res) {
            //return {ajaxC,res}
            console.log({ a:ajaxC, b:JSON.parse(res)})
        },
        fail: function (res) {
            //return { ajaxC, res}
            console.log({ a:ajaxC, b:res})
        }
    });
    
}
