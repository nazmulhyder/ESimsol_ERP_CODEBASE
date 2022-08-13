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
    public class VOrderController : Controller
    {
        #region Declaration
        VOrder _oVOrder = new VOrder();
        List<VOrder> _oVOrders = new List<VOrder>();
        List<VOrderRegister> _oVOrderRegisters = new List<VOrderRegister>();
        VOrderRegister _oVOrderRegister = new VOrderRegister();
        string _sErrorMesage = "";
        #endregion

        #region Function
        private List<VOrderRegister> GetList(List<VOrderRegister> oVOrderRegisters)
        {

            List<VOrderRegister> oTempVOrderRegisters = new List<VOrderRegister>();
            List<VOrderRegister> oNewVOrderRegisters = new List<VOrderRegister>();
            oNewVOrderRegisters = oVOrderRegisters;

            for (int i = 0; i < oVOrderRegisters.Count(); i++)
            {
                if (oTempVOrderRegisters.Where(x => x.AccountHeadID == oVOrderRegisters[i].AccountHeadID && x.LCID == oVOrderRegisters[i].LCID).ToList().Count <= 0)
                {
                    _oVOrderRegister = new VOrderRegister();
                    _oVOrderRegister = oVOrderRegisters[i];
                    _oVOrderRegister.CreditAmount = GetAmount(oNewVOrderRegisters, oVOrderRegisters[i], false, false);//oVOrderRegisters.Where(x => x.AccountHeadID == oItem.AccountHeadID && x.LCID == oItem.LCID && x.IsDebit == false).Sum(x => (double?)x.Amount) ?? 0;
                    _oVOrderRegister.CreditAmountInCurrency = GetAmount(oNewVOrderRegisters, oVOrderRegisters[i], false, true);//oVOrderRegisters.Where(x => x.AccountHeadID == oItem.AccountHeadID && x.LCID == oItem.LCID && x.IsDebit == false).Sum(x => (double?)x.AmountInCurrency) ?? 0;
                    _oVOrderRegister.Amount = GetAmount(oNewVOrderRegisters, oVOrderRegisters[i], true, false);// oVOrderRegisters.Where(x => x.AccountHeadID == oItem.AccountHeadID && x.LCID == oItem.LCID && x.IsDebit == true).Sum(x => (double?)x.Amount) ?? 0;
                    _oVOrderRegister.AmountInCurrency = GetAmount(oNewVOrderRegisters, oVOrderRegisters[i], true, true);//oVOrderRegisters.Where(x => x.AccountHeadID == oItem.AccountHeadID && x.LCID == oItem.LCID && x.IsDebit == true).Sum(x => (double?)x.AmountInCurrency) ?? 0;
                    oTempVOrderRegisters.Add(_oVOrderRegister);
                }
            }
            return oTempVOrderRegisters;
        }

        public double GetAmount(List<VOrderRegister> oList, VOrderRegister oVOrderRegister, bool bIsDebit, bool bIsCurrency)
        {
            double nReturn = 0;

            for (int j = 0; j < oList.Count(); j++)
            {
                if (oList[j].LCID == oVOrderRegister.LCID && oList[j].AccountHeadID == oVOrderRegister.AccountHeadID && oList[j].IsDebit == bIsDebit)
                {
                    if (bIsCurrency)
                    {
                        nReturn += oList[j].AmountInCurrency;
                    }
                    else
                    {
                        nReturn += oList[j].Amount;
                    }

                }
            }

            return nReturn;
        }
        #endregion

        #region Actions
        public ActionResult ViewVOrders(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            ViewBag.VOrderRefTypes = EnumObject.jGets(typeof(EnumVOrderRefType));
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oVOrders = new List<VOrder>();
            return View(_oVOrders);
        }

        public ActionResult ViewVOrder(int id, double ts)
        {
            _oVOrder = new VOrder();
            if (id > 0)
            {
                _oVOrder = _oVOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.vOrderRef = EnumObject.jGets(typeof(EnumVOrderRefType));
            return View(_oVOrder);
        }

        [HttpPost]
        public JsonResult Save(VOrder oVOrder)
        {
            _oVOrder = new VOrder();
            try
            {
                _oVOrder = oVOrder;
                _oVOrder = _oVOrder.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(VOrder oVOrder)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oVOrder.Delete(oVOrder.VOrderID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsSubledger(ACCostCenter oACCostCenter)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            oACCostCenter.Name = oACCostCenter.Name == null ? "" : oACCostCenter.Name;
            oACCostCenters = ACCostCenter.GetsByCodeOrName(oACCostCenter, (int)Session[SessionInfo.currentUserID], oACCostCenter.BUID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Order Reference Register
        public ActionResult ViewOrderReferenceRegisters(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            EnumObject oEnumObject = new EnumObject();

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.Export_LC_Summery || (EnumReportLayout)oItem.id == EnumReportLayout.Export_LC_PI_Summery || (EnumReportLayout)oItem.id == EnumReportLayout.Export_LC_Details || (EnumReportLayout)oItem.id == EnumReportLayout.Order_Wise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            #region Compare Operator
            List<EnumObject> oCompareOperators = new List<EnumObject>();
            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumCompareOperator.None;
            oEnumObject.Value = "--Select One--";
            oCompareOperators.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumCompareOperator.Between;
            oEnumObject.Value = EnumCompareOperator.Between.ToString();
            oCompareOperators.Add(oEnumObject);
            #endregion

            #region VOrderRefTypes
            List<EnumObject> oVOrderRefTypes = new List<EnumObject>();
            oEnumObject = new EnumObject();
            oEnumObject.id = 0;
            oEnumObject.Value = "--Select One--";
            oVOrderRefTypes.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumVOrderRefType.ExportLC;
            oEnumObject.Value = EnumVOrderRefType.ExportLC.ToString();
            oVOrderRefTypes.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumVOrderRefType.ImportLC;
            oEnumObject.Value = EnumVOrderRefType.ImportLC.ToString();
            oVOrderRefTypes.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumVOrderRefType.Product;
            oEnumObject.Value = EnumVOrderRefType.Product.ToString();
            oVOrderRefTypes.Add(oEnumObject);
            #endregion

            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = oCompareOperators;
            ViewBag.VOrderRefTypes = oVOrderRefTypes;
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(_oVOrder);
        }

        [HttpPost]
        public ActionResult SetOrderRefRegistersData(VOrderRegister oVOrderRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oVOrderRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetRegisterSQL(VOrderRegister oVOrderRegister)
        {
            string sReturn1 = "SELECT HH.VOrderID FROM VOrder AS HH";
            string sReturn = "";

            #region RefNo
            if (oVOrderRegister.RefNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.RefNo LIKE '%" + oVOrderRegister.RefNo + "%'";
            }
            #endregion

            #region Business Unit
            if (oVOrderRegister.BUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.BUID =" + oVOrderRegister.BUID.ToString();
            }
            #endregion

            #region RefType
            if (oVOrderRegister.VOrderRefTypeInt != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.VOrderRefType =" + oVOrderRegister.VOrderRefTypeInt.ToString();
            }
            #endregion

            #region Subledger
            if (oVOrderRegister.SubledgerName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.SubledgerID IN (" + oVOrderRegister.SubledgerName + ")";
            }
            #endregion

            #region RefObjIDs
            if (oVOrderRegister.RefObjIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.VOrderRefID IN (" + oVOrderRegister.RefObjIDs + ")";
            }
            #endregion

            #region OrderNo
            if (oVOrderRegister.OrderNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.OrderNo LIKE '%" + oVOrderRegister.OrderNo + "%'";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        public ActionResult PrintVOrderRegister(double ts)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            VOrderRegister oVOrderRegister = new VOrderRegister();
            List<VOrderRegister> oVOrderRegisters = new List<VOrderRegister>();
            List<VOrderRegister> oNewItems = new List<VOrderRegister>();
            _oVOrderRegisters = new List<VOrderRegister>();
            Company oCompany = new Company();
            try
            {
                oVOrderRegister = (VOrderRegister)Session[SessionInfo.ParamObj];
                if (oVOrderRegister.RefNo == null) { oVOrderRegister.RefNo = ""; }
                if (oVOrderRegister.OrderNo == null) { oVOrderRegister.OrderNo = ""; }
                if (oVOrderRegister.StartDate == null) { oVOrderRegister.StartDate = DateTime.Today.ToString("dd MMM yyyy"); }
                if (oVOrderRegister.EndDate == null) { oVOrderRegister.EndDate = DateTime.Today.ToString("dd MMM yyyy"); }
                if (oVOrderRegister.SubledgerName == null) { oVOrderRegister.SubledgerName = ""; }
                if (oVOrderRegister.RefObjIDs == null) { oVOrderRegister.RefObjIDs = ""; }
                string sSQL = this.GetRegisterSQL(oVOrderRegister);
                _oVOrderRegisters = VOrderRegister.Gets(oVOrderRegister, sSQL, (int)Session[SessionInfo.currentUserID]);

                if (oVOrderRegister.ReportLayoutInt == (int)EnumReportLayout.Export_LC_Summery)
                {
                    oNewItems = GetList(_oVOrderRegisters);
                    oVOrderRegisters = oNewItems;
                }
                else
                {
                    oVOrderRegisters = _oVOrderRegisters;
                }

                oBusinessUnit = oBusinessUnit.GetWithImage(oVOrderRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oBusinessUnit.BULogo = this.GetBULogo(oBusinessUnit);
            }
            catch (Exception ex)
            {
                oVOrderRegister = new VOrderRegister();
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport(ex.Message);
                return File(aErrorMessagebytes, "application/pdf");
            }

            rptVOrderRegister oReport = new rptVOrderRegister();
            byte[] abytes = oReport.PrepareReport(oVOrderRegisters, oBusinessUnit, (EnumReportLayout)oVOrderRegister.ReportLayoutInt, oCompany);
            return File(abytes, "application/pdf");
        }
        public void ExportToExcelVOrderRegister(double ts)
        {
            VOrderRegister oVOrderRegister = new VOrderRegister();
            oVOrderRegister = (VOrderRegister)Session[SessionInfo.ParamObj];
            if (oVOrderRegister.RefNo == null) { oVOrderRegister.RefNo = ""; }
            if (oVOrderRegister.OrderNo == null) { oVOrderRegister.OrderNo = ""; }
            if (oVOrderRegister.StartDate == null) { oVOrderRegister.StartDate = DateTime.Today.ToString("dd MMM yyyy"); }
            if (oVOrderRegister.EndDate == null) { oVOrderRegister.EndDate = DateTime.Today.ToString("dd MMM yyyy"); }
            if (oVOrderRegister.SubledgerName == null) { oVOrderRegister.SubledgerName = ""; }
            if (oVOrderRegister.RefObjIDs == null) { oVOrderRegister.RefObjIDs = ""; }
            string sSQL = this.GetRegisterSQL(oVOrderRegister);
            _oVOrderRegisters = VOrderRegister.Gets(oVOrderRegister, sSQL, (int)Session[SessionInfo.currentUserID]);
            if (_oVOrderRegisters.Count <= 0)
            {
                _sErrorMesage = "There is no data for print!";
            }
            if (oVOrderRegister.ReportLayoutInt == (int)EnumReportLayout.Export_LC_Summery)
            {
                _oVOrderRegisters = GetList(_oVOrderRegisters);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(oVOrderRegister.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 18, nTempCol = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Order Reference Register");
                string sReportHeader = " ";

                if (oVOrderRegister.ReportLayoutInt == (int)EnumReportLayout.Export_LC_Details)
                {

                    #region Report Body & Header
                    sReportHeader = "Order Reference Register For LC Details";
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//sl      
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//V.Date
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Order No
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Remarks
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//A/H
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//C.Rate
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Debit
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Credit
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//debit(Base curr) 
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Crdit(Base curr) 

                    #endregion
                    nEndCol = nTempCol + 1;
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oBU.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[nRowIndex, nEndCol - 4, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = sReportHeader; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;

                    //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    //cell.Value = oBU.PringReportHead; cell.Style.Font.Bold = false;
                    //cell.Style.WrapText = true;
                    //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    //cell = sheet.Cells[nRowIndex, nEndCol - 4, nRowIndex, nEndCol]; cell.Merge = true;
                    //cell.Value = _sDateRange; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                    //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //nRowIndex = nRowIndex + 1;
                    #endregion
                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #region Data (Paty Wise Details)
                    int nLCID = 0; int nTableRow = 0, nVorderType = 0;
                    double nTotalDebitAmount = 0, nTotalCreditAmount = 0, nTotalDebitAmountInCurrency = 0, nTotalCreditAmountInCurrency = 0;

                    foreach (VOrderRegister oItem in _oVOrderRegisters)
                    {

                        if (nLCID != oItem.LCID)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 6]; cell.Value = " "; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 10]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                #endregion

                                #region Blank space
                                nRowIndex++;
                                #endregion

                                nTableRow = 0;
                            }
                            #region Header
                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 4]; cell.Value = "LC No : " + oItem.LCNo; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5, nRowIndex, 10]; cell.Value = "Party Name: " + oItem.SubledgerName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;

                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Voucher Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Account Head"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "C.Rate"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Debit (" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Credit (" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            #endregion
                            #endregion
                            nVorderType = (int)oItem.VOrderRefType;
                            nTotalDebitAmount = 0.00; nTotalDebitAmountInCurrency = 0;
                            nTotalCreditAmount = 0.00; nTotalCreditAmountInCurrency = 0;
                            nSL = 0;
                        }
                        if (nVorderType != (int)oItem.VOrderRefType)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 6]; cell.Value = " "; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 10]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                #endregion
                                nTableRow = 0;
                            }
                            nTotalDebitAmount = 0.00; nTotalDebitAmountInCurrency = 0;
                            nTotalCreditAmount = 0.00; nTotalCreditAmountInCurrency = 0;
                            nSL = 0;
                        }
                        nSL++;
                        nTableRow++;
                        nRowIndex++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.TransactionDateInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormat(oItem.ConversionRate); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (oItem.IsDebit)
                        {

                            nTotalDebitAmountInCurrency = nTotalDebitAmountInCurrency + oItem.AmountInCurrency;
                            nTotalCreditAmountInCurrency = nTotalCreditAmountInCurrency + 0.00;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Symbol + " " + Global.MillionFormat(oItem.AmountInCurrency); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = 0; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            nTotalDebitAmount = nTotalDebitAmount + oItem.Amount;
                            nTotalCreditAmount = nTotalCreditAmount + 0.00;
                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = Global.MillionFormat(oItem.Amount); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = 0; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        else
                        {

                            nTotalDebitAmountInCurrency = nTotalDebitAmountInCurrency + 0.00;
                            nTotalCreditAmountInCurrency = nTotalCreditAmountInCurrency + oItem.AmountInCurrency;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "0"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.Symbol + " " + Global.MillionFormat(oItem.AmountInCurrency); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            nTotalDebitAmount = nTotalDebitAmount + 0.00;
                            nTotalCreditAmount = nTotalCreditAmount + oItem.Amount;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "0"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = Global.MillionFormat(oItem.Amount); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }


                        nVorderType = (int)oItem.VOrderRefType;
                        nLCID = oItem.LCID;
                        nEndRow = nRowIndex;


                    }

                    #region SubTotal
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 6]; cell.Value = " "; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmountInCurrency); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmountInCurrency); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion

                    #endregion
                }
                else if (oVOrderRegister.ReportLayoutInt == (int)EnumReportLayout.Export_LC_PI_Summery)
                {

                    #region Report Body & Header
                    sReportHeader = "Order Reference Register PI Wise Summery";
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//sl      
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Order No
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//A/H
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//C.Rate
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Debit
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Credit
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//debit(Base curr) 
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Crdit(Base curr) 

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


                    #endregion
                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #region Data PI Sumery
                    int nLCID = 0; int nTableRow = 0, nVorderType = 0;
                    double nTotalDebitAmount = 0, nTotalCreditAmount = 0, nTotalDebitAmountInCurrency = 0, nTotalCreditAmountInCurrency = 0;

                    foreach (VOrderRegister oItem in _oVOrderRegisters)
                    {

                        if (nLCID != oItem.LCID)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 4]; cell.Value = " "; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                #endregion

                                #region Blank space
                                nRowIndex++;
                                #endregion

                            }
                            #region Header
                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 3]; cell.Value = "LC No : " + oItem.LCNo; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4, nRowIndex, 8]; cell.Value = "Party Name: " + oItem.SubledgerName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Account Head"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "C.Rate"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Debit (" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Credit (" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            #endregion
                            #endregion
                            nVorderType = (int)oItem.VOrderRefType;
                            nTotalDebitAmount = 0.00; nTotalDebitAmountInCurrency = 0;
                            nTotalCreditAmount = 0.00; nTotalCreditAmountInCurrency = 0;
                            nSL = 0;
                        }
                        if (nVorderType != (int)oItem.VOrderRefType)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 4]; cell.Value = " "; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                #endregion
                                nTableRow = 0;
                            }
                            nTotalDebitAmount = 0.00; nTotalDebitAmountInCurrency = 0;
                            nTotalCreditAmount = 0.00; nTotalCreditAmountInCurrency = 0;
                            nSL = 0;
                        }
                        nSL++;
                        nTableRow++;
                        nRowIndex++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = Global.MillionFormat(oItem.ConversionRate); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (oItem.IsDebit)
                        {

                            nTotalDebitAmountInCurrency = nTotalDebitAmountInCurrency + oItem.AmountInCurrency;
                            nTotalCreditAmountInCurrency = nTotalCreditAmountInCurrency + 0.00;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Symbol + " " + Global.MillionFormat(oItem.AmountInCurrency); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = 0; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            nTotalDebitAmount = nTotalDebitAmount + oItem.Amount;
                            nTotalCreditAmount = nTotalCreditAmount + 0.00;
                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormat(oItem.Amount); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = 0; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        else
                        {

                            nTotalDebitAmountInCurrency = nTotalDebitAmountInCurrency + 0.00;
                            nTotalCreditAmountInCurrency = nTotalCreditAmountInCurrency + oItem.AmountInCurrency;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = 0; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Symbol + " " + Global.MillionFormat(oItem.AmountInCurrency); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            nTotalDebitAmount = nTotalDebitAmount + 0.00;
                            nTotalCreditAmount = nTotalCreditAmount + oItem.Amount;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "0"; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormat(oItem.Amount); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }


                        nVorderType = (int)oItem.VOrderRefType;
                        nLCID = oItem.LCID;
                        nEndRow = nRowIndex;
                    }

                    #region SubTotal
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 4]; cell.Value = " "; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmountInCurrency); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmountInCurrency); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion

                    #endregion
                }
                else if (oVOrderRegister.ReportLayoutInt == (int)EnumReportLayout.Export_LC_Summery)
                {


                    #region Report Body & Header
                    sReportHeader = "Order Reference Register LC Summery";
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//sl      
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//A/H
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//C.Rate
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Debit
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Credit
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//debit(Base curr) 
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Crdit(Base curr) 
                    #endregion
                    nEndCol = nTempCol + 1;
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oBU.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    cell = sheet.Cells[nRowIndex, nEndCol - 4, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = sReportHeader; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;


                    #endregion
                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #region Data (Paty Wise Details)
                    int nLCID = 0; int nTableRow = 0;
                    double nTotalDebitAmount = 0, nTotalCreditAmount = 0, nTotalDebitAmountInCurrency = 0, nTotalCreditAmountInCurrency = 0;

                    foreach (VOrderRegister oItem in _oVOrderRegisters)
                    {

                        if (nLCID != oItem.LCID)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                nRowIndex++;
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 3]; cell.Value = " "; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 4]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmountInCurrency); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmount); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                #endregion

                                #region Blank space
                                nRowIndex++;
                                #endregion

                                nTableRow = 0;
                            }
                            #region Header
                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex, 7]; cell.Value = "LC No : " + oItem.LCNo; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[nRowIndex, 4, nRowIndex, 7]; cell.Value = "Party Name: " + oItem.SubledgerName; cell.Style.Font.Bold = true;
                            //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                            //cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Account Head"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "C.Rate"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Debit (" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Credit (" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            #endregion
                            #endregion
                            nTotalDebitAmount = 0.00; nTotalDebitAmountInCurrency = 0;
                            nTotalCreditAmount = 0.00; nTotalCreditAmountInCurrency = 0;
                            nSL = 0;
                        }

                        nSL++;
                        nTableRow++;
                        nRowIndex++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = Global.MillionFormat(oItem.ConversionRate); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalDebitAmountInCurrency = nTotalDebitAmountInCurrency + oItem.AmountInCurrency;
                        nTotalCreditAmountInCurrency = nTotalCreditAmountInCurrency + oItem.CreditAmountInCurrency;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.Symbol + " " + Global.MillionFormat(oItem.AmountInCurrency); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Symbol + " " + Global.MillionFormat(oItem.CreditAmountInCurrency); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nTotalDebitAmount = nTotalDebitAmount + oItem.Amount;
                        nTotalCreditAmount = nTotalCreditAmount + oItem.CreditAmount;
                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormat(oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Symbol + " " + Global.MillionFormat(oItem.CreditAmount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nLCID = oItem.LCID;
                        nEndRow = nRowIndex;


                    }

                    #region SubTotal
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 3]; cell.Value = " "; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmountInCurrency); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmountInCurrency); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = Global.MillionFormatActualDigit(nTotalDebitAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(nTotalCreditAmount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion

                    #endregion
                }

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Order Reference Register.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }

        #endregion

        #region Print
        public ActionResult PrintTransaction(int id, int nCurrencyID, string StartDate, string EndDate, double ts)
        {
            _oVOrder = new VOrder();
            _oVOrder = _oVOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (_oVOrder.VOrderRefType == EnumVOrderRefType.ExportPI)
            {
                string sSQL = "";
                if (StartDate == "" || EndDate == "")
                {
                    sSQL = "SELECT * FROM View_VOReference WHERE OrderID=" + id + "  OR OrderID IN (SELECT VO.VOrderID FROM VOrder AS VO WHERE VO.VOrderRefType = " + (int)EnumVOrderRefType.ExportPI + " AND VO.VOrderRefID IN (SELECT MP.ExportPIID FROM MasterPIMapping AS MP WHERE MP.MasterPIID IN (SELECT MM.VOrderRefID FROM VOrder AS MM WHERE MM.VOrderID = " + id + "))) OR OrderID IN (SELECT VO.VOrderID FROM VOrder AS VO WHERE VO.VOrderRefType = " + (int)EnumVOrderRefType.ExportPI + " AND VO.VOrderRefID IN (SELECT MP.MasterPIID FROM MasterPIMapping AS MP WHERE MP.ExportPIID IN (SELECT MM.VOrderRefID FROM VOrder AS MM WHERE MM.VOrderID = " + id + ")))  ORDER BY AccountHeadID ASC";
                }
                else
                {
                    sSQL = "SELECT * FROM View_VOReference WHERE VoucherDate BETWEEN '" + Convert.ToDateTime(StartDate).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(EndDate).ToString("dd MMM yyyy") + "' AND OrderID=" + id + "  OR OrderID IN (SELECT VO.VOrderID FROM VOrder AS VO WHERE VO.VOrderRefType = " + (int)EnumVOrderRefType.ExportPI + " AND VO.VOrderRefID IN (SELECT MP.ExportPIID FROM MasterPIMapping AS MP WHERE MP.MasterPIID IN (SELECT MM.VOrderRefID FROM VOrder AS MM WHERE MM.VOrderID = " + id + "))) OR OrderID IN (SELECT VO.VOrderID FROM VOrder AS VO WHERE VO.VOrderRefType = " + (int)EnumVOrderRefType.ExportPI + " AND VO.VOrderRefID IN (SELECT MP.MasterPIID FROM MasterPIMapping AS MP WHERE MP.ExportPIID IN (SELECT MM.VOrderRefID FROM VOrder AS MM WHERE MM.VOrderID = " + id + ")))  ORDER BY AccountHeadID ASC";
                }
                
                _oVOrder.VOReferences = VOReference.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oVOrder.VOReferences = VOReference.GetsByOrder(id, (int)Session[SessionInfo.currentUserID]);
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetWithImage(_oVOrder.BUID, (int)Session[SessionInfo.currentUserID]);
            oBusinessUnit.BULogo = GetBULogo(oBusinessUnit);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nCurrencyID == oCompany.BaseCurrencyID)
            {
                foreach (VOReference oItem in _oVOrder.VOReferences)
                {
                    oItem.AmountInCurrency = oItem.Amount;
                    oItem.CurrencyID = nCurrencyID;
                    oItem.Symbol = oCompany.CurrencySymbol;
                    oItem.CurrencyName = oCompany.CurrencyName;
                }
            }
            rptVOTransaction oReport = new rptVOTransaction();
            byte[] abytes = oReport.PrepareReport(_oVOrder, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintVOrders(string ids, double ts)
        {
            _oVOrders = new List<VOrder>();
            string sSql = "SELECT * FROM View_VOrder WHERE VOrderID IN (" + ids + ") ORDER BY VOrderID";
            _oVOrders = VOrder.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptVOrders oReport = new rptVOrders();
            byte[] abytes = oReport.PrepareReport(_oVOrders, oCompany);
            return File(abytes, "application/pdf");
        }
        public Image GetCompanyLogo(Company oCompany)
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
        public Image GetBULogo(BusinessUnit oBusinessUnit)
        {
            if (oBusinessUnit.BUImage != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oBusinessUnit.BUImage);
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

        #region Advance Search
        [HttpPost]
        public JsonResult AdvanceSearch(VOrder oVOrder)
        {
            _oVOrders = new List<VOrder>();
            try
            {
                string sSQL = this.GetSQL(oVOrder.Remarks);
                _oVOrders = VOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVOrder = new VOrder();
                _oVOrders = new List<VOrder>();
                _oVOrder.ErrorMessage = ex.Message;
                _oVOrders.Add(_oVOrder);

            }

            var jsonResult = Json(_oVOrders, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQL(string sSearchingData)
        {
            string sRefNo = Convert.ToString(sSearchingData.Split('~')[0]);
            string sOrderNo = Convert.ToString(sSearchingData.Split('~')[1]);
            int nBUID = Convert.ToInt32(sSearchingData.Split('~')[2]);
            int nRefType = Convert.ToInt32(sSearchingData.Split('~')[3]);
            EnumCompareOperator eOrderDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[4]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[6]);
            string sRemarks = Convert.ToString(sSearchingData.Split('~')[7]);
            string sLCNo = Convert.ToString(sSearchingData.Split('~')[8]);
            string sSubledgerIDs = Convert.ToString(sSearchingData.Split('~')[9]);

            string sReturn1 = "SELECT * FROM View_VOrder";
            string sReturn = "";

            #region Business Unit
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID =" + nBUID.ToString();
            }
            #endregion

            #region RefType
            if (nRefType != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VOrderRefType =" + nRefType.ToString();
            }
            #endregion

            #region RefNo
            if (sRefNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo LIKE '%" + sRefNo + "%'";
            }
            #endregion

            #region OrderNo
            if (sOrderNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderNo LIKE '%" + sOrderNo + "%'";
            }
            #endregion

            #region VOrder Date
            if (eOrderDate != EnumCompareOperator.None)
            {
                if (eOrderDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eOrderDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eOrderDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eOrderDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eOrderDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eOrderDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Subledger
            if (sSubledgerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SubledgerID IN (" + sSubledgerIDs + ")";
            }
            #endregion

            #region Remarks
            if (sRemarks != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Remarks LIKE '%" + sRemarks + "%'";
            }
            #endregion

            #region LCNo
            if (sLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCNo LIKE '%" + sLCNo + "%'";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region VOSummery
        public ActionResult ViewVOrderSummery(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            VOSummery oVOSummery = new VOSummery();
            List<VOSummery> oVOSummerys = new List<VOSummery>();
            try
            {
                oVOSummery = (VOSummery)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oVOSummery = null;
            }

            if (oVOSummery != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oVOSummery.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oVOSummerys = VOSummery.GetsVOSummerys(oVOSummery, (int)Session[SessionInfo.currentUserID]);
                oVOSummery.VOSummerys = new List<VOSummery>();
                oVOSummery.VOSummerys = oVOSummerys;
            }
            else
            {
                oVOSummery = new VOSummery();
                oVOSummery.VOSummerys = new List<VOSummery>();
            }
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oVOSummery.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CC = new ACCostCenter().Get(oVOSummery.SubledgerID, (int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BalanceStatus = EnumObject.jGets(typeof(EnumBalanceStatus));
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVOSummery);
        }
        public ActionResult PrintVOrderSummery(decimal tsv)
        {
            VOSummery oVOSummery = new VOSummery();
            oVOSummery = (VOSummery)Session[SessionInfo.ParamObj];
            byte[] abytes = null;
            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(oVOSummery.BUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (oVOSummery.BUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(oVOSummery.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
          
            oVOSummery.VOSummerys = new List<VOSummery>();
            oVOSummery.VOSummerys = VOSummery.GetsVOSummerys(oVOSummery, (int)Session[SessionInfo.currentUserID]);
            rptVOrderSummery rptVOrderSummery = new rptVOrderSummery();
            abytes = rptVOrderSummery.PrepareReport(oVOSummery.VOSummerys, oVOSummery.ErrorMessage, oCompany, (EnumBalanceStatus)oVOSummery.BalanceStatusInt);

            return File(abytes, "application/pdf");
        }

        public void ExportToExcelVOrderSummery(decimal tsv)
        {


            #region Dataget
            VOSummery oVOSummery = new VOSummery();
            oVOSummery = (VOSummery)Session[SessionInfo.ParamObj];
            byte[] abytes = null;
            //#region Check Authorize Business Unit
            //if (!BusinessUnit.IsPermittedBU(oVOSummery.BUID, (int)Session[SessionInfo.currentUserID]))
            //{
            //    rptErrorMessage oErrorReport = new rptErrorMessage();
            //    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
            //    return File(aErrorMessagebytes, "application/pdf");
            //}
            //#endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (oVOSummery.BUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(oVOSummery.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }

            oVOSummery.VOSummerys = new List<VOSummery>();
            oVOSummery.VOSummerys = VOSummery.GetsVOSummerys(oVOSummery, (int)Session[SessionInfo.currentUserID]);
            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SubLedger Transactions");
                sheet.Name = "VOrder Transactions";
                sheet.Column(2).Width = 10;//SL
                sheet.Column(3).Width = 50;//Order No
                sheet.Column(4).Width = 10;//Due
                sheet.Column(5).Width = 20;//Openint
                sheet.Column(6).Width = 20;//debit
                sheet.Column(7).Width = 20;//creidt
                sheet.Column(8).Width = 20;//closing
                nEndCol = 8;

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
                cell.Value = oVOSummery.ErrorMessage; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "[All Amount Display in " + oCompany.CurrencyName + "]"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Due Day(s)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Opening Value"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Debit Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Credit Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Closing Value"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;
                foreach (VOSummery oItem in oVOSummery.VOSummerys)
                {
                    if (oItem.VOrderID != 0)
                    {

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.DueDays; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.OpeningAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ClosingBalance; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                   
                }
                #endregion

                #region Total
                VOSummery oTempVOSummery = new VOSummery();
                oTempVOSummery = oVOSummery.VOSummerys.Where(x => x.VOrderID == 0).FirstOrDefault();
                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = oTempVOSummery.OpeningAmount; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oTempVOSummery.DebitAmount; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = oTempVOSummery.CreditAmount; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = oTempVOSummery.CreditAmount; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Vorder_Transactions.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }        
      
        [HttpPost]
        public ActionResult SetSessionData(VOSummery oVOSummery)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oVOSummery);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Gets Ref Objects
         [HttpGet]
        public JsonResult GetsRefObjectsAutocomplete(string ObjRefNo, VOrder oVOrder)
        {
            List<EnumObject> oRefObjects = new List<EnumObject>();
            ObjRefNo = ObjRefNo == null ? "" : ObjRefNo;
            if (oVOrder.VOrderRefTypeInt == (int)EnumVOrderRefType.ImportLC)
            {
                oRefObjects = GetsImportLCs(ObjRefNo, oVOrder.BUID);
            }
            else if (oVOrder.VOrderRefTypeInt == (int)EnumVOrderRefType.ImportPI)
            {
                oRefObjects = GetsImportPIs(ObjRefNo, oVOrder.BUID);
            }
            else if (oVOrder.VOrderRefTypeInt == (int)EnumVOrderRefType.ExportPI)
            {
                oRefObjects = GetsExportPIs(ObjRefNo, oVOrder.BUID);
            }
            else if (oVOrder.VOrderRefTypeInt == (int)EnumVOrderRefType.ExportLC)
            {
                oRefObjects = GetsExportLCs(ObjRefNo, oVOrder.BUID);
            }
            var jsonResult = Json(oRefObjects, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            // Query is not required as of version 1.2.5
            //jsonResult = { "query": "Unit",    "suggestions": }
            return jsonResult;
        }
         private List<EnumObject> GetsExportLCs(string sExportLCNo, int nBUID)
         {
             EnumObject oRefObject = new EnumObject();
             List<EnumObject> oRefObjects = new List<EnumObject>();
             List<ExportLC> oExportLCs = new List<ExportLC>();
             string sSQL = "SELECT * FROM View_ExportLC AS HH WHERE HH.BUID=" + nBUID.ToString() + " AND HH.ExportLCNo LIKE '%" + sExportLCNo + "%' ORDER BY HH.ExportLCNo ASC";
             oExportLCs = ExportLC.GetsSQL(sSQL, (int)Session[SessionInfo.currentUserID]);

             foreach (ExportLC oItem in oExportLCs)
             {
                 oRefObject = new EnumObject();
                 oRefObject.id = oItem.ExportLCID;
                 oRefObject.Value = oItem.ExportLCNo;
                 oRefObject.Description = oItem.ApplicantName;
                 oRefObjects.Add(oRefObject);
             }
             return oRefObjects;
         }
         private List<EnumObject> GetsExportPIs(string sExportPINo, int nBUID)
         {
             EnumObject oRefObject = new EnumObject();
             List<EnumObject> oRefObjects = new List<EnumObject>();
             List<ExportPI> oExportPIs = new List<ExportPI>();
             string sSQL = "SELECT * FROM View_ExportPI AS HH WHERE HH.BUID=" + nBUID.ToString() + " AND HH.PINo LIKE '%" + sExportPINo + "%' ORDER BY HH.PINo ASC";
             oExportPIs = ExportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

             foreach (ExportPI oItem in oExportPIs)
             {
                 oRefObject = new EnumObject();
                 oRefObject.id = oItem.ExportPIID;
                 oRefObject.Value = oItem.PINo;
                 oRefObject.Description = oItem.ContractorName;
                 oRefObjects.Add(oRefObject);
             }
             return oRefObjects;
         }

         private List<EnumObject> GetsImportPIs(string sImportPINo, int nBUID)
         {
             EnumObject oRefObject = new EnumObject();
             List<EnumObject> oRefObjects = new List<EnumObject>();
             List<ImportPI> oImportPIs = new List<ImportPI>();
             string sSQL = "SELECT * FROM View_ImportPI AS HH WHERE HH.BUID=" + nBUID.ToString() + " AND HH.ImportPINo LIKE '%" + sImportPINo + "%' ORDER BY HH.ImportPINo ASC";
             oImportPIs = ImportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

             foreach (ImportPI oItem in oImportPIs)
             {
                 oRefObject = new EnumObject();
                 oRefObject.id = oItem.ImportPIID;
                 oRefObject.Value = oItem.ImportPINo;
                 oRefObject.Description = oItem.SupplierName;
                 oRefObjects.Add(oRefObject);
             }
             return oRefObjects;
         }
         private List<EnumObject> GetsImportLCs(string sImportLCNo, int nBUID)
         {
             EnumObject oRefObject = new EnumObject();
             List<EnumObject> oRefObjects = new List<EnumObject>();
             List<ImportLC> oImportLCs = new List<ImportLC>();
             string sSQL = "SELECT * FROM View_ImportLC AS HH WHERE HH.BUID=" + nBUID.ToString() + " AND HH.ImportLCNo LIKE '%" + sImportLCNo + "%' ORDER BY HH.ImportLCNo ASC";
             oImportLCs = ImportLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

             foreach (ImportLC oItem in oImportLCs)
             {
                 oRefObject = new EnumObject();
                 oRefObject.id = oItem.ImportLCID;
                 oRefObject.Value = oItem.ImportLCNo;
                 oRefObject.Description = oItem.ContractorName;
                 oRefObjects.Add(oRefObject);
             }
             return oRefObjects;
         }
        #endregion
    }
}
