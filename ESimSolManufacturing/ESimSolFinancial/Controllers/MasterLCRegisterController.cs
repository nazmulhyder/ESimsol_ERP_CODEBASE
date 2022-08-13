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
    public class MasterLCRegisterController : Controller
    {
        #region Declaration
        string _sDateRange = "";
        string _sErrorMesage = "";
        MasterLC _oMasterLC = new MasterLC();
        List<MasterLC> _oMasterLCs = new List<MasterLC>();
        #endregion

        #region Actions
        public ActionResult ViewMasterLCRegister(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            MasterLC oMasterLC = new MasterLC();

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.None || (EnumReportLayout)oItem.id == EnumReportLayout.Month_Wise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.LCStatus = EnumObject.jGets(typeof(EnumLCStatus));
            return View(oMasterLC);
        }
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(MasterLC oMasterLC)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oMasterLC);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Print(double ts)
        {
            MasterLC oMasterLC = new MasterLC();
            try
            {
                _sErrorMesage = "";
                _oMasterLCs = new List<MasterLC>();
                oMasterLC = (MasterLC)Session[SessionInfo.ParamObj];

                string sSQL = this.GetSQL(oMasterLC);
                _oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (_oMasterLCs.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oMasterLCs = new List<MasterLC>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oMasterLCs.Max(x => x.BUID), (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptMasterLCRegisters oReport = new rptMasterLCRegisters();
                byte[] abytes = oReport.PrepareReport(_oMasterLCs, oCompany, oMasterLC.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        #endregion

        #region Excel
        public void ExportToExcel(double ts)
        {
            Company oCompany = new Company();
            MasterLC oMasterLC = new MasterLC();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oMasterLCs = new List<MasterLC>();
                oMasterLC = (MasterLC)Session[SessionInfo.ParamObj];

                string sSQL = this.GetSQL(oMasterLC);
                _oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (_oMasterLCs.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oMasterLCs = new List<MasterLC>();
                _sErrorMesage = ex.Message;
            }
            if (_sErrorMesage == "")
            {
                if (oMasterLC.ReportLayout == EnumReportLayout.PartyWise)
                {
                    ExportToExcelBuyerWise(_oMasterLCs, oCompany, _sDateRange);
                }
                else
                {
                    ExportToExcelMonthWise(_oMasterLCs, oCompany, _sDateRange);
                }
            }
        }
        public void ExportToExcelBuyerWise(List<MasterLC> _oMasterLCs, Company oCompany, string _sDateRange)
        {
            #region Print XL

            int nRowIndex = 2, nStartCol = 2, nEndCol = 12, nColumn = 1, nCount = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ExportMLCRegister");
                sheet.Name = "Export MLC Register";
                sheet.Column(++nColumn).Width = 10; //SL No
                sheet.Column(++nColumn).Width = 15; //MasterLC No
                sheet.Column(++nColumn).Width = 40; //Buyer Name
                sheet.Column(++nColumn).Width = 45; //Advice Bank
                sheet.Column(++nColumn).Width = 20; //LC Open Date
                sheet.Column(++nColumn).Width = 20; //Shipment Date
                sheet.Column(++nColumn).Width = 20; //Expire Date
                sheet.Column(++nColumn).Width = 20; //Qty
                sheet.Column(++nColumn).Width = 25; //LC Amount
                sheet.Column(++nColumn).Width = 25; //OrderTag Amount
                sheet.Column(++nColumn).Width = 25; //Yet To OrderTag

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol + 1, nRowIndex, nEndCol - 3]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Export MLC Register(Buyer wise)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, nStartCol + 1, nRowIndex, nEndCol - 3]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Header
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Advice Bank"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Open Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Expire Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "OrderTag Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yet To OrderTag"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                double nPartyWiseTotalQty = 0; double nPartyWiseTotalOrderTag = 0; double nPartyWiseTotalYetToOrderTag = 0; double nPartyWiseTotalLCValue = 0;
                double nLayoutWiseTotalQty = 0; double nLayoutWiseTotalOrderTag = 0; double nLayoutWiseTotalYetToOrderTag = 0; double nLayoutWiseTotalLCValue = 0;
                string sApplicantName = "";
                foreach (MasterLC oItem in _oMasterLCs)
                {
                    if (oItem.ApplicantName != sApplicantName)
                    {
                        if (nCount > 0)
                        {
                            #region Buyer Wise Total
                            cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                            cell.Value = "Buyer Wise Total :"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nPartyWiseTotalQty; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nPartyWiseTotalLCValue; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = nPartyWiseTotalOrderTag; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nPartyWiseTotalYetToOrderTag; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex++;
                            #endregion
                        }
                        
                        #region Buyer Name Assign
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "@Buyer Name : "+ oItem.ApplicantName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        nRowIndex++;
                        #endregion

                        sApplicantName = oItem.ApplicantName;
                        nCount = 0; nPartyWiseTotalQty = 0; nPartyWiseTotalOrderTag = 0; nPartyWiseTotalYetToOrderTag = 0; nPartyWiseTotalLCValue = 0;
                    }

                    nCount++;
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ApplicantName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.AdviceBankName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LastDateofShipmentSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ExpireDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LCQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LCValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.OrderTagAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.YetToOrderTagAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;

                    nPartyWiseTotalQty = nPartyWiseTotalQty + oItem.LCQty;
                    nPartyWiseTotalOrderTag = nPartyWiseTotalOrderTag + oItem.OrderTagAmount;
                    nPartyWiseTotalYetToOrderTag = nPartyWiseTotalYetToOrderTag + oItem.YetToOrderTagAmount;
                    nPartyWiseTotalLCValue = nPartyWiseTotalLCValue + oItem.LCValue;

                    nLayoutWiseTotalQty = nLayoutWiseTotalQty + oItem.LCQty;
                    nLayoutWiseTotalOrderTag = nLayoutWiseTotalOrderTag + oItem.OrderTagAmount;
                    nLayoutWiseTotalYetToOrderTag = nLayoutWiseTotalYetToOrderTag + oItem.YetToOrderTagAmount;
                    nLayoutWiseTotalLCValue = nLayoutWiseTotalLCValue + oItem.LCValue;
                }
                #endregion

                #region Buyer Wise Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                cell.Value = "Buyer Wise Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nPartyWiseTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nPartyWiseTotalLCValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = nPartyWiseTotalOrderTag; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nPartyWiseTotalYetToOrderTag; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Grand Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nLayoutWiseTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nLayoutWiseTotalLCValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = nLayoutWiseTotalOrderTag; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nLayoutWiseTotalYetToOrderTag; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportMLCRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        public void ExportToExcelMonthWise(List<MasterLC> _oMasterLCs, Company oCompany, string _sDateRange)
        {
            #region Print XL

            int nRowIndex = 2, nStartCol = 2, nEndCol = 12, nColumn = 1, nCount = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ExportMLCRegister");
                sheet.Name = "Export MLC Register";
                sheet.Column(++nColumn).Width = 10; //SL No
                sheet.Column(++nColumn).Width = 15; //MasterLC No
                sheet.Column(++nColumn).Width = 40; //Buyer Name
                sheet.Column(++nColumn).Width = 45; //Advice Bank
                sheet.Column(++nColumn).Width = 20; //LC Open Date
                sheet.Column(++nColumn).Width = 20; //Shipment Date
                sheet.Column(++nColumn).Width = 20; //Expire Date
                sheet.Column(++nColumn).Width = 20; //Qty
                sheet.Column(++nColumn).Width = 25; //LC Amount
                sheet.Column(++nColumn).Width = 25; //OrderTag Amount
                sheet.Column(++nColumn).Width = 25; //Yet To OrderTag

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol + 1, nRowIndex, nEndCol - 3]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Export MLC Register(Month wise)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, nStartCol + 1, nRowIndex, nEndCol - 3]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Header
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Advice Bank"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Open Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Expire Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "OrderTag Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yet To OrderTag"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                double nMonthWiseTotalQty = 0; double nMonthWiseTotalOrderTag = 0; double nMonthWiseTotalYetToOrderTag = 0; double nMonthWiseTotalLCValue = 0;
                double nLayoutWiseTotalQty = 0; double nLayoutWiseTotalOrderTag = 0; double nLayoutWiseTotalYetToOrderTag = 0; double nLayoutWiseTotalLCValue = 0;
                string sMonthName = "";
                foreach (MasterLC oItem in _oMasterLCs)
                {
                    if (oItem.MasterLCDate.ToString("MMM/yyyy") != sMonthName)
                    {
                        if (nCount > 0)
                        {
                            #region Month Wise Total
                            cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                            cell.Value = "Month Wise Total :"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nMonthWiseTotalQty; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nMonthWiseTotalLCValue; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = nMonthWiseTotalOrderTag; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nMonthWiseTotalYetToOrderTag; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex++;
                            #endregion
                        }

                        #region Buyer Name Assign
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Month Name : " + oItem.MasterLCDate.ToString("MMM/yyyy"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                        #endregion

                        sMonthName = oItem.MasterLCDate.ToString("MMM/yyyy");
                        nCount = 0; nMonthWiseTotalQty = 0; nMonthWiseTotalOrderTag = 0; nMonthWiseTotalYetToOrderTag = 0; nMonthWiseTotalLCValue = 0;
                    }

                    nCount++;
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ApplicantName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.AdviceBankName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LastDateofShipmentSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ExpireDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LCQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LCValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.OrderTagAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.YetToOrderTagAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;

                    nMonthWiseTotalQty = nMonthWiseTotalQty + oItem.LCQty;
                    nMonthWiseTotalOrderTag = nMonthWiseTotalOrderTag + oItem.OrderTagAmount;
                    nMonthWiseTotalYetToOrderTag = nMonthWiseTotalYetToOrderTag + oItem.YetToOrderTagAmount;
                    nMonthWiseTotalLCValue = nMonthWiseTotalLCValue + oItem.LCValue;

                    nLayoutWiseTotalQty = nLayoutWiseTotalQty + oItem.LCQty;
                    nLayoutWiseTotalOrderTag = nLayoutWiseTotalOrderTag + oItem.OrderTagAmount;
                    nLayoutWiseTotalYetToOrderTag = nLayoutWiseTotalYetToOrderTag + oItem.YetToOrderTagAmount;
                    nLayoutWiseTotalLCValue = nLayoutWiseTotalLCValue + oItem.LCValue;
                }
                #endregion

                #region Month Wise Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                cell.Value = "Month Wise Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nMonthWiseTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol -2]; cell.Value = nMonthWiseTotalLCValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = nMonthWiseTotalOrderTag; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nMonthWiseTotalYetToOrderTag; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Grand Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nLayoutWiseTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nLayoutWiseTotalLCValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = nLayoutWiseTotalOrderTag; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nLayoutWiseTotalYetToOrderTag; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportMLCRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        #endregion

        #region Support Functions
        private string GetSQL(MasterLC oMasterLC)
        {
            _sDateRange = "";
            string sSearchingData = oMasterLC.SearchingData;
            EnumCompareOperator eLCOpenOperationTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            EnumCompareOperator eShipmentOperationTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            EnumCompareOperator eReceiveOperationTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            EnumCompareOperator eExpireOperationTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";


            #region BusinessUnit
            if (oMasterLC.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oMasterLC.BUID.ToString();
            }
            #endregion

            #region OrderRecapNo And StyleNo
            if ((oMasterLC.OrderRecapNo != null && oMasterLC.OrderRecapNo != "") || (oMasterLC.StyleNo != null && oMasterLC.StyleNo != ""))
            {
                if (oMasterLC.OrderRecapNo != null && oMasterLC.OrderRecapNo != "" && oMasterLC.StyleNo != null && oMasterLC.StyleNo != "")
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " MasterLCID IN (SELECT MasterLCID FROM View_MasterLCDetail WHERE ProformaInvoiceID IN (SELECT ProformaInvoiceID  FROM View_ProformaInvoiceDetail WHERE StyleNo LIKE '%"+ oMasterLC.StyleNo +"%' AND OrderRecapNo LIKE '%"+ oMasterLC.OrderRecapNo +"%'))";
                }
                else if (oMasterLC.OrderRecapNo != null && oMasterLC.OrderRecapNo != "" && (oMasterLC.StyleNo == null || oMasterLC.StyleNo == ""))
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " MasterLCID IN (SELECT MasterLCID FROM View_MasterLCDetail WHERE ProformaInvoiceID IN (SELECT ProformaInvoiceID  FROM View_ProformaInvoiceDetail WHERE OrderRecapNo LIKE '%"+ oMasterLC.OrderRecapNo +"%'))";
                }
                else
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " MasterLCID IN (SELECT MasterLCID FROM View_MasterLCDetail WHERE ProformaInvoiceID IN (SELECT ProformaInvoiceID  FROM View_ProformaInvoiceDetail WHERE StyleNo LIKE '%"+ oMasterLC.StyleNo +"%'))";
                }
            }
            #endregion

            #region LCStatus
            if (oMasterLC.LCStatusInInt > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LCStatus =" + oMasterLC.LCStatusInString;
            }
            #endregion

            #region AdviceBankID
            if (oMasterLC.AdviceBankID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " AdviceBankID =" + oMasterLC.AdviceBankID;
            }
            #endregion

            #region Applicant
            if (oMasterLC.Applicant != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Applicant IN (" + oMasterLC.Applicant + ")";
            }
            #endregion

            #region LCOpenDate
            if ((int)eLCOpenOperationTime > 0)
            {
                if ((int)eLCOpenOperationTime == 5 || (int)eLCOpenOperationTime == 6)
                {
                    DateTime dLCOpenStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
                    DateTime dLCOpenEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
                    DateObject.CompareDateQuery(ref sWhereCluse, " MasterLCDate ", (int)eLCOpenOperationTime, dLCOpenStartDate, dLCOpenEndDate);
                }
                else
                {
                    DateTime dLCOpenStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
                    DateTime dLCOpenEndDate = DateTime.Now;
                    DateObject.CompareDateQuery(ref sWhereCluse, " MasterLCDate ", (int)eLCOpenOperationTime, dLCOpenStartDate, dLCOpenEndDate);
                }
            }
            #endregion

            #region ShipmentDate
            if ((int)eShipmentOperationTime > 0)
            {
                if ((int)eShipmentOperationTime == 5 || (int)eShipmentOperationTime == 6)
                {
                    DateTime dShipmentStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
                    DateTime dShipmentEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
                    DateObject.CompareDateQuery(ref sWhereCluse, " LastDateOfShipment ", (int)eShipmentOperationTime, dShipmentStartDate, dShipmentEndDate);
                }
                else
                {
                    DateTime dShipmentStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
                    DateTime dShipmentEndDate = DateTime.Now;
                    DateObject.CompareDateQuery(ref sWhereCluse, " LastDateOfShipment ", (int)eShipmentOperationTime, dShipmentStartDate, dShipmentEndDate);
                }
            }
            #endregion

            #region ReceiveDate
            if ((int)eReceiveOperationTime > 0)
            {
                if ((int)eReceiveOperationTime == 5 || (int)eReceiveOperationTime == 6)
                {
                    DateTime dReceiveStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
                    DateTime dReceiveEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);
                    DateObject.CompareDateQuery(ref sWhereCluse, " ReceiveDate ", (int)eLCOpenOperationTime, dReceiveStartDate, dReceiveEndDate);
                }
                else
                {
                    DateTime dReceiveStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
                    DateTime dReceiveEndDate = DateTime.Now;
                    DateObject.CompareDateQuery(ref sWhereCluse, " ReceiveDate ", (int)eLCOpenOperationTime, dReceiveStartDate, dReceiveEndDate);
                }
            }
            #endregion

            #region ExpireDate
            if ((int)eExpireOperationTime > 0)
            {
                if ((int)eExpireOperationTime == 5 || (int)eExpireOperationTime == 6)
                {
                    DateTime dExpireStartDate = Convert.ToDateTime(sSearchingData.Split('~')[10]);
                    DateTime dExpireEndDate = Convert.ToDateTime(sSearchingData.Split('~')[11]);
                    DateObject.CompareDateQuery(ref sWhereCluse, " ExpireDate ", (int)eExpireOperationTime, dExpireStartDate, dExpireEndDate);
                }
                else
                {
                    DateTime dExpireStartDate = Convert.ToDateTime(sSearchingData.Split('~')[10]);
                    DateTime dExpireEndDate = DateTime.Now;
                    DateObject.CompareDateQuery(ref sWhereCluse, " ExpireDate ", (int)eExpireOperationTime, dExpireStartDate, dExpireEndDate);
                }
            }
            #endregion

            #region Report Layout
            if (oMasterLC.ReportLayout == EnumReportLayout.Month_Wise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_MasterLC ";
                sOrderBy = " ORDER BY MasterLCDate, MasterLCID ASC";
            }
            else if (oMasterLC.ReportLayout == EnumReportLayout.PartyWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_MasterLC ";
                sOrderBy = " ORDER BY  ApplicantName, Applicant, MasterLCID ASC";
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

        #endregion
    }
}