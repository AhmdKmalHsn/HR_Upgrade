function readSQL(sql)
        {
            var data = '';
            $.ajax({
                url: '/it/SQL/Read',
                async: false,
                data: { sql: sql },
                success: function (res)
                    {
                        data=JSON.parse(res);
                    }
               });
            return data;
        }
function excuteSQL(sql)
        {
            var data = '';
            $.ajax({
                url: '/it/SQL/Excute',
                async: false,
                data: { sql: sql },
                success: function (res)
                    {
                        data=JSON.parse(res);
                    }
               });
            return data;
        }

function setCookie(cname,cvalue,exdays) {
  const d = new Date();
  d.setTime(d.getTime() + (exdays*24*60*60*1000));
  let expires = "expires=" + d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
  let name = cname + "=";
  let decodedCookie = decodeURIComponent(document.cookie);
  let ca = decodedCookie.split(';');
  for(let i = 0; i < ca.length; i++) {
    let c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}

function checkCookie() { 
   /*let  user = ["10","12"];
   setCookie("username", JSON.stringify(user), 1);*/
  }

function EmpGet(user){
		var cook=[];
		var sql1=
		" select FileNumber from employees "+
		" where ("+
		"       ((select Employees from users2 where id= "+user+")=1 and FileNumber in(select EmpId from EmpAssign where AssignId= "+user+"))or"+
		//"       ((select Employees from users2 where id= "+user+")=0)or"+
		"	   ((select Employees from users2 where id= "+user+")=-1 and FileNumber not in(select EmpId from EmpAssign where AssignId= "+user+"))"+
		"	  )or"+
		"	  ("+
		"	  ((select Departments from users2 where id= "+user+")=1 and DepartementId in(select DeptId from DeptAssign where AssignId= "+user+"))or"+
		//"      ((select Departments from users2 where id= "+user+")=0 )or"+
		"	  ((select Departments from users2 where id= "+user+")=-1 and DepartementId not in(select DeptId from DeptAssign where AssignId= "+user+"))"+
		"	  )or"+
		"	  ("+
		"	  ((select [All] from users2 where id= "+user+")=0 and DepartementId in(0))or "+
		"	  ((select [All] from users2 where id= "+user+")=1 and DepartementId not in(0))"+
		"	  )"+
		" order by cast(FileNumber as int)";
		var data =readSQL(sql1)
		for(var i=0;i<data.length;i++)
			{
				cook.push(Object.values(data[i])[0]);
			}
			return cook;
	}		

