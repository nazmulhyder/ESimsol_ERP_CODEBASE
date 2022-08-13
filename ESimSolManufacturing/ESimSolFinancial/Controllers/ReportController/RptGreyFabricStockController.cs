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
    public class RptGreyFabricStockController : Controller
    {
        #region Declaration
        int _nType = 0;
        DateTime _dStartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;
        List<RptGreyFabricStock> oRptGreyFabricStocks = new List<RptGreyFabricStock>();
        List<FabricQCGrade> oFabricQCGrades = new List<FabricQCGrade>();
        #endregion
        public ActionResult ViewGreyFabricStock(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            oRptGreyFabricStocks = new List<RptGreyFabricStock>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == (int)EnumCompareOperator.EqualTo || x.id == (int)EnumCompareOperator.Between);
            string sSQL = "SELECT * FROM FabricQCGrade Order By FabricQCGradeID";
            oFabricQCGrades = FabricQCGrade.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FQCGrades = oFabricQCGrades;
            ViewBag.BUID = buid;
            return View(oRptGreyFabricStocks);
        }

        [HttpPost]
        public JsonResult AdvSearch(RptGreyFabricStock oRptGreyFabricStock)
        {
            List<RptGreyFabricStock> oRptGreyFabricStocks = new List<RptGreyFabricStock>();
            string sString = oRptGreyFabricStock.ErrorMessage;
            string sDispoNo = Convert.ToString(sString.Split('~')[0]);
            string sCustomerIDs = Convert.ToString(sString.Split('~')[1]);
            int nFabricStock = Convert.ToInt32(sString.Split('~')[2]);
            int nDate = Convert.ToInt32(sString.Split('~')[3]);                    
            int BUID = Convert.ToInt32(sString.Split('~')[6]);
            string QCGradeIDs = Convert.ToString(sString.Split('~')[7]);
            DateTime dEndDate = DateTime.Now;
            DateTime dStartDate = DateTime.Now; 
            int StoreID = 0;
            int nReportType = 0;
            try
            {
                if (nDate == 1)
                {
                    dStartDate = Convert.ToDateTime(sString.Split('~')[4]);
                    dEndDate = dStartDate.AddDays(1);
                }
                if (nDate == 5)
                {
                     dStartDate = Convert.ToDateTime(sString.Split('~')[4]);
                     dEndDate = Convert.ToDateTime(sString.Split('~')[5]);
                }

                nReportType = (nFabricStock > 1) ? 2 : 1; 

                string sSQL = MakeSQL(sString);
                oRptGreyFabricStocks = RptGreyFabricStock.Gets(sSQL,  dStartDate, dEndDate, nReportType,StoreID, (int)Session[SessionInfo.currentUserID]);
                oRptGreyFabricStocks = oRptGreyFabricStocks.OrderBy(x => x.ProcessType).ToList();
            }
            catch (Exception ex)
            {
                oRptGreyFabricStocks = new List<RptGreyFabricStock>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRptGreyFabricStocks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public string MakeSQL(string sParams)
        {
            string sDispoNo = "";
            string sCustomerIDs = "";
            string QCGradeIDs = "";
            int nFabricStock = 0;
            int nDate = 0;
            int BUID = 0;
            DateTime dStartDate = DateTime.MinValue;
            DateTime dEndDate = DateTime.MinValue;
            if (!String.IsNullOrEmpty(sParams))
            {
                sDispoNo = Convert.ToString(sParams.Split('~')[0]);
                sCustomerIDs = Convert.ToString(sParams.Split('~')[1]);
                nFabricStock = Convert.ToInt32(sParams.Split('~')[2]);
                BUID = Convert.ToInt32(sParams.Split('~')[6]);

            }
            string sReturn = " ";
            #region Dispo
            if (!string.IsNullOrEmpty(sDispoNo))
            {
                sReturn = sReturn + " Where FSCDID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail  WHERE ExeNo Like '%" + sDispoNo + "%')";
            }
            #endregion
            return sReturn;            
        }
        [HttpGet]
        public ActionResult GreyFabricPrints(string sValue,double nts)
        {
            List<RptGreyFabricStock> oRptGreyFabricStocks = new List<RptGreyFabricStock>();
            string sString = sValue, sDateMsg = "";
            string sDispoNo = Convert.ToString(sString.Split('~')[0]);
            string sCustomerIDs = Convert.ToString(sString.Split('~')[1]);
            int nFabricStock = Convert.ToInt32(sString.Split('~')[2]);
            int nDate = Convert.ToInt32(sString.Split('~')[3]);
            int BUID = Convert.ToInt32(sString.Split('~')[6]);
            string QCGradeIDs = Convert.ToString(sString.Split('~')[7]);
            DateTime dEndDate = DateTime.Now;
            DateTime dStartDate = DateTime.Now;
            int StoreID = 0;
            int nReportType = 0;
            try
            {
                if (nDate == 1)
                {
                    dStartDate = Convert.ToDateTime(sString.Split('~')[4]);
                    dEndDate = dStartDate.AddDays(1);
                    sDateMsg = "Equal: " + dStartDate.ToString("dd MMM yyyy");
                }
                if (nDate == 5)
                {
                    dStartDate = Convert.ToDateTime(sString.Split('~')[4]);
                    dEndDate = Convert.ToDateTime(sString.Split('~')[5]);
                    sDateMsg = "Between: " + dStartDate.ToString("dd MMM yyyy") + "  To  " + dEndDate.ToString("dd MMM yyyy");
                }

                nReportType = (nFabricStock > 1) ? 2 : 1;
                string sSQL = MakeSQL(sString);
                oRptGreyFabricStocks = RptGreyFabricStock.Gets(sSQL, dStartDate, dEndDate, nReportType, StoreID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRptGreyFabricStocks = new List<RptGreyFabricStock>();

            }

            if (oRptGreyFabricStocks.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBusinessUnit = new BusinessUnit();
                rptGreyFabricPrints oReport = new rptGreyFabricPrints();
                string sHeaderName = "Gery Fabric Stock Report";
                byte[] abytes = oReport.PrepareReport(oRptGreyFabricStocks, oCompany, sHeaderName, sDateMsg);
                return File(abytes, "application/pdf");

            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }
                    
        }
        [HttpGet]
        public void GreyFabricExcel(string sValue, double nts)
        {
            List<RptGreyFabricStock> oRptGreyFabricStocks = new List<RptGreyFabricStock>();
            string sString = sValue, sDateMsg = "";
            string sDispoNo = Convert.ToString(sString.Split('~')[0]);
            string sCustomerIDs = Convert.ToString(sString.Split('~')[1]);
            int nFabricStock = Convert.ToInt32(sString.Split('~')[2]);
            int nDate = Convert.ToInt32(sString.Split('~')[3]);
            int BUID = Convert.ToInt32(sString.Split('~')[6]);
            string QCGradeIDs = Convert.ToString(sString.Split('~')[7]);
            DateTime dEndDate = DateTime.Now;
            DateTime dStartDate = DateTime.Now;
            int StoreID = 0;
            int nReportType = 0;
            try
            {
                if (nDate == 1)
                {
                    dStartDate = Convert.ToDateTime(sString.Split('~')[4]);
                    dEndDate = dStartDate.AddDays(1);
                    sDateMsg = "Equal: " + dStartDate.ToString("dd MMM yyyy");
                }
                if (nDate == 5)
                {
                    dStartDate = Convert.ToDateTime(sString.Split('~')[4]);
                    dEndDate = Convert.ToDateTime(sString.Split('~')[5]);
                    sDateMsg = "Between: " + dStartDate.ToString("dd MMM yyyy") + "  To  " + dEndDate.ToString("dd MMM yyyy");
                }
                nReportType = (nFabricStock > 1) ? 2 : 1;
                string sSQL = MakeSQL(sString);
                oRptGreyFabricStocks = RptGreyFabricStock.Gets(sSQL, dStartDate, dEndDate, nReportType, StoreID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRptGreyFabricStocks = new List<RptGreyFabricStock>();

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Gery Fabric Stock Report");
                sheet.Name = "Gery Fabric Stock Report";

                sheet.Column(colIndex++).Width = 10; //SL
                sheet.Column(colIndex++).Width = 15; //dispono
                sheet.Column(colIndex++).Width = 25; //customer
                sheet.Column(colIndex++).Width = 15; //po no
                sheet.Column(colIndex++).Width = 15; //mkt rref no
                sheet.Column(colIndex++).Width = 12; //is YD
                sheet.Column(colIndex++).Width = 15; //order type              
                sheet.Column(colIndex++).Width = 25; //construction
                sheet.Column(colIndex++).Width = 15; //opening qty
                sheet.Column(colIndex++).Width = 15; //receive qty
                sheet.Column(colIndex++).Width = 15; //issue qty
                sheet.Column(colIndex++).Width = 15; //closing qty

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Gery Fabric Stock Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex += 1;

                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = sDateMsg; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex += 2;
                #endregion
                #region Report Body
                var GreyFabricStocks = oRptGreyFabricStocks.GroupBy(x => new { x.Grade })
                                            .OrderBy(x => x.Key.Grade)
                                            .Select(x => new
                                            {
                                                _Grade = x.Key.Grade,
                                                _GreyFabricStockList = x.OrderBy(c => c.DispoNo),
                                                SubTotalOpeningQty = x.Sum(y => y.OpeningQty),
                                                SubTotalReceivingQty = x.Sum(y => y.QtyIn),
                                                SubTotalIssueQty = x.Sum(y => y.QtyOut),
                                                SubTotalClosingQty = x.Sum(y => y.ClosingQty),

                                            });

                foreach (var oData in GreyFabricStocks)
                {
                    int nSL = 1;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 13]; cell.Merge = true; cell.Value = "Grade: " + oData._Grade; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex++;

                    #region Header 1
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Dispo No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "PO No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Mkt.Ref No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Is YD"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Opening Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Receive Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Issue Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Closing Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    #endregion
                    //data
                    foreach (var oItem in oData._GreyFabricStockList)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DispoNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CustomerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SCNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FabricNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.IsYDST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderTypeSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OpeningQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyIn; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyOut; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ClosingQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        nSL++;
                        rowIndex++;

                    }

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oData.SubTotalOpeningQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = oData.SubTotalReceivingQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = oData.SubTotalIssueQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = oData.SubTotalClosingQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rowIndex++;

                }

                cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Grand Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = oRptGreyFabricStocks.Sum(x => x.OpeningQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 11]; cell.Value = oRptGreyFabricStocks.Sum(x => x.QtyIn); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 12]; cell.Value = oRptGreyFabricStocks.Sum(x => x.QtyOut); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 13]; cell.Value = oRptGreyFabricStocks.Sum(x => x.ClosingQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rowIndex++;
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=RPT_GreyFabricStocks.xlsx");
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