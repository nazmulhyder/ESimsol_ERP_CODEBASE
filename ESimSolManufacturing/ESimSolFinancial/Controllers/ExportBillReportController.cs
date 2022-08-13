using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using System.IO;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Runtime.Serialization;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using ESimSol.Reports;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ExportBillReportController : Controller
    {
        #region ExportBillReport 
        ExportBillReport _oExportBillReport = new ExportBillReport();
        List<ExportBillReport> _oExportBillReports = new List<ExportBillReport>();
        public ActionResult ViewExportBillReports(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ExportBillReport> oExportBillReports = new List<ExportBillReport>();
            string sSQL = "SELECT * FROM View_ExportBillReport where [State]<1 and BUID=" + buid;
            oExportBillReports = ExportBillReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<BankBranch> oBankBranchs_Nego = new List<BankBranch>();
            oBankBranchs_Nego = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankBranchs_Nego = oBankBranchs_Nego;

            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.LCBillEventObj = EnumObject.jGets(typeof(EnumLCBillEvent));


            List<Currency> oCurrencys = new List<Currency>();
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

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
            ViewBag.Currencys = oCurrencys;
            ViewBag.BUID = buid;
            ViewBag.LCTypes = EnumObject.jGets(typeof(EnumExportLCType));
            return View(oExportBillReports);
        }

        public void Print_ExportBillReportXL(int BUID,string sParam)
        {
            ExportBillReport oExportBillReport = new ExportBillReport();
            List<ExportUPDetail> oExportUPDetails = new List<ExportUPDetail>();
            oExportBillReport.BUID = BUID;
            oExportBillReport.Params = sParam;
            string sSQL = MakeSQL(oExportBillReport);
            if (sSQL == "Error")
            {
                _oExportBillReport = new ExportBillReport();
                _oExportBillReport.ErrorMessage = "Please select a searching critaria.";
                _oExportBillReports = new List<ExportBillReport>();
            }
            else
            {
                _oExportBillReports = new List<ExportBillReport>();
                _oExportBillReports = ExportBillReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oExportBillReports.Count > 0)
                {
                    sSQL = "Select * from View_ExportUPDetail where ExportLCID in (" + string.Join(",", _oExportBillReports.Select(x => x.ExportLCID).ToList()) + ")";
                    oExportUPDetails = ExportUPDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    _oExportBillReports.ForEach(x =>
                    {
                        if (oExportUPDetails.FirstOrDefault() != null && oExportUPDetails.FirstOrDefault().ExportLCID > 0 && oExportUPDetails.Where(b => b.ExportLCID == x.ExportLCID).Count() > 0)
                        {
                            x.UPNo = oExportUPDetails.Where(p => p.ExportLCID == x.ExportLCID).FirstOrDefault().UPNo;
                        }
                    });

                   
                }

                if (_oExportBillReports.Count == 0)
                {
                    _oExportBillReports = new List<ExportBillReport>();
                }
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export Bill Report");
                sheet.Name = "Export Bill Report";
                sheet.View.FreezePanes(1, 4);

                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                sheet.Column(12).Width = 20;
                sheet.Column(13).Width = 20;
                sheet.Column(14).Width = 20;
                sheet.Column(15).Width = 20;
                sheet.Column(16).Width = 20;
                sheet.Column(17).Width = 20;
                sheet.Column(18).Width = 20;//bill date
                sheet.Column(19).Width = 20;
                sheet.Column(20).Width = 20;
                sheet.Column(21).Width = 20;
                sheet.Column(22).Width = 20;
                sheet.Column(23).Width = 20;
                sheet.Column(24).Width = 20;
                sheet.Column(25).Width = 20;
                sheet.Column(26).Width = 20;
                sheet.Column(27).Width = 20;
                sheet.Column(28).Width = 20;
                sheet.Column(29).Width = 20;
                sheet.Column(30).Width = 20;
                sheet.Column(31).Width = 20;
                sheet.Column(32).Width = 25;
                //added more 13 col for time lag
                sheet.Column(33).Width = 20;
                sheet.Column(34).Width = 20;
                sheet.Column(35).Width = 20;
                sheet.Column(36).Width = 20;
                sheet.Column(37).Width = 20;
                sheet.Column(38).Width = 20;
                sheet.Column(39).Width = 20;
                sheet.Column(40).Width = 20;
                sheet.Column(41).Width = 20;
                sheet.Column(42).Width = 20;
                sheet.Column(43).Width = 20;
                sheet.Column(44).Width = 20;
                sheet.Column(45).Width = 20;

                nMaxColumn = 45;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);


                #region Report Header
                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rowIndex = rowIndex + 1;

                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = "Export Bill Report"; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rowIndex = rowIndex + 2;
                #endregion

                #region Table Header
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Textile Unit"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Master LC No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "L/C Open Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Expiry Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Buying House"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Amount($)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Currency"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Advice Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Negotiate Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Current Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Issue Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Issue Bank Branch"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Doc Prepare Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bill Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Send To Party"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Recd From Party"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Send To Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Recd From Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "IBC/LDBC No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "IBC/LDBC Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Submit To Party Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Maturity Red Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Discounted Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Relization Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BankFDD Recd Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Encashment Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Delay Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Concern Person"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "UP No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                rowIndex++;
                #endregion

                #region Table Body
                double nTotalQty = 0,
                       nTotalAmount = 0;



                int nSL = 0;
                foreach (ExportBillReport oItem in _oExportBillReports)
                {
                    //nTotalQty += oItem.Qty;
                    nTotalAmount += oItem.Amount;
                    //nTotalQty_DC += oItem.Qty_DC;
                    //nTotalChallanValue += oItem.ChallanValue;
                    //nTotalInvoiceValue += oItem.InvoiceValue;
                    //nTotalQty_DO += oItem.DOQty;
                    //nTotalDOValue += oItem.DOValue;

                    nSL++;
                    colIndex = 2;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BUID; cell.Style.Font.Bold = false;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ExportBillNoSt; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MasterLCNos; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LCOpeningDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ApplicantName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ShipmentDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ExpiryDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =Math.Round(oItem.Amount,2);  cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Currency; cell.Style.Font.Bold = false;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankName_Advice; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankName_Nego; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StateSt; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankName_Issue; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BBranchName_Issue; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DocPrepareDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StartDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag1; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SendToParty; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag2; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RecdFromParty; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag3; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SendToBankDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag4; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RecedFromBankDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag5; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LDBCNo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag6; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LDBCDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AcceptanceDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag7; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MaturityReceivedDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag8; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MaturityDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag9; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DiscountedDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag10; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RelizationDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag11; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankFDDRecDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag12; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EncashmentDate; cell.Style.Font.Bold = false;
                    cell.Style.Numberformat.Format = "dd mmm yyyy"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeLag13; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.UPNo; cell.Style.Font.Bold = false;
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

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total : "; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;//nTotalQty
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalAmount; cell.Style.Font.Bold = true;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportBillReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public ActionResult Print_ExportBillReport(int BUID, string sParam, string sExportBillFieldST)
        {
            ExportBillReport oExportBillReport = new ExportBillReport();
            oExportBillReport.Params = sParam;
            string sSQL = MakeSQL(oExportBillReport);
            if (sSQL == "Error")
            {
                _oExportBillReport = new ExportBillReport();
                _oExportBillReport.ErrorMessage = "Please select a searching critaria.";
                _oExportBillReports = new List<ExportBillReport>();
            }
            else
            {
                _oExportBillReports = new List<ExportBillReport>();
                _oExportBillReports = ExportBillReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportBillReports.Count == 0)
                {
                    _oExportBillReports = new List<ExportBillReport>();
                }
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany= oCompanys.First();
            oExportBillReport.ErrorMessage = sExportBillFieldST;

            rptExportBillReport oReport = new rptExportBillReport();
            byte[] abytes = oReport.PrepareReport(_oExportBillReports,oBusinessUnit, oCompany,"Export Bill List", sExportBillFieldST);
            return File(abytes, "application/pdf");

        }

        #region search 
      
        public JsonResult AdvanceSearch(ExportBillReport oExportBillReport)
        {
            List<ExportBillReport> oExportBillReports = new List<ExportBillReport>();
            List<ExportUPDetail> oExportUPDetails = new List<ExportUPDetail>();
            string sSQL = MakeSQL(oExportBillReport);
            if (sSQL == "Error")
            {
                _oExportBillReport = new ExportBillReport();
                _oExportBillReport.ErrorMessage = "Please select a searching critaria.";
                oExportBillReports = new List<ExportBillReport>();
            }
            else
            {
                oExportBillReports = new List<ExportBillReport>();
                oExportBillReports = ExportBillReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oExportBillReports.Count > 0)
                {
                    sSQL = "Select * from View_ExportUPDetail where ExportLCID in (" + string.Join(",", oExportBillReports.Select(x => x.ExportLCID).ToList()) + ")";
                    oExportUPDetails = ExportUPDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


                    oExportBillReports.ForEach(x =>
                    {
                        if (oExportUPDetails.FirstOrDefault() != null && oExportUPDetails.FirstOrDefault().ExportLCID > 0 && oExportUPDetails.Where(b => b.ExportLCID == x.ExportLCID).Count() > 0)
                        {
                            x.UPNo = oExportUPDetails.Where(p => p.ExportLCID == x.ExportLCID).FirstOrDefault().UPNo;
                        }
                    });

                   
                }
                if (oExportBillReports.Count == 0)
                {
                    oExportBillReports = new List<ExportBillReport>();
                }
            }
            var jsonResult = Json(oExportBillReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSQL(ExportBillReport oExportBillReport)
        {

            string sParams = oExportBillReport.Params;
          
            string sContractorIds = "";
            string sCurrentStatus = "";
            int nBankBranchID_Nego = 0;
            string sBankBranch_IssueIds = "";
            int nSearchAmountType = 0;
            double nFromAmount = 0;
            double nToAmount = 0;

            string sDateType = "";
            int nDateSearchCriteria = 0;
            DateTime dStartDateCritaria = DateTime.Today;
            DateTime dEndDateCritaria = DateTime.Today;

            int nStateDateType = 0;
            int nDateSearchState = 0;
            DateTime dStartDateState = DateTime.Today;
            DateTime dEndDateState = DateTime.Today;
            string sExportLCNo="";
               string  sExportBillNo="";
              string sExportLDBCNo="";
              string sExportPINo = "";
            int nBUID = 0;
            bool IsYetToEUP  = false;
            bool IsEUPDone = false;
            int nLCType = 0;

            if (!string.IsNullOrEmpty(sParams))
            {
                sContractorIds = Convert.ToString(sParams.Split('~')[0]);
                sCurrentStatus = Convert.ToString(sParams.Split('~')[1]);
                nBankBranchID_Nego = Convert.ToInt32(sParams.Split('~')[2]);
                sBankBranch_IssueIds = Convert.ToString(sParams.Split('~')[3]);
                nSearchAmountType = Convert.ToInt32(sParams.Split('~')[4]);
                nFromAmount = Convert.ToDouble(sParams.Split('~')[5]);
                nToAmount = Convert.ToDouble(sParams.Split('~')[6]);

                sDateType = Convert.ToString(sParams.Split('~')[7]);
                nDateSearchCriteria = Convert.ToInt32(sParams.Split('~')[8]);
                if (nDateSearchCriteria > 0)
                {
                    dStartDateCritaria = (sParams.Split('~')[9] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[9]);
                    dEndDateCritaria = (sParams.Split('~')[10] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[10]);
                }
                nStateDateType = Convert.ToInt32(sParams.Split('~')[11]);
                nDateSearchState = Convert.ToInt32(sParams.Split('~')[12]);
                if (nDateSearchState > 0)
                {
                    dStartDateState = (sParams.Split('~')[13] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[13]);
                    dEndDateState = (sParams.Split('~')[14] == "") ? DateTime.Now : Convert.ToDateTime(sParams.Split('~')[14]);
                }
                sExportLCNo = Convert.ToString(sParams.Split('~')[15]);
                 sExportBillNo = Convert.ToString(sParams.Split('~')[16]);
                 sExportLDBCNo = Convert.ToString(sParams.Split('~')[17]);
                 sExportPINo = Convert.ToString(sParams.Split('~')[18]);
                nBUID = Convert.ToInt32(sParams.Split('~')[19]);
                IsYetToEUP = Convert.ToBoolean(oExportBillReport.Params.Split('~')[20]);
                IsEUPDone = Convert.ToBoolean(oExportBillReport.Params.Split('~')[21]);
                nLCType = Convert.ToInt16(oExportBillReport.Params.Split('~')[22]);


            }
            else
            {
                sCurrentStatus = "0";
                nBUID = oExportBillReport.BUID;
            }

            string sReturn1 = "SELECT * FROM View_ExportBillReport AS EB";
            string sReturn = "";

            #region PartyName
            if (!String.IsNullOrEmpty(sContractorIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ApplicantID IN (" + sContractorIds + ")";
            }
            #endregion

            #region Current State
            if (!String.IsNullOrEmpty(sCurrentStatus))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.State IN (" + sCurrentStatus + ") ";

            }
            #endregion

            #region _Nego Bank
            if (nBankBranchID_Nego > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BankBranchID_Negotiation = " + nBankBranchID_Nego;
            }
            #endregion

            #region Issue Bank
            if (!string.IsNullOrEmpty(sBankBranch_IssueIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BankBranchID_Issue = " + sBankBranch_IssueIds;
            }
            #endregion

            #region Bill Amount
            if (nSearchAmountType > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nSearchAmountType == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " EB.Amount = " + nFromAmount;
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " EB.Amount != " + nFromAmount;
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " EB.Amount > " + nFromAmount;
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " EB.Amount < " + nFromAmount;
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " EB.Amount BETWEEN " + nFromAmount + " AND " + nToAmount + "";
                }
                else if (nSearchAmountType == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " EB.Amount NOT BETWEEN " + nFromAmount + " AND " +nToAmount + "";
                }
            }
            #endregion

            #region Date Criteria
            if (sDateType != "None")
            {
                
                if (nDateSearchCriteria > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (nDateSearchCriteria == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " EB." + sDateType + " = '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " EB." + sDateType + " != '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " EB." + sDateType + " > '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " EB." + sDateType + " < '" + dStartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EB." + sDateType + " BETWEEN '" + dStartDateCritaria.ToString("dd MMM yyy") + "' AND '" + dEndDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (nDateSearchCriteria == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EB." + sDateType + " NOT BETWEEN '" + dStartDateCritaria.ToString("dd MMM yyy") + "' AND '" + dEndDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                }
            }
            #endregion

            #region State on Date
            if (nStateDateType > -1)
            {
                
                if (nDateSearchState > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (nDateSearchState== (int)EnumCompareOperator.EqualTo)
                    {
                        // sReturn = sReturn + " EB.ExportBillReportID IN (SELECT EBH.ExportBillReportID FROM View_ExportBillReportHistory AS EBH WHERE EBH.State= " + oExportBillReport.StateDateType + " AND EBH.DBServerDateTime = '" + oExportBillReport.StartDateState.ToString("dd MMM yyyy") + "') ";
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime >= '" + dStartDateState.ToString("dd MMM yyyy") + "' AND EBH.DBServerDateTime<'" +dStartDateState.AddDays(1).ToString("dd MMM yyyy") + "') ";

                    }
                    else if (nDateSearchState== (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime != '" + dStartDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (nDateSearchState== (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime > '" + dStartDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (nDateSearchState== (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime < '" + dStartDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (nDateSearchState== (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime >= '" + dStartDateState.ToString("dd MMM yyyy") + "' AND  EBH.DBServerDateTime<'" + dEndDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (nDateSearchState== (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + nStateDateType + " AND EBH.DBServerDateTime NOT BETWEEN '" + dStartDateState.ToString("dd MMM yyyy") + "' AND '" + dEndDateState.ToString("dd MMM yyyy") + "') ";
                    }
                }
            }
            #endregion
            #region L/C No
            if (!String.IsNullOrEmpty(sExportLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportLCNo like '%" + sExportLCNo + "%' ";
            }
            #endregion
            #region L/C No
            if (!String.IsNullOrEmpty(sExportBillNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportBillNo like '%" + sExportBillNo + "%' ";
            }
            #endregion
            #region LDBCNo
            if (!String.IsNullOrEmpty(sExportLDBCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.LDBCNo like '%" + sExportLDBCNo + "%' ";
            }
            #endregion
            #region PICNo
            if (!String.IsNullOrEmpty(sExportPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "EB.ExportBillID in (Select ExportBillID from ExportBillDetail where ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINo like '%" + sExportPINo + "%')))";
            }
            #endregion
            #region BUID
            if (nBUID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BUID=" + nBUID;
            }
            #endregion

            #region Export UP

            if (IsYetToEUP)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportLCID Not In (Select  ExportLCID From View_ExportUPDetail)";
            }
            if (IsEUPDone)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportLCID In (Select  ExportLCID From View_ExportUPDetail)";
            }

            #endregion

            #region Export LC Type
            if (nLCType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportLCType = " + nLCType.ToString();
            }
            #endregion


            sReturn = sReturn1 + sReturn;
          
            return sReturn;
        }

        #endregion search
        #endregion ExportReport
    }
}