using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using System.Net.Mail;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing.Imaging;
using System.Drawing;


namespace ESimSolFinancial.Controllers
{
    public class ChequeRequisitionController : Controller
    {
        #region Declaration
        ChequeRequisition _oChequeRequisition = new ChequeRequisition();
        List<ChequeRequisition> _oChequeRequisitions = new List<ChequeRequisition>();
        #endregion

        #region Actions
        public ActionResult ViewChequeRequisitions(int menuid)
        {   
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ChequeRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oChequeRequisitions = new List<ChequeRequisition>();
            _oChequeRequisitions = ChequeRequisition.GetsInitialChequeRequisitions((int)Session[SessionInfo.currentUserID]);

            #region Searching Related Data
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM ChequeRequisition AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));

            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));

            BankAccount oBankAccount = new BankAccount();
            List<BankAccount> oBankAccounts = new List<BankAccount>();
            oBankAccount = new BankAccount();
            oBankAccount.AccountNo = "--Select Account No--";
            oBankAccounts.Add(oBankAccount);
            sSQL = "SELECT * FROM View_BankAccount AS HH WHERE HH.BankAccountID IN(SELECT DISTINCT MM.BankAccountID FROM ChequeRequisition AS MM) ORDER BY BankAccountID ASC";
            oBankAccounts.AddRange(BankAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]));
            ViewBag.BankAccounts = oBankAccounts;
            #endregion
                        
            return View(_oChequeRequisitions);
        }

        public ActionResult ViewChequeRequisition(int id, double ts)
        {            
            _oChequeRequisition = new ChequeRequisition(); double nDueAmount = 0;
            List<ChequeRequisitionDetail> oChequeRequisitionDetails = new List<ChequeRequisitionDetail>();
            List<VoucherBill> oVoucherBills = new List<VoucherBill>();
            List<ChequeBook> oChequeBooks = new List<ChequeBook>();
            List<IssueFigure> oIssueFigures = new List<IssueFigure>();
            if (id > 0)
            {
                string sSQL = "";
                _oChequeRequisition = _oChequeRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                oChequeRequisitionDetails = ChequeRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);                
                
                #region Map Yet to Paid Amount
                sSQL = "SELECT * FROM View_VoucherBill AS HH WHERE HH.VoucherBillID IN (SELECT MM.VoucherBillID FROM ChequeRequisitionDetail AS MM WHERE MM.ChequeRequisitionID=" + id.ToString() + ") ORDER BY HH.VoucherBillID ASC";
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (ChequeRequisitionDetail oItem in oChequeRequisitionDetails)
                {
                    nDueAmount = this.GetDueAmount(oVoucherBills, oItem.VoucherBillID);
                    oItem.YetToChequeRequisition = nDueAmount + oItem.Amount;
                }
                #endregion
                _oChequeRequisition.ChequeRequisitionDetails = oChequeRequisitionDetails;

                sSQL = "SELECT * FROM View_ChequeBook AS HH WHERE HH.BankAccountID=" + _oChequeRequisition.BankAccountID.ToString() + " AND HH.IsActive=1 OR HH.ChequeBookID=" + _oChequeRequisition.BankBookID.ToString() + " ORDER BY ChequeBookID ASC";
                oChequeBooks = ChequeBook.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM IssueFigure AS HH WHERE HH.ContractorID IN (SELECT MM.ReferenceObjectID FROM ACCostCenter AS MM WHERE MM.ReferenceType IN(" + (int)EnumReferenceType.Customer + "," + (int)EnumReferenceType.Vendor + "," + (int)EnumReferenceType.Vendor_Foreign + ") AND MM.ACCostCenterID=" + _oChequeRequisition.SubledgerID.ToString() + ") AND HH.IsActive=1 OR HH.IssueFigureID=" + _oChequeRequisition.PayTo.ToString() + " ORDER BY IssueFigureID ASC";
                oIssueFigures = IssueFigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            List<EnumObject> oPaymentTypes = new List<EnumObject>();
            List<EnumObject> oTempPaymentTypes = new List<EnumObject>();
            oTempPaymentTypes = EnumObject.jGets(typeof(EnumPaymentType));
            foreach (EnumObject oItem in oTempPaymentTypes)
            {
                if ((EnumPaymentType)oItem.id == EnumPaymentType.None || (EnumPaymentType)oItem.id == EnumPaymentType.Cash || (EnumPaymentType)oItem.id == EnumPaymentType.AccountPay)
                {
                    oPaymentTypes.Add(oItem);
                }
            }
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.PaymentTypes = oPaymentTypes;
            ViewBag.ChequeBooks = oChequeBooks;
            ViewBag.IssueFigures = oIssueFigures;
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

            return View(_oChequeRequisition);
        }

        [HttpPost]
        public JsonResult Save(ChequeRequisition oChequeRequisition)
        {
            _oChequeRequisition = new ChequeRequisition();
            try
            {
                _oChequeRequisition = oChequeRequisition;
                _oChequeRequisition = _oChequeRequisition.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oChequeRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChequeRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ChequeRequisition oChequeRequisition)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oChequeRequisition.Delete((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approved(ChequeRequisition oChequeRequisition)
        {
            _oChequeRequisition = new ChequeRequisition();
            try
            {
                _oChequeRequisition = oChequeRequisition;
                _oChequeRequisition = _oChequeRequisition.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oChequeRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChequeRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsSubledger(ACCostCenter oACCostCenter)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            string sAccountNo = oACCostCenter.Name == null ? "" : oACCostCenter.Name;
            string sSQL = "SELECT * FROM View_Subledger AS CC WHERE CC.ACCostCenterID IN(SELECT HH.SubLedgerID FROM View_VoucherBill AS HH WHERE HH.DueCheque>0 AND HH.BUID =" + oACCostCenter.BUID.ToString() + " AND HH.IsHoldBill=0  AND HH.IsDebit =0 AND HH.SubLedgerName LIKE '%" + oACCostCenter.Name + "%')  AND CC.Name LIKE '%" + oACCostCenter.Name + "%' ORDER BY CC.Name ASC";
            oACCostCenters = ACCostCenter.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBankAccounts(BankAccount oBankAccount)
        {
            List<BankAccount> oBankAccounts = new List<BankAccount>();            
            string sAccountNo = oBankAccount.AccountNo == null ? "" : oBankAccount.AccountNo;
            string sSQL = "SELECT * FROM View_BankAccount AS HH WHERE HH.BankBranchID IN (SELECT MM.BankBranchID FROM BankBranchBU AS MM WHERE MM.BUID=" + oBankAccount.BusinessUnitID.ToString() + ") AND  (ISNULL(HH.AccountNo,'')+ISNULL(HH.BankName,'')+ISNULL(HH.BranchName,'')+ISNULL(HH.BankShortName,'')) LIKE '%" + sAccountNo + "%' ORDER BY HH.BankID, HH.BankAccountID ASC";           
            oBankAccounts = BankAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBankAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCheques(ChequeRequisition oChequeRequisition)
        {
            List<Cheque> oCheques = new List<Cheque>();
            string sChequeNo = oChequeRequisition.ChequeNo == null ? "" : oChequeRequisition.ChequeNo;
            string sSQL = "SELECT * FROM View_Cheque AS HH WHERE HH.ChequeNo LIKE '%" + sChequeNo + "%' AND HH.ChequeStatus IN(" + (int)EnumChequeStatus.Activate + "," + (int)EnumChequeStatus.Issued + ") AND HH.ChequeBookID = " + oChequeRequisition.BankBookID.ToString() + " AND HH.ChequeID NOT IN (SELECT MM.ChequeID FROM ChequeRequisition AS MM WHERE MM.ChequeRequisitionID!=" + oChequeRequisition.ChequeRequisitionID.ToString() + " AND MM.RequisitionStatus!=" + (int)EnumChequeRequisitionStatus.Cancel + ") ORDER BY HH.ChequeID";
            oCheques = Cheque.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oCheques);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsIssueFigure(ChequeRequisition oChequeRequisition)
        {
            List<IssueFigure> oIssueFigures = new List<IssueFigure>();
            string sSQL = "SELECT * FROM IssueFigure AS HH WHERE HH.ContractorID IN (SELECT MM.ReferenceObjectID FROM ACCostCenter AS MM WHERE MM.ReferenceType IN(" + (int)EnumReferenceType.Customer + "," + (int)EnumReferenceType.Vendor + "," + (int)EnumReferenceType.Vendor_Foreign + ") AND MM.ACCostCenterID=" + oChequeRequisition.SubledgerID.ToString() + ") AND HH.IsActive=1 ORDER BY IssueFigureID ASC";
            oIssueFigures = IssueFigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oIssueFigures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBankBook(ChequeRequisition oChequeRequisition)
        {
            List<ChequeBook> oChequeBooks = new List<ChequeBook>();
            string sSQL = "SELECT * FROM View_ChequeBook AS HH WHERE HH.BankAccountID=" + oChequeRequisition.BankAccountID.ToString() + " AND HH.IsActive=1 ORDER BY ChequeBookID ASC";
            oChequeBooks = ChequeBook.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oChequeBooks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdateVoucherEffect

        [HttpPost]
        public JsonResult UpdateVoucherEffect(ChequeRequisition oChequeRequisition)
        {
            try
            {
                oChequeRequisition = oChequeRequisition.UpdateVoucherEffect((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oChequeRequisition = new ChequeRequisition();
                oChequeRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oChequeRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        //public ActionResult PrintChequeRequisition(int id, double ts)
        //{
        //    _oChequeRequisition = new ChequeRequisition();
        //    _oChequeRequisition = _oChequeRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
        //    _oChequeRequisition.ChequeRequisitionDetails = ChequeRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    rptChequeRequisition oReport = new rptChequeRequisition();
        //    byte[] abytes = oReport.PrepareReport(_oChequeRequisition, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        public ActionResult PrintChequeRequisitions(string ids, double ts)
        {
            _oChequeRequisitions = new List<ChequeRequisition>();
            string sSql = "SELECT * FROM View_ChequeRequisition WHERE ChequeRequisitionID IN (" + ids + ") ORDER BY ChequeRequisitionID";
            _oChequeRequisitions = ChequeRequisition.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptChequeRequisitions oReport = new rptChequeRequisitions();
            byte[] abytes = oReport.PrepareReport(_oChequeRequisitions, oCompany);
            return File(abytes, "application/pdf");
        }

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
        #endregion

        #region Advance Search
        [HttpPost]
        public JsonResult WaitForApproval(ChequeRequisition oChequeRequisition)
        {
            _oChequeRequisitions = new List<ChequeRequisition>();
            try
            {
                string sSQL = "SELECT * FROM View_ChequeRequisition WHERE ISNULL(ApprovedBy,0)=0 ORDER BY ChequeRequisitionID ASC";
                _oChequeRequisitions = ChequeRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oChequeRequisition = new ChequeRequisition();
                _oChequeRequisitions = new List<ChequeRequisition>();
                _oChequeRequisition.ErrorMessage = ex.Message;
                _oChequeRequisitions.Add(_oChequeRequisition);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChequeRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(ChequeRequisition oChequeRequisition)
        {
            _oChequeRequisitions = new List<ChequeRequisition>();
            try
            {
                string sSQL = this.GetSQL(oChequeRequisition.Remarks);
                _oChequeRequisitions = ChequeRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oChequeRequisition = new ChequeRequisition();
                _oChequeRequisitions = new List<ChequeRequisition>();
                _oChequeRequisition.ErrorMessage = ex.Message;
                _oChequeRequisitions.Add(_oChequeRequisition);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChequeRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        private string GetSQL(string sSearchingData)
        {
            string sChequeNo = Convert.ToString(sSearchingData.Split('~')[0]);
            string sBillNo = Convert.ToString(sSearchingData.Split('~')[1]);
            int nBankAccountID = Convert.ToInt32(sSearchingData.Split('~')[2]);
            EnumCompareOperator eRequisitionDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
            EnumCompareOperator eChequeRequisitionAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[7]);
            double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[8]);
            int nApprovedBy = Convert.ToInt32(sSearchingData.Split('~')[9]);
            string sPartyIDs = Convert.ToString(sSearchingData.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_ChequeRequisition";
            string sReturn = "";

            #region ChequeRequisitionAccount
            if (nBankAccountID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BankAccountID =" + nBankAccountID.ToString();
            }
            #endregion

            #region ChequeNo
            if (sChequeNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChequeNo LIKE '%" + sChequeNo + "%'";
            }
            #endregion

            #region InvoiceNo
            if (sBillNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChequeRequisitionID IN (SELECT HH.ChequeRequisitionID FROM View_ChequeRequisitionDetail AS HH WHERE HH.BillNo like '%" + sBillNo + "%')";
            }
            #endregion

            #region ChequeRequisition Date
            if (eRequisitionDate != EnumCompareOperator.None)
            {
                if (eRequisitionDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequisitionDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequisitionDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequisitionDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequisitionDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequisitionDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequisitionDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequisitionDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequisitionDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequisitionDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eRequisitionDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RequisitionDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region ChequeRequisitionAmount
            if (eChequeRequisitionAmount != EnumCompareOperator.None)
            {
                if (eChequeRequisitionAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeAmount = " + nStartAmount.ToString("0.00");
                }
                else if (eChequeRequisitionAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeAmount != " + nStartAmount.ToString("0.00");
                }
                else if (eChequeRequisitionAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeAmount < " + nStartAmount.ToString("0.00");
                }
                else if (eChequeRequisitionAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeAmount > " + nStartAmount.ToString("0.00");
                }
                else if (eChequeRequisitionAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeAmount BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
                else if (eChequeRequisitionAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeAmount NOT BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
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
                sReturn = sReturn + " SubledgerID IN (" + sPartyIDs + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Pick ChequeRequisition
        [HttpPost]
        public JsonResult PickVoucherBills(ChequeRequisition oChequeRequisition)
        {
            ChequeRequisitionDetail oChequeRequisitionDetail = new ChequeRequisitionDetail();
            ChequeRequisitionDetail oTempChequeRequisitionDetail = new ChequeRequisitionDetail();
            List<ChequeRequisitionDetail> oChequeRequisitionDetails = new List<ChequeRequisitionDetail>();
            List<ChequeRequisitionDetail> oTempChequeRequisitionDetails = new List<ChequeRequisitionDetail>();
            try
            {
                oTempChequeRequisitionDetails = ChequeRequisitionDetail.Gets(oChequeRequisition.ChequeRequisitionID, (int)Session[SessionInfo.currentUserID]);
                
                #region Map Cheque Requisition Detail
                string sSQL = "SELECT * FROM View_VoucherBill AS HH WHERE HH.DueCheque>0 AND HH.BUID =" + oChequeRequisition.BUID.ToString() + " AND HH.IsHoldBill=0  AND HH.SubLedgerID =" + oChequeRequisition.SubledgerID.ToString() + " AND HH.IsDebit =0  ORDER BY HH.OverDueDays, HH.BillDate ASC";
                List<VoucherBill> oVoucherBills = new List<VoucherBill>();
                oVoucherBills = VoucherBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (VoucherBill oItem in oVoucherBills)
                {
                    oTempChequeRequisitionDetail = new ChequeRequisitionDetail();
                    oTempChequeRequisitionDetail = this.AlreadyExistChequeRequisition(oTempChequeRequisitionDetails, oItem.VoucherBillID);

                    if (oTempChequeRequisitionDetail.ChequeRequisitionDetailID > 0)
                    {
                        oChequeRequisitionDetail = new ChequeRequisitionDetail();
                        oChequeRequisitionDetail.ChequeRequisitionDetailID = oTempChequeRequisitionDetail.ChequeRequisitionDetailID;
                        oChequeRequisitionDetail.ChequeRequisitionID = oTempChequeRequisitionDetail.ChequeRequisitionID;
                        oChequeRequisitionDetail.VoucherBillID = oTempChequeRequisitionDetail.VoucherBillID;
                        oChequeRequisitionDetail.Amount = oTempChequeRequisitionDetail.Amount;
                        oChequeRequisitionDetail.Remarks = oTempChequeRequisitionDetail.Remarks;
                        oChequeRequisitionDetail.BillNo = oTempChequeRequisitionDetail.BillNo;
                        oChequeRequisitionDetail.BillDate = oTempChequeRequisitionDetail.BillDate;
                        oChequeRequisitionDetail.AccountHeadName = oTempChequeRequisitionDetail.AccountHeadName;
                        oChequeRequisitionDetail.BillAmount = oTempChequeRequisitionDetail.BillAmount;
                        oChequeRequisitionDetail.RemainningBalance = oItem.RemainningBalance;
                        oChequeRequisitionDetail.YetToChequeRequisition = oItem.DueCheque + oTempChequeRequisitionDetail.Amount;
                        oChequeRequisitionDetails.Add(oChequeRequisitionDetail);
                    }
                    else
                    {
                        oChequeRequisitionDetail = new ChequeRequisitionDetail();
                        oChequeRequisitionDetail.ChequeRequisitionDetailID = 0;
                        oChequeRequisitionDetail.ChequeRequisitionID = 0;
                        oChequeRequisitionDetail.VoucherBillID = oItem.VoucherBillID;
                        oChequeRequisitionDetail.Amount = oItem.DueCheque;
                        oChequeRequisitionDetail.Remarks = "N/A";
                        oChequeRequisitionDetail.BillNo = oItem.BillNo;
                        oChequeRequisitionDetail.BillDate = oItem.BillDate;
                        oChequeRequisitionDetail.AccountHeadName = oItem.AccountHeadName;
                        oChequeRequisitionDetail.BillAmount = oItem.Amount;
                        oChequeRequisitionDetail.RemainningBalance = oItem.RemainningBalance;
                        oChequeRequisitionDetail.YetToChequeRequisition = oItem.DueCheque;
                        oChequeRequisitionDetails.Add(oChequeRequisitionDetail);
                    }
                }

                //Get Existing ChequeRequisition In Case of edit ChequeRequisition.
                foreach (ChequeRequisitionDetail oItem in oTempChequeRequisitionDetails)
                {
                    if (!this.Exist(oChequeRequisitionDetails, oItem.ChequeRequisitionDetailID))
                    {
                        oItem.YetToChequeRequisition = oItem.Amount;
                        oChequeRequisitionDetails.Add(oItem);
                    }
                }
                #endregion              
            }
            catch (Exception ex)
            {
                oChequeRequisitionDetails = new List<ChequeRequisitionDetail>();
                oChequeRequisitionDetail = new ChequeRequisitionDetail();
                oChequeRequisitionDetail.ErrorMessage = ex.Message;
                oChequeRequisitionDetails.Add(oChequeRequisitionDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oChequeRequisitionDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private ChequeRequisitionDetail AlreadyExistChequeRequisition(List<ChequeRequisitionDetail> oChequeRequisitionDetails, int nVoucherBillID)
        {
            ChequeRequisitionDetail oChequeRequisitionDetail = new ChequeRequisitionDetail();
            foreach (ChequeRequisitionDetail oItem in oChequeRequisitionDetails)
            {
                if (oItem.VoucherBillID == nVoucherBillID)
                {
                    return oItem;
                }
            }
            return oChequeRequisitionDetail;
        }
        private bool Exist(List<ChequeRequisitionDetail> oChequeRequisitionDetails, int nChequeRequisitionDetailID)
        {
            foreach (ChequeRequisitionDetail oItem in oChequeRequisitionDetails)
            {
                if (oItem.ChequeRequisitionDetailID == nChequeRequisitionDetailID)
                {
                    return true;
                }
            }
            return false;
        }
        private double GetDueAmount(List<VoucherBill> oVoucherBills, int nVoucherBillID)
        {
            double nDueAmount = 0;
            foreach (VoucherBill oItem in oVoucherBills)
            {
                if (oItem.VoucherBillID == nVoucherBillID)
                {
                    return oItem.DueCheque;                    
                }
            }
            return nDueAmount;
        }
        #endregion
    }
}
