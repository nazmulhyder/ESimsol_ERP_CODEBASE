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

namespace ESimSolFinancial.Controllers
{
    public class DUDeliveryChallanController : Controller
    {
        #region Declaration
        DUDeliveryChallan _oDUDeliveryChallan = new DUDeliveryChallan();
        DUDeliveryChallanDetail _oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();
        List<DUDeliveryChallan> _oDUDeliveryChallans = new List<DUDeliveryChallan>();
        List<DUDeliveryChallanDetail> _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
        string _sErrorMessage = "";
        #endregion

        #region DUDeliveryChallan
        public ActionResult ViewDUDeliveryChallans(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oDUDeliveryChallans = new List<DUDeliveryChallan>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            string sIssueStoreIDs = "";
            oWorkingUnits = new List<WorkingUnit>();
            
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DUDeliveryChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            sIssueStoreIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).ToList());
            if (string.IsNullOrEmpty(sIssueStoreIDs))
                sIssueStoreIDs = "0";

            _oDUDeliveryChallans = DUDeliveryChallan.Gets("Select * from View_DUDeliveryChallan where  WorkingUnitID in (" + sIssueStoreIDs + ") and  isnull(ApproveBy,0)=0", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
          
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
         
            return View(_oDUDeliveryChallans);
        }

        public ActionResult ViewDUDeliveryChallan(int nId, double ts)
        {
            _oDUDeliveryChallan = new DUDeliveryChallan();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, (int)Session[SessionInfo.currentUserID]);
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(oBusinessUnit.BusinessUnitID, EnumModuleName.DUDeliveryChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            DyeingOrder oDyeingOrder = new DyeingOrder();
            if (nId > 0)
            {
                _oDUDeliveryChallan = DUDeliveryChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUDeliveryChallan.DUDeliveryChallanID > 0)
                {
                    _oDUDeliveryChallan.DUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
             
              if (_oDUDeliveryChallan.DUDeliveryChallanDetails.Count > 0)
              {
                  if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryChallan.OrderType == (int)EnumOrderType.DyeingOnly)
                  {
                      _oDUDeliveryChallan.OrderNos = string.Join(",", _oDUDeliveryChallan.DUDeliveryChallanDetails.Select(x => x.PI_SampleNo).Distinct().ToList());
                  }
                  else
                  {
                      _oDUDeliveryChallan.OrderNos = string.Join(",", _oDUDeliveryChallan.DUDeliveryChallanDetails.Select(x => x.OrderNo).Distinct().ToList());
                  }
                  _oDUDeliveryChallan.DONos = string.Join(",", _oDUDeliveryChallan.DUDeliveryChallanDetails.Select(x => x.DONo).Distinct().ToList());
              }
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.WorkingUnits = oWorkingUnits;
           //ViewBag.WorkingUnits = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit where WorkingUnitID=8", ((User)Session[SessionInfo.CurrentUser]).UserID);
         
            ViewBag.OrderTypes = ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PackCountList = EnumObject.jGets(typeof(EnumPackCountBy));
            return View(_oDUDeliveryChallan);
        }
      
        private bool ValidateInput(DUDeliveryChallan oDUDeliveryChallan)
        {
           if (oDUDeliveryChallan.ContractorID <=0)
             {
               _sErrorMessage = "Please pick Party";
               return false;
            }
            return true;
        }

