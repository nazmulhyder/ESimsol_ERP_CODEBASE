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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DUHardWindingReportController : Controller
    {    
        #region Declaration
        DUHardWindingReport _oDUHardWindingReport = new DUHardWindingReport();
        List<DUHardWindingReport> _oDUHardWindingReports = new List<DUHardWindingReport>();
        #endregion

        #region Functions
        #endregion

        #region Actions
        public ActionResult ViewDUHardWindingReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUHardWindingReports = new List<DUHardWindingReport>();
            //_oDUHardWindingReports = DUHardWindingReport.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == (int)EnumCompareOperator.EqualTo || x.id == (int)EnumCompareOperator.Between);
            return View(_oDUHardWindingReports);
        }
        #endregion

        #region Get
        [HttpPost]
        public JsonResult GetsData(DUHardWindingReport oDUHardWindingReport)//Id=ContractorID
        {
            try
            {
                _oDUHardWindingReports = new List<DUHardWindingReport>();
                _oDUHardWindingReports = DUHardWindingReport.Gets(oDUHardWindingReport, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUHardWindingReports = new List<DUHardWindingReport>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUHardWindingReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region print
        [HttpPost]
        public ActionResult SetDUHardWindingReport(DUHardWindingReport oDUHardWindingReport)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDUHardWindingReport);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintDUHardWindingReports()
        {
            DUHardWindingReport oDUHardWindingReport = new DUHardWindingReport();
            //int nBUID = 0;
            try
            {
                oDUHardWindingReport = (DUHardWindingReport)Session[SessionInfo.ParamObj];
                //nBUID = oDUHardWindingReport.BUID;
                _oDUHardWindingReports = new List<DUHardWindingReport>();
                _oDUHardWindingReports = DUHardWindingReport.Gets(oDUHardWindingReport, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUHardWindingReport = new DUHardWindingReport();
                _oDUHardWindingReports = new List<DUHardWindingReport>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            
            if (_oDUHardWindingReports.Count > 0)
            {
                rptDUHardWindingReport oReport = new rptDUHardWindingReport();
                byte[] abytes = oReport.PrepareReport(_oDUHardWindingReports, oCompany);
                return File(abytes, "application/pdf");
            }
            else 
            {
				rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }
        public void PrintDUHardWindingExcel()
        {
            DUHardWindingReport oDUHardWindingReport = new DUHardWindingReport();
            try
            {
                 oDUHardWindingReport = (DUHardWindingReport)Session[SessionInfo.ParamObj];
                _oDUHardWindingReports = new List<DUHardWindingReport>();
                _oDUHardWindingReports = DUHardWindingReport.Gets(oDUHardWindingReport, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUHardWindingReport = new DUHardWindingReport();
                _oDUHardWindingReports = new List<DUHardWindingReport>();
            }
            
            #region EXCEL Start
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nTotalCol = 0;
            int nCount = 0;
            int colIndex = 2;
            int nSL = 0;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Hard Winding Report");
                sheet.Name = "Hard Winding Report";
                sheet.Column(colIndex++).Width = 10; //SL
                sheet.Column(colIndex++).Width = 20; //date
                sheet.Column(colIndex++).Width = 15; //hw/in
                sheet.Column(colIndex++).Width = 15; //hw/out
                sheet.Column(colIndex++).Width = 15; //reprocess/in
                sheet.Column(colIndex++).Width = 15; //reprocess/out
                sheet.Column(colIndex++).Width = 15; //greige prod.
                sheet.Column(colIndex++).Width = 15; //total
                sheet.Column(colIndex++).Width = 15; //L/O
                sheet.Column(colIndex++).Width = 15; //G.Total
                sheet.Column(colIndex++).Width = 15; //Bream Comm
                sheet.Column(colIndex++).Width = 15; //beam TF
                sheet.Column(colIndex++).Width = 15; //WARPING
                sheet.Column(colIndex++).Width = 15; //BEAM Stock

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Hard Winding Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Range: " + oDUHardWindingReport.StartDateInString + " To " + oDUHardWindingReport.EndDateInString; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex += 2;
                #endregion
                #region Header 1
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex,colIndex++]; cell.Merge = true; cell.Value = "Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex+=1]; cell.Merge = true; cell.Value = "HW(In) + HW(Out)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex++;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Reprocess(In)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Reprocess(Out)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Greige Prod."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "L/O"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "G. Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Bream Comm."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Bream T/F"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Warping"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Beam Stock"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion
                #region Report Body

                nSL = 1;
                foreach (DUHardWindingReport oItem in _oDUHardWindingReports)
                {
                    colIndex = 2; 
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StartDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyHWIn; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyHWOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyReHWIn; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyReHWOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyGreige; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty_LO; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrandTotalQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BeamCom; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BeamTF; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Warping; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BeamStock; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rowIndex++;
                    nSL++;

                }
                #region TOTAL
                cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Merge = true; cell.Value = "Grand Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = _oDUHardWindingReports.Sum(x => x.QtyHWIn); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = _oDUHardWindingReports.Sum(x => x.QtyHWOut); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 6]; cell.Value = _oDUHardWindingReports.Sum(x => x.QtyReHWIn); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 7]; cell.Value = _oDUHardWindingReports.Sum(x => x.QtyReHWOut); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = _oDUHardWindingReports.Sum(x => x.QtyGreige); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = _oDUHardWindingReports.Sum(x => x.TotalQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = _oDUHardWindingReports.Sum(x => x.Qty_LO); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 11]; cell.Value = _oDUHardWindingReports.Sum(x => x.GrandTotalQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 12]; cell.Value = _oDUHardWindingReports.Sum(x => x.BeamCom); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 13]; cell.Value = _oDUHardWindingReports.Sum(x => x.BeamTF); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 14]; cell.Value = _oDUHardWindingReports.Sum(x => x.Warping); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = _oDUHardWindingReports.Sum(x => x.BeamStock); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rowIndex++;
                #endregion
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=RPT_HardWindingReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
                #endregion


            }



            #endregion
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region print Detail
        public ActionResult PrintDUHardWindingDetail()
        {
            string sSql = "";
            DUHardWindingReport oDUHardWindingReport = new DUHardWindingReport();
            List<DUHardWinding> oDUHardWindings = new List<DUHardWinding>();
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            List<FabricBeamFinish> oFabricBeamFinishs = new List<FabricBeamFinish>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            try
            {
           
                oDUHardWindingReport = (DUHardWindingReport)Session[SessionInfo.ParamObj];

                oBusinessUnit = oBusinessUnit.Get(oDUHardWindingReport.BUID, (int)Session[SessionInfo.currentUserID]);

                if (oDUHardWindingReport.ReportType == 1)/// Dyed Yarn Received
                {
                    sSql = "Select * from view_DUHardWinding where isnull(RouteSheetID,0)>0 and isnull(IsRewinded,0)=0 and ReceiveDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and ReceiveDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                }
                else if (oDUHardWindingReport.ReportType == 2)/// Re Hard Winding
                {
                    sSql = "Select * from view_DUHardWinding where isnull(RouteSheetID,0)>0 and isnull(IsRewinded,0)=1 and ReceiveDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and ReceiveDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                }
                else if (oDUHardWindingReport.ReportType == 3)/// ///Greige Yarn Receive
                {
                    sSql = "Select * from view_DUHardWinding where isnull(RouteSheetID,0)<=0  and ReceiveDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and ReceiveDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                }
                if (!string.IsNullOrEmpty(sSql))
                {
                    oDUHardWindings = DUHardWinding.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                }
                if (oDUHardWindingReport.ReportType == 6)/// Beam TF
                {
                    sSql = "SELECT * FROM View_FabricExecutionOrderYarnReceive where  WarpWeftType=" + (int)EnumWarpWeft.Warp + " and WYRequisitionID in (select WYRequisitionID from  WYRequisition where WYarnType=" + (int)EnumWYarnType.DyedYarn + " and IssueDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and IssueDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy  hh:mm tt") + "') ORDER BY FSCDID";
                }
                if (oDUHardWindingReport.ReportType == 8)///Left Over
                {
                    sSql = "SELECT * FROM View_FabricExecutionOrderYarnReceive where  isnull(ReceiveBy,0)<>0 and WYRequisitionID in (select WYRequisitionID from  WYRequisition where RequisitionType=" + (int)EnumInOutType.Disburse + "  and WYarnType=" + (int)EnumWYarnType.LeftOver + " and IssueDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and IssueDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy  hh:mm tt") + "') ORDER BY FSCDID";
                }

                if (!string.IsNullOrEmpty(sSql))
                {
                    oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                }
                if (oDUHardWindingReport.ReportType == 4)/// Beam Comp
                {
                    sSql = "SELECT * FROM View_FabricBeamFinish WHERE ReadyDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and ReadyDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                    if (!string.IsNullOrEmpty(sSql))
                    {
                        oFabricBeamFinishs = FabricBeamFinish.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                    }
                }
                if (oDUHardWindingReport.ReportType == 7)/// Beam Stock
                {
                    sSql = "SELECT * FROM View_FabricBeamFinish WHERE ISNULL(IsFinish,0) = 0 ";
                    if (!string.IsNullOrEmpty(sSql))
                    {
                        oFabricBeamFinishs = FabricBeamFinish.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                    }
                }
            
            }
            catch (Exception ex)
            {
                _oDUHardWindingReport = new DUHardWindingReport();
                _oDUHardWindingReports = new List<DUHardWindingReport>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (oDUHardWindings.Count > 0)
            {
                rptDUHardWindingReport oReport = new rptDUHardWindingReport();
                byte[] abytes = oReport.PrepareReport_Detail(oDUHardWindingReport, oDUHardWindings, oFabricExecutionOrderYarnReceives, oCompany, oBusinessUnit, oFabricBeamFinishs);
                return File(abytes, "application/pdf");
            }
            else if (oFabricExecutionOrderYarnReceives.Count > 0)
            {
                rptDUHardWindingReport oReport = new rptDUHardWindingReport();
                byte[] abytes = oReport.PrepareReport_Detail(oDUHardWindingReport, oDUHardWindings, oFabricExecutionOrderYarnReceives, oCompany, oBusinessUnit, oFabricBeamFinishs);
                return File(abytes, "application/pdf");
            }
            else if (oFabricBeamFinishs.Count > 0)
            {
                rptDUHardWindingReport oReport = new rptDUHardWindingReport();
                byte[] abytes = oReport.PrepareReport_Detail(oDUHardWindingReport, oDUHardWindings, oFabricExecutionOrderYarnReceives, oCompany, oBusinessUnit, oFabricBeamFinishs);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }
        public void PrintDUHardWindingDetailExcel()
        {
            string sSql = "";
            DUHardWindingReport oDUHardWindingReport = new DUHardWindingReport();
            List<DUHardWinding> oDUHardWindings = new List<DUHardWinding>();           
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            List<FabricBeamFinish> oFabricBeamFinishs = new List<FabricBeamFinish>();
            List<FabricBeamFinish> oTempFabricBeamFinishs = new List<FabricBeamFinish>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            try
            {

                oDUHardWindingReport = (DUHardWindingReport)Session[SessionInfo.ParamObj];

                oBusinessUnit = oBusinessUnit.Get(oDUHardWindingReport.BUID, (int)Session[SessionInfo.currentUserID]);

                if (oDUHardWindingReport.ReportType == 1)/// Dyed Yarn Received
                {
                    sSql = "Select * from view_DUHardWinding where isnull(RouteSheetID,0)>0 and isnull(IsRewinded,0)=0 and ReceiveDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and ReceiveDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                }
                else if (oDUHardWindingReport.ReportType == 2)/// Re Hard Winding
                {
                    sSql = "Select * from view_DUHardWinding where isnull(RouteSheetID,0)>0 and isnull(IsRewinded,0)=1 and ReceiveDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and ReceiveDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                }
                else if (oDUHardWindingReport.ReportType == 3)/// ///Greige Yarn Receive
                {
                    sSql = "Select * from view_DUHardWinding where isnull(RouteSheetID,0)<=0  and ReceiveDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and ReceiveDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                }
                if (!string.IsNullOrEmpty(sSql))
                {
                    oDUHardWindings = DUHardWinding.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                }
                if (oDUHardWindingReport.ReportType == 6)/// Beam TF
                {
                    sSql = "SELECT * FROM View_FabricExecutionOrderYarnReceive where  WarpWeftType=" + (int)EnumWarpWeft.Warp + " and WYRequisitionID in (select WYRequisitionID from  WYRequisition where WYarnType=" + (int)EnumWYarnType.DyedYarn + " and IssueDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and IssueDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy  hh:mm tt") + "') ORDER BY FSCDID";
                }
                if (oDUHardWindingReport.ReportType == 8)///Left Over
                {
                    sSql = "SELECT * FROM View_FabricExecutionOrderYarnReceive where  isnull(ReceiveBy,0)<>0 and WYRequisitionID in (select WYRequisitionID from  WYRequisition where RequisitionType=" + (int)EnumInOutType.Disburse + "  and WYarnType=" + (int)EnumWYarnType.LeftOver + " and IssueDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and IssueDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy  hh:mm tt") + "') ORDER BY FSCDID";
                }

                if (!string.IsNullOrEmpty(sSql))
                {
                    oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                }
                if (oDUHardWindingReport.ReportType == 4)/// Beam Comp
                {
                    sSql = "SELECT * FROM View_FabricBeamFinish WHERE ReadyDate>='" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt") + "' and ReadyDate<'" + oDUHardWindingReport.EndDate.ToString("dd MMM yyyy hh:mm tt") + "'";
                    if (!string.IsNullOrEmpty(sSql))
                    {
                        oFabricBeamFinishs = FabricBeamFinish.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                    }
                }
                if (oDUHardWindingReport.ReportType == 7)/// Beam Stock
                {
                    sSql = "SELECT * FROM View_FabricBeamFinish WHERE ISNULL(IsFinish,0) = 0 ";
                    if (!string.IsNullOrEmpty(sSql))
                    {
                        oTempFabricBeamFinishs = FabricBeamFinish.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                    }
                }

            }
            catch (Exception ex)
            {
                _oDUHardWindingReport = new DUHardWindingReport();
                _oDUHardWindingReports = new List<DUHardWindingReport>();
            }
           
            if (oDUHardWindingReport.ReportType == 1)
            {
                List<DUHardWinding> oTempDUHardWindings = new List<DUHardWinding>();
                #region Excel
                if (oDUHardWindings.Count > 0)
                {
                    oTempDUHardWindings = oDUHardWindings.Where(x => x.IsInHouse == false).ToList();
                    oDUHardWindings = oDUHardWindings.Where(x => x.IsInHouse == true).ToList();
                  
                     Company oCompany = new Company();
                     oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                     int rowIndex = 2;
                     ExcelRange cell;
                     ExcelFill fill;
                     OfficeOpenXml.Style.Border border;
                     int colIndex = 2;
                     int nSL = 0;

                     using (var excelPackage = new ExcelPackage())
                     {
                         excelPackage.Workbook.Properties.Author = "ESimSol";
                         excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                         var sheet = excelPackage.Workbook.Worksheets.Add("Hard Winding Report");
                         sheet.Name = "Dyed Yarn Receive in H/W";
                         sheet.Column(colIndex++).Width = 10; //SL
                         sheet.Column(colIndex++).Width = 20; //date
                         sheet.Column(colIndex++).Width = 15; //order no
                         sheet.Column(colIndex++).Width = 30; //customer
                         sheet.Column(colIndex++).Width = 30; //yarn type
                         sheet.Column(colIndex++).Width = 15; //batch/lotno
                         sheet.Column(colIndex++).Width = 15; //qty.

                         #region Report Header
                         sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                         cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                         cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         rowIndex++;
                         sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                         cell = sheet.Cells[rowIndex, 2]; cell.Value = "Dyed Yarn Receive in H/W"; cell.Style.Font.Bold = true;
                         cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         rowIndex++;

                         sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                         cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Range: " + oDUHardWindingReport.StartDateInString + " To " + oDUHardWindingReport.EndDateInString; cell.Style.Font.Bold = true;
                         cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         rowIndex++;

                         sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                         cell = sheet.Cells[rowIndex, 2]; cell.Value = "In House"; cell.Style.Font.Bold = true;
                         cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;                       
                         rowIndex += 2;
                         #endregion

                         #region Header 1
                         colIndex = 2;
                         cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                         cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Recd. Date"; cell.Style.Font.Bold = true;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                         cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                         cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                         cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                         cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch/LotNo"; cell.Style.Font.Bold = true;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                         cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                         fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                         rowIndex++;
                         #endregion

                         #region Print Body/IN House
                         nSL = 1;
                         foreach (DUHardWinding oItem in oDUHardWindings)
                         {
                             colIndex = 2;
                             cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                             cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceiveDateST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                             cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DyeingOrderNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                             cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContractorName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                             cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                             cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LotNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                             cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                             rowIndex++;
                             nSL++;

                         }
                         #region TOTAL
                         cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                         cell = sheet.Cells[rowIndex, 8]; cell.Value = oDUHardWindings.Sum(x => x.Qty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                         rowIndex +=2;
                         #endregion
                         #endregion

                         #region Print Body/Out House
                         if (oTempDUHardWindings.Count > 0)
                         {
                             rowIndex = rowIndex; colIndex = 2;
                             sheet.Cells[rowIndex, colIndex, rowIndex, 14].Merge = true;
                             cell = sheet.Cells[rowIndex, 2]; cell.Value = "Out House"; cell.Style.Font.Bold = true;
                             cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                             rowIndex+=2;
                             nSL = 1;
                             foreach (DUHardWinding oItem in oTempDUHardWindings)
                             {
                                 colIndex = 2;
                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceiveDateST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DyeingOrderNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContractorName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LotNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                 cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                 rowIndex++;
                                 nSL++;

                             }
                             #region TOTAL
                             cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                             cell = sheet.Cells[rowIndex, 8]; cell.Value = oTempDUHardWindings.Sum(x => x.Qty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                             rowIndex += 2;
                             #endregion
                            #endregion
                         }
                         
                         Response.ClearContent();
                         Response.BinaryWrite(excelPackage.GetAsByteArray());
                         Response.AddHeader("content-disposition", "attachment; filename=RPT_HardWinding.xlsx");
                         Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                         Response.Flush();
                         Response.End();
                     }

                }
                #endregion
            }

            if (oDUHardWindingReport.ReportType == 2)
            {
                List<DUHardWinding> oTempDUHardWindings1 = new List<DUHardWinding>();
                List<DUHardWinding> oTempDUHardWindings2 = new List<DUHardWinding>();               
                #region Excel
                if (oDUHardWindings.Count >=0)
                {
                    oTempDUHardWindings1 = oDUHardWindings.Where(x => x.IsInHouse == true && x.IsRewinded == true).ToList();
                    oTempDUHardWindings2 = oDUHardWindings.Where(x => x.IsInHouse == false && x.IsRewinded == true).ToList();
                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int colIndex = 2;
                    int nSL = 0;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Hard Winding Report");
                        sheet.Name = "Dyed Yarn Receive in H/W";
                        sheet.Column(colIndex++).Width = 10; //SL
                        sheet.Column(colIndex++).Width = 20; //date
                        sheet.Column(colIndex++).Width = 15; //order no
                        sheet.Column(colIndex++).Width = 25; //customer
                        sheet.Column(colIndex++).Width = 30; //yarn type
                        sheet.Column(colIndex++).Width = 15; //batch/lotno
                        sheet.Column(colIndex++).Width = 15; //qty.

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;
                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Dyed Yarn Receive in H/W"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Range: " + oDUHardWindingReport.StartDateInString + " To " + oDUHardWindingReport.EndDateInString; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;
                       

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "In House"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex += 2;

                  
                        #endregion

                        #region Header 1
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Recd. Date"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch/LotNo"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        #endregion

                        #region Print Body/IN House
                        nSL = 1;
                        foreach (DUHardWinding oItem in oTempDUHardWindings1)
                        {
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceiveDateST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DyeingOrderNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContractorName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LotNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rowIndex++;
                            nSL++;

                        }
                        #region TOTAL
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oTempDUHardWindings1.Sum(x => x.Qty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex += 2;
                        #endregion
                        #endregion

                        #region Print Body/Out House
                        if (oTempDUHardWindings2.Count > 0)
                        {
                            rowIndex = rowIndex; colIndex = 2;
                            sheet.Cells[rowIndex, colIndex, rowIndex, 14].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = "Out House"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex += 2;
                            nSL = 1;
                            foreach (DUHardWinding oItem in oTempDUHardWindings2)
                            {
                                colIndex = 2;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceiveDateST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DyeingOrderNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContractorName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LotNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                rowIndex++;
                                nSL++;

                            }
                            #region TOTAL
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oTempDUHardWindings2.Sum(x => x.Qty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rowIndex += 2;
                            #endregion
                        #endregion
                        }

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=RPT_HardWinding.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }

                }
                #endregion             
            }

            if (oDUHardWindingReport.ReportType == 6)
            {
                #region Excel
                if (oFabricExecutionOrderYarnReceives.Count >= 0)
                {
                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int colIndex = 2;
                    int nSL = 0;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Beam T/F");
                        sheet.Name = "Beam T/F";
                        sheet.Column(colIndex++).Width = 10; //SL
                        sheet.Column(colIndex++).Width = 15; //order no
                        sheet.Column(colIndex++).Width = 25; //customer
                        sheet.Column(colIndex++).Width = 30; //yarn type
                        sheet.Column(colIndex++).Width = 15; //batch/lotno
                         sheet.Column(colIndex++).Width = 15; //T/F

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;
                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Beam T/F"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Range: " + oDUHardWindingReport.StartDateInString + " To " + oDUHardWindingReport.EndDateInString; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "In House"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex+=2;
                        #endregion

                        #region Header 1
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch/LotNo"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "T/F"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        #endregion

                        #region Print FabricExecutionOrderYarnReceives
                        nSL = 1;
                        var data = oFabricExecutionOrderYarnReceives.GroupBy(x => new { x.FSCDID, x.DispoNo,x.WYRequisitionID }, (key, grp) => new
                        {
                            FSCDID = key.FSCDID,
                            WYRequisitionID = key.WYRequisitionID,
                            DispoNo = key.DispoNo,
                            TFLength = grp.Max(x=>x.TFLength),
                            Results = grp.ToList()
                        });

                          double nTotalTFLength = 0;
                          int nFSCDID = -99,nWYRequisitionID=99;

                          data = data.OrderBy(x => x.FSCDID).ThenBy(x => x.WYRequisitionID);

                        foreach (var oData in data)
                        {
                            foreach (var oItem1 in oData.Results)
                            {
                            colIndex = 2;

                            if (nFSCDID != oData.FSCDID || nWYRequisitionID != oData.WYRequisitionID)//sDispoNo != oData.DispoNo
                            {
                                cell = sheet.Cells[rowIndex, colIndex, rowIndex + oData.Results.Count() - 1, colIndex++]; cell.Merge = true; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex, rowIndex + oData.Results.Count() - 1, colIndex++]; cell.Merge = true; cell.Value = oData.DispoNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                nSL++;
                            }
                            else colIndex=4;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.LotNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            if (nFSCDID != oData.FSCDID || nWYRequisitionID != oData.WYRequisitionID)//sDispoNo != oData.DispoNo
                            {
                                cell = sheet.Cells[rowIndex, colIndex, rowIndex + oData.Results.Count() - 1, colIndex++]; cell.Merge = true; cell.Value = Global.MillionFormat(oData.TFLength); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                nTotalTFLength += oData.TFLength;
                            }
                            else colIndex++;

                            nFSCDID = oData.FSCDID;
                            nWYRequisitionID= oData.WYRequisitionID;
                            rowIndex++;
                           
                            }

                        }
                        #region TOTAL
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = Global.MillionFormat(nTotalTFLength); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex += 2;
                        #endregion
                        #endregion
                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=RPT_HardWinding.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                       }
                }
                #endregion
            }

            if (oDUHardWindingReport.ReportType == 8)
            {
                #region Excel
                if (oFabricExecutionOrderYarnReceives.Count >= 0)
                {
                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int colIndex = 2;
                    int nSL = 0;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Left Over");
                        sheet.Name = "Left Over";
                        sheet.Column(colIndex++).Width = 10; //SL
                        sheet.Column(colIndex++).Width = 15; //order no
                        sheet.Column(colIndex++).Width = 25; //customer
                        sheet.Column(colIndex++).Width = 30; //yarn type
                        sheet.Column(colIndex++).Width = 15; //batch/lotno
                        sheet.Column(colIndex++).Width = 15; //T/F

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Range: " + oDUHardWindingReport.StartDateInString + " To " + oDUHardWindingReport.EndDateInString; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Left Over"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        #endregion

                        #region Header 1
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch/LotNo"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty(KG)"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        #endregion

                        #region Print FabricExecutionOrderYarnReceives
                        nSL = 1;
                        var data = oFabricExecutionOrderYarnReceives.GroupBy(x => new { x.FSCDID, x.DispoNo, x.WYRequisitionID }, (key, grp) => new
                        {
                            FSCDID = key.FSCDID,
                            WYRequisitionID = key.WYRequisitionID,
                            DispoNo = key.DispoNo,
                            TFLength = grp.Max(x => x.TFLength),
                            Results = grp.ToList()
                        });

                        double nTotalTFLength = 0;
                        int nFSCDID = -99, nWYRequisitionID = 99;

                        data = data.OrderBy(x => x.FSCDID).ThenBy(x => x.WYRequisitionID);

                        foreach (var oData in data)
                        {
                            foreach (var oItem1 in oData.Results)
                            {
                                colIndex = 2;

                                if (nFSCDID != oData.FSCDID || nWYRequisitionID != oData.WYRequisitionID)//sDispoNo != oData.DispoNo
                                {
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oData.DispoNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                }

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.LotNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem1.ReceiveQty); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                nTotalTFLength += oItem1.ReceiveQty;
                                nFSCDID = oData.FSCDID;
                                nWYRequisitionID = oData.WYRequisitionID;
                                rowIndex++;
                                nSL++;
                            }

                        }
                        #region TOTAL
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = Global.MillionFormat(nTotalTFLength); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex += 2;
                        #endregion
                        #endregion
                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=RPT_HardWinding.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                }
                #endregion
            }

            if (oDUHardWindingReport.ReportType == 4)
            {
                #region Excel
                if (oFabricBeamFinishs.Count >= 0)
                {
                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int colIndex = 2;
                    int nSL = 0;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Beam Complete");
                        sheet.Name = "Beam Complete";
                        sheet.Column(colIndex++).Width = 10; //SL
                        sheet.Column(colIndex++).Width = 15; //order no
                        sheet.Column(colIndex++).Width = 25; //customer
                        sheet.Column(colIndex++).Width = 30; //yarn type
                        sheet.Column(colIndex++).Width = 15; //batch/lotno
                        sheet.Column(colIndex++).Width = 15; //Ready Mtr.
                        sheet.Column(colIndex++).Width = 15; //Transfer Mtr
                        sheet.Column(colIndex++).Width = 15; //Beam No
                        sheet.Column(colIndex++).Width = 15; //Balance

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Beam Complete"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Range: " + oDUHardWindingReport.StartDateInString + " To " + oDUHardWindingReport.EndDateInString; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        #endregion

                        #region Header 1
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Ready Date"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Count"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Ready Mtr."; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Beam No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        #endregion

                        #region Print FabricExecutionOrderYarnReceives

                        foreach (var oItem1 in oFabricBeamFinishs)
                        {
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.ReadyDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.ExeNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.CustomerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Count"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem1.LengthFinish); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.BeamNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rowIndex++;
                            nSL++;

                        }
                        #region TOTAL
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = Global.MillionFormat(oFabricBeamFinishs.Sum(x => x.LengthFinish)); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = ""; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = ""; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex += 2;
                        #endregion
                        #endregion
                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=RPT_HardWinding.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                }
                #endregion
            }

            if (oDUHardWindingReport.ReportType == 7)
            {
                #region Excel
                if (oTempFabricBeamFinishs.Count >= 0)
                {
                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int colIndex = 2;
                    int nSL = 0;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Beam Stock");
                        sheet.Name = "Beam Stock";
                        sheet.Column(colIndex++).Width = 10; //SL
                        sheet.Column(colIndex++).Width = 15; //order no
                        sheet.Column(colIndex++).Width = 25; //customer
                        sheet.Column(colIndex++).Width = 30; //yarn type
                        sheet.Column(colIndex++).Width = 15; //batch/lotno
                        sheet.Column(colIndex++).Width = 15; //Ready Mtr.
                        sheet.Column(colIndex++).Width = 15; //Transfer Mtr
                        sheet.Column(colIndex++).Width = 15; //Beam No
                        sheet.Column(colIndex++).Width = 15; //Balance

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Range: " + oDUHardWindingReport.StartDateInString + " To " + oDUHardWindingReport.EndDateInString; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Beam Stock"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        #endregion

                        #region Header 1
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Ready Date"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Count"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Ready Mtr."; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Transfer Mtr."; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Beam No"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        #endregion

                        #region Print FabricExecutionOrderYarnReceives
                        nSL = 1;
                        var data = oTempFabricBeamFinishs.GroupBy(x => new { x.FEOSID }, (key, grp) => new
                        {
                            nFEOSID = key.FEOSID,
                            Results = grp.ToList()
                        });
                        double nTotalBalance = 0, nTotalTFlength = 0;
                        int nFEOSID = 0;

                       foreach (var oData in data)
                        {
                            foreach (var oItem1 in oData.Results)
                            {
                                colIndex = 2;                   
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.ReadyDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.ExeNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.CustomerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value ="Count"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem1.LengthFinish); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                if (nFEOSID != oData.nFEOSID)
                                {
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem1.TFlength); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    nTotalTFlength += oItem1.TFlength;
                                }

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =oItem1.BeamNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                if (nFEOSID != oData.nFEOSID)
                                {
                                    double nBalance = oTempFabricBeamFinishs.Where(x => x.FEOSID == oData.nFEOSID).Sum(y => y.LengthFinish) - oItem1.TFlength;
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nBalance); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    nTotalBalance += nBalance;
                                }

                                nFEOSID = oData.nFEOSID;
                                rowIndex++;
                                nSL++;
                            }

                        }
                        #region TOTAL
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = Global.MillionFormat(oTempFabricBeamFinishs.Sum(x => x.LengthFinish)); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = Global.MillionFormat(nTotalTFlength); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = ""; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value =  Global.MillionFormat(nTotalBalance); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rowIndex += 2;
                        #endregion
                        #endregion
                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=RPT_HardWinding.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                }
                #endregion
            }

                 
     }
        
        #endregion

    }

}
