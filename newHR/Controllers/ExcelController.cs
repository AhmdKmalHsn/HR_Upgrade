using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using OfficeOpenXml;

namespace newHR.Controllers
{
    public class ExcelController : Controller
    {
        //   routing
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Excel()
        {
            return View();
        }
        //   Actions
        public ActionResult ImportData(HttpPostedFileBase excelFile)
        {  
            string conString = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
            string cmdText="";
            Console.WriteLine("ahmd.kmal=>" + cmdText);
            FileInfo existingFile = new FileInfo(excelFile.FileName);
            //using (MemoryStream stream = new MemoryStream(existingFile))
            using (ExcelPackage excelPackage = new ExcelPackage(excelFile.InputStream))
            {
                //loop all worksheets
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    if (worksheet.Dimension != null)
                        //loop all rows
                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            //loop all columns in a row
                            //for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                            {
                                //add the cell data to the List
                                //if (worksheet.Cells[i, j].Value != null && j <= 8)
                                {
                                    //excelData.Add(worksheet.Cells[i, j].Text.ToString());
                                    using (SqlConnection con = new SqlConnection(conString))
                                    {
                                        cmdText = //worksheet.Cells[i, 1].Value.ToString(); //worksheet.Cells[i, 5].Value.ToString();
                " if(select count(*) from Attendances where DateFrom=cast('" + worksheet.Cells[i, 5].Text.ToString() + "' as date) and EmployeeId=(select id from Employees where FileNumber='" + worksheet.Cells[i, 1].Value.ToString() + "'))=0" +
                " insert into Attendances(DateFrom, TimeFrom, TimeTo, Remarks, AttendanceTypeId, EmployeeId)" +
                " values(cast('" +
                 worksheet.Cells[i, 5].Text.ToString() + "' as date),cast( '" +
                (worksheet.Cells[i, 6].Text.ToString() == "" ? "" : worksheet.Cells[i, 6].Text.ToString()) + "' as time(0)),cast( '" +
                (worksheet.Cells[i, 7].Text.ToString() == "" ? "" : worksheet.Cells[i, 7].Text.ToString())+ "' as time(0)), '" +
                worksheet.Cells[i, 8].Value.ToString() + "', " +
                " (select id from AttendanceTypes where name = '" + worksheet.Cells[i, 4].Value.ToString() + "')," +
                " (select id from Employees where FileNumber = '" + worksheet.Cells[i, 1].Value.ToString() + "')" +
                " )";
                                        Console.WriteLine("ahmd.kmal=>"+cmdText);
                                        try{
                                            SqlCommand sqlcom = new SqlCommand(cmdText, con);
                                            con.Open();
                                            sqlcom.ExecuteNonQuery();
                                        }catch (Exception ex) { Console.WriteLine(ex.Message); }

                                    }
                                }
                            }
                        }
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        /*******************************/
        public ActionResult ImportData2(HttpPostedFileBase excelFile)
        {
            string conString = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
            string cmdText = "";
            Console.WriteLine("ahmd.kmal2=>" + cmdText);
            FileInfo existingFile = new FileInfo(excelFile.FileName);
            //using (MemoryStream stream = new MemoryStream(existingFile))
            using (ExcelPackage excelPackage = new ExcelPackage(excelFile.InputStream))
            {
                //loop all worksheets
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    if (worksheet.Dimension != null)
                        //loop all rows
                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            //loop all columns in a row
                            //for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                            {
                                //add the cell data to the List
                                //if (worksheet.Cells[i, j].Value != null && j <= 8)
                                {
                                    //excelData.Add(worksheet.Cells[i, j].Text.ToString());
                                    using (SqlConnection con = new SqlConnection(conString))
                                    {
                                        cmdText = 
"if(select count(*) from Absences where DateFrom='" + worksheet.Cells[i, 5].Text.ToString() + "' and EmployeeId=(select id from Employees where FileNumber='" +
worksheet.Cells[i, 1].Value.ToString() + "'))=0 " +
" insert into Absences(DateFrom, TimeFrom, TimeTo, Remarks, AbsenceTypeId, EmployeeId)" +
" values(cast('" +
worksheet.Cells[i, 5].Text.ToString() + "' as date), '" +
(worksheet.Cells[i, 6].Text.ToString() == "" ? "" : worksheet.Cells[i, 6].Text.ToString()) + "' ,'" +
(worksheet.Cells[i, 7].Text.ToString() == "" ? "" : worksheet.Cells[i, 7].Text.ToString()) + "' ,'" +
worksheet.Cells[i, 8].Value.ToString() + "', " +
"(select id from AbsenceTypes where name = '" + worksheet.Cells[i, 4].Value.ToString() + "')," +
"(select id from Employees where FileNumber = '" + worksheet.Cells[i, 1].Value.ToString() + "')" +
")";

                                        try
                                        {
                                            SqlCommand sqlcom = new SqlCommand(cmdText, con);
                                            con.Open();
                                            sqlcom.ExecuteNonQuery();
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                                    }
                                }
                            }
                        }
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /****************************/
    }
}