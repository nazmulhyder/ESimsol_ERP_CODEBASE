using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
 
using ESimSol.Reports;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
namespace ESimSolFinancial.Controllers
{

    public class TAPExecutionController : Controller
    {
        #region Declaration
        TAPExecution _oTAPExecution = new TAPExecution();
        List<TAPExecution> _oTAPExecutions = new List<TAPExecution>();
        TAP _oTAP = new TAP();
        List<TAP> _oTAPs = new List<TAP>();
        List<TAPDetail> _oTAPDetails = new List<TAPDetail>();
        TAPDetail _oTAPDetail = new TAPDetail();
        TTAPExecution _oTTAPExecution = new TTAPExecution();
        List<TTAPExecution> _oTTAPExecutions = new List<TTAPExecution>();
        Job _oJob = new Job();
        List<Job> _oJobs = new List<Job>();
        #endregion

        #region Function
        private List<TAPDetail> GetChildSteps(int nOrderStepID,  int nTAPID, List<TAPDetail> oTAPDetails)
        {
            List<TAPDetail> oTempTAPDetails = new List<TAPDetail>();
            foreach (TAPDetail oItem in oTAPDetails)
            {
                if(oItem.OrderStepParentID ==  nOrderStepID  && oItem.TAPID == nTAPID)
                {

                    oTempTAPDetails.Add(oItem);
                }
            }
            return oTempTAPDetails;
        }

        private TAPExecution GetTAPExecution(int nTAPDetailID, List<TAPExecution> oTAPExecutions)
        {
            _oTAPExecution = new TAPExecution();
            foreach (TAPExecution oItem in oTAPExecutions)
            {
                if (oItem.TAPDetailID == nTAPDetailID)
                {
                    return oItem;
                }
            }
            return _oTAPExecution;
        }
        private List<TAPExecution> GetChildTAPExecutions(int nOrdrStepID, int nTAPID,  List<TAPExecution> oTAPExecutions)
        {
            List<TAPExecution> oTempTAPExecutions = new List<TAPExecution>();
            foreach(TAPExecution oItem in oTAPExecutions)
            {
                if(oItem.OrderStepParentID ==  nOrdrStepID && oItem.TAPID == nTAPID)
                {
                    oTempTAPExecutions.Add(oItem);
                }
            }
            return oTempTAPExecutions;
        }

        #region Make Tree
        private IEnumerable<TAPExecution> GetChild(int nOrderStepID)
        {
            List<TAPExecution> oTAPExecutions = new List<TAPExecution>();
            foreach (TAPExecution oItem in _oTAPExecutions)
            {
                if (oItem.OrderStepParentID == nOrderStepID)
                {
                    oTAPExecutions.Add(oItem);
                }
            }
            return oTAPExecutions;
        }
        private void AddTreeNodes(ref TAPExecution oTAPExecution)
        {
            IEnumerable<TAPExecution> oChildNodes;
            oChildNodes = GetChild(oTAPExecution.OrderStepID);
            oTAPExecution.ChildNodes = oChildNodes;

            foreach (TAPExecution oItem in oChildNodes)
            {
                TAPExecution oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private IEnumerable<TTAPExecution> GetChildren(int nOrderStepID)
        {
            List<TTAPExecution> oTTAPExecutions = new List<TTAPExecution>();
            foreach (TTAPExecution oItem in _oTTAPExecutions)
            {
                if (oItem.parentid == nOrderStepID)
                {
                    oTTAPExecutions.Add(oItem);
                }
            }
            return oTTAPExecutions;
        }

        private void AddTreeNodes(ref TTAPExecution oTTAPExecution)
        {
            IEnumerable<TTAPExecution> oChildNodes;
            oChildNodes = GetChildren(oTTAPExecution.id);
            oTTAPExecution.children = oChildNodes;

            foreach (TTAPExecution oItem in oChildNodes)
            {
                TTAPExecution oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private TTAPExecution GetRoot(int nParentID)
        {
            TTAPExecution oTTAPExecution = new TTAPExecution();
            foreach (TTAPExecution oItem in _oTTAPExecutions)
            {
                if (oItem.parentid == nParentID)
                {
                    return oItem;
                }
            }
            return _oTTAPExecution;
        }

        private TTAPExecution MakeTree(List<TAPExecution> oTAPExecutions)
        {
            _oTTAPExecutions = new List<TTAPExecution>();
            //SEt Root
            _oTTAPExecution = new TTAPExecution();
            _oTTAPExecution.id = 1;
            _oTTAPExecution.parentid = 0;
            _oTTAPExecution.text = "";
            _oTTAPExecutions.Add(_oTTAPExecution);

            foreach (TAPExecution oItem in oTAPExecutions)
            {
                _oTTAPExecution = new TTAPExecution();
                _oTTAPExecution.id = oItem.OrderStepID;//Use for Make Tree
                _oTTAPExecution.parentid = oItem.OrderStepParentID;
                _oTTAPExecution.TAPExecutionID = oItem.TAPExecutionID;
                _oTTAPExecution.text = oItem.OrderStepName+"("+oItem.ApprovalPlanDateInString+")";
                _oTTAPExecution.TAPDetailID = oItem.TAPDetailID;
                _oTTAPExecution.UpDatedData = oItem.UpdatedData;
                _oTTAPExecution.OrderStepID = oItem.OrderStepID;
                if (oItem.OrderStepParentID == 1)
                {
                    _oTTAPExecution.RequiredDataTypeInInt = (int)EnumRequiredDataType.Date;
                }
                else
                {
                    _oTTAPExecution.RequiredDataTypeInInt = oItem.RequiredDataTypeInInt;
                }
                _oTTAPExecution.IsDone = oItem.IsDone;
                _oTTAPExecution.DoneDate = oItem.DoneDate;
                _oTTAPExecution.TAPID = oItem.TAPID;
                _oTTAPExecutions.Add(_oTTAPExecution);
            }
            _oTTAPExecution = new TTAPExecution();
            _oTTAPExecution = GetRoot(0);
            this.AddTreeNodes(ref _oTTAPExecution);
            return _oTTAPExecution;
        }

        #endregion

        #endregion

        #region Actions
        public ActionResult ViewTAPExecution(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TAPExecution).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oTAPExecution = new TAPExecution();
            return View(_oTAPExecution);
        }

        public ActionResult ViewJobWiseTAPExecution(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TAPExecution).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oJob = new Job();
            return View(_oJob);
        }


        public ActionResult ViewTAPExecutionFollowUp(int menuid, int buid, int TAPID)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TAPExecution).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oTAPExecution = new TAPExecution();
            string sSql = "SELECT * FROM View_User WHERE UserID IN (SELECT Distinct  ApprovedBy FROM View_TAP WHERE ISNULL(ApprovedBy,0)!=0)";
            
            ViewBag.Users = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Merchandisers = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
            ViewBag.TAP = _oTAP.Get(TAPID, (int)Session[SessionInfo.currentUserID]);
            return View(_oTAPExecution);
        }

        public ActionResult ViewNotificationTAP(int nTAPID, bool IsPendingTask, int nHIAID, int nOrderStepID, double ts)//this action call from Layout and MerchandiserDashBoard Page
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TAP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oTAP = new TAP();
            List<TAPExecution> oTAPExecutions = new List<TAPExecution>();
            if(nTAPID>0)
            {
                _oTAP = _oTAP.Get(nTAPID, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "";
                if(IsPendingTask)//Get pending tasks
                {
                    //Get pending tasks
                    sSQL = "SELECT * FROM View_TAPDetail WHERE TAPID = " + nTAPID + " AND ISNULL(ExecutionIsDone,0)=0 AND CONVERT(DATE, CONVERT(VARCHAR(12), ApprovalPlanDate))< CONVERT(DATE, CONVERT(VARCHAR(12), GETDATE())) ";
                }
                else
                {
                    //Get Regular tasks
                    sSQL = "SELECT * FROM View_TAPDetail WHERE TAPID = " + nTAPID + " AND ISNULL(ExecutionIsDone,0)=0 AND CONVERT(DATE, CONVERT(VARCHAR(12), ApprovalPlanDate))= CONVERT(DATE, CONVERT(VARCHAR(12), GETDATE())) ";
                }
                _oTAPDetails = new List<TAPDetail>();
                _oTAPDetails = TAPDetail.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
                if(_oTAPDetails.Count>0)
                {
                    nOrderStepID = _oTAPDetails[0].OrderStepID;
                }
            }else
            {
                if (nHIAID > 0)
                {
                    _oTAP = _oTAP.GetByHIA(nHIAID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            oTAPExecutions = TAPExecution.GetOrderSteps(_oTAP.TAPID, (int)Session[SessionInfo.currentUserID]);
            _oTAPExecution.TTAPExecution = MakeTree(oTAPExecutions);
            _oTAP.TAPExecution = _oTAPExecution;
            ViewBag.OrderStepID = nOrderStepID;//set Order step ID
            return View(_oTAP);
        }

        #region BuyerWiseTAPsPicker

        [HttpPost]
        public JsonResult BuyerWiseTAPsPicker(TAP oTAP)
        {
            _oTAPs = new List<TAP>();
            string sSQL = "SELECT * FROM View_TAP  WHERE   BUID =" + oTAP.BUID + " AND  BuyerID = " + oTAP.BuyerID + " AND  ISNULL(ApprovedBy,0) != 0 AND ISNULL(IsActive,0)= 1";
            try
            {
                if (oTAP.ProductionFactoryID > 0)
                {
                    sSQL += " AND ProductionFactoryID = " + oTAP.ProductionFactoryID;
                }
                if (oTAP.BuyerID > 0)
                {
                    _oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oTAP = new TAP();
                _oTAP.ErrorMessage = ex.Message;
                _oTAPs.Add(_oTAP);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region HTTP GET  Functions
        [HttpGet]
        public JsonResult GetOrderSteps(int nTAPID,int nOrderStepID,   double ts)
        {
            List<TAPExecution> oTAPExecutions = new List<TAPExecution>();
            List<TAPExecution> oNewTAPExecutions = new List<TAPExecution>();
            _oTAPExecution = new TAPExecution();
            try
            {
                oTAPExecutions = TAPExecution.GetOrderSteps(nTAPID, (int)Session[SessionInfo.currentUserID]);
                if (nOrderStepID!=0)
                {
                    oNewTAPExecutions = oTAPExecutions.Where(x => x.OrderStepID == nOrderStepID | x.OrderStepParentID == nOrderStepID).OrderBy(x => x.TAPDetailSequence).ToList();
                    oTAPExecutions = oNewTAPExecutions;
                }
                
              _oTAPExecution.TTAPExecution = MakeTree(oTAPExecutions);
            }
            catch (Exception ex)
            {
                _oTAPExecution = new TAPExecution();
                _oTAPExecution.ErrorMessage = ex.Message;
                oTAPExecutions.Add(_oTAPExecution);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPExecution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTAPDetials(TAP oTAP)
        {
            _oTAPDetails = new List<TAPDetail>();
            _oTAPs = new List<TAP>();
            _oTAPExecutions = new List<TAPExecution>();
            List<TAPDetail> oTempTAPDetails = new List<TAPDetail>();
            string ids = "";
            foreach(TAP oItem in oTAP.TAPs)
            {
                ids += oItem.TAPID + ",";
            }
            ids = ids.Substring(0,ids.Length -1);


            string sSQL = "SELECT * FROM View_TAPDetail WHERE TAPID IN (" + ids + ") Order By TAPID, Sequence";//Get Detail
            try
            {
                oTempTAPDetails = TAPDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                foreach(TAPDetail oItem in oTempTAPDetails)
                {
                    if(oItem.OrderStepParentID==1)
                    {
                        _oTAPDetails.Add(oItem);
                    }
                }
                _oTAPExecutions = TAPExecution.GetsByTaPs(oTAP, (int)Session[SessionInfo.currentUserID]);
                foreach(TAPDetail oItem in _oTAPDetails )
                {
                    oItem.ChildOrderSteps = GetChildSteps(oItem.OrderStepID, oItem.TAPID,  oTempTAPDetails);
                    foreach(TAPDetail oChildItem in oItem.ChildOrderSteps)
                    {
                        oChildItem.TAPExecution = GetTAPExecution(oChildItem.TAPDetailID, _oTAPExecutions);//For each one Detail get one Execution
                    }
                    oItem.TAPExecutions = GetChildTAPExecutions(oItem.OrderStepID,oItem.TAPID,  _oTAPExecutions);
                    oItem.TAPExecution = GetTAPExecution(oItem.TAPDetailID, _oTAPExecutions);
                }
            }
            catch (Exception ex)
            {
                _oTAPDetail = new TAPDetail();
                _oTAPDetail.ErrorMessage = ex.Message;
                _oTAPDetails.Add(_oTAPDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Http Get For TAP
        [HttpGet]
        public JsonResult Gets(string sTemp)
        {
            List<TAP> oTAPs = new List<TAP>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTAP = new TAP();
                _oTAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTAPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            int nCboPlanDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dSendingEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            string sPlanNo = sTemp.Split('~')[3];
            int nTechnicalSheetID = Convert.ToInt32(sTemp.Split('~')[4]);
            int nApproveByID = Convert.ToInt32(sTemp.Split('~')[5]);
            string sBuyerIDs = sTemp.Split('~')[6];
            int nSaleOrderID = Convert.ToInt32(sTemp.Split('~')[7]);
            int nBrandID = Convert.ToInt32(sTemp.Split('~')[8]);
            string sFactoryIDs = sTemp.Split('~')[9];
            int nMerchandiserID = Convert.ToInt32(sTemp.Split('~')[10]);
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[11]);
            bool bIsRunningShipmentWise = Convert.ToBoolean(sTemp.Split('~')[12]);
            bool bIsOnlyforApprove = Convert.ToBoolean(sTemp.Split('~')[13]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[14]);

            string sReturn1 = "SELECT * FROM View_TAP";
            string sReturn = "";

            #region BU
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region REf No

            if (sPlanNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PlanNo ='" + sPlanNo + "'";
            }
            #endregion

            #region nSaleOrderID

            if (nSaleOrderID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapID =" + nSaleOrderID;
            }
            #endregion

            #region Buyer Name
            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Brand
            if (nBrandID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BrandID = " + nBrandID;
            }
            #endregion

            #region Merchandiser
            if (nMerchandiserID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID = " + nMerchandiserID;
            }
            #endregion

            #region Session
            if (nSessionID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapID IN (SELECt SaleOrderID FROm SaleOrder  WHERE BusinessSessionID  = " + nSessionID + ")";
            }
            #endregion

            #region Active
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " ISNULL(IsActive,0)= 1";
            #endregion

            #region Factory Name
            if (sFactoryIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductionFactoryID IN (" + sFactoryIDs + ")";
            }
            #endregion

            #region Style Wise
            if (nTechnicalSheetID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TechnicalSheetID  =" + nTechnicalSheetID;
            }
            #endregion

            #region Approve By
            if (nApproveByID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApprovedBy  =" + nApproveByID;
            }
            #endregion
            #region Is running
            if (bIsRunningShipmentWise == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " IsShippedOut=0";
            }
            #endregion


            #region Date Wise
            #region Sending Date
            if (nCboPlanDate > 0)
            {
                if (nCboPlanDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate = '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate != '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate > '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate < '" + dIssueStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate>= '" + dIssueStartDate.ToString("dd MMM yyyy") + "' AND PlanDate < '" + dSendingEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate< '" + dIssueStartDate.ToString("dd MMM yyyy") + "' OR PlanDate > '" + dSendingEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion


            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BuyerID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
            }
            #endregion

            if (bIsOnlyforApprove)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(TAPStatus,0)=2 AND ISNULL(ApprovedBy,0)!=0";
            }

            sReturn = sReturn1 + sReturn + " ORDER BY ProductionFactoryID ASC";
            return sReturn;
        }
        #endregion

        #region TAPPrint
        public ActionResult TAPPrint(string IDs, int nCountID)
        {
            _oTAP = new TAP();
            _oTAPs = new List<TAP>();
            List<TAP> oTAPs = new List<TAP>();
            _oTAPDetails = new List<TAPDetail>();
            List<TAPDetail> oTempTAPDetails = new List<TAPDetail>();
            _oTAPExecutions = new List<TAPExecution>();
            for (int i = 0; i < nCountID; i++)
            {
                TAP oTAP = new TAP();
                oTAP.TAPID = Convert.ToInt32(IDs.Split(',')[i]);
                _oTAPs.Add(oTAP);
            }
            _oTAP.TAPs = _oTAPs;
            string sSQLMain = "SELECT * FROM View_TAP WHERE TAPID IN (" + IDs + ") ORder By ProductionFactoryID ASC";
            oTAPs = TAP.Gets(sSQLMain, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_TAPDetail WHERE TAPID IN (" + IDs + ") Order By TAPID, Sequence";//Get Detail
            oTempTAPDetails = TAPDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (TAPDetail oItem in oTempTAPDetails)
            {
                if (oItem.OrderStepParentID == 1)
                {
                    _oTAPDetails.Add(oItem);
                }
            }
            _oTAPExecutions = TAPExecution.GetsByTaPs(_oTAP, (int)Session[SessionInfo.currentUserID]);
            foreach (TAPDetail oItem in _oTAPDetails)
            {
                oItem.ChildOrderSteps = GetChildSteps(oItem.OrderStepID, oItem.TAPID, oTempTAPDetails);
                foreach (TAPDetail oChildItem in oItem.ChildOrderSteps)
                {
                    oChildItem.TAPExecution = GetTAPExecution(oChildItem.TAPDetailID, _oTAPExecutions);//For each one Detail get one Execution
                }
                oItem.TAPExecutions = GetChildTAPExecutions(oItem.OrderStepID, oItem.TAPID, _oTAPExecutions);
                oItem.TAPExecution = GetTAPExecution(oItem.TAPDetailID, _oTAPExecutions);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptTAPFollowUp oReport = new rptTAPFollowUp();
            byte[] abytes = oReport.PrepareReport(oTAPs, _oTAPDetails, oCompany);
            return File(abytes, "application/pdf");
        }

        #endregion

        #region HTTP Post Functions
        [HttpPost]
        public JsonResult Save(TAPExecution oTAPExecution)
        {
            _oTAPExecutions = new List<TAPExecution>();
            _oTAPExecution = new TAPExecution();
            try
            {
                if (oTAPExecution.RequiredDataTypeInInt == (int)EnumRequiredDataType.Date)
                {
                    if (oTAPExecution.UpdatedData != "")
                    {
                        oTAPExecution.UpdatedData = Convert.ToDateTime(oTAPExecution.UpdatedData).ToString("dd MMM yyyy");
                    }
                }
                _oTAPExecution = oTAPExecution.SingleSave((int)Session[SessionInfo.currentUserID]);
                _oTAPExecution.TTAPExecution = MakeTree(_oTAPExecution.TAPExecutions);
            }
            catch (Exception ex)
            {
                _oTAPExecution = new TAPExecution();
                _oTAPExecution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPExecution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Done
        [HttpPost]
        public JsonResult Done(TAPExecution oTAPExecution)
        {
            _oTAPExecutions = new List<TAPExecution>();
            _oTAPExecution = new TAPExecution();
            try
            {
                _oTAPExecution = oTAPExecution.Done((int)Session[SessionInfo.currentUserID]);
                _oTAPExecution.TTAPExecution = MakeTree(_oTAPExecution.TAPExecutions);
            }
            catch (Exception ex)
            {
                _oTAPExecution = new TAPExecution();
                _oTAPExecution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPExecution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region JObs Wise Actions
        [HttpPost]
        public JsonResult GetJobs(Job oJob)
        {
            _oJobs = new List<Job>();
            try
            {
                string sSQL = "SELECT * FROM View_Job WHERE JobID IN (SELECT JobID FROM JobDetail WHERE OrderRecapID IN (SELECT OrderRecapID FROM TAP)) AND BUID ="+oJob.BUID;
                if(!string.IsNullOrEmpty(oJob.JobNo))
                {
                    sSQL += " AND JobNo LIKE '%"+oJob.JobNo+"%'";
                }
                _oJobs = Job.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oJob = new Job();
                _oJob.ErrorMessage = ex.Message;
                _oJobs.Add(_oJob);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oJobs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult GetAllTAPsForJob(Job oJob)
        {
            _oTAPs = new List<TAP>();
            try
            {
                string sSQL ="SELECT * FROM View_TAP WHERE OrderRecapID In (SELECT OrderRecapID FROM JobDetail WHERE JobID = "+oJob.JobID+") ";
                _oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTAP = new TAP();
                _oTAP.ErrorMessage = ex.Message;
                _oTAPs.Add(_oTAP);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult GetTAPDetailsForExecution(TAP oTAP)
        {
            _oTAPDetails = new List<TAPDetail>();
            string sSQL = "SELECT * FROM View_TAPDetail WHERE TAPID=" + oTAP.TAPID + " Order By OrderStepID, OrderStepParentID, Sequence";
            _oTAPDetails = TAPDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
         
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult JobWiseSave(TAPExecution oTAPExecution)
        {
            _oTAPExecutions = new List<TAPExecution>();
            _oTAPExecution = new TAPExecution();
            try
            {

                string sSQL = "SELECT * FROM View_TAPDetail  WHERE TAPID IN (SELECT TAPID FROM TAP   WHERE OrderRecapID IN (SELECT OrderRecapID FROM JobDetail WHERE JobID = " + oTAPExecution.JobID + ")) AND OrderStepID = "+oTAPExecution.OrderStepID;
                _oTAPDetails = TAPDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_TAPExecution  WHERE TAPID IN (SELECT TAPID FROM TAP   WHERE OrderRecapID IN (SELECT OrderRecapID FROM JobDetail WHERE JobID = " + oTAPExecution.JobID + ")) AND OrderStepID = " + oTAPExecution.OrderStepID;
                _oTAPExecutions = TAPExecution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (TAPDetail oItem in _oTAPDetails)
                {
                    _oTAPExecution = new TAPExecution();
                    _oTAPExecution.TAPDetailID = oItem.TAPDetailID;
                    _oTAPExecution.TAPID = oItem.TAPID;
                    TAPExecution oTempTAPExecution = new TAPExecution();
                    oTempTAPExecution = _oTAPExecutions.Where(x => x.TAPDetailID == oItem.TAPDetailID & x.OrderStepID == oTAPExecution.OrderStepID).FirstOrDefault();
                    _oTAPExecution.TAPExecutionID =  oTempTAPExecution!=null? oTempTAPExecution.TAPExecutionID:0;
                    _oTAPExecution.OrderStepID = oTAPExecution.OrderStepID;
                    _oTAPExecution.UpdatedData = oTAPExecution.UpdatedData;
                    _oTAPExecution.RequiredDataTypeInInt =oTAPExecution.RequiredDataTypeInInt;
                    if (_oTAPExecution.RequiredDataTypeInInt == (int)EnumRequiredDataType.Date)
                    {
                        if (_oTAPExecution.UpdatedData != "")
                        {
                            _oTAPExecution.UpdatedData = Convert.ToDateTime(oTAPExecution.UpdatedData).ToString("dd MMM yyyy");
                        }
                    }
                    _oTAPExecution = _oTAPExecution.SingleSave((int)Session[SessionInfo.currentUserID]);
                }
                _oTAPExecutions = new List<TAPExecution>();
                _oTAPExecutions = _oTAPExecution.TAPExecutions.Where(x => x.OrderStepID == oTAPExecution.OrderStepParentID | x.OrderStepParentID == oTAPExecution.OrderStepParentID).OrderBy(x => x.OrderStepParentID).ToList();
                _oTAPExecution.TTAPExecution = MakeTree(_oTAPExecutions);
            }
            catch (Exception ex)
            {
                _oTAPExecution = new TAPExecution();
                _oTAPExecution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPExecution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult JobWiseDone(TAPExecution oTAPExecution)
        {
            _oTAPExecutions = new List<TAPExecution>();
            _oTAPExecution = new TAPExecution();
            try
            {

                string sSQL = "SELECT * FROM View_TAPDetail  WHERE TAPID IN (SELECT TAPID FROM TAP   WHERE OrderRecapID IN (SELECT OrderRecapID FROM JobDetail WHERE JobID = " + oTAPExecution.JobID + ")) AND OrderStepID = " + oTAPExecution.OrderStepID;
                _oTAPDetails = TAPDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_TAPExecution  WHERE TAPID IN (SELECT TAPID FROM TAP   WHERE OrderRecapID IN (SELECT OrderRecapID FROM JobDetail WHERE JobID = " + oTAPExecution.JobID + ")) AND OrderStepID = " + oTAPExecution.OrderStepID;
                _oTAPExecutions = TAPExecution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (TAPDetail oItem in _oTAPDetails)
                {
                    _oTAPExecution = new TAPExecution();
                    _oTAPExecution.TAPDetailID = oItem.TAPDetailID;
                    _oTAPExecution.TAPID = oItem.TAPID;
                    TAPExecution oTempTAPExecution = new TAPExecution();
                    oTempTAPExecution = _oTAPExecutions.Where(x => x.TAPDetailID == oItem.TAPDetailID & x.OrderStepID == oTAPExecution.OrderStepID).FirstOrDefault();
                    if (oTAPExecution.RequiredDataTypeInInt!=2 && oTempTAPExecution == null)
                    {
                        throw new Exception("Sorry, There is No TAP Exection for TAP"+oItem.OrderRecapNo+", Pleas Update Data first Then Executed.");
                    }
                   
                    _oTAPExecution.TAPExecutionID = oTempTAPExecution == null?0:oTempTAPExecution.TAPExecutionID;
                    _oTAPExecution.OrderStepID = oTAPExecution.OrderStepID;
                    _oTAPExecution.RequiredDataTypeInInt = oTAPExecution.RequiredDataTypeInInt;
                    _oTAPExecution = _oTAPExecution.Done((int)Session[SessionInfo.currentUserID]);
                }

                _oTAPExecutions = new List<TAPExecution>();
                _oTAPExecutions = _oTAPExecution.TAPExecutions.Where(x => x.OrderStepID == oTAPExecution.OrderStepParentID | x.OrderStepParentID == oTAPExecution.OrderStepParentID).OrderBy(x => x.OrderStepParentID).ToList();
                _oTAPExecution.TTAPExecution = MakeTree(_oTAPExecutions);
            }
            catch (Exception ex)
            {
                _oTAPExecution = new TAPExecution();
                _oTAPExecution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPExecution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
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
    
    
    }
}