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


namespace ESimSolFinancial.Controllers
{
    public class ImportPaymentRequestController : Controller
    {
        #region Declaration
        ImportPaymentRequest _oImportPaymentRequest = new ImportPaymentRequest();
        ImportPaymentRequestDetail _oImportPaymentRequestDetail = new ImportPaymentRequestDetail();
        List<ImportPaymentRequest> _oImportPaymentRequests = new List<ImportPaymentRequest>();
        List<ImportPaymentRequestDetail> _oImportPaymentRequestDetails = new List<ImportPaymentRequestDetail>();
        List<ImportInvoice> _oImportInvoices = new List<ImportInvoice>();
        string _sErrorMessage = "";
        #endregion

        #region Actions       
        public ActionResult ViewImportPaymentRequests(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oImportPaymentRequests = new List<ImportPaymentRequest>();
            _oImportPaymentRequests = ImportPaymentRequest.WaitForApproval(buid,(int)Session[SessionInfo.currentUserID]);

            #region Get User
            string sSQL = "SELECT * FROM Users AS HH WHERE HH.UserID IN (SELECT NN.ApprovedBy FROM View_ImportPaymentRequest AS NN WHERE NN.BUID=" + buid.ToString() + ") ORDER BY UserName ASC";
            List<User> oApprovedUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovedUser = new ESimSol.BusinessObjects.User();
            oApprovedUser.UserID = 0; oApprovedUser.UserName = "--Select Approved User--";
            oApprovedUsers.Add(oApprovedUser);
            oApprovedUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.ApprovedUsers = oApprovedUsers;
            ViewBag.CompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));            
            ViewBag.BankAccounts = BankAccount.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, (int)Session[SessionInfo.currentUserID]);
            return View(_oImportPaymentRequests);
        }
        public ActionResult ViewImportPaymentRequest(int id, int buid)
        {
            _oImportPaymentRequest = new ImportPaymentRequest();
            BankAccount oBankAccount = new BankAccount();
            if (id > 0)
            {
                _oImportPaymentRequest = _oImportPaymentRequest.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPaymentRequest.ImportPaymentRequestDetails = ImportPaymentRequestDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPaymentRequest.BankAccounts = new List<BankAccount>();
                oBankAccount.BankAccountID = _oImportPaymentRequest.BankAccountID;
                oBankAccount.AccountNo = _oImportPaymentRequest.AccountNo;
                _oImportPaymentRequest.BankAccounts.Add(oBankAccount);
            }
            ViewBag.BUID = buid;
            ViewBag.LiabilityTypeObj = EnumObject.jGets(typeof(EnumLiabilityType));
            ViewBag.CurrencyTypes = EnumObject.jGets(typeof(EnumCurrencyType));
            return View(_oImportPaymentRequest);
        }

        [HttpPost]
        public JsonResult GetsImportInvoice(ImportPaymentRequestDetail oPIPReDetail)
        {
            _oImportInvoices = new List<ImportInvoice>();
            _oImportPaymentRequest = new ImportPaymentRequest();
            try
            {
                if (oPIPReDetail.BankBranchID > 0)
                {
                    _oImportInvoices = ImportInvoice.Gets("SELECT * FROM View_ImportInvoice where BUID=" + oPIPReDetail.BUID+ " and BankStatus=" + (int)EnumInvoiceBankStatus.ABP + " and BankBranchID_Nego=" + oPIPReDetail.BankBranchID + " and ImportInvoiceID not in (Select ImportInvoiceID from ImportPaymentRequestDetail)", (int)Session[SessionInfo.currentUserID]);
                    
                }
                else
                {
                    _oImportInvoices = ImportInvoice.Gets("SELECT * FROM View_ImportInvoice where BUID=" + oPIPReDetail.BUID + " and  BankStatus=" + (int)EnumInvoiceBankStatus.ABP + "  and ImportInvoiceID not in (Select ImportInvoiceID from ImportPaymentRequestDetail)", (int)Session[SessionInfo.currentUserID]);
                }

                foreach (ImportInvoice oItem in _oImportInvoices)
                {
                    _oImportPaymentRequestDetail = new ImportPaymentRequestDetail();
                    _oImportPaymentRequestDetail.ImportInvoiceNo = oItem.ImportInvoiceNo;
                    _oImportPaymentRequestDetail.ImportLCNo = oItem.ImportLCNo;
                    _oImportPaymentRequestDetail.ImportInvoiceID = oItem.ImportInvoiceID;
                    _oImportPaymentRequestDetail.CurrencyID = oItem.CurrencyID;
                    _oImportPaymentRequestDetail.Currency = oItem.Currency;
                    _oImportPaymentRequestDetail.DateofMaturity = oItem.DateofMaturity;
                    _oImportPaymentRequestDetail.Amount = oItem.Amount;
                    _oImportPaymentRequestDetail.BankBranchID = oItem.BankBranchID_Nego;
                    _oImportPaymentRequestDetails.Add(_oImportPaymentRequestDetail);
                }
            }
            catch (Exception ex)
            {
                _oImportPaymentRequestDetails = new List<ImportPaymentRequestDetail>();
                _oImportPaymentRequestDetail  = new ImportPaymentRequestDetail();
                _oImportPaymentRequestDetail.ErrorMessage = ex.Message;
                _oImportPaymentRequestDetails.Add(_oImportPaymentRequestDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPaymentRequestDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Save(ImportPaymentRequest oImportPaymentRequest)
        {
            _oImportPaymentRequest = new ImportPaymentRequest();
            try
            {
                _oImportPaymentRequest = oImportPaymentRequest;
                _oImportPaymentRequest.LiabilityType = (EnumLiabilityType)_oImportPaymentRequest.LiabilityTypeInt;
                _oImportPaymentRequest = _oImportPaymentRequest.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPaymentRequest = new ImportPaymentRequest();
                _oImportPaymentRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPaymentRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ImportPaymentRequest oImportPaymentRequest)
        {
            string sFeedBackMessage = "";
            try
            {                
                sFeedBackMessage = oImportPaymentRequest.Delete( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Cancel_LDBP_Req(ImportPaymentRequestDetail oImportPaymentRequestDetail)
        {
            PurchaseInvoiceHistory oPurchaseInvoiceHistory = new PurchaseInvoiceHistory();
            ImportInvoice oImportInvoice = new ImportInvoice();
            string sFeedBackMessage = "";
            try
            {
                oImportInvoice = oImportInvoice.Get(oImportPaymentRequestDetail.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
                if (oImportInvoice.BankStatus != EnumInvoiceBankStatus.Payment_Request)
                {
                    sFeedBackMessage = "Invoice status is " + oImportInvoice.BankStatus.ToString() + "";
                }               
                if (sFeedBackMessage == "")
                {
                    sFeedBackMessage = oPurchaseInvoiceHistory.Delete((int)Session[SessionInfo.currentUserID]);
                }

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Cancel_Request(ImportPaymentRequest oImportPaymentRequest)
        {
            _oImportPaymentRequest = new ImportPaymentRequest();
            try
            {
                _oImportPaymentRequest = oImportPaymentRequest; 
                _oImportPaymentRequest = _oImportPaymentRequest.Cancel_Request((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPaymentRequest = new ImportPaymentRequest();
                _oImportPaymentRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPaymentRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approved(ImportPaymentRequest oInvoicePurchase)
        {            
            try
            {
                oInvoicePurchase = oInvoicePurchase.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oInvoicePurchase = new ImportPaymentRequest();
                oInvoicePurchase.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oInvoicePurchase);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PriviewImportPaymentRequest(int id)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            Company oCompany = new Company();
            string sSQL = "";
            if (id > 0)
            {
                _oImportPaymentRequest = _oImportPaymentRequest.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oImportPaymentRequest.ImportPaymentRequestDetails = ImportPaymentRequestDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

                sSQL = sSQL + " Order By LCPaymentType DESC";
                oImportLetterSetup = oImportLetterSetup.GetForIPR((int)EnumImportLetterType.Invoice_Settlement, (int)EnumImportLetterIssueTo.Bank, _oImportPaymentRequest.BUID, _oImportPaymentRequest.ImportPaymentRequestID, sSQL, (int)Session[SessionInfo.currentUserID]);

                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                oBusinessUnit = oBusinessUnit.Get(_oImportPaymentRequest.BUID, (int)Session[SessionInfo.currentUserID]);

            }

           
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptImportPaymentRequest oReport = new rptImportPaymentRequest();
            byte[] abytes = oReport.PrepareReport(_oImportPaymentRequest, oImportLetterSetup, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");


        }      
        
        #endregion



        #region Advance Search
        [HttpPost]
        public JsonResult SearchByNo(ImportPaymentRequest oImportPaymentRequest)
        {
            List<ImportPaymentRequest> oImportPaymentRequests = new List<ImportPaymentRequest>();
            try
            {
                string sSQL = "SELECT * FROM View_ImportPaymentRequest WHERE BUID = " + oImportPaymentRequest.BUID.ToString() + " AND ImportPaymentRequestID IN (SELECT HH.ImportPaymentRequestID FROM View_ImportPaymentRequestDetail AS HH WHERE (ISNULL(HH.ImportInvoiceNo,'')+ISNULL(HH.ImportLCNo,'')) LIKE '%" + oImportPaymentRequest.RefNo + "%') ORDER BY ImportPaymentRequestID ASC";
                oImportPaymentRequests = ImportPaymentRequest.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportPaymentRequests = new List<ImportPaymentRequest>();
            }
            var jsonResult = Json(oImportPaymentRequests, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult WaitForApproved(ImportPaymentRequest oImportPaymentRequest)
        {
            _oImportPaymentRequests = new List<ImportPaymentRequest>();
            try
            {
                string sSQL = "SELECT * FROM View_ImportPaymentRequest WHERE BUID = " + oImportPaymentRequest.BUID.ToString() + " AND ISNULL(ApprovedBy,0)=0 ORDER BY ImportPaymentRequestID ASC";
                _oImportPaymentRequests = ImportPaymentRequest.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPaymentRequest = new ImportPaymentRequest();
                _oImportPaymentRequests = new List<ImportPaymentRequest>();
                _oImportPaymentRequest.ErrorMessage = ex.Message;
                _oImportPaymentRequests.Add(_oImportPaymentRequest);
            }

            var jsonResult = Json(_oImportPaymentRequests, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AdvanceSearch(ImportPaymentRequest oImportPaymentRequest)
        {
            _oImportPaymentRequests = new List<ImportPaymentRequest>();
            try
            {
                string sSQL = this.GetSQL(oImportPaymentRequest);
                _oImportPaymentRequests = ImportPaymentRequest.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPaymentRequest = new ImportPaymentRequest();
                _oImportPaymentRequests = new List<ImportPaymentRequest>();
                _oImportPaymentRequest.ErrorMessage = ex.Message;
                _oImportPaymentRequests.Add(_oImportPaymentRequest);

            }

            var jsonResult = Json(_oImportPaymentRequests, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQL(ImportPaymentRequest oImportPaymentRequest)
        {
            string sSearchingData = oImportPaymentRequest.Note;
            EnumCompareOperator eIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sImportInvoiceNo = Convert.ToString(sSearchingData.Split('~')[3]);
            string sImportLCNo = Convert.ToString(sSearchingData.Split('~')[4]);
            string sRemarks = Convert.ToString(sSearchingData.Split('~')[5]);
            bool bApproved = Convert.ToBoolean(sSearchingData.Split('~')[6]);
            bool bUnApproved = Convert.ToBoolean(sSearchingData.Split('~')[7]);

            string sReturn1 = "SELECT * FROM View_ImportPaymentRequest";
            string sReturn = "";

            #region BUID
            if (oImportPaymentRequest.BUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oImportPaymentRequest.BUID.ToString();
            }
            #endregion

            #region RefNo
            if (oImportPaymentRequest.RefNo == null) oImportPaymentRequest.RefNo = "";
            if (oImportPaymentRequest.RefNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo LIKE '%" + oImportPaymentRequest.RefNo + "%'";
            }
            #endregion

            #region sImportInvoiceNo
            if (sImportInvoiceNo == null) sImportInvoiceNo = "";
            if (sImportInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPaymentRequestID IN (SELECT HH.ImportPaymentRequestID FROM View_ImportPaymentRequestDetail AS HH WHERE HH.ImportInvoiceNo LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            #region sImportLCNo
            if (sImportLCNo == null) sImportLCNo = "";
            if (sImportLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPaymentRequestID IN (SELECT HH.ImportPaymentRequestID FROM View_ImportPaymentRequestDetail AS HH WHERE HH.ImportLCNo LIKE '%" + sImportLCNo + "%')";
            }
            #endregion

            #region ApprovedBy
            if (oImportPaymentRequest.ApprovedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) = " + oImportPaymentRequest.ApprovedBy.ToString();
            }
            #endregion

            #region BankAccountID
            if (oImportPaymentRequest.BankAccountID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(BankAccountID,0) = " + oImportPaymentRequest.BankAccountID.ToString();
            }
            #endregion

            #region Issue Date
            if (eIssueDate != EnumCompareOperator.None)
            {
                if (eIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LetterIssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Remarks
            if (sRemarks != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Note LIKE '%" + sRemarks + "%'";
            }
            #endregion

            #region Approved
            if (bApproved == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) != 0 ";
            }
            #endregion

            #region Un-Approved
            if (bUnApproved == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) = 0 ";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY ImportPaymentRequestID ASC";
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
