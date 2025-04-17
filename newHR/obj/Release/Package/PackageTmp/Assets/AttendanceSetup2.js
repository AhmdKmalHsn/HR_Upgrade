var dateFrom, FN;
var html = '';
//var ajax, ajaxC;
const sql1 = `select filenumber,knownas name,(select name from departements where id=departementId)dept  from employees`;
const sqlAbsence = `select name,id from AbsenceTypes where id not in (1,2,7,8,9,10,4,11)`;
const sqlVacations = `select * from Vacations where AttendanceValue=0 --and DATEPART(year,date)=DATEPART(year,GETDATE())`;
const sqlDelay = `select id, ShiftName 'name' from Delays`
const Delay = readSQL(sqlDelay);
var absenceType = readSQL(sqlAbsence)
var vacations = readSQL(sqlVacations)
// ضبط اختيارات ال select

for (var i = 0; i < absenceType.length; i++) {

    html += `<option style="text-align:right" value="${absenceType[i].id}">${absenceType[i].name}</option>`;
}
$("#type").html(html);
html = '';
for (var i = 0; i < vacations.length; i++) {
    html += `<option style="text-align:right" value="${vacations[i].Date.substr(0, 10)}">${vacations[i].Name}</option>`;
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
            data: "filenumber",
        },
        {
            title: "name",
            data: "name",
        },
          {
              title: "department",
              data: "dept",
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
    width: 600,
    height: 500,
    autoOpen: false
});
//احضار البيانات مع للعامل 

//ضبط الشهر الافتراضي
$("#M").val(new Date().getMonth() + 1);
const weekDays=[
                  {Ar:"السبت",EN:"Saturday"},
                  {Ar:"الاحد",EN:"Sunday"},
                  {Ar:"الاثنين",EN:"Monday"},
                  {Ar:"الثلاثاء",EN:"Tuesday"},
                  {Ar:"الاربعاء",EN:"Wednesday"},
                  {Ar:"الخميس",EN:"Thursday"},
                  {Ar:"الجمعة",EN:"Friday"},
               ]
//دالة عرض الجدول

