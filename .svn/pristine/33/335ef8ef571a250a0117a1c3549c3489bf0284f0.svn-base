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


namespace ESimSolFinancial.Controllers
{
    public class ChequeController : Controller
    {
        #region Declaration
        Cheque _oCheque = new Cheque();
        List<Cheque> _oCheques = new List<Cheque>();
        private string _sSQL = "";
        string _sErrorMessage = "";
        ChequeBook _oChequeBook = new ChequeBook();
        bool _bForPrint = false;
        #endregion
        #region Functions
        private bool ValidateInput(Cheque oCheque)
        {
            if (oCheque.PaymentType == null || oCheque.PaymentType == EnumPaymentType.None)
            {
                _sErrorMessage = "Please Select a Payment Type";
                return false;
            }
            if (oCheque.PayTo == null || oCheque.PayTo == 0)
            {
                _sErrorMessage = "Please Select a Contractor";
                return false;
            }
            if (oCheque.IssueFigureID == null || oCheque.IssueFigureID <= 0)
            {
                _sErrorMessage = "Please Select Pay to Name";
                return false;
            }
            if (oCheque.Amount == null || oCheque.Amount <= 0)
            {
                _sErrorMessage = "Please Enter Amount";
                return false;
            }

            return true;
        }
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_Cheque";
            string sChequeNo = (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            string sContractorName =_bForPrint?"": (_oCheque.Selected) ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";
            string sBankName = _bForPrint ? "" : (_oCheque.Selected) ? (Arguments.Split(';')[1].Split('~')[2] == null) ? "" : Arguments.Split(';')[1].Split('~')[2] : "";
            string sCompanyName = _bForPrint ? "" : (_oCheque.Selected) ? (Arguments.Split(';')[1].Split('~')[3] == null) ? "" : Arguments.Split(';')[1].Split('~')[3] : "";
            int nChequeStatus = _bForPrint ? 0 : (_oCheque.Selected) ? (Arguments.Split(';')[1].Split('~')[4] == null) ? 0 : (Arguments.Split(';')[1].Split('~')[4] == "") ? 0 : Convert.ToInt32(Arguments.Split(';')[1].Split('~')[4]) : 0;
            string sChequeIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[5] == null) ? "" : Arguments.Split(';')[1].Split('~')[5] : "";

            string sSQL = _bForPrint ? "" : (_oCheque.Selected) ? "" : " WHERE ChequeBookID=" + _oCheque.ChequeBookID;



