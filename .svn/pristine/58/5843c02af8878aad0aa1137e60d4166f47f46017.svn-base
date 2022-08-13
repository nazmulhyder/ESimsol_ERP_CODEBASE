using System;
using System.Collections.Generic;
using System.Data;
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
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class PaymentController : Controller
    {
        #region
        string _sErrorMessage = "";
        Payment _oPayment = new Payment();
        List<Payment> _oPayments = new List<Payment>();
        List<Contractor> _oContractors = new List<Contractor>();
        public ActionResult ViewPayments(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPayments = new List<Payment>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            //_oPayments = Payment.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PaymentModes = EnumObject.jGets(typeof(EnumPaymentMethod));
            ViewBag.PaymentTypes = EnumObject.jGets(typeof(EnumPaymentReceiveType));
            ViewBag.PaymentStatus = EnumObject.jGets(typeof(EnumPaymentStatus));
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit where BusinessUnitID in (Select Distinct BUID from ExportPI)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            return View(_oPayments);
        }
    
        public ActionResult ViewPayment(int id, int buid)
        {
            Company oCompany = new Company();
            string sSQL = "";
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oPayment = new Payment();
            if (id > 0)
            {
                _oPayment = _oPayment.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oPayment.PaymentDetails = PaymentDetail.Gets(_oPayment.PaymentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oPayment.PrepareByName = ((User)Session[SessionInfo.CurrentUser]).UserName;
            }
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentModes = EnumObject.jGets(typeof(EnumPaymentMethod));
            ViewBag.PaymentTypes = EnumObject.jGets(typeof(EnumPaymentReceiveType));
            ViewBag.PaymentStatus = EnumObject.jGets(typeof(EnumPaymentStatus));
            ViewBag.Company = oCompany;
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit where BusinessUnitID in (Select Distinct BUID from ExportPI)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            sSQL = "SELECT * FROM View_BankAccount WHERE BankBranchID IN (SELECT BankBranchID FROM BankBranchBU WHERE BUID = "+buid+") AND BankBranchID IN (SELECT BankBranchID FROM BankBranchDept WHERE OperationalDept = "+(int)EnumOperationalDept.Accounts+") ";
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.BankAccounts = BankAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]); 
            return View(_oPayment);
        }
        #region HTTP Save
        private bool ValidateInput(Payment oPayment)
        {

            if (oPayment.ContractorID <= 0)
            {
                _sErrorMessage = "Please Pick, Applicant Name.";
                return false;
            }
            if (oPayment.CurrencyID<= 0)
            {
                _sErrorMessage = "Please Pick,Issue Bank.";
                return false;
            }
            return true;
        }

        [HttpPost]
        public JsonResult Save(Payment oPayment)
        {
            oPayment.RemoveNulls();
            _oPayment = new Payment();
            try
            {
                _oPayment = oPayment;
                _oPayment.MRNo = _oPayment.MRNo.Trim();
                _oPayment.PaymentType =  (EnumPaymentReceiveType)oPayment.PaymentTypeInInt;
                _oPayment.PaymentMode = (EnumPaymentMethod)oPayment.PaymentModeInt;
                if (this.ValidateInput(_oPayment))
                {
                    oPayment = _oPayment.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oPayment.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                oPayment = new Payment();
                oPayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     

        [HttpPost]
        public JsonResult Delete_Detail(PaymentDetail oPaymentDetail)
        {
            string sFeedBackMessage = "";
            try
            {
               
                sFeedBackMessage = oPaymentDetail.Delete( ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Approved(Payment oPayment)
        {
            oPayment.RemoveNulls();
            try
            {

                _oPayment = oPayment;
                if (this.ValidateInput(_oPayment))
                {
                    _oPayment = _oPayment.Payment_Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oPayment.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oPayment = new Payment();
                _oPayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApproved(Payment oPayment)
        {
            oPayment.RemoveNulls();
            try
            {
                _oPayment = oPayment;
                if (this.ValidateInput(_oPayment))
                {
                    _oPayment = _oPayment.Payment_UndoApprove(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oPayment.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oPayment = new Payment();
                _oPayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBankAccountBUWise(BankAccount oBankAccount)
        {
            List<BankAccount> oBankAccounts = new List<BankAccount>();            
            try
            {
                string sSQL = "SELECT * FROM View_BankAccount WHERE BankBranchID IN (SELECT BankBranchID FROM BankBranchBU WHERE BUID = " + oBankAccount.BusinessUnitID + ") AND BankBranchID IN (SELECT BankBranchID FROM BankBranchDept WHERE OperationalDept = " + (int)EnumOperationalDept.Accounts + ") ";
                oBankAccounts = BankAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]); 
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

        #region HTTP Delete

        [HttpPost]
        public JsonResult DeletePayment(Payment oPayment)
        {
            string sMessage = "";
            try
            {
                sMessage = oPayment.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #endregion
        [HttpPost]
        public JsonResult TakaInWord(Payment oPayment)
        {
            string sTakaInWord = "";
            if (oPayment.CurrencyID == 1)
            {
                sTakaInWord = Global.TakaWords(oPayment.Amount);
            }
            else
            {
                sTakaInWord = Global.DollarWords(oPayment.Amount);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sTakaInWord);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Get Company Logo

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

        #endregion

        #region Advance Search

       
        #region HttpGet For Search
        [HttpPost]
        public JsonResult AdvanchSearch(Payment oPayment)
        {
            _oPayments = new List<Payment>();
            try
            {
                string sSQL = GetSQL(oPayment.ErrorMessage);
                _oPayments = Payment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPayment = new Payment();
                _oPayment.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oPayments, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQL(string sTemp)
        {
            string sReturn1 = "SELECT * FROM View_Payment ";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sTemp))
            {
                #region Set Values
                int nBUID = 0;
                string sMRNo = Convert.ToString(sTemp.Split('~')[0]);
                string sDoCNo = Convert.ToString(sTemp.Split('~')[1]);
                string sContractorIDs = Convert.ToString(sTemp.Split('~')[2]);
               
                int nCboMRDate = Convert.ToInt32(sTemp.Split('~')[3]);
                DateTime dFromMRDate = DateTime.Now;
                DateTime dToMRDate = DateTime.Now;
                if (nCboMRDate>0)
                {
                    dFromMRDate = Convert.ToDateTime(sTemp.Split('~')[4]);
                    dToMRDate = Convert.ToDateTime(sTemp.Split('~')[5]);
                }
                nBUID = Convert.ToInt32(sTemp.Split('~')[6]);
               
                #endregion

                #region Make Query

                #region MR NO
                if (!string.IsNullOrEmpty(sMRNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "MRNo like '%" + sMRNo + "%'";
                }
                #endregion

                #region sDoCNo
                if (!string.IsNullOrEmpty(sDoCNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DocNo like '%" + sDoCNo + "%'";
                }
                #endregion

                #region ContractorID
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID in( " + sContractorIDs + ") ";
                }
                #endregion
                #region nBUID
                if (nBUID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "BUID =" + nBUID;
                }
                #endregion
              
                #region MRDate
                if (nCboMRDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboMRDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromMRDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboMRDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromMRDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboMRDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromMRDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboMRDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromMRDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboMRDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromMRDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToMRDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboMRDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromMRDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToMRDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

             
              

                

                #endregion

            }
            sReturn = sReturn1 + sReturn + " ORDER BY MRDate,PaymentID ASC";
            return sReturn;
        }
        #endregion

        [HttpPost]
        public JsonResult GetbyNo(Payment oPayment)
        {

            _oPayments = new List<Payment>();
            string sReturn1 = "SELECT * FROM View_Payment ";
            string sReturn = "";

            #region MRNo
            if (!string.IsNullOrEmpty(oPayment.MRNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MRNo LIKE '%" + oPayment.MRNo + "%' ";
            }
            #endregion

            //#region MRDate

            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "  CONVERT(DATE,CONVERT(VARCHAR(12),[MRDate] ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oPayment.MRDate.ToString("dd MMM yyyy") + "',106))";
       
            //#endregion
            

            #region
            if (oPayment.ContractorID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in (" + oPayment.ContractorID + "," + oPayment.ContractorID + ")";
            }
          
            #endregion
            #region Sample Invoice No
            if (!string.IsNullOrEmpty(oPayment.Parm))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "PaymentID in (Select PaymentID from View_PaymentDetail  where SampleInvoiceNo like '%" + oPayment.Parm + "%')";
            }
            #endregion
            
            string sSQL = sReturn1 + sReturn;
            _oPayments = Payment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPayments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Gets_SampleInvoice(SampleInvoice oSampleInvoice)
        {

            List<PaymentDetail> oPaymentDetails = new List<PaymentDetail>();
            PaymentDetail oPaymentDetail = new PaymentDetail();
            List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            string sReturn1 = "SELECT * FROM View_SampleInvoice ";
            string sReturn = "";

            #region SampleInvoiceNo
            if (!string.IsNullOrEmpty(oSampleInvoice.SampleInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SampleInvoiceNo LIKE '%" + oSampleInvoice.SampleInvoiceNo + "%' ";
            }
            #endregion
            #region ContractorID
            if (oSampleInvoice.ContractorID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID=" + oSampleInvoice.ContractorID;
            }
            #endregion
            #region BUID
            if (oSampleInvoice.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BUID=" + oSampleInvoice.BUID;
            }
            #endregion
            #region Invoice Type
            if (oSampleInvoice.InvoiceType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "InvoiceType=" + oSampleInvoice.InvoiceType;
            }
            #endregion
            /// Gets Only Cash_Cheque type and have due
            #region Amount
             Global.TagSQL(ref sReturn);
             sReturn = sReturn + "PaymentType=1 and ISNULL((Amount * CRate),0) >ISNULL(AlreadyPaid,0) and Isnull(PaymentSettlementStatus,0)=0";
            #endregion
        
            string sSQL = sReturn1 + sReturn;
            oSampleInvoices = SampleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            foreach(SampleInvoice oItem in oSampleInvoices)
            {
                oPaymentDetail = new PaymentDetail();
                oPaymentDetail.AmountInCurrency= oItem.Amount;
                oPaymentDetail.CurrencyID = oItem.CurrencyID;
                oPaymentDetail.CurrencySymbol = oItem.CurrencySymbol;
                oPaymentDetail.ContractorName = oItem.ContractorName;
                oPaymentDetail.ContractorID = oItem.ContractorID;
                oPaymentDetail.CRate= oItem.ConversionRate;
                oPaymentDetail.AmountWithExchangeRate = (oItem.Amount * oItem.ConversionRate);
                oPaymentDetail.ExchangeCurrencyID = oItem.ExchangeCurrencyID;
                oPaymentDetail.ExchangeCurrencySymbol = oItem.ExchangeCurrencySymbol;
                oPaymentDetail.ReferenceID = oItem.SampleInvoiceID;
                oPaymentDetail.SampleInvoiceNo = oItem.InvoiceNo;
                oPaymentDetail.SampleInvoiceDate = oItem.SampleInvoiceDate;
                oPaymentDetail.YetToPayment = ((oItem.Amount * oItem.ConversionRate) + oItem.AlreadyAdditionalAmount) - (oItem.AlreadyPaid + oItem.AlreadyDiscount);
                oPaymentDetail.PaymentAmount = ((oItem.Amount * oItem.ConversionRate)+oItem.AlreadyAdditionalAmount) - (oItem.AlreadyPaid + oItem.AlreadyDiscount);
                oPaymentDetail.ReferenceType =  (EnumSampleInvoiceType)oItem.InvoiceType;
                oPaymentDetail.ReferenceTypeInInt = oItem.InvoiceType;
                oPaymentDetail.InvoiceType = oItem.InvoiceType;
                oPaymentDetail.DisCount = 0;
                oPaymentDetail.AdditionalAmount = 0;
                oPaymentDetail.AlreadyPaid = oItem.AlreadyPaid;
                oPaymentDetail.SampleInvoiceDate = oItem.SampleInvoiceDate;
                oPaymentDetail.AlreadyDiscount = oItem.AlreadyDiscount;
                oPaymentDetail.AlreadyAdditionalAmount = oItem.AlreadyAdditionalAmount;
                oPaymentDetails.Add(oPaymentDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPaymentDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ContractorGetsByPaymentType(Contractor oContractor) // Added By Mahabub
        {
            _oContractors = new List<Contractor>();
            try
            {
                int nBUID = Convert.ToInt32(oContractor.Params.Split('~')[0]);
                string sSQL = "SELECT * FROM Contractor WHERE ContractorID IN ( SELECT SI.ContractorID FROM View_SampleInvoice AS  SI WHERE PaymentType="+(int)EnumOrderPaymentType.CashOrCheque+" AND BUID = "+nBUID+" AND ISNULL((Amount * CRate),0) >ISNULL(AlreadyPaid,0))";
                _oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oContractors.Count <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                oContractor = new Contractor();
                oContractor.ErrorMessage = ex.Message;
                _oContractors.Add(oContractor);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult SetSessionSearchCriterias(Payment oPayment)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oPayment);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintLists()
        {
            _oPayment = new Payment();
            _oPayments = new List<Payment>();
            _oPayment = (Payment)Session[SessionInfo.ParamObj];

            string sSQL = "SELECT * FROM View_Payment AS HH WHERE HH.PaymentID IN (" + _oPayment.ErrorMessage + ")";
            _oPayments = Payment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            rptPayments oReport = new rptPayments();
            byte[] abytes = oReport.PrepareReport(_oPayments, oCompany, "");
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintPreview(int nID, double nts)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oPayment = _oPayment.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oPayment.PaymentDetails = PaymentDetail.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            #endregion
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPayment.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string _sMessage = "";
            rptPayment oReport = new rptPayment();
            byte[] abytes = oReport.PrepareReport(_oPayment, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");

        }

        #region  print Excel

        public ActionResult SetSessionSearchCriteria(Payment oPayment)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oPayment);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ExcelPaymentInfo(int nBuid)
        {
            string sStartCell = "", sEndCell = "";
            int nStartRow = 0, nEndRow = 0;
            int colIndex = 1;          
            string sErrorMessage = "";
            List<Payment> oPayments = new List<Payment>();

            try
            {
                _oPayment = (Payment)Session[SessionInfo.ParamObj];
                string sql = "select * from View_Payment AS HH WHERE HH.PaymentID IN (" + _oPayment.MRNo + ") ORDER BY HH.MRDate,HH.PaymentID ASC";
                oPayments = Payment.Gets(sql, (int)Session[SessionInfo.currentUserID]);
                if (oPayments.Count <= 0)
                {
                    sErrorMessage = "No Data Found";
                }

            }
            catch (Exception e)
            {
                oPayments = new List<Payment>();
                sErrorMessage = e.Message;
            }
            if (sErrorMessage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBuid, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                #region Excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Payment Info");
                    sheet.Name = "Payment Info";
                    sheet.Column(2).Width = 8; //SL
                    sheet.Column(3).Width = 20; //MR-NO
                    sheet.Column(4).Width = 20; //MR - Date
                    sheet.Column(5).Width = 20; //Status
                    sheet.Column(6).Width = 18; //Party
                    sheet.Column(7).Width = 20; //Pay. Type
                    sheet.Column(8).Width = 16; //currency
                    sheet.Column(9).Width = 16; //Amount
                    sheet.Column(10).Width = 20; //Doc No
                    sheet.Column(11).Width = 20; //Bank
                    sheet.Column(12).Width = 20; //remarks
                   
                    #region Report Header
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true;
                    cell.Value = "Payment Info"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion
                    #region Column Header
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "MR No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "MR Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Status"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Party"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Pay. Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Currency"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Doc No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "Bank"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "Remaks"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

      
                    rowIndex++;
                    #endregion
                    #region Body
                    int sl = 1;
                    nStartRow = rowIndex; nEndRow = 0;
                    foreach (Payment oItem in oPayments)
                    {
                        cell = sheet.Cells[rowIndex, 2]; cell.Merge = true; cell.Value = sl; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sl++;

                        cell = sheet.Cells[rowIndex, 3]; cell.Merge = true; cell.Value = oItem.MRNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.MRDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.PaymentStatus; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.PaymentModeSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.Currency; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.Amount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                           
                        cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.DocNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.BankName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.Note; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        nEndRow = rowIndex;
                        rowIndex++;                       
                    }

                   
                    #endregion

                    #region Grand Total
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Gross Amount                                 
                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Customer_Personal_Info.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

            }
        }

        #endregion

        #region UpdateVoucherEffect

        [HttpPost]
        public JsonResult UpdateVoucherEffect(Payment oPayment)
        {
            try
            {
                oPayment = oPayment.UpdateVoucherEffect((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPayment = new Payment();
                oPayment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}

