
var i = setInterval(() => {
   fetch("/noti/InOutNoti")
     .then((x) => x.json())
     .then((y) => {
       postMessage(y);
     }); 
}, 5000);

	
 