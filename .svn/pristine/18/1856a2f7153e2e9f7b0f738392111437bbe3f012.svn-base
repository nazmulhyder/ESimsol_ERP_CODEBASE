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
    public class DUClaimOrderController : Controller
    {
        #region Declaration
        DUClaimOrder _oDUClaimOrder = new DUClaimOrder();
        DUClaimOrderDetail _oDUClaimOrderDetail = new DUClaimOrderDetail();
        List<DUClaimOrder> _oDUClaimOrders = new List<DUClaimOrder>();
        List<DUClaimOrderDetail> _oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
        string _sErrorMessage = "";
        #endregion

        #region Action
        public ActionResult ViewDUClaimOrders(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUClaimOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oDUClaimOrders = new List<DUClaimOrder>();
            _oDUClaimOrders = DUClaimOrder.Gets("Select * from VIEW_DUClaimOrder where isnull(ApproveBy,0)=0", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); 
            ViewBag.BUID = buid;
            ViewBag.DUOrderSetups = DUOrderSetup.GetsActive(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ClaimOrderTypes = EnumObject.jGets(typeof(EnumClaimOrderType)); 
            return View(_oDUClaimOrders);
        }

        public ActionResult ViewDUClaimOrder(int nId, double ts)
        {
            _oDUClaimOrder = new DUClaimOrder();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string sOrderNo = "";
            if (nId > 0)
            {
                _oDUClaimOrder = _oDUClaimOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUClaimOrder.DUClaimOrderID > 0)
                {
                    _oDUClaimOrder.DUClaimOrderDetails = DUClaimOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DUClaimOrderDetail oItem in _oDUClaimOrder.DUClaimOrderDetails)
                    {
                        if (sOrderNo != oItem.OrderNo)
                        {
                            _oDUClaimOrder.ParentDONo = _oDUClaimOrder.ParentDONo = oItem.OrderNo;
                        }
                        sOrderNo = oItem.OrderNo;
                    }
                }
            }
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = DUOrderSetup.GetsActive(0,((User)Session[SessionInfo.CurrentUser]).UserID);
            
            foreach(DUOrderSetup oItem in oDUOrderSetups)
            {
                if(oItem.OrderType==(int)EnumOrderType.ClaimOrder)
                {
                    oDUOrderSetup = oItem;
                }
            }
            List<ClaimReason> oClaimReasons = new List<ClaimReason>();
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oClaimReasons = ClaimReason.Gets((int)EnumClaimOperationType.Export, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.ClaimReasons = oClaimReasons;
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.ClaimOrderTypes = EnumObject.jGets(typeof(EnumClaimOrderType));
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.PaymentTypes = EnumObject.jGets(typeof(EnumOrderPaymentType));  //Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oDUClaimOrder);
        }

        private bool ValidateInput(DUClaimOrder oDUClaimOrder)
        {
            if (oDUClaimOrder.OrderType <= 0)
            {
                _sErrorMessage = "Please pick order";
                return false;
            }
            return true;
        }

        [HttpPost]
        public JsonResult Save(DUClaimOrder oDUClaimOrder)
        {
            try
            {
                if (this.ValidateInput(oDUClaimOrder))
                {
                    _oDUClaimOrder = oDUClaimOrder.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUClaimOrder.DUClaimOrderID > 0)
                    {
                        _oDUClaimOrder.DUClaimOrderDetails = DUClaimOrderDetail.Gets(_oDUClaimOrder.DUClaimOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    _oDUClaimOrder.ErrorMessage = _sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oDUClaimOrder = new DUClaimOrder();
                _oDUClaimOrder.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_Log(DUClaimOrder oDUClaimOrder)
        {
            try
            {
                if (this.ValidateInput(oDUClaimOrder))
                {
                    _oDUClaimOrder = oDUClaimOrder.Save_Log(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUClaimOrder.DUClaimOrderID > 0)
                    {
                        _oDUClaimOrder.DUClaimOrderDetails = DUClaimOrderDetail.Gets(_oDUClaimOrder.DUClaimOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    _oDUClaimOrder.ErrorMessage = _sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oDUClaimOrder = new DUClaimOrder();
                _oDUClaimOrder.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approve(DUClaimOrder oDUClaimOrder)
        {
            string sErrorMease = "";
            _oDUClaimOrder = oDUClaimOrder;
            try
            {
                _oDUClaimOrder = oDUClaimOrder.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Checked(DUClaimOrder oDUClaimOrder)
        {
            string sErrorMease = "";
            _oDUClaimOrder = oDUClaimOrder;
            try
            {
                _oDUClaimOrder = oDUClaimOrder.Checked(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(DUClaimOrder oDUClaimOrder)
        {
            try
            {
                if (oDUClaimOrder.DUClaimOrderID <= 0) { throw new Exception("Please select an valid item."); }
                oDUClaimOrder.ErrorMessage = oDUClaimOrder.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUClaimOrder = new DUClaimOrder();
                oDUClaimOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUClaimOrder.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail(DUClaimOrderDetail oDUClaimOrderDetail)
        {
            try
            {
                if (oDUClaimOrderDetail.DUClaimOrderDetailID > 0)
                {
                    oDUClaimOrderDetail.ErrorMessage = oDUClaimOrderDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDUClaimOrderDetail = new DUClaimOrderDetail();
                oDUClaimOrderDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUClaimOrderDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DOSave_Auto(DUClaimOrder oDUClaimOrder)
        {
            string sErrorMease = "";
            _oDUClaimOrder = oDUClaimOrder;
            try
            {
                _oDUClaimOrder = _oDUClaimOrder.DOSave_Auto(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OrderClose(DUClaimOrder oDUClaimOrder)
        {
            _oDUClaimOrder = new DUClaimOrder();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            oDyeingOrder.DyeingOrderID = oDUClaimOrder.DyeingOrderID;
            oDyeingOrder = oDyeingOrder.OrderClose(((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDyeingOrder.ErrorMessage = sMsg;
            _oDUClaimOrder = _oDUClaimOrder.Get(oDUClaimOrder.DUClaimOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OrderCancel(DUClaimOrder oDUClaimOrder)
        {
            _oDUClaimOrder = new DUClaimOrder();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            oDyeingOrder.DyeingOrderID = oDUClaimOrder.DyeingOrderID;
            oDyeingOrder = oDyeingOrder.DOCancel(((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDyeingOrder.ErrorMessage = sMsg;
            if (oDyeingOrder.DyeingOrderID > 0)
            {
                _oDUClaimOrder = _oDUClaimOrder.Get(oDUClaimOrder.DUClaimOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oDUClaimOrder.ErrorMessage = oDyeingOrder.ErrorMessage;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      

        #endregion

        #region Gets
        [HttpPost]
        public JsonResult GetExportPIs(DUClaimOrder oDUClaimOrder)
        {
            List<ExportSCDO> oExportSCDOs = new List<ExportSCDO>();
            List<DUClaimOrder> oDUClaimOrders = new List<DUClaimOrder>();
            try
            {
                string sSQL = "Select Top(100)* from View_ExportSC_DO ";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oDUClaimOrder.PINo))
                {
                    oDUClaimOrder.PINo = oDUClaimOrder.PINo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PINo Like'%" + oDUClaimOrder.PINo + "%'";
                }

                if (oDUClaimOrder.BUID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "BUID=" + oDUClaimOrder.BUID;
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "BUID in (Select BusinessUnitID from BusinessUnit where BusinessUnit.BusinessUnitType="+(int)EnumBusinessUnitType.Dyeing+")";
                }

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "IsRevisePI=0 and PIStatus in (2,3,4)"; //For Dyeing/Production Order issue->Must Checked valid PI(  Initialized = 0, RequestForApproved = 1,
                //Approved = 2,     PIIssue = 3,   BindWithLC = 4,   RequestForRevise = 5,  Cancel = 6) and Sales Contract is Approved
                sSQL = sSQL + "" + sReturn;
                oExportSCDOs = ExportSCDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportSCDO oItem in oExportSCDOs)
                {
                    oDUClaimOrder = new DUClaimOrder();
                    oDUClaimOrder.DyeingOrderID = oDUClaimOrder.DyeingOrderID;
                    oDUClaimOrder.ExportPIID = oItem.ExportPIID;
                    oDUClaimOrder.ContractorName = oItem.ContractorName;
                    oDUClaimOrder.PINo = oItem.PINo;
                    oDUClaimOrder.LCNo = oItem.ExportLCNo + " " + oItem.ExportLCStatusSt;
                    oDUClaimOrders.Add(oDUClaimOrder);
                }
            }
            catch (Exception ex)
            {
                oDUClaimOrders = new List<DUClaimOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUClaimOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDyeingOrders(DUClaimOrder oDUClaimOrder)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DUClaimOrder> oDUClaimOrders = new List<DUClaimOrder>();
            try
            {
                string sSQL = "Select top(150)* from View_DyeingOrder where [Status]<9";
                string sReturn = " ";
                if (!String.IsNullOrEmpty(oDUClaimOrder.ParentDONo))
                {
                    oDUClaimOrder.ParentDONo = oDUClaimOrder.ParentDONo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderNo Like'%" + oDUClaimOrder.ParentDONo + "%'";
                }
                if (oDUClaimOrder.ExportPIID > 0)
                {
                   
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ExportPIID =" + oDUClaimOrder.ExportPIID;
                }
                if (oDUClaimOrder.ExportPIID > 0 && (oDUClaimOrder.OrderType == (int)EnumOrderType.BulkOrder || oDUClaimOrder.OrderType == (int)EnumOrderType.DyeingOnly))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ExportPIID in (Select ExportPIID from view_ExportSC where ExportPIID=" + oDUClaimOrder.ExportPIID + " and IsRevisePI=0 and PIStatus in (2,3,4))";
                }
                if (oDUClaimOrder.OrderType > 0)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DyeingOrderType =" + oDUClaimOrder.OrderType;
                }

                sSQL = sSQL + "" + sReturn;
                oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                DUClaimOrder oDUClaimOrderTemp = new DUClaimOrder();
                foreach (DyeingOrder oItem in oDyeingOrders)
                {
                    oDUClaimOrderTemp = new DUClaimOrder();
                    oDUClaimOrderTemp.DyeingOrderID = oDUClaimOrder.DyeingOrderID;
                    oDUClaimOrderTemp.ParentDOID = oItem.DyeingOrderID;
                    oDUClaimOrderTemp.OrderType = oItem.DyeingOrderType;
                    oDUClaimOrderTemp.OrderType = oItem.DyeingOrderType;
                    oDUClaimOrderTemp.ContractorName = oItem.ContractorName;
                    oDUClaimOrderTemp.ExportPIID = oItem.ExportPIID;
                    oDUClaimOrderTemp.ParentDONo = oItem.OrderNoFull;
                    oDUClaimOrderTemp.PINo = oItem.ExportPINo;
                    oDUClaimOrderTemp.Qty = oItem.Qty;
                    oDUClaimOrders.Add(oDUClaimOrderTemp);
                }
            }
            catch (Exception ex)
            {
                oDUClaimOrders = new List<DUClaimOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUClaimOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDUReturnChallans(DUClaimOrder oDUClaimOrder)
        {
            List<DUReturnChallan> oDUReturnChallans = new List<DUReturnChallan>();
            List<DUClaimOrder> oDUClaimOrders = new List<DUClaimOrder>();
            try
            {
                string sSQL = "Select top(150)* from View_DUReturnChallan where isnull(ApprovedBy,0)<>0";
                string sReturn = " ";
                if (!String.IsNullOrEmpty(oDUClaimOrder.DUReturnChallanNo))
                {
                    oDUClaimOrder.DUReturnChallanNo = oDUClaimOrder.DUReturnChallanNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DUReturnChallanNo Like'%" + oDUClaimOrder.DUReturnChallanNo + "%'";
                }
                if (oDUClaimOrder.ParentDOID > 0)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DUReturnChallanID in (Select DUReturnChallanID from View_DUReturnChallanDetail as dd where dd.DyeingOrderDetailID  in(Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + oDUClaimOrder.ParentDOID + "))";
                }
               
                if (oDUClaimOrder.OrderType > 0)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderType =" + oDUClaimOrder.OrderType;
                }

                sSQL = sSQL + "" + sReturn + " order by DUReturnChallanID Desc";
                oDUReturnChallans = DUReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                DUClaimOrder oDUClaimOrderTemp = new DUClaimOrder();
                foreach (DUReturnChallan oItem in oDUReturnChallans)
                {
                    oDUClaimOrderTemp = new DUClaimOrder();
                    oDUClaimOrderTemp.DyeingOrderID = oDUClaimOrder.ParentDOID;
                  //  oDUClaimOrderTemp.ParentDOID = oItem.DyeingOrderID;
                    oDUClaimOrderTemp.DUReturnChallanID = oItem.DUReturnChallanID;
                    oDUClaimOrderTemp.OrderType = oItem.OrderType;
                    oDUClaimOrderTemp.DUReturnChallanNo = oItem.DUReturnChallanNo;
                    oDUClaimOrderTemp.ContractorName = oItem.ContractorName;
                    //oDUClaimOrderTemp.ExportPIID = oItem.ExportPIID;
                    oDUClaimOrderTemp.ParentDONo = oItem.OrderNo;
                    //oDUClaimOrderTemp.PINo = oItem.ExportPINo;
                    oDUClaimOrderTemp.Qty = oItem.Qty;
                    oDUClaimOrders.Add(oDUClaimOrderTemp);
                }
            }
            catch (Exception ex)
            {
                oDUClaimOrders = new List<DUClaimOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUClaimOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult GetDyeingOrderDetails(DUClaimOrder oDUClaimOrder)
        {
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            _oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            DUClaimOrderDetail oDUClaimOrderDetail = new DUClaimOrderDetail();
            try
            {
               DyeingOrder oDyeingOrder = new DyeingOrder();
               oDyeingOrder = DyeingOrder.Get(oDUClaimOrder.ParentDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               oDyeingOrderDetails = DyeingOrderDetail.Gets(oDUClaimOrder.ParentDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               foreach (DyeingOrderDetail oItemDOD in oDyeingOrderDetails)
                {
                    oDUClaimOrderDetail = new DUClaimOrderDetail();
                    oDUClaimOrderDetail.OrderNo = oDyeingOrder.OrderNoFull;
                    oDUClaimOrderDetail.ParentDODetailID = oItemDOD.DyeingOrderDetailID;
                    oDUClaimOrderDetail.ProductID = oItemDOD.ProductID;
                    //oDUClaimOrderDetail.ProductCode = oItemDOD.ProductCode;
                    oDUClaimOrderDetail.ProductName = oItemDOD.ProductName;
                    oDUClaimOrderDetail.ColorNo = oItemDOD.ColorNo;
                    oDUClaimOrderDetail.ColorName = oItemDOD.ColorName;
                    oDUClaimOrderDetail.Shade = oItemDOD.Shade;
                    oDUClaimOrderDetail.ColorNo = oItemDOD.ColorNo;
                    oDUClaimOrderDetail.Note = oItemDOD.Note;
                    oDUClaimOrderDetail.OrderQty = oItemDOD.Qty;
                    oDUClaimOrderDetail.Qty = oItemDOD.Qty;
                    _oDUClaimOrderDetails.Add(oDUClaimOrderDetail);
                    
                }
            }
            catch (Exception ex)
            {
                _oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsDUReturnChallanDetails(DUClaimOrder oDUClaimOrder)
        {
            List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
            _oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            DUClaimOrderDetail oDUClaimOrderDetail = new DUClaimOrderDetail();
            try
            {
                string sSQL = "Select * from View_DUReturnChallanDetail";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oDUClaimOrder.ParentDONo))
                {
                    oDUClaimOrder.ParentDONo = oDUClaimOrder.ParentDONo.Trim();
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " OrderNo Like'%" + oDUClaimOrder.ParentDONo + "%'";
                    sReturn = sReturn + "DyeingOrderDetailID in (select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderid in (Select DyeingOrderID from DyeingOrder where DyeingOrder.OrderNo  Like'%" + oDUClaimOrder.ParentDONo + "%'))";
                }
                if (!String.IsNullOrEmpty(oDUClaimOrder.ParentDONo))
                {
                    oDUClaimOrder.ParentDONo = oDUClaimOrder.ParentDONo.Trim();
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " OrderNo Like'%" + oDUClaimOrder.ParentDONo + "%'";
                    sReturn = sReturn + "DyeingOrderDetailID in (select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderid in (Select DyeingOrderID from DyeingOrder where DyeingOrder.OrderNo  Like'%" + oDUClaimOrder.ParentDONo + "%'))";
                }
                if (oDUClaimOrder.ParentDOID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DyeingOrderDetailID in (select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderid in (" + oDUClaimOrder.ParentDOID + "))";
                }
                if (oDUClaimOrder.DUReturnChallanID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DUReturnChallanID=" + oDUClaimOrder.DUReturnChallanID;
                }
                sSQL = sSQL + sReturn + " order by DUReturnChallanDetailID Desc, DyeingOrderDetailID Asc";
                oDUReturnChallanDetails = DUReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



                foreach (DUReturnChallanDetail oItemDOD in oDUReturnChallanDetails)
                {
                    oDUClaimOrderDetail = new DUClaimOrderDetail();
                    oDUClaimOrderDetail.OrderNo = oItemDOD.OrderNo;
                    oDUClaimOrderDetail.ParentDODetailID = oItemDOD.DyeingOrderDetailID;
                    oDUClaimOrderDetail.ParentDOID = oItemDOD.DyeingOrderID;
                    oDUClaimOrderDetail.ProductID = oItemDOD.ProductID;
                    //oDUClaimOrderDetail.ProductCode = oItemDOD.ProductCode;
                    oDUClaimOrderDetail.ProductName = oItemDOD.ProductName;
                    oDUClaimOrderDetail.BatchNo = oItemDOD.LotNo;
                    oDUClaimOrderDetail.LotID = oItemDOD.LotID;
                    oDUClaimOrderDetail.ColorNo = oItemDOD.ColorNo;
                    oDUClaimOrderDetail.ColorName = oItemDOD.ColorName;
                    oDUClaimOrderDetail.Shade = oItemDOD.Shade;
                    oDUClaimOrderDetail.ColorNo = oItemDOD.ColorNo;
                    oDUClaimOrderDetail.Note = oItemDOD.Note;
                    oDUClaimOrderDetail.OrderQty = oItemDOD.Qty;
                    oDUClaimOrderDetail.Qty = oItemDOD.Qty;
                    _oDUClaimOrderDetails.Add(oDUClaimOrderDetail);

                }
            }
            catch (Exception ex)
            {
                _oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search
        [HttpPost]
        public JsonResult AdvSearch(DUClaimOrder oDUClaimOrder)
        {
            _oDUClaimOrders = new List<DUClaimOrder>();
            try
            {
                string sSQL = MakeSQL(oDUClaimOrder);
                _oDUClaimOrders = DUClaimOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUClaimOrders = new List<DUClaimOrder>();
                _oDUClaimOrder.ErrorMessage = ex.Message;
                _oDUClaimOrders.Add(_oDUClaimOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUClaimOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(DUClaimOrder oDUClaimOrder)
        {
            string sParams = oDUClaimOrder.Note;
            string sContractorIDs = "";
            int nCboDate = 0;
            DateTime dFromDate = DateTime.Today;
            DateTime dToDate = DateTime.Today;
            string sClaimNo = "";
            string sPINo = "";
            string sOrderNo = "";
            int ncboOrderType = 0;
            int ncboClaimType = 0;
            int nBUID = 0;



            if (!string.IsNullOrEmpty(sParams))
            {
                string sTemp = "";
                sContractorIDs = Convert.ToString(sParams.Split('~')[0]);
                nCboDate = Convert.ToInt32(sParams.Split('~')[1]);
                dFromDate = Convert.ToDateTime(sParams.Split('~')[2]);
                dToDate = Convert.ToDateTime(sParams.Split('~')[3]);
                sClaimNo = Convert.ToString(sParams.Split('~')[4]);
                sPINo = Convert.ToString(sParams.Split('~')[5]);
                sOrderNo = Convert.ToString(sParams.Split('~')[6]);
                ncboOrderType = Convert.ToInt32(sParams.Split('~')[7]);
                ncboClaimType = Convert.ToInt32(sParams.Split('~')[8]);
             
              
            }

            string sReturn1 = "SELECT * FROM View_DUClaimOrder AS DUCO ";
            string sReturn = "";

            #region Date
            if (nCboDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DUCO.[OrderDate],106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DUCO.[OrderDate],106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DUCO.[OrderDate],106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DUCO.[OrderDate],106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DUCO.[OrderDate],106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DUCO.[OrderDate],106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            //#region Contractor IDs
            //if (!string.IsNullOrEmpty(sContractorIDs))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "DUClaimOrderID in (Select DUCO.DUClaimOrderID from DUClaimOrderDetail as DUCO where ProductID in (" + sProductIDs + "))";
            //}
            //#endregion

            #region DUClaimOrderNo
            if (!string.IsNullOrEmpty(sClaimNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUCO.OrderNo Like  '%" + sClaimNo + "%'";
            }
            #endregion
            #region sOrderNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUCO.ParentDONo Like  '%" + sOrderNo + "%'";
            }
            #endregion
            #region OrderType
            if (ncboOrderType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUCO.OrderType=" + ncboOrderType + "";
            }
            #endregion
            #region cboClaimType
            if (ncboClaimType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUCO.ClaimType=" + ncboClaimType + "";
            }
            #endregion
            #region PINo
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUCO.PINo Like  '%" + sPINo + "%'";
            }
            #endregion

            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DUCO.BUID = " + nBUID;
            }
            #endregion
            string sSQL = sReturn1 + " " + sReturn ;
            return sSQL;
        }
    
        #endregion
        #region GetCompanyLogo
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
        #region Print
        public ActionResult PrintDUClaimOrder(int nId, double nts)
        {
            _oDUClaimOrder = new DUClaimOrder();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            DUReturnChallan oDUReturnChallan = new DUReturnChallan();
            List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
            oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.ClaimOrder, ( (User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            try
            {
                if (nId > 0)
                {
                    _oDUClaimOrder = _oDUClaimOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUClaimOrder.DUClaimOrderID > 0)
                    {
                     _oDUClaimOrder.DUClaimOrderDetails = DUClaimOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    if (_oDUClaimOrder.DUReturnChallanID > 0)
                    {
                        oDUReturnChallan = oDUReturnChallan.Get(_oDUClaimOrder.DUReturnChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oDUReturnChallan.DUReturnChallanDetails = DUReturnChallanDetail.Gets(oDUReturnChallan.DUReturnChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (oDUOrderSetup.PrintNo == (int)EnumExcellColumn.A)
            {
                rptDUClaimOrder oReport = new rptDUClaimOrder();
                byte[] abytes = oReport.PrepareReport(_oDUClaimOrder, oCompany, oBusinessUnit, oDUOrderSetup, oDUReturnChallan);
                return File(abytes, "application/pdf");
            }
            else if (oDUOrderSetup.PrintNo == (int)EnumExcellColumn.B)
            {
                rptDUClaimOrder oReport = new rptDUClaimOrder();
                byte[] abytes = oReport.PrepareReport_B(_oDUClaimOrder, oCompany, oBusinessUnit, oDUOrderSetup);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptDUClaimOrder oReport = new rptDUClaimOrder();
                byte[] abytes = oReport.PrepareReport(_oDUClaimOrder, oCompany, oBusinessUnit, oDUOrderSetup, oDUReturnChallan);
                return File(abytes, "application/pdf");
            }
            

        }

        [HttpPost]
        public ActionResult SetDUClaimOrderListData(DUClaimOrder oDUClaimOrder)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDUClaimOrder);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintDUClaimOrderDetails()
        {
            _oDUClaimOrder = new DUClaimOrder();
            try
            {
                _oDUClaimOrder = (DUClaimOrder)Session[SessionInfo.ParamObj];
                _oDUClaimOrders = DUClaimOrder.Gets("SELECT * FROM View_DUClaimOrder WHERE DUClaimOrderID IN (" + _oDUClaimOrder.ErrorMessage + ") Order By DUClaimOrderID", (int)Session[SessionInfo.currentUserID]);
                _oDUClaimOrderDetails = DUClaimOrderDetail.Gets("SELECT * FROM View_DUClaimOrderDetail WHERE DUClaimOrderID IN (" + _oDUClaimOrder.ErrorMessage + ") Order By DUClaimOrderID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUClaimOrder = new DUClaimOrder();
                _oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oDUClaimOrder.BUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oDUClaimOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

            rptDUClaimOrderDetails oReport = new rptDUClaimOrderDetails();
            byte[] abytes = oReport.PrepareReport(_oDUClaimOrders,_oDUClaimOrderDetails, oCompany);
            return File(abytes, "application/pdf");
        }

        public void ExportToExcel()
        {
            _oDUClaimOrder = new DUClaimOrder();
            try
            {
                _oDUClaimOrder = (DUClaimOrder)Session[SessionInfo.ParamObj];
                _oDUClaimOrders = DUClaimOrder.Gets("SELECT * FROM View_DUClaimOrder WHERE DUClaimOrderID IN (" + _oDUClaimOrder.ErrorMessage + ") Order By DUClaimOrderID", (int)Session[SessionInfo.currentUserID]);
                _oDUClaimOrderDetails = DUClaimOrderDetail.Gets("SELECT * FROM View_DUClaimOrderDetail WHERE DUClaimOrderID IN (" + _oDUClaimOrder.ErrorMessage + ") Order By DUClaimOrderID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUClaimOrder = new DUClaimOrder();
                _oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
            }

            if (_oDUClaimOrders.Count > 0)
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUClaimOrder.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(_oDUClaimOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                int count = 0, nStartCol = 2, nTotalCol =0;

                #region full excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Claim Order Details");
                    sheet.Name = "Claim Order Details";
                    sheet.Column(nStartCol++).Width = 5; //SL
                    sheet.Column(nStartCol++).Width = 14; //challan no
                    sheet.Column(nStartCol++).Width = 12; //Date
                    sheet.Column(nStartCol++).Width = 14; //Type
                    sheet.Column(nStartCol++).Width = 14; //Order No
                    sheet.Column(nStartCol++).Width = 30; //Buyer
                    sheet.Column(nStartCol++).Width = 40; //Product
                    sheet.Column(nStartCol++).Width = 15; //Order qty
                    sheet.Column(nStartCol++).Width = 15; //qty
                    sheet.Column(nStartCol++).Width = 20; //Reason
                    sheet.Column(nStartCol++).Width = 20; //Note

                    nTotalCol = nStartCol;
                    nStartCol = 2;

                    #region Report Header
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true;cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = "Claim Order Details"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Claim No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Claim Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Claim Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Reason"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Note"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    rowIndex = rowIndex + 1;
                    #endregion

                    #region data
                    int nCount = 0;
                    foreach (DUClaimOrder oItem in _oDUClaimOrders)
                    {
                        List<DUClaimOrderDetail> oTempDUClaimOrderDetails = new List<DUClaimOrderDetail>();
                        oTempDUClaimOrderDetails = _oDUClaimOrderDetails.Where(x => x.DUClaimOrderID == oItem.DUClaimOrderID).ToList();
                        int rowCount = (oTempDUClaimOrderDetails.Count() - 1);
                        if (rowCount <= 0) rowCount = 0;
                        nStartCol = 2;

                        #region main object
                        nCount++;
                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ClaimOrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.OrderDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ClaimTypeSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ParentDONo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion

                        #region Detail
                        if (oTempDUClaimOrderDetails.Count > 0)
                        {
                            foreach (DUClaimOrderDetail oItemDetail in oTempDUClaimOrderDetails)
                            {
                                nStartCol = 8;
                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.Qty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.Note; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
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

                            rowIndex++;
                        }
                        #endregion

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, 9]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = _oDUClaimOrderDetails.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11, rowIndex, 12]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Claim_Order_Details.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }
            #endregion
                
        }
    }
#endregion
}