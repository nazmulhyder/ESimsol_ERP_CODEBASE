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

namespace ESimSolFinancial.Controllers
{
    public class ExportPIRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        List<ExportPIRegister> _oExportPIRegisters = new List<ExportPIRegister>();        

        #region Actions
        public ActionResult ViewExportPIRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ExportPIRegister oExportPIRegister = new ExportPIRegister();

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.PartyWise || (EnumReportLayout)oItem.id == EnumReportLayout.MotherBuyerWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ExportPITypes = EnumObject.jGets(typeof(EnumPIType));
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProductNatureList = EnumObject.jGets(typeof(EnumProductNature));
            ViewBag.CurrencyList = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(oExportPIRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ExportPIRegister oExportPIRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oExportPIRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintExportPIRegister(double ts)
        {
            ExportPIRegister oExportPIRegister = new ExportPIRegister();
            try
            {
                _sErrorMesage = "";
                _oExportPIRegisters = new List<ExportPIRegister>();                
                oExportPIRegister = (ExportPIRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oExportPIRegister);
                _oExportPIRegisters = ExportPIRegister.Gets(sSQL, (int)oExportPIRegister.ReportLayout,oExportPIRegister.DueOptions, (int)Session[SessionInfo.currentUserID]);
                if (_oExportPIRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oExportPIRegisters = new List<ExportPIRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oExportPIRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptExportPIRegisters oReport = new rptExportPIRegisters();
                byte[] abytes = oReport.PrepareReport(_oExportPIRegisters, oCompany, oExportPIRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public void ExportToExcelExportPIRegister(double ts)
        {
            ExportPIRegister oExportPIRegister = new ExportPIRegister();
            try
            {
                _sErrorMesage = "";
                _oExportPIRegisters = new List<ExportPIRegister>();
                oExportPIRegister = (ExportPIRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oExportPIRegister);
                _oExportPIRegisters = ExportPIRegister.Gets(sSQL, (int)oExportPIRegister.ReportLayout, oExportPIRegister.DueOptions, (int)Session[SessionInfo.currentUserID]);
                if (_oExportPIRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oExportPIRegisters = new List<ExportPIRegister>();
                _sErrorMesage = ex.Message;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(_oExportPIRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

           #region Buyer/Mother Buyer Wise
             double nGrandTotalPIAmount = 0, nGrandTotalDeliveryAmout = 0, nGrandTotalPaidByCash = 0, nGrandTotalPaidByLC = 0, nGrandTotalDueAmount = 0;
            if ((int)oExportPIRegister.ReportLayout == (int)EnumReportLayout.PartyWise || (int)oExportPIRegister.ReportLayout == (int)EnumReportLayout.MotherBuyerWise)
            {
              
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Export PI Report");
                    if ((int)oExportPIRegister.ReportLayout == (int)EnumReportLayout.PartyWise) { sheet.Name = "Export PI Report Buyer Wise"; } else { sheet.Name = "Export PI Report Mother Buyer Wise"; }

                    #region SHEET COLUMN WIDTH
                    int nColumn = 1;
                    sheet.Column(++nColumn).Width = 6;   // SL NO    
                    sheet.Column(++nColumn).Width = 20;  // PI No
                    sheet.Column(++nColumn).Width = 20;  // PI Date                 
                    sheet.Column(++nColumn).Width = 20;  // Buyer/ Mother Buyer      
                    sheet.Column(++nColumn).Width = 8;   // Currency Symbol
                    sheet.Column(++nColumn).Width = 20;  // PI Value
                    sheet.Column(++nColumn).Width = 20;  // Delivery Value
                    sheet.Column(++nColumn).Width = 15;  // Paid by cash
                    sheet.Column(++nColumn).Width = 15; // Paid By LC
                    sheet.Column(++nColumn).Width = 15; // Due Amout
                    nEndCol = nColumn;

                    #endregion

                    #region Report Header

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = (int)oExportPIRegister.ReportLayout==(int)EnumReportLayout.PartyWise?"Buyer Wise Export PI List":"Mother Buyer Wise Export PI List"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    string sStartCell = "", sEndCell = "";
                    string sBuyerName = ""; int nCount = 0; bool IsTotalPrint = false;
                    foreach (ExportPIRegister oItem in _oExportPIRegisters)
                    {
                        nCount++;
                        if ((int)oExportPIRegister.ReportLayout==(int)EnumReportLayout.PartyWise?sBuyerName != oItem.BuyerName:sBuyerName!=oItem.MotherBuyerName)//write by Mahabub:  this report print Either Buyer wise neither Mother Buyer wise.
                        {
                            if (IsTotalPrint == true)
                            {
                                #region Total
                                int nCellIndex = 6;
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nCellIndex];
                              
                                cell.Merge = true;
                                cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //PI value
                                sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nCellIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, nCellIndex);
                                cell = sheet.Cells[nRowIndex, nCellIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //Delivery value
                                sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nCellIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, nCellIndex);
                                cell = sheet.Cells[nRowIndex, nCellIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0.00)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //Paid By Cash
                                sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nCellIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, nCellIndex);
                                cell = sheet.Cells[nRowIndex, nCellIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                //Paid by LC
                                sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nCellIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, nCellIndex);
                                cell = sheet.Cells[nRowIndex, nCellIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                //Due amount
                                sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nCellIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, nCellIndex);
                                cell = sheet.Cells[nRowIndex, nCellIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion
                            }
                            nCount = 1;

                            #region Buyer/Mother Buyer Name
                            nRowIndex = nRowIndex + 2;
                            cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                            cell.Value = (int)oExportPIRegister.ReportLayout==(int)EnumReportLayout.PartyWise?"Buyer Name : " + oItem.BuyerName:"Mother Buyer Name : " + oItem.MotherBuyerName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Column Header
                            nRowIndex = nRowIndex + 1;
                            nStartRow = nRowIndex;
                            int nHeaderIndex = 1;
                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "PI Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = (int)oExportPIRegister.ReportLayout==(int)EnumReportLayout.PartyWise?"Mother Buyer":"Buyer"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Currency"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "PI Value "; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Delivery Value "; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Paid By Cash "; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Paid By LC "; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Due Amount  "; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex = nRowIndex + 1;
                            IsTotalPrint = true;
                            sBuyerName =  (int)oExportPIRegister.ReportLayout==(int)EnumReportLayout.PartyWise?oItem.BuyerName:oItem.MotherBuyerName;
                            #endregion
                        }

                        #region Data

                        int nDataIndex = 1;
                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "###0;(###0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                      
                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ExportPINo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ExportPIDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value =  (int)oExportPIRegister.ReportLayout==(int)EnumReportLayout.PartyWise?oItem.MotherBuyerName:oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.CurrencySymbol; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //PI Value
                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.PIValue; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Delivery Value
                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value =  oItem.DeliveryValue; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Paid By Cash
                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value =  oItem.PaidByCash; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Paid By LC
                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.PaidByLC; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Due Amount
                        cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.DueAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        #endregion

                        nEndRow = nRowIndex;
                        nRowIndex++;

                        //Grand total
                        nGrandTotalPIAmount = nGrandTotalPIAmount + oItem.PIValue;
                        nGrandTotalDeliveryAmout += oItem.DeliveryValue;
                        nGrandTotalPaidByCash += oItem.PaidByCash;
                        nGrandTotalPaidByLC += oItem.PaidByLC;
                        nGrandTotalDueAmount += oItem.DueAmount;
                    }

                    #region Total
                    int nTotalIndex = 6;
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nTotalIndex];
                    cell.Merge = true;
                    cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
                    //PI value
                    sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    cell = sheet.Cells[nRowIndex, nTotalIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Delivery valu
                    sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    cell = sheet.Cells[nRowIndex, nTotalIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Paid by cash
                    sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    cell = sheet.Cells[nRowIndex, nTotalIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Paid by Lc
                    sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    cell = sheet.Cells[nRowIndex, nTotalIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Due amount
                    sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    cell = sheet.Cells[nRowIndex, nTotalIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion


                    #region Grand Total Total
                    
                    nTotalIndex = 6;
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nTotalIndex];
                    cell.Merge = true;
                    cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    //PI value
                    cell = sheet.Cells[nRowIndex, ++nTotalIndex]; cell.Value = nGrandTotalPIAmount; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Delivery valu
                    cell = sheet.Cells[nRowIndex, ++nTotalIndex]; cell.Value = nGrandTotalDeliveryAmout; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Paid by cash
                    cell = sheet.Cells[nRowIndex, ++nTotalIndex]; cell.Value = nGrandTotalPaidByCash; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Paid by Lc
                    cell = sheet.Cells[nRowIndex, ++nTotalIndex]; cell.Value = nGrandTotalPaidByLC; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Due amount
                    cell = sheet.Cells[nRowIndex, ++nTotalIndex]; cell.Value = nGrandTotalDueAmount; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=BuyerWise_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
             
            }
            #endregion
            

        }
        #endregion

        #region Support Functions
        public  string GetSQL(ExportPIRegister oExportPIRegister)
        {
            _sDateRange = "";
            string sSearchingData = oExportPIRegister.SearchingData;
            EnumCompareOperator ePIIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPIIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPIIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            int nMarketingAccountID = Convert.ToInt32(sSearchingData.Split('~')[3]);

            string sSQLQuery = "SELECT ExportPIID, PINo, IssueDate, MotherBuyerID, MotherBuyerName, ContractorID, ContractorName, BUID, ProductNature, CurrencyID, Currency, Amount FROM View_ExportPI", sWhereCluse = "";

            #region Not IN  Master PI
            Global.TagSQL(ref sWhereCluse);
            sWhereCluse = sWhereCluse + " ExportPIID NOT IN (SELECT ExportPIID FROM MasterPIMapping )";
            #endregion

            #region Default status
            Global.TagSQL(ref sWhereCluse);
            sWhereCluse = sWhereCluse + " ISNULL(PIStatus,0)>" + (int)EnumPIStatus.RequestForApproved + " AND ISNULL(PIStatus,0)<" + (int)EnumPIStatus.Cancel;
            #endregion

            #region BusinessUnit
            if (oExportPIRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oExportPIRegister.BUID.ToString();
            }
            #endregion

            #region Currency
            if (oExportPIRegister.CurrencyID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " CurrencyID =" + oExportPIRegister.CurrencyID.ToString();
            }
            #endregion
            #region ProductNature
            if (oExportPIRegister.ProductNatureInt > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductNature =" + oExportPIRegister.ProductNatureInt.ToString();
            }
            #endregion

            #region ExportPINo
            if (oExportPIRegister.ExportPINo != null && oExportPIRegister.ExportPINo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PINo LIKE'%" + oExportPIRegister.ExportPINo + "%'";
            }
            #endregion

            #region ApprovedBy
            if (oExportPIRegister.DeliveryValue != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApprovedBy =" + oExportPIRegister.DeliveryValue.ToString();
            }
            #endregion

            #region Mother Buyer
            if (oExportPIRegister.MotherBuyerName != null && oExportPIRegister.MotherBuyerName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " MotherBuyerID IN(" + oExportPIRegister.MotherBuyerName + ")";
            }
            #endregion

            #region Buyer
            if (oExportPIRegister.BuyerName != null && oExportPIRegister.BuyerName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ContractorID IN(" + oExportPIRegister.BuyerName + ")";
            }
            #endregion

            #region Marketing Account
            if (nMarketingAccountID >0 )
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " MKTEmpID = " + nMarketingAccountID;
            }
            #endregion

            
            #region Product
            if (oExportPIRegister.PIType != 0 )
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PIType = " + oExportPIRegister.PIType;
            }
            #endregion

            #region PI Issue Date
            if (ePIIssueDate != EnumCompareOperator.None)
            {
                if (ePIIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Not Equal @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Greater Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Smaller Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date NOT Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region  with Due
            if (oExportPIRegister.DueOptions==1)//Due with Challan
            {
                Global.TagSQL(ref sWhereCluse);
                //sWhereCluse = sWhereCluse + " ExportPIID  IN(SELECT ExportPIID FROM ExportSC WHERE ExportSCID IN(SELECT DC.ExportSCID FROM View_DeliveryChallan AS DC WHERE DC.RefType = 1 AND  ISNULL(DC.ApproveBy,0)!=0)) AND ExportPIID NOT IN (SELECT ExportPIID FROM ExportPILCMapping) AND ExportPIID NOT IN (SELECT ExportPIID FROM SampleInvoice  WHERE  SampleInvoiceID IN (SELECT ReferenceID FROM PaymentDetail WHERE ReferenceType = " + (int)EnumSampleInvoiceType.SalesContract + "))";
                sWhereCluse = sWhereCluse + " ExportPIID  IN(SELECT ExportPIID FROM ExportSC WHERE ExportSCID IN(SELECT DC.ExportSCID FROM View_DeliveryChallan AS DC WHERE DC.RefType = 1 AND  ISNULL(DC.ApproveBy,0)!=0)) ";
            }
            else if (oExportPIRegister.DueOptions == 2)//Only Due
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PIStatus IN (" + (int)EnumPIStatus.PIIssue + "," + (int)EnumPIStatus.BindWithLC + "," + (int)EnumPIStatus.RequestForRevise+ ")";
            }
            #endregion
            sSQLQuery = sSQLQuery + sWhereCluse;
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

