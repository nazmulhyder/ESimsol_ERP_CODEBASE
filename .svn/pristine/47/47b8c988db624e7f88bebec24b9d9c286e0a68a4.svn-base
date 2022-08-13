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


namespace ESimSolFinancial.Controllers
{

    public class ConsumptionRequisitionController : Controller
    {

        #region Declartion
        ConsumptionRequisition _oConsumptionRequisition = new ConsumptionRequisition();
        List<ConsumptionRequisition> _oConsumptionRequisitions = new List<ConsumptionRequisition>();
        ConsumptionRequisitionDetail _oConsumptionRequisitionDetail = new ConsumptionRequisitionDetail();
        List<ConsumptionRequisitionDetail> _oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
        ConsumptionUnit _oConsumptionUnit = new ConsumptionUnit();
        List<ConsumptionUnit> _oConsumptionUnits = new List<ConsumptionUnit>();
        TConsumptionUnit _oTConsumptionUnit = new TConsumptionUnit();
        List<TConsumptionUnit> _oTConsumptionUnits = new List<TConsumptionUnit>();
        #endregion

        #region Collection Page
        public ActionResult ViewConsumptionRequisitions(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ConsumptionRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            _oConsumptionRequisitions = new List<ConsumptionRequisition>();
            string sSQL = "SELECT * FROM View_ConsumptionRequisition AS HH WHERE  ISNULL(HH.ApprovedBy,0)=0 ";
            if (buid>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " AND HH.BUID = " + buid ;
            }
            sSQL += " ORDER BY ConsumptionRequisitionID ASC";
            _oConsumptionRequisitions = ConsumptionRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            #region Requisition User
            if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.RequisitionBy FROM ConsumptionRequisition AS MM WHERE MM.BUID =" + buid.ToString() + " AND ISNULL(MM.RequisitionBy,0)!=0) ORDER BY HH.UserName";
            }
            else
            {
                sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.RequisitionBy FROM ConsumptionRequisition AS MM WHERE ISNULL(MM.RequisitionBy,0)!=0) ORDER BY HH.UserName";
            }
            List<User> oRequisitionUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oRequisitionUser = new ESimSol.BusinessObjects.User();
            oRequisitionUser.UserID = 0; oRequisitionUser.UserName = "--Select Requisition User--";
            oRequisitionUsers.Add(oRequisitionUser);
            oRequisitionUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.RequisitionUsers = oRequisitionUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumCRRefType));

            return View(_oConsumptionRequisitions);
        }
        #endregion

        #region Add, Edit, Delete
        public ActionResult ViewConsumptionRequisition(int id, int buid)
        {
            _oConsumptionRequisition = new ConsumptionRequisition();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            if (id > 0)
            {
                _oConsumptionRequisition = _oConsumptionRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oConsumptionRequisition.ConsumptionRequisitionDetails = ConsumptionRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oConsumptionRequisition.RequisitionBy = (int)Session[SessionInfo.currentUserID];
                _oConsumptionRequisition.RequisitionByName = (string)Session[SessionInfo.currentUserName];
            }

            #region Gets Stores

            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.ConsumptionRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.Stores = oWorkingUnits;
            ViewBag.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CRTypes = EnumObject.jGets(typeof(EnumConsumptionType));
            ViewBag.Shifts = EnumObject.jGets(typeof(EnumShift));
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumCRRefType));
            return View(_oConsumptionRequisition);
        }

        public ActionResult ViewConsumptionRequisitionRevise(int id, int buid)
        {
            _oConsumptionRequisition = new ConsumptionRequisition();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            if (id > 0)
            {
                _oConsumptionRequisition = _oConsumptionRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oConsumptionRequisition.ConsumptionRequisitionDetails = ConsumptionRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oConsumptionRequisition.RequisitionBy = (int)Session[SessionInfo.currentUserID];
                _oConsumptionRequisition.RequisitionByName = (string)Session[SessionInfo.currentUserName];
            }

            #region Gets Stores
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.ConsumptionRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Stores = oWorkingUnits;
            ViewBag.CRTypes = EnumObject.jGets(typeof(EnumConsumptionType));
            ViewBag.Shifts = EnumObject.jGets(typeof(EnumShift));
            return View(_oConsumptionRequisition);
        }

        public ActionResult ViewConsumptionRequisitionDisburse(int id, int buid)
        {
            _oConsumptionRequisition = new ConsumptionRequisition();
            if (id > 0)
            {
                _oConsumptionRequisition = _oConsumptionRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oConsumptionRequisition.ConsumptionRequisitionDetails = ConsumptionRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oConsumptionRequisition.RequisitionBy = (int)Session[SessionInfo.currentUserID];
                _oConsumptionRequisition.RequisitionByName = (string)Session[SessionInfo.currentUserName];
            }

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.ConsumptionRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.Stores = oWorkingUnits;
            ViewBag.CRTypes = EnumObject.jGets(typeof(EnumConsumptionType));
            return View(_oConsumptionRequisition);
        }

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(ConsumptionRequisition oConsumptionRequisition)
        {

            _oConsumptionRequisition = new ConsumptionRequisition();
            _oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
            try
            {
                _oConsumptionRequisition = oConsumptionRequisition;
                _oConsumptionRequisition = _oConsumptionRequisition.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oConsumptionRequisition = new ConsumptionRequisition();
                _oConsumptionRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oConsumptionRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Delivery(ConsumptionRequisition oConsumptionRequisition)
        {

            _oConsumptionRequisition = new ConsumptionRequisition();
            _oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
            try
            {
                _oConsumptionRequisition = oConsumptionRequisition;
                _oConsumptionRequisition = _oConsumptionRequisition.Delivery((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oConsumptionRequisition = new ConsumptionRequisition();
                _oConsumptionRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oConsumptionRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int nConsumptionRequisitionID)
        {
            string smessage = "";
            try
            {
                ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
                smessage = oConsumptionRequisition.Delete(nConsumptionRequisitionID, (int)Session[SessionInfo.currentUserID]);

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

        #region HTTP AcceptCRRevise
        [HttpPost]
        public JsonResult AcceptCRRevise(ConsumptionRequisition oConsumptionRequisition)
        {
            _oConsumptionRequisition = new ConsumptionRequisition();
            List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
            try
            {
                _oConsumptionRequisition = oConsumptionRequisition;
                _oConsumptionRequisition = _oConsumptionRequisition.AcceptConsumptionRequisitionRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oConsumptionRequisition = new ConsumptionRequisition();
                _oConsumptionRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oConsumptionRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Print and Preview
        public ActionResult ConsumptionRequisitionPrintList(string sIDs, double ts)
        {
            _oConsumptionRequisition = new ConsumptionRequisition();
            string sSQL = "";
            sSQL = "SELECT * FROM  View_ConsumptionRequisition WHERE ConsumptionRequisitionID IN ( " + sIDs + " )";
            _oConsumptionRequisition.ConsumptionRequisitions = ConsumptionRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oConsumptionRequisition.ConsumptionRequisitions[0].BUID, (int)Session[SessionInfo.currentUserID]);
            _oConsumptionRequisition.Company = oCompany;
            _oConsumptionRequisition.BusinessUnit = oBusinessUnit;

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ConsumptionRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            rptConsumptionRequisitionList oReport = new rptConsumptionRequisitionList();
            if (_oConsumptionRequisition.ConsumptionRequisitions.Count > 0)
            {
                byte[] abytes = oReport.PrepareReport(_oConsumptionRequisition, bIsRateView);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
        }

        public ActionResult ConsumptionRequisitionPreview(int id)
        {
            _oConsumptionRequisition = new ConsumptionRequisition();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            Contractor oContractor = new Contractor();
            ContactPersonnel oContactPersonnel = new ContactPersonnel();
            Company oCompany = new Company();
            _oConsumptionRequisition = _oConsumptionRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oConsumptionRequisition.ConsumptionRequisitionDetails = ConsumptionRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oConsumptionRequisition.Company = oCompany;

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ConsumptionRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }
            
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oConsumptionRequisition.BUID, (int)Session[SessionInfo.currentUserID]);
            _oConsumptionRequisition.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oConsumptionRequisition.BusinessUnit = oBusinessUnit;

            rptConsumptionRequisition oReport = new rptConsumptionRequisition();
            byte[] abytes = oReport.PrepareReport(_oConsumptionRequisition, bIsRateView);
            return File(abytes, "application/pdf");
        }

        public ActionResult ConsumptionRequisitionLogPreview(int id)
        {
            _oConsumptionRequisition = new ConsumptionRequisition();
            Contractor oContractor = new Contractor();
            ContactPersonnel oContactPersonnel = new ContactPersonnel();
            Company oCompany = new Company();
            _oConsumptionRequisition = _oConsumptionRequisition.GetLog(id, (int)Session[SessionInfo.currentUserID]);
            _oConsumptionRequisition.ConsumptionRequisitionDetails = ConsumptionRequisitionDetail.GetsLog(id, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oConsumptionRequisition.Company = oCompany;

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ConsumptionRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oConsumptionRequisition.BUID, (int)Session[SessionInfo.currentUserID]);
            _oConsumptionRequisition.BusinessUnit = oBusinessUnit;

            rptConsumptionRequisition oReport = new rptConsumptionRequisition();
            byte[] abytes = oReport.PrepareReport(_oConsumptionRequisition, bIsRateView);
            return File(abytes, "application/pdf");
        }

        #endregion Print

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



        private TConsumptionUnit GetRoot(int nParentConsumptionUnitID)
        {
            TConsumptionUnit oTConsumptionUnit = new TConsumptionUnit();
            foreach (TConsumptionUnit oItem in _oTConsumptionUnits)
            {
                if (oItem.parentid == nParentConsumptionUnitID)
                {
                    return oItem;
                }
            }
            return _oTConsumptionUnit;
        }



        private void AddTreeNodes(ref TConsumptionUnit oTConsumptionUnit)
        {
            IEnumerable<TConsumptionUnit> oChildNodes;
            oChildNodes = GetChild(oTConsumptionUnit.id);
            oTConsumptionUnit.children = oChildNodes;

            foreach (TConsumptionUnit oItem in oChildNodes)
            {
                TConsumptionUnit oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }



        private IEnumerable<TConsumptionUnit> GetChild(int nParentConsumptionUnit)
        {
            List<TConsumptionUnit> oTConsumptionUnits = new List<TConsumptionUnit>();
            foreach (TConsumptionUnit oItem in _oTConsumptionUnits)
            {
                if (oItem.parentid == nParentConsumptionUnit)
                {
                    oTConsumptionUnits.Add(oItem);
                }
            }
            return oTConsumptionUnits;
        }



        private TConsumptionUnit GetParentByID(int nParentConsumptionUnit)
        {
            TConsumptionUnit oTConsumptionUnit = new TConsumptionUnit();
            foreach (TConsumptionUnit oItem in _oTConsumptionUnits)
            {
                if (oItem.id == nParentConsumptionUnit)
                {
                    return oItem;
                }
            }
            return _oTConsumptionUnit;
        }



        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(ConsumptionRequisition oConsumptionRequisition)
        {
            _oConsumptionRequisition = new ConsumptionRequisition();
            _oConsumptionRequisition = oConsumptionRequisition;
            try
            {
                if (oConsumptionRequisition.ActionTypeExtra == "RequestForApproval")
                {
                    _oConsumptionRequisition.CRActionType = EnumCRActionType.RequestForApproval;
                    _oConsumptionRequisition.CRActionTypeInt = (int)EnumCRActionType.RequestForApproval;
                    _oConsumptionRequisition.CRStatus = EnumCRStatus.RequestForApproval;
                    _oConsumptionRequisition.CRStatusInt = (int)EnumCRStatus.RequestForApproval;

                }
                else if (oConsumptionRequisition.ActionTypeExtra == "UndoRequest")
                {
                    _oConsumptionRequisition.CRActionType = EnumCRActionType.UndoRequest;
                    _oConsumptionRequisition.CRActionTypeInt = (int)EnumCRActionType.UndoRequest;
                }
                else if (oConsumptionRequisition.ActionTypeExtra == "Approve")
                {
                    _oConsumptionRequisition.CRActionType = EnumCRActionType.Approve;
                    _oConsumptionRequisition.CRActionTypeInt = (int)EnumCRActionType.Approve;
                    _oConsumptionRequisition.CRStatus = EnumCRStatus.Initiallize;
                    _oConsumptionRequisition.CRStatusInt = (int)EnumCRStatus.Initiallize;
                }
                else if (oConsumptionRequisition.ActionTypeExtra == "UndoApprove")
                {
                    _oConsumptionRequisition.CRActionType = EnumCRActionType.UndoApprove;
                    _oConsumptionRequisition.CRActionTypeInt = (int)EnumCRActionType.UndoApprove;
                    _oConsumptionRequisition.CRStatus = EnumCRStatus.Approve;
                    _oConsumptionRequisition.CRStatusInt = (int)EnumCRStatus.Approve;
                }
                else if (oConsumptionRequisition.ActionTypeExtra == "RequestForRevise")
                {
                    _oConsumptionRequisition.CRActionType = EnumCRActionType.Request_revise;
                    _oConsumptionRequisition.CRActionTypeInt =(int)EnumCRActionType.Request_revise;
                    _oConsumptionRequisition.CRStatus = EnumCRStatus.Approve;
                    _oConsumptionRequisition.CRStatusInt =(int)EnumCRStatus.Approve;
                }
                else if (oConsumptionRequisition.ActionTypeExtra == "Disburse")
                {
                    _oConsumptionRequisition.CRActionType = EnumCRActionType.StockOut;
                    _oConsumptionRequisition.CRActionTypeInt = (int)EnumCRActionType.StockOut;
                    _oConsumptionRequisition.CRStatus = EnumCRStatus.StockOut;
                    _oConsumptionRequisition.CRStatusInt = (int)EnumCRStatus.StockOut;
                }
                _oConsumptionRequisition.Remarks = oConsumptionRequisition.Remarks;
                _oConsumptionRequisition = oConsumptionRequisition.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oConsumptionRequisition = new ConsumptionRequisition();
                _oConsumptionRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oConsumptionRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion
        
        #region Advance Search

        #region Http Get For Search
        [HttpPost]
        public JsonResult AdvanceSearch(ConsumptionRequisition oConsumptionRequisition)
        {
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();
            try
            {
                string sSQL = GetSQL(oConsumptionRequisition.Remarks, oConsumptionRequisition.BUID);
                oConsumptionRequisitions = ConsumptionRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oConsumptionRequisition = new ConsumptionRequisition();
                _oConsumptionRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sSearchingData, int nBUID)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            string sRefNo = Convert.ToString(sSearchingData.Split('~')[0]);            
            EnumCompareOperator eIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[1]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[3]);
            EnumCompareOperator eCRAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[4]);
            double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[5]);
            double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[6]);
            int nRequsitionBy = Convert.ToInt32(sSearchingData.Split('~')[7]);
            string sConsumptionUnits = Convert.ToString(sSearchingData.Split('~')[8]);
            string sProductIDs = Convert.ToString(sSearchingData.Split('~')[9]);
            string sStyleIDs = Convert.ToString(sSearchingData.Split('~')[10]);
            string sRefObjIDs = Convert.ToString(sSearchingData.Split('~')[11]);

            string sReturn1 = "SELECT * FROM View_ConsumptionRequisition";
            string sReturn = "";

            #region BUID
            if (nBUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID.ToString();
            }
            #endregion

            #region RefNo
            if (sRefNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo LIKE '%" + sRefNo + "%'";
            }
            #endregion
            
            #region IssueDate
            if (eIssueDate != EnumCompareOperator.None)
            {
                if (eIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Amount
            if (eCRAmount != EnumCompareOperator.None)
            {
                if (eCRAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount = " + nStartAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount != " + nStartAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount < " + nStartAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount > " + nStartAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
                else if (eCRAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount NOT BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
            }
            #endregion

            #region RequisitionBy
            if (nRequsitionBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(RequisitionBy,0) = " + nRequsitionBy.ToString();
            }
            #endregion

            #region ConsumptionUnits
            if (sConsumptionUnits != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequisitionFor IN (" + sConsumptionUnits + ") AND CRType=1";
            }
            #endregion

            #region Style
            if (sStyleIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequisitionFor IN (" + sStyleIDs + ") AND CRType=2";
            }
            #endregion

            #region Products
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ConsumptionRequisitionID IN (SELECT TT.ConsumptionRequisitionID FROM ConsumptionRequisitionDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            #region RefObjs
            if (sRefObjIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefObjID IN ("+sRefObjIDs+")";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        [HttpPost]
        public JsonResult GetsByRefNo(ConsumptionRequisition oConsumptionRequisition)
        {
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_ConsumptionRequisition AS HH WHERE  (ISNULL(HH.RefNo,'')+ISNULL(HH.RequisitionForName,'')+ISNULL(HH.RequisitionNo,'')) LIKE '%" + oConsumptionRequisition.RefNo + "%' ";
                #region BUID
                if (oConsumptionRequisition.BUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL = sSQL + " BUID = " + oConsumptionRequisition.BUID.ToString();
                }
                sSQL = sSQL + " ORDER BY ConsumptionRequisitionID ASC";
                #endregion
                oConsumptionRequisitions = ConsumptionRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oConsumptionRequisitions = new List<ConsumptionRequisition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WaitingSearch(ConsumptionRequisition oConsumptionRequisition)
        {
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_ConsumptionRequisition AS HH WHERE ISNULL(HH.CRStatus,0) = 1 ";
                #region BUID
                if (oConsumptionRequisition.BUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL = sSQL + " BUID = " + oConsumptionRequisition.BUID.ToString();
                }
                sSQL = sSQL + " ORDER BY ConsumptionRequisitionID ASC";
                #endregion
                oConsumptionRequisitions = ConsumptionRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oConsumptionRequisitions = new List<ConsumptionRequisition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReviseHistory(ConsumptionRequisition oConsumptionRequisition)
        {
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();
            try
            {
                string sSQL = "SELECT * FROM View_ConsumptionRequisitionLog AS HH WHERE HH.ConsumptionRequisitionID=" + oConsumptionRequisition.ConsumptionRequisitionID.ToString() + " ORDER BY ConsumptionRequisitionLogID";
                oConsumptionRequisitions = ConsumptionRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oConsumptionRequisitions = new List<ConsumptionRequisition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Get Lots
        [HttpPost]
        public JsonResult GetLots(Lot oInvLot)
        {
            List<Lot> oInvLots = new List<Lot>();
            if (oInvLot.ProductID > 0 && oInvLot.WorkingUnitID > 0 && (int)oInvLot.ParentType > 0)
            {
                //oInvLots = InvLot.Gets((int)oInvLot.ParentType, oInvLot.WorkingUnitID,oInvLot.ProductID, (int)Session[SessionInfo.currentUserID]);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oInvLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Get SubLeders
        [HttpPost]
        public JsonResult GetSubLedgers(ACCostCenter oACCostCenter)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            string sSQL = "SELECT * FROM View_ACCostCenter WHERE ParentID IN (1,6,14,13,3435)";
            if (!string.IsNullOrEmpty(oACCostCenter.Name))
            {
                sSQL += " AND Name LIKE '%"+oACCostCenter.Name+"%'";
            }
            oACCostCenters = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oACCostCenters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion


        #region Get RefObj
        [HttpPost]
        public JsonResult GetRefObjs(ConsumptionRequisition oConsumptionRequisition)
        {
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();
            try
            {
                if ((EnumCRRefType)oConsumptionRequisition.RefTypeInt == EnumCRRefType.Order)
                {
                    oConsumptionRequisitions = MapOrder(oConsumptionRequisition.RefObjNo);
                }
                else if ((EnumCRRefType)oConsumptionRequisition.RefTypeInt == EnumCRRefType.Dispo)
                {
                    oConsumptionRequisitions = MapDispo(oConsumptionRequisition.RefObjNo);
                }
            }
            catch (Exception ex)
            {
                oConsumptionRequisitions = new List<ConsumptionRequisition>();
                ConsumptionRequisition oCR = new ConsumptionRequisition();
                oConsumptionRequisition.ErrorMessage = ex.Message;
                oConsumptionRequisitions.Add(oCR);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<ConsumptionRequisition> MapOrder(string sValue)
        {
            ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();

            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();

            string sSQL = "SELECT * FROM View_FabricSalesContractDetail AS FSC WHERE FSC.OrderType in (3,2,9,10,11,12)  AND ISNULL(FSC.ApproveBy,0)!=0 AND SCNoFull LIKE '%" + sValue + "%'";
            oFabricSalesContractDetails = FabricSalesContractDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (FabricSalesContractDetail oItem in oFabricSalesContractDetails)
            {
                oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.SCNoFull = oItem.SCNoFull;
                oConsumptionRequisition.SCDate = oItem.SCDate;
                oConsumptionRequisition.BuyerName = oItem.BuyerName;
                oConsumptionRequisition.OrderName = oItem.OrderName;
                oConsumptionRequisition.Qty = oItem.Qty;
                oConsumptionRequisition.RefType = EnumCRRefType.Order;
                oConsumptionRequisition.RefTypeInt = (int)EnumCRRefType.Order;
                oConsumptionRequisition.RefObjID = oItem.FabricSalesContractID;
                oConsumptionRequisition.RefObjNo = oItem.SCNoFull;
                oConsumptionRequisitions.Add(oConsumptionRequisition);
            }
            return oConsumptionRequisitions;
        }
        private List<ConsumptionRequisition> MapDispo(string sValue)
        {
            ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();

            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();

            string sSQL = "SELECT * FROM View_FabricSalesContractDetail AS FSCD WHERE FSCD.OrderType in (3,2,9,10,11,12) AND ISNULL(FSCD.ApproveBy,0)!=0 AND FSCD.ExeNoFull LIKE '%"+sValue+"%'";
            oFabricSalesContractDetails = FabricSalesContractDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (FabricSalesContractDetail oItem in oFabricSalesContractDetails)
            {
                oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.SCNoFull = oItem.SCNoFull;
                oConsumptionRequisition.ExeNoFull = oItem.ExeNoFull;
                oConsumptionRequisition.SCDate = oItem.SCDate;
                oConsumptionRequisition.BuyerName = oItem.BuyerName;
                oConsumptionRequisition.OrderName = oItem.OrderName;
                oConsumptionRequisition.Qty = oItem.Qty;
                oConsumptionRequisition.RefType = EnumCRRefType.Order;
                oConsumptionRequisition.RefTypeInt = (int)EnumCRRefType.Order;
                oConsumptionRequisition.RefObjID = oItem.FabricSalesContractDetailID;
                oConsumptionRequisition.ColorInfo = oItem.ColorInfo;
                oConsumptionRequisition.Construction = oItem.Construction;
                oConsumptionRequisition.ProductName = oItem.ProductName;
                oConsumptionRequisition.FabricNo = oItem.FabricNo;
                oConsumptionRequisition.RefObjNo = oItem.ExeNoFull;
                oConsumptionRequisitions.Add(oConsumptionRequisition);
            }
            return oConsumptionRequisitions;
        }
        #endregion
        
        #region Consumption Unit
        public ActionResult ViewConsumptionUnits(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ConsumptionUnit).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));


            _oConsumptionUnit = new ConsumptionUnit();
            _oTConsumptionUnit = new TConsumptionUnit();
            _oConsumptionUnits = new List<ConsumptionUnit>();
            _oTConsumptionUnits = new List<TConsumptionUnit>();
            //_oConsumptionUnits = ConsumptionUnit.Gets((int)Session[SessionInfo.currentUserID]);



            try
            {

                _oConsumptionUnits = ConsumptionUnit.Gets((int)Session[SessionInfo.currentUserID]);

                foreach (ConsumptionUnit oItem in _oConsumptionUnits)
                {

                    _oTConsumptionUnit = new TConsumptionUnit();
                    _oTConsumptionUnit.id = oItem.ConsumptionUnitID;
                    _oTConsumptionUnit.CUSequence = oItem.CUSequence;
                    _oTConsumptionUnit.text = oItem.Name;
                    _oTConsumptionUnit.state = "";
                    _oTConsumptionUnit.attributes = "";
                    _oTConsumptionUnit.parentid = oItem.ParentConsumptionUnitID;
                    _oTConsumptionUnit.Description = oItem.Note;
                    _oTConsumptionUnit.IsLastLayer = oItem.IsLastLayer;
                    
                    //_oTProductCategory.AssetTypeInString = oItem.AssetTypeInString;
                    _oTConsumptionUnits.Add(_oTConsumptionUnit);
                }
                _oTConsumptionUnit = new TConsumptionUnit();
                _oTConsumptionUnit = GetRoot(0);
                this.AddTreeNodes(ref _oTConsumptionUnit);
                return View(_oTConsumptionUnit);
            }

            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oTConsumptionUnit);
            }



            //return View(_oConsumptionUnits);
        }

        public ActionResult ViewConsumptionUnit(int id)
        {
            _oConsumptionUnit = new ConsumptionUnit();
            if (id > 0)
            {
                _oConsumptionUnit = _oConsumptionUnit.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oConsumptionUnit.BUWiseConsumptionUnits = BUWiseConsumptionUnit.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oConsumptionUnit.BUWiseConsumptionUnits = new List<BUWiseConsumptionUnit>();
            }
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oConsumptionUnit);
        }

        [HttpPost]
        public JsonResult SaveCU(ConsumptionUnit oConsumptionUnit)
        {
            _oConsumptionUnit = new ConsumptionUnit();
            try
            {
                _oConsumptionUnit = oConsumptionUnit;
                _oConsumptionUnit = _oConsumptionUnit.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oConsumptionUnit = new ConsumptionUnit();
                _oConsumptionUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oConsumptionUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeGroup(ConsumptionUnit oConsumptionUnit)
        {
            string sFeedBackMessage = "";
            _oConsumptionUnit = new ConsumptionUnit();
            try
            {
                var sSql = "UPDATE ConsumptionUnit SET ParentConsumptionUnitID = " + oConsumptionUnit.ParentConsumptionUnitID + "WHERE ConsumptionUnitID IN (" + oConsumptionUnit.Name + ")";
                sFeedBackMessage = _oConsumptionUnit.ChangeGroup(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteCU(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ConsumptionUnit oConsumptionUnit = new ConsumptionUnit();
                sFeedBackMessage = oConsumptionUnit.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult RefreshCUSequence(ConsumptionUnit oConsumptionUnit)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oConsumptionUnit.RefreshSequence((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #region GET Consumption Units
        public JsonResult ConsumptionUnitPicker(ConsumptionUnit oConsumptionUnit)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            List<ConsumptionUnit> oConsumptionUnits = new List<ConsumptionUnit>();
            string sSQL = "SELECT * FROM ConsumptionUnit AS HH ";
            string sNextSql = "";

            #region Fixed Condition
            Global.TagSQL(ref sNextSql);
            sNextSql += " HH.ParentConsumptionUnitID IN (SELECT CU.ConsumptionUnitID FROM ConsumptionUnit AS CU WHERE CU.ParentConsumptionUnitID=1)";
            #endregion

            #region BUID
            if (oConsumptionUnit.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sNextSql);
                sNextSql += " HH.ConsumptionUnitID IN (SELECT MM.ConsumptionUnitID FROM BUWiseConsumptionUnit AS MM WHERE MM.BUID=" + oConsumptionUnit.BUID + ")";
            }
            #endregion

            #region Name
            if (!string.IsNullOrEmpty(oConsumptionUnit.Name))
            {
                Global.TagSQL(ref sNextSql);
                sNextSql += " HH.Name Like '%" + oConsumptionUnit.Name + "%'";
            }
            #endregion

            sSQL += sNextSql;
            oConsumptionUnits = ConsumptionUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        
        #region Gets Product & Lots
        //[HttpPost]
        //public JsonResult SearchProducts(Product oProduct)
        //{
        //    List<Product>  oProducts = new List<Product>();
        //    ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
        //    oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
        //    try
        //    {
        //        string sSQL = "SELECT * FROM View_Product";
        //        string sSQL1 = "";

        //        #region BUID
        //        if (oProduct.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID =" + oProduct.BUID.ToString() + ")";
        //        }
        //        #endregion

        //        #region ProductName
        //        if (oProduct.ProductName == null) { oProduct.ProductName = ""; }
        //        if (oProduct.ProductName != "")
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " ProductName LIKE '%" + oProduct.ProductName + "%'";
        //        }
        //        #endregion

        //        #region Deafult
        //        Global.TagSQL(ref sSQL1);
        //        sSQL1 = sSQL1 + " Activity=1";
        //        #endregion

        //        #region Style Wise Suggested Product
        //        if (oProduct.ProductID > 0) //Hare ProductID  Use as a StyleID
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " ProductID IN (SELECT HH.ProductID FROM BillOfMaterial AS HH WHERE HH.TechnicalSheetID=" + oProduct.ProductID.ToString() + ")";
        //        }
        //        #endregion

        //        sSQL = sSQL + sSQL1 + " Order By ProductName ASC";

        //        oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //        if (oProducts.Count() <= 0) throw new Exception("No product found.");
        //    }
        //    catch (Exception ex)
        //    {
        //        oProducts = new List<Product>();
        //        oProduct = new Product();
        //        oProduct.ErrorMessage = ex.Message;
        //        oProducts.Add(oProduct);
        //    }
        //    var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}
        [HttpPost]
        public JsonResult SearchProducts(Lot oLot)
        {
            List<Product> oProducts = new List<Product>();
            Product oProduct = new Product();
            try
            {
                string sSQL = "SELECT * FROM View_Product";
                string sSQL1 = "";

                #region BUID
                if (oLot.BUID > 0)//if apply style configuration business unit
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID =" + oLot.BUID.ToString() + ")";
                }
                #endregion

                #region ProductName

                if (!string.IsNullOrEmpty(oLot.ProductName))
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductName LIKE '%" + oLot.ProductName + "%'";
                }
                #endregion

                #region Deafult
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " Activity=1";
                #endregion
                #region WorkingUnitID
                if (oLot.WorkingUnitID > 0) //Hare ProductID  Use as a StyleID
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductID in (Select ProductID from Lot where Balance>0 and WorkingUnitID=" + oLot.WorkingUnitID + ")";
                }
                #endregion

                #region Style Wise Suggested Product
                if (oLot.ProductID > 0) //Hare ProductID  Use as a StyleID
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductID IN (SELECT HH.ProductID FROM BillOfMaterial AS HH WHERE HH.TechnicalSheetID=" + oLot.ProductID.ToString() + ")";
                }
                #endregion

                sSQL = sSQL + sSQL1 + " Order By ProductName ASC";

                oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oProducts.Count() <= 0) throw new Exception("No product found.");
            }
            catch (Exception ex)
            {
                oProducts = new List<Product>();
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult SearchLots(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_Lot";
                string sSQL1 = "";

                #region BUID
                if (oLot.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " BUID =" + oLot.BUID.ToString();
                }
                #endregion

                #region ProductID
                if (oLot.ProductID > 0)
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " ProductID =" + oLot.ProductID.ToString();
                }
                #endregion

                #region WorkingUnitID
                if (oLot.WorkingUnitID > 0)
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " WorkingUnitID =" + oLot.WorkingUnitID.ToString();
                }
                #endregion

                #region LotNo
                if (oLot.LotNo == null) { oLot.LotNo = ""; }
                if (oLot.LotNo != "")
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " LotNo LIKE '%" + oLot.LotNo + "%'";
                }
                #endregion

                #region Deafult
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " Balance>0";
                #endregion

                #region Style Wise Suggested Lot
                if (oLot.StyleID > 0)
                {
                    Global.TagSQL(ref sSQL1);
                    sSQL1 = sSQL1 + " LotID IN (SELECT HH.LotID FROM Lot AS HH WHERE HH.StyleID=" + oLot.StyleID.ToString() + ")";
                }
                #endregion

                sSQL = sSQL + sSQL1 + " ORDER BY LotID ASC";

                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oLots.Count() <= 0) throw new Exception("No Lot found.");
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots.Add(oLot);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdateVoucherEffect

        [HttpPost]
        public JsonResult UpdateVoucherEffect(ConsumptionRequisition oConsumptionRequisition)
        {
            try
            {
                oConsumptionRequisition = oConsumptionRequisition.UpdateVoucherEffect((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}