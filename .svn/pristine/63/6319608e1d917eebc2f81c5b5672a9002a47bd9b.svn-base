using System;
using System.Linq;
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
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Reflection;

namespace ESimSolFinancial.Controllers
{
    public class BillOutStandingController : Controller
    {
        #region Declaration
        List<VoucherBill> _oVoucherBills = new List<VoucherBill>();
        VoucherBill _oVoucherBill = new VoucherBill();
        #endregion

        #region New Version
        #region Functions
        private string MakeSQL(VoucherBill oVoucherBill, bool bIsGroup)
        {
            string sSQL = "", sSQL2 = "", sOrderBy;
            
            if(bIsGroup)
            {
                sSQL = "SELECT VB.AccountHeadID, VB.BUID, VB.SubLedgerID, VB.SubLedgerCode, VB.SubLedgerName, VB.BaseCurrencyID, VB.BaseCurrencySymbol, VB.CurrencyID, VB.CurrencySymbol, VB.CurrencyRate, SUM(VB.Amount) AS Amount, SUM(VB.NewRefAmount - VB.AgeRefAmount) AS RemainningBalance FROM View_VoucherBillMgt AS VB";
                sSQL2 = ""; sOrderBy = " GROUP BY VB.AccountHeadID, VB.BUID, VB.SubLedgerID, VB.SubLedgerCode, VB.SubLedgerName, VB.BaseCurrencyID, VB.BaseCurrencySymbol, VB.CurrencyID, VB.CurrencySymbol, VB.CurrencyRate ORDER BY VB.SubLedgerName ASC";
            }
            else
            {
                sSQL = "SELECT *, (VB.NewRefAmount - VB.AgeRefAmount) AS RemainningBalance FROM View_VoucherBillMgt AS VB";
                sSQL2 = ""; sOrderBy = " ORDER BY VB.DueDate ASC";
            }
            
                        
            #region AccountHead
            if (oVoucherBill.BUID>0)
            {
                Global.TagSQL(ref sSQL2);
                sSQL2 = sSQL2 + " VB.BUID=" + oVoucherBill.BUID;
            }
            #endregion

            #region AccountHead
            if (oVoucherBill.AccountHeadID>0)
            {
                Global.TagSQL(ref sSQL2);
                sSQL2 = sSQL2 + " VB.AccountHeadID=" + oVoucherBill.AccountHeadID;
            }
            #endregion

            #region SubLedger
            if (oVoucherBill.SubLedgerID >0)
            {
                Global.TagSQL(ref sSQL2);
                sSQL2 = sSQL2 + " VB.SubLedgerID=" + oVoucherBill.SubLedgerID;
            }
            #endregion

            #region Currency
            if (oVoucherBill.CurrencyID >0)
            {
                Global.TagSQL(ref sSQL2);
                sSQL2 = sSQL2 + " VB.CurrencyID=" + oVoucherBill.CurrencyID;
            }
            #endregion

            #region Due
            if (oVoucherBill.DueType == EnumDueType.OverDueBill)
            {
                Global.TagSQL(ref sSQL2);
                sSQL2 = sSQL2 + " VB.OverDueDays>=0 ";               
            }
            #endregion

            #region Bill Date
            if (oVoucherBill.BillDate != null)
            {
                Global.TagSQL(ref sSQL2);
                sSQL2 = sSQL2 + " CONVERT(DATE,CONVERT(VARCHAR(12),VB.BillDate,106))<=CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBill.BillDate.ToString("dd MMM yyyy") + "',106))";
            }
            #endregion

            #region RemainningBalance
            Global.TagSQL(ref sSQL2);
            sSQL2 = sSQL2 + " (VB.NewRefAmount - VB.AgeRefAmount)>0";
            #endregion

            #region Hold Bill Not Allow
            Global.TagSQL(ref sSQL2);
            sSQL2 = sSQL2 + " VB.IsHoldBill=0";
            #endregion

            if (sSQL2 != "")
            {
                sSQL = sSQL + sSQL2 + sOrderBy;
            }
            else
            {
                sSQL = sSQL + sOrderBy;
            }
            return sSQL;
        }
        private List<VoucherBillAging> PrepareBillAging(List<VoucherBill> oVoucherBills, EnumDueType eEnumDueType, List<VoucherBillAgeSlab> oVoucherBillAgeSlabs)
        {
            List<VoucherBillAging> oVoucherBillAgings = new List<VoucherBillAging>();
            foreach (VoucherBill oItem in oVoucherBills)
            {
                VoucherBillAging oVoucherBillAging = new VoucherBillAging();
                oVoucherBillAging.AccountHeadID = oItem.AccountHeadID;
                oVoucherBillAging.AccountHeadName = oItem.AccountHeadName;
                oVoucherBillAging.SubLedgerID = oItem.SubLedgerID;
                oVoucherBillAging.SubLedgerCode = oItem.SubLedgerCode;
                oVoucherBillAging.SubLedgerName = oItem.SubLedgerName;
                oVoucherBillAging.BillDate = oItem.BillDate;
                oVoucherBillAging.DueDate = oItem.DueDate;
                oVoucherBillAging.BillNo = oItem.BillNo;
                oVoucherBillAging.VoucherBillID = oItem.VoucherBillID;
                oVoucherBillAging.DueDays = oItem.DueDays;
                oVoucherBillAging.OverDueDays = oItem.OverDueDays;
                oVoucherBillAging.Amount = oItem.RemainningBalance;
                oVoucherBillAging.IsDebit = oItem.IsDebit;

                int nCount = 0;
                string sPropertyName = "";
                foreach (VoucherBillAgeSlab oSlab in oVoucherBillAgeSlabs)
                {
                    nCount++;
                    if (eEnumDueType  == EnumDueType.OverDueBill)
                    {
                        if (nCount == oVoucherBillAgeSlabs.Count)
                        {
                            if (oItem.OverDueByDays > oSlab.Start)
                            {
                                sPropertyName = "Slab" + nCount.ToString();
                                PropertyInfo prop = oVoucherBillAging.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                                if (null != prop && prop.CanWrite)
                                {
                                    prop.SetValue(oVoucherBillAging, oItem.RemainningBalance, null);
                                }
                            }
                        }
                        else
                        {
                            if (oItem.OverDueByDays >= oSlab.Start && oItem.OverDueByDays <= oSlab.End)
                            {
                                sPropertyName = "Slab" + nCount.ToString();
                                PropertyInfo prop = oVoucherBillAging.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                                if (null != prop && prop.CanWrite)
                                {
                                    prop.SetValue(oVoucherBillAging, oItem.RemainningBalance, null);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (nCount == oVoucherBillAgeSlabs.Count)
                        {
                            if (oItem.DueForDays > oSlab.Start)
                            {
                                sPropertyName = "Slab" + nCount.ToString();
                                PropertyInfo prop = oVoucherBillAging.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                                if (null != prop && prop.CanWrite)
                                {
                                    prop.SetValue(oVoucherBillAging, oItem.RemainningBalance, null);
                                }
                            }
                        }
                        else
                        {
                            if (oItem.DueForDays >= oSlab.Start && oItem.DueForDays <= oSlab.End)
                            {
                                sPropertyName = "Slab" + nCount.ToString();
                                PropertyInfo prop = oVoucherBillAging.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                                if (null != prop && prop.CanWrite)
                                {
                                    prop.SetValue(oVoucherBillAging, oItem.RemainningBalance, null);
                                }
                            }
                        }

                        
                    }
                }
                oVoucherBillAgings.Add(oVoucherBillAging);
            }
            return oVoucherBillAgings;
        }
        private List<VoucherBillAgeSlab> GenerateDefaultSlabs()
        {
            int nDiff = 29;
            List<VoucherBillAgeSlab> oVoucherBillAgeSlabs = new List<VoucherBillAgeSlab>();
            VoucherBillAgeSlab oVoucherBillAgeSlab = new VoucherBillAgeSlab();
            oVoucherBillAgeSlab = new VoucherBillAgeSlab();
            oVoucherBillAgeSlab.Start = 0;
            oVoucherBillAgeSlab.Separator = " To ";
            oVoucherBillAgeSlab.End = 30;
            oVoucherBillAgeSlabs.Add(oVoucherBillAgeSlab);
            for (int i = 0; i < 3; i++)
            {
                oVoucherBillAgeSlab = new VoucherBillAgeSlab();                
                if (i == 2)
                {
                    oVoucherBillAgeSlab.Start = oVoucherBillAgeSlabs[oVoucherBillAgeSlabs.Count - 1].End;
                    oVoucherBillAgeSlab.Separator = " Above ";
                    oVoucherBillAgeSlab.End = 0;
                }
                else
                {
                    oVoucherBillAgeSlab.Start = oVoucherBillAgeSlabs[oVoucherBillAgeSlabs.Count - 1].End + 1;
                    oVoucherBillAgeSlab.Separator = " To ";
                    oVoucherBillAgeSlab.End = oVoucherBillAgeSlab.Start + nDiff;
                }
                oVoucherBillAgeSlabs.Add(oVoucherBillAgeSlab);
            }
            return oVoucherBillAgeSlabs;
        }
        
        #endregion

        #region Set Bill SessionData
        [HttpPost]
        public ActionResult SetVBMSessionData(VoucherBill oVoucherBill)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oVoucherBill);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Bill Mgt
        public ActionResult ViewVoucherBillManagement(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

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
                oVoucherBill.DueType = (EnumDueType)oVoucherBill.DueTypeInt;
                string sSQL = this.MakeSQL(oVoucherBill, false);
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oVoucherBill.VoucherBills = new List<VoucherBill>();
                oVoucherBill.VoucherBills = oVoucherBills;
            }
            else
            {
                oVoucherBill = new VoucherBill();
                oVoucherBill.VoucherBills = new List<VoucherBill>();
            }
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oVoucherBill.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CC = new ACCostCenter().Get(oVoucherBill.SubLedgerID, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVoucherBill);
        }
          
        [HttpPost]
        public JsonResult GetsVoucherBillManagement(VoucherBill oVoucherBill)
        {            
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            try
            {
                oVoucherBill.DueType = (EnumDueType)oVoucherBill.DueTypeInt;
                string sSQL = this.MakeSQL(oVoucherBill, false);
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oVoucherBill = new VoucherBill();
                oVoucherBill.ErrorMessage = ex.Message;
                oVoucherBills = new List<VoucherBill>();
                oVoucherBills.Add(oVoucherBill);
            }
            var jsonResult = Json(oVoucherBills, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult PrintVoucherBillManagement(string Params)
        {
            int nDueType = Params.Split('~')[0] == null ? 1 : Params.Split('~')[0] == "" ? 1 : Convert.ToInt32(Params.Split('~')[0]);
            int nAccountHeadID = Params.Split('~')[1] == null ? 0 : Params.Split('~')[1] == "" ? 0 : Convert.ToInt32(Params.Split('~')[1]);
            int nCCID = Params.Split('~')[2] == null ? 0 : Params.Split('~')[2] == "" ? 0 : Convert.ToInt32(Params.Split('~')[2]);
            DateTime dBillDate = Params.Split('~')[3] == null ? DateTime.Today : Params.Split('~')[3] == "" ? DateTime.Today : Convert.ToDateTime(Params.Split('~')[3]);
            int nBUID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsLandscap = Params.Split('~')[5] == null ? true : Params.Split('~')[5] == "" ? true : Convert.ToBoolean(Params.Split('~')[5]);

            byte[] abytes = null;
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            VoucherBill oVoucherBill = new VoucherBill();
            oVoucherBill.DueType = (EnumDueType)nDueType;
            oVoucherBill.AccountHeadID = nAccountHeadID;
            oVoucherBill.SubLedgerID = nCCID;
            oVoucherBill.BillDate = dBillDate;
            oVoucherBill.BUID = nBUID;

            string sSQL = this.MakeSQL(oVoucherBill, false);
            oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.VoucherBills = new List<VoucherBill>();
            oVoucherBill.VoucherBills = oVoucherBills;

            #region Account Head & Subledger Name
            ACCostCenter oACCostCenter = new ACCostCenter();
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oACCostCenter = oACCostCenter.Get(nCCID, (int)Session[SessionInfo.currentUserID]);
            oChartsOfAccount = oChartsOfAccount.Get(nAccountHeadID, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.AccountHeadName = oChartsOfAccount.AccountHeadName;
            oVoucherBill.SubLedgerName = oACCostCenter.Name;
            if (oACCostCenter.ReferenceType == EnumReferenceType.Customer || oACCostCenter.ReferenceType == EnumReferenceType.Vendor || oACCostCenter.ReferenceType == EnumReferenceType.Vendor_Foreign)
            {
                Contractor oContractor = new Contractor();
                oContractor = oContractor.Get(oACCostCenter.ReferenceObjectID, (int)Session[SessionInfo.currentUserID]);
                oVoucherBill.PartyAddress = oContractor.Address;
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
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
            rptVoucherBills orptVoucherBills = new rptVoucherBills();
            string sMessage = "";
            if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = "Over Due Bills"; } else { sMessage = "Due Bills"; }
            string sUserName = (string)Session[SessionInfo.currentUserName];
            abytes = orptVoucherBills.PrepareReport(oVoucherBill, sMessage, bIsLandscap, sUserName);
            return File(abytes, "application/pdf");
        }
        public void ExportVBMToExcel(string Params)
        {
            #region Dataget
            int nDueType = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            int nAccountHeadID = Params.Split('~')[1] == null ? 0 : Params.Split('~')[1] == "" ? 0 : Convert.ToInt32(Params.Split('~')[1]);
            int nCCID = Params.Split('~')[2] == null ? 0 : Params.Split('~')[2] == "" ? 0 : Convert.ToInt32(Params.Split('~')[2]);
            DateTime dBillDate = Params.Split('~')[3] == null ? DateTime.Today : Params.Split('~')[3] == "" ? DateTime.Today : Convert.ToDateTime(Params.Split('~')[3]);
            int nBUID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);

            byte[] abytes = null;
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            VoucherBill oVoucherBill = new VoucherBill();
            oVoucherBill.DueType = (EnumDueType)nDueType;
            oVoucherBill.AccountHeadID = nAccountHeadID;
            oVoucherBill.SubLedgerID = nCCID;
            oVoucherBill.BillDate = dBillDate;
            oVoucherBill.BUID = nBUID;

            string sSQL = this.MakeSQL(oVoucherBill, false);
            oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.VoucherBills = new List<VoucherBill>();
            oVoucherBill.VoucherBills = oVoucherBills;

            #region Account Head & Subledger Name
            ACCostCenter oACCostCenter = new ACCostCenter();
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oACCostCenter = oACCostCenter.Get(nCCID, (int)Session[SessionInfo.currentUserID]);
            oChartsOfAccount = oChartsOfAccount.Get(nAccountHeadID, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.AccountHeadName = oChartsOfAccount.AccountHeadName;
            oVoucherBill.SubLedgerName = oACCostCenter.Name;
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
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
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                nEndCol = 10;

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

                string sMessage = "";
                if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = "Over Due Bills"; } else { sMessage = "Due Bills";} 
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

                sMessage = ""; 
                if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = "OverDue Days"; } else { sMessage = "Due Days"; }
                cell = sheet.Cells[nRowIndex, 8]; cell.Value = sMessage; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Remaining Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 10];
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

                    sMessage = "";
                    if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = oItem.OverDueByDays.ToString(); } else { sMessage = oItem.DueForDays.ToString(); }
                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = sMessage; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.RemainningBalance; cell.Style.Font.Bold = false;
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
                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nRemaining; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nAmount; cell.Style.Font.Bold = true;
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

        public ActionResult ViewSubLedgerWiseBillMgt(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

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
                oVoucherBill.DueType = (EnumDueType)oVoucherBill.DueTypeInt;
                string sSQL = this.MakeSQL(oVoucherBill, true);
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oVoucherBill.VoucherBills = new List<VoucherBill>();
                oVoucherBill.VoucherBills = oVoucherBills;
            }
            else
            {
                oVoucherBill = new VoucherBill();
                oVoucherBill.VoucherBills = new List<VoucherBill>();
            }
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oVoucherBill.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CC = new ACCostCenter().Get(oVoucherBill.SubLedgerID, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVoucherBill);
        }
        public ActionResult PrintSubLedgerWiseBillMgt(string Params)
        {
            int nDueType = Params.Split('~')[0] == null ? 1 : Params.Split('~')[0] == "" ? 1 : Convert.ToInt32(Params.Split('~')[0]);
            int nAccountHeadID = Params.Split('~')[1] == null ? 0 : Params.Split('~')[1] == "" ? 0 : Convert.ToInt32(Params.Split('~')[1]);
            int nCCID = Params.Split('~')[2] == null ? 0 : Params.Split('~')[2] == "" ? 0 : Convert.ToInt32(Params.Split('~')[2]);
            DateTime dBillDate = Params.Split('~')[3] == null ? DateTime.Today : Params.Split('~')[3] == "" ? DateTime.Today : Convert.ToDateTime(Params.Split('~')[3]);
            int nBUID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);

