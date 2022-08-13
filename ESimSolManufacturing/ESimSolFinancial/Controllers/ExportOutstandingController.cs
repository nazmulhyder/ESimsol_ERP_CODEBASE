using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ESimSolFinancial.Models;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Reports;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web.Script.Serialization;


namespace ESimSolFinancial.Controllers.ReportController
{
    public class ExportOutstandingController : Controller
    {


        #region Export Mgt Challan

        #region Export Mgt Challan (First interface)
        public ActionResult ViewExportOutstandings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ExportOutstanding oExportOutstanding = new ExportOutstanding();
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>();

            oExportOutstanding.FromDate = DateTime.Now;
            oExportOutstanding.ToDate = DateTime.Now;
            oExportOutstanding.FromDate = oExportOutstanding.ToDate.AddYears(-1);
            oExportOutstanding.BUID = buid;
            oExportOutstandings = this.GetsExportOutstandingList(oExportOutstanding);

            ViewBag.ExportOutstanding = oExportOutstanding;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.BUID = buid;
            Company oCompany = new Company();
            ViewBag.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oExportOutstandings);
        }
        private List<ExportOutstanding> GetsExportOutstandingList(ExportOutstanding oExportOutstanding)
        {
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>();
            oExportOutstandings = ExportOutstanding.Gets(oExportOutstanding.FromDate, oExportOutstanding.ToDate, oExportOutstanding.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //oExportOutstandings.ForEach(o => o.OperationStageSt = o.OperationStageFromEnum);
            oExportOutstandings.Add(this.GetTotalExportOutstanding(oExportOutstandings));
            return oExportOutstandings;
        }
        //public static string GetOperationStageObjs(EnumOperationStage eEnumOperationStage)
        //{
        //}
        private ExportOutstanding GetTotalExportOutstanding(List<ExportOutstanding> oExportOutstandings)
        {
            ExportOutstanding oExportOutstanding = new ExportOutstanding();
            oExportOutstanding.OperationStageSt = "Total : ";
            oExportOutstanding.Qty = oExportOutstandings.Select(o => o.Qty).Sum();
            oExportOutstanding.Amount = oExportOutstandings.Select(o => o.Amount).Sum();
            return oExportOutstanding;
        }
        [HttpPost]
        public JsonResult GetsExportOutstanding(ExportOutstanding oExportOutstanding)
        {
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>();
            try
            {
                oExportOutstandings = this.GetsExportOutstandingList(oExportOutstanding);
            }
            catch (Exception ex)
            {
                oExportOutstandings = new List<ExportOutstanding>();
                oExportOutstanding = new ExportOutstanding();
                oExportOutstanding.ErrorMessage = ex.Message;
                oExportOutstandings.Add(oExportOutstanding);
            }
            var jsonResult = Json(oExportOutstandings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public void ExcelExportOutstandings(string sParams)
        {
            ExportOutstanding oExportOutstanding = new ExportOutstanding();
            List<ExportOutstanding> oExportOutstandings = new List<ExportOutstanding>();
            if (!string.IsNullOrEmpty(sParams))
            {
                oExportOutstanding.BUID = Convert.ToInt32(sParams.Split('~')[0]);
                oExportOutstanding.FromDate = Convert.ToDateTime(sParams.Split('~')[1]);
                oExportOutstanding.ToDate = Convert.ToDateTime(sParams.Split('~')[2]);
                oExportOutstandings = this.GetsExportOutstandingList(oExportOutstanding);
                oExportOutstandings = oExportOutstandings.Take(oExportOutstandings.Count - 1).ToList();
            }

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oExportOutstanding.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Sales & Marketing Department");
                sheet.Name = "Sales & Marketing Department";

                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 45; //Operation Stage
                sheet.Column(4).Width = 14; //Quantity in KG
                sheet.Column(5).Width = 14; //Value in US$
                sheet.Column(6).Width = 18; //Remarks

                nMaxColumn = 6;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = this.GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Sales & Marketing Department"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Textile unit
                //string sTextileUnit = "";
                //if (oExportMgtChallan.B == 1) { sTextileUnit = "Spinning"; }
                //else if (oExportMgtChallan.TextileUnit == 2) { sTextileUnit = "Weaving"; }
                //else if (oExportMgtChallan.TextileUnit == 3) { sTextileUnit = "Dyeing"; }
                //else if (oExportMgtChallan.TextileUnit == 4) { sTextileUnit = "Finishing"; }

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = " Unit : " + oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;
                #endregion

                #region Date Range
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date : " + oExportOutstanding.FromDate.ToString("dd MMM yyyy") + " to " + oExportOutstanding.ToDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;
                #endregion

                #region Table Header 01
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Operation Stage"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Quantity"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Value in US$"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                #region Table Body

                int nSL = 0;
                foreach (ExportOutstanding oItem in oExportOutstandings)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OperationStageFromEnum; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                }
                #endregion

                #region Total
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total : "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                var nTotalQty = oExportOutstandings.Sum(x => x.Qty);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                var nTotalAmount = oExportOutstandings.Sum(x => x.Amount);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalAmount; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Sales & Marketing Department.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        public ActionResult Print_Report_Bill(string sTempString)
        {
            List<OperationStage> oOperationStages = new List<OperationStage>(); 
            DateTime dFromDate = DateTime.Today;
            DateTime dToDate = DateTime.Today;
            string sDateRange = "";
            int nReportType = Convert.ToInt32(sTempString.Split('~')[0]);
            dFromDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            dToDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            int nBUID = Convert.ToInt32(sTempString.Split('~')[3]);
            int nPrintype = Convert.ToInt32(sTempString.Split('~')[4]);

            string sReportHead = "";

            EnumReportLayout eReportLayout = EnumReportLayout.PartyWise;
            if (nPrintype == 1)
            {
                eReportLayout = EnumReportLayout.PartyWise;
            }
            else if (nPrintype == 2)
            {
                eReportLayout = EnumReportLayout.BankWise;
            }
            else
            {
                eReportLayout = EnumReportLayout.PartyWise;
            }
            oOperationStages = OperationStage.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
             sReportHead = OperationStage.GetsName((EnumOperationStage)nReportType, oOperationStages);
            List<ExportOutstandingDetail> oExportOutstandingDetails = new List<ExportOutstandingDetail>();
            oExportOutstandingDetails = ExportOutstandingDetail.Gets(nReportType, nBUID, dFromDate, dToDate, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptExportOutstandingDetails oReport = new rptExportOutstandingDetails();
            byte[] abytes = oReport.PrepareReportMgt(oExportOutstandingDetails, nReportType, oCompanys.First(), eReportLayout, sReportHead, sDateRange, oBusinessUnit);
            return File(abytes, "application/pdf");

        }

        #region Print XlX
        public void Print_Report_BillXL(string sTempString)
        {
            //ProductReconciliationReport 
            List<OperationStage> oOperationStages = new List<OperationStage>(); 
            DateTime dFromDate = DateTime.Today;
            DateTime dToDate = DateTime.Today;
            string sDateRange = "";
            int nReportType = Convert.ToInt32(sTempString.Split('~')[0]);
            dFromDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            dToDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            int nBUID = Convert.ToInt32(sTempString.Split('~')[3]);
            int nPrintype = Convert.ToInt32(sTempString.Split('~')[4]);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oOperationStages = OperationStage.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            string sReportHead = OperationStage.GetsName((EnumOperationStage)nReportType, oOperationStages);

            List<ExportOutstandingDetail> oExportOutstandingDetails = new List<ExportOutstandingDetail>();
            oExportOutstandingDetails = ExportOutstandingDetail.Gets(nReportType, nBUID, dFromDate, dToDate, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            int nCount = 0;
            double nAmount = 0;
            String sCurrency = "";


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 14;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export Outstanding Report");
                sheet.Name = "Export Bill Outstanding Report";
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 35;
                sheet.Column(6).Width = 30;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 25;
                sheet.Column(10).Width = 15;
                sheet.Column(11).Width = 8;
                sheet.Column(12).Width = 12;
                sheet.Column(13).Width = 20;
                sheet.Column(14).Width = 25;

                if ((EnumOperationStage)nReportType > EnumOperationStage.BOINHand)
                {
                    sheet.Column(15).Width = 15;
                    sheet.Column(16).Width = 6;
                    sheet.Column(17).Width = 15;
                    sheet.Column(18).Width = 8;
                    sheet.Column(19).Width = 15;
                    sheet.Column(20).Width = 8;
                    sheet.Column(21).Width = 15;
                    sheet.Column(22).Width = 8;
                    sheet.Column(23).Width = 15;
                    sheet.Column(24).Width = 8;
                    sheet.Column(25).Width = 15;
                    sheet.Column(26).Width = 8;
                }
                //   nEndCol = 10;
                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sReportHead; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region
                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if ((EnumOperationStage)nReportType == EnumOperationStage.AdvanceDelivery  || (EnumOperationStage)nReportType == EnumOperationStage.LCInHand_Delivery_Done)
                {
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PI Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Bank Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PI Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Delivered Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Yet To Delivery Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PI Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Delivered Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Yet To Delivery Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                else
                {


                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Bill No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "L/C Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Bank Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Concern Person"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Last Delivery"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Due Days"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Bill Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Issue Bank"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Issue Bank Branch"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if ((EnumOperationStage)nReportType > EnumOperationStage.BOINHand)
                    {

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Send To Party Date "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Due Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Receive From Party"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Due Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if ((EnumOperationStage)nReportType >= EnumOperationStage.PendingBankSubmition)
                        {
                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Bank Submit Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Due Days"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "LDBC Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Due Days"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "LDBC No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if ((EnumOperationStage)nReportType > EnumOperationStage.PendingBankAcceptance)
                            {
                                nCount++;
                                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Maturity Receive Date"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nCount++;
                                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nCount++;
                                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Due Days"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                        }
                    }
                }
                nRowIndex++;
                #endregion

                #region Data

                foreach (ExportOutstandingDetail oItem in oExportOutstandingDetails)
                {
                    sCurrency = oItem.Currency;

                    nCount = 0;
                    nSL++;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if ((EnumOperationStage)nReportType == EnumOperationStage.AdvanceDelivery  || (EnumOperationStage)nReportType == EnumOperationStage.LCInHand_Delivery_Done)
                    {
                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.LCOpeningDatest; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.BankName_Nego; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.DeliveryQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Qty - oItem.DeliveryQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value =  oItem.Amount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Currency + "" + oItem.DeliveryValue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Currency + "" + (oItem.Amount - oItem.DeliveryValue); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    else
                    {



                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + oItem.ExportBillNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.LCOpeningDatest; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.BankName_Nego; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = (oItem.PINo); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value =oItem.Amount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.DeliveryDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.TimeLag_Dalivery; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.StartDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.BankName_Issue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.BBranchName_Issue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if ((EnumOperationStage)nReportType > EnumOperationStage.BOINHand)
                        {
                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.SendToPartySt; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.TimeLag3; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.RecdFromPartySt; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if ((EnumOperationStage)nReportType >= EnumOperationStage.PendingBankSubmition)
                            {
                                nCount++;
                                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.TimeLag4; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nCount++;
                                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.SendToBankDateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nCount++;
                                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.TimeLag5; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nCount++;
                                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.LDBCDateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nCount++;
                                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.TimeLag6; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nCount++;
                                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.LDBCNo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                if ((EnumOperationStage)nReportType > EnumOperationStage.PendingBankAcceptance)
                                {
                                  
                                    nCount++;
                                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.MaturityReceivedDateSt; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    nCount++;
                                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.MaturityDateSt; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                }
                            }
                        }
                    }
                    nAmount = nAmount + oItem.Amount;


                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                nCount = 0;
                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
              
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value =  nAmount; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
              
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                if ((EnumOperationStage)nReportType > EnumOperationStage.BOINHand)
                {
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if ((EnumOperationStage)nReportType >= EnumOperationStage.PendingBankSubmition)
                    {
                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nCount++;
                        cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if ((EnumOperationStage)nReportType > EnumOperationStage.PendingBankAcceptance)
                        {
                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nCount++;
                            cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }
                }


                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportPendingReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }


        #endregion



        #endregion

        

        #endregion

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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

        #region OperationStage

        public ActionResult ViewOperationStages(int buid ,int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<OperationStage> oOperationStages = new List<OperationStage>();
            oOperationStages = OperationStage.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumOperationStages = EnumObject.jGets(typeof(EnumOperationStage));
            ViewBag.BUID = buid;
            return View(oOperationStages);

        }

        [HttpPost]
        public JsonResult SaveOperationStage(OperationStage oOperationStage)
        {
            try
            {
                if (oOperationStage.OperationStageID <= 0)
                {
                    oOperationStage = oOperationStage.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oOperationStage = oOperationStage.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oOperationStage = new OperationStage();
                oOperationStage.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOperationStage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteOperationStage(OperationStage oOperationStage)
        {
            try
            {
                if (oOperationStage.OperationStageID <= 0) { throw new Exception("Please select an valid item."); }
                oOperationStage = oOperationStage.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oOperationStage = new OperationStage();
                oOperationStage.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOperationStage.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOperationStage(OperationStage oOperationStage)
        {
            try
            {
                if (oOperationStage.OperationStageID <= 0) { throw new Exception("Please select an valid item."); }
                oOperationStage = OperationStage.Get(oOperationStage.OperationStageID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oOperationStage = new OperationStage();
                oOperationStage.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOperationStage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      
        #endregion
    }

   


}