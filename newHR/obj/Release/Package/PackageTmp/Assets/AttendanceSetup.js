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
    width: 500,
    height: 300,
    autoOpen: false
});
//احضار البيانات مع للعامل 

//ضبط الشهر الافتراضي
$("#M").val(new Date().getMonth() + 1);
//دالة عرض الجدول
function display(res) {
    var v = JSON.parse(res);

    $("#personal_data").html('<div class="row row-reversed" style="font-size:24px;"><div class="col-1 p-1" >الاسم</div><div class="col-5 p-1">' + ajaxC[0].name + '</div>'
                            + '<div class="col-1 p-1" >القسم</div><div class="col-5 p-1">' + ajaxC[0].dept + '</div></div>');
    console.log(v)
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
    html += '</tr></thead><tbody>';
    var attSum = 0, absSum = 0, vacSum = 0, lateSum = 0,lateDeduSum = 0, leaveSum = 0, overSum = 0, errandSum = 0;
    //console.log(v.findIndex(W=>W.lateHours < 1 && W.lateHours > 0))
    if (v.findIndex(W=>W.lateHours < 1 && W.lateHours > 0) > -1) v[v.findIndex(W=>W.lateHours < 1 && W.lateHours > 0)]["lateHours"] = "انذار";
    

    for (var i = 0; i < v.length; i++) {
       
        if (v[i]["weekend"] == 0) {
            html += '<tr>';
        }
        else {
            html += '<tr style="background-color:#05F548">';
            //(v[i-1]["att"] == null ? 0: v[i-1]["att"])
            //(v[i+1]["att"] == null ? 0: v[i+1]["att"])
            switch (i) {
                case 0: {
                    if ((v[i + 1]["att"] == null ? 0 : v[i + 1]["att"]) == 0) {
                        v[i]["att"] = 0;
                    }
                } break;
                case v.length-1: {
                    if ((v[i - 1]["att"] == null ? 0 : v[i - 1]["att"]) == 0) {
                        v[i]["att"] = 0;
                    }
                } break;
                default: {
                    if ((v[i + 1]["att"] == null ? 0 : v[i + 1]["att"]) == 0 && (v[i - 1]["att"] == null ? 0 : v[i - 1]["att"]) == 0) {
                        v[i]["att"] = 0;
                    }
                } break;
            }
        }
        var abs = (1 - v[i]["att"] - v[i]["vacDay"]).toFixed(2);

        html += '<td class="--bs-cyan" style="background-color:">' + v[i]["DayName"] + '</td>';
        html += '<td style="background-color:#2098bb55">' + v[i]["dateDay"].substr(0, 10) + '</td>';
        html += '<td style="background-color:">' + (v[i]["TimeFrom"] == null ? '' : v[i]["TimeFrom"]) + '</td>';
        html += '<td style="background-color:">' + (v[i]["TimeTo"] == null ? '' : v[i]["TimeTo"]) + '</td>';
        html += '<td class="table-success">' + (v[i]["att"] == null ? '' : v[i]["att"] <= 0 ? '' : v[i]["att"]) + '</td>';
        html += '<td class="table-danger">' + (abs > 0 ? abs : '') + '</td>';
        html += '<td class="table-warning">' + (v[i]["vacDay"] == null ? '' : v[i]["vacDay"] <= 0 ? '' : v[i]["vacDay"]) + '</td>';
        html += '<td class="table-secondary">' + (v[i]["lateHours"] == null ? '' : v[i]["lateHours"] <= 0 ? '' : v[i]["lateHours"]) + '</td>';
        html += '<td class="table-primary">' + (v[i]["leaveHours"] == null ? '' : v[i]["leaveHours"] <= 0 ? '' : v[i]["leaveHours"]) + '</td>';
        if (v[i].overTimeC < -0.66 && v[i]["overTimeHour"] == null) html += '<td style="background-color:silver">' + (v[i]["overTimeHour"] == null ? '&#9889' : v[i]["overTimeHour"] <= 0 ? '' : v[i]["overTimeHour"]) + '</td>';
        else html += '<td style="background-color:gold">' + (v[i]["overTimeHour"] == null ? '' : v[i]["overTimeHour"] <= 0 ? '' : v[i]["overTimeHour"]) + '</td>';
        html += '<td ">' + (v[i]["errand"] == null ? '' : '&#10003') + '</td>';
        html += '</tr>';
        attSum += v[i]["att"] == null ? 0 : v[i]["att"];
        absSum += parseFloat(abs)
        vacSum += v[i]["vacDay"] == null ? 0 : v[i]["vacDay"];
        if (lateSum == 2) { lateDeduSum++; }
        lateSum += v[i]["lateHours"] == null ? 0 : (Object.is(parseFloat(v[i]["lateHours"]), NaN) ? 0 : parseFloat(v[i]["lateHours"]));
        if (lateSum > 2) { lateSum -= v[i]["lateHours"]; lateDeduSum++; }
        leaveSum += v[i]["leaveHours"] == null ? 0 : v[i]["leaveHours"];
        overSum += v[i]["overTimeHour"] == null ? 0 : v[i]["overTimeHour"];
        errandSum += v[i]["errand"] == null ? 0 : v[i]["errand"];
    }
    var dValue = 0;
    switch (lateDeduSum % 3) {
        case 0: dValue = parseInt(lateDeduSum / 3) * 1.75; break;
        case 1: dValue = parseInt(lateDeduSum / 3) * 1.75 + .25; break;
        case 2: dValue = parseInt(lateDeduSum / 3) * 1.75 + .75; break;
    }
    html += '<tr><td rowspan=2 colspan="4">أجمالي</td><td rowspan=2>' + attSum + '</td><td rowspan=2>' + absSum + '</td><td rowspan=2>' + vacSum + '</td><td>' + lateSum + '</td><td rowspan=2>' + leaveSum + '</td><td rowspan=2>' + overSum + '</td><td rowspan=2>' + errandSum + '</td></tr><tr><td>' + dValue + '</td></tr></tbody></table>';
    $("#mytable").html(html);
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
//console.log($(".myModal .btn").length);
$(".myModal .btn").on("click", function (e) {
    switch ($(".myModal .btn").index(e.target)) {
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
    FN = ajaxC[0].filenumber;

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
        case 6: { $('.myModal').eq(4).modal('show'); } break;
        case 9: { $('.myModal').eq(3).modal('show'); } break;
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
            $('.myModal').eq(2).modal('show');
        } break;
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
                     values('${dateFrom}', '${$("#vacFrom").val()}', '${$("#vacTo").val()}', ${type}, (select id from Employees where FileNumber=${FN}),
(select case when '${$("#vacFrom").val()}'='' or '${$("#vacTo").val()}'='' then 1 else cast(case when '${$("#vacFrom").val()}'<= '${$("#vacTo").val()}' then DATEDIFF(MINUTE, '${$("#vacFrom").val()}', '${$("#vacTo").val()}')
else 24*60+DATEDIFF(MINUTE, '${$("#vacFrom").val()}', '${$("#vacTo").val()}')
end as float(2)) /60/(select DailyHours from Shifts where ShiftId=(select ShiftId from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=${FN})))
                   end),
                           '${ref1}', '${ref2}') `;

    if (excuteSQL(sql) == 1) alert("تم الحفظ بنجاح");
    else alert("خطأ");
}

function del_vacation() {
    var sql = `delete from absences where EmployeeId=(select id from Employees where FileNumber=${FN}) and dateFrom ='${dateFrom}' `;
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
    var data = ExcuteSQL(sql);
    console.log(data);
}