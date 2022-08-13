using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{

    public class ProformaInvoiceController : Controller
    {
        #region Declartion
        ProformaInvoice _oProformaInvoice = new ProformaInvoice();
        List<ProformaInvoice> _oProformaInvoices = new List<ProformaInvoice>();
        ProformaInvoiceDetail _oProformaInvoiceDetail = new ProformaInvoiceDetail();
        List<ProformaInvoiceDetail> _oProformaInvoiceDetails = new List<ProformaInvoiceDetail>();
        ProformaInvoiceRequiredDoc _oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
        List<ProformaInvoiceRequiredDoc> _oProformaInvoiceRequiredDocs = new List<ProformaInvoiceRequiredDoc>();
        ProformaInvoiceTermsAndCondition _oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
        List<ProformaInvoiceTermsAndCondition> _oProformaInvoiceTermsAndConditions = new List<ProformaInvoiceTermsAndCondition>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        ExportTermsAndCondition _oExportTermsAndCondition = new ExportTermsAndCondition();
        List<ExportTermsAndCondition> _oExportTermsAndConditions =  new List<ExportTermsAndCondition>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        ReviseRequest _oReviseRequest = new ReviseRequest();
        #endregion
        
        #region Actions
        public ActionResult ViewProformaInvoiceIssue(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProformaInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]); //true for RT Own Accounts 
            ViewBag.PaymentTermObjs = EnumObject.jGets(typeof(EnumPaymentTerm));
            ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            _oProformaInvoices = new List<ProformaInvoice>();
            _oProformaInvoices = ProformaInvoice.Gets(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(_oProformaInvoices);
        }

        public ActionResult ViewProformaInvoice(int id, int buid)
        {
            _oProformaInvoice = new ProformaInvoice();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            _oClientOperationSetting = new ClientOperationSetting();
            if (id > 0)
            {
                _oProformaInvoice = _oProformaInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceDetails = ProformaInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceRequiredDocs = ProformaInvoiceRequiredDoc.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceTermsAndConditions = ProformaInvoiceTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            string sSQL = "SELECT * FROM View_BankAccount AS MM WHERE MM.BankBranchID IN(SELECT HH.BankBranchID FROM BankBranchDept AS HH WHERE HH.OperationalDept=" + ((int)EnumOperationalDept.Export_Own).ToString() + ")  AND MM.BankBranchID IN (SELECT UU.BankBranchID FROM BankBranchBU AS UU WHERE UU.BUID = " + buid.ToString() + ") ORDER BY AccountNo ASC";
            ViewBag.PaymentTerms = EnumObject.jGets(typeof(EnumPaymentTerm));
            ViewBag.DeliveryTerms = EnumObject.jGets(typeof(EnumDeliveryTerm));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oBusinessUnits.Add(oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]));
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.BankAccounts = BankAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.PIOperation , (int)Session[SessionInfo.currentUserID]);
            ViewBag.MeasurementUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            return View(_oProformaInvoice);
        }


        #endregion

        #region Post Method
        [HttpPost]
        public JsonResult GetsBuyerWiseOrderRecap(ProformaInvoice oProformaInvoice)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();            
            try
            {
                string sSQL = "SELECT * FROM View_OrderRecap AS HH WHERE  HH.BuyerID=" + oProformaInvoice.BuyerID.ToString() + " AND ISNULL(HH.ApproveBy,0)!=0 AND HH.BUID=" + oProformaInvoice.BUID.ToString() + " AND HH.OrderRecapID NOT IN(SELECT MM.OrderRecapID FROM ProformaInvoiceDetail AS MM WHERE MM.ProformaInvoiceID!=" + oProformaInvoice.ProformaInvoiceID.ToString() + ") ORDER BY HH.OrderRecapID ASC";
                oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oOrderRecaps = new List<OrderRecap>();
                OrderRecap oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = ex.Message;
                oOrderRecaps.Add(oOrderRecap);
            }
            var jsonResult = Json(oOrderRecaps, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oOrderRecaps);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDocumentTypes(ProformaInvoice oProformaInvoice)
        {
            List<EnumObject> oDocumentTypes = new List<EnumObject>();
            try
            {
                oDocumentTypes = EnumObject.jGets(typeof(EnumDocumentType));
                oDocumentTypes.Remove(oDocumentTypes[0]);// there remove None Type value 
            }
            catch (Exception ex)
            {
                oDocumentTypes[0].Value = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDocumentTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(ProformaInvoice oProformaInvoice)
        {
            _oProformaInvoice = new ProformaInvoice();
            List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDocs = new List<ProformaInvoiceRequiredDoc>();
            try
            {
                oProformaInvoice.PIStatus = (EnumPIStatus)oProformaInvoice.PIStatusInInt;
                oProformaInvoice.DeliveryTerm = (EnumDeliveryTerm)oProformaInvoice.DeliveryTermInInt;
                oProformaInvoice.PaymentTerm = (EnumPaymentTerm)oProformaInvoice.PaymentTermInInt;
                foreach (ProformaInvoiceRequiredDoc oitem in oProformaInvoice.ProformaInvoiceRequiredDocs)
                {
                    _oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
                    _oProformaInvoiceRequiredDoc = oitem;
                    _oProformaInvoiceRequiredDoc.DocType = (EnumDocumentType)oitem.DocTypeInInt;
                    oProformaInvoiceRequiredDocs.Add(_oProformaInvoiceRequiredDoc);
                }
                oProformaInvoice.ProformaInvoiceRequiredDocs = oProformaInvoiceRequiredDocs;
                oProformaInvoice.SessionName = oProformaInvoice.ProformaInvoiceDetails[0].SessionName;//SEt SEssion Name for PI format 2
                _oProformaInvoice = oProformaInvoice.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProformaInvoice = new ProformaInvoice();
                _oProformaInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProformaInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region function
        private bool HaveRateViewPermission(EnumRoleOperationType OperationType)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProformaInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            if ((int)Session[SessionInfo.currentUserID] == -9)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < oAuthorizationRoleMappings.Count; i++)
                {
                    if (oAuthorizationRoleMappings[i].OperationType == OperationType && oAuthorizationRoleMappings[i].ModuleName == EnumModuleName.ProformaInvoice)
                    {
                        return true;
                    }

                }
            }

            return false;
        }
        #endregion

        #region Proforma Invoice  Issue and Management 
        


        public ActionResult ViewProformaInvoiceReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oProformaInvoice = new ProformaInvoice();
            //_oProformaInvoice.ReportLayouts = ReportLayout.Gets(EnumModuleName.ProformaInvoice, (int)Session[SessionInfo.currentUserID]);
            return View(_oProformaInvoice);
        }

        #endregion

        #region Add, Edit, Delete

        #region Proforma Invoice Default format
        
        #endregion

        #region Proforma Invoice Applicant and Discount Format
        public ActionResult ViewProformaInvoiceWithApplicant(int id, double ts)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProformaInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oProformaInvoice = new ProformaInvoice();
            _oClientOperationSetting = new ClientOperationSetting();
            string sSQL = "SELECT * FROM MeasurementUnit WHERE UnitType = " + (int)EnumUniteType.Count;
            if (id > 0)
            {
                _oProformaInvoice = _oProformaInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceDetails = ProformaInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceRequiredDocs = ProformaInvoiceRequiredDoc.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceTermsAndConditions = ProformaInvoiceTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            //_oProformaInvoice.MeasurementUnits = MeasurementUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //_oProformaInvoice.ClientOperationSetting = _oClientOperationSetting.Get(1, (int)Session[SessionInfo.currentUserID]);
            //_oProformaInvoice.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            //_oProformaInvoice.Companies = Company.Gets((int)Session[SessionInfo.currentUserID]);
            //_oProformaInvoice.BankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]); //true for RT Own Accounts 
            //_oProformaInvoice.PaymentTermObjs = EnumObject.jGets(typeof(EnumPaymentTerm));
            //_oProformaInvoice.DocumentTypes = EnumObject.jGets(typeof(EnumDocumentType));
            return View(_oProformaInvoice);
        }
        #endregion

        #region Proforma Invoice Revise Normal format
        public ActionResult ViewProformaInvoiceRevise(int id, int buid)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProformaInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            _oProformaInvoice = new ProformaInvoice();
            string sSQL = "SELECT * FROM MeasurementUnit WHERE UnitType = " + (int)EnumUniteType.Count;
            if (id > 0)
            {
                _oProformaInvoice = _oProformaInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceDetails = ProformaInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceRequiredDocs = ProformaInvoiceRequiredDoc.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceTermsAndConditions = ProformaInvoiceTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            sSQL = "SELECT * FROM View_BankAccount AS MM WHERE MM.BankBranchID IN(SELECT HH.BankBranchID FROM BankBranchDept AS HH WHERE HH.OperationalDept=" + ((int)EnumOperationalDept.Export_Own).ToString() + ")  AND MM.BankBranchID IN (SELECT UU.BankBranchID FROM BankBranchBU AS UU WHERE UU.BUID = " + buid.ToString() + ") ORDER BY AccountNo ASC";
            ViewBag.PaymentTerms = EnumObject.jGets(typeof(EnumPaymentTerm));
            ViewBag.DeliveryTerms = EnumObject.jGets(typeof(EnumDeliveryTerm));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oBusinessUnits.Add(oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]));
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.BankAccounts = BankAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.PIOperation, (int)Session[SessionInfo.currentUserID]);
            ViewBag.MeasurementUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            return View(_oProformaInvoice);
        }
        #endregion

        #region Proforma Invoice Revise Applicant and Discount Format
        public ActionResult ViewPIReviseWithApplicant(int id, double ts)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProformaInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            _oProformaInvoice = new ProformaInvoice();
            string sSQL = "SELECT * FROM MeasurementUnit WHERE UnitType = " + (int)EnumUniteType.Count;
            if (id > 0)
            {
                _oProformaInvoice = _oProformaInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceDetails = ProformaInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceRequiredDocs = ProformaInvoiceRequiredDoc.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProformaInvoice.ProformaInvoiceTermsAndConditions = ProformaInvoiceTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            //_oProformaInvoice.MeasurementUnits = MeasurementUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //_oProformaInvoice.ClientOperationSetting = _oClientOperationSetting.Get(1, (int)Session[SessionInfo.currentUserID]);
            //_oProformaInvoice.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            //_oProformaInvoice.Companies = Company.Gets((int)Session[SessionInfo.currentUserID]);
            //_oProformaInvoice.BankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]); //true for RT Own Accounts 
            //_oProformaInvoice.PaymentTermObjs = EnumObject.jGets(typeof(EnumPaymentTerm));
            //_oProformaInvoice.DocumentTypes = EnumObject.jGets(typeof(EnumDocumentType));
            return View(_oProformaInvoice);
        }
        #endregion

        #region HTTP Save
        
        #endregion

        #region HTTP AcceptProformaInvoiceRevise
        [HttpPost]
        public JsonResult AcceptProformaInvoiceRevise(ProformaInvoice oProformaInvoice)
        {
            _oProformaInvoice = new ProformaInvoice();
            List<ProformaInvoiceRequiredDoc> oProformaInvoiceRequiredDocs = new List<ProformaInvoiceRequiredDoc>();
            try
            {
                oProformaInvoice.PIStatus = (EnumPIStatus)oProformaInvoice.PIStatusInInt;
                oProformaInvoice.DeliveryTerm = (EnumDeliveryTerm)oProformaInvoice.DeliveryTermInInt;
                oProformaInvoice.PaymentTerm = (EnumPaymentTerm)oProformaInvoice.PaymentTermInInt;
                foreach (ProformaInvoiceRequiredDoc oitem in oProformaInvoice.ProformaInvoiceRequiredDocs)
                {
                    _oProformaInvoiceRequiredDoc = new ProformaInvoiceRequiredDoc();
                    _oProformaInvoiceRequiredDoc = oitem;
                    _oProformaInvoiceRequiredDoc.DocType = (EnumDocumentType)oitem.DocTypeInInt;
                    oProformaInvoiceRequiredDocs.Add(_oProformaInvoiceRequiredDoc);
                }
                oProformaInvoice.ProformaInvoiceRequiredDocs = oProformaInvoiceRequiredDocs;
                _oProformaInvoice = oProformaInvoice.AcceptProformaInvoiceRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProformaInvoice = new ProformaInvoice();
                _oProformaInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProformaInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string smessage = "";
            try
            {
                ProformaInvoice oProformaInvoice = new ProformaInvoice();
                smessage = oProformaInvoice.Delete(id, (int)Session[SessionInfo.currentUserID]);

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

        #region GetOrderRecap
        [HttpGet]
        public JsonResult GetOrderRecap(int id)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            try
            {
                oOrderRecap = oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                
            }
            catch (Exception ex)
            {
                oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        
        #endregion

        #region GetPIDetail
        [HttpGet]
        public JsonResult GetPIDetail(int id)
        {
            ProformaInvoiceDetail oProformaInvoiceDetail = new ProformaInvoiceDetail();
            try
            {
                oProformaInvoiceDetail = oProformaInvoiceDetail.Get(id, (int)Session[SessionInfo.currentUserID]);
                
            }
            catch (Exception ex)
            {
                oProformaInvoiceDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProformaInvoiceDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        
        #endregion

        #region Proforma Invoice Hitory Picker
        public ActionResult ProformaInvoiceHistory(int id, double ts) // id is PI Id
        {
            _oProformaInvoice = new ProformaInvoice();
            _oProformaInvoice .ProformaInvoiceHistories = ESimSol.BusinessObjects.ProformaInvoiceHistory.Gets(id, (int)Session[SessionInfo.currentUserID]);
            return View(_oProformaInvoice);
        }
        #endregion

        #region GetPI
        [HttpGet]
        public JsonResult GetPI(int id)
        {
            ProformaInvoice  oProformaInvoice  = new ProformaInvoice ();
            try
            {
                oProformaInvoice = oProformaInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProformaInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProformaInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        
        #endregion

        #region Wait for Revise
        [HttpGet]
        public JsonResult WaitForRevise(int buid, double ts)
        {
            string sSQL = "SELECT * FROM View_ProformaInvoice Where PIStatus = " + (int)EnumPIStatus.RequestForRevise+" AND BUID ="+buid;
            try
            {
                _oProformaInvoices = new List<ProformaInvoice>();
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

        #region Waiting Search
        [HttpGet]
        public JsonResult WaitingSearch(int buid)
        {
            _oProformaInvoices = new List<ProformaInvoice>();
            string sSQL = "SELECT * FROM View_ProformaInvoice WHERE  PIStatus = " + (int)EnumPIStatus.RequestForApproved + " AND ProformaInvoiceID IN (SELECT OperationObjectID FROM ApprovalRequest WHERE OperationType = " + (int)EnumApprovalRequestOperationType.ProformaInvoice+ " AND RequestTo = " + ((User)Session[SessionInfo.CurrentUser]).UserID + ") AND BUID ="+buid;
            try
            {
                #region User Set
                if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                {
                    sSQL += " AND BuyerID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
                }
                #endregion
                _oProformaInvoices  = ProformaInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oProformaInvoice = new ProformaInvoice();
                _oProformaInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProformaInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PI Revise Hitory Picker
        [HttpPost]
        public JsonResult GetProformaInvoiceReviseHistory(ProformaInvoice oProformaInvoice)
        {
            _oProformaInvoices = new List<ProformaInvoice>();
            try
            {
                _oProformaInvoices = ProformaInvoice.GetsPILog(oProformaInvoice.ProformaInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProformaInvoice = new ProformaInvoice();
                _oProformaInvoice.ErrorMessage = ex.Message;
                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProformaInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PI Order Picker
        public ActionResult PIOrderPicker(int id, double ts) // id is Master LCID
        {
            _oProformaInvoiceDetail = new ProformaInvoiceDetail();
            string sSQL = "SELECT * FROM  View_ProformaInvoiceDetail WHERE ProformaInvoiceID IN (SELECT ProformaInvoiceID FROM MasterLCDetail WHERE MasterLCID =" + id + " AND YetToTransfer > 0)";
            _oProformaInvoiceDetail.ProformaInvoiceDetails = ProformaInvoiceDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oProformaInvoiceDetail);
        }
        #endregion

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(ProformaInvoice oProformaInvoice)
        {
            _oProformaInvoice = new ProformaInvoice();
            _oProformaInvoice = oProformaInvoice;
            try
            {
                if (oProformaInvoice.ActionTypeExtra == "RequestForApproved")
                {
                    _oProformaInvoice.ProformaInvoiceActionType = EnumProformaInvoiceActionType.RequestForApproval;
                    _oProformaInvoice.ApprovalRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                    _oProformaInvoice.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.ProformaInvoice;

                }
                else if (oProformaInvoice.ActionTypeExtra == "UndoRequest")
                {

                    _oProformaInvoice.ProformaInvoiceActionType = EnumProformaInvoiceActionType.UndoRequest;

                }
                else if (oProformaInvoice.ActionTypeExtra == "Approve")
                {
                      
                    _oProformaInvoice.ProformaInvoiceActionType = EnumProformaInvoiceActionType.Approve;

                }
                else if (oProformaInvoice.ActionTypeExtra == "UndoApprove")
                {

                    _oProformaInvoice.ProformaInvoiceActionType = EnumProformaInvoiceActionType.UndoApprove;
                }
                else if (oProformaInvoice.ActionTypeExtra == "PI_In_Buyer_Hand")
                {

                    _oProformaInvoice.ProformaInvoiceActionType = EnumProformaInvoiceActionType.PI_In_Buyer_Hand;
                }
                else if (oProformaInvoice.ActionTypeExtra == "RequestForRevise")
                {
                    _oProformaInvoice.ProformaInvoiceActionType = EnumProformaInvoiceActionType.RequestForRevise;
                    _oProformaInvoice.ReviseRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                    _oProformaInvoice.ReviseRequest.OperationType = EnumReviseRequestOperationType.ProformaInvoice;
                }
                else if (oProformaInvoice.ActionTypeExtra == "Cancel")
                {

                    _oProformaInvoice.ProformaInvoiceActionType = EnumProformaInvoiceActionType.Cancel;
                }

                _oProformaInvoice.Note = oProformaInvoice.Note;
                _oProformaInvoice.OperationBy = oProformaInvoice.OperationBy;
                oProformaInvoice = SetPIStatus(_oProformaInvoice);
                _oProformaInvoice = oProformaInvoice.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProformaInvoice = new ProformaInvoice();
                _oProformaInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProformaInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private ProformaInvoice SetPIStatus(ProformaInvoice oProformaInvoice)//Set EnumOrderStatus Value
        {
            switch (oProformaInvoice.PIStatusInInt)
            {
                case 1:
                    {
                        oProformaInvoice.PIStatus = EnumPIStatus.Initialized;
                        break;
                    }
                case 2:
                    {
                        oProformaInvoice.PIStatus = EnumPIStatus.RequestForApproved;
                        break;
                    }
                case 3:
                    {
                        oProformaInvoice.PIStatus = EnumPIStatus.Approved;
                        break;
                    }

                case 4:
                    {
                        oProformaInvoice.PIStatus = EnumPIStatus.Approved;
                        break;
                    }
                case 5:
                    {
                        oProformaInvoice.PIStatus = EnumPIStatus.RequestForRevise;
                        break;
                    }
                case 6:
                    {
                        oProformaInvoice.PIStatus = EnumPIStatus.BindWithLC;
                        break;
                    }
                case 7:
                    {
                        oProformaInvoice.PIStatus = EnumPIStatus.Cancel;
                        break;
                    }
            }

            return oProformaInvoice;
        }
        #endregion
        #endregion

        #region Advance Search
       
        #region HttpGet For Search
        [HttpGet]
        public JsonResult Gets(string sTemp)
        {
            List<ProformaInvoice> oProformaInvoices = new List<ProformaInvoice>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oProformaInvoices = ProformaInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProformaInvoice = new ProformaInvoice();
                _oProformaInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProformaInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dIssueEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            string sPINo = sTemp.Split('~')[3];
            string sStyleNo = sTemp.Split('~')[4];
            string sBuyerIDs = sTemp.Split('~')[5];
            string sOrderRecapIDs= sTemp.Split('~')[6];

            int nPaymentTermID = Convert.ToInt32(sTemp.Split('~')[7]);
            int nBankAccountID = Convert.ToInt32(sTemp.Split('~')[8]);
            int nBusinessSessionID = Convert.ToInt32(sTemp.Split('~')[9]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_ProformaInvoice";
            string sReturn = "";

            #region PI No

            if (sPINo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PINo ='" + sPINo + "'";

            }
            #endregion

            #region Style  No

            if (sStyleNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProformaInvoiceID IN (SELECT ProformaInvoiceID FROM View_ProformaInvoiceDetail WHERE StyleNo = '" + sStyleNo+"')";
            }
            #endregion

            #region Buyer wise

            if (sBuyerIDs!="")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs+")";
            }
            #endregion

            #region Merchandiser Name

            if (sOrderRecapIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProformaInvoiceID IN (SELECT ProformaInvoiceID FROM ProformaInvoiceDetail WHERE OrderRecapID IN ("+ sOrderRecapIDs+"))" ;
            }
            #endregion

            #region Payment Term
            if (nPaymentTermID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PaymentTerm ="+ nPaymentTermID;

            }
            #endregion

            #region Bank Account
            if (nBankAccountID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TransferBankAccountID =" + nBankAccountID;

            }
            #endregion
            #region Business Session
            if (nBusinessSessionID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProformaInvoiceID IN (SELECT ProformaInvoiceID FROM ProformaInvoiceDetail WHERE OrderRecapID IN (SELECT OrderRecapID FROM OrderRecap WHERE BusinessSessionID = "+nBusinessSessionID+" ))";

            }
            #endregion

            #region Date Wise
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate != '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate>= '" + dIssueStartDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dIssueEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate< '" + dIssueStartDate.ToString("dd MMM yyyy") + "' OR IssueDate > '" + dIssueEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region BU

            if (nBUID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID ;
            }
            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BuyerID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
            }
            #endregion


            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #endregion

        #region document Type Picker
        public ActionResult DocumentTypePicker(double ts)
        {
            List<EnumObject> oDocumentTypes = new List<EnumObject>();
            oDocumentTypes = EnumObject.jGets(typeof(EnumDocumentType));
            oDocumentTypes.Remove(oDocumentTypes[0]);// there remove None Type value 
            return PartialView(oDocumentTypes);
        }

        

        #endregion

        #region Report
        public ActionResult MISReports(string Param)
        {
            string sProformaInvoiceIDs = Param.Split('~')[0];
            int nReportType = Convert.ToInt32(Param.Split('~')[1]);
            string sReportNo = Param.Split('~')[2];
            _oProformaInvoices = new List<ProformaInvoice>();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            string sSQL = "SELECT * FROM View_ProformaInvoice WHERE ProformaInvoiceID IN ( " + sProformaInvoiceIDs + " )";
           switch (nReportType)
            {
                case (int)EnumReportLayout.PartyWise:
                    {

                        sSQL += " Order By BuyerID";
                        _oProformaInvoices = ProformaInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        rptBuyerWiseExportPIList oReport = new rptBuyerWiseExportPIList();
                        byte[] abytes = oReport.PrepareReport(_oProformaInvoices, oCompany, sReportNo);
                        return File(abytes, "application/pdf");
                    };
                case (int)EnumReportLayout.Month_Wise:
                    {
                        sSQL += " Order By YEAR(IssueDate), MONTH(IssueDate)";
                        _oProformaInvoices = ProformaInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        rptMonthWiseExportPIList oReport = new rptMonthWiseExportPIList();
                        byte[] abytes = oReport.PrepareReport(_oProformaInvoices, oCompany, sReportNo);
                        return File(abytes, "application/pdf");
                    };
                default:
                    return RedirectToAction("MessageHelper", "User", new { message = "Sorry there is no Report" });
            }

        }

        #region Printing
        public ActionResult PrintPIList(string sIDs)
        {
            _oProformaInvoice = new  ProformaInvoice();
            string sSQL = "SELECT * FROM View_ProformaInvoice WHERE ProformaInvoiceID IN ("+ sIDs+")";
            _oProformaInvoice.ProformaInvoiceList = ProformaInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptProformaInvoiceList oReport = new rptProformaInvoiceList();
            byte[] abytes = oReport.PrepareReport(_oProformaInvoice, oCompany);
            return File(abytes, "application/pdf");
        }     
        public ActionResult PrintProformaInvoicePreview(int id)
        {
            _oProformaInvoice = new ProformaInvoice();
            _oClientOperationSetting = new ClientOperationSetting();
            _oProformaInvoice = _oProformaInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oProformaInvoice.ProformaInvoiceDetails = ProformaInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oProformaInvoice.ProformaInvoiceRequiredDocs = ProformaInvoiceRequiredDoc.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oProformaInvoice.ProformaInvoiceTermsAndConditions = ProformaInvoiceTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            
            _oClientOperationSetting = _oClientOperationSetting.Get(1, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            byte[] abytes = null;
            if(_oClientOperationSetting.OperationType == EnumOperationType.PIFormat)
            {
                rptProformaInvoice oReport = new rptProformaInvoice();
                abytes = oReport.PrepareReport(_oProformaInvoice, oCompany);
            }
            else
            {
                Contractor oApplicant = new Contractor();
                _oProformaInvoice.Applicant = oApplicant.Get(_oProformaInvoice.ApplicantID, (int)Session[SessionInfo.currentUserID]);
                rptProformaInvoiceWithApplicant oReport = new rptProformaInvoiceWithApplicant();
                abytes = oReport.PrepareReport(_oProformaInvoice, oCompany);
            }
            return File(abytes, "application/pdf");
        }

        public ActionResult PIHistoryPreview(int id)
        {
            List<ProformaInvoiceHistory> _oProformaInvoiceHistorys = new List<ESimSol.BusinessObjects.ProformaInvoiceHistory>();
            _oProformaInvoiceHistorys = ESimSol.BusinessObjects.ProformaInvoiceHistory.Gets(id, (int)Session[SessionInfo.currentUserID]);            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptPIHistory oReport = new rptPIHistory();
            byte[] abytes = oReport.PrepareReport(_oProformaInvoiceHistorys, oCompany);
            return File(abytes, "application/pdf");
        }     

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                //img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public ActionResult PrintProformaInvoiceLogPreview(int id)
        {
            _oProformaInvoice = new ProformaInvoice();
            _oProformaInvoice = _oProformaInvoice.GetLog(id, (int)Session[SessionInfo.currentUserID]);
            _oProformaInvoice.ProformaInvoiceDetails = ProformaInvoiceDetail.GetsPILog(id, (int)Session[SessionInfo.currentUserID]);
            _oProformaInvoice.ProformaInvoiceRequiredDocs = ProformaInvoiceRequiredDoc.GetsPILog(id, (int)Session[SessionInfo.currentUserID]);
            _oProformaInvoice.ProformaInvoiceTermsAndConditions = ProformaInvoiceTermsAndCondition.GetsPILog(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptProformaInvoice oReport = new rptProformaInvoice();
            byte[] abytes = oReport.PrepareReport(_oProformaInvoice, oCompany);
            return File(abytes, "application/pdf");
        }     
        #endregion

        #endregion

        #region Search Style OR Buyer by Press Enter
        [HttpGet]
        public JsonResult SearchStyleAndBuyer(string sTempData, int buid, bool bIsStyle, double ts)
        {
            _oProformaInvoices = new List<ProformaInvoice>();
            string sSQL = "";
            if (bIsStyle == true)
            {
                sSQL = "SELECT * FROM  View_ProformaInvoice AS PI Where PI.ProformaInvoiceID IN (SELECT PID.ProformaInvoiceID FROm View_ProformaInvoiceDetail AS PID WHERE PID.StyleNo LIke ('%" + sTempData + "%')) ";
            }
            else
            {
                sSQL = "SELECT * FROM View_ProformaInvoice WHERE BuyerName LIKE ('%" + sTempData + "%')";
            }
            sSQL += " AND BUID = " + buid;
            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                sSQL += " AND  ProformaInvoiceID IN (SELECT PID.ProformaInvoiceID FROM View_ProformaInvoiceDetail AS PID WHERE  PID.TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + ")))";
            }
            #endregion
            try
            {
                ProformaInvoice oProformaInvoice = new ProformaInvoice();
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

    } 
}
