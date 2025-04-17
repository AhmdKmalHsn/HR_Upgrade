select t1.ItemCode,t2.ToWH,t2.Code,t3.code from OITM t1 left join OITT t2 on  t1.ItemCode=t2.code left join ITT1 t3 on t1.ItemCode=t3.Code
where t2.code is not null and t3.Code is null