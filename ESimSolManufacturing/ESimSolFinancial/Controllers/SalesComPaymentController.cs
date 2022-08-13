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
using ReportManagement;
using System.Security;

namespace ESimSolFinancial.Controllers
{
    public class SalesComPaymentController :PdfViewController
    {
        #region Declaration
        SalesComPayment _oSalesComPayment = new SalesComPayment();
        List<SalesComPayment> _oSalesComPayments = new List<SalesComPayment>();
        SalesComPaymentDetail _oSalesComPaymentDetail = new SalesComPaymentDetail();
        List<SalesComPaymentDetail> _oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
        #endregion

        #region SalesComPayment View
        public ActionResult ViewSalesComPayments(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<SalesComPayment> oSalesComPayments = new List<SalesComPayment>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(_oSalesComPayments);
        }
        public ActionResult ViewSalesComPayment(int nId, double ts,int buid)
        {
            _oSalesComPayment = new SalesComPayment();
            _oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
            SalesComPaymentDetail oSalesComPaymentDetail = new SalesComPaymentDetail();
            List<SalesComPaymentDetail> oSalesComPaymentDetails = new List<SalesComPaymentDetail>();

            string sSQL = "";
            if (nId > 0)
            {
                _oSalesComPayment = SalesComPayment.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "Select * from View_SalesComPaymentDetail Where SalesComPaymentID=" + _oSalesComPayment.SalesComPaymentID + "";
                _oSalesComPaymentDetails = SalesComPaymentDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
             }
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.SalesComPaymentDetails = _oSalesComPaymentDetails;
            ViewBag.EnumPaymentModes = EnumObject.jGets(typeof(EnumPaymentMethod)).Where(x => x.id != (int)EnumPaymentMethod.None && x.id != (int)EnumPaymentMethod.Cash); ;
            ViewBag.EnumPaymentTypes = EnumObject.jGets(typeof(EnumPayment_CommissionType)).Where(x => x.id != (int)EnumPayment_CommissionType.None);
            if (_oSalesComPayment.BankAccountID > 0)
            {
                sSQL = "Select * from View_BankAccount  Where BankBranchID =" + _oSalesComPayment.BankBranchID + "AND BankID = " + _oSalesComPayment.BankID + "  Order By AccountNo";
                ViewBag.BankAccounts = BankAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                ViewBag.BankAccounts = new List<BankAccount>();
            }
            ViewBag.PreparedByName = oUser;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Company = oCompany;
            return View(_oSalesComPayment);
        }
        #endregion

