function readSQL(sql) {
  var data = "";
  $.ajax({
    url: "/SQL/Read",
    async: false,
    data: { sql: sql },
    success: function (res) {
      data = JSON.parse(res);
    },
  });
  return data;
}
function readSQL_async(sql) {
    var data = "";
    $.ajax({
        url: "/SQL/Read",
        //async: false,
        data: { sql: sql },
        success: function (res) {
            data = JSON.parse(res);
        },
    });
    return data;
}

function AK_readSQL(sql) {
  var data = "";
  $.ajax({
    url: "/crud/getbysql",
    //async: false,
    data: { sql: sql },
    success: function (res) {
      //data = JSON.parse(res);
	  
	  return res;
    },
	
  });
  
}

function readSQL_MAX(sql) {
  var data = "";
  $.ajax({
    url: "/SQL/ReadSQL",
    async: false,
    data: { sql: sql },
    success: function (res) {
      data = JSON.parse(res);
    },
  });
  return data;
}
function excuteSQL(sql) {
  var data = "";
  $.ajax({
    url: "/SQL/Excute",
    async: false,
    data: { sql: sql },
    success: function (res) {
      data = JSON.parse(res);
    },
  });
  return data;
}
function readSQL2(sql) {
  let data = new Promise((resolve, reject) => {
    $.ajax({
      url: "/SQL/Read",
      //async: false,
      data: { sql: sql },
      success: function (res) {
        data = JSON.parse(res);
        resolve(data);
      },
    });
  });
}

function excuteSQL2(sql) {
  var data = "";
  $.ajax({
    url: "/SQL/Excute",
    //async: false,
    data: { sql: sql },
    success: function (res) {
      data = JSON.parse(res);
    },
  });
  return data;
}
function setCookie(cname, cvalue, exdays) {
  const d = new Date();
  d.setTime(d.getTime() + exdays * 24 * 60 * 60 * 1000);
  let expires = "expires=" + d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
  let name = cname + "=";
  let decodedCookie = decodeURIComponent(document.cookie);
  let ca = decodedCookie.split(";");
  for (let i = 0; i < ca.length; i++) {
    let c = ca[i];
    while (c.charAt(0) == " ") {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}

function checkCookie() { }

function RolesGet(user) {
  var cook = [];
  var sql1 =
     `SELECT
  AK_Modules_lines.name
 ,AK_Roles_lines.[access]
 ,AK_Roles_lines.[read]
 ,AK_Roles_lines.[create]
 ,AK_Roles_lines.[update]
 ,AK_Roles_lines.[delete]
FROM dbo.AK_Modules_lines
INNER JOIN dbo.AK_Modules
  ON AK_Modules_lines.Module_id = AK_Modules.Id
INNER JOIN dbo.AK_Roles_lines
  ON AK_Roles_lines.module_line_id = AK_Modules_lines.id
INNER JOIN dbo.AK_Roles
  ON AK_Roles_lines.role_id = AK_Roles.Id
INNER JOIN dbo.Users2
  ON Users2.RoleId = AK_Roles.Id
WHERE Users2.Id = ${user}
`;
  var data = readSQL(sql1);
  
 
  for (var i = 0; i < data.length; i++) {
    let temp=data[i]["name"];
    delete data[i]["name"];
    cook[temp]= data[i];
  }
  return cook;
}

function EmpGet(user) {
  var cook = [];
  var sql1 =
    " select FileNumber from employees " +
    " where (" +
    "       ((select Employees from users2 where id= " +
    user +
    ")=1 and FileNumber in(select EmpId from EmpAssign where AssignId= " +
    user +
    "))or" +
    //"       ((select Employees from users2 where id= "+user+")=0)or"+
    "	   ((select Employees from users2 where id= " +
    user +
    ")=-1 and FileNumber not in(select EmpId from EmpAssign where AssignId= " +
    user +
    "))" +
    "	  )or" +
    "	  (" +
    "	  ((select Departments from users2 where id= " +
    user +
    ")=1 and DepartementId in(select DeptId from DeptAssign where AssignId= " +
    user +
    "))or" +
    //"      ((select Departments from users2 where id= "+user+")=0 )or"+
    "	  ((select Departments from users2 where id= " +
    user +
    ")=-1 and DepartementId not in(select DeptId from DeptAssign where AssignId= " +
    user +
    "))" +
    "	  )or" +
    "	  (" +
    "	  ((select [All] from users2 where id= " +
    user +
    ")=0 and DepartementId in(0))or " +
    "	  ((select [All] from users2 where id= " +
    user +
    ")=1 and DepartementId not in(0))" +
    "	  )" +
    " order by cast(FileNumber as int)";
  var data = readSQL(sql1);
  for (var i = 0; i < data.length; i++) {
    cook.push(Object.values(data[i])[0]);
  }
  return cook;
}
