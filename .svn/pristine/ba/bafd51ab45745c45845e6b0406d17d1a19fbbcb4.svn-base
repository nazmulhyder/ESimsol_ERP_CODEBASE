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
using ReportManagement;

namespace ESimSolFinancial.Controllers
{
    public class FabricReturnChallanController : Controller
    {
        #region Declaration
        FabricReturnChallan _oFabricReturnChallan = new FabricReturnChallan();
        List<FabricReturnChallan> _oFabricReturnChallans = new List<FabricReturnChallan>();
        FabricReturnChallanDetail _oFabricReturnChallanDetail = new FabricReturnChallanDetail();
        List<FabricReturnChallanDetail> _oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
        string _sDateRange = "";
        #endregion

        #region Fabric Return Challans
        public ActionResult ViewFabricReturnChallans(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricReturnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
         
            string sSQL = "SELECT * FROM View_FabricReturnChallan FRC WHERE ISNULL(FRC.ReceivedBy,0)=0 "; //AND FRC.FabricDeliveryChallanID IN (SELECT FDCID FROM FabricDeliveryChallan WHERE DisburseBy>0)
            _oFabricReturnChallans = FabricReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.ChallanUnit = 0;
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricReturnChallan, EnumStoreType.ReceiveStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFabricReturnChallans);
        }
     
        #endregion

