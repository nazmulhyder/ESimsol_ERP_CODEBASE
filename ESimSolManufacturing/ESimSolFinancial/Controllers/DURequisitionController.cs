using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class DURequisitionController : Controller
    {
        #region Declaration
        DURequisition _oDURequisition = new DURequisition();
        List<DURequisition> _oDURequisitions = new List<DURequisition>();
        int _nBUID = 0;
        #endregion

        #region Action/JSon Result
        public ActionResult ViewDURequisitions(int OptType,int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DURequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            #region Issue Stores
            oIssueStores = new List<WorkingUnit>();
            oIssueStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DURequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            #endregion
            #region Received Stores
            oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DURequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

           
            string sIssueStoreIDs = string.Join(",", oIssueStores.Select(x=>x.WorkingUnitID).ToList());
            string sReceivedStoreIDs = string.Join(",", oReceivedStores.Select(x => x.WorkingUnitID).ToList());

            if (string.IsNullOrEmpty(sIssueStoreIDs))
                sIssueStoreIDs = "0";
            if (string.IsNullOrEmpty(sReceivedStoreIDs))
                sReceivedStoreIDs = "0";

            List<DURequisition> oDURequisitions = new List<DURequisition>();
            string sSQL = "SELECT * FROM View_DURequisition WHERE OperationUnitType=" + OptType + " and ISNULL(ReceiveByID,0)=0 AND WorkingUnitID_Issue IN (" + sIssueStoreIDs + ") AND WorkingUnitID_Receive IN (" + sReceivedStoreIDs + ") ORDER BY DBServerDateTime DESC";
            oDURequisitions = DURequisition.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.RequisitionTypes = DURequisitionSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DURequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            #region Stores
            ViewBag.IssueStores = oIssueStores;
            ViewBag.ReceivedStores = oReceivedStores; 
            #endregion

            ViewBag.DUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OptType = OptType;
            return View(oDURequisitions);
        }
        public ActionResult ViewDURequisition(int OptType,int nId, int buid, double ts)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            DURequisition oDURequisition = new DURequisition();

            if (nId > 0)
            {
                oDURequisition = oDURequisition.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDURequisition.DURequisitionDetails = DURequisitionDetail.Gets(oDURequisition.DURequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else 
            {
                oDURequisition.RequisitionByName = ((User)Session[SessionInfo.CurrentUser]).UserName;
                oDURequisition.OpeartionUnitType = (EnumOperationUnitType)OptType;
            }
            oDURequisition.BUID = buid;
            #region Issue Stores
            oIssueStores = new List<WorkingUnit>();
            oIssueStores = WorkingUnit.GetsPermittedStore(oDURequisition.BUID, EnumModuleName.DURequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region Received Stores
            oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(oDURequisition.BUID, EnumModuleName.DURequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.IssueStores = oIssueStores;
            ViewBag.ReceivedStores = oReceivedStores;
            
            ViewBag.BUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RequisitionTypes = DURequisitionSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            oDURequisition.BUID = buid;
            //oDURequisition.OperationUnitTypeInt = OptType;
            return View(oDURequisition);
        }
        public ActionResult AdvSearchDURequisition()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult Save(DURequisition oDURequisition)
        {
            oDURequisition.RemoveNulls();
            _oDURequisition = new DURequisition();
            oDURequisition.ReqDate = Convert.ToDateTime(oDURequisition.ReqDate.ToString("dd MMM yyyy"));
            oDURequisition.ReqDate= oDURequisition.ReqDate.AddHours(DateTime.Now.Hour);
            oDURequisition.ReqDate= oDURequisition.ReqDate.AddMinutes(DateTime.Now.Minute);
            oDURequisition.ReqDate=oDURequisition.ReqDate.AddSeconds(DateTime.Now.Second);
            try
            {
                _oDURequisition = oDURequisition.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDURequisition.DURequisitionID > 0)
                {
                    _oDURequisition.DURequisitionDetails = DURequisitionDetail.Gets(_oDURequisition.DURequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oDURequisition = new DURequisition();
                _oDURequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDURequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(DURequisition oDURequisition)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDURequisition.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ApproveDURequisition(DURequisition oDURequisition)
        {
            _oDURequisition = new DURequisition();
            _oDURequisition = oDURequisition.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDURequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApproveDURequisition(DURequisition oDURequisition)
        {
            _oDURequisition = new DURequisition();
            _oDURequisition = oDURequisition.UndoApprove(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDURequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult IssueDURequisition(DURequisition oDURequisition)
        {
            _oDURequisition = new DURequisition();
            _oDURequisition = oDURequisition.Issue(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDURequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoIssueDURequisition(DURequisition oDURequisition)
        {
            _oDURequisition = new DURequisition();
            _oDURequisition = oDURequisition.UndoIssue(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDURequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RecivedDURequisition(DURequisition oDURequisition)
        {
            _oDURequisition = new DURequisition();
            _oDURequisition = oDURequisition.Receive(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDURequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region
        public ActionResult ViewDURequisitions_Open(int OptType, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DURequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            #region Issue Stores
            oIssueStores = new List<WorkingUnit>();
            oIssueStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DURequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            #endregion
            #region Received Stores
            oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DURequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            #endregion


            string sIssueStoreIDs = string.Join(",", oIssueStores.Select(x => x.WorkingUnitID).ToList());
            string sReceivedStoreIDs = string.Join(",", oReceivedStores.Select(x => x.WorkingUnitID).ToList());

            if (string.IsNullOrEmpty(sIssueStoreIDs))
                sIssueStoreIDs = "0";
            if (string.IsNullOrEmpty(sReceivedStoreIDs))
                sReceivedStoreIDs = "0";

            List<DURequisition> oDURequisitions = new List<DURequisition>();
            string sSQL = "SELECT * FROM View_DURequisition WHERE ISNULL(ReceiveByID,0)=0 AND IsOpenOrder=1 AND WorkingUnitID_Issue IN (" + sIssueStoreIDs + ") AND WorkingUnitID_Receive IN (" + sReceivedStoreIDs + ") ORDER BY DBServerDateTime DESC";
            oDURequisitions = DURequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.RequisitionTypes = DURequisitionSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DURequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            #region Stores
            ViewBag.IssueStores = oIssueStores;
            ViewBag.ReceivedStores = oReceivedStores;
            #endregion

            ViewBag.DUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OptType = OptType;
            return View(oDURequisitions);
        }
        public ActionResult ViewDURequisition_Open(int OptType, int nId, int buid, double ts)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            DURequisition oDURequisition = new DURequisition();

            if (nId > 0)
            {
                oDURequisition = oDURequisition.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDURequisition.DURequisitionDetails = DURequisitionDetail.Gets(oDURequisition.DURequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                oDURequisition.RequisitionByName = ((User)Session[SessionInfo.CurrentUser]).UserName;
                oDURequisition.OpeartionUnitType = (EnumOperationUnitType)OptType;
            }
            oDURequisition.IsOpenOrder = true;
            oDURequisition.BUID = buid;
            #region Issue Stores
            oIssueStores = new List<WorkingUnit>();
            oIssueStores = WorkingUnit.GetsPermittedStore(oDURequisition.BUID, EnumModuleName.DURequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region Received Stores
            oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(oDURequisition.BUID, EnumModuleName.DURequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.IssueStores = oIssueStores;
            ViewBag.ReceivedStores = oReceivedStores;

            ViewBag.BUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RequisitionTypes = DURequisitionSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            oDURequisition.BUID = buid;
            return View(oDURequisition);
        }
        #endregion

        #region  Search
        [HttpPost]
        public JsonResult GetsDURByNo(DURequisition oDURequisition)
        {
            List<DURequisition> oDURequisitions = new List<DURequisition>();
            try
            {
                string sReturn1 = "SELECT * FROM View_DURequisition";
                string sReturn = "";


                #region Issue Store
                List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
                oIssueStores = WorkingUnit.GetsPermittedStore(oDURequisition.BUID, EnumModuleName.DURequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                  
                string sIssueStoreIDs = string.Join(",", oIssueStores.Select(x => x.WorkingUnitID).ToList());
                Global.TagSQL(ref sReturn);
                if (string.IsNullOrEmpty(sIssueStoreIDs))
                {
                    sIssueStoreIDs = "0";
                }
                sReturn = sReturn + " WorkingUnitID_Issue IN ( " + sIssueStoreIDs + " )";
                #endregion

                #region Receive Store
                List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
                oReceivedStores = WorkingUnit.GetsPermittedStore(oDURequisition.BUID, EnumModuleName.DURequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
                string sReceivedStoreIDs = string.Join(",", oReceivedStores.Select(x => x.WorkingUnitID).ToList());
                if (string.IsNullOrEmpty(sReceivedStoreIDs))
                {
                    sReceivedStoreIDs = "0";
                }
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkingUnitID_Receive IN ( " + sReceivedStoreIDs + " )";
                #endregion

                #region Requisition No
                if (!string.IsNullOrEmpty(oDURequisition.RequisitionNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " RequisitionNo LIKE '%" + oDURequisition.RequisitionNo + "%'";
                }
                #endregion

                #region Order No
                if (!string.IsNullOrEmpty(oDURequisition.ErrorMessage))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DURequisitionID IN (SELECT DURequisitionID FROM DURequisitionDetail WHERE DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE OrderNo LIKE '%" + oDURequisition.ErrorMessage + "%' ))";
                }
                #endregion

                #region OperationUnitTypeInt
                if (oDURequisition.OperationUnitTypeInt>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OperationUnitType=" + oDURequisition.OperationUnitTypeInt;
                }
                #endregion

                string sSQL = sReturn1 + sReturn;
                oDURequisitions = DURequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDURequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDURequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult AdvSearch(string sTemp)
        {
            List<DURequisition> oDURequisitions = new List<DURequisition>();
            DURequisition oDURequisition = new DURequisition();
            try
            {
                string sSQL = GetSQL(sTemp);
                oDURequisitions = DURequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDURequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDURequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            int nCount = 0;
            int nApproveDateCompare = 0;
            int nOperationUnitType = 0;
            DateTime dtApproveDateStart = DateTime.Now;
            DateTime dtApproveEndDate = DateTime.Now;

            string sRequisitionNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            int nRequisitionType = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            int nOrderType = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            string sOrderNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            string sLotNo = (sTemp.Split('~')[nCount++]);
            int nIssueStore = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            int nReceiveStore = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            int nDURequisitionDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtDURequisitionDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtDURequisitionEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);

            int nStatus = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            int nReceiveDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtReceiveEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            string sYarnIDs = (sTemp.Split('~')[nCount++]);
            string sContractorIDs = (sTemp.Split('~')[nCount++]);

            bool nYTApprove = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTIssue = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            bool nYTReceive = Convert.ToBoolean(sTemp.Split('~')[nCount++]);
            _nBUID = Convert.ToInt32(sTemp.Split('~')[nCount++]);

            if(sTemp.Split('~').Count() > 20)
                nApproveDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            if (sTemp.Split('~').Count() > 21)
                dtApproveDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            if (sTemp.Split('~').Count() > 22)
                dtApproveEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            if (sTemp.Split('~').Count() > 23)
                nOperationUnitType = Convert.ToInt16(sTemp.Split('~')[nCount++]);

            
            string sReturn1 = "SELECT * FROM View_DURequisition";
            string sReturn = "";

            //#region BUID
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "BUID = " + nBUID;
            //}
            //#endregion
            #region OperationUnitTypeInt
            if (nOperationUnitType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OperationUnitType=" + nOperationUnitType;
            }
            #endregion

            #region Requisition No
            if (!string.IsNullOrEmpty(sRequisitionNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequisitionNo LIKE '%" + sRequisitionNo + "%'";
            }
            #endregion

            #region Requisition Type
            if (nRequisitionType != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequisitionType = " + nRequisitionType;
            }
            #endregion

            #region Order Type
            if (nOrderType != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DURequisitionID IN (SELECT DURequisitionID FROM View_DURequisitionDetail WHERE DyeingOrderType = " + nOrderType + " AND DyeingOrderNo LIKE '%" + (string.IsNullOrEmpty(sOrderNo) ? "" : sOrderNo) + "%')";
            }
            #endregion

            #region Lot No
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DURequisitionID IN (SELECT DURequisitionID FROM View_DURequisitionDetail WHERE LotNo LIKE '%" + sLotNo + "%' )";
            }
            #endregion

            #region Issue Store
            if (nIssueStore != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkingUnitID_Issue = " + nIssueStore;
            }
            else 
            {
                #region Issue Stores
                oIssueStores = new List<WorkingUnit>();
                oIssueStores = WorkingUnit.GetsPermittedStore(_nBUID, EnumModuleName.DURequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                #endregion
                string sIssueStoreIDs = string.Join(",", oIssueStores.Select(x => x.WorkingUnitID).ToList());
                Global.TagSQL(ref sReturn);
                if (string.IsNullOrEmpty(sIssueStoreIDs))
                {
                    sIssueStoreIDs="0";
                }
                sReturn = sReturn + " WorkingUnitID_Issue IN ( " + sIssueStoreIDs + " )";
            }
            #endregion

            #region Receive Store
            if (nReceiveStore != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkingUnitID_Receive = " + nReceiveStore;
            }
            else 
            {
                #region Received Stores
                oReceivedStores = new List<WorkingUnit>();
                oReceivedStores = WorkingUnit.GetsPermittedStore(_nBUID, EnumModuleName.DURequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
                #endregion
                string sReceivedStoreIDs = string.Join(",", oReceivedStores.Select(x => x.WorkingUnitID).ToList());
                if (string.IsNullOrEmpty(sReceivedStoreIDs))
                {
                    sReceivedStoreIDs = "0";
                }
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkingUnitID_Receive IN ( " + sReceivedStoreIDs + " )";
            }
            #endregion

            #region Requisition Date Wise
            if (nDURequisitionDateCompare > 0)
            {
                if (nDURequisitionDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDURequisitionDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDURequisitionDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDURequisitionDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDURequisitionDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDURequisitionDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDURequisitionDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDURequisitionDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDURequisitionDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDURequisitionDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDURequisitionEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nDURequisitionDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReqDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDURequisitionDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDURequisitionEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region ReceiveDate Wise
            if (nReceiveDateCompare > 0)
            {
                if (nReceiveDateCompare == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nReceiveDateCompare == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtReceiveEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Yarn
            if (!string.IsNullOrEmpty(sYarnIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DURequisitionID IN (SELECT DURequisitionID FROM DURequisitionDetail WHERE ProductID IN (" + sYarnIDs + "))";
            }
            #endregion

            #region Contractor
            if (!string.IsNullOrEmpty(sContractorIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DURequisitionID IN (SELECT DURequisitionID FROM DURequisitionDetail WHERE DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE ContractorID IN (" + sContractorIDs + ")))";
            }
            #endregion

            #region nYTApprove
            if (nYTApprove)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(ApprovebyID,0)= 0";
            }
            #endregion 
            #region nYTIssue
            if (nYTIssue)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(ApprovebyID,0)!= 0 AND ISNULL(IssuebyID,0)= 0";
            }
            #endregion
            #region nYTReceive
            if (nYTReceive)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(ApprovebyID,0)!= 0 AND ISNULL(IssuebyID,0)!= 0 AND ISNULL(ReceiveByID,0)= 0";
            }
            #endregion

            #region In/Out Side
            if (nStatus != 0)
            {
                if (nStatus == 2) nStatus = 0;
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DURequisitionID IN (SELECT DURequisitionID FROM DURequisitionDetail WHERE DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE DyeingOrderType IN (SELECT OrderType FROM DUOrderSetup WHERE IsInHouse = " + nStatus + "))) ";
            }
            #endregion

            #region Approve Date Wise
            if (nApproveDateCompare > 0)
                DateObject.CompareDateQuery(ref sReturn, "ApproveDate", nApproveDateCompare, dtApproveDateStart, dtApproveEndDate);
            #endregion
            
           
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        #endregion

        #region Product
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product>  oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.DURequisition, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.DURequisition, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetsDyeingOrder
        [HttpPost]
        public JsonResult GetsDyeingOrder(DURequisition oDURequisition)
        {
            string sSQL="";
            List<DUProductionYetTo> oDUProductionYetTos = new List<DUProductionYetTo>();
            List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();
            List<LotParent> oLotParents = new List<LotParent>();
            if (oDURequisition.RequisitionType == EnumInOutType.Receive)
            {
                if (oDURequisition.ObjectID >= 0) //!string.IsNullOrEmpty(oDUProductionYetTo.OrderNo) //NoValidation*
                {
                    sSQL = "SELECT TOP 200 * FROM View_DUProductionYetToFORReq WHERE DyeingOrderID<>0 and DyeingOrderType not in (" + (int)EnumOrderType.LoanOrder + ")";

                    if (oDURequisition.DyeingOrderType > 0)
                        sSQL += "AND DyeingOrderType=" + oDURequisition.DyeingOrderType;
                    if (!string.IsNullOrEmpty(oDURequisition.OrderNo))
                        sSQL += " AND OrderNo LIKE '%" + oDURequisition.OrderNo + "%' ";
                     if (!string.IsNullOrEmpty(oDURequisition.Params))
                     {
                    if (!string.IsNullOrEmpty(oDURequisition.Params) && oDURequisition.Params == "NotInHouse")
                    { sSQL += " AND IsInHouse=0"; }
                    else
                    { sSQL += " AND IsInHouse=1"; }
                     }

                    sSQL += " ORDER BY OrderType, OrderNo DESC";
                    oDUProductionYetTos = DUProductionYetTo.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                foreach (DUProductionYetTo oItem in oDUProductionYetTos)
                {
                    if ((oItem.Qty - oItem.Qty_Prod) > 0)
                    {
                        DURequisitionDetail oDURequisitionDetail = new DURequisitionDetail();
                        oDURequisitionDetail.DURequisitionID = 0;// oDUProductionYetTo.ObjectID;
                        oDURequisitionDetail.MUnit = oItem.MUName;
                        //oDURequisitionDetail.MUnitID = oItem.M;
                        oDURequisitionDetail.Qty = (oItem.Qty - oItem.Qty_Req);
                        if (oDURequisitionDetail.Qty < 0) { oDURequisitionDetail.Qty = 0; }
                        oDURequisitionDetail.Qty_Preq = oItem.Qty_Req;
                        oDURequisitionDetail.Qty_Order = oItem.Qty;
                        if (oItem.IsInHouse == true)
                        {
                            oDURequisitionDetail.Qty = Math.Ceiling(oDURequisitionDetail.Qty);
                            oDURequisitionDetail.Qty_Order = Math.Ceiling(oDURequisitionDetail.Qty_Order);
                        }
                        oDURequisitionDetail.BagNo = 0;
                        oDURequisitionDetail.LotID = 0;
                        oDURequisitionDetail.ProductID = oItem.ProductID;
                        oDURequisitionDetail.ContractorID = oItem.ContractorID;
                        oDURequisitionDetail.ProductName = oItem.ProductName;
                        oDURequisitionDetail.DyeingOrderID = oItem.DyeingOrderID;//ForPickerOnly
                        oDURequisitionDetail.DyeingOrderNo = oItem.OrderNo;//ForPickerOnly
                        oDURequisitionDetail.BuyerName = oItem.ContractorName;//ForPickerOnly
                        oDURequisitionDetail.IsInHouse = oItem.IsInHouse;//ForPickerOnly
                        oDURequisitionDetail.OrderType = oItem.DyeingOrderType;
                        oDURequisitionDetails.Add(oDURequisitionDetail);
                    }
                }

            }
            else
            {
                sSQL = "SELECT TOP 200 * FROM View_DURequisitionDetail WHERE DyeingOrderID<>0";
                if (oDURequisition.DyeingOrderType > 0)
                    sSQL += "AND DyeingOrderType=" + oDURequisition.DyeingOrderType;
                if (!string.IsNullOrEmpty(oDURequisition.OrderNo))
                    sSQL += " AND DyeingOrderNo LIKE '%" + oDURequisition.OrderNo + "%' ";
                if (!string.IsNullOrEmpty(oDURequisition.Params))
                {
                    if (!string.IsNullOrEmpty(oDURequisition.Params) && oDURequisition.Params == "NotInHouse")
                    { sSQL += " AND IsInHouse=0"; }
                    else
                    { sSQL += " AND IsInHouse=1"; }
                }
                sSQL += " ORDER BY DyeingOrderType, DyeingOrderNo DESC";
                oDURequisitionDetails = DURequisitionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDURequisitionDetails.ForEach(o => o.LotID = o.DestinationLotID);
                oDURequisitionDetails.ForEach(o => o.DestinationLotID = 0);
                oDURequisitionDetails.ForEach(o => o.DURequisitionDetailID = 0);
                oDURequisitionDetails.ForEach(o => o.DURequisitionID = 0);

                oDURequisitionDetails = oDURequisitionDetails.GroupBy(x => new { x.LotID, x.LotNo, x.ProductName, x.ProductID, x.RequisitionType, x.DyeingOrderNo, x.BuyerName, x.MUnitID, x.MUnit, x.DyeingOrderID }, (key, grp) =>
                                          new DURequisitionDetail
                                          {
                                              LotNo = key.LotNo,
                                              LotID = key.LotID,
                                              ProductName = key.ProductName,
                                              ProductID = key.ProductID,
                                              RequisitionType = key.RequisitionType,
                                              DyeingOrderNo = key.DyeingOrderNo,
                                              BuyerName = key.BuyerName,
                                              Qty_Order = Math.Round(grp.Sum(p => p.Qty_Order),2),
                                              Qty = Math.Round(grp.Sum(p => p.Qty),2),
                                              MUnitID = key.MUnitID,
                                              DyeingOrderID = key.DyeingOrderID,
                                              MUnit = key.MUnit,

                                          }).ToList();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDURequisitionDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets Lot
        [HttpPost]
        public JsonResult GetsLot(LotParent oLotParent)
        {
            List<Lot> oLots = new List<Lot>();
            List<LotParent> oLotParents = new List<LotParent>();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            string sLotID = "";
            string sIssueStoreIDs = "";
            string sParentLotID = "";
            string sSQL = "";
            try
            {
                if (oLotParent.WorkingUnitID<=0)
                {
                    oIssueStores = new List<WorkingUnit>();
                    oIssueStores = WorkingUnit.GetsPermittedStore(oLotParent.BUID, EnumModuleName.DURequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                    sIssueStoreIDs = string.Join(",", oIssueStores.Select(x => x.WorkingUnitID).ToList());
                    if (string.IsNullOrEmpty(sIssueStoreIDs))
                        sIssueStoreIDs = "0";
                }


                oLotParent.LotNo = (!string.IsNullOrEmpty(oLotParent.LotNo)) ? oLotParent.LotNo.Trim() : "";
                oDUOrderSetup = oDUOrderSetup.GetByType(oLotParent.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUOrderSetup.IsOpenRawLot == false)
                {
                    if (oDUOrderSetup.IsInHouse)//   if (oLotParent.Params == "IsInHouse")
                    {
                        sSQL = "Select * from View_FabricLotAssign where FEOSDID in (Select FEOSDID from DyeingOrderFabricDetail where DyeingOrderID=" + oLotParent.DyeingOrderID + " )";
                        //if (!string.IsNullOrEmpty(oLotParent.LotNo))
                        //    sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";
                        if (oLotParent.ProductID > 0)
                            sSQL = sSQL + " And ProductID=" + oLotParent.ProductID;
                        //if (oLotParent.DyeingOrderID > 0)
                        //    sSQL = sSQL + " And DyeingOrderID=" + oLotParent.DyeingOrderID;
                        oFabricLotAssigns = FabricLotAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        sLotID = string.Join(",", oFabricLotAssigns.Select(x => x.LotID).Distinct().ToList());
                        sParentLotID = string.Join(",", oFabricLotAssigns.Where(b => b.ParentLotID > 0).Select(x => x.ParentLotID).Distinct().ToList());

                        if (!string.IsNullOrEmpty(sParentLotID))
                        {
                            sLotID = sLotID + "," + sParentLotID;
                        }

                        if (oFabricLotAssigns.Count <= 0)
                        {
                            throw new Exception("Lot yet not assign with this order!!");
                        }
                    }
                    else
                    {
                        sSQL = "Select * from View_LotParent Where LotID<>0 and isnull(BalanceLot,0)>0.1";
                        //if (!string.IsNullOrEmpty(oLotParent.LotNo))
                        //    sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";

                        if (oLotParent.ProductID > 0)
                            sSQL = sSQL + " And ProductID=" + oLotParent.ProductID;
                        
                        if (oLotParent.DyeingOrderID > 0)
                            sSQL = sSQL + " And DyeingOrderID=" + oLotParent.DyeingOrderID;
                        oLotParents = LotParent.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        sLotID = string.Join(",", oLotParents.Select(x => x.LotID).Distinct().ToList());
                        sParentLotID = string.Join(",", oLotParents.Where(b => b.ParentLotID > 0).Select(x => x.ParentLotID).Distinct().ToList());

                        if (!string.IsNullOrEmpty(sParentLotID))
                        {
                            sLotID = sLotID + "," + sParentLotID;
                        }
                        if (oLotParent.DyeingOrderID <= 0)
                        {
                            throw new Exception("Lot yet not assign with this order!!");
                        }
                    }
                }

                sSQL = "Select * from View_Lot Where Balance>0 and WorkingUnitID in (Select WorkingUnitID from WorkingUnit where UnitType=" + (int)EnumWoringUnitType.Raw + ")";

                if (oLotParent.WorkingUnitID > 0)
                {
                    sSQL = sSQL + " And WorkingUnitID=" + oLotParent.WorkingUnitID;
                }
                else
                {
                    sSQL = sSQL + " And WorkingUnitID in ("+ sIssueStoreIDs+")";
                }
                //if (oLotParent.ContractorID > 0) { sSQL = sSQL + " And ContractorID=" + oLotParent.ContractorID; }
                if (oDUOrderSetup.IsOpenRawLot == true)
                {
                    if (!string.IsNullOrEmpty(oLotParent.LotNo))
                        sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";

                    if (oLotParent.ProductID > 0)
                        sSQL = sSQL + " And ProductID=" + oLotParent.ProductID;

                }
                else
                {
                    if (!string.IsNullOrEmpty(sLotID))
                        sSQL = sSQL + " And  (LotID in (" + sLotID + ") or ParentLotID in (" + sLotID + "))";

                    if (string.IsNullOrEmpty(sLotID))
                    {
                        throw new Exception("Lot yet not assign with this order!!");
                    }
                }
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oDUOrderSetup.IsOpenRawLot == false)
                {
                    if (oDUOrderSetup.IsInHouse)
                    {
                        oLots.ForEach(x =>
                        {
                            if (oFabricLotAssigns.FirstOrDefault() != null && oFabricLotAssigns.FirstOrDefault().ProductID > 0 && oFabricLotAssigns.Where(b => (b.LotID == x.LotID || b.ParentLotID == x.ParentLotID || b.LotID == x.ParentLotID)).Count() > 0)
                            {
                                x.OrderRecapNo = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID || p.ParentLotID == x.ParentLotID) && p.ProductID > 0).FirstOrDefault().ExeNo;
                                x.StockValue = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).Sum(a => a.Qty);
                                //x.ProductName = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).FirstOrDefault().ProductNameLot;
                                // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                            }
                        });
                    }
                    else
                    {
                        oLots.ForEach(x =>
                        {
                            if (oLotParents.FirstOrDefault() != null && oLotParents.FirstOrDefault().DyeingOrderID > 0 && oLotParents.Where(b => (b.LotID == x.LotID || b.LotID == x.ParentLotID || b.LotID == x.ParentLotID)).Count() > 0)
                            {
                                x.OrderRecapNo = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                                x.StockValue = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().Balance;
                                //x.ProductName = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().ProductNameLot;
                                // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLots.Add( new Lot(){ErrorMessage = ex.Message});
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetsLotSoftWinding(LotParent oLotParent)
        {

            List<Lot> oLots = new List<Lot>();
            List<LotParent> oLotParents = new List<LotParent>();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            //string sLotID = "";
            //string sParentLotID = "";
            string sSQL = "";
            try
            {
                oLotParent.LotNo = (!string.IsNullOrEmpty(oLotParent.LotNo)) ? oLotParent.LotNo.Trim() : "";

                //if (oLotParent.Params == "IsInHouse")
                //{

                //    sSQL = "Select * from View_FabricLotAssign where  FEOSDID in (Select FEOSDID from DyeingOrderFabricDetail where DyeingOrderID=" + oLotParent.DyeingOrderID + " )";
                //    //if (!string.IsNullOrEmpty(oLotParent.LotNo))
                //    //    sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";
                //    //if (oLotParent.ProductID > 0)
                //    //    sSQL = sSQL + " And ProductID=" + oLotParent.ProductID;
                //    //if (oLotParent.DyeingOrderID > 0)
                //    //    sSQL = sSQL + " And DyeingOrderID=" + oLotParent.DyeingOrderID;
                //    oFabricLotAssigns = FabricLotAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //    sLotID = string.Join(",", oFabricLotAssigns.Select(x => x.LotID).Distinct().ToList());
                //    sParentLotID = string.Join(",", oFabricLotAssigns.Where(b => b.ParentLotID > 0).Select(x => x.ParentLotID).Distinct().ToList());

                //    if (!string.IsNullOrEmpty(sParentLotID))
                //    {
                //        sLotID = sLotID + "," + sParentLotID;
                //    }

                //    if (oFabricLotAssigns.Count <= 0)
                //    {
                //        throw new Exception("Lot yet not assign with this order!!");
                //    }
                //}
                //else
                //{
                //    sSQL = "Select * from View_LotParent Where LotID<>0 ";
                //    //if (!string.IsNullOrEmpty(oLotParent.LotNo))
                //    //    sSQL = sSQL + " And LotNo Like '%" + oLotParent.LotNo + "%'";
                //    if (oLotParent.ProductID > 0)
                //        sSQL = sSQL + " And ProductID=" + oLotParent.ProductID;
                //    if (oLotParent.DyeingOrderID > 0)
                //        sSQL = sSQL + " And DyeingOrderID=" + oLotParent.DyeingOrderID;
                //    oLotParents = LotParent.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //    sLotID = string.Join(",", oLotParents.Select(x => x.LotID).Distinct().ToList());
                //    sParentLotID = string.Join(",", oLotParents.Where(b => b.ParentLotID > 0).Select(x => x.ParentLotID).Distinct().ToList());

                //    if (!string.IsNullOrEmpty(sParentLotID))
                //    {
                //        sLotID = sLotID + "," + sParentLotID;
                //    }
                //    if (oLotParent.DyeingOrderID <= 0)
                //    {
                //        throw new Exception("Lot yet not assign with this order!!");
                //    }
                //}
                if (oLotParent.DyeingOrderID > 0)
                {
                    oDyeingOrder = DyeingOrder.Get(oLotParent.DyeingOrderID, (int)Session[SessionInfo.currentUserID]);
                }


                sSQL = "Select * from View_Lot Where Balance>0 ";

                if (oLotParent.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLotParent.WorkingUnitID;
                if (oLotParent.DyeingOrderID > 0)
                {
                    if (oDyeingOrder.DyeingOrderType == (int)EnumOrderType.TwistOrder)
                    {
                        sSQL = sSQL + " And LotID in (Select LotID from TwistingDetail where InOutType=101 and TwistingID in (Select TwistingID from Twisting where DyeingOrderID=" + oLotParent.DyeingOrderID + "))"; 
                    }
                    else
                    {
                       sSQL = sSQL + " And LotID in (Select DestinationLotID from DURequisitionDetail where DyeingOrderID=" + oLotParent.DyeingOrderID + ")"; 
                    }
                }
                

                //if (!string.IsNullOrEmpty(sLotID))
                //    sSQL = sSQL + " And  (LotID in (" + sLotID + ") or ParentLotID in (" + sLotID + "))";

                //if (string.IsNullOrEmpty(sLotID))
                //{
                //    throw new Exception("Lot yet not assign with this order!!");
                //}
                if ((oLotParent.WorkingUnitID<=0))
                {
                    throw new Exception("Store Not Found!!");
                }

                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //if (oLotParent.Params == "IsInHouse")
                //{
                //    oLots.ForEach(x =>
                //    {
                //        if (oFabricLotAssigns.FirstOrDefault() != null && oFabricLotAssigns.FirstOrDefault().ProductID > 0 && oFabricLotAssigns.Where(b => (b.LotID == x.LotID || b.ParentLotID == x.LotID || b.LotID == x.ParentLotID)).Count() > 0)
                //        {
                //            x.OrderRecapNo = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).FirstOrDefault().ExeNo;
                //            x.StockValue = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).FirstOrDefault().Balance;
                //            if(x.StockValue<0)
                //            {
                //                x.StockValue = 0;
                //            }
                //            // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                //        }
                //    });
                //}
                //else
                //{
                //    oLots.ForEach(x =>
                //    {
                //        if (oLotParents.FirstOrDefault() != null && oLotParents.FirstOrDefault().DyeingOrderID > 0 && oLotParents.Where(b => (b.LotID == x.LotID || b.LotID == x.ParentLotID || b.LotID == x.ParentLotID)).Count() > 0)
                //        {
                //            x.OrderRecapNo = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                //            x.StockValue = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().Balance;
                //            // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                //        }
                //    });
                //}
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLots.Add(new Lot() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Gets
        [HttpPost]
        public JsonResult GetsLot_ForOpen(Lot oLot)
        {
            List<Lot> _oLots = new List<Lot>();
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            string sIssueStoreIDs = "";
            try
            {
                oIssueStores = new List<WorkingUnit>();
                oIssueStores = WorkingUnit.GetsPermittedStore(oLot.BUID, EnumModuleName.DURequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                sIssueStoreIDs = string.Join(",", oIssueStores.Select(x => x.WorkingUnitID).ToList());
                if (string.IsNullOrEmpty(sIssueStoreIDs))
                    sIssueStoreIDs = "0";

                string sSQL = "SELECT * FROM View_Lot WHERE LotID<>0 ";

                sSQL = sSQL + " And WorkingUnitID in (" + sIssueStoreIDs + ")";

                if (oLot.ProductID>0)
                    sSQL = sSQL + " And ProductID = " + oLot.ProductID;
                if (oLot.BUID >0)
                    sSQL = sSQL + " And BUID = " + oLot.BUID;
                if (oLot.WorkingUnitID >0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLot.WorkingUnitID;
                if (!string.IsNullOrEmpty(oLot.LotNo))
                    sSQL = sSQL + " And LotNo LIKE '%" + oLot.LotNo + "%'";
               
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult DURequisitionPreview(int nDURequisitionID, int nBUID)
        {
            DURequisition oDURequisition = new DURequisition();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<LotParent> oLotParents = new List<LotParent>();

            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();

            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
            List<DURequisitionDetail> oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
            List<DURequisitionDetail> oDURequisitionDetails_SRM = new List<DURequisitionDetail>();

            List<DURequisitionDetail> oDURequisitionDetails_Product = new List<DURequisitionDetail>();

            DUProductionStatusReport oDOD = new DUProductionStatusReport();

            #region Print Setup
            if (nDURequisitionID > 0)
            {
                _oDURequisition = oDURequisition.Get(nDURequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                _oDURequisition.DURequisitionDetails = DURequisitionDetail.Gets(nDURequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            #endregion

            DURequisitionSetup oDURequisitionSetup = new DURequisitionSetup();
            oDURequisitionSetup = oDURequisitionSetup.GetByType(_oDURequisition.RequisitionTypeInt, (int)Session[SessionInfo.currentUserID]);

            //EXEC SP_ProductionStatusReport ' BUID =1 AND DyeingOrderID IN (SELECT DyeingOrderID FROM DURequisitionDetail WHERE DURequisitionID =15)',1
            List<DUProductionStatusReport> oDUProductionStatusReports = new List<DUProductionStatusReport>();
            oDUProductionStatusReports = DUProductionStatusReport.Gets(" where DyeingOrderID IN (SELECT DyeingOrderID FROM DURequisitionDetail WHERE DURequisitionID =" + _oDURequisition.DURequisitionID + ") ", EnumReportLayout.BankWise, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region ORDER STATUS
            if (_oDURequisition.DURequisitionDetails.Any())
            {
                string sDyeingOrderIDs = string.Join(",", _oDURequisition.DURequisitionDetails.Select(x => x.DyeingOrderID));
                oDyeingOrderReports = DyeingOrderReport.Gets("Select * from View_DyeingOrderReport where [Status]<9 and DyeingOrderID IN (" + sDyeingOrderIDs +")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                oDUOrderSetup = oDUOrderSetup.GetByType(_oDURequisition.DURequisitionDetails.FirstOrDefault().OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
                oDUProGuideLineDetails_Receive = DUProGuideLineDetail.Gets("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) != " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID IN (" + sDyeingOrderIDs + ")) ORDER BY ProductID", (int)Session[SessionInfo.currentUserID]);
                oDUProGuideLineDetails_Return = DUProGuideLineDetail.Gets("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) = " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID IN (" + sDyeingOrderIDs + ")) ORDER BY ProductID", (int)Session[SessionInfo.currentUserID]);
                
                oDURequisitionDetails_SRS = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ")  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE ISNULL(ReceiveByID,0)<>0 and RequisitionType="+(int)EnumInOutType.Receive+" ) ", (int)Session[SessionInfo.currentUserID]);
                oDURequisitionDetails_SRM = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ")  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE ISNULL(ReceiveByID,0)<>0 and RequisitionType=" + (int)EnumInOutType.Disburse + " ) ", (int)Session[SessionInfo.currentUserID]);
                oLotParents = LotParent.Gets("SELECT * FROM View_LotParent WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ") OR DyeingOrderID_Out IN (" + sDyeingOrderIDs + ")", (int)Session[SessionInfo.currentUserID]);

             

                oDyeingOrderFabricDetails = DyeingOrderFabricDetail.Gets("SELECT * FROM DyeingOrderFabricDetail WHERE DyeingOrderID IN (" + sDyeingOrderIDs + ")", (int)Session[SessionInfo.currentUserID]);

                string sProductIDs = string.Join(",", oDyeingOrderFabricDetails.Select(x => x.ProductID));

                if (!string.IsNullOrEmpty(sProductIDs))
                {
                    oDURequisitionDetails_Product = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE  ProductID not IN (" + sProductIDs + ") and DyeingOrderID IN (" + sDyeingOrderIDs + ")  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition )", (int)Session[SessionInfo.currentUserID]);
                }

                if (oDURequisitionDetails_Product.Count>0)
                {
                    foreach(DURequisitionDetail oItem in  oDURequisitionDetails_Product)
                    {
                        oDOD = new DUProductionStatusReport();
                        oDOD.ProductID = oItem.ProductID;
                        oDOD.ProductName = oItem.ProductName;
                        oDOD.DyingOrderID = oItem.DyeingOrderID;
                        oDOD.OrderNo = oItem.DyeingOrderNo;
                        oDUProductionStatusReports.Add(oDOD);
                    }
                }

                oFabricLotAssigns = FabricLotAssign.Gets("SELECT * FROM FabricLotAssign WHERE FEOSDID IN (SELECT FEOSDID FROM DyeingOrderFabricDetail WHERE DyeingOrderID IN (" + sDyeingOrderIDs + "))", (int)Session[SessionInfo.currentUserID]);
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);

            rptDURequisition oReport = new rptDURequisition();

            oReport.DUOrderSetup = oDUOrderSetup;
            oReport.LotParents = oLotParents;
            oReport.DyeingOrderReports = oDyeingOrderReports;
            oReport.DURequisitionDetails_SRS = oDURequisitionDetails_SRS;
            oReport.DURequisitionDetails_SRM = oDURequisitionDetails_SRM;
            oReport.DUProGuideLineDetails_Receive = oDUProGuideLineDetails_Receive;
            oReport.DUProGuideLineDetails_Return = oDUProGuideLineDetails_Return;
            oReport.LotParents = oLotParents;
            oReport.FabricLotAssigns = oFabricLotAssigns;
            oReport.DyeingOrderFabricDetails = oDyeingOrderFabricDetails;

            byte[] abytes = oReport.PrepareReport(_oDURequisition, oCompany, oBusinessUnit, oDURequisitionSetup, oDUProductionStatusReports);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintDURequisitionList(string sTemp)
        {
            List<DURequisition> oDURequisitions = new List<DURequisition>();
            List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oDURequisitions = DURequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oDURequisitionDetails = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DURequisitionID IN (" + string.Join(",", oDURequisitions.Select(x=>x.DURequisitionID)) + ")", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDURequisitions = new List<DURequisition>();
                oDURequisitionDetails = new List<DURequisitionDetail>();
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_nBUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

            rptDURequisitionDetails oReport = new rptDURequisitionDetails();
            byte[] abytes = oReport.PrepareReport(oDURequisitions, oDURequisitionDetails, oCompany);
            return File(abytes, "application/pdf");
        }

        public void ExcelDURequisitionList(string sTemp)
        {
            List<DURequisition> oDURequisitions = new List<DURequisition>();
            List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oDURequisitions = DURequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oDURequisitionDetails = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DURequisitionID IN (" + string.Join(",", oDURequisitions.Select(x => x.DURequisitionID)) + ")", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDURequisitions = new List<DURequisition>();
                oDURequisitionDetails = new List<DURequisitionDetail>();
            }
            if (oDURequisitions.Count > 0)
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_nBUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(_nBUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                int count = 0, nStartCol = 2, nTotalCol = 0;
                #region full excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Requisition Details");
                    sheet.Name = "Requisition Details";
                    sheet.Column(nStartCol++).Width = 5; //SL
                    sheet.Column(nStartCol++).Width = 15; //Req no
                    sheet.Column(nStartCol++).Width = 12; //Order no
                    sheet.Column(nStartCol++).Width = 25; //Buyer
                    sheet.Column(nStartCol++).Width = 40; //Yarn
                    sheet.Column(nStartCol++).Width = 18; //lot No
                    sheet.Column(nStartCol++).Width = 12; //Qty
                    sheet.Column(nStartCol++).Width = 18; //Issue dt
                    sheet.Column(nStartCol++).Width = 18; //rcv dt
                    
                    nTotalCol = nStartCol;
                    nStartCol = 2;

                    #region Report Header
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = "Requisition Details"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Requisition No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Yarn Count"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Issue Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Receive Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    rowIndex = rowIndex + 1;
                    #endregion

                    #region data
                    int nCount = 0;
                    foreach (DURequisition oItem in oDURequisitions)
                    {
                        List<DURequisitionDetail> oTempDURequisitionDetails = new List<DURequisitionDetail>();
                        oTempDURequisitionDetails = oDURequisitionDetails.Where(x => x.DURequisitionID == oItem.DURequisitionID).ToList();
                        int rowCount = (oTempDURequisitionDetails.Count() - 1);
                        if (rowCount <= 0) rowCount = 0;
                        nStartCol = 2;

                        #region main object
                        nCount++;
                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.RequisitionNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        #endregion

                        #region Detail
                        if (oTempDURequisitionDetails.Count > 0)
                        {
                            foreach (DURequisitionDetail oItemDetail in oTempDURequisitionDetails)
                            {
                                nStartCol = 4;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.DyeingOrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.LotNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.Qty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                
                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ReqDate.ToString("dd MMM yyyy hh:mm tt"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ReceiveDate.ToString("dd MMM yyyy hh:mm tt"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                
                                rowIndex++;
                            }
                        }
                        else
                        {
                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            
                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex++;
                        }
                        #endregion

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, 7]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = oDURequisitionDetails.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[rowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Requisition_Details.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
                

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

        public JsonResult Gets()
        {
            List<DURequisition> oDURequisitions = new List<DURequisition>();
            oDURequisitions = DURequisition.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDURequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult YarnSearchByName(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                string sSQL = "SELECT * FROM Product WHERE ProductID IN (SELECT ProductID FROM DURequisitionDetail)";
                if (!string.IsNullOrEmpty(oProduct.Params))
                {
                    sSQL += " AND ProductName LIKE '%" + oProduct.Params + "%'";
                }
                oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
