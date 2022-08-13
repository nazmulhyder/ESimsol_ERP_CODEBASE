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
    public class TransferRequisitionSlipRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        List<TransferRequisitionSlipRegister> _oTransferRequisitionSlipRegisters = new List<TransferRequisitionSlipRegister>();        

        #region Actions
        public ActionResult ViewTransferRequisitionSlipRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            TransferRequisitionSlipRegister oTransferRequisitionSlipRegister = new TransferRequisitionSlipRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.AuthorisedByID FROM TransferRequisitionSlip AS MM WHERE ISNULL(MM.AuthorisedByID,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Receive User
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ReceivedByUserID FROM TransferRequisitionSlip AS MM WHERE ISNULL(MM.ReceivedByUserID,0)!=0) ORDER BY HH.UserName";
            List<User> oReceivedUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oReceivedUser = new ESimSol.BusinessObjects.User();
            oReceivedUser.UserID = 0; oReceivedUser.UserName = "--Select Received User--";
            oReceivedUsers.Add(oReceivedUser);
            oReceivedUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.Issue_Store_Wise  || (EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.Receive_Store_Wise || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReceivedUsers = oReceivedUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.TransferRequisitionSlipStates = EnumObject.jGets(typeof(EnumChallanStatus));
            //ViewBag.IssueStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.TransferRequisitionSlip, EnumStoreType.IssueStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.ReceiveStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.TransferRequisitionSlip, EnumStoreType.ReceiveStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RequisitionTypes = EnumObject.jGets(typeof(EnumRequisitionType));
            return View(oTransferRequisitionSlipRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(TransferRequisitionSlipRegister oTransferRequisitionSlipRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oTransferRequisitionSlipRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintTransferRequisitionSlipRegister(double ts)
        {
            TransferRequisitionSlipRegister oTransferRequisitionSlipRegister = new TransferRequisitionSlipRegister();
            try
            {
                _sErrorMesage = "";
                _oTransferRequisitionSlipRegisters = new List<TransferRequisitionSlipRegister>();
                oTransferRequisitionSlipRegister = (TransferRequisitionSlipRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oTransferRequisitionSlipRegister);
                _oTransferRequisitionSlipRegisters = TransferRequisitionSlipRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oTransferRequisitionSlipRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oTransferRequisitionSlipRegisters = new List<TransferRequisitionSlipRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oTransferRequisitionSlipRegister.IssueBUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oTransferRequisitionSlipRegister.IssueBUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptTransferRequisitionSlipRegisters oReport = new rptTransferRequisitionSlipRegisters();
                byte[] abytes = oReport.PrepareReport(_oTransferRequisitionSlipRegisters, oCompany, oTransferRequisitionSlipRegister.ReportLayout, _sDateRange);
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
        public void ExportToExcelTransferRequisitionSlipRegister(double ts)
        {
            TransferRequisitionSlipRegister oTransferRequisitionSlipRegister = new TransferRequisitionSlipRegister();
            _oTransferRequisitionSlipRegisters = new List<TransferRequisitionSlipRegister>();
            oTransferRequisitionSlipRegister = (TransferRequisitionSlipRegister)Session[SessionInfo.ParamObj];
            string sSQL = this.GetSQL(oTransferRequisitionSlipRegister);
            _oTransferRequisitionSlipRegisters = TransferRequisitionSlipRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            if (_oTransferRequisitionSlipRegisters.Count <= 0)
            {
                _sErrorMesage = "There is no data for print!";
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            if (oTransferRequisitionSlipRegister.IssueBUID > 0)
            {
                oBU = oBU.Get(oTransferRequisitionSlipRegister.IssueBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Internal Challan Register");
                string sReportHeader = "";
                #region Report Body & Header
                if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.Issue_Store_Wise)
                {
                    sReportHeader = "Issue Store Wise Internal Challan Register(Details)";
                }
                else if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.Receive_Store_Wise)
                {
                    sReportHeader = "Receive Store Wise Internal Challan Register(Details)";
                }
                else if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                {
                    sReportHeader = "Date Wise Internal Challan Register(Details)";
                }
                else if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    sReportHeader = "Product Wise Internal Challan Register";
                }
                sheet.Column(nTempCol).Width = 30; nTempCol++;//sl      
                sheet.Column(nTempCol).Width = 20; nTempCol++;//Challan no
                sheet.Column(nTempCol).Width = 25; nTempCol++;//Challan date or status
                sheet.Column(nTempCol).Width = 30; nTempCol++;//Buyer Name or Challan NO
                sheet.Column(nTempCol).Width = 30; nTempCol++;//DO no
                sheet.Column(nTempCol).Width = 30; nTempCol++;//PI No
                sheet.Column(nTempCol).Width = 30; nTempCol++;//LC no
                sheet.Column(nTempCol).Width = 30; nTempCol++;//Vichle no
                sheet.Column(nTempCol).Width = 30; nTempCol++;//Code 
                if (oTransferRequisitionSlipRegister.ReportLayout != EnumReportLayout.ProductWise)
                {
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Product name
                }
                sheet.Column(nTempCol).Width = 20; nTempCol++;//Style 
                sheet.Column(nTempCol).Width = 20; nTempCol++;//Color 
                sheet.Column(nTempCol).Width = 20; nTempCol++;//Lot 
                sheet.Column(nTempCol).Width = 20; nTempCol++;//Brand 
                sheet.Column(nTempCol).Width = 20; nTempCol++;//m unit
                sheet.Column(nTempCol).Width = 20;//qty   
                
                #endregion

                nEndCol = nTempCol + 1;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-6]; cell.Merge = true;
                cell.Value = oTransferRequisitionSlipRegister.IssueBUID>0?oBU.Name:oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[nRowIndex, nEndCol-5, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-6]; cell.Merge = true;
                cell.Value = oTransferRequisitionSlipRegister.IssueBUID>0?oBU.PringReportHead:oCompany.PringReportHead; cell.Style.Font.Bold = false; 
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
                
                if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.Issue_Store_Wise)
                {
                    #region Data (Issue Store Wise Details)
                    int nTRSID = 0; double nTotalQty = 0, nGrandTotalQty = 0, nIssueStoreWiseQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
                    string sIssueStoreName = "";
                    foreach (TransferRequisitionSlipRegister oItem in _oTransferRequisitionSlipRegisters)
                    {

                        if (sIssueStoreName != oItem.IssueStoreName)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Date Wise Total
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Store Wise Total"; cell.Style.Font.Bold = true; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nIssueStoreWiseQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTotalQty = 0; nIssueStoreWiseQty = 0; nTempRowIndex = 0; nTableRow = 0;
                            }
                            #region Header

                            #region Blank Row
                            nRowIndex++;
                            #endregion


                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 16]; cell.Value = "Store Name : " + oItem.ReceivedStoreName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;

                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Challan Status"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Challan Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Receive Store"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Disburse By"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Received By"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Vihicle No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Brand Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Style Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 15]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex++;
                            #endregion
                            #endregion

                            nSL = 0;
                        }
                        if (oItem.TRSID != nTRSID)
                        {

                            if (nTableRow > 0)
                            {

                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTableRow = 0;
                                nTotalQty = 0;
                            }
                            nTableRow++;
                            nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

                            nRowSpan = _oTransferRequisitionSlipRegisters.Where(ChallanR => ChallanR.TRSID == oItem.TRSID).ToList().Count;
                            nTempRowIndex = nRowIndex + nRowSpan;
                            nSL++;
                            cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.TRSNO; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.TransferStatusSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.IssueDateTimeSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.ReceivedStoreName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.DisbursedByName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7, nTempRowIndex, 7]; cell.Value = oItem.ReceivedByName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8, nTempRowIndex, 8]; cell.Value = oItem.VehicleNo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.SupplierSName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTRSID = oItem.TRSID;
                        sIssueStoreName = oItem.IssueStoreName;
                        nTotalQty = nTotalQty + oItem.Qty;
                        nIssueStoreWiseQty = nIssueStoreWiseQty + oItem.Qty;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region SubTotal
                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Date Wise Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Store Wise Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nIssueStoreWiseQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                {
                    #region Data (Date wise)
                    int nTRSID = 0; double nTotalQty = 0, nGrandTotalQty = 0, nDateWiseQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
                    DateTime dIssueDateTime = DateTime.MinValue;
                    foreach (TransferRequisitionSlipRegister oItem in _oTransferRequisitionSlipRegisters)
                    {

                        if (dIssueDateTime.ToString("dd MMM yyyy") != oItem.IssueDateTime.ToString("dd MMM yyyy"))
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                              
                                #region Date Wise Total
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Date Wise Total"; cell.Style.Font.Bold = true; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nDateWiseQty); cell.Style.Font.Bold = true;
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
                            cell = sheet.Cells[nRowIndex,1,nRowIndex, 16]; cell.Value = "Challan Date @ " + oItem.IssueDateTimeSt; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;
                            
                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Challan Status"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Issue Store"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Receive Store"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Approved By"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Received By"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Vihicle No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Brand Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Style"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 15]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex++;
                            #endregion
                            #endregion

                            nSL = 0;
                        }
                        if (oItem.TRSID != nTRSID)
                        {

                            if (nTableRow > 0)
                            {

                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTableRow = 0;
                                nTotalQty = 0;
                            }
                            nTableRow++;
                            nRowIndex= nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

                            nRowSpan = _oTransferRequisitionSlipRegisters.Where(ChallanR => ChallanR.TRSID == oItem.TRSID).ToList().Count;
                            nTempRowIndex = nRowIndex + nRowSpan;
                            nSL++;
                            cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.TRSNO; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.TransferStatusSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.IssueStoreName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.ReceivedStoreName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.AythorisedByName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7, nTempRowIndex, 7]; cell.Value = oItem.ReceivedByName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8, nTempRowIndex, 8]; cell.Value = oItem.VehicleNo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.SupplierSName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTRSID = oItem.TRSID;
                        dIssueDateTime = oItem.IssueDateTime;
                        nTotalQty = nTotalQty + oItem.Qty;
                        nDateWiseQty = nDateWiseQty + oItem.Qty;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region SubTotal
                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Date Wise Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Date Wise Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nDateWiseQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.Receive_Store_Wise)
                {
                    #region Data (Receive Store Wise Details)
                    int nTRSID = 0; double nTotalQty = 0, nGrandTotalQty = 0, nReceiveStoreWiseQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
                    string sReceiveStoreName = "";
                    foreach (TransferRequisitionSlipRegister oItem in _oTransferRequisitionSlipRegisters)
                    {

                        if (sReceiveStoreName != oItem.ReceivedStoreName)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Date Wise Total
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Store Wise Total"; cell.Style.Font.Bold = true; cell.Merge = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nReceiveStoreWiseQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTotalQty = 0; nReceiveStoreWiseQty = 0; nTempRowIndex = 0; nTableRow = 0;
                            }
                            #region Header

                            #region Blank Row
                            nRowIndex++;
                            #endregion


                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 16]; cell.Value = "Store Name : " + oItem.ReceivedStoreName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;

                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Challan Status"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Challan Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Issue Store"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Disburse By"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Received By"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Vihicle No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Brand Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 15]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex++;
                            #endregion
                            #endregion

                            nSL = 0;
                        }
                        if (oItem.TRSID != nTRSID)
                        {

                            if (nTableRow > 0)
                            {

                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion
                                nTableRow = 0;
                                nTotalQty = 0;
                            }
                            nTableRow++;
                            nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

                            nRowSpan = _oTransferRequisitionSlipRegisters.Where(ChallanR => ChallanR.TRSID == oItem.TRSID).ToList().Count;
                            nTempRowIndex = nRowIndex + nRowSpan;
                            nSL++;
                            cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.TRSNO; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.TransferStatusSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.IssueDateTimeSt; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.IssueStoreName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.DisbursedByName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7, nTempRowIndex, 7]; cell.Value = oItem.ReceivedByName; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8, nTempRowIndex, 8]; cell.Value = oItem.VehicleNo; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.SupplierSName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTRSID = oItem.TRSID;
                        sReceiveStoreName = oItem.ReceivedStoreName;                        
                        nTotalQty = nTotalQty + oItem.Qty;
                        nReceiveStoreWiseQty = nReceiveStoreWiseQty + oItem.Qty;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region SubTotal
                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Date Wise Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Store Wise Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nReceiveStoreWiseQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
                else if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    #region Data (Product Wise)
                    double nTotalQty = 0, nGrandTotalQty = 0; int nTableRow = 0;
                    string sProductName = "";
                    foreach (TransferRequisitionSlipRegister oItem in _oTransferRequisitionSlipRegisters)
                    {

                        if (sProductName != oItem.ProductName)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 14]; cell.Value = "Product Wise Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 15]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                nTotalQty = 0;   nTableRow = 0;
                            }
                            #region Header

                            #region Blank Row
                            nRowIndex++;
                            #endregion


                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 15]; cell.Value = "Product Name : " + oItem.ProductName+"["+oItem.ProductCode+"]"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true; cell.Style.Font.UnderLine = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;

                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Challan Status"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Issue Store"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Receive Store"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Approve By"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Received By"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Vihicle No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Challan Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Lot"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Brand"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 14]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
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

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.TRSNO; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.TransferStatusSt; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.IssueStoreName; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ReceivedStoreName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.AythorisedByName; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ReceivedByName; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.VehicleNo; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.IssueDateTimeSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.SupplierSName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
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
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 14]; cell.Value = "Product Wise Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 14]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
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
                Response.AddHeader("content-disposition", "attachment; filename=TransferRequisitionSlipRegister(Bulk).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        #endregion

        #endregion



        #region Support Functions
        

        private string GetSQL(TransferRequisitionSlipRegister oTransferRequisitionSlipRegister)
        {
            _sDateRange = "";
            string sSearchingData = oTransferRequisitionSlipRegister.SearchingData;
            EnumCompareOperator ePIIssueDateTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPIIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPIIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sStyleIDs = sSearchingData.Split('~')[3];
            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region RequisitionType
            if (oTransferRequisitionSlipRegister.RequisitionTypeInInt!=0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RequisitionType =" + oTransferRequisitionSlipRegister.RequisitionTypeInInt;
            }
            
            #endregion

            #region Issue BusinessUnit
            if (oTransferRequisitionSlipRegister.IssueBUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " IssueBUID =" + oTransferRequisitionSlipRegister.IssueBUID.ToString();
            }
            #endregion

            #region TransferRequisitionSlipNo
            if (oTransferRequisitionSlipRegister.TRSNO != null && oTransferRequisitionSlipRegister.TRSNO != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " TRSNO LIKE'%" + oTransferRequisitionSlipRegister.TRSNO + "%'";
            }
            #endregion

            #region ApproveBy
            if (oTransferRequisitionSlipRegister.AuthorisedByID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " AuthorisedByID =" + oTransferRequisitionSlipRegister.AuthorisedByID.ToString();
            }
            #endregion

            #region Received By
            if (oTransferRequisitionSlipRegister.ReceivedByUserID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ReceivedByUserID =" + oTransferRequisitionSlipRegister.ReceivedByUserID.ToString();
            }
            #endregion

            #region ChallanStatus
            if (oTransferRequisitionSlipRegister.TransferStatusInInt>-1)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " TransferStatus =" + (oTransferRequisitionSlipRegister.TransferStatusInInt).ToString();
            }
            #endregion

            #region Remarks
            if (oTransferRequisitionSlipRegister.Remark != null && oTransferRequisitionSlipRegister.Remark != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Remark LIKE'%" + oTransferRequisitionSlipRegister.Remark + "%'";
            }
            #endregion

            #region Brand
            if (oTransferRequisitionSlipRegister.SupplierSName != null && oTransferRequisitionSlipRegister.SupplierSName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " SupplierSName LIKE'%" + oTransferRequisitionSlipRegister.SupplierSName + "%'";
            }
            #endregion

            #region Color No
            if (oTransferRequisitionSlipRegister.ColorName != null && oTransferRequisitionSlipRegister.ColorName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ColorName LIKE'%" + oTransferRequisitionSlipRegister.ColorName + "%'";
            }
            #endregion

            #region Lot No
            if (oTransferRequisitionSlipRegister.LotNo != null && oTransferRequisitionSlipRegister.LotNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LotNo LIKE'%" + oTransferRequisitionSlipRegister.LotNo + "%'";
            }
            #endregion

            #region Product
            if (oTransferRequisitionSlipRegister.ProductName != null && oTransferRequisitionSlipRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oTransferRequisitionSlipRegister.ProductName + ")";
            }
            #endregion

            #region Style 
            if (!string.IsNullOrEmpty(sStyleIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " TRSID IN (SELECT TRSID FROM TransferRequisitionSlipDetail WHERE StyleID In (" + sStyleIDs + "))";
            }
            #endregion

            #region Product Category
            if (oTransferRequisitionSlipRegister.ProductCategoryID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductCategoryID =" + oTransferRequisitionSlipRegister.ProductCategoryID;
            }
            #endregion

            #region Issue Date
            if (ePIIssueDateTime != EnumCompareOperator.None)
            {
                if (ePIIssueDateTime == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDateTime,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDateTime == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDateTime,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date Not Equal @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDateTime == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDateTime,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date Greater Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDateTime == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDateTime,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date Smaller Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDateTime == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDateTime,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDateTime == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDateTime,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date NOT Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Issue store
            if (!string.IsNullOrEmpty( oTransferRequisitionSlipRegister.IssueStoreName))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " IssueWorkingUnitID IN (" + oTransferRequisitionSlipRegister.IssueStoreName+")"; 
            }
            #endregion

            #region Receive store
            if (!string.IsNullOrEmpty(oTransferRequisitionSlipRegister.ReceivedStoreName))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ReceivedWorkingUnitID IN (" + oTransferRequisitionSlipRegister.ReceivedStoreName + ")"; 
            }
            #endregion

            #region Supplier
            if (!string.IsNullOrEmpty(oTransferRequisitionSlipRegister.ContractorName))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ContractorID IN (" + oTransferRequisitionSlipRegister.ContractorName + ")";
            }
            #endregion

          
            #region Report Layout
            if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.Issue_Store_Wise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_TransferRequisitionSlipRegister ";
                sOrderBy = " ORDER BY  IssueWorkingUnitID, TRSID, TRSDetailID ASC";
            }
            else if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.Receive_Store_Wise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_TransferRequisitionSlipRegister ";
                sOrderBy = " ORDER BY  ReceivedWorkingUnitID, TRSID, TRSDetailID ASC";
            }
            else if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_TransferRequisitionSlipRegister ";
                sOrderBy = " ORDER BY  IssueDateTime, TRSID, TRSDetailID ASC";
            }
           
            else if (oTransferRequisitionSlipRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_TransferRequisitionSlipRegister ";
                sOrderBy = " ORDER BY  ProductName ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_TransferRequisitionSlipRegister ";
                sOrderBy = " ORDER BY IssueDateTime, TRSID, TRSDetailID ASC";
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