        #region Fabric Return Challan
        public ActionResult ViewFabricReturnChallan(int nFRCID, int nChallanFrom)
        {
           
            _oFabricReturnChallan = new FabricReturnChallan();
            _oFabricReturnChallan.FRCDetails = new List<FabricReturnChallanDetail>();

            if (nFRCID > 0)
            {
                _oFabricReturnChallan = _oFabricReturnChallan.Get(nFRCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "SELECT * FROM View_FabricReturnChallanDetail WHERE FabricReturnChallanID =" + _oFabricReturnChallan.FabricReturnChallanID;
                _oFabricReturnChallan.FRCDetails = FabricReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            //else if (nChallanFrom > 0)
            //{
            //    oFabricDeliveryChallan = FabricDeliveryChallan.Get(nChallanFrom, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //    _oFabricReturnChallan.ChallanNo = oFabricDeliveryChallan.ChallanNo;
            //    _oFabricReturnChallan.BuyerID = oFabricDeliveryChallan.ContractorID;
            //    _oFabricReturnChallan.BuyerName = oFabricDeliveryChallan.BuyerName;
            //    _oFabricReturnChallan.WorkingUnitID = oFabricDeliveryChallan.WorkingUnitID;
            //    _oFabricReturnChallan.ChallanDate = oFabricDeliveryChallan.IssueDate;

            //    string sSQL = "SELECT *, MUID AS MUnitID FROM View_FabricReturnChallanDetail WHERE FabricDeliveryChallanID =" + _oFabricReturnChallan.FabricReturnChallanID;
            //    _oFabricReturnChallan.FRCDetails = FabricReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}
            _oFabricReturnChallan.IssuedByName =  ((User)Session[SessionInfo.CurrentUser]).UserName;

            ViewBag.ChallanUnit = nChallanFrom;
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricReturnChallan, EnumStoreType.ReceiveStore,((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFabricReturnChallan);
        }
        [HttpPost]
        public JsonResult GetsChllanByBuyer(FabricDeliveryChallan oFabricDeliveryChallan)
        {

            List<FabricDeliveryChallan> oFabricDeliveryChallans = new List<FabricDeliveryChallan>();
            try
            {
                int nBuyerID = oFabricDeliveryChallan.ContractorID;
                string sChalanNo = oFabricDeliveryChallan.ChallanNo;
                string sSQL = string.Empty;

                sSQL = "SELECT TOP 500 * FROM View_FabricDeliveryChallan WHERE  DisburseBy>0 ";
              
                if (nBuyerID != 0)
                {
                    sSQL += " AND ContractorID =" + nBuyerID;
                }
                if (!string.IsNullOrEmpty(sChalanNo))
                {

                    sSQL += " AND ChallanNo LIKE '%" + sChalanNo + "%'";
                }
                sSQL +=" ORDER BY FDCID DESC";
                oFabricDeliveryChallans = FabricDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricDeliveryChallans = new List<FabricDeliveryChallan>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsChllans(FabricDeliveryChallan oFabricDeliveryChallan)
        {

            List<FabricDeliveryChallan> oFabricDeliveryChallans = new List<FabricDeliveryChallan>();
            try
            {   
                int nDeliveryFrom = Convert.ToInt32(oFabricDeliveryChallan.Params);
                string sSQL = "SELECT * FROM View_FabricDeliveryChallan WHERE  DisburseBy>0 AND FDOID IN (SELECT FDOID FROM FabricDeliveryOrder WHERE DeliveryFrom = " + nDeliveryFrom + ")";
                sSQL += " AND ChallanNo LIKE '%" + oFabricDeliveryChallan.ChallanNo + "%'";
                oFabricDeliveryChallans = FabricDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFabricDeliveryChallans = new List<FabricDeliveryChallan>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsChllanDetailByChallan(FabricDeliveryChallanDetail oFabricDeliveryChallan)
        {
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            List<FabricReturnChallanDetail> oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
            List<FabricReturnChallanDetail> oFabricReturnChallanDetailsTemp = new List<FabricReturnChallanDetail>();

            FabricReturnChallanDetail oFabricReturnChallanDetail = new FabricReturnChallanDetail();
            string sIDs = "";
            try
            {
                string sSQL = "SELECT TOP 250 * FROM View_FabricDeliveryChallanDetail WHERE FDCID > 0 ";

                if (!string.IsNullOrEmpty(oFabricDeliveryChallan.Params))
                    sSQL += " AND FDCDID NOT IN (" + oFabricDeliveryChallan.Params + ")";

                if (oFabricDeliveryChallan.ContractorID > 0)
                    sSQL += " AND ContractorID IN ( " + oFabricDeliveryChallan.ContractorID + ")";

                if (!string.IsNullOrEmpty(oFabricDeliveryChallan.FEONo))
                    sSQL += " AND FEONo LIKE '%" + oFabricDeliveryChallan.FEONo + "%'";

                if (!string.IsNullOrEmpty(oFabricDeliveryChallan.ChallanNo))
                    sSQL += " AND ChallanNo LIKE '%" + oFabricDeliveryChallan.ChallanNo + "%'";

                if (!string.IsNullOrEmpty(oFabricDeliveryChallan.ExeNo))
                    sSQL += " AND ExeNo LIKE '%" + oFabricDeliveryChallan.ExeNo + "%'";

                oFabricDeliveryChallanDetails = FabricDeliveryChallanDetail.Gets(sSQL + " ORDER BY DBServerDateTime DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);

                sIDs = string.Join(",", oFabricDeliveryChallanDetails.Select(x => x.FDCDID).Distinct().ToList());

                oFabricReturnChallanDetailsTemp = FabricReturnChallanDetail.Gets("Select * from View_FabricReturnChallanDetail where FDCDID in (" + sIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

               oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
                foreach(FabricDeliveryChallanDetail oItem in oFabricDeliveryChallanDetails)
                {
                    oFabricReturnChallanDetail = new FabricReturnChallanDetail();
                    oFabricReturnChallanDetail.FabricNo = oItem.FabricNo;
                    oFabricReturnChallanDetail.FDCDID = oItem.FDCDID;
                    oFabricReturnChallanDetail.ExeNo = oItem.ExeNo;
                    oFabricReturnChallanDetail.ProductID = oItem.ProductID;
                    oFabricReturnChallanDetail.ExeNo = oItem.ExeNo;
                    oFabricReturnChallanDetail.FabricReturnChallanDetailID = 0;
                    oFabricReturnChallanDetail.LotID = oItem.LotID;
                    oFabricReturnChallanDetail.LotNo = oItem.LotNo;
                    oFabricReturnChallanDetail.MUName = oItem.MUName;
                    oFabricReturnChallanDetail.MUnitID = oItem.MUID;

                    if (oFabricReturnChallanDetailsTemp.FirstOrDefault() != null && oFabricReturnChallanDetailsTemp.FirstOrDefault().FDCDID > 0 && oFabricReturnChallanDetailsTemp.Where(b => (b.FDCDID == oItem.FDCDID)).Count() > 0)
                    {
                        oFabricReturnChallanDetail.Qty_Return_Prv = oFabricReturnChallanDetailsTemp.Where(p => (p.FDCDID == oItem.FDCDID) && p.FDCDID > 0).Sum(x => x.Qty); ;
                    }
                    oFabricReturnChallanDetail.Qty = oItem.Qty - oFabricReturnChallanDetail.Qty_Return_Prv;
                    oFabricReturnChallanDetail.ChallanQty = oItem.Qty;
                    oFabricReturnChallanDetail.ChallanNo = oItem.ChallanNo;
                    oFabricReturnChallanDetail.Construction = oItem.Construction;
                    oFabricReturnChallanDetail.FNBatchQCDetailID = oItem.FNBatchQCDetailID;
                    oFabricReturnChallanDetail.ProductName = oItem.ProductName;
                    if (oFabricReturnChallanDetail.Qty > 0)
                    {
                        oFabricReturnChallanDetails.Add(oFabricReturnChallanDetail);
                    }
                }


            }
            catch (Exception ex)
            {
                oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricReturnChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveFRC(FabricReturnChallan oFabricReturnChallan)
        {
            try
            {
                oFabricReturnChallan = oFabricReturnChallan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (oFabricReturnChallan.FabricReturnChallanID > 0)
                //{
                //    string sSQL = "SELECT * FROM View_FabricReturnChallanDetail WHERE FabricReturnChallanID=" + oFabricReturnChallan.FabricReturnChallanID;
                //    oFabricReturnChallan.FRCDetails = FabricReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
            }
            catch (Exception ex)
            {
                oFabricReturnChallan = new FabricReturnChallan();
                oFabricReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFRC(FabricReturnChallan oFRC)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFRC.Delete(oFRC.FabricReturnChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveFRCD(FabricReturnChallanDetail oFRCD)
        {
            try
            {
                oFRCD = oFRCD.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFRCD = new FabricReturnChallanDetail();
                oFRCD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFRCD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFRCD(FabricReturnChallanDetail oFRCD)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFRCD.Delete(oFRCD.FabricReturnChallanDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ApproveFRC(FabricReturnChallan oFabricReturnChallan)
        {
            try
            {
                if (oFabricReturnChallan.FabricReturnChallanID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricReturnChallan = oFabricReturnChallan.ApproveOrReceive((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricReturnChallan = new FabricReturnChallan();
                oFabricReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApproveFRC(FabricReturnChallan oFabricReturnChallan)
        {
            try
            {
                if (oFabricReturnChallan.FabricReturnChallanID <= 0) { throw new Exception("Please select an valid item."); }
                oFabricReturnChallan = oFabricReturnChallan.ApproveOrReceive((int)EnumDBOperation.UnApproval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricReturnChallan = new FabricReturnChallan();
                oFabricReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReceiveFRC(FabricReturnChallan oFabricReturnChallan)
        {
            try
            {
                if (oFabricReturnChallan.FabricReturnChallanID <= 0) { throw new Exception("Please select an valid item."); }

                oFabricReturnChallan = oFabricReturnChallan.ApproveOrReceive((int)EnumDBOperation.Receive, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricReturnChallan = new FabricReturnChallan();
                oFabricReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ContractorSearchByNameType(Contractor oContractor) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            List<Contractor> oContractors = new List<Contractor>();
            try
            {
                string sType = oContractor.Params.Split('~')[0];
                string sName = oContractor.Params.Split('~')[1].Trim();

                string sSQL = "SELECT * FROM Contractor WHERE Activity = 1 ";

                if (sName.Trim() != "")
                {
                    sSQL = sSQL + " AND [Name] LIKE ('%" + sName.Trim() + "%')";
                }

                sSQL = sSQL + " AND ContractorID IN (SELECT DISTINCT ContractorID FROM View_FabricDeliveryChallan)";
                
                sSQL = sSQL + " ORDER BY [Name]";
                
                oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                
                if (oContractors.Count <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                oContractor = new Contractor();
                oContractor.ErrorMessage = ex.Message;
                oContractors.Add(oContractor);
            }
            var jsonResult = Json(oContractors, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Adv Search 
        [HttpPost]
        public JsonResult AdvSearch(FabricReturnChallan oFabricReturnChallan)
        {
            _oFabricReturnChallans = new List<FabricReturnChallan>();
            try
            {
                string sSQL = MakeSQL(oFabricReturnChallan);
                _oFabricReturnChallans = FabricReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricReturnChallans = new List<FabricReturnChallan>();
                oFabricReturnChallan.ErrorMessage = ex.Message;
                _oFabricReturnChallans.Add(oFabricReturnChallan);
            }
            var jsonResult = Json(_oFabricReturnChallans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GridSearch(FabricReturnChallan oFabricReturnChallan)
        {
            _oFabricReturnChallans = new List<FabricReturnChallan>();
            try
            {
                string sSQL = MakeSQLGridSearch(oFabricReturnChallan.Params);
                _oFabricReturnChallans = FabricReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricReturnChallans = new List<FabricReturnChallan>();
                oFabricReturnChallan.ErrorMessage = ex.Message;
                _oFabricReturnChallans.Add(oFabricReturnChallan);
            }
            var jsonResult = Json(_oFabricReturnChallans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FabricReturnChallan oFabricReturnChallan)
        {
            string sParams = oFabricReturnChallan.ErrorMessage;
           
            int ncboReturnDate = 0;
            DateTime dFromReturnDate = DateTime.Today;
            DateTime dToReturnDate = DateTime.Today;

            int ncboChallanDate = 0;
            DateTime dFromChallanDate = DateTime.Today;
            DateTime dToChallanDate = DateTime.Today;

            string _sBuyerIDs = "";
            string _sFDCIDs = "";
            int nStoreID = 0;
            bool isApprove = false, isReceived = false;

           
            if (!string.IsNullOrEmpty(sParams))
            {
                ncboReturnDate = Convert.ToInt32(sParams.Split('~')[0]);
                if (ncboReturnDate > 0)
                {
                    dFromReturnDate = Convert.ToDateTime(sParams.Split('~')[1]);
                    dToReturnDate = Convert.ToDateTime(sParams.Split('~')[2]);
                }

                ncboChallanDate = Convert.ToInt32(sParams.Split('~')[3]);
                if (ncboChallanDate > 0)
                {
                    dFromChallanDate = Convert.ToDateTime(sParams.Split('~')[4]);
                    dToChallanDate = Convert.ToDateTime(sParams.Split('~')[5]);
                }
                _sBuyerIDs = sParams.Split('~')[6];
                //_sFDCIDs = sParams.Split('~')[7];
                //nStoreID = Convert.ToInt32(sParams.Split('~')[8]);
                isApprove = Convert.ToBoolean(sParams.Split('~')[7]);
                isReceived = Convert.ToBoolean(sParams.Split('~')[8]);

            }


            string sReturn1 = "";
            string sReturn = "";
            sReturn1 = "SELECT * FROM View_FabricReturnChallan";

            #region Contractor
            if (!String.IsNullOrEmpty(_sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + _sBuyerIDs + ")";
            }
            #endregion

            #region Challan
            if (!String.IsNullOrEmpty(_sFDCIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricDeliveryChallanID in(" + _sFDCIDs + ")";
            }
            #endregion

            #region Store 
            if (nStoreID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "StoreID in(" + nStoreID + ")";
            }
            #endregion

            #region Approve
            if (isApprove)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(ApprovedBy,0) = 0";
            }
            #endregion

            #region Approve
            if (isReceived)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(ReceivedBy,0) = 0";
            }
            #endregion
         
            #region Issue Date
            if (ncboReturnDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboReturnDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dFromReturnDate.ToString("dd MMM yyyy");
                }
                else if (ncboReturnDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromReturnDate.ToString("dd MMM yyyy");
                }
                else if (ncboReturnDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromReturnDate.ToString("dd MMM yyyy");
                }
                else if (ncboReturnDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromReturnDate.ToString("dd MMM yyyy");
                }
                else if (ncboReturnDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dFromReturnDate.ToString("dd MMM yyyy") + " To " + dToReturnDate.ToString("dd MMM yyyy");
                }
                else if (ncboReturnDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReturnDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReturnDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromReturnDate.ToString("dd MMM yyyy") + " To " + dToReturnDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region ncboChallanDate
            if (ncboChallanDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (ncboChallanDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " FabricReturnChallanID IN (SELECT FabricReturnChallanID FROM View_FabricReturnChallanDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106)) )";
                    _sDateRange = "Date: " + dFromChallanDate.ToString("dd MMM yyyy");
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " FabricReturnChallanID IN (SELECT FabricReturnChallanID FROM View_FabricReturnChallanDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106)) )";
                    _sDateRange = "Date: NotEqualTo->" + dFromChallanDate.ToString("dd MMM yyyy");
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " FabricReturnChallanID IN (SELECT FabricReturnChallanID FROM View_FabricReturnChallanDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106)) )";
                    _sDateRange = "Date: GreaterThen->" + dFromChallanDate.ToString("dd MMM yyyy");
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " FabricReturnChallanID IN (SELECT FabricReturnChallanID FROM View_FabricReturnChallanDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106)) )";
                    _sDateRange = "Date: SmallerThen->" + dFromChallanDate.ToString("dd MMM yyyy");
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " FabricReturnChallanID IN (SELECT FabricReturnChallanID FROM View_FabricReturnChallanDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToChallanDate.ToString("dd MMM yyyy") + "',106)) )";
                    _sDateRange = "Date: From " + dFromChallanDate.ToString("dd MMM yyyy") + " To " + dToChallanDate.ToString("dd MMM yyyy");
                }
                else if (ncboChallanDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " FabricReturnChallanID IN (SELECT FabricReturnChallanID FROM View_FabricReturnChallanDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToChallanDate.ToString("dd MMM yyyy") + "',106)) )";
                    _sDateRange = "Date: NotBetween " + dFromChallanDate.ToString("dd MMM yyyy") + " To " + dToChallanDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            

            string sSQL = sReturn1 + " " + sReturn ;
            return sSQL;
        }
        private string MakeSQLGridSearch(String sParams)
        {
            string _sParams = sParams;
            string _sOrderNo = "";
            string _sDispoNo = "";
            string _sReturnChallanNo = "";
            string _sChallanNo = "";
            if (!string.IsNullOrEmpty(sParams))
            {
                _sOrderNo = Convert.ToString(sParams.Split('~')[0]);
                _sReturnChallanNo = Convert.ToString(sParams.Split('~')[1]);
                _sDispoNo = Convert.ToString(sParams.Split('~')[2]);      
                _sChallanNo = Convert.ToString(sParams.Split('~')[3]);
            }
            string sReturn1 = "";
            string sReturn = "";
            sReturn1 = "SELECT * FROM View_FabricReturnChallan";

            #region ChallanNo
            if (!String.IsNullOrEmpty(_sReturnChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ReturnNo LIKE '%" + _sReturnChallanNo + "%'";
            }
            #endregion

            #region ChallanNo
            if (!String.IsNullOrEmpty(_sChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanNo LIKE '%" + _sChallanNo + "%'";
            }
            #endregion

            #region DispoNo
            if (!String.IsNullOrEmpty(_sDispoNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricReturnChallanID IN(SELECT FabricReturnChallanID FROM View_FabricReturnChallanDetail WHERE ExeNo LIKE '%" + _sDispoNo + "%)'";
            }
            #endregion
        

            string sSQL = sReturn1 + " " + sReturn;
            return sSQL;
        }
        #endregion

        #region Print
        public ActionResult PrintPreviewFabricReturnChallan(string sParams)
        {
            _oFabricReturnChallans = new List<FabricReturnChallan>();
            _oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
            
            if (!string.IsNullOrEmpty(sParams))
            {
                string sSQL = "SELECT * FROM View_FabricReturnChallan  WHERE FabricReturnChallanID IN (" + sParams + ")";
                _oFabricReturnChallans = FabricReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT * FROM View_FabricReturnChallanDetail WHERE FabricReturnChallanID IN (" + sParams + ")";
                _oFabricReturnChallanDetails = FabricReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFabricReturnChallan oReport = new rptFabricReturnChallan();
            byte[] abytes = oReport.PrepareReport(_oFabricReturnChallans, _oFabricReturnChallanDetails,oCompany);
            return File(abytes, "application/pdf");
        }
        [HttpPost]
        public ActionResult PrintListFRC(FormCollection DataCollection)
        {
            _oFabricReturnChallans = new List<FabricReturnChallan>();
            _oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
            string sParams = DataCollection["FRCIDs"];
            if (!string.IsNullOrEmpty(sParams))
            {
                string sSQL = "SELECT * FROM View_FabricReturnChallan  WHERE FabricReturnChallanID IN (" + sParams + ")";
                _oFabricReturnChallans = FabricReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT * FROM View_FabricReturnChallanDetail WHERE FabricReturnChallanID IN (" + sParams + ")";
                _oFabricReturnChallanDetails = FabricReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
            rptFabricReturnChallans oReport = new rptFabricReturnChallans();
            byte[] abytes = oReport.PrepareReport(_oFabricReturnChallans, _oFabricReturnChallanDetails, oCompany);
            return File(abytes, "application/pdf");
        }
        #endregion
    }
}