function display(v) {
    $("#personal_data").html('<div class="row row-reversed" style="font-size:24px;border:1px #1595 solid;"><div class="col-1 p-1" >الاسم:</div><div class="col-3 p-1 text-end">' + v.all.name + '</div>'
                            + '<div class="col-1 p-1 " >القسم:</div><div class="col-3 p-1 text-end">' + v.all.dept + '</div>'
        /*+ '<div class="col-2 p-1 text-danger" >الرصيد:</div><div class="col-2 p-1 text-end text-danger">' + v.total.balance+ '</div>*/
        + '</div>');
    //console.error("here display");
    console.log(JSON.parse(JSON.stringify(v)));
    let s = salary(v);
    
    //console.log(s);

    var html = '<table class="table table-hover"><thead class="">';
    html += '<tr style="background-color:darkorange;">';
    html += '<th class="--bs-cyan" style="background-color:">اليوم</th>';
    html += '<th style="">التاريخ</th>';
    html += '<th style="">الدخول</th>';
    html += '<th style="">الخروج</th>';
    html += '<th style="">الحضور</th>';
    html += '<th style="">غياب</th>';
    html += '<th style="">اجازة</th>';
    html += '<th style="">تأخير</th>';
    html += '<th style="">انصراف</th>';
    html += '<th style="">اضافي</th>';
    html += '<th style="">مأمورية</th>';
    html += '<th style="">مأمورية خاصة</th>';
    html += '</tr></thead><tbody>';
   
    for (var i = 0; i < v.days.length; i++) {
       
        if (v.days[i]["weekend"] == 0) {
            html += '<tr>';
            html +=
              '<td class="--bs-cyan" style="background-color:">' +
              v.days[i]["DayName"] +
              "</td>";
            html +=
              '<td style="background-color:#2098bb55">' +
              v.days[i]["dateDay"] +
              "</td>";
            html +=
              '<td style="background-color:">' +
              v.days[i]["TimeFrom"] +
              "</td>";
            html +=
              '<td style="background-color:">' + v.days[i]["TimeTo"] + "</td>";
            html += '<td class="table-success">' + v.days[i]["att"] + "</td>";
            html += '<td class="table-danger">' + v.days[i]["abs"] + "</td>";
            html +=
              '<td class="table-warning">' + v.days[i]["vacDay"] + "</td>";
            html +=
              '<td class="table-secondary">' + v.days[i]["lateHours"] + "</td>";
            html +=
              '<td class="table-primary">' + v.days[i]["leaveHours"] + "</td>";
            html +=
              '<td style="background-color:silver">' +
              v.days[i]["overTimeHour"] +
              "</td>";
            html += '<td ">' + v.days[i]["errand"] + "</td>";
            html +=
              '<td style="background-color:rgb(200,150,84);">' +
              v.days[i]["special"] +
              "</td>";
        }
        else {
            html += '<tr style="background-color:#05F548">';
            html +=
              '<td  style="background-color:">' +
              v.days[i]["DayName"] +
              "</td>";
            html +=
              '<td style="background-color:#2098bb55">' +
              v.days[i]["dateDay"] +
              "</td>";
            html +=
              '<td style="background-color:">' +
              v.days[i]["TimeFrom"] +
              "</td>";
            html +=
              '<td style="background-color:">' + v.days[i]["TimeTo"] + "</td>";
            html += '<td >' + v.days[i]["att"] + "</td>";
            html += '<td >' + v.days[i]["abs"] + "</td>";
            html +=
              '<td >' + v.days[i]["vacDay"] + "</td>";
            html +=
              '<td >' + v.days[i]["lateHours"] + "</td>";
            html +=
              '<td >' + v.days[i]["leaveHours"] + "</td>";
            html +=
              '<td style="background-color:">' +
              v.days[i]["overTimeHour"] +
              "</td>";
            html += '<td ">' + v.days[i]["errand"] + "</td>";
            html +=
              '<td style="">' +
              v.days[i]["special"] +
              "</td>";
        }
        
        html += '</tr>';
    }
    html +=
      '<tr><td rowspan=2 colspan="4">أجمالي</td><td rowspan=2>' +
      v.total.attSum +
      "</td><td>" +
      v.total.absSum +
      "</td><td rowspan=2>" +
      v.total.vacSum +
      "</td><td>" +
      v.total.lateSum +
      "</td><td rowspan=2>" +
      v.total.leaveSum +
      "</td><td rowspan=2>" +
      v.total.overSum +
      "</td><td rowspan=2>" +
      v.total.errandSum +
      "</td><td rowspan=2>" +
      v.total.specialSum +
      "</td></tr><tr><td>" +
      v.total.absCount +
      "</td><td>" +
      v.total.dValue +
      "</td></tr></tbody></table>";
    $("#mytable").html(html);
    $("#loader").css("display", "none");
    
    var htmlDom =
      "<tr><td colspan=4>مفردات المرتب</td></tr><tr><td>الاساسي</td><td>" +
      //s.basicM +
      //"</td><td>" +
      s.basicV +
      "</td><td>خصم التأمينات</td><td>" +
      s.insu +
      "</td>" +
      "</tr><tr><td>الانتظام</td><td>" +
      s.regular +
      "</td><td>التاخيرات</td><td>" +
      s.delays +
      "</td>" +
      "</tr><tr><td>غلاء المعيشة</td><td>" +
      s.expensive +
      "</td><td>الجزاءات</td><td>" +
      s.sanctions +
      "</td>" +
      "</tr><tr><td>مجلس الادارة</td><td>" +
      s.management +
      "</td><td>الملبس</td><td>" +
      s.clothes +
      "</td>" +
      "</tr><tr><td>المهارة</td><td>" +
      s.skill +
      "</td><td>السلف</td><td>" +
      s.loans +
      "</td>" +
      "</tr><tr><td>الاضافي</td><td>" +
      s.over +
      "</td>" +
      "</tr><tr><td>أجمالي الاستحقاقات</td><td>" +
      s.total1 +
      "</td><td>اجمالي الاستقطاعات</td><td>" +
      s.total2 +
      "</td>" +
      "</tr><tr><td colspan=2>صافي المرتب</td><td colspan=2>" +
      s.total3 +
      "</td></tr>";
     
     
      
      
      
     

$('#salary').html(htmlDom)
}

