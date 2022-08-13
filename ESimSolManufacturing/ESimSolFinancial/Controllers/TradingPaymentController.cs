using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol;
using System.Web.Script.Serialization;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;

namespace ESimSol.Controllers
{
    public class TradingPaymentController : Controller
    {
        #region Declaration
        TradingPayment _oTradingPayment = new TradingPayment();
        List<TradingPayment> _oTradingPayments = new List<TradingPayment>();
        #endregion

        #region Actions
        public ActionResult ViewTradingPayments(int prt, int pby, int buid, int menuid)
        {
            ViewBag.BUID = buid;
            ViewBag.ReferenceType = prt;
            ViewBag.TradingPaymentBy = pby; // 1 => Accounts, 2=> Salesman

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TradingPayment).ToString() + "," + ((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            string sSQL = "";

            _oTradingPayments = new List<TradingPayment>();
            _oTradingPayments = TradingPayment.GetsInitialTradingPayments(buid, (EnumPaymentRefType)prt, pby, (int)Session[SessionInfo.currentUserID]);

            #region Searching Related Data
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM TradingPayment AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<BusinessObjects.User>();
            BusinessObjects.User oApprovalUser = new BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));

            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
                                    
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            oChartsOfAccount = new ChartsOfAccount();
            oChartsOfAccount.AccountHeadName = "--Select Account Head--";
            oChartsOfAccounts.Add(oChartsOfAccount);
            //oChartsOfAccounts.AddRange(ChartsOfAccount.GetsTransactionAcounts(buid, (int)Session[SessionInfo.currentUserID]));
            oChartsOfAccounts.AddRange(ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]));
            ViewBag.PaymentAccounts = oChartsOfAccounts;
            #endregion

            return View(_oTradingPayments);
        }

        public ActionResult ViewTradingPayment(int id, int pby, int buid, double ts)
        {
            ViewBag.TradingPaymentBy = pby; // 1 => Accounts, 2=> Salesman

            _oTradingPayment = new TradingPayment();double nDueAmount = 0;
            List<TradingPaymentDetail> oTradingPaymentDetails =new List<TradingPaymentDetail>();
            List<TradingSaleInvoice> oTradingSaleInvoices = new List<TradingSaleInvoice>();
            List<PurchaseInvoice> oPurchaseInvoices = new List<PurchaseInvoice>();
            List<TradingSaleReturn> oTradingSaleReturns = new List<TradingSaleReturn>();
            if (id > 0)
            {
                _oTradingPayment = _oTradingPayment.Get(id, (int)Session[SessionInfo.currentUserID]);
                oTradingPaymentDetails = TradingPaymentDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                #region Map Yet to TradingPayment Amount
                if (_oTradingPayment.ReferenceType == EnumPaymentRefType.ReceivedPayment)
                {   
                    string sSQL = "SELECT * FROM View_TradingSaleInvoice AS HH WHERE HH.TradingSaleInvoiceID IN (SELECT MM.ReferenceID FROM TradingPaymentDetail AS MM WHERE MM.TradingPaymentID="+_oTradingPayment.TradingPaymentID.ToString()+") ORDER BY TradingSaleInvoiceID ASC";
                    oTradingSaleInvoices = TradingSaleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (TradingPaymentDetail oItem in oTradingPaymentDetails)
                    {
                        nDueAmount = this.GetDueAmount(oTradingSaleInvoices, oPurchaseInvoices, oTradingSaleReturns, oItem.ReferenceID, EnumPaymentRefType.ReceivedPayment);
                        oItem.YetToPayment = nDueAmount + oItem.Amount;
                    }
                }
                else if (_oTradingPayment.ReferenceType == EnumPaymentRefType.PaidPayment)
                {
                    string sSQL = "SELECT * FROM View_PurchaseInvoice AS HH WHERE HH.PurchaseInvoiceID IN (SELECT MM.ReferenceID FROM TradingPaymentDetail AS MM WHERE MM.TradingPaymentID=" + _oTradingPayment.TradingPaymentID.ToString() + ") ORDER BY PurchaseInvoiceID ASC";
                    oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (TradingPaymentDetail oItem in oTradingPaymentDetails)
                    {
                        nDueAmount = this.GetDueAmount(oTradingSaleInvoices, oPurchaseInvoices, oTradingSaleReturns, oItem.ReferenceID, EnumPaymentRefType.PaidPayment);
                        oItem.YetToPayment = nDueAmount + oItem.Amount;
                    }
                }

                else if (_oTradingPayment.ReferenceType == EnumPaymentRefType.SaleReturnPaid)
                {
                    string sSQL = "SELECT * FROM View_TradingSaleReturn AS HH WHERE HH.TradingSaleReturnID IN (SELECT MM.ReferenceID FROM TradingPaymentDetail AS MM WHERE MM.TradingPaymentID=" + _oTradingPayment.TradingPaymentID.ToString() + ") ORDER BY TradingSaleReturnID ASC";
                    oTradingSaleReturns = TradingSaleReturn.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (TradingPaymentDetail oItem in oTradingPaymentDetails)
                    {
                        nDueAmount = this.GetDueAmount(oTradingSaleInvoices, oPurchaseInvoices, oTradingSaleReturns, oItem.ReferenceID, EnumPaymentRefType.SaleReturnPaid);
                        oItem.YetToPayment = nDueAmount + oItem.Amount;
                    }
                }
                #endregion
                _oTradingPayment.TradingPaymentDetails = oTradingPaymentDetails;

            }
            else
            {
                Company oCompany = new Company();
                oCompany = (oCompany.Get(1, (int)Session[SessionInfo.currentUserID]));

                _oTradingPayment.CurrencyID = oCompany.BaseCurrencyID;
                _oTradingPayment.CurrencySymbol = oCompany.CurrencySymbol;

                string sSQL = "SELECT * FROM View_PaymentAccount AS HH WHERE HH.BUID=" + buid.ToString() + " And IsDefault=1";
                var oPaymentAccounts = PaymentAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oPaymentAccounts.Any() && oPaymentAccounts.FirstOrDefault().PaymentAccountID > 0)
                {
                    _oTradingPayment.AccountHeadID = oPaymentAccounts.FirstOrDefault().AccountHeadID;
                }

            }

            #region Gets Transaction Accounts
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            //oChartsOfAccounts.AddRange(ChartsOfAccount.GetsTransactionAcounts(buid, (int)Session[SessionInfo.currentUserID]));
            oChartsOfAccounts.AddRange(ChartsOfAccount.Gets((int)Session[SessionInfo.currentUserID]));

            #endregion

            #region Gets BankAccount
            List<BankAccount> oBankAccounts = new List<BankAccount>();
            oBankAccounts.AddRange(BankAccount.Gets((int)Session[SessionInfo.currentUserID]));
            #endregion

            _oTradingPayment.BankAccounts = oBankAccounts;
            _oTradingPayment.ChartsOfAccounts = oChartsOfAccounts;
            _oTradingPayment.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);

            //SalesmanBusinessUnit oSalesmanBusinessUnit = new SalesmanBusinessUnit();
            //if (pby == 2)
            //{
            //    string sSQL = "Select * from View_SalesmanBusinessUnit Where SalesmanID In (Select SalesmanID from SalesMan Where UserID=" + (int)Session[SessionInfo.currentUserID] + ") And BUID=" + buid + "";
            //    var oSalesmanBusinessUnits = SalesmanBusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //    if (oSalesmanBusinessUnits.Any() && oSalesmanBusinessUnits.FirstOrDefault().SalesmanBUID > 0)
            //    {
            //        oSalesmanBusinessUnit = oSalesmanBusinessUnits.FirstOrDefault();
            //    }
            //}
            ViewBag.PaymentMethods = EnumObject.jGets(typeof(EnumPaymentMethod));
            ViewBag.BankAccounts = oBankAccounts;
            return View(_oTradingPayment);
        }




        [HttpPost]
        public JsonResult Save(TradingPayment oTradingPayment)
        {
            _oTradingPayment = new TradingPayment();
            try
            {
                _oTradingPayment = oTradingPayment;
                _oTradingPayment = _oTradingPayment.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingPayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingPayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(TradingPayment oTradingPayment)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oTradingPayment.Delete((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approved(TradingPayment oTradingPayment)
        {
            _oTradingPayment = new TradingPayment();
            try
            {
                _oTradingPayment = oTradingPayment;
                _oTradingPayment = _oTradingPayment.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingPayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingPayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult PrintTradingPayment(int id, double ts)
        {
            _oTradingPayment = new TradingPayment();
            _oTradingPayment = _oTradingPayment.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTradingPayment.TradingPaymentDetails = TradingPaymentDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptMoneyReceipt oReport = new rptMoneyReceipt();
            byte[] abytes = oReport.PrepareReport(_oTradingPayment, oCompany);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintTradingPayments(string ids, double ts)
        {
            _oTradingPayments = new List<TradingPayment>();
            string sSql = "SELECT * FROM View_TradingPayment WHERE TradingPaymentID IN (" + ids + ") ORDER BY TradingPaymentID";
            _oTradingPayments = TradingPayment.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptTradingPayments oReport = new rptTradingPayments();
            byte[] abytes = oReport.PrepareReport(_oTradingPayments, oCompany);
            return File(abytes, "application/pdf");
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

        #region Advance Search
        [HttpPost]
        public JsonResult WaitForApproval(TradingPayment oTradingPayment)
        {
            _oTradingPayments = new List<TradingPayment>();
            try
            {
                string sSQL = "SELECT * FROM View_TradingPayment WHERE BUID = " + oTradingPayment.BUID.ToString() + " AND ISNULL(ApprovedBy,0)=0 AND ISNULL(SalesManID,0)=" + oTradingPayment.SalesManID.ToString() + " ORDER BY TradingPaymentID ASC";
                _oTradingPayments = TradingPayment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingPayment = new TradingPayment();
                _oTradingPayments = new List<TradingPayment>();
                _oTradingPayment.ErrorMessage = ex.Message;
                _oTradingPayments.Add(_oTradingPayment);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingPayments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AdvanceSearch(TradingPayment oTradingPayment)
        {
            _oTradingPayments = new List<TradingPayment>();
            try
            {
                string sSQL = this.GetSQL(oTradingPayment.Note, oTradingPayment.BUID, oTradingPayment.SalesManID, (EnumPaymentRefType)oTradingPayment.ReferenceTypeInt);
                _oTradingPayments = TradingPayment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingPayment = new TradingPayment();
                _oTradingPayments = new List<TradingPayment>();
                _oTradingPayment.ErrorMessage = ex.Message;
                _oTradingPayments.Add(_oTradingPayment);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingPayments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sSearchingData, int nBUID, int nSalesManID, EnumPaymentRefType eTradingPaymentRefType)
        {
            string sMRNo = Convert.ToString(sSearchingData.Split('~')[0]);
            string sInvoiceNo = Convert.ToString(sSearchingData.Split('~')[1]);
            int nPaymentAccount = Convert.ToInt32(sSearchingData.Split('~')[2]);
            EnumCompareOperator eTradingPaymentDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
            EnumCompareOperator eTradingPaymentAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[7]);
            double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[8]);
            int nApprovedBy = Convert.ToInt32(sSearchingData.Split('~')[9]);
            string sPartyIDs = Convert.ToString(sSearchingData.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_TradingPayment";
            string sReturn = "";

            #region BUID
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID.ToString();
            }
            #endregion

            #region SalesMan
            if (nSalesManID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(SalesManID,0) = " + nSalesManID.ToString();
            }
            #endregion

            #region ReferenceType
            if (eTradingPaymentRefType != EnumPaymentRefType.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ReferenceType = " + ((int)eTradingPaymentRefType).ToString();
            }
            #endregion

            #region PaymentAccount
            if (nPaymentAccount != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " AccountHeadID =" + nPaymentAccount.ToString();
            }
            #endregion

            #region MRNo
            if (sMRNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo LIKE '%" + sMRNo + "%'";
            }
            #endregion

            #region InvoiceNo
            if (sInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TradingPaymentID IN (SELECT HH.TradingPaymentID FROM View_TradingPaymentDetail AS HH WHERE HH.InvoiceNo like '%" + sInvoiceNo + "%')";
            }
            #endregion

            #region TradingPayment Date
            if (eTradingPaymentDate != EnumCompareOperator.None)
            {
                if (eTradingPaymentDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),TradingPaymentDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eTradingPaymentDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),TradingPaymentDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eTradingPaymentDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),TradingPaymentDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eTradingPaymentDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),TradingPaymentDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eTradingPaymentDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),TradingPaymentDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eTradingPaymentDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),TradingPaymentDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region TradingPaymentAmount
            if (eTradingPaymentAmount != EnumCompareOperator.None)
            {
                if (eTradingPaymentAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount = " + nStartAmount.ToString("0.00");
                }
                else if (eTradingPaymentAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount != " + nStartAmount.ToString("0.00");
                }
                else if (eTradingPaymentAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount < " + nStartAmount.ToString("0.00");
                }
                else if (eTradingPaymentAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount > " + nStartAmount.ToString("0.00");
                }
                else if (eTradingPaymentAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
                else if (eTradingPaymentAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount NOT BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
            }
            #endregion

            #region ApprovedBy
            if (nApprovedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) = " + nApprovedBy.ToString();
            }
            #endregion

            #region Party
            if (sPartyIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sPartyIDs + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Pick TradingPayment
        [HttpPost]
        public JsonResult PickInvoice(TradingPayment oTradingPayment)
        {            
            TradingPaymentDetail oTradingPaymentDetail = new TradingPaymentDetail();
            TradingPaymentDetail oTempTradingPaymentDetail = new TradingPaymentDetail();
            List<TradingPaymentDetail> oTradingPaymentDetails = new List<TradingPaymentDetail>();
            List<TradingPaymentDetail> oTempTradingPaymentDetails = new List<TradingPaymentDetail>();
            try
            {                
                oTempTradingPaymentDetails = TradingPaymentDetail.Gets(oTradingPayment.TradingPaymentID, (int)Session[SessionInfo.currentUserID]);
                if ((EnumPaymentRefType)oTradingPayment.ReferenceTypeInt == EnumPaymentRefType.ReceivedPayment)
                {
                    #region Map Received TradingPayment
                    string sSQL = "SELECT * FROM View_TradingSaleInvoice AS HH WHERE HH.BUID=" + oTradingPayment.BUID.ToString() + " AND HH.BuyerID=" + oTradingPayment.ContractorID.ToString() + " AND HH.DueAmount>0 ORDER BY TradingSaleInvoiceID ASC";
                    List<TradingSaleInvoice> oTradingSaleInvoices = new List<TradingSaleInvoice>();
                    oTradingSaleInvoices = TradingSaleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (TradingSaleInvoice oItem in oTradingSaleInvoices)
                    {
                        oTempTradingPaymentDetail = new TradingPaymentDetail();
                        oTempTradingPaymentDetail = this.AlreadyExistTradingPayment(oTempTradingPaymentDetails, oItem.TradingSaleInvoiceID);

                        if (oTempTradingPaymentDetail.TradingPaymentDetailID > 0)
                        {
                            oTradingPaymentDetail = new TradingPaymentDetail();
                            oTradingPaymentDetail.TradingPaymentDetailID = oTempTradingPaymentDetail.TradingPaymentDetailID;
                            oTradingPaymentDetail.TradingPaymentID = oTempTradingPaymentDetail.TradingPaymentID;
                            oTradingPaymentDetail.ReferenceID = oTempTradingPaymentDetail.ReferenceID;
                            oTradingPaymentDetail.Amount = oTempTradingPaymentDetail.Amount;
                            oTradingPaymentDetail.Remarks = oTempTradingPaymentDetail.Remarks;
                            oTradingPaymentDetail.InvoiceNo = oTempTradingPaymentDetail.InvoiceNo;
                            oTradingPaymentDetail.InvoiceDate = oTempTradingPaymentDetail.InvoiceDate;
                            oTradingPaymentDetail.InvoiceAmount = oTempTradingPaymentDetail.InvoiceAmount;
                            oTradingPaymentDetail.ReferenceType = EnumPaymentRefType.ReceivedPayment;
                            oTradingPaymentDetail.ReferenceTypeInt = (int)EnumPaymentRefType.ReceivedPayment;
                            oTradingPaymentDetail.YetToPayment = oItem.DueAmount + oTempTradingPaymentDetail.Amount;
                            oTradingPaymentDetails.Add(oTradingPaymentDetail);
                        }
                        else
                        {
                            oTradingPaymentDetail = new TradingPaymentDetail();
                            oTradingPaymentDetail.TradingPaymentDetailID = 0;
                            oTradingPaymentDetail.TradingPaymentID = 0;
                            oTradingPaymentDetail.ReferenceID = oItem.TradingSaleInvoiceID;
                            oTradingPaymentDetail.Amount = oItem.DueAmount;
                            oTradingPaymentDetail.Remarks = "N/A";
                            oTradingPaymentDetail.InvoiceNo = oItem.InvoiceNo;
                            oTradingPaymentDetail.InvoiceDate = oItem.InvoiceDate;
                            oTradingPaymentDetail.InvoiceAmount = oItem.NetAmount;
                            oTradingPaymentDetail.ReferenceType = EnumPaymentRefType.ReceivedPayment;
                            oTradingPaymentDetail.ReferenceTypeInt = (int)EnumPaymentRefType.ReceivedPayment;
                            oTradingPaymentDetail.YetToPayment = oItem.DueAmount;
                            oTradingPaymentDetails.Add(oTradingPaymentDetail);                        
                        }
                    }
                    
                    //Get Existing TradingPayment In Case of edit TradingPayment.
                    foreach (TradingPaymentDetail oItem in oTempTradingPaymentDetails)
                    { 
                        if(!this.Exist(oTradingPaymentDetails, oItem.TradingPaymentDetailID))
                        {
                            oItem.YetToPayment = oItem.Amount;
                            oTradingPaymentDetails.Add(oItem);
                        }
                    }
                    #endregion
                }
                else if ((EnumPaymentRefType)oTradingPayment.ReferenceTypeInt == EnumPaymentRefType.PaidPayment)
                {
                    #region Map Paid TradingPayment
                    string sSQL = "SELECT * FROM View_PurchaseInvoice AS HH WHERE HH.BUID="+oTradingPayment.BUID.ToString()+" AND HH.ContractorID="+oTradingPayment.ContractorID.ToString()+" AND HH.DueAmount>0 ORDER BY PurchaseInvoiceID";
                    List<PurchaseInvoice> oPurchaseInvoices = new List<PurchaseInvoice>();
                    oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (PurchaseInvoice oItem in oPurchaseInvoices)
                    {
                        oTempTradingPaymentDetail = new TradingPaymentDetail();
                        oTempTradingPaymentDetail = this.AlreadyExistTradingPayment(oTempTradingPaymentDetails, oItem.PurchaseInvoiceID);

                        if (oTempTradingPaymentDetail.TradingPaymentDetailID > 0)
                        {
                            oTradingPaymentDetail = new TradingPaymentDetail();
                            oTradingPaymentDetail.TradingPaymentDetailID = oTempTradingPaymentDetail.TradingPaymentDetailID;
                            oTradingPaymentDetail.TradingPaymentID = oTempTradingPaymentDetail.TradingPaymentID;
                            oTradingPaymentDetail.ReferenceID = oTempTradingPaymentDetail.ReferenceID;
                            oTradingPaymentDetail.Amount = oTempTradingPaymentDetail.Amount;
                            oTradingPaymentDetail.Remarks = oTempTradingPaymentDetail.Remarks;
                            oTradingPaymentDetail.InvoiceNo = oTempTradingPaymentDetail.InvoiceNo;
                            oTradingPaymentDetail.InvoiceDate = oTempTradingPaymentDetail.InvoiceDate;
                            oTradingPaymentDetail.InvoiceAmount = oTempTradingPaymentDetail.InvoiceAmount;
                            oTradingPaymentDetail.ReferenceType = EnumPaymentRefType.PaidPayment;
                            oTradingPaymentDetail.ReferenceTypeInt = (int)EnumPaymentRefType.PaidPayment;
                            oTradingPaymentDetail.YetToPayment = oItem.Amount + oTempTradingPaymentDetail.Amount;
                            oTradingPaymentDetails.Add(oTradingPaymentDetail);
                        }
                        else
                        {
                            oTradingPaymentDetail = new TradingPaymentDetail();
                            oTradingPaymentDetail.TradingPaymentDetailID = 0;
                            oTradingPaymentDetail.TradingPaymentID = 0;
                            oTradingPaymentDetail.ReferenceID = oItem.PurchaseInvoiceID;
                            oTradingPaymentDetail.Amount = oItem.Amount;
                            oTradingPaymentDetail.Remarks = "N/A";
                            oTradingPaymentDetail.InvoiceNo = oItem.PurchaseInvoiceNo;
                            oTradingPaymentDetail.InvoiceDate = oItem.DateofInvoice;
                            oTradingPaymentDetail.InvoiceAmount = oItem.NetAmount;
                            oTradingPaymentDetail.ReferenceType = EnumPaymentRefType.PaidPayment;
                            oTradingPaymentDetail.ReferenceTypeInt = (int)EnumPaymentRefType.PaidPayment;
                            oTradingPaymentDetail.YetToPayment = oItem.Amount;
                            oTradingPaymentDetails.Add(oTradingPaymentDetail);
                        }
                    }

                    //Get Existing TradingPayment In Case of edit TradingPayment.
                    foreach (TradingPaymentDetail oItem in oTempTradingPaymentDetails)
                    {
                        if (!this.Exist(oTradingPaymentDetails, oItem.TradingPaymentDetailID))
                        {
                            oItem.YetToPayment = oItem.Amount;
                            oTradingPaymentDetails.Add(oItem);
                        }
                    }
                    #endregion                    
                }
                else if ((EnumPaymentRefType)oTradingPayment.ReferenceTypeInt == EnumPaymentRefType.SaleReturnPaid)
                {
                    #region Map Sale Return TradingPayment

                    string sSQL = "SELECT * FROM View_TradingSaleReturn WHERE BUID = " + oTradingPayment.BUID.ToString() + " AND BuyerID=" + oTradingPayment.ContractorID.ToString() + " AND ISNULL(ApprovedBy,0)>0 And DueAmount>0 ORDER BY TradingSaleReturnID ASC";
                    List<TradingSaleReturn> oTradingSaleReturns = new List<TradingSaleReturn>();
                    oTradingSaleReturns = TradingSaleReturn.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (TradingSaleReturn oItem in oTradingSaleReturns)
                    {
                        oTempTradingPaymentDetail = new TradingPaymentDetail();
                        oTempTradingPaymentDetail = this.AlreadyExistTradingPayment(oTempTradingPaymentDetails, oItem.TradingSaleReturnID);
                       
                        //oTempTradingPaymentDetail = (oTempTradingPaymentDetails.Any(x => x.TradingPaymentDetailID == oItem.TradingSaleReturnID)) ? oTempTradingPaymentDetails.Where(x => x.TradingPaymentDetailID == oItem.PurchaseInvoiceID).FirstOrDefault() : new TradingPaymentDetail();

                        if (oTempTradingPaymentDetail.TradingPaymentDetailID > 0)
                        {
                            oTradingPaymentDetail = new TradingPaymentDetail();
                            oTradingPaymentDetail.TradingPaymentDetailID = oTempTradingPaymentDetail.TradingPaymentDetailID;
                            oTradingPaymentDetail.TradingPaymentID = oTempTradingPaymentDetail.TradingPaymentID;
                            oTradingPaymentDetail.ReferenceID = oTempTradingPaymentDetail.ReferenceID;
                            oTradingPaymentDetail.Amount = oTempTradingPaymentDetail.Amount;
                            oTradingPaymentDetail.Remarks = oTempTradingPaymentDetail.Remarks;
                            oTradingPaymentDetail.InvoiceNo = oTempTradingPaymentDetail.InvoiceNo;
                            oTradingPaymentDetail.InvoiceDate = oTempTradingPaymentDetail.InvoiceDate;
                            oTradingPaymentDetail.InvoiceAmount = oTempTradingPaymentDetail.InvoiceAmount;
                            oTradingPaymentDetail.ReferenceType = EnumPaymentRefType.PaidPayment;
                            oTradingPaymentDetail.ReferenceTypeInt = (int)EnumPaymentRefType.PaidPayment;
                            oTradingPaymentDetail.YetToPayment = oItem.DueAmount + oTempTradingPaymentDetail.Amount;
                            oTradingPaymentDetails.Add(oTradingPaymentDetail);
                        }
                        else
                        {
                            oTradingPaymentDetail = new TradingPaymentDetail();
                            oTradingPaymentDetail.TradingPaymentDetailID = 0;
                            oTradingPaymentDetail.TradingPaymentID = 0;
                            oTradingPaymentDetail.ReferenceID = oItem.TradingSaleReturnID;
                            oTradingPaymentDetail.Amount = oItem.DueAmount;
                            oTradingPaymentDetail.Remarks = "N/A";
                            oTradingPaymentDetail.InvoiceNo = oItem.ReturnNo;
                            oTradingPaymentDetail.InvoiceDate = oItem.ReturnDate;
                            oTradingPaymentDetail.InvoiceAmount = oItem.GrossAmount;
                            oTradingPaymentDetail.ReferenceType = EnumPaymentRefType.SaleReturnPaid;
                            oTradingPaymentDetail.ReferenceTypeInt = (int)EnumPaymentRefType.SaleReturnPaid;
                            oTradingPaymentDetail.YetToPayment = oItem.DueAmount;
                            oTradingPaymentDetails.Add(oTradingPaymentDetail);
                        }
                    }

                    //Get Existing TradingPayment In Case of edit TradingPayment.
                    foreach (TradingPaymentDetail oItem in oTempTradingPaymentDetails)
                    {
                        if (!this.Exist(oTradingPaymentDetails, oItem.TradingPaymentDetailID))
                        {
                            oItem.YetToPayment = oItem.Amount;
                            oTradingPaymentDetails.Add(oItem);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                oTradingPaymentDetails = new List<TradingPaymentDetail>();
                oTradingPaymentDetail = new TradingPaymentDetail();
                oTradingPaymentDetail.ErrorMessage = ex.Message;
                oTradingPaymentDetails.Add(oTradingPaymentDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTradingPaymentDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private TradingPaymentDetail AlreadyExistTradingPayment(List<TradingPaymentDetail> oTradingPaymentDetails, int nInvoiceID)
        {
            TradingPaymentDetail oTradingPaymentDetail = new TradingPaymentDetail();
            foreach (TradingPaymentDetail oItem in oTradingPaymentDetails)
            {
                if (oItem.ReferenceID == nInvoiceID)
                {
                    return oItem;
                }
            }
            return oTradingPaymentDetail;
        }
        private bool Exist(List<TradingPaymentDetail> oTradingPaymentDetails, int nTradingPaymentDetailID)
        {            
            foreach (TradingPaymentDetail oItem in oTradingPaymentDetails)
            {
                if (oItem.TradingPaymentDetailID == nTradingPaymentDetailID)
                {
                    return true;
                }
            }
            return false;
        }
        private double GetDueAmount(List<TradingSaleInvoice> oTradingSaleInvoices, List<PurchaseInvoice> oPurchaseInvoices, List<TradingSaleReturn> oTradingSaleReturns, int nInvoiceID, EnumPaymentRefType eTradingPaymentRefType)
        {
            double nDueAmount = 0;
            if (eTradingPaymentRefType == EnumPaymentRefType.ReceivedPayment)
            {
                foreach (TradingSaleInvoice oItem in oTradingSaleInvoices)
                {
                    if (oItem.TradingSaleInvoiceID == nInvoiceID)
                    {
                        return oItem.DueAmount;
                    }
                }
            }
            else if (eTradingPaymentRefType == EnumPaymentRefType.PaidPayment)
            {
                foreach (PurchaseInvoice oItem in oPurchaseInvoices)
                {
                    if (oItem.PurchaseInvoiceID == nInvoiceID)
                    {
                        return oItem.Amount;
                    }
                }
            }
            else if (eTradingPaymentRefType == EnumPaymentRefType.SaleReturnPaid)
            {
                foreach (TradingSaleReturn oItem in oTradingSaleReturns)
                {
                    if (oItem.TradingSaleReturnID == nInvoiceID)
                    {
                        return oItem.DueAmount;
                    }
                }
            }
            return nDueAmount;
        }
        #endregion
    }
}
