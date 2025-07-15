var dateFrom, FN, shiftOfDay;
var html = '';//temp html dom creator
//var ajax, ajaxC;
const sql1 = `select filenumber,knownas name,(select name from departements where id=departementId)dept  from employees`;
const sqlAbsence = `select name,id from AbsenceTypes where id not in (1,2,7,8,9,10,4,11)`;
const sqlVacations = `select * from Vacations where AttendanceValue=0 --and DATEPART(year,date)=DATEPART(year,GETDATE())`;
const sqlShifts = `select id,cast(id as nvarchar)+' - '+ShiftName 'name' from shifts`
const Shifts = readSQL(sqlShifts);
const absenceType = readSQL(sqlAbsence)
const vacations = readSQL(sqlVacations)
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
for (var i = 0; i < Shifts.length; i++) {
    html += `<option style="text-align:right" value="${Shifts[i].id}">${Shifts[i].name}</option>`;
}
$("#shiftId").html(html);
$("#shft").html(html);
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
const weekDays = [
    { Ar: "السبت", EN: "Saturday" },
    { Ar: "الاحد", EN: "Sunday" },
    { Ar: "الاثنين", EN: "Monday" },
    { Ar: "الثلاثاء", EN: "Tuesday" },
    { Ar: "الاربعاء", EN: "Wednesday" },
    { Ar: "الخميس", EN: "Thursday" },
    { Ar: "الجمعة", EN: "Friday" },
]
//دالة عرض الجدول

