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
using ESimSolFinancial.Controllers;

namespace ESimSolFinancial.Controllers
{

    public class MasterLCV2Controller : Controller
    {
        #region Declartion
        MasterLC _oMasterLC = new MasterLC();
        List<MasterLC> _oMasterLCs = new List<MasterLC>();
        MasterLCDetail _oMasterLCDetail = new MasterLCDetail();
        List<MasterLCDetail> _oMasterLCDetails = new List<MasterLCDetail>();
        MasterLCTermsAndCondition _oMasterLCTermsAndCondition = new MasterLCTermsAndCondition();
        List<MasterLCTermsAndCondition> _oMasterLCTermsAndConditions = new List<MasterLCTermsAndCondition>();
        //CommercialTermsAndCondition _oCommercialTermsAndCondition = new CommercialTermsAndCondition();
        //List<CommercialTermsAndCondition> _oCommercialTermsAndConditions = new List<CommercialTermsAndCondition>();
        ProformaInvoice _oProformaInvoice = new ProformaInvoice();
        List<ProformaInvoice> _oProformaInvoices = new List<ProformaInvoice>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        MasterLCSummery _oMasterLCSummery = new MasterLCSummery();
        List<MasterLCSummery> _oMasterLCSummeries = new List<MasterLCSummery>();
        ReviseRequest _oReviseRequest = new ReviseRequest();
        MasterLCMapping _oMasterLCMapping = new MasterLCMapping();
        #endregion

