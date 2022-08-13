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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Mail;


namespace ESimSolFinancial.Controllers
{
    public class FabricSalesContractController : Controller
    {
        #region 
        string _sErrorMessage = "";
        FabricSalesContract _oFabricSalesContract = new FabricSalesContract();
        FabricSalesContractDetail _oFabricSalesContractDetail = new FabricSalesContractDetail();
        List<FabricSalesContract> _oFabricSalesContracts = new List<FabricSalesContract>();

        public ActionResult ViewFabricSalesContracts(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricPO).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFabricSalesContracts = new List<FabricSalesContract>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricSalesContracts = FabricSalesContract.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsLocal == false).ToList();// EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            return View(_oFabricSalesContracts);
        }

        public ActionResult ViewFabricSalesContract(int id, int buid)
        {
            _oFabricSalesContract = new FabricSalesContract();
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            List<FabricDeliverySchedule> oFabricDeliverySchedules = new List<FabricDeliverySchedule>();
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oFabricSalesContract = _oFabricSalesContract.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract.FabricSalesContractDetails = FabricSalesContractDetail.Gets(_oFabricSalesContract.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricDeliverySchedules = FabricDeliverySchedule.GetsFSCID(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSalesContractNotes = FabricSalesContractNote.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract.PINo = this.GetPINos(_oFabricSalesContract.FabricSalesContractDetails);
            }
            else
            {
                oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(buid, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    foreach (MarketingAccount oItem in oMarketingAccounts)
                    {
                        if (oItem.IsGroupHead)
                        {
                            _oFabricSalesContract.MktGroupID = oItem.MarketingAccountID;
                            _oFabricSalesContract.MKTGroupName = oItem.Name;
                        }
                        if (oItem.UserID == ((User)Session[SessionInfo.CurrentUser]).UserID) 
                        {
                            _oFabricSalesContract.MktAccountID = oItem.MarketingAccountID;
                            _oFabricSalesContract.MKTPName = oItem.Name;
                        }
                    }
                }
            }

            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DeliveryOrderName> oDeliveryOrderNames = new List<DeliveryOrderName>();
            oDeliveryOrderNames = DeliveryOrderName.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)); 
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsLocal == false).ToList();
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.BUID = buid;
            ViewBag.DeliveryOrderNames = oDeliveryOrderNames;
            ViewBag.FabricDeliverySchedules = oFabricDeliverySchedules;
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.FabricSalesContractNotes = oFabricSalesContractNotes;
            ViewBag.BusinessUnit = oBusinessUnit;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            return View(_oFabricSalesContract);
        }
        public ActionResult ViewFabricSalesContractRevise(int id, int buid)
        {
            _oFabricSalesContract = new FabricSalesContract();
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            FabricSalesContractDetail oFSCD = new FabricSalesContractDetail();
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            List<FabricDeliverySchedule> oFabricDeliverySchedules = new List<FabricDeliverySchedule>();
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                oFSCD = oFSCD.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract = _oFabricSalesContract.Get(oFSCD.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract.FabricSalesContractDetails.Add(oFSCD);
                _oFabricSalesContract.PINo = this.GetPINos(_oFabricSalesContract.FabricSalesContractDetails);
            }
           
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DeliveryOrderName> oDeliveryOrderNames = new List<DeliveryOrderName>();
            oDeliveryOrderNames = DeliveryOrderName.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)); 
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsLocal == false).ToList();
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.BUID = buid;
            ViewBag.DeliveryOrderNames = oDeliveryOrderNames;
            ViewBag.FabricDeliverySchedules = oFabricDeliverySchedules;
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.FabricSalesContractNotes = oFabricSalesContractNotes;
            ViewBag.BusinessUnit = oBusinessUnit;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            return View(_oFabricSalesContract);
        }
        public ActionResult ViewFabricSalesContractStatus(int id, int buid)
        {
            _oFabricSalesContract = new FabricSalesContract();
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            FabricSalesContractDetail oFSCD = new FabricSalesContractDetail();
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            List<FabricDeliverySchedule> oFabricDeliverySchedules = new List<FabricDeliverySchedule>();
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                oFSCD = oFSCD.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract = _oFabricSalesContract.Get(oFSCD.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract.FabricSalesContractDetails.Add(oFSCD);
                _oFabricSalesContract.PINo = this.GetPINos(_oFabricSalesContract.FabricSalesContractDetails);
            }

            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DeliveryOrderName> oDeliveryOrderNames = new List<DeliveryOrderName>();
            oDeliveryOrderNames = DeliveryOrderName.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)); 
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsLocal == false).ToList();
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.BUID = buid;
            ViewBag.DeliveryOrderNames = oDeliveryOrderNames;
            ViewBag.FabricDeliverySchedules = oFabricDeliverySchedules;
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.FSCStatus = EnumObject.jGets(typeof(EnumPOState)).Where(x => x.id == (int)EnumPOState.Running || x.id == (int)EnumPOState.Hold);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.FabricSalesContractNotes = oFabricSalesContractNotes;
            ViewBag.BusinessUnit = oBusinessUnit;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            return View(_oFabricSalesContract);
        }
 
        public ActionResult ViewFabricSalesContracts_Own(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricPO).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFabricSalesContracts = new List<FabricSalesContract>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oFabricSalesContracts = FabricSalesContract.Gets("SELECT * FROM View_FabricSalesContract as FSC where Isnull(ApproveBy,0)=0 and OrderType in (" + ((int)EnumFabricRequestType.Local_Bulk).ToString() + "," + ((int)EnumFabricRequestType.Local_Sample).ToString() + " ) order by SCDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsLocal == true).ToList();
            
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)).Where(o => o.id == (int)(EnumFabricRequestType.Local_Bulk)).ToList(); // EnumObject.jGets(typeof(EnumFabricRequestType)).Where(x => int(EnumFabricRequestType.OwnPO)).ToList(); // EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            return View(_oFabricSalesContracts);
        }
        public ActionResult ViewFabricSalesContract_Own(int id, int buid)
        {
            _oFabricSalesContract = new FabricSalesContract();
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            List<FabricDeliverySchedule> oFabricDeliverySchedules = new List<FabricDeliverySchedule>();
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oFabricSalesContract = _oFabricSalesContract.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract.FabricSalesContractDetails = FabricSalesContractDetail.Gets(_oFabricSalesContract.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract.FabricSalesContractDetails = _oFabricSalesContract.FabricSalesContractDetails.OrderBy(o => o.SLNo).ToList();
                oFabricDeliverySchedules = FabricDeliverySchedule.GetsFSCID(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSalesContractNotes = FabricSalesContractNote.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract.PINo = this.GetPINos(_oFabricSalesContract.FabricSalesContractDetails);
            }
            else
            {
                oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(buid, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    foreach (MarketingAccount oItem in oMarketingAccounts)
                    {
                        if (oItem.IsGroupHead)
                        {
                            _oFabricSalesContract.MktAccountID = oItem.MarketingAccountID;
                            _oFabricSalesContract.MKTPName = oItem.Name;
                        }

                    }
                }
            }

            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DeliveryOrderName> oDeliveryOrderNames = new List<DeliveryOrderName>();
            oDeliveryOrderNames = DeliveryOrderName.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsLocal == true).ToList();
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.BUID = buid;
            ViewBag.DeliveryOrderNames = oDeliveryOrderNames;
            ViewBag.FabricDeliverySchedules = oFabricDeliverySchedules;
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.FabricSalesContractNotes = oFabricSalesContractNotes;
            ViewBag.BusinessUnit = oBusinessUnit;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            return View(_oFabricSalesContract);
        }

        public string GetPINos(List<FabricSalesContractDetail> oFabricSalesContractDetails)
        {
            List<FabricSalesContractDetail> oFSCDetails=new List<FabricSalesContractDetail>();
            oFSCDetails = oFabricSalesContractDetails.GroupBy(x => new { x.PINo, }, (key, grp) => new FabricSalesContractDetail { PINo = key.PINo, }).ToList();
            string sPINos = "";
            foreach (FabricSalesContractDetail oFSDetail in oFSCDetails)
            {
                    sPINos = sPINos + oFSDetail.PINo + ",";
            }
            if (sPINos.Length > 0) sPINos = sPINos.Remove(sPINos.Length - 1, 1);
            else sPINos = "P/I Not Issue";
            return sPINos;
        }
     
        #region HTTP Save
        private bool ValidateInput(FabricSalesContract oFabricSalesContract)
        {

            if (oFabricSalesContract.ContractorID <= 0)
            {
                _sErrorMessage = "Please Pick, Applicant Name.";
                return false;
            }
            if (oFabricSalesContract.CurrencyID<= 0)
            {
                _sErrorMessage = "Please select currency.";
                return false;
            }
            if (!String.IsNullOrEmpty(oFabricSalesContract.SCNo))
            {
                if (oFabricSalesContract.SCNo.Length < 9)
                {
                    _sErrorMessage = "Please PO Number Must be 9 digit. Like ->YYMMXXXXX Example:(180100001)";
                    return false;
                }
                if (oFabricSalesContract.SCNo.Length > 9)
                {
                    _sErrorMessage = "Please PO Number Must be 9 digit. Like ->YYMMXXXXX Example:(180100001)";
                    return false;
                }
            }

            return true;
        }
        [HttpPost]
        public JsonResult Save(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
            try
            {
                _oFabricSalesContract = oFabricSalesContract;
                _oFabricSalesContract.SCNo = _oFabricSalesContract.SCNo.Trim();
                if (this.ValidateInput(_oFabricSalesContract))
                {
                    _oFabricSalesContract = _oFabricSalesContract.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                    _oFabricSalesContract.FabricSalesContractDetails = FabricSalesContractDetail.Gets(_oFabricSalesContract.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {
                    _oFabricSalesContract.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oFabricSalesContract = new FabricSalesContract();
                _oFabricSalesContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveTwo(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
             List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            try
            {
                _oFabricSalesContract = oFabricSalesContract;
                _oFabricSalesContract.SCNo = _oFabricSalesContract.SCNo.Trim();
                if (this.ValidateInput(_oFabricSalesContract))
                {
                    _oFabricSalesContract = _oFabricSalesContract.UpdateInfo(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricSalesContractNotes = FabricSalesContractNote.Gets(oFabricSalesContract.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabricSalesContract.FabricSalesContractNotes = oFabricSalesContractNotes;
                }
                else
                {
                    _oFabricSalesContract.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oFabricSalesContract = new FabricSalesContract();
                _oFabricSalesContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_FSCNote(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
            List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            try
            {
                _oFabricSalesContract = oFabricSalesContract;

                _oFabricSalesContract = _oFabricSalesContract.Save_FSCNote(((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSalesContractNotes = FabricSalesContractNote.Gets(oFabricSalesContract.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract.FabricSalesContractNotes = oFabricSalesContractNotes;

            }
            catch (Exception ex)
            {
                _oFabricSalesContract = new FabricSalesContract();
                _oFabricSalesContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save_FabricDeliverySchedule(FabricDeliverySchedule oFabricDeliverySchedule)
        {
            oFabricDeliverySchedule.RemoveNulls();
            try
            {
                oFabricDeliverySchedule = oFabricDeliverySchedule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
          
            }
            catch (Exception ex)
            {
                oFabricDeliverySchedule = new FabricDeliverySchedule();
                oFabricDeliverySchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliverySchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete_FabricDeliverySchedule(FabricDeliverySchedule oFabricDeliverySchedule)
        {
            string sFeedBackMessage = "";
            try
            {
               
                sFeedBackMessage = oFabricDeliverySchedule.Delete( ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Approved(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
            try
            {

                _oFabricSalesContract = oFabricSalesContract;
                if (this.ValidateInput(_oFabricSalesContract))
                {

                    _oFabricSalesContract = _oFabricSalesContract.Approved(((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {
                    _oFabricSalesContract.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oFabricSalesContract = new FabricSalesContract();
                _oFabricSalesContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveLog(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
            try
            {
                _oFabricSalesContract = oFabricSalesContract;
                _oFabricSalesContract.SCNo = _oFabricSalesContract.SCNo.Trim();
                if (this.ValidateInput(_oFabricSalesContract))
                {
                    _oFabricSalesContract = _oFabricSalesContract.SaveLog(((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabricSalesContract.FabricSalesContractDetails = FabricSalesContractDetail.Gets(_oFabricSalesContract.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {
                    _oFabricSalesContract.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oFabricSalesContract = new FabricSalesContract();
                _oFabricSalesContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveRevise(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
            FabricSalesContractDetail oFSCDD = new FabricSalesContractDetail();
            try
            {
                _oFabricSalesContract = oFabricSalesContract;
                _oFabricSalesContract.SCNo = _oFabricSalesContract.SCNo.Trim();
                if (this.ValidateInput(_oFabricSalesContract))
                {
                    _oFabricSalesContract = _oFabricSalesContract.SaveRevise(((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oFabricSalesContract.FabricSalesContractDetails.Any())
                        oFSCDD = oFSCDD.Get(oFabricSalesContract.FabricSalesContractDetails[0].FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFSCDD.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                oFSCDD = new FabricSalesContractDetail();
                oFSCDD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFSCDD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_UpdateDispoNo(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
            try
            {

                _oFabricSalesContract = new FabricSalesContract();
                _oFabricSalesContract.FabricSalesContractDetails = FabricSalesContractDetail.Save_UpdateDispoNo(oFabricSalesContract.FabricSalesContractDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract = _oFabricSalesContract.Get(oFabricSalesContract.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSalesContract.FabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
            }
            catch (Exception ex)
            {
                _oFabricSalesContract = new FabricSalesContract();
                _oFabricSalesContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetHandLoomNo(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
            try
            {
                int nFSCID= oFabricSalesContract.FabricSalesContractID;

                var oFSCDetails = FabricSalesContractDetail.SetHandLoomNo(oFabricSalesContract.FabricSalesContractDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oFabricSalesContract=new FabricSalesContract();
                oFabricSalesContract = _oFabricSalesContract.Get(nFSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSalesContract.FabricSalesContractDetails = oFSCDetails;
            }
            catch (Exception ex)
            {
                oFabricSalesContract = new FabricSalesContract();
                oFabricSalesContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SetFabricExcNo(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            List<FabricSalesContractDetail> _oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            try
            {
                _oFabricSalesContractDetails = FabricSalesContractDetail.SetFabricExcNo(oFabricSalesContract, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            }
            catch (Exception ex)
            {
                oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetail.ErrorMessage = ex.Message;
                _oFabricSalesContractDetails.Add(oFabricSalesContractDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContractDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OrderHold(FabricSalesContractDetail oSCDetail)
        {
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            oFabricSalesContractDetail = oSCDetail.OrderHold(((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDyeingOrder.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContractDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveExtra(FabricSalesContractDetail oSCDetail)
        {
            FabricSCReport oFabricSCReport = new FabricSCReport();
            oFabricSCReport = oFabricSCReport.SaveExtra(oSCDetail, EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDyeingOrder.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSCReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Cancel(FabricSalesContract oFabricSalesContract)
        {
            string sErrorMease = "";
            _oFabricSalesContract = oFabricSalesContract;
            try
            {
                _oFabricSalesContract = oFabricSalesContract.Cancel(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Check(FabricSalesContract oFabricSalesContract)
        {
            string sErrorMease = "";
            _oFabricSalesContract = oFabricSalesContract;
            try
            {
                _oFabricSalesContract = oFabricSalesContract.Check(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSampleInvoice(FabricSalesContract oFabricSalesContract)
        {
            string sErrorMease = "";
            _oFabricSalesContract = oFabricSalesContract;
            try
            {
                _oFabricSalesContract = oFabricSalesContract.SaveSampleInvoice(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region HTTP Gets PI

        [HttpPost]
        public JsonResult GetsOpenPIFabric(FabricSalesContract oFabricSalesContract)
        {
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            try
            {
                //oFabricSalesContract.PINo
                if (string.IsNullOrEmpty(oFabricSalesContract.Params))
                {
                    oFabricSalesContract.Params = "0";
                }
                if (oFabricSalesContract.ContractorID>0)
                {
                    oFabricSalesContract.ContractorName = oFabricSalesContract.ContractorID.ToString();
                }
                if (oFabricSalesContract.ContractorID > 0 && oFabricSalesContract.BuyerID > 0)
                {
                    oFabricSalesContract.ContractorName = oFabricSalesContract.ContractorID.ToString() + "," + oFabricSalesContract.BuyerID.ToString();
                }
                if (string.IsNullOrEmpty(oFabricSalesContract.ContractorName))
                {
                    oExportPIDetails = ExportPIDetail.Gets("Select * from View_ExportPIDetail where isnull(FabricID,0)>0 and ExportPIID=" + oFabricSalesContract.ExportPIID + " and isnull(OrderSheetDetailID,0)=0 and ExportPIDetailID not in (" + oFabricSalesContract.Params + ") and ExportPIID in (Select ExportPIID from ExportPI where PIType=" + (int)EnumPIType.Open + " and ExportPI.PIStatus not in (" + (int)EnumPIStatus.Cancel + ") and BUID =" + oFabricSalesContract.BUID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oExportPIDetails = ExportPIDetail.Gets("Select * from View_ExportPIDetail where isnull(FabricID,0)>0 and ExportPIID=" + oFabricSalesContract.ExportPIID + " and isnull(OrderSheetDetailID,0)=0 and ExportPIDetailID not in (" + oFabricSalesContract.Params + ") and ExportPIID in (Select ExportPIID from ExportPI where PIType=" + (int)EnumPIType.Open + " and ContractorID in (" + oFabricSalesContract.ContractorName + ") and ExportPI.PIStatus not in (" + (int)EnumPIStatus.Cancel + ") and BUID =" + oFabricSalesContract.BUID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                foreach (ExportPIDetail oItem in oExportPIDetails)
                {
                    oFabricSalesContractDetail = new FabricSalesContractDetail();
                    oFabricSalesContractDetail.ExportPIDetailID = oItem.ExportPIDetailID;

                    oFabricSalesContractDetail.ProductID = oItem.ProductID;
                    oFabricSalesContractDetail.FabricID = oItem.FabricID;
                    oFabricSalesContractDetail.Qty = oItem.Qty;
                    oFabricSalesContractDetail.MUnitID = oItem.MUnitID;
                    oFabricSalesContractDetail.UnitPrice = oItem.UnitPrice;
                    oFabricSalesContractDetail.Amount = oItem.Amount;
                    oFabricSalesContractDetail.ProductCode = oItem.ProductCode;
                    oFabricSalesContractDetail.ProductName = oItem.ProductName;
                    oFabricSalesContractDetail.MUName = oItem.MUName;
                    oFabricSalesContractDetail.Currency = oItem.Currency;
                    oFabricSalesContractDetail.ColorInfo = oItem.ColorInfo;
                    oFabricSalesContractDetail.BuyerReference = oItem.BuyerReference;
                    oFabricSalesContractDetail.StyleNo = oItem.StyleNo;
                    oFabricSalesContractDetail.FabricSalesContractID =0;
                    oFabricSalesContractDetail.ExeNo = "";
                    oFabricSalesContractDetail.Size = oItem.SizeName;
                    //derive for Fabric
                    //oFabricSalesContractDetail.ColorName = oItem.GetString("ColorName");
                    oFabricSalesContractDetail.FabricNo = oItem.FabricNo;
                    oFabricSalesContractDetail.OptionNo ="";
                    oFabricSalesContractDetail.FabricWidth = oItem.FabricWidth;
                    oFabricSalesContractDetail.Construction = oItem.Construction;
                    oFabricSalesContractDetail.ConstructionPI = oItem.Construction;
                    oFabricSalesContractDetail.ProcessType = oItem.ProcessType;
                    oFabricSalesContractDetail.FabricWeave = oItem.FabricWeave;
                    oFabricSalesContractDetail.FinishType = oItem.FinishType;
                    oFabricSalesContractDetail.FabricDesignID = oItem.FabricDesignID;
                    oFabricSalesContractDetail.FabricDesignName = "";
                    oFabricSalesContractDetail.ProcessTypeName = oItem.ProcessTypeName;
                    oFabricSalesContractDetail.FabricWeaveName = oItem.FabricWeaveName;
                    oFabricSalesContractDetail.FinishTypeName = oItem.FinishTypeName;
                    oFabricSalesContractDetail.Note ="";
                    oFabricSalesContractDetail.PINo = oItem.PINo;
                    oFabricSalesContractDetail.HLReference ="";
                    oFabricSalesContractDetail.DesignPattern = "";
                    oFabricSalesContractDetail.IsProduction = true;
                    oFabricSalesContractDetail.Qty_PI = oItem.Qty;
                    oFabricSalesContractDetail.Status = EnumPOState.Initialized;
                    oFabricSalesContractDetails.Add(oFabricSalesContractDetail);

                }
            }
            catch (Exception ex)
            {
                 oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContractDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsOpenPI(FabricSalesContract oFabricSalesContract)
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();
            List<FabricSalesContract> oFabricSalesContracts = new List<FabricSalesContract>();
            FabricSalesContract oFabricSalesContract_Temp= new FabricSalesContract();
            try
            {
                if (string.IsNullOrEmpty(oFabricSalesContract.Params))
                {
                    oFabricSalesContract.Params = "0";
                }

                if (oFabricSalesContract.ContractorID > 0)
                {
                    oFabricSalesContract.ContractorName = oFabricSalesContract.ContractorID.ToString();
                }
                if (oFabricSalesContract.ContractorID > 0 && oFabricSalesContract.BuyerID > 0)
                {
                    oFabricSalesContract.ContractorName = oFabricSalesContract.ContractorID.ToString() + "," + oFabricSalesContract.BuyerID.ToString();
                }

                string sSQL = "Select top(50)* from View_ExportPI where ExportPIID>0";

                if (!string.IsNullOrEmpty(oFabricSalesContract.ContractorName))
                {
                    sSQL = sSQL + " And ContractorID in (" + oFabricSalesContract.ContractorName + ")";
                }
                sSQL = sSQL + " and ExportPIID in (Select ExportPIID from View_ExportPIDetail where isnull(OrderSheetDetailID,0)=0 and ExportPIDetailID not in (" + oFabricSalesContract.Params + "))and PIType=" + (int)EnumPIType.Open + " and PIStatus not in (" + (int)EnumPIStatus.Cancel + ") and BUID =" + oFabricSalesContract.BUID;

                oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


                foreach (ExportPI oItem in oExportPIs)
                {
                    oFabricSalesContract_Temp = new FabricSalesContract();
                    oFabricSalesContract_Temp.FabricSalesContractID = oFabricSalesContract.FabricSalesContractID;
                    oFabricSalesContract_Temp.ExportPIID = oItem.ExportPIID;
                    oFabricSalesContract_Temp.SCNo = oFabricSalesContract.SCNo;
                    oFabricSalesContract_Temp.PINo = oItem.PINo;
                    oFabricSalesContract_Temp.BuyerID = oItem.BuyerID;
                    oFabricSalesContract_Temp.BuyerName = oItem.BuyerName;
                    oFabricSalesContract_Temp.ContractorName = oItem.ContractorName;
                    oFabricSalesContract_Temp.ContractorID = oItem.ContractorID;
                    oFabricSalesContract_Temp.Amount = oItem.Amount;
                    oFabricSalesContract_Temp.Qty = oItem.Qty;
                    oFabricSalesContract_Temp.PaymentType = oItem.PaymentType;
                    oFabricSalesContract_Temp.LCTermID = oItem.LCTermID;
                    oFabricSalesContract_Temp.LCTermsName = oItem.LCTermsName;
                    oFabricSalesContract_Temp.Currency = oItem.Currency;
                    oFabricSalesContract_Temp.CurrencyID = oItem.CurrencyID;
                    oFabricSalesContract_Temp.OrderType = oFabricSalesContract.OrderType;
                    oFabricSalesContracts.Add(oFabricSalesContract_Temp);

                }
            }
            catch (Exception ex)
            {
                oFabricSalesContract_Temp = new FabricSalesContract();
                oFabricSalesContract_Temp.ErrorMessage = ex.Message;
                oFabricSalesContracts.Add(oFabricSalesContract_Temp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContracts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region HTTP Delete

        [HttpPost]
        public JsonResult DeleteFabricSalesContract(FabricSalesContract oFabricSalesContract)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oFabricSalesContract.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    
        #endregion


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

        [HttpPost]
        public JsonResult GetsDistingItem(FabricSalesContract oFabricSalesContract)
        {
            string sSQL = "";
            List<FabricSalesContract> oFabricSalesContracts = new List<FabricSalesContract>();
            try
            {

                #region EndUse
                if (oFabricSalesContract.EndUse == "EndUse")
                {
                    if (!string.IsNullOrEmpty(oFabricSalesContract.Params))
                    {
                        sSQL = "SELECT DISTINCT(EndUse) As DistinctItem  FROM FabricSalesContract where LEN(EndUse) > 1 and EndUse LIKE '%" + oFabricSalesContract.Params + "%'";
                    }
                    else
                    {
                        sSQL = "SELECT DISTINCT(EndUse) As DistinctItem  FROM FabricSalesContract where LEN(EndUse) > 1";
                    }
                }
                #endregion
                #region EndUse
                if (oFabricSalesContract.QualityParameters == "QualityParameters")
                {

                    if (!string.IsNullOrEmpty(oFabricSalesContract.Params))
                    {
                        sSQL = "SELECT DISTINCT(QualityParameters) As DistinctItem  FROM FabricSalesContract where LEN(QualityParameters) > 1 and QualityParameters LIKE '%" + oFabricSalesContract.Params + "%'";
                    }
                    else
                    {
                        sSQL = "SELECT DISTINCT(QualityParameters) As DistinctItem  FROM FabricSalesContract where LEN(QualityParameters) > 1";
                    }

                }
                #endregion
                #region EndUse
                if (oFabricSalesContract.QtyTolarance == "QtyTolarance")
                {
                   
                    if (!string.IsNullOrEmpty(oFabricSalesContract.Params))
                    {
                        sSQL = "SELECT DISTINCT(QtyTolarance) As DistinctItem  FROM FabricSalesContract where LEN(QtyTolarance) > 1 and QtyTolarance LIKE '%" + oFabricSalesContract.Params + "%'";
                    }
                    else
                    {
                        sSQL = "SELECT DISTINCT(QtyTolarance) As DistinctItem  FROM FabricSalesContract where LEN(QtyTolarance) > 1";
                    }
                }
                #endregion
                #region EndUse
                if (oFabricSalesContract.GarmentWash == "GarmentWash")
                {
                    if (!string.IsNullOrEmpty(oFabricSalesContract.Params))
                    {
                        sSQL = "SELECT DISTINCT(GarmentWash) As DistinctItem  FROM FabricSalesContract where LEN(GarmentWash) > 1 and GarmentWash LIKE '%" + oFabricSalesContract.Params + "%'";
                    }
                    else
                    {
                        sSQL = "SELECT DISTINCT(GarmentWash) As DistinctItem  FROM FabricSalesContract where LEN(GarmentWash) > 1";
                    }
                }
                #endregion

                _oFabricSalesContracts = FabricSalesContract.Gets_DistinctItem(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
                if(_oFabricSalesContracts.Count<=0)
                {
                    _oFabricSalesContract = new FabricSalesContract();
                    _oFabricSalesContract.ErrorMessage = "";
                    _oFabricSalesContracts.Add(_oFabricSalesContract);
                }

            }
            catch (Exception ex)
            {
                _oFabricSalesContract = new FabricSalesContract();
                _oFabricSalesContract.ErrorMessage = ex.Message;
                _oFabricSalesContracts.Add(_oFabricSalesContract);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContracts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region HttpGet For Search
        [HttpPost]
        public JsonResult AdvanchSearch(FabricSalesContract oFabricSalesContract)
        {
            _oFabricSalesContracts = new List<FabricSalesContract>();
            try
            {
                string sSQL = GetSQL(oFabricSalesContract.ErrorMessage);
                _oFabricSalesContracts = FabricSalesContract.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricSalesContract = new FabricSalesContract();
                _oFabricSalesContract.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oFabricSalesContracts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQL(string sTemp)
        {
            string sReturn1 = "SELECT * FROM View_FabricSalesContract ";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sTemp))
            {
                #region Set Values

                string sExportPINo = Convert.ToString(sTemp.Split('~')[0]);
                string sFabricID = Convert.ToString(sTemp.Split('~')[1]);
                string sContractorIDs = Convert.ToString(sTemp.Split('~')[2]);
                string sBuyerIDs = Convert.ToString(sTemp.Split('~')[3]);
                string sMAccountIDs = Convert.ToString(sTemp.Split('~')[4]);
                

                int nCboSCDate = Convert.ToInt32(sTemp.Split('~')[5]);
                DateTime dFromSCDate = DateTime.Now;
                DateTime dToSCDate = DateTime.Now;
                if (nCboSCDate>0)
                {
                    dFromSCDate = Convert.ToDateTime(sTemp.Split('~')[6]);
                    dToSCDate = Convert.ToDateTime(sTemp.Split('~')[7]);
                }

                int ncboApproveDate = Convert.ToInt32(sTemp.Split('~')[8]);
                DateTime dFromApproveDate = DateTime.Now;
                DateTime dToApproveDate = DateTime.Now;
                if (ncboApproveDate > 0)
                {
                    dFromApproveDate = Convert.ToDateTime(sTemp.Split('~')[9]);
                    dToApproveDate = Convert.ToDateTime(sTemp.Split('~')[10]);
                }

                int ncboProductionType = Convert.ToInt32(sTemp.Split('~')[11]);
                string sOrderTypeIDs = Convert.ToString(sTemp.Split('~')[12]);
                string sCurrentStatus = Convert.ToString(sTemp.Split('~')[13]);
                int nIsPrinting = Convert.ToInt32(sTemp.Split('~')[14]);
                string sMKTGroupIDs = Convert.ToString(sTemp.Split('~')[15]);
                int nBUID = Convert.ToInt32(sTemp.Split('~')[16]);
               
                #endregion

                #region Make Query

                #region Export PI NO
                if (!string.IsNullOrEmpty(sExportPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricSalesContractID in (Select FabricSalesContractDetail.FabricSalesContractID from FabricSalesContractDetail where ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINo like '%" + sExportPINo + "%'))";
                }
                #endregion

                #region sFabricID
                if (!string.IsNullOrEmpty(sFabricID))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "Select * from FabricSalesContract where FabricSalesContractID in (Select FabricSalesContractDetail.FabricSalesContractID from FabricSalesContractDetail where FabricID in (Select Fabric.FabricID from Fabric where FabricNo like '%" + sFabricID + "%'))";
                }
                #endregion

                #region ContractorID
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID in( " + sContractorIDs + ") ";
                }
                #endregion

                #region BBuyerID
                if (!String.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in( " + sBuyerIDs + ") ";
                }
                #endregion

                #region sMAccountIDs
                if (!String.IsNullOrEmpty(sMAccountIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID in(" + sMAccountIDs + ") ";
                }
                #endregion

                #region sMKTGroupIDs
                if (!String.IsNullOrEmpty(sMKTGroupIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID in(Select MarketingAccountID  from View_MarketingAccount where GroupID >0 and GroupID in (" + sMKTGroupIDs + ")) ";
                }
                #endregion
               
                #region F SC Date
                if (nCboSCDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboSCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                #region Approval Date
                if (ncboApproveDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboApproveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                
                #region ncboProductionType
                if (ncboProductionType > 0)
                {
                    
                    if (ncboProductionType == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsInHouse =1";
                    } 
                    else  if (ncboProductionType == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsInHouse =0";
                    }
                   
                }
                #endregion
                #region nIsPrinting
                if (nIsPrinting > 0)
                {
                    if (nIsPrinting == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsPrint =1";
                    }
                    else if (nIsPrinting == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "IsPrint =0";
                    }
                }
                #endregion

                #region OrderType
                if (!string.IsNullOrEmpty(sOrderTypeIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderType IN (" + sOrderTypeIDs + ") ";
                }
                #endregion
                #region CurrentStatus
                if (!string.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Currentstatus IN (" + sCurrentStatus + ") ";
                }
                #endregion

                List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(nBUID, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                }
                #endregion

            }
            sReturn = sReturn1 + sReturn + " order by SCDate DESC";
            return sReturn;
        }
        #endregion

        #endregion
        [HttpPost]
        public JsonResult GetbySCNo(FabricSalesContract oFabricSalesContract)
        {

            _oFabricSalesContracts = new List<FabricSalesContract>();
            string sReturn1 = "SELECT top(150)* FROM View_FabricSalesContract ";
            string sReturn = "";

            #region Export LC NO
            if (!string.IsNullOrEmpty(oFabricSalesContract.SCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SCNo LIKE '%" + oFabricSalesContract.SCNo + "%' ";
            }
            #endregion
            #region SCDate
            if (oFabricSalesContract.PaymentTypeInt == (int)EnumCompareOperator.EqualTo)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  CONVERT(DATE,CONVERT(VARCHAR(12),[SCDate] ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oFabricSalesContract.SCDate.ToString("dd MMM yyyy") + "',106))";
            }
            #endregion
            #region ApproveBy
            if (oFabricSalesContract.ApproveBy>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Isnull(ApproveBy,0)<>0 ";
                //sReturn = sReturn + "Isnull(ApproveBy,0)<>0 and FabricSalesContractID in (Select FabricSalesContractID from view_FabricSalesContractDetail as FS where  FS.Qty>isnull(FS.Qty_PI,0))";
            }
            #endregion

            #region 
            if (oFabricSalesContract.ContractorID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in ("+oFabricSalesContract.ContractorID+","+oFabricSalesContract.BuyerID+")";
            }
            if (oFabricSalesContract.BuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in (" + oFabricSalesContract.ContractorID + "," + oFabricSalesContract.BuyerID + ")";
            }
            #endregion
            #region sFabricID
            if (!string.IsNullOrEmpty(oFabricSalesContract.Params))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricSalesContractID in (Select FabricSalesContractDetail.FabricSalesContractID from FabricSalesContractDetail where FabricID in (Select Fabric.FabricID from Fabric where FabricNo like '%" + oFabricSalesContract.Params + "%'))";
            }
            #endregion
            List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(oFabricSalesContract.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oMarketingAccounts.Count > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            }
            //#region sFabricID
            //if (!string.IsNullOrEmpty(oFabricSalesContract.PINo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "FabricSalesContractID in (Select FabricSalesContractDetail.FabricSalesContractID from FabricSalesContractDetail where Isnull(FabricSalesContractDetail.ExportPIDetailID,0)<=0)";
            //}
            //#endregion
            #region OrderType
            if (oFabricSalesContract.OrderType>0)
            {
                Global.TagSQL(ref sReturn);
                if (oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
                {
                    sReturn = sReturn + "OrderType in ("+(int)EnumFabricRequestType.Buffer + "," +(int)EnumFabricRequestType.StockSale + "," + (int)EnumFabricRequestType.SampleFOC + "," + (int)EnumFabricRequestType.Sample + "," + (int)EnumFabricRequestType.Bulk + ")";
                }
                else
                {
                    sReturn = sReturn + "OrderType=" + oFabricSalesContract.OrderType;
                }
            }
            #endregion
            string sSQL = sReturn1 + sReturn;
            _oFabricSalesContracts = FabricSalesContract.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContracts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UpdateReviseNo(int id, int ReviseNo)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            try
            {
                oFabricSalesContract = oFabricSalesContract.UpdateReviseNo(id, ReviseNo, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFabricSalesContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFSCLog(FabricSalesContract oFabricSalesContract)
        {
            List<FabricSalesContract> oFabricSalesContractLogs = new List<FabricSalesContract>();
            oFabricSalesContractLogs = FabricSalesContract.Gets("Select * from View_FabricSalesContractLog where  FabricSalesContractID = '" + oFabricSalesContract.FabricSalesContractID + "'", ((User)Session[SessionInfo.CurrentUser]).UserID);

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFabricSalesContractDetails);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            return Json(oFabricSalesContractLogs);
        }
        [HttpPost]
        public JsonResult GetbySCNoMKT(FabricSalesContract oFabricSalesContract)
        {

            _oFabricSalesContracts = new List<FabricSalesContract>();
            string sReturn1 = "SELECT * FROM View_FabricSalesContract ";
            string sReturn = "";

            #region Export LC NO
            if (!string.IsNullOrEmpty(oFabricSalesContract.SCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SCNo LIKE '%" + oFabricSalesContract.SCNo + "%' ";
            }
            #endregion
            #region SCDate
            if (oFabricSalesContract.PaymentTypeInt == (int)EnumCompareOperator.EqualTo)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  CONVERT(DATE,CONVERT(VARCHAR(12),[SCDate] ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oFabricSalesContract.SCDate.ToString("dd MMM yyyy") + "',106))";
            }
            #endregion
            #region ApproveBy
            if (oFabricSalesContract.ApproveBy > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Isnull(ApproveBy,0)<>0";
            }
            #endregion

            #region
            if (oFabricSalesContract.ContractorID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in (" + oFabricSalesContract.ContractorID + "," + oFabricSalesContract.BuyerID + ")";
            }
            if (oFabricSalesContract.BuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in (" + oFabricSalesContract.ContractorID + "," + oFabricSalesContract.BuyerID + ")";
            }
            #endregion
            #region sFabricID
            if (!string.IsNullOrEmpty(oFabricSalesContract.Params))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricSalesContractID in (Select FabricSalesContractDetail.FabricSalesContractID from FabricSalesContractDetail where FabricID in (Select Fabric.FabricID from Fabric where FabricNo like '%" + oFabricSalesContract.Params + "%'))";
            }
            #endregion
            #region Export LC NO
            if (oFabricSalesContract.OrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType=" + oFabricSalesContract.OrderType;
            }
            #endregion

            List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(oFabricSalesContract.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oMarketingAccounts.Count > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            }

            string sSQL = sReturn1 + sReturn;
            _oFabricSalesContracts = FabricSalesContract.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContracts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsDetail(FabricSalesContractDetail oFabricSalesContractDetail)
        {

            List<FabricSalesContractDetail> _oFabricSalesContractDetail = new List<FabricSalesContractDetail>();
            string sReturn1 = "SELECT * FROM View_FabricSalesContractDetail ";
            string sReturn = "";

            #region ID
            if (oFabricSalesContractDetail.FabricSalesContractID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricSalesContractID=" + oFabricSalesContractDetail.FabricSalesContractID + "";
            }
            #endregion

            #region FabricNo
            if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricNo like'%" + oFabricSalesContractDetail.FabricNo + "%'";
            }
            #endregion
            #region ExportPIID
            if (oFabricSalesContractDetail.ExportPIID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricSalesContractDetailID  in (Select OrderSheetDetailID from ExportPIDetail where isnull(OrderSheetDetailID,0)>0 and ExportPIID=" + oFabricSalesContractDetail.ExportPIID + ")";
            }
            #endregion

            #region ExportPIID
            if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ErrorMessage))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricSalesContractDetailID in (" + oFabricSalesContractDetail.ErrorMessage + ")";
            }
            #endregion

            string sSQL = sReturn1 + sReturn;
            _oFabricSalesContractDetail = FabricSalesContractDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContractDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetsDetailForPI(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<FabricSCReport> _oFabricSCReport = new List<FabricSCReport>();
        
            string sReturn1 = "";
            string sReturn = "";
            if (oFabricSalesContractDetail.FabricSalesContractID > 0)
            {
                sReturn1 = "SELECT * FROM View_FabricSalesContractReport ";
                sReturn = "";
                #region ID
                if (oFabricSalesContractDetail.FabricSalesContractID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricSalesContractID=" + oFabricSalesContractDetail.FabricSalesContractID + "";
                }
                #endregion

                #region FabricNo
                if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricNo like'%" + oFabricSalesContractDetail.FabricNo + "%'";
                }
                #endregion

                #region ExportPIID
                //if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ErrorMessage))
                //{
                //    Global.TagSQL(ref sReturn, true);
                //    sReturn = sReturn + "FabricSalesContractDetailID in (" + oFabricSalesContractDetail.ErrorMessage + ")";
                //}
                //Global.TagSQL(ref sReturn);
                //sReturn = sReturn + "Qty>isnull(Qty_PI,0)";
                #endregion

                string sSQL = sReturn1 + sReturn;
                _oFabricSCReport = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ErrorMessage))
                {
                    oExportPIDetails = ExportPIDetail.Gets("Select * from view_ExportPIDetail as EPID where  isnull(OrderSheetDetailID,0)>0 and OrderSheetDetailID in (" + oFabricSalesContractDetail.ErrorMessage + ") and ExportPIID="+oFabricSalesContractDetail.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oExportPIDetails.Any() && oExportPIDetails.FirstOrDefault().FabricID > 0)
                    {
                       // _oFabricSCReport.ForEach(x => { x.Qty_PI = 0+ _oFSCDetails_PI.Where(p => p.FabricID == x.FabricID).Sum(o => o.Qty_PI); });
                        _oFabricSCReport.ForEach(x => { x.Qty_PI = x.Qty_PI - oExportPIDetails.Where(p => p.OrderSheetDetailID == x.FabricSalesContractDetailID).Sum(o => o.Qty); });
                    }
                }
            }
            else
            {
                List<Fabric> _oFabrics = new List<Fabric>();
                FabricSCReport oESCDetail = new FabricSCReport();
                sReturn1 = "SELECT * FROM View_Fabric";
                sReturn = "";

                #region FabricNo
                if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricNo like'%" + oFabricSalesContractDetail.FabricNo + "%'";
                }
                #endregion
                #region ExportPIID
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Isnull(ApprovedBy,0)<>0";
                #endregion

                string sSQL = sReturn1 + sReturn;
                _oFabrics = Fabric.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach(Fabric oItem in _oFabrics)
                {
                    oESCDetail = new FabricSCReport();
                    oESCDetail.FabricID = oItem.FabricID;
                    oESCDetail.FabricNo = oItem.FabricNo;
                    oESCDetail.FabricWeave = oItem.FabricWeave;

                    if (oItem.ConstructionPI == "" || oItem.ConstructionPI == null)
                    {
                        oESCDetail.Construction = oItem.Construction;
                    }
                    else
                    {
                        oESCDetail.Construction = oItem.ConstructionPI;
                    }
                    oESCDetail.FinishType = oItem.FinishType;
                    oESCDetail.BuyerReference = oItem.BuyerReference;
                    oESCDetail.BuyerName = oItem.BuyerName;
                    oESCDetail.ProductID = oItem.ProductID;
                    oESCDetail.ProductName = oItem.ProductName;
                    oESCDetail.ColorInfo = oItem.ColorInfo;
                    oESCDetail.FabricWeave = oItem.FabricWeave;
                    oESCDetail.FabricWeaveName = oItem.FabricWeaveName;
                    oESCDetail.FinishTypeName = oItem.FinishTypeName;
                    oESCDetail.ProcessType = oItem.ProcessType;
                    oESCDetail.ProcessTypeName = oItem.ProcessTypeName;
                    oESCDetail.MUName = "Y";
                    oESCDetail.StyleNo = oItem.StyleNo;
                    oESCDetail.FabricWidth = oItem.FabricWidth;
                    oESCDetail.FabricDesignName = oItem.FabricDesignName;
                    oESCDetail.ProcessTypeName = oItem.ProcessTypeName;
                    _oFabricSCReport.Add(oESCDetail);
                }
              
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSCReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsDetailByFSC(FabricSalesContract oFabricSalesContract)
        {
            List<FabricSalesContractDetail> _oFabricSalesContractDetail = new List<FabricSalesContractDetail>();

            _oFabricSalesContractDetail = FabricSalesContractDetail.Gets(oFabricSalesContract.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContractDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStatus(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            string sMsg = "";
            //string sReturn1 = "UPDATE FabricSalesContractDetail ";
            bool bIsWithMail = false;
            bIsWithMail = oFabricSalesContractDetail.IsWithMail;
            try
            {
                if (oFabricSalesContractDetail.FabricSalesContractDetailID <= 0 || oFabricSalesContractDetail.Status <= 0)
                    throw new Exception("Invalid Operation !!");
               
                //string sReturn = "SET [Status]=" + (int)oFabricSalesContractDetail.Status + " WHERE FabricSalesContractDetailID=" + oFabricSalesContractDetail.FabricSalesContractDetailID;
                //string sSQL = sReturn1 + sReturn;
                //sMsg = _oFabricSalesContract.UpdateBySQL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oFabricSalesContractDetail = oFabricSalesContractDetail.OrderHold(((User)Session[SessionInfo.CurrentUser]).UserID);

                if (string.IsNullOrEmpty(oFabricSalesContractDetail.ErrorMessage)) 
                {
                    //sMsg = Global.SuccessMessage;
                    if (bIsWithMail) 
                    {
                        sMsg = this.SendMail(oFabricSalesContractDetail);
                    }
                }
                else sMsg = "Failed To Update Order Status!" + oFabricSalesContractDetail.ErrorMessage;
            }
            catch (Exception ex) { sMsg = ex.Message; }
           
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMsg);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string SendMail(FabricSalesContractDetail oFabricSalesContractDetail) 
        {
            _oFabricSalesContract = new FabricSalesContract();
            MarketingAccount oMarketingAccount = new MarketingAccount();
            //List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();

            List<MailAssignedPerson> oMailAssignedPersons = new List<MailAssignedPerson>();
            if (oFabricSalesContractDetail.FabricSalesContractID > 0) 
            {
                try
                {
                    _oFabricSalesContract = _oFabricSalesContract.Get(oFabricSalesContractDetail.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oMarketingAccount = oMarketingAccount.Get(_oFabricSalesContract.MktAccountID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //oMarketingAccounts.Add(oMarketingAccount);

                   // oMailAssignedPersons = MailAssignedPerson.Gets("SELECT * FROM MailAssignedPerson WHERE MSID IN (SELECT MSID FROM MailSetUp WHERE IsActive=1 AND ReportID IN (SELECT ReportID FROM MailReporting  WHERE IsActive=1 AND ControllerName='FabricSalesContract' AND FunctionName='UpdateStatus'))", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oMailAssignedPersons = MailAssignedPerson.Gets("SELECT * FROM MailAssignedPerson WHERE MSID IN (4)", ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oMailAssignedPersons.Count>0)
                    {
                        string sSubject = _oFabricSalesContract.OrderTypeSt + (oFabricSalesContractDetail.Status == EnumPOState.Hold ? " ORDER HOLD" : " ORDER RUNNING") + " [Mkt ref:" + oFabricSalesContractDetail.FabricNo + ", Dispo No:" + oFabricSalesContractDetail.ExeNo + "]";
                        string sBodyInformation ="<h2>Mr. " + oMarketingAccount.Name + ",</h2>" + "<br> " + (oFabricSalesContractDetail.Status == EnumPOState.Hold ? " Order is held for : " : " Order is run for:");
                        sBodyInformation += "<br>Order No    :" + _oFabricSalesContract.SCNoFull;
                        sBodyInformation += "<br>Buyer Name  :" + _oFabricSalesContract.BuyerName;
                        sBodyInformation += "<br>Mkt Ref     :" + oFabricSalesContractDetail.FabricNo;
                      if(!string.IsNullOrEmpty(oFabricSalesContractDetail.ExeNo)) { sBodyInformation += "<br>Dispo Number:" + oFabricSalesContractDetail.ExeNo;}
                        sBodyInformation += "<br><br><div style='float:right; Font-size:11px;'> " + (oFabricSalesContractDetail.Status == EnumPOState.Hold ? "Hold at :" : "Running at :") + DateTime.Now.ToString("dd MMM yyyy hh:mm") + "</div>";
                        sBodyInformation += "<div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> " + ((User)Session[SessionInfo.CurrentUser]).UserName + "</div> <div style='padding-top:5px;'>" + "" + "</div>";

                        List<string> sMailTos = oMailAssignedPersons.Where(x => x.IsCCMail == false).Select(x => x.MailTo).ToList();
                        List<string> sMailCCs = oMailAssignedPersons.Where(x => x.IsCCMail == true).Select(x => x.MailTo).ToList();
                        if (!string.IsNullOrEmpty(oMarketingAccount.Email)) { sMailCCs.Add(oMarketingAccount.Email); }
                        if (!string.IsNullOrEmpty(((User)Session[SessionInfo.CurrentUser]).EmailAddress)) { sMailCCs.Add(((User)Session[SessionInfo.CurrentUser]).EmailAddress); }

                        #region Email Credential
                        EmailConfig oEmailConfig = new EmailConfig();
                        oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                        #endregion

                        Global.MailSend(sSubject, sBodyInformation, sMailTos, sMailCCs, new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
                    }
                    else throw new Exception("Failed To Get Mail Setup!");
                }
                catch (Exception ex) 
                {
                    return "Status is updated but failed to send mail. Because of " + ex.Message + "!";
                }
            }
            return Global.SuccessMessage;
        }

        #region Print
        public Image GetContractorImage(AttachDocument oAttachDocument)
        {
            if (oAttachDocument.AttachFile != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oAttachDocument.AttachFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public ActionResult PrintPreview(int nID, bool bIsRnd, double nts, bool bPrintFormat, int nTitleTypeInImg)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            List<FabricSCHistory> oFabricSCHistorys = new List<FabricSCHistory>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
            _oFabricSalesContract = _oFabricSalesContract.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricSalesContract.FabricSalesContractDetails = FabricSalesContractDetail.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricDeliverySchedule> _oFabricDeliverySchedules = FabricDeliverySchedule.GetsFSCID(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricSalesContractNote> oFabricSalesContractNotes = FabricSalesContractNote.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportPIs = ExportPI.Gets("Select * from view_ExportPI where ExportPIID in (Select ExportPIID from ExportPIDetail where isnull(OrderSheetDetailID,0)>0 and OrderSheetDetailID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractID="+_oFabricSalesContract.FabricSalesContractID+") )",((User)Session[SessionInfo.CurrentUser]).UserID);
            
            //_oFabricSalesContract.PINo = this.GetPINos(_oFabricSalesContract.FabricSalesContractDetails);
            oFabricSCHistorys = FabricSCHistory.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            AttachDocument oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oFabricSalesContract.ContractorID, (int)Session[SessionInfo.currentUserID]);
            _oFabricSalesContract.ConImage = this.GetContractorImage(oAttachDocument);

            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);

            FabricOrderSetup oFabricOrderSetup = new FabricOrderSetup();
            oFabricOrderSetup = oFabricOrderSetup.GetByType(_oFabricSalesContract.OrderType,((User)Session[SessionInfo.CurrentUser]).UserID);

            #endregion
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(oBusinessUnit.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            _oFabricSalesContract.FabricSalesContractDetails.ForEach(o => o.MUName ="Y");
            if (!bPrintFormat)
            {
                _oFabricSalesContract.FabricSalesContractDetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));
                _oFabricSalesContract.FabricSalesContractDetails.ForEach(o => o.UnitPrice = (o.UnitPrice / oMeasurementUnitCon.Value));
                _oFabricSalesContract.FabricSalesContractDetails.ForEach(o => o.MUName = ( oMeasurementUnitCon.ToMUnit));
                _oFabricDeliverySchedules.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));
            }

            Employee oEmployee = new Employee();
            if (_oFabricSalesContract.PreapeBy > 0)
            {
                oAttachDocument = oAttachDocument.GetUserSignature(_oFabricSalesContract.PreapeBy, (int)Session[SessionInfo.currentUserID]);
                _oFabricSalesContract.PreparedBySignature = this.GetContractorImage(oAttachDocument);
                oEmployee = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeID = (SELECT EmployeeID FROM Users WHERE UserID=" + _oFabricSalesContract.PreapeBy + ")", (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                if (oEmployee != null) _oFabricSalesContract.PreapeByDesignation = oEmployee.DesignationName;
            }
            if (_oFabricSalesContract.ApproveBy > 0)
            {
                oAttachDocument = oAttachDocument.GetUserSignature(_oFabricSalesContract.ApproveBy, (int)Session[SessionInfo.currentUserID]);
                _oFabricSalesContract.ApprovedBySignature = this.GetContractorImage(oAttachDocument);
                oEmployee = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeID = (SELECT EmployeeID FROM Users WHERE UserID=" + _oFabricSalesContract.ApproveBy + ")", (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                if (oEmployee != null) _oFabricSalesContract.ApproveByDesignation = oEmployee.DesignationName;
            }
            if (_oFabricSalesContract.CheckedBy > 0)
            {
                oAttachDocument = oAttachDocument.GetUserSignature(_oFabricSalesContract.CheckedBy, (int)Session[SessionInfo.currentUserID]);
                _oFabricSalesContract.CheckBySignature = this.GetContractorImage(oAttachDocument);
                oEmployee = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeID = (SELECT EmployeeID FROM Users WHERE UserID=" + _oFabricSalesContract.CheckedBy + ")", (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                if (oEmployee != null) _oFabricSalesContract.CheckByDesignation = oEmployee.DesignationName;
            }
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Local_Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Local_Sample)
            {
                if (oFabricOrderSetup.PrintNo == EnumExcellColumn.A)
                {
                    rptFabricSalesContract oReport = new rptFabricSalesContract();
                    byte[] abytes = oReport.PrepareReport(_oFabricSalesContract, oCompany, oBusinessUnit, _oFabricDeliverySchedules, oFabricSalesContractNotes, oFabricSCHistorys, bIsRnd, false);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptFabricSalesContract oReport = new rptFabricSalesContract();
                    byte[] abytes = oReport.PrepareReport_Local(_oFabricSalesContract, oCompany, oBusinessUnit, _oFabricDeliverySchedules, oFabricSalesContractNotes, oFabricSCHistorys, oFabricOrderSetup, bIsRnd, false);
                    return File(abytes, "application/pdf");
                }
            }
            else if (oFabricPOSetup.PrintNo == (int)EnumExcellColumn.A)
            {
                rptFabricSalesContract oReport = new rptFabricSalesContract();
                byte[] abytes = oReport.PrepareReport(_oFabricSalesContract, oCompany, oBusinessUnit, _oFabricDeliverySchedules, oFabricSalesContractNotes, oFabricSCHistorys, bIsRnd,false);
                return File(abytes, "application/pdf");
            }
            else if (oFabricPOSetup.PrintNo == (int)EnumExcellColumn.B)
            {
                _oFabricSalesContract.FabricSalesContractDetails = _oFabricSalesContract.FabricSalesContractDetails.Where(o => o.Status != EnumPOState.Hold).ToList();
                rptFabricSalesContract oReport = new rptFabricSalesContract();
                byte[] abytes = oReport.PrepareReport_B(_oFabricSalesContract, oCompany, oBusinessUnit, _oFabricDeliverySchedules, oFabricSalesContractNotes, oFabricSCHistorys, oFabricOrderSetup, bIsRnd, false, oExportPIs);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptFabricSalesContract oReport = new rptFabricSalesContract();
                byte[] abytes = oReport.PrepareReport(_oFabricSalesContract, oCompany, oBusinessUnit, _oFabricDeliverySchedules, oFabricSalesContractNotes, oFabricSCHistorys, bIsRnd, false);
                return File(abytes, "application/pdf");
            }

        }
        public ActionResult PrintPreviewSC(int nID, double nts)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oFabricSalesContract = _oFabricSalesContract.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricSalesContract.FabricSalesContractDetails = FabricSalesContractDetail.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricDeliverySchedule> _oFabricDeliverySchedules = FabricDeliverySchedule.GetsFSCID(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricSalesContractNote> oFabricSalesContractNotes = FabricSalesContractNote.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricSalesContract.PINo = this.GetPINos(_oFabricSalesContract.FabricSalesContractDetails);

            AttachDocument oAttachDocument = new AttachDocument();
            if (_oFabricSalesContract.ContractorID > 0)
            {                
                oAttachDocument = oAttachDocument.GetUserSignature(_oFabricSalesContract.ContractorID, (int)Session[SessionInfo.currentUserID]);
                _oFabricSalesContract.ConImage = this.GetContractorImage(oAttachDocument);
            }
            Employee oEmployee = new Employee();
            if(_oFabricSalesContract.PreapeBy > 0)
            {
                oAttachDocument = oAttachDocument.GetUserSignature(_oFabricSalesContract.PreapeBy, (int)Session[SessionInfo.currentUserID]);
                _oFabricSalesContract.PreparedBySignature = this.GetContractorImage(oAttachDocument);
                oEmployee = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeID = (SELECT EmployeeID FROM Users WHERE UserID=" + _oFabricSalesContract.PreapeBy + ")", (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                if (oEmployee != null) _oFabricSalesContract.PreapeByDesignation = oEmployee.DesignationName;
            }
            if (_oFabricSalesContract.ApproveBy > 0)
            {
                oAttachDocument = oAttachDocument.GetUserSignature(_oFabricSalesContract.ApproveBy, (int)Session[SessionInfo.currentUserID]);
                _oFabricSalesContract.ApprovedBySignature = this.GetContractorImage(oAttachDocument);
                oEmployee = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeID = (SELECT EmployeeID FROM Users WHERE UserID=" + _oFabricSalesContract.ApproveBy + ")", (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                if (oEmployee != null) _oFabricSalesContract.ApproveByDesignation = oEmployee.DesignationName;
            }
            #endregion
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string _sMessage = "";
            rptFabricSalesContract oReport = new rptFabricSalesContract();
            byte[] abytes = oReport.PrepareReport_SalesContract(_oFabricSalesContract, oCompany, oBusinessUnit, _oFabricDeliverySchedules, oFabricSalesContractNotes);
            return File(abytes, "application/pdf");

        }
        #endregion

        #region
        public ActionResult PrintPreviewLog(int nID, bool bIsRnd, double nts, bool bIsLog)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            List<FabricSCHistory> oFabricSCHistorys = new List<FabricSCHistory>();
            _oFabricSalesContract = _oFabricSalesContract.GetByLog(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oFabricSalesContract.FabricSalesContractDetails = FabricSalesContractDetail.GetsLog(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricDeliverySchedule> _oFabricDeliverySchedules = FabricDeliverySchedule.GetsFSCIDLog(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricSalesContractNote> oFabricSalesContractNotes = FabricSalesContractNote.GetsLog(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //oFabricSCHistorys = FabricSCHistory.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            AttachDocument oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oFabricSalesContract.ContractorID, (int)Session[SessionInfo.currentUserID]);
            _oFabricSalesContract.ConImage = this.GetContractorImage(oAttachDocument);

            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);

            FabricOrderSetup oFabricOrderSetup = new FabricOrderSetup();
            oFabricOrderSetup = oFabricOrderSetup.GetByType(_oFabricSalesContract.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #endregion
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);


            if (oFabricPOSetup.PrintNo == (int)EnumExcellColumn.A)
            {

                rptFabricSalesContract oReport = new rptFabricSalesContract();
                byte[] abytes = oReport.PrepareReport(_oFabricSalesContract, oCompany, oBusinessUnit, _oFabricDeliverySchedules, oFabricSalesContractNotes, oFabricSCHistorys, bIsRnd, bIsLog);
                return File(abytes, "application/pdf");
            }
            else if (oFabricPOSetup.PrintNo == (int)EnumExcellColumn.B)
            {
                List<ExportPI> oExportPIs = new List<ExportPI>();
                rptFabricSalesContract oReport = new rptFabricSalesContract();
                byte[] abytes = oReport.PrepareReport_B(_oFabricSalesContract, oCompany, oBusinessUnit, _oFabricDeliverySchedules, oFabricSalesContractNotes, oFabricSCHistorys, oFabricOrderSetup, bIsRnd, bIsLog, oExportPIs);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptFabricSalesContract oReport = new rptFabricSalesContract();
                byte[] abytes = oReport.PrepareReport(_oFabricSalesContract, oCompany, oBusinessUnit, _oFabricDeliverySchedules, oFabricSalesContractNotes, oFabricSCHistorys, bIsRnd, bIsLog);
                return File(abytes, "application/pdf");
            }

        }
        #endregion

        #region
        private string GetSQL2(string sTemp)
        {
            //string sReturn1 = "SELECT * FROM View_FabricSalesContract ";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sTemp))
            {
                #region Set Values

                string sExportPINo = Convert.ToString(sTemp.Split('~')[0]);
                string sFabricID = Convert.ToString(sTemp.Split('~')[1]);
                string sContractorIDs = Convert.ToString(sTemp.Split('~')[2]);
                string sBuyerIDs = Convert.ToString(sTemp.Split('~')[3]);
                string sMAccountIDs = Convert.ToString(sTemp.Split('~')[4]);


                int nCboSCDate = Convert.ToInt32(sTemp.Split('~')[5]);
                DateTime dFromSCDate = DateTime.Now;
                DateTime dToSCDate = DateTime.Now;
                if (nCboSCDate > 0)
                {
                    dFromSCDate = Convert.ToDateTime(sTemp.Split('~')[6]);
                    dToSCDate = Convert.ToDateTime(sTemp.Split('~')[7]);
                }

                int ncboApproveDate = Convert.ToInt32(sTemp.Split('~')[8]);
                DateTime dFromApproveDate = DateTime.Now;
                DateTime dToApproveDate = DateTime.Now;
                if (ncboApproveDate > 0)
                {
                    dFromApproveDate = Convert.ToDateTime(sTemp.Split('~')[9]);
                    dToApproveDate = Convert.ToDateTime(sTemp.Split('~')[10]);
                }

                int ncboProductionType = Convert.ToInt32(sTemp.Split('~')[11]);
                string sOrderTypeIDs = Convert.ToString(sTemp.Split('~')[12]);
                string sCurrentStatus = Convert.ToString(sTemp.Split('~')[13]);
                int nIsPrinting = Convert.ToInt32(sTemp.Split('~')[14]);
                string sMKTGroupIDs = Convert.ToString(sTemp.Split('~')[15]);
                int nBUID = Convert.ToInt32(sTemp.Split('~')[16]);

                #endregion

                #region Make Query

                #region Export PI NO
                if (!string.IsNullOrEmpty(sExportPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricSalesContractID in (Select FabricSalesContractDetail.FabricSalesContractID from FabricSalesContractDetail where ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINo like '%" + sExportPINo + "%'))";
                }
                #endregion

                #region sFabricID
                if (!string.IsNullOrEmpty(sFabricID))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "Select * from FabricSalesContract where FabricSalesContractID in (Select FabricSalesContractDetail.FabricSalesContractID from FabricSalesContractDetail where FabricID in (Select Fabric.FabricID from Fabric where FabricNo like '%" + sFabricID + "%'))";
                }
                #endregion

                #region ContractorID
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID in( " + sContractorIDs + ") ";
                }
                #endregion

                #region BBuyerID
                if (!String.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in( " + sBuyerIDs + ") ";
                }
                #endregion

                #region sMAccountIDs
                if (!String.IsNullOrEmpty(sMAccountIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID in(" + sMAccountIDs + ") ";
                }
                #endregion

                #region sMKTGroupIDs
                if (!String.IsNullOrEmpty(sMKTGroupIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID in(Select MarketingAccountID  from View_MarketingAccount where GroupID >0 and GroupID in (" + sMKTGroupIDs + ")) ";
                }
                #endregion

                #region F SC Date
                if (nCboSCDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboSCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                #region Approval Date
                if (ncboApproveDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboApproveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                #region ncboProductionType
                if (ncboProductionType > 0)
                {

                    if (ncboProductionType == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsInHouse =1";
                    }
                    else if (ncboProductionType == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsInHouse =0";
                    }

                }
                #endregion
                #region nIsPrinting
                if (nIsPrinting > 0)
                {
                    if (nIsPrinting == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsPrint =1";
                    }
                    else if (nIsPrinting == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "IsPrint =0";
                    }
                }
                #endregion

                #region OrderType
                if (!string.IsNullOrEmpty(sOrderTypeIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderType IN (" + sOrderTypeIDs + ") ";
                }
                #endregion
                #region CurrentStatus
                if (!string.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Currentstatus IN (" + sCurrentStatus + ") ";
                }
                #endregion

                List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(nBUID, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                }
                #endregion

            }
            sReturn = sReturn + " order by SCDate DESC";
            return sReturn;
        }
        public void Print_ReportXL(string sTempString)
        {
            string sSQL = "";
            _oFabricSalesContracts = new List<FabricSalesContract>();
            List<FabricSalesContract> oTempFabricSalesContracts = new List<FabricSalesContract>();
            if (!String.IsNullOrEmpty(sTempString))
            {
                string SQL = "SELECT * ,(SELECT TOP 1 ELC.ExportLCNo FROM ExportLC AS ELC WHERE ELC.ExportLCID IN ( SELECT EPILCM.ExportLCID FROM ExportPILCMapping AS EPILCM WHERE EPILCM.ExportPIID IN (SELECT EPID.ExportPIID FROM ExportPIDetail AS EPID WHERE EPID.OrderSheetDetailID IN (SELECT FSCD.FabricSalesContractDetailID FROM FabricSalesContractDetail AS FSCD WHERE FSCD.FabricSalesContractID = FSC.FabricSalesContractID)))) AS LCNo, (SELECT TOP 1 ELC.OpeningDate FROM ExportLC AS ELC WHERE ELC.ExportLCID IN ( SELECT EPILCM.ExportLCID FROM ExportPILCMapping AS EPILCM WHERE EPILCM.ExportPIID IN (SELECT EPID.ExportPIID FROM ExportPIDetail AS EPID WHERE EPID.OrderSheetDetailID IN (SELECT FSCD.FabricSalesContractDetailID FROM FabricSalesContractDetail AS FSCD WHERE FSCD.FabricSalesContractID = FSC.FabricSalesContractID)))) AS LCDate FROM View_FabricSalesContract AS FSC ";
                //sSQL = GetSQL(sTempString);
                sSQL = GetSQL2(sTempString);
                _oFabricSalesContracts = FabricSalesContract.Gets(SQL + sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                string SQL = "SELECT * ,(SELECT TOP 1 ELC.ExportLCNo FROM ExportLC AS ELC WHERE ELC.ExportLCID IN ( SELECT EPILCM.ExportLCID FROM ExportPILCMapping AS EPILCM WHERE EPILCM.ExportPIID IN (SELECT EPID.ExportPIID FROM ExportPIDetail AS EPID WHERE EPID.OrderSheetDetailID IN (SELECT FSCD.FabricSalesContractDetailID FROM FabricSalesContractDetail AS FSCD WHERE FSCD.FabricSalesContractID = FSC.FabricSalesContractID)))) AS LCNo, (SELECT TOP 1 ELC.OpeningDate FROM ExportLC AS ELC WHERE ELC.ExportLCID IN ( SELECT EPILCM.ExportLCID FROM ExportPILCMapping AS EPILCM WHERE EPILCM.ExportPIID IN (SELECT EPID.ExportPIID FROM ExportPIDetail AS EPID WHERE EPID.OrderSheetDetailID IN (SELECT FSCD.FabricSalesContractDetailID FROM FabricSalesContractDetail AS FSCD WHERE FSCD.FabricSalesContractID = FSC.FabricSalesContractID)))) AS LCDate FROM View_FabricSalesContract AS FSC ORDER BY FSC.SCDate DESC";
                _oFabricSalesContracts = FabricSalesContract.Gets(SQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            //oTempFabricSalesContracts = FabricSalesContract.Gets("SELECT TOP 1 ELC.ExportLCNo AS LCNo, ELC.OpeningDate AS LCDate FROM ExportLC AS ELC WHERE ELC.ExportLCID IN ( SELECT EPILCM.ExportLCID FROM ExportPILCMapping AS EPILCM WHERE EPILCM.ExportPIID IN (SELECT EPID.ExportPIID FROM ExportPIDetail AS EPID WHERE EPID.OrderSheetDetailID IN (SELECT FSCD.FabricSalesContractDetailID FROM FabricSalesContractDetail AS FSCD WHERE FSCD.FabricSalesContractID IN ( " + string.Join(",", _oFabricSalesContracts.Select(x => x.FabricSalesContractID)) + "))))", ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nQty = 0;
            double nAmount = 0;



            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 15;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            int nCount = 0;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PO Report");
                sheet.Name = "PO Report";
                sheet.Column(1).Width = 8;
                sheet.Column(2).Width = 12;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 15;
                sheet.Column(5).Width = 15;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 25;
                sheet.Column(9).Width = 25;
                sheet.Column(10).Width = 25;
                sheet.Column(11).Width = 15;
                sheet.Column(12).Width = 15;
                sheet.Column(13).Width = 20;
                sheet.Column(14).Width = 20;
                sheet.Column(15).Width = 15;


                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "PO Report "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region
                nCount++;
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "PO No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nCount++;
                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                nCount++;
                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Buying House"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Concern Person "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Approve By"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Attachment Count"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

             

                nRowIndex++;
                #endregion
                

                #region Data
                foreach (FabricSalesContract oItem in _oFabricSalesContracts)
                {

                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.SCNoFull; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.SCDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.LCDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.OrderTypeSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = (oItem.MKTPName); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value =Math.Round(oItem.Qty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Math.Round(oItem.Amount); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.ApproveByName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.CurrentStatusSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.AttCount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nQty = nQty + oItem.Qty;
                    nAmount = nAmount + oItem.Amount;
                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=POReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        #endregion
    }
}