            #region ChequeNo
            if (sChequeNo != null)
            {
                if (sChequeNo != "")
                {
                    if (sChequeNo != "Search by Cheque No")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " ChequeNo LIKE '%" + sChequeNo + "%' ";
                    }
                }
            }
            #endregion
            #region ContractorName
            if (sContractorName != null)
            {
                if (sContractorName != "")
                {
                    if (sContractorName != "Search By Contractor")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " ContractorName LIKE '%" + sContractorName + "%' ";
                    }
                }
            }
            #endregion
            #region BankName
            if (sBankName != null)
            {
                if (sBankName != "")
                {
                    if (sBankName != "Search By Bank")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " BankName LIKE '%" + sBankName + "%' ";
                    }
                }
            }
            #endregion
            #region CompanyName
            if (sCompanyName != null)
            {
                if (sCompanyName != "")
                {
                    if (sCompanyName != "Search By Company")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " CompanyName LIKE '%" + sCompanyName + "%' ";
                    }
                }
            }
            #region ChequeStatus
            if (nChequeStatus == (int)EnumChequeStatus.DeliverToParty)
            {

                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ChequeStatus IN(" + ((int)EnumChequeStatus.Sealed).ToString() + "," + ((int)EnumChequeStatus.Return).ToString() + ")";
            }
            else if (nChequeStatus == (int)EnumChequeStatus.Encash)
            {

                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ChequeStatus IN(" + ((int)EnumChequeStatus.DeliverToParty).ToString() + "," + ((int)EnumChequeStatus.Dishonor).ToString() + ")";
            }
            else if (nChequeStatus == (int)EnumChequeStatus.Dishonor)
            {

                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ChequeStatus IN(" + ((int)EnumChequeStatus.DeliverToParty).ToString() + ")";
            }
            else if (nChequeStatus == (int)EnumChequeStatus.Return)
            {

                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ChequeStatus IN(" + ((int)EnumChequeStatus.DeliverToParty).ToString() + "," + ((int)EnumChequeStatus.Dishonor).ToString() + ")";
            }
            #endregion
            #endregion
            #region ChequeIDs
            if (sChequeIDs != null)
            {
                if (sChequeIDs != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ChequeID IN (" + sChequeIDs + ") ";
                }
            }
            #endregion
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
            string sOrderby = (_oCheque.Selected) ? " ORDER BY BankName, AccountNo, ChequeNo ASC" : " ORDER BY ChequeID ASC";
            _sSQL = _sSQL + sOrderby;
        }
        private string GenerateChangeLog(Cheque oCheque)//call after RefreshObject
        {
            Cheque _oDBCheque = new Cheque();
            _oDBCheque = _oDBCheque.Get(oCheque.ChequeID, (int)Session[SessionInfo.currentUserID]);
            string sReturn = "";
            if (_oDBCheque.ChequeDate != oCheque.ChequeDate)
            {
                sReturn = sReturn + " \nPrevious Cheque Date: " + _oDBCheque.ChequeDateInString + ", Current Cheque Date: " + oCheque.ChequeDateInString + ".";
            }
            if (_oDBCheque.PaymentType != oCheque.PaymentType)
            {
                sReturn = sReturn + " \nPrevious Payment Type: " + _oDBCheque.PaymentTypeInString + ", Current Payment Type: " + oCheque.PaymentTypeInString + ".";
            }
            if (_oDBCheque.PayTo != oCheque.PayTo)
            {
                sReturn = sReturn + " \nPrevious Contractor: " + _oDBCheque.ContractorName + ", Current Contractor: " + oCheque.ContractorName + ".";
            }
            if (_oDBCheque.IssueFigureID != oCheque.IssueFigureID)
            {
                sReturn = sReturn + " \nPrevious Pay to Person: " + _oDBCheque.ChequeIssueTo + ", Current Pay to Person: " + oCheque.ChequeIssueTo + ".";
            }
            if (_oDBCheque.Amount != oCheque.Amount)
            {
                sReturn = sReturn + " \nPrevious Amount: " + _oDBCheque.AmountInString + ", Current Amount: " + oCheque.AmountInString + ".";
            }
            if (_oDBCheque.VoucherReference != oCheque.VoucherReference)
            {
                sReturn = sReturn + " \nPrevious Voucher Reference: " + _oDBCheque.VoucherReference + ", Current Voucher Reference: " + oCheque.VoucherReference + ".";
            }
            if (_oDBCheque.Note != oCheque.Note)
            {
                sReturn = sReturn + " \nPrevious Note: " + _oDBCheque.Note + ", Current Note: " + oCheque.Note + ".";
            }
            return sReturn;
        }
        #endregion

        #region Actions
        public ActionResult ViewChequeRegister(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Cheque).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, oAuthorizationRoleMappings);

            _oCheques = new List<Cheque>();
            string sSQL = "SELECT TOP 500 * FROM View_Cheque AS HH WHERE ISNULL(HH.RegisterPrint,0)=0 AND HH.ChequeStatus NOT IN (0,1,10) ORDER BY HH.ChequeDate DESC";
            _oCheques = Cheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oCheques);
        }
        public ActionResult ViewChequeReconcile(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Cheque).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, oAuthorizationRoleMappings);

            _oCheque = new Cheque();
            _oCheque.ChequeStatuses = new List<EnumObject>();
            EnumObject oChequeStatusObj = new EnumObject();
            oChequeStatusObj.id = 0;
            oChequeStatusObj.Value = "--Operation Type--";
            _oCheque.ChequeStatuses.Add(oChequeStatusObj);

            List<EnumObject> oTempChequeStatusObjs = new List<EnumObject>();
            oTempChequeStatusObjs = EnumObject.jGets(typeof(EnumChequeStatus));
            foreach (EnumObject oItem in oTempChequeStatusObjs)
            {
                if ((EnumChequeStatus)oItem.id == EnumChequeStatus.DeliverToParty && AuthorizationRoleMapping.HasPermission(EnumRoleOperationType.DeliverToParty, EnumModuleName.Cheque, oAuthorizationRoleMappings, (int)Session[SessionInfo.currentUserID]))
                {
                    _oCheque.ChequeStatuses.Add(oItem);
                }
                else if ((EnumChequeStatus)oItem.id == EnumChequeStatus.Encash && AuthorizationRoleMapping.HasPermission(EnumRoleOperationType.Encash, EnumModuleName.Cheque, oAuthorizationRoleMappings, (int)Session[SessionInfo.currentUserID]))
                {
                    _oCheque.ChequeStatuses.Add(oItem);
                }
                else if ((EnumChequeStatus)oItem.id == EnumChequeStatus.Dishonor && AuthorizationRoleMapping.HasPermission(EnumRoleOperationType.Dishonor, EnumModuleName.Cheque, oAuthorizationRoleMappings, (int)Session[SessionInfo.currentUserID]))
                {
                    _oCheque.ChequeStatuses.Add(oItem);
                }
                else if ((EnumChequeStatus)oItem.id == EnumChequeStatus.Return && AuthorizationRoleMapping.HasPermission(EnumRoleOperationType.Return, EnumModuleName.Cheque, oAuthorizationRoleMappings, (int)Session[SessionInfo.currentUserID]))
                {
                    _oCheque.ChequeStatuses.Add(oItem);
                }
            }

            return View(_oCheque);
        }
        public ActionResult ViewChequeMgt(int nid, int DocID)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Cheque).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oChequeBook.Cheques = new List<Cheque>();
            _oChequeBook = new ChequeBook();
            if (nid > 0)
            {
                _oChequeBook = _oChequeBook.Get(nid, (int)Session[SessionInfo.currentUserID]);
                _oChequeBook.Cheques = Cheque.Gets(nid, EnumChequeStatus.Sealed, (int)Session[SessionInfo.currentUserID]);
            }
            if (DocID>0)
            {
                _oChequeBook.Cheques = new List<Cheque>();
                _oCheque = _oCheque.Get(DocID, (int)Session[SessionInfo.currentUserID]);
                _oChequeBook.Cheques.Add(_oCheque);
            }
            return View(_oChequeBook);
        }
        public ActionResult ViewChequeIssue(int id, int nid) // ChequeID
        {
            _oCheque = new Cheque();
            if (id > 0)
            {
                _oCheque = _oCheque.Get(id, (int)Session[SessionInfo.currentUserID]);
            }

            else
            {
                _oCheque.ChequeBookID = nid;
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
            _oCheque.PaymentTypes = oPaymentTypes;
            _oCheque.Contractor = new Contractor().Get(_oCheque.PayTo, (int)Session[SessionInfo.currentUserID]);
            _oCheque.Contractor.IssueFigures = IssueFigure.Gets(_oCheque.PayTo, (int)Session[SessionInfo.currentUserID]);
            return View(_oCheque);
        }
        public ActionResult ViewStatusChangeConfirmation()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetVouchers(Cheque oCheque)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = "";

            #region Create Voucher Batch
            string sSQL = "SELECT * FROM View_VoucherBatch AS TT WHERE (TT.CreateBy =" + ((int)Session[SessionInfo.currentUserID]).ToString() + " AND  TT.BatchStatus=1) ORDER BY VoucherBatchID ASC";
            List<VoucherBatch> oVoucherBatchs = VoucherBatch.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            if (oVoucherBatchs.Count <= 0)
            {
                VoucherBatch oVB = new VoucherBatch();
                oVB.BatchStatus = EnumVoucherBatchStatus.BatchOpen;
                oVB = oVB.Save((int)Session[SessionInfo.currentUserID]);
                oVoucherBatchs.Add(oVB);
            }
            #endregion

            IntegrationSetup oIntegrationSetup = new IntegrationSetup();
            oIntegrationSetup = oIntegrationSetup.GetByVoucherSetup(oCheque.Setup, (int)Session[SessionInfo.currentUserID]);
            if (oIntegrationSetup.IntegrationSetupID <= 0)
            {
                oCheque = new Cheque();
                oCheque.ErrorMessage = "Please Configure Integration Setup for Payment Cheque";
                serializer = new JavaScriptSerializer();
                sjson = serializer.Serialize(oCheque);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<Voucher> oVouchers = new List<Voucher>();
                oVouchers = Voucher.GetsAutoVoucher(oIntegrationSetup, (object)oCheque, true, (int)Session[SessionInfo.currentUserID]);

                if (oVouchers[0].ErrorMessage == "")
                {
                    oCheque.Vouchers = new List<Voucher>();
                    oCheque.Vouchers = Voucher.MapVoucherExplanationObject(oVouchers);
                    sjson = serializer.Serialize(oCheque);
                    return Json(sjson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    oCheque = new Cheque();
                    oCheque.ErrorMessage = oVouchers[0].ErrorMessage;
                    serializer = new JavaScriptSerializer();
                    sjson = serializer.Serialize(oCheque);
                    return Json(sjson, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        public JsonResult Commit(Cheque oCheque)
        {
            _oCheque = new Cheque();
            List<Cheque> oCheques = new List<Cheque>();
            if (oCheque.Selected)
            {
                try
                {

                    _oCheque = oCheque;
                    oCheques = _oCheque.Cheques;
                    if (oCheques == null || oCheques.Count <= 0)
                    {
                        _oCheque.ErrorMessage = "Invalid Cheque!";
                    }
                    ChequeHistory oChequeHistory = new ChequeHistory();
                    List<ChequeHistory> oChequeHistorys = new List<ChequeHistory>();

                    foreach (Cheque oItem in oCheques)
                    {
                        oChequeHistory = new ChequeHistory();
                        oChequeHistory.ChequeHistoryID = 0;
                        oChequeHistory.ChequeID = oItem.ChequeID;
                        oChequeHistory.PreviousStatus = oItem.ChequeStatus;
                        oChequeHistory.CurrentStatus = _oCheque.ChequeStatus;
                        oChequeHistorys.Add(oChequeHistory);
                    }

                    oCheques = Cheque.UpdateChequeStatus(oChequeHistorys, (int)Session[SessionInfo.currentUserID]);

                }
                catch (Exception ex)
                {
                    _oCheque = new Cheque();
                    _oCheque.ErrorMessage = ex.Message;
                    oCheques.Add(_oCheque);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string sjson = serializer.Serialize(oCheques);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    if (!ValidateInput(oCheque)) { throw new Exception(_sErrorMessage); }
                    _oCheque = oCheque;
                    _oCheque.VoucherReference = (_oCheque.VoucherReference == null) ? "" : _oCheque.VoucherReference;
                    _oCheque.Note = (_oCheque.Note == null) ? "" : _oCheque.Note;
                    string sMSG = "";
                    if (_oCheque.ErrorMessage == "Issue Cheque") { sMSG = "Cheque Issued Successfully"; } else if (_oCheque.ErrorMessage == "Edit Cheque") { sMSG = "Cheque Edited Successfully"; }
                    #region Cheque History
                    ChequeHistory oChequeHistory = new ChequeHistory().Get(_oCheque.ChequeID, (int)EnumChequeStatus.Issued, (int)Session[SessionInfo.currentUserID]);
                    if (oChequeHistory.ChequeHistoryID <= 0) { oChequeHistory.ChequeID = _oCheque.ChequeID; }


                    if (_oCheque.ErrorMessage == "Issue Cheque")
                    {
                        oChequeHistory.PreviousStatus = EnumChequeStatus.Activate;
                        oChequeHistory.CurrentStatus = EnumChequeStatus.Issued;
                        oChequeHistory.ChangeLog = oChequeHistory.ChangeLog + " " + oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                    }
                    else if (_oCheque.ErrorMessage == "Edit Cheque")
                    {
                        oChequeHistory.PreviousStatus = EnumChequeStatus.EditMode;
                        oChequeHistory.CurrentStatus = EnumChequeStatus.Authorized;
                        oChequeHistory.ChangeLog = oChequeHistory.ChangeLog + " " + oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                    }

                    oChequeHistory.Note = _oCheque.Note;
                    oChequeHistory.ChangeLog = oChequeHistory.ChangeLog + this.GenerateChangeLog(oCheque);
                    #endregion
                    int nSLNo = _oCheque.SerialNo;
                    _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                    _oCheque.SerialNo = nSLNo;
                    if (_oCheque.ChequeID > 0)
                    {
                        _oCheque.ErrorMessage = sMSG;
                    }
                }
                catch (Exception ex)
                {
                    _oCheque.ErrorMessage = ex.Message;
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string sjson = serializer.Serialize(_oCheque);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditMode(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = EnumChequeStatus.Authorized;
                oChequeHistory.CurrentStatus = EnumChequeStatus.EditMode;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Edited Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Cancel(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = _oCheque.ChequeStatus;
                oChequeHistory.CurrentStatus = EnumChequeStatus.Cancel;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Cancel Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Return(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = _oCheque.ChequeStatus;
                oChequeHistory.CurrentStatus = EnumChequeStatus.Return;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Return Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Dishonor(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = _oCheque.ChequeStatus;
                oChequeHistory.CurrentStatus = EnumChequeStatus.Dishonor;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Dishonor Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Encash(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = _oCheque.ChequeStatus;
                oChequeHistory.CurrentStatus = EnumChequeStatus.Encash;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Encash Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeliverToParty(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = _oCheque.ChequeStatus;
                oChequeHistory.CurrentStatus = EnumChequeStatus.DeliverToParty;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Deliver To Party Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Sealed(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = _oCheque.ChequeStatus;
                oChequeHistory.CurrentStatus = EnumChequeStatus.Sealed;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Sealed Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Authorized(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = _oCheque.ChequeStatus;
                oChequeHistory.CurrentStatus = EnumChequeStatus.Authorized;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Authorized Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Activate(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = _oCheque.ChequeStatus;
                oChequeHistory.CurrentStatus = EnumChequeStatus.Activate;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Activated Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Initiate(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                _oCheque = oCheque;
                #region Cheque History
                ChequeHistory oChequeHistory = new ChequeHistory();
                oChequeHistory.PreviousStatus = _oCheque.ChequeStatus;
                oChequeHistory.CurrentStatus = EnumChequeStatus.Initiate;
                oChequeHistory.ChequeID = _oCheque.ChequeID;
                oChequeHistory.Note = (_oCheque.ErrorMessage == null) ? "" : _oCheque.ErrorMessage;
                oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                #endregion
                int nSLNo = _oCheque.SerialNo;
                _oCheque = _oCheque.UpdateChequeStatus(oChequeHistory, (int)Session[SessionInfo.currentUserID]);
                _oCheque.SerialNo = nSLNo;
                if (_oCheque.ChequeID > 0)
                {
                    _oCheque.ErrorMessage = "Cheque Initiated Successfully";
                }
            }
            catch (Exception ex)
            {
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ActiveAll(List<Cheque> oCheques)
        {
            _oCheques = new List<Cheque>();
            _oCheques = oCheques;
            try
            {
                if (_oCheques == null || _oCheques.Count <= 0)
                {
                    _sErrorMessage = "List is Empty. \nPlease Load Data First.";
                    throw new Exception(_sErrorMessage);
                }
                //if (_oCheques[0].ChequeStatus != EnumChequeStatus.Initiate)
                //{
                //    MessageBox.Show("Please Select a Initiated Cheque!", "Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}

                ChequeHistory oChequeHistory = new ChequeHistory();
                List<ChequeHistory> oChequeHistorys = new List<ChequeHistory>();
                oCheques = new List<Cheque>();
                string sChequeIDs = "";
                foreach (Cheque oItem in _oCheques)
                {
                    sChequeIDs += oItem.ChequeID + ",";
                    if (oItem.ChequeStatus == EnumChequeStatus.Initiate)
                    {
                        oChequeHistory = new ChequeHistory();
                        oChequeHistory.ChequeHistoryID = 0;
                        oChequeHistory.ChequeID = oItem.ChequeID;
                        oChequeHistory.PreviousStatus = oItem.ChequeStatus;
                        oChequeHistory.CurrentStatus = EnumChequeStatus.Activate;
                        oChequeHistory.Note = "N/A";
                        oChequeHistory.ChangeLog = oChequeHistory.CurrentStatusInString + " by " + (string)Session[SessionInfo.currentUserName] + ". \nPrevious Status: " + oChequeHistory.PreviousStatusInString + " Current Status: " + oChequeHistory.CurrentStatusInString;
                        oChequeHistorys.Add(oChequeHistory);
                    }
                }
                oCheques = Cheque.UpdateChequeStatus(oChequeHistorys, (int)Session[SessionInfo.currentUserID]);

                _oCheques = new List<Cheque>();
                string sSQL = "SELECT * FROM View_Cheque WHERE ChequeID IN (" + sChequeIDs.Remove(sChequeIDs.Length - 1) + ") ORDER BY ChequeID ASC";
                _oCheques = Cheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {                
                _oCheques = new List<Cheque>();
                Cheque oCheque = new Cheque();
                oCheque.ErrorMessage = ex.Message;
                _oCheques.Add(oCheque);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheques);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Refresh(Cheque oCheque)
        {
            _oCheque = new Cheque();
            _oCheque = oCheque;
            this.MakeSQL(_oCheque.ErrorMessage);
            _oCheques = new List<Cheque>();
            _oCheques = Cheque.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheques);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WaitForRegPrint(Cheque oCheque)
        {
            _oCheque = new Cheque();
            _sSQL = "SELECT * FROM View_Cheque AS HH WHERE ISNULL(HH.RegisterPrint,0)=0 AND HH.ChequeStatus NOT IN (0,1,10) ORDER BY ChequeDate, ChequeBookID, ChequeID ASC";
            _oCheques = Cheque.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheques);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DateWiseCheque(Cheque oCheque)
        {
            _oCheque = new Cheque();
            _sSQL = "SELECT * FROM View_Cheque AS HH WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ChequeDate,106))=CONVERT(DATE,'" + oCheque.ChequeDateInString + "') AND HH.ChequeStatus NOT IN (0,1,10) ORDER BY ChequeDate, ChequeBookID, ChequeID ASC";
            _oCheques = Cheque.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheques);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommitRegisterPrint(Cheque oCheque)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = Cheque.ConfirmRegisterPrint(oCheque.Cheques, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception e)
            {
                sFeedBackMessage = e.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Save(Cheque oCheque)
        {
            _oCheque = new Cheque();
            try
            {
                oCheque.ChequeNo = oCheque.ChequeNo == null ? "" : oCheque.ChequeNo;
                oCheque.VoucherReference = oCheque.VoucherReference == null ? "" : oCheque.VoucherReference;
                oCheque.Note = oCheque.Note == null ? "" : oCheque.Note;
                if (!this.ValidateInput(oCheque))
                {
                    throw new Exception(_sErrorMessage);
                }
                _oCheque = oCheque;
                _oCheque = _oCheque.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCheque = new Cheque();
                _oCheque.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCheque);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id, double ts)
        {
            string sFeedBackMessage = "";
            try
            {
                Cheque oCheque = new Cheque();
                sFeedBackMessage = oCheque.Delete(id, (int)Session[SessionInfo.currentUserID]);
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

        #region Searching
        private string GetSQL(string sTemp)
        {
            int nIssueDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dtIssueFrom = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dtIssueTo = Convert.ToDateTime(sTemp.Split('~')[2]);

            int nChkDateCom = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dtChkFrom = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dtChkTo = Convert.ToDateTime(sTemp.Split('~')[5]);
            
            string sBookCode = sTemp.Split('~')[6];
            string sChequeNo = sTemp.Split('~')[7];
            string sAccountNo = sTemp.Split('~')[8];
            string sPartyName = sTemp.Split('~')[9];
            
            int nChequeAmountCom = 0;
            double nChequeAmountFrom = 0;
            double nChequeAmountTo = 0;
            if (sTemp.Split('~').Length > 10)
            {
                nChequeAmountCom = Convert.ToInt32(sTemp.Split('~')[10]);
                nChequeAmountFrom = Convert.ToDouble(sTemp.Split('~')[11]);
                nChequeAmountTo = Convert.ToDouble(sTemp.Split('~')[12]);
            }

            string sReturn1 = "SELECT * FROM View_Cheque";
            string sReturn = "";

            #region sBookCode
            if (!string.IsNullOrEmpty(sBookCode))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BookCodePartOne+BookCodePartTwo LIKE '%" + sBookCode + "%'";
            }
            #endregion

            #region sChequeNo
            if (!string.IsNullOrEmpty(sChequeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChequeNo LIKE '%" + sChequeNo + "%'";
            }
            #endregion

            #region sAccountNo
            if (!string.IsNullOrEmpty(sAccountNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " AccountNo LIKE '%" + sAccountNo + "%'";
            }
            #endregion

            #region sPartyName
            if (!string.IsNullOrEmpty(sPartyName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorName LIKE '%" + sPartyName + "%'";
            }
            #endregion
            
            #region Issue Date Wise
            if (nIssueDateCom > 0)
            {
                if (nIssueDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeID IN (SELECT HH.ChequeID FROM ChequeHistory AS HH WHERE HH.CurrentStatus=2 AND CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) = '" + dtIssueFrom.ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeID IN (SELECT HH.ChequeID FROM ChequeHistory AS HH WHERE HH.CurrentStatus=2 AND CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) != '" + dtIssueFrom.ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeID IN (SELECT HH.ChequeID FROM ChequeHistory AS HH WHERE HH.CurrentStatus=2 AND CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) > '" + dtIssueFrom.ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeID IN (SELECT HH.ChequeID FROM ChequeHistory AS HH WHERE HH.CurrentStatus=2 AND CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) < '" + dtIssueFrom.ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeID IN (SELECT HH.ChequeID FROM ChequeHistory AS HH WHERE HH.CurrentStatus=2 AND CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))>= '" + dtIssueFrom.ToString("dd MMM yyyy") + "' AND CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) < '" + dtIssueTo.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nIssueDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeID IN (SELECT HH.ChequeID FROM ChequeHistory AS HH WHERE HH.CurrentStatus=2 AND CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))< '" + dtIssueFrom.ToString("dd MMM yyyy") + "' OR CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) > '" + dtIssueTo.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
            }

            #endregion

            #region ChequeDate Wise
            if (nChkDateCom > 0)
            {
                if (nChkDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate = '" + dtIssueFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nChkDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate != '" + dtIssueFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nChkDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate > '" + dtIssueFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nChkDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChequeDate < '" + dtIssueFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nChkDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChequeDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtChkFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtChkTo.ToString("dd MMM yyyy") + "',106)) ";
                }
                if (nChkDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChequeDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtChkFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtChkTo.ToString("dd MMM yyyy") + "',106)) ";
                }              
            }

            #endregion

            #region Cheque Amount Wise
            if (nChequeAmountCom > 0)
            {
                if (nChequeAmountCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount = " + nChequeAmountFrom.ToString("0.00");
                }
                if (nChequeAmountCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount != " + nChequeAmountFrom.ToString("0.00");
                }
                if (nChequeAmountCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount > " + nChequeAmountFrom.ToString("0.00");
                }
                if (nChequeAmountCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount < " + nChequeAmountFrom.ToString("0.00");
                }
                if (nChequeAmountCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount  BETWEEN " + nChequeAmountFrom.ToString("0.00") + " AND " + nChequeAmountTo.ToString("0.00");
                }
                if (nChequeAmountCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount NOT BETWEEN " + nChequeAmountFrom.ToString("0.00") + " AND " + nChequeAmountTo.ToString("0.00");                    
                }
            }

            #endregion
            
            sReturn = sReturn1 + sReturn + " AND ChequeStatus NOT IN (0,1,10) ORDER BY ChequeDate, ChequeBookID, ChequeID ASC";
            return sReturn;
        }

        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<Cheque> oCheques = new List<Cheque>();
            try
            {
                string sSQL = GetSQL(Temp);
                oCheques = Cheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oCheques = new List<Cheque>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCheques);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult PrintCheques(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            _oCheques = new List<Cheque>();
            _oCheques = Cheque.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);



            string Messge = "Cheque List";
            rptCheques oReport = new rptCheques();
            byte[] abytes = oReport.PrepareReport(_oCheques, oCompany, Messge);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintChequesInXL(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            //_productsServices = new ProductsServices();
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ChequeXL>));

            //We load the data
            List<Cheque> oCheques = Cheque.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            int nCount = 0; double nTotal = 0;
            ChequeXL oChequeXL = new ChequeXL();
            List<ChequeXL> oChequeXLs = new List<ChequeXL>();
            foreach (Cheque oItem in oCheques)
            {
                nCount++;
                oChequeXL = new ChequeXL();
                oChequeXL.SLNo = nCount.ToString();
                oChequeXL.ChequeNo = oItem.ChequeNo;
                oChequeXL.ChequeDate = oItem.ChequeDateInString;
                oChequeXL.ChequeStatus = oItem.ChequeStatusInString;
                oChequeXL.PaymentType = oItem.PaymentTypeInString;
                oChequeXL.OperationBy = oItem.OperationByName;
                oChequeXL.OperationDateTime = oItem.OperationDateTimeInString;
                oChequeXLs.Add(oChequeXL);               
            }

           

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oChequeXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Cheques.xls");
        }

        public ActionResult PrintLeaf(int nid, bool bIsDatePrint)
        {
            //_bForPrint = true;
            //this.MakeSQL(arguments);
            //_oCheques = new List<Cheque>();
            //_oCheques = Cheque.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            Cheque oCheque = new Cheque();
            //PrintSetup oPrintSetup = new PrintSetup();
            ChequeSetup oChequeSetup = new ChequeSetup();
            //Company oCompany = new Company();
            oCheque = oCheque.Get(nid, (int)Session[SessionInfo.currentUserID]);
            //oPrintSetup = oPrintSetup.Get(oCheque.PrintSetupID, (int)Session[SessionInfo.currentUserID]);
            oChequeSetup = oChequeSetup.Get(oCheque.ChequeSetupID, (int)Session[SessionInfo.currentUserID]);
            string spath;
            System.Drawing.Image iACPayee;
            System.Drawing.Image iEquel3;
            spath = this.ControllerContext.HttpContext.Server.MapPath("../Content/Images/ACPayee.jpg");
            using (FileStream fileStream = System.IO.File.OpenRead(spath))
            {
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                iACPayee = System.Drawing.Image.FromStream(memStream);
                iACPayee.Save(Response.OutputStream, ImageFormat.Jpeg);
            }
            spath = this.ControllerContext.HttpContext.Server.MapPath("../Content/Images/Equel3.jpg");
            using (FileStream fileStream = System.IO.File.OpenRead(spath))
            {
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                iEquel3 = System.Drawing.Image.FromStream(memStream);
                iEquel3.Save(Response.OutputStream, ImageFormat.Jpeg);
            }
            //oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);



            string Messge = "Cheque List";
            rptChequeLeafPrint oReport = new rptChequeLeafPrint();
            //byte[] abytes = oReport.PrepareReport(oCheque, oPrintSetup, iACPayee, iEquel3);
            byte[] abytes = oReport.PrepareReport(oCheque, oChequeSetup, iACPayee, iEquel3, bIsDatePrint);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintRegister(string ids, double ts)
        {
            _oCheques = new List<Cheque>();
            _sSQL = "SELECT * FROM View_Cheque AS HH WHERE HH.ChequeID IN (" + ids + ") ORDER BY ChequeDate, ChequeBookID, ChequeID ASC";
            _oCheques = Cheque.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = this.GetCompanyLogo(oCompany);

            rptChequeRegisters oReport = new rptChequeRegisters();
            byte[] abytes = oReport.PrepareReport(_oCheques, oCompany);
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
    }  
        
}