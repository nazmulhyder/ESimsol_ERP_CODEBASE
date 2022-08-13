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
using ReportManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSolFinancial.Controllers.ReportController
{
    public class RptStockReportSimplifiedController : Controller
    {
        #region Declaration
        int _nType = 0;
        DateTime _dStartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;
        List<RptStockReportSimplified> oRptStockReportSimplifieds = new List<RptStockReportSimplified>();
        List<FabricQCGrade> oFabricQCGrades = new List<FabricQCGrade>();
        #endregion
        public ActionResult ViewStockReportSimplified(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            oRptStockReportSimplifieds = new List<RptStockReportSimplified>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == (int)EnumCompareOperator.EqualTo || x.id == (int)EnumCompareOperator.Between);
            ViewBag.BUID = buid;
            return View(oRptStockReportSimplifieds);
        }

        [HttpPost]
        public JsonResult AdvSearch(RptStockReportSimplified oRptStockReportSimplified)
        {
            List<RptStockReportSimplified> oRptStockReportSimplifieds = new List<RptStockReportSimplified>();
            int nDate = Convert.ToInt32(oRptStockReportSimplified.ErrorMessage.Split('~')[0]);                                
            DateTime dEndDate = DateTime.Now;
            DateTime dStartDate = DateTime.Now; 
            try
            {
                if (nDate == 1)
                {
                    dStartDate = Convert.ToDateTime(oRptStockReportSimplified.ErrorMessage.Split('~')[1]);
                    dEndDate = dStartDate.AddDays(1);
                }
                if (nDate == 5)
                {
                    dStartDate = Convert.ToDateTime(oRptStockReportSimplified.ErrorMessage.Split('~')[1]);
                    dEndDate = Convert.ToDateTime(oRptStockReportSimplified.ErrorMessage.Split('~')[2]);
                }

                string sSQL = "";
                oRptStockReportSimplified.WorkingUnitID = 0;
                oRptStockReportSimplifieds = RptStockReportSimplified.Gets(sSQL, dStartDate, dEndDate, oRptStockReportSimplified.WorkingUnitID, (int)Session[SessionInfo.currentUserID]);
               oRptStockReportSimplifieds = oRptStockReportSimplifieds.OrderBy(x => x.ProductID).ThenBy(x=>x.LotID).ToList();
            }
            catch (Exception ex)
            {
                oRptStockReportSimplifieds = new List<RptStockReportSimplified>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRptStockReportSimplifieds);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void StockReportSimplifiedExcel(string sValue, double nts)
        {
            List<RptStockReportSimplified> oRptStockReportSimplifieds = new List<RptStockReportSimplified>();
            string sString = sValue, sDateMsg = "";
            int nDate = Convert.ToInt32(sString.Split('~')[0]);
            DateTime dEndDate = DateTime.Now;
            DateTime dStartDate = DateTime.Now;
            int StoreID = 0;
            int nReportType = 0;
            try
            {
                if (nDate == 1)
                {
                    dStartDate = Convert.ToDateTime(sString.Split('~')[1]);
                    dEndDate = dStartDate.AddDays(1);
                    sDateMsg = "Equal: " + dStartDate.ToString("dd MMM yyyy");
                }
                if (nDate == 5)
                {
                    dStartDate = Convert.ToDateTime(sString.Split('~')[1]);
                    dEndDate = Convert.ToDateTime(sString.Split('~')[2]);
                    sDateMsg = "Between: " + dStartDate.ToString("dd MMM yyyy") + "  To  " + dEndDate.ToString("dd MMM yyyy");
                }

                string sSQL = "";
                oRptStockReportSimplifieds = RptStockReportSimplified.Gets(sSQL, dStartDate, dEndDate, 0, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRptStockReportSimplifieds = new List<RptStockReportSimplified>();

            }

            #region EXCEL START
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nTotalCol = 0;
            int nCount = 0;
            int colIndex = 2;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Stock Report Simplified");
                sheet.Name = "Stock Report Simplified";

                sheet.Column(colIndex++).Width = 10; //SL
                sheet.Column(colIndex++).Width = 20; //brand
                sheet.Column(colIndex++).Width = 15; //code
                sheet.Column(colIndex++).Width = 15; //use in process
                sheet.Column(colIndex++).Width = 18; //category
                sheet.Column(colIndex++).Width = 15; //product rcv date              
                sheet.Column(colIndex++).Width = 20; //lot no
                sheet.Column(colIndex++).Width = 15; //store name
                sheet.Column(colIndex++).Width = 15; //imported qty
                sheet.Column(colIndex++).Width = 15; //opening qty
                sheet.Column(colIndex++).Width = 15; //qty transger-PT
                sheet.Column(colIndex++).Width = 10; //qty transger-DYEING
                sheet.Column(colIndex++).Width = 15; //qty transger-PRINTING
                sheet.Column(colIndex++).Width = 15; //qty transger--FINISING
                sheet.Column(colIndex++).Width = 15; //qty USGE--pt
                sheet.Column(colIndex++).Width = 15; //qty USGE--dyeing
                sheet.Column(colIndex++).Width = 15; //qty USGE--printing
                sheet.Column(colIndex++).Width = 15; //qty USGE-finising             
                sheet.Column(colIndex++).Width = 15; //stock in main store
                sheet.Column(colIndex++).Width = 15; //stock in production floor
                sheet.Column(colIndex++).Width = 15; //unit
                sheet.Column(colIndex++).Width = 15; //stock value
                sheet.Column(colIndex++).Width = 15; //manufacturing date
                sheet.Column(colIndex++).Width = 15; //expirey date
                sheet.Column(colIndex++).Width = 15; //lot age
                sheet.Column(colIndex++).Width = 15; //consumption
                sheet.Column(colIndex++).Width = 12; //approx stock out deadline 
                sheet.Column(colIndex++).Width = 15; //import pi        
                sheet.Column(colIndex++).Width = 15; //lc no
                sheet.Column(colIndex++).Width = 15; //unit price
                sheet.Column(colIndex++).Width = 15; //receive qty

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Dyes/Chemical Master Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex += 1;

                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = sDateMsg; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex += 2;
                #endregion
                #region Report Body
                var StockReportSimplifieds = oRptStockReportSimplifieds.GroupBy(x => new { x.ProductName })
                                            .OrderBy(x => x.Key.ProductName)
                                            .Select(x => new
                                            {
                                                _ProductName = x.Key.ProductName,
                                                _StockReportSimplifiedList = x.OrderBy(c => c.LotID),
                                                _SubTotalOpeningQty = x.Sum(y => y.OpeningQty),
                                                _SubTotalImportedQty = x.Sum(y => y.ImportedQty),
                                                _SubTotalQtyTransferredPT = x.Sum(y => y.PTTransferedQty),
                                                _SubTotalQtyTransferDyeing = x.Sum(y => y.DyeingTransferdQty),
                                                _SubTotalQtyTransferPrinting = x.Sum(y => y.PrintingTransferedQty),
                                                _SubTotalQtyTransferFinishing = x.Sum(y => y.FinisingTransferedQty),
                                                _SubTotalQtyUsagePT = x.Sum(y => y.PretreatmentUsage),
                                                _SubTotalQtyUsageDyeing = x.Sum(y => y.DyeingUsage),
                                                _SubTotalQtyUsagePrinting = x.Sum(y => y.PrintingUsage),
                                                _SubTotalQtyUsageFinishing= x.Sum(y => y.FinisingUsage),

                                            });

                foreach (var oData in StockReportSimplifieds)
                {
                    int nSL = 1;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 31]; cell.Merge = true; cell.Value = "" + oData._ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex++;

                    #region Header 1
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Brand"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Code"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Use In Process"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Category"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Receive Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Imported Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Opening Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Transferred-PT"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Transferred-Dyeing"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Transferred-Printing"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty Transferred-Finishing"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "PT-Usage"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Dyeing Usage"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Printing Usage"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Finishing Usage"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Stock In Main Store"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Stock In Production Floor"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Stock Value"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Manufacturing Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Expirey Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Lot Age"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Consumption Frequency(monthly)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Approx Stock Out Deadline"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Import PI"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    #endregion
                    //data
                    foreach (var oItem in oData._StockReportSimplifiedList)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Brand; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.UseInProcesss; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CategoryName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductReceivedDateST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LotNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StoreName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ImportedQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OpeningQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PTTransferedQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DyeingTransferdQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PrintingTransferedQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FinisingTransferedQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PretreatmentUsage; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DyeingUsage; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PrintingUsage; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FinisingUsage; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StockInMainStore; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StockInProductionFloor; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Unit; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StockValue; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ManufacturingDateST;  cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ExpireyDateST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LotAge; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ConsumptionFrequency; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ApproxStockOutDeadlineST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ImportPI;  cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LCNo;cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.UnitPrice;  cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        nSL++;
                        rowIndex++;

                    }

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oData._SubTotalImportedQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = oData._SubTotalOpeningQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = oData._SubTotalQtyTransferredPT; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = oData._SubTotalQtyTransferDyeing; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = oData._SubTotalQtyTransferPrinting; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = oData._SubTotalQtyTransferFinishing; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = oData._SubTotalQtyUsagePT; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = oData._SubTotalQtyUsageDyeing; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = oData._SubTotalQtyUsagePrinting; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = oData._SubTotalQtyUsageFinishing; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    nCount = 12; int ii=20;
                    while (nCount > 0)
                    {
                        cell = sheet.Cells[rowIndex, ii]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        ii++; nCount--;
                    }
                    rowIndex++;

                }

                cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Grand Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.ImportedQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 11]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.OpeningQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 12]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.PTTransferedQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 13]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.DyeingTransferdQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 14]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.PrintingTransferedQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.FinisingTransferedQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 16]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.PretreatmentUsage); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 17]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.DyeingUsage); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 18]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.PrintingUsage); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 19]; cell.Value = oRptStockReportSimplifieds.Sum(x => x.FinisingUsage); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                
                nCount = 12; int jj= 20;
                while (nCount > 0)
                {
                    cell = sheet.Cells[rowIndex, jj]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    jj++; nCount--;
                }

                rowIndex++;
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=RPT_StockReportSimplified.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
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

    }
}