        #region function
        private bool HaveRateViewPermission(EnumRoleOperationType OperationType)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.MasterLC).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            if ((int)Session[SessionInfo.currentUserID] == -9)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < oAuthorizationRoleMappings.Count; i++)
                {
                    if (oAuthorizationRoleMappings[i].OperationType == OperationType && oAuthorizationRoleMappings[i].ModuleNameST == "MasterLC")
                    {
                        return true;

                    }

                }
            }

            return false;
        }
        #endregion

        #region Master LC Issue and Management
        public ActionResult ViewMasterLCIssue(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.MasterLC).ToString() + ',' + ((int)EnumModuleName.B2BLCAllocation).ToString() + ',' + ((int)EnumModuleName.LCTransfer).ToString() + ',' + ((int)EnumModuleName.CommercialInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oMasterLCs = new List<MasterLC>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            //_oMasterLCs = MasterLC.Gets(buid,(int)Session[SessionInfo.currentUserID]);
            ViewBag.BussinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            ViewBag.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.CommercialInvoiceFormat, (int)Session[SessionInfo.currentUserID]);
            return View(_oMasterLCs);
        }
        #endregion

        #region Add, Edit, Delete

        #region Master LC
        public ActionResult ViewMasterLC(int id, int buid)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LCTransfer).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oMasterLC = new MasterLC();
            if (id > 0)
            {
                _oMasterLC = _oMasterLC.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oMasterLC.MasterLCDetails = MasterLCDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oMasterLC.MasterLCTermsAndConditions = MasterLCTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
                //_oMasterLC.LCTransfers = LCTransfer.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oMasterLC.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            _oMasterLC.BusinessUnits.Add(oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]));
            _oMasterLC.BankAccounts = BankAccount.GetsByDeptAndBU(((int)EnumOperationalDept.Accounts).ToString(), buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTypes = EnumObject.jGets(typeof(EnumLCType));
            ViewBag.PartialShipmentAllows = EnumObject.jGets(typeof(EnumPartialShipmentAllow));
            ViewBag.Transferables = EnumObject.jGets(typeof(EnumTransferable));
            ViewBag.DeferredFroms = EnumObject.jGets(typeof(EnumDefferedFrom));

            return View(_oMasterLC);
        }

        #endregion

        #region MasterLC With Order
        public ActionResult ViewMasterLCWithOrder(int id, int buid)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LCTransfer).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oMasterLC = new MasterLC();
            if (id > 0)
            {
                _oMasterLC = _oMasterLC.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oMasterLC.MasterLCDetails = MasterLCDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oMasterLC.MasterLCTermsAndConditions = MasterLCTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
                //_oMasterLC.LCTransfers = LCTransfer.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oMasterLC.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            _oMasterLC.BusinessUnits.Add(oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]));
            _oMasterLC.BankAccounts = BankAccount.GetsByDeptAndBU(((int)EnumOperationalDept.Accounts).ToString(), buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.PaymentTerms = EnumObject.jGets(typeof(EnumPaymentTerm));
            ViewBag.DeliveryTerms = EnumObject.jGets(typeof(EnumDeliveryTerm));
            ViewBag.MeasurementUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);

            return View(_oMasterLC);
        }

        [HttpPost]
        public JsonResult SaveMLCDetailByOrderTrack(MasterLCDetail oMasterLCDetail)
        {
            _oMasterLCDetail = new MasterLCDetail();
            try
            {
                _oMasterLCDetail = oMasterLCDetail.SaveMLCDetailByOrderTrack((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLCDetail = new MasterLCDetail();
                _oMasterLCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AcceptPIReviseWithMLCDetail(MasterLCDetail oMasterLCDetail)
        {
            _oMasterLCDetail = new MasterLCDetail();
            try
            {
                List<ProformaInvoiceDetail> oProformaInvoiceDetails = new List<ProformaInvoiceDetail>();
                oMasterLCDetail.ProformaInvoice.ProformaInvoiceActionType = EnumProformaInvoiceActionType.RequestForRevise;
                oMasterLCDetail.ProformaInvoice.ReviseRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                oMasterLCDetail.ProformaInvoice.ReviseRequest.OperationType = EnumReviseRequestOperationType.ProformaInvoice;
                oMasterLCDetail.ProformaInvoice.Note = oMasterLCDetail.ProformaInvoice.Note;
                oMasterLCDetail.ProformaInvoice.OperationBy = oMasterLCDetail.ProformaInvoice.OperationBy;
                oProformaInvoiceDetails = oMasterLCDetail.ProformaInvoice.ProformaInvoiceDetails;

                oMasterLCDetail.ProformaInvoice = oMasterLCDetail.ProformaInvoice.ChangeStatus((int)Session[SessionInfo.currentUserID]);
                oMasterLCDetail.ProformaInvoice.ProformaInvoiceDetails = oProformaInvoiceDetails;
                _oMasterLCDetail = oMasterLCDetail.AcceptPIReviseWithMLCDetail((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLCDetail = new MasterLCDetail();
                _oMasterLCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getPIWithPIDetails(MasterLCDetail oMasterLCDetail)
        {
            ProformaInvoice oProformaInvoice = new ProformaInvoice();
            try
            {
                oProformaInvoice = oProformaInvoice.Get(oMasterLCDetail.ProformaInvoiceID, (int)Session[SessionInfo.currentUserID]);
                oProformaInvoice.ProformaInvoiceDetails = ProformaInvoiceDetail.Gets(oMasterLCDetail.ProformaInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProformaInvoice = new ProformaInvoice();
                oProformaInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProformaInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMLCDeatil(MasterLCDetail oMasterLCDetail)
        {
            string smessage = "";
            try
            {
                smessage = oMasterLCDetail.DeleteMLCDeatil(oMasterLCDetail.MasterLCDetailID, oMasterLCDetail.ProformaInvoiceID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        public ActionResult ViewTabPractice()
        {
            return View(_oMasterLC);
        }

        #region Master LC Amendment
        public ActionResult ViewMasterLCAmendment(int id, int buid)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LCTransfer).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oMasterLC = new MasterLC();
            if (id > 0)
            {
                _oMasterLC = _oMasterLC.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oMasterLC.MasterLCDetails = MasterLCDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oMasterLC.MasterLCTermsAndConditions = MasterLCTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
                //_oMasterLC.LCTransfers = LCTransfer.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oMasterLC.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            _oMasterLC.BusinessUnits.Add(oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]));
            _oMasterLC.BankAccounts = BankAccount.GetsByDeptAndBU(((int)EnumOperationalDept.Accounts).ToString(), buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTypes = EnumObject.jGets(typeof(EnumLCType));
            ViewBag.PartialShipmentAllows = EnumObject.jGets(typeof(EnumPartialShipmentAllow));
            ViewBag.Transferables = EnumObject.jGets(typeof(EnumTransferable));
            ViewBag.DeferredFroms = EnumObject.jGets(typeof(EnumDefferedFrom));
            return View(_oMasterLC);
        }
        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(MasterLC oMasterLC)
        {
            _oMasterLC = new MasterLC();
            try
            {
                oMasterLC.LCStatus = (EnumLCStatus)oMasterLC.LCStatusInInt;
                oMasterLC.DeferredFrom = (EnumDefferedFrom)oMasterLC.DeferredFromInInt;
                oMasterLC.PartialShipmentAllow = (EnumPartialShipmentAllow)oMasterLC.PartialShipmentAllowInInt;
                oMasterLC.LCType = (EnumLCType)oMasterLC.LCTypeInInt;
                oMasterLC.Transferable = (EnumTransferable)oMasterLC.TransferableInInt;
                foreach (MasterLCDetail oItem in oMasterLC.MasterLCDetails)
                {
                    oItem.PIStatus = (EnumPIStatus)oItem.PIStatusInInt;
                }
                _oMasterLC = oMasterLC.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLC = new MasterLC();
                _oMasterLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP Accept Master LC Ammendment
        [HttpPost]
        public JsonResult AcceptMasterLCAmendment(MasterLC oMasterLC)
        {
            _oMasterLC = new MasterLC();

            try
            {
                oMasterLC.LCStatus = (EnumLCStatus)oMasterLC.LCStatusInInt;
                oMasterLC.DeferredFrom = (EnumDefferedFrom)oMasterLC.DeferredFromInInt;
                oMasterLC.PartialShipmentAllow = (EnumPartialShipmentAllow)oMasterLC.PartialShipmentAllowInInt;
                oMasterLC.LCType = (EnumLCType)oMasterLC.LCTypeInInt;
                oMasterLC.Transferable = (EnumTransferable)oMasterLC.TransferableInInt;
                foreach (MasterLCDetail oItem in oMasterLC.MasterLCDetails)
                {
                    oItem.PIStatus = (EnumPIStatus)oItem.PIStatusInInt;
                }
                _oMasterLC = oMasterLC.AcceptMasterLCAmmendment((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLC = new MasterLC();
                _oMasterLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP GET Delete
        [HttpPost]
        public JsonResult Delete(MasterLC oMasterLC)
        {
            string smessage = "";
            try
            {
                smessage = oMasterLC.Delete(oMasterLC.MasterLCID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region Master LC Mapping Use for Exportlc part
        [HttpPost]
        public JsonResult SaveMasterLCMapping(MasterLCMapping oMasterLCMapping)
        {
            _oMasterLCMapping = new MasterLCMapping();
            try
            {
                _oMasterLCMapping = oMasterLCMapping;
                _oMasterLCMapping = _oMasterLCMapping.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMasterLCMapping = new MasterLCMapping();
                _oMasterLCMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCMapping);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveWithMasterLC(MasterLCMapping oMasterLCMapping)
        {
            _oMasterLCMapping = new MasterLCMapping();
            try
            {
                _oMasterLCMapping = oMasterLCMapping;
                _oMasterLCMapping = _oMasterLCMapping.SaveWithMasterLC(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMasterLCMapping = new MasterLCMapping();
                _oMasterLCMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCMapping);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsMasterLC(MasterLC oMasterLC)
        {
            _oMasterLCs = new List<MasterLC>();
            try
            {


                string sSQL = "";

                if (oMasterLC.MasterLCNo == "" || oMasterLC.MasterLCNo == null)
                {
                    sSQL = sSQL + "Select * from MasterLC where MasterlcID not in (Select MasterLCMapping.MasterLCID from MasterLCMapping) or MasterLCID in (Select MasterLCMapping.MasterLCID from MasterLCMapping where ContractorID=" + oMasterLC.ContractorID + " ) order by MasterLCNo";
                }
                else
                {
                    oMasterLC.MasterLCNo = oMasterLC.MasterLCNo.Trim();
                    sSQL = sSQL + "Select * from MasterLC where MasterLCNo Like '%" + oMasterLC.MasterLCNo + "%' order by MasterLCNo";
                }
                _oMasterLCs = MasterLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oMasterLCs.Count <= 0) { throw new Exception("No information found."); }

            }
            catch (Exception ex)
            {
                oMasterLC = new MasterLC();
                oMasterLC.ErrorMessage = ex.Message;
                _oMasterLCs.Add(oMasterLC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteMasterLCMapping(MasterLCMapping oMasterLCMapping)
        {
            string sFeedBackMessage = "";
            oMasterLCMapping.MasterLCDate = DateTime.Now;
            oMasterLCMapping.MasterLCNo = "";
            oMasterLCMapping.ErrorMessage = "";
            oMasterLCMapping.ContractorID = 0;
            if (oMasterLCMapping.MasterLCID <= 0)
            {
                sFeedBackMessage = "In valid Master LC";
            }
            try
            {
                if (sFeedBackMessage.Length <= 0)
                {
                    sFeedBackMessage = oMasterLCMapping.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        #endregion

        #region Master LC Hitory Picker

        [HttpPost]
        public JsonResult GetMasterLCHistory(MasterLC oMasterLC)
        {
            List<MasterLCHistory> oMasterLCHistorys = new List<MasterLCHistory>();

            try
            {
                oMasterLCHistorys = ESimSol.BusinessObjects.MasterLCHistory.Gets(oMasterLC.MasterLCID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                MasterLCHistory oMasterLCHistory = new ESimSol.BusinessObjects.MasterLCHistory();
                oMasterLCHistory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMasterLCHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP GET PI
        [HttpGet]
        public JsonResult GetPI(int BuyerID)
        {

            _oProformaInvoices = new List<ProformaInvoice>();
            string sSQL = "SELECT * FROM View_ProformaInvoice WHERE ApprovedBy!=0  AND BuyerID =" + BuyerID + " AND ProformaInvoiceID NOT IN (SELECT ProformaInvoiceID FROM MasterLCDetail WHERE MasterLCID IN (SELECT MasterLCID FROM MasterLC WHERE ApprovedBy!=0 AND Applicant = " + BuyerID + "))";
            try
            {
                _oProformaInvoices = ProformaInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProformaInvoice = new ProformaInvoice();
                _oProformaInvoice.ErrorMessage = ex.Message;
                _oProformaInvoices.Add(_oProformaInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProformaInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Wait for Revise
        [HttpGet]
        public JsonResult WaitForRevise(double ts)
        {
            string sSQL = "SELECT * FROM View_MasterLC Where LCStatus = " + (int)EnumLCStatus.Req_for_Ammendment;
            try
            {
                _oMasterLCs = new List<MasterLC>();
                _oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLC = new MasterLC();
                _oMasterLC.ErrorMessage = ex.Message;
                _oMasterLCs.Add(_oMasterLC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Waiting Search
        [HttpGet]
        public JsonResult WaitingSearch()
        {
            _oMasterLCs = new List<MasterLC>();
            string sSQL = "SELECT * FROM View_MasterLC WHERE LCStatus = " + (int)EnumLCStatus.Req_For_App + " AND MasterLCID IN (SELECT OperationObjectID FROM ApprovalRequest WHERE OperationType = " + (int)EnumApprovalRequestOperationType.MasterLC + " AND RequestTo = " + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            try
            {
                #region User Set
                if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                {
                    sSQL += " AND Applicant IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
                }
                #endregion

                _oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLC = new MasterLC();
                _oMasterLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Master LC Hitory Picker
        [HttpPost]
        public JsonResult GetMasterLCReviseHistory(MasterLC oMasterLC)
        {
            _oMasterLCs = new List<MasterLC>();
            try
            {
                _oMasterLCs = MasterLC.GetsMasterLCLog(oMasterLC.MasterLCID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLC = new MasterLC();
                _oMasterLC.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(MasterLC oMasterLC)
        {
            _oMasterLC = new MasterLC();
            _oMasterLC = oMasterLC;
            try
            {
                if (oMasterLC.ActionTypeExtra == "RequestForApproved")
                {

                    _oMasterLC.MasterLCActionType = EnumMasterLCActionType.RequestForApproval;
                    _oMasterLC.ApprovalRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                    _oMasterLC.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.MasterLC;

                }
                else if (oMasterLC.ActionTypeExtra == "UndoRequest")
                {

                    _oMasterLC.MasterLCActionType = EnumMasterLCActionType.UndoRequest;

                }
                else if (oMasterLC.ActionTypeExtra == "Approve")
                {

                    _oMasterLC.MasterLCActionType = EnumMasterLCActionType.Approve;

                }
                else if (oMasterLC.ActionTypeExtra == "UndoApprove")
                {

                    _oMasterLC.MasterLCActionType = EnumMasterLCActionType.UndoApprove;
                }

                else if (oMasterLC.ActionTypeExtra == "Req_for_Ammendment")
                {
                    _oMasterLC.MasterLCActionType = EnumMasterLCActionType.Req_for_Ammendment;
                    _oMasterLC.ReviseRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                    _oMasterLC.ReviseRequest.OperationType = EnumReviseRequestOperationType.MasterLC;
                }
                else if (oMasterLC.ActionTypeExtra == "Cancel")
                {

                    _oMasterLC.MasterLCActionType = EnumMasterLCActionType.Cancel;
                }

                _oMasterLC.Remark = oMasterLC.Remark;
                _oMasterLC.OperationBy = oMasterLC.OperationBy;
                oMasterLC = SetLCStatus(_oMasterLC);
                _oMasterLC = oMasterLC.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLC = new MasterLC();
                _oMasterLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private MasterLC SetLCStatus(MasterLC oMasterLC)//Set EnumOrderStatus Value
        {
            switch (oMasterLC.LCStatusInInt)
            {
                case 1:
                    {
                        oMasterLC.LCStatus = EnumLCStatus.Initilaized;
                        break;
                    }
                case 2:
                    {
                        oMasterLC.LCStatus = EnumLCStatus.Req_For_App;
                        break;
                    }
                case 3:
                    {
                        oMasterLC.LCStatus = EnumLCStatus.Approved;
                        break;
                    }

                case 4:
                    {
                        oMasterLC.LCStatus = EnumLCStatus.Req_for_Ammendment;
                        break;
                    }
                case 5:
                    {
                        oMasterLC.LCStatus = EnumLCStatus.Cancel;
                        break;
                    }
            }

            return oMasterLC;
        }
        #endregion
        #endregion

        #region Search Style OR Buyer by Press Enter
        [HttpGet]
        public JsonResult SearchLCAndBuyer(string sTempData, bool bIsLC, int BUID, double ts)
        {
            _oMasterLCs = new List<MasterLC>();
            string sSQL = "";
            if (bIsLC == true)
            {
                sSQL = "SELECT * FROM View_MasterLC WHERE BUID = " + BUID + " AND MasterLCNo LIKE'%" + sTempData + "%'";
            }
            else
            {
                sSQL = "SELECT * FROM View_MasterLC WHERE BUID = " + BUID + " AND ApplicantName LIKE'%" + sTempData + "%'";
            }
            try
            {
                _oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLC = new MasterLC();
                _oMasterLC.ErrorMessage = ex.Message;
                _oMasterLCs.Add(_oMasterLC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Advance Search
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }

        #region HttpGet For Search
        [HttpGet]
        public JsonResult Search(string sTemp)
        {
            List<MasterLC> oMasterLCs = new List<MasterLC>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLC = new MasterLC();
                _oMasterLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMasterLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            //Issue Date
            int nIssueCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dIssueEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            //ReceiveDate
            int nReceiveCreateDateCom = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dReceiveStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dReceiveEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            //Shipment Date
            int nShipementCreateDateCom = Convert.ToInt32(sTemp.Split('~')[6]);
            DateTime dShipmentStartDate = Convert.ToDateTime(sTemp.Split('~')[7]);
            DateTime dShipmentEndDate = Convert.ToDateTime(sTemp.Split('~')[8]);

            //Expire Date
            int nExpireCreateDateCom = Convert.ToInt32(sTemp.Split('~')[9]);
            DateTime dExpireStartDate = Convert.ToDateTime(sTemp.Split('~')[10]);
            DateTime dExpireEndDate = Convert.ToDateTime(sTemp.Split('~')[11]);

            string sLCNo = sTemp.Split('~')[12];
            string sFileNo = sTemp.Split('~')[13];
            string sBuyerIDs = sTemp.Split('~')[14];
            string sPINo = sTemp.Split('~')[15];

            int nAdviceBankAccountID = Convert.ToInt32(sTemp.Split('~')[16]);
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[17]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[18]);
            string sReturn1 = "SELECT * FROM View_MasterLC";
            string sReturn = "";


            #region Business Unit
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID =" + nBUID;

            }
            #endregion

            #region LC No

            if (sLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MasterLCNo LIKE '%" + sLCNo + "%'";

            }
            #endregion

            #region File No
            if (sFileNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FileNo LIKE '%" + sFileNo + "%'";
            }
            #endregion

            #region Buyer wise

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Applicant IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region PI Wise

            if (sPINo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MasterLCID IN (SELECT MasterLCID FROM View_MasterLCDetail WHERE PINo LIKE '%" + sPINo + "%')";
            }
            #endregion

            #region Bank Account
            if (nAdviceBankAccountID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " AdviceBankID =" + nAdviceBankAccountID;

            }
            #endregion

            #region Business Session
            if (nSessionID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MasterLCID  IN ( SELECT MasterLCID FROM VIew_MasterLCDetail WHERE ProformaInvoiceID IN (SELECT ProformaInvoiceID FROM View_ProformaInvoiceDetail WHERE OrderRecapID IN ( SELECT SaleOrderID FROM SaleOrder WHERE BusinessSessionID = " + nSessionID + ")))";

            }
            #endregion

            #region Issue Date Wise
            if (nIssueCreateDateCom > 0)
            {
                if (nIssueCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MasterLCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MasterLCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MasterLCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MasterLCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MasterLCDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MasterLCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Recive Date Wise
            if (nReceiveCreateDateCom > 0)
            {
                if (nReceiveCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Shipment Date Wise
            if (nShipementCreateDateCom > 0)
            {
                if (nShipementCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LastDateofShipment,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dShipmentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nShipementCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LastDateofShipment,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dShipmentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nShipementCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LastDateofShipment,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dShipmentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nShipementCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LastDateofShipment,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dShipmentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nShipementCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LastDateofShipment,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dShipmentStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dShipmentEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nShipementCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LastDateofShipment,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dShipmentStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dShipmentEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Expire Date Wise
            if (nExpireCreateDateCom > 0)
            {
                if (nExpireCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dExpireStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dExpireStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dExpireStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dExpireStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dExpireStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dExpireEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nIssueCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dExpireStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dExpireEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " Applicant IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion



        #endregion



        #region Master LC Summery
        #region View Master LC Summery
        public ActionResult ViewMasterLCSummery(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oMasterLCSummeries = new List<MasterLCSummery>();

            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'MasterLCSummery'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return View(_oMasterLCSummeries);
        }
        #endregion

        #region HTTP GET Master LC Summery
        [HttpPost]
        public JsonResult GetMasterLCSummery(MasterLC oMasterLC)
        {
            _oMasterLCSummeries = new List<MasterLCSummery>();
            _oMasterLCs = new List<MasterLC>();
            _oMasterLCs = oMasterLC.MasterLCList;
            try
            {

                _oMasterLCSummeries = MasterLCSummery.Gets(oMasterLC.Remark, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMasterLCSummery = new MasterLCSummery();
                _oMasterLCSummery.ErrorMessage = ex.Message;
                _oMasterLCSummeries.Add(_oMasterLCSummery);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMasterLCSummeries);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View Order Recap List
        public ActionResult ViewOrderRecapList(int id, double ts)
        {
            List<VOrder> oSaleOrders = new List<VOrder>();
            string sSQL = "";
            if (id > 0)
            {
                sSQL = "SELECT * FROM View_SaleOrder WHERE SaleOrderID IN (SELECT OrderRecapID FROM ProformaInvoiceDetail WHERE ProformaInvoiceID IN (SELECT ProformaInvoiceID FROM MasterLCDetail WHERE MasterLCID = " + id + "))";
                oSaleOrders = VOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            return PartialView(oSaleOrders);
        }
        #endregion

        //#region View Sales Contact List
        //public ActionResult ViewSalesContactList(int id, double ts)
        //{
        //    List<SalesContract> oSalesContracts = new List<SalesContract>();
        //    string sSQL = "";
        //    if (id > 0)
        //    {
        //        sSQL = "SELECT * FROM View_SalesContract WHERE MasterLCID = " + id;
        //        oSalesContracts = SalesContract.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }

        //    return PartialView(oSalesContracts);
        //}
        //#endregion

        //#region View LC Transfer List
        //public ActionResult ViewTransferList(int id, double ts)
        //{
        //    List<LCTransfer> oLCTransfers = new List<LCTransfer>();
        //    string sSQL = "";
        //    if (id > 0)
        //    {
        //        sSQL = "SELECT * FROM View_LCTransfer WHERE MasterLCID = " + id;
        //        oLCTransfers = LCTransfer.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }

        //    return PartialView(oLCTransfers);
        //}
        //#endregion

        #endregion

        #region Printing
        public ActionResult PrintMasterList(string sIDs)
        {
            _oMasterLC = new MasterLC();
            string sSQL = "SELECT * FROM View_MasterLC WHERE MasterLCID IN (" + sIDs + ") ORDER BY LastDateofShipment ASC";
            _oMasterLC.MasterLCList = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptMasterLCList oReport = new rptMasterLCList();
            byte[] abytes = oReport.PrepareReport(_oMasterLC, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintMasterLCPreview(int id)
        {
            _oMasterLC = new MasterLC();
            _oMasterLC = _oMasterLC.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oMasterLC.MasterLCDetails = MasterLCDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oMasterLC.MasterLCTermsAndConditions = MasterLCTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptMasterLC oReport = new rptMasterLC();
            byte[] abytes = oReport.PrepareReport(_oMasterLC, oCompany);
            return File(abytes, "application/pdf");
        }

        public ActionResult MasterLCHistoryPreview(int id)
        {
            List<MasterLCHistory> oMasterLCHistories = new List<MasterLCHistory>();
            oMasterLCHistories = ESimSol.BusinessObjects.MasterLCHistory.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptMasterLCHistory oReport = new rptMasterLCHistory();
            byte[] abytes = oReport.PrepareReport(oMasterLCHistories, oCompany);
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

        public ActionResult PrintMasterLCLogPreview(int id)
        {
            _oMasterLC = new MasterLC();
            _oMasterLC = _oMasterLC.GetLog(id, (int)Session[SessionInfo.currentUserID]);
            _oMasterLC.MasterLCDetails = MasterLCDetail.GetsMasterLCLog(id, (int)Session[SessionInfo.currentUserID]);
            _oMasterLC.MasterLCTermsAndConditions = MasterLCTermsAndCondition.GetsMasterLCLog(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptMasterLC oReport = new rptMasterLC();
            byte[] abytes = oReport.PrepareReport(_oMasterLC, oCompany);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region MIS Reports
        //MasterLCReport
        public ActionResult MasterLCReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.MasterLC).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oMasterLC = new MasterLC();
            _oMasterLC.ReportLayouts = ReportLayout.Gets(((int)EnumERPOperationType.MasterLC).ToString(), (int)Session[SessionInfo.currentUserID]);
            return View(_oMasterLC);
        }
        //public ActionResult MISReports(string Param)
        //{
        //    string sMasterLCIDs = Param.Split('~')[0];
        //    int nReportType = Convert.ToInt32(Param.Split('~')[1]);
        //    string sReportNo = Param.Split('~')[2];
        //    _oMasterLCs = new List<MasterLC>();
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    string sSQL = "SELECT * FROM View_MasterLC WHERE MasterLCID IN ( " + sMasterLCIDs + " )";
        //    switch (nReportType)
        //    {
        //        case (int)EnumReportLayout.PartyWise:
        //            {
        //                sSQL += "Order By Applicant";
        //                _oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //                rptBuyerWiseMasterLC oReport = new rptBuyerWiseMasterLC();
        //                byte[] abytes = oReport.PrepareReport(_oMasterLCs, oCompany, sReportNo);
        //                return File(abytes, "application/pdf");
        //            };
        //        case (int)EnumReportLayout.Session_Wise:
        //            {
        //                sSQL += "Order By Year(IssueDate), MONTH(IssueDate)";
        //                _oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //                rptSessionWiseMasterLC oReport = new rptSessionWiseMasterLC();
        //                byte[] abytes = oReport.PrepareReport(_oMasterLCs, oCompany, sReportNo);
        //                return File(abytes, "application/pdf");
        //            };
        //        case (int)EnumReportLayout.ShipmentDateWise:
        //            {
        //                sSQL += "Order By Applicant , LastDateofShipment";
        //                _oMasterLCs = MasterLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //                rptShipmentWiseMasterLC oReport = new rptShipmentWiseMasterLC();
        //                byte[] abytes = oReport.PrepareReport(_oMasterLCs, oCompany, sReportNo);
        //                return File(abytes, "application/pdf");
        //            };
        //        default:
        //            return RedirectToAction("MessageHelper", "User", new { message = "Sorry there is no Report" });
        //    }

        //}
        #endregion

    }


}
