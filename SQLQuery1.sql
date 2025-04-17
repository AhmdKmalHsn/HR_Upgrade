--select * from AClogs where filenumber=2549 and datetime>'2023-08-05'
 select * from (
select FileNumber,KnownAs,statusid ,d.Name
from Employees e join Personals p on e.PersonalId=p.id
join Departements d on d.id=e.DepartementId
where p.statusId<>1
) q1 join aclogs ac on ac.filenumber =q1.FileNumber
where datetime>'2023-08-05'
--update aclogs set FileNumber=6 where FileNumber=15212 