            byte[] abytes = null;
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            VoucherBill oVoucherBill = new VoucherBill();
            oVoucherBill.DueType = (EnumDueType)nDueType;
            oVoucherBill.AccountHeadID = nAccountHeadID;
            oVoucherBill.SubLedgerID = nCCID;
            oVoucherBill.BillDate = dBillDate;
            oVoucherBill.BUID = nBUID;

            string sSQL = this.MakeSQL(oVoucherBill, true);
            oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.VoucherBills = new List<VoucherBill>();
            oVoucherBill.VoucherBills = oVoucherBills;

            #region Account Head & Subledger Name
            ACCostCenter oACCostCenter = new ACCostCenter();
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oACCostCenter = oACCostCenter.Get(nCCID, (int)Session[SessionInfo.currentUserID]);
            oChartsOfAccount = oChartsOfAccount.Get(nAccountHeadID, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.AccountHeadName = oChartsOfAccount.AccountHeadName;
            oVoucherBill.SubLedgerName = oACCostCenter.Name;            
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
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
            rptVoucherBillMgt orptVoucherBillMgt = new rptVoucherBillMgt();
            string sMessage = "";
            if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = "Over Due Bills"; } else { sMessage = "Due Bills"; }
            abytes = orptVoucherBillMgt.PrepareReport(oVoucherBill, sMessage);
            return File(abytes, "application/pdf");
        }
        public void ExportSLBMgtToExcel(string Params)
        {
            #region Dataget
            int nDueType = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            int nAccountHeadID = Params.Split('~')[1] == null ? 0 : Params.Split('~')[1] == "" ? 0 : Convert.ToInt32(Params.Split('~')[1]);
            int nCCID = Params.Split('~')[2] == null ? 0 : Params.Split('~')[2] == "" ? 0 : Convert.ToInt32(Params.Split('~')[2]);
            DateTime dBillDate = Params.Split('~')[3] == null ? DateTime.Today : Params.Split('~')[3] == "" ? DateTime.Today : Convert.ToDateTime(Params.Split('~')[3]);
            int nBUID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);

