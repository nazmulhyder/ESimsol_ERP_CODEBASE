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
using System.Linq;
using ICS.Core.Utility;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class SalesComissionRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        List<SalesComissionRegister> _oSalesComissionRegisters = new List<SalesComissionRegister>();        
        #region Actions
        public ActionResult ViewSalesComissionRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            SalesComissionRegister oSalesComissionRegister = new SalesComissionRegister();

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.PIWise || (EnumReportLayout)oItem.id == EnumReportLayout.Month_Wise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise || (EnumReportLayout)oItem.id == EnumReportLayout.PI_Wise_Details || (EnumReportLayout)oItem.id == EnumReportLayout.SalesCommissionDetails || (EnumReportLayout)oItem.id == EnumReportLayout.TOP_Sheet)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PIStatusList = EnumObject.jGets(typeof(EnumPIStatus));
            return View(oSalesComissionRegister);
        }
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(SalesComissionRegister oSalesComissionRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSalesComissionRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintSalesComissionRegister(double ts)
        {
            SalesComissionRegister oSalesComissionRegister = new SalesComissionRegister();
            string sMohterBuyerName = "";
            
            try
            {
                _sErrorMesage = "";
                _oSalesComissionRegisters = new List<SalesComissionRegister>();
                oSalesComissionRegister = (SalesComissionRegister)Session[SessionInfo.ParamObj];
                sMohterBuyerName = oSalesComissionRegister.MotherBuyerName;
                string sSQL = this.GetSQL(oSalesComissionRegister);
                _oSalesComissionRegisters = SalesComissionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oSalesComissionRegister.ReportLayout == EnumReportLayout.SalesCommissionDetails && _oSalesComissionRegisters.Count()>0 )
                {
                    sSQL = "SELECT * FROM View_ExportBill AS Bill WHERE Bill.ExportLCID In ("+string.Join(",", _oSalesComissionRegisters.Select(x => x.ExportLCID))+")";
                    oSalesComissionRegister.ExportBills = ExportBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }

                if (_oSalesComissionRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oSalesComissionRegisters = new List<SalesComissionRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oSalesComissionRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptSalesComissionRegisters oReport = new rptSalesComissionRegisters();
                byte[] abytes = oReport.PrepareReport(_oSalesComissionRegisters, oCompany, oSalesComissionRegister.ReportLayout, oSalesComissionRegister.ExportBills,_sDateRange,  sMohterBuyerName);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }
        #region Print XlX
        public void ExportToExcelSalesComissionRegister(double ts)
        {
            SalesComissionRegister oSalesComissionRegister = new SalesComissionRegister();
            _oSalesComissionRegisters = new List<SalesComissionRegister>();
            List<ExportBill> _oExportBills = new List<ExportBill>();
            oSalesComissionRegister = (SalesComissionRegister)Session[SessionInfo.ParamObj];
            string sMohterBuyerName = oSalesComissionRegister.MotherBuyerName;
            string sSQL = this.GetSQL(oSalesComissionRegister);
            _oSalesComissionRegisters = SalesComissionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            if (oSalesComissionRegister.ReportLayout == EnumReportLayout.SalesCommissionDetails && _oSalesComissionRegisters.Count() > 0)
            {
                sSQL = "SELECT * FROM View_ExportBill AS Bill WHERE Bill.ExportLCID In (" + string.Join(",", _oSalesComissionRegisters.Select(x => x.ExportLCID)) + ")";
                _oExportBills = ExportBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            if (_oSalesComissionRegisters.Count <= 0)
            {
                _sErrorMesage = "There is no data for print!";
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();    
            oBU = oBU.Get(oSalesComissionRegister.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 20, nTempCol = 1, nTempEndCol = 2;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Sales Comission Register");
                string sReportHeader = "";
                #region Report Body & Header
                sheet.Column(nTempCol).Width = 20;
                nTempCol++;sheet.Column(nTempCol).Width = 30;
                nTempCol++; sheet.Column(nTempCol).Width = 30; //3
                nTempCol++; sheet.Column(nTempCol).Width = 30; //4
                if (oSalesComissionRegister.ReportLayout == EnumReportLayout.Month_Wise)
                {
                    sReportHeader = sMohterBuyerName+" Month Wise Register "+_sDateRange;
                    nTempEndCol = 2;
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.PartyWise)
                {
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //5
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //6
                    sReportHeader = sMohterBuyerName + " Party Wise Sales Commission Register "+_sDateRange;
                    nTempEndCol = 3;
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.PIWise)
                {
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //5th col
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //6
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //7
                    sReportHeader = sMohterBuyerName + " PI Wise Sales Commission Register "+_sDateRange;
                    nTempEndCol = 3;
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.PI_Wise_Details)
                {
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //5th col
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //6
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //7
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //8
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //9
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //10
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//11
                    sReportHeader = sMohterBuyerName + " PI Wise Sales Commission Register(Details) "+_sDateRange;
                    nTempEndCol = 6;
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.SalesCommissionDetails)
                {
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //5th col
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //6
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //7
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //8
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //9
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //10
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//11
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//12
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//13
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//14
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//15
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//16
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//17
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//18
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//19
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//20
                    nTempCol++; sheet.Column(nTempCol).Width = 30;//21
                    sReportHeader = sMohterBuyerName + " Sales Commission Register " + _sDateRange;
                    nTempEndCol = 12;
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.TOP_Sheet)
                {
                    nTempCol++; sheet.Column(nTempCol).Width = 30; //5th col
                    sReportHeader = sMohterBuyerName + " TOP Sheet " + _sDateRange;
                    nTempEndCol = 3;
                }

                #endregion
                nEndCol = nTempCol;
                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - nTempEndCol]; cell.Merge = true;
                cell.Value = oBU.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[nRowIndex, nEndCol - (nTempEndCol-1), nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - nTempEndCol]; cell.Merge = true;
                cell.Value = oBU.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[nRowIndex, nEndCol - (nTempEndCol-1), nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;
                #endregion
                #region Report Data

                if (oSalesComissionRegister.ReportLayout == EnumReportLayout.Month_Wise)
                {
                    #region column title
                    nTempCol = 1;
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Month"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

          
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    int nExportPIID = 0; double nGrandTotalAmount = 0, nGrandTotalQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
                    foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.PIDateMonthSt; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = Global.MillionFormatActualDigit(oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                        nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                 
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 2]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.PartyWise)
                {
                    #region Data (Paty Wise)
                    double nTotalQty = 0, nGrandTotalQty = 0, nMonthWiseQty = 0, nMonthWiseAmount = 0, nGrandTotalAmount = 0; int nTableRow = 0;
                    DateTime sPIDate = DateTime.Now;
                    foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
                    {

                        if (sPIDate != oItem.PIDateMonth)
                        {
                            if (nTableRow > 0)
                            {

                                #region Month Wise Total
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 4]; cell.Value = "Month Wise Total"; cell.Style.Font.Bold = true; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = Global.MillionFormatActualDigit(nMonthWiseQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nMonthWiseAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nMonthWiseQty = 0; nMonthWiseAmount = 0;  nTableRow = 0;
                            }
                            #region Header

                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 6]; cell.Value = "Month Name : " + oItem.PIDateMonthSt; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;

                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Customer Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Export LC No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex++;
                            #endregion
                            #endregion
                            nSL = 0;
                        }

                        nTableRow++;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.BuyerName; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        
                        sPIDate = oItem.PIDateMonth;
                        nMonthWiseQty = nMonthWiseQty + oItem.Qty;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;

                        nMonthWiseAmount = nMonthWiseAmount + oItem.Amount;
                        nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region Month Wise Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 4]; cell.Value = "Month Wise Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = Global.MillionFormatActualDigit(nMonthWiseQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nMonthWiseAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion

                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 4]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.PIWise)
                {
                    #region column title
                    nTempCol = 1;
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Customer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                     double nGrandTotalAmount = 0, nGrandTotalQty = 0;
                    foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + oItem.IssueDateSt; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = "" + oItem.BuyerName; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = "" + oItem.MUSymbol; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                        nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }


                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 5]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.PI_Wise_Details)
                {
                    #region Header Row
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Staus"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Issue Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    
                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion
                    
                    #region Data (Pi Wise Details)
                    int nExportPIID = 0; double nTotalQty = 0, nTotalAmount = 0, nGrandTotalQty = 0, nGrandTotalAmount = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
                    
                    foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
                    {
                        if (oItem.ExportPIID != nExportPIID)
                        {
                            if (nTableRow > 0)
                            {
                                if(nTotalQty>0)
                                {
                                    #region SubTotal
                                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(nTotalAmount); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                    #endregion

                                    nTableRow = 0;
                                    nTotalQty = 0;
                                    nTotalAmount = 0;
                                }
                            }
                            nTableRow++;
                            nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

                            nRowSpan = _oSalesComissionRegisters.Where(ChallanR => ChallanR.ExportPIID == oItem.ExportPIID).ToList().Count;
                            nTempRowIndex = nRowIndex + nRowSpan;
                            nSL++;
                            cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.PIStatusSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.IssueDateSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nExportPIID = oItem.ExportPIID;
                        nTotalQty = nTotalQty + oItem.Qty;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                        nTotalAmount = nTotalAmount + oItem.Amount;
                        nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region SubTotal
                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(nTotalAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                   
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 9]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.SalesCommissionDetails)
                {
                    #region Header Row
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "PI Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex,9]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    

                    cell = sheet.Cells[nRowIndex,10]; cell.Value = "Delivery Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Delivery Qty"; cell.Style.Font.Bold = true;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = "LC Rcv Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = "LC Value"; cell.Style.Font.Bold = true;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Acceptance.Sub. Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Acceptance.Rcv. Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Doc.Rcv. Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = "Maturity Rcv.Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = "Realization Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 21]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Data (Pi Wise Details)
                    double nTotalQty = 0, nTotalAmount = 0; int nExportPIID = 0, nExportLCID = -1, nTableRow = 0, nRowSpan = 0, nRowSpanForLC = 0, nTempRowIndex = 0, nTempRowIndexForPI = 0;

                    foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
                    {
                        if (oItem.ExportLCID != nExportLCID)
                        {
                            if (nTableRow > 0)
                            {

                                #region Total Print
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 5]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7, nRowIndex, 15]; cell.Value = "Total L/C Received:"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 17, nRowIndex, 21]; cell.Value = " "; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTableRow = 0;
                                nTotalQty = 0;
                                nTotalAmount = 0;
                                nRowIndex++;
                            }
                            nTableRow++;
                            if(nTotalQty!=0)
                            {
                                nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print
                            }
                            nRowSpanForLC = _oSalesComissionRegisters.Where(ChallanR => ChallanR.ExportLCID == oItem.ExportLCID).ToList().Count - 1;

                            nTempRowIndex = nRowIndex + nRowSpanForLC;
                            nSL++;
                            cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = ""+nSL; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        if (nExportPIID != oItem.ExportPIID)
                        {
                            nRowSpan= _oSalesComissionRegisters.Where(ChallanR => ChallanR.ExportPIID == oItem.ExportPIID).ToList().Count-1;
                            if (nTotalQty != 0)
                            {
                                nRowIndex = nTempRowIndexForPI > 0 ? nTempRowIndexForPI + 1 : nRowIndex;//REset fo next Row print
                            }
                            nTempRowIndexForPI = nRowIndex + nRowSpan;

                            cell = sheet.Cells[nRowIndex, 2, nTempRowIndexForPI, 2]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3, nTempRowIndexForPI, 3]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4, nTempRowIndexForPI, 4]; cell.Value = oItem.IssueDateSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(oItem.UnitPrice); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = false;//challan info
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = false;//challan info
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = false;//challan info
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(oItem.Balance); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (nExportLCID != oItem.ExportLCID)
                        {

                            cell = sheet.Cells[nRowIndex, 13, nTempRowIndex, 13]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 14, nTempRowIndex, 14]; cell.Value = oItem.LCRecivedDateInString; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 15, nTempRowIndex, 15]; cell.Value = oItem.LCValue; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            List<ExportBill> oExportBills = new List<ExportBill>();
                            oExportBills = _oExportBills.Where(x => x.ExportLCID == oItem.ExportLCID).ToList();

                            foreach (ExportBill BItem in oExportBills)
                            {

                                cell = sheet.Cells[nRowIndex, 16, nTempRowIndex, 16]; cell.Value = BItem.SendToPartySt; cell.Style.Font.Bold = false; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 17, nTempRowIndex, 17]; cell.Value = BItem.AcceptanceDateStr; cell.Style.Font.Bold = false; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 18, nTempRowIndex, 18]; cell.Value = BItem.AmountSt; cell.Style.Font.Bold = false; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 19, nTempRowIndex, 19]; cell.Value = BItem.MaturityReceivedDateSt; cell.Style.Font.Bold = false; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 20, nTempRowIndex, 20]; cell.Value = BItem.RelizationDateSt; cell.Style.Font.Bold = false; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 21, nTempRowIndex, 21]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                        }

                        nExportLCID = oItem.ExportLCID;
                        nExportPIID = oItem.ExportPIID;
                        nTotalQty = nTotalQty + oItem.Qty;
                        nTotalAmount = nTotalAmount + oItem.LCValue;   
                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #region Total Print
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 5]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 15]; cell.Value = "Total L/C Received:"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17, nRowIndex, 21]; cell.Value = " "; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion
                    #endregion
                }
                else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.TOP_Sheet)
                {
                    #region column title
                    nTempCol = 1;
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Customer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Month"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    double nGrandTotalAmount = 0, nGrandTotalQty = 0; DateTime dLCOpenDateMonth = DateTime.MinValue;
                    foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.BuyerName; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        
                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = Global.MillionFormatActualDigit(oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (dLCOpenDateMonth != oItem.LCOpenDateMonth)
                        {
                            int nRowSpan = _oSalesComissionRegisters.Where(x => x.LCOpenDateMonth == oItem.LCOpenDateMonth).Count()-1;
                            cell = sheet.Cells[nRowIndex, 5, (nRowIndex + nRowSpan), 5]; cell.Value = "" + oItem.LCOpenDateMonthSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            dLCOpenDateMonth = oItem.LCOpenDateMonth;
                        }
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                        nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }
                    #region Grand Total
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 2]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                #endregion
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SalesComissionRegister(Bulk).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        #endregion
        #endregion
        
        #region Support Functions
        public int GetRowSpan(int nID)
        {
            int nCount = _oSalesComissionRegisters.Where(ChallanR => ChallanR.ExportLCID == nID).ToList().Count - 1;
            List<SalesComissionRegister> oSalesComissionRegisters = new List<SalesComissionRegister>();
            oSalesComissionRegisters = _oSalesComissionRegisters.Where(ChallanR => ChallanR.ExportLCID == nID).ToList();
            foreach(SalesComissionRegister oItem in oSalesComissionRegisters)
            {
                nCount += oItem.ChallanInfo.Split(',').Count()>1?oItem.ChallanInfo.Split(',').Count():0;
            }
            return nCount;
        }
        private string GetSQL(SalesComissionRegister oSalesComissionRegister)
        {
            _sDateRange = "";
            string sSearchingData = oSalesComissionRegister.SearchingData;
            EnumCompareOperator ePIIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPIIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPIIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "", sTempQuyery = "" ;

            #region BusinessUnit
            if (oSalesComissionRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LC.BUID =" + oSalesComissionRegister.BUID.ToString();
            }
            #endregion

            #region Buyer Name
            if (oSalesComissionRegister.BuyerName != null && oSalesComissionRegister.BuyerName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LC.ApplicantID IN(" + oSalesComissionRegister.BuyerName + ")";
            }
            #endregion

            #region Mother Buyer Name
            if (oSalesComissionRegister.MotherBuyerID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LC.MotherBuyerID =" + oSalesComissionRegister.MotherBuyerID;
            }
            #endregion

            #region Issue Date
            if (ePIIssueDate != EnumCompareOperator.None)
            {
                if (ePIIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LC.OpeningDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Issue Date @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LC.OpeningDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Issue Date Not Equal @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LC.OpeningDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Issue Date Greater Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LC.OpeningDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Issue Date Smaller Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LC.OpeningDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Issue Date Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LC.OpeningDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Issue Date NOT Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Default Clause
            sWhereCluse += "))";
            #endregion

            #region Report Layout
            if (oSalesComissionRegister.ReportLayout == EnumReportLayout.Month_Wise)
            {
                sSQLQuery = ""; sGroupBy = " GROUP BY PIDateMonth "; sOrderBy = "";
                //sSQLQuery = "SELECT PIDateMonth, SUM(Qty) AS Qty, SUM(Amount) AS Amount  FROM View_SalesComissionRegister";
                sSQLQuery = "SELECT PIDateMonth, SUM(Qty) AS Qty, SUM(Amount) AS Amount  FROM View_SalesComissionRegister AS GG WHERE GG.ExportPIID In (SELECT LCMAP.ExportPIID FROM ExportPILCMapping AS LCMAP WHERE LCMAP.ExportLCID IN (SELECT LC.ExportLCID FROM View_ExportLC AS LC ";
                sOrderBy = " ORDER BY  PIDateMonth ASC";
            }
            
            else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.PartyWise)
            {
                sSQLQuery = ""; sGroupBy = " GROUP BY PIDateMonth, BuyerID, BuyerName"; sOrderBy = "";
                //sSQLQuery = "SELECT PIDateMonth, BuyerID, BuyerName, SUM(Qty) AS Qty, SUM(Amount) AS Amount FROM View_SalesComissionRegister ";
                sSQLQuery = "SELECT PIDateMonth, BuyerID, BuyerName, SUM(Qty) AS Qty, SUM(Amount) AS Amount FROM View_SalesComissionRegister AS GG WHERE GG.ExportPIID In (SELECT LCMAP.ExportPIID FROM ExportPILCMapping AS LCMAP WHERE LCMAP.ExportLCID IN (SELECT LC.ExportLCID FROM View_ExportLC AS LC ";
                sOrderBy = " ORDER BY  PIDateMonth, BuyerID ASC";
            }
            else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.PIWise)
            {
                sSQLQuery = ""; sGroupBy = " GROUP BY ExportPIID, IssueDate, PINo, BuyerID, BuyerName"; sOrderBy = "";
                //sSQLQuery = "SELECT ExportPIID, PINo, IssueDate, BuyerID, BuyerName, SUM(Qty) AS Qty, SUM(Amount) AS Amount FROM View_SalesComissionRegister ";
                sSQLQuery = "SELECT ExportPIID, PINo, IssueDate, BuyerID, BuyerName, SUM(Qty) AS Qty, SUM(Amount) AS Amount FROM View_SalesComissionRegister AS GG WHERE GG.ExportPIID In (SELECT LCMAP.ExportPIID FROM ExportPILCMapping AS LCMAP WHERE LCMAP.ExportLCID IN (SELECT LC.ExportLCID FROM View_ExportLC AS LC ";
                sOrderBy = " ORDER BY  ExportPIID, IssueDate, BuyerID ASC";
            }
            else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.PI_Wise_Details)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_SalesComissionRegister AS GG WHERE GG.ExportPIID In (SELECT LCMAP.ExportPIID FROM ExportPILCMapping AS LCMAP WHERE LCMAP.ExportLCID IN (SELECT LC.ExportLCID FROM View_ExportLC AS LC ";
                sOrderBy = " ORDER BY  ExportPIID ASC";
            }
            else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.SalesCommissionDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_SalesComissionRegister AS GG WHERE GG.ExportPIID In (SELECT LCMAP.ExportPIID FROM ExportPILCMapping AS LCMAP WHERE LCMAP.ExportLCID IN (SELECT LC.ExportLCID FROM View_ExportLC AS LC  ";
                sOrderBy = " ORDER BY  ExportLCID,ExportPIID, BuyerID ASC";
            }
            else if (oSalesComissionRegister.ReportLayout == EnumReportLayout.TOP_Sheet)
            {
                sSQLQuery = ""; sGroupBy = " GROUP BY LCOpenDateMonth, BuyerID, BuyerName"; sOrderBy = "";
                sSQLQuery = "SELECT  LCOpenDateMonth, BuyerID, BuyerName, SUM(Qty) AS Qty, SUM(Amount) AS Amount FROM View_SalesComissionRegister AS GG WHERE GG.ExportPIID In (SELECT LCMAP.ExportPIID FROM ExportPILCMapping AS LCMAP WHERE LCMAP.ExportLCID IN (SELECT LC.ExportLCID FROM View_ExportLC AS LC ";
                sOrderBy = " ORDER BY LCOpenDateMonth, BuyerID DESC";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
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
        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyImageTitle.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
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
    }
}

