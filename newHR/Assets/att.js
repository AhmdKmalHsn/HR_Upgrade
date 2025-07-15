 
    var v1=[], v2=[];
	var data=[];

    function check(d,f) {
        $.ajax({
            url:"/PostingPeriod/checkPeriod",
            data: {date: d},
            success: function (data) {
                console.log(JSON.parse(data));
                if (JSON.parse(data)[0].c == 0) f();
                else alert("Not permited!your posting period is Locked");

            }
        });
    }

    function addAddition(from1, fileNo1, hoursNo1, offset1) {
           var dat={ from: from1, fileNo: fileNo1, hoursNo: hoursNo1, offset: offset1 }
           var values=[];
           if(offset1==1)
           {
               values=["التاريخ",from1,"رقم الملف",fileNo1,"أضافي قبل",hoursNo1];
           }
           else
           {
               values=["التاريخ",from1,"رقم الملف",fileNo1,"أضافي بعد",hoursNo1];
           }
		   changeLog("اضافي","c",values,[],false);
			$.ajax({
                url: "/reports/createAddition",//:,,,
                data: dat,
                method: "post",
                type: "json",
                success: function (data) {
                    console.log(data);
                    //search();
                },
                error:
                    function (response) {
                        console.log(response);
                    }
            });
        }

    function addVacation(from1, fileNo1, dayPart1,type1) {
            changeLog("أجازة","c",["التاريخ",data.Date,"رقم الملف",fileNo1,"مدة الاجازة",dayPart1,"نوع الاجازة",type1],[],false);

            $.ajax({
                url: "/reports/createVacation",//:,,,
                data: { from: from1, fileNo: fileNo1, dayPart: dayPart1,type:type1 },
                method: "post",
                type: "json",
                success: function (data) {
                    console.log(data);
                    //search();
                },
                error:
                    function (response) {
                        console.log(response);
                    }
            });
        }
    function delAddition(from1, fileNo1) {
        var values=["التاريخ",from1,"رقم الملف",fileNo1,"أضافي",data.addition];

           changeLog("اضافي","d",values,[],false);
            $.ajax({
                url: "/reports/deleteAddition",//:,,,
                data: { from: from1, fileNo: fileNo1 },
                method: "post",
                type: "json",
                success: function (data) {
                    console.log(data);
                    //search();
                },
                error:
                    function (response) {
                        console.log(response);
                    }
            });
        }
    function delVacation(from1, fileNo1) {
            changeLog("أجازة","d",["التاريخ",data.Date,"رقم الملف",data.fileNo,"مدة الاجازة",data.vaction],[],false);

            $.ajax({
                url: "/reports/deleteVacation",//:,,,
                data: { from: from1, fileNo: fileNo1 },
                method: "post",
                type: "json",
                success: function (data) {
                    console.log(data);
                    //search();
                },
                error:
                    function (response) {
                        console.log(response);
                    }
            });
        }



    $('#add').click(function () {

             if (document.getElementById("afr_chk").checked == true) {
                if ($("#afr_txt").val() > 0) {
                    addAddition($('#modalDate').text(),document.getElementById("modalFN").innerHTML, $("#afr_txt").val(), 0)
                    console.log("ok after");
                }
                else {
                    alert("تأكد من قيمة ساعات الاضافي");
                }
            }
            if (document.getElementById("bfr_chk").checked == true) {
                if ($("#bfr_txt").val() > 0) {
                    addAddition($('#modalDate').text(), document.getElementById("modalFN").innerHTML, $("#bfr_txt").val(), 1)
                    console.log("ok before");
                }
                else {
                    alert("تأكد من قيمة ساعات الاضافي");
                }
            }
        });
    $('#delete').click(function () {
            //console.log($('#dt').val() + "," + document.getElementById("modalFN1").innerHTML + "," + $("#vac_txt").val() + "," + $("#type").val());
            delAddition($('#modalDate').text(), document.getElementById("modalFN").innerHTML);
        });
    $('#add1').click(function () {
            if ($("#vac_txt").val()==1||$("#vac_txt").val() ==.5||$("#vac_txt").val() ==.25) {
                addVacation($('#modalDate1').text(), document.getElementById("modalFN1").innerHTML, $("#vac_txt").val(), $("#type").val());
                }
                else {
                    alert("تأكد من قيمة الاجازة");
                }
        });
    $('#delete1').click(function () {
                console.log($('#modalDate1').text() + "," + document.getElementById("modalFN1").innerHTML + "," + $("#vac_txt").val() + "," + $("#type").val());
                delVacation($('#modalDate1').text(), document.getElementById("modalFN1").innerHTML);
        });



