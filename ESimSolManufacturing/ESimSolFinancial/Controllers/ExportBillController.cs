using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using ESimSol.Reports;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ExportBillController : Controller
    {
        //private string _sReturn = "";
        ExportBill _oExportBill = new ExportBill();
        List<ExportBill> _oExportBills = new List<ExportBill>();
        List<Currency> _oCurrencys = new List<Currency>();

        //ExportClaimSettle
        ExportClaimSettle _oExportClaimSettle = new ExportClaimSettle();
        List<ExportClaimSettle> _oExportClaimSettles = new List<ExportClaimSettle>();

        #region View bill operation
        public ActionResult ViewExportBills(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ExportBill> oExportBills = new List<ExportBill>();
            //string sSQL = "SELECT * FROM View_ExportBill where [State]<1 and BUID=" + buid;
            //oExportBills = ExportBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            }
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);

            List<BankBranch> oBankBranchs_Nego = new List<BankBranch>();
            oBankBranchs_Nego = BankBranch.GetsOwnBranchs((int)Session[SessionInfo.currentUserID]);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); //Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.BankBranchs_Nego = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCBillEventObj = EnumObject.jGets(typeof(EnumLCBillEvent));

            List<BankAccount> oBankAccounts = new List<BankAccount>();

            List<Currency> oCurrencys = new List<Currency>();

            oBankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]);
            oCurrencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);

            ViewBag.BankAccounts = oBankAccounts;
            ViewBag.Currencys = oCurrencys;
            //ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.Companys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.LCTermss = LCTerm.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.LCTypes = EnumObject.jGets(typeof(EnumExportLCType));
            return View(oExportBills);
        }
        public ActionResult ViewExportBill_SendToParty(int nId, double ts)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            if (nId <= 0) { throw new Exception("Please select a valid contractor."); }
            _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
            oExportBillHistory.State = EnumLCBillEvent.BOEInCustomerHand;
            oExportBillHistory = oExportBillHistory.Getby(nId, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
            _oExportBill.NoteCarry = oExportBillHistory.Note;

            //ViewBag.BUID = buid;
            return View(_oExportBill);
        }
        public ActionResult ViewExportBill_ReceiveFromParty(int nId, double ts)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            if (nId > 0)
            {
                _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.BuyerAcceptedBill;
                oExportBillHistory = oExportBillHistory.Getby(nId, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;
            }
            //ViewBag.BUID = buid;
            return View(_oExportBill);
        }
        public ActionResult ViewExportBill_SendToBank(int nId, double ts)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            if (nId > 0)
            {
                _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.NegoTransit;
                oExportBillHistory = oExportBillHistory.Getby(nId, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;

             
            }
            //ViewBag.BUID = buid;
            return View(_oExportBill);
        }
        public ActionResult ViewExportBill_RecdFromBank(int nId, double ts)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            if (nId > 0)
            {
                _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.NegotiatedBill;
                oExportBillHistory = oExportBillHistory.Getby(nId, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;
            }
            Company oCompany = new Company();
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            return View(_oExportBill);
        }
        public ActionResult ViewExportBill_MaturityReceive(int nId, double ts)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            ExportLC oExportLC = new ExportLC();
            LCTerm oLCTerm = new LCTerm();
            if (nId > 0)
            {
                _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.BankAcceptedBill;
                oExportBillHistory = oExportBillHistory.Getby(nId, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;
                oExportLC = oExportLC.Get(_oExportBill.ExportLCID, (int)Session[SessionInfo.currentUserID]);
                if (_oExportBill.LCTramsID <= 0)
                {
                    _oExportBill.LCTramsID = oExportLC.LCTramsID;
                }
                _oExportBill.LCTermsName = oExportLC.LCTermsName + " From the Date of " + ((EnumPaymentInstruction)oExportLC.PaymentInstruction).ToString();

                if (_oExportBill.MaturityDate == DateTime.MinValue)
                {
                    oLCTerm = oLCTerm.Get(_oExportBill.LCTramsID,(int)Session[SessionInfo.currentUserID]);

                    if (oExportLC.PaymentInstruction == EnumPaymentInstruction.BL)
                    {
                            if (_oExportBill.MaturityDate == DateTime.MinValue) { _oExportBill.MaturityDate = DateTime.Now; }
                            //  _oExportBill.MaturityDate = _oExportBill.MaturityDate.AddDays(oExportLC.Da);
                        //_oExportBill.LCTermsName = _oExportBill.LCTermsName + ",  BL Date:" + _oExportBill.BLDateSt;
                    }
                    if (oExportLC.PaymentInstruction == EnumPaymentInstruction.LCOpen)
                    {
                        if (_oExportBill.MaturityDate == DateTime.MinValue) { _oExportBill.MaturityDate = DateTime.Now; }
                        _oExportBill.LCTermsName = _oExportBill.LCTermsName + ",  LCOpen Date:" + oExportLC.OpeningDate.ToString("dd MMM yyyy");
                        if (oLCTerm.Days > 0)
                        {
                            _oExportBill.MaturityDate = oExportLC.OpeningDate.AddDays(oLCTerm.Days);
                        }
                    }
                    if (oExportLC.PaymentInstruction == EnumPaymentInstruction.Acceptence)
                    {
                        if (_oExportBill.MaturityDate == DateTime.MinValue) { _oExportBill.MaturityDate = DateTime.Now; }
                        _oExportBill.LCTermsName = _oExportBill.LCTermsName + ",  Acceptence Date:";
                        if (oLCTerm.Days > 0)
                        {
                            _oExportBill.MaturityDate = _oExportBill.MaturityDate.AddDays(oLCTerm.Days);
                        }
                    }
                    if (oExportLC.PaymentInstruction == EnumPaymentInstruction.Negotiation)
                    {
                        if (_oExportBill.MaturityDate == DateTime.MinValue) { _oExportBill.MaturityDate = DateTime.Now; }
                        _oExportBill.LCTermsName = _oExportBill.LCTermsName + ",  Negotiation Date:";
                        if (oLCTerm.Days > 0)
                        {
                            _oExportBill.MaturityDate = _oExportBill.MaturityDate.AddDays(oLCTerm.Days);
                        }
                    }
                }

            }
            Company oCompany = new Company();
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oExportBill);
        }
        public ActionResult ViewExportBill_SAN(int nId, double ts)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            if (nId > 0)
            {
                _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.BankAcceptedBill;
                oExportBillHistory = oExportBillHistory.Getby(nId, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;


            }
            //ViewBag.BUID = buid;
            return View(_oExportBill);
        }
        public ActionResult ViewExportBill_BillRelization(int nId, double ts)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            if (nId > 0)
            {
                _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.BillRealized;
                oExportBillHistory = oExportBillHistory.Getby(nId, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;

                _oExportBill.LCTermss = LCTerm.Gets((int)Session[SessionInfo.currentUserID]);
                _oExportBill.ExportBillRealizeds = ExportBillRealized.Gets(_oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

                List<ExportBillParticular> oExportBillParticulars = new List<ExportBillParticular>();
                oExportBillParticulars = ExportBillParticular.Gets(true, (int)Session[SessionInfo.currentUserID]);
                ViewBag.ExportBillParticulars = oExportBillParticulars;
                ViewBag.Company = oCompany;
            }
            //ViewBag.BUID = buid;
            return View(_oExportBill);
        }
        public ActionResult ViewExportBill_FDDinHand(int nId, double ts)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            if (nId > 0)
            {
                _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.FDDInHand;
                oExportBillHistory = oExportBillHistory.Getby(nId, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;
                _oExportBill.LCTermss = LCTerm.Gets((int)Session[SessionInfo.currentUserID]);                
            }            
            return View(_oExportBill);
        }
        public ActionResult ViewExportBill_Encashment(int nId, double ts)
        {
            Company oCompany = new Company();
            List<Loan> oLoans = new List<Loan>();
            LoanInstallment oLoanInstallment = new LoanInstallment();
            ExportBillEncashment oExportBillEncashment = new ExportBillEncashment();
            List<ExportBillEncashment> oExportBillEncashments = new List<ExportBillEncashment>();
            if (nId > 0)
            {
                _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);                
                _oExportBill.ExportBillEncashments = ExportBillEncashment.Gets(_oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);
                foreach (ExportBillEncashment oItem in _oExportBill.ExportBillEncashments)
                {
                    if (oItem.LoanInstallmentID > 0)
                    { 
                        oLoanInstallment = new LoanInstallment();
                        oLoanInstallment = oLoanInstallment.Get(oItem.LoanInstallmentID, (int)Session[SessionInfo.currentUserID]);
                        string sSQL = "SELECT * FROM View_LoanSettlement AS HH WHERE HH.LoanInstallmentID = " + oItem.LoanInstallmentID.ToString() + " AND ISNULL(HH.BankAccountID,0) = 0 AND ISNULL(HH.ExpenseHeadID,0) > 0 ORDER BY HH.LoanSettlementID ASC";
                        oLoanInstallment.LoanchargeList = LoanSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        sSQL = "SELECT * FROM View_LoanSettlement AS HH WHERE HH.LoanInstallmentID = " + oItem.LoanInstallmentID.ToString() + " AND ISNULL(HH.BankAccountID,0) > 0 AND ISNULL(HH.ExpenseHeadID,0) = 0 ORDER BY HH.LoanSettlementID ASC";
                        oLoanInstallment.PaymentList = LoanSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        oItem.LoanInstallment = oLoanInstallment;
                    }
                }
                if (_oExportBill.EncashCRate <= 0)
                {
                    if (_oExportBill.CurrencyID == _oExportBill.EncashCurrencyID)
                    {
                        _oExportBill.EncashCRate = 1;
                        _oExportBill.EncashAmountBC = _oExportBill.Amount;
                    }                     
                }
            }
            List<EnumObject> oTrnasferTypes = new List<EnumObject>();
            List<EnumObject> oTempTrnasferTypes = EnumObject.jGets(typeof(EnumLoanTransfer));
            foreach (EnumObject oItem in oTempTrnasferTypes)
            {
                if (oItem.id == (int)EnumLoanTransfer.Regular_To_OverDue)
                {
                    oTrnasferTypes.Add(oItem);
                }
            }
            ViewBag.TrnasferTypes = oTrnasferTypes;
            ViewBag.ExpenditureHeads = ExpenditureHead.Gets((int)EnumExpenditureType.ExportBill_Encash, (int)Session[SessionInfo.currentUserID]); 
            ViewBag.BankAccounts = BankAccount.GetsByBankBranch(_oExportBill.BankBranchID_Bill, (int)Session[SessionInfo.currentUserID]); 
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            return View(_oExportBill);
        }
        [HttpPost]
        public JsonResult GetsPCLoan(LoanInstallment oLoanInstallment)
        {
            Loan oLoan = new Loan();
            try
            {
                if (oLoanInstallment.LoanInstallmentID > 0)
                {
                    oLoanInstallment = oLoanInstallment.Get(oLoanInstallment.LoanInstallmentID, (int)Session[SessionInfo.currentUserID]);
                    string sSQL = "SELECT * FROM View_LoanSettlement AS HH WHERE HH.LoanInstallmentID = " + oLoanInstallment.LoanInstallmentID.ToString() + " AND ISNULL(HH.BankAccountID,0) = 0 AND ISNULL(HH.ExpenseHeadID,0) > 0 ORDER BY HH.LoanSettlementID ASC";
                    oLoanInstallment.LoanchargeList = LoanSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    sSQL = "SELECT * FROM View_LoanSettlement AS HH WHERE HH.LoanInstallmentID = " + oLoanInstallment.LoanInstallmentID.ToString() + " AND ISNULL(HH.BankAccountID,0) > 0 AND ISNULL(HH.ExpenseHeadID,0) = 0 ORDER BY HH.LoanSettlementID ASC";
                    oLoanInstallment.PaymentList = LoanSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oLoan = new Loan();
                    oLoan = oLoan.Get(oLoanInstallment.LoanID, (int)Session[SessionInfo.currentUserID]);
                    oLoanInstallment = new LoanInstallment();
                    oLoanInstallment.LoanInstallmentID = 0;
                    oLoanInstallment.LoanID = oLoan.LoanID;
                    oLoanInstallment.InstallmentNo = "";
                    oLoanInstallment.InstallmentStartDate = oLoan.StlmtStartDate;
                    oLoanInstallment.InstallmentDate = oLoan.ApproxSettlement;
                    oLoanInstallment.LoanPrincipalAmount = oLoan.PrincipalAmount;
                    oLoanInstallment.PrincipalAmount = oLoan.LoanAmount;
                    oLoanInstallment.LoanTransferType = EnumLoanTransfer.None;
                    oLoanInstallment.LoanTransferTypeInt = 0;
                    oLoanInstallment.TransferDate = DateTime.MinValue;
                    oLoanInstallment.TransferDays = 0;
                    oLoanInstallment.TransferInterestRate = 0;
                    oLoanInstallment.TransferInterestAmount = 0;
                    oLoanInstallment.SettlementDate = oLoan.ApproxSettlement;
                    oLoanInstallment.InterestDays = (Convert.ToInt32((oLoan.ApproxSettlement - oLoan.StlmtStartDate).TotalDays));
                    oLoanInstallment.InterestRate = oLoan.InterestRate;
                    oLoanInstallment.InterestAmount = (((oLoan.LoanAmount * oLoan.InterestRate) / 100.00) / 360.00) * Convert.ToDouble(oLoanInstallment.InterestDays);
                    oLoanInstallment.LiborRate = 0;
                    oLoanInstallment.LiborInterestAmount = 0;
                    oLoanInstallment.TotalInterestAmount = oLoanInstallment.InterestAmount;
                    oLoanInstallment.ChargeAmount = 0;
                    oLoanInstallment.DiscountPaidAmount = 0;
                    oLoanInstallment.DiscountRcvAmount = 0;
                    oLoanInstallment.TotalPayableAmount = (oLoan.LoanAmount + oLoanInstallment.InterestAmount);
                    oLoanInstallment.PaidAmount = 0;
                    oLoanInstallment.PaidAmountBC = 0;
                    oLoanInstallment.PrincipalDeduct = 0;
                    oLoanInstallment.PrincipalBalance = oLoan.LoanAmount;
                    oLoanInstallment.Remarks = "";
                    oLoanInstallment.SettlementBy = 0;
                    oLoanInstallment.SettlementByName = "";
                    oLoanInstallment.FileNo = oLoan.FileNo;
                    oLoanInstallment.LoanNo = oLoan.LoanNo;
                    oLoanInstallment.LoanRefType = oLoan.LoanRefType;
                    oLoanInstallment.LoanCRate = oLoan.CRate;
                    oLoanInstallment.LoanRefID = oLoan.LoanRefID;
                    oLoanInstallment.LoanRefNo = oLoan.LoanRefNo;
                    oLoanInstallment.LoanType = oLoan.LoanType;
                    oLoanInstallment.ApproxSettlement = oLoan.ApproxSettlement;
                    oLoanInstallment.IssueDate = oLoan.IssueDate;
                    oLoanInstallment.BankAccNo = oLoan.BankAccNo;
                    oLoanInstallment.LoanCurencyID = oLoan.LoanCurencyID;
                    oLoanInstallment.LoanCurency = oLoan.CurrencySymbol;
                    oLoanInstallment.LoanchargeList = new List<LoanSettlement>();
                    oLoanInstallment.PaymentList = new List<LoanSettlement>();
                }
            }
            catch (Exception ex)
            {
                oLoanInstallment = new LoanInstallment();
                oLoanInstallment.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoanInstallment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsPCLoanByLoanNumberORLCNo(ExportBill oExportBill)
        {
            Loan oLoan = new Loan();
            List<Loan> oLoans = new List<Loan>();
            try
            {
                string sSQL = "SELECT * FROM View_Loan AS HH WHERE HH.LoanRefType = " + ((int)EnumLoanRefType.ExportLC).ToString();
                if (oExportBill.ExportLCNo == null || oExportBill.ExportLCNo == "")
                {
                    sSQL = sSQL + " AND HH.LoanRefID IN (" + oExportBill.ExportLCID + ")";
                }
                else
                {
                    sSQL = sSQL + " AND HH.BUID = " + oExportBill.BUID.ToString() + "  AND (ISNULL(HH.LoanRefNo,'')+ISNULL(HH.LoanNo,'')) LIKE '%" + oExportBill.ExportLCNo + "%'";
                }
                sSQL = sSQL + " ORDER BY HH.ApproxSettlement ASC";
                oLoans = Loan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLoan = new Loan();
                oLoan.ErrorMessage = ex.Message;
                oLoans = new List<Loan>();
                oLoans.Add(oLoan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewExportBill_History(int nId, double ts)
        {
            List<ExportBillHistory> oExportBillHistorys = new List<ExportBillHistory>();
            _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
            oExportBillHistorys = ExportBillHistory.Gets(nId, (int)Session[SessionInfo.currentUserID]);

            bool bFlag = false;
            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            foreach (ExportBillHistory oItem in oExportBillHistorys)
            {
                dEndDate = oItem.DateTime;
                if (bFlag == true)
                {
                    TimeSpan n = dEndDate - dStartDate;
                    if (n.Days >= 1)
                    {
                        oItem.ErrorMsg = n.Days + " days";
                    }
                    else if (n.Hours > 0)
                    {
                        oItem.ErrorMsg =oItem.ErrorMsg +" "+ n.Hours.ToString() + " hrs.";
                    }
                    else
                    {
                        oItem.ErrorMsg = "";
                    }

                }
                else
                {
                    oItem.ErrorMsg = "";
                }
                dStartDate = oItem.DateTime;
                bFlag = true;
            }
            ViewBag.ExportBillHistorys= oExportBillHistorys;
            //ViewBag.BUID = buid;
            return View(_oExportBill);
        }        
        public ActionResult ViewExportBill_LDBP(int nId, int buid)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            Export_LDBPDetail oExport_LDBPDetail = new Export_LDBPDetail();
            Company oCompany = new Company();
            if (nId > 0)
            {
                _oExportBill = _oExportBill.Get(nId, (int)Session[SessionInfo.currentUserID]);
                oExport_LDBPDetail = oExport_LDBPDetail.GetBy(nId, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.ReqForDiscounted;
                oExportBillHistory = oExportBillHistory.Getby(nId, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;
            }
            ViewBag.BUID = _oExportBill.BUID;
            ViewBag.BankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            return View(oExport_LDBPDetail);
        }
        [HttpPost]
        public JsonResult GetExportBill(ExportBill oExportBill)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            _oExportBill = new ExportBill();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            try
            {
                if (oExportBill.ExportBillID <= 0) { throw new Exception("Please select a valid contractor."); }

                _oExportBill = _oExportBill.Get(oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);
                //_oExportBill.ExportBillDetails = ExportBillDetail.Gets(oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);
               oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(oExportBill.BUID, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.BOEinHand;
                oExportBillHistory = oExportBillHistory.Getby(oExportBill.ExportBillID, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;
                _oExportBill.ExportBillDetails = ExportBillDetail.Gets(oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);
                if (oMeasurementUnitCon.MeasurementUnitConID > 0 && oMeasurementUnitCon.Value>0)
                {
                    _oExportBill.ExportBillDetails.ForEach(o => o.QtyTwo = Math.Round((o.Qty *oMeasurementUnitCon.Value),6));
                    _oExportBill.ExportBillDetails.ForEach(o => o.UnitPriceTwo = Math.Round((o.UnitPrice/oMeasurementUnitCon.Value),6));
                }
                else
                {
                    oMeasurementUnitCon.Value = 1;
                    if (_oExportBill.ExportBillDetails.Count > 0)
                    {
                        oMeasurementUnitCon.ToMUnit = _oExportBill.ExportBillDetails[0].MUName;
                        oMeasurementUnitCon.FromMUnit = _oExportBill.ExportBillDetails[0].MUName;
                    }
                }

            }
            catch (Exception ex)
            {
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(ExportBill oExportBill)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            _oExportBill = new ExportBill();
            try
            {
                if (oExportBill.ExportBillID <= 0) { throw new Exception("Please select a valid contractor."); }
                _oExportBill = _oExportBill.Get(oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);
                oExportBillHistory.State = EnumLCBillEvent.BOEInCustomerHand;
                oExportBillHistory = oExportBillHistory.Getby(oExportBill.ExportBillID, (int)oExportBillHistory.State, (int)Session[SessionInfo.currentUserID]);
                _oExportBill.NoteCarry = oExportBillHistory.Note;

                //Ratin
                if (_oExportBill.ExportLCID > 0)
                {
                    ExportLC oExportLC = new ExportLC();
                    _oExportBill.ExportLC = oExportLC.Get(_oExportBill.ExportLCID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #endregion

        #region Save bill operation
        [HttpPost]
        public JsonResult SaveExportBill(ExportBill oExportBill)
        {
            oExportBill.RemoveNulls();
            try
            {
                _oExportBill = oExportBill.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save_ExportBillDetail(ExportBillDetail oExportBillDetail)
        {
            oExportBillDetail.RemoveNulls();
            try
            {
                oExportBillDetail = oExportBillDetail.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportBillDetail = new ExportBillDetail();
                oExportBillDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportBillDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_ExportBillDetailMultipleBills(ExportBillDetail oExportBillDetail)
        {
            try
            {
                oExportBillDetail = oExportBillDetail.SaveMultipleBills((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportBillDetail = new ExportBillDetail();
                oExportBillDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportBillDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveBOEinCustomerHand(ExportBill oExportBill)
        {
            try
            {
                oExportBill.RemoveNulls();
                _oExportBill = new ExportBill();
                oExportBill.State = EnumLCBillEvent.BOEInCustomerHand;
                _oExportBill = oExportBill.Save_SendToBuyer((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveExportBillReceiveFromParty(ExportBill oExportBill)
        {
            try
            {
                oExportBill.RemoveNulls();
                _oExportBill = new ExportBill();
                oExportBill.State = EnumLCBillEvent.BuyerAcceptedBill;
                _oExportBill = oExportBill.SaveHistory((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveExportBillSendToBank(ExportBill oExportBill)
        {
            try
            {
                _oExportBill = new ExportBill();
                oExportBill.State = EnumLCBillEvent.NegoTransit;
                _oExportBill = oExportBill.SaveHistory((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveExportBillReceiveFromBank(ExportBill oExportBill)
        {
            try
            {
                oExportBill.RemoveNulls();
                _oExportBill = new ExportBill();
                oExportBill.State = EnumLCBillEvent.NegotiatedBill;
                _oExportBill = oExportBill.SaveHistory((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveMaturityReceived(ExportBill oExportBill)
        {
            try
            {
                oExportBill.RemoveNulls();
                _oExportBill = new ExportBill();
                oExportBill.State = EnumLCBillEvent.BankAcceptedBill;
                _oExportBill = oExportBill.SaveMaturityReceived((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSAN(ExportBill oExportBill)
        {
            try
            {
                oExportBill.RemoveNulls();
                _oExportBill = new ExportBill();
                _oExportBill = oExportBill.SaveSAN((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveDocDate(ExportBill oExportBill)
        {
            try
            {
                oExportBill.RemoveNulls();
                _oExportBill = new ExportBill();
                _oExportBill = oExportBill.SaveDocDate((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveBBankFDDReceived(ExportBill oExportBill)
        {
            try
            {
                oExportBill.RemoveNulls();
                _oExportBill = new ExportBill();
                oExportBill.State = EnumLCBillEvent.FDDInHand;
                _oExportBill = oExportBill.SaveHistory((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveBillRealized(ExportBill oExportBill)
        {
            _oExportBill = new ExportBill();
            try
            {
                oExportBill.RemoveNulls();

                oExportBill.State = EnumLCBillEvent.BillRealized;
                _oExportBill = oExportBill.Save_BillRealized((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveEncashmentRecived(ExportBill oExportBill)
        {
            _oExportBill = new ExportBill();
            try
            {
                oExportBill.RemoveNulls();
                oExportBill.State = EnumLCBillEvent.Encashment;
                _oExportBill = oExportBill.Save_Encashment((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_UpdateStatus(ExportBill oExportBill)
        {
            oExportBill.RemoveNulls();
            try
            {

                _oExportBill = oExportBill;
                _oExportBill = _oExportBill.Save_UpdateStatus((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ExportBill oExportBill)
        {
            string sFeedBackMessage = "";
            try
            {
                _oExportBill = new ExportBill();

                sFeedBackMessage = oExportBill.Delete((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult DeleteExportBillDetail(ExportBillDetail oExportBillDetail)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportBillDetail.Delete((int)Session[SessionInfo.currentUserID]);

                //if (sFeedBackMessage == Global.DeleteMessage)
                //{
                //    if (oExportBillDetail.ExportBill.ExportBillID > 0)
                //    {
                //        ExportBill oEB = new ExportBill();
                //        oEB = oExportBillDetail.ExportBill;
                //        oEB = oEB.Save((int)Session[SessionInfo.currentUserID]);
                //    }
                //}
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
        public JsonResult Delete_EBEncashment(ExportBillEncashment oExportBillEncashment)
        {
            string sFeedBackMessage = "";
            try
            {

                sFeedBackMessage = oExportBillEncashment.Delete((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Delete_ExportBillRealized(ExportBillRealized oExportBillRealized)
        {
            string sFeedBackMessage = "";
            try
            {

                sFeedBackMessage = oExportBillRealized.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Gets PI Product
        [HttpPost]
        public JsonResult GetsExportBillDetails(ExportBill oExportBill)
        {
             List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            ExportBillDetail oExportBillDetail = new ExportBillDetail();
            List<ExportPILCMappingDetail> oExportPILCMappingDetails = new List<ExportPILCMappingDetail>();
            List<ExportPILCMappingDetail> oExportPILCMappingDetails_Temp = new List<ExportPILCMappingDetail>();

            if (oExportBill.ExportLCID > 0)
            {
                oExportPILCMappingDetails = ExportPILCMappingDetail.GetsBy(oExportBill.ExportLCID, (int)Session[SessionInfo.currentUserID]);

                if (!String.IsNullOrEmpty(oExportBill.Params))
                {
                    oExportPILCMappingDetails_Temp = ExportPILCMappingDetail.Gets("SELECT * FROM View_ExportPILCMappingDetail WHERE ExportPILCMappingID in (Select ExportPILCMapping.ExportPILCMappingID from ExportPILCMapping where Activity=1 and ExportLCID=" + oExportBill.ExportLCID + ") and ExportPIDetailID in (" + oExportBill.Params + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oExportPILCMappingDetails_Temp.Any() && oExportPILCMappingDetails_Temp.FirstOrDefault().ExportPIDetailID > 0)
                    {
                        oExportPILCMappingDetails.ForEach(x => { x.Qty = x.Qty - oExportPILCMappingDetails_Temp.Where(p => p.ExportPIDetailID == x.ExportPIDetailID).Sum(o => o.Qty); });
                    }
                }
            }

            foreach (ExportPILCMappingDetail oItem in oExportPILCMappingDetails)
            {
                oExportBillDetail = new ExportBillDetail();
                oExportBillDetail.ProductCode = oItem.ProductCode;
                oExportBillDetail.ProductName = oItem.ProductName;
                oExportBillDetail.ProductID = oItem.ProductID;
                oExportBillDetail.MUnitID = oItem.MUnitID;
                oExportBillDetail.MUName = oItem.MUName;
                oExportBillDetail.RateUnit = oItem.RateUnit;
                oExportBillDetail.Qty = oItem.Qty - oItem.EBillQty;
                oExportBillDetail.QtyTwo = 0;
                oExportBillDetail.UnitPrice = oItem.UnitPrice;
                oExportBillDetail.ExportPIDetailID = oItem.ExportPIDetailID;
                oExportBillDetail.IsDeduct = oItem.IsDeduct;
                oExportBillDetail.PINo = oItem.PINo;
                oExportBillDetail.ReviseNo = oItem.ReviseNo;
                oExportBillDetail.Construction = oItem.Construction;
                oExportBillDetail.FabricWidth = oItem.FabricWidth;
                oExportBillDetail.FabricNo = oItem.FabricNo;
                oExportBillDetail.ColorInfo = oItem.ColorInfo;
                oExportBillDetail.FabricWeaveName = oItem.FabricWeaveName;
                oExportBillDetail.FinishTypeName = oItem.FinishTypeName;
                oExportBillDetail.ProcessTypeName = oItem.ProcessTypeName;
                oExportBillDetail.StyleNo = oItem.StyleNo;
                oExportBillDetail.Currency = oExportBill.Currency;
                oExportBillDetail.ExportBillID = oExportBill.ExportBillID;
                //oExportBillDetail.PIYear = oItem.PIYear;
                //oExportBillDetail.PICode = oItem.PICode;
                oExportBillDetail.Qty = Math.Round(oExportBillDetail.Qty, 6);
                if (oExportBillDetail.Qty > 0)
                {
                    //oExportBillDetail.QtyTwo = oExportBillDetail.Qty/1.0936132983;
                    oExportBillDetails.Add(oExportBillDetail);
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oExportBillDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExportDocs(ExportBill oExportBill)
        {
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            List<ExportDocSetup> oExportDocSetups_Bill = new List<ExportDocSetup>();
            string sSQL = "SELECT * FROM View_ExportDocSetup WHERE ExportDocSetupID IN (SELECT ExportDocID FROM ExportDocForwarding WHERE ReferenceID =" + oExportBill.ExportLCID + "  AND RefType =" + (int)EnumMasterLCType.ExportLC + " ) and ExportDocSetupID not in (Select ExportDocSetupID from ExportBillDoc where ExportBillID=" + oExportBill.ExportBillID + ") ORDER BY Sequence ASC";
            //string sSQL = "SELECT * FROM View_ExportDocSetup WHERE ExportDocSetupID IN (SELECT ExportDocID FROM ExportDocForwarding WHERE ReferenceID = " + oExportBill.ExportLCID + " AND RefType =" + (int)EnumMasterLCType.ExportLC + " )  ORDER BY Sequence ASC";
            if (oExportBill.ExportLCID > 0)
            {
                oExportDocSetups = ExportDocSetup.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            if (oExportBill.ExportBillID > 0)
            {
                oExportDocSetups_Bill = ExportDocSetup.GetsBy(oExportBill.ExportBillID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            foreach (ExportDocSetup oItemDOD in oExportDocSetups)
            {
                oItemDOD.ExportBillID = oExportBill.ExportBillID;
                oItemDOD.ContractorID = oExportBill.ApplicantID;
                oExportDocSetups_Bill.Add(oItemDOD);
            }
            oExportDocSetups_Bill = oExportDocSetups_Bill.OrderBy(x => x.Sequence).ToList();
          
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oExportDocSetups_Bill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //
        #endregion

        #region View Advance Search

       

        #endregion

        [HttpPost]
        public JsonResult GetsDocumentType()
        {
            List<EnumObject> oDocumentTypes = new List<EnumObject>();
            oDocumentTypes = EnumObject.jGets(typeof(EnumDocumentType));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDocumentTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsExportDocSetup(ExportDocSetup oExportDocSetup)
        {
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            oExportDocSetups = ExportDocSetup.Gets(true, oExportDocSetup.BUID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportDocSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintExportDoc(int id, int nDocType, int nPrintType, int nUnitType, int nPageSize, bool bIsApplicant)
        {
            /*
             * When nUnitType == 1 then "In KG"
             * When nUnitType == 2 then "In LBS"
             * When nPageSize == 1 then "A4"
             * When nPageSize == 2 then "Legal"
             */

            _oExportBill = new ExportBill();
            _oExportBill = _oExportBill.Get(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            Company oSubCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = this.GetCompanyTitle(oCompany);

            ExportDocSetup oExportDocSetup = new ExportDocSetup();
            oExportDocSetup = oExportDocSetup.GetBy(nDocType,_oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportBill.BUID, (int)Session[SessionInfo.currentUserID]);

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(_oExportBill.BUID, (int)Session[SessionInfo.currentUserID]);

           
            ExportCommercialDoc oExportCommercialDoc = new ExportCommercialDoc();//new ExportCommercialDoc(oExportDocSetup, _oExportBill);
            oExportCommercialDoc = oExportCommercialDoc.Get(id, (int)Session[SessionInfo.currentUserID]);


            #region Mapping from Setup
            oExportCommercialDoc.ExportBillDocID = oExportDocSetup.ExportBillDocID;
            oExportCommercialDoc.DocumentType = oExportDocSetup.DocumentType;
            oExportCommercialDoc.IsPrintHeader = oExportDocSetup.IsPrintHeader;
            oExportCommercialDoc.DocName = oExportDocSetup.DocName;
            oExportCommercialDoc.BillNo = oExportDocSetup.BillNo;
            oExportCommercialDoc.DocHeader = oExportDocSetup.DocHeader;
            oExportCommercialDoc.Beneficiary = oExportDocSetup.Beneficiary;
            oExportCommercialDoc.NoAndDateOfDoc = oExportDocSetup.NoAndDateOfDoc;
            oExportCommercialDoc.ProformaInvoiceNoAndDate = oExportDocSetup.ProformaInvoiceNoAndDate;
            oExportCommercialDoc.AccountOf = oExportDocSetup.AccountOf;
            oExportCommercialDoc.DocumentaryCreditNoDate = oExportDocSetup.DocumentaryCreditNoDate;
            oExportCommercialDoc.AgainstExportLC = oExportDocSetup.AgainstExportLC;
            oExportCommercialDoc.PortofLoading = oExportDocSetup.PortofLoading;
            oExportCommercialDoc.FinalDestination = oExportDocSetup.FinalDestination;
            oExportCommercialDoc.IssuingBank = oExportDocSetup.IssuingBank;
            oExportCommercialDoc.NegotiatingBank = oExportDocSetup.NegotiatingBank;
            oExportCommercialDoc.ToTheOrderOf = oExportDocSetup.ToTheOrderOf;
            
            oExportCommercialDoc.CountryofOrigin = oExportDocSetup.CountryofOrigin;
            oExportCommercialDoc.TermsofPayment = oExportDocSetup.TermsofPayment;
            oExportCommercialDoc.AmountInWord = oExportDocSetup.AmountInWord;
            oExportCommercialDoc.Wecertifythat = oExportDocSetup.Wecertifythat;
            oExportCommercialDoc.Certification = oExportDocSetup.Certification;
            oExportCommercialDoc.ClauseOne = oExportDocSetup.ClauseOne;
            oExportCommercialDoc.ClauseTwo = oExportDocSetup.ClauseTwo;
            oExportCommercialDoc.ClauseThree = oExportDocSetup.ClauseThree;
            oExportCommercialDoc.ClauseFour = oExportDocSetup.ClauseFour;
            oExportCommercialDoc.Carrier = oExportDocSetup.Carrier;
            oExportCommercialDoc.Account = oExportDocSetup.Account;
            oExportCommercialDoc.NotifyParty = oExportDocSetup.NotifyParty;
            //oExportCommercialDoc.CompanyName =oExportDocSetup.CompanyName;
            oExportCommercialDoc.DeliveryTo = oExportDocSetup.DeliveryTo;
            oExportCommercialDoc.SellingOnAbout = oExportDocSetup.SellingOnAbout;
            oExportCommercialDoc.IsPrintInvoiceDate = oExportDocSetup.IsPrintInvoiceDate;

            oExportCommercialDoc.AuthorisedSignature = oExportDocSetup.AuthorisedSignature;
            oExportCommercialDoc.ReceiverSignature = oExportDocSetup.ReceiverSignature;
            oExportCommercialDoc.For = oExportDocSetup.For;

            oExportCommercialDoc.FinalDestinationName = oExportDocSetup.FinalDestinationName;
            oExportCommercialDoc.PortofLoadingName = oExportDocSetup.PortofLoadingName;
            oExportCommercialDoc.CountryofOriginName = oExportDocSetup.CountryofOriginName;
            oExportCommercialDoc.CTPApplicant = oExportDocSetup.CTPApplicant;
            oExportCommercialDoc.GRPNoDate = oExportDocSetup.GRPNoDate;
            oExportCommercialDoc.GrossWeightPTage = oExportDocSetup.GrossWeightPTage;
            oExportCommercialDoc.FontSize_Normal = oExportDocSetup.FontSize_Normal;
            //oExportCommercialDoc.WeightPBag = oExportDocSetup.WeightPBag;
            
            if (oExportDocSetup.ExportBillDocID > 0)
            {
                oExportCommercialDoc.TruckNo = oExportDocSetup.TruckNo;
                oExportCommercialDoc.DriverName = oExportDocSetup.DriverName;
                oExportCommercialDoc.DriverName_Print = oExportDocSetup.Driver_Print;
                oExportCommercialDoc.TRNo = oExportDocSetup.TRNo;
                oExportCommercialDoc.TRDate = oExportDocSetup.TRDateSt;
                oExportCommercialDoc.NotifyBy = oExportDocSetup.NotifyBy;
                oExportCommercialDoc.BagCount = oExportDocSetup.BagCount;
                oExportCommercialDoc.WeightPerBag = oExportDocSetup.WeightPBag;
                oExportCommercialDoc.CTPApplicant = oExportDocSetup.CTPApplicant;
               
            
            }
            oExportCommercialDoc.GoodsDesViewType = oExportDocSetup.GoodsDesViewType;
            oExportCommercialDoc.NetWeight = oExportDocSetup.NetWeightName;
            oExportCommercialDoc.GrossWeight = oExportDocSetup.GrossWeightName;
            oExportCommercialDoc.Bag_Name = oExportDocSetup.Bag_Name;
            oExportCommercialDoc.IsPrintUnitPrice = oExportDocSetup.IsPrintUnitPrice;
            oExportCommercialDoc.IsPrintValue = oExportDocSetup.IsPrintValue;
            oExportCommercialDoc.OrderOfBankType = oExportDocSetup.OrderOfBankType;

            oExportCommercialDoc.PrintOn = oExportDocSetup.PrintOn;
            oExportCommercialDoc.ProductPrintType = oExportDocSetup.ProductPrintType;
            oExportCommercialDoc.ChallanNo = oExportDocSetup.ChallanNo;
            oExportCommercialDoc.TextWithGoodsRow = oExportDocSetup.TextWithGoodsRow;
            oExportCommercialDoc.TextWithGoodsCol = oExportDocSetup.TextWithGoodsCol;
            if (String.IsNullOrEmpty(oExportDocSetup.CTPApplicant)) { oExportCommercialDoc.CTPApplicant = ""; }
            if (String.IsNullOrEmpty(oExportDocSetup.GRPNoDate)) { oExportCommercialDoc.GRPNoDate = ""; }
        
            

            //if (String.IsNullOrEmpty(oExportDocSetup.Certification))
            //{ oExportCommercialDoc.Certification_Entry = ""; }

         
           
            oExportCommercialDoc.ASPERPI = oExportDocSetup.ASPERPI;

            oExportCommercialDoc.GarmentsQty_Head = oExportDocSetup.GarmentsQty;
            if (String.IsNullOrEmpty(oExportDocSetup.GarmentsQty)) { oExportCommercialDoc.GarmentsQty = ""; }

            oExportCommercialDoc.Remark_Head = oExportDocSetup.Remark;
            if (String.IsNullOrEmpty(oExportDocSetup.Remark)) { oExportCommercialDoc.Remark = ""; }

            oExportCommercialDoc.SpecialNote_Head = oExportDocSetup.SpecialNote;
            if (String.IsNullOrEmpty(oExportDocSetup.SpecialNote)) { oExportCommercialDoc.SpecialNote = ""; }

            oExportCommercialDoc.AreaCode_Head = oExportDocSetup.AreaCode;
            if (String.IsNullOrEmpty(oExportDocSetup.AreaCode)) { oExportCommercialDoc.AreaCode = ""; }

            oExportCommercialDoc.HSCode_Head = oExportDocSetup.HSCode;
            if (String.IsNullOrEmpty(oExportDocSetup.HSCode)) { oExportCommercialDoc.HSCode = ""; }

            oExportCommercialDoc.IRC_Head = oExportDocSetup.IRC;
            if (String.IsNullOrEmpty(oExportDocSetup.IRC)) { oExportCommercialDoc.IRC = ""; }

            oExportCommercialDoc.Carrier = oExportDocSetup.Carrier;
            oExportCommercialDoc.CarrierName = oExportDocSetup.CarrierName;
            oExportCommercialDoc.DescriptionOfGoods = oExportDocSetup.DescriptionOfGoods;
            if (String.IsNullOrEmpty(oExportCommercialDoc.DescriptionOfGoods)) { oExportCommercialDoc.DescriptionOfGoods = "Description Of Goods"; }
               oExportCommercialDoc.MarkSAndNos= oExportDocSetup.MarkSAndNos;// = "MARKS & NOS";
               oExportCommercialDoc.NetWeight = oExportDocSetup.NetWeightName;
               oExportCommercialDoc.GrossWeight = oExportDocSetup.GrossWeightName;
               oExportCommercialDoc.NoOfBag = oExportDocSetup.NoOfBag;

            if (String.IsNullOrEmpty(oExportDocSetup.Carrier)) { oExportCommercialDoc.CarrierName = ""; }
            if (String.IsNullOrEmpty(oExportCommercialDoc.CarrierName))
            {
                if (_oExportBill.ProductNature == EnumProductNature.Dyeing)
                {
                    oExportCommercialDoc.CarrierName = "By Truck";
                }
                else
                {
                    oExportCommercialDoc.CarrierName = "Our Own Vehicle";
                }
            }

            oExportCommercialDoc.TruckNo_Print = oExportDocSetup.TruckNo_Print;
            if (String.IsNullOrEmpty(oExportCommercialDoc.TruckNo_Print)) { oExportCommercialDoc.TruckNo_Print = ""; }

            oExportCommercialDoc.To = oExportDocSetup.TO;
            if (String.IsNullOrEmpty(oExportCommercialDoc.To)) { oExportCommercialDoc.To = ""; }

            oExportCommercialDoc.ShippingMark = oExportDocSetup.ShippingMark;
            if (!String.IsNullOrEmpty(oExportCommercialDoc.ShippingMark))
            {
                Contractor oContractor = new Contractor();
                oContractor = oContractor.Get(_oExportBill.ApplicantID, (int)Session[SessionInfo.currentUserID]);
                oExportCommercialDoc.ShippingMarkName = (oBusinessUnit.ShortName+"/" + oContractor.ShortName + "/REF").ToUpper();
            }

            oExportCommercialDoc.ReceiverCluse = oExportDocSetup.ReceiverCluse;
            if (String.IsNullOrEmpty(oExportCommercialDoc.ReceiverCluse)) { oExportCommercialDoc.ReceiverCluse = ""; }
            oExportCommercialDoc.ForCaptionInDubleLine = oExportDocSetup.ForCaptionInDubleLine;

            oExportCommercialDoc.NoOfPackages = _oExportBill.NoOfPackages;
            oExportCommercialDoc.PlasticNetWeight = _oExportBill.NetWeight;
            oExportCommercialDoc.PlasticGrossWeight = _oExportBill.GrossWeight;

            //oExportCommercialDoc.Var_ReqNo_Head = "VAT Reg. No.";
            //if (!oExportDocSetup.IsVat) { oExportCommercialDoc.Vat_ReqNo = ""; }
            //oExportCommercialDoc.TIN_Head = "TIN.";
            //if (!oExportDocSetup.IsRegistration) { oExportCommercialDoc.TIN = ""; }
            if (!oExportDocSetup.IsPrintFrieghtPrepaid) { oExportCommercialDoc.FrightPrepaid = ""; }

            if (oExportDocSetup.IsPrintOriginal == true && oExportCommercialDoc.IsPrintOriginal == true)
            {
                oExportCommercialDoc.Orginal = "Orginal";
            }
            else
            {
                oExportCommercialDoc.Orginal = "";
            }

            oExportCommercialDoc.Term_Head = "Trade Term ";
            if (oExportDocSetup.IsPrintTerm == true && oExportCommercialDoc.IsTerm == true)
            {
                oExportCommercialDoc.Term = oExportCommercialDoc.Term;
            }
            else
            {
                oExportCommercialDoc.Term = "";
            }
            if (oExportDocSetup.IsPrintDeliveryBy == true && oExportCommercialDoc.IsDeliveryBy == true)
            {
                oExportCommercialDoc.DeliveryBy = oExportCommercialDoc.DeliveryBy;
            }
            else
            {
                oExportCommercialDoc.DeliveryBy = "";
            }

            if (oExportDocSetup.IsPrintGrossNetWeight == true && oExportCommercialDoc.IsPrintGrossNetWeight == true)
            {
                oExportCommercialDoc.IsPrintGrossNetWeight = true;
            }
            else
            {
                oExportCommercialDoc.IsPrintGrossNetWeight = false;
            }



            #endregion
            List<ExportClaimSettle> oExportClaimSettles = new List<ExportClaimSettle>();

            oExportClaimSettles = ExportClaimSettle.GetsByBillID(_oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);

            List<ExportPILCMapping> oExportPILCMappings = new List<ExportPILCMapping>();
            oExportPILCMappings = ExportPILCMapping.GetsByEBillID(_oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);
            oExportCommercialDoc.PINos = "";
            oExportCommercialDoc.ExportClaimSettles = oExportClaimSettles;
          
                foreach (ExportPILCMapping oItem in oExportPILCMappings)
                {
                    oExportCommercialDoc.PINos = oExportCommercialDoc.PINos + "" + oItem.PINo_Full + " DT : " + oItem.IssueDateST + ",";
                    oExportCommercialDoc.VersionNo = oItem.VersionNo;
                    oExportCommercialDoc.AmendmentDate = oItem.DateST;
                }
            
            //else
            //{
            //    foreach (ExportPILCMapping oItem in oExportPILCMappings)
            //    {
            //        oExportCommercialDoc.PINos = oExportCommercialDoc.PINos + "" + oItem.PINo_Full + " DT : " + oItem.IssueDateST + "\n";
            //        oExportCommercialDoc.VersionNo = oItem.VersionNo;
            //        oExportCommercialDoc.AmendmentDate = oItem.DateST;
            //    }
            //}
            if (oExportCommercialDoc.PINos.Length > 0)
            {
                oExportCommercialDoc.PINos = oExportCommercialDoc.PINos.Remove(oExportCommercialDoc.PINos.Length - 1, 1);
            }

            if (oExportCommercialDoc.VersionNo > 0)
            {
                oExportCommercialDoc.AmendmentNonDate = "";
                if (oExportDocSetup.IsShowAmendmentNo == true)
                {
                    oExportCommercialDoc.AmendmentNonDate = ", A. No: " + oExportCommercialDoc.VersionNo + "(" + oExportCommercialDoc.AmendmentDate + ")";
                }

            }
            else
            {
                oExportCommercialDoc.AmendmentNonDate = "";
            }

            #region Master LC and Export LC
            // Gets PIs
            List<MasterLCMapping> oMasterLCMappings = new List<MasterLCMapping>();
            List<MasterLCMapping> oMasterLCMappings_Master = new List<MasterLCMapping>();
            List<MasterLCMapping> oMasterLCMappings_Export = new List<MasterLCMapping>();

            oExportCommercialDoc.MasterLCNo = "";
            oMasterLCMappings = MasterLCMapping.Gets(_oExportBill.ExportLCID, (int)Session[SessionInfo.currentUserID]);

            oMasterLCMappings_Master = oMasterLCMappings.Where(o => o.MasterLCTypeInInt == (int)EnumMasterLCType.MasterLC).ToList();
            oMasterLCMappings_Export = oMasterLCMappings.Where(o => o.MasterLCTypeInInt == (int)EnumMasterLCType.ExportLC).ToList();
            string sMasterLC = "";
            string sExportLC = "";
            string sTemp = "";
            #region Master LC
            foreach (MasterLCMapping oItem in oMasterLCMappings_Master)
            {
                if (string.IsNullOrEmpty(oItem.MasterLCDateSt))
                {
                    sTemp = sTemp + " " + oItem.MasterLCNo + ",";
                }
                else
                {
                    sTemp = sTemp + " " + oItem.MasterLCNo + " DT:" + oItem.MasterLCDateSt + ",";
                }
            }
            if (sTemp.Length > 0)
            {
                sTemp = sTemp.Remove(sTemp.Length - 1, 1);
            }
            sMasterLC = sMasterLC + "" + sTemp;
            #endregion
            #region Export LC
            sTemp = "";
            foreach (MasterLCMapping oItem in oMasterLCMappings_Export)
            {
                if (string.IsNullOrEmpty(oItem.MasterLCDateSt))
                {
                    sTemp = sTemp + " " + oItem.MasterLCNo + ",";
                }
                else
                {
                    sTemp = sTemp+" "+ oItem.MasterLCNo + " DT:" + oItem.MasterLCDateSt + ",";
                }
            }
            if (sTemp.Length > 0)
            {
                sTemp = sTemp.Remove(sTemp.Length - 1, 1);
            }
            sExportLC = sExportLC + "" + sTemp;
            #endregion
            if (sMasterLC.Length > 0)
            {
                oExportCommercialDoc.AllMasterLCNo = sMasterLC;
                if (sExportLC.Length > 0)
                {
                    oExportCommercialDoc.AllMasterLCNo = sMasterLC + ", " + sExportLC;
                }
            }
            else
            {
                oExportCommercialDoc.AllMasterLCNo = sExportLC;
            }


            #endregion
            //oExportCommercialDoc.PortofLoadingName = "Beneficiary Factoy"; ///Add In setup Applicant Factory
            //oExportCommercialDoc.PortofLoadingPoint = "Beneficiary Factoy";
            oExportCommercialDoc.PortofLoadingAddress = "";
            oExportCommercialDoc.ExportBillDetails = ExportBillDetail.Gets(_oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);
            #region Set Unit Type
            string sMUName = oMeasurementUnitCon.FromMUnit;
            /* By Default nUnitType = 2 means LBS*/
            #endregion

            int nMUnitID = 0;
            string sCurrency = "";
            foreach (ExportBillDetail oItem in oExportCommercialDoc.ExportBillDetails)
            {
                sMUName = oItem.MUName;
                nMUnitID = oItem.MUnitID;
                sCurrency = oItem.Currency;
                if (oMeasurementUnitCon.ToMUnitID <= 0)
                {
                    oMeasurementUnitCon.ToMUnit = sMUName;
                    oMeasurementUnitCon.ToMUnitID = nMUnitID;
                }
                break;
            }
             //sMUName = oMeasurementUnitCon.FromMUnit;
             if (nUnitType == 1)
             {
                 oExportCommercialDoc.ExportBillDetails.ForEach(o => o.Qty = (o.Qty*oMeasurementUnitCon.Value));
                 oExportCommercialDoc.ExportBillDetails.ForEach(o => o.UnitPrice = (o.UnitPrice/oMeasurementUnitCon.Value));
                 sMUName = oMeasurementUnitCon.ToMUnit;
             }
            oExportCommercialDoc.MUnitCon = oMeasurementUnitCon;
            oExportCommercialDoc.SL = "SL";
            //oExportCommercialDoc.DescriptionOfGoods = "Description Of Goods";
            //oExportCommercialDoc.NoOfBag = "No of Bag";
            oExportCommercialDoc.WtperBag = "Wt/Bag";
            oExportCommercialDoc.Header_Construction = "CONSTRUCTION";
            oExportCommercialDoc.Header_FabricsType = "Fabrics Type";
            oExportCommercialDoc.Header_Color = "COLOR";
            oExportCommercialDoc.Header_Style = "STYLE/BUYER REF";
            if (oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing || oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving)
            {

                if (oExportCommercialDoc.WeightPerBag <= 0)
                {
                    oExportCommercialDoc.WeightPerBag = 100;
                }
                if (oExportCommercialDoc.BagWeight <= 0)
                {
                    oExportCommercialDoc.BagWeight = 0.25;
                }
                if(oExportCommercialDoc.BagWeight>0)
                {
                    oExportCommercialDoc.BagWeight = oExportCommercialDoc.BagWeight/1000;
                }
            }



            oExportCommercialDoc.ValueDes = oExportDocSetup.ValueName;
            oExportCommercialDoc.QtyInKg = oExportDocSetup.QtyInOne;
            oExportCommercialDoc.QtyInLBS = oExportDocSetup.QtyInOne;
            oExportCommercialDoc.UnitPriceDes = oExportDocSetup.UPName;
            if (string.IsNullOrEmpty(oExportCommercialDoc.QtyInKg ))
            {
                oExportCommercialDoc.QtyInKg = "Qty";
            }
            else
            {
                oExportCommercialDoc.QtyInKg = oExportCommercialDoc.QtyInKg+".(" + sMUName + ")";
            }
            if (string.IsNullOrEmpty(oExportCommercialDoc.QtyInLBS))
            {
                oExportCommercialDoc.QtyInLBS = "Qty";
            }
            else
            {
                oExportCommercialDoc.QtyInLBS = oExportCommercialDoc.QtyInLBS + ".(" + sMUName + ")";
            }
            if (string.IsNullOrEmpty(oExportCommercialDoc.ValueDes))
            {
                oExportCommercialDoc.ValueDes = "Amount";
            }
            else
            {
                oExportCommercialDoc.ValueDes = oExportCommercialDoc.ValueDes + "\n(" + oExportCommercialDoc.CurrencyName + ")";
            }
            if (string.IsNullOrEmpty(oExportCommercialDoc.UnitPriceDes))
            {
                oExportCommercialDoc.UnitPriceDes = "Amount";
            }
            else
            {
                oExportCommercialDoc.UnitPriceDes = oExportCommercialDoc.UnitPriceDes + "\n(" + sCurrency + "/" + sMUName + ")";
            }
     
          
            oExportCommercialDoc.MUName = sMUName;


            oExportCommercialDoc.DocumentsDes = "Documents for which are attached herewith."; // Bill of Exchange
            oExportCommercialDoc.BagCount = 12;
            oExportCommercialDoc.ProductNature = _oExportBill.ProductNature;



            UserImage oUserImage = new UserImage();
            oUserImage = oUserImage.Get(1, (int)Session[SessionInfo.currentUserID]);
            oExportCommercialDoc.BOEImage = GetBoEImage(oUserImage);

            if (bIsApplicant)
            {
                if (!string.IsNullOrEmpty(oExportCommercialDoc.To))
                {
                    oExportCommercialDoc.To = "For " + oExportCommercialDoc.ApplicantName;
                }
                if (!string.IsNullOrEmpty(oExportCommercialDoc.For))
                {
                    oExportCommercialDoc.For = "For " + oExportCommercialDoc.ApplicantName;
                }
            }

            if (oExportCommercialDoc.To.Contains("@APPLICANT"))
            {
                oExportCommercialDoc.To = oExportCommercialDoc.To.Replace("@APPLICANT", "");
                oExportCommercialDoc.To = oExportCommercialDoc.To+" " + oExportCommercialDoc.ApplicantName;
            }
            if (oExportCommercialDoc.For.Contains("@APPLICANT"))
            {
                oExportCommercialDoc.For = oExportCommercialDoc.For.Replace("@APPLICANT", "");
                oExportCommercialDoc.For = oExportCommercialDoc.For + " " + oExportCommercialDoc.ApplicantName;
            }

            oExportCommercialDoc.BUAddress = oBusinessUnit.Address;
            oExportCommercialDoc.BUName = oBusinessUnit.Name;
            
            if (oExportDocSetup.DocumentType == EnumDocumentType.Bill_Of_Exchange)
            {
                if (oExportDocSetup.PrintOn == EnumExcellColumn.A)
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_BillOfExchange(oExportCommercialDoc, oCompany, nPrintType, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else if (oExportDocSetup.PrintOn == EnumExcellColumn.B)
                {
                    rptExportDocB oReport = new rptExportDocB();
                    byte[] abytes = oReport.PrepareReport_BillOfExchange(oExportCommercialDoc, oCompany, nPrintType, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_BillOfExchange(oExportCommercialDoc, oCompany, nPrintType, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Delivery_Challan || oExportDocSetup.DocumentType == EnumDocumentType.Truck_Receipt || oExportDocSetup.DocumentType == EnumDocumentType.Weight_MeaList)
            {
                if (oExportCommercialDoc.PrintOn == EnumExcellColumn.A)
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_DeliveryChallan(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else if (oExportCommercialDoc.PrintOn == EnumExcellColumn.B || oExportCommercialDoc.PrintOn == EnumExcellColumn.C)
                {
                    rptExportDocB oReport = new rptExportDocB();
                    byte[] abytes = oReport.PrepareReport_DeliveryChallan(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_DeliveryChallan(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Commercial_Invoice || oExportDocSetup.DocumentType == EnumDocumentType.Packing_List || oExportDocSetup.DocumentType == EnumDocumentType.Packing_List_Detail)
            {
                if (oExportDocSetup.PrintOn == EnumExcellColumn.A)
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_CommercialInvoice(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else if (oExportDocSetup.PrintOn == EnumExcellColumn.B)
                {
                    rptExportDocB oReport = new rptExportDocB();
                    byte[] abytes = oReport.PrepareReport_CommercialInvoice(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_CommercialInvoice(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Beneficiary_Certificate)
            {
                if (oExportDocSetup.PrintOn == EnumExcellColumn.A)
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_BeneficiaryCertificate(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else if (oExportDocSetup.PrintOn == EnumExcellColumn.B)
                {
                    rptExportDocB oReport = new rptExportDocB();
                    byte[] abytes = oReport.PrepareReport_BeneficiaryCertificate(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_BeneficiaryCertificate(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Bank_Submission)
            {
                if (oExportDocSetup.PrintOn == EnumExcellColumn.A)
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_BankForwarding(oExportCommercialDoc, oCompany, nPrintType, _oExportBill, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else if (oExportDocSetup.PrintOn == EnumExcellColumn.B)
                {
                    rptExportDocB oReport = new rptExportDocB();
                    byte[] abytes = oReport.PrepareReport_Submition(oExportCommercialDoc, oCompany, nPrintType, _oExportBill, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_BankForwarding(oExportCommercialDoc, oCompany, nPrintType, _oExportBill, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Certificate_of_Origin)
            {
                if (oExportDocSetup.PrintOn == EnumExcellColumn.A)
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_CertificateOfORGIN(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else if (oExportDocSetup.PrintOn == EnumExcellColumn.B)
                {
                    rptExportDocB oReport = new rptExportDocB();
                    byte[] abytes = oReport.PrepareReport_CertificateOfORGIN(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_CertificateOfORGIN(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
            }
            else if (oExportDocSetup.DocumentType == EnumDocumentType.Bank_Forwarding)
            {
                if (oExportDocSetup.PrintOn == EnumExcellColumn.A)
                {
                    List<ExportDocForwarding> oExportDocForwardings = new List<ExportDocForwarding>();
                    oExportDocForwardings = ExportDocForwarding.Gets(_oExportBill.ExportLCID, (int)EnumMasterLCType.ExportLC, (int)Session[SessionInfo.currentUserID]);

                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_Bank_Forwarding(oExportCommercialDoc, oCompany, nPrintType, oExportDocForwardings, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else if (oExportDocSetup.PrintOn == EnumExcellColumn.B)
                {
                    List<ExportDocForwarding> oExportDocForwardings = new List<ExportDocForwarding>();
                    oExportDocForwardings = ExportDocForwarding.Gets(_oExportBill.ExportLCID, (int)EnumMasterLCType.ExportLC, (int)Session[SessionInfo.currentUserID]);

                    rptExportDocB oReport = new rptExportDocB();
                    byte[] abytes = oReport.PrepareReport_Bank_Forwarding(oExportCommercialDoc, oCompany, nPrintType, oExportDocForwardings, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    List<ExportDocForwarding> oExportDocForwardings = new List<ExportDocForwarding>();
                    oExportDocForwardings = ExportDocForwarding.Gets(_oExportBill.ExportLCID, (int)EnumMasterLCType.ExportLC, (int)Session[SessionInfo.currentUserID]);

                    rptExportDoc oReport = new rptExportDoc();
                    byte[] abytes = oReport.PrepareReport_Bank_Forwarding(oExportCommercialDoc, oCompany, nPrintType, oExportDocForwardings, oBusinessUnit, nPageSize);
                    return File(abytes, "application/pdf");
                }
            }
            else
            {
                rptExportDoc oReport = new rptExportDoc();
                byte[] abytes = oReport.PrepareReport_CommercialInvoice(oExportCommercialDoc, oCompany, nPrintType, sMUName, oBusinessUnit, nPageSize);
                return File(abytes, "application/pdf");
            }
        }
       
        public void ExportToXL(string ids, int buid, double ts)
        {
            _oExportBills = new List<ExportBill>();
            string sSQL = "SELECT * FROM View_ExportBill AS HH WHERE HH.ExportBillID IN (" + ids + ") ORDER BY HH.ExportBillID";
            _oExportBills = ExportBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            using (var excelPackage = new ExcelPackage())
            {
                int nRowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export Bill List");
                sheet.Name = "Contractor";
                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 22; //Bill No
                sheet.Column(4).Width = 40; //Bill Date
                sheet.Column(5).Width = 35; //Bill Status
                sheet.Column(6).Width = 35; //LC No
                sheet.Column(7).Width = 25; //IDBC/ IBC No
                sheet.Column(8).Width = 20; //Applicant
                sheet.Column(9).Width = 20; //Issue Bank
                sheet.Column(10).Width = 20; //Advice Bank
                sheet.Column(11).Width = 20; //Submit  To Party Bank
                sheet.Column(12).Width = 20; //Send T0 Party
                sheet.Column(13).Width = 20; //Recv From Bank
                sheet.Column(14).Width = 20; //Maturity Red Date
                sheet.Column(15).Width = 20; //Maturity Date
                sheet.Column(16).Width = 20; //Discounted Date
                sheet.Column(17).Width = 20; //Bank FDD Recv Date
                sheet.Column(18).Width = 20; //Doc Prepare Date
                sheet.Column(19).Width = 20; //Relization Date
                sheet.Column(20).Width = 20; //Encashment Date
                sheet.Column(21).Width = 25; //Bill Amount
                sheet.Column(22).Width = 20; //Up No

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);

                #region Report Header
                sheet.Cells[nRowIndex, 2, nRowIndex, 20].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 20].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Export Bill List"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Issue Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Advice Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Bill No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Current Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Doc Prepare Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Send To Party"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Recv From Party"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Send To Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Recd From Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "LDBC/IBC Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = "LDBC/IBC No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Submit To Party Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Maturity Red Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 19]; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 20]; cell.Value = "Discounted Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 21]; cell.Value = "BankFDD Recd Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 22]; cell.Value = "Relization Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 23]; cell.Value = "Encashment Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 24]; cell.Value = "UP No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0; double nTotalAmount = 0; string sCurrency = "";
                foreach (ExportBill oItem in _oExportBills)
                {
                    nCount++;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ApplicantName; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.BankName_Issue; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.BankName_Advice; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ExportBillNoSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.StateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.StartDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.SendToPartySt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.RecdFromPartySt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.SendToBankDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.RecedFromBankDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.LDBCDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.LDBCNo; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.AcceptanceDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = oItem.MaturityReceivedDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = oItem.MaturityDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = oItem.DiscountedDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 21]; cell.Value = oItem.BankFDDRecDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 22]; cell.Value = oItem.RelizationDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 23]; cell.Value = oItem.EncashmentDateSt; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 24]; cell.Value = oItem.UPNo; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    sCurrency = oItem.Currency;
                    nTotalAmount = nTotalAmount + oItem.Amount;                 
                    nRowIndex++;
                }

                #region Total
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                cell.Value = "Total : "; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nTotalAmount; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 24]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportBill.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
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

        public System.Drawing.Image GetCompanyTitle(Company oCompany)
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

        #region Get Export Bill For Export UP
        [HttpPost]
        public JsonResult GetExportBillForExportUP(ExportBill oExportBill)
        {

            List<ExportBill> oExportBills = new List<ExportBill>();
            try
            {
                string sSQL = "";

                if (!string.IsNullOrEmpty(oExportBill.ExportLCNo))
                    sSQL = "Select top(100)* from View_ExportBill Where ExportBillID<>0 And ExportBillID Not In (Select ExportBillID from ExportUPDetail) And ExportLCNo Like '%" + oExportBill.ExportLCNo.Trim() + "%' AND BUID ="+oExportBill.BUID;
                else if (!string.IsNullOrEmpty(oExportBill.ExportBillNo))
                    sSQL = "Select top(100)* from View_ExportBill Where ExportBillID<>0 And ExportBillID Not In (Select ExportBillID from ExportUPDetail) And ExportBillNo Like '%" + oExportBill.ExportBillNo.Trim() + "%' AND BUID =" + oExportBill.BUID;

                if (sSQL != "")
                    oExportBills = ExportBill.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oExportBills = new List<ExportBill>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportBills);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Export Buyer Letter
        public ActionResult PrintExport_BuyerLetter(int id, int buid, double ts)
        {
            _oExportBill = new ExportBill();
            _oExportBill = _oExportBill.Get(id, (int)Session[SessionInfo.currentUserID]);
            
            Company oCompany = new Company();
            Company oSubCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oCompany.CompanyTitle = this.GetCompanyTitle(oCompany);

            DocPrintEngine oDocPrintEngine = new DocPrintEngine();
            oDocPrintEngine = oDocPrintEngine.GetActiveByType((int)EnumDocumentPrintType.Buyer_Letter_Bill, (int)Session[SessionInfo.currentUserID]);
            oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID =" + oDocPrintEngine.DocPrintEngineID + "  ORDER BY CONVERT(INT, SLNo)", (int)Session[SessionInfo.currentUserID]);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportBill.BUID, (int)Session[SessionInfo.currentUserID]);

            List<ExportPILCMapping> oExportPILCMappings = new List<ExportPILCMapping>();
            oExportPILCMappings = ExportPILCMapping.GetsByEBillID(_oExportBill.ExportBillID, (int)Session[SessionInfo.currentUserID]);
            _oExportBill.PINo = "";

            foreach (ExportPILCMapping oItem in oExportPILCMappings)
            {
                _oExportBill.PINo = _oExportBill.PINo + "" + oItem.PINo_Full + " DT : " + oItem.IssueDateST + ",";
            }
            _oExportBill.PINo.TrimEnd(new char[',']);

            rptExportBill_BuyerLetter oReport = new rptExportBill_BuyerLetter();
            byte[] abytes = oReport.PrepareReport(_oExportBill, oCompany, oBusinessUnit, oDocPrintEngine);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Advance Search
        [HttpPost]
        public JsonResult AdvanceSearch(ExportBill oExportBill)
        {
            List<ExportBill> oExportBills = new List<ExportBill>();
            string sSQL = MakeSQL(oExportBill);
            if (sSQL == "Error")
            {
                _oExportBill = new ExportBill();
                _oExportBill.ErrorMessage = "Please select a searching critaria.";
                oExportBills = new List<ExportBill>();
                //oExportBills.Add(_oExportBill);
            }
            else
            {
                oExportBills = new List<ExportBill>();
                oExportBills = ExportBill.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oExportBills.Count == 0)
                {
                    _oExportBill = new ExportBill();
                    _oExportBill.ErrorMessage = "No Data Found.";
                    oExportBills = new List<ExportBill>();
                    //oExportBills.Add(_oExportBill);
                }
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oExportBills);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(oExportBills, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(ExportBill oExportBill)
        {
            int nflag = 0;
            string sReturn1 = "SELECT * FROM View_ExportBill AS EB";
            string sReturn = "";

            #region Bill No
            if (!String.IsNullOrEmpty(oExportBill.ExportBillNo))
            {
                    oExportBill.ExportBillNo = oExportBill.ExportBillNo.Trim();
                    nflag = 1;
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "EB.Bill+'-'+ExportBillNo LIKE '%" + oExportBill.ExportBillNo + "%'";
            }
            #endregion
            #region L/C No
            if (!String.IsNullOrEmpty(oExportBill.ExportLCNo))
            {
                oExportBill.ExportLCNo = oExportBill.ExportLCNo.Trim();
                nflag = 1;
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportLCNo  LIKE '%" + oExportBill.ExportLCNo + "%'";
            }
            #endregion
            #region LDBCNo No
            if (!String.IsNullOrEmpty(oExportBill.LDBCNo))
            {
                oExportBill.LDBCNo = oExportBill.LDBCNo.Trim();
                nflag = 1;
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "LDBCNo LIKE '%" + oExportBill.LDBCNo + "%'";
            }
            #endregion

            #region Current State

            if (oExportBill.BUID > 0)
            {
                nflag = 1;
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BUID IN (" + oExportBill.BUID + ") ";
            }

            #endregion

            #region PartyName
            if (!String.IsNullOrEmpty(oExportBill.ApplicantName))
            {

                nflag = 1;
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ApplicantID IN (" + oExportBill.ApplicantName + ")";

            }
            #endregion
            #region Export LC Type
            if (oExportBill.ExportLCType != null && oExportBill.ExportLCType != EnumExportLCType.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ExportLCType = " + ((int)oExportBill.ExportLCType).ToString();
            }
            #endregion

            #region Current State
            if (oExportBill.CurrentStateIDs != "")
            {
                if (oExportBill.CurrentStateIDs != null)
                {
                    nflag = 1;
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " EB.State IN (" + oExportBill.CurrentStateIDs + ") ";
                }
            }
            #endregion

            #region Advice Bank
            if (oExportBill.BankBranchID_Advice > 0)
            {
                nflag = 1;
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BankBranchID_Negotiation = " + oExportBill.BankBranchID_Advice;
            }
            #endregion
            #region Nego Bank
            if (oExportBill.BankBranchID_Nego > 0)
            {
                nflag = 1;
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BankBranchID_Negotiation = " + oExportBill.BankBranchID_Nego;
            }
            #endregion

            #region Issue Bank
            if (!String.IsNullOrEmpty(oExportBill.BBranchName_Issue))
            {
                nflag = 1;
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BankBranchID_Issue in(" + oExportBill.BBranchName_Issue+ ")";
            }
            #endregion

            #region Bill Amount
            if (oExportBill.SearchAmountType > 0)
            {
                nflag = 1;
                Global.TagSQL(ref sReturn);
                if (oExportBill.SearchAmountType == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " EB.Amount = " + oExportBill.FromAmount;
                }
                else if (oExportBill.SearchAmountType == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " EB.Amount != " + oExportBill.FromAmount;
                }
                else if (oExportBill.SearchAmountType == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " EB.Amount > " + oExportBill.FromAmount;
                }
                else if (oExportBill.SearchAmountType == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " EB.Amount < " + oExportBill.FromAmount;
                }
                else if (oExportBill.SearchAmountType == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " EB.Amount BETWEEN " + oExportBill.FromAmount + " AND " + oExportBill.ToAmount + "";
                }
                else if (oExportBill.SearchAmountType == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " EB.Amount NOT BETWEEN " + oExportBill.FromAmount + " AND " + oExportBill.ToAmount + "";
                }
            }
            #endregion

            #region Date Criteria
            if (oExportBill.DateType != "None")
            {

                if (oExportBill.DateSearchCriteria > 0)
                {
                    nflag = 1;
                    Global.TagSQL(ref sReturn);
                    if (oExportBill.DateSearchCriteria == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " EB." + oExportBill.DateType + " = '" + oExportBill.StartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (oExportBill.DateSearchCriteria == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " EB." + oExportBill.DateType + " != '" + oExportBill.StartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (oExportBill.DateSearchCriteria == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " EB." + oExportBill.DateType + " > '" + oExportBill.StartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (oExportBill.DateSearchCriteria == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " EB." + oExportBill.DateType + " < '" + oExportBill.StartDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (oExportBill.DateSearchCriteria == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EB." + oExportBill.DateType + " BETWEEN '" + oExportBill.StartDateCritaria.ToString("dd MMM yyy") + "' AND '" + oExportBill.EndDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                    else if (oExportBill.DateSearchCriteria == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " EB." + oExportBill.DateType + " NOT BETWEEN '" + oExportBill.StartDateCritaria.ToString("dd MMM yyy") + "' AND '" + oExportBill.EndDateCritaria.ToString("dd MMM yyy") + "' ";
                    }
                }
            }
            #endregion

            #region State on Date
            if (oExportBill.StateDateType > -1)
            {

                if (oExportBill.DateSearchState > 0)
                {
                    Global.TagSQL(ref sReturn);

                    nflag = 1;
                    if (oExportBill.DateSearchState == (int)EnumCompareOperator.EqualTo)
                    {
                        // sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + oExportBill.StateDateType + " AND EBH.DBServerDateTime = '" + oExportBill.StartDateState.ToString("dd MMM yyyy") + "') ";
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + oExportBill.StateDateType + " AND EBH.DBServerDateTime >= '" + oExportBill.StartDateState.ToString("dd MMM yyyy") + "' AND EBH.DBServerDateTime<'" + oExportBill.StartDateState.AddDays(1).ToString("dd MMM yyyy") + "') ";

                    }
                    else if (oExportBill.DateSearchState == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + oExportBill.StateDateType + " AND EBH.DBServerDateTime != '" + oExportBill.StartDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (oExportBill.DateSearchState == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + oExportBill.StateDateType + " AND EBH.DBServerDateTime > '" + oExportBill.StartDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (oExportBill.DateSearchState == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + oExportBill.StateDateType + " AND EBH.DBServerDateTime < '" + oExportBill.StartDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (oExportBill.DateSearchState == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + oExportBill.StateDateType + " AND EBH.DBServerDateTime >= '" + oExportBill.StartDateState.ToString("dd MMM yyyy") + "' AND  EBH.DBServerDateTime<'" + oExportBill.EndDateState.ToString("dd MMM yyyy") + "') ";
                    }
                    else if (oExportBill.DateSearchState == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + oExportBill.StateDateType + " AND EBH.DBServerDateTime NOT BETWEEN '" + oExportBill.StartDateState.ToString("dd MMM yyyy") + "' AND '" + oExportBill.EndDateState.ToString("dd MMM yyyy") + "') ";
                    }
                }
            }
            #endregion

            #region Export UP
            if (!string.IsNullOrEmpty(oExportBill.Params) && oExportBill.Params.Split('~').Length == 2)
            {
                nflag = 1;
                bool IsYetToEUP = Convert.ToBoolean(oExportBill.Params.Split('~')[0]);
                bool IsEUPDone = Convert.ToBoolean(oExportBill.Params.Split('~')[1]);

                if (IsYetToEUP)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " EB.ExportBillID Not In (Select  ExportBillID From ExportUPDetail)";
                }
                if (IsEUPDone)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " EB.ExportBillID In (Select  ExportBillID From ExportUPDetail)";
                }
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            if (nflag == 0)
            {
                sReturn = "Error";
            }
            return sReturn;
        }

        public System.Drawing.Image GetBoEImage(UserImage oUserImage)
        {
            if (oUserImage.ImageFile != null)
            {
                MemoryStream m = new MemoryStream(oUserImage.ImageFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "BoEImage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region Actions ExportClaimSettle
        public ActionResult ViewExportClaimSettle(int id, int buid)
        {
            ExportBill oExportBill = new ExportBill();
            _oExportClaimSettles = new List<ExportClaimSettle>();
            try
            {
                oExportBill = oExportBill.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oExportClaimSettles = ExportClaimSettle.GetsByBillID(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex) 
            {
                _oExportClaimSettle = new ExportClaimSettle();
                _oExportClaimSettle.ErrorMessage = ex.Message;
                _oExportClaimSettles.Add(_oExportClaimSettle);
            }
            ViewBag.BUID = buid;
            ViewBag.ExportBill = oExportBill;
            return View(_oExportClaimSettles);
        }

        [HttpPost]
        public JsonResult SaveExportClaimSettle(ExportClaimSettle oExportClaimSettle)
        {
            _oExportClaimSettle = new ExportClaimSettle();
            try
            {
                _oExportClaimSettle = oExportClaimSettle;
                _oExportClaimSettle = _oExportClaimSettle.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportClaimSettle = new ExportClaimSettle();
                _oExportClaimSettle.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportClaimSettle);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteExportClaimSettle(ExportClaimSettle oExportClaimSettle)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportClaimSettle.Delete(oExportClaimSettle.ExportClaimSettleID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
