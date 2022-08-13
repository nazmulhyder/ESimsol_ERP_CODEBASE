using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
	public class CommercialBSController : Controller
	{
		#region Declaration

		CommercialBS _oCommercialBS = new CommercialBS();
		List<CommercialBS> _oCommercialBSs = new  List<CommercialBS>();
        CommercialFDBP _oCommercialFDBP = new CommercialFDBP();
        List<CommercialFDBP> _oCommercialFDBPs = new List<CommercialFDBP>();
        CommercialEncashment _oCommercialEncashment = new CommercialEncashment();
        List<CommercialEncashment> _oCommercialEncashments = new List<CommercialEncashment>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewCommercialBSList(int buid, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CommercialBS).ToString() + "," + ((int)EnumModuleName.CommercialFDBP).ToString() + "," + ((int)EnumModuleName.CommercialEncashment).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oCommercialBSs = new List<CommercialBS>();
            _oCommercialBSs = CommercialBS.Gets("SELECT * FROM View_CommercialBS WHERE BUID= "+buid+" AND BSStatus ="+(int)EnumCommercialBSStatus.Intialized, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Buyers = Contractor.GetsByNamenType("", ((int)EnumContractorType.Buyer).ToString(), buid, (int)Session[SessionInfo.currentUserID]);
			return View(_oCommercialBSs);
		}

		public ActionResult ViewCommercialBS(int id)
		{
			_oCommercialBS = new CommercialBS();
			if (id > 0)
			{
				_oCommercialBS = _oCommercialBS.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oCommercialBS.CommercialBSDetails = CommercialBSDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}
			return View(_oCommercialBS);
		}
        public ActionResult ViewSubmitToBank(int id)
        {
            _oCommercialBS = new CommercialBS();
            if (id > 0)
            {
                _oCommercialBS = _oCommercialBS.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oCommercialBS);
        }
        public ActionResult ViewFDBPReceived(int id)
        {
            _oCommercialBS = new CommercialBS();
            Company oCompany = new Company();

            if (id > 0)
            {
                _oCommercialBS = _oCommercialBS.Get(id, (int)Session[SessionInfo.currentUserID]);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);//get company object for base currency
                _oCommercialBS.BCurrencyID = oCompany.BaseCurrencyID;
                _oCommercialBS.BCurrencySymbol = oCompany.CurrencySymbol;
                if(_oCommercialBS.BCurrencyID ==_oCommercialBS.CurrencyID)
                {
                    _oCommercialBS.CRate = 1;
                }
                _oCommercialBS.BSAmountBC =Math.Round(_oCommercialBS.BSAmount * _oCommercialBS.CRate,2);
            }
            return View(_oCommercialBS);
        }
        public ActionResult ViewMaturityRcv(int id)
        {
            _oCommercialBS = new CommercialBS();
            if (id > 0)
            {
                _oCommercialBS = _oCommercialBS.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oCommercialBS);
        }
        public ActionResult ViewDocRealization(int id)
        {
            _oCommercialBS = new CommercialBS();
            if (id > 0)
            {
                _oCommercialBS = _oCommercialBS.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oCommercialBS);
        }
        public ActionResult ViewEncashment(int id)
        {
            _oCommercialBS = new CommercialBS();
            if (id > 0)
            {
                _oCommercialBS = _oCommercialBS.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oCommercialBS);
        }
		[HttpPost]
		public JsonResult Save(CommercialBS oCommercialBS)
		{
			_oCommercialBS = new CommercialBS();
			try
			{
				_oCommercialBS = oCommercialBS;
				_oCommercialBS = _oCommercialBS.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oCommercialBS = new CommercialBS();
				_oCommercialBS.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oCommercialBS);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult Delete(CommercialBS oCommercialBS)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oCommercialBS.Delete(oCommercialBS.CommercialBSID, (int)Session[SessionInfo.currentUserID]);
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
        #region Searcing funciton
         [HttpPost]
        public JsonResult SearchBS(CommercialBS oCommercialBS)
        {
            _oCommercialBSs = new List<CommercialBS>();
          
            try
            {
                _oCommercialBSs = CommercialBS.Gets(MakeSQL(oCommercialBS), (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialBS = new CommercialBS();
                _oCommercialBS.ErrorMessage = ex.Message;
            }
            var jsonResult = Json(_oCommercialBSs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public string MakeSQL(CommercialBS oCommercialBS)
        {
            string sSearchingData = oCommercialBS.SearchingData;
            bool diSearcingDate= Convert.ToBoolean(sSearchingData.Split('~')[0]);
            DateTime dInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dInoviceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
          
            string sSQL = "SELECT * FROM View_CommercialBS ", sWhereCluse = "";
            #region BusinessUnit
            if (oCommercialBS.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oCommercialBS.BUID.ToString();
            }
            #endregion
            #region MasterLCNo
            if (oCommercialBS.MasterLCNo != null && oCommercialBS.MasterLCNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " MasterLCNo LIKE'%" + oCommercialBS.MasterLCNo + "%'";
            }
            #endregion
            #region BuyerID
            if (oCommercialBS.BuyerID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BuyerID IN (" + oCommercialBS.BuyerID + ")";
            }
            #endregion
            #region InvoiceDate
            if (diSearcingDate)
            {
                DateObject.CompareDateQuery(ref sWhereCluse, "InvoiceDate ", 5, dInvoiceStartDate, dInoviceEndDate);
            }
            #endregion
            sSQL = sSQL + sWhereCluse;

            return sSQL;
        }
        #endregion

        #region Get functions
        [HttpPost]
        public JsonResult GetMasterLCs(CommercialBS oCommercialBS)
        {
            List<MasterLC> oMasterLCs = new List<MasterLC>();
            try
            {
                string sSQL = "SELECT * FROM View_MasterLC WHERE BUID = "+oCommercialBS.BUID+" AND  MasterLCID In ( SELECT MasterLCID FROM CommercialInvoice WHERE ISNULL(GSP,0)=1 AND ISNULL(BL,0)=1AND ISNULL(IC,0)=1 AND CommercialInvoiceID NOT IN (SELECT CommercialInvoiceID FROM CommercialBSDetail))";
                if(!string.IsNullOrWhiteSpace(oCommercialBS.MasterLCNo))
                {
                    sSQL += " AND MasterLCNo LIKE '%" + oCommercialBS.MasterLCNo+"'%";
                }
                oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialBS = new CommercialBS();
                _oCommercialBS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMasterLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCommercialBSDetails(CommercialBS oCommercialBS)
        {
            List<CommercialInvoice> oCommercialInvoices = new List<CommercialInvoice>();
            List<CommercialBSDetail> oCommercialBSDetails = new List<CommercialBSDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_CommercialInvoice WHERE MasterLCID = " + oCommercialBS.MasterLCID + " AND ISNULL(ApprovedBy,0)!=0 AND ISNULL(GSP,0)=1 AND ISNULL(BL,0)=1AND ISNULL(IC,0)=1 AND CommercialInvoiceID NOT IN (SELECT CommercialInvoiceID FROM CommercialBSDetail)";
                oCommercialInvoices = CommercialInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach(CommercialInvoice oItem in oCommercialInvoices)
                {
                    CommercialBSDetail oCommercialBSDetail = new CommercialBSDetail();
                    oCommercialBSDetail.CommercialInvoiceID = oItem.CommercialInvoiceID;
                    oCommercialBSDetail.InvoiceNo = oItem.InvoiceNo;
                    oCommercialBSDetail.InvoiceQty = oItem.InvoiceQty;
                    oCommercialBSDetail.ShipmentMode = oItem.ShipmentMode;
                    oCommercialBSDetail.InvoiceAmount = oItem.InvoiceAmount;
                    oCommercialBSDetails.Add(oCommercialBSDetail);
                }
            }
            catch (Exception ex)
            {
                _oCommercialBS = new CommercialBS();
                _oCommercialBS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCommercialBSDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP ChangeStatus

        [HttpPost]
        public JsonResult ChangeStatus(CommercialBS oCommercialBS)
        {
            _oCommercialBS = new CommercialBS();
            try
            {
                if (oCommercialBS.ActionTypeInString == "Approved")
                {

                    oCommercialBS.ActionType = EnumCommercialBSActionType.Approved;

                }
                else if (oCommercialBS.ActionTypeInString == "SubmitToBank")
                {

                    oCommercialBS.ActionType = EnumCommercialBSActionType.SubmitToBank;

                }

                else if (oCommercialBS.ActionTypeInString == "FDBPRcv")
                {

                    oCommercialBS.ActionType = EnumCommercialBSActionType.FDBPRcv;

                }
                else if (oCommercialBS.ActionTypeInString == "MaturityRcv")
                {

                    oCommercialBS.ActionType = EnumCommercialBSActionType.MaturityRcv;

                }
                else if (oCommercialBS.ActionTypeInString == "DocRialization")
                {

                    oCommercialBS.ActionType = EnumCommercialBSActionType.DocRialization;

                }
                else if (oCommercialBS.ActionTypeInString == "Encashment")
                {

                    oCommercialBS.ActionType = EnumCommercialBSActionType.Encashment;

                }
                else if (oCommercialBS.ActionTypeInString == "BillClosed")
                {

                    oCommercialBS.ActionType = EnumCommercialBSActionType.BillClosed;
                }
                else if (oCommercialBS.ActionTypeInString == "BillCancel")
                {

                    oCommercialBS.ActionType = EnumCommercialBSActionType.BillCancel;
                }
                else if (oCommercialBS.ActionTypeInString == "Undo")
                {

                    oCommercialBS.ActionType = EnumCommercialBSActionType.Undo;
                }
                oCommercialBS = SetCommercialBSStatus(oCommercialBS);
                _oCommercialBS = oCommercialBS.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialBS = new CommercialBS();
                _oCommercialBS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCommercialBS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private CommercialBS SetCommercialBSStatus(CommercialBS oCommercialBS)//Set EnumOrderStatus Value
        {
            switch (oCommercialBS.BSStatusInInt)
            {
                case 0:
                    {
                        oCommercialBS.BSStatus = EnumCommercialBSStatus.Intialized;
                        break;
                    }
                case 1:
                    {
                        oCommercialBS.BSStatus = EnumCommercialBSStatus.Approved;
                        break;
                    }
                case 2:
                    {
                        oCommercialBS.BSStatus = EnumCommercialBSStatus.SubmitToBank;
                        break;
                    }
                case 3:
                    {
                        oCommercialBS.BSStatus = EnumCommercialBSStatus.FDBPRcv;
                        break;
                    }
                case 4:
                    {
                        oCommercialBS.BSStatus = EnumCommercialBSStatus.MaturityRcv;
                        break;
                    }
                case 5:
                    {
                        oCommercialBS.BSStatus = EnumCommercialBSStatus.DocRialization;
                        break;
                    }
                case 6:
                    {
                        oCommercialBS.BSStatus = EnumCommercialBSStatus.Encashment;
                        break;
                    }
                case 7:
                    {
                        oCommercialBS.BSStatus = EnumCommercialBSStatus.BillClosed;
                        break;

                    }
                case 8:
                    {
                        oCommercialBS.BSStatus = EnumCommercialBSStatus.BillCancel;
                        break;

                    }
            }

            return oCommercialBS;
        }
        #endregion

        #endregion

        #region Commercial FDBP
       
        public ActionResult ViewCommercialFDBP(int id, int CommercialBSID)
        {
            _oCommercialFDBP = new CommercialFDBP();
            if (id > 0)
            {
                _oCommercialFDBP = _oCommercialFDBP.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oCommercialFDBP.CommercialFDBPDetails = CommercialFDBPDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oCommercialBS = _oCommercialBS.Get(CommercialBSID, (int)Session[SessionInfo.currentUserID]);
                _oCommercialFDBP.CommercialBSID = _oCommercialBS.CommercialBSID;
                _oCommercialFDBP.MasterLCNo = _oCommercialBS.MasterLCNo;
                _oCommercialFDBP.BuyerName = _oCommercialBS.BuyerName;
                _oCommercialFDBP.BSAmount = _oCommercialBS.BSAmount;
                _oCommercialFDBP.PurchaseAmount = _oCommercialBS.BSAmount;
                _oCommercialFDBP.BankID = _oCommercialBS.BankID;
                _oCommercialFDBP.CRate = _oCommercialBS.CRate;
                _oCommercialFDBP.CurrencyID = _oCommercialBS.CurrencyID;
                _oCommercialFDBP.CurrencySymbol = _oCommercialBS.CurrencySymbol;
            }
            ViewBag.BankAccounts = BankAccount.GetsByBank(_oCommercialFDBP.BankID,(int)Session[SessionInfo.currentUserID]);
            return View(_oCommercialFDBP);
        }
        [HttpPost]
        public JsonResult SaveCommercialFDBP(CommercialFDBP oCommercialFDBP)
        {
            _oCommercialFDBP = new CommercialFDBP();
            _oCommercialBS = new CommercialBS();
            try
            {
                _oCommercialFDBP = oCommercialFDBP;
                _oCommercialFDBP = _oCommercialFDBP.Save((int)Session[SessionInfo.currentUserID]);
                _oCommercialFDBP.CommercialBS = _oCommercialBS.Get(_oCommercialFDBP.CommercialBSID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialFDBP = new CommercialFDBP();
                _oCommercialFDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCommercialFDBP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveCommercialFDBP(CommercialFDBP oCommercialFDBP)
        {
            _oCommercialFDBP = new CommercialFDBP();
            _oCommercialBS = new CommercialBS();
            try
            {
                _oCommercialFDBP = oCommercialFDBP;
                _oCommercialFDBP = _oCommercialFDBP.Approve((int)Session[SessionInfo.currentUserID]);
                _oCommercialFDBP.CommercialBS = _oCommercialBS.Get(_oCommercialFDBP.CommercialBSID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialFDBP = new CommercialFDBP();
                _oCommercialFDBP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCommercialFDBP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Commercial Encashment

        public ActionResult ViewCommercialEncashment(int id, int CommercialBSID)
        {
            _oCommercialEncashment = new CommercialEncashment();
            if (id > 0)
            {
                _oCommercialEncashment = _oCommercialEncashment.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oCommercialEncashment.CommercialEncashmentDetails = CommercialEncashmentDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                _oCommercialBS = _oCommercialBS.Get(CommercialBSID, (int)Session[SessionInfo.currentUserID]);
                _oCommercialEncashment.CommercialBSID = _oCommercialBS.CommercialBSID;
                _oCommercialEncashment.MasterLCNo = _oCommercialBS.MasterLCNo;
                _oCommercialEncashment.BuyerName = _oCommercialBS.BuyerName;
                _oCommercialEncashment.BSAmount = _oCommercialBS.BSAmount;
                _oCommercialEncashment.PurchaseValue = _oCommercialBS.PurchaseAmount;
                _oCommercialEncashment.PurchaseAmountBC = _oCommercialBS.PurchaseAmount * _oCommercialBS.CRate;
                _oCommercialEncashment.BankChargeBC = _oCommercialBS.BankChargeBC;
                
                _oCommercialEncashment.AmountInCurrency = _oCommercialBS.BSAmount;
                _oCommercialEncashment.AmountBC = _oCommercialBS.BSAmountBC;
                _oCommercialEncashment.EncashRate = 0;
                _oCommercialEncashment.OverDueInCurrency = 0;
                _oCommercialEncashment.OverDueBC = 0;
                _oCommercialEncashment.GainLossCurrencyID = oCompany.BaseCurrencyID;
                _oCommercialEncashment.GainLossCurrencySymbol = oCompany.CurrencySymbol;
                _oCommercialEncashment.GainLossAmount = 0;
                _oCommercialEncashment.IsGain = true;
                _oCommercialEncashment.PDeductionInCurrency = _oCommercialBS.BSAmount - _oCommercialBS.PurchaseAmount;
                _oCommercialEncashment.PDeductionBC = (_oCommercialBS.BSAmount - _oCommercialBS.PurchaseAmount)*_oCommercialBS.CRate;
                _oCommercialEncashment.BankRefNo = _oCommercialBS.BankRefNo;
                _oCommercialEncashment.FDBPNo = _oCommercialBS.FDBPNo;
                _oCommercialEncashment.BankName = _oCommercialBS.BankName;
                _oCommercialEncashment.BSAmountBC = _oCommercialBS.BSAmountBC;
                _oCommercialEncashment.CRate = _oCommercialBS.CRate;
                _oCommercialEncashment.LCValue = _oCommercialBS.LCValue;
                _oCommercialEncashment.BankCharge = _oCommercialBS.BankCharge;
                _oCommercialEncashment.BankID = _oCommercialBS.BankID;
                _oCommercialEncashment.LCCurrencyID = _oCommercialBS.CurrencyID;
                _oCommercialEncashment.CurrencySymbol = _oCommercialBS.CurrencySymbol;
                _oCommercialEncashment.BCurrencySymbol = _oCommercialBS.BCurrencySymbol;
                _oCommercialEncashment.BSIssueDate = _oCommercialBS.IssueDate;
                _oCommercialEncashment.SubmissionDate = _oCommercialBS.SubmissionDate;
                _oCommercialEncashment.FDBPReceiveDate = _oCommercialBS.FDBPReceiveDate;
                _oCommercialEncashment.MaturityDate = _oCommercialBS.MaturityDate;
                _oCommercialEncashment.RealizationDate = _oCommercialBS.RealizationDate;
                _oCommercialEncashment.RemainingBalance = (_oCommercialBS.BSAmount - _oCommercialBS.PurchaseAmount);
                _oCommercialEncashment.RemainingBalanceBC = (_oCommercialBS.BSAmountBC - _oCommercialBS.PurchaseAmountBC);
            }
            ViewBag.BankAccounts = BankAccount.GetsByBank(_oCommercialEncashment.BankID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CurrenncyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ExpenditureHeads = ExpenditureHead.Gets((int)Session[SessionInfo.currentUserID]);//for operation type :ExportBill_Encash
            return View(_oCommercialEncashment);
        }
        [HttpPost]
        public JsonResult SaveCommercialEncashment(CommercialEncashment oCommercialEncashment)
        {
            _oCommercialEncashment = new CommercialEncashment();
            _oCommercialBS = new CommercialBS();
            try
            {
                _oCommercialEncashment = oCommercialEncashment;
                _oCommercialEncashment = _oCommercialEncashment.Save((int)Session[SessionInfo.currentUserID]);
                _oCommercialEncashment.CommercialBS = _oCommercialBS.Get(_oCommercialEncashment.CommercialBSID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialEncashment = new CommercialEncashment();
                _oCommercialEncashment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCommercialEncashment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveCommercialEncashment(CommercialEncashment oCommercialEncashment)
        {
            _oCommercialEncashment = new CommercialEncashment();
            _oCommercialBS = new CommercialBS();
            try
            {
                _oCommercialEncashment = oCommercialEncashment;
                _oCommercialEncashment = _oCommercialEncashment.Approve((int)Session[SessionInfo.currentUserID]);
                _oCommercialEncashment.CommercialBS = _oCommercialBS.Get(_oCommercialEncashment.CommercialBSID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialEncashment = new CommercialEncashment();
                _oCommercialEncashment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCommercialEncashment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}