$('#container').on('click', 'td', function () {
            //var table = $('#example').DataTable();
            //data = table.row(this).data();
            var index =this.cellIndex;
            console.log($(this).closest('tr').find('td:eq(1)').text());
            if (index == 6) {
			    //الاجازة
    v1=[
        "Date", $(this).closest('tr').find('td:eq(1)').text(),
        "vaction", $(this).closest('tr').find('td:eq(6)').text(),
        "deptName", data.deptName,
        "fileNo", $("#fileNo").val(),
        "name", $("#n").text()
       ]


                $('#modalName1').html($("#n").text());
                $('#modalDate1').html($(this).closest('tr').find('td:eq(1)').text());
                $('#modalFN1').html( $("#fileNo").val());
                $('#vacationModal').modal('show');
            }
            else if (index == 9) {
                //الاضافي
 v1=[
     "Date", $(this).closest('tr').find('td:eq(1)').text(),
     "addition", $(this).closest('tr').find('td:eq(9)').text(),
     "deptName", data.deptName,
     "fileNo", $("#fileNo").val(),
     "name", $("#n").text()
    ];
                $('#modalName').html($("#n").text());
                $('#modalDate').html($(this).closest('tr').find('td:eq(1)').text());
                $('#modalFN').html($("#fileNo").val());
                $('#exampleModalLong').modal('show');
            }
            console.log(v1);
        });