        #region HTTP
        [HttpPost]
        public JsonResult Save(SalesComPayment oSalesComPayment)
        {
            _oSalesComPayment = new SalesComPayment();
            List<SalesComPaymentDetail> oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
            try
            {
                _oSalesComPayment = oSalesComPayment;

                foreach (SalesComPaymentDetail oItem in oSalesComPayment.SalesComPaymentDetails)
                {
                    _oSalesComPaymentDetail = new SalesComPaymentDetail();
                    _oSalesComPaymentDetail = oItem;
                    oSalesComPaymentDetails.Add(_oSalesComPaymentDetail);
                }
                if (_oSalesComPayment.SalesComPaymentID <= 0)
                {
                    _oSalesComPayment = _oSalesComPayment.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oSalesComPayment = _oSalesComPayment.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
             }
            catch (Exception ex)
            {
                _oSalesComPayment = new SalesComPayment();
                _oSalesComPayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesComPayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(SalesComPayment oSalesComPayment)
        {
            try
            {
                if (oSalesComPayment.SalesComPaymentID <= 0) { throw new Exception("Please select an valid item."); }
                oSalesComPayment = oSalesComPayment.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSalesComPayment = new SalesComPayment();
                oSalesComPayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesComPayment.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approve(SalesComPayment oSalesComPayment)
        {
            try
            {
                if (oSalesComPayment.SalesComPaymentID <= 0) { throw new Exception("Please select an valid item."); }
                else if (oSalesComPayment.ApproveBy != 0) { throw new Exception("Already Approved."); }

                oSalesComPayment = oSalesComPayment.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSalesComPayment = new SalesComPayment();
                oSalesComPayment.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesComPayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail(SalesComPaymentDetail oSalesComPaymentDetail)
        {
            try
            {
                if (oSalesComPaymentDetail.SalesComPaymentDetailID <= 0) { throw new Exception("Please select an valid item."); }
                oSalesComPaymentDetail = oSalesComPaymentDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSalesComPaymentDetail = new SalesComPaymentDetail();
                oSalesComPaymentDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesComPaymentDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsSalesCommissionPayableForComPayment(SalesCommissionPayable oSalesCommissionPayable)
        {
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            try
            {
                string sReturn1 = "SELECT * FROM View_SalesCommissionPayable ";
                string sReturn = "";
                #region Status_Payable

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Status_Payable IN (4,5)";
                #endregion
                #region BUID

                if (oSalesCommissionPayable.BUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "BUID = " + oSalesCommissionPayable.BUID;

                }
                #endregion

                #region LDBC No

                if (!string.IsNullOrEmpty(oSalesCommissionPayable.LDBCNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LDBCNo LIKE '%" + oSalesCommissionPayable.LDBCNo + "%'";

                }
                #endregion

                #region PI No
                if (!string.IsNullOrEmpty(oSalesCommissionPayable.PINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PINo LIKE '%" + oSalesCommissionPayable.PINo + "%'";
                }
                #endregion

                #region LC No
                if (!string.IsNullOrEmpty(oSalesCommissionPayable.ExportLCNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExportLCNo LIKE '%" + oSalesCommissionPayable.ExportLCNo + "%'";
                }
                #endregion

                #region Buyer/Contact person WIse wise

                if (oSalesCommissionPayable.ContractorID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContactPersonnelID IN(" + oSalesCommissionPayable.ContactPersonnelID + ")";
                }

                if (oSalesCommissionPayable.ContactPersonnelID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContactPersonnelID IN(" + oSalesCommissionPayable.ContactPersonnelID + ")";
                }
                #endregion

                #region SalesComssionPayableiD
                if (!string.IsNullOrEmpty(oSalesCommissionPayable.Params))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SalesCommissionPayableID NOT IN ( " + oSalesCommissionPayable.Params + " )";
                }
                #endregion
                #region Yet Not Paid
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "(MaturityAmount + RealizeAmount + ISNULL(AdjAdd,0) -isnull(AdjDeduct,0))> Amount_Paid";
                #endregion

                sReturn = sReturn1 + sReturn + " Order By ExportLCID,CPName,ExportBillID";

                oSalesCommissionPayables = SalesCommissionPayable.Gets(sReturn, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
                SalesComPaymentDetail oSalesComPaymentDetail = new SalesComPaymentDetail();
                foreach (SalesCommissionPayable oItem in oSalesCommissionPayables)
                {
                    oSalesComPaymentDetail = new SalesComPaymentDetail();
                    oSalesComPaymentDetail.SalesCommissionPayableID = oItem.SalesCommissionPayableID;
                    oSalesComPaymentDetail.PINo = oItem.PINo;
                    oSalesComPaymentDetail.LDBCNo = oItem.LDBCNo;
                    oSalesComPaymentDetail.ExportLCNo = oItem.ExportLCNo;
                    oSalesComPaymentDetail.CommissionAmount = oItem.CommissionAmount;
                    oSalesComPaymentDetail.MaturityAmount = Math.Round(oItem.MaturityAmount,2);
                    oSalesComPaymentDetail.RealizeAmount = Math.Round(oItem.RealizeAmount,2);
                    oSalesComPaymentDetail.AdjAdd = oItem.AdjAdd;
                    oSalesComPaymentDetail.AdjDeduct = Math.Round(oItem.AdjDeduct , 2);
                    oSalesComPaymentDetail.Amount_Paid = Math.Round(oItem.Amount_W_Paid, 2);
                    oSalesComPaymentDetail.Amount = Math.Round((oItem.MaturityAmount + oItem.RealizeAmount + oItem.AdjAdd) - (oItem.AdjDeduct + oItem.AdjPayable + oItem.Amount_W_Paid), 5);
                    oSalesComPaymentDetail.SalesComPaymentDetailID = 0;
                    oSalesComPaymentDetail.SalesComPaymentID = 0;
                    oSalesComPaymentDetail.ContactPersonnelID = oItem.ContactPersonnelID;
                    oSalesComPaymentDetail.ContractorID = oItem.ContractorID;
                    oSalesComPaymentDetail.ContractorName = oItem.ContractorName;
                    oSalesComPaymentDetail.CPName = oItem.CPName;
                    oSalesComPaymentDetail.Param_CRate = oItem.CRate;
                    _oSalesComPaymentDetails.Add(oSalesComPaymentDetail);
                }
            }
            catch (Exception ex)
            {
                oSalesCommissionPayables = new List<SalesCommissionPayable>();
                SalesCommissionPayable oContractorPersonal = new SalesCommissionPayable();
                oContractorPersonal.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesComPaymentDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsSampleAdjustment(SampleInvoice oSampleInvoice)
        {
            List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            try
            {
                string sReturn1 = "SELECT * FROM View_SampleInvoice ";
                string sReturn = "";
                #region SampleInvoiceType

                if (oSampleInvoice.BUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "Invoicetype=  " + (int)EnumSampleInvoiceType.Adjstment_Commission;

                }
                #endregion

                #region SampleInvoiceStatus

                if (oSampleInvoice.BUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CurrentStatus IN ( " + (int)EnumSampleInvoiceStatus.WaitingForApprove +","+(int)EnumSampleInvoiceStatus.Initialized +")";

                }
                #endregion
                
                #region BUID

                if (oSampleInvoice.BUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "BUID = " + oSampleInvoice.BUID;

                }
                #endregion

                #region SampleInvoiceNo

                if (!string.IsNullOrEmpty(oSampleInvoice.SampleInvoiceNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleInvoiceNo LIKE '%" + oSampleInvoice.SampleInvoiceNo + "%'";

                }
                #endregion

                #region Contractor ID
                if (oSampleInvoice.ContractorID >0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID IN ( " + oSampleInvoice.ContractorID + " )";
                }
                #endregion

                #region ContractorPersopnnalID
                if (oSampleInvoice.ContractorPersopnnalID >0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorPersopnnalID IN (  " + oSampleInvoice.ContractorPersopnnalID + " )";
                }
                #endregion
                sReturn = sReturn1 + sReturn;

                oSampleInvoices = SampleInvoice.Gets(sReturn, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            }
            catch (Exception ex)
            {
                oSampleInvoices = new List<SampleInvoice>();
                oSampleInvoice = new SampleInvoice();
                oSampleInvoice.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSampleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Seach Bank Account
        [HttpPost]
        public JsonResult GetsBankAccountSearchByBankNameAndBranch(BankAccount oBankAccount)
        {
            List<BankAccount> oBankAccounts = new List<BankAccount>();
            try
            {
                string sSQL = "";
                int BankBranchID = Convert.ToInt32(oBankAccount.BankName.Split('~')[0]);
                int BankID = Convert.ToInt32(oBankAccount.BankName.Split('~')[1]);

                if (!string.IsNullOrEmpty(oBankAccount.BankName))
                {
                    sSQL = sSQL + "Select * from View_BankAccount  Where BankBranchID =" + BankBranchID + "AND BankID = " + BankID + "  Order By AccountNo";
                }

                oBankAccounts = BankAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oBankAccounts.Count <= 0) { throw new Exception("No information found."); }

            }
            catch (Exception ex)
            {
                oBankAccount = new BankAccount();
                oBankAccount.ErrorMessage = ex.Message;
                oBankAccounts.Add(oBankAccount);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBankAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Advance Search
        public ActionResult AdvSearchSalesCompayment()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult AdvSearch(string sTemp)
        {
            List<SalesComPayment> oSalesComPayments = new List<SalesComPayment>();
            SalesComPayment oSalesComPayment = new SalesComPayment();
            try
            {
                string sSQL = GetSQL(sTemp);
                oSalesComPayments = SalesComPayment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {

                oSalesComPayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesComPayments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
         
            //MR Date
            int nMRDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dtMRDateStart = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dtMREndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sMRNo = sTemp.Split('~')[3];
            string sDOCNo = sTemp.Split('~')[4];
            string sCPIDs = sTemp.Split('~')[5];
            string sConIDs = sTemp.Split('~')[6];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[7]);
          
           string sReturn1 = "SELECT * FROM View_SalesComPayment";
            string sReturn = "";
            #region BUID

            if (nBUID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BUID = " + nBUID ;

            }
            #endregion

            #region MR No

            if (sMRNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MRNo LIKE '%" + sMRNo + "%'";

            }
            #endregion

            #region Doc No
            if (sDOCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DocNo LIKE '%" + sDOCNo + "%'";
            }
            #endregion

            #region Person wise

            if (sCPIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContactPersonnelID IN (" + sCPIDs + ")";
            }
            if (sConIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SalesComPaymentID IN (SELECT SalesComPaymentID FROM View_SalesComPaymentDetail where ContractorID in (" + sConIDs + "))";
            }
            #endregion
            

            #region MR Date Wise
            if (nMRDateCompare > 0)
            {
                if (nMRDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMRDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMRDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMRDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMRDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMRDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMRDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMRDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMRDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMRDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMREndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nMRDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMRDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtMREndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        #endregion
        
        #region PDF 
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
        public ActionResult PrintCommissionReport(int nSalesComPaymentID, double nts)
        {
            string sSQL = "";
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            List<ExportPIReport> oExportPIReports = new List<ExportPIReport>();
            SalesComPayment oSalesComPayment = new SalesComPayment();
            List<ExportBill> oExportBills = new List<ExportBill>();
            List<SalesComPayment> oSalesComPaymentsByPayables = new List<SalesComPayment>();
            List<SalesComPaymentDetail> oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
            oSalesComPayment = SalesComPayment.Get(nSalesComPaymentID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //sSQL = "SELECT *  FROM View_SalesComPaymentDetail Where SalesComPaymentID =  " + oSalesComPayment.SalesComPaymentID;
            //oSalesComPaymentDetails = SalesComPaymentDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "SELECT * FROM view_ExportBill WHERE ExportBillID IN (SELECT ExportBillID FROM SalesCommissionPayable WHERE SalesCommissionPayableID IN(SELECT SalesCommissionPayableID FROM SalesComPaymentDetail WHERE SalesComPaymentID = " + oSalesComPayment.SalesComPaymentID+") )";
            oExportBills = ExportBill.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "select * from View_ExportPIReport where ExportPIID IN (Select ExportPIID  from SalesCommissionPayable  Where SalesCommissionPayableID IN (select SalesCommissionPayableID from SalesComPaymentDetail where SalesComPaymentID =" + oSalesComPayment.SalesComPaymentID + "))";
            oExportPIReports = ExportPIReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "Select * from View_SalesCommissionPayable where SalesCommissionPayableID in(select SalesCommissionPayableID from SalesComPaymentDetail where SalesComPaymentID = " + oSalesComPayment.SalesComPaymentID + ")";
            oSalesCommissionPayables = SalesCommissionPayable.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "Select * from view_SalesComPayment where SalesComPaymentID in (Select SalesComPaymentID from SalesComPaymentDetail where SalesCommissionPayableID in (Select SalesCommissionPayableID from SalesCommissionPayable where SalesCommissionPayableID IN(Select SalesCommissionPayableID from SalesComPaymentDetail where SalesComPaymentID =" + oSalesComPayment.SalesComPaymentID + ")))";
            oSalesComPaymentsByPayables = SalesComPayment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptSalesCommissionPaymentreport oReport = new rptSalesCommissionPaymentreport();
            byte[] abytes = oReport.PrepareReport(oSalesComPayment, oSalesComPaymentDetails, oExportPIReports, oExportBills, oSalesCommissionPayables, oSalesComPaymentsByPayables, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
         }
        #endregion
    }
}