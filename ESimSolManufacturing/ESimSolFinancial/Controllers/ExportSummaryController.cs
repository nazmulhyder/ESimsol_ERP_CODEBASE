using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ESimSolFinancial.Controllers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using System.Data;
using System.Dynamic;
using System.Threading;
using ESimSolFinancial.Hubs;

namespace ESimSolFinancial.Controllers
{
    public class ExportSummaryController : Controller
    {
        #region Declaration
        #endregion


        public ActionResult ViewExportSummary(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ReportNames = EnumObject.jGets(typeof(EnumExportSummaryReportName));
            ViewBag.ReportTypes = EnumObject.jGets(typeof(EnumExportSummaryReportType));
            ViewBag.ReportLayouts = EnumObject.jGets(typeof(EnumExportSummaryReportLayout));
            
            ExportSummary oExportSummary = new ExportSummary();            
            DateTime dFirstDayofCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime dLastDayofLastMonth = dFirstDayofCurrentMonth.AddDays(-1);
            DateTime dFirstDayofOneYearAgo = dFirstDayofCurrentMonth.AddYears(-1);
            oExportSummary.StartDate = dFirstDayofOneYearAgo;
            oExportSummary.EndDate = dLastDayofLastMonth;
            oExportSummary.BUID = buid;
            return View(oExportSummary);

        }
        
