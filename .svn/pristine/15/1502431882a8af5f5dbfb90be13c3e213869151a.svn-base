using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ESimSolFinancial.Hubs;
using System.Threading;

namespace ESimSolFinancial.Controllers
{
    public class ArchiveSalaryStrucController : Controller
    {
        #region Declaration

        ArchiveSalaryStruc _oArchiveSalaryStruc = new ArchiveSalaryStruc();
        List<ArchiveSalaryStruc> _oArchiveSalaryStrucs = new List<ArchiveSalaryStruc>();
        #endregion

        #region Actions

        public ActionResult ViewArchiveSalaryStrucs(int nEmployeeID)
        {
            Employee oEmployee = new Employee();
            _oArchiveSalaryStrucs = new List<ArchiveSalaryStruc>();
            string sSQL = "SELECT * FROM View_ArchiveSalaryStruc AS HH WHERE HH.EmployeeID = " + nEmployeeID.ToString() + " ORDER BY HH.SalaryYearID, HH.SalaryMonthID ASC";            
            _oArchiveSalaryStrucs = ArchiveSalaryStruc.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oEmployee = oEmployee.Get(nEmployeeID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Employee = oEmployee;
            return View(_oArchiveSalaryStrucs);
        }

        public ActionResult ViewChangeSalaryStructure(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ArchiveData _oArchiveData = new ArchiveData();
            List<ArchiveData> _oArchiveDatas = new List<ArchiveData>();
            List<EmployeeBatch> _oEmployeeBatchs = new List<EmployeeBatch>();
            string sSQL = "SELECT * FROM View_ArchiveData AS HH WHERE ISNULL(HH.ApprovedBy,0) != 0 ORDER BY HH.ArchiveYearID, HH.ArchiveMonthID ASC";
            _oArchiveDatas = ArchiveData.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            sSQL = "SELECT * FROM View_EmployeeBatch AS HH WHERE ISNULL(HH.ApprovedBy,0) != 0 ORDER BY HH.EmployeeBatchID ASC";
            _oEmployeeBatchs = EmployeeBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ArchiveData = _oArchiveDatas;
            ViewBag.EmployeeBatch = _oEmployeeBatchs;
            return View(_oArchiveData);

        }
        #endregion
        [HttpGet]
        public JsonResult GetArchiveSalaryStruc(int id)
        {
            ArchiveSalaryStruc _oArchiveSalaryStruc = new ArchiveSalaryStruc();
            try
            {
                _oArchiveSalaryStruc = _oArchiveSalaryStruc.Get(id, (int)Session[SessionInfo.currentUserID]);
                List<ArchiveSalaryStrucDtl> oArchiveSalaryStrucDtls = new List<ArchiveSalaryStrucDtl>();
                oArchiveSalaryStrucDtls = ArchiveSalaryStrucDtl.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oArchiveSalaryStruc.BasicArchiveSalaryStrucDtls = oArchiveSalaryStrucDtls.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic).ToList();
                _oArchiveSalaryStruc.AllowanceArchiveSalaryStrucDtls = oArchiveSalaryStrucDtls.Where(x => x.SalaryHeadType != EnumSalaryHeadType.Basic).ToList();
            }
            catch (Exception ex)
            {
                _oArchiveSalaryStruc = new ArchiveSalaryStruc();
                _oArchiveSalaryStruc.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oArchiveSalaryStruc);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommitChangeSalaryStructure(ArchiveSalaryStruc oArchiveSalaryStruc)
        {            
            ArchiveData oArchiveData = new ArchiveData();
            string sFeedBackMessage = "";             
            List<EmployeeBatchDetail> oTempEmployeeBatchDetails = new List<EmployeeBatchDetail>();
            ArchiveSalaryStruc oTempArchiveSalaryStruc = new ArchiveSalaryStruc();
            List<ArchiveSalaryStruc> oArchiveSalaryStrucs = new List<ArchiveSalaryStruc>();
            List<ArchiveSalaryStruc> oTempArchiveSalaryStrucs = new List<ArchiveSalaryStruc>();
            try
            {
                oTempEmployeeBatchDetails = EmployeeBatchDetail.Gets(oArchiveSalaryStruc.EmployeeBatchID, (int)Session[SessionInfo.currentUserID]);
                var oEmployeeBatchDetails = oTempEmployeeBatchDetails.GroupBy(x => new { x.Location, x.Department, x.Designation }, (key, grp) => new
                {
                    Location = key.Location,
                    Department = key.Department,
                    Designation = key.Designation,
                    Results = grp.ToList()
                });



                double nPercentCount = 5;
                Thread.Sleep(100);
                ProgressHub.SendMessage("Process : ", nPercentCount, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(500);
                int nTotalGroupCount = oEmployeeBatchDetails.Count();
                double nPerUnitPercentCount = (95.00 / nTotalGroupCount);
                
                foreach (var oEmployeeBatchGroup in oEmployeeBatchDetails)
                {
                    oTempArchiveSalaryStrucs = new List<ArchiveSalaryStruc>();

                    nPercentCount = nPercentCount + nPerUnitPercentCount;
                    ProgressHub.SendMessage("Location : " + oEmployeeBatchGroup.Location + ", Dept : " + oEmployeeBatchGroup.Department + ", Designation : " + oEmployeeBatchGroup.Designation + "(" + oEmployeeBatchGroup.Results.Count.ToString() + ")", nPercentCount, (int)Session[SessionInfo.currentUserID]);                    
                    oTempArchiveSalaryStrucs = ArchiveSalaryStruc.ArchiveSalaryChnage(oArchiveSalaryStruc.ArchiveDataID, oEmployeeBatchGroup.Results, (int)Session[SessionInfo.currentUserID]);
                    if (oTempArchiveSalaryStrucs != null && oTempArchiveSalaryStrucs.Count>0)
                    {
                        foreach (ArchiveSalaryStruc oItem in oArchiveSalaryStrucs)
                        {
                            oTempArchiveSalaryStruc = new ArchiveSalaryStruc();
                            oTempArchiveSalaryStruc.EmpCode = oItem.EmpCode;
                            oTempArchiveSalaryStruc.EmpName = oItem.EmpName;
                            oTempArchiveSalaryStruc.ErrorMessage = sFeedBackMessage;
                            oArchiveSalaryStrucs.Add(oTempArchiveSalaryStruc);
                        }
                    }
                }
                oArchiveData.ArchiveSalaryStrucs = oArchiveSalaryStrucs;

                this.Session.Remove(SessionInfo.ErrorData);
                this.Session.Add(SessionInfo.ErrorData, oArchiveData);

                ProgressHub.SendMessage("Finishing Salary Changes", 100, (int)Session[SessionInfo.currentUserID]);
                ProgressHub.SendMessage("Finishing Salary Changes", 100, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                oArchiveData = new ArchiveData();
                oArchiveData.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oArchiveData);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommitChangeSalaryStructureTest(ArchiveSalaryStruc oArchiveSalaryStruc)
        {
            ArchiveData oArchiveData = new ArchiveData();            
            List<EmployeeBatchDetail> oEmployeeBatchDetails = new List<EmployeeBatchDetail>();            
            try
            {

                oEmployeeBatchDetails = EmployeeBatchDetail.ArchiveSalaryChnage(oArchiveSalaryStruc, (int)Session[SessionInfo.currentUserID]);
                oArchiveData.EmployeeBatchDetails = oEmployeeBatchDetails;

                this.Session.Remove(SessionInfo.ErrorData);
                this.Session.Add(SessionInfo.ErrorData, oArchiveData);
            }
            catch (Exception ex)
            {
                oArchiveData = new ArchiveData();
                oArchiveData.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oArchiveData);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void DownloadErrorListTest(double ts)
        {
            string sErrorMessage = "";
            ArchiveData oArchiveData = new ArchiveData();
            try
            {
                oArchiveData = (ArchiveData)Session[SessionInfo.ErrorData];
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
            }
                
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 2;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Error List");
                sheet.Name = "Error List";

                int n = 1;
                sheet.Column(n++).Width = 15;
                sheet.Column(n++).Width = 20;
                sheet.Column(n++).Width = 100;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Error Message"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;                
                rowIndex += 1;

                if (sErrorMessage != "")
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = sErrorMessage; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                }
                else
                {
                    foreach (EmployeeBatchDetail oItem in oArchiveData.EmployeeBatchDetails)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Invalid Salary Scheme/Gross Amount/Comp Gross Amount!"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                }

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ErrorList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        
        }

        public void DownloadErrorList(double ts)
        {
            string sErrorMessage = "";
            ArchiveData oArchiveData = new ArchiveData();
            try
            {
                oArchiveData = (ArchiveData)Session[SessionInfo.ErrorData];
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
            }

            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 2;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Error List");
                sheet.Name = "Error List";

                int n = 1;
                sheet.Column(n++).Width = 15;
                sheet.Column(n++).Width = 20;
                sheet.Column(n++).Width = 100;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Error Message"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex += 1;

                if (sErrorMessage != "")
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = sErrorMessage; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                }
                else
                {
                    foreach (ArchiveSalaryStruc oItem in oArchiveData.ArchiveSalaryStrucs)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmpCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmpName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ErrorMessage; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                }

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ErrorList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }
    }
}