//ضبط اختيار نوع الاجازة
function checkType(e) {
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
        //FN = ajaxC[0].filenumber;
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
     /*
    $("#contextMenu li").click(function (e) {
        var f = $(this);
        console.log(f)
    });*/

});
//console.log($(".myModal .btn").length);
$(".myModal .btn").on("click", function (e) {
    console.log($(".myModal .btn").index(e.target));
    switch ($(".myModal .btn").index(e.target)) {
       /* case 0: { ins_in() } break;
        case 1: { ins_out() } break;
        case 2: { ins_mission() } break;
        case 3: { del_mission() } break;
        case 4: { ins_overtime() } break;
        case 5: { del_overtime() } break;
        case 6: { ins_vacation() } break;
        case 7: { del_vacation() } break;
        case 8: { ins_temp() } break;
        //case 9: { del_temp() } break;
        case 9: { ins_ATT() } break;
        case 10: { del_ATT() } break;
        case 11: { ins_LATE() } break;
        case 12: { del_LATE() } break;
        case 13: { ins_LEAVE() } break;
        case 14: { del_LEAVE() } break;*/
    }
});
function showtime(){
    let sh=$("#shft").val();
    let part=$("#vacValue").val();
    let off=0;

switch(parseFloat($("#vacValue").val()))
{
   case 0.5: off=$('input:radio[name=hlf]:checked').val();break;
   case 0.25:off=$('input:radio[name=qtr]:checked').val();break;
}
let sql=`declare  @sh int=${sh},@off int=${off},@part float =${part}
select cast(dateadd(MINUTE,DailyHours*@part*60*(@off-1),StartTime)as time(0))s ,
cast(dateadd(MINUTE,DailyHours*@part*60*(@off),StartTime)as time(0))e 
from Shifts 
where ShiftId=@sh`;
//console.log(run);
if(parseFloat($("#vacValue").val())!=1 /*&& parseInt($("#type").val())==3*/)
{
   $("#vacFrom").val(readSQL(sql)[0].s);
   $("#vacTo").val(readSQL(sql)[0].e);
}
else
{
    $("#vacFrom").val('');
    $("#vacTo").val('');
}  
} 
$("#ModalVacation input,#ModalVacation select").not("#vacFrom,#vacTo").on("change",function (e) {
  showtime();  
});
/***************test****************/
$("#vacValue").on("change", function () {
    $("#qtrDiv").css("display", "none");
    $("#hlfDiv").css("display", "none");
    $("#shftDiv").css("display", "none");
    switch (parseFloat($(this).val())) {
        case 1: break;
        case 0.25: $("#qtrDiv").css("display", "flex"); $("#shftDiv").css("display", "flex"); break;
        case 0.5: $("#hlfDiv").css("display", "flex"); $("#shftDiv").css("display", "flex"); break;
    }
});
/*******************************/
$("#mytable").on("click", "table tr td", function (e) {
    var col = $(this).parent().children().index($(this));
    var row = $(this).parent().parent().children().index($(this).parent());
    dateFrom = $(this).closest('tr').find('td:eq(1)').html();
    //FN = //ajaxC[0].filenumber;
    
    $(".dateFrom").html(dateFrom)
    switch (col) {
        case 2: {
            var sql = "select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='" + dateFrom + "' and FileNumber=" + FN + " and type='in' order by DateTime";
            var data = readSQL(sql);
            var html = '<option >--select--</option>';
            for (var i = 0; i < data.length; i++) {
                html += '<option>' + data[i].time + '</option>';
            }
            $('#TIN').val('');
            $('#deviceIN').html(html);
            $('#MODALIN').modal('show');
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
            $('#MODALOUT').modal('show');
        } break;
        case 4:
            {
            $('#MODALATT').modal('show');
            }break;
        case 6: { 
            $('#ModalVacation').modal('show');
            $("#shftDiv").css("display", "none");
         } break;
        case 7:
            {
                $('#MODALLATE').modal('show');
            }break;
        case 8:
            {
                $('#MODALLEAVE').modal('show');
            }break;
        case 9: { $('#ModalOvertime').modal('show'); } break;
        case 10: {
            var sql = "select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='" + dateFrom + "' and FileNumber=" + FN + " and type='in' order by DateTime";
            var data = readSQL(sql);
            var html = '<option >--select--</option>';
            for (var i = 0; i < data.length; i++) {
                html += '<option>' + data[i].time + '</option>';
            }

            $('#deviceMOUT').html(html);
            $('#TOUT').val('');

            sql = "select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='" + dateFrom + "' and FileNumber=" + FN + " and type='out' order by DateTime";
            data = readSQL(sql);
            html = '<option >--select--</option>';
            for (var i = 0; i < data.length; i++) {
                html += '<option>' + data[i].time + '</option>';
            }
            $('#deviceMIN').html(html);
            $('#TIN').val('');
            $('#MODALMission').modal('show');
        } break;
        case 11: {
            $("#MODALSPECIAL").modal("show");

        } break;
    }
});
//كاميرا زيزو هام
$("#type").change(function(){
  if(parseInt($("#type").val())==3){
      //$("#qtrDiv")
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
    var sql = `
        insert into Absences(DateFrom,TimeFrom,TimeTo,AbsenceTypeId,EmployeeId)
                     values('${dateFrom}', '${$("#TINMission").val()}', '${$("#TOUTMission").val()}', 4, (select id from Employees where FileNumber=${FN})) `;
    console.log(sql);
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function del_mission() {
    var sql = `delete from Absences where DateFrom='${dateFrom}' and AbsenceTypeId=4 and EmployeeId=(select id from Employees where FileNumber=${FN})`;
    console.log(sql);
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function ins_special() {
  var sql = `
        insert into Absences(DateFrom,daypart,AbsenceTypeId,EmployeeId)
                     values('${dateFrom}', '${$("#dayPartSpecial").val()}', 11, (select id from Employees where FileNumber=${FN})
                     ) `;
  console.log(sql);
  if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
  else alert("خطأ");
}

function del_special() {
  var sql = `delete from Absences where DateFrom='${dateFrom}' and AbsenceTypeId=11 and EmployeeId=(select id from Employees where FileNumber=${FN})`;
  console.log(sql);
  if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
  else alert("خطأ");
}

function ins_overtime() {
    var sql = ``;
    let a = $("#afr_txt").val();
    let b = $("#bfr_txt").val();
    if (a || b) {
        if (a) sql += `insert into AdditionApprovals (NoOfHour	,Date,EmployeeId,TimeOf)values('${a}','${dateFrom}',(select id from Employees where FileNumber=${FN}),'0');`;
        if (b) sql += `insert into AdditionApprovals (NoOfHour	,Date,EmployeeId,TimeOf)values('${b}','${dateFrom}',(select id from Employees where FileNumber=${FN}),'1');`;
    }
    else {

    }
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function del_overtime() {
    var sql = `delete from  AdditionApprovals where date='${dateFrom}' and EmployeeId=(select id from Employees where FileNumber=${FN})`
    console.log(sql);
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function ins_vacation() {
    var type = $('#type').val();
    console.log(type);
    var ref1 = '',
        ref2 = '';
    switch (type) {
        case '6': ref1 = $('#official').val(); break;
        case '14': ref2 = $('#exchange').val(); break;
        case '15': ref2 = $('#exchange').val(); break;
    }
    console.log(`ref1=${ref1},ref2=${ref2}`);
    var sql = `
        insert into Absences(DateFrom,TimeFrom,TimeTo,AbsenceTypeId,EmployeeId,daypart,Ref1,Ref2)
        values('${dateFrom}', '${$("#vacFrom").val()}', '${$("#vacTo").val()}', ${type}, (select id from Employees where FileNumber=${FN}) ,'${$("#vacValue").val()}','${ref1}', '${ref2}') `;
    console.log(sql);
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function del_vacation() {
    var sql = `delete from absences where AbsenceTypeId not in (4,11) and EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}' `;
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
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
    var data = excuteSQL(sql);
    console.log(data);
}

function ins_ATT() {
    var sql = `update attendances set AttValue=${$("#ATTvalue").val()} where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    console.log($("#ATTvalue").val());
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function del_ATT() {
    var sql = `update attendances set AttValue=NULL where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function ins_LATE() {
    var sql = `update attendances set Late=${$("#LATEvalue").val()} where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function del_LATE() {
    var sql = `update attendances set Late=NULL where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function ins_LEAVE() {
    var sql = `update attendances set Leave=${$("#LEAVEvalue").val()} where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    console.log(sql);
    var data = excuteSQL(sql);
    console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function del_LEAVE() {
    var sql = `update attendances set Leave='NULL where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    console.log(sql);
    var data = ExcuteSQL(sql);
    console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}
//console.log(userLog());
function changeLog()
{
  
}