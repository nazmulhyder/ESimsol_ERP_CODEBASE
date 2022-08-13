using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Threading;
using ESimSolFinancial.Hubs;


namespace ESimSolFinancial.Controllers
{
    public class VoucherBillController : Controller
    {
        #region Declaration
        VoucherBill _oVoucherBill = new VoucherBill();
        List<VoucherBill> _oVoucherBills = new List<VoucherBill>();
        List<VoucherBill> _LstVoucherBill = new List<VoucherBill>();        
        string _sErrorMessage = "";
        #endregion

        #region VoucherBills
        public ActionResult ViewVoucherBills(int nAHID, double ntsv)
        {
            _oVoucherBills = new List<VoucherBill>();            
            _oVoucherBills = VoucherBill.GetsBy(nAHID, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oVoucherBills);
        }

        [HttpPost]
        public JsonResult Save(VoucherBill oVoucherBill)
        {
            _oVoucherBill = new VoucherBill();
            try
            {
                _oVoucherBill = oVoucherBill;               
                _oVoucherBill = _oVoucherBill.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVoucherBill = new VoucherBill();
                _oVoucherBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HoldUnHold(VoucherBill oVoucherBill)
        {
            _oVoucherBill = new VoucherBill();
            try
            {
                _oVoucherBill = oVoucherBill;
                _oVoucherBill = _oVoucherBill.HoldUnHold((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVoucherBill = new VoucherBill();
                _oVoucherBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RefreshControl(VoucherBill oVoucherBill)
        {
            _oVoucherBill = new VoucherBill();            
            _oVoucherBill = _oVoucherBill.Get(oVoucherBill.VoucherBillID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVoucherBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sfeedbackmessage = "";
            _oVoucherBill = new VoucherBill();
            try
            {
                sfeedbackmessage = _oVoucherBill.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sfeedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sfeedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Report
        public ActionResult ViewVoucherRefReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<VoucherRefReport> _oVoucherRefReports = new List<VoucherRefReport>();
            return View(_oVoucherRefReports);
        }
        [HttpPost]
        public JsonResult RefreshVoucherRefReport(VoucherRefReport oVRefR)
        {

            List<VoucherRefReport> _oVoucherRefReports = new List<VoucherRefReport>();
            VoucherRefReport _oVoucherRefReport = new VoucherRefReport();
            try
            {                
                _oVoucherRefReports = VoucherRefReport.GetsVoucherBillBreakDown( oVRefR, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVoucherRefReport.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherRefReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult GetsTrType(VoucherBill oVoucherBill)
        {
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            foreach (int oItem in Enum.GetValues(typeof(EnumVoucherBillTrType)))
            {
                if (oItem != 0)
                {
                    VoucherBillTransaction oVoucherBillTransaction = new VoucherBillTransaction();
                    oVoucherBillTransaction.EnumId = oItem;
                    oVoucherBillTransaction.EnumValue = ((EnumVoucherBillTrType)oItem).ToString();
                    oVoucherBillTransactions.Add(oVoucherBillTransaction);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVoucherBillTransactions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewNewVoucherBillPiker(int id, double ts)
        {
            VoucherBill oVoucherBill = new VoucherBill();            
            oVoucherBill = oVoucherBill.Get(id, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(oVoucherBill);
        }

        [HttpPost]
        public JsonResult GetsBill(VoucherBill oVoucherBill)
        {            
            if (oVoucherBill.BillNo == null) oVoucherBill.BillNo = "";
            string sSQL = "";
            if (oVoucherBill.TrTypeInInt == (int)EnumVoucherBillTrType.NewRef || oVoucherBill.TrTypeInInt == (int)EnumVoucherBillTrType.Advance)
            {
                sSQL = "SELECT * FROM View_VoucherBill WHERE BUID="+oVoucherBill.BUID.ToString()+" AND ((VoucherBillID NOT IN (SELECT BreakdownObjID FROM AccountOpenningBreakdown WHERE BreakdownType=" + ((int)EnumBreakdownType.BillReference).ToString() + ") AND VoucherBillID NOT IN (SELECT VoucherBillID FROM VoucherBillTransaction) AND BillNo LIKE ('%" + oVoucherBill.BillNo + "%') AND AccountHeadID = " + oVoucherBill.AccountHeadID.ToString();

                #region Sub Ledger
                sSQL = sSQL + "  AND SubLedgerID = " + oVoucherBill.SubLedgerID.ToString() + ""; //Here subledger 0/zero allow because of if bill pick for Sub ledger bill then must be get with subledger,other hand if bill pick for account head bill it's subledger id must be 0/Zero
                #endregion


                #region Close First Part
                sSQL = sSQL + ")";
                #endregion

                #region OR VoucherBill ID
                if (oVoucherBill.VoucherBillID > 0)
                {
                    sSQL = sSQL + "  OR VoucherBillID IN (SELECT HH.VoucherBillID FROM View_VoucherBillTransaction AS HH WHERE HH.CCID =" + oVoucherBill.SubLedgerID.ToString() + " AND HH.AccountHeadID =" + oVoucherBill.AccountHeadID.ToString() + " AND HH.VoucherID = " + oVoucherBill.VoucherBillID.ToString() + ")";
                }
                #endregion

                #region Close And Part
                sSQL = sSQL + ")";
                #endregion

            }
            else
            {
                sSQL = "SELECT * FROM View_VoucherBill WHERE RemainningBalance>0 AND BUID=" + oVoucherBill.BUID.ToString() + " AND IsHoldBill=0 AND AccountHeadID = " + oVoucherBill.AccountHeadID.ToString() + " AND IsDebit !='" + Convert.ToInt32(oVoucherBill.IsDebit).ToString() + "'";

                #region Sub Ledger
                sSQL = sSQL + "  AND SubLedgerID = " + oVoucherBill.SubLedgerID.ToString() + "";   //Here subledger 0/zero allow because of if bill pick for Sub ledger bill then must be get with subledger,other hand if bill pick for account head bill it's subledger id must be 0/Zero             
                #endregion

                #region OR Bill No
                if (oVoucherBill.BillNo != "")
                {
                    sSQL = sSQL + " AND (VoucherBillID IN (SELECT VoucherBillID FROM VoucherBillTransaction) OR VoucherBillID IN (SELECT BreakdownObjID FROM AccountOpenningBreakdown WHERE BreakdownType=2)) AND BillNo LIKE ('%" + oVoucherBill.BillNo + "%') ";
                }
                #endregion
            }
            _oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVoucherBills);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBillAO(VoucherBill oVoucherBill)
        {            
            if (oVoucherBill.BillNo == null) oVoucherBill.BillNo = "";
            string sSQL = "SELECT * FROM View_VoucherBill WHERE SubLedgerID=" + oVoucherBill.SubLedgerID.ToString() + " AND AccountHeadID = " + oVoucherBill.AccountHeadID.ToString() + " AND BillNo LIKE ('%" + oVoucherBill.BillNo + "%') AND (VoucherBillID IN (SELECT VoucherBillID FROM View_VoucherBillTransaction WHERE TrType NOT IN (" + (int)EnumVoucherBillTrType.NewRef + "," + (int)EnumVoucherBillTrType.Advance + ")) OR VoucherBillID NOT IN (SELECT VoucherBillID FROM View_VoucherBillTransaction))";
            _oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oVoucherBills);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBills(VoucherBill oVoucherBill)
        {
            if (oVoucherBill.BillNo == null) oVoucherBill.BillNo = "";
            if (oVoucherBill.AccountHeadName == null) oVoucherBill.AccountHeadName = "";
            if (oVoucherBill.SubLedgerName == null) oVoucherBill.SubLedgerName = "";
            string sSQL = GetSQL(oVoucherBill);
            _oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(_oVoucherBills, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string GetSQL(VoucherBill oVoucherBill)
        {            
            string sReturn1 = "SELECT * FROM View_VoucherBill";
            string sReturn = "";

            if (oVoucherBill.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oVoucherBill.BUID.ToString();
            }
            if (oVoucherBill.BillNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BillNo LIKE ('%" + oVoucherBill.BillNo + "%')";
            }
            if (oVoucherBill.AccountHeadName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " AccountHeadName LIKE ('%" + oVoucherBill.AccountHeadName + "%')";
            }
            if (oVoucherBill.SubLedgerName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SubLedgerName LIKE ('%" + oVoucherBill.SubLedgerName + "%')";
            }
            if (oVoucherBill.RemainningBalance == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RemainningBalance>0";
            }
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        #region Voucher Bill Configures
        public ActionResult ViewVoucherBillConfigures(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VoucherBill).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oVoucherBill = new VoucherBill();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(_oVoucherBill);
        }
        public ActionResult ViewBillTransactions(int id, double ts)
        {
            List<VoucherBillTransaction> oVBTs = new List<VoucherBillTransaction>();
            string sSQL = "SELECT * FROM View_VoucherBillTransaction WHERE VoucherBillID = " + id + " Order By TransactionDate";
            oVBTs = VoucherBillTransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(oVBTs);
        }

        [HttpPost]
        public ActionResult SetSessionData(VoucherBill oVoucherBill)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oVoucherBill);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VoucherBillRegister(bool blandscape)
        {
            int nBUID = 0;
            VoucherBill oVoucherBill = new VoucherBill();
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            try
            {
                oVoucherBill = (VoucherBill)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oVoucherBill = null;
            }
            if (oVoucherBill != null)
            {
                nBUID = oVoucherBill.BUID;
                string sSQL = this.GetSQL(oVoucherBill);
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oVoucherBill.VoucherBills = new List<VoucherBill>();
                oVoucherBill.VoucherBills = oVoucherBills;
            }
            else
            {
                oVoucherBill = new VoucherBill();
                oVoucherBill.VoucherBills = new List<VoucherBill>();
            }

            byte[] abytes = null;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);            
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany.Name = oBusinessUnit.Name;
                oCompany.Address = oBusinessUnit.Address;
                oCompany.Phone = oBusinessUnit.Phone;
                oCompany.Email = oBusinessUnit.Email;
                oCompany.WebAddress = oBusinessUnit.WebAddress;
            }
            oVoucherBill.Company = oCompany;
            string sMessage = "Bill Register";
            string sUserName = (string)Session[SessionInfo.currentUserName];

            rptVoucherBillConfigure orptVoucherBillConfigure = new rptVoucherBillConfigure();
            abytes = orptVoucherBillConfigure.PrepareReport(oVoucherBill, sMessage, blandscape, sUserName);
            return File(abytes, "application/pdf");
        }

        public void ExportToXL()
        {
            #region Data Gets
            int nBUID = 0;
            VoucherBill oVoucherBill = new VoucherBill();
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            try
            {
                oVoucherBill = (VoucherBill)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oVoucherBill = null;
            }
            if (oVoucherBill != null)
            {
                nBUID = oVoucherBill.BUID;
                string sSQL = this.GetSQL(oVoucherBill);
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oVoucherBill.VoucherBills = new List<VoucherBill>();
                oVoucherBill.VoucherBills = oVoucherBills;
            }
            else
            {
                oVoucherBill = new VoucherBill();
                oVoucherBill.VoucherBills = new List<VoucherBill>();
            }
                        
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany.Name = oBusinessUnit.Name;
                oCompany.Address = oBusinessUnit.Address;
                oCompany.Phone = oBusinessUnit.Phone;
                oCompany.Email = oBusinessUnit.Email;
                oCompany.WebAddress = oBusinessUnit.WebAddress;
            }
            oVoucherBill.Company = oCompany;
            string sMessage = "Bill Register";
            string sUserName = (string)Session[SessionInfo.currentUserName];
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
                var sheet = excelPackage.Workbook.Worksheets.Add("Voucher Bill Management");
                sheet.Name = "Voucher Bill Management";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 50;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 15;                
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                nEndCol = 9;

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
                cell.Value = sMessage; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;


                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Bill No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Account"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "SubLedger"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Bill Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Due Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Remaining Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 9];
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nAmount = 0, nRemaining = 0;
                foreach (VoucherBill oItem in oVoucherBills)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.BillNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.SubLedgerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.BillDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DueDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.RemainningBalance; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                    nEndRow = nRowIndex;
                    nRowIndex++;

                    nRemaining = nRemaining + oItem.Amount;
                    nAmount = nAmount + oItem.RemainningBalance;
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nRemaining; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nAmount; cell.Style.Font.Bold = true;
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
                Response.AddHeader("content-disposition", "attachment; filename=Voucher_Bill_Management.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion

        }
        #endregion
    }
}

