using System;
using System.Text;
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

using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
namespace ESimSolFinancial.Controllers
{

    public class CostSheetController : Controller
    {

        #region Declartion
        CostSheet _oCostSheet = new CostSheet();
        List<CostSheet> _oCostSheets = new List<CostSheet>();
        CostSheetDetail _oCostSheetDetail = new CostSheetDetail();
        List<CostSheetDetail> _oCostSheetDetails = new List<CostSheetDetail>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        CostSheetPackage _oCostSheetPackage = new CostSheetPackage();
        List<CostSheetPackage> _oCostSheetPackages = new List<CostSheetPackage>();
        CostSheetPackageDetail _oCostSheetPackageDetail = new CostSheetPackageDetail();
        List<CostSheetPackageDetail> _oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
        #endregion

        #region function
        private bool HaveRateViewPermission(EnumRoleOperationType OperationType)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CostSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            if ((int)Session[SessionInfo.currentUserID] == -9)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < oAuthorizationRoleMappings.Count; i++)
                {
                    if (oAuthorizationRoleMappings[i].OperationType == OperationType && oAuthorizationRoleMappings[i].ModuleName == EnumModuleName.CostSheet)
                    {
                        return true;

                    }

                }
            }

            return false;
        }

        private List<CostSheetDetail> GetSpecificDetails(List<CostSheetDetail> oCostSheetDetails, bool IsYarn)
        {
            _oCostSheetDetails = new List<CostSheetDetail>();

            foreach (CostSheetDetail oItem in oCostSheetDetails)
            {
                if (IsYarn == true)
                {
                    if (oItem.MaterialType == EnumCostSheetMeterialType.Yarn)
                    {
                        _oCostSheetDetails.Add(oItem);
                    }
                }
                else
                {
                    if (oItem.MaterialType == EnumCostSheetMeterialType.Accessories)
                    {
                        _oCostSheetDetails.Add(oItem);
                    }
                }
            }

            return _oCostSheetDetails;
        }

        private List<CostSheetPackageDetail> GetPackageDetails(int nCPID, List<CostSheetPackageDetail> oTempCostSheetPackageDetails)
        {
            _oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
            foreach (CostSheetPackageDetail oItem in oTempCostSheetPackageDetails)
            {
                if (oItem.CostSheetPackageID == nCPID)
                {
                    _oCostSheetPackageDetails.Add(oItem);
                }
            }
            return _oCostSheetPackageDetails;
        }
        #endregion

        #region Cost Sheet Management
        public ActionResult ViewCostSheetMgt(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            _oCostSheets = new List<CostSheet>();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CostSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            string sSQL = "SELECT * FROM View_CostSheet WHERE CostSheetStatus=1";
            if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " AND BUID =" + buid;
            }
            //_oCostSheets = CostSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CostSheetTypes = EnumObject.jGets(typeof(EnumCostSheetType));
            ViewBag.BUID = buid;

            List<User> oUsers = new List<ESimSol.BusinessObjects.User>();
            string sSql = "SELECT * FROM View_User WHERE UserID IN (SELECT Distinct ApprovedBy FROM View_CostSheet)";
            //ViewBag.Employees = Employee.Gets(EnumEmployeeType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Users = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessSessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.CostSheetStatusList = EnumObject.jGets(typeof(EnumCostSheetStatus));
            return View(_oCostSheets);
        }

        #endregion


        #region Add, Edit, Delete

        #region Cost Sheet Entry
        public ActionResult ViewCostSheet(int id)
        {
            _oCostSheet = new CostSheet();
            if (id > 0)
            {
                _oCostSheet = _oCostSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetDetails = CostSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
                _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
            }
            else
            {
                _oCostSheet.CostSheetDetails = new List<CostSheetDetail>();
            }
            _oCostSheet.CountUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.WeightUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CostSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return View(_oCostSheet);
        }
        public ActionResult ViewCostSheetWoven(int id)
        {
            _oCostSheet = new CostSheet();
            List<CostSheetPackageDetail> oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
            if (id > 0)
            {
                _oCostSheet = _oCostSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetCMs = CostSheetCM.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetDetails = CostSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
                _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
                _oCostSheet.CostSheetStepDetails = _oCostSheet.CostSheetDetails.Where(x => x.MaterialType == EnumCostSheetMeterialType.Production_Step).ToList();
                _oCostSheet.CostSheetPackages = CostSheetPackage.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oCostSheetPackageDetails = CostSheetPackageDetail.GetsByCostSheetID(id, (int)Session[SessionInfo.currentUserID]);
                foreach (CostSheetPackage oItem in _oCostSheet.CostSheetPackages)
                {
                    oItem.CostSheetPackageDetails = GetPackageDetails(oItem.CostSheetPackageID, oCostSheetPackageDetails);
                }
            }
            else
            {
                _oCostSheet.CostSheetDetails = new List<CostSheetDetail>();
            }
            _oCostSheet.CountUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.WeightUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            _oCostSheet.ProductionSteps = ProductionStep.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CMTypes = EnumObject.jGets(typeof(EnumCMType));
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CostSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return View(_oCostSheet);
        }
        #region ViewCostSheetPackage
        public ActionResult ViewCostSheetPackage()
        {
            _oCostSheetPackage = new CostSheetPackage();
            return PartialView(_oCostSheetPackage);
        }
        #endregion

        #endregion

        #region Cost Sheet Revise
        public ActionResult ViewCostSheetRevise(int id, int OperationType)
        {
            _oCostSheet = new CostSheet();
            if (id > 0)
            {
                _oCostSheet = _oCostSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetDetails = CostSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
                _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
            }
            _oCostSheet.CountUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.WeightUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.OperationType = OperationType;
            return View(_oCostSheet);
        }
        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(CostSheet oCostSheet)
        {
            _oCostSheet = new CostSheet();
            List<CostSheetDetail> oCostSheetDetails = new List<CostSheetDetail>();
            try
            {
                _oCostSheet = oCostSheet;
                foreach (CostSheetDetail oItem in oCostSheet.CostSheetYarnDetails)
                {
                    _oCostSheetDetail = new CostSheetDetail();
                    _oCostSheetDetail = oItem;
                    oCostSheetDetails.Add(_oCostSheetDetail);
                }
                foreach (CostSheetDetail oItem in oCostSheet.CostSheetAccessoriesDetails)
                {
                    _oCostSheetDetail = new CostSheetDetail();
                    _oCostSheetDetail = oItem;
                    oCostSheetDetails.Add(_oCostSheetDetail);
                }

                _oCostSheet.CostSheetDetails = oCostSheetDetails;
                _oCostSheet.CostSheetType = (EnumCostSheetType)_oCostSheet.CostSheetTypeInInt;
                _oCostSheet = _oCostSheet.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostSheet = new CostSheet();
                _oCostSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP AcceptCostSheetRevise
        [HttpPost]
        public JsonResult AcceptCostSheetRevise(CostSheet oCostSheet)
        {
            _oCostSheet = new CostSheet();
            List<CostSheetDetail> oCostSheetDetails = new List<CostSheetDetail>();
            try
            {
                _oCostSheet = oCostSheet;
                foreach (CostSheetDetail oItem in oCostSheet.CostSheetYarnDetails)
                {
                    _oCostSheetDetail = new CostSheetDetail();
                    _oCostSheetDetail = oItem;
                    oCostSheetDetails.Add(_oCostSheetDetail);
                }
                foreach (CostSheetDetail oItem in oCostSheet.CostSheetAccessoriesDetails)
                {
                    _oCostSheetDetail = new CostSheetDetail();
                    _oCostSheetDetail = oItem;
                    oCostSheetDetails.Add(_oCostSheetDetail);
                }
                _oCostSheet.CostSheetDetails = oCostSheetDetails;
                _oCostSheet = _oCostSheet.AcceptCostSheetRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostSheet = new CostSheet();
                _oCostSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP GET Delete
        [HttpPost]
        public JsonResult Delete(CostSheet oCostSheet)
        {
            string smessage = "";
            try
            {
                smessage = oCostSheet.Delete(oCostSheet.CostSheetID, (int)Session[SessionInfo.currentUserID]);
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

        #region LoadCostingInfo
        [HttpPost]
        public JsonResult LoadCostingInfo(TechnicalSheet oTechnicalSheet)
        {
            _oCostSheet = new CostSheet();
            _oCostSheets = new List<CostSheet>();
            string sSQL = "SELECT top 1 * FROM View_CostSheet WHERE TechnicalSheetID = " + oTechnicalSheet.TechnicalSheetID;
            List<CostSheetPackageDetail> oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
            try
            {

                _oCostSheets = CostSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oCostSheets.Count > 0)
                {

                    _oCostSheet = _oCostSheets[0];
                    _oCostSheet.CostSheetDetails = CostSheetDetail.Gets(_oCostSheet.CostSheetID, (int)Session[SessionInfo.currentUserID]);
                    foreach (CostSheetDetail oItem in _oCostSheet.CostSheetDetails) { oItem.CostSheetID = 0; oItem.CostSheetDetailID = 0; }//Reset id
                    _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
                    _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
                    _oCostSheet.CostSheetStepDetails = _oCostSheet.CostSheetDetails.Where(x => x.MaterialType == EnumCostSheetMeterialType.Production_Step).ToList();
                    _oCostSheet.CostSheetPackages = CostSheetPackage.Gets(_oCostSheet.CostSheetID, (int)Session[SessionInfo.currentUserID]);
                    oCostSheetPackageDetails = CostSheetPackageDetail.GetsByCostSheetID(_oCostSheet.CostSheetID, (int)Session[SessionInfo.currentUserID]);
                    foreach (CostSheetPackage oItem in _oCostSheet.CostSheetPackages)
                    {
                        oItem.CostSheetPackageDetails = GetPackageDetails(oItem.CostSheetPackageID, oCostSheetPackageDetails);
                        //Reset Region
                        oItem.CostSheetID = 0; ; oItem.CostSheetPackageID = 0;
                        foreach (CostSheetPackageDetail oDItem in oItem.CostSheetPackageDetails) { oDItem.CostSheetPackageID = 0; oDItem.CostSheetPackageDetailID = 0; }
                    }

                    //Retset
                    _oCostSheet.CostSheetID = 0;//Reset for new


                }
            }
            catch (Exception ex)
            {
                _oCostSheet = new CostSheet();
                _oCostSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Cost Sheet Hitory Picker

        [HttpPost]
        public JsonResult GetCostSheetHistory(CostSheet oCostSheet)
        {
            List<CostSheetHistory> oCostSheetHistorys = new List<CostSheetHistory>();

            try
            {
                oCostSheetHistorys = ESimSol.BusinessObjects.CostSheetHistory.Gets(oCostSheet.CostSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                CostSheetHistory oCostSheetHistory = new ESimSol.BusinessObjects.CostSheetHistory();
                oCostSheetHistory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCostSheetHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Waiting Search
        [HttpPost]
        public JsonResult WaitingSearch(CostSheet oCostSheet)
        {
            _oCostSheets = new List<CostSheet>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            StringBuilder sSQL = new StringBuilder("SELECT * FROM View_CostSheet WHERE CostSheetStatus = " + (int)EnumCostSheetStatus.Req_For_App + " AND CostSheetID IN (SELECT OperationObjectID FROM ApprovalRequest WHERE  RequestTo = " + ((User)Session[SessionInfo.CurrentUser]).UserID + ")");
            try
            {
                if (oCostSheet.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL.Append(" AND BUID = " + oCostSheet.BUID);
                }

                #region User Set
                if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                {

                    sSQL.Append(" AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))");
                }
                #endregion
                _oCostSheets = CostSheet.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostSheet = new CostSheet();
                _oCostSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Hitory Gets
        [HttpPost]
        public JsonResult GetCostSheetReviseHistory(CostSheet oCostSheet)
        {
            _oCostSheets = new List<CostSheet>();
            try
            {
                _oCostSheets = CostSheet.GetsCostSheetLog(oCostSheet.CostSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostSheet = new CostSheet();
                _oCostSheet.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cost Sheet Log
        public ActionResult ViewCostSheetLog(int id, double ts) // id is log id
        {
            _oCostSheet = new CostSheet();
            if (id > 0)
            {
                _oCostSheet = _oCostSheet.GetLog(id, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetDetails = CostSheetDetail.GetsCostSheetLog(id, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
                _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
            }
            _oCostSheet.CountUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.WeightUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CostSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return PartialView(_oCostSheet);
        }
        #endregion

        #region HTTP Approve
        [HttpPost]
        public JsonResult Approve(CostSheet oCostSheet)
        {
            _oCostSheet = new CostSheet();
            List<CostSheetDetail> oCostSheetDetails = new List<CostSheetDetail>();
            try
            {
                _oCostSheet = oCostSheet;
                _oCostSheet.StatusInInt = (int)EnumCostSheetStatus.Req_For_App;
                foreach (CostSheetDetail oItem in oCostSheet.CostSheetYarnDetails)
                {
                    _oCostSheetDetail = new CostSheetDetail();
                    _oCostSheetDetail = oItem;
                    oCostSheetDetails.Add(_oCostSheetDetail);
                }
                foreach (CostSheetDetail oItem in oCostSheet.CostSheetAccessoriesDetails)
                {
                    _oCostSheetDetail = new CostSheetDetail();
                    _oCostSheetDetail = oItem;
                    oCostSheetDetails.Add(_oCostSheetDetail);
                }
                _oCostSheet.CostSheetDetails = oCostSheetDetails;
                _oCostSheet = _oCostSheet.Save((int)Session[SessionInfo.currentUserID]);
                _oCostSheet.CostSheetActionType = EnumCostSheetActionType.Approve;
                _oCostSheet = SetCostSheetStatus(_oCostSheet);
                _oCostSheet.ApprovalRequest = new ApprovalRequest();
                _oCostSheet = _oCostSheet.ChangeStatus(_oCostSheet.ApprovalRequest, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostSheet = new CostSheet();
                _oCostSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(CostSheet oCostSheet)
        {
            _oCostSheet = new CostSheet();
            _oCostSheet = oCostSheet;
            try
            {
                if (oCostSheet.ActionTypeExtra == "RequestForApproved")
                {

                    _oCostSheet.CostSheetActionType = EnumCostSheetActionType.RequestForApproval;

                }
                else if (oCostSheet.ActionTypeExtra == "UndoRequest")
                {

                    _oCostSheet.CostSheetActionType = EnumCostSheetActionType.UndoRequest;

                }
                //else if (oCostSheet.ActionTypeExtra == "Approve")
                //{

                //    _oCostSheet.CostSheetActionType = EnumCostSheetActionType.Approve;

                //}
                else if (oCostSheet.ActionTypeExtra == "UndoApprove")
                {

                    _oCostSheet.CostSheetActionType = EnumCostSheetActionType.UndoApprove;
                }

                else if (oCostSheet.ActionTypeExtra == "RequestForRevise")
                {

                    _oCostSheet.CostSheetActionType = EnumCostSheetActionType.Request_revise;
                }
                else if (oCostSheet.ActionTypeExtra == "Cancel")
                {

                    _oCostSheet.CostSheetActionType = EnumCostSheetActionType.Cancel;
                }

                //_oCostSheet.Note = oCostSheet.Note;
                //_oCostSheet.OperationBy = oCostSheet.OperationBy;
                oCostSheet = SetCostSheetStatus(_oCostSheet);

                if (oCostSheet.ActionTypeExtra == "RequestForApproved") // for SEt Approval Request Value
                {
                    oCostSheet.ApprovalRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                    oCostSheet.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.CostSheet;

                }
                else
                {
                    oCostSheet.ApprovalRequest = new ApprovalRequest();
                }

                _oCostSheet = oCostSheet.ChangeStatus(oCostSheet.ApprovalRequest, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostSheet = new CostSheet();
                _oCostSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private CostSheet SetCostSheetStatus(CostSheet oCostSheet)//Set EnumOrderStatus Value
        {
            switch (oCostSheet.StatusInInt)
            {
                case 1:
                    {
                        oCostSheet.CostSheetStatus = EnumCostSheetStatus.Initialized;
                        break;
                    }
                case 2:
                    {
                        oCostSheet.CostSheetStatus = EnumCostSheetStatus.Req_For_App;
                        break;
                    }
                case 3:
                    {
                        oCostSheet.CostSheetStatus = EnumCostSheetStatus.Approved;
                        break;
                    }

                case 4:
                    {
                        oCostSheet.CostSheetStatus = EnumCostSheetStatus.RequestForRevise;
                        break;
                    }
                case 5:
                    {
                        oCostSheet.CostSheetStatus = EnumCostSheetStatus.Cancel;
                        break;
                    }
            }

            return oCostSheet;
        }
        #endregion
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
            List<CostSheet> oCostSheets = new List<CostSheet>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oCostSheets = CostSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostSheet = new CostSheet();
                _oCostSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCostSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region GetSQL
        private string GetSQL(string sTemp)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            int nCostingDateComboValue = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dStartCositngDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dEndCostingDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            int nShipmentDateComboValue = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dStartShipmentDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dEndShipmentDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            string sFileNo = sTemp.Split('~')[6];
            string sTechnicalSheetIDs = sTemp.Split('~')[7];
            int nApproveByID = Convert.ToInt32(sTemp.Split('~')[8]);
            string nMerchandiserIDs = sTemp.Split('~')[9];
            string sStatus = sTemp.Split('~')[10];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[11]);
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[12]);
            string nBuyerIDs = sTemp.Split('~')[13];
            string nDepartmentIDs = sTemp.Split('~')[14];


            string sReturn1 = "SELECT * FROM View_CostSheet";
            string sReturn = "";

            #region File No

            if (sFileNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FileNo ='" + sFileNo + "'";
            }
            #endregion

            #region MerchandiserID
            if (!string.IsNullOrEmpty(nMerchandiserIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID IN (" + nMerchandiserIDs + ")";
            }
            #endregion

            #region Buyer
            if (!string.IsNullOrEmpty(nBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + nBuyerIDs + ")";
            }
            #endregion
            #region DepartmentID
            if (!string.IsNullOrEmpty(nDepartmentIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Dept IN (" + nDepartmentIDs + ")";
            }
            #endregion

            #region Approve ByID
            if (nApproveByID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApprovedBy = " + nApproveByID;
            }
            #endregion


            #region SessionID
            if (nSessionID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + nSessionID;
            }
            #endregion



            #region Technical Sheets
            if (sTechnicalSheetIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TechnicalSheetID IN (" + sTechnicalSheetIDs + ")";
            }
            #endregion

            #region Status
            if (sStatus != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CostSheetStatus IN (" + sStatus + ")";
            }
            #endregion

            #region Costing  Date Wise
            if (nCostingDateComboValue > 0)
            {
                if (nCostingDateComboValue == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate = '" + dStartCositngDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate != '" + dStartCositngDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate > '" + dStartCositngDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate < '" + dStartCositngDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate>= '" + dStartCositngDate.ToString("dd MMM yyyy") + "' AND CostingDate < '" + dEndCostingDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate< '" + dStartCositngDate.ToString("dd MMM yyyy") + "' OR CostingDate > '" + dEndCostingDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region Shipment Date Wise
            if (nShipmentDateComboValue > 0)
            {
                if (nShipmentDateComboValue == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate = '" + dStartShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate != '" + dStartShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate > '" + dStartShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate < '" + dStartShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate>= '" + dStartShipmentDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dEndShipmentDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate< '" + dStartShipmentDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dEndShipmentDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region BU
            if (nBUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += "  TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY CostSheetID";
            return sReturn;
        }
        #endregion


        #endregion


        #endregion


        public ActionResult PrintCostSheetList(string sIDs)
        {
            _oCostSheet = new CostSheet();
            string sSQL = "SELECT * FROM View_CostSheet WHERE CostSheetID IN (" + sIDs + ")";
            _oCostSheet.CostSheetList = CostSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptCostSheetList oReport = new rptCostSheetList();
            byte[] abytes = oReport.PrepareReport(_oCostSheet, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintCostSheetPreview(int id)
        {
            _oCostSheet = new CostSheet();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            _oCostSheet = _oCostSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetCMs = CostSheetCM.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(_oCostSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetDetails = CostSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
            _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptCostSheet oReport = new rptCostSheet();
            byte[] abytes = oReport.PrepareReport(_oCostSheet, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult CostSheetWovenPreview(int id)
        {
            _oCostSheet = new CostSheet();
            List<CostSheetPackageDetail> oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            _oCostSheet = _oCostSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetCMs = CostSheetCM.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(_oCostSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetDetails = CostSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
            _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
            _oCostSheet.CostSheetStepDetails = _oCostSheet.CostSheetDetails.Where(x => x.MaterialType == EnumCostSheetMeterialType.Production_Step).ToList();
            _oCostSheet.CostSheetPackages = CostSheetPackage.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oCostSheetPackageDetails = CostSheetPackageDetail.GetsByCostSheetID(id, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCostSheet.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (CostSheetPackage oItem in _oCostSheet.CostSheetPackages)
            {
                oItem.CostSheetPackageDetails = GetPackageDetails(oItem.CostSheetPackageID, oCostSheetPackageDetails);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptCostSheetWoven oReport = new rptCostSheetWoven();
            byte[] abytes = oReport.PrepareReport(_oCostSheet, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult CostSheetWovenPreviewWithBodyPart(int id)
        {
            _oCostSheet = new CostSheet();
            List<CostSheetPackageDetail> oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            _oCostSheet = _oCostSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(_oCostSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.TechnicalSheetSizeList = TechnicalSheetSize.Gets(_oCostSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);

            _oCostSheet.CostSheetCMs = CostSheetCM.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetDetails = CostSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
            _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
            _oCostSheet.CostSheetStepDetails = _oCostSheet.CostSheetDetails.Where(x => x.MaterialType == EnumCostSheetMeterialType.Production_Step).ToList();
            _oCostSheet.CostSheetPackages = CostSheetPackage.Gets(id, (int)Session[SessionInfo.currentUserID]);

            oCostSheetPackageDetails = CostSheetPackageDetail.GetsByCostSheetID(id, (int)Session[SessionInfo.currentUserID]);
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.CostSheetPriview, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCostSheet.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (CostSheetPackage oItem in _oCostSheet.CostSheetPackages)
            {
                oItem.CostSheetPackageDetails = GetPackageDetails(oItem.CostSheetPackageID, oCostSheetPackageDetails);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<BodyMeasure> oBodyMeasures = new List<BodyMeasure>();
            oBodyMeasures = BodyMeasure.Gets(id, (int)Session[SessionInfo.currentUserID]);

            rptCostSheetWovenWithBodyPart oReport = new rptCostSheetWovenWithBodyPart();
            byte[] abytes = oReport.PrepareReport(_oCostSheet, oCompany, oBusinessUnit, oBodyMeasures, oSignatureSetups);
            return File(abytes, "application/pdf");
        }

        public ActionResult CostSheetWovenPreviewWithBodyPartLog(int id)//Log ID
        {
            _oCostSheet = new CostSheet();
            List<CostSheetPackageDetail> oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            _oCostSheet = _oCostSheet.GetLog(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(_oCostSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.TechnicalSheetSizeList = TechnicalSheetSize.Gets(_oCostSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetCMs = CostSheetCM.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetDetails = CostSheetDetail.GetsCostSheetLog(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
            _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
            _oCostSheet.CostSheetStepDetails = _oCostSheet.CostSheetDetails.Where(x => x.MaterialType == EnumCostSheetMeterialType.Production_Step).ToList();
            //_oCostSheet.CostSheetPackages = CostSheetPackage.(id, (int)Session[SessionInfo.currentUserID]);
            oCostSheetPackageDetails = CostSheetPackageDetail.GetsByCostSheetID(id, (int)Session[SessionInfo.currentUserID]);
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.CostSheetPriview, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCostSheet.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (CostSheetPackage oItem in _oCostSheet.CostSheetPackages)
            {
                oItem.CostSheetPackageDetails = GetPackageDetails(oItem.CostSheetPackageID, oCostSheetPackageDetails);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<BodyMeasure> oBodyMeasures = new List<BodyMeasure>();
            oBodyMeasures = BodyMeasure.Gets(id, (int)Session[SessionInfo.currentUserID]);

            rptCostSheetWovenWithBodyPart oReport = new rptCostSheetWovenWithBodyPart();
            byte[] abytes = oReport.PrepareReport(_oCostSheet, oCompany, oBusinessUnit, oBodyMeasures, oSignatureSetups);
            return File(abytes, "application/pdf");
        }



        public ActionResult CompareSheet(int id)
        {
            _oCostSheet = new CostSheet();
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            _oCostSheet = _oCostSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(_oCostSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            //GEt and Set Assumption Values
            _oCostSheet.CostSheetDetails = CostSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetCMs = CostSheetCM.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
            _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
            string sSQL = "SELECT top 1 * FROM View_OrderRecap WHERE TechnicalSheetID = " + _oCostSheet.TechnicalSheetID;
            oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            if (oOrderRecaps.Count > 0)
            {
                _oCostSheet.OrderRecap = oOrderRecaps[0];
                //GEt and Set Actual Values
                _oCostSheet.CostSheetDetails = CostSheetDetail.GetActualSheet(_oCostSheet.OrderRecap.OrderRecapID, (int)Session[SessionInfo.currentUserID]);
                _oCostSheet.ActualCostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
                _oCostSheet.ActualCostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
            }
            else
            {
                _oCostSheet.ActualCostSheetYarnDetails = new List<CostSheetDetail>();
                _oCostSheet.ActualCostSheetAccessoriesDetails = new List<CostSheetDetail>();
                _oCostSheet.OrderRecap = new OrderRecap();
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptCostCompareSheet oReport = new rptCostCompareSheet();
            byte[] abytes = oReport.PrepareReport(_oCostSheet, oCompany);
            return File(abytes, "application/pdf");
        }

        public void PrintExcelPreview(int id)
        {
            _oCostSheet = new CostSheet();
            List<CostSheetPackageDetail> oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            _oCostSheet = _oCostSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(_oCostSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.TechnicalSheetImage.TSImage = GetTechnicalSheetImage(_oCostSheet.TechnicalSheetImage);
            _oCostSheet.TechnicalSheetSizeList = TechnicalSheetSize.Gets(_oCostSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);

            _oCostSheet.CostSheetCMs = CostSheetCM.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetDetails = CostSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oCostSheet.CostSheetYarnDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, true);
            _oCostSheet.CostSheetAccessoriesDetails = GetSpecificDetails(_oCostSheet.CostSheetDetails, false);
            _oCostSheet.CostSheetStepDetails = _oCostSheet.CostSheetDetails.Where(x => x.MaterialType == EnumCostSheetMeterialType.Production_Step).ToList();
            _oCostSheet.CostSheetPackages = CostSheetPackage.Gets(id, (int)Session[SessionInfo.currentUserID]);

            oCostSheetPackageDetails = CostSheetPackageDetail.GetsByCostSheetID(id, (int)Session[SessionInfo.currentUserID]);
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.CostSheetPriview, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oCostSheet.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (CostSheetPackage oItem in _oCostSheet.CostSheetPackages)
            {
                oItem.CostSheetPackageDetails = GetPackageDetails(oItem.CostSheetPackageID, oCostSheetPackageDetails);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<BodyMeasure> oBodyMeasures = new List<BodyMeasure>();
            oBodyMeasures = BodyMeasure.Gets(id, (int)Session[SessionInfo.currentUserID]);
            this.ExcelPreview(_oCostSheet, oCompany, oBusinessUnit, oBodyMeasures, oSignatureSetups);

        }

        private void ExcelPreview(CostSheet oCostSheet, Company oCompany, BusinessUnit oBusinessUnit, List<BodyMeasure> oBodyMeasures, List<SignatureSetup> oSignatureSetups)
        {
            int rowIndex = 2;
            int nMaxColumn = 0;
            int colIndex = 1;
            ExcelRange cell;
            Border border;
            ExcelFill fill;
            double nTotalPercent = 0, nTotalValue = 0;
            string sQtyCell = "";
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Cost Sheet");
                sheet.Name = "Cost Sheet";
               // sheet.View.FreezePanes(9, 1);

                #region Declare Column
                colIndex = 0;
                sheet.Column(++colIndex).Width = 11;//SL
                sheet.Column(++colIndex).Width = 3;//Empty Col
                sheet.Column(++colIndex).Width = 18;//Item
                sheet.Column(++colIndex).Width = 18;//Description
                sheet.Column(++colIndex).Width = 10;//Unit
                sheet.Column(++colIndex).Width = 10;//Yarn price
                sheet.Column(++colIndex).Width = 10;//Knitting
                sheet.Column(++colIndex).Width = 10;//Dyeing
                sheet.Column(++colIndex).Width = 10;//Lycra
                sheet.Column(++colIndex).Width = 10;//AOP
                sheet.Column(++colIndex).Width = 10;//Wash
                sheet.Column(++colIndex).Width = 10;//Y/D
                sheet.Column(++colIndex).Width = 10;//Sueded
                sheet.Column(++colIndex).Width = 10;//Finish
                sheet.Column(++colIndex).Width = 10;//Brash
                sheet.Column(++colIndex).Width = 10;//Fabric Cost
                sheet.Column(++colIndex).Width = 10;//Con/Dz
                sheet.Column(++colIndex).Width = 10;//Con/Pc
                sheet.Column(++colIndex).Width = 10;//Cost/Dz
                sheet.Column(++colIndex).Width = 10;//Cost/Pc
                sheet.Column(++colIndex).Width = 10;//%
                sheet.Column(++colIndex).Width = 10;//Total Value
                nMaxColumn = colIndex;
                #endregion

                #region Report Header
                cell = sheet.Cells[rowIndex, 8, rowIndex++,15]; cell.Merge = true; cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                if(oBusinessUnit.Address!=null)
                {
                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = oBusinessUnit.Address; cell.Merge = true; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 8;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    rowIndex++;
                }
              if(oBusinessUnit.Phone.Length>0 )
              {
                  cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = "Tel: "+oBusinessUnit.Phone; cell.Merge = true;  cell.Style.Font.Bold = true; cell.Style.Font.Size = 8;
                  cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                  rowIndex++;
              }
              string BUEmailWeb = "";
                if(oBusinessUnit.Email!=null && oBusinessUnit.Email.Length>0)
                {
                    BUEmailWeb += "Email: " + oBusinessUnit.Email;
                }

                if (oBusinessUnit.WebAddress != null && oBusinessUnit.WebAddress.Length > 0)
                {
                    BUEmailWeb +=  oBusinessUnit.WebAddress;
                }
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = BUEmailWeb; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 8;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 10, rowIndex, 12]; cell.Value = "Cost Sheet"; cell.Merge = true; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true; cell.Style.Font.Size = 12;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rowIndex+=2;
                #endregion

                #region Report Data
                #region Buyer
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Buyer";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                int nStartBodyMeasureRow = rowIndex + 1;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = oCostSheet.BuyerName;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = "Fabrics";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = oCostSheet.YarnCategoryName;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = ""; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                int nStartBodyMeasureColIn = colIndex-1;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = "Meas In CM"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = "GSM"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                int nCount = 0;
                foreach (BodyMeasure oItem in oBodyMeasures)
                {
                    nCount = nCount + 1;
                    colIndex = nStartBodyMeasureColIn-1;
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = oItem.BodyPartName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = oItem.MeasureInCM; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,###.###";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = oItem.GSM; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,###.###";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                }

                if (nCount < 7)
                {
                    for (int n = nCount; n <= 7; n++)
                    {
                        colIndex = nStartBodyMeasureColIn-1;
                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                }
                #endregion
                #region Dept
                colIndex = 0;
                rowIndex = nStartBodyMeasureRow;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Dept";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = oCostSheet.DeptName; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = "Wt(GSM)";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = oCostSheet.GG;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion

                #region Style

                colIndex = 0;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Style";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = oCostSheet.StyleNo;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = "Count";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = oCostSheet.Count;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion

                #region Item
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Item";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = oCostSheet.GarmentsName;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                if (oCostSheet.TechnicalSheetImage.TSImage != null)
                {
                    int imCol=colIndex;
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 3, colIndex += 6]; cell.Merge = true; cell.Value = "";
                    cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Row(rowIndex).Height = 40; border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    ExcelPicture excelImage = null;
                    excelImage = sheet.Drawings.AddPicture("dfgdsg", oCostSheet.TechnicalSheetImage.TSImage);
                    excelImage.From.Column = imCol+3;
                    excelImage.From.Row = rowIndex-1;
                    excelImage.SetSize(90, 90);
                    excelImage.From.ColumnOff = this.Pixel2MTU(2);
                    excelImage.From.RowOff = this.Pixel2MTU(2);
                }
                else
                {
                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 3, colIndex += 6]; cell.Merge = true; cell.Value = "No Image "; 
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;

                #endregion
                #region Qty
                colIndex = 0;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Qty";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex];  cell.Value = oCostSheet.ApproxQty;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0";
                border = cell.Style.Border; border.Top.Style = border.Left.Style  = border.Bottom.Style = ExcelBorderStyle.Thin;
                sQtyCell = Global.GetExcelCellName(rowIndex, colIndex);

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true; 
                border = cell.Style.Border; border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion
                #region Color
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Color";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = oCostSheet.ColorRange;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                #region Size
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Size";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                string sSizeRange = "";
                foreach (TechnicalSheetSize oItem in oCostSheet.TechnicalSheetSizeList) { sSizeRange += oItem.QtyInPercent > 0 ? oItem.SizeCategoryName + " " + oItem.QtyInPercent.ToString() + "%" : oItem.SizeCategoryName; sSizeRange += ","; }
                if (sSizeRange.Length > 0) { sSizeRange = sSizeRange.Substring(0, sSizeRange.Length - 1); }

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = sSizeRange;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion

                #region Yarn Details
                rowIndex+=2;
                colIndex = 0;
                #region Yarn Title
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex + 2]; cell.Merge = true; cell.Value = "Fabric Description:"; cell.Style.Font.Bold = true; cell.Style.WrapText = true; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rowIndex++;
                #endregion

                #region Yarn Heading
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Item"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Description"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Y.Price(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Knitting(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Dyeing(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Lycra(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "AOP(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Wash(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Y/D(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Sued(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Finish(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Brush(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "F.Cost(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Con/Dz"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Con/Pc"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Price/Dz(" + _oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Price/Pc\n(" + _oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "%"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total Val(" + _oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion
                #region Yarn Value Print
                colIndex = 0;
                string[] Later = new string[] { "A", "B", "C", "D", "E", "F", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                nCount = 0; double nTotalFabricCostPerPc = 0;
                string sStartCell = "", sEndCell = "";
                int nStartCol = 0, nstartRow = 0, nEndRow = 0,nStartRowYarnDetail=0 ;
                if (oCostSheet.CostSheetYarnDetails.Count > 0)
                {
                    nstartRow = rowIndex;
                    nStartRowYarnDetail = rowIndex;
                    foreach (CostSheetDetail oItem in oCostSheet.CostSheetYarnDetails)
                    {
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Later[nCount++]; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.ProductName; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.Description; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.UnitSymbol;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.MaterialMarketPrice;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        nStartCol = colIndex;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.KnittingCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.DyeingCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.LycraCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.AOPCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.WashCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.YarnDyeingCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.SuedeCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.FinishingCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.BrushingCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(rowIndex, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex, colIndex);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        nStartCol = colIndex;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.Consumption;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(rowIndex, colIndex);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "("+sStartCell+ "/"+12+")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(rowIndex, nStartCol);
                        sEndCell = Global.GetExcelCellName(rowIndex, nStartCol+1);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "*" + sEndCell + ")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        sEndCell = Global.GetExcelCellName(rowIndex, colIndex-1);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "*" + sEndCell + ")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.0000;(#,##0.0000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        nTotalPercent += (oItem.CostPerPcs / oCostSheet.FOBPricePerPcs) * (100);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ((oItem.CostPerPcs / oCostSheet.FOBPricePerPcs) * (100));
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        nTotalValue += (oItem.CostPerPcs * oCostSheet.ApproxQty);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = (oItem.CostPerPcs * oCostSheet.ApproxQty);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        nTotalFabricCostPerPc = nTotalFabricCostPerPc + oItem.CostPerPcs;

                        rowIndex++;

                    }

                }
                #endregion

                #region Total Consumption
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex +=15]; cell.Merge = true; cell.Value = "Consumption:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                sStartCell = Global.GetExcelCellName(nstartRow, colIndex + 1);
                sEndCell = Global.GetExcelCellName(rowIndex-1, colIndex + 1);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nstartRow, colIndex + 1);
                sEndCell = Global.GetExcelCellName(rowIndex-1, colIndex + 1);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion
                #region Total Fabric Cost
                colIndex = 0;
                string sFabricCostCell = "";
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 18]; cell.Merge = true; cell.Value = "Total Fabric Cost:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nstartRow, colIndex + 1);
                sEndCell = Global.GetExcelCellName(rowIndex - 2, colIndex + 1);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.0000;(#,##0.0000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                sFabricCostCell = Global.GetExcelCellName(rowIndex, colIndex);

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion
                #region Accessories
                #region Accessories Tittle
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 4]; cell.Merge = true; cell.Value = "Accessories Description :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 4;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Inclued(%)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                


                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + oCostSheet.CostSheetAccessoriesDetails.Count+1, colIndex]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + oCostSheet.CostSheetAccessoriesDetails.Count+1, colIndex]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + oCostSheet.CostSheetAccessoriesDetails.Count+1, colIndex]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion
              
                #region Accessoris Heading
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 5]; cell.Merge = true; cell.Value = "Item"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 3]; cell.Merge = true; cell.Value = "Ref"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Symbol"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex];  cell.Value = "Price (" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex];  cell.Value = "Con/Dz"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex];  cell.Value = "Cost/Dz(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex];  cell.Value = "Cost/Pcs(" + oCostSheet.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.CostSheetAccessoriesDetails[0].UsePercentage; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                string sIncluedPercentageCell =  Global.GetExcelCellName(rowIndex, colIndex);
                rowIndex++;
                #endregion
                #region Accessores Value
                double   nTotalAccessoriesCostPerPcInclude = 0, _nTotalPercent = 0, _nTotalValue=0; nCount = 0;
                string sUnitPriceCell = "",sConPerDznCell="",sRateUnitCell="";
                if(oCostSheet.CostSheetAccessoriesDetails.Count>0)
                {
                    nstartRow = rowIndex;
                    foreach(CostSheetDetail oItem in oCostSheet.CostSheetAccessoriesDetails)
                    {
                        nCount++;
                        colIndex = 0;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount.ToString(); 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value =":"; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 5]; cell.Merge = true; cell.Value = oItem.ProductName; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 3]; cell.Merge = true; cell.Value = oItem.Description;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.RateUnit;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sRateUnitCell = Global.GetExcelCellName(rowIndex, colIndex);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.UnitSymbol;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        //cell.Style.Numberformat.Format = "dd MMM yyyy"; 
                        cell = sheet.Cells[rowIndex, ++colIndex];  cell.Value = oItem.MaterialMarketPrice;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00000;(#,##0.00000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        sUnitPriceCell = Global.GetExcelCellName(rowIndex, colIndex);

                        cell = sheet.Cells[rowIndex, ++colIndex];  cell.Value = oItem.Consumption;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.000;(#,##0.000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sConPerDznCell = Global.GetExcelCellName(rowIndex, colIndex);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sUnitPriceCell + "/" + sRateUnitCell +"*"+ sConPerDznCell+")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00000;(#,##0.00000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sConPerDznCell = Global.GetExcelCellName(rowIndex, colIndex);


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sConPerDznCell + "/" + 12 + ")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00000;(#,##0.00000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sStartCell = Global.GetExcelCellName(rowIndex, colIndex);

                        //cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "" + sStartCell + "+" + "(" + sStartCell + "*" + "(" + sIncluedPercentageCell + "/" + 100 + "))";
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "/" + "(( 100 - " + sIncluedPercentageCell+")/100.00))";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00000;(#,##0.00000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                        nTotalAccessoriesCostPerPcInclude += oItem.EstimatedCostPerPc;
                    }
                    nEndRow = rowIndex - 1;
                }
                #endregion

                #region Cost Sheet Package
                if(oCostSheet.CostSheetPackages!=null)
                {
                    foreach(CostSheetPackage oItem in oCostSheet.CostSheetPackages)
                    {
                        if(oItem.CostSheetPackageDetails.Count>0)
                        {
                            foreach(CostSheetPackageDetail oDItem in oItem.CostSheetPackageDetails)
                            {
                                nCount++;
                                colIndex = 0;
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nCount; 
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; 
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = oDItem.ProductName; 
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = oDItem.Description;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; 
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; 
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; 
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; 
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; 
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; 
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; 
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; 
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = ""; 
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; 
                                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                                rowIndex++;
                            }
                        }

                    }
                }
                #endregion
                #endregion
                #region Total Acc & Package
                colIndex = 0;
                string sTrimCell = "";
                int TotalTrimStartRow = rowIndex;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = "Total trims"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn-6]; cell.Merge = true; cell.Value = "cost/pc"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 6;

                sStartCell = Global.GetExcelCellName(nstartRow, colIndex + 1);
                sEndCell = Global.GetExcelCellName(nEndRow, colIndex + 1);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                sStartCell = Global.GetExcelCellName(nstartRow, colIndex + 1);
                sEndCell = Global.GetExcelCellName(nEndRow, colIndex + 1);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                sStartCell = Global.GetExcelCellName(nstartRow, colIndex + 1);
                sEndCell = Global.GetExcelCellName(nEndRow, colIndex + 1);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                sStartCell=Global.GetExcelCellName(rowIndex,colIndex);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = sStartCell; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                sTrimCell = Global.GetExcelCellName(rowIndex, colIndex);

                _nTotalPercent += ((nTotalAccessoriesCostPerPcInclude / oCostSheet.FOBPricePerPcs) * 100);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ((nTotalAccessoriesCostPerPcInclude / _oCostSheet.FOBPricePerPcs) * 100); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                _nTotalValue += oCostSheet.CostSheetAccessoriesDetails.Sum(x => x.EstimatedCostPerPc * oCostSheet.ApproxQty);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.CostSheetAccessoriesDetails.Sum(x => x.EstimatedCostPerPc * oCostSheet.ApproxQty); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion
                #region Print/Emb/Test Cost
                #region Print/Emb/Test Cost Heading
                colIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex,nMaxColumn-4]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 4;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Price / dz"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                colIndex = 0;
                #endregion
                double nUtilityCostPercPc = 0;
                string sPrintCell = "";
                #region Print/Pc
                int nPrintStartRow = rowIndex;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Print"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 4]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 4;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.PrintPricePerDozen; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                sStartCell = Global.GetExcelCellName(rowIndex, colIndex);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "/" + 12 +")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol+" #,##0.00000;(#,##0.00000)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sPrintCell = Global.GetExcelCellName(rowIndex, colIndex);


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex++;
                colIndex = 0;
                #endregion

                #region Emb/Pc
                string sEmbrodaryCell = "";
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Embrodary"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 4]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 4;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.EmbrodaryPricePerDozen; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sStartCell = Global.GetExcelCellName(rowIndex, colIndex);

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "/" + 12 + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sEmbrodaryCell = Global.GetExcelCellName(rowIndex, colIndex);


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex++;
                colIndex = 0;
                #endregion

                #region Test/Pc
                string sTestCell = "";
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Test";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 4]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 4;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.TestPricePerDozen; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sStartCell = Global.GetExcelCellName(rowIndex, colIndex);

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "/" + 12 + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sTestCell = Global.GetExcelCellName(rowIndex, colIndex);


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                colIndex = 0;
                #endregion

                #region Courier/Pc
                string sDHLCell = "";
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.CourierCaption == "" ? "DHL" : oCostSheet.CourierCaption;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 4]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 4;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.CourierPricePerDozen; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sStartCell = Global.GetExcelCellName(rowIndex, colIndex);

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "/" + 12 + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sDHLCell = Global.GetExcelCellName(rowIndex, colIndex);

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex++;
                colIndex = 0;
                #endregion

                #region Others/Pc
                string sOtherCell = "";
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.OthersCaption == "" ? "Others" : oCostSheet.OthersCaption; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 4]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 4;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.OthersPricePerDozen; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sStartCell = Global.GetExcelCellName(rowIndex, colIndex);

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "/" + 12 + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sOtherCell = Global.GetExcelCellName(rowIndex, colIndex);


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex++;
                colIndex = 0;
                #endregion
                #region Back To Back LC Percantage
                int BackToBackStartRow = rowIndex,BackToBackColIndex=0;
                string sBackToBackCell = "";
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 5]; cell.Merge = true; cell.Value = "Back to Back L/C Percantage"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                colIndex = nMaxColumn - 5;

                cell = sheet.Cells[rowIndex, ++colIndex ];  cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                BackToBackColIndex = colIndex;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sFabricCostCell + "+"+sTrimCell+"+" + sPrintCell + "+" + sEmbrodaryCell + "+"+sTestCell+"+"+sDHLCell+"+"+sOtherCell+")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                sBackToBackCell = Global.GetExcelCellName(rowIndex, colIndex);

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                colIndex = 0;
                #endregion
                #endregion
                #region CM/Pc
                double nCMPerPC = 0;
                string sCMPCCell = "";
                int CMPCStartRow = rowIndex, CMPCColIndex = 0;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "CM";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 5]; cell.Merge = true; cell.Value = "CM Percantage"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 5;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                CMPCColIndex = colIndex;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.CMCost; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                sStartCell = Global.GetExcelCellName(rowIndex, colIndex);

                nCMPerPC = oCostSheet.CMCostPerPcs;
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "/" + 12 + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                sCMPCCell = Global.GetExcelCellName(rowIndex, colIndex);


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                colIndex = 0;
                #endregion
                #region TotalB2BAndCM
                string sB2BCell = "";
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "B2B+CM";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 3]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 3;


                double nTotalB2BAndCM = (nTotalFabricCostPerPc + nTotalAccessoriesCostPerPcInclude + nUtilityCostPercPc + nCMPerPC);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sBackToBackCell + "+" + sCMPCCell + ")"; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00000;(#,##0.00000)"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                sB2BCell = Global.GetExcelCellName(rowIndex, colIndex);


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value =""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value =""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                colIndex = 0;
                #endregion
                #region Bank Cost
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Banking Cost"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ":"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 3]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 3;

                double nBankCost = ((nTotalB2BAndCM * oCostSheet.BankingCost) / 100);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nBankCost; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex++;
                colIndex = 0;
                #endregion
                #region FOB After Considering
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 3]; cell.Merge = true; cell.Value = "FOB After Consideration Of Discount and Banking Cost";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 3;

                sStartCell = Global.GetExcelCellName(rowIndex-1, colIndex+1);
                sEndCell = Global.GetExcelCellName(rowIndex - 2, colIndex+1);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "+" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                string sFOBCell = Global.GetExcelCellName(rowIndex, colIndex);

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                colIndex = 0;
                #endregion
                #region BackToBackPercentage
                cell = sheet.Cells[BackToBackStartRow, BackToBackColIndex]; cell.Formula = "(" + sBackToBackCell + "/" + sB2BCell + "*"+100+")";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                #endregion

                #region CMPC Percantage
                cell = sheet.Cells[CMPCStartRow, CMPCColIndex]; cell.Formula = "(" + sCMPCCell + "/" + sB2BCell + "*" + 100 + ")";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                #endregion
                #region Print Percantage and Total
                sStartCell = Global.GetExcelCellName(TotalTrimStartRow, nMaxColumn - 2);
                cell = sheet.Cells[TotalTrimStartRow, nMaxColumn - 1]; cell.Formula = "(" + 100 + "/" + sB2BCell + "*" + sStartCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[TotalTrimStartRow, nMaxColumn]; cell.Formula = "(" + sStartCell + "*" + sQtyCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nPrintStartRow, nMaxColumn-2);
                cell = sheet.Cells[nPrintStartRow, nMaxColumn - 1]; cell.Formula = "(" + 100 + "/" + sB2BCell + "*" + sStartCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nPrintStartRow, nMaxColumn]; cell.Formula = "(" + sStartCell + "*"  + sQtyCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                sStartCell = Global.GetExcelCellName(nPrintStartRow+1, nMaxColumn - 2);
                cell = sheet.Cells[nPrintStartRow+1, nMaxColumn - 1]; cell.Formula = "(" + 100 + "/" + sB2BCell + "*" + sStartCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nPrintStartRow+1, nMaxColumn]; cell.Formula = "(" + sStartCell + "*" + sQtyCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nPrintStartRow + 2, nMaxColumn - 2);
                cell = sheet.Cells[nPrintStartRow + 2, nMaxColumn - 1]; cell.Formula = "(" + 100 + "/" + sB2BCell + "*" + sStartCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nPrintStartRow + 2, nMaxColumn]; cell.Formula = "(" + sStartCell + "*" + sQtyCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nPrintStartRow + 3, nMaxColumn - 2);
                cell = sheet.Cells[nPrintStartRow + 3, nMaxColumn - 1]; cell.Formula = "(" + 100 + "/" + sB2BCell + "*" + sStartCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nPrintStartRow + 3, nMaxColumn]; cell.Formula = "(" + sStartCell + "*" + sQtyCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nPrintStartRow + 4, nMaxColumn - 2);
                cell = sheet.Cells[nPrintStartRow + 4, nMaxColumn - 1]; cell.Formula = "(" + 100 + "/" + sB2BCell + "*" + sStartCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nPrintStartRow + 4, nMaxColumn]; cell.Formula = "(" + sStartCell + "*" + sQtyCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nPrintStartRow + 6, nMaxColumn - 2);
                cell = sheet.Cells[nPrintStartRow + 6, nMaxColumn - 1]; cell.Formula = "(" + 100 + "/" + sB2BCell + "*" + sStartCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nPrintStartRow + 6, nMaxColumn]; cell.Formula = "(" + sStartCell + "*" + sQtyCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                #endregion

                #region Yarn Details
                int tempRowIndex = 0;
                if (oCostSheet.CostSheetYarnDetails.Count > 0)
                {
                    tempRowIndex = nStartRowYarnDetail;
                    foreach (CostSheetDetail oItem in oCostSheet.CostSheetYarnDetails)
                    {
                        sStartCell = Global.GetExcelCellName(nStartRowYarnDetail, nMaxColumn - 2);

                        cell = sheet.Cells[nStartRowYarnDetail, nMaxColumn - 1]; cell.Formula = "(" + 100 + "/" + sB2BCell + "*" + sStartCell + ")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nStartRowYarnDetail, nMaxColumn]; cell.Formula = "(" + sStartCell + "*" + sQtyCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        nStartRowYarnDetail++;

                    }

                }
                nStartRowYarnDetail = tempRowIndex;
                #endregion
                #region Late Penalty
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 3]; cell.Merge = true; cell.Value = "Late Penalty,Short Shipment";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 3;


                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sB2BCell + "*" + 0.02 + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                colIndex = 0;
                #endregion
                #region Total FOB
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex += 1]; cell.Merge = true; cell.Value = "Total FOB Cost/Pc"; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style  = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 3]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style  = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                colIndex = nMaxColumn - 3;

                sStartCell = Global.GetExcelCellName(rowIndex - 1, colIndex+1);
                sEndCell = Global.GetExcelCellName(rowIndex - 2, colIndex+1);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "+" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                colIndex = 0;
                #endregion
                #region Offered Price/Pc
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn - 3]; cell.Merge = true; cell.Value = "Offered Price/Pc";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                colIndex = nMaxColumn - 3;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.OfferPricePerPcs; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)"; 
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nStartRowYarnDetail, nMaxColumn - 1);
                sEndCell = Global.GetExcelCellName(rowIndex-1, nMaxColumn - 1);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                sStartCell = Global.GetExcelCellName(nStartRowYarnDetail, nMaxColumn);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nMaxColumn);
                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex+=2;
                colIndex = 2;
                #endregion
                #region CM Calculation and print
                if(oCostSheet.CostSheetCMs.Count>0)
                {
                    #region Header
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Part Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "O.Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "UnitSymbol"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "No.MC"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "MC Cost"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Prod/day"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Buffer Days"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Total Days req."; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "CM/pc"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Considiering\n(%)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "Final CM"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "CM/Dz"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                   
                    #endregion
                    #region Value print
                    string sOrderQtyCell = "", sNOMCsCell = "", sMCCostCell = "", sProdDayCell = "", sBufferDayCell = "",sReqDayCell="";
                    foreach(CostSheetCM oItem in oCostSheet.CostSheetCMs)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.CMPart; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oCostSheet.ApproxQty; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sOrderQtyCell = Global.GetExcelCellName(rowIndex, colIndex);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value ="Pcs";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.NumberOfMachine.ToString(); 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sNOMCsCell = Global.GetExcelCellName(rowIndex, colIndex);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.MachineCost;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00000;(#,##0.00000)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sMCCostCell = Global.GetExcelCellName(rowIndex, colIndex);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.ProductionPerDay; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sProdDayCell = Global.GetExcelCellName(rowIndex, colIndex);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.BufferDays; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sBufferDayCell = Global.GetExcelCellName(rowIndex, colIndex);


                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sOrderQtyCell + "/" + sProdDayCell + "+" + sBufferDayCell + ")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        sReqDayCell = Global.GetExcelCellName(rowIndex, colIndex);

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sNOMCsCell + "*" + sMCCostCell + "*" + sReqDayCell + ")" + "/" + sOrderQtyCell;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.CMAdditionalPerent;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(rowIndex, colIndex-1);
                        sEndCell = Global.GetExcelCellName(rowIndex, colIndex);
                        //cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "" + sStartCell +"+"+"("+sStartCell+ "*" + "(" + sEndCell + "/" + 100 + "))";
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "/" + "(( 100 - " + sEndCell + ")/100.00))";                        
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(rowIndex, colIndex);
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "*" + 12 + ")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }
                    #endregion

                    #region True CM
                    string sMidCell = "";
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "True CM"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(oCostSheet.ApproxQty, 0) + " Pcs"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(rowIndex-3, colIndex + 1);
                    sMidCell = Global.GetExcelCellName(rowIndex - 2, colIndex + 1);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, colIndex + 1);
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "-" + sMidCell + "-" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    nStartCol = colIndex;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(CMPCStartRow, CMPCColIndex + 1);
                    sMidCell = Global.GetExcelCellName(rowIndex - 2, colIndex + 1);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, colIndex + 1);
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "-" + sMidCell + "-" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(rowIndex, colIndex);
                    cell = sheet.Cells[rowIndex, nStartCol]; cell.Formula = "(" + sStartCell + "/" + 12 + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = oCostSheet.CurrencySymbol + " #,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(rowIndex, nStartCol);
                    rowIndex++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Formula = "(" + sStartCell + "/" + sFOBCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex + 10]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    #endregion
                }
                
                #endregion
                #endregion
                #endregion
                cell = sheet.Cells[1, 1, rowIndex + 3, nMaxColumn + 3];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=CostSheet.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        public Image GetLargeImage(int id)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }



        [HttpPost]
        public JsonResult GetEmployees(Employee oEmployee)
        {
            List<Employee> oEmployees = new List<Employee>();
            string sSQL = "SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeDesignationType.Merchandiser;
            if (!String.IsNullOrEmpty(oEmployee.Name))
            {
                sSQL += " AND Name Like '%" + oEmployee.Name + "%'";
            }
            try
            {
                oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                Employee _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetStyleImageInBase64(TechnicalSheet oTechnicalSheet)
        {

            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            if (oTechnicalSheet.IsFronImage == true)
            {
                oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oTechnicalSheetImage = oTechnicalSheetImage.GetBackImage(oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            if (oTechnicalSheetImage.LargeImage == null)
            {
                oTechnicalSheetImage.LargeImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oTechnicalSheetImage.LargeImage) }, JsonRequestBehavior.AllowGet);
        }
        public Image GetTechnicalSheetImage(TechnicalSheetImage oTechnicalSheetImage)
        {

            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }

        #region Search Style OR Buyer by Press Enter
        [HttpGet]
        public JsonResult SearchStyleAndBuyer(string sTempData, bool bIsStyle, int BUID, double ts)
        {
            _oCostSheets = new List<CostSheet>();
            string sSQL = "";
            if (bIsStyle == true)
            {
                sSQL = "SELECT * FROM View_CostSheet WHERE StyleNo LIKE ('%" + sTempData + "%')";
            }
            else
            {
                sSQL = "SELECT * FROM View_CostSheet WHERE BuyerName LIKE ('%" + sTempData + "%')";
            }
            sSQL += " AND BUID = " + BUID;

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {

                sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion
            try
            {
                CostSheet oCostSheet = new CostSheet();
                _oCostSheets = CostSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCostSheet = new CostSheet();
                _oCostSheet.ErrorMessage = ex.Message;
                _oCostSheets.Add(_oCostSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }

}