        [HttpPost]
        public JsonResult GetsExportSummary(ExportSummary oExportSummary)
        {
            ExportSummary oTempExportSummary = new ExportSummary();          
            ExportSummary oReturnExportSummary = new ExportSummary();          
            try
            {
                Thread.Sleep(100);
                ProgressHub.SendMessage("Process : ", 5, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(500);

                ProgressHub.SendMessage("Data Collect from Database", 5, (int)Session[SessionInfo.currentUserID]);
                oTempExportSummary = ExportSummary.GetsExportSummary(oExportSummary, (int)Session[SessionInfo.currentUserID]);
                ProgressHub.SendMessage("Data Mapping", 40, (int)Session[SessionInfo.currentUserID]);
                if (oTempExportSummary.DataCarrier != null)
                {
                    oReturnExportSummary = this.GetsReportData(oTempExportSummary.DataCarrier);
                    oReturnExportSummary.BaseColumnCaption = this.GetColumnCaption(oExportSummary, true);
                    oReturnExportSummary.TotalColumnCaption = this.GetColumnCaption(oExportSummary, false);
                }
                ProgressHub.SendMessage("Data Display", 80, (int)Session[SessionInfo.currentUserID]);                
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                oReturnExportSummary = new ExportSummary();
                oReturnExportSummary.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(oReturnExportSummary, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
        public ExportSummary GetsReportData(DataSet oDataSet)
        {
            double nReportValue = 0.00;                     
            List<ReportRow> oReportRows = new List<ReportRow>();
            List<ReportMonth> oReportMonths = new List<ReportMonth>();
            List<ReportData> oReportDatas = new List<ReportData>();
            ExportSummary oReturnExportSummary = new ExportSummary();
            List<ExpandoObject> oExportSummarys = new List<ExpandoObject>();
                       
            DataTable oReportRowDataTable = oDataSet.Tables[0];
            DataTable oReportMonthDataTable = oDataSet.Tables[1];
            DataTable oReportDataTable = oDataSet.Tables[2];

            oReportRows = ReportRow.GetObjects(oReportRowDataTable.Rows);
            oReportMonths = ReportMonth.GetObjects(oReportMonthDataTable.Rows);
            oReportDatas = ReportData.GetObjects(oReportDataTable.Rows);


            foreach (ReportRow oReportRow in oReportRows)
            {
                dynamic oExportSummary = new ExpandoObject();
                oExportSummary.BaseColumn = oReportRow.RowObjName;
                oExportSummary.TotalValue = oReportRow.TotalValue;
                oExportSummary.TotalValueSt = oReportRow.TotalValue.ToString("#,##,##0.00;(#,##,##0.00)");
                foreach (ReportMonth oReportMonth in oReportMonths)
                {
                    nReportValue = this.GetValue(oReportRow.RowObjTypeID, oReportMonth.MonthID, oReportMonth.YearID, oReportDatas);
                    AddProperty(oExportSummary, "Month" + oReportMonth.MonthPKID.ToString(), nReportValue);
                    AddProperty(oExportSummary, "Month" + oReportMonth.MonthPKID.ToString() + "St", nReportValue.ToString("#,##,##0.00;(#,##,##0.00)"));
                }
                oExportSummarys.Add(oExportSummary);
            }

            #region FooterRow
            nReportValue = 0.00;
            dynamic oFooterRow = new ExpandoObject();
            oFooterRow.BaseColumn = "Grand Total";            
            foreach (ReportMonth oReportMonth in oReportMonths)
            {
                nReportValue = nReportValue + oReportMonth.TotalValue;
                AddProperty(oFooterRow, "Month" + oReportMonth.MonthPKID.ToString(), oReportMonth.TotalValue);
                AddProperty(oFooterRow, "Month" + oReportMonth.MonthPKID.ToString() + "St", oReportMonth.TotalValue.ToString("#,##,##0.00;(#,##,##0.00)"));
            }
            oFooterRow.TotalValue = nReportValue;
            oFooterRow.TotalValueSt = nReportValue.ToString("#,##,##0.00;(#,##,##0.00)");
            #endregion

            oReturnExportSummary.FooterRow = oFooterRow;
            oReturnExportSummary.ExportSummarys = oExportSummarys;
            oReturnExportSummary.ReportMonths = oReportMonths;
            return oReturnExportSummary;
        }

        public double GetValue(int nRowObjTypeID, int nMonthID, int nYearID, List<ReportData> oReportDatas)
        {
            double nValue = 0.00;
            foreach (ReportData oReportData in oReportDatas)
            {
                if (oReportData.RowObjTypeID == nRowObjTypeID && oReportData.MonthID == nMonthID && oReportData.YearID == nYearID)
                {
                    return oReportData.ReportValue;
                }
            }
            return nValue;
        }

        public static void AddProperty(ExpandoObject oExpandoObject, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = oExpandoObject as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public string GetColumnCaption(ExportSummary oExportSummary, bool bIsBaseColumn)
        {
            if ((EnumExportSummaryReportLayout)oExportSummary.ReportlayoutInt == EnumExportSummaryReportLayout.MKTPersonWise)
            {
                if (bIsBaseColumn)
                {
                    return "MKT Person";
                }
                else
                {
                    return "Total Amount";
                }
            }
            else if ((EnumExportSummaryReportLayout)oExportSummary.ReportlayoutInt == EnumExportSummaryReportLayout.BuyerWise)
            {
                if (bIsBaseColumn)
                {
                    return "Buyer Name";
                }
                else
                {
                    return "Total Amount";
                }
            }
            else if ((EnumExportSummaryReportLayout)oExportSummary.ReportlayoutInt == EnumExportSummaryReportLayout.BankWise)
            {
                if (bIsBaseColumn)
                {
                    return "Bank Name";
                }
                else
                {
                    return "Total Amount";
                }
            }
            return "";
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ExportSummary oExportSummary)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oExportSummary);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ExportSummaryExportExcel(double ts)
        {
            Company oCompany = new Company();
            ExportSummary oTempExportSummary = new ExportSummary();
            ExportSummary oExportSummary = new ExportSummary();
            ExportSummary oReturnExportSummary = new ExportSummary();
            try
            {
                oExportSummary = (ExportSummary)Session[SessionInfo.ParamObj];
                if (oExportSummary == null)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oTempExportSummary = ExportSummary.GetsExportSummary(oExportSummary, (int)Session[SessionInfo.currentUserID]);
                if (oTempExportSummary.DataCarrier != null && oTempExportSummary.ErrorMessage == "")
                {
                    List<ReportRow> oReportRows = new List<ReportRow>();
                    List<ReportMonth> oReportMonths = new List<ReportMonth>();
                    List<ReportData> oReportDatas = new List<ReportData>();                  
                    List<ExpandoObject> oExportSummarys = new List<ExpandoObject>();

                    DataTable oReportRowDataTable = oTempExportSummary.DataCarrier.Tables[0];
                    DataTable oReportMonthDataTable = oTempExportSummary.DataCarrier.Tables[1];
                    DataTable oReportDataTable = oTempExportSummary.DataCarrier.Tables[2];

                    oReportRows = ReportRow.GetObjects(oReportRowDataTable.Rows);
                    oReportMonths = ReportMonth.GetObjects(oReportMonthDataTable.Rows);
                    oReportDatas = ReportData.GetObjects(oReportDataTable.Rows);

                    oReturnExportSummary = new ExportSummary();
                    oReturnExportSummary.StartDate = oExportSummary.StartDate;
                    oReturnExportSummary.EndDate = oExportSummary.EndDate;
                    oReturnExportSummary.ReportName = (EnumExportSummaryReportName)oExportSummary.ReportNameInt;
                    oReturnExportSummary.ReportType = (EnumExportSummaryReportType)oExportSummary.ReportTypeInt;
                    oReturnExportSummary.Reportlayout = (EnumExportSummaryReportLayout)oExportSummary.ReportlayoutInt;
                    oReturnExportSummary.ReportMonths = oReportMonths;
                    oReturnExportSummary.ReportRows = oReportRows;
                    oReturnExportSummary.ReportDatas = oReportDatas;
                    oReturnExportSummary.BaseColumnCaption = this.GetColumnCaption(oExportSummary, true);
                    oReturnExportSummary.TotalColumnCaption = this.GetColumnCaption(oExportSummary, false);
                    ExportToExcel(oReturnExportSummary, oCompany);
                }
                else               
                {
                    throw new Exception(oTempExportSummary.ErrorMessage);
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("Export Summary");
                    sheet.Name = "Export Summary";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ExportSummary.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        private void ExportToExcel(ExportSummary oExportSummary, Company oCompany)
        {          
            #region Report Body
            int rowIndex = 2;
            int nMaxColumn = 0;
            int colIndex = 2;
            ExcelRange cell;
            Border border;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export Summery");
                sheet.Name = "Export Summery";

                #region Declare Column
                colIndex = 1;
                sheet.Column(++colIndex).Width = 8;  //SL
                sheet.Column(++colIndex).Width = 28; //Base Column
                foreach (ReportMonth oMonth in oExportSummary.ReportMonths)
                {
                    if (oExportSummary.ReportMonths.Count > 4)
                    {
                        sheet.Column(++colIndex).Width = 13; //Month Wise Value
                    }
                    else
                    {
                        sheet.Column(++colIndex).Width = 20; //Month Wise Value
                    }
                }
                sheet.Column(++colIndex).Width = 20; //Total
                nMaxColumn = colIndex;
                #endregion

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                string sCompareStatement = "";
                if (oExportSummary.ReportType == EnumExportSummaryReportType.CompareStatement)
                {
                    sCompareStatement = "(Compare Statement)";
                }
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 15;
                cell.Value = EnumObject.jGet(oExportSummary.Reportlayout) + " " + EnumObject.jGet(oExportSummary.ReportName) + " " + sCompareStatement; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                string sToOrAnd = "to";
                if (oExportSummary.ReportType == EnumExportSummaryReportType.CompareStatement)
                {
                    sToOrAnd = "&";
                }
                string sMonthRange = oExportSummary.StartDate.ToString("MMM yyyy") + " --" + sToOrAnd + "--" + oExportSummary.EndDate.ToString("MMM yyyy");                
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 1]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 12;
                cell.Value = sMonthRange; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, nMaxColumn]; cell.Style.Font.Bold = false;
                cell.Value = "Print : " + DateTime.Now.ToString("dd MMM yyyy"); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rowIndex++;
                #endregion

                #region Column Header
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "SL"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                colIndex = colIndex + 1;

                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oExportSummary.BaseColumnCaption; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                colIndex = colIndex + 1;

                foreach (ReportMonth oMonth in oExportSummary.ReportMonths)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oMonth.ReportColHeaderTex; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;
                }
                //Total
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oExportSummary.TotalColumnCaption; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                rowIndex++;
                #endregion

                #region Report Body
                int nCount = 0;
                string sStartCell = "", sEndCell = "";
                int nStartRow = 0, nEndRow = 0, nStartCol = 0;
                string sDataColumn = "", sNumberFormat = ""; double nReportValue = 0;
                nStartRow = rowIndex;
                foreach (ReportRow oItem in oExportSummary.ReportRows)
                {
                    //SL 
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ++nCount; cell.Style.Numberformat.Format = "##,###;(##,###)"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    //Base Column Name
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.RowObjName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    nStartCol = colIndex;
                    foreach (ReportMonth oMonth in oExportSummary.ReportMonths)
                    {
                        sNumberFormat = "#,##,###;(#,##,###)";
                        nReportValue = this.GetValue(oItem.RowObjTypeID, oMonth.MonthID, oMonth.YearID, oExportSummary.ReportDatas);
                        if (nReportValue > 0)
                        {
                            sNumberFormat = "#,##,##0.00;(#,##,##0.00)";
                        }
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nReportValue; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = sNumberFormat;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;
                    }

                    #region Row Wise Total
                    sStartCell = Global.GetExcelCellName(rowIndex, nStartCol);
                    sEndCell = Global.GetExcelCellName(rowIndex, (colIndex - 1));
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    #endregion

                    nEndRow = rowIndex;
                    rowIndex++;
                }
                #endregion

                #region Grand Total
                cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Value = "Grand Total"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                colIndex = 4;
                nStartCol = 4;
                foreach (ReportMonth oMonth in oExportSummary.ReportMonths)
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;
                }

                #region Row Wise Total
                sStartCell = Global.GetExcelCellName(rowIndex, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex, (colIndex - 1));
                cell = sheet.Cells[rowIndex, colIndex]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                #endregion
                rowIndex++;
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportSummary.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
    }
}