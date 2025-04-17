var oldValue=[];
var newValue=[];
var changes=[];
var keys=[];
var vals=[];
console.log("changlog has been loaded");
function setKeys(){
	//can set directly
}
function setVals(){
	
}
function setOldValue(key,val)//في حالة التعديل
{
	oldValue=[]
	if(key.length==val.length)
	for(var i=0;i<arr;i++){
		oldValue.push(key[i]);
		oldValue.push($("#"+val[i]).val());
	}else console.log("error keys not equal vals");
	console.log("old="+oldValue);
}
