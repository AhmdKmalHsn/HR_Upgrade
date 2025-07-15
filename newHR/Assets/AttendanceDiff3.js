/****************************** START حساب الخروج في منتصف اليوم ****************/
//حساب بيانات يوم 
function getDayData(fn, d1, d2, sh)
{
  var sqlcom = `select * from Ak_Cuts_V2( '${d1}','${d2}',${fn},${sh}) order by datetime,Type desc`;
  return readSQL(sqlcom);
}//حساب بيانات يوم 

//تصفية البيانات 
function reSet(data)               
{
  var array2 = []
  if (data != null && data.length > 2) {

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
//حساب الفرق
function reCalc(array2)
{
  //if (array2.length > 0)if(array2[0].type=="In")array2=array2.slice(1)
  var absTime = 0;
  if (array2.length > 2) {
    for (var i = 0; i < array2.length - 1; i++) {
      if (array2[i].type == 'Out') {
        var d1 = new Date(array2[i].date.substr(0, 10) + ' ' + array2[i].time);
        var d2 = new Date(array2[i + 1].date.substr(0, 10) + ' ' + array2[i + 1].time);
        let c = (d2.getHours() * 60 + d2.getMinutes()) - (d1.getHours() * 60 + d1.getMinutes());
        absTime += (c < 0 ? (24 * 60 + c) : c);
      }
    }
  }
  return absTime;
}//حساب الفرق  
/****************************** END حساب الخروج في منتصف اليوم ****************/