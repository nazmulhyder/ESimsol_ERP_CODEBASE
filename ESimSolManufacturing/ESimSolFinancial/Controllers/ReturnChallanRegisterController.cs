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
    public class ReturnChallanRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        List<ReturnChallanRegister> _oReturnChallanRegisters = new List<ReturnChallanRegister>();        

        #region Actions
        public ActionResult ViewReturnChallanRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ReturnChallanRegister oDeliveryChallanRegister = new ReturnChallanRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM ReturnChallan AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
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
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails|| (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.ReturnChallan, EnumStoreType.IssueStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProductCategoryies = ProductCategory.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oDeliveryChallanRegister);
        }
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ReturnChallanRegister oDeliveryChallanRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDeliveryChallanRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintReturnChallanRegister(double ts)
        {
            ReturnChallanRegister oReturnChallanRegister = new ReturnChallanRegister();
            try
            {
                _sErrorMesage = "";
                _oReturnChallanRegisters = new List<ReturnChallanRegister>();
                oReturnChallanRegister = (ReturnChallanRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oReturnChallanRegister);
                _oReturnChallanRegisters = ReturnChallanRegister.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
                if (_oReturnChallanRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oReturnChallanRegisters = new List<ReturnChallanRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oReturnChallanRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptReturnChallanRegisters oReport = new rptReturnChallanRegisters();
                byte[] abytes = oReport.PrepareReport(_oReturnChallanRegisters, oCompany, oReturnChallanRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        //#region Print XlX
        //public void ExportToExcelDeliveryChallanRegister(double ts)
        //{
        //    ReturnChallanRegister oDeliveryChallanRegister = new ReturnChallanRegister();
        //    _oReturnChallanRegisters = new List<ReturnChallanRegister>();
        //    oDeliveryChallanRegister = (ReturnChallanRegister)Session[SessionInfo.ParamObj];
        //    string sSQL = this.GetSQL(oDeliveryChallanRegister);
        //    _oReturnChallanRegisters = ReturnChallanRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    if (_oReturnChallanRegisters.Count <= 0)
        //    {
        //        _sErrorMesage = "There is no data for print!";
        //    }
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    BusinessUnit oBU = new BusinessUnit();
        //    oBU = oBU.Get(oDeliveryChallanRegister.BUID, (int)Session[SessionInfo.currentUserID]);
        //    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

        //    int nSL = 0;
        //    #region Export Excel
        //    int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 20, nTempCol = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    using (var excelPackage = new ExcelPackage())
        //    {
        //        excelPackage.Workbook.Properties.Author = "ESimSol";
        //        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //        var sheet = excelPackage.Workbook.Worksheets.Add("Delivery Challan Register");
        //        string sReportHeader = " ";
        //        #region Report Body & Header
        //        if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ChallanWise)
        //        {
        //            sReportHeader = "Delivery Challan Register";
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
        //        {
        //            sReportHeader = "Date Wise Delivery Challan Register(Details)";
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
        //        {
        //            sReportHeader = "Party Wise Delivery Challan Register(Details)";
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ProductWise)
        //        {
        //            sReportHeader = "Product Wise Delivery Challan Register";
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ProductSummary)
        //        {
        //            sReportHeader = "Product Summary Register";
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.Vehicle_Wise)
        //        {
        //            sReportHeader = "Vehicle Service Register";
        //        }
        //        sheet.Column(nTempCol).Width = 30; nTempCol++;//sl      
        //        if (oDeliveryChallanRegister.ReportLayout != EnumReportLayout.ProductSummary)
        //        {
        //            sheet.Column(nTempCol).Width = 50; nTempCol++;//Challan no
        //            sheet.Column(nTempCol).Width = 30; nTempCol++;//Challan date or status
        //            sheet.Column(nTempCol).Width = 30; nTempCol++;//Buyer Name or Challan NO
        //            sheet.Column(nTempCol).Width = 20; nTempCol++;//DO no
        //            sheet.Column(nTempCol).Width = 20; nTempCol++;//PI No
        //            sheet.Column(nTempCol).Width = 20; nTempCol++;//LC no
        //            sheet.Column(nTempCol).Width = 30; nTempCol++;//Vichle no
        //        }
        //        sheet.Column(nTempCol).Width = 20; nTempCol++;//Code 
        //        if (oDeliveryChallanRegister.ReportLayout != EnumReportLayout.ProductWise || oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ProductSummary)
        //        {
        //            sheet.Column(nTempCol).Width = 30; nTempCol++;//Product name
        //        }
        //        sheet.Column(nTempCol).Width = 15; nTempCol++;//m unit
        //        sheet.Column(nTempCol).Width = 25;//qty   
        //        #endregion

        //        nEndCol = nTempCol + 1;

        //        #region Report Header
        //        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex,oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ProductSummary?nEndCol-3:nEndCol-6]; cell.Merge = true;
        //        cell.Value = oBU.Name; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //        cell = sheet.Cells[nRowIndex, oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ProductSummary?nEndCol-2:nEndCol-5, nRowIndex, nEndCol]; cell.Merge = true;
        //        cell.Value = sReportHeader; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
        //        cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        nRowIndex = nRowIndex + 1;

        //        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ProductSummary ? nEndCol - 3 : nEndCol - 6]; cell.Merge = true;
        //        cell.Value = oBU.PringReportHead; cell.Style.Font.Bold = false; 
        //        cell.Style.WrapText = true;
        //        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //        cell = sheet.Cells[nRowIndex, oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ProductSummary ? nEndCol - 2 : nEndCol - 5, nRowIndex, nEndCol]; cell.Merge = true;
        //        cell.Value = _sDateRange; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        nRowIndex = nRowIndex + 1;
        //        #endregion

        //        #region Blank
        //        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
        //        cell.Value = ""; cell.Style.Font.Bold = true;
        //        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        //        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        nRowIndex = nRowIndex + 1;
        //        #endregion

        //        #region Report Data
                
        //        if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ChallanWise)
        //        {
        //            #region column title
        //            nTempCol = 1;
        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Challan Date"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                   
        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "DO  No"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Vihicle No"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            nRowIndex++;
        //            #endregion

        //            #region Data
        //            int nDeliveryChallanID = 0; double nTotalQty = 0, nGrandTotalQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
        //            foreach (ReturnChallanRegister oItem in _oReturnChallanRegisters)
        //            {

        //                if (oItem.ReturnChallanID != nDeliveryChallanID)
        //                {

        //                    if (nTableRow > 0)
        //                    {
        //                        #region SubTotal
        //                        cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        #endregion
        //                        nTableRow = 0;
        //                        nTotalQty = 0;
        //                    }
        //                    nTableRow++;
        //                    nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

        //                    nRowSpan = _oReturnChallanRegisters.Where(ChallanR => ChallanR.ReturnChallanID == oItem.ReturnChallanID).ToList().Count;
        //                    nTempRowIndex = nRowIndex + nRowSpan;
        //                    nSL++;
        //                    cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.ChallanNo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.ChallanDateSt; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 7, nTempRowIndex, 7]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 8, nTempRowIndex, 8]; cell.Value = oItem.VehicleNo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                }
        //                cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                nDeliveryChallanID = oItem.ReturnChallanID;
        //                nTotalQty = nTotalQty + oItem.Qty;
        //                nGrandTotalQty = nGrandTotalQty + oItem.Qty;

        //                nEndRow = nRowIndex;
        //                nRowIndex++;

        //            }

        //            #region SubTotal

        //            cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
        //            #region Grand Total
        //            nRowIndex++;
        //            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
        //            #endregion
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
        //        {
        //            #region Data (Date wise)
        //            int nDeliveryChallanID = 0; double nTotalQty = 0, nGrandTotalQty = 0, nDateWiseQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
        //            DateTime dChallanDate = DateTime.MinValue;
        //            foreach (ReturnChallanRegister oItem in _oReturnChallanRegisters)
        //            {

        //                if (dChallanDate.ToString("dd MMM yyyy") != oItem.ReturnDate.ToString("dd MMM yyyy"))
        //                {
        //                    if (nTableRow > 0)
        //                    {
        //                        #region SubTotal
        //                        cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        #endregion
                              
        //                        #region Date Wise Total
        //                        nRowIndex++;
        //                        cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Date Wise Total"; cell.Style.Font.Bold = true; cell.Merge = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nDateWiseQty); cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        #endregion
        //                        nTotalQty = 0; nDateWiseQty = 0; nTempRowIndex = 0; nTableRow = 0;
        //                    }
        //                    #region Header

        //                    #region Blank Row
        //                    nRowIndex++;
        //                    #endregion


        //                    #region Date Heading
        //                    nRowIndex++;
        //                    cell = sheet.Cells[nRowIndex,1,nRowIndex, 12]; cell.Value = "Challan Date @ " + oItem.ChallanDateSt; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
        //                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    #endregion

        //                    #region Header Row
        //                    nRowIndex++;
                            
        //                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Challan Status"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "DO  No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Vihicle No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Code"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nRowIndex++;
        //                    #endregion
        //                    #endregion

        //                    nSL = 0;
        //                }
        //                if (oItem.ReturnChallanID != nDeliveryChallanID)
        //                {

        //                    if (nTableRow > 0)
        //                    {

        //                        #region SubTotal
        //                        cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        #endregion
        //                        nTableRow = 0;
        //                        nTotalQty = 0;
        //                    }
        //                    nTableRow++;
        //                    nRowIndex= nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

        //                    nRowSpan = _oReturnChallanRegisters.Where(ChallanR => ChallanR.ReturnChallanID == oItem.ReturnChallanID).ToList().Count;
        //                    nTempRowIndex = nRowIndex + nRowSpan;
        //                    nSL++;
        //                    cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.ChallanNo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.ChallanStatusSt; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 7, nTempRowIndex, 7]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 8, nTempRowIndex, 8]; cell.Value = oItem.VehicleNo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                }
        //                cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                nDeliveryChallanID = oItem.ReturnChallanID;
        //                dChallanDate = oItem.ReturnDate;
        //                nTotalQty = nTotalQty + oItem.Qty;
        //                nDateWiseQty = nDateWiseQty + oItem.Qty;
        //                nGrandTotalQty = nGrandTotalQty + oItem.Qty;

        //                nEndRow = nRowIndex;
        //                nRowIndex++;

        //            }

        //            #region SubTotal
        //            cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
        //            #region Date Wise Total
        //            nRowIndex++;
        //            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Date Wise Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nDateWiseQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
        //            #region Grand Total
        //            nRowIndex++;
        //            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
        //            #endregion
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
        //        {
        //            #region Data (Paty Wise Details)
        //            int nDeliveryChallanID = 0; double nTotalQty = 0, nGrandTotalQty = 0, nPartyWiseQty = 0; int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;
        //            string sBuyerName = "";
        //            foreach (ReturnChallanRegister oItem in _oReturnChallanRegisters)
        //            {

        //                if (sBuyerName != oItem.ContractorName)
        //                {
        //                    if (nTableRow > 0)
        //                    {
        //                        #region SubTotal
        //                        cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        #endregion

        //                        #region Date Wise Total
        //                        nRowIndex++;
        //                        cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Party Wise Total"; cell.Style.Font.Bold = true; cell.Merge = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nPartyWiseQty); cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        #endregion
        //                        nTotalQty = 0; nPartyWiseQty = 0; nTempRowIndex = 0; nTableRow = 0;
        //                    }
        //                    #region Header

        //                    #region Blank Row
        //                    nRowIndex++;
        //                    #endregion


        //                    #region Date Heading
        //                    nRowIndex++;
        //                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 12]; cell.Value = "Party Name : " + oItem.ContractorName; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
        //                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    #endregion

        //                    #region Header Row
        //                    nRowIndex++;

        //                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Challan Status"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Challan Date"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "DO  No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Vihicle No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Code"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nRowIndex++;
        //                    #endregion
        //                    #endregion

        //                    nSL = 0;
        //                }
        //                if (oItem.ReturnChallanID != nDeliveryChallanID)
        //                {

        //                    if (nTableRow > 0)
        //                    {

        //                        #region SubTotal
        //                        cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        #endregion
        //                        nTableRow = 0;
        //                        nTotalQty = 0;
        //                    }
        //                    nTableRow++;
        //                    nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

        //                    nRowSpan = _oReturnChallanRegisters.Where(ChallanR => ChallanR.ReturnChallanID == oItem.ReturnChallanID).ToList().Count;
        //                    nTempRowIndex = nRowIndex + nRowSpan;
        //                    nSL++;
        //                    cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2]; cell.Value = "" + oItem.ChallanNo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.ChallanStatusSt; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 4, nTempRowIndex, 4]; cell.Value = oItem.ChallanDateSt; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 5, nTempRowIndex, 5]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 6, nTempRowIndex, 6]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 7, nTempRowIndex, 7]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 8, nTempRowIndex, 8]; cell.Value = oItem.VehicleNo; cell.Style.Font.Bold = false; cell.Merge = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                }
        //                cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                nDeliveryChallanID = oItem.ReturnChallanID;
        //                sBuyerName = oItem.ContractorName;                        
        //                nTotalQty = nTotalQty + oItem.Qty;
        //                nPartyWiseQty = nPartyWiseQty + oItem.Qty;
        //                nGrandTotalQty = nGrandTotalQty + oItem.Qty;

        //                nEndRow = nRowIndex;
        //                nRowIndex++;

        //            }

        //            #region SubTotal
        //            cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
        //            #region Date Wise Total
        //            nRowIndex++;
        //            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Party Wise Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nPartyWiseQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
        //            #region Grand Total
        //            nRowIndex++;
        //            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 12]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
        //            #endregion
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ProductWise)
        //        {
        //            #region Data (Product Wise)
        //            double nTotalQty = 0, nGrandTotalQty = 0; int nTableRow = 0;
        //            string sProductName = "";
        //            foreach (ReturnChallanRegister oItem in _oReturnChallanRegisters)
        //            {

        //                if (sProductName != oItem.ProductName)
        //                {
        //                    if (nTableRow > 0)
        //                    {
        //                        #region SubTotal
        //                        cell = sheet.Cells[nRowIndex, 1, nRowIndex, 10]; cell.Value = "Product Wise Sub Total"; cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        #endregion

        //                        nTotalQty = 0;   nTableRow = 0;
        //                    }
        //                    #region Header

        //                    #region Blank Row
        //                    nRowIndex++;
        //                    #endregion


        //                    #region Date Heading
        //                    nRowIndex++;
        //                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 11]; cell.Value = "Product Name : " + oItem.ProductName+"["+oItem.ProductCode+"]"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true; cell.Style.Font.UnderLine = true;
        //                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    #endregion

        //                    #region Header Row
        //                    nRowIndex++;

        //                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Challan Status"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "DO  No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Vihicle No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Challan Date"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nRowIndex++;
        //                    #endregion
        //                    #endregion
        //                    nSL = 0;
                            
        //                }
        //                nTableRow++;
        //                 nSL++;
        //                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; 
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.ChallanNo; cell.Style.Font.Bold = false; 
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ChallanStatusSt; cell.Style.Font.Bold = false; 
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; 
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false; 
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false; 
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.VehicleNo; cell.Style.Font.Bold = false; 
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ChallanDateSt; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                sProductName = oItem.ProductName;
        //                nTotalQty = nTotalQty + oItem.Qty;
        //                nGrandTotalQty = nGrandTotalQty + oItem.Qty;

        //                nEndRow = nRowIndex;
        //                nRowIndex++;

        //            }

        //            #region SubTotal
        //            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 10]; cell.Value = "Product Wise Sub Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(nTotalQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
                    
        //            #region Grand Total
        //            nRowIndex++;
        //            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 10]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 11]; cell.Value = Global.MillionFormatActualDigit(nGrandTotalQty); cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            #endregion
        //            #endregion
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.Vehicle_Wise)
        //        {
        //            #region Data (Vehicle Wise)
        //             int nRowSpan = 0, nExtraRowIndex = 0;
        //            string sVehicleNo = "";
        //            foreach (ReturnChallanRegister oItem in _oReturnChallanRegisters)
        //            {

        //                if (sVehicleNo != oItem.VehicleNo)
        //                {
                     
        //                    #region Header

        //                    #region Blank Row
        //                    nRowIndex++;
        //                    #endregion


        //                    #region Date Heading
        //                    nRowIndex++;
        //                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 10]; cell.Value = "Vehicel Name & No: " + oItem.VehicleName + " " + oItem.VehicleNo; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true; cell.Style.Font.UnderLine = true;
        //                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    #endregion

        //                    #region Header Row
        //                    nRowIndex++;

        //                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Time"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Status"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "DO No"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Delivery Place"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Delivery Address"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Product"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Qty(Pcs)"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nRowIndex++;
        //                    #endregion
        //                    #endregion
        //                    nSL = 0;
        //                    nRowSpan = 0;

        //                }
        //                if (nRowSpan <= 0)
        //                {
        //                    nSL++;
        //                    nRowSpan = _oReturnChallanRegisters.Where(x => x.VehicleDateTime == oItem.VehicleDateTime && x.VehicleNo == oItem.VehicleNo).Count();
        //                    if (nRowSpan > 1) { nExtraRowIndex = (nRowIndex + nRowSpan)-1; } else { nExtraRowIndex = nRowIndex; }
        //                    cell = sheet.Cells[nRowIndex, 1, nExtraRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; if (nRowSpan > 1) { cell.Merge = true; }
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[nRowIndex, 2, nExtraRowIndex, 2]; cell.Value = "" + oItem.VehicleDateTimeInString; cell.Style.Font.Bold = false;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; if (nRowSpan > 1) { cell.Merge = true; }
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                }


        //                cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ChallanStatusSt; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ChallanNo; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.DONo; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.DeliveryToName; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DeliveryToAddress; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 9]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                sVehicleNo = oItem.VehicleNo;
        //                nRowSpan--;
        //                nEndRow = nRowIndex;
        //                nRowIndex++;

        //            }

        //            #endregion
        //        }
        //        else if (oDeliveryChallanRegister.ReportLayout == EnumReportLayout.ProductSummary)
        //        {
        //            #region Data 
        //            int nRowSpan = 0;

        //            #region Header Row
        //            nRowIndex++;
        //            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Code"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Unit Name"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            nRowIndex++;
        //            #endregion
        //            foreach (ReturnChallanRegister oItem in _oReturnChallanRegisters)
        //            {
        //                nSL++;
        //                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; if (nRowSpan > 1) { cell.Merge = true; }
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      

        //                cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //                cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(oItem.Qty); cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                nRowIndex++;

        //            }

        //            #endregion
        //        }
        //        #endregion

        //        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //        fill.BackgroundColor.SetColor(Color.White);

        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=DeliveryVehicleRegister(Bulk).xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();
        //    }
        //    #endregion


        //}
        //#endregion

        #endregion
        #region Support Functions        
        private string GetSQL(ReturnChallanRegister oReturnChallanRegister)
        {
            _sDateRange = "";
            string sSearchingData = oReturnChallanRegister.SearchingData;
            EnumCompareOperator ePIChallanDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPIIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPIIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

         
            int nWorkingUnitID = Convert.ToInt32(sSearchingData.Split('~')[3]);
            string sDONo = sSearchingData.Split('~')[4];
            string sExportPINo = sSearchingData.Split('~')[5];


            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oReturnChallanRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);  
                sWhereCluse = sWhereCluse + " BUID =" + oReturnChallanRegister.BUID.ToString();
               

            }
            #endregion

            #region DeliveryChallanNo
            if (oReturnChallanRegister.ReturnChallanNo != null && oReturnChallanRegister.ReturnChallanNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ReturnChallanNo LIKE'%" + oReturnChallanRegister.ReturnChallanNo + "%'";

            }
            #endregion

            //#region ApproveBy
            //if (oReturnChallanRegister.ApproveBy != 0)
            //{
            //    Global.TagSQL(ref sWhereCluse);  
            //    sWhereCluse = sWhereCluse + " ApproveBy =" + oReturnChallanRegister.ApproveBy.ToString();
            //}
            //#endregion
            
            #region ApproveBy
            if (oReturnChallanRegister.ProductCategoryID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductCategoryID =" + oReturnChallanRegister.ProductCategoryID.ToString();
            }
            #endregion

       

        

            #region Remarks
            if (oReturnChallanRegister.Remarks != null && oReturnChallanRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);  
                sWhereCluse = sWhereCluse + " Remarks LIKE'%" + oReturnChallanRegister.Remarks + "%'";

            }
            #endregion

            #region Supplier
            if (oReturnChallanRegister.ContractorName != null && oReturnChallanRegister.ContractorName != "")
            {
                Global.TagSQL(ref sWhereCluse);  
                sWhereCluse = sWhereCluse + " ContractorID IN(" + oReturnChallanRegister.ContractorName + ")";

            }
            #endregion

            #region Product
            if (oReturnChallanRegister.ProductName != null && oReturnChallanRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);  
                sWhereCluse = sWhereCluse + " ProductID IN(" + oReturnChallanRegister.ProductName + ")";

            }
            #endregion

            #region ReturnDate Date
            if (ePIChallanDate != EnumCompareOperator.None)
            {
                if (ePIChallanDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);  
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIChallanDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);  
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date Not Equal @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIChallanDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);  
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                   
                    _sDateRange = "Challan Date Greater Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIChallanDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);  
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date Smaller Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIChallanDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);  
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (ePIChallanDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);  
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Challan Date NOT Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region PINo
            if (sExportPINo != null && sExportPINo != "")
            {
                Global.TagSQL(ref sWhereCluse);  
                sWhereCluse = sWhereCluse + " DeliveryOrderID IN (SELECT MM.DeliveryOrderID FROM View_DeliveryOrder AS MM WHERE MM.RefNo  LIKE '%" + sExportPINo + "%' AND RefType = "+(int)EnumRefType.ExportPI+" )";
                
            }
            #endregion

          

            #region Report Layout
           if (oReturnChallanRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ReturnChallanRegister ";
                sOrderBy = " ORDER BY  ReturnDate, ReturnChallanID, ReturnChallanDetailID ASC";
            }
            else if (oReturnChallanRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ReturnChallanRegister ";
                sOrderBy = " ORDER BY  ContractorID, ReturnChallanID, ReturnChallanDetailID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ReturnChallanRegister ";
                sOrderBy = " ORDER BY ReturnDate, ReturnChallanID, ReturnChallanDetailID ASC";
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