            byte[] abytes = null;
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            VoucherBill oVoucherBill = new VoucherBill();
            oVoucherBill.DueType = (EnumDueType)nDueType;
            oVoucherBill.AccountHeadID = nAccountHeadID;
            oVoucherBill.SubLedgerID = nCCID;
            oVoucherBill.BillDate = dBillDate;
            oVoucherBill.BUID = nBUID;

            string sSQL = this.MakeSQL(oVoucherBill, true);
            oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.VoucherBills = new List<VoucherBill>();
            oVoucherBill.VoucherBills = oVoucherBills;

            #region Account Head & Subledger Name
            ACCostCenter oACCostCenter = new ACCostCenter();
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oACCostCenter = oACCostCenter.Get(nCCID, (int)Session[SessionInfo.currentUserID]);
            oChartsOfAccount = oChartsOfAccount.Get(nAccountHeadID, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.AccountHeadName = oChartsOfAccount.AccountHeadName;
            oVoucherBill.SubLedgerName = oACCostCenter.Name;
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
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
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 60;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;                
                nEndCol = 6;

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

                string sMessage = "";
                if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = "Over Due Bills"; } else { sMessage = "Due Bills"; }
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

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "SubLedger"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Remaining Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 6];
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nAmount = 0, nRemaining = 0;
                foreach (VoucherBill oItem in oVoucherBills)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.SubLedgerCode; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.SubLedgerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.RemainningBalance; cell.Style.Font.Bold = false;
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
                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nRemaining; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nAmount; cell.Style.Font.Bold = true;
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
        
        #region Bill Aging
        public ActionResult ViewVoucherBillAging(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

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
                oVoucherBill.DueType = (EnumDueType)oVoucherBill.DueTypeInt;
                string sSQL = this.MakeSQL(oVoucherBill, false);
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oVoucherBill.VoucherBillAgeSlabs = oVoucherBill.VoucherBillAgeSlabs.Where(x => (x.Start > 0 || x.End > 0)).ToList();
                oVoucherBill.VoucherBills = new List<VoucherBill>();
                oVoucherBill.VoucherBillAgings = PrepareBillAging(oVoucherBills, oVoucherBill.DueType, oVoucherBill.VoucherBillAgeSlabs);                
            }
            else
            {
                oVoucherBill = new VoucherBill();
                oVoucherBill.VoucherBills = new List<VoucherBill>();
                oVoucherBill.VoucherBillAgings = new List<VoucherBillAging>();              
                oVoucherBill.VoucherBillAgeSlabs = GenerateDefaultSlabs();
            }

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oVoucherBill.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CC = new ACCostCenter().Get(oVoucherBill.SubLedgerID, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVoucherBill);
        }
        public ActionResult PrintVoucherBillAging(string Params)
        {
            int nDueType = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            int nAccountHeadID = Params.Split('~')[1] == null ? 0 : Params.Split('~')[1] == "" ? 0 : Convert.ToInt32(Params.Split('~')[1]);
            int nCCID = Params.Split('~')[2] == null ? 0 : Params.Split('~')[2] == "" ? 0 : Convert.ToInt32(Params.Split('~')[2]);
            byte[] abytes = null;
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            VoucherBill oVoucherBill = new VoucherBill();
            oVoucherBill.DueType = (EnumDueType)nDueType;
            oVoucherBill.AccountHeadID = nAccountHeadID;
            oVoucherBill.SubLedgerID = nCCID;

            string sSQL = this.MakeSQL(oVoucherBill, false);
            oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oVoucherBill.VoucherBills = new List<VoucherBill>();
            oVoucherBill.VoucherBills = oVoucherBills;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oVoucherBill.Company = oCompany;
            rptVoucherBills orptVoucherBills = new rptVoucherBills();
            string sMessage = "";
            if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = "OverDue Bills"; } else { sMessage = "Due Bills"; }
            abytes = orptVoucherBills.PrepareReport(oVoucherBill, sMessage, true);
            return File(abytes, "application/pdf");
        }        
        public void ExportVBAToExcel()
        {
            #region Dataget
            VoucherBill oVoucherBill = new VoucherBill();
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            List<VoucherBillAging> oVoucherBillAgings = new List<VoucherBillAging>();
            List<VoucherBillAgeSlab> oVoucherBillAgeSlabs = new List<VoucherBillAgeSlab>();
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
                oVoucherBill.DueType = (EnumDueType)oVoucherBill.DueTypeInt;
                string sSQL = this.MakeSQL(oVoucherBill, false);
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oVoucherBill.VoucherBillAgeSlabs = oVoucherBill.VoucherBillAgeSlabs.Where(x => (x.Start > 0 || x.End > 0)).ToList();
                oVoucherBill.VoucherBills = new List<VoucherBill>();
                oVoucherBill.VoucherBills = oVoucherBills;
                oVoucherBill.VoucherBillAgings = PrepareBillAging(oVoucherBills, oVoucherBill.DueType, oVoucherBill.VoucherBillAgeSlabs);
                oVoucherBillAgings = oVoucherBill.VoucherBillAgings;
                oVoucherBillAgeSlabs = oVoucherBill.VoucherBillAgeSlabs;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oVoucherBill.Company = oCompany;

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Voucher Bill Aging");
                sheet.Name = "Voucher Bill Aging";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 20;


                for (int i = 1; i <= oVoucherBillAgeSlabs.Count;i++ )
                {
                    sheet.Column(i + 9).Width = 20;
                    nEndCol = i + 9;
                }
                
               

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

                string sMessage = "";
                if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = "OverDue Bill Aging"; } else { sMessage = "Due Bill Aging"; }
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

                sMessage = "";
                if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = "OverDue Days"; } else { sMessage = "Due Days"; }
                cell = sheet.Cells[nRowIndex, 8]; cell.Value = sMessage; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                int nHeaderCol = 9;
                foreach (VoucherBillAgeSlab oItem in oVoucherBillAgeSlabs)
                {
                    nHeaderCol++;
                    cell = sheet.Cells[nRowIndex, nHeaderCol]; cell.Value = oItem.Range; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                }
                

                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nAmount = 0, nRemaining = 0;
                foreach (VoucherBillAging oItem in oVoucherBillAgings)
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

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.BillDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DueDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    sMessage = "";
                    if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = oItem.OverDueByDays.ToString(); } else { sMessage = oItem.DueForDays.ToString(); }
                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = sMessage; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    int nDataColumn = 9, nSlab = 0;
                    string sPropertyName = "";
                    foreach (VoucherBillAgeSlab oVBAS in oVoucherBillAgeSlabs)
                    {
                        nDataColumn++;
                        nSlab++;
                        sPropertyName = "Slab" + nSlab.ToString();
                        PropertyInfo prop = oItem.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            cell = sheet.Cells[nRowIndex, nDataColumn]; cell.Value = Convert.ToDouble(prop.GetValue(oItem)); ; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        }
                    }
                    


                    nEndRow = nRowIndex;
                    nRowIndex++;

                    nRemaining = nRemaining + oItem.Amount;
                    //nAmount = nAmount + oItem.RemainningBalance;
                }
                #endregion

                #region Total
                string sStartCell = "", sEndCell = "";

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sStartCell = Global.GetExcelCellName(nStartRow + 1, 9);
                sEndCell = Global.GetExcelCellName(nEndRow, 9);
                cell = sheet.Cells[nRowIndex, 9]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                for (int i = 1; i <= oVoucherBillAgeSlabs.Count; i++)
                {
                    sStartCell = Global.GetExcelCellName(nStartRow + 1, i + 9);
                    sEndCell = Global.GetExcelCellName(nEndRow, i + 9);
                    cell = sheet.Cells[nRowIndex, i + 9]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                }



                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                this.Session.Remove(SessionInfo.ParamObj);
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Voucher_Bill_Aging.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        
        public ActionResult ViewSLBillAging(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

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
                oVoucherBill.DueType = (EnumDueType)oVoucherBill.DueTypeInt;
                string sSQL = this.MakeSQL(oVoucherBill, false);
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oVoucherBill.VoucherBillAgeSlabs = oVoucherBill.VoucherBillAgeSlabs.Where(x => (x.Start > 0 || x.End > 0)).ToList();
                oVoucherBill.VoucherBills = new List<VoucherBill>();                
                //oVoucherBill.VoucherBillAgings = PrepareBillAging(oVoucherBills, oVoucherBill.DueType, oVoucherBill.VoucherBillAgeSlabs);

                List<VoucherBillAging> oVoucherBillAgings = PrepareBillAging(oVoucherBills, oVoucherBill.DueType, oVoucherBill.VoucherBillAgeSlabs);
                List<VoucherBillAging> oBillAgings = oVoucherBillAgings
                                                    .GroupBy(vbag => new { vbag.SubLedgerID, vbag.IsDebit })
                                                    .Select(vba => new VoucherBillAging
                                                    {
                                                        AccountHeadID = vba.Last().AccountHeadID,
                                                        AccountHeadName = vba.Last().AccountHeadName,
                                                        SubLedgerID = vba.Last().SubLedgerID,
                                                        SubLedgerCode = vba.Last().SubLedgerCode,
                                                        SubLedgerName = vba.Last().SubLedgerName,
                                                        IsDebit = vba.Last().IsDebit,
                                                        Amount = vba.Sum(c => c.Amount),
                                                        Slab1 = vba.Sum(c => c.Slab1),
                                                        Slab2 = vba.Sum(c => c.Slab2),
                                                        Slab3 = vba.Sum(c => c.Slab3),
                                                        Slab4 = vba.Sum(c => c.Slab4),
                                                        Slab5 = vba.Sum(c => c.Slab5),
                                                        Slab6 = vba.Sum(c => c.Slab6),
                                                        Slab7 = vba.Sum(c => c.Slab7),
                                                        Slab8 = vba.Sum(c => c.Slab8),
                                                        Slab9 = vba.Sum(c => c.Slab9),
                                                        Slab10 = vba.Sum(c => c.Slab10),
                                                        Slab11 = vba.Sum(c => c.Slab11),
                                                        Slab12 = vba.Sum(c => c.Slab12),
                                                        Slab13 = vba.Sum(c => c.Slab13),
                                                        Slab14 = vba.Sum(c => c.Slab14),
                                                        Slab15 = vba.Sum(c => c.Slab15),
                                                        Slab16 = vba.Sum(c => c.Slab16),
                                                        Slab17 = vba.Sum(c => c.Slab17),
                                                        Slab18 = vba.Sum(c => c.Slab18),
                                                        Slab19 = vba.Sum(c => c.Slab19),
                                                        Slab20 = vba.Sum(c => c.Slab20)
                                                    }).ToList();
                oVoucherBill.VoucherBillAgings = oBillAgings;
            }
            else
            {
                oVoucherBill = new VoucherBill();
                oVoucherBill.VoucherBills = new List<VoucherBill>();
                oVoucherBill.VoucherBillAgings = new List<VoucherBillAging>();
                oVoucherBill.VoucherBillAgeSlabs = GenerateDefaultSlabs();
            }
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oVoucherBill.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CC = new ACCostCenter().Get(oVoucherBill.SubLedgerID, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVoucherBill);
        }

        public void ExportSLVBAToExcel()
        {
            #region Dataget
            VoucherBill oVoucherBill = new VoucherBill();
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            List<VoucherBillAging> oVoucherBillAgings = new List<VoucherBillAging>();
            List<VoucherBillAgeSlab> oVoucherBillAgeSlabs = new List<VoucherBillAgeSlab>();
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
                oVoucherBill.DueType = (EnumDueType)oVoucherBill.DueTypeInt;
                string sSQL = this.MakeSQL(oVoucherBill, false);
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oVoucherBill.VoucherBillAgeSlabs = oVoucherBill.VoucherBillAgeSlabs.Where(x => (x.Start > 0 || x.End > 0)).ToList();
                oVoucherBill.VoucherBills = new List<VoucherBill>();
                oVoucherBill.VoucherBills = oVoucherBills;
                //oVoucherBill.VoucherBillAgings = PrepareBillAging(oVoucherBills, oVoucherBill.DueType, oVoucherBill.VoucherBillAgeSlabs);

                oVoucherBillAgings = PrepareBillAging(oVoucherBills, oVoucherBill.DueType, oVoucherBill.VoucherBillAgeSlabs);
                List<VoucherBillAging> oBillAgings = oVoucherBillAgings
                                                    .GroupBy(vbag => new { vbag.SubLedgerID, vbag.IsDebit })
                                                    .Select(vba => new VoucherBillAging
                                                    {
                                                        AccountHeadID = vba.Last().AccountHeadID,
                                                        AccountHeadName = vba.Last().AccountHeadName,
                                                        SubLedgerID = vba.Last().SubLedgerID,
                                                        SubLedgerCode = vba.Last().SubLedgerCode,
                                                        SubLedgerName = vba.Last().SubLedgerName,
                                                        IsDebit = vba.Last().IsDebit,
                                                        Amount = vba.Sum(c => c.Amount),
                                                        Slab1 = vba.Sum(c => c.Slab1),
                                                        Slab2 = vba.Sum(c => c.Slab2),
                                                        Slab3 = vba.Sum(c => c.Slab3),
                                                        Slab4 = vba.Sum(c => c.Slab4),
                                                        Slab5 = vba.Sum(c => c.Slab5),
                                                        Slab6 = vba.Sum(c => c.Slab6),
                                                        Slab7 = vba.Sum(c => c.Slab7),
                                                        Slab8 = vba.Sum(c => c.Slab8),
                                                        Slab9 = vba.Sum(c => c.Slab9),
                                                        Slab10 = vba.Sum(c => c.Slab10),
                                                        Slab11 = vba.Sum(c => c.Slab11),
                                                        Slab12 = vba.Sum(c => c.Slab12),
                                                        Slab13 = vba.Sum(c => c.Slab13),
                                                        Slab14 = vba.Sum(c => c.Slab14),
                                                        Slab15 = vba.Sum(c => c.Slab15),
                                                        Slab16 = vba.Sum(c => c.Slab16),
                                                        Slab17 = vba.Sum(c => c.Slab17),
                                                        Slab18 = vba.Sum(c => c.Slab18),
                                                        Slab19 = vba.Sum(c => c.Slab19),
                                                        Slab20 = vba.Sum(c => c.Slab20)
                                                    }).ToList();
                oVoucherBill.VoucherBillAgings = oBillAgings;              
                oVoucherBillAgings = oVoucherBill.VoucherBillAgings;
                oVoucherBillAgeSlabs = oVoucherBill.VoucherBillAgeSlabs;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oVoucherBill.Company = oCompany;

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Voucher Bill Aging");
                sheet.Name = "Voucher Bill Aging";
                sheet.Column(2).Width = 10; //SL No
                sheet.Column(3).Width = 15; //Code                
                sheet.Column(4).Width = 50; //SubLedger                
                sheet.Column(5).Width = 20; //Amount
                for (int i = 1; i <= oVoucherBillAgeSlabs.Count; i++)
                {
                    sheet.Column(i + 5).Width = 20;
                    nEndCol = i + 5;
                }

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

                string sMessage = "";
                if (oVoucherBill.DueType == EnumDueType.OverDueBill) { sMessage = "OverDue Bill Aging"; } else { sMessage = "Due Bill Aging"; }
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

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Subledger Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "SubLedger"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                int nHeaderCol = 5;
                foreach (VoucherBillAgeSlab oItem in oVoucherBillAgeSlabs)
                {
                    nHeaderCol++;
                    cell = sheet.Cells[nRowIndex, nHeaderCol]; cell.Value = oItem.Range; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                }


                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nAmount = 0, nRemaining = 0;
                foreach (VoucherBillAging oItem in oVoucherBillAgings)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.SubLedgerCode; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    
                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.SubLedgerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    int nDataColumn = 5, nSlab = 0;
                    string sPropertyName = "";
                    foreach (VoucherBillAgeSlab oVBAS in oVoucherBillAgeSlabs)
                    {
                        nDataColumn++;
                        nSlab++;
                        sPropertyName = "Slab" + nSlab.ToString();
                        PropertyInfo prop = oItem.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            cell = sheet.Cells[nRowIndex, nDataColumn]; cell.Value = Convert.ToDouble(prop.GetValue(oItem)); ; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        }
                    }



                    nEndRow = nRowIndex;
                    nRowIndex++;

                    nRemaining = nRemaining + oItem.Amount;
                    //nAmount = nAmount + oItem.RemainningBalance;
                }
                #endregion

                #region Total
                string sStartCell = "", sEndCell = "";

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sStartCell = Global.GetExcelCellName(nStartRow + 1, 5);
                sEndCell = Global.GetExcelCellName(nEndRow, 5);
                cell = sheet.Cells[nRowIndex, 5]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                for (int i = 1; i <= oVoucherBillAgeSlabs.Count; i++)
                {
                    sStartCell = Global.GetExcelCellName(nStartRow + 1, i + 5);
                    sEndCell = Global.GetExcelCellName(nEndRow, i + 5);
                    cell = sheet.Cells[nRowIndex, i + 5]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                }



                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                this.Session.Remove(SessionInfo.ParamObj);
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Voucher_Bill_Aging.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion
        #endregion

        #region ReceivableBill
        public ActionResult ViewReceivableBills(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oVoucherBills = VoucherBill.GetsReceivableOrPayableBill((int)EnumComponentType.Asset, (int)Session[SessionInfo.currentUserID]);
            return View(_oVoucherBills);
        }

        public ActionResult PrintReceivableBills()
        {
            _oVoucherBill.VoucherBills = VoucherBill.GetsReceivableOrPayableBill((int)EnumComponentType.Asset, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get((int)Session[SessionInfo.CurrentCompanyID], (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oVoucherBill.Company = oCompany;
            rptVoucherBills orptVoucherBills = new rptVoucherBills();
            string sMessage = "Receivable Bills";
            byte[] abytes = orptVoucherBills.PrepareReport(_oVoucherBill, sMessage, true);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintReceivableBillInXL(string sParams)
        {
            _oVoucherBills = VoucherBill.GetsReceivableOrPayableBill((int)EnumComponentType.Asset, (int)Session[SessionInfo.currentUserID]);
            
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ReceivableOrPayableBillXL>));


            int nCount = 0; 
            ReceivableOrPayableBillXL oReceivableOrPayableBillXL = new ReceivableOrPayableBillXL();
            List<ReceivableOrPayableBillXL> oReceivableOrPayableBillXLs = new List<ReceivableOrPayableBillXL>();
            foreach (VoucherBill oItem in _oVoucherBills)
            {
                nCount++;
                oReceivableOrPayableBillXL = new ReceivableOrPayableBillXL();
                oReceivableOrPayableBillXL.SLNo = nCount.ToString();
                oReceivableOrPayableBillXL.BillNo = oItem.BillNo;
                oReceivableOrPayableBillXL.AccountHeadName = oItem.AccountHeadName;
                oReceivableOrPayableBillXL.BillDateString = oItem.BillDate.ToString("dd MMM yyy");
                oReceivableOrPayableBillXL.DueDateString = oItem.DueDate.ToString("dd MMM yyy");;
                oReceivableOrPayableBillXL.Amount = oItem.Amount;
                oReceivableOrPayableBillXL.RemainningBalance = oItem.RemainningBalance * oItem.CurrencyConversionRate;
                oReceivableOrPayableBillXLs.Add(oReceivableOrPayableBillXL);
            }

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oReceivableOrPayableBillXLs);
            stream.Position = 0;
            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "ReceivableOrPayableBill.xls");
        }

        #endregion

        #region Payable Bill
        public ActionResult ViewPayableBills(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oVoucherBills = VoucherBill.GetsReceivableOrPayableBill((int)EnumComponentType.Liability, (int)Session[SessionInfo.currentUserID]);
            return View(_oVoucherBills);
        }

        public ActionResult PrintPayableBills()
        {
            _oVoucherBill.VoucherBills = VoucherBill.GetsReceivableOrPayableBill((int)EnumComponentType.Liability, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get((int)Session[SessionInfo.CurrentCompanyID], (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oVoucherBill.Company = oCompany;
            rptVoucherBills orptVoucherBills = new rptVoucherBills();
            string sMessage = "Payable Bills";
            byte[] abytes = orptVoucherBills.PrepareReport(_oVoucherBill, sMessage, true);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintPayableBillInXL(string sParams)
        {
            _oVoucherBills = VoucherBill.GetsReceivableOrPayableBill((int)EnumComponentType.Liability, (int)Session[SessionInfo.currentUserID]);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ReceivableOrPayableBillXL>));

            int nCount = 0;
            ReceivableOrPayableBillXL oReceivableOrPayableBillXL = new ReceivableOrPayableBillXL();
            List<ReceivableOrPayableBillXL> oReceivableOrPayableBillXLs = new List<ReceivableOrPayableBillXL>();

            foreach (VoucherBill oItem in _oVoucherBills)
            {
                nCount++;
                oReceivableOrPayableBillXL = new ReceivableOrPayableBillXL();
                oReceivableOrPayableBillXL.SLNo = nCount.ToString();
                oReceivableOrPayableBillXL.BillNo = oItem.BillNo;
                oReceivableOrPayableBillXL.AccountHeadName = oItem.AccountHeadName;
                oReceivableOrPayableBillXL.BillDateString = oItem.BillDate.ToString("dd MMM yyy");
                oReceivableOrPayableBillXL.DueDateString = oItem.DueDate.ToString("dd MMM yyy"); ;
                oReceivableOrPayableBillXL.Amount = oItem.Amount;
                oReceivableOrPayableBillXL.RemainningBalance = oItem.RemainningBalance * oItem.CurrencyConversionRate;
                oReceivableOrPayableBillXLs.Add(oReceivableOrPayableBillXL);
            }

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oReceivableOrPayableBillXLs);
            stream.Position = 0;
            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "ReceivableOrPayableBill.xls");
        }

        #endregion

        #region Advance Search
        public ActionResult AdvanceSearch(double ts)
        {
            _oVoucherBill = new VoucherBill();
            _oVoucherBill.CompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            string sSQL = "SELECT * FROM View_AccountingSession WHERE SessionType=" + (int)EnumSessionType.YearEnd + " AND CompanyID=" + (int)Session[SessionInfo.CurrentCompanyID];
            _oVoucherBill.AccountingSessions = AccountingSession.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oVoucherBill);
        }

        [HttpPost]
        public JsonResult RefreshReceivableAndPayable(VoucherBill oVoucherBill)
        {
            try
            {
                string sSQL = MakeSQLOld(oVoucherBill);
                _oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVoucherBills = new List<VoucherBill>();
                _oVoucherBill.ErrorMessage = ex.Message;
                _oVoucherBills.Add(_oVoucherBill);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherBills);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string MakeSQLOld(VoucherBill oVoucherBill)
        {
            string sReturn1 = "SELECT * FROM View_VoucherBill AS VB ";
            string sReturn = "";            
            #region Company
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " VB.CompanyID= " + (int)Session[SessionInfo.CurrentCompanyID] + " ";
            #endregion

            #region Component Type
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " VB.AccountHeadID IN (SELECT COA.AccountHeadID FROM View_ChartsOfAccount AS COA WHERE COA.ComponentID=" + oVoucherBill.ComponentID + ") ";
            #endregion

            #region Remaining Balance
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " VB.RemainningBalance>0 ";
            #endregion

            #region Bill No
            if (oVoucherBill.BillNo != "")
            {
                if (oVoucherBill.BillNo != null)
                {                    
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " VB.BillNo LIKE '%" + oVoucherBill.BillNo + "%' ";
                }
            }
            #endregion

            #region Account Head IDs
            if (oVoucherBill.IDs != "")
            {
                if (oVoucherBill.IDs != null)
                {                    
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " VB.AccountHeadID IN (" + oVoucherBill.IDs + ") ";
                }
            }
            #endregion

            #region Accounting Session
            if (oVoucherBill.AccSessionID != 0)
            {
                AccountingSession oAccountingSession = new AccountingSession();
                oAccountingSession = oAccountingSession.Get(oVoucherBill.AccSessionID, (int)Session[SessionInfo.currentUserID]);
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VB.BillDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oAccountingSession.StartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oAccountingSession.EndDate.ToString("dd MMM yyyy") + "',106)) ";
            }
            #endregion

            #region Bill Amount
            if (oVoucherBill.BillAmountOpeType != (int)EnumCompareOperator.None)
            {                
                Global.TagSQL(ref sReturn);
                if (oVoucherBill.BillAmountOpeType == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " VB.Amount = " + oVoucherBill.FromAmount;
                }
                else if (oVoucherBill.BillAmountOpeType == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " VB.Amount != " + oVoucherBill.FromAmount;
                }
                else if (oVoucherBill.BillAmountOpeType == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " VB.Amount > " + oVoucherBill.FromAmount;
                }
                else if (oVoucherBill.BillAmountOpeType == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " VB.Amount < " + oVoucherBill.FromAmount;
                }
                else if (oVoucherBill.BillAmountOpeType == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " VB.Amount BETWEEN " + oVoucherBill.FromAmount + " AND " + oVoucherBill.ToAmount + " ";
                }
                else if (oVoucherBill.BillAmountOpeType == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " VB.Amount NOT BETWEEN " + oVoucherBill.FromAmount + " AND " + oVoucherBill.ToAmount + " ";
                }
            }
            #endregion

            #region Voucher Bill Date
            if (oVoucherBill.BillDateOpeType != (int)EnumCompareOperator.None)
            {                
                Global.TagSQL(ref sReturn);
                oVoucherBill.FromDate = Convert.ToDateTime(oVoucherBill.FromDateString);
                oVoucherBill.ToDate = Convert.ToDateTime(oVoucherBill.ToDateString);
                if (oVoucherBill.BillDateOpeType == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VB.BillDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBill.FromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (oVoucherBill.BillDateOpeType == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VB.BillDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBill.FromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (oVoucherBill.BillDateOpeType == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VB.BillDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBill.FromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (oVoucherBill.BillDateOpeType == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VB.BillDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBill.FromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (oVoucherBill.BillDateOpeType == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VB.BillDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBill.FromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBill.ToDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (oVoucherBill.BillDateOpeType == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VB.BillDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBill.FromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBill.ToDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
    }
}
