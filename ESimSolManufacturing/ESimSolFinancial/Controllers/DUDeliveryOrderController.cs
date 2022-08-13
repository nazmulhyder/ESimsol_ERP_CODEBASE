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
    public class DUDeliveryOrderController : Controller
    {
        #region Declaration
        DUDeliveryOrder _oDUDeliveryOrder = new DUDeliveryOrder();
        DUDeliveryOrderDetail _oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
        List<DUDeliveryOrder> _oDUDeliveryOrders = new List<DUDeliveryOrder>();
        List<DUDeliveryOrderDetail> _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
        string _sErrorMessage = "";
        #endregion

        #region DUDeliveryOrder
        public ActionResult ViewDUDeliveryOrders(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUDeliveryOrders = new List<DUDeliveryOrder>();
            _oDUDeliveryOrders = DUDeliveryOrder.Gets("Select * from View_DUDeliveryOrder where  isnull(ApproveBy,0)=0", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
          

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.BUID = buid;
            return View(_oDUDeliveryOrders);
        }

        public ActionResult ViewDUDeliveryOrder(int nId, int buid, double ts)
        {
            _oDUDeliveryOrder = new DUDeliveryOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            ExportSCDO oExportSCDO = new ExportSCDO();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            if (nId > 0)
            {
                _oDUDeliveryOrder = DUDeliveryOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUDeliveryOrder.DUDeliveryOrderID > 0)
                {
                    _oDUDeliveryOrder.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDUDeliveryOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly)
                {
                    oExportSCDO = oExportSCDO.Get(_oDUDeliveryOrder.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDUDeliveryOrder.ContractorName = oExportSCDO.ContractorName;
                    _oDUDeliveryOrder.ExportPINo = oExportSCDO.PINo_Full;
                    _oDUDeliveryOrder.ExportLCNo = oExportSCDO.ELCNo;
                    _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                    _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                    _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;
                }
                if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.SampleOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.SaleOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.Sampling)
                {

                    oDyeingOrder = DyeingOrder.Get(_oDUDeliveryOrder.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDUDeliveryOrder.ContractorName = oDyeingOrder.ContractorName;
                    _oDUDeliveryOrder.OrderNo = oDyeingOrder.OrderNoFull;
                    _oDUDeliveryOrder.ExportPINo = oDyeingOrder.ExportPINo;
                    _oDUDeliveryOrder.ExportLCNo = oDyeingOrder.ExportLCNo;
                    _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                    _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                    _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;
                    _oDUDeliveryOrder.PaymentType = oDyeingOrder.PaymentType;
                }
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            List<EnumObject> oOrderTypeObjs = new List<EnumObject>();
            List<EnumObject> oOrderTypes = new List<EnumObject>();
            oOrderTypeObjs = EnumObject.jGets(typeof(EnumOrderType));

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.BUID = buid;
            return View(_oDUDeliveryOrder);
        }
        public ActionResult ViewDUDeliveryOrdersTwo(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUDeliveryOrders = new List<DUDeliveryOrder>();
            _oDUDeliveryOrders = DUDeliveryOrder.Gets("Select * from View_DUDeliveryOrder where  isnull(ApproveBy,0)=0 order by DODATE DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.BUID = buid;
            return View(_oDUDeliveryOrders);
        }
        public ActionResult ViewDUDeliveryOrderTwo(int nId, double ts)
        {
            _oDUDeliveryOrder = new DUDeliveryOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            ExportSCDO oExportSCDO = new ExportSCDO();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            if (nId > 0)
            {
                _oDUDeliveryOrder = DUDeliveryOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUDeliveryOrder.DUDeliveryOrderID > 0)
                {
                    _oDUDeliveryOrder.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDUDeliveryOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly)
                {
                    oExportSCDO = oExportSCDO.Get(_oDUDeliveryOrder.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDUDeliveryOrder.ContractorName = oExportSCDO.ContractorName;
                    _oDUDeliveryOrder.ExportPINo = oExportSCDO.PINo_Full;
                    _oDUDeliveryOrder.ExportLCNo = oExportSCDO.ELCNo;
                    _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                    _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                    _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;
                }
                if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.SampleOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.SaleOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.Sampling)
                {

                    oDyeingOrder = DyeingOrder.Get(_oDUDeliveryOrder.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDUDeliveryOrder.ContractorName = oDyeingOrder.ContractorName;
                    _oDUDeliveryOrder.OrderNo = oDyeingOrder.OrderNoFull;
                    _oDUDeliveryOrder.ExportPINo = oDyeingOrder.ExportPINo;
                    _oDUDeliveryOrder.ExportLCNo = oDyeingOrder.ExportLCNo;
                    _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                    _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                    _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;
                    _oDUDeliveryOrder.PaymentType = oDyeingOrder.PaymentType;
                }
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            List<EnumObject> oOrderTypeObjs = new List<EnumObject>();
            List<EnumObject> oOrderTypes = new List<EnumObject>();
            oOrderTypeObjs = EnumObject.jGets(typeof(EnumOrderType));

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;

            return View(_oDUDeliveryOrder);
        }

        private bool ValidateInput(DUDeliveryOrder oDUDeliveryOrder)
        {
            if (oDUDeliveryOrder.ContractorID <= 0)
            {
                _sErrorMessage = "Please pick Party";
                return false;
            }
            return true;
        }

        [HttpPost]
        public JsonResult Save(DUDeliveryOrder oDUDeliveryOrder)
        {
            ExportSCDO oExportSCDO = new ExportSCDO();
            try
            {
                if (this.ValidateInput(oDUDeliveryOrder))
                {

                    _oDUDeliveryOrder = oDUDeliveryOrder.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUDeliveryOrder.DUDeliveryOrderID > 0)
                    {
                        _oDUDeliveryOrder.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(_oDUDeliveryOrder.DUDeliveryOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        if (_oDUDeliveryOrder.ExportPIID > 0 &&( _oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder|| _oDUDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly)) /// For Refresh Data (PI Info)  First Time, after Click Save
                        {
                            oExportSCDO = oExportSCDO.GetByPI(_oDUDeliveryOrder.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oDUDeliveryOrder.ExportPINo = oExportSCDO.PINo_Full;
                            _oDUDeliveryOrder.ExportLCNo = oExportSCDO.ELCNo;
                            _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                            _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                            _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;
                        }
                    }
                }
                else
                {
                    _oDUDeliveryOrder.ErrorMessage = _sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oDUDeliveryOrder = new DUDeliveryOrder();
                _oDUDeliveryOrder.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save_Log(DUDeliveryOrder oDUDeliveryOrder)
        {
            ExportSCDO oExportSCDO = new ExportSCDO();
            try
            {
                if (this.ValidateInput(oDUDeliveryOrder))
                {

                    _oDUDeliveryOrder = oDUDeliveryOrder.Save_Log(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUDeliveryOrder.DUDeliveryOrderID > 0)
                    {
                        _oDUDeliveryOrder.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(_oDUDeliveryOrder.DUDeliveryOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        if (_oDUDeliveryOrder.ExportPIID > 0 && (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder||_oDUDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly)) /// For Refresh Data (PI Info)  First Time, after Click Save
                        {
                            oExportSCDO = oExportSCDO.GetByPI(_oDUDeliveryOrder.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oDUDeliveryOrder.ExportPINo = oExportSCDO.PINo_Full;
                            _oDUDeliveryOrder.ExportLCNo = oExportSCDO.ELCNo;
                            _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                            _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                            _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;
                        }
                    }
                }
                else
                {
                    _oDUDeliveryOrder.ErrorMessage = _sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oDUDeliveryOrder = new DUDeliveryOrder();
                _oDUDeliveryOrder.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approve(DUDeliveryOrder oDUDeliveryOrder)
        {
            string sErrorMease = "";
            _oDUDeliveryOrder = oDUDeliveryOrder;
            try
            {
                _oDUDeliveryOrder = oDUDeliveryOrder.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DOCancel(DUDeliveryOrder oDUDeliveryOrder)
        {
            string sErrorMease = "";
            _oDUDeliveryOrder = oDUDeliveryOrder;
            try
            {
                _oDUDeliveryOrder = oDUDeliveryOrder.DOCancel(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateDONo(DUDeliveryOrder oDUDeliveryOrder)
        {
            _oDUDeliveryOrder = new DUDeliveryOrder();
            try
            {
                _oDUDeliveryOrder = oDUDeliveryOrder.UpdateDONo(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrder = new DUDeliveryOrder();
                _oDUDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(DUDeliveryOrder oDUDeliveryOrder)
        {
            try
            {
                if (oDUDeliveryOrder.DUDeliveryOrderID <= 0) { throw new Exception("Please select an valid item."); }
                oDUDeliveryOrder.ErrorMessage = oDUDeliveryOrder.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUDeliveryOrder = new DUDeliveryOrder();
                oDUDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDeliveryOrder.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail(DUDeliveryOrderDetail oDUDeliveryOrderDetail)
        {
            try
            {
                if (oDUDeliveryOrderDetail.DUDeliveryOrderDetailID > 0)
                {
                    oDUDeliveryOrderDetail.ErrorMessage = oDUDeliveryOrderDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                oDUDeliveryOrderDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDeliveryOrderDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOrderDetails(DUDeliveryOrder oDUDeliveryOrder)
        {
            string sSQL="";
            _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            List<ExportSCDetailDO> oExportSCDetailDOs = new List<ExportSCDetailDO>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DULotDistribution> oDULotDistributions = new List<DULotDistribution>();
            try
            {
                string sReturn = "";
               
                if (oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder || oDUDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly)
                {
                    sSQL = "SELECT * FROM View_ExportSCDetail_DO WHERE ExportSCID=" + oDUDeliveryOrder.OrderID;
                    if (!String.IsNullOrEmpty(oDUDeliveryOrder.ErrorMessage)) sSQL = sSQL + " And ExportSCDetailID not in (" + oDUDeliveryOrder.ErrorMessage + ")";
                   // oExportSCDetailDOs = ExportSCDetailDO.GetsByESCID(oDUDeliveryOrder.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oExportSCDetailDOs = ExportSCDetailDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    List<DyeingOrderDetail> oDOs = new List< DyeingOrderDetail>();
                    if (oExportSCDetailDOs.Count > 0)
                    {
                        sSQL = "SELECT * FROM View_DyeingOrderDetail WHERE ExportSCDetailID>0 and ExportSCDetailID in (" + string.Join(",", oExportSCDetailDOs.Select(x => x.ExportSCDetailID).ToList()) + ") and DyeingorderID in (Select DyeingOrderID from DyeingOrder where DyeingOrder.Status<>9 and DyeingOrderType<>"+(int)EnumOrderType.ClaimOrder+" )";
                        oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                  
                    foreach (ExportSCDetailDO oItem in oExportSCDetailDOs)
                    {
                        oDOs = oDyeingOrderDetails.Where(p => p.ExportSCDetailID == oItem.ExportSCDetailID).ToList();
                        _oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                        _oDUDeliveryOrderDetail.ExportSCDetailID = oItem.ExportSCDetailID;
                        _oDUDeliveryOrderDetail.ProductID = oItem.ProductID;
                        _oDUDeliveryOrderDetail.ProductName = oItem.ProductName;
                        _oDUDeliveryOrderDetail.ColorName = oItem.ColorInfo;
                        _oDUDeliveryOrderDetail.Lotbalance = oDOs.Where(p => p.ExportSCDetailID == oItem.ExportSCDetailID).Sum(o => o.Qty);
                        _oDUDeliveryOrderDetail.OrderQty = oItem.Qty;
                        _oDUDeliveryOrderDetail.Qty = (oItem.Qty - oItem.DOQty);
                        _oDUDeliveryOrderDetail.DOQty = oItem.DOQty;
                        if (_oDUDeliveryOrderDetail.Qty > 0)
                        {
                            _oDUDeliveryOrderDetails.Add(_oDUDeliveryOrderDetail);
                        }
                        //if (oDyeingOrderDetails.Any() && oDyeingOrderDetails.FirstOrDefault().ExportSCDetailID > 0)
                        //{
                        //    _oDUDeliveryOrderDetails.ForEach(x => { x.Lotbalance = x.Lotbalance + oDyeingOrderDetails.Where(p => p.ExportSCDetailID == x.ExportSCDetailID).Sum(o => o.Qty); });
                        //}
                    }
                }

                else if (oDUDeliveryOrder.OrderType == (int)EnumOrderType.LoanOrder)
                {
                    sSQL = "SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderID=" + oDUDeliveryOrder.OrderID;
                    if (!String.IsNullOrEmpty(oDUDeliveryOrder.ErrorMessage)) sSQL = sSQL + " And DyeingOrderDetailID not in (" + oDUDeliveryOrder.ErrorMessage + ")";
                    oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "Select * from View_DULotDistribution where DODID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + oDUDeliveryOrder.OrderID + ")";
                    //sSQL = "Select * from View_DULotDistribution WHERE DyeingOrderID=" + oDUDeliveryOrder.OrderID;
                    if (!String.IsNullOrEmpty(oDUDeliveryOrder.ErrorMessage)) sSQL = sSQL + " And DODID not in (" + oDUDeliveryOrder.ErrorMessage + ")";
                    oDULotDistributions = DULotDistribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DULotDistribution oItem in oDULotDistributions)
                    {
                        _oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                        _oDUDeliveryOrderDetail.ExportSCDetailID = 0;
                        _oDUDeliveryOrderDetail.DyeingOrderDetailID = oItem.DODID;
                        _oDUDeliveryOrderDetail.ProductID = oItem.ProductID;
                        _oDUDeliveryOrderDetail.ProductName = oItem.ProductName;
                        _oDUDeliveryOrderDetail.OrderQty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.ColorName = oItem.ColorName;
                        //_oDUDeliveryOrderDetail.Lot = oItem.LotID;
                        _oDUDeliveryOrderDetail.OrderNo = oDUDeliveryOrder.OrderNo;
                        _oDUDeliveryOrderDetail.OrderQty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.Qty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.DOQty = 0;
                        if (_oDUDeliveryOrderDetail.Qty > 0)
                        {
                            _oDUDeliveryOrderDetails.Add(_oDUDeliveryOrderDetail);
                        }
                    }
                    _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.GroupBy(x => new { x.ExportSCDetailID, x.DyeingOrderDetailID, x.ProductID, x.ProductName, x.ColorName, x.OrderNo}, (key, grp) =>
                                        new DUDeliveryOrderDetail
                                        {
                                            ExportSCDetailID = key.ExportSCDetailID,
                                            DyeingOrderDetailID = key.DyeingOrderDetailID,
                                            ProductID = key.ProductID,
                                            ProductName = key.ProductName,
                                            ColorName = key.ColorName,
                                            OrderNo = key.OrderNo,
                                            OrderQty=0,
                                            DOQty=0,
                                            Qty = grp.Sum(x => x.Qty),
                                            
                                }).ToList();
                    if (oDyeingOrderDetails.Any() && oDyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID > 0)
                    {
                        _oDUDeliveryOrderDetails.ForEach(x => { x.OrderQty = x.Qty + oDyeingOrderDetails.Where(p => p.DyeingOrderDetailID == x.DyeingOrderDetailID).Sum(o => o.Qty); });
                        _oDUDeliveryOrderDetails.ForEach(x => { x.DOQty = x.DOQty + oDyeingOrderDetails.Where(p => p.DyeingOrderDetailID == x.DyeingOrderDetailID).Sum(o => o.DOQty); });
                        //oImportPIDetails.RemoveAll(x => x.Qty <= 0);
                    }
                 
                }
              
                else
                {
                    sSQL = "SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderID=" + oDUDeliveryOrder.OrderID;
                    if (!String.IsNullOrEmpty(oDUDeliveryOrder.ErrorMessage)) sSQL = sSQL + " And DyeingOrderDetailID not in (" + oDUDeliveryOrder.ErrorMessage + ")";
                    oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //  oDyeingOrderDetails = DyeingOrderDetail.Gets(oDUDeliveryOrder.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DyeingOrderDetail oItem in oDyeingOrderDetails)
                    {
                        _oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                        _oDUDeliveryOrderDetail.ExportSCDetailID = oItem.ExportSCDetailID;
                        _oDUDeliveryOrderDetail.DyeingOrderDetailID = oItem.DyeingOrderDetailID;
                        _oDUDeliveryOrderDetail.ProductID = oItem.ProductID;
                        _oDUDeliveryOrderDetail.ProductName = oItem.ProductName;
                        _oDUDeliveryOrderDetail.OrderQty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.ColorName = oItem.ColorName;
                        _oDUDeliveryOrderDetail.OrderNo = oDUDeliveryOrder.OrderNo;
                        _oDUDeliveryOrderDetail.OrderQty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.Qty = Math.Round(oItem.Qty - oItem.DOQty, 2);
                        _oDUDeliveryOrderDetail.DOQty = Math.Round(oItem.DOQty, 2);
                        if (_oDUDeliveryOrderDetail.Qty > 0)
                        {
                            _oDUDeliveryOrderDetails.Add(_oDUDeliveryOrderDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExportPIs_DO(DUDeliveryOrder oDeliveryOrder)// For Delivery Order
        {
            string sSQL = "";
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<ExportSCDO> oExportSCDOs = new List<ExportSCDO>();
            List<DUDeliveryOrder> _oDUDeliveryOrders = new List<DUDeliveryOrder>();
            //List<DyeingOrderDetail> _oDyeingOrderDetails = new List<DyeingOrderDetail>();
            DUDeliveryOrder oDUDeliveryOrder = new DUDeliveryOrder();
            try
            {
                if (oDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder || oDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly )
                {
                    sSQL = "Select top(100)* from View_ExportSC_DO ";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oDeliveryOrder.OrderNo))
                    {
                        oDeliveryOrder.OrderNo = oDeliveryOrder.OrderNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " PINo Like'%" + oDeliveryOrder.OrderNo + "%'";
                    }
                    if (!String.IsNullOrEmpty(oDeliveryOrder.ExportLCNo))
                    {
                        oDeliveryOrder.ExportLCNo = oDeliveryOrder.ExportLCNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ExportLCNo Like'%" + oDeliveryOrder.ExportLCNo + "%'";
                    }
                    if (oDeliveryOrder.BUID>0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " BUID=" + oDeliveryOrder.BUID;
                    }
                    if (oDeliveryOrder.ContractorID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ContractorID  in(" + oDeliveryOrder.ContractorID + ")";
                    }
                    if (oDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ExportSCID in (Select ExportSCID from ExportSCDetail where ExportSCDetail.ProductionType in (" + (int)EnumProductionType.Commissioning + "," + (int)EnumProductionType.RawSale + ") )";

                    }

                    //if (oDeliveryOrder.OrderType == (int)EnumOrderType.ClaimOrder)
                    //{
                    //    Global.TagSQL(ref sReturn);
                    //    sReturn = sReturn + " ExportSCID in (Select OrderID from DUClaimOrder where OrderType=" + (int)EnumOrderType.BulkOrder + ")";

                    //}
                    //if (!oExportSCDO.IsRevisePI)
                    //{
                    //Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + "IsRevisePI=0";
                    //}

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PIStatus in (" + (int)EnumPIStatus.Approved + "," + (int)EnumPIStatus.PIIssue + "," + (int)EnumPIStatus.BindWithLC + ") and  isnull(IsClose,0)=0 and isnull(IsRevisePI,0)=0 order by exportpiid desc"; //For Dyeing/Production Order issue->Must Checked valid PI(  Initialized = 0, RequestForApproved = 1,
                    //Approved = 2,     PIIssue = 3,   BindWithLC = 4,   RequestForRevise = 5,  Cancel = 6) and Sales Contract is Approved

                    sSQL = sSQL + "" + sReturn;
                    oExportSCDOs = ExportSCDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //_oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (ExportSCDO oItem in oExportSCDOs)
                    {
                        oDUDeliveryOrder = new DUDeliveryOrder();
                        oDUDeliveryOrder.ExportPIID = oItem.ExportPIID;
                        oDUDeliveryOrder.OrderType = oDeliveryOrder.OrderType;
                        oDUDeliveryOrder.OrderTypeSt = ((EnumOrderType)oDeliveryOrder.OrderType).ToString();
                        oDUDeliveryOrder.ContractorID = oItem.ContractorID;
                        oDUDeliveryOrder.ContractorName = oItem.ContractorName;
                        //oDUDeliveryOrder.DeliveryToID = oItem.BuyerID;
                        oDUDeliveryOrder.DeliveryToName = oItem.ContractorName;
                        oDUDeliveryOrder.ContactPersonnelID = 0;
                        oDUDeliveryOrder.OrderID = oItem.ExportSCID;
                        oDUDeliveryOrder.MKTPName = oItem.MKTPName;
                        oDUDeliveryOrder.ExportPINo = oItem.PINo;
                        oDUDeliveryOrder.OrderNo = oItem.PINo;
                        oDUDeliveryOrder.ExportLCNo = oItem.ExportLCNo + " " + oItem.ExportLCStatusSt;
                        oDUDeliveryOrder.OrderValue = oItem.TotalAmount;
                        oDUDeliveryOrder.OrderQty = oItem.TotalQty;
                        //oDUDeliveryOrder.TotalDOQty = oItem.POQty;
                        _oDUDeliveryOrders.Add(oDUDeliveryOrder);
                    }
                }
              
            
                else 
                {
                    sSQL = "Select top(100)* from View_DyeingOrder";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oDeliveryOrder.OrderNo))
                    {
                        oDeliveryOrder.OrderNo = oDeliveryOrder.OrderNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "NoCode+OrderNo Like'%" + oDeliveryOrder.OrderNo + "%'";
                    }

                    if (oDeliveryOrder.ContractorID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ContractorID  in(" + oDeliveryOrder.ContractorID + ")";
                    }
                    //if (!oExportSCDO.IsRevisePI)
                    //{
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "  DyeingOrderType in (" + oDeliveryOrder.OrderType + ")";
                    //}


                    sSQL = sSQL + "" + sReturn + " order by OrderDate desc";
                    oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DyeingOrder oItem in oDyeingOrders)
                    {
                        oDUDeliveryOrder = new DUDeliveryOrder();
                        oDUDeliveryOrder.ExportPIID = oItem.ExportPIID;

                        oDUDeliveryOrder.OrderType = oDeliveryOrder.OrderType;
                        oDUDeliveryOrder.OrderTypeSt = oItem.OrderTypeSt;
                        oDUDeliveryOrder.ContractorID = oItem.ContractorID;
                        oDUDeliveryOrder.ContractorName = oItem.ContractorName;
                        //oDUDeliveryOrder.DeliveryToID = oItem.BuyerID;
                        oDUDeliveryOrder.DeliveryToName = oItem.ContractorName;
                        oDUDeliveryOrder.ContactPersonnelID = 0;
                        oDUDeliveryOrder.OrderID = oItem.DyeingOrderID;
                        oDUDeliveryOrder.MKTPName = oItem.MKTPName;
                        oDUDeliveryOrder.ExportPINo = oItem.ExportPINo;
                        oDUDeliveryOrder.OrderNo = oItem.OrderNoFull;
                        oDUDeliveryOrder.ExportLCNo = oItem.SampleInvocieNo;
                        //oDUDeliveryOrder.OrderValue = oItem.TotalAmount;
                        oDUDeliveryOrder.OrderQty = oItem.Qty;
                        //oDUDeliveryOrder.TotalDOQty = oItem.POQty;
                        _oDUDeliveryOrders.Add(oDUDeliveryOrder);
                    }
                }
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrders = new List<DUDeliveryOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetOrders(DUDeliveryOrder oDeliveryOrder)// For Delivery Order
        {
            string sSQL = "";
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<ExportSCDO> oExportSCDOs = new List<ExportSCDO>();
            List<DUDeliveryOrder> _oDUDeliveryOrders = new List<DUDeliveryOrder>();
            DUDeliveryOrder oDUDeliveryOrder = new DUDeliveryOrder();
            try
            {
              
                    sSQL = "Select top(100)* from View_DyeingOrder";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oDeliveryOrder.OrderNo))
                    {
                        oDeliveryOrder.OrderNo = oDeliveryOrder.OrderNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "NoCode+OrderNo Like'%" + oDeliveryOrder.OrderNo + "%'";
                    }

                    if (oDeliveryOrder.ContractorID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ContractorID  in(" + oDeliveryOrder.ContractorID + ")";
                    }
                    //if (!oExportSCDO.IsRevisePI)
                    //{
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "  DyeingOrderType in (" + oDeliveryOrder.OrderType + ")";
                    //}


                    sSQL = sSQL + "" + sReturn + " order by OrderDate desc";
                    oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DyeingOrder oItem in oDyeingOrders)
                    {
                        oDUDeliveryOrder = new DUDeliveryOrder();
                        oDUDeliveryOrder.ExportPIID = oItem.ExportPIID;

                        oDUDeliveryOrder.OrderType = oDeliveryOrder.OrderType;
                        oDUDeliveryOrder.OrderTypeSt = oItem.OrderTypeSt;
                        oDUDeliveryOrder.ContractorID = oItem.ContractorID;
                        oDUDeliveryOrder.ContractorName = oItem.ContractorName;
                        //oDUDeliveryOrder.DeliveryToID = oItem.BuyerID;
                        oDUDeliveryOrder.DeliveryToName = oItem.ContractorName;
                        oDUDeliveryOrder.ContactPersonnelID = 0;
                        oDUDeliveryOrder.OrderID = oItem.DyeingOrderID;
                        oDUDeliveryOrder.MKTPName = oItem.MKTPName;
                        oDUDeliveryOrder.ExportPINo = oItem.ExportPINo;
                        oDUDeliveryOrder.OrderNo = oItem.OrderNoFull;
                        oDUDeliveryOrder.ExportLCNo = oItem.SampleInvocieNo;
                        //oDUDeliveryOrder.OrderValue = oItem.TotalAmount;
                        oDUDeliveryOrder.OrderQty =  Math.Round(oItem.Qty,2);
                        //oDUDeliveryOrder.TotalDOQty = oItem.POQty;
                        _oDUDeliveryOrders.Add(oDUDeliveryOrder);
                    }
                
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrders = new List<DUDeliveryOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsOrderDetail(DUDeliveryOrder oDUDeliveryOrder)
        {
            string sSQL = "";
            _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            List<ExportSCDetailDO> oExportSCDetailDOs = new List<ExportSCDetailDO>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DULotDistribution> oDULotDistributions = new List<DULotDistribution>();
            try
            {
               
           

                if (oDUDeliveryOrder.OrderType == (int)EnumOrderType.LoanOrder)
                {
                    sSQL = "SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderID=" + oDUDeliveryOrder.OrderID;
                    if (!String.IsNullOrEmpty(oDUDeliveryOrder.ErrorMessage)) sSQL = sSQL + " And DyeingOrderDetailID not in (" + oDUDeliveryOrder.ErrorMessage + ")";
                    oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "Select * from View_DULotDistribution where DODID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + oDUDeliveryOrder.OrderID + ")";
                    //sSQL = "Select * from View_DULotDistribution WHERE DyeingOrderID=" + oDUDeliveryOrder.OrderID;
                    if (!String.IsNullOrEmpty(oDUDeliveryOrder.ErrorMessage)) sSQL = sSQL + " And DODID not in (" + oDUDeliveryOrder.ErrorMessage + ")";
                    oDULotDistributions = DULotDistribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DULotDistribution oItem in oDULotDistributions)
                    {
                        _oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                        _oDUDeliveryOrderDetail.ExportSCDetailID = 0;
                        _oDUDeliveryOrderDetail.DyeingOrderDetailID = oItem.DODID;
                        _oDUDeliveryOrderDetail.ProductID = oItem.ProductID;
                        _oDUDeliveryOrderDetail.ProductName = oItem.ProductName;
                        _oDUDeliveryOrderDetail.OrderQty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.ColorName = oItem.ColorName;
                        //_oDUDeliveryOrderDetail.Lot = oItem.LotID;
                        _oDUDeliveryOrderDetail.OrderNo = oDUDeliveryOrder.OrderNo;
                        _oDUDeliveryOrderDetail.OrderQty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.Qty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.DOQty = 0;
                        if (_oDUDeliveryOrderDetail.Qty > 0)
                        {
                            _oDUDeliveryOrderDetails.Add(_oDUDeliveryOrderDetail);
                        }
                    }
                    _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.GroupBy(x => new { x.ExportSCDetailID, x.DyeingOrderDetailID, x.ProductID, x.ProductName, x.ColorName, x.OrderNo }, (key, grp) =>
                                        new DUDeliveryOrderDetail
                                        {
                                            ExportSCDetailID = key.ExportSCDetailID,
                                            DyeingOrderDetailID = key.DyeingOrderDetailID,
                                            ProductID = key.ProductID,
                                            ProductName = key.ProductName,
                                            ColorName = key.ColorName,
                                            OrderNo = key.OrderNo,
                                            OrderQty = 0,
                                            DOQty = 0,
                                            Qty = grp.Sum(x => x.Qty),

                                        }).ToList();
                    if (oDyeingOrderDetails.Any() && oDyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID > 0)
                    {
                        _oDUDeliveryOrderDetails.ForEach(x => { x.OrderQty =  oDyeingOrderDetails.Where(p => p.DyeingOrderDetailID == x.DyeingOrderDetailID).Sum(o => o.Qty); });
                        _oDUDeliveryOrderDetails.ForEach(x => { x.DOQty = oDyeingOrderDetails.Where(p => p.DyeingOrderDetailID == x.DyeingOrderDetailID).Sum(o => o.DOQty); });
                        //oImportPIDetails.RemoveAll(x => x.Qty <= 0);
                    }

                }

                else
                {
                    List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>(); 
                    sSQL = "SELECT * FROM View_DyeingOrderReport WHERE DyeingOrderID=" + oDUDeliveryOrder.OrderID;
                    if (!String.IsNullOrEmpty(oDUDeliveryOrder.ErrorMessage)) sSQL = sSQL + " And DyeingOrderDetailID not in (" + oDUDeliveryOrder.ErrorMessage + ")";
                    sSQL = sSQL + " Order by ProductID";
                    oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //  oDyeingOrderDetails = DyeingOrderDetail.Gets(oDUDeliveryOrder.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DyeingOrderReport oItem in oDyeingOrderReports)
                    {
                        if(oItem.Qty_DC>0 &&  oItem.Qty_DO<=0)
                        {
                            oItem.Qty_DO = oItem.Qty_DC;
                        }
                        _oDUDeliveryOrderDetail = new DUDeliveryOrderDetail();
                        _oDUDeliveryOrderDetail.ExportSCDetailID = oItem.ExportSCDetailID;
                        _oDUDeliveryOrderDetail.DyeingOrderDetailID = oItem.DyeingOrderDetailID;
                        _oDUDeliveryOrderDetail.ProductID = oItem.ProductID;
                        _oDUDeliveryOrderDetail.ProductName = oItem.ProductName;
                        _oDUDeliveryOrderDetail.OrderQty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.ColorName = oItem.ColorName;
                        _oDUDeliveryOrderDetail.OrderNo = oDUDeliveryOrder.OrderNo;
                        _oDUDeliveryOrderDetail.OrderQty = Math.Round(oItem.Qty, 2);
                        _oDUDeliveryOrderDetail.Qty = Math.Round(oItem.Qty - oItem.Qty_DO, 2);
                        _oDUDeliveryOrderDetail.DOQty = Math.Round(oItem.Qty_DO, 2);
                        _oDUDeliveryOrderDetail.Qty_DC = Math.Round(oItem.Qty_DC, 2);
                        if (_oDUDeliveryOrderDetail.Qty > 0)
                        {
                            _oDUDeliveryOrderDetails.Add(_oDUDeliveryOrderDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintDUDeliveryOrder(int nId, bool bIsPrintHistory, double nts)
        {
            string sTemp = "";
            _oDUDeliveryOrder = new DUDeliveryOrder();
            ExportSCDO oExportSCDO = new ExportSCDO();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
       
            List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails_Previous = new List<DUDeliveryOrderDetail>();

            try
            {
                if (nId > 0)
                {
                    _oDUDeliveryOrder = DUDeliveryOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUDeliveryOrder.DUDeliveryOrderID > 0)
                    {
                        _oDUDeliveryOrder.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        if ((_oDUDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly ||_oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder) && _oDUDeliveryOrder.ExportPIID > 0)
                        {
                            oExportSCDO = oExportSCDO.GetByPI(_oDUDeliveryOrder.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oDUDeliveryOrder.ContractorName = oExportSCDO.ContractorName;
                            _oDUDeliveryOrder.ExportPINo = oExportSCDO.PINo_Full;
                            _oDUDeliveryOrder.ExportLCNo = oExportSCDO.ELCNo;
                            _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                            _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                            _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;

                            sTemp = string.Join(",", _oDUDeliveryOrder.DUDeliveryOrderDetails.Select(x => x.ExportSCDetailID).Distinct().ToList());
                            if (!string.IsNullOrEmpty(sTemp))
                            {
                                sTemp = "SELECT * FROM View_DyeingOrderDetail as FF where FF.ExportSCDetailID >0 and FF.ExportSCDetailID  in (" + sTemp + ")";
                                oDyeingOrderDetails = DyeingOrderDetail.Gets(sTemp, ((User)Session[SessionInfo.CurrentUser]).UserID);

                                if (oDyeingOrderDetails.Any() && oDyeingOrderDetails.FirstOrDefault().ExportSCDetailID > 0)
                                {
                                    foreach (DUDeliveryOrderDetail oItem in _oDUDeliveryOrder.DUDeliveryOrderDetails)
                                    {
                                        sTemp = string.Join(",", oDyeingOrderDetails.Where(p => p.ExportSCDetailID == oItem.ExportSCDetailID).Select(x => x.OrderNo).Distinct().ToList());
                                        oItem.ColorName = oItem.ColorName + sTemp;
                                    }

                                }
                            }

                        }
                        if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.SampleOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.SaleOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.TwistOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.ReConing)
                        {

                            oDyeingOrder = DyeingOrder.Get(_oDUDeliveryOrder.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oDUDeliveryOrder.ContractorName = oDyeingOrder.ContractorName;
                            //_oDUDeliveryOrder.OrderNo = oDyeingOrder.D;
                            _oDUDeliveryOrder.ExportPINo = oDyeingOrder.ExportPINo;
                            _oDUDeliveryOrder.ExportLCNo = oDyeingOrder.ExportLCNo;
                            _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                            _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                            _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;
                            _oDUDeliveryOrder.PaymentType = oDyeingOrder.PaymentType;
                        }

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
            rptDUDeliveryOrder oReport = new rptDUDeliveryOrder();
            byte[] abytes = oReport.PrepareReport(_oDUDeliveryOrder, oCompany, oBusinessUnit, oDUDeliveryOrderDetails_Previous);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintDUDeliveryOrderTwo(int nId, int nBUID, double nts)
        {
            _oDUDeliveryOrder = new DUDeliveryOrder();
            ExportSCDO oExportSCDO = new ExportSCDO();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            DUDyeingStep oDUDyeingStep = new DUDyeingStep();
            List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails_Previous = new List<DUDeliveryOrderDetail>();
            try
            {
                if (nId > 0)
                {
                    _oDUDeliveryOrder = DUDeliveryOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUDeliveryOrder.DUDeliveryOrderID > 0)
                    {
                        _oDUDeliveryOrder.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        if ((_oDUDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder) && _oDUDeliveryOrder.ExportPIID > 0)
                        {
                            oExportSCDO = oExportSCDO.GetByPI(_oDUDeliveryOrder.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oDUDeliveryOrder.ContractorName = oExportSCDO.ContractorName;
                            _oDUDeliveryOrder.ExportPINo = oExportSCDO.PINo_Full;
                            _oDUDeliveryOrder.ExportLCNo = oExportSCDO.ELCNo;
                            _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                            _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                            _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;
                        }
                        else //if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.SampleOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.SaleOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.TwistOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.ReConing || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.ClaimOrder)
                        {

                            oDyeingOrder = DyeingOrder.Get(_oDUDeliveryOrder.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oDUDeliveryOrder.ContractorName = oDyeingOrder.ContractorName;
                            //_oDUDeliveryOrder.OrderNo = oDyeingOrder.D;
                            _oDUDeliveryOrder.ExportPINo = oDyeingOrder.ExportPINo;
                            _oDUDeliveryOrder.ExportLCNo = oDyeingOrder.ExportLCNo;
                            _oDUDeliveryOrder.OrderValue = oExportSCDO.TotalAmount;
                            _oDUDeliveryOrder.OrderQty = oExportSCDO.TotalQty;
                            _oDUDeliveryOrder.TotalDOQty = oExportSCDO.POQty;
                            _oDUDeliveryOrder.PaymentType = oDyeingOrder.PaymentType;
                        }
                        oDUOrderSetup = oDUOrderSetup.GetByType(_oDUDeliveryOrder.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oDUDyeingStep = oDUDyeingStep.GetBy((int)_oDUDeliveryOrder.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
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


            if (oDUOrderSetup.PrintNo ==(int)EnumExcellColumn.A)
            {
                rptDUDeliveryOrder oReport = new rptDUDeliveryOrder();
                byte[] abytes = oReport.PrepareReport(_oDUDeliveryOrder, oCompany, oBusinessUnit, oDUDeliveryOrderDetails_Previous);
                return File(abytes, "application/pdf");
            }
            else if (oDUOrderSetup.PrintNo == (int)EnumExcellColumn.B)
            {
                rptDUDeliveryOrder oReport = new rptDUDeliveryOrder();
                byte[] abytes = oReport.PrepareReportTwo(_oDUDeliveryOrder, oCompany, oBusinessUnit, oDUOrderSetup, oDUDyeingStep);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptDUDeliveryOrder oReport = new rptDUDeliveryOrder();
                byte[] abytes = oReport.PrepareReport(_oDUDeliveryOrder, oCompany, oBusinessUnit, oDUDeliveryOrderDetails_Previous);
                return File(abytes, "application/pdf");
            }

        }
        #region Search
        [HttpPost]
        public JsonResult AdvSearch(DUDeliveryOrder oDUDeliveryOrder)
        {
            _oDUDeliveryOrders = new List<DUDeliveryOrder>();
            try
            {
                string sSQL = MakeSQL(oDUDeliveryOrder);
                _oDUDeliveryOrders = DUDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrders = new List<DUDeliveryOrder>();
                //_oDUDeliveryOrder.ErrorMessage = ex.Message;
                //_oDUDeliveryOrders.Add(_oDUDeliveryOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(DUDeliveryOrder oDUDeliveryOrder)
        {
            string sParams = oDUDeliveryOrder.Note;

            int nCboDODate = 0;
            DateTime dFromDODate = DateTime.Today;
            DateTime dToDODate = DateTime.Today;
            int nCboInvoiceDate = 0;
            DateTime dFromInvoiceDate = DateTime.Today;
            DateTime dToInvoiceDate = DateTime.Today;
            int nCboDeliveryDate = 0;
            DateTime dFromDeliveryDate = DateTime.Today;
            DateTime dToDeliveryDate = DateTime.Today;

            int nCboMkPerson = 0;
            int nPaymentType = 0;
            int nOrderType = 0;

            string sProductIDs = "";

            string sPINo = "";
            string sInvoiceNo = "";
            string sPONo = "";
            string sRefNo = "";

            bool bYetNotSendLabReq = false;
            bool bYetNotSendPro = false;
            int nOrderTypeFixec = 0;
            int nBUID = 0;



            if (!string.IsNullOrEmpty(sParams))
            {
                string sTemp = "";
                _oDUDeliveryOrder.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                _oDUDeliveryOrder.DeliveryToName = Convert.ToString(sParams.Split('~')[1]);

                nCboDODate = Convert.ToInt32(sParams.Split('~')[2]);
                dFromDODate = Convert.ToDateTime(sParams.Split('~')[3]);
                dToDODate = Convert.ToDateTime(sParams.Split('~')[4]);
                //nCboInvoiceDate = Convert.ToInt32(sParams.Split('~')[5]);
                //dFromInvoiceDate = Convert.ToDateTime(sParams.Split('~')[6]);
                //dToInvoiceDate = Convert.ToDateTime(sParams.Split('~')[7]);
                nCboDeliveryDate = Convert.ToInt32(sParams.Split('~')[8]);
                sTemp = Convert.ToString(sParams.Split('~')[9]);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    dFromDeliveryDate = Convert.ToDateTime(sParams.Split('~')[9]);
                    dToDeliveryDate = Convert.ToDateTime(sParams.Split('~')[10]);
                }

                nCboMkPerson = Convert.ToInt32(sParams.Split('~')[11]);
                nPaymentType = Convert.ToInt32(sParams.Split('~')[12]);
                nOrderType = Convert.ToInt32(sParams.Split('~')[13]);

                sPINo = Convert.ToString(sParams.Split('~')[14]);
                sInvoiceNo = Convert.ToString(sParams.Split('~')[15]);
                sPONo = Convert.ToString(sParams.Split('~')[16]);
                sRefNo = Convert.ToString(sParams.Split('~')[17]);

                sProductIDs = Convert.ToString(sParams.Split('~')[18]);

                bYetNotSendLabReq = Convert.ToBoolean(sParams.Split('~')[19]);
                bYetNotSendPro = Convert.ToBoolean(sParams.Split('~')[20]);

                nOrderTypeFixec = Convert.ToInt16(sParams.Split('~')[21]);


                nBUID = Convert.ToInt32(sParams.Split('~')[22]);
            }


            string sReturn1 = "SELECT * FROM View_DUDeliveryOrder AS DO ";
            string sReturn = "";

            #region Contractor
            if (!String.IsNullOrEmpty(_oDUDeliveryOrder.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.ContractorID in(" + _oDUDeliveryOrder.ContractorName + ")";
            }
            #endregion

            #region Buyer Id
            if (!String.IsNullOrEmpty(_oDUDeliveryOrder.DeliveryToName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.ContractorID in(" + _oDUDeliveryOrder.DeliveryToName + ")";
            }
            #endregion

            #region DODate Date
            if (nCboDODate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboDODate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Delivery Date
            if (nCboDeliveryDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (nCboDeliveryDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) )";
                }

                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }

                else if (nCboDeliveryDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }

            }
            #endregion

            #region nOrderTypeFixec
            /// This Searching Critria for Different Window Bulk and Sample
            if (nOrderTypeFixec > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.OrderType in (" + nOrderTypeFixec.ToString() + ")";
            }
            #endregion


        #endregion

        #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUDeliveryOrderID in (Select DOD.DUDeliveryOrderID from DUDeliveryOrderDetail as DOD where ProductID in (" + sProductIDs + "))";
            }
            #endregion

        #region Po No
            if (!string.IsNullOrEmpty(sPONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType=" + (int)EnumOrderType.SampleOrder + " and OrderID  in (Select DyeingOrder.DyeingOrderID from DyeingOrder where OrderNo Like  '%" + sPONo + "%')";
                // sReturn = sReturn + "(OrderType=" + (int)EnumOrderType.SampleOrder + " and OrderID  in (Select DyeingOrder.DyeingOrderID from DyeingOrder where OrderNo Like  '%" + sPONo + "%'))) or (OrderType=2 and OrderID in (Select DyeingOrder.ExportPIID from DyeingOrder where OrderNo Like  '%" + sPONo + "%'))";
            }
            #endregion

       #region Invoice No
            if (!string.IsNullOrEmpty(sInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType=2 and OrderID in (Select DyeingOrder.DyeingOrderID from DyeingOrder where DyeingOrder.SampleInvoiceID in (Select sampleInvoice.SampleInvoiceID from sampleInvoice where sampleInvoiceNo like '%0%' ))";
            }
            #endregion

       #region P/I  No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType=3 and OrderID in  (Select ExportSC.ExportSCID from ExportSC where ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINO Like '%" + sPINo + "%'))";
            }
            #endregion

            #region Business Unit
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " DO.BUID = " + nBUID;
            //}
            #endregion
            string sSQL = sReturn1 + " " + sReturn + " order by OrderNo";
            return sSQL;
        }
        [HttpPost]
        public JsonResult GetbyNo(DUDeliveryOrder oDUDeliveryOrder)
        {
            _oDUDeliveryOrders = new List<DUDeliveryOrder>();

            try
            {
                string sSQL = "SELECT * FROM View_DUDeliveryOrder ";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oDUDeliveryOrder.DONo))
                {
                    oDUDeliveryOrder.DONo = oDUDeliveryOrder.DONo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DONo like '%" + oDUDeliveryOrder.DONo + "%'";
                }
                if (!String.IsNullOrEmpty(oDUDeliveryOrder.OrderNo))
                {
                    oDUDeliveryOrder.OrderNo = oDUDeliveryOrder.OrderNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DUDeliveryOrderID in ( SELECT DUDeliveryOrderID FROM View_DUDeliveryOrderDetail where OrderNo like '%" + oDUDeliveryOrder.OrderNo + "%')";

                }

                if (oDUDeliveryOrder.OrderType > 0)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderType=" + oDUDeliveryOrder.OrderType;
                }

                sSQL = sSQL + "" + sReturn;
                _oDUDeliveryOrders = DUDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrders = new List<DUDeliveryOrder>();
                //_oDUDeliveryOrder.ErrorMessage = ex.Message;
                //_oDUDeliveryOrders.Add(_oDUDeliveryOrder);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 
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

        [HttpPost]
        public ActionResult SetDUDeliveryOrderData(DUDeliveryOrder oDUDeliveryOrder)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDUDeliveryOrder);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintDUDeliveryOrderList()
        {
            _oDUDeliveryOrder = new DUDeliveryOrder();
            try
            {
                _oDUDeliveryOrder = (DUDeliveryOrder)Session[SessionInfo.ParamObj];
                _oDUDeliveryOrders = DUDeliveryOrder.Gets("SELECT * FROM View_DUDeliveryOrder WHERE DUDeliveryOrderID IN (" + _oDUDeliveryOrder.ErrorMessage + ") Order By DUDeliveryOrderID", (int)Session[SessionInfo.currentUserID]);
                _oDUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets("SELECT * FROM View_DUDeliveryOrderDetail WHERE DUDeliveryOrderID IN (" + _oDUDeliveryOrder.ErrorMessage + ") Order By DUDeliveryOrderID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrder = new DUDeliveryOrder();
                _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oDUDeliveryOrder.BUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oDUDeliveryOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUDeliveryOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            rptDUDeliveryOrderDetails oReport = new rptDUDeliveryOrderDetails();
            byte[] abytes = oReport.PrepareReport(_oDUDeliveryOrders, _oDUDeliveryOrderDetails, oCompany, bIsRateView);
            return File(abytes, "application/pdf");
        }

        public void ExcelDUDeliveryOrderList()
        {
            _oDUDeliveryOrder = new DUDeliveryOrder();
            try
            {
                _oDUDeliveryOrder = (DUDeliveryOrder)Session[SessionInfo.ParamObj];
                _oDUDeliveryOrders = DUDeliveryOrder.Gets("SELECT * FROM View_DUDeliveryOrder WHERE DUDeliveryOrderID IN (" + _oDUDeliveryOrder.ErrorMessage + ") Order By DUDeliveryOrderID", (int)Session[SessionInfo.currentUserID]);
                _oDUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets("SELECT * FROM View_DUDeliveryOrderDetail WHERE DUDeliveryOrderID IN (" + _oDUDeliveryOrder.ErrorMessage + ") Order By DUDeliveryOrderID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrder = new DUDeliveryOrder();
                _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            }

            if (_oDUDeliveryOrders.Count > 0)
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUDeliveryOrder.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(_oDUDeliveryOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                bool bIsRateView = false;
                List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
                oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUDeliveryOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

                oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
                if (oAuthorizationRoleMapping.Count > 0)
                {
                    bIsRateView = true;
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("Delivery Order Details");
                    sheet.Name = "Delivery Order Details";
                    sheet.Column(nStartCol++).Width = 5; //SL
                    sheet.Column(nStartCol++).Width = 15; //challan no
                    sheet.Column(nStartCol++).Width = 12; //Date
                    sheet.Column(nStartCol++).Width = 25; //Buyer
                    sheet.Column(nStartCol++).Width = 15; //Order No
                    sheet.Column(nStartCol++).Width = 15; //PI No
                    sheet.Column(nStartCol++).Width = 40; //Product
                    sheet.Column(nStartCol++).Width = 15; //qty
                    if (bIsRateView)
                    {
                        sheet.Column(nStartCol++).Width = 12; //U. Price
                        sheet.Column(nStartCol++).Width = 20; //Note
                    }


                    nTotalCol = nStartCol;
                    nStartCol = 2;

                    #region Report Header
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = "Delivery Order Details"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "PI No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    if (bIsRateView)
                    {
                        cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    }

                    rowIndex = rowIndex + 1;
                    #endregion

                    #region data
                    int nCount = 0;
                    foreach (DUDeliveryOrder oItem in _oDUDeliveryOrders)
                    {
                        List<DUDeliveryOrderDetail> oTempDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
                        oTempDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.Where(x => x.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList();
                        int rowCount = (oTempDUDeliveryOrderDetails.Count() - 1);
                        if (rowCount <= 0) rowCount = 0;
                        nStartCol = 2;

                        #region main object
                        nCount++;
                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.DONo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.DeliveryDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.DeliveryToName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ExportPINo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion

                        #region Detail
                        if (oTempDUDeliveryOrderDetails.Count > 0)
                        {
                            foreach (DUDeliveryOrderDetail oItemDetail in oTempDUDeliveryOrderDetails)
                            {
                                nStartCol = 8;
                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.Qty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                if (bIsRateView)
                                {
                                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.UnitPrice; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = (oItemDetail.Qty * oItemDetail.UnitPrice); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                }

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

                            if (bIsRateView)
                            {
                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            }

                            rowIndex++;
                        }
                        #endregion

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = _oDUDeliveryOrderDetails.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (bIsRateView)
                    {
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = _oDUDeliveryOrderDetails.Select(x => (x.Qty * x.UnitPrice)).Sum(); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Return_Challan_Details.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }
                #endregion

        }

        #endregion

    }
}