function display(v) {
    $("#personal_data").html(
        '<div class="row row-reversed" style="background:' + v.all.color +
        ';font-size:24px;border:1px #1595 solid;"><table>'
        + '<tr><td>الاسم:</td><td>' + v.all.name + '</td></tr>'
        + '<tr><td>القسم:</td><td>' + v.all.dept + '</td></tr>'
        + '<tr><td>الوظيفة</td><td>' + v.all.job + '</td></tr>'
        + '<tr><td>تاريخ التعيين:</td><td>' + v.all.JoiningDate + '</td></tr>'
        + '<tr><td>رصيد الاجازات:</td><td>' +  v.total.balance+ '</td></tr>'
        + '</table></div>');
    console.error(v);

    let s = salary(v);

    let json = JSON.parse(JSON.stringify(v));

    let fno = json.all.filenumber;

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
    html += '<th style="">اذن خاص</th>';
    html += '</tr></thead><tbody>';

    for (var i = 0; i < v.days.length; i++) {

        //console.log(v.days[i]);
        if (v.days[i]["weekend"] == 0) {
            html += '<tr>';
            html +=
                '<td class="--bs-cyan" style="background-color:">' +
                v.days[i]["DayName"] +
                "</td>";
            html +=
                `<td style="background-color:${(v.days[i]['shift_id_flag'] == 0 || v.days[i]['ShiftId'] == null) ? '#2098bb55' : ''}">` +
                v.days[i]["dateDay"] +
                "</td>";
            html +=
                '<td style="background-color:">' +
                v.days[i]["TimeFrom"] +
                "</td>";
            html +=
                '<td style="background-color:">' + v.days[i]["TimeTo"] + "</td>";
            html += `<td class="table-success" style="background-color:${v.days[i]['attValue_flag'] == 1 ? '' : ''}">` + (v.days[i]["att"]) + "</td>";
            html += '<td class="table-danger">' + v.days[i]["abs"] + "</td>";
            html +=
                `<td style="background-color:#ffc107" >` +
                (v.days[i]["vacDay"]) +"<p style='background-color:#2599AA'>"+(v.days[i]["vacDayTypeId"]!=3 && v.days[i]["vacDayTypeId"]!="" ?v.days[i]["vacDayType"]:"")+(v.days[i]["vacDayTypeId2"]!=3 && v.days[i]["vacDayTypeId2"]!=""?v.days[i]["vacDayType2"]:"")+"</p>"
                + "</td>";
            html +=
                `<td class="table-secondary" style="background-color:${v.days[i]['late_flag'] == 1 ? '' : ''}">` + v.days[i]["lateHours"] + "</td>";
            html +=
                `<td class="table-primary" style="background-color:${v.days[i]['leave_flag'] == 1 ? '' : ''}">` + v.days[i]["leaveHours"] + "</td>";
            html +=
                '<td style="background-color:silver">' +
                v.days[i]["overTimeHour"] +
                "</td>";
            html += '<td style="background-color:rgb(169 83 225 / 80%);">' + v.days[i]["errand"] + "</td>";

            html +=
                '<td style="background-color:rgb(237 46 136 / 75%);">' +
                (v.days[i]["special"] > 0 ? v.days[i]["special"] : "") +
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
        "</td><td rowspan=2>" +
        v.total.absSum +
        "</td><td rowspan=2>" +
        v.total.vacSumAll +
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
        "</td></tr>" +
        "</td><td>" +
        (parseFloat(v.total.dValue) + parseFloat(v.all.sanctions2)) +
        "</td></tr></tbody></table>";
    $("#mytable").html(html);
    $("#loader").css("display", "none");

    var htmlDom =
        "<tr><td colspan=4>مفردات المرتب</td></tr><tr><td>الاساسي</td><td>" +
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
        "</tr><tr><td>المكافات</td><td>" +
        s.rewards +
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

function displayFromArchive(v) {

    console.error(v);
    for (let j = 0; j < v.length; j++) {
        for (let [key, value] of Object.entries(v[j])) {
            if (v[j][key] == null) v[j][key] = '';
            //console.log(`${key}: ${value}`);
        }
    }
    console.error(v);
    

    let fno = v[0].FileNumber;
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
    html += '<th style="">اذن خاص</th>';
    html += '</tr></thead><tbody>';

    for (var i = 0; i < v.length; i++) {

        if (v[i].weekend == "0") {
            html += '<tr>';
            html +=
                '<td class="--bs-cyan" style="background-color:">' +
                v[i]["Dayname"] +
                "</td>";
            html +=
                '<td style="background-color:#2098bb55">' +
                v[i]["dateDay"].substr(0, 10) +
                "</td>";
            html +=
                '<td style="background-color:">' +
                v[i]["TimeFrom"] +
                "</td>";
            html +=
                '<td style="background-color:">' + v[i]["TimeTo"] + "</td>";
            html += '<td class="table-success">' + v[i]["att"] + "</td>";
            html += '<td class="table-danger">' + v[i]["abs"] + "</td>";
            html +=
                '<td class="table-warning">' + v[i]["vacDay"] +"<span style='background-color:#2599AA'>"+(v[i]["AbsenceTypeId"]!=3?v[i]["AbsenceType"]:"")+(v[i]["AbsenceTypeId2"]!=3?v[i]["AbsenceType2"]:"")+"</span></td>";
            html +=
                '<td class="table-secondary">' + v[i]["lateHours"] + "</td>";
            html +=
                '<td class="table-primary">' + v[i]["leaveHours"] + "</td>";
            html +=
                '<td style="background-color:silver">' +
                v[i]["overTimeHour"] +
                "</td>";
            html += '<td style="background-color:rgb(169 83 225 / 80%);">' + v[i]["errand"] + "</td>";

            html +=
                '<td style="background-color:rgb(237 46 136 / 75%);">' +
                v[i]["special"] +
                "</td>";
        }
        else {
            html += '<tr style="background-color:#05F548">';
            html +=
                '<td  style="background-color:">' +
                v[i]["Dayname"] +
                "</td>";
            html +=
                '<td style="background-color:#2098bb55">' +
                v[i]["dateDay"].substr(0, 10) +
                "</td>";
            html +=
                '<td style="background-color:">' +
                v[i]["TimeFrom"] +
                "</td>";
            html += '<td style="background-color:">' + v[i]["TimeTo"] + "</td>";
            html += '<td >' + v[i]["att"] + "</td>";
            html += '<td >' + v[i]["abs"] + "</td>";
            html += '<td >' + v[i]["vacDay"] +"<span style='background-color:#2599AA'>"+(v[i]["AbsenceTypeId"]!=3?v[i]["AbsenceType"]:"")+(v[i]["AbsenceTypeId2"]!=3?v[i]["AbsenceType2"]:"")+"</span></td>";
            html += '<td >' + v[i]["lateHours"] + "</td>";
            html += '<td >' + v[i]["leaveHours"] + "</td>";
            html += '<td style="background-color:">' +
                v[i]["overTimeHour"] +
                "</td>";
            html += '<td ">' + v[i]["errand"] + "</td>";
            html += '<td style="">' +
                v[i]["special"] +
                "</td>";
        }

        html += '</tr>';
    }
    html +=
        '<tr><td rowspan=2 colspan="4">أجمالي</td><td rowspan=2>' +
        v[0].Attendance +
        "</td><td rowspan=2>" +
        v[0].Abcence +
        "</td><td rowspan=2>" +
        v[0].Vacation +
        "</td><td>" +
        v[0].lateSum +
        "</td><td rowspan=2>" +
        v[0].leaveSum +
        "</td><td rowspan=2>" +
        v[0].overtimeSum +
        "</td><td rowspan=2>" +
        v[0].errand +
        "</td><td rowspan=2>" +
        v[0].sepcialSum +
        "</td></tr>" +

        "</td><td>" +
        v[0].deductionSum + //parseFloat(v[0].sanctions2))+
        "</td></tr></tbody></table>";
    $("#mytable2").html(html);
    $("#loader").css("display", "none");

    var htmlDom =
        "<tr><td colspan=4>مفردات المرتب</td></tr><tr><td>الاساسي</td><td>" +
        //s.basicM +
        //"</td><td>" +
        v[0].SalaryMonth +
        "</td><td>خصم التأمينات</td><td>" +
        v[0].Insurance +
        "</td>" +
        "</tr><tr><td>الانتظام</td><td>" +
        v[0].RegularityIncentive +
        "</td><td>التاخيرات</td><td>" +
        v[0].Delays +
        "</td>" +
        "</tr><tr><td>غلاء المعيشة</td><td>" +
        v[0].ExpensiveIncetive +
        "</td><td>الجزاءات</td><td>" +
        v[0].Deductions +
        "</td>" +
        "</tr><tr><td>مجلس الادارة</td><td>" +
        v[0].ManagmentIncetive +
        "</td><td>الملبس</td><td>" +
        v[0].Clothes +
        "</td>" +
        "</tr><tr><td>المهارة</td><td>" +
        v[0].SkillIncentive +
        "</td><td>السلف</td><td>" +
        v[0].Loans +
        "</td>" +
        "</tr><tr><td>الاضافي</td><td>" +
        v[0].Overtime +
        "</td>" +
        "</tr><tr><td>أجمالي الاستحقاقات</td><td>" +
        v[0].Total1 +
        "</td><td>اجمالي الاستقطاعات</td><td>" +
        v[0].Total2 +
        "</td>" +
        "</tr><tr><td colspan=2>صافي المرتب</td><td colspan=2>" +
        v[0].Total3 +
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


function showtime() {
    let sh = '';
    if (shiftOfDay == null) {
        sh = $("#shft").val();
    }
    else {
        sh = shiftOfDay;
    }
    
    let part = $("#vacValue").val();
    let off = 0;

    switch (parseFloat($("#vacValue").val())) {
        case 0.5: off = $('input:radio[name=hlf]:checked').val(); break;
        case 0.25: off = $('input:radio[name=qtr]:checked').val(); break;
    }
    let sql = `declare  @sh int=${sh},@off int=${off},@part float =${part}
select cast(dateadd(MINUTE,DailyHours*@part*60*(@off-1),StartTime)as time(0))s ,
cast(dateadd(MINUTE,DailyHours*@part*60*(@off),StartTime)as time(0))e 
from Shifts 
where ShiftId=@sh`;
    //console.log(run);
    if (parseFloat($("#vacValue").val()) != 1 /*&& parseInt($("#type").val())==3*/) {
        $("#vacFrom").val(readSQL(sql)[0].s);
        $("#vacTo").val(readSQL(sql)[0].e);
    }
    else {
        $("#vacFrom").val('');
        $("#vacTo").val('');
    }
}
$("#ModalVacation input,#ModalVacation select").not("#vacFrom,#vacTo").on("change", function (e) {
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
    showtime();
});
/****************table showing modals***************/
$("#mytable").on("click", "table tr td", function (e) {
    var col = $(this).parent().children().index($(this));
    var row = $(this).parent().parent().children().index($(this).parent());
    dateFrom = $(this).closest('tr').find('td:eq(1)').html();
    shiftOfDay = dataDay.filter(w => w.dateDay.substr(0, 10) == dateFrom)[0].ShiftId
    console.log(col);
    $(".dateFrom").html(dateFrom)
    switch (col) {
        //ModalTemp
        case 1:{
            $("#ModalTemp .dateFrom").val(dateFrom);
            $("#ModalTemp .dateTo").val(dateFrom);
            $("#ModalTemp #shiftId").val(shiftOfDay);
            $('#ModalTemp').modal('show');
        }break;
        case 2: {
            $("#MODALIN .dayIn").prop('checked', false);
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
            $("#MODALOUT .dayOut").prop('checked', false);
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
            } break;
        case 5:
            {
                $("#MODALDEDU #date").val(dateFrom);
                $('#MODALDEDU').modal('show');
            } break;
        case 6: {
            showtime();
            $('#ModalVacation').modal('show');
            $("#shft").val(shiftOfDay);
            $("#shftDiv").css("display", "none");
        } break;
        case 7:
            {
                $('#MODALLATE').modal('show');
            } break;
        case 8:
            {
                $('#MODALLEAVE').modal('show');
            } break;
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

            var data1 = readSQL("select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='" + dateFrom + "' and FileNumber=" + FN + " and type='in' order by DateTime");
            var html1 = '<option >--select--</option>';
            for (var i = 0; i < data1.length; i++) {
                html1 += '<option>' + data1[i].time + '</option>';
            }
            $('.sp-ch #deviceMOUT').html(html1);
            $('.sp-ch #TOUT').val('');

            data1 = readSQL("select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='" + dateFrom + "' and FileNumber=" + FN + " and type='out' order by DateTime");
            html1 = '<option >--select--</option>';
            for (var i = 0; i < data1.length; i++) {
                html1 += '<option>' + data1[i].time + '</option>';
            }
            $('.sp-ch #deviceMIN').html(html1);
            $('.sp-ch #TIN').val('');

            //open special modal
            $("#MODALSPECIAL").modal("show");

        } break;
    }
});

//كاميرا زيزو هام
$("#type").change(function () {
    if (parseInt($("#type").val()) == 3) {
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
    ////console.log(sql);
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function del_mission() {
    var sql = `delete from Absences where DateFrom='${dateFrom}' and AbsenceTypeId=4 and EmployeeId=(select id from Employees where FileNumber=${FN})`;
    ////console.log(sql);
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function ins_special() {
    var sql = `
        insert into Absences(DateFrom,daypart,AbsenceTypeId,EmployeeId)
                     values('${dateFrom}', '${$("#dayPartSpecial").val()}', 11, (select id from Employees where FileNumber=${FN})
                     ) `;
    ////console.log(sql);
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function del_special() {
    var sql = `delete from Absences where DateFrom='${dateFrom}' and AbsenceTypeId=11 and EmployeeId=(select id from Employees where FileNumber=${FN})`;
    ////console.log(sql);
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
    ////console.log(sql);
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function ins_vacation() {
    var type = $('#type').val();
    //console.log(type);
    var ref1 = '',
        ref2 = '';
    switch (type) {
        case '6': ref1 = $('#official').val(); break;
        case '14': ref2 = $('#exchange').val(); break;
        case '15': ref2 = $('#exchange').val(); break;
    }
    //console.log(`ref1=${ref1},ref2=${ref2}`);
    var sql = `
        insert into Absences(DateFrom,TimeFrom,TimeTo,AbsenceTypeId,EmployeeId,daypart,Ref1,Ref2)
        values('${dateFrom}', '${$("#vacFrom").val()}', '${$("#vacTo").val()}', ${type}, (select id from Employees where FileNumber=${FN}) ,'${$("#vacValue").val()}','${ref1}', '${ref2}') `;
    ////console.log(sql);
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function del_vacation() {
    var sql = `delete from absences where AbsenceTypeId not in (4,11) and EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}' `;
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function ins_temp() {
    var sql = `
        begin transaction 
            UPDATE 
                Attendances 
            SET 
                ShiftId =${$("#ModalTemp #shiftId").val()}  
            WHERE 
                DateFrom>='${$("#ModalTemp .dateFrom").val()}' and DateFrom<='${$("#ModalTemp .dateTo").val()}' and 
                EmployeeId=(select id from Employees where FileNumber=${FN})  
        commit transaction `;
    ////console.log(sql);
    var res = excuteSQL(sql)
    if (res == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
    //console.log(res);
}

function del_temp() {
    var sql = `
        begin transaction 
            UPDATE 
                Attendances 
            SET 
                ShiftId = NULL 
            WHERE 
                DateFrom>='${$("#ModalTemp .dateFrom").val()}' and DateFrom<='${$("#ModalTemp .dateTo").val()}' and 
                EmployeeId=(select id from Employees where FileNumber=${FN})  
        commit transaction `;
    ////console.log(sql);
    var res = excuteSQL(sql)
    if (res == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
    //console.log(res);
}

function ins_ATT() {
    //var sql = `update attendances set AttValue=${$("#ATTvalue").val()} where EmployeeId=(select id from Employees where FileNumber=) and dateFrom ='${dateFrom}'`;
    var sql = `declare @empid int=(select id from employees where filenumber=${FN}),@att float(2)=${$("#ATTvalue").val()},@date date='${dateFrom}';
            if((select count(*)cnt from attendances where employeeid=@empid and [DateFrom]=@date)>0)
            begin 
                update attendances set [AttValue]=@att
                where employeeid=@empid and [DateFrom]=@date
            end
            else 
            begin
                insert into Attendances(EmployeeId,DateFrom,AttendanceTypeId,AttValue)
                values(@empid,@date,'1',@att)
            end`
    //console.log($("#ATTvalue").val());
    // //console.log(sql);
    var data = excuteSQL(sql);
    ////console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function del_ATT() {
    var sql = `update attendances set AttValue=NULL where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    ////console.log(sql);
    var data = excuteSQL(sql);
    ////console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function ins_LATE() {
    var sql = `update attendances set Late=${$("#LATEvalue").val()} where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    ////console.log(sql);
    var data = excuteSQL(sql);
    ////console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function del_LATE() {
    var sql = `update attendances set Late=NULL where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    ////console.log(sql);
    var data = excuteSQL(sql);
    ////console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function ins_LEAVE() {
    var sql = `update attendances set Leave=${$("#LEAVEvalue").val()} where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    ////console.log(sql);
    var data = excuteSQL(sql);
    ////console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}

function del_LEAVE() {
    var sql = `update attendances set Leave=NULL where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}'`;
    //console.log(sql);
    var data = excuteSQL(sql);
    //console.log(data);
    if (data == 1)
        alert("تم التسجيل بنجاح");
    else
        alert("خطأ");
}
$("#MODALIN .dayIn").change(() => {
    var sql1 = "select cast(DateTime as time(0))time,Type from AClogs where dateadd(day,-1,cast(DateTime as date))='" + $(".MODALIN #DIN").html() + "' and FileNumber=" + FN + " and type='in' order by DateTime";
    var sql0 = "select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='" + $(".MODALIN #DIN").html() + "' and FileNumber=" + FN + " and type='in' order by DateTime";
    let data;
    if ($("#MODALIN .dayIn").is(':checked')) {
        data = readSQL(sql1);
    } else {
        data = readSQL(sql0);
    }

    var html = '<option >--select--</option>';
    for (var i = 0; i < data.length; i++) {
        html += '<option>' + data[i].time + '</option>';
    }
    //console.log(html);
    $('#TIN').val('');
    $('.MODALIN #deviceIN').html(html);
    //$('#MODALIN').modal('show');     
});
$("#MODALOUT .dayOut").change(() => {
    var sql1 = "select cast(DateTime as time(0))time,Type from AClogs where dateadd(day,-1,cast(DateTime as date))='" + $(".MODALOUT #DOUT").html() + "' and FileNumber=" + FN + " and type='out' order by DateTime";
    var sql0 = "select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='" + $(".MODALOUT #DOUT").html() + "' and FileNumber=" + FN + " and type='out' order by DateTime";
    let data;
    if ($("#MODALOUT .dayOut").is(':checked')) {
        data = readSQL(sql1);
    } else {
        data = readSQL(sql0);
    }

    var html = '<option >--select--</option>';
    for (var i = 0; i < data.length; i++) {
        html += '<option>' + data[i].time + '</option>';
    }
    //console.log(html);
    $('#TOUT').val('');
    $('.MODALOUT #deviceOUT').html(html);
    //$('#MODALIN').modal('show');     
});
//console.log(userLog());
function changeLog() {

}
// <script>

$(".sp-ch").eq(0).show();

$("#specialChoise").change(() => {
    $(".sp-ch").hide();
    $(".sp-ch").eq($("#specialChoise").val() - 1).show();
})

//for save removel
$("#spBTN").click(() => {

    let sql = ''
    let date = $("#DRemove").text();
    switch ($("#specialChoise").val()) {
        case "1":
            {//removal_ak
                sql += 'insert into Absences([DateFrom],[HoursNo],[AbsenceTypeId],[EmployeeId],[isRemoveGap])'
                sql += `values('${date}',${$(".sp-ch #dayPartSpecial").eq(0).val()},11,(select id from employees where filenumber=${FN}),1)`

            }
            break;
        case "2":
            {
                sql += 'insert into Absences([DateFrom],[TimeFrom],[TimeTo],[AbsenceTypeId],[EmployeeId],[isRemoveGap])'
                sql += `values('${date}','${$(".sp-ch #TINMission").eq(0).val()}','${$(".sp-ch #TOUTMission").eq(0).val()}',4,(select id from employees where filenumber=${FN}),1)`

            } break;
        case "3":
            {
                sql += 'insert into Absences([DateFrom],[TimeFrom],[TimeTo],[DayPart] ,[AbsenceTypeId],[EmployeeId],[isRemoveGap])'
                sql += `values('${date}','${$(".sp-ch #TINMission").eq(1).val()}','${$(".sp-ch #TOUTMission").eq(1).val()}',${$(".sp-ch #dayPartSpecial").eq(1).val()},3,(select id from employees where filenumber=${FN}),1)`

            }
            break;
        case "4":
            {
                sql += 'update attendances set [AttValue]=1-' + $(".sp-ch #dayPartSpecial").eq(2).val()
                sql += ` where employeeid=(select id from employees where filenumber=${FN}) and [DateFrom]='${date}'`

            }
            break;
        case "5":
            {
                sql += 'insert into Absences([DateFrom],[AbsenceTypeId],[EmployeeId],[isRemoveGap])'
                sql += `values('${date}',16,(select id from employees where filenumber=${FN}),1)`

            }
            break;

    }
    console.log(sql)
    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
})
function valu(e) { return e == '' ? `NULL` : `'${e}'` }
function create_dedu() {
    let sqlC = `
      insert into Deductions
      (Date,Amount,PaymentDeductionId,RecurringOneTimeId,TotalNet,
      EmployeeId,Remarkes,DaysNumber,Reason,Imposer)
      values(
        ${valu($("#MODALDEDU #date").val())},
        ${valu($("#MODALDEDU #amount").val())},
        ${valu($("#MODALDEDU #type").val())},
        ${valu($("#MODALDEDU #times").val())},
        ${valu($("#MODALDEDU #totalNet").val())},
        (select id from employees where filenumber=${$("#FN").val()}),
        ${valu($("#MODALDEDU #remarks").val())},
        ${valu($("#MODALDEDU #daysNumber").val())},
        ${valu($("#MODALDEDU #reason").val())},
        ${valu($("#MODALDEDU #by").val())}
      )
     `
   // console.log($("#MODALDEDU #fileNo").val());
    //console.log($("#FN").val());
    //console.log(sqlC);

    if (excuteSQL(sqlC) == 1) {
        alert("تم الحفظ بنجاح");
    }
    else {
        alert("خطأ");
    }
}
function delete_dedu(){
 let sqlC = `
      delete from Deductions where
      Date=${valu($("#MODALDEDU #date").val())} and 
      PaymentDeductionId =4 and 
      EmployeeId = (select id from employees where filenumber=${$("#FN").val()})
     `
    console.log(sqlC);

    if (excuteSQL(sqlC) == 1) {
        alert("تم الحذف بنجاح");
    }
    else {
        alert("خطأ");
    }
}

