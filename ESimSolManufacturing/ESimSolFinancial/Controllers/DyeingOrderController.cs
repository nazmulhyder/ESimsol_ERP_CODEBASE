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
    public class DyeingOrderController : Controller
    {
        #region Declaration
        DyeingOrder _oDyeingOrder = new DyeingOrder();
        DyeingOrderDetail _oDyeingOrderDetail = new DyeingOrderDetail();
        List<DyeingOrder> _oDyeingOrders = new List<DyeingOrder>();
        List<DyeingOrderDetail> _oDyeingOrderDetails = new List<DyeingOrderDetail>();
        string _sErrorMessage = "";
        #endregion

        #region DyeingOrder
        public ActionResult ViewDyeingOrders(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrders = new List<DyeingOrder>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            _oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where  [Status]!=" + (int)EnumDyeingOrderState.Cancelled + " and DyeingOrderType in (" + (int)EnumOrderType.BulkOrder + ") and isnull(ApproveBy,0)=0  order by OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.BulkOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups.Add(oDUOrderSetup);
            ViewBag.OrderTypes = oDUOrderSetups;
          
            ViewBag.BUID = buid;
            return View(_oDyeingOrders);
        }
        public ActionResult ViewDyeingOrder(int nId, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
           
            ExportSCDO oExportSCDO = new ExportSCDO();
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (_oDyeingOrder.ExportPIID > 0)
                {
                    oExportSCDO = oExportSCDO.GetByPI(_oDyeingOrder.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDyeingOrder.ExportPINo = oExportSCDO.PINo_Full;
                    _oDyeingOrder.ExportLCNo = oExportSCDO.ELCNo;
                    _oDyeingOrder.OrderValue = oExportSCDO.TotalAmount;
                    _oDyeingOrder.OrderQty = oExportSCDO.TotalQty;
                    _oDyeingOrder.TotalDOQty = oExportSCDO.POQty;
                }
            }

            _oDyeingOrder.DyeingOrderType = (int)EnumOrderType.BulkOrder;

            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();


            oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.BulkOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
         
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
            return View(_oDyeingOrder);
        }
        [HttpGet]
        public ActionResult ViewSaleOrderLayerTwo(int DOID)
        {
            List<DUOrderTracker> oDUOrderTrackers = new List<DUOrderTracker>();
            DUOrderTracker oDUOrderTracker = new DUOrderTracker();
            _oDyeingOrder = new DyeingOrder();
            string Status = "";
            try
            {
                string sSQL = " Where DyeingOrderID = "+DOID;
                oDUOrderTrackers = DUOrderTracker.Gets(sSQL,0, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDyeingOrder = DyeingOrder.Get(DOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                Status = "Order No: " + _oDyeingOrder.OrderNoFull + ", Buyer: " + _oDyeingOrder.ContractorName + ", Order Date: " + _oDyeingOrder.OrderDateSt + ", Mkt Person: " + _oDyeingOrder.CPName;
                ViewBag.Status = Status;
            }
            catch (Exception ex)
            {
                 oDUOrderTracker = new DUOrderTracker();
                 oDUOrderTracker.ErrorMessage = ex.Message;
                 oDUOrderTrackers = new List<DUOrderTracker>();
                 oDUOrderTrackers.Add(oDUOrderTracker);
            }
            return View(oDUOrderTrackers);
        }

        [HttpGet]
        public ActionResult ViewSaleOrderLayerThree(int DODID)
        {
            List<RSFreshDyedYarn> oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            RSFreshDyedYarn oRSFreshDyedYarn = new RSFreshDyedYarn();
            _oDyeingOrderDetail = new DyeingOrderDetail();
            string Status = "";
            try
            {
                string sSQL = " Where DyeingOrderDetailID = " +DODID;
                oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL,0, DateTime.Now, DateTime.Now,1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRSFreshDyedYarns.Count > 0)
                {
                    List<RSInQCSubStatus> oRSInQCSubStatuss = new List<RSInQCSubStatus>();
                    sSQL = string.Join(",", oRSFreshDyedYarns.Select(x => x.RouteSheetID).ToList());
                    oRSInQCSubStatuss = RSInQCSubStatus.Gets("select * from ( SELECT *,ROW_NUMBER() OVER(Partition by RouteSheetID ORDER BY RouteSheetID Asc,DBserverdatetime Desc) as ID FROM View_RSInQCSubStatus WHERE RouteSheetID in (" + sSQL + ")) as TT where TT.ID=1", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oRSFreshDyedYarns.ForEach(x =>
                    {
                        if (oRSInQCSubStatuss.FirstOrDefault() != null && oRSInQCSubStatuss.FirstOrDefault().RouteSheetID > 0 && oRSInQCSubStatuss.Where(b => (b.RouteSheetID == x.RouteSheetID)).Count() > 0)
                        {
                            x.QCDate = oRSInQCSubStatuss.Where(p => (p.RouteSheetID == x.RouteSheetID) && p.RouteSheetID > 0).FirstOrDefault().LastUpdateDateTime;
                            x.RSSubStates = oRSInQCSubStatuss.Where(p => (p.RouteSheetID == x.RouteSheetID) && p.RouteSheetID > 0).FirstOrDefault().RSSubStatus;
                            x.RequestByName = oRSInQCSubStatuss.Where(p => (p.RouteSheetID == x.RouteSheetID) && p.RouteSheetID > 0).FirstOrDefault().UpdateByName;
                            x.RSSubNote = oRSInQCSubStatuss.Where(p => (p.RouteSheetID == x.RouteSheetID) && p.RouteSheetID > 0).FirstOrDefault().Note;
                        }
                    });
                }
                _oDyeingOrderDetail = _oDyeingOrderDetail.Get(DODID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                Status = "Order No: " + _oDyeingOrderDetail.OrderNo + ", Buyer: " + _oDyeingOrderDetail.BuyerName + ", Order Date: " + _oDyeingOrderDetail.OrderDateSt + ", Yarn:" + _oDyeingOrderDetail.ProductName + ",Color:" + _oDyeingOrderDetail.ColorName;
                ViewBag.Status = Status;
            }
            catch (Exception ex)
            {
                oRSFreshDyedYarn = new RSFreshDyedYarn();
                oRSFreshDyedYarn.ErrorMessage = ex.Message;
                oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                oRSFreshDyedYarns.Add(oRSFreshDyedYarn);
            }
            return View(oRSFreshDyedYarns);
        }

        public ActionResult ViewSaleOrderLayerFour(int FSCDID)
        {
            List<DUOrderTracker> oDUOrderTrackers = new List<DUOrderTracker>();
            DUOrderTracker oDUOrderTracker = new DUOrderTracker();
            _oDyeingOrder = new DyeingOrder();
            try
            {
                string sSQL = " WHERE DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE FSCDetailID=" + FSCDID + ")";
                oDUOrderTrackers = DUOrderTracker.Gets(sSQL, 0, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oDyeingOrder = DyeingOrder.Get(DOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //Status = "Order No: " + _oDyeingOrder.OrderNoFull + ", Buyer: " + _oDyeingOrder.ContractorName + ", Order Date: " + _oDyeingOrder.OrderDateSt + ", Mkt Person: " + _oDyeingOrder.CPName;
                //ViewBag.Status = Status;
            }
            catch (Exception ex)
            {
                oDUOrderTracker = new DUOrderTracker();
                oDUOrderTracker.ErrorMessage = ex.Message;
                oDUOrderTrackers = new List<DUOrderTracker>();
                oDUOrderTrackers.Add(oDUOrderTracker);
            }
            return View(oDUOrderTrackers);
        }
        public ActionResult ViewSampleOrders(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DyeingOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oDyeingOrders = new List<DyeingOrder>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            _oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where [Status]!=" + (int)EnumDyeingOrderState.Cancelled + " and DyeingOrderType in (" + (int)EnumOrderType.SampleOrder + ") and isnull(SampleInvoiceID,0)<=0  and  isnull(Approveby,0)=0 order by  Convert(int, [dbo].[SplitedStringGet](PONo, '-', 1)) DESC,OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
         
            oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.SampleOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups.Add(oDUOrderSetup);
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.BUID = buid;
            return View(_oDyeingOrders);
        }
        public ActionResult ViewSampleOrder(int nId, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            //List< > oDUDyeingSteps = new List<DUDyeingStep>();
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.SampleOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
           
            return View(_oDyeingOrder);
        }
        //public ActionResult ViewSampleOrders_Sampling(int buid, int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    DUOrderSetup oDUOrderSetup = new DUOrderSetup();
        //    List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
        //    _oDyeingOrders = new List<DyeingOrder>();
        //    _oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where  [Status]!=" + (int)EnumDyeingOrderState.Cancelled + " and DyeingOrderType in (" + (int)EnumOrderType.Sampling + ") and isnull(SampleInvoiceID,0)<=0  and  isnull(Approveby,0)=0 order by OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
        //    ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.BUID = buid;
        //    ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

        //    oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.Sampling, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oDUOrderSetups.Add(oDUOrderSetup);
        //    ViewBag.OrderTypes = oDUOrderSetups;
        //    ViewBag.BUID = buid;
        //    return View(_oDyeingOrders);
        //}
        public ActionResult ViewSampleOrders_Sampling(bool IsInHouse, int OrderType, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrders = new List<DyeingOrder>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            _oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where  [Status]!=" + (int)EnumDyeingOrderState.Cancelled + " and DyeingOrderType in (" + (int)OrderType + ") and isnull(SampleInvoiceID,0)<=0 and  isnull(Approveby,0)=0 order by OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUOrderSetup.DUOrderSetupID > 0 && oDUOrderSetup.IsApplyDyeingStep == true)
            {
                oDUDyeingSteps = DUDyeingStep.GetsByOrderSetup(oDUOrderSetup.DUOrderSetupID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            oDUOrderSetups.Add(oDUOrderSetup);
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.OrderType = OrderType;
            ViewBag.IsInHouse = IsInHouse;
            ViewBag.BUID = buid;
            return View(_oDyeingOrders);
        }
      
        public ActionResult ViewSampleOrder_Sampling(int nId, bool IsInHouse, int OrderType, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            List<ContractorType> oContractorTypes = new List<ContractorType>();
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDyeingOrder.ContractorID > 0)
                    {
                        oContractorTypes = ContractorType.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            else
            {
                _oDyeingOrder.DyeingOrderType = OrderType;
                _oDyeingOrder.IsInHouse = IsInHouse;
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            //ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUOrderSetup.DUOrderSetupID > 0 && oDUOrderSetup.IsApplyDyeingStep == true)
            {
                oDUDyeingSteps = DUDyeingStep.GetsByOrderSetup(oDUOrderSetup.DUOrderSetupID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oContractorTypes.Count > 0)
            {
                ViewBag.ContractorType = oContractorTypes[0].ContractorTypeID;
            }
            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
            return View(_oDyeingOrder);
        }
        public ActionResult ViewSaleOrders(int buid, bool IsInHouse, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DyeingOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oDyeingOrders = new List<DyeingOrder>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            oDUOrderSetups = DUOrderSetup.GetByOrderTypes(buid,IsInHouse, ((int)EnumOrderType.SampleOrder).ToString() + "," + ((int)EnumOrderType.SampleOrder_Two).ToString() + "," + ((int)EnumOrderType.SaleOrder).ToString() + "," + ((int)EnumOrderType.SaleOrder_Two).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sOrderType = string.Join(",", oDUOrderSetups.Select(x => x.OrderType).ToList());
            if (!string.IsNullOrEmpty(sOrderType))
            {
                _oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where  [Status]!=" + (int)EnumDyeingOrderState.Cancelled + " and DyeingOrderType in (" + sOrderType + ") and isnull(SampleInvoiceID,0)<=0 and  isnull(Approveby,0)=0 order by  Convert(int, [dbo].[SplitedStringGet](PONo, '-', 1)) DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            if (oDUOrderSetups.Count > 0)
            {
                oDUOrderSetup = oDUOrderSetups[0];
            }
            else
            {
                oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.SaleOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oDUOrderSetup.DUOrderSetupID > 0)
            {
                oDUDyeingSteps = DUDyeingStep.GetsByOrderSetup(oDUOrderSetup.DUOrderSetupID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.BUID = buid;
            ViewBag.IsInHouse = IsInHouse;
            return View(_oDyeingOrders);
        }
        public ActionResult ViewSaleOrder(int nId, bool IsInHouse, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            _oDyeingOrder.IsInHouse = IsInHouse;
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = DUOrderSetup.GetByOrderTypes(oBusinessUnit.BusinessUnitID, IsInHouse, ((int)EnumOrderType.SampleOrder).ToString() + "," + ((int)EnumOrderType.SampleOrder_Two).ToString() + "," + ((int)EnumOrderType.SaleOrder).ToString() + "," + ((int)EnumOrderType.SaleOrder_Two).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            //oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.SaleOrder,((User)Session[SessionInfo.CurrentUser]).UserID);
           
            //oDUDyeingSteps = DUDyeingStep.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oDUOrderSetups.Count>0)
            {
                oDUOrderSetup = oDUOrderSetups[0];
            }
        
           oDUDyeingSteps = DUDyeingStep.GetsByOrderSetup(oDUOrderSetup.DUOrderSetupID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);

            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
            return View(_oDyeingOrder);
        }
        public ActionResult ViewSaleOrdersTwo(bool IsInHouse, int OrderType, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrders = new List<DyeingOrder>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            _oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where  [Status]!=" + (int)EnumDyeingOrderState.Cancelled + " and DyeingOrderType in (" + (int)OrderType + ") and isnull(SampleInvoiceID,0)<=0 and  isnull(Approveby,0)=0 order by OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUOrderSetup.DUOrderSetupID > 0 && oDUOrderSetup.IsApplyDyeingStep == true)
            {
                oDUDyeingSteps = DUDyeingStep.GetsByOrderSetup(oDUOrderSetup.DUOrderSetupID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            oDUOrderSetups.Add(oDUOrderSetup);
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.OrderType = OrderType;
            ViewBag.IsInHouse = IsInHouse;
            ViewBag.BUID = buid;
            return View(_oDyeingOrders);
        }
        public ActionResult ViewSaleOrderTwo(int nId, bool IsInHouse, int OrderType, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            else
            {
                _oDyeingOrder.DyeingOrderType = OrderType;
                _oDyeingOrder.IsInHouse = IsInHouse;
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUOrderSetup.DUOrderSetupID > 0 && oDUOrderSetup.IsApplyDyeingStep == true)
            {
                oDUDyeingSteps = DUDyeingStep.GetsByOrderSetup(oDUOrderSetup.DUOrderSetupID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
            return View(_oDyeingOrder);
        }
        public ActionResult ViewDyeingOnlyOrders(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrders = new List<DyeingOrder>();

            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();

            _oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where  [Status]!=" + (int)EnumDyeingOrderState.Cancelled + " and DyeingOrderType in (" + (int)EnumOrderType.DyeingOnly + ") and isnull(ApproveBy,0)=0  order by OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));// Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.DyeingOnly, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups.Add(oDUOrderSetup);
            ViewBag.OrderTypes = oDUOrderSetups;

            ViewBag.BUID = buid;
            return View(_oDyeingOrders);
        }
        public ActionResult ViewDyeingOnlyOrder(int nId, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();

            ExportSCDO oExportSCDO = new ExportSCDO();
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (_oDyeingOrder.ExportPIID > 0)
                {
                    oExportSCDO = oExportSCDO.GetByPI(_oDyeingOrder.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDyeingOrder.ExportPINo = oExportSCDO.PINo_Full;
                    _oDyeingOrder.ExportLCNo = oExportSCDO.ELCNo;
                    _oDyeingOrder.OrderValue = oExportSCDO.TotalAmount;
                    _oDyeingOrder.OrderQty = oExportSCDO.TotalQty;
                    _oDyeingOrder.TotalDOQty = oExportSCDO.POQty;
                }
            }

            _oDyeingOrder.DyeingOrderType = (int)EnumOrderType.DyeingOnly;

            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();


            oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.DyeingOnly, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
            return View(_oDyeingOrder);
        }
        public ActionResult ViewTwistOrders(bool IsInHouse, int OrderType, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrders = new List<DyeingOrder>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            _oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where  [Status]!=" + (int)EnumDyeingOrderState.Cancelled + " and DyeingOrderType in (" + (int)OrderType + ") and isnull(SampleInvoiceID,0)<=0 and  isnull(Approveby,0)=0 order by OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            List<EnumObject> oOrderTypeObjs = new List<EnumObject>();
            List<EnumObject> oOrderTypes = new List<EnumObject>();
            oOrderTypeObjs = EnumObject.jGets(typeof(EnumOrderType));
            oDUOrderSetup = oDUOrderSetup.GetByType((int)OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            
            oDUOrderSetups.Add(oDUOrderSetup);
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.IsInHouse = IsInHouse;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.OrderType = OrderType;
            ViewBag.BUID = buid;
            return View(_oDyeingOrders);
        }
        public ActionResult ViewTwistOrder(int nId,bool IsInHouse, int OrderType, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    /////
                    if (_oDyeingOrder.DyeingOrderDetails.Count > 0)
                    {
                        if (_oDyeingOrder.DyeingOrderDetails.FirstOrDefault() != null && _oDyeingOrder.DyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID > 0 && _oDyeingOrder.DyeingOrderDetails.Where(x => x.BuyerCombo > 0).Count() > 0)
                        {
                            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                            _oDyeingOrder.DyeingOrderDetails.ForEach((item) => { oDyeingOrderDetails.Add(item); });
                            _oDyeingOrder.DyeingOrderDetails = this.TwistedDyeingOrderDetails(_oDyeingOrder.DyeingOrderDetails);
                            _oDyeingOrder.DyeingOrderDetails[0].CellRowSpans = this.RowMerge(oDyeingOrderDetails);
                        }
                    }
                }
            }
            else
            {
                _oDyeingOrder.DyeingOrderType = OrderType;
                _oDyeingOrder.IsInHouse=  IsInHouse;
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
            return View(_oDyeingOrder);
        }
        public ActionResult ViewDyeingOrdersLadDipPending( int buid,int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //isnull(ApproveBy,0)!=0 and
            _oDyeingOrders = new List<DyeingOrder>();
            string sSQL = "Select * from View_DyeingOrder Where  DyeingOrderID In (Select DyeingOrderID from DyeingOrderDetail Where LabDipDetailID<=0  And LabDipType In (2,3,4)) order by OrderNo ";
            _oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));// Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType));
            ViewBag.BUID = buid;
            return View(_oDyeingOrders);
        }
        public ActionResult ViewDyeingOrdersSendToProduction(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrders = new List<DyeingOrder>();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.GetsActive(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where DyeingOrderType in (2,3,4,5,6,7,8) and isnull(ApproveBy,0)!=0 and DyeingOrderID in (Select DyeingOrderDetail.DyeingOrderID from DyeingOrderDetail where LabDipDetailID>0 and Isnull(DyeingOrderDetail.PTUID,0)<=0 and LabDipDetailID in (Select LabDipDetailID from LabDipDetail where LabdipID in  ( SELECT Labdip.LabDipID from Labdip Where OrderStatus in (5,6,7,8,9)))) order by OrderNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDyeingOrders = DyeingOrder.Gets("Select top(120)* from View_DyeingOrder where DyeingOrderType in (2,3,4,5,6,7,8) and isnull(ApproveBy,0)!=0 and DyeingOrderID in (Select DyeingOrderDetail.DyeingOrderID from DyeingOrderDetail where LabDipDetailID>0 and Isnull(DyeingOrderDetail.PTUID,0)<=0 ) order by OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));// Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.OrderTypes = oDUOrderSetups;
           
            return View(_oDyeingOrders);
        }
        public ActionResult ViewDUProGuideLine(int nId, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            DUProGuideLine oDUProGuideLine = new DUProGuideLine();
            List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
            DUProGuideLineDetail oDUProGuideLineDetail = new DUProGuideLineDetail();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDUProGuideLines = DUProGuideLine.Gets("Select top(1)* from view_DUProGuideLine where BUID=" + oBusinessUnit.BusinessUnitID + " and ProductType="+(int)EnumProductNature.Yarn+" and OrderType =" + _oDyeingOrder.DyeingOrderType + " and DyeingOrderID=" + _oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDUProGuideLineDetails = DUProGuideLineDetail.Gets("Select * from View_DUProGuideLineDetail where DUProGuideLineID in (Select DUProGuideLineID from DUProGuideLine where BUID=" + oBusinessUnit.BusinessUnitID + " and ProductType=" + (int)EnumProductNature.Yarn + " and OrderType =" + _oDyeingOrder.DyeingOrderType + " and DyeingOrderID=" + _oDyeingOrder.DyeingOrderID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            oDUProGuideLine.BUID = oBusinessUnit.BusinessUnitID;
            //oDUProGuideLine.ProductTypeInt = (int)EnumProductNature.Yarn;
            oDUProGuideLine.ProductType = EnumProductNature.Yarn;
            //if (oDUProGuideLines.Any() && oDUProGuideLines.FirstOrDefault().DUProGuideLineID>0)
            //{
            //    oDUProGuideLine.DUProGuideLineID = oDUProGuideLines.Where(p => p.ApproveByID <= 0 && p.ReceiveByID <= 0).FirstOrDefault().DUProGuideLineID;
            //}
            foreach( DUProGuideLine oItem in oDUProGuideLines)
            {
                oDUProGuideLine.SLNo = oDUProGuideLine.SLNo +" "+ oItem.SLNo+",";
                oDUProGuideLine.WorkingUnitID = oItem.WorkingUnitID;
                oDUProGuideLine.DUProGuideLineID = oItem.DUProGuideLineID;
                oDUProGuideLine.ReceiveByID = oItem.ReceiveByID;
                oDUProGuideLine.ApproveByID = oItem.ApproveByID;
                oDUProGuideLine.IssueDate = oItem.IssueDate;
                oDUProGuideLine.ReceiveDate = oItem.ReceiveDate;
            }
            if (oDUProGuideLineDetails.Count<=0)
            {
                oDyeingOrderDetails = _oDyeingOrder.DyeingOrderDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.BuyerRef }, (key, grp) =>
                    new DyeingOrderDetail
                    {
                        ProductName = key.ProductName,
                        ProductID = key.ProductID,
                        BuyerRef = key.BuyerRef,
                        Qty = grp.Sum(p => p.Qty),
                        MUnitID = grp.First().MUnitID,
                        MUnit = grp.First().MUnit,

                    }).ToList();
                foreach (DyeingOrderDetail oitem in oDyeingOrderDetails)
                {
                    oDUProGuideLineDetail = new DUProGuideLineDetail();
                    oDUProGuideLineDetail.ProductID = oitem.ProductID;
                    oDUProGuideLineDetail.ProductName = oitem.ProductName;
                    oDUProGuideLineDetail.LotNo = oitem.BuyerRef;
                    oDUProGuideLineDetail.Qty = oitem.Qty;
                    oDUProGuideLineDetail.Qty_Order = oitem.Qty;
                    oDUProGuideLineDetail.MUnitID = oitem.MUnitID;
                    oDUProGuideLineDetail.MUnit = oitem.MUnit;
                    oDUProGuideLineDetails.Add(oDUProGuideLineDetail);
                }
            }

            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            
            oWorkingUnits = WorkingUnit.Gets("Select * from View_WorkingUnit Where IsActive=1 And UnitType=" + (short)EnumWoringUnitType.Raw + " and IsStore=1 And BUID="+oBusinessUnit.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oDUProGuideLine.DUProGuideLineDetails = oDUProGuideLineDetails;
            oDUOrderSetup = oDUOrderSetup.GetByType(_oDyeingOrder.DyeingOrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
         
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BU = oBusinessUnit;
            ViewBag.DUProGuideLine = oDUProGuideLine;
            ViewBag.WorkingUnits = oWorkingUnits;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
          
            return View(_oDyeingOrder);
        }
        public ActionResult ViewOrderView(int nId, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.SaleOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
            return View(_oDyeingOrder);
        }
        public ActionResult ViewFNSaleOrders(int buid, bool IsInHouse, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrders = new List<DyeingOrder>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            oDUOrderSetups = DUOrderSetup.GetByOrderTypes(buid, IsInHouse, ((int)EnumOrderType.SampleOrder).ToString() + "," + ((int)EnumOrderType.SampleOrder_Two).ToString() + "," + ((int)EnumOrderType.SaleOrder).ToString() + "," + ((int)EnumOrderType.SaleOrder_Two).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sOrderType = string.Join(",", oDUOrderSetups.Select(x => x.OrderType).ToList());
            if (!string.IsNullOrEmpty(sOrderType))
            {
                _oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where  [Status]!=" + (int)EnumDyeingOrderState.Cancelled + " and DyeingOrderType in (" + sOrderType + ") and isnull(SampleInvoiceID,0)<=0 and  isnull(Approveby,0)=0 order by  Convert(int, [dbo].[SplitedStringGet](PONo, '-', 1)) DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            if (oDUOrderSetups.Count > 0)
            {
                oDUOrderSetup = oDUOrderSetups[0];
            }
            else
            {
                oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.SaleOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oDUOrderSetup.DUOrderSetupID > 0)
            {
                oDUDyeingSteps = DUDyeingStep.GetsByOrderSetup(oDUOrderSetup.DUOrderSetupID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.BUID = buid;
            ViewBag.IsInHouse = IsInHouse;
            return View(_oDyeingOrders);
        }
        public ActionResult ViewFNSaleOrder(int nId, int nFSCDID, bool IsInHouse, int buid, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            FabricSalesContractDetail oFSCDetail = new FabricSalesContractDetail();
            FabricSalesContract oFSC = new FabricSalesContract();
           
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
             
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.FSCDetailID > 0)
                {
                    oFSCDetail = oFSCDetail.Get(_oDyeingOrder.FSCDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFSC = oFSC.Get(oFSCDetail.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDyeingOrder.FSCNo = oFSC.SCNoFull;
                }
            }
            else
            {
                oFSCDetail = oFSCDetail.Get(nFSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFSCDetail.FabricSalesContractDetailID > 0)
                {
                    _oDyeingOrder = DyeingOrder.GetFSCD(oFSCDetail.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFSC=oFSC.Get(oFSCDetail.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if(_oDyeingOrder.DyeingOrderID <=0)
                {
                    _oDyeingOrder.DyeingOrderID = 0;
                    if (oFSC.OrderType == (int)EnumFabricRequestType.Sample || oFSC.OrderType == (int)EnumFabricRequestType.SampleFOC)
                    {
                        _oDyeingOrder.DyeingOrderType = (int)EnumOrderType.SampleOrder_Two;
                    }
                    else if (oFSC.OrderType == (int)EnumFabricRequestType.Bulk)
                    {
                        _oDyeingOrder.DyeingOrderType = (int)EnumOrderType.SaleOrder_Two;
                    }
                    else
                    {
                        _oDyeingOrder.DyeingOrderType = (int)EnumOrderType.SampleOrder_Two;
                    }

                    _oDyeingOrder.FSCNo = oFSC.SCNoFull;
                    _oDyeingOrder.FSCDetailID = oFSCDetail.FabricSalesContractDetailID;
                    _oDyeingOrder.ContractorID = oFSC.ContractorID;
                    _oDyeingOrder.DeliveryToID = oFSC.BuyerID;
                    _oDyeingOrder.ContractorName = oFSC.ContractorName;
                    _oDyeingOrder.DeliveryToName = oFSC.BuyerName;
                    _oDyeingOrder.BUID = buid;
                    _oDyeingOrder.LightSourchID = oFSC.LightSourceID;
                    _oDyeingOrder.LightSourchIDTwo = oFSC.LightSourceIDTwo;
                    _oDyeingOrder.ContactPersonnelID = oFSC.ContactPersonnelID;
                    _oDyeingOrder.ContactPersonnelID_DelTo = oFSC.ContactPersonnelID_Buyer;
                    _oDyeingOrder.MKTEmpID = oFSC.MktAccountID;
                    _oDyeingOrder.StyleNo = oFSCDetail.StyleNo;
                    _oDyeingOrder.RefNo = oFSCDetail.BuyerReference;
                    _oDyeingOrder.Amount = oFSC.Amount;
                    _oDyeingOrder.Qty = oFSC.Qty;
                    _oDyeingOrder.IsInHouse = true;
                    _oDyeingOrder.OrderDate = DateTime.Now;
                    _oDyeingOrder.MKTPName = oFSC.MKTPName;
                    oLabDipDetails = LabDipDetail.Gets("Select * from View_LabdipDetail Where LabDipID in (Select LabdipID from Labdip where FSCDetailID =" + oFSC .FabricSalesContractID+ ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach(LabDipDetail oitem in oLabDipDetails)
                    {
                        oDyeingOrderDetail = new DyeingOrderDetail();
                        oDyeingOrderDetail.DyeingOrderDetailID =0;
                        oDyeingOrderDetail.DyeingOrderID = _oDyeingOrder.DyeingOrderID;
                        oDyeingOrderDetail.LabDipDetailID = oitem.LabDipDetailID;
                        oDyeingOrderDetail.LabDipType =(int)EnumLabDipType.Normal;
                        oDyeingOrderDetail.Shade = (int)EnumShade.A;
                        oDyeingOrderDetail.ColorName = oitem.ColorName;
                        oDyeingOrderDetail.ColorNo = oitem.ColorNo;
                        oDyeingOrderDetail.PantonNo = oitem.PantonNo;
                        oDyeingOrderDetail.RGB = oitem.RGB;
                        oDyeingOrderDetail.Qty = 0;
                        oDyeingOrderDetail.HankorCone =0;
                        oDyeingOrderDetail.BuyerCombo = 0;
                        oDyeingOrderDetail.BuyerRef = oitem.RefNo;
                        oDyeingOrderDetail.ApproveLotNo ="";
                        oDyeingOrderDetail.LabdipNo = oitem.LabdipNo;
                        oDyeingOrderDetail.LDNo = oitem.LDNo;
                        oDyeingOrderDetail.ColorDevelopProduct = oitem.ProductName;
                        oDyeingOrderDetail.ProductID = oitem.ProductID;
                        oDyeingOrderDetail.ProductName = oitem.ProductName;
                        oDyeingOrderDetail.ProductNameCode = oitem.ProductNameCode;
                        oDyeingOrderDetail.ColorDevelopProduct = oitem.ProductName;
                        oDyeingOrderDetail.ColorDevelopProduct = oitem.ProductName;
                        _oDyeingOrder.DyeingOrderDetails.Add(oDyeingOrderDetail);
                    }

                }
            }
            if (_oDyeingOrder.DyeingOrderID > 0)
            {
                _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = DUOrderSetup.GetByOrderTypes(oBusinessUnit.BusinessUnitID, IsInHouse, ((int)EnumOrderType.SampleOrder).ToString() + "," + ((int)EnumOrderType.SampleOrder_Two).ToString() + "," + ((int)EnumOrderType.SaleOrder).ToString() + "," + ((int)EnumOrderType.SaleOrder_Two).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oDUOrderSetups.Count > 0)
            {
                oDUOrderSetup = oDUOrderSetups[0];
            }
            if (oDUOrderSetup.DUOrderSetupID > 0)
            {
                oDUDyeingSteps = DUDyeingStep.GetsByOrderSetup(oDUOrderSetup.DUOrderSetupID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
            ViewBag.FSCDetail = oFSCDetail;
            return View(_oDyeingOrder);
        }
        public ActionResult ViewDyeingOrderLotAssigns(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrders = new List<DyeingOrder>();
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where DyeingOrderType in (2,3,4,5,6,7,8) and isnull(ApproveBy,0)!=0 and DyeingOrderID in (Select DyeingOrderDetail.DyeingOrderID from DyeingOrderDetail where LabDipDetailID>0 and Isnull(DyeingOrderDetail.PTUID,0)<=0 and LabDipDetailID in (Select LabDipDetailID from LabDipDetail where LabdipID in  ( SELECT Labdip.LabDipID from Labdip Where OrderStatus in (5,6,7,8,9)))) order by OrderNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDyeingOrders = DyeingOrder.Gets("Select top(120)* from View_DyeingOrder where IsInHouse=1 and DyeingOrderID not in (Select DyeingOrderID from LoTParent) order by OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));// Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.OrderTypes = oDUOrderSetups;

            return View(_oDyeingOrders);
        }
        public ActionResult ViewDyeingOrderLotAssign(int nId, double ts)
        {
            _oDyeingOrder = new DyeingOrder();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            List<DUDyeingStep> oDUDyeingSteps = new List<DUDyeingStep>();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            if (nId > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDyeingOrder.DyeingOrderDetails = _oDyeingOrder.DyeingOrderDetails.GroupBy(x => new { x.ProductID, x.ProductName,x.ProductNameCode, x.BuyerRef }, (key, grp) =>
                 new DyeingOrderDetail
                 {
                     ProductName = key.ProductName,
                     ProductNameCode = key.ProductNameCode,
                     ProductID = key.ProductID,
                     BuyerRef = key.BuyerRef,
                     Qty = grp.Sum(p => p.Qty),
                     MUnitID = grp.First().MUnitID,
                     MUnit = grp.First().MUnit,

                 }).ToList();

                  
                    oCPIssueTos = ContactPersonnel.Gets(_oDyeingOrder.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCPDeliveryTos = ContactPersonnel.Gets(_oDyeingOrder.DeliveryToID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            oDUOrderSetup = oDUOrderSetup.GetByType((int)EnumOrderType.SaleOrder, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oMeasurementUnitCon = oMeasurementUnitCon.GetBy(oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUDyeingSteps = DUDyeingStep.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            oDyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BU = oBusinessUnit;
            ViewBag.DUDyeingSteps = oDUDyeingSteps;
            ViewBag.DUOrderSetup = oDUOrderSetup;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.DyeingTypes = oDyeingTypes;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));
            ViewBag.DyeingOrderNotes = oDyeingOrderNotes;
            return View(_oDyeingOrder);
        }
        private bool ValidateInput(DyeingOrder oDyeingOrder)
        {

            string sYear = "";
            string sNo = "";
            bool isNumeric;
            int i;
            string str = "";

            if (!string.IsNullOrEmpty(oDyeingOrder.OrderNo) && (oDyeingOrder.DyeingOrderType == (int)EnumOrderType.BulkOrder || oDyeingOrder.DyeingOrderType == (int)EnumOrderType.DyeingOnly))
            {
                if (oDyeingOrder.OrderNo != null)
                {
                    if (oDyeingOrder.OrderNo.Length < 4 && oDyeingOrder.OrderNo != null)
                    {
                        _sErrorMessage = "Please enter valid Order No";
                        return false;
                    }
                }
                if (oDyeingOrder.OrderNo != null && (oDyeingOrder.DyeingOrderType == (int)EnumOrderType.BulkOrder || oDyeingOrder.DyeingOrderType == (int)EnumOrderType.DyeingOnly))
                {
                    str = oDyeingOrder.OrderNo.Split('-')[1];
                    isNumeric = int.TryParse(str, out i);
                    if (!isNumeric)
                    {
                        _sErrorMessage = "Please enter Numeric Order No";
                        return false;
                    }
                    //str = oDyeingOrder.OrderNo.Split('-')[0];
                    //if (str != "17" && str != "18")
                    //{
                    //    _sErrorMessage = "Please enter valid Year";
                    //    return false;
                    //}
                }
            }
            
            return true;
        }

        [HttpPost]
        public JsonResult Save(DyeingOrder oDyeingOrder)
        {
            ExportSCDO oExportSCDO = new ExportSCDO();
            try
            {
                if (this.ValidateInput(oDyeingOrder))
                {

                    _oDyeingOrder = oDyeingOrder.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDyeingOrder.DyeingOrderID > 0)
                    {
                        _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        if (_oDyeingOrder.ExportPIID > 0 && (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.BulkOrder || _oDyeingOrder.DyeingOrderType == (int)EnumOrderType.DyeingOnly)) /// For Refresh Data (PI Info)  First Time, after Click Save
                        {
                            oExportSCDO = oExportSCDO.GetByPI(_oDyeingOrder.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oDyeingOrder.ExportPINo = oExportSCDO.PINo_Full;
                            _oDyeingOrder.ExportLCNo = oExportSCDO.ELCNo;
                            _oDyeingOrder.OrderValue = oExportSCDO.TotalAmount;
                            _oDyeingOrder.OrderQty = oExportSCDO.TotalQty;
                            _oDyeingOrder.TotalDOQty = oExportSCDO.POQty;
                        }
                        if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.TwistOrder)/// only For TwistingOrder
                        {
                            if (_oDyeingOrder.DyeingOrderDetails.FirstOrDefault() != null && _oDyeingOrder.DyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID > 0 && _oDyeingOrder.DyeingOrderDetails.Where(x => x.BuyerCombo > 0).Count() > 0)
                            {
                                List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                                _oDyeingOrder.DyeingOrderDetails.ForEach((item) => { oDyeingOrderDetails.Add(item); });
                                _oDyeingOrder.DyeingOrderDetails = this.TwistedDyeingOrderDetails(_oDyeingOrder.DyeingOrderDetails);
                                _oDyeingOrder.DyeingOrderDetails[0].CellRowSpans = this.RowMerge(oDyeingOrderDetails);
                            }
                        }


                    }
                }
                else
                {
                    _oDyeingOrder.ErrorMessage=_sErrorMessage;
                }

            }
            catch (Exception ex)
            {
                _oDyeingOrder = new DyeingOrder();
                _oDyeingOrder.ErrorMessage = "Invalid entry Order No";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_Log(DyeingOrder oDyeingOrder)
        {
            ExportSCDO oExportSCDO = new ExportSCDO();
            try
            {
                _oDyeingOrder = oDyeingOrder.Save_Log(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDyeingOrder.DyeingOrderID > 0)
                {
                    _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (_oDyeingOrder.ExportPIID > 0 && (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.BulkOrder || _oDyeingOrder.DyeingOrderType == (int)EnumOrderType.DyeingOnly)) /// For Refresh Data (PI Info)  First Time, after Click Save
                    {
                        oExportSCDO = oExportSCDO.GetByPI(_oDyeingOrder.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        _oDyeingOrder.ExportPINo = oExportSCDO.PINo_Full;
                        _oDyeingOrder.ExportLCNo = oExportSCDO.ELCNo;
                        _oDyeingOrder.OrderValue = oExportSCDO.TotalAmount;
                        _oDyeingOrder.OrderQty = oExportSCDO.TotalQty;
                        _oDyeingOrder.TotalDOQty = oExportSCDO.POQty;
                    }
                    if (_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.TwistOrder)/// only For TwistingOrder
                    {
                        if (_oDyeingOrder.DyeingOrderDetails.FirstOrDefault() != null && _oDyeingOrder.DyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID > 0 && _oDyeingOrder.DyeingOrderDetails.Where(x => x.BuyerCombo > 0).Count() > 0)
                        {
                            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                            _oDyeingOrder.DyeingOrderDetails.ForEach((item) => { oDyeingOrderDetails.Add(item); });
                            _oDyeingOrder.DyeingOrderDetails = this.TwistedDyeingOrderDetails(_oDyeingOrder.DyeingOrderDetails);
                            _oDyeingOrder.DyeingOrderDetails[0].CellRowSpans = this.RowMerge(oDyeingOrderDetails);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                oDyeingOrder = new DyeingOrder();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult Approve(DyeingOrder oDyeingOrder)
        {
            string sErrorMease = "";
            _oDyeingOrder = oDyeingOrder;
            try
            {
                _oDyeingOrder = oDyeingOrder.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DOSave_Auto(DyeingOrder oDyeingOrder)
        {
            string sErrorMease = "";
            _oDyeingOrder = oDyeingOrder;
            try
            {
                _oDyeingOrder = oDyeingOrder.DOSave_Auto(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DOCancel(DyeingOrder oDyeingOrder)
        {
            string sErrorMease = "";
            _oDyeingOrder = oDyeingOrder;
            try
            {
                _oDyeingOrder = oDyeingOrder.DOCancel(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateServisePI(DyeingOrder oDyeingOrder)
        {
            string sErrorMease = "";
            _oDyeingOrder = oDyeingOrder;

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oDyeingOrder.BUID = oBusinessUnit.BusinessUnitID;

            try
            {
                _oDyeingOrder = oDyeingOrder.CreateServisePI(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(DyeingOrder oDyeingOrder)
        {
            try
            {
                if (oDyeingOrder.DyeingOrderID <= 0) { throw new Exception("Please select an valid item."); }
                oDyeingOrder.ErrorMessage = oDyeingOrder.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrder.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
        public JsonResult DeleteDetail(DyeingOrderDetail oDyeingOrderDetail)
        {
            try
            {
                if (oDyeingOrderDetail.DyeingOrderDetailID > 0)
                {
                    oDyeingOrderDetail.ErrorMessage = oDyeingOrderDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDyeingOrderDetail = new DyeingOrderDetail();
                oDyeingOrderDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateInvoice(DyeingOrder oDyeingOrder)
        {
           
            try
            {
                List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
                SampleInvoiceDetail oSampleInvoiceDetail = new SampleInvoiceDetail();

                SampleInvoice oSampleInvoice = new SampleInvoice();
                oSampleInvoice.SampleInvoiceID=0;
                oSampleInvoice.ContractorID = oDyeingOrder.ContractorID;
                oSampleInvoice.ContractorPersopnnalID = oDyeingOrder.ContactPersonnelID;
                oSampleInvoice.CurrencyID = 2;
                oSampleInvoice.PaymentType = oDyeingOrder.PaymentType;
                oSampleInvoice.MKTEmpID = oDyeingOrder.MKTEmpID;
                oSampleInvoice.OrderType = oDyeingOrder.DyeingOrderType;
                oSampleInvoice.PaymentType = oDyeingOrder.PaymentType;
                oSampleInvoice.SampleInvoiceDate = oDyeingOrder.OrderDate;
                oSampleInvoice.BUID = oDyeingOrder.BUID; //Pls change it by BU
                oSampleInvoice.ConversionRate = 78.0; //Pls change it by BU
                oSampleInvoice.InvoiceType = (int)EnumSampleInvoiceType.SampleInvoice;
                oSampleInvoice.InvoiceTypeInt = (int)EnumSampleInvoiceType.SampleInvoice; 
                //foreach (DyeingOrder oitem in oDyeingOrders)
                //{
                    oSampleInvoiceDetail = new SampleInvoiceDetail();
                    oSampleInvoiceDetail.DyeingOrderID = oDyeingOrder.DyeingOrderID;
                    oSampleInvoiceDetails.Add(oSampleInvoiceDetail);
                //}
                oSampleInvoice.SampleInvoiceDetails = oSampleInvoiceDetails;
                oSampleInvoice = oSampleInvoice.SaveFromOrder( ((User)Session[SessionInfo.CurrentUser]).UserID);

                oDyeingOrder.SampleInvoiceID = oSampleInvoice.SampleInvoiceID;
                oDyeingOrder.SampleInvocieNo = oSampleInvoice.SampleInvoiceNo;
                //if (oDyeingOrder.SampleInvocieNo.Length > 0)
                //{
                //    oDyeingOrder.Status =(int)EnumDyeingOrderState.WatingForApprove;
                //}
                oDyeingOrder.ErrorMessage = oSampleInvoice.ErrorMessage;
            }
            catch (Exception ex)
            {

                oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OrderClose(DyeingOrder oDyeingOrder)
        {
            _oDyeingOrder = new DyeingOrder();
            _oDyeingOrder = oDyeingOrder.OrderClose(((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDyeingOrder.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RequestSend(DyeingOrder oDyeingOrder)
        {
            
            _oDyeingOrder = new DyeingOrder();
            _oDyeingOrder = oDyeingOrder.DyeingOrderHistory(((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDyeingOrder.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateMasterBuyer(DyeingOrder oDyeingOrder)
        {
            _oDyeingOrder = new DyeingOrder();
            _oDyeingOrder = oDyeingOrder.UpdateMasterBuyer(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OrderHold(DyeingOrderDetail oDyeingOrderDetail)
        {
            _oDyeingOrderDetail = new DyeingOrderDetail();
            _oDyeingOrderDetail = oDyeingOrderDetail.OrderHold(((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDyeingOrder.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrderDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Merger Row

        private List<CellRowSpan> RowMerge(List<DyeingOrderDetail> oDyeingOrderDetails)
        {

            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[] rowIndex = new int[1];
            int[] rowSpan = new int[1];

            List<DyeingOrderDetail> oTWGLDDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderDetail> oLDDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderDetail> oTempLDDetails = new List<DyeingOrderDetail>();

            oTWGLDDetails = oDyeingOrderDetails.Where(x => x.BuyerCombo > 0).ToList();
            oLDDetails = oDyeingOrderDetails.Where(x => x.BuyerCombo == 0).ToList();

            while (oDyeingOrderDetails.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oDyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID == oTWGLDDetails.FirstOrDefault().DyeingOrderDetailID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.BuyerCombo == oTWGLDDetails.FirstOrDefault().BuyerCombo).ToList();

                    oDyeingOrderDetails.RemoveAll(x => x.BuyerCombo == oTempLDDetails.FirstOrDefault().BuyerCombo);
                    oTWGLDDetails.RemoveAll(x => x.BuyerCombo == oTempLDDetails.FirstOrDefault().BuyerCombo);

                }
                else if (oLDDetails.FirstOrDefault() != null && oDyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID == oLDDetails.FirstOrDefault().DyeingOrderDetailID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.DyeingOrderDetailID == oLDDetails.FirstOrDefault().DyeingOrderDetailID).ToList();

                    oDyeingOrderDetails.RemoveAll(x => x.DyeingOrderDetailID == oTempLDDetails.FirstOrDefault().DyeingOrderDetailID);
                    oLDDetails.RemoveAll(x => x.DyeingOrderDetailID == oTempLDDetails.FirstOrDefault().DyeingOrderDetailID);
                }

                rowIndex[0] = rowIndex[0] + rowSpan[0];
                rowSpan[0] = oTempLDDetails.Count();
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("BuyerCombo", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }
        private List<DyeingOrderDetail> TwistedDyeingOrderDetails(List<DyeingOrderDetail> oDyeingOrderDetails)
        {
            List<DyeingOrderDetail> oTwistedLDDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderDetail> oTWGLDDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderDetail> oLDDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderDetail> oTempLDDetails = new List<DyeingOrderDetail>();

            oTWGLDDetails = oDyeingOrderDetails.Where(x => x.BuyerCombo > 0).ToList();
            oLDDetails = oDyeingOrderDetails.Where(x => x.BuyerCombo == 0).ToList();

            while (oDyeingOrderDetails.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oDyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID == oTWGLDDetails.FirstOrDefault().DyeingOrderDetailID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.BuyerCombo == oTWGLDDetails.FirstOrDefault().BuyerCombo).ToList();
                    oDyeingOrderDetails.RemoveAll(x => x.BuyerCombo == oTempLDDetails.FirstOrDefault().BuyerCombo);
                    oTWGLDDetails.RemoveAll(x => x.BuyerCombo == oTempLDDetails.FirstOrDefault().BuyerCombo);

                }
                else if (oLDDetails.FirstOrDefault() != null && oDyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID == oLDDetails.FirstOrDefault().DyeingOrderDetailID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.DyeingOrderDetailID == oLDDetails.FirstOrDefault().DyeingOrderDetailID).ToList();

                    oDyeingOrderDetails.RemoveAll(x => x.DyeingOrderDetailID == oTempLDDetails.FirstOrDefault().DyeingOrderDetailID);
                    oLDDetails.RemoveAll(x => x.DyeingOrderDetailID == oTempLDDetails.FirstOrDefault().DyeingOrderDetailID);
                }
                oTwistedLDDetails.AddRange(oTempLDDetails);
            }
            return oTwistedLDDetails;
        }
        [HttpPost]
        public JsonResult MakeTwistedGroup(DyeingOrderDetail oDyeingOrderDetail)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            try
            {
                string sDyeingOrderDetailID = string.IsNullOrEmpty(oDyeingOrderDetail.ErrorMessage) ? "" : oDyeingOrderDetail.ErrorMessage;
                if (sDyeingOrderDetailID == "")
                    throw new Exception("No items found to make twisted.");
                if (oDyeingOrderDetail.DyeingOrderID <= 0)
                    throw new Exception("No valid Order found.");

                oDyeingOrderDetails = DyeingOrderDetail.MakeTwistedGroup(sDyeingOrderDetailID, oDyeingOrderDetail.DyeingOrderID, oDyeingOrderDetail.BuyerCombo, (int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDyeingOrderDetails.FirstOrDefault() != null && oDyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID > 0 && oDyeingOrderDetails.Where(x => x.BuyerCombo > 0).Count() > 0)
                {
                    List<DyeingOrderDetail> oTempDyeingOrderDetails = new List<DyeingOrderDetail>();
                    oDyeingOrderDetails.ForEach((item) => { oTempDyeingOrderDetails.Add(item); });
                    oDyeingOrderDetails = this.TwistedDyeingOrderDetails(oDyeingOrderDetails);
                    oDyeingOrderDetails[0].CellRowSpans = this.RowMerge(oTempDyeingOrderDetails);
                }
                oDyeingOrder.DyeingOrderDetails = oDyeingOrderDetails;

            }
            catch (Exception ex)
            {
                oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveTwistedGroup(DyeingOrderDetail oDyeingOrderDetail)
        {
            DyeingOrder oDyeingOrder = new DyeingOrder();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            try
            {
                string sDyeingOrderDetailID = string.IsNullOrEmpty(oDyeingOrderDetail.ErrorMessage) ? "" : oDyeingOrderDetail.ErrorMessage;
                if (sDyeingOrderDetailID == "")
                    throw new Exception("No items found to make twisted.");
                if (oDyeingOrderDetail.DyeingOrderID <= 0)
                    throw new Exception("No valid labdip found.");

                oDyeingOrderDetails = DyeingOrderDetail.MakeTwistedGroup(sDyeingOrderDetailID, oDyeingOrderDetail.DyeingOrderID, oDyeingOrderDetail.BuyerCombo, (int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oDyeingOrderDetails.FirstOrDefault() != null && oDyeingOrderDetails.FirstOrDefault().DyeingOrderDetailID > 0 && oDyeingOrderDetails.Where(x => x.BuyerCombo > 0).Count() > 0)
                {
                    List<DyeingOrderDetail> oTempDyeingOrderDetails = new List<DyeingOrderDetail>();
                    oDyeingOrderDetails.ForEach((item) => { oTempDyeingOrderDetails.Add(item); });
                    oDyeingOrderDetails = this.TwistedDyeingOrderDetails(oDyeingOrderDetails);
                    oDyeingOrderDetails[0].CellRowSpans = this.RowMerge(oTempDyeingOrderDetails);
                }
                oDyeingOrder.DyeingOrderDetails = oDyeingOrderDetails;

            }
            catch (Exception ex)
            {
                oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Send To Production
        [HttpPost]
        public JsonResult DyeingOrderSendToProduction(DyeingOrder oDyeingOrder)
        {
            try
            {
                //_oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where DyeingOrderType in (2,3,4,5,6,7,8) and isnull(ApproveBy,0)!=0 and DyeingOrderID in (Select DyeingOrderDetail.DyeingOrderID from DyeingOrderDetail where LabDipDetailID>0 and Isnull(DyeingOrderDetail.PTUID,0)<=0 ) order by OrderNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDyeingOrder = oDyeingOrder.DyeingOrderSendToProduction(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {

                _oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DyeingOrderDetailSendToProduction(DyeingOrderDetail oDyeingOrderDetail)
        {
            try
            {
                oDyeingOrderDetail.Status = (int)EnumDOState.Running;
                _oDyeingOrderDetail = oDyeingOrderDetail.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDyeingOrderDetail = new DyeingOrderDetail();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrderDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DyeingOrderDetailBackFromProduction(DyeingOrderDetail oDyeingOrderDetail)
        {
            try
            {
                oDyeingOrderDetail.Status = (int)EnumDOState.Cancelled;
                _oDyeingOrderDetail = oDyeingOrderDetail.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDyeingOrderDetail = new DyeingOrderDetail();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrderDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DOPendingSendToProduction(DyeingOrder oDyeingOrder)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            try
            {


                string sSQL = "Select * from View_DyeingOrder where DyeingOrderType in (2,4,3) and isnull(ApproveBy,0)!=0 and DyeingOrderID in (Select DyeingOrderDetail.DyeingOrderID from DyeingOrderDetail where LabDipDetailID>0 and Isnull(DyeingOrderDetail.PTUID,0)<=0 and LabDipDetailID in (Select LabDipDetailID from LabDipDetail where LabdipID in  ( SELECT Labdip.LabDipID from Labdip Where OrderStatus in (5,6,7,8,9)))) order by OrderNo";

                oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Labdip By Dyeing Order

        [HttpPost]
        public JsonResult DOPendingLabDipReq(DyeingOrder oDyeingOrder)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            try
            {
                //string sInvoiceNo = (!string.IsNullOrEmpty(oDyeingOrder.SampleInvocieNo)) ? oDyeingOrder.SampleInvocieNo.Trim() : "";
                //string sSQL = "Select * from View_DyeingOrder Where ContractorID=" + oDyeingOrder.ContractorID + " And DyeingOrderID In ( Select DyeingOrderID from SampleInvoiceDetail Where " +
                //            " DyeingOrderID In (Select DyeingOrderID from DyeingOrder Where PaymentType=" + (int)EnumOrderPaymentType.Adjustwith_LC + " And ExportPIID=0 " + ((!string.IsNullOrEmpty(oDyeingOrder.Params)) ? " And DyeingOrderID Not In (" + oDyeingOrder.Params + ") " : "") + " )" +
                //            " And SampleInvoiceID In (Select SampleInvoiceID from SampleInvoice Where PaymentType= " + (int)EnumOrderPaymentType.Adjustwith_LC + " " + ((sInvoiceNo != "") ? " And SampleInvoiceNo Like '%" + sInvoiceNo + "%'" : "") + " ))";


                string sSQL = "Select * from View_DyeingOrder Where isnull(ApproveBy,0)!=0 and DyeingOrderID In (Select DyeingOrderID from DyeingOrderDetail Where LabDipDetailID<=0  And LabDipType In (2,3)) order by OrderNo";

                oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DOPendingLabDipDone(DyeingOrder oDyeingOrder)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            try
            {
                string sSQL = "Select * from View_DyeingOrder Where isnull(ApproveBy,0)!=0 and DyeingOrderID In (Select DyeingOrderID from DyeingOrderDetail Where " +
                              "LabDipDetailID>0  And LabDipType In (2,3)) And DyeingOrderID In (Select OrderReferenceID from Labdip Where OrderStatus<=4 And OrderReferenceType != 1) order by OrderNo";
                oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLabdipByDO(DyeingOrder oDyeingOrder)
        {
            LabDip oLabDip = new LabDip();
            try
            {
                if (oDyeingOrder.DyeingOrderID <= 0) { throw new Exception("Please select a valid item for list."); }
                oLabDip = LabDip.CreateLabdipByDO(oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDyeingOrder = DyeingOrder.Get(oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (!string.IsNullOrEmpty(oLabDip.ErrorMessage))
                {
                    _oDyeingOrder.ErrorMessage = oLabDip.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                oLabDip = new LabDip();
                oLabDip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult ContractorSearchByNameType(Contractor oContractor) 
        {
            List<Contractor> oContractors = new List<Contractor>();
            try
            {
                string sType = oContractor.Params.Split('~')[0];
                string sName = oContractor.Params.Split('~')[1].Trim();
                int nBUID = 0;
                if (oContractor.Params.Split('~').Count() > 2)
                {
                    nBUID = Convert.ToInt32(oContractor.Params.Split('~')[2]);
                }
                /// Specialy for RnD  Order for Production
                oContractors = Contractor.Gets("SELECT * FROM Contractor where ContractorID in (1927,2093)", (int)Session[SessionInfo.currentUserID]);
              
                if (oContractors.Count <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                oContractor = new Contractor();
                oContractor.ErrorMessage = ex.Message;
                oContractors.Add(oContractor);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDyeingOrderByInvoice(DyeingOrder oDyeingOrder)
       {
           string sSQL = "";
           
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
           try
           {
               string sInvoiceNo = (!string.IsNullOrEmpty(oDyeingOrder.SampleInvocieNo)) ? oDyeingOrder.SampleInvocieNo.Trim() : "";
               if (string.IsNullOrEmpty(oDyeingOrder.SampleInvocieNo))
               {
                   sSQL = "Select * from View_DyeingOrder Where Isnull(SampleInvoiceID,0)>0 and  ContractorID=" + oDyeingOrder.ContractorID + " And DyeingOrderID In ( Select DyeingOrderID from SampleInvoiceDetail Where " +
                              " DyeingOrderID In (Select DyeingOrderID from DyeingOrder Where PaymentType in(" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + ") And ExportPIID=0 " + ((!string.IsNullOrEmpty(oDyeingOrder.Params)) ? " And DyeingOrderID Not In (" + oDyeingOrder.Params + ") " : "") + " )" +
                              " And SampleInvoiceID In (Select SampleInvoiceID from SampleInvoice Where PaymentType in(" + (int)EnumOrderPaymentType.AdjWithNextLC + "," +(int)EnumOrderPaymentType.AdjWithPI +") " + ((sInvoiceNo != "") ? " And SampleInvoiceNo Like '%" + sInvoiceNo + "%'" : "") + " ))";
               }
               else
               {
                   sSQL = "Select * from View_DyeingOrder Where Isnull(SampleInvoiceID,0)>0  And DyeingOrderID In ( Select DyeingOrderID from SampleInvoiceDetail Where " +
                           " DyeingOrderID In (Select DyeingOrderID from DyeingOrder Where PaymentType in(" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + ") And ExportPIID=0 " + ((!string.IsNullOrEmpty(oDyeingOrder.Params)) ? " And DyeingOrderID Not In (" + oDyeingOrder.Params + ") " : "") + " )" +
                           " And SampleInvoiceID In (Select SampleInvoiceID from SampleInvoice Where PaymentType in(" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + ") " + ((sInvoiceNo != "") ? " And SampleInvoiceNo Like '%" + sInvoiceNo + "%'" : "") + " ))";
          
               }
               oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
           }
           catch (Exception ex)
           {
               oDyeingOrder = new DyeingOrder();
               oDyeingOrder.ErrorMessage = ex.Message;

               oDyeingOrders = new List<DyeingOrder>();
               oDyeingOrders.Add(oDyeingOrder);
           }
           JavaScriptSerializer serializer = new JavaScriptSerializer();
           string sjson = serializer.Serialize(oDyeingOrders);
           return Json(sjson, JsonRequestBehavior.AllowGet);
       }
       
        [HttpPost]
        public JsonResult SaveDyeingOrderAdjustmentForExportPI(DyeingOrder oDyeingOrder)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            try
            {
                string sDyeingOrderID = oDyeingOrder.Params.Split('~')[0];
                int nExportPIID = Convert.ToInt32(oDyeingOrder.Params.Split('~')[1]);
                oDyeingOrders = DyeingOrder.DyeingOrderAdjustmentForExportPI(sDyeingOrderID, nExportPIID, (int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDyeingOrders.Count() > 0)
                {
                    oDyeingOrder = new DyeingOrder();
                    oDyeingOrder.DyeingOrders = oDyeingOrders;
                }
            }
            catch (Exception ex)
            {
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDyeingOrderAdjustmentForExportPI(DyeingOrder oDyeingOrder)

        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            try
            {
                oDyeingOrders = DyeingOrder.DyeingOrderAdjustmentForExportPI(oDyeingOrder.DyeingOrderID.ToString(), oDyeingOrder.ExportPIID, (int)EnumDBOperation.Delete ,((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDyeingOrders.Count() >0)
                {
                    oDyeingOrder = new DyeingOrder();
                    oDyeingOrder.ErrorMessage = oDyeingOrders.FirstOrDefault().ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrder.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                //if (oImportPI.ProductType > 0)
                //{
                oProducts = Product.Gets_Import(oProduct.ProductName, oProduct.BUID, (int)EnumProductNature.Yarn, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
                //else
                //{

                //    if (oImportPI.ErrorMessage != null && oImportPI.ErrorMessage != "")
                //    {
                //        oProducts = Product.GetsPermittedProductByNameCode(oImportPI.BUID, EnumModuleName.ImportPI, EnumProductUsages.Regular, oImportPI.ErrorMessage, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    }
                //    else
                //    {
                //        oProducts = Product.GetsPermittedProduct(oImportPI.BUID, EnumModuleName.ImportPI, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //    }
                //}

            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
      

        public ActionResult PrintDyeingOrder(int nId, bool bIsPrintHistory,  double nts)
        {
            _oDyeingOrder = new DyeingOrder();
            ExportSCDO _oExportSCDO = new ExportSCDO();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            //oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<DyeingOrderDetail> oDyeingOrderDetails_PreviousTemp = new List<DyeingOrderDetail>();
            List<DyeingOrderDetail> oDyeingOrderDetails_Previous = new List<DyeingOrderDetail>();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            DUDyeingStep oDUDyeingStep = new DUDyeingStep();
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            List<DUColorCombo> oDUColorCombos_Group = new List<DUColorCombo>();
            List<DUColorCombo> oDUColorCombos = new List<DUColorCombo>();
            LightSource oLightSource = new LightSource();
            try
            {
                if (nId > 0)
                {
                    _oDyeingOrder = DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oDUOrderSetup = oDUOrderSetup.GetByType(_oDyeingOrder.DyeingOrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDUDyeingStep = oDUDyeingStep.GetBy(_oDyeingOrder.DyeingStepTypeInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oBusinessUnit = oBusinessUnit.Get(oDUOrderSetup.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDyeingOrder.DyeingOrderID > 0)
                    {
                        _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        if ((_oDyeingOrder.DyeingOrderType ==(int)EnumOrderType.DyeingOnly  ||_oDyeingOrder.DyeingOrderType ==(int)EnumOrderType.BulkOrder) && _oDyeingOrder.ExportPIID>0)
                        {
                            _oExportSCDO = _oExportSCDO.GetByPI(_oDyeingOrder.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oDyeingOrder.ExportPINo = _oExportSCDO.PINo_Full;
                            _oDyeingOrder.ExportLCNo = _oExportSCDO.ExportLCNo;
                            _oDyeingOrder.OrderQty = _oExportSCDO.TotalQty;
                        }
                        if (_oDyeingOrder.ExportPIID > 0 && bIsPrintHistory==true)
                        {
                            _oDyeingOrders = DyeingOrder.GetsByPI(_oDyeingOrder.ExportPIID,((User)Session[SessionInfo.CurrentUser]).UserID);
                            if (_oDyeingOrders.Count > 0)
                            {
                                oDyeingOrderDetails_PreviousTemp = DyeingOrderDetail.Gets("SELECT * FROM View_DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where [Status]!="+(int)EnumDyeingOrderState.Cancelled+" and  DyeingOrder.ExportPIID=" + _oDyeingOrder.ExportPIID + " and DyeingOrderID!=" + nId + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                                List<DyeingOrderDetail> oDODetails = new List<DyeingOrderDetail>();
                                _oExportSCDO.POQty = 0;
                                foreach (DyeingOrder oItemDO in _oDyeingOrders)
                                {
                                    if (oItemDO.DyeingOrderType == (int)EnumOrderType.SampleOrder)
                                    {
                                        oDODetails = oDyeingOrderDetails_PreviousTemp.Where(o => o.DyeingOrderID == oItemDO.DyeingOrderID).ToList();
                                        _oExportSCDO.POQty = _oExportSCDO.POQty + oDODetails.Select(c => c.Qty).Sum();
                                    }
                                }
                                
                                
                                foreach (DyeingOrder oItem in _oDyeingOrders)
                                {
                                    List<DyeingOrderDetail> oDODetailsTwo = new List<DyeingOrderDetail>();
                                    oDODetailsTwo = oDyeingOrderDetails_PreviousTemp.Where(o => o.DyeingOrderID == oItem.DyeingOrderID).ToList();
                                    foreach (DyeingOrderDetail oItemDOD in oDODetailsTwo)
                                    {
                                        oItemDOD.OrderNo = oItem.OrderNoFull;
                                        //oItemDOD.OrderDate = oItem.OrderDateSt;
                                        oDyeingOrderDetails_Previous.Add(oItemDOD);
                                    }
                                }
                            }
                        }
                        oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        oDUColorCombos = DUColorCombo.Gets(_oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (oDUColorCombos.Count > 0)
                        {
                            oDUColorCombos_Group = oDUColorCombos.GroupBy(x => new { x.ComboID, x.DyeingOrderID }, (key, grp) =>
                                   new DUColorCombo
                                   {
                                       ComboID = key.ComboID,
                                       DyeingOrderID = key.DyeingOrderID
                                   }).ToList();


                            foreach (DUColorCombo oitem in oDUColorCombos_Group)
                            {
                                foreach (DUColorCombo oitemTwo in oDUColorCombos)
                                {
                                    if (oitemTwo.ComboID == oitem.ComboID)
                                    {
                                        if (string.IsNullOrEmpty(oitem.ColorName))
                                        {
                                            oitem.ColorName = oitemTwo.ColorName;
                                        }
                                        else
                                        {
                                            oitem.ColorName = oitem.ColorName + " + " + oitemTwo.ColorName;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ContactPersonnel oContactPersonnel = new ContactPersonnel();
                    oContactPersonnel = oContactPersonnel.Get(_oDyeingOrder.ContactPersonnelID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDyeingOrder.ContactPersonnelName = oContactPersonnel.Name;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if ((_oDyeingOrder.DyeingOrderType == (int)EnumOrderType.SaleOrder_Two || _oDyeingOrder.DyeingOrderType == (int)EnumOrderType.SaleOrder) && oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving )
            {
                FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();
                List<FabricExecutionOrderSpecification> oFEOSs = new List<FabricExecutionOrderSpecification>();
                List<FabricDeliverySchedule> oFebricDeliverySchedules = new List<FabricDeliverySchedule>();
                List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
                List<FabricSpecificationNote> oFabricSpecificationNotes = new List<FabricSpecificationNote>();
                List<FabricDyeingRecipe> oFabricDyeingRecipes = new List<FabricDyeingRecipe>();
                FabricSalesContract oFSC = new FabricSalesContract();
                Contractor oContractor = new Contractor();
                Fabric oFabric = new Fabric();
                List<FabricDispo> oFabricDispos = new List<FabricDispo>();
                FabricSCReport oFabricSCReport = new FabricSCReport();
                oFEOSs = FabricExecutionOrderSpecification.Gets("Select * from View_FabricExecutionOrderSpecification where FEOSID in (Select FEOSID from DyeingOrderFabric where DyeingOrderID=" + nId + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFEOSs.Count > 0)
                {
                    oFEOS.FEOSID = 0;
                    oFEOS = oFEOSs[0];
                    oFEOS.FEOSDetails = FabricExecutionOrderSpecificationDetail.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    string sSQL = "SELECT * FROM View_FabricSalesContractReport Where FabricSalesContractDetailID =" + oFEOS.FSCDID;
                    var result = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (result.Any() && result.First().FabricSalesContractDetailID > 0)
                    {
                        oFabricSCReport = result.First();
                        oFSC = oFSC.Get(oFabricSCReport.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    oFabricDispos = FabricDispo.Gets("SELECT * FROM FabricDispo where FabricDispo.FabricOrderType=" + oFSC.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFebricDeliverySchedules = FabricDeliverySchedule.GetsFSCID(oFabricSCReport.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricSalesContractNotes = FabricSalesContractNote.Gets(oFabricSCReport.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oFabricSpecificationNotes = FabricSpecificationNote.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                 
                    if (oFEOS.IsOutSide && oFEOS.ContractorID > 0) // if Out side 
                    {
                        oContractor = oContractor.Get(oFEOS.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
             
                rptFabricSpecification oReport = new rptFabricSpecification();
                byte[] abytes = oReport.PrepareReport(oFEOS, oFabricSCReport, oFSC, oFabric, oCompany, oFebricDeliverySchedules, oFabricSalesContractNotes, oFabricSpecificationNotes, oFabricDispos, oFabricDyeingRecipes, oContractor);

                return File(abytes, "application/pdf");

            }
            else
            {
                if (oDUOrderSetup.PrintNo == (int)EnumExcellColumn.A)
                {
                    rptDyeingOrder oReport = new rptDyeingOrder();
                    byte[] abytes = oReport.PrepareReport(_oDyeingOrder, oCompany, oBusinessUnit, oDyeingOrderDetails_Previous, _oExportSCDO, oDUOrderSetup, oDyeingOrderNotes);
                    return File(abytes, "application/pdf");
                }
                else if (oDUOrderSetup.PrintNo == (int)EnumExcellColumn.B)
                {
                    oLightSource = new LightSource();
                    if (_oDyeingOrder.LightSourchID > 0)
                    {
                        oLightSource = oLightSource.Get(_oDyeingOrder.LightSourchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    if (_oDyeingOrder.LightSourchIDTwo > 0)
                    {
                        oLightSource.NameTwo = oLightSource.Get(_oDyeingOrder.LightSourchIDTwo, ((User)Session[SessionInfo.CurrentUser]).UserID).Descriptions;
                    }

                    rptDyeingOrder oReport = new rptDyeingOrder();
                    byte[] abytes = oReport.PrepareReport_B(_oDyeingOrder, oCompany, oBusinessUnit, oDUOrderSetup, oDUDyeingStep, oDyeingOrderNotes, oDUColorCombos_Group, oLightSource);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptDyeingOrder oReport = new rptDyeingOrder();
                    byte[] abytes = oReport.PrepareReport(_oDyeingOrder, oCompany, oBusinessUnit, oDyeingOrderDetails_Previous, _oExportSCDO, oDUOrderSetup, oDyeingOrderNotes);
                    return File(abytes, "application/pdf");
                }
            }
            

        }
        #region Search
        [HttpPost]
        public JsonResult AdvSearch(DyeingOrder oDyeingOrder)
        {
            _oDyeingOrders = new List<DyeingOrder>();
            try
            {
                string sSQL = MakeSQL(oDyeingOrder);
                _oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDyeingOrders = new List<DyeingOrder>();
                //_oDyeingOrder.ErrorMessage = ex.Message;
                //_oDyeingOrders.Add(_oDyeingOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(DyeingOrder oDyeingOrder)
        {
            string sParams = oDyeingOrder.Note;

            int nCboOrderDate = 0;
            DateTime dFromOrderDate = DateTime.Today;
            DateTime dToOrderDate = DateTime.Today;
            int nCboInvoiceDate = 0;
            DateTime dFromInvoiceDate = DateTime.Today;
            DateTime dToInvoiceDate = DateTime.Today;
            int nCboDeliveryDate = 0;
            DateTime dFromDeliveryDate = DateTime.Today;
            DateTime dToDeliveryDate = DateTime.Today;
        
            int nCboMkPerson = 0;
            int nPaymentType = 0;
            string sOrderType = "";
          
            string sProductIDs = "";
         
            string sPINo = "";
            string sInvoiceNo = "";
            string sStyleNo = "";
            string sRefNo = "";
            string sMatchNo = "";
            bool bYetNotSendLabReq = false;
            bool bYetNotSendPro = false;
            int nOrderTypeFixec = 0;
            int nBUID = 0;
            int nReceiveType = 0;
            int nBookingStatus= 0;
            

            if (!string.IsNullOrEmpty(sParams))
            {
                _oDyeingOrder.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                _oDyeingOrder.DeliveryToName = Convert.ToString(sParams.Split('~')[1]);
               
                nCboOrderDate = Convert.ToInt32(sParams.Split('~')[2]);
                dFromOrderDate = Convert.ToDateTime(sParams.Split('~')[3]);
                dToOrderDate = Convert.ToDateTime(sParams.Split('~')[4]);
                nCboInvoiceDate = Convert.ToInt32(sParams.Split('~')[5]);
                dFromInvoiceDate = Convert.ToDateTime(sParams.Split('~')[6]);
                dToInvoiceDate = Convert.ToDateTime(sParams.Split('~')[7]);
                nCboDeliveryDate = Convert.ToInt32(sParams.Split('~')[8]);
                dFromDeliveryDate = Convert.ToDateTime(sParams.Split('~')[9]);
                dToDeliveryDate = Convert.ToDateTime(sParams.Split('~')[10]);

                nCboMkPerson = Convert.ToInt32(sParams.Split('~')[11]);
                nPaymentType = Convert.ToInt32(sParams.Split('~')[12]);
                sOrderType = Convert.ToString(sParams.Split('~')[13]);

                sPINo = Convert.ToString(sParams.Split('~')[14]);
                sInvoiceNo = Convert.ToString(sParams.Split('~')[15]);
                sStyleNo = Convert.ToString(sParams.Split('~')[16]);
                sRefNo = Convert.ToString(sParams.Split('~')[17]);

                sProductIDs = Convert.ToString(sParams.Split('~')[18]);

                bYetNotSendLabReq = Convert.ToBoolean(sParams.Split('~')[19]);
                bYetNotSendPro = Convert.ToBoolean(sParams.Split('~')[20]);

                nOrderTypeFixec = Convert.ToInt16(sParams.Split('~')[21]);

                nBUID = Convert.ToInt32(sParams.Split('~')[22]);
                
                   
                if (sParams.Split('~').Length > 23)
                    sMatchNo = sParams.Split('~')[23];
             
                if (sParams.Split('~').Length > 24)
                    int.TryParse(sParams.Split('~')[24], out nReceiveType);
                if (sParams.Split('~').Length > 25)
                    int.TryParse(sParams.Split('~')[25], out nBookingStatus);

            }


            string sReturn1 = "SELECT * FROM View_DyeingOrder AS DO ";
            string sReturn = "";

            #region Contractor
            if (!String.IsNullOrEmpty(_oDyeingOrder.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.ContractorID in(" + _oDyeingOrder.ContractorName + ")";
            }
            #endregion

            #region Buyer Id
            if (!String.IsNullOrEmpty(_oDyeingOrder.DeliveryToName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.DeliveryToID in(" + _oDyeingOrder.DeliveryToName + ")";
            }
            #endregion

            #region OrderDate Date
            if (nCboOrderDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboOrderDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboOrderDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboOrderDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboOrderDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboOrderDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.OrderDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboOrderDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Sample Invoice Date
            if (nCboInvoiceDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
               
                if (nCboInvoiceDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoice where  CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) )";
                }
              
                else if (nCboInvoiceDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoice where CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (nCboInvoiceDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoice where CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  )";
                }

                else if (nCboInvoiceDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoice where CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboInvoiceDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoice where  CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboInvoiceDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoice where  CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106))  )";
                }
              
            }
            #endregion

            #region Delivery Date
            if (nCboDeliveryDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (nCboDeliveryDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) )";
                }

                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }

                else if (nCboDeliveryDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }

            }
            #endregion

            #region nOrderTypeFixec
            /// This Searching Critria for Different Window Bulk and Sample
            if (nOrderTypeFixec > 0)
            {
                if (nOrderTypeFixec == (int)EnumOrderType.BulkOrder)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DO.DyeingOrderType in ("+ (int)EnumOrderType.BulkOrder + ")";
                }
                if (nOrderTypeFixec == (int)EnumOrderType.SampleOrder)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DO.DyeingOrderType in (" + (int)EnumOrderType.SampleOrder + ")";
                }
                if (nOrderTypeFixec == (int)EnumOrderType.SaleOrder)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DO.DyeingOrderType in (" + (int)EnumOrderType.SaleOrder + ")";
                }
                if (nOrderTypeFixec == (int)EnumOrderType.DyeingOnly)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DO.DyeingOrderType in (" + (int)EnumOrderType.DyeingOnly + ")";
                }
                if (nOrderTypeFixec == (int)EnumOrderType.Sampling)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DO.DyeingOrderType in (" + nOrderTypeFixec + ")";
                }
            }
            #endregion

            #region nPayment Type
            if (nPaymentType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.PaymentType = " + nPaymentType;
            }
            #endregion
            #region Mkt. Person
            if (nCboMkPerson > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.MKTEmpID = " + nCboMkPerson;
            }
            #endregion
            #region Order Type
            if (!string.IsNullOrEmpty(sOrderType))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.DyeingOrderType in (" + sOrderType + ")";
            }
            #endregion
           
            #region Yet Not Send Lab Requist
            if (bYetNotSendLabReq)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " isnull(ApproveBy,0)!=0 and DyeingOrderID In (Select DyeingOrderID from DyeingOrderDetail Where LabDipDetailID<=0  And LabDipType In (2,3))  ";
            }
            #endregion
            #region Yet Not Send To Production
            if (bYetNotSendPro)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DyeingOrderType in (2,4,3) and isnull(ApproveBy,0)!=0 and DyeingOrderID in (Select DyeingOrderDetail.DyeingOrderID from DyeingOrderDetail where LabDipDetailID>0 and Isnull(DyeingOrderDetail.PTUID,0)<=0 and LabDipDetailID in (Select LabDipDetailID from LabDipDetail where LabdipID in  ( SELECT Labdip.LabDipID from Labdip Where OrderStatus in (5,6,7,8,9)))) ";
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DyeingOrderID in (Select DOD.DyeingOrderID from DyeingOrderDetail as DOD where ProductID in (" + sProductIDs + "))";
            }
            #endregion

            #region sMatchNo No
            if (!string.IsNullOrEmpty(sMatchNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DyeingOrderID in (Select DOD.DyeingOrderID from DyeingOrderDetail as DOD where ApproveLotNo like '%" + sMatchNo + "%')";
            }
            #endregion
            #region Style No
            if (!string.IsNullOrEmpty(sStyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "StyleNo like '%" + sStyleNo + "%'";
            }
            #endregion
            #region Ref No
            if (!string.IsNullOrEmpty(sRefNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RefNo like '%" + sRefNo + "%'";
            }
            #endregion

            #region Invoice No
            if (!string.IsNullOrEmpty(sInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SampleInvoiceID in (Select SInvoice.SampleInvoiceID from SampleInvoice as SInvoice where SInvoice.SampleInvoiceNo like '%" + sInvoiceNo + "%')";
            }
            #endregion

            #region P/I  No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINo like '%" + sPINo + "%') ";
            }
            #endregion

            #region ReceiveType
            if (nReceiveType > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nReceiveType == 1)
                {
                    sReturn = sReturn + "DyeingOrderID not in (Select DyeingOrderID from DUProGuideLine  where isnull(DUProGuideLine.ReceiveByID,0)>0)";
                }
                if (nReceiveType == 2)
                {
                    sReturn = sReturn + "DyeingOrderID  in (Select DyeingOrderID from DUProGuideLine where isnull(DUProGuideLine.ReceiveByID,0)=0)";
                }
            }
            #endregion
            #region Booking Status
            if (nBookingStatus > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nBookingStatus == 1)
                {
                    sReturn = sReturn + "IsBooking=1";
                }
                else if (nBookingStatus == 2)
                {
                    sReturn = sReturn + "IsBooking=0";
                }
            }
            #endregion
            string sSQL = sReturn1 + " " + sReturn + " order by Convert(int, [dbo].[SplitedStringGet](PONo, '-', 1)) DESC";
            return sSQL;
        }
        [HttpPost]
        public JsonResult GetsbyNo(DyeingOrder oDyeingOrder)
        {
            _oDyeingOrders = new List<DyeingOrder>();

            try
            {
                string sSQL = "SELECT * FROM View_DyeingOrder ";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oDyeingOrder.OrderNo))
                {
                    oDyeingOrder.OrderNo = oDyeingOrder.OrderNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderNo like '%" + oDyeingOrder.OrderNo + "%'";
                }
                if (!String.IsNullOrEmpty(oDyeingOrder.ExportPINo))
                {
                    oDyeingOrder.OrderNo = oDyeingOrder.ExportPINo.Trim();
                    Global.TagSQL(ref sReturn, "ExportPINo", oDyeingOrder.ExportPINo);
                    //sReturn = sReturn + "SampleInvocieNo like '%" + oDyeingOrder.SampleInvocieNo + "%'";
                }
                if (!String.IsNullOrEmpty(oDyeingOrder.SampleInvocieNo))
                {
                    oDyeingOrder.OrderNo = oDyeingOrder.SampleInvocieNo.Trim();
                    Global.TagSQL(ref sReturn, "SampleInvocieNo", oDyeingOrder.SampleInvocieNo);
                }
                if (oDyeingOrder.DyeingOrderType >0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DyeingOrderType in(" + oDyeingOrder.DyeingOrderType + ")";
                }
                if (!string.IsNullOrEmpty(oDyeingOrder.ErrorMessage))/// For Multiple Order
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DyeingOrderType in(" + oDyeingOrder.ErrorMessage + ")";
                }
                if (!string.IsNullOrEmpty(oDyeingOrder.FSCNo))/// For Multiple Order
                {
                    string sFSCNo = string.Empty;
                    if (oDyeingOrder.FSCNo.Split('~').Length > 0)
                        sFSCNo = oDyeingOrder.FSCNo.Split('~')[0];
                    if (!string.IsNullOrEmpty(sFSCNo))
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "isnull(FSCDetailID,0)>0 and FSCDetailID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where SCNo like '%" + sFSCNo + "%' ) )";
                    }
                    if (oDyeingOrder.FSCNo.Split('~').Length > 1)
                        sFSCNo = oDyeingOrder.FSCNo.Split('~')[1];
                    if (!string.IsNullOrEmpty(sFSCNo))
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "isnull(FSCDetailID,0)>0 and FSCDetailID in (Select FabricSalesContractDetailID from View_FabricSalesContractDetail where  FabricNo like '%" + sFSCNo + "%')";
                    }
                }
                sSQL = sSQL + "" + sReturn;
                _oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDyeingOrders = new List<DyeingOrder>();
                _oDyeingOrder = new DyeingOrder();
                _oDyeingOrder.ErrorMessage = ex.Message;
                _oDyeingOrders.Add(_oDyeingOrder);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDyeingOrder(DyeingOrder oDyeingOrder)
        {
            _oDyeingOrders = new List<DyeingOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_DyeingOrder ";
                string sReturn = " WHERE DyeingOrderID IN (SELECT DOD.DyeingOrderID FROM View_DyeingOrderDetail AS DOD WHERE ISNULL(DOD.PTUID,0)>0 AND ISNULL(DOD.YetToProductionQty,0)>0)";
                if (!String.IsNullOrEmpty(oDyeingOrder.OrderNo))
                {
                    oDyeingOrder.OrderNo = oDyeingOrder.OrderNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderNo like '%" + oDyeingOrder.OrderNo + "%'";
                }
                if (oDyeingOrder.DyeingOrderType > 0)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DyeingOrderType=" + oDyeingOrder.DyeingOrderType;
                }

                sSQL = sSQL + "" + sReturn;
                _oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDyeingOrders = new List<DyeingOrder>();
                _oDyeingOrder = new DyeingOrder();
                _oDyeingOrder.ErrorMessage = ex.Message;
                _oDyeingOrders.Add(_oDyeingOrder);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region PS
        [HttpPost]
        public JsonResult GetsDOD(DyeingOrderDetail oDyeingOrderDetail)
        {
            _oDyeingOrderDetails = new List<DyeingOrderDetail>();
            try
            {
                _oDyeingOrderDetails = DyeingOrderDetail.Gets(oDyeingOrderDetail.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDyeingOrderDetail= new DyeingOrderDetail();
                _oDyeingOrderDetail.ErrorMessage = ex.Message;
                _oDyeingOrderDetails.Add(_oDyeingOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        [HttpGet]
        public JsonResult GetDOD(int nID, double nts)
        {
            try
            {
                _oDyeingOrderDetail = new DyeingOrderDetail();
                if (nID > 0)
                {
                    _oDyeingOrderDetail = _oDyeingOrderDetail.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDyeingOrderDetail.DyeingOrderDetailID <= 0)
                    {
                        throw new Exception("No data found.");
                    }

                }
                else
                {
                    throw new Exception("Invalid entry.");
                }

            }
            catch (Exception ex)
            {
                _oDyeingOrderDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrderDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region DUColorCombo
        [HttpPost]
        public JsonResult Gets_DUColorCombo(DyeingOrder oDyeingOrder)
        {
            List<DUColorCombo> oDUColorCombos_Group = new List<DUColorCombo>();
            List<DUColorCombo> oDUColorCombos_Temp = new List<DUColorCombo>();
            try
            {
                oDUColorCombos_Temp = DUColorCombo.Gets(oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDUColorCombos_Group = oDUColorCombos_Temp.GroupBy(x => new { x.ComboID, x.DyeingOrderID }, (key, grp) =>
                       new DUColorCombo
                       {
                           ComboID = key.ComboID,
                           DyeingOrderID = key.DyeingOrderID
                       }).ToList();
             

                foreach (DUColorCombo oitem in oDUColorCombos_Group)
                {
                    foreach (DUColorCombo oitemTwo in oDUColorCombos_Temp)
                    {
                        if (oitemTwo.ComboID == oitem.ComboID)
                        {
                            if (string.IsNullOrEmpty(oitem.ColorName))
                            {
                                oitem.ColorName = oitemTwo.ColorName;
                            }
                            else
                            {
                                oitem.ColorName = oitem.ColorName + " + " + oitemTwo.ColorName;
                            }
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                _oDyeingOrderDetail = new DyeingOrderDetail();
                _oDyeingOrderDetail.ErrorMessage = ex.Message;
                _oDyeingOrderDetails.Add(_oDyeingOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUColorCombos_Group);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_DUColorCombo(List<DUColorCombo> oDUColorCombos)
        {
            List<DUColorCombo> oDUColorCombos_Group = new List<DUColorCombo>();
            List<DUColorCombo> oDUColorCombos_Temp = new List<DUColorCombo>();
            DUColorCombo oDUColorCombo = new DUColorCombo();
            try
            {
                oDUColorCombos = DUColorCombo.Save(oDUColorCombos, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oDUColorCombos_Temp = DUColorCombo.Gets(oDUColorCombos[0].DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDUColorCombos_Group = oDUColorCombos_Temp.GroupBy(x => new { x.ComboID, x.DyeingOrderID }, (key, grp) =>
                   new DUColorCombo
                   {
                       ComboID = key.ComboID,
                       DyeingOrderID = key.DyeingOrderID
                   }).ToList();

                foreach (DUColorCombo oitem in oDUColorCombos_Group)
                {
                    foreach (DUColorCombo oitemTwo in oDUColorCombos_Temp)
                    {
                        if (oitemTwo.ComboID == oitem.ComboID)
                        {
                            if (string.IsNullOrEmpty(oitem.ColorName))
                            {
                                oitem.ColorName = oitemTwo.ColorName;
                            }
                            else
                            {
                                oitem.ColorName =oitem.ColorName+" + "+oitemTwo.ColorName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oDUColorCombo = new DUColorCombo();
                oDUColorCombo.ErrorMessage = ex.Message;
                oDUColorCombos_Group.Add(new DUColorCombo());
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUColorCombos_Group);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDUColorCombo(DUColorCombo oDUColorCombo)
        {
            try
            {
                if (oDUColorCombo.DyeingOrderID > 0)
                {
                    oDUColorCombo.ErrorMessage = oDUColorCombo.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDUColorCombo = new DUColorCombo();
                oDUColorCombo.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUColorCombo.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
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

        #region Dyeing Order Detail Search use in Production Schedule
        #region SEARCHING
        private string GetSQL(string sTemp)
        {
            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dSOStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dSOEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sOrderNo = sTemp.Split('~')[3];
            string sBuyerIDs = sTemp.Split('~')[4];
            string sReturn1 = "SELECT * FROM View_DyeingOrderDetail";
            string sReturn = " WHERE ISNULL(PTUID,0)>0";

            #region Ordre No

            if (sOrderNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " JobNo LIKE '%" + sOrderNo + "%'";
            }
            #endregion

          
            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Order Date Wise
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate = '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate != '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate > '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate < '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate>= '" + dSOStartDate.ToString("dd MMM yyyy") + "' AND OrderDate < '" + dSOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate< '" + dSOStartDate.ToString("dd MMM yyyy") + "' OR OrderDate > '" + dSOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY ContractorID, DyeingOrderDetailID";
            return sReturn;
        }
        [HttpGet]
        public JsonResult DODetailsSearch(string Temp)
        {
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            try
            {
                string sSQL = GetSQL(Temp);
                oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDyeingOrderDetails = new List<DyeingOrderDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Dyeing Order To Lot Parent
        public ActionResult View_DUDetailProducts(int buid, int menuid, bool isInHouse)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDyeingOrderDetails = new List<DyeingOrderDetail>();

            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();

            oDyeingOrderDetails = DyeingOrderDetail.Gets(
                    "SELECT top(50)* FROM View_DyeingOrderDetail WHERE ProductID>0"
                    + " AND DyeingOrderID IN (SELECT DO.DyeingOrderID FROM DyeingOrder AS DO WHERE ISNULL(DO.ApproveBy,0)<>0 AND DO.DyeingOrderType IN (Select OrderType from DUOrderSetup WHERE ISNULL(IsInHouse,0)=" + (isInHouse ? 1 : 0) + "))  ORDER BY OrderDate DESC",
                   ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oDyeingOrderDetails = oDyeingOrderDetails.GroupBy(x => new { x.DyeingOrderID, x.OrderNo, x.ProductID, x.ProductName }, (key, grp) => new DyeingOrderDetail
            {
                ProductID = key.ProductID,
                ProductName = key.ProductName,
                ProductCodeName = grp.Select(x => x.ProductCodeName).FirstOrDefault(),
                DyeingOrderID = key.DyeingOrderID,
                OrderNo = key.OrderNo,
                OrderDate = grp.Select(x => x.OrderDate).FirstOrDefault(),
                BuyerName = grp.Select(x => x.BuyerName).FirstOrDefault(),
                Qty = grp.Sum(x => x.Qty),
                UnitPrice = grp.Max(x => x.UnitPrice)
            }).ToList();

            ViewBag.BUID = buid;
            ViewBag.IsInHouse = isInHouse;

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            
            return View(_oDyeingOrderDetails);
        }
        public ActionResult View_DUDetailProduct(int buid, int id, int productid, bool isInHouse)
        {
            Lot oLot = new Lot();
            LotParent oLotParent = new LotParent();
            DyeingOrder oDyeingOrder = new DyeingOrder();

            oDyeingOrder = DyeingOrder.Get(id, (int)Session[SessionInfo.currentUserID]);

            oLotParent.DyeingOrderID = oDyeingOrder.DyeingOrderID;
            oLotParent.DyeingOrderNo = oDyeingOrder.OrderNo;
            oLotParent.ContractorID = oDyeingOrder.ContractorID;
            oLotParent.ProductID = productid;
            oLotParent.ContractorName = oDyeingOrder.ContractorName;
            oLotParent.OrderDateInString = oDyeingOrder.OrderDateSt;
            oLotParent.Qty = oDyeingOrder.Qty;

            ViewBag.IsInHouse = isInHouse;

            oLotParent.LotParents = LotParent.Gets("SELECT * FROM View_LotParent WHERE DyeingOrderID=" + id + " and WorkingUnitID in (Select WorkingUnitID from WorkingUnit where UnitType="+(int)EnumWoringUnitType.Raw+")", (int)Session[SessionInfo.currentUserID]);
            return View(oLotParent);
        }
        public JsonResult Gets_Lot(LotParent oLotParent)
        {
            List<Lot> oLots = new List<Lot>();
            List<LotParent> oLotParents = new List<LotParent>();
            try
            {
                if (oLotParent.IsInHouse)
                {
                    #region Lot For Inhouse

                    oLots = Lot.Gets("SELECT top(100)* FROM View_Lot AS Lot WHERE ParentType !=" + (int)EnumTriggerParentsType.DUProGuideLineDetail + " and Lot.WorkingUnitID IN (SELECT WU.WorkingUnitID FROM WorkingUnit AS WU WHERE WU.UnitType=" + (int)EnumWoringUnitType.Raw + ") AND Lot.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE BU.BusinessUnitType= " + (int)EnumBusinessUnitType.Dyeing + ") AND ISNULL(Balance,0)>0 " + (string.IsNullOrEmpty(oLotParent.LotNo) ? "" : " AND LotNo Like '%" + oLotParent.LotNo + "%' "), ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oLots.ForEach(x =>  oLotParents.Add(new LotParent
                    {
                        LotID = x.LotID,
                        LotNo = x.LotNo,
                        ProductID = oLotParent.ProductID,
                        ProductName = x.ProductName,
                        StoreName = x.OperationUnitName,
                        DyeingOrderID = oLotParent.DyeingOrderID,
                        Qty = x.Balance,
                        Balance = x.Balance,
                        UnitPrice = x.UnitPrice
                    }));
                    #endregion
                }
                else
                {
                    #region Lot For Out Side

                    oLotParents = LotParent.Gets(
                    "SELECT top(100)* FROM View_LotParent WHERE workingunitID=150 and ParentType =" + (int)EnumTriggerParentsType.DUProGuideLineDetail + " and ProductID>0 AND ISNULL(Qty,0)>0"
                    + (oLotParent.ContractorID > 0 ? " AND ContractorID=" + oLotParent.ContractorID : "")

                    + (oLotParent.ProductID <= 0 ? "" : " AND ProductID =" + oLotParent.ProductID)

                    + (string.IsNullOrEmpty(oLotParent.DyeingOrderNo) ? "" : " AND DyeingOrderNo Like '%" + oLotParent.DyeingOrderNo + "%' ")
                    //+ " AND  DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE ISNULL(IsInHouse,0)=" + (oLotParent.IsInHouse ? 1 : 0) + ")  ORDER BY OrderDate",
                    , ((User)Session[SessionInfo.CurrentUser]).UserID);

                    #endregion
                }
            }
            catch (Exception e)
            {
                oLotParents.Add(new LotParent() { ErrorMessage = e.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLotParents);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Gets_SearchLot(LotParent oLotParent)
        {
            List<Lot> oLots = new List<Lot>();
            List<LotParent> oLotParents = new List<LotParent>();
            try
            {
                if (oLotParent.IsInHouse)
                {
                    #region Lot For Inhouse

                    oLots = Lot.Gets("SELECT top(100)* FROM View_Lot AS Lot WHERE ParentType !=" + (int)EnumTriggerParentsType.DUProGuideLineDetail + " and Lot.WorkingUnitID IN (SELECT WU.WorkingUnitID FROM WorkingUnit AS WU WHERE WU.UnitType=" + (int)EnumWoringUnitType.Raw + ") AND Lot.BUID IN (SELECT BU.BusinessUnitID FROM BusinessUnit AS BU WHERE BU.BusinessUnitType= " + (int)EnumBusinessUnitType.Dyeing + ") " + (string.IsNullOrEmpty(oLotParent.LotNo) ? "" : " AND LotNo Like '%" + oLotParent.LotNo + "%' "), ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oLots.ForEach(x => oLotParents.Add(new LotParent
                    {
                        LotID = x.LotID,
                        LotNo = x.LotNo,
                        ProductID = oLotParent.ProductID,
                        ProductName = x.ProductName,
                        StoreName = x.OperationUnitName,
                        DyeingOrderID = oLotParent.DyeingOrderID,
                        Qty = x.Balance,
                        Balance = x.Balance,
                        UnitPrice = x.UnitPrice
                    }));
                    #endregion
                }
                else
                {
                    #region Lot For Out Side

                    oLotParents = LotParent.Gets(
                    "SELECT top(100)* FROM View_LotParent WHERE ParentType =" + (int)EnumTriggerParentsType.DUProGuideLineDetail + " AND WorkingUnitID IN (SELECT WU.WorkingUnitID FROM WorkingUnit AS WU WHERE WU.UnitType=" + (int)EnumWoringUnitType.Raw + ")"
                    + (oLotParent.ContractorID > 0 ? " AND ContractorID=" + oLotParent.ContractorID : "")
                    + (string.IsNullOrEmpty(oLotParent.LotNo) ? "" : " AND LotNo Like '%" + oLotParent.LotNo + "%' ")
                    , ((User)Session[SessionInfo.CurrentUser]).UserID);

                    #endregion
                }
            }
            catch (Exception e)
            {
                oLotParents.Add(new LotParent() { ErrorMessage = e.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLotParents);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsDistingDeliveryNote(DyeingOrder oDyeingOrder)
        {
            string sSQL = "";
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            try
            {

                    if (!string.IsNullOrEmpty(oDyeingOrder.DeliveryNote))
                    {
                        sSQL = "SELECT DISTINCT(DeliveryNote) As DeliveryNote  FROM DyeingOrder where LEN(DeliveryNote) > 1 and DeliveryNote LIKE '%" + oDyeingOrder.DeliveryNote + "%'";
                    }
                    else
                    {
                        sSQL = "SELECT DISTINCT(DeliveryNote) As DeliveryNote  FROM DyeingOrder where LEN(DeliveryNote) > 1";
                    }
             
               
                oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDyeingOrders.Count <= 0)
                {
                    oDyeingOrder = new DyeingOrder();
                    oDyeingOrder.ErrorMessage = "";
                    oDyeingOrders.Add(oDyeingOrder);
                }

            }
            catch (Exception ex)
            {
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = ex.Message;
                oDyeingOrders.Add(oDyeingOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets_DUDetailProducts(DyeingOrder oDyeingOrder)
        {
            _oDyeingOrderDetails = new List<DyeingOrderDetail>();

            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();

            oDyeingOrderDetails = DyeingOrderDetail.Gets(
                    "SELECT top(500) * FROM View_DyeingOrderDetail WHERE ProductID>0"
                    + (string.IsNullOrEmpty(oDyeingOrder.OrderNo) ? "" : " AND OrderNo Like '%" + oDyeingOrder.OrderNo + "%' ")
                    + (string.IsNullOrEmpty(oDyeingOrder.Params) ? "" : " AND DyeingOrderID IN (SELECT DyeingOrderID FROM LoTParent WHERE LotID IN (" + oDyeingOrder.Params + "))")
                    + " AND DyeingOrderID IN (SELECT DO.DyeingOrderID FROM DyeingOrder AS DO WHERE ISNULL(DO.ApproveBy,0)<>0 AND DO.DyeingOrderType IN (Select OrderType from DUOrderSetup WHERE ISNULL(IsInHouse,0)=" + (oDyeingOrder.IsInHouse ? 1 : 0) + "))  ORDER BY OrderDate DESC",
                    ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oDyeingOrderDetails = oDyeingOrderDetails.GroupBy(x => new { x.DyeingOrderID, x.OrderNo, x.ProductID, x.ProductName }, (key, grp) => new DyeingOrderDetail
            {
                ProductID = key.ProductID,
                ProductName = key.ProductName,
                ProductCodeName = grp.Select(x => x.ProductCodeName).FirstOrDefault(),
                DyeingOrderID = key.DyeingOrderID,
                OrderNo = key.OrderNo,
                OrderDate = grp.Select(x => x.OrderDate).FirstOrDefault(),
                BuyerName = grp.Select(x => x.BuyerName).FirstOrDefault(),
                Qty = grp.Sum(x => x.Qty),
                UnitPrice = grp.Max(x => x.UnitPrice)
            }).ToList();

            var jsonResult = Json(_oDyeingOrderDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Transfer_Lot(LotParent oLotParent)
        {
            try
            {
                oLotParent = oLotParent.Lot_Transfer((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLotParent = new LotParent();
                oLotParent.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotParent);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Return_Lot(LotParent oLotParent) //DB OPERATION :: CANCEL
        {
            try
            { 
                oLotParent = oLotParent.Lot_Return((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLotParent = new LotParent();
                oLotParent.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotParent);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Edit_Lot(LotParent oLotParent)
        {
            try
            {
                oLotParent = oLotParent.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLotParent = new LotParent();
                oLotParent.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotParent);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Loan Order Report
        //public ActionResult ViewLoanOrders(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);

        //    List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
        //    ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));

        //    return View(oDyeingOrderDetails);
        //}
        //[HttpPost]
        //public JsonResult AdvanceSearch(DyeingOrder oDyeingOrder)
        //{
        //    DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
        //    List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
        //    try
        //    {
        //        string sSQL = MakeSQL(oDyeingOrder.Params);
        //        oDyeingOrderDetails = DyeingOrderDetail.GetsLoanOrder(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        oDyeingOrderDetail.ErrorMessage = ex.Message;
        //        oDyeingOrderDetails.Add(oDyeingOrderDetail);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oDyeingOrderDetails);
        //     return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
  
        public void ExportToExcel(string sParams, int nReportType)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();

            List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails = new List<DUProGuideLineDetail>();

            List<DUDeliveryChallanRegister> oDUDeliveryChallanRegisters = new List<DUDeliveryChallanRegister>();

            DyeingOrder oDyeingOrder = new DyeingOrder();
            oDyeingOrder.Note = sParams;
            string sString = MakeSQL(oDyeingOrder);

            oDyeingOrders = DyeingOrder.Gets(sString, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //oDyeingOrderDetails = DyeingOrderDetail.Gets("SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderID IN (" + string.Join(",", oDyeingOrders.Select(x => x.DyeingOrderID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            //oDUProGuideLines = DUProGuideLine.Gets("SELECT * FROM View_DUProGuideLine WHERE DyeingOrderID IN (" + string.Join(",", oDyeingOrders.Select(x => x.DyeingOrderID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            //oDUProGuideLineDetails = DUProGuideLineDetail.Gets("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (" + string.Join(",", oDUProGuideLines.Select(x => x.DUProGuideLineID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            //oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets("SELECT * FROM View_DUDeliveryChallanDetail WHERE OrderID IN (" + string.Join(",", oDyeingOrders.Select(x => x.DyeingOrderID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            //oDUDeliveryChallans = DUDeliveryChallan.Gets("SELECT * FROM View_DUDeliveryChallan WHERE DUDeliveryChallanID IN (" + string.Join(",", oDUDeliveryChallanDetails.Select(x => x.DUDeliveryChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            oDyeingOrderDetails = DyeingOrderDetail.Gets("select * from View_DyeingOrderDetail where DyeingOrderID IN (" + string.Join(",", oDyeingOrders.Select(x => x.DyeingOrderID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUProGuideLineDetails = DUProGuideLineDetail.Gets("select * from View_DUProGuideLineDetail where DUProGuideLineID IN(SELECT DUProGuideLineID FROM DUProGuideLine WHERE DyeingOrderID IN (" + string.Join(",", oDyeingOrders.Select(x => x.DyeingOrderID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets("select * from View_DUDeliveryChallanRegister where DyeingOrderDetailID IN (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderDetailID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Order Date", Width = 15f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Buyer/ Supplier", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Qty", Width = 12f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Unit", Width = 15f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 30f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Qty", Width = 12f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Unit", Width = 15f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "No", Width = 20f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Qty", Width = 12f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Unit", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Status", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Loan Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Loan Report");
                sheet.Name = "Loan Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 18; //23
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Loan Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region Group Header
                ExcelTool.FillCellMerge(ref sheet, "Order Info", nRowIndex, nRowIndex, 2, 8, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, true);
                ExcelTool.FillCellMerge(ref sheet, "Receiving Info", nRowIndex, nRowIndex, 9, 13, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, true);
                ExcelTool.FillCellMerge(ref sheet, "Delivery Info", nRowIndex, nRowIndex, 14, 19, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, true);
                nRowIndex++;
                #endregion

                #region Data
                int nSL = 1;
                nEndCol = table_header.Count() + nStartCol;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
                oDyeingOrderDetails = oDyeingOrderDetails.OrderBy(x => x.OrderNo).ToList();
                string sPreviousOrderName = "";

                int nOrderRowSpan = 0;
                int nOrderCount = 0;
                int nReceiveCount = 0;
                int nChallanCount = 0;
                int nTempRowIndex = 0;
                int nStartRowIndex = 0;
                int nColStepCount = 0;

                foreach (var oItem in oDyeingOrders)
                {
                    nStartCol = 2;
                    
                    var oItem_DUDetails = oDyeingOrderDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList();
                    var oItem_DUPGLDetails = oDUProGuideLineDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList();
                    var oItem_oDUDeliveryChallanDetails = oDUDeliveryChallanRegisters.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList();

                    nOrderCount = oItem_DUDetails.Count();
                    nReceiveCount = oItem_DUPGLDetails.Count();
                    nChallanCount = oItem_oDUDeliveryChallanDetails.Count();
                    nOrderRowSpan = Math.Max(Math.Max(nOrderCount, nReceiveCount), nChallanCount) - 1;
                    if (nOrderRowSpan < 0) nOrderRowSpan = 0;
                    //if (nOrderRowSpan<=0)
                    //{
                    //    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, nSL++.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                    //    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem.OrderNo, false, ExcelHorizontalAlignment.Left, false, false);
                    //    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem.OrderDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                    //    ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                    //}
                    //else
                    //{
                    //    ExcelTool.FillCellMerge(ref sheet, nSL++.ToString(), nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                    //    ExcelTool.FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                    //    ExcelTool.FillCellMerge(ref sheet, oItem.OrderDateSt, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                    //    ExcelTool.FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                    //}

                    ExcelTool.FillCellMerge(ref sheet, nSL++.ToString(), nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                    ExcelTool.FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                    ExcelTool.FillCellMerge(ref sheet, oItem.OrderDateSt, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                    ExcelTool.FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);

                    nTempRowIndex = 0;

                    if (oItem_DUDetails.Count > 0)
                    {
                        foreach (DyeingOrderDetail obj in oItem_DUDetails)
                        {
                            nStartRowIndex = nStartCol;
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, Math.Round(obj.Qty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.MUnit, false, ExcelHorizontalAlignment.Center, false, false);
                            nTempRowIndex++;
                            nColStepCount = nStartCol++;
                            nStartCol = nStartRowIndex;
                        }
                    }
                    else 
                    {
                        //ExcelTool.FillCellBasic(sheet, nRowIndex , nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex , nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        nColStepCount = nStartCol++;
                    }
                    nStartCol = nColStepCount;
                    nTempRowIndex = 0;

                    if (oItem_DUPGLDetails.Count > 0)
                    {
                        foreach (DUProGuideLineDetail obj in oItem_DUPGLDetails)
                        {
                            nStartRowIndex = nStartCol;
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanNo.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ReceiveDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, Math.Round(obj.Qty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.MUnit, false, ExcelHorizontalAlignment.Center, false, false);
                            nTempRowIndex++;
                            nColStepCount = nStartCol++;
                            nStartCol = nStartRowIndex;
                        }
                    }
                    else
                    {
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);

                        nColStepCount = nStartCol++;
                    }
                    nStartCol = nColStepCount;
                    nTempRowIndex = 0;

                    if (oItem_oDUDeliveryChallanDetails.Count > 0)
                    {
                        foreach (DUDeliveryChallanRegister obj in oItem_oDUDeliveryChallanDetails)
                        {
                            nStartCol = 14;

                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanNo, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, Math.Round(obj.Qty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.MUnit, false, ExcelHorizontalAlignment.Center, false, false);
                            nTempRowIndex++;
                            //nStartCol = nStartRowIndex;
                        }
                    }
                    else
                    {
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                        nStartCol = 14;
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);

                        nColStepCount = nStartCol;
                    }

                    //ExcelTool.FillCellBasic(sheet, nRowIndex, nColStepCount, oItem.IsCloseSt, false, ExcelHorizontalAlignment.Center, false, false);
                    ExcelTool.FillCellMerge(ref sheet, oItem.IsCloseSt, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);

                    nRowIndex = nRowIndex + nOrderRowSpan+1;
                    #region Sub total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Order Sub Total :", nRowIndex, nRowIndex, 2, 6, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, 7, Math.Round(oDyeingOrderDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList().Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, 8, "", false, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellMerge(ref sheet, "Receiving Sub Total:", nRowIndex, nRowIndex, 9, 11, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, 12, Math.Round(oDUProGuideLineDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList().Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, 13, "", false, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellMerge(ref sheet, "Delivery Sub Total", nRowIndex, nRowIndex, 14, 16, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, 17, Math.Round(oDUDeliveryChallanRegisters.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList().Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, 18, "", false, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, 19, "", false, ExcelHorizontalAlignment.Right, false, false);
                    #endregion
                    nRowIndex++;
                }

                #region total
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total(Order) :", nRowIndex, nRowIndex, 2, 6, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 7, Math.Round(oDyeingOrderDetails.Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 8, "", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellMerge(ref sheet, "Grand Total(Receiving) :", nRowIndex, nRowIndex, 9, 11, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 12, Math.Round(oDUProGuideLineDetails.Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 13, "", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellMerge(ref sheet, "Grand Total(Delivery) :", nRowIndex, nRowIndex, 14, 16, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 17, Math.Round(oDUDeliveryChallanRegisters.Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 18, "", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 19, "", false, ExcelHorizontalAlignment.Right, false, false);
                #endregion

                #endregion
                    nRowIndex++;
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LoanReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        public void ExportToExcel2(string sParams, int nReportType, int nBUID)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();

            DyeingOrderReport oDyeingOrderReport =new DyeingOrderReport();

            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();

            List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            List<DUProGuideLineDetail> oDUProGuideLineDetails = new List<DUProGuideLineDetail>();

            List<DUDeliveryChallanRegister> oDUDeliveryChallanRegisters = new List<DUDeliveryChallanRegister>();

            DyeingOrder oDyeingOrder = new DyeingOrder();
            oDyeingOrder.Note = sParams;
            string sString = MakeSQL(oDyeingOrder);

            oDyeingOrders = DyeingOrder.Gets(sString, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDyeingOrderReports = DyeingOrderReport.Gets("select * from View_DyeingOrderReport where DyeingOrderID IN (" + string.Join(",", oDyeingOrders.Select(x => x.DyeingOrderID)) + ") ORDER BY DyeingOrderID, ProductID", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUProGuideLineDetails = DUProGuideLineDetail.Gets("select * from View_DUProGuideLineDetail where DUProGuideLineID IN(SELECT DUProGuideLineID FROM DUProGuideLine WHERE DyeingOrderID IN (" + string.Join(",", oDyeingOrders.Select(x => x.DyeingOrderID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets("select * from View_DUDeliveryChallanRegister where DyeingOrderDetailID IN (" + string.Join(",", oDyeingOrderReports.Select(x => x.DyeingOrderDetailID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            //// Find Order Received That not including in Orders
            oDUProGuideLineDetails.ForEach(x =>
            {
                if (oDyeingOrderReports.FirstOrDefault() != null && oDyeingOrderReports.FirstOrDefault().DyeingOrderID > 0 && oDyeingOrderReports.Where(b => b.DyeingOrderID == x.DyeingOrderID && b.ProductID == x.ProductID).Count() > 0)
                {
                    x.ErrorMessage = "NotInOrder";
                }
            });

            var oDUProguidelines = oDUProGuideLineDetails.Where(p => p.ErrorMessage != "NotInOrder").GroupBy(x => new { x.ProductID, x.ProductName, x.DyeingOrderID }, (key, grp) =>
                                      new
                                      {
                                          ProductID = key.ProductID,
                                          DyeingOrderID = key.DyeingOrderID,
                                          ProductName = key.ProductName,
                                      }).ToList();


            foreach (var oItem in oDUProguidelines)
            {
                oDyeingOrderReport=new DyeingOrderReport();
                oDyeingOrderReport.ProductID=oItem.ProductID;
                oDyeingOrderReport.DyeingOrderID = oItem.DyeingOrderID;
                oDyeingOrderReport.ProductName = oItem.ProductName;
                if (oDyeingOrderReports.FirstOrDefault() != null && oDyeingOrderReports.FirstOrDefault().DyeingOrderID > 0 && oDyeingOrderReports.Where(b => b.DyeingOrderID == oItem.DyeingOrderID ).Count() > 0)
                {
                    oDyeingOrderReport.ContractorName = oDyeingOrderReports.Where(m => m.DyeingOrderID == oItem.DyeingOrderID).FirstOrDefault().ContractorName;
                }
                oDyeingOrderReports.Add(oDyeingOrderReport);
            }
            //// Find Delivery Challan That not including in Orders
            oDUDeliveryChallanRegisters.ForEach(x =>
            {
                if (oDyeingOrderReports.FirstOrDefault() != null && oDyeingOrderReports.FirstOrDefault().DyeingOrderID > 0 && oDyeingOrderReports.Where(b => b.DyeingOrderID == x.DyeingOrderID && b.ProductID == x.ProductID).Count() > 0)
                {
                    x.ErrorMessage = "NotInOrder";
                }
            });

            var oDUDeliveryChallans = oDUDeliveryChallanRegisters.Where(p => p.ErrorMessage != "NotInOrder").GroupBy(x => new { x.ProductID, x.ProductName, x.DyeingOrderID }, (key, grp) =>
                                    new
                                    {
                                        ProductID = key.ProductID,
                                        DyeingOrderID = key.DyeingOrderID,
                                        ProductName = key.ProductName,
                                    }).ToList();


            foreach (var oItem in oDUDeliveryChallans)
            {
                oDyeingOrderReport = new DyeingOrderReport();
                oDyeingOrderReport.ProductID = oItem.ProductID;
                oDyeingOrderReport.DyeingOrderID = oItem.DyeingOrderID;
                oDyeingOrderReport.ProductName = oItem.ProductName;
                if (oDyeingOrderReports.FirstOrDefault() != null && oDyeingOrderReports.FirstOrDefault().DyeingOrderID > 0 && oDyeingOrderReports.Where(b => b.DyeingOrderID == oItem.DyeingOrderID).Count() > 0)
                {
                    oDyeingOrderReport.ContractorName = oDyeingOrderReports.Where(m => m.DyeingOrderID == oItem.DyeingOrderID).FirstOrDefault().ContractorName;
                }
                oDyeingOrderReports.Add(oDyeingOrderReport);
            }
            ////End

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

            }
            
            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Item", Width = 20f, IsRotate = false, Align = TextAlign.Left });

            table_header.Add(new TableHeader { Header = "Challan No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Date", Width = 12f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Quantity", Width = 12f, IsRotate = false, Align = TextAlign.Left });

            table_header.Add(new TableHeader { Header = "Challan No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Date", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Quantity", Width = 12f, IsRotate = false, Align = TextAlign.Left });
            
            //table_header.Add(new TableHeader { Header = "Net Loan Position", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol+1;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Loan Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Loan Report");
                sheet.Name = "Loan Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 18; //23
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region title
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Loan Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion

                var oContractors = oDyeingOrderReports.GroupBy(x => new { x.ContractorName }, (key, grp) => new
                {
                    ContractorName = key.ContractorName,
                   // ContractorID = key.ContractorID,
                    Results = grp.ToList() //All data
                });

                //var dataForBOP = oDyeingOrderDetails.GroupBy(x => new { x.BuyerName, x.ProductID, x.DyeingOrderID }, (key, grp) => new
                //{
                //    BuyerNameForBOP = key.BuyerName,
                //    ProductIDForBOP = key.ProductID,
                //    DyeingOrderIDForBOP = key.DyeingOrderID,
                //    ResultsForBOP = grp.ToList() //All data
                //});

                foreach (var oContractor in oContractors)
                {
                    #region Group Header
                    ExcelTool.FillCellMerge(ref sheet, "Supplier/Buyer: " + oContractor.ContractorName, nRowIndex, nRowIndex, 2, 4, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, true);
                    ExcelTool.FillCellMerge(ref sheet, "Receiving Info", nRowIndex, nRowIndex, 5, 7, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, true);
                    ExcelTool.FillCellMerge(ref sheet, "Delivery Info", nRowIndex, nRowIndex, 8, 10, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, true);
                    ExcelTool.FillCellMerge(ref sheet, "Net Loan Position", nRowIndex, nRowIndex+1, 11, 11, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, true);
                    nRowIndex++;
                    #endregion

                    #region Header Print
                    nEndCol = table_header.Count() + nStartCol;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol-1, 10, true, true);
                    #endregion

                    var oDyeingOrders_Lon = oContractor.Results.GroupBy(x => new { oContractor.ContractorName, x.ProductID, x.ProductName, x.DyeingOrderID, x.OrderNo }, (key, grp) => new
                    {
                        ContractorName = key.ContractorName,
                        ProductIDForBOP = key.ProductID,
                        ProductNameForBOP = key.ProductName,
                        DyeingOrderIDForBOP = key.DyeingOrderID,
                        OrderNoForBOP = key.OrderNo,
                        ResultsForBOP = grp.ToList() //All data
                    });



                    //oDUProGuideLineDetails = DUProGuideLineDetail.Gets("select * from View_DUProGuideLineDetail where DUProGuideLineID IN(SELECT DUProGuideLineID FROM DUProGuideLine WHERE DyeingOrderID IN (" + string.Join(",", dataForBOP.Select(x => x.DyeingOrderIDForBOP)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //oDUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets("select * from View_DUDeliveryChallanRegister where DyeingOrderDetailID IN (SELECT DyeingOrderDetailID FROM DyeingOrderDetail WHERE DyeingOrderID IN ( " + string.Join(",", dataForBOP.Select(x => x.DyeingOrderIDForBOP)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrders_Lon = oDyeingOrders_Lon.OrderBy(p => p.ProductIDForBOP).ToList();
                    int nSL = 1, nProductID = -9999;
                    double nReceiveQty = 0, nChallanQty = 0;
                    foreach (var oDataForBOP in oDyeingOrders_Lon)
                    {
                        #region Sub Total
                        if (nProductID != oDataForBOP.ProductIDForBOP && nSL > 1)
                        {
                            nStartCol = 2;
                            ExcelTool.FillCellMerge(ref sheet, "Sub Total :", nRowIndex, nRowIndex, nStartCol, nStartCol+=4, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                            ExcelTool.Formatter = "#,##0.00;(#,##0.00)"; nStartCol++;
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(nReceiveQty, 2).ToString(), false, ExcelHorizontalAlignment.Right, true, false);
                            ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 1, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                            nStartCol++;
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(nChallanQty, 2).ToString(), false, ExcelHorizontalAlignment.Right, true, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round((nReceiveQty-nChallanQty), 2).ToString(), false, ExcelHorizontalAlignment.Left, true, false);
                            nReceiveQty = 0; nChallanQty = 0; nRowIndex++;
                        }
                        #endregion
                        nStartCol = 2;
                        var oItem_DUPGLDetails = oDUProGuideLineDetails.Where(x => x.DyeingOrderID == oDataForBOP.DyeingOrderIDForBOP && x.ProductID == oDataForBOP.ProductIDForBOP).OrderBy(y => y.ReceiveDate).ToList();
                        var oItem_oDUDeliveryChallanDetails = oDUDeliveryChallanRegisters.Where(x => x.DyeingOrderID == oDataForBOP.DyeingOrderIDForBOP && x.ProductID == oDataForBOP.ProductIDForBOP).OrderBy(y => y.ChallanDate).ToList();
                        int nMaxRow = (Math.Max(oItem_DUPGLDetails.Count, oItem_oDUDeliveryChallanDetails.Count)) - 1;                        
                        if (nMaxRow < 0) nMaxRow = 0;

                        #region Detail
                        ExcelTool.FillCellMerge(ref sheet, nSL++.ToString(), nRowIndex, nRowIndex + nMaxRow, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oDataForBOP.OrderNoForBOP, nRowIndex, nRowIndex + nMaxRow, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oDataForBOP.ProductNameForBOP, nRowIndex, nRowIndex + nMaxRow, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        #endregion

                        int nMR = Math.Max(oItem_DUPGLDetails.Count, oItem_oDUDeliveryChallanDetails.Count);
                        //int nTempIndex = nRowIndex;
                        int nR = 0, nDcount=0;
                        if (nMR > 0)
                        {
                            #region Receive info
                            foreach (var obj in oItem_DUPGLDetails)
                            {
                                nR++; 
                                nStartCol = 5;
                                ExcelTool.FillCellBasic(sheet, nRowIndex+nDcount, nStartCol++, obj.ChallanNo, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, obj.ReceiveDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, Math.Round(obj.Qty, 2).ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                                nReceiveQty += obj.Qty;
                                nDcount++;
                            }
                            if (nR < nMR)
                            {
                                while (nMR != nR)
                                {
                                    nR++;
                                    nStartCol = 5;
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    nDcount++;
                                }
                            }

                            #endregion

                            #region Challan info
                            nR = 0; nDcount = 0;
                            foreach (var obj in oItem_oDUDeliveryChallanDetails)
                            {
                                nR++;
                                nStartCol = 8;
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, obj.ChallanNo, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, obj.ChallanDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, Math.Round(obj.Qty, 2).ToString(), false, ExcelHorizontalAlignment.Right, false, false);

                                nChallanQty += obj.Qty;
                                nDcount++;
                            }

                            if (nR < nMR)
                            {
                                while (nMR != nR)
                                {
                                    nR++; nStartCol = 8;
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                    ExcelTool.FillCellBasic(sheet, nRowIndex + nDcount, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                                    nDcount++;
                                }
                            }

                            #endregion
                            //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex + nMaxRow, nStartCol, nStartCol, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);

                            nRowIndex += nMR;
                        }
                        else
                        {
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                            nRowIndex++;
                        }

                        
                        nProductID = oDataForBOP.ProductIDForBOP;
                    }

                    #region Sub Total
                    nStartCol = 2; //nRowIndex++;
                    ExcelTool.FillCellMerge(ref sheet, "Sub Total :", nRowIndex, nRowIndex, nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)"; nStartCol++;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(nReceiveQty, 2).ToString(), false, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 1, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                    nStartCol++;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(nChallanQty, 2).ToString(), false, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round((nReceiveQty - nChallanQty), 2).ToString(), false, ExcelHorizontalAlignment.Left, true, false);
                    nReceiveQty = 0; nChallanQty = 0; nRowIndex++;
                    #endregion

                }

                #region Comment Code
                //#region Data
                //int nSL = 1;
                //nEndCol = table_header.Count() + nStartCol;
                //ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                //DyeingOrderDetail oDyeingOrderDetail = new DyeingOrderDetail();
                //oDyeingOrderDetails = oDyeingOrderDetails.OrderBy(x => x.OrderNo).ToList();
                //string sPreviousOrderName = "";

                //int nOrderRowSpan = 0;
                //int nOrderCount = 0;
                //int nReceiveCount = 0;
                //int nChallanCount = 0;
                //int nTempRowIndex = 0;
                //int nStartRowIndex = 0;
                //int nColStepCount = 0;

                //foreach (var oItem in oDyeingOrders)
                //{
                //    nStartCol = 2;

                //    var oItem_DUDetails = oDyeingOrderDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList();
                //    var oItem_DUPGLDetails = oDUProGuideLineDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList();
                //    var oItem_oDUDeliveryChallanDetails = oDUDeliveryChallanRegisters.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList();

                //    nOrderCount = oItem_DUDetails.Count();
                //    nReceiveCount = oItem_DUPGLDetails.Count();
                //    nChallanCount = oItem_oDUDeliveryChallanDetails.Count();
                //    nOrderRowSpan = Math.Max(Math.Max(nOrderCount, nReceiveCount), nChallanCount) - 1;

                //    if (nOrderRowSpan <= 0)
                //    {
                //        ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, nSL++.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem.OrderNo, false, ExcelHorizontalAlignment.Left, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem.OrderDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, oItem.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                //    }
                //    else
                //    {
                //        ExcelTool.FillCellMerge(ref sheet, nSL++.ToString(), nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //        ExcelTool.FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //        ExcelTool.FillCellMerge(ref sheet, oItem.OrderDateSt, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //        ExcelTool.FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex + nOrderRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                //    }

                //    nTempRowIndex = 0;

                //    if (oItem_DUDetails.Count > 0)
                //    {
                //        foreach (DyeingOrderDetail obj in oItem_DUDetails)
                //        {
                //            nStartRowIndex = nStartCol;
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, Math.Round(obj.Qty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.MUnit, false, ExcelHorizontalAlignment.Center, false, false);
                //            nTempRowIndex++;
                //            nColStepCount = nStartCol++;
                //            nStartCol = nStartRowIndex;
                //        }
                //    }
                //    else
                //    {
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                //        nColStepCount = nStartCol++;
                //    }
                //    nStartCol = nColStepCount;
                //    nTempRowIndex = 0;

                //    if (oItem_DUPGLDetails.Count > 0)
                //    {
                //        foreach (DUProGuideLineDetail obj in oItem_DUPGLDetails)
                //        {
                //            nStartRowIndex = nStartCol;
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanNo.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ReceiveDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, Math.Round(obj.Qty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.MUnit, false, ExcelHorizontalAlignment.Center, false, false);
                //            nTempRowIndex++;
                //            nColStepCount = nStartCol++;
                //            nStartCol = nStartRowIndex;
                //        }
                //    }
                //    else
                //    {
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);

                //        nColStepCount = nStartCol++;
                //    }
                //    nStartCol = nColStepCount;
                //    nTempRowIndex = 0;

                //    if (oItem_oDUDeliveryChallanDetails.Count > 0)
                //    {
                //        foreach (DUDeliveryChallanRegister obj in oItem_oDUDeliveryChallanDetails)
                //        {
                //            nStartRowIndex = nStartCol;

                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanNo, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ChallanDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, Math.Round(obj.Qty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                //            ExcelTool.FillCellBasic(sheet, nRowIndex + nTempRowIndex, nStartCol++, obj.MUnit, false, ExcelHorizontalAlignment.Center, false, false);
                //            nTempRowIndex++;
                //            nStartCol = nStartRowIndex;
                //        }
                //    }
                //    else
                //    {
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                //        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);

                //        nColStepCount = nStartCol++;
                //    }

                //    ExcelTool.FillCellBasic(sheet, nRowIndex, nColStepCount, oItem.IsCloseSt, false, ExcelHorizontalAlignment.Center, false, false);

                //    nRowIndex = nRowIndex + nOrderRowSpan + 1;
                //    #region Sub total
                //    nStartCol = 2;
                //    ExcelTool.FillCellMerge(ref sheet, "Order Sub Total :", nRowIndex, nRowIndex, 2, 6, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                //    ExcelTool.FillCellBasic(sheet, nRowIndex, 7, Math.Round(oDyeingOrderDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList().Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //    ExcelTool.FillCellBasic(sheet, nRowIndex, 8, "", false, ExcelHorizontalAlignment.Right, false, false);
                //    ExcelTool.FillCellMerge(ref sheet, "Receiving Sub Total:", nRowIndex, nRowIndex, 9, 11, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                //    ExcelTool.FillCellBasic(sheet, nRowIndex, 12, Math.Round(oDUProGuideLineDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList().Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //    ExcelTool.FillCellBasic(sheet, nRowIndex, 13, "", false, ExcelHorizontalAlignment.Right, false, false);
                //    ExcelTool.FillCellMerge(ref sheet, "Delivery Sub Total", nRowIndex, nRowIndex, 14, 16, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                //    ExcelTool.FillCellBasic(sheet, nRowIndex, 17, Math.Round(oDUDeliveryChallanRegisters.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList().Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //    ExcelTool.FillCellBasic(sheet, nRowIndex, 18, "", false, ExcelHorizontalAlignment.Right, false, false);
                //    ExcelTool.FillCellBasic(sheet, nRowIndex, 19, "", false, ExcelHorizontalAlignment.Right, false, false);
                //    #endregion
                //    nRowIndex++;
                //}

                //#region total
                //nStartCol = 2;
                //ExcelTool.FillCellMerge(ref sheet, "Grand Total(Order) :", nRowIndex, nRowIndex, 2, 6, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, 7, Math.Round(oDyeingOrderDetails.Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, 8, "", false, ExcelHorizontalAlignment.Right, false, false);
                //ExcelTool.FillCellMerge(ref sheet, "Grand Total(Receiving) :", nRowIndex, nRowIndex, 9, 11, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, 12, Math.Round(oDUProGuideLineDetails.Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, 13, "", false, ExcelHorizontalAlignment.Right, false, false);
                //ExcelTool.FillCellMerge(ref sheet, "Grand Total(Delivery) :", nRowIndex, nRowIndex, 14, 16, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, 17, Math.Round(oDUDeliveryChallanRegisters.Sum(x => x.Qty), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, 18, "", false, ExcelHorizontalAlignment.Right, false, false);
                //#endregion

                //#endregion

                #endregion


                nRowIndex++;
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LoanReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        #endregion


        [HttpPost]
        public JsonResult GetsRouteSheet(DyeingOrderDetail oDyeingOrderDetail)
        {
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
         
            try
            {
                string sSQL = "Select top(50)* from view_RouteSheetDO where  RouteSheetNo like '%" + oDyeingOrderDetail.ApproveLotNo + "%'  order By RouteSheetID Desc,RouteSheetNo Asc";
                oRouteSheetDOs = RouteSheetDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach(RouteSheetDO oItem in oRouteSheetDOs)
                {
                    oDyeingOrderReport = new DyeingOrderReport();
                    oDyeingOrderReport.DyeingOrderDetailID = 0;
                    oDyeingOrderReport.Shade = (int)oItem.Shade;
                    oDyeingOrderReport.OrderNo = oItem.OrderNo;
                    oDyeingOrderReport.ContractorName = oItem.ContractorName;
                    oDyeingOrderReport.DyeingOrderType = (int)oItem.OrderType;
                    oDyeingOrderReport.PantonNo = oItem.PantonNo;
                    oDyeingOrderReport.ProductID = oItem.ProductID;
                    oDyeingOrderReport.ProductName = oItem.ProductName;
                    oDyeingOrderReport.Qty = oItem.Qty_RS;
                    oDyeingOrderReport.StyleNo = oItem.StyleNo;
                    oDyeingOrderReport.ColorName = oItem.ColorName;
                    oDyeingOrderReport.ColorNo = oItem.ColorNo;
                    oDyeingOrderReport.NoCode = oItem.NoCode;
                    oDyeingOrderReport.LabdipNo = oItem.LabdipNo;
                    oDyeingOrderReport.ApproveLotNo = oItem.RoutesheetNo;
                    oDyeingOrderReport.LabDipDetailID = oItem.LabDipDetailID;
                    //oDyeingOrderReport.Qty = 0;
                    oDyeingOrderReport.HankorCone = oItem.HankorCone;
                    oDyeingOrderReport.BuyerRef = oItem.BuyerRef;
                    oDyeingOrderReport.Note = oItem.Note;
                    oDyeingOrderReport.HankorCone = oItem.HankorCone;
                    
                    oDyeingOrderReport.LabDipType = (int)EnumLabDipType.Normal;
                    
                    oDyeingOrderReport.DyeingOrderDetailID = 0;
                    oDyeingOrderReports.Add(oDyeingOrderReport);
                }

            }
            catch (Exception ex)
            {
                oDyeingOrderReports = new List<DyeingOrderReport>();
                //_oDyeingOrder.ErrorMessage = ex.Message;
                //_oDyeingOrders.Add(_oDyeingOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}