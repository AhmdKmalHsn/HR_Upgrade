var dateFrom, FN;
var html = '';
var ajax, ajaxC;
const sql1 = `select filenumber,knownas name from employees`;
const sqlAbsence = `select name,id from AbsenceTypes where id not in (1,2,7,8,9,10,4,11)`;
const sqlVacations = `select * from Vacations where AttendanceValue=0 --and DATEPART(year,date)=DATEPART(year,GETDATE())`;
const sqlDelay = `select id, ShiftName 'name' from Delays`
const Delay = readSQL(sqlDelay);
var absenceType = readSQL(sqlAbsence)
var vacations =  readSQL(sqlVacations)
// ضبط اختيارات ال select

for (var i = 0; i < absenceType.length; i++) {
    
    html += `<option style="text-align:right" value="${absenceType[i].id}">${absenceType[i].name}</option>`;
}
$("#type").html(html);
html = '';
for (var i = 0; i < vacations.length; i++) {
    html += `<option style="text-align:right" value="${vacations[i].Date}">${vacations[i].Name}</option>`;
}
$("#official").html(html);
html = '';
for (var i = 0; i < Delay.length; i++) {
    html += `<option style="text-align:right" value="${Delay[i].id}">${Delay[i].name}</option>`;
}
$("#delay").html(html);
$("#leave").html(html);
// قائمة العمال 
var empTable = $("#empTable").dataTable({
    data: readSQL(sql1),
    columns: [
        {
    title: "filenumber",
    data:"filenumber",
},
        {
    title: "name",
    data:"name",
},
        {
    title: "choose",
    data: "filenumber",
    render: function (a) {
                return `<button onclick="document.getElementById('FN').value=${a};dialog.dialog('close')" >choose</button>`
}
}
]
});
var dialog = $("#empContainer").dialog({
    width: 500,
    height: 300,
    autoOpen: false
});
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
	   --ManagementIncentive,
	   VacationDeferredDate,
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
where FileNumber=${$("#FN").val()}`;
    $("#loader").css("display","block");
    ajaxC = readSQL(sqlC);
    console.log(ajaxC);
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
//ضبط الشهر الافتراضي
$("#M").val(new Date().getMonth()+1);
//دالة عرض الجدول
function display(res) {
    var v = JSON.parse(res);
    console.log(v)
    var html = '<table class="table"><thead class="sticky">';
    html += '<tr style="background-color:darkorange;">';
    html += '<th class="--bs-cyan" style="background-color:">اليوم</th>';
    html += '<th style="background-color:">التاريخ</th>';
    html += '<th style="background-color:">الدخول</th>';
    html += '<th style="background-color:">الخروج</th>';
    html += '<th style="background-color:">الحضور</th>';
    html += '<th style="background-color:">غياب</th>';
    html += '<th style="background-color:">اجازة</th>';
    html += '<th style="background-color:">تأخير</th>';
    html += '<th style="background-color:">انصراف</th>';
    html += '<th style="background-color:">اضافي</th>';
    html += '<th style="background-color:">مأمورية</th>';
    html += '</tr></thead><tbody>';

    for (var i = 0; i < v.length; i++)
    {
        var abs = (1 - v[i]["att"] - v[i]["vacDay"]).toFixed(2);
        if(v[i]["weekend"]==0)html += '<tr>'; else html += '<tr style="background-color:#05F548">';
        html += '<td class="--bs-cyan" style="background-color:">' + v[i]["DayName"] + '</td>';
        html += '<td style="background-color:">' + v[i]["dateDay"].substr(0, 10) + '</td>';
        html += '<td style="background-color:">' + (v[i]["TimeFrom"] == null ? '' : v[i]["TimeFrom"]) + '</td>';
        html += '<td style="background-color:">' + (v[i]["TimeTo"]== null ? '' : v[i]["TimeTo"]) + '</td>';
        html += '<td class="table-success">' + (v[i]["att"] == null ? '' : v[i]["att"] <= 0 ? '' : v[i]["att"]) + '</td>';
        html += '<td class="table-danger">' + (abs > 0 ? abs : '') + '</td>';
        html += '<td class="table-warning">' + (v[i]["vacDay"] == null ? '' : v[i]["vacDay"] <= 0 ? '' : v[i]["vacDay"]) + '</td>';
        html += '<td class="table-secondary">' + (v[i]["lateHours"] == null ? '' : v[i]["lateHours"] <= 0 ? '' : v[i]["lateHours"]) + '</td>';
        html += '<td class="table-primary">' + (v[i]["leaveHours"] == null ? '' : v[i]["leaveHours"] <= 0 ? '' : v[i]["leaveHours"]) + '</td>';
        if (v[i].overTimeC < -0.66 && v[i]["overTimeHour"] == null) html += '<td style="background-color:silver">' + (v[i]["overTimeHour"] == null ? '&#9889' : v[i]["overTimeHour"] <= 0 ? '' : v[i]["overTimeHour"]) + '</td>';
        else html += '<td style="background-color:gold">' + (v[i]["overTimeHour"] == null ? '' : v[i]["overTimeHour"] <= 0 ? '' : v[i]["overTimeHour"]) + '</td>';
        html += '<td ">' + (v[i]["AbsenceType"] == null ? '' : v[i]["AbsenceType"] ) + '</td>';
        html += '</tr>';
    }
    html += '</tbody></table>';
    $("#mytable").html(html);
}
//ضبط اختيار نوع الاجازة
function checkType(e)
{
    $("#officialDiv").css("display", "none");
    $("#exchangeDiv").css("display", "none");
    switch (e) {
        case '6': $("#officialDiv").css("display", "flex"); break;
        case '14': $("#exchangeDiv").css("display", "flex"); break;
        case '15': $("#exchangeDiv").css("display", "flex"); break;
    }  
}

$(function () {

    var $contextMenu = $("#contextMenu");

    $("#mytable").on("contextmenu", "table tr td", function (e) {
        dateFrom = $(this).closest('tr').find('td:eq(1)').html();
        FN = ajaxC[0].filenumber;
        $(".dateFrom").val(dateFrom)
        $(".dateTo").val(dateFrom)
        $contextMenu.css({
            display: "block",
            left: e.pageX,
            top: e.pageY
        });
        //debugger;
        return false;
    });

    $('html').click(function () {
        $contextMenu.hide();
    });

    $("#contextMenu li").click(function (e) {
        var f = $(this);
        console.log(f)
    });

});
console.log($(".myModal .btn").length);
$(".myModal .btn").on("click", function (e) {
    switch ($(".myModal .btn").index(e.target))
    {
        case 0: { ins_in() } break;
        case 1: { ins_out() } break;
        case 2: { ins_mission() } break;
        case 3: { del_mission() } break;
        case 4: { ins_overtime() } break;
        case 5: { del_overtime() } break;
        case 6: { ins_vacation() } break;
        case 7: { del_vacation() } break;
        case 8: { ins_temp() } break;
        case 9: { del_temp() } break;
    }
});
/***************test****************/
$("#vacValue").on("focusout", function () {
    $("#qtrDiv").css("display", "none");
    $("#hlfDiv").css("display", "none");
    $("#shftDiv").css("display", "none");
    switch (parseFloat($(this).val()))
    {
        case 1: break;
        case 0.25: $("#qtrDiv").css("display", "flex");$("#shftDiv").css("display", "flex"); break;
        case 0.5: $("#hlfDiv").css("display", "flex"); $("#shftDiv").css("display", "flex"); break;
    }
});
/*******************************/
$("#mytable").on("click", "table tr td", function (e) {
    var col = $(this).parent().children().index($(this));
    var row = $(this).parent().parent().children().index($(this).parent());
    dateFrom = $(this).closest('tr').find('td:eq(1)').html();
    FN = ajaxC[0].filenumber;
    
    $(".dateFrom").html(dateFrom)
    switch (col) {
        case 2: {  
            var sql="select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='"+dateFrom+"' and FileNumber="+FN+" and type='in' order by DateTime";
            var data=readSQL(sql);
            var html='<option >--select--</option>';
            for(var i=0;i<data.length;i++)
            {
               html+='<option>'+data[i].time+'</option>';
            }
            $('#TIN').val('');
            $('#deviceIN').html(html);
            $('.myModal').eq(0).modal('show');
        } break;
        case 3: {
            var sql = "select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='" + dateFrom + "' and FileNumber=" + FN + " and type='out' order by DateTime";
            var data = readSQL(sql);
            var html = '<option >--select--</option>';
            for (var i = 0; i < data.length; i++) {
                html += '<option>' + data[i].time + '</option>';
            }
            $('#deviceOUT').html(html);
            $('#TOUT').val('');
            $('.myModal').eq(1).modal('show');
        } break;
        case 6:{ $('.myModal').eq(4).modal('show'); }break;
        case 9:{ $('.myModal').eq(3).modal('show'); }break;
    }
});
/*
حضور-0
1-انصراف
2-مامورية
3-أضافي
4-أجازة
5-وردية خاصة  $('.myModal').eq(5).modal('show');
*/
function ins_in() {
    //INOUT = $('#TIN').val();
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/truant/AttendIN',
        data: { fileNo: FN, d: dateFrom, t: $('#TIN').val() },
        success: function (response) {

            if (response == 1) {
                //$("#container tr:eq(" + (i - 1) + ") td:eq(2)").html(INOUT);//////////////////
                alert("Saved Successfully!");
            }
            else
                alert("Error!");
        },
        error: function (response) {
            alert("Error: " + response);
        }
    });
    $('#TIN').val("");
}
function ins_out() {
    //INOUT = $('#TOUT').val();
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/truant/AttendOUT',
        data: { fileNo: FN, d: dateFrom, t: $('#TOUT').val() },
        success: function (response) {
            if (response == 1) {
                //$("#container tr:eq(" + (i - 1) + ") td:eq(3)").html(INOUT);//////////////////
                alert("Saved Successfully!");
            }
            else
                alert("Error!");
        },
        error: function (response) {
            alert("Error: " + response);
        }
    });
    $('#TOUT').val("");
}
function ins_mission() {
    var sql = ``;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
}
function del_mission() {
    var sql = ``;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
}
function ins_overtime() {
    var sql = ``;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
}
function del_overtime() {
    var sql = ``;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
}
function ins_vacation() {
    let qtr=$('input[name="qtr"]:checked').val();
    let hlf = $('input[name="hlf"]:checked').val();
    console.log(`.25=>${qtr} hlf=>${hlf}`);
    var sql = ``;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
}
function del_vacation() {
    var sql = ``;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
}
function ins_temp() {
    let v = $("#ModalTemp #timeTo").val() - $("#ModalTemp #timeFrom").val()
    var sql = `
        begin transaction
        insert into TempShifts(DateFrom,DateTo,ShiftId,ShiftFrom,ShiftTo,ShiftHours,DepartmentId,isPrivate,DelayId,LeaveId)
              values('${$("#ModalTemp .dateFrom").val()}', '${$("#ModalTemp .dateTo").val()}', 0, '${$("#ModalTemp #timeFrom").val()}', '${$("#ModalTemp #timeTo").val()}', (select case when datediff(minute, '${$("#ModalTemp #timeFrom").val()}', '${$("#ModalTemp #timeTo").val()}') <0 then 24+datediff(minute, '${$("#ModalTemp #timeFrom").val()}', '${$("#ModalTemp #timeTo").val()}') /60 else datediff(minute, '${$("#ModalTemp #timeFrom").val()}', '${$("#ModalTemp #timeTo").val()}') /60 end), 0, 1, '${$("#ModalTemp #delay").val()}', '${$("#ModalTemp #leave").val()}')
        insert into tempshiftentry(fileNumber,tempShiftId)values('${FN}',(select IDENT_CURRENT('tempshifts')))
        commit transaction `;
    //console.log(sql);
    var res = excuteSQL(sql)
    if (res == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
    //console.log(res);
}
function del_temp() {
    var sql = ``;
    console.log(sql);
    var data = ExcuteSQL(sql);
    console.log(data);
}