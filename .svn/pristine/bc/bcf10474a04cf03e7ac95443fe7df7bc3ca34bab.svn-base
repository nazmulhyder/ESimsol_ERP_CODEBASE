using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class ArchiveDataController : Controller
    {
        public ActionResult ViewArchiveDatas(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ArchiveData).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<ArchiveData> _oArchiveData = new List<ArchiveData>();
            string sSQL = "SELECT * FROM View_ArchiveData  AS HH ORDER BY ArchiveDataID ASC";
            _oArchiveData = ArchiveData.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oArchiveData);

        }
        public ActionResult ViewArchiveData(int id)
        {
            ArchiveData _oArchiveData = new ArchiveData();
            if (id > 0)
            {
                _oArchiveData = _oArchiveData.Get(id, (int)Session[SessionInfo.currentUserID]);
            }

            DateTime StartDate = Convert.ToDateTime("01 Jan 2018");
            DateTime EndDate = DateTime.Today.AddYears(5);
            List<EnumObject> oYears = new List<EnumObject>();
            while (StartDate <= EndDate)
            {
                EnumObject oEnumObject = new EnumObject();
                oEnumObject.id =Convert.ToInt32(StartDate.ToString("yyyy"));
                oEnumObject.Value = StartDate.ToString("yyyy");
                oYears.Add(oEnumObject);
                StartDate=StartDate.AddYears(1);
            }
            ViewBag.Years = oYears;
            ViewBag.Months = EnumObject.jGets(typeof(EnumMonth));
            return View(_oArchiveData);

        }
        [HttpPost]
        public JsonResult Save(ArchiveData oArchiveData)
        {
            ArchiveData _oArchiveData = new ArchiveData();
            try
            {

                _oArchiveData = oArchiveData.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oArchiveData = new ArchiveData();
                _oArchiveData.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oArchiveData);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(ArchiveData oArchiveData)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oArchiveData.Delete(oArchiveData.ArchiveDataID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approved(ArchiveData oArchiveData)
        {
            ArchiveData _oArchiveData = new ArchiveData();
            try
            {

                _oArchiveData = oArchiveData.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oArchiveData = new ArchiveData();
                _oArchiveData.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oArchiveData);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Backup(ArchiveData oArchiveData)
        {
            ArchiveData _oArchiveData = new ArchiveData();
            try
            {

                _oArchiveData = oArchiveData.Backup((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oArchiveData = new ArchiveData();
                _oArchiveData.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oArchiveData);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       #region ExportToExcel
      
        public void ExportToExcelArchiveData(int nArchiveDataID,double tsv)
        {

            List<ArchiveSalaryStruc> oArchiveSalaryStruc = new List<ArchiveSalaryStruc>();
            try
            {
                string SSQL = "SELECT * FROM View_ArchiveSalaryStruc WHERE ArchiveDataID=" + nArchiveDataID + "";
                oArchiveSalaryStruc = ArchiveSalaryStruc.Gets(SSQL, (int)Session[SessionInfo.currentUserID]);
                string sMonth = "";
                if (oArchiveSalaryStruc.Count > 0)
                {
                    sMonth = oArchiveSalaryStruc[0].SalaryMonth;
                }
                int nRowIndex = 2;
                int nColIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Archive Salary Structure");
                    sheet.Name = "Archive_Salary_Structure";
                    sheet.View.FreezePanes(5, 1);
                    sheet.Column(nColIndex++).Width = 10; //SL
                    sheet.Column(nColIndex++).Width = 20; //Code
                    sheet.Column(nColIndex++).Width = 20; //Name
                    sheet.Column(nColIndex++).Width = 13; //BU
                    sheet.Column(nColIndex++).Width = 17; //Location
                    sheet.Column(nColIndex++).Width = 17; //Department
                    sheet.Column(nColIndex++).Width = 18; //Designation
                    sheet.Column(nColIndex++).Width = 13; //Date of Join
                    sheet.Column(nColIndex++).Width = 20; //Salary scheme
                    sheet.Column(nColIndex++).Width = 13; //Salary Month
                    sheet.Column(nColIndex++).Width = 15; //Actual Gross Salary
                    sheet.Column(nColIndex++).Width = 15; //Compliance Gross Salary

                    #region Report Header

                    cell = sheet.Cells[nRowIndex, 4, nRowIndex, 10]; cell.Merge = true; cell.Value = "Archive Salary Structure for " + sMonth; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex = nRowIndex + 2;
                    #endregion
                    nColIndex = 2;
                    #region Column Header

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "SL:"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Employee Code"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Employee Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Business Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Date Of Join"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Salary Scheme"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Salary Month"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Actual Gross Salary"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = "Compliance Gross Salary"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nRowIndex = nRowIndex + 1;

                    int nCount = 0;
                    foreach (ArchiveSalaryStruc oItem in oArchiveSalaryStruc)
                    {

                        nCount++;
                        nColIndex = 2;
                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.EmpCode; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.EmpName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.BUShortName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.LocName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.DeptName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.DesigName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.DateOfJoinSt; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.SchemeName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.SalaryMonth; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.GrossAmount; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[nRowIndex, nColIndex++]; cell.Value = oItem.CompGrossAmount; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        nRowIndex = nRowIndex + 1;
                    }

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Archive_salary.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Archive Salary Structure");
                    sheet.Name = "Archive_salary_Structure";

                    cell = sheet.Cells[2, 4, 2, 10]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Archive_salary.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }

        }
      #endregion

    }
}