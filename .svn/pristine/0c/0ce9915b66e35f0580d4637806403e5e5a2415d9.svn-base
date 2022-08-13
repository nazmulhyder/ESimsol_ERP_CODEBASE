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
    public class DeliveryOrderRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        List<DeliveryOrderRegister> _oDeliveryOrderRegisters = new List<DeliveryOrderRegister>();        

        #region Actions
        public ActionResult ViewDeliveryOrderRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            DeliveryOrderRegister oDeliveryOrderRegister = new DeliveryOrderRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApproveBy FROM DeliveryOrder AS MM WHERE ISNULL(MM.ApproveBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DO_Wise  || (EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.DeliveryOrderStates = EnumObject.jGets(typeof(EnumDOStatus));
            return View(oDeliveryOrderRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(DeliveryOrderRegister oDeliveryOrderRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDeliveryOrderRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintDeliveryOrderRegister(double ts)
        {
            DeliveryOrderRegister oDeliveryOrderRegister = new DeliveryOrderRegister();
            try
            {
                _sErrorMesage = "";
                _oDeliveryOrderRegisters = new List<DeliveryOrderRegister>();
                oDeliveryOrderRegister = (DeliveryOrderRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oDeliveryOrderRegister);
                _oDeliveryOrderRegisters = DeliveryOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oDeliveryOrderRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDeliveryOrderRegisters = new List<DeliveryOrderRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oDeliveryOrderRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptDeliveryOrderRegisters oReport = new rptDeliveryOrderRegisters();
                byte[] abytes = oReport.PrepareReport(_oDeliveryOrderRegisters, oCompany, oDeliveryOrderRegister.ReportLayout, _sDateRange);
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
        public void ExportToExcelDeliveryOrderRegister(double ts)
        {
            DeliveryOrderRegister oDeliveryOrderRegister = new DeliveryOrderRegister();
            _oDeliveryOrderRegisters = new List<DeliveryOrderRegister>();
            oDeliveryOrderRegister = (DeliveryOrderRegister)Session[SessionInfo.ParamObj];
            string sSQL = this.GetSQL(oDeliveryOrderRegister);
            _oDeliveryOrderRegisters = DeliveryOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            if (_oDeliveryOrderRegisters.Count <= 0)
            {
                _sErrorMesage = "There is no data for print!";
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(_oDeliveryOrderRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 20, nTempCol = 2;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Delivery Order Register");
                string sReportHeader = "Delivery Order Register";
                #region Report Body & Header
                if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.DO_Wise)
                {

                }
                else if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                {
                    sReportHeader = "Date Wise Delivery Order Register(Details)";

                }
                else if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
                {
                    sReportHeader = "Party Wise Delivery Order Register(Details)";

                }
                else if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    sReportHeader = "Product Wise Delivery Order Register";

                }
                sheet.Column(nTempCol).Width = 30; nTempCol++;//sl      
                sheet.Column(nTempCol).Width = 50; nTempCol++;//DO no
                sheet.Column(nTempCol).Width = 30; nTempCol++;//DO date or status
                sheet.Column(nTempCol).Width = 30; nTempCol++;//Buyer Name or DO NO
                sheet.Column(nTempCol).Width = 20; nTempCol++;//DO no
                sheet.Column(nTempCol).Width = 20; nTempCol++;//PI No
                sheet.Column(nTempCol).Width = 20; nTempCol++;//LC no
                sheet.Column(nTempCol).Width = 30; nTempCol++;//Vichle no
                sheet.Column(nTempCol).Width = 30; nTempCol++;//Code 
                if (oDeliveryOrderRegister.ReportLayout != EnumReportLayout.ProductWise)
                {
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Product name
                }
                sheet.Column(nTempCol).Width = 30; nTempCol++;//m unit
                sheet.Column(nTempCol).Width = 30;//qty   

                #endregion

                nEndCol = nTempCol + 1;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oBU.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[nRowIndex, nEndCol - 5, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oBU.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[nRowIndex, nEndCol - 5, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data

                if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.DO_Wise)
                {
                    #region column title
                    nTempCol = 1;
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "DO No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "DO Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "DO  No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Delivery Point"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    int nDeliveryOrderID = 0; double nTotalQty = 0, nGrandTotalQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
                    foreach (DeliveryOrderRegister oItem in _oDeliveryOrderRegisters)
                    {

                        if (oItem.DeliveryOrderID != nDeliveryOrderID)
                        {

                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTableRow = 0;
                                nTotalQty = 0;
                            }
                            nTableRow++;
                            nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

                            nRowSpan = _oDeliveryOrderRegisters.Where(DOR => DOR.DeliveryOrderID == oItem.DeliveryOrderID).ToList().Count;
                            nTempRowIndex = nRowIndex + nRowSpan;
                            nSL++;
                            cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.DONo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.DODateSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7, nTempRowIndex, 7]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8, nTempRowIndex, 8]; cell.Value = oItem.DeliveryPoint; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nDeliveryOrderID = oItem.DeliveryOrderID;
                        nTotalQty = nTotalQty + oItem.Qty;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region SubTotal

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                {
                    #region Data (Date wise)
                    int nDeliveryOrderID = 0; double nTotalQty = 0, nGrandTotalQty = 0, nDateWiseQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
                    DateTime dDODate = DateTime.MinValue;
                    foreach (DeliveryOrderRegister oItem in _oDeliveryOrderRegisters)
                    {

                        if (dDODate.ToString("dd MMM yyyy") != oItem.DODate.ToString("dd MMM yyyy"))
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Date Wise Total
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Date Wise Total"; cell.Style.Font.Bold = true; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nDateWiseQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTotalQty = 0; nDateWiseQty = 0; nTempRowIndex = 0; nTableRow = 0;
                            }
                            #region Header

                            #region Blank Row
                            nRowIndex++;
                            #endregion


                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 12]; cell.Value = "DO Date @ " + oItem.DODateSt; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;

                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "DO No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "DO Status"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "DO  No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Delivery Point"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex++;
                            #endregion
                            #endregion

                            nSL = 0;
                        }
                        if (oItem.DeliveryOrderID != nDeliveryOrderID)
                        {

                            if (nTableRow > 0)
                            {

                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTableRow = 0;
                                nTotalQty = 0;
                            }
                            nTableRow++;
                            nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

                            nRowSpan = _oDeliveryOrderRegisters.Where(DOR => DOR.DeliveryOrderID == oItem.DeliveryOrderID).ToList().Count;
                            nTempRowIndex = nRowIndex + nRowSpan;
                            nSL++;
                            cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.DONo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.DOStatusSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7, nTempRowIndex, 7]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8, nTempRowIndex, 8]; cell.Value = oItem.DeliveryPoint; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nDeliveryOrderID = oItem.DeliveryOrderID;
                        dDODate = oItem.DODate;
                        nTotalQty = nTotalQty + oItem.Qty;
                        nDateWiseQty = nDateWiseQty + oItem.Qty;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region SubTotal
                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Date Wise Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Date Wise Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nDateWiseQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
                {
                    #region Data (Paty Wise Details)
                    int nDeliveryOrderID = 0; double nTotalQty = 0, nGrandTotalQty = 0, nPartyWiseQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
                    string sBuyerName = "";
                    foreach (DeliveryOrderRegister oItem in _oDeliveryOrderRegisters)
                    {

                        if (sBuyerName != oItem.ContractorName)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Date Wise Total
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Party Wise Total"; cell.Style.Font.Bold = true; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nPartyWiseQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTotalQty = 0; nPartyWiseQty = 0; nTempRowIndex = 0; nTableRow = 0;
                            }
                            #region Header

                            #region Blank Row
                            nRowIndex++;
                            #endregion


                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 12]; cell.Value = "Party Name : " + oItem.ContractorName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;

                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "DO No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "DO Status"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "DO Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "DO  No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Delivery Point"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex++;
                            #endregion
                            #endregion

                            nSL = 0;
                        }
                        if (oItem.DeliveryOrderID != nDeliveryOrderID)
                        {

                            if (nTableRow > 0)
                            {

                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTableRow = 0;
                                nTotalQty = 0;
                            }
                            nTableRow++;
                            nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

                            nRowSpan = _oDeliveryOrderRegisters.Where(DOR => DOR.DeliveryOrderID == oItem.DeliveryOrderID).ToList().Count;
                            nTempRowIndex = nRowIndex + nRowSpan;
                            nSL++;
                            cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.DONo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.DOStatusSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.DODateSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7, nTempRowIndex, 7]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8, nTempRowIndex, 8]; cell.Value = oItem.DeliveryPoint; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nDeliveryOrderID = oItem.DeliveryOrderID;
                        sBuyerName = oItem.ContractorName;
                        nTotalQty = nTotalQty + oItem.Qty;
                        nPartyWiseQty = nPartyWiseQty + oItem.Qty;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region SubTotal
                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Date Wise Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Party Wise Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nPartyWiseQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    #region Data (Product Wise)
                    double nTotalQty = 0, nGrandTotalQty = 0; int nTableRow = 0;
                    string sProductName = "";
                    foreach (DeliveryOrderRegister oItem in _oDeliveryOrderRegisters)
                    {

                        if (sProductName != oItem.ProductName)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 10]; cell.Value = "Product Wise Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                nTotalQty = 0; nTableRow = 0;
                            }
                            #region Header

                            #region Blank Row
                            nRowIndex++;
                            #endregion


                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Product Name : " + oItem.ProductName + "[" + oItem.ProductCode + "]"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true; cell.Style.Font.UnderLine = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;

                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "DO No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "DO Status"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "DO  No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Delivery Point"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "DO Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
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
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.DONo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.DOStatusSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.DeliveryPoint; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.DODateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sProductName = oItem.ProductName;
                        nTotalQty = nTotalQty + oItem.Qty;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region SubTotal
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 10]; cell.Value = "Product Wise Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion

                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 10]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
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
                Response.AddHeader("content-disposition", "attachment; filename=DeliveryOrderRegister(Bulk).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        #endregion

        #endregion



        #region Support Functions
        

        private string GetSQL(DeliveryOrderRegister oDeliveryOrderRegister)
        {
            _sDateRange = "";
            string sSearchingData = oDeliveryOrderRegister.SearchingData;
            EnumCompareOperator ncboDODate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dDoIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dDoIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eDeliveryDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dDeliveryStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dDeliveryEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
            
            string sDONo = sSearchingData.Split('~')[6];
            string sExportPINo = sSearchingData.Split('~')[7];

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oDeliveryOrderRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oDeliveryOrderRegister.BUID.ToString();
            }
            #endregion

            #region DeliveryOrderNo
            if (oDeliveryOrderRegister.DONo != null && oDeliveryOrderRegister.DONo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " DONo LIKE'%" + oDeliveryOrderRegister.DONo + "%'";
            }
            #endregion

            #region ApproveBy
            if (oDeliveryOrderRegister.ApproveBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApproveBy =" + oDeliveryOrderRegister.ApproveBy.ToString();
            }
            #endregion

            #region DOStatus
            if (oDeliveryOrderRegister.DOStatusInInt>-1)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " DOStatus =" + (oDeliveryOrderRegister.DOStatusInInt).ToString();
            }
            #endregion

            #region Supplier
            if (oDeliveryOrderRegister.ContractorName != null && oDeliveryOrderRegister.ContractorName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ContractorID IN(" + oDeliveryOrderRegister.ContractorName + ")";
            }
            #endregion

            #region Product
            if (oDeliveryOrderRegister.ProductName != null && oDeliveryOrderRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oDeliveryOrderRegister.ProductName + ")";
            }
            #endregion

            #region do Date
            if (ncboDODate != EnumCompareOperator.None)
            {
                if (ncboDODate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDoIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "DO Date @ " + dDoIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDoIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "DO Date Not Equal @ " + dDoIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDoIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "DO Date Greater Then @ " + dDoIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDoIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "DO Date Smaller Then @ " + dDoIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDoIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDoIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "DO Date Between " + dDoIssueStartDate.ToString("dd MMM yyyy") + " To " + dDoIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (ncboDODate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDoIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDoIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "DO Date NOT Between " + dDoIssueStartDate.ToString("dd MMM yyyy") + " To " + dDoIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Delivery Date
            if (eDeliveryDate != EnumCompareOperator.None)
            {
                if (eDeliveryDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDeliveryStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDeliveryDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDeliveryStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDeliveryDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDeliveryStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDeliveryDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDeliveryStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDeliveryDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDeliveryStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDeliveryEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDeliveryDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDeliveryStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDeliveryEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion


            #region PINo
            if (sExportPINo != null && sExportPINo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PINo  LIKE '%" + sExportPINo + "%'";
            }
            #endregion

            #region Report Layout
            if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.DO_Wise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_DeliveryOrderRegister ";
                sOrderBy = " ORDER BY  DODate, DeliveryOrderID, DeliveryOrderDetailID ASC";
            }
            
            else if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_DeliveryOrderRegister ";
                sOrderBy = " ORDER BY  DODate, DeliveryOrderID, DeliveryOrderDetailID ASC";
            }
            else if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_DeliveryOrderRegister ";
                sOrderBy = " ORDER BY  ContractorID, DeliveryOrderID, DeliveryOrderDetailID ASC";
            }
            else if (oDeliveryOrderRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_DeliveryOrderRegister ";
                sOrderBy = " ORDER BY  ProductName ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_DeliveryOrderRegister ";
                sOrderBy = " ORDER BY DODate, DeliveryOrderID, DeliveryOrderDetailID ASC";
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