        [HttpPost]
        public JsonResult Save(DUDeliveryChallan oDUDeliveryChallan)
        {
           
            try
            {
                if (this.ValidateInput(oDUDeliveryChallan))
                {

                    _oDUDeliveryChallan = oDUDeliveryChallan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUDeliveryChallan.DUDeliveryChallanID > 0)
                    {
                        _oDUDeliveryChallan.DUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(_oDUDeliveryChallan.DUDeliveryChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    _oDUDeliveryChallan.ErrorMessage=_sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oDUDeliveryChallan = new DUDeliveryChallan();
                _oDUDeliveryChallan.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult Approve(DUDeliveryChallan oDUDeliveryChallan)
        {
            string sErrorMease = "";
            _oDUDeliveryChallan = oDUDeliveryChallan;
            try
            {
                _oDUDeliveryChallan = oDUDeliveryChallan.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       

        [HttpPost]
        public JsonResult Delete(DUDeliveryChallan oDUDeliveryChallan)
        {
            try
            {
                if (oDUDeliveryChallan.DUDeliveryChallanID <= 0) { throw new Exception("Please select an valid item."); }
                oDUDeliveryChallan.ErrorMessage = oDUDeliveryChallan.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUDeliveryChallan = new DUDeliveryChallan();
                oDUDeliveryChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDeliveryChallan.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
          [HttpPost]
        public JsonResult DeleteDetail(DUDeliveryChallanDetail oDUDeliveryChallanDetail)
        {
            try
            {
                if (oDUDeliveryChallanDetail.DUDeliveryChallanDetailID > 0)
                {
                    oDUDeliveryChallanDetail.ErrorMessage = oDUDeliveryChallanDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();
                oDUDeliveryChallanDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDeliveryChallanDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

          [HttpPost]
          public JsonResult GetDOs(DUDeliveryOrder oDUDeliveryOrder)
          {
              _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
              List<DUDeliveryOrder> oDUDeliveryOrders = new List<DUDeliveryOrder>();
              try
              {
                  string sSQL = "SELECT top(200)* FROM View_DUDeliveryOrder";
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
                  if (oDUDeliveryOrder.ContractorID > 0)
                  {
                      Global.TagSQL(ref sReturn);
                      sReturn = sReturn + "ContractorID=" + oDUDeliveryOrder.ContractorID + "";
                  }

                  if (oDUDeliveryOrder.OrderType > 0)
                  {
                      Global.TagSQL(ref sReturn);
                      sReturn = sReturn + "OrderType=" + oDUDeliveryOrder.OrderType;
                  }
                  Global.TagSQL(ref sReturn);
                  sReturn = sReturn + "isnull(ApproveBy,0)!=0 and DOStatus not in ("+(int)EnumDOStatus.Cancel+")";
                  //Global.TagSQL(ref sReturn);
                  //sReturn = sReturn + "OrderType=" + oDUDeliveryOrder.OrderType;

                  sSQL = sSQL + "" + sReturn + " order by DUDeliveryOrderID DESC";
                  oDUDeliveryOrders = DUDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

              }
              catch (Exception ex)
              {
                  oDUDeliveryOrders = new List<DUDeliveryOrder>();
              }
              JavaScriptSerializer serializer = new JavaScriptSerializer();
              string sjson = serializer.Serialize(oDUDeliveryOrders);
              return Json(sjson, JsonRequestBehavior.AllowGet);
          }
         [HttpPost]
         public JsonResult GetDODs(DUDeliveryOrder oDUDeliveryOrder)
         {
                          
             List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
             List<DULotDistribution> oDULotDistributions = new List<DULotDistribution>();
             List<DUDeliveryOrder> oDUDeliveryOrders = new List<DUDeliveryOrder>();

             List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();

             try
             {
                 string sSQL = "SELECT top(200)* FROM View_DUDeliveryOrderDetail";
                 string sReturn = "";

                 if (oDUDeliveryOrder.DUDeliveryOrderID > 0)
                 {
                     Global.TagSQL(ref sReturn);
                     sReturn = sReturn + "DUDeliveryOrderID=" + oDUDeliveryOrder.DUDeliveryOrderID;
                 }
                 if (!string.IsNullOrEmpty(oDUDeliveryOrder.OrderNo))
                 {
                     Global.TagSQL(ref sReturn);
                     sReturn = sReturn + "OrderNo like '%" + oDUDeliveryOrder.OrderNo + "%'";
                 }

                 sSQL = sSQL + "" + sReturn + " order by DUDeliveryOrderDetailID DESC";
                 oDUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 string sDODIDs = string.Join(",", oDUDeliveryOrderDetails.Where(p=>p.DyeingOrderDetailID>0).Select(x => x.DyeingOrderDetailID).ToList());

                 if (!string.IsNullOrEmpty(sDODIDs))
                 {
                     sSQL = "select * from [View_DULotDistribution] where Qty>0 and DODID in ("+sDODIDs+")";
                     oDULotDistributions = DULotDistribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                     if (oDULotDistributions.Any() && oDULotDistributions.FirstOrDefault().DODID > 0)
                     {
                         oDUDeliveryOrderDetails.ForEach(x => { x.Lotbalance = x.Lotbalance + oDULotDistributions.Where(p => p.DODID == x.DyeingOrderDetailID).Sum(o => o.Qty); });
                     }
                 }
                 else
                 {
                     sDODIDs = string.Join(",", oDUDeliveryOrderDetails.Where(p => p.ExportSCDetailID > 0).Select(x => x.ExportSCDetailID).ToList());
                     if (!string.IsNullOrEmpty(sDODIDs))
                     {
                         sSQL = "select * from [View_DyeingOrderReport] where isnull(ExportSCDetailID,0)>0 and ExportSCDetailID in (" + sDODIDs + ")";
                         oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                         if (oDyeingOrderReports.Any() && oDyeingOrderReports.FirstOrDefault().ExportSCDetailID > 0)
                         {
                             oDUDeliveryOrderDetails.ForEach(x => { x.Lotbalance = x.Lotbalance + oDyeingOrderReports.Where(p => p.ExportSCDetailID == x.ExportSCDetailID).Sum(o => o.StockInHand); });
                         }
                     }

                 }

                  sDODIDs = string.Join(",", oDUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderID).ToList());
                 if (!string.IsNullOrEmpty(sDODIDs))
                 {
                     sSQL = "select * from [View_DUDeliveryOrder] where DUDeliveryOrderID in (" + sDODIDs + ")";
                     oDUDeliveryOrders = DUDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                     if (oDUDeliveryOrders.Any() && oDUDeliveryOrders.FirstOrDefault().DUDeliveryOrderID > 0)
                     {
                         oDUDeliveryOrderDetails.ForEach(x => { x.ContractorID = oDUDeliveryOrders.Where(p => p.DUDeliveryOrderID == x.DUDeliveryOrderID).FirstOrDefault().ContractorID; });
                         oDUDeliveryOrderDetails.ForEach(x => { x.DeliveryToName = oDUDeliveryOrders.Where(p => p.DUDeliveryOrderID == x.DUDeliveryOrderID).FirstOrDefault().DeliveryToName; });
                     }
                 }

             }
             catch (Exception ex)
             {
                 oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(oDUDeliveryOrderDetails);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
        
         [HttpPost]
         public JsonResult GetsOrders(DUDeliveryChallanDetail oDCD)// For Delivery Order
         {
             string sSQL = "";
             List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
             _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
             DUDeliveryChallanDetail oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();
             try
             {

                 if (oDCD.DOID <= 0 && oDCD.DODetailID<=0)
                 {
                     throw new Exception("Delivery Order not found.");
                 }
                 if (oDCD.OrderType <= 0 )
                 {
                     throw new Exception("Order Type Not Found.");
                 }
              

                 sSQL = "Select * from View_DyeingOrderReport";
                 string sReturn = "";
                 if (oDCD.OrderType == (int)EnumOrderType.BulkOrder || oDCD.OrderType == (int)EnumOrderType.DyeingOnly)
                 {
                     if (oDCD.DODetailID > 0)
                     {
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + " isnull(StockInHand,0)>0 and DyeingOrderType=" + oDCD.OrderType + " and isnull(ExportSCDetailID,0)>0 and ExportSCDetailID in (Select ExportSCDetailID from DUDeliveryOrderDetail where DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrder where isnull(ApproveBy,0)<>0 and isnull(DOStatus,0)<>9) and  DUDeliveryOrderDetailID=" + oDCD.DODetailID + ")";
                     }
                     else if (oDCD.DOID > 0)
                     {
                         Global.TagSQL(ref sReturn);
                        // sReturn = sReturn + " isnull(StockInHand,0)>0 and DyeingOrderType=" + oDCD.OrderType + " and isnull(DyeingOrderDetailID,0)>0 and DyeingOrderDetailID  in(Select DyeingOrderDetailID from DUDeliveryOrderDetail where DyeingOrderDetailID>0 and DUDeliveryOrderID=" + oDCD.DOID + ")";
                         sReturn = sReturn + " isnull(StockInHand,0)>0 and DyeingOrderType=" + oDCD.OrderType + " and isnull(ExportSCDetailID,0)>0 and ExportSCDetailID in (Select ExportSCDetailID from DUDeliveryOrderDetail where DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrder where isnull(ApproveBy,0)<>0 and isnull(DOStatus,0)<>9) and DUDeliveryOrderID=" + oDCD.DOID + ")";
                     }

                 }
                 else
                 {
                     if (oDCD.DODetailID > 0)
                     {
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "DyeingOrderType=" + oDCD.OrderType + " and isnull(DyeingOrderDetailID,0)>0 and DyeingOrderDetailID  in(Select DyeingOrderDetailID from DUDeliveryOrderDetail where DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrder where isnull(ApproveBy,0)<>0 and isnull(DOStatus,0)<>9) and DyeingOrderDetailID>0 and DUDeliveryOrderDetailID=" + oDCD.DODetailID + ")";
                     }
                     else if (oDCD.DOID > 0)
                     {
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "DyeingOrderType=" + oDCD.OrderType + " and isnull(DyeingOrderDetailID,0)>0 and DyeingOrderDetailID  in(Select DyeingOrderDetailID from DUDeliveryOrderDetail where DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrder where isnull(ApproveBy,0)<>0 and isnull(DOStatus,0)<>9) and DyeingOrderDetailID>0 and DUDeliveryOrderID=" + oDCD.DOID + ")";
                     }
                 }

                 sSQL = sSQL + "" + sReturn + " order by ProductID";
                 oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 foreach (DyeingOrderReport oItem in oDyeingOrderReports)
                 {
                     oDUDeliveryChallanDetail = new DUDeliveryChallanDetail();
                     oDUDeliveryChallanDetail.ProductID = oItem.ProductID;
                     oDUDeliveryChallanDetail.DyeingOrderDetailID = oItem.DyeingOrderDetailID;
                     oDUDeliveryChallanDetail.ProductName = oItem.ProductName;
                     oDUDeliveryChallanDetail.OrderType = oDCD.OrderType;
                     //oDUDeliveryChallanDetail.OrderID = oItem.OrderID;
                     oDUDeliveryChallanDetail.ColorName = oItem.ColorName;
                     oDUDeliveryChallanDetail.MUnit = oItem.MUName;
                     oDUDeliveryChallanDetail.OrderNo = oItem.OrderNoFull;
                     oDUDeliveryChallanDetail.ColorNo = oItem.ColorNo;
                     oDUDeliveryChallanDetail.Shade = oItem.Shade;
                     oDUDeliveryChallanDetail.StockInHand = oItem.StockInHand;
                     oDUDeliveryChallanDetail.Qty = oItem.Qty - oItem.Qty_DC;
                     oDUDeliveryChallanDetail.OrderQty = oItem.Qty;
                     oDUDeliveryChallanDetail.DeliveryQty = oItem.Qty_DC;
                     _oDUDeliveryChallanDetails.Add(oDUDeliveryChallanDetail);
                 }

             }
             catch (Exception ex)
             {
                 _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oDUDeliveryChallanDetails);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
       

         [HttpPost]
         public JsonResult GetLots(DUDeliveryChallanDetail oDCD)
         {
             _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
             
             try
             {
                 _oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets_Lot(oDCD.WorkingUnitID, oDCD.DODetailID, oDCD.PTUID, oDCD.DyeingOrderDetailID, oDCD.LotID, ((User)Session[SessionInfo.CurrentUser]).UserID);

             }
             catch (Exception ex)
             {
                 _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oDUDeliveryChallanDetails);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }

         [HttpPost]
         public JsonResult UpdateFields(DUDeliveryChallan oDUDeliveryChallan)
         {
             _oDUDeliveryChallan = new DUDeliveryChallan();
             try
             {
                 _oDUDeliveryChallan = oDUDeliveryChallan.UpdateFields((int)Session[SessionInfo.currentUserID]);
             }
             catch (Exception ex)
             {
                 _oDUDeliveryChallan = new DUDeliveryChallan();
                 _oDUDeliveryChallan.ErrorMessage = ex.Message;
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oDUDeliveryChallan);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }

        #endregion

        #region Search
         [HttpPost]
        public JsonResult AdvSearch(DUDeliveryChallan oDUDeliveryChallan)
        {
            _oDUDeliveryChallans = new List<DUDeliveryChallan>();
            try
            {
                string sSQL = MakeSQL(oDUDeliveryChallan);
                _oDUDeliveryChallans = DUDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryChallans = new List<DUDeliveryChallan>();
                //_oDUDeliveryChallan.ErrorMessage = ex.Message;
                //_oDUDeliveryChallans.Add(_oDUDeliveryChallan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(DUDeliveryChallan oDUDeliveryChallan)
        {
            string sParams = oDUDeliveryChallan.Note;

            int nCboDODate = 0;
            DateTime dFromDODate = DateTime.Today;
            DateTime dToDODate = DateTime.Today;
            int nCboChallanDate = 0;
            DateTime dFromChallanDate = DateTime.Today;
            DateTime dToChallanDate = DateTime.Today;
          
            int nOrderType = 0;
            string sProductIDs = "";
            string sPINo = "";
            string sInvoiceNo = "";
            string sSONo = "";
            string sChallanNo = "";
            string sLotNo = "";
            int nBUID = 0;

            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            string sIssueStoreIDs = "";
            oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(nBUID, EnumModuleName.DUDeliveryChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            sIssueStoreIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).ToList());
            //if (string.IsNullOrEmpty(sIssueStoreIDs))
            //    sIssueStoreIDs = "0";

            if (!string.IsNullOrEmpty(sParams))
            {
                string sTemp = "";
                _oDUDeliveryChallan.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                sProductIDs = Convert.ToString(sParams.Split('~')[1]);
                nCboChallanDate = Convert.ToInt32(sParams.Split('~')[2]);
                sTemp = Convert.ToString(sParams.Split('~')[3]);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    dFromChallanDate = Convert.ToDateTime(sParams.Split('~')[3]);
                    dToChallanDate = Convert.ToDateTime(sParams.Split('~')[4]);
                }

                nCboDODate = Convert.ToInt32(sParams.Split('~')[5]);
                sTemp = Convert.ToString(sParams.Split('~')[6]);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    dFromDODate = Convert.ToDateTime(sParams.Split('~')[6]);
                    dToDODate = Convert.ToDateTime(sParams.Split('~')[7]);
                }

                nOrderType = Convert.ToInt32(sParams.Split('~')[8]);
                sPINo = Convert.ToString(sParams.Split('~')[9]);
                sInvoiceNo = Convert.ToString(sParams.Split('~')[10]);
                sSONo = Convert.ToString(sParams.Split('~')[11]);
                sChallanNo = Convert.ToString(sParams.Split('~')[12]);
                sLotNo = Convert.ToString(sParams.Split('~')[13]);
               /// nBUID = Convert.ToInt32(sParams.Split('~')[13]);
            }


            string sReturn1 = "SELECT * FROM View_DUDeliveryChallan AS DC ";
            string sReturn = "";

            #region sIssueStoreIDs
            if (!String.IsNullOrEmpty(sIssueStoreIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DC.WorkingUnitID in(" + sIssueStoreIDs + ")";
            }
            #endregion

            #region Contractor
            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DC.ContractorID in(" + _oDUDeliveryChallan.ContractorName + ")";
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
            #region Challan Date
            if (nCboChallanDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboChallanDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DC.ChallanDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboChallanDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DC.ChallanDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboChallanDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboChallanDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DC.ChallanDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboChallanDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DC.ChallanDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToChallanDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboChallanDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DC.ChallanDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromChallanDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToChallanDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion
            #region nOrderType
            /// This Searching Critria for Different Window Bulk and Sample
            if (nOrderType > 0)
            {
                if (nOrderType != (int)EnumOrderType.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DC.OrderType="+nOrderType;
                }
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUDeliveryChallanID in (Select DOD.DUDeliveryChallanID from DUDeliveryChallanDetail as DOD where ProductID in (" + sProductIDs + "))";
            }
            #endregion

            #region Po No
            if (!string.IsNullOrEmpty(sSONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DC.DUDeliveryChallanID in ( Select DUDeliveryChallanID from View_DUDeliveryChallanDetail where  OrderNo Like  '%" + sSONo + "%')";
            }
            #endregion

            #region Invoice No
            if (!string.IsNullOrEmpty(sInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType not in (" + ((int)EnumOrderType.BulkOrder).ToString() + "," + ((int)EnumOrderType.DyeingOnly).ToString() + ") and  DC.DUDeliveryChallanID in ( Select DCD.DUDeliveryChallanID from DUDeliveryChallanDetail as DCD where OrderID in  (Select DyeingOrder.DyeingOrderID from DyeingOrder where DyeingOrder.SampleInvoiceID in (Select sampleInvoice.SampleInvoiceID from sampleInvoice where sampleInvoiceNo like '%" + sInvoiceNo + "%' )))";
            }
            #endregion

            #region P/I  No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  DC.DUDeliveryChallanID in (Select DUDeliveryChallanID from DUDeliveryChallanDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingorderDetail where DyeingorderID in (Select DyeingorderID  from Dyeingorder where Dyeingorder.ExportPIID in (Select ExportPIID from exportPI where PINo like '%" + sPINo + "%'))))";
            }
            #endregion
            #region Challan  No
            if (!string.IsNullOrEmpty(sChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ChallanNo Like  '%" + sChallanNo + "%'";
            }
            #endregion
            #region DO No
            if (!String.IsNullOrEmpty(oDUDeliveryChallan.DONos))
            {
                oDUDeliveryChallan.DONos = oDUDeliveryChallan.DONos.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUDeliveryChallanID in ( Select DCD.DUDeliveryChallanID from DUDeliveryChallanDetail as DCD where DONo Like  '%" + oDUDeliveryChallan.DONos + "%')";
            }
            #endregion
            #region Lot No No
            if (!String.IsNullOrEmpty(sLotNo))
            {
                sLotNo = sLotNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUDeliveryChallanID in (Select DCD.DUDeliveryChallanID from View_DUDeliveryChallanDetail AS DCD where DCD.LotNo like  '%" + sLotNo + "%')";
            }
            #endregion
            string sSQL = sReturn1 + " " + sReturn + "";
            return sSQL;
        }
        [HttpPost]
        public JsonResult GetbyNo(DUDeliveryChallan oDUDeliveryChallan)
        {
            _oDUDeliveryChallans = new List<DUDeliveryChallan>();

            try
            {
                string sSQL = "SELECT * FROM View_DUDeliveryChallan ";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oDUDeliveryChallan.ChallanNo))
                {
                    oDUDeliveryChallan.ChallanNo = oDUDeliveryChallan.ChallanNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ChallanNo like '%" + oDUDeliveryChallan.ChallanNo + "%'";
                }
              
                #region DO No
                if (!String.IsNullOrEmpty(oDUDeliveryChallan.DONos))
                {
                    oDUDeliveryChallan.DONos = oDUDeliveryChallan.DONos.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DUDeliveryChallanID in ( Select DCD.DUDeliveryChallanID from view_DUDeliveryChallanDetail as DCD where DONo Like  '%" + oDUDeliveryChallan.DONos + "%')";
                }
                #endregion
                if (oDUDeliveryChallan.OrderType > 0)
                {
                   
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderType=" + oDUDeliveryChallan.OrderType;
                }

                sSQL = sSQL + "" + sReturn;
                _oDUDeliveryChallans = DUDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryChallans = new List<DUDeliveryChallan>();
                //_oDUDeliveryChallan.ErrorMessage = ex.Message;
                //_oDUDeliveryChallans.Add(_oDUDeliveryChallan);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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
        public ActionResult PrintDUDeliveryChallan(int nId, double nts, bool bPrintFormat, int nTitleType)
        {
            string sOrderNo = "";
            string sDONo = "";
            _oDUDeliveryChallan = new DUDeliveryChallan();
            ExportSCDO oExportSCDO = new ExportSCDO();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();

            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();

            try
            {
                if (nId > 0)
                {
                    _oDUDeliveryChallan = DUDeliveryChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   
                    oDUOrderSetup=oDUOrderSetup.GetByType(_oDUDeliveryChallan.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, (int)Session[SessionInfo.currentUserID]);

                    if (_oDUDeliveryChallan.DUDeliveryChallanID > 0)
                    {
                        _oDUDeliveryChallan.DUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        foreach (DUDeliveryChallanDetail oItem in _oDUDeliveryChallan.DUDeliveryChallanDetails)
                        {
                            oItem.Qty = (bPrintFormat == false ? (oItem.Qty * oMeasurementUnitCon.Value) : oItem.Qty);
                            oItem.MUnit = (bPrintFormat == false ? oDUOrderSetup.MUName_Alt : oItem.MUnit);

                            //oItem.Qty = (bPrintFormat == false ? Global.GetKG(oItem.Qty, 10) : oItem.Qty);
                            //oItem.MUnit = (bPrintFormat == false ? oDUOrderSetup.MUName_Alt : oItem.MUnit);

                            //if (sOrderNo != oItem.OrderNo)
                            //{
                            //    _oDUDeliveryChallan.OrderNos = _oDUDeliveryChallan.OrderNos = oItem.PI_SampleNo;
                            //}
                            //sOrderNo = oItem.OrderNo;

                            //if (sDONo != oItem.DONo)
                            //{
                            //    _oDUDeliveryChallan.DONos = _oDUDeliveryChallan.DONos = oItem.DONo;
                            //}
                            //sDONo = oItem.DONo;
                            if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryChallan.OrderType == (int)EnumOrderType.DyeingOnly)
                            {
                                oExportSCDO = oExportSCDO.Get(oItem.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                                _oDUDeliveryChallan.LCNo = oExportSCDO.ExportLCNo;
                                _oDUDeliveryChallan.MKTPName = oExportSCDO.MKTPName;
                                _oDUDeliveryChallan.ContactPersonnelName = oExportSCDO.ContractorContactPersonName;

                               

                            }
                            else  
                            {
                                oDyeingOrder = DyeingOrder.Get(oItem.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                                _oDUDeliveryChallan.OrderNos = oDyeingOrder.OrderNoFull;
                                _oDUDeliveryChallan.ContactPersonnelName = oDyeingOrder.ContactPersonnelName;
                                _oDUDeliveryChallan.MKTPName = oDyeingOrder.MKTPName;
                             
                            }
                           
                        }
                    }
                    if( _oDUDeliveryChallan.DUDeliveryChallanDetails.Count>0)
                    {
                        if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryChallan.OrderType == (int)EnumOrderType.DyeingOnly)
                        {
                            _oDUDeliveryChallan.OrderNos = string.Join(",", _oDUDeliveryChallan.DUDeliveryChallanDetails.Select(x => x.PI_SampleNo).Distinct().ToList());
                        }
                        else
                        {
                            _oDUDeliveryChallan.OrderNos = string.Join(",", _oDUDeliveryChallan.DUDeliveryChallanDetails.Select(x => x.OrderNo).Distinct().ToList());
                        }
                        _oDUDeliveryChallan.DONos = string.Join(",", _oDUDeliveryChallan.DUDeliveryChallanDetails.Select(x => x.DONo).Distinct().ToList()); 
                        //_oDUDeliveryChallan.OrderNos = string.Join(",", _oDUDeliveryChallan.DUDeliveryChallanDetails.Select(x => x.OrderNo).Distinct().ToList());
                        
                        sOrderNo = string.Join(",", _oDUDeliveryChallan.DUDeliveryChallanDetails.Select(x => x.DyeingOrderDetailID).Distinct().ToList());

                        if (!string.IsNullOrEmpty(sOrderNo))
                        {
                            oDyeingOrders = DyeingOrder.Gets("SELECT * FROM View_DyeingOrder where LEN(DeliveryNote) > 1 and DyeingOrderID in (Select Distinct(DyeingOrderID) from DyeingOrderDetail where DyeingOrderDetailID in (" + sOrderNo + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        if (oDyeingOrders.Count > 0)
                        {
                            _oDUDeliveryChallan.DeliveryZone = oDyeingOrders[0].DeliveryNote;
                            _oDUDeliveryChallan.MBuyer = oDyeingOrders[0].MBuyer;
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

            DeliverySetup oDeliverySetup = new DeliverySetup();
            oDeliverySetup = oDeliverySetup.GetByBU(oBusinessUnit.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oDUDeliveryChallan.DeliverySetup = oDeliverySetup;
            _oDUDeliveryChallan.DeliverySetup.Image = GetImage(_oDUDeliveryChallan.DeliverySetup.ImagePad);

            #region ApprovalHead
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.DUDeliveryChallan + "  AND BUID = " + oDeliverySetup.BUID + " Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            if (oDeliverySetup.ChallanPrintNo == EnumExcellColumn.A)
            {
                rptDUDeliveryChallan oReport = new rptDUDeliveryChallan();
                byte[] abytes;
                abytes = oReport.PrepareReport(_oDUDeliveryChallan, oCompany, oBusinessUnit, nTitleType, oDUOrderSetup, oApprovalHeads);
                return File(abytes, "application/pdf");
            }
            else if (oDeliverySetup.ChallanPrintNo == EnumExcellColumn.B)
            {
               
                rptDUDeliveryChallan oReport = new rptDUDeliveryChallan();
                byte[] abytes;
                abytes = oReport.PrepareReportNew(_oDUDeliveryChallan, oCompany, oBusinessUnit, nTitleType, oDUOrderSetup, oApprovalHeads);
                return File(abytes, "application/pdf");
            }
            else 
            {
              
                rptDUDeliveryChallan oReport = new rptDUDeliveryChallan();
                byte[] abytes;
                abytes = oReport.PrepareReport(_oDUDeliveryChallan, oCompany, oBusinessUnit, nTitleType, oDUOrderSetup, oApprovalHeads);
                return File(abytes, "application/pdf");
            }
        }

        public Image GetImage(byte[] Image)
        {
            if (Image != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Image.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(Image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;

            }
            else
            {
                return null;
            }
        }

        public ActionResult PrintDUDeliveryChallanNew(int nId, double nts, bool bPrintFormat, int nTitleType)
        {
            string sOrderNo = "";
            string sDONo = "";
            _oDUDeliveryChallan = new DUDeliveryChallan();
            ExportSCDO oExportSCDO = new ExportSCDO();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            DeliverySetup oDeliverySetup = new DeliverySetup();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            try
            {
                if (nId > 0)
                {
                    _oDUDeliveryChallan = DUDeliveryChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oDUOrderSetup = oDUOrderSetup.GetByType(_oDUDeliveryChallan.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
                    oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, (int)Session[SessionInfo.currentUserID]);

                    if (_oDUDeliveryChallan.DUDeliveryChallanID > 0)
                    {
                        _oDUDeliveryChallan.DUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        foreach (DUDeliveryChallanDetail oItem in _oDUDeliveryChallan.DUDeliveryChallanDetails)
                        {

                            //_oExportPI.ExportPIDetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));
                            oItem.Qty = (bPrintFormat == false ? (oItem.Qty * oMeasurementUnitCon.Value) : oItem.Qty);
                            oItem.MUnit = (bPrintFormat == false ? oDUOrderSetup.MUName_Alt : oItem.MUnit);

                            if (sOrderNo != oItem.OrderNo)
                            {
                                _oDUDeliveryChallan.OrderNos = _oDUDeliveryChallan.OrderNos = oItem.PI_SampleNo;
                            }
                            sOrderNo = oItem.OrderNo;

                            if (sDONo != oItem.DONo)
                            {
                                _oDUDeliveryChallan.DONos = _oDUDeliveryChallan.DONos = oItem.DONo;
                            }
                            sDONo = oItem.DONo;
                            if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryChallan.OrderType == (int)EnumOrderType.DyeingOnly)
                            {
                                oExportSCDO = oExportSCDO.Get(oItem.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                                _oDUDeliveryChallan.LCNo = oExportSCDO.ExportLCNo;
                                _oDUDeliveryChallan.MKTPName = oExportSCDO.MKTPName;
                                _oDUDeliveryChallan.ContactPersonnelName = oExportSCDO.ContractorContactPersonName;
                            }
                            else
                            {
                                oDyeingOrder = DyeingOrder.Get(oItem.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                                _oDUDeliveryChallan.OrderNos = oDyeingOrder.OrderNoFull;
                                _oDUDeliveryChallan.ContactPersonnelName = oDyeingOrder.ContactPersonnelName;
                                _oDUDeliveryChallan.MKTPName = oDyeingOrder.MKTPName;
                            }
                        }
                    }
                }
             
                oDeliverySetup = oDeliverySetup.GetByBU(oBusinessUnit.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUDeliveryChallan.DeliverySetup = oDeliverySetup;
                _oDUDeliveryChallan.DeliverySetup.Image = GetImage(_oDUDeliveryChallan.DeliverySetup.ImagePad);
               
                string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.DUDeliveryChallan + "";//  AND BUID = " + oDeliverySetup.BUID + " Order By Sequence";
                oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptDUDeliveryChallan oReport = new rptDUDeliveryChallan();
            byte[] abytes = oReport.PrepareReport_GatePass(_oDUDeliveryChallan, oCompany, oBusinessUnit, nTitleType, oDUOrderSetup, oApprovalHeads);
            return File(abytes, "application/pdf");
            
            

        }
        #endregion

        #region save Image
        [HttpPost]
        public JsonResult Get(DUDeliveryChallan oDUDeliveryChallan)
        {
            DUDeliveryChallanImage oDUDeliveryChallanImage = new DUDeliveryChallanImage();
            try
            {
                if (oDUDeliveryChallan.DUDeliveryChallanID <= 0) { throw new Exception("Please select a valid Item."); }
                oDUDeliveryChallanImage = DUDeliveryChallanImage.GetByDeliveryChallan(oDUDeliveryChallan.DUDeliveryChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUDeliveryChallanImage.Picture != null)
                {
                    oDUDeliveryChallanImage.ByteInString = "data:image/Jpeg;base64," + Convert.ToBase64String(oDUDeliveryChallanImage.Picture);
                }
            }
            catch (Exception ex)
            {
                oDUDeliveryChallanImage.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDeliveryChallanImage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDUDeliveryChallanImage(double nts)
        {
            string sMessage = "";
            DUDeliveryChallanImage oDUDeliveryChallanImage = new DUDeliveryChallanImage();
            try
            {
                oDUDeliveryChallanImage.DUDeliveryChallanImageID = Convert.ToInt32(Request.Headers["DUDeliveryChallanImageID"]);
                oDUDeliveryChallanImage.DUDeliveryChallanID = Convert.ToInt32(Request.Headers["DUDeliveryChallanID"]);
                oDUDeliveryChallanImage.Name = Request.Headers["Name"].Trim();
                oDUDeliveryChallanImage.ContractNo = Request.Headers["ContractNo"].Trim();
                oDUDeliveryChallanImage.Note = Request.Headers["Note"].Trim();
                oDUDeliveryChallanImage.IsImg = Convert.ToBoolean(Request.Headers["IsImg"]);

                byte[] data;
                #region File
                HttpPostedFileBase file = null;
                if (oDUDeliveryChallanImage.IsImg == true)
                {
                    file = Request.Files[0];
                }
                
                if (file != null)
                {
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                        double nMaxLength = 512 * 1024;
                        if (data.Length > nMaxLength)
                        {
                            throw new Exception("Youe Photo Image " + data.Length / 1024 + "KB! You can selecte maximum 512KB image");
                        }
                    }
                    oDUDeliveryChallanImage.Picture = data;
                }

                //}
                #endregion
                oDUDeliveryChallanImage = oDUDeliveryChallanImage.Save(oDUDeliveryChallanImage, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUDeliveryChallanImage.DUDeliveryChallanImageID > 0 && (oDUDeliveryChallanImage.ErrorMessage == null || oDUDeliveryChallanImage.ErrorMessage == ""))
                {
                    sMessage = "Save Successfully" + "~" + oDUDeliveryChallanImage.DUDeliveryChallanImageID + "~" + oDUDeliveryChallanImage.DUDeliveryChallanID + "~" + oDUDeliveryChallanImage.Name + "~" + oDUDeliveryChallanImage.ContractNo + "~" +
                               oDUDeliveryChallanImage.Note;
                }
                else
                {
                    if ((oDUDeliveryChallanImage.ErrorMessage == null || oDUDeliveryChallanImage.ErrorMessage == "")) { throw new Exception("Unable to Save/Upload."); }
                    else { throw new Exception(oDUDeliveryChallanImage.ErrorMessage); }

                }
            }
            catch (Exception ex)
            {
                oDUDeliveryChallanImage = new DUDeliveryChallanImage();
                oDUDeliveryChallanImage.ErrorMessage = ex.Message;
                sMessage = oDUDeliveryChallanImage.ErrorMessage + "~" + oDUDeliveryChallanImage.DUDeliveryChallanImageID + "~" + oDUDeliveryChallanImage.DUDeliveryChallanID + "~" + oDUDeliveryChallanImage.Name + "~" + oDUDeliveryChallanImage.ContractNo + "~" +
                               oDUDeliveryChallanImage.Note;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delivery Challan Pack
        [HttpPost]
        public JsonResult GetDUDeliveryChallanPacks(DUDeliveryChallanDetail oDCD)
        {
            DUDeliveryChallanPack oDUDeliveryChallanPack = new DUDeliveryChallanPack();
            List<DUDeliveryChallanPack> oDUDeliveryChallanPacks = new List<DUDeliveryChallanPack>();
            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();
            try
            {
                oDUDeliveryChallanPacks = DUDeliveryChallanPack.Gets("SELECT * FROM [View_DUDeliveryChallanPack] WHERE DUDeliveryChallanDetailID = " + oDCD.DUDeliveryChallanDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUDeliveryChallanPacks.Count <= 0)
                {
                    oRouteSheetPackings = RouteSheetPacking.Gets("Select *,(Select SUM(Qty) from DUDeliveryChallanPack where RouteSheetPackingID =RouteSheetPacking.PackingID ) as QtyPack from RouteSheetPacking  where RouteSheetID in (Select ParentID from Lot where  ParentType=106 and LotID=" + oDCD.LotID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oRouteSheetPackings.Count > 0)
                    {
                        oDUDeliveryChallanPacks = new List<DUDeliveryChallanPack>();
                        foreach (RouteSheetPacking oRSP in oRouteSheetPackings)
                        {
                            if ((oRSP.Weight - oRSP.QtyPack) > 0)
                            {
                                oDUDeliveryChallanPack = new DUDeliveryChallanPack();
                                oDUDeliveryChallanPack.DUDeliveryChallanPackID = 0;
                                oDUDeliveryChallanPack.DUDeliveryChallanDetailID = oDCD.DUDeliveryChallanDetailID;
                                oDUDeliveryChallanPack.DUDeliveryChallanID = oDCD.DUDeliveryChallanID;
                                oDUDeliveryChallanPack.RouteSheetPackingID = oRSP.PackingID;
                                oDUDeliveryChallanPack.QTY = oRSP.Weight;
                                oDUDeliveryChallanPack.BagWeight = 1;
                                oDUDeliveryChallanPack.GrossWeight = oRSP.Weight + 1;
                                oDUDeliveryChallanPacks.Add(oDUDeliveryChallanPack);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDeliveryChallanPacks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllRSPacking(DUDeliveryChallanDetail oDCD)
        {
            DUDeliveryChallanPack oDUDeliveryChallanPack = new DUDeliveryChallanPack();
            List<DUDeliveryChallanPack> oDUDeliveryChallanPacks = new List<DUDeliveryChallanPack>();
            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();
            try
            {
                oRouteSheetPackings = RouteSheetPacking.Gets("Select *,(Select SUM(Qty) from DUDeliveryChallanPack where RouteSheetPackingID =RouteSheetPacking.PackingID ) as QtyPack from RouteSheetPacking  where RouteSheetID in (Select ParentID from Lot where  ParentType=106 and LotID=" + oDCD.LotID + ") AND PackingID NOT IN ("+ oDCD.ErrorMessage +")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRouteSheetPackings.Count > 0)
                {
                    oDUDeliveryChallanPacks = new List<DUDeliveryChallanPack>();
                    foreach (RouteSheetPacking oRSP in oRouteSheetPackings)
                    {
                        if ((oRSP.Weight - oRSP.QtyPack) > 0)
                        {
                            oDUDeliveryChallanPack = new DUDeliveryChallanPack();
                            oDUDeliveryChallanPack.DUDeliveryChallanPackID = 0;
                            oDUDeliveryChallanPack.DUDeliveryChallanDetailID = oDCD.DUDeliveryChallanDetailID;
                            oDUDeliveryChallanPack.DUDeliveryChallanID = oDCD.DUDeliveryChallanID;
                            oDUDeliveryChallanPack.RouteSheetPackingID = oRSP.PackingID;
                            oDUDeliveryChallanPack.QTY = oRSP.Weight;
                            oDUDeliveryChallanPack.BagWeight = 1;
                            oDUDeliveryChallanPack.GrossWeight = oRSP.Weight + 1;
                            oDUDeliveryChallanPacks.Add(oDUDeliveryChallanPack);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                oDUDeliveryChallanPack = new DUDeliveryChallanPack();
                oDUDeliveryChallanPack.ErrorMessage = ex.Message;
                oDUDeliveryChallanPacks.Add(oDUDeliveryChallanPack);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDeliveryChallanPacks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePackingDetails(List<DUDeliveryChallanPack> oDUDeliveryChallanPacks)
        {
            List<DUDeliveryChallanPack> _oDUDeliveryChallanPacks = new List<DUDeliveryChallanPack>();
            DUDeliveryChallanPack oDUDeliveryChallanPack = new DUDeliveryChallanPack();
            try
            {
                _oDUDeliveryChallanPacks = oDUDeliveryChallanPack.SavePackingDetails(oDUDeliveryChallanPacks, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDUDeliveryChallanPack = new DUDeliveryChallanPack();
                oDUDeliveryChallanPack.ErrorMessage = ex.Message;
                _oDUDeliveryChallanPacks.Add(oDUDeliveryChallanPack);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryChallanPacks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePackingDetail(DUDeliveryChallanPack oDUDeliveryChallanPack)
        {
            string sMsg = "";
            try
            {
                sMsg = oDUDeliveryChallanPack.Delete(oDUDeliveryChallanPack, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sMsg = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMsg);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPackingList(int nId, bool bPrintFormat, int nTitleType)
        {
            _oDUDeliveryChallan = new DUDeliveryChallan();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            ExportSCDO oExportSCDO = new ExportSCDO();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            string sDONo = "", sOrderNo = "";
            try
            {
                if (nId > 0)
                {
                    _oDUDeliveryChallan = DUDeliveryChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oDUOrderSetup = oDUOrderSetup.GetByType(_oDUDeliveryChallan.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, (int)Session[SessionInfo.currentUserID]);
                    
                    oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUDeliveryChallan.DUDeliveryChallanID > 0)
                    {
                        _oDUDeliveryChallan.DUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oDUDeliveryChallan.DUDeliveryChallanPacks = DUDeliveryChallanPack.Gets("SELECT * FROM [View_DUDeliveryChallanPack] WHERE DUDeliveryChallanDetailID IN (" + string.Join(",", _oDUDeliveryChallan.DUDeliveryChallanDetails.Select(x => x.DUDeliveryChallanDetailID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                        foreach (DUDeliveryChallanDetail oItem in _oDUDeliveryChallan.DUDeliveryChallanDetails)
                        {
                          

                            oItem.Qty = (bPrintFormat == false ? (oItem.Qty * oMeasurementUnitCon.Value) : oItem.Qty);
                            oItem.MUnit = (bPrintFormat == false ? oDUOrderSetup.MUName_Alt : oItem.MUnit);

                            if (sOrderNo != oItem.OrderNo)
                            {
                                _oDUDeliveryChallan.OrderNos = _oDUDeliveryChallan.OrderNos = oItem.PI_SampleNo;
                            }
                            sOrderNo = oItem.OrderNo;

                            if (sDONo != oItem.DONo)
                            {
                                _oDUDeliveryChallan.DONos = _oDUDeliveryChallan.DONos = oItem.DONo;
                            }
                            sDONo = oItem.DONo;
                            if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryChallan.OrderType == (int)EnumOrderType.DyeingOnly)
                            {
                                oExportSCDO = oExportSCDO.Get(oItem.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                                _oDUDeliveryChallan.LCNo = oExportSCDO.ExportLCNo;
                                _oDUDeliveryChallan.MKTPName = oExportSCDO.MKTPName;
                                _oDUDeliveryChallan.ContactPersonnelName = oExportSCDO.ContractorContactPersonName;
                            }
                            else
                            {
                                oDyeingOrder = DyeingOrder.Get(oItem.OrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                                _oDUDeliveryChallan.OrderNos = oDyeingOrder.OrderNoFull;
                                _oDUDeliveryChallan.ContactPersonnelName = oDyeingOrder.ContactPersonnelName;
                                _oDUDeliveryChallan.MKTPName = oDyeingOrder.MKTPName;
                            }

                        }
                        foreach (DUDeliveryChallanPack oItem in _oDUDeliveryChallan.DUDeliveryChallanPacks)
                        {
                            oItem.QTY = (bPrintFormat == false ? (oItem.QTY * oMeasurementUnitCon.Value) : oItem.QTY);
                        }

                        DeliverySetup oDeliverySetup = new DeliverySetup();
                        oDeliverySetup = oDeliverySetup.GetByBU(oBusinessUnit.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        _oDUDeliveryChallan.DeliverySetup = oDeliverySetup;
                        _oDUDeliveryChallan.DeliverySetup.Image = GetImage(_oDUDeliveryChallan.DeliverySetup.ImagePad);
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

            rptDeliveryChallanPackingList oReport = new rptDeliveryChallanPackingList();
            byte[] abytes;
            abytes = oReport.PrepareReport(_oDUDeliveryChallan, oCompany, nTitleType, bPrintFormat);
            return File(abytes, "application/pdf");
        }

        #endregion
    }
}