function changeLog(tbl,crud,data1,data2,is_update)
	{
        $.ajax({
            url: "/ChangeLog/createLog",
            data: {
                user: '@Request.Cookies["UserName"].Value' ,
                ip: '@HttpContext.Current.Request.UserHostAddress' ,
                crud: crud,
                table: tbl,
                value1: JSON.stringify(data1),
                value2: JSON.stringify(data2),
                isUpdate: is_update
            },
            success: function (res) { console.log(res);},
            fail: function (res) { console.log(res); }
        });
    }
  
 $(function () {
            $('#loader').hide();
            $('.table-responsive-lg').show();
            function ajaxExcute() {
                $('#loader').show();
                $('.table-responsive-lg').hide();
                $.ajax(
                {
                    type: 'POST',
                    dataType: 'JSON',
                    url: 'SalaryFNO',
                    data: { fileNo: $('#fileNo').val(), M: $('#M').val(), Y: $('#Y').val() },
                    success:
                        function (response) {

                            $('#loader').hide();
                            response = JSON.parse(response);
                            console.log(response);
                            if (EMPS.includes('' + response[0].fileNo))
                            {

                           $('.table-responsive-lg').show();
                           var Days=[];
                            let  l = 0,
                                ll = 0,//
                                d = 0,//
                                occur = 0,//
                                add = 0,//
                                attSum = 0,//
                                vacSum = 0,//
                                ghSum = 0,//
                                h = 0,//
                                occurFirst=false,
								abs;
								fridays=0;
                            //var event = new Date();
                            var html = '';
                            var data = [];
                            response[0]["dates"] = response[0]["dates"].substr(0, 10);// "2021-12-26T00:00:00",
                            response[0]["TIMEFROM"] = response[0]["TIMEFROM"] == null ? '' : response[0]["TIMEFROM"];
                            response[0]["TIMETO"] = response[0]["TIMETO"] == null ? '' : response[0]["TIMETO"];
                            response[0]["addition"] = response[0]["addition"] == null ? 0 : response[0]["addition"];
                            response[0]["DayPart"] = response[0]["DayPart"] == null ? 0 : response[0]["DayPart"];
                            response[0]["absenceValue"] = response[0]["absenceValue"] == null ? 0 : response[0]["absenceValue"];
                            response[0]["joiningDate"] = response[0]["joiningDate"].substr(0, 10);
                            response[0]["lateValue"] = response[0]["lateValue"] == null ? -1 : response[0]["lateValue"];
                            response[0]["leaveValue"] = response[0]["leaveValue"] == null ? -1 : response[0]["leaveValue"];


                            data.push(response[0]);
							for(var i=1;i<response.length;i++)
							{
							    if (response[i].day !== response[i - 1].day)
							    {
							        response[i]["dates"] = response[i]["dates"].substr(0, 10);// "2021-12-26T00:00:00",
							        response[i]["TIMEFROM"] = response[i]["TIMEFROM"] == null ? '' : response[i]["TIMEFROM"];
							        response[i]["TIMETO"] = response[i]["TIMETO"] == null ? '' : response[i]["TIMETO"];
							        response[i]["addition"] = response[i]["addition"] == null ? 0 : response[i]["addition"];
							        response[i]["DayPart"] = response[i]["DayPart"] == null ? 0 : response[i]["DayPart"];
							        response[i]["joiningDate"] = response[i]["joiningDate"].substr(0, 10);//"2020-08-15T00:00:00",
							        response[i]["absenceValue"] = response[i]["absenceValue"] == null ? 0 : response[i]["absenceValue"];
                                    response[i]["lateValue"] = response[i]["lateValue"] == null ? -1 : response[i]["lateValue"];
                                    response[i]["leaveValue"] = response[i]["leaveValue"] == null ? -1 : response[i]["leaveValue"];
							        data.push(response[i])
							    }
							}
							var diffDay = getDayData(response[0].fileNo, data[0].dates.substr(0, 10), data[data.length-1].dates.substr(0, 10));
							console.log(data);
							for (var i = 0; i < data.length; i++)
							{
                                if (data[i].joiningDate == "" && data[i].weekend == 0) data[i].joiningDate = "1990-01-01";
							let ghDay = 0, gh = 0; var xlate = 0;

                            if (data[i].weekend == 0)
                            {
								//بداية الحسابات

                                abs = data[i].DayPart == 0 ? data[i].absenceValue : data[i].DayPart
                                //if (Days[i].weekend == 0) {
                                    var a = reCalc(reSet(diffDay.filter(w=>w.date.substr(0,10) == data[i].dates)));
                                    //console.log(data[i].dates+":=>"+ a);
                                //}
                                // calc day numberس
                                gh = (60 * data[i].DailyHours)
                                   - data[i].attend
                                   - (data[i].lateValue > -1 ?
								          (data[i].Late>=data[i].DailyHours/4*60 && data[i].Late<data[i].DailyHours/2*60 ?data[i].Late-data[i].DailyHours/4*60:
										  (data[i].Late>=data[i].DailyHours/2*60 ?data[i].Late-data[i].DailyHours/2*60:
										  data[i].Late)) : 0)
                                        - (data[i].leaveValue > -1 ?
                                          (data[i].Leave>=data[i].DailyHours/4*60 && data[i].Leave<data[i].DailyHours/2*60 ?data[i].Leave-data[i].DailyHours/4*60:
										  (data[i].Leave>=data[i].DailyHours/2*60 ?data[i].Leave-data[i].DailyHours/2*60:
								          data[i].Leave)) : 0)
                                   - (data[i].Leave < 0 ? data[i].Leave : 0)
                                   - (data[i].Late < 0 ? data[i].Late : 0)
                                   - (60 * abs * data[i].DailyHours)
                                   + (data[i].specialValue == -1 ? data[i].special : 0)
                                   //+ a;
                                //end no of days

                                ghDay = 0;
                                if (gh > 0 && gh <= (60 * data[i].DailyHours / 4)) ghDay = 0.25;
                                if (gh <= (60 * data[i].DailyHours / 2) && gh > (60 * data[i].DailyHours / 4)) ghDay = 0.5;
                                if (gh > (60 * data[i].DailyHours / 2) && gh <= (3 * 60 * data[i].DailyHours / 4)) ghDay = 0.75;
                                if (gh > (3 * 60 * data[i].DailyHours / 4)) ghDay = 1;
                                if (data[i].attend == 0) ghDay = 1 - abs;
                                /*if(data[i].AttValue!=null)*/
                                ////now here 
                                //نهاية الحسابات
                                $('#n').html('<h1>' + data[i].name + '</h1>');
                                //التاخيرات

                                if (
								       data[i].lateValue > 0
									   &&(
									   (data[i].Late > 0 && data[i].Late < 53) ||
									   (data[i].Late-data[i].DailyHours*60/4 > 0 && data[i].Late-data[i].DailyHours*60/4 < 53)||
									   (data[i].Late-data[i].DailyHours*60/2 > 0 && data[i].Late-data[i].DailyHours*60/2 < 53)
									   )
									)
									{
									   occur++;
									}

                                if (data[i].lateValue > 0
								&&// لما يكون الحضور بدون اضافي
								     (
									   parseInt(data[i].attend) +
                                       data[i].DayPart * 60 * data[i].DailyHours +
                                       (data[i].Leave < 0 ? data[i].Leave : 0)-
                                       (data[i].specialValue == -1 ? data[i].special : 0)
									   <data[i].DailyHours*60
									 )
									)
									{
                                    if (occur == 1&& occurFirst==false)
									{
									    occurFirst=true;
                                        xlate = 'انذار';
                                    }
									else
									{

                                        xlate = data[i].lateValue;
                                        if (data[i].lateValue > 0 && l >= 2) { d++; }
                                        if (l < 2 && l >= 0) { l += data[i].lateValue; }
                                    }
                                }
                                else
                                {
                                    xlate = '';
                                }
								}

                                //ايام الحضور
                                if (data[i].weekend == 1 )
									{
                                         ghDay=0;
									     fridays++;

									}
                                   //نهاية الحضور

									//بداية الغياب


                                var obj =
                                    {
                                     day:data[i].day
                                    ,fileNo: data[i].fileNo
                                    ,date:data[i].dates.substr(0,10)
                                    ,timeIn:data[i].TIMEFROM
                                    ,timeOut:data[i].TIMETO
                                    ,att:(data[i].AttValue==null?(1-ghDay-data[i].DayPart):(data[i].AttValue))//hoho
                                    ,vac:data[i].DayPart
                                    ,abs:(data[i].AttValue==null?(ghDay):(1-data[i].DayPart-data[i].AttValue))
                                    ,weekend:data[i].weekend
                                    ,worktime:data[i].DailyHours*60
                                    ,vacTypeId:data[i].AbsenceTypeId
                                    ,late:xlate
                                    ,leave:data[i].leaveValue
                                    ,overtime:data[i].addition
                                    ,mission:data[i].mission==null?'':data[i].mission
                                    ,special:data[i].special==null?'':data[i].special
                                    ,specialValue:data[i].specialValue==null?'':data[i].specialValue
                                   }
                                var stData =
                                    {
                                        name: response[0].name,
                                        fileNo: response[0].fileNo,
                                        dept: response[0].Dept,
                                        monthly: response[0].monthlySalary,
                                        hourNo: response[0].DailyHours,
                                        regular: response[0].regular,
                                        expensive: response[0].expensive,
                                        management: response[0].management,
                                        skill: response[0].skill,
                                        overRate: response[0].Overtime,
                                        insurance: response[0].insurance,
                                        deductions: response[0].deductions,
                                        sanctions: response[0].sanctions,
                                        clothes: response[0].clothes,
                                        loans: response[0].loans,
                                    }
                                Days.push(obj);
							}
							console.log(Days);
							console.log(stData);
                            ///نهاية الشهر
                            //حساب جزاءات التأخير
                        
							var salary = display1(Days);
							var totalCut = 0, totalHave = 0;
					//vacSum3//سنوية
                    //vacSum5//بدون خصم
                    //vacSum6//رسمية
                    //vacSum12//من رصيد قادم
                    //vacSum13//بالخصم
                    //vacSum14//بدل جمعة
                    //vacSum15//بدل تطبيق
							var TotalDays = salary.absSum + salary.vacSum + salary.attSum;
							var ActualDays = salary.attSum + salary.vacSum3 + salary.vacSum5 + salary.vacSum6 + salary.vacSum12 + salary.vacSum14 + salary.vacSum15;
							console.log("total="+ TotalDays);
							console.log("actual=" + ActualDays);
							console.log("attsum=" + salary.attSum);
							console.log("salary");
							console.log(salary);

							var CalcExpensive = (ActualDays / (TotalDays > 30 ? TotalDays - 1 : TotalDays) * stData.expensive).toFixed(2) ;
							var CalcSkill = (ActualDays / (TotalDays > 30 ? TotalDays - 1 : TotalDays) * stData.skill).toFixed(2);
							var CalcManage = (ActualDays / (TotalDays > 30 ? TotalDays - 1 : TotalDays) * stData.management).toFixed(2);
							
							var CalcRegular=0;
							switch (true)
							{
							    case salary.absSum<= 1.5: CalcRegular = stData.regular; break;
							    case (salary.absSum> 1.5 && salary.absSum<= 1.75): CalcRegular = stData.regular * 0.75; break;
							    case salary.absSum> 1.75: CalcRegular = 0; break;
							}

							totalHave = (
                                parseFloat(ActualDays * stData.monthly / 30) +
                                parseFloat(CalcRegular )+
                                parseFloat(CalcSkill )+
                                parseFloat(CalcExpensive) +
                                parseFloat(CalcManage) +
                                parseFloat(salary.overSum * stData.overRate * stData.monthly / 30 / stData.hourNo)
                                ).toFixed(2);

							totalCut = (
                                        parseFloat(stData.insurance) +
                                        parseFloat(stData.deductions) +
                                        parseFloat(salary.dValue * stData.monthly / 30) +
                                        parseFloat(stData.sanctions) +
                                        parseFloat(stData.clothes) +
                                        parseFloat((salary.lateSum +salary.leaveSum  + salary.specSum) * stData.monthly / 30 / stData.hourNo) +
                                        parseFloat(stData.loans)
                                        ).toFixed(2);
                            $('#_container').html(
                           '<tr ><td colspan="4">' + stData.name + '</td></tr>' +
                           '<tr style="background-color:yellow"><td>' + stData.fileNo + '</td><td>رقم الملف</td><td>' + stData.dept + '</td><td>القسم</td></tr>' +
                           '<tr><td colspan="2" style="background-color:red">الاستقطاعات</td><td colspan="2" style="background-color:green">الاستحقاقات</td></tr>' +

                           '<tr><td >' + stData.insurance + '</td><td > خصم التامينات </td><td >' + (ActualDays * stData.monthly / 30).toFixed(2) + '</td><td > قيمة الحضور </td></tr>' +
                           '<tr><td >' + (parseFloat(stData.deductions) + (salary.dValue * stData.monthly / 30)).toFixed(2) + '</td><td > الخصومات </td><td >' + CalcRegular + '</td><td > حافز الانتظام </td></tr>' +
                           '<tr><td >' + stData.sanctions + '</td><td > الجزاءات </td><td >' + CalcSkill + '</td><td > حافز المهارة </td></tr>' +
                           '<tr><td >' + stData.clothes + '</td><td > ملبس </td><td >' + CalcExpensive + '</td><td > غلاء المعيشة </td></tr>' +
                           '<tr><td >' + ((salary.lateSum +  salary.leaveSum  + salary.specSum) * stData.monthly / 30 / stData.hourNo).toFixed(2) + '</td><td > تاخيرات </td><td >' + CalcManage + '</td><td > حافز مجلس الادارة </td></tr>' +
                           '<tr><td >' + stData.loans + '</td><td > سلف </td><td >' + (salary.overSum * stData.overRate * stData.monthly / 30/stData.hourNo).toFixed(2) + '</td><td > أضافي </td></tr>' +

                           '<tr><td >' + totalCut + '</td><td >  اجمالي الاستقطاعات</td><td >'
                           + totalHave + '</td><td > اجمالي الاستحقاقات </td></tr>' +
                           '<tr><td colspan="2">' + (totalHave - totalCut).toFixed(2) + '</td><td colspan="2"> صافي المرتب </td></tr>'
                           );
                           }
                           else
                               alert("غير مسموح !");
                        },
                    error:
                        function (response)
                        {
                            console.log("Error: " + response);
                        }
                });
                //end ajax card
           }

            $('#ok').click(function () {
                if ($('#fileNo').val())
                        ajaxExcute();
                    else alert("enter file number!");
            });

            $('#fileNo,#Y,#M').keyup(function (event) {
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '13') {
                    if ($('#fileNo').val())
                        ajaxExcute();
                    else alert("enter file number!");
                }
            });

        });
        /**************display()**************/

 function display1(Days)
          {
            var html = '',
               ActualAttSum=0
               attSum=0 ,
               absSum=0 ,
               vacSum=0 ,
               vacSum3=0 ,
               vacSum5=0 ,
               vacSum6=0 ,
               vacSum12=0 ,
               vacSum13=0 ,
               vacSum14=0 ,
               vacSum15=0 ,
               lateSum=0 ,
               leaveSum=0 ,
               overSum =0,
               missionSum=0,
               specSum=0,
               deduSum=0;
              //var diffDay = getDayData(Days[i].fileNo);

 if (Days.length > 30) {
     Days[Days.findIndex((w) => w.weekend ==1)].att = 0  
 } 
for (var i = 0; i < Days.length; i++)
    {
         
         if (Days[i].weekend == 1)
         {
             var j=i, k=i;
             if (i == 0)
             {
                 /*while (Days[k].weekend == 1)
                 {
                     k++;
                 }
                 if (Days[k].att > 0)
                 {
                     Days[i].att = 1;
                     Days[i].abs = 0;
                 }
                 else
                 {
                     Days[i].att = 0;
                     Days[i].abs = 1;
                 } */
             }
             else if (i == Days.length - 1)
             {
                 /*while (Days[j].weekend == 1)
                 {
                     j--;
                 }
                 if (Days[j].att > 0) {
                     Days[i].att = 1;
                     Days[i].abs = 0;
                 }
                 else {
                     Days[i].att = 0;
                     Days[i].abs = 1;
                 }*/
             }
             else
             {
                 while (Days[j].weekend == 1 && j>0) {
                     j--;
                 }
                 while (Days[k].weekend == 1 && k<Days.length-1) {
                     k++;
                 }
                 if (Days[j].att > 0 || Days[k].att > 0) {
                     Days[i].att = 1;
                     Days[i].abs = 0;
                 }
                 else {
                     Days[i].att = 0;
                     Days[i].abs = 1;
                 }
             }
             
         }
              attSum+=Days[i].att ;
              absSum+=Days[i].abs ;
              vacSum+=Days[i].vac ;
              switch(Days[i].vacTypeId)
              {
                  case 3:vacSum3+=Days[i].vac; break;//سنوية
                  case 5:vacSum5+=Days[i].vac; break;//بدون خصم
                  case 6:vacSum6+=Days[i].vac; break;//رسمية
                  case 12:vacSum12+=Days[i].vac; break;//من رصيد قادم
                  case 13:vacSum13+=Days[i].vac; break;//بالخصم
                  case 14:vacSum14+=Days[i].vac; break;//بدل جمعة
                  case 15:vacSum15+=Days[i].vac; break;//بدل تطبيق
              }
            if(Days[i].late>0)
            {
                deduSum+=(lateSum==2?1:0);
                if(lateSum+parseFloat(Days[i].late) <=2)
                {
                    lateSum+=parseFloat(Days[i].late);
                }
                else 
                {
                    if(lateSum<2)deduSum++;
                }
                //console.log(Days[i].date + "=>" + lateSum + "," + deduSum)
            }
            var dValue = 0;
            switch (deduSum % 3) {
                case 0: dValue = parseInt(deduSum / 3) * 1.75; break;
                case 1: dValue = parseInt(deduSum / 3) * 1.75 + .25; break;
                case 2: dValue = parseInt(deduSum / 3) * 1.75 + .75; break;

            }
              leaveSum += (Days[i].leave==-1?0:Days[i].leave);
              overSum += Days[i].overtime ;
              missionSum += (Days[i].mission==1?1:0);
              specSum += (Days[i].specialValue==-1?0:Days[i].specialValue);
             
if (Days.length > 30)
{
    if (Days.find(w=>w.weekend == 1).att == 1) ActualAttSum = attSum - 1;
    else ActualAttSum = attSum;
}
              if(Days[i].weekend==0) html+='<tr>';
                  else html+='<tr style="background-color:lightgreen">';
              html+='<td class="pt-3">'+Days[i].day+'</td>'+
                    '<td class="pt-3">'+Days[i].date+'</td>'+
                    '<td class="pt-3" onclick="selectIN(this)">'+Days[i].timeIn+'</td>'+
                    '<td class="pt-3" onclick="selectOUT(this)">'+Days[i].timeOut+'</td>'+
                    '<td class="pt-3" style="background-color: #27b527">'+(Days[i].att==0?'':Days[i].att)+'</td>'+
                    '<td class="pt-3" style="background-color: #dc3545">'+(Days[i].abs==0?'':Days[i].abs)+'</td>'+
                    '<td class="pt-3" style="background-color: #ffc107">'+(Days[i].vac==0?'':Days[i].vac)+'</td>'+
                    '<td class="pt-3" style="background-color: #df14c1">'+(Days[i].late==0||Days[i].late==-1?'':Days[i].late)+'</td>'+
                    '<td class="pt-3" style="background-color: #1196dd">'+(Days[i].leave==0||Days[i].leave==-1?'':Days[i].leave)+'</td>'+
                    '<td class="pt-3" style="background-color: #8907ff">'+(Days[i].overtime==0?'':Days[i].overtime)+'</td>'+
                    '<td class="pt-3" style="background-color: #77abbd" onclick="selectMission(this)">'+(Days[i].mission==1?'<svg xmlns="http://www.w3.org/2000/svg" color="green" width="30" height="30" fill="currentColor" class="bi bi-check" viewBox="0 0 16 16"><path d="M10.97 4.97a.75.75 0 0 1 1.07 1.05l-3.99 4.99a.75.75 0 0 1-1.08.02L4.324 8.384a.75.75 0 1 1 1.06-1.06l2.094 2.093 3.473-4.425a.267.267 0 0 1 .02-.022z"/></svg>':'')+'</td>'+
                    '<td class="pt-3" style="background-color: #0277bd" onclick="selectMission(this)">'+(Days[i].specialValue==0 ?'': Days[i].specialValue==-1?'<svg xmlns="http://www.w3.org/2000/svg" color="red" width="30" height="30" fill="currentColor" class="bi bi-check" viewBox="0 0 16 16"><path d="M10.97 4.97a.75.75 0 0 1 1.07 1.05l-3.99 4.99a.75.75 0 0 1-1.08.02L4.324 8.384a.75.75 0 1 1 1.06-1.06l2.094 2.093 3.473-4.425a.267.267 0 0 1 .02-.022z"/></svg>':Days[i].specialValue)+'</td>'+
                    '</tr>';
            }


$('#thead').html('<tr style="">' +
                                '<th scope=col>اليوم</th>' +
                                '<th scope=col>التاريخ</th>' +
                                '<th scope=col>الدخول</th>' +
                                '<th scope=col>الخروج</th>' +
                                '<th scope=col>حضور</th>' +
                                '<th scope=col>غياب</th>' +
                                '<th scope=col>اجازة</th>' +
                                '<th scope=col>تأخير</th>' +
                                '<th scope=col>انصراف مبكر</th>' +
                                '<th scope=col>أضافي</th>' +
                                '<th scope=col>مأمورية</th>' +
                                '<th scope=col>أذن</th>' +
                            '</tr>');
            $('#container').html(html)
            $('#tfoot').html(
                    '<tr><td colspan="4" rowspan=2>total</td><td rowspan=2>' +
                        attSum + '</td><td rowspan=2>' +
                        absSum + '</td><td rowspan=2>' +
                        vacSum + '</td><td>' +
                        lateSum + '</td><td rowspan=2>' +
                        leaveSum + '</td><td rowspan=2>' +
                        overSum + '</td><td rowspan=2>'+
                        missionSum+'</td><td rowspan=2>'+
                        specSum+'</td></tr><tr><td>'+
                        dValue+'</td></tr>'
                        );
            return {
                ActualAttSum: ActualAttSum,
                attSum: attSum,
                absSum: absSum,
                vacSum: vacSum,
                vacSum3: vacSum3,
                vacSum5: vacSum5,
                vacSum6: vacSum6,
                vacSum12: vacSum12,
                vacSum13: vacSum13,
                vacSum14: vacSum14,
                vacSum15: vacSum15,
                lateSum: lateSum,
                leaveSum: leaveSum,
                overSum: overSum,
                missionSum: missionSum,
                specSum: specSum,
                dValue: dValue,
            }
           
          }
        /***************************/

        var i = -1,INOUT='';
        function selectIN(r) {
            i = r.parentNode.rowIndex;
            var date=$("#container tr:eq(" + (i - 1) + ") td:eq(1)").html();
            var FN=$('#fileNo').val();
            var sql="select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='"+date+"' and FileNumber="+FN+" and type='in' order by DateTime";
            var data=readSQL(sql);
            var html='<option >--select--</option>';
            for(var i=0;i<data.length;i++)
            {
               html+='<option>'+data[i].time+'</option>';
            }
            $('#deviceIN').html(html);
            $('#FNIN').html(FN);
            $('#DIN').html(date);
            $('#TIN').val('');
            $('#MODALIN').modal('show');
        }
        function selectOUT(r) {
            i = r.parentNode.rowIndex;
            var date=$("#container tr:eq(" + (i - 1) + ") td:eq(1)").html();
            var FN=$('#fileNo').val();
            var sql="select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='"+date+"' and FileNumber="+FN+" and type='out' order by DateTime";
            var data=readSQL(sql);
            var html='<option >--select--</option>';
            for(var i=0;i<data.length;i++)
            {
               html+='<option>'+data[i].time+'</option>';
            }
            $('#deviceOUT').html(html);
            $('#FNOUT').html(FN);
            $('#DOUT').html(date);
            $('#TOUT').val('');
            $('#MODALOUT').modal('show');
        }
        function selectMission(r) {
            i = r.parentNode.rowIndex;
            var date=$("#container tr:eq(" + (i - 1) + ") td:eq(1)").html();
            var FN=$('#fileNo').val();
            var sql="select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='"+date+"' and FileNumber="+FN+" and type='in' order by DateTime";
            var data=readSQL(sql);
            var html='<option >--select--</option>';
            for(var i=0;i<data.length;i++)
            {
               html+='<option>'+data[i].time+'</option>';
            }
             
			var sql2="select cast(DateTime as time(0))time,Type from AClogs where cast(DateTime as date)='"+date+"' and FileNumber="+FN+" and type='out' order by DateTime";
            var data2=readSQL(sql2);
            var html2='<option >--select--</option>';
            for(var i=0;i<data2.length;i++)
            {
               html2+='<option>'+data2[i].time+'</option>';
            }///critical
            var sqlMission =`select 	TimeFrom	,TimeTo
 from Absences a 
 where  AbsenceTypeId=4 and 
        DateFrom = '${date}' and 
        EmployeeId=(select id from Employees where FileNumber='${FN}') `
        var dataMission=readSQL(sqlMission)
         console.log("dataMission");
        console.log(dataMission);
        if(dataMission.length==0){
            $('#deviceMIN').html(html);
            $('#deviceMOUT').html(html2);
            $('#FNMission').html(FN);
            $('#DMission').html(date);
            $('#TINMission').val('');
			$('#TOUTMission').val('');
            $('#MODALMission').modal('show');
            $('#deviceMIN').prop("disabled",false);
            $('#deviceMOUT').prop("disabled",false);
            $('#TINMission').prop("disabled",false);
            $('#TOUTMission').prop("disabled",false);
        }else
        {
            //prop('disabled', true);
            $('#deviceMIN').html();
            $('#deviceMIN').prop("disabled",true);
            $('#deviceMOUT').html();
            $('#deviceMOUT').prop("disabled",true);
            $('#FNMission').html(FN);
            $('#DMission').html(date);
            $('#TINMission').val(dataMission[0].TimeFrom);
            $('#TINMission').prop("disabled",true);
			$('#TOUTMission').val(dataMission[0].TimeTo);
            $('#TOUTMission').prop("disabled",true);
            $('#MODALMission').modal('show');
        }
           
        }
        $('#ADDIN').click(function(){
            INOUT=$('#TIN').val();
            $.ajax({
            type: 'POST',
            dataType: 'JSON',
            url: 'AttendIN',
            data: { fileNo: $('#FNIN').html(), d: $('#DIN').html(), t: $('#TIN').val() },
            success: function (response)
            {

                if (response == 1) {
                    $("#container tr:eq(" + (i - 1) + ") td:eq(2)").html(INOUT);//////////////////
                    alert("Saved Successfully!");
                }
                else
                    alert("Error!");
            },
            error: function (response)
            {
                alert("Error: " + response);
            }
        });
        });
        $('#ADDOUT').click(function () {
            INOUT = $('#TOUT').val();
            $.ajax({
                type: 'POST',
                dataType: 'JSON',
                url: 'AttendOUT',
                data: { fileNo: $('#FNOUT').html(), d: $('#DOUT').html(), t: $('#TOUT').val() },
                success: function (response) {
                    if (response == 1) {
                        $("#container tr:eq(" + (i - 1) + ") td:eq(3)").html(INOUT);//////////////////
                        alert("Saved Successfully!");
                    }
                    else
                        alert("Error!");
                },
                error: function (response) {
                    alert("Error: " + response);
                }
            });
        });
        $("#ADDMission").click(function(){
            if(!$("#TINMission").val()||!$("#TOUTMission").val())alert("املا الحقول الفارغة");
            else{
                 if(!$("#TINMission").val()>$("#TOUTMission").val()){
                     alert("وقت الدخول اكبر من وقت الخروج");
                 }else {
                    var cmd="insert into Absences (DateFrom,TimeFrom,TimeTo,AbsenceTypeId,EmployeeId) values ('"+
                    $("#DMission").html()+"','"+$("#TINMission").val()+"','"+$("#TOUTMission").val()+"','"+$("#typeMission").val()+"',(select id from Employees where FileNumber='"+$("#FNMission").html()+"'))";
                    if(excuteSQL(cmd)==1)alert("تم التسجيل بنجاح");
                    else console.log(cmd);
                 }
            }
        });
        $("#DELMission").click(function(){
                    var cmd="delete from Absences where DateFrom='"+$("#DMission").html()+"' and (AbsenceTypeId=4 or AbsenceTypeId=11) and EmployeeId=(select id from Employees where FileNumber='"+$("#FNMission").html()+"')";

                    if(excuteSQL(cmd)==1)alert("تم التسجيل بنجاح");
                    else console.log(cmd);


        });

 
 
    
$(document).ready(function(){
  $("#myInput").on("keyup", function() {
    var value = $(this).val().toLowerCase();
    $("#myTable tr").filter(function() {
      $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
  });
  $("#myTable").DataTable({
  "pagingType":"full_numbers"
  });
});
 
 
function getDayData(fn, d1,d2)//حساب بيانات يوم 
{
        var sqlcom = `select *
from
(
select FileNumber,
       cast(DateTime as date)'date',
	   cast(DateTime as time(0))time ,
	   type
 from AClogs ac
 where FileNumber=${fn}

 union

 select (select FileNumber from Employees e  where EmployeeId=e.id),cast( DateFrom as date)'DateFrom',	TimeFrom,'In'
 from Attendances a
 where EmployeeId=(select id from Employees where FileNumber=${fn}) and TimeFrom is not null and TimeFrom <> ''

 union

 select (select FileNumber from Employees e  where EmployeeId=e.id),cast( DateFrom as date)'DateFrom',	TimeTo,'Out'
 from Attendances a
 where EmployeeId=(select id from Employees where FileNumber=${fn}) and TimeTo is not null and TimeTo <> ''

 union


 select (select FileNumber from Employees e  where EmployeeId=e.id),cast( DateFrom as date)'DateFrom',	timefrom,'In'
 from Absences ab
 where EmployeeId=(select id from Employees where FileNumber=${fn}) and TimeTo is not null and TimeTo <> '' and  AbsenceTypeId in(4,11)

 UNION

 select (select FileNumber from Employees e  where EmployeeId=e.id),cast( DateFrom as date)'DateFrom',	TimeTo,'Out'
 from Absences ab
 where EmployeeId=(select id from Employees where FileNumber=${fn}) and TimeTo is not null and TimeTo <> '' and  AbsenceTypeId in(4,11)

 ) q where date >='${d1}' and date <='${d2}'  order by date, time,Type desc`;
        return readSQL(sqlcom);
}//حساب بيانات يوم 
function reSet(data) //تصفية البيانات               
{
    var array2 = []
    if (data != null && data.length > 2)
    {
        
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
        var absTime = 0;
        if (array2.length > 2) {
            for (var i = 1; i < array2.length; i += 2) {
                if (i < array2.length - 1) {
                    var d1 = new Date(array2[i].date.substr(0, 10) + ' ' + array2[i].time);
                    var d2 = new Date(array2[i + 1].date.substr(0, 10) + ' ' + array2[i + 1].time);
                    absTime += (d2.getHours() * 60 + d2.getMinutes()) - (d1.getHours() * 60 + d1.getMinutes())
                
                }
            }
        }
        return absTime;
    }//حساب الفرق  
 
 
    function showNoti(e){
        var r=e.parentNode.rowIndex
        var d=document.getElementById("container").rows[r-1].cells[1].innerHTML;
        var sql =`select 	TimeFrom	,TimeTo
 from Absences a 
 where  AbsenceTypeId=4 and 
        DateFrom = '${d}' and 
        EmployeeId=(select id from Employees where FileNumber='${$("#fileNo").val()}') `
        var data=readSQL(sql)
        console.log(data);
    }
    function hideNoti(){}
    $("#M").val(new Date().getMonth()+1);
 