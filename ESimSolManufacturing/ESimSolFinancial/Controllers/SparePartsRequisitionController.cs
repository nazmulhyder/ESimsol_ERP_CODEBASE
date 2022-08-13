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

    public class SparePartsRequisitionController : Controller
    {

        #region Declartion
        SparePartsRequisition _oSparePartsRequisition = new SparePartsRequisition();
        List<SparePartsRequisition> _oSparePartsRequisitions = new List<SparePartsRequisition>();
        SparePartsRequisitionDetail _oSparePartsRequisitionDetail = new SparePartsRequisitionDetail();
        List<SparePartsRequisitionDetail> _oSparePartsRequisitionDetails = new List<SparePartsRequisitionDetail>();
        #endregion

        #region Collection Page
        public ActionResult ViewSparePartsRequisitions(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.SparePartsRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            _oSparePartsRequisitions = new List<SparePartsRequisition>();
            string sSQL = "SELECT * FROM View_SparePartsRequisition AS HH WHERE  ISNULL(HH.ApprovedBy,0)=0 ";
            if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " AND HH.BUID = " + buid;
            }
            sSQL += " ORDER BY SparePartsRequisitionID ASC";
            //_oSparePartsRequisitions = SparePartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oSparePartsRequisitions = SparePartsRequisition.Gets((int)Session[SessionInfo.currentUserID]);


            #region Requisition User
            if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.RequisitionBy FROM SparePartsRequisition AS MM WHERE MM.BUID =" + buid.ToString() + " AND ISNULL(MM.RequisitionBy,0)!=0) ORDER BY HH.UserName";
            }
            else
            {
                sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.RequisitionBy FROM SparePartsRequisition AS MM WHERE ISNULL(MM.RequisitionBy,0)!=0) ORDER BY HH.UserName";
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

            return View(_oSparePartsRequisitions);
        }
        #endregion

        #region Add, Edit, Delete
        public ActionResult ViewSparePartsRequisition(int id, int buid)
        {
            _oSparePartsRequisition = new SparePartsRequisition();
            //ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            if (id > 0)
            {
                _oSparePartsRequisition = _oSparePartsRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oSparePartsRequisition.SparePartsRequisitionDetails = SparePartsRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oSparePartsRequisition.RequisitionBy = (int)Session[SessionInfo.currentUserID];
                _oSparePartsRequisition.RequisitionByName = (string)Session[SessionInfo.currentUserName];
            }

            return View(_oSparePartsRequisition);
        }

        public ActionResult ViewSparePartsRequisitionRevise(int id, int buid)
        {
            _oSparePartsRequisition = new SparePartsRequisition();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            if (id > 0)
            {
                _oSparePartsRequisition = _oSparePartsRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oSparePartsRequisition.SparePartsRequisitionDetails = SparePartsRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oSparePartsRequisition.RequisitionBy = (int)Session[SessionInfo.currentUserID];
                _oSparePartsRequisition.RequisitionByName = (string)Session[SessionInfo.currentUserName];
            }

            #region Gets Stores
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.SparePartsRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Stores = oWorkingUnits;
            return View(_oSparePartsRequisition);
        }

        public ActionResult ViewSparePartsRequisitionDisburse(int id, int buid)
        {
            _oSparePartsRequisition = new SparePartsRequisition();
            if (id > 0)
            {
                _oSparePartsRequisition = _oSparePartsRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oSparePartsRequisition.SparePartsRequisitionDetails = SparePartsRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oSparePartsRequisition.RequisitionBy = (int)Session[SessionInfo.currentUserID];
                _oSparePartsRequisition.RequisitionByName = (string)Session[SessionInfo.currentUserName];
            }

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.SparePartsRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.Stores = oWorkingUnits;
            return View(_oSparePartsRequisition);
        }

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(SparePartsRequisition oSparePartsRequisition)
        {

            _oSparePartsRequisition = new SparePartsRequisition();
            _oSparePartsRequisitionDetails = new List<SparePartsRequisitionDetail>();
            try
            {
                _oSparePartsRequisition = oSparePartsRequisition;
                _oSparePartsRequisition = _oSparePartsRequisition.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSparePartsRequisition = new SparePartsRequisition();
                _oSparePartsRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSparePartsRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP Save
        //[HttpPost]
        //public JsonResult Delivery(SparePartsRequisition oSparePartsRequisition)
        //{

        //    _oSparePartsRequisition = new SparePartsRequisition();
        //    _oSparePartsRequisitionDetails = new List<SparePartsRequisitionDetail>();
        //    try
        //    {
        //        _oSparePartsRequisition = oSparePartsRequisition;
        //        _oSparePartsRequisition = _oSparePartsRequisition.Delivery((int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oSparePartsRequisition = new SparePartsRequisition();
        //        _oSparePartsRequisition.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oSparePartsRequisition);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        [HttpPost]
        public JsonResult GetsByNameCRAndBUID(CRWiseSpareParts oCRWiseSpareParts)
        {
            List<CRWiseSpareParts> _oCRWiseSparePartss = new List<CRWiseSpareParts>();
            try
            {
                _oCRWiseSparePartss = CRWiseSpareParts.GetsByNameCRAndBUIDWithLot(oCRWiseSpareParts.ProductName, oCRWiseSpareParts.CRID, oCRWiseSpareParts.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCRWiseSparePartss = new List<CRWiseSpareParts>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCRWiseSparePartss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int nSparePartsRequisitionID)
        {
            string smessage = "";
            try
            {
                SparePartsRequisition oSparePartsRequisition = new SparePartsRequisition();
                smessage = oSparePartsRequisition.Delete(nSparePartsRequisitionID, (int)Session[SessionInfo.currentUserID]);

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
        public JsonResult AcceptCRRevise(SparePartsRequisition oSparePartsRequisition)
        {
            _oSparePartsRequisition = new SparePartsRequisition();
            List<SparePartsRequisitionDetail> oSparePartsRequisitionDetails = new List<SparePartsRequisitionDetail>();
            try
            {
                _oSparePartsRequisition = oSparePartsRequisition;
                _oSparePartsRequisition = _oSparePartsRequisition.AcceptSparePartsRequisitionRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSparePartsRequisition = new SparePartsRequisition();
                _oSparePartsRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSparePartsRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion



        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(SparePartsRequisition oSparePartsRequisition)
        {
            _oSparePartsRequisition = new SparePartsRequisition();
            _oSparePartsRequisition = oSparePartsRequisition;
            try
            {
                if (oSparePartsRequisition.ActionTypeExtra == "RequestForApproval")
                {
                    _oSparePartsRequisition.SPStatus = EnumSPStatus.RequestForApproval;
                    _oSparePartsRequisition.SPStatusInt = (int)EnumSPStatus.RequestForApproval;
                }
                else if (oSparePartsRequisition.ActionTypeExtra == "Approve")
                {
                    _oSparePartsRequisition.SPStatus = EnumSPStatus.Approve;
                    _oSparePartsRequisition.SPStatusInt = (int)EnumSPStatus.Approve;
                }
                else if (oSparePartsRequisition.ActionTypeExtra == "SendToStore")
                {
                    _oSparePartsRequisition.SPStatus = EnumSPStatus.InStore;
                    _oSparePartsRequisition.SPStatusInt = (int)EnumSPStatus.InStore;
                }
                else if (oSparePartsRequisition.ActionTypeExtra == "PartialDisburse")
                {
                    _oSparePartsRequisition.SPStatus = EnumSPStatus.PartialDisverse;
                    _oSparePartsRequisition.SPStatusInt = (int)EnumSPStatus.PartialDisverse;
                }
                else if (oSparePartsRequisition.ActionTypeExtra == "Disburse")
                {
                    _oSparePartsRequisition.SPStatus = EnumSPStatus.Disburse;
                    _oSparePartsRequisition.SPStatusInt = (int)EnumSPStatus.Disburse;
                }
                _oSparePartsRequisition = oSparePartsRequisition.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSparePartsRequisition = new SparePartsRequisition();
                _oSparePartsRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSparePartsRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Advance Search

        #region Http Get For Search
        [HttpPost]
        public JsonResult AdvanceSearch(SparePartsRequisition oSparePartsRequisition)
        {
            List<SparePartsRequisition> oSparePartsRequisitions = new List<SparePartsRequisition>();
            try
            {
                string sSQL = GetSQL(oSparePartsRequisition.Remarks, oSparePartsRequisition.BUID);
                oSparePartsRequisitions = SparePartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSparePartsRequisition = new SparePartsRequisition();
                _oSparePartsRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSparePartsRequisitions);
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
            string sCapitalResources = Convert.ToString(sSearchingData.Split('~')[8]);
            string sProductIDs = Convert.ToString(sSearchingData.Split('~')[9]);
            string sStyleIDs = Convert.ToString(sSearchingData.Split('~')[10]);
            string sRefObjIDs = Convert.ToString(sSearchingData.Split('~')[11]);

            string sReturn1 = "SELECT * FROM View_SparePartsRequisition";
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

            #region CapitalResources
            if (sCapitalResources != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RequisitionFor IN (" + sCapitalResources + ") AND CRType=1";
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
                sReturn = sReturn + " SparePartsRequisitionID IN (SELECT TT.SparePartsRequisitionID FROM SparePartsRequisitionDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            #region RefObjs
            if (sRefObjIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefObjID IN (" + sRefObjIDs + ")";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        //[HttpPost]
        //public JsonResult GetsByRefNo(SparePartsRequisition oSparePartsRequisition)
        //{
        //    List<SparePartsRequisition> oSparePartsRequisitions = new List<SparePartsRequisition>();
        //    ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
        //    oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
        //    try
        //    {
        //        string sSQL = "SELECT * FROM View_SparePartsRequisition AS HH WHERE  (ISNULL(HH.RefNo,'')+ISNULL(HH.RequisitionForName,'')+ISNULL(HH.RequisitionNo,'')) LIKE '%" + oSparePartsRequisition.RefNo + "%' ";
        //        #region BUID
        //        if (oSparePartsRequisition.BUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
        //        {
        //            sSQL = sSQL + " BUID = " + oSparePartsRequisition.BUID.ToString();
        //        }
        //        sSQL = sSQL + " ORDER BY SparePartsRequisitionID ASC";
        //        #endregion
        //        oSparePartsRequisitions = SparePartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        oSparePartsRequisitions = new List<SparePartsRequisition>();
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oSparePartsRequisitions);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult WaitingSearch(SparePartsRequisition oSparePartsRequisition)
        {
            List<SparePartsRequisition> oSparePartsRequisitions = new List<SparePartsRequisition>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_SparePartsRequisition AS HH WHERE ISNULL(HH.SPStatus,0) = 1 ";
                #region BUID
                if (oSparePartsRequisition.BUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL = sSQL + " BUID = " + oSparePartsRequisition.BUID.ToString();
                }
                sSQL = sSQL + " ORDER BY SparePartsRequisitionID ASC";
                #endregion
                oSparePartsRequisitions = SparePartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oSparePartsRequisitions = new List<SparePartsRequisition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSparePartsRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReviseHistory(SparePartsRequisition oSparePartsRequisition)
        {
            List<SparePartsRequisition> oSparePartsRequisitions = new List<SparePartsRequisition>();
            try
            {
                string sSQL = "SELECT * FROM View_SparePartsRequisitionLog AS HH WHERE HH.SparePartsRequisitionID=" + oSparePartsRequisition.SparePartsRequisitionID.ToString() + " ORDER BY SparePartsRequisitionLogID";
                oSparePartsRequisitions = SparePartsRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oSparePartsRequisitions = new List<SparePartsRequisition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSparePartsRequisitions);
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


        #region Gets Product & Lots
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

        #region Reports
        public ActionResult PrintSparePartsPreview(int id, int buid)
        {
            int nCompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            SparePartsRequisition oSparePartsRequisition = new SparePartsRequisition();
            oSparePartsRequisition = oSparePartsRequisition.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oSparePartsRequisition.SparePartsRequisitionDetails = SparePartsRequisitionDetail.Gets("SELECT * FROM View_SparePartsRequisitionDetail Where SparePartsRequisitionID =" + id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptSparePartsRequsition oReport = new rptSparePartsRequsition();
            byte[] abytes = oReport.PrepareReport(oSparePartsRequisition, oCompany, oBusinessUnit);
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
        #endregion
    }
}