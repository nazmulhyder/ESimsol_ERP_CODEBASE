using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class LoanController : Controller
    {
        #region Declaration
        Loan _oLoan = new Loan();
        LoanInstallment _oLoanInstallment = new LoanInstallment();
        LoanSettlement _oLoanSettlement = new LoanSettlement();
        List<LoanSettlement> _oLoanSettlements = new List<LoanSettlement>();
        #endregion

        private bool IsBUExists(List<BusinessUnit> oBusinessUnits, int nBUID)
        {
            foreach (BusinessUnit oItem in oBusinessUnits)
            {
                if (oItem.BusinessUnitID == nBUID)
                {
                    return true;
                }
            }
            return false;
        }
        
        public ActionResult ViewLoans( int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<BusinessUnit> oPermitedBUs = new List<BusinessUnit>();
            List<Loan> oLoans = new List<Loan>();
            //oLoans = Loan.Gets(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.LoanTypes = EnumObject.jGets(typeof(EnumFinanceLoanType));
            ViewBag.LoanRefTypes = EnumObject.jGets(typeof(EnumLoanRefType));
            ViewBag.LoanCompoundTypes = EnumObject.jGets(typeof(EnumLoanCompoundType));
            ViewBag.CycleTypes = EnumObject.jGets(typeof(EnumCycleType));
            ViewBag.LoanTransferTypes = EnumObject.jGets(typeof(EnumLoanTransfer));
            ViewBag.BankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oPermitedBUs =BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]); 
            string sSQL = "SELECT * FROM View_Loan WHERE LoanStatus = "+(int)EnumLoanStatus.Initialize+" AND BUID IN ("+string.Join(",", oPermitedBUs.Select(x=>x.BusinessUnitID.ToString()))+")";
            oLoans = Loan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUs = oPermitedBUs;

            return View(oLoans);
        }

        public ActionResult ViewLoan(int id)
        {
            Loan oLoan = new Loan();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oBusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

            if (id > 0)
            {
                oLoan = oLoan.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oLoan.LoanInstallments = LoanInstallment.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oLoan.LoanInterests = LoanInterest.GetsByLoan(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSql = "SELECT * FROM View_LoanSettlement WHERE LoanID = " + id.ToString();
                _oLoanSettlements = LoanSettlement.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oLoanSettlements.Count > 0)
                {
                    foreach (LoanInstallment oItem in oLoan.LoanInstallments)
                    {
                        oItem.PaymentList = _oLoanSettlements.Where(x => x.LoanInstallmentID == oItem.LoanInstallmentID && x.BankAccountID != 0).ToList();
                        oItem.LoanchargeList = _oLoanSettlements.Where(x => x.LoanInstallmentID == oItem.LoanInstallmentID && x.ExpenseHeadID != 0).ToList();
                    }
                }

                if (!IsBUExists(oBusinessUnits, oLoan.BUID))
                {
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = oBusinessUnit.Get(oLoan.BUID, (int)Session[SessionInfo.currentUserID]);
                    oBusinessUnits.Add(oBusinessUnit);
                }
            }
            Company oCompany = new Company();
            ViewBag.LoanTypes = EnumObject.jGets(typeof(EnumFinanceLoanType));
            ViewBag.LoanRefTypes = EnumObject.jGets(typeof(EnumLoanRefType));
            ViewBag.LoanCompoundTypes = EnumObject.jGets(typeof(EnumLoanCompoundType));
            ViewBag.CycleTypes = EnumObject.jGets(typeof(EnumCycleType));
            ViewBag.LoanTransferTypes = EnumObject.jGets(typeof(EnumLoanTransfer));
            ViewBag.BaseCurrency = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ExpenditureHeads = ExpenditureHead.Gets((int)Session[SessionInfo.currentUserID]);           
            ViewBag.BUs = oBusinessUnits;
            return View(oLoan);
        }

        [HttpPost]
        public ActionResult SaveLoan(Loan oLoan)
        {
            _oLoan = new Loan();
            _oLoan = oLoan;
            try
            {

                _oLoan = _oLoan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oLoan = new Loan();
                _oLoan.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLoan);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        [HttpPost]
        public ActionResult UpdateStmlStartDate(Loan oLoan)
        {
            _oLoan = new Loan();
            _oLoan = oLoan;
            try
            {

                _oLoan = _oLoan.UpdateStmlStartDate(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oLoan = new Loan();
                _oLoan.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLoan);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            string sFeedBackMassage = "";
            try
            {
                sFeedBackMassage = _oLoan.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMassage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMassage);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;

        }
        public ActionResult ApproveLoan(Loan oLoan)
        {
            _oLoan = new Loan();
            _oLoan = oLoan;
            try
            {
                _oLoan = _oLoan.ApproveOrReceived(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLoan = new Loan();
                _oLoan.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLoan);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;

        }
        public ActionResult ReceiveLoan(Loan oLoan)
        {
            _oLoan = new Loan();
            _oLoan = oLoan;
            try
            {
                _oLoan = _oLoan.ApproveOrReceived(false, ((User)Session[SessionInfo.CurrentUser]).UserID);//bIsLoand: Received
            }
            catch (Exception ex)
            {
                _oLoan = new Loan();
                _oLoan.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLoan);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;

        }
               
        [HttpPost]
        public JsonResult AdvanceSearch(Loan oLoan)
        {

            List<Loan> oLoans = new List<Loan>();

            try
            {
                string sSQL = GetSQL(oLoan);
                oLoans = Loan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLoan = new Loan();
                oLoans = new List<Loan>();
                oLoan.ErrorMessage = ex.Message;
                oLoans.Add(oLoan);
            }

            var jsonResult = Json(oLoans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SetPrintingData(Loan oLoan)
        {
            _oLoan.SearchingData = oLoan.ErrorMessage == null ? "" : oLoan.ErrorMessage;
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oLoan.SearchingData);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oLoan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintLoans()
        {
            Loan oLoan = new Loan();
            List<Loan> oLoans = new List<Loan>();

            var ids = (string)Session[SessionInfo.ParamObj];

            string _sSQL = "SELECT * FROM View_Loan AS HH WHERE HH.LoanID IN (" + ids + ") ORDER BY LoanID ASC";
            oLoans = Loan.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = this.GetCompanyLogo(oCompany);

            rptLoans oReport = new rptLoans();
            byte[] abytes = oReport.PrepareReport(oLoans, oCompany);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintPreview()
        {
            Loan oLoan = new Loan();
            List<LoanSettlement> oLoanSettlements = new List<LoanSettlement>();
            List<LoanInstallment> oLoanInstallments = new List<LoanInstallment>();

            var id = Convert.ToInt32(Session[SessionInfo.ParamObj]);
            oLoan = oLoan.Get(id, (int)Session[SessionInfo.currentUserID]);

            string sSql = "SELECT * FROM View_LoanInstallment WHERE LoanID = " + id;
            oLoanInstallments = LoanInstallment.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            sSql = "SELECT * FROM View_LoanSettlement WHERE LoanID = " + id;
            oLoanSettlements = LoanSettlement.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            #endregion

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string _sMessage = "";
            rptLoanPreview oReport = new rptLoanPreview();
            byte[] abytes = oReport.PrepareReport(oLoan, oLoanInstallments, oLoanSettlements, oCompany, oBusinessUnit);
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

        [HttpPost]
        public JsonResult GetByLoanORRefNo(Loan oLoan)
        {
            List<Loan> oLoans = new List<Loan>();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();

            try
            {
                ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
                oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
                oBusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
                string sBusinessUnitIDs = "";
                foreach (BusinessUnit oItem in oBusinessUnits)
                {
                    if (oItem.BusinessUnitID > 0)
                    {
                        sBusinessUnitIDs = sBusinessUnitIDs + oItem.BusinessUnitID + ",";
                    }
                }

                if (sBusinessUnitIDs.Length > 0)
                {

                    sBusinessUnitIDs = sBusinessUnitIDs.Remove(sBusinessUnitIDs.Length - 1, 1);
                }
                string sSql = "SELECT * FROM View_Loan AS HH WHERE HH.BUID IN (" + sBusinessUnitIDs + ")";
                if (oLoan.LoanNo != null && oLoan.LoanNo != "")
                {
                    Global.TagSQL(ref sSql);
                    sSql = sSql + " HH.LoanNo LIKE '%" + oLoan.LoanNo + "%'";
                }
                if (oLoan.LoanRefTypeInt != (int)EnumFinanceLoanType.None)
                {
                    Global.TagSQL(ref sSql);
                    sSql = sSql + " HH.LoanRefType = " + oLoan.LoanRefTypeInt.ToString();
                }
                if (oLoan.LoanRefNo != null && oLoan.LoanRefNo != "")
                {
                    Global.TagSQL(ref sSql);
                    sSql = sSql + " HH.LoanRefNo LIKE '%" + oLoan.LoanRefNo + "%'";
                }
                sSql = sSql + "  ORDER BY HH.LoanID ASC";
                oLoans = Loan.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLoans = new List<Loan>();
                oLoan = new Loan();
                oLoan.ErrorMessage = ex.Message;
                oLoans.Add(oLoan);
            }

            var jsonResult = Json(oLoans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult GetByImportOrExportLC(Loan oLoan)
        {

            List<Loan> oLoans = new List<Loan>();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            var BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            var BUnitIds = "";
            foreach (BusinessUnit oItem in BUs)
            {
                BUnitIds = BUnitIds + oItem.BusinessUnitID + ",";
            }

            BUnitIds = BUnitIds.TrimEnd(',');

            try
            {
                string sSql = "SELECT * FROM View_Loan AS HH WHERE HH.LoanRefType = " + oLoan.LoanRefType + " HH.LoanRefNo LIKE '%" + oLoan.LoanRefNo + "%'" + "AND HH.BUID IN (" + BUnitIds + ")";
                oLoans = Loan.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oLoans = new List<Loan>();
                oLoan = new Loan();
                oLoan.ErrorMessage = ex.Message;
                oLoans.Add(oLoan);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoans);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;

        }

        private string GetSQL(Loan oLoan)
        {

            string sSearchingData = oLoan.SearchingData;

            int IssueDate = Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            int LoanStartDate = Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dLoanStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dLoanEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            int ApproxDate = Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dApproxDateStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dApproxDateEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            int TransferDate = Convert.ToInt32(sSearchingData.Split('~')[9]);
            DateTime dTransferStartDate = Convert.ToDateTime(sSearchingData.Split('~')[10]);
            DateTime dTransferEndDate = Convert.ToDateTime(sSearchingData.Split('~')[11]);

            int SettlementDate = Convert.ToInt32(sSearchingData.Split('~')[12]);
            DateTime dSettlementStartDate = Convert.ToDateTime(sSearchingData.Split('~')[13]);
            DateTime dSettlementEndDate = Convert.ToDateTime(sSearchingData.Split('~')[14]);

            int LoanAmount = Convert.ToInt32(sSearchingData.Split('~')[15]);
            Double nLoanStartAmount = Convert.ToDouble(sSearchingData.Split('~')[16]);
            Double nLoanEndAmount = Convert.ToDouble(sSearchingData.Split('~')[17]);

            int InterestRate = Convert.ToInt32(sSearchingData.Split('~')[18]);
            Double nInterestRateStartAmount = Convert.ToDouble(sSearchingData.Split('~')[19]);
            Double nInterestRateEndAmount = Convert.ToDouble(sSearchingData.Split('~')[20]);

            int LiborRate = Convert.ToInt32(sSearchingData.Split('~')[21]);
            Double nLiborRateStartAmount = Convert.ToDouble(sSearchingData.Split('~')[22]);
            Double nLiborRateEndAmount = Convert.ToDouble(sSearchingData.Split('~')[23]);

            int TotalChargeAmount = Convert.ToInt32(sSearchingData.Split('~')[24]);
            Double nLoanChargeStartAmount = Convert.ToDouble(sSearchingData.Split('~')[25]);
            Double nLoanChargeEndAmount = Convert.ToDouble(sSearchingData.Split('~')[26]);

            int TotalInterestAmount = Convert.ToInt32(sSearchingData.Split('~')[27]);
            Double nTotalInterestStartAmount = Convert.ToDouble(sSearchingData.Split('~')[28]);
            Double nTotalInterestEndAmount = Convert.ToDouble(sSearchingData.Split('~')[29]);

            int SettlementAmount = Convert.ToInt32(sSearchingData.Split('~')[30]);
            Double nSettlementAmountStartAmount = Convert.ToDouble(sSearchingData.Split('~')[31]);
            Double nSettlementAmountEndAmount = Convert.ToDouble(sSearchingData.Split('~')[32]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            List<BusinessUnit> oPermitedBUs = new List<BusinessUnit>();
            oPermitedBUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
           


            string sReturn1 = "SELECT * FROM View_Loan AS HH";
            string sReturn = " WHERE HH.BUID IN (" + string.Join(",", oPermitedBUs.Select(x => x.BusinessUnitID.ToString())) + ")";

            #region LoanNo
            if (oLoan.LoanNo != null && oLoan.LoanNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.LoanNo LIKE '%" + oLoan.LoanNo + "%'";
            }
            #endregion

            #region LoanType
            if (oLoan.LoanTypeInt > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.LoanType = " + oLoan.LoanTypeInt;
            }
            #endregion

            #region LoanRefNo
            if (oLoan.LoanRefTypeInt > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.LoanRefType = " + oLoan.LoanRefTypeInt + "AND HH.LoanRefNo LIKE '%" + oLoan.LoanRefNo + "%'";
            }
            #endregion

            #region RcvBankAccountType
            if (oLoan.RcvBankAccountID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.RcvBankAccountID = " + oLoan.RcvBankAccountID;
            }
            #endregion

            #region Date & Amount
            if (IssueDate != (int)EnumCompareOperator.None)
            {
                if (IssueDate == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                }

                else if (IssueDate == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }

            if (LoanStartDate != (int)EnumCompareOperator.None)
            {
                if (LoanStartDate == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.LoanStartDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanStartDate.ToString("dd MMM yyyy") + "', 106))";
                }

                else if (LoanStartDate == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.LoanStartDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }

            if (ApproxDate != (int)EnumCompareOperator.None)
            {
                if (ApproxDate == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.ApproxSettlement,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproxDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }

                else if (ApproxDate == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.ApproxSettlement,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproxDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApproxDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }

            if (TransferDate != (int)EnumCompareOperator.None)
            {
                if (TransferDate == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.TransferDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dTransferStartDate.ToString("dd MMM yyyy") + "', 106))";
                }

                else if (TransferDate == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.TransferDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dTransferStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dTransferEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }

            if (SettlementDate != (int)EnumCompareOperator.None)
            {
                if (SettlementDate == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.SettlementDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSettlementStartDate.ToString("dd MMM yyyy") + "', 106))";
                }

                else if (SettlementDate == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.SettlementDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSettlementStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSettlementEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }

            if (LoanAmount > 0)
            {
                if (LoanAmount == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LoanAmount = " + nLoanStartAmount;
                }
                if (LoanAmount == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LoanAmount >= " + nLoanStartAmount + " AND LoanAmount < " + nLoanEndAmount;
                }
            }

            if (InterestRate > 0)
            {
                if (InterestRate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InterestRate = " + nInterestRateStartAmount;
                }
                if (InterestRate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InterestRate >= " + nInterestRateStartAmount + " AND InterestRate < " + nInterestRateEndAmount;
                }
            }

            if (LiborRate > 0)
            {
                if (LiborRate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LiborRate = " + nLiborRateStartAmount;
                }
                if (LiborRate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LiborRate >= " + nLiborRateStartAmount + " AND LiborRate < " + nLiborRateEndAmount;
                }
            }

            if (TotalChargeAmount > 0)
            {
                if (TotalChargeAmount == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalChargeAmount = " + nLoanChargeStartAmount;
                }
                if (TotalChargeAmount == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalChargeAmount >= " + nLoanChargeStartAmount + " AND TotalChargeAmount < " + nLoanChargeEndAmount;
                }
            }

            if (TotalInterestAmount > 0)
            {
                if (TotalInterestAmount == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalInterestAmount = " + nTotalInterestStartAmount;
                }
                if (TotalInterestAmount == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalInterestAmount >= " + nTotalInterestStartAmount + " AND TotalInterestAmount < " + nTotalInterestEndAmount;
                }
            }

            if (SettlementAmount > 0)
            {
                if (SettlementAmount == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SettlementAmount = " + nSettlementAmountStartAmount;
                }
                if (SettlementAmount == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SettlementAmount >= " + nSettlementAmountStartAmount + " AND SettlementAmount < " + nSettlementAmountEndAmount;
                }
            }

            #endregion

            #region BU Cluse
            if (oLoan.BUID != 0 )
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.BUID =" + oLoan.BUID;
            }
            #endregion


            sReturn = sReturn1 + sReturn + " order by HH.LoanID ASC";
            return sReturn;
        }

        #region Get master LC
        [HttpPost]
        public JsonResult GetExportLC(ExportLC oExportLC)
        {
            string sSQL = "", sTempSql = "";
            var oExportLCs = new List<ExportLC>();
            try
            {
                sSQL = "SELECT * FROM View_ExportLC";
                if (oExportLC.BUID != 0)
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += " BUID =" + oExportLC.BUID;
                }
                if (!string.IsNullOrEmpty(oExportLC.ExportLCNo))
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += "  ExportLCNo LIKE '%" + oExportLC.ExportLCNo + "%'";
                }

                sSQL += sTempSql;
                oExportLCs = ExportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportLCs = new List<ExportLC>();
                oExportLC = new ExportLC();
                oExportLC.ErrorMessage = ex.Message;
                oExportLCs.Add(oExportLC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportLCs);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        #endregion

        #region Gets Import PI
        [HttpPost]
        public JsonResult GetsImportInvoice(ImportInvoice oImportInvoice)
        {
            string sSQL = "", sTempSql = "";
            var oImportInvoices = new List<ImportInvoice>();
            try
            {
                sSQL = "SELECT * FROM View_ImportInvoice";
                if (oImportInvoice.BUID!=0)
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += " BUID =" + oImportInvoice.BUID;
                }

                if (!string.IsNullOrEmpty(oImportInvoice.ImportInvoiceNo))
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += " ImportInvoiceNo Like '%" + oImportInvoice.ImportInvoiceNo + "%'";
                }
                sSQL += sTempSql;

                oImportInvoices = ImportInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportInvoices = new List<ImportInvoice>();
                oImportInvoice = new ImportInvoice();
                oImportInvoice.ErrorMessage = ex.Message;
                oImportInvoices.Add(oImportInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportInvoices);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        #endregion

        #region Loan Settlement
        [HttpPost]
        public JsonResult GetLoanInstallment(LoanInstallment oLoanInstallment)
        {
            _oLoanInstallment = new LoanInstallment();
            try
            {
                if (oLoanInstallment.LoanInstallmentID > 0)
                {
                    _oLoanInstallment = _oLoanInstallment.Get(oLoanInstallment.LoanInstallmentID, (int)Session[SessionInfo.currentUserID]);
                    string sSQL = "SELECT * FROM View_LoanSettlement AS HH WHERE HH.LoanInstallmentID = " + oLoanInstallment.LoanInstallmentID.ToString() + " AND ISNULL(HH.BankAccountID,0) = 0 AND ISNULL(HH.ExpenseHeadID,0) > 0 ORDER BY HH.LoanSettlementID ASC";
                    _oLoanInstallment.LoanchargeList = LoanSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    sSQL = "SELECT * FROM View_LoanSettlement AS HH WHERE HH.LoanInstallmentID = " + oLoanInstallment.LoanInstallmentID.ToString() + " AND ISNULL(HH.BankAccountID,0) > 0 AND ISNULL(HH.ExpenseHeadID,0) = 0 ORDER BY HH.LoanSettlementID ASC";
                    _oLoanInstallment.PaymentList = LoanSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    Loan oLoan = new Loan();
                    oLoan = oLoan.Get(oLoanInstallment.LoanID, (int)Session[SessionInfo.currentUserID]);
                    _oLoanInstallment = new LoanInstallment();
                    _oLoanInstallment.LoanInstallmentID = 0;
                    _oLoanInstallment.LoanID = oLoan.LoanID;
                    _oLoanInstallment.InstallmentNo = "";
                    _oLoanInstallment.InstallmentStartDate = oLoan.StlmtStartDate;
                    _oLoanInstallment.InstallmentDate = oLoan.ApproxSettlement;
                    _oLoanInstallment.LoanPrincipalAmount = oLoan.PrincipalAmount;
                    _oLoanInstallment.PrincipalAmount = oLoan.LoanAmount;
                    _oLoanInstallment.LoanTransferType = EnumLoanTransfer.None;
                    _oLoanInstallment.LoanTransferTypeInt = 0;
                    _oLoanInstallment.TransferDate = DateTime.MinValue;
                    _oLoanInstallment.TransferDays = 0;
                    _oLoanInstallment.TransferInterestRate = 0;
                    _oLoanInstallment.TransferInterestAmount = 0;
                    _oLoanInstallment.SettlementDate = oLoan.ApproxSettlement;
                    _oLoanInstallment.InterestDays = (Convert.ToInt32((oLoan.ApproxSettlement - oLoan.StlmtStartDate).TotalDays));
                    _oLoanInstallment.InterestRate = oLoan.InterestRate;
                    _oLoanInstallment.InterestAmount = (((oLoan.LoanAmount * oLoan.InterestRate) / 100.00) / 360.00) * Convert.ToDouble(_oLoanInstallment.InterestDays);
                    _oLoanInstallment.LiborRate = oLoan.LiborRate;
                    _oLoanInstallment.LiborInterestAmount = (((oLoan.LoanAmount * oLoan.LiborRate) / 100.00) / 360.00) * Convert.ToDouble(_oLoanInstallment.InterestDays);
                    _oLoanInstallment.TotalInterestAmount = _oLoanInstallment.InterestAmount;
                    _oLoanInstallment.ChargeAmount = 0;
                    _oLoanInstallment.DiscountPaidAmount = 0;
                    _oLoanInstallment.DiscountRcvAmount = 0;
                    _oLoanInstallment.TotalPayableAmount = (oLoan.LoanAmount + _oLoanInstallment.InterestAmount);
                    _oLoanInstallment.PaidAmount = 0;
                    _oLoanInstallment.PaidAmountBC = 0;
                    _oLoanInstallment.PrincipalDeduct = 0;
                    _oLoanInstallment.PrincipalBalance = oLoan.LoanAmount;
                    _oLoanInstallment.Remarks = "";
                    _oLoanInstallment.SettlementBy = 0;
                    _oLoanInstallment.SettlementByName = "";
                    _oLoanInstallment.FileNo = oLoan.FileNo;
                    _oLoanInstallment.LoanNo = oLoan.LoanNo;
                    _oLoanInstallment.LoanRefType = oLoan.LoanRefType;
                    _oLoanInstallment.LoanCRate = oLoan.CRate;
                    _oLoanInstallment.LoanRefID = oLoan.LoanRefID;
                    _oLoanInstallment.LoanRefNo = oLoan.LoanRefNo;
                    _oLoanInstallment.LoanType = oLoan.LoanType;
                    _oLoanInstallment.LoanTypeInt = oLoan.LoanTypeInt;
                    _oLoanInstallment.ApproxSettlement = oLoan.ApproxSettlement;
                    _oLoanInstallment.IssueDate = oLoan.IssueDate;
                    _oLoanInstallment.BankAccNo = oLoan.BankAccNo;
                    _oLoanInstallment.LoanCurencyID = oLoan.LoanCurencyID;
                    _oLoanInstallment.LoanCurency = oLoan.CurrencySymbol;
                    _oLoanInstallment.LoanchargeList = new List<LoanSettlement>();
                    _oLoanInstallment.PaymentList = new List<LoanSettlement>();
                }
            }
            catch (Exception ex)
            {
                _oLoanInstallment = new LoanInstallment();
                _oLoanInstallment.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oLoanInstallment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLoanInstallment(LoanInstallment oLoanInstallment)
        {
            _oLoanInstallment = new LoanInstallment();
            try
            {
                _oLoanInstallment = oLoanInstallment.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLoanInstallment = new LoanInstallment();
                _oLoanInstallment.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oLoanInstallment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApprovedLoanInstallment(LoanInstallment oLoanInstallment)
        {
            _oLoanInstallment = new LoanInstallment();
            try
            {
                _oLoanInstallment = oLoanInstallment.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLoanInstallment = new LoanInstallment();
                _oLoanInstallment.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oLoanInstallment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLoanInstallment(LoanInstallment oLoanInstallment)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLoanInstallment.Delete(oLoanInstallment.LoanInstallmentID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {                
                sFeedBackMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Loan Tag  LC
        [HttpPost]
        public JsonResult GetLoanExportLC(Loan oLoan)
        {
            List<LoanExportLC> oLoanExportLCs = new List<LoanExportLC>();
            LoanExportLC oLoanExportLC = new LoanExportLC();
            Loan oTempLoan = new Loan();

            try
            {
                string sSql = "SELECT * FROM View_LoanExportLC AS HH WHERE HH.LoanID = " + oLoan.LoanID;
                oLoanExportLCs = LoanExportLC.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLoanExportLCs.Any())
                {
                    oLoanExportLCs.Where(x => x.ExportLCID == oLoan.LoanRefID).ToList().ForEach(b => b.isDeletable = false);
                    oLoanExportLCs.Where(x => x.ExportLCID != oLoan.LoanRefID).ToList().ForEach(b => b.isDeletable = true);
                }
                else
                {
                    oTempLoan = oLoan;
                    oTempLoan = oTempLoan.Get(oTempLoan.LoanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    List<ExportLC> oExportLCs = ExportLC.Gets("SELECT Amount,CurrencySymbol,ExportLCNo FROM View_ExportLC WHERE ExportLCID= " + oLoan.LoanRefID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oLoanExportLCs.Add(new LoanExportLC
                    {
                        LoanExportLCID= 0,
                        LoanID = oTempLoan.LoanID,
                        ExportLCID=oTempLoan.LoanRefID,
                        Amount = oTempLoan.LoanAmount,
                        Remarks ="",
                        isDeletable=false,
                        ExportLCAmount = oExportLCs.First().Amount,
                        ExportLCCurrencySymbol = oExportLCs.First().Currency,
                        ExportLCNo= oExportLCs.First().ExportLCNo
                    });
                   
                }

            }
            catch (Exception ex)
            {
                oLoanExportLCs = new List<LoanExportLC>();
                oLoanExportLC = new LoanExportLC();
                oLoanExportLC.ErrorMessage = ex.Message;
                oLoanExportLCs.Add(oLoanExportLC);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoanExportLCs);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetExportLCForLoanExportLC(ExportLC oExportLC)
        {
            string sSQL = "", sTempSql = "";
            var oExportLCs = new List<ExportLC>();
            List<LoanExportLC> oLoanExportLCs = new List<LoanExportLC>();
            LoanExportLC oLoanExportLC = new LoanExportLC();
            try
            {
                int nLoanID =Convert.ToInt32(oExportLC.Params.Split('~')[1]);
                sSQL = "SELECT * FROM View_ExportLC";
                if (oExportLC.BUID != 0)
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += " BUID =" + oExportLC.BUID;
                }
                if (!string.IsNullOrEmpty(oExportLC.ExportLCNo))
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += "  ExportLCNo LIKE '%" + oExportLC.ExportLCNo + "%'";
                }
                if (!string.IsNullOrEmpty(oExportLC.Params.Split('~')[0]))
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += "  ExportLCID NOT IN(" + oExportLC.Params.Split('~')[0] + ")";
                }
                sSQL += sTempSql;
                oExportLCs = ExportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportLC oItem in oExportLCs)
                {       
                        oLoanExportLC = new LoanExportLC();
                        oLoanExportLC.LoanExportLCID= 0;
                        oLoanExportLC.LoanID = nLoanID;
                        oLoanExportLC.ExportLCID=oItem.ExportLCID;
                        oLoanExportLC.Amount = 0;
                        oLoanExportLC.Remarks ="";
                        oLoanExportLC.isDeletable=true;
                        oLoanExportLC.ExportLCAmount = oItem.Amount;
                        oLoanExportLC.ExportLCCurrencySymbol = oItem.Currency;
                        oLoanExportLC.ExportLCNo = oItem.ExportLCNo;
                        oLoanExportLCs.Add(oLoanExportLC);
                }
            }
            catch (Exception ex)
            {
                oLoanExportLCs = new List<LoanExportLC>();
                oLoanExportLC = new LoanExportLC();
                oLoanExportLC.ErrorMessage = ex.Message;
                oLoanExportLCs.Add(oLoanExportLC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoanExportLCs);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public ActionResult SaveLoanExportLC(LoanExportLC oLoanExportLC)
        {
            
            try
            {

                oLoanExportLC = oLoanExportLC.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                List<Loan> oLoans = Loan.Gets("SELECT * FROM View_Loan WHERE LoanID= "+oLoanExportLC.LoanExportLCs.FirstOrDefault().LoanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLoanExportLC.LoanExportLCs.Any())
                {
                    oLoanExportLC.LoanExportLCs.Where(x => x.ExportLCID == oLoans.FirstOrDefault().LoanRefID).ToList().ForEach(b => b.isDeletable = false);
                    oLoanExportLC.LoanExportLCs.Where(x => x.ExportLCID != oLoans.FirstOrDefault().LoanRefID).ToList().ForEach(b => b.isDeletable = true);
                }

            }
            catch (Exception ex)
            {
                _oLoan = new Loan();
                _oLoan.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoanExportLC);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        [HttpGet]
        public ActionResult DeleteExportLC(int id)
        {
            string sFeedBackMassage = "";
            LoanExportLC oLoanExportLC = new LoanExportLC();
            try
            {
                //oLoanExportLC
                sFeedBackMassage = oLoanExportLC.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMassage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMassage);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;

        }
        #endregion

    }
}