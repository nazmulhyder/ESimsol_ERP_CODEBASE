using System;
using System.Collections.Generic;
using System.Dynamic;
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
using System.Collections;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class FabricDeliveryOrderController : PdfViewController
    {
        #region Declaration
        FabricDeliveryOrder _oFabricDeliveryOrder = new FabricDeliveryOrder();
        FabricDeliveryOrderDetail _oFDODetail = new FabricDeliveryOrderDetail();

        List<FabricDeliveryOrder> _oFDOrders = new List<FabricDeliveryOrder>();
        List<FabricDeliveryOrderDetail> _oFDODetails = new List<FabricDeliveryOrderDetail>();
        FabricSCReport oFabricSCReport = new FabricSCReport();
        static bool _tab=false;
        string _sSQL = "";
        #endregion

        #region   Fabric Execution Orders
        public ActionResult ViewFabricDeliveryOrders(bool tab,int buid, int menuid)
        {
            _tab = tab;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            _oFDOrders = new List<FabricDeliveryOrder>();
            string sSQL = "SELECT TOP 50 * FROM View_FabricDeliveryOrder WHERE DOStatus IN (" + (int)EnumDOStatus.Initialized + "," + (int)EnumDOStatus.RequestForApproved + ") AND ApproveBy<=0 ORDER BY FDOID DESC";
            _oFDOrders = FabricDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BaseCurrencyID = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID).BaseCurrencyID;
            ViewBag.BUID = buid;
          
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            //FDOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.FDOTypes = FDOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.FDOTypes = EnumObject.jGets(typeof(EnumDOType));
            //ViewBag.FDOTypes = DOTypeObj.Gets().Where(o => o.id != (int)EnumDOType.YarnTransfer_DO && o.id != (int)EnumDOType.YarnTransferForDEO); // Enum.GetValues(typeof(EnumFDOType)).Cast<EnumFDOType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oFDOrders);
        }
        public ActionResult ViewFabricDeliveryOrder(int nID, int buid, double ts)
        {
            _oFabricDeliveryOrder = new FabricDeliveryOrder();

            List<FDONote> oFDONotes = new List<FDONote>();
            List<FabricDeliveryOrder> oBCPs = new List<FabricDeliveryOrder>();

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);

            _oFabricDeliveryOrder = _oFabricDeliveryOrder.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oFabricDeliveryOrder.FDOID > 0)
            {
                oFDONotes = FDONote.GetByOrderID(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDODetails = FabricDeliveryOrderDetail.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDODetails.ForEach(o => o.Qty_Meter = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
                if(_oFDODetails!=null && _oFDODetails.Count>0)
                {
                    _oFabricDeliveryOrder.ContractorName = _oFDODetails[0].ContractorName;
                    _oFabricDeliveryOrder.BuyerName = _oFDODetails[0].BuyerName;
                    _oFabricDeliveryOrder.IssueToID = _oFDODetails[0].ContractorID;
                    _oFabricDeliveryOrder.BuyerID = _oFDODetails[0].BuyerID;
                }
                oBCPs.Add(_oFabricDeliveryOrder);
            }

            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.tab = _tab;
            ViewBag.BUID = buid;
            ViewBag.BCPs = oBCPs;
            ViewBag.FDONotes = oFDONotes;
            //ViewBag.FDOTypes = EnumObject.jGets(typeof(EnumDOType)).Where(x => x.id != (int)EnumDOType.None);
            ViewBag.FDOTypes = FDOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BaseCurrencyID = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID).BaseCurrencyID;
            ViewBag.FDODetails = _oFDODetails;
            return View(_oFabricDeliveryOrder);
        }

        public ActionResult View_FabricDeliveryOrders(int buid, int menuid)
        {
            //_tab = tab;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oFDOrders = new List<FabricDeliveryOrder>();
            string sSQL = "SELECT TOP 50 * FROM View_FabricDeliveryOrder WHERE DOStatus IN (" + (int)EnumDOStatus.Initialized + "," + (int)EnumDOStatus.RequestForApproved + ") AND ApproveBy<=0 ORDER BY FDOID DESC";
            _oFDOrders = FabricDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BaseCurrencyID = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID).BaseCurrencyID;
            ViewBag.BUID = buid;

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            //FDOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.FDOTypes = FDOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.FDOTypes = EnumObject.jGets(typeof(EnumDOType));
            //ViewBag.FDOTypes = DOTypeObj.Gets().Where(o => o.id != (int)EnumDOType.YarnTransfer_DO && o.id != (int)EnumDOType.YarnTransferForDEO); // Enum.GetValues(typeof(EnumFDOType)).Cast<EnumFDOType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oFDOrders);
        }
        public ActionResult View_FabricDeliveryOrder(int nID, int buid, double ts)
        {
            _oFabricDeliveryOrder = new FabricDeliveryOrder();

            List<FDONote> oFDONotes = new List<FDONote>();
            List<FabricDeliveryOrder> oBCPs = new List<FabricDeliveryOrder>();

            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);

            _oFabricDeliveryOrder = _oFabricDeliveryOrder.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oFabricDeliveryOrder.FDOID > 0)
            {
                oFDONotes = FDONote.GetByOrderID(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDODetails = FabricDeliveryOrderDetail.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDODetails.ForEach(o => o.Qty_Meter = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
                if (_oFDODetails != null && _oFDODetails.Count > 0)
                {
                    _oFabricDeliveryOrder.ContractorName = _oFDODetails[0].ContractorName;
                    _oFabricDeliveryOrder.BuyerName = _oFDODetails[0].BuyerName;
                    _oFabricDeliveryOrder.IssueToID = _oFDODetails[0].ContractorID;
                    _oFabricDeliveryOrder.BuyerID = _oFDODetails[0].BuyerID;
                }
                oBCPs.Add(_oFabricDeliveryOrder);
            }

            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.tab = _tab;
            ViewBag.BUID = buid;
            ViewBag.BCPs = oBCPs;
            ViewBag.FDONotes = oFDONotes;
            //ViewBag.FDOTypes = EnumObject.jGets(typeof(EnumDOType)).Where(x => x.id != (int)EnumDOType.None);
            ViewBag.FDOTypes = FDOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BaseCurrencyID = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID).BaseCurrencyID;
            ViewBag.FDODetails = _oFDODetails;
            return View(_oFabricDeliveryOrder);
        }
      
        [HttpPost]
        public JsonResult Save(FabricDeliveryOrder oFDOrder)
        {
            FDONote oFDONote = new FDONote();
            List<FDONote> oFDONotes = new List<FDONote>();
            try
            {
                oFDONotes=oFDOrder.FDONotes;
                oFDOrder.DeliveryPoint = (oFDOrder.DeliveryPoint == null) ? "" : oFDOrder.DeliveryPoint;
                oFDOrder.Note = (oFDOrder.Note == null) ? "" : oFDOrder.Note;
                oFDOrder.FDOType = (EnumDOType)oFDOrder.FDOTypeInInt;
                oFDOrder = oFDOrder.IUD((oFDOrder.FDOID <= 0 ? (int)EnumDBOperation.Insert : (int)EnumDBOperation.Update), ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (string.IsNullOrEmpty(oFDOrder.ErrorMessage)) 
                {
                    foreach (FDONote oItem in oFDONotes)
                    {
                        oItem.FDOID = oFDOrder.FDOID;
                    }
                    oFDONote.FDONotes = oFDONotes;
                    oFDOrder.ErrorMessage = oFDONote.SaveAll(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFDOrder = new FabricDeliveryOrder();
                oFDOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approved(FabricDeliveryOrder oFabricDeliveryOrder)
        {
            oFabricDeliveryOrder.RemoveNulls();
            try
            {

                _oFabricDeliveryOrder = oFabricDeliveryOrder;
             

                    _oFabricDeliveryOrder = _oFabricDeliveryOrder.Approved(((User)Session[SessionInfo.CurrentUser]).UserID);

               
            }
            catch (Exception ex)
            {
                _oFabricDeliveryOrder = new FabricDeliveryOrder();
                _oFabricDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Checked(FabricDeliveryOrder oFabricDeliveryOrder)
        {
            oFabricDeliveryOrder.RemoveNulls();
            try
            {
                _oFabricDeliveryOrder = oFabricDeliveryOrder;
                _oFabricDeliveryOrder = _oFabricDeliveryOrder.Checked(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oFabricDeliveryOrder = new FabricDeliveryOrder();
                _oFabricDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CancelFDO(FabricDeliveryOrder oFabricDeliveryOrder)
        {
            oFabricDeliveryOrder.RemoveNulls();
            try
            {
                _oFabricDeliveryOrder = oFabricDeliveryOrder;
                _oFabricDeliveryOrder = _oFabricDeliveryOrder.Cancel(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricDeliveryOrder = new FabricDeliveryOrder();
                _oFabricDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveLog(FabricDeliveryOrder oFabricDeliveryOrder)
        {
            oFabricDeliveryOrder.RemoveNulls();
            try
            {
                _oFabricDeliveryOrder = oFabricDeliveryOrder;
                _oFabricDeliveryOrder.SCNo = _oFabricDeliveryOrder.SCNo.Trim();

                _oFabricDeliveryOrder = _oFabricDeliveryOrder.SaveLog(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricDeliveryOrder.FDODetails = FabricDeliveryOrderDetail.Gets(_oFabricDeliveryOrder.FDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricDeliveryOrder = new FabricDeliveryOrder();
                _oFabricDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }  

        [HttpPost]
        public JsonResult StatusUpdateFDO(FabricDeliveryOrder oFDOrder)
        {
            try
            {
                if (oFDOrder.FDOID <= 0) { throw new Exception("Please select an valid item."); }
                oFDOrder = oFDOrder.UpdateFDOStatus(oFDOrder.FDOID, oFDOrder.DOStatusInInt, oFDOrder.DeliveryPoint == null ? "" : oFDOrder.DeliveryPoint, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFDOrder.FDOID > 0)
                {
                    oFDOrder.FDODetails = FabricDeliveryOrderDetail.Gets(oFDOrder.FDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFDOrder = new FabricDeliveryOrder();
                oFDOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      
        [HttpPost]
        public JsonResult Delete(FabricDeliveryOrder oFabricDeliveryOrder)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricDeliveryOrder.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsExportPI(FabricDeliveryOrder oFabricDeliveryOrder)
        {
            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
            try
            {
                string sReturn = "";
                _sSQL = "";
                _sSQL = "SELECT top(130)* FROM View_ExportPI";

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BUID=" + oFabricDeliveryOrder.BUID;

                #region PO
                if (oFabricDeliveryOrder.IssueToID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID in (" + oFabricDeliveryOrder.IssueToID + ")";
                }
                #endregion
                #region PO
                if (oFabricDeliveryOrder.BuyerID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in (" + oFabricDeliveryOrder.BuyerID + ")";
                }
                #endregion
             
                #region PINo
                if (!String.IsNullOrEmpty(oFabricDeliveryOrder.PINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PINo LIKE '%" + oFabricDeliveryOrder.PINo + "%'";
                }
                #endregion
                #region L/CNo
                if (!String.IsNullOrEmpty(oFabricDeliveryOrder.LCNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExportLCNo LIKE '%" + oFabricDeliveryOrder.LCNo + "%'";
                }
                #endregion
                #region FDOTypeInInt
                if (oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.Export)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "PaymentType<>" + ((int)EnumPIPaymentType.NonLC).ToString() + " and PIStatus in (" + (int)EnumPIStatus.BindWithLC + ")";
                }
                #endregion
                #region FDOTypeInInt
                if (oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.PP_Sample)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PIStatus in (" + (int)EnumPIStatus.Approved + "," + (int)EnumPIStatus.PIIssue + "," + (int)EnumPIStatus.BindWithLC + " )";
                }
                #endregion
                #region FDOTypeInInt
                if (oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.Advance)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "PaymentType<>" + ((int)EnumPIPaymentType.NonLC).ToString() + " and PIStatus in (" + (int)EnumPIStatus.Initialized + "," + (int)EnumPIStatus.RequestForApproved + "," + (int)EnumPIStatus.Approved + "," + (int)EnumPIStatus.PIIssue + ")";
                }
                #endregion
                #region FDOTypeInInt
                if (oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.Local_PI)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "PaymentType=" + ((int)EnumPIPaymentType.NonLC).ToString() + " and PIStatus in (" + (int)EnumPIStatus.Initialized + "," + (int)EnumPIStatus.RequestForApproved + "," + (int)EnumPIStatus.Approved + "," + (int)EnumPIStatus.PIIssue + ")";
                }
                #endregion

                if (sReturn != "")
                { _sSQL = _sSQL + sReturn + " order by Issuedate,ContractorName,BuyerName desc"; }

                oExportPIs = ExportPI.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                FabricDeliveryOrderDetail oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                foreach (ExportPI oItem in oExportPIs)
                {
                    oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                    oFabricDeliveryOrderDetail.ExportPIID = oItem.ExportPIID;

                    oFabricDeliveryOrderDetail.PINo = oItem.PINo;
                    oFabricDeliveryOrderDetail.LCNo = oItem.ExportLCNo;
                    oFabricDeliveryOrderDetail.Qty = oItem.Qty;
                    oFabricDeliveryOrderDetail.ContractorID = oItem.ContractorID;
                    oFabricDeliveryOrderDetail.BuyerID = oItem.BuyerID;
                    oFabricDeliveryOrderDetail.ContractorName = oItem.ContractorName;
                    oFabricDeliveryOrderDetail.BuyerName = oItem.BuyerName;

                    oFabricDeliveryOrderDetails.Add(oFabricDeliveryOrderDetail);
                }
            }
            catch (Exception ex)
            {
                oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
                oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
                //oFabricDeliveryOrderDetails.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsDeliveryZone(DeliveryZone oDeliveryZone)
        {
            Contractor oContractor = new Contractor();
            List<DeliveryZone> oDeliveryZones = new List<DeliveryZone>();
            try
            {
                string sSQL = "Select * from DeliveryZone Where DeliveryZoneID<>0";

                if (!string.IsNullOrEmpty(oDeliveryZone.DeliveryZoneName))
                {
                    sSQL = sSQL + " And DeliveryZoneName Like '%" + oDeliveryZone.DeliveryZoneName.Trim() + "%'";
                }
                if (!string.IsNullOrEmpty(oDeliveryZone.ErrorMessage))
                {
                    sSQL = sSQL + "And DeliveryZoneID in  (Select DeliveryZoneID from FabricDeliveryOrder where ContractorID in (" + oDeliveryZone.ErrorMessage + ") )";
                }

                oDeliveryZones = DeliveryZone.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oDeliveryZones.Count <= 0) 
                {
                    oContractor = oContractor.Get( Convert.ToInt32(oDeliveryZone.ErrorMessage), ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oContractor.ContractorID > 0)
                    {
                        DeliveryZone oDeliveryZone_temp = new DeliveryZone()
                        {
                            DeliveryZoneID = 0,
                            DeliveryZoneName = oContractor.Address
                        };
                        oDeliveryZone_temp = oDeliveryZone_temp.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                        oDeliveryZones.Add(oDeliveryZone_temp);
                    }
                }

                if (oDeliveryZones.FirstOrDefault().ErrorMessage.Contains("Already Exists")) 
                {
                    sSQL = "Select * from DeliveryZone Where DeliveryZoneID<>0";
                    if (!string.IsNullOrEmpty(oContractor.Address))
                    {
                        sSQL = sSQL + " And DeliveryZoneName Like '%" + oContractor.Address.Trim() + "%'";
                    }
                    oDeliveryZones = DeliveryZone.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDeliveryZone = new DeliveryZone();
                oDeliveryZone.ErrorMessage = ex.Message;
                oDeliveryZones.Add(oDeliveryZone);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryZones);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsPO(FabricDeliveryOrder oFabricDeliveryOrder)
        {
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            FabricDeliveryOrderDetail oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            List<FabricDeliveryOrderDetail> oFDODDetails = new List<FabricDeliveryOrderDetail>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
         
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(oFabricDeliveryOrder.BUID, (int)Session[SessionInfo.currentUserID]);
           
            try
            {
                string sReturn = "";
                _sSQL = "";

                if (oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.Export || oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.Advance || oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.Local_PI || oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.PP_Sample)
                {
                    oExportPIDetails = ExportPIDetail.GetsByPI(oFabricDeliveryOrder.ExportPIID,((User)Session[SessionInfo.CurrentUser]).UserID);
                    _sSQL = "SELECT * FROM View_FabricSalesContractReport where FabricSalesContractDetailID in (Select OrderSheetDetailID from ExportPIdetail where OrderSheetDetailID>0 and ExportPIID=" + oFabricDeliveryOrder.ExportPIID + ")";
                    oFabricSCReports = FabricSCReport.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _sSQL = "SELECT * FROM View_FabricDeliveryOrderDetail where ExportPIID=" + oFabricDeliveryOrder.ExportPIID + " and FDOID in  (Select FDOID from FabricDeliveryOrder where isnull(DOStatus,0)<>8)";
                    oFDODDetails = FabricDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (ExportPIDetail oItem in oExportPIDetails)
                    {
                        oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                        oFabricDeliveryOrderDetail.FabricID = oItem.FabricID;
                        oFabricDeliveryOrderDetail.FabricNo = oItem.FabricNo;
                        oFabricDeliveryOrderDetail.ExportPIID = oItem.ExportPIID;
                        oFabricDeliveryOrderDetail.ExportPIDetailID = oItem.ExportPIDetailID;
                        oFabricDeliveryOrderDetail.PINo = oItem.PINo;
                        //oFabricDeliveryOrderDetail.LCNo = oItem.LCNo;
                        oFabricDeliveryOrderDetail.FEONo = oItem.FSCNo;
                        oFabricDeliveryOrderDetail.FEOID = oItem.OrderSheetDetailID;
                        if (oFDODDetails.Count > 0)
                        {
                            oFabricDeliveryOrderDetail.Qty_DO = Math.Round(oFDODDetails.Where(c => c.ExportPIDetailID == oItem.ExportPIDetailID && c.FDOID != oFabricDeliveryOrder.FDOID).Sum(x => x.Qty), 2);
                            oFabricDeliveryOrderDetail.Qty_DC = Math.Round(oFDODDetails.Where(c => c.ExportPIDetailID == oItem.ExportPIDetailID).Sum(x => x.Qty_DC), 2);
                        }
                        oFabricDeliveryOrderDetail.Qty_PI = Math.Round(oItem.Qty, 2);
                        oFabricDeliveryOrderDetail.Qty = Math.Round(oItem.Qty -oFabricDeliveryOrderDetail.Qty_DO);
                        oFabricDeliveryOrderDetail.Qty_Meter = oFabricDeliveryOrderDetail.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value);
                        //_oFDODetails.ForEach(o => o.Qty_Meter = (o.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value)));
                        //oFabricDeliveryOrderDetail.FEOID = oItem.FabricSalesContractDetailID;
                        oFabricDeliveryOrderDetail.ContractorID = oFabricDeliveryOrder.ContractorID;
                        oFabricDeliveryOrderDetail.BuyerID = oFabricDeliveryOrder.BuyerID;
                        oFabricDeliveryOrderDetail.ContractorName = oFabricDeliveryOrder.ContractorName;
                        oFabricDeliveryOrderDetail.BuyerName = oItem.BuyerName;
                        oFabricDeliveryOrderDetail.Construction = oItem.Construction;
                        //oFabricDeliveryOrderDetail.OrderType = (EnumFabricRequestType)oItem.OrderType;
                        oFabricDeliveryOrderDetail.MUID = oItem.MUnitID;
                        oFabricDeliveryOrderDetail.MUName = oItem.MUName;
                        oFabricDeliveryOrderDetail.Construction = oItem.Construction;
                        oFabricDeliveryOrderDetail.ColorInfo = oItem.ColorInfo;
                        oFabricDeliveryOrderDetail.StyleNo = oItem.StyleNo;
                        oFabricDeliveryOrderDetail.BuyerRef = oItem.BuyerReference;
                        //oFabricDeliveryOrderDetail.ExeNo = oItem.ExeNo;
                        //oFabricDeliveryOrderDetail.HLReference = oItem.HLReference;
                        //oFabricDeliveryOrderDetail.BCPID = oItem.ContactPersonnelID;
                        //oFabricDeliveryOrderDetail.BuyerCPName = oItem.BuyerCPName;
                        if (oFabricSCReports.Count > 0 && oItem.OrderSheetDetailID > 0)
                        {
                            oFabricDeliveryOrderDetail.ExeNo = oFabricSCReports.Where(x => x.FabricSalesContractDetailID == oItem.OrderSheetDetailID && x.FabricID == oItem.FabricID).FirstOrDefault().ExeNo;
                        }
                        //oFabricDeliveryOrderDetail.ContractorAddress = oItem.ContractorAddress;
                        if (oFabricDeliveryOrderDetail.Qty > 0)
                        {
                            oFabricDeliveryOrderDetails.Add(oFabricDeliveryOrderDetail);
                        }
                    }
                }
                else
                {
                    _sSQL = "SELECT top(130)* FROM View_FabricSalesContractReport";

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "Isnull(ApproveBy,0)<>0 "; /// and isnull(Qty,0)>isnull(Qty_DO,0)

                    #region PO
                    if (oFabricDeliveryOrder.IssueToID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ContractorID in (" + oFabricDeliveryOrder.IssueToID + ")";
                    }
                    #endregion
                    #region PO
                    if (oFabricDeliveryOrder.BuyerID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " BuyerID in (" + oFabricDeliveryOrder.BuyerID + ")";
                    }
                    #endregion
                    #region SCNO OR DISPO
                    if (!String.IsNullOrEmpty(oFabricDeliveryOrder.SCNo))
                    {
                        Global.TagSQL(ref sReturn);

                        if (oFabricDeliveryOrder.IsDisPo)
                            sReturn = sReturn + " ExeNo LIKE '%" + oFabricDeliveryOrder.SCNo + "%'";
                        else
                            sReturn = sReturn + " SCNoFull LIKE '%" + oFabricDeliveryOrder.SCNo + "%'";
                    }
                    #endregion
                    #region PO
                    if (!String.IsNullOrEmpty(oFabricDeliveryOrder.Params))
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "FabricSalesContractDetailID not in (" + oFabricDeliveryOrder.Params + ")";
                    }
                    #endregion
                    #region PINo
                    if (!String.IsNullOrEmpty(oFabricDeliveryOrder.PINo))
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " PINo LIKE '%" + oFabricDeliveryOrder.PINo + "%'";
                    }
                    #endregion
                    #region FDOTypeInInt
                    if (oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.Export)
                    {
                        Global.TagSQL(ref sReturn);
                        if (oFabricDeliveryOrder.ExportPIID > 0)
                        {
                            sReturn = sReturn + "OrderType in (3,2,9) and FabricSalesContractDetailID in (Select ExportPIDetail.OrderSheetDetailID from ExportPIDetail where ExportPIID=" + oFabricDeliveryOrder.ExportPIID + " and ExportPIID in (Select ExportPIID from ExportPI where ExportPI.PIStatus in (" + (int)EnumPIStatus.BindWithLC + ") ))";
                        }
                        else
                        {
                            sReturn = sReturn + "OrderType in (3,2,9) and FabricSalesContractDetailID in (Select ExportPIDetail.OrderSheetDetailID from ExportPIDetail where ExportPIID in (Select ExportPIID from ExportPI where ExportPI.PIStatus in (" + (int)EnumPIStatus.BindWithLC + ") ))";
                        }
                    }
                    #endregion
                    #region FDOTypeInInt
                    if (oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.PP_Sample)
                    {
                        Global.TagSQL(ref sReturn);
                        if (oFabricDeliveryOrder.ExportPIID > 0)
                        {
                            sReturn = sReturn + "OrderType in (3,2,9) and FabricSalesContractDetailID in (Select ExportPIDetail.OrderSheetDetailID from ExportPIDetail where ExportPIID=" + oFabricDeliveryOrder.ExportPIID + " and ExportPIID in (Select ExportPIID from ExportPI where ExportPI.PIStatus in (" + (int)EnumPIStatus.BindWithLC + ") ))";
                        }
                        else
                        {
                            sReturn = sReturn + "OrderType in (3,2,9) and FabricSalesContractDetailID in (Select ExportPIDetail.OrderSheetDetailID from ExportPIDetail where ExportPIID in (Select ExportPIID from ExportPI where ExportPI.PIStatus in (" + (int)EnumPIStatus.BindWithLC + ") ))";
                        }
                    }
                    #endregion
                    #region FDOTypeInInt
                    if (oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.Advance)
                    {
                        Global.TagSQL(ref sReturn);
                        if (oFabricDeliveryOrder.ExportPIID > 0)
                        {
                            sReturn = sReturn + "OrderType in (3,2,9) and FabricSalesContractDetailID in (Select ExportPIDetail.OrderSheetDetailID from ExportPIDetail where  ExportPIID=" + oFabricDeliveryOrder.ExportPIID + " and ExportPIID in (Select ExportPIID from ExportPI where ExportPI.PIStatus in (" + (int)EnumPIStatus.Initialized + "," + (int)EnumPIStatus.RequestForApproved + "," + (int)EnumPIStatus.Approved + "," + (int)EnumPIStatus.PIIssue + ") ))";
                        }
                        else
                        {
                            sReturn = sReturn + "OrderType in (3,2,9) and FabricSalesContractDetailID in (Select ExportPIDetail.OrderSheetDetailID from ExportPIDetail where  ExportPIID in (Select ExportPIID from ExportPI where ExportPI.PIStatus in (" + (int)EnumPIStatus.Initialized + "," + (int)EnumPIStatus.RequestForApproved + "," + (int)EnumPIStatus.Approved + "," + (int)EnumPIStatus.PIIssue + ") ))";
                        }
                    }
                    #endregion
                    #region FDOTypeInInt
                    if (oFabricDeliveryOrder.FDOTypeInInt == (int)EnumDOType.Sample)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "OrderType in (2,9) ";
                    }
                    #endregion

                    if (sReturn != "")
                    { _sSQL = _sSQL + sReturn + " order by SCDate,ContractorName,BuyerName desc"; }

                    oFabricSCReports = FabricSCReport.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _sSQL = "SELECT * FROM View_FabricDeliveryOrderDetail where ExportPIID=" + oFabricDeliveryOrder.ExportPIID;
                    oFDODDetails = FabricDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   
                    foreach (FabricSCReport oItem in oFabricSCReports)
                    {
                        oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                        oFabricDeliveryOrderDetail.FabricID = oItem.FabricID;
                        oFabricDeliveryOrderDetail.FabricNo = oItem.FabricNo;
                        oFabricDeliveryOrderDetail.ExportPIID = oFabricDeliveryOrder.ExportPIID;
                        ; oFabricDeliveryOrderDetail.PINo = oItem.PINo;
                        oFabricDeliveryOrderDetail.LCNo = oItem.LCNo;
                        oFabricDeliveryOrderDetail.FEONo = oItem.SCNoFull;
                        oFabricDeliveryOrderDetail.FEOID = oItem.FabricSalesContractDetailID;
                        if (oFDODDetails.Count > 0)
                        {
                            oFabricDeliveryOrderDetail.Qty_DO = Math.Round(oFDODDetails.Where(c => c.FEOID == oItem.FabricSalesContractDetailID && c.FabricID == oItem.FabricID && c.FDOID != oFabricDeliveryOrder.FDOID).Sum(x => x.Qty), 2);
                            oFabricDeliveryOrderDetail.Qty_DC = Math.Round(oFDODDetails.Where(c => c.FEOID == oItem.FabricSalesContractDetailID && c.FabricID == oItem.FabricID).Sum(x => x.Qty_DC), 2);
                        }
                        oFabricDeliveryOrderDetail.Qty_PI = oItem.Qty;
                        oFabricDeliveryOrderDetail.Qty = oItem.Qty - oFabricDeliveryOrderDetail.Qty_DO;
                        oFabricDeliveryOrderDetail.Qty_Meter = oFabricDeliveryOrderDetail.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value);
                        oFabricDeliveryOrderDetail.FEOID = oItem.FabricSalesContractDetailID;
                        oFabricDeliveryOrderDetail.ContractorID = oItem.ContractorID;
                        oFabricDeliveryOrderDetail.BuyerID = oItem.BuyerID;
                        oFabricDeliveryOrderDetail.ContractorName = oItem.ContractorName;
                        oFabricDeliveryOrderDetail.BuyerName = oItem.BuyerName;
                        oFabricDeliveryOrderDetail.Construction = oItem.Construction;
                        oFabricDeliveryOrderDetail.OrderType = (EnumFabricRequestType)oItem.OrderType;
                        oFabricDeliveryOrderDetail.MUID = oItem.MUnitID;
                        oFabricDeliveryOrderDetail.MUName = oItem.MUName;
                        oFabricDeliveryOrderDetail.Construction = oItem.Construction;
                        oFabricDeliveryOrderDetail.ColorInfo = oItem.ColorInfo;
                        oFabricDeliveryOrderDetail.StyleNo = oItem.StyleNo;
                        oFabricDeliveryOrderDetail.BuyerRef = oItem.BuyerReference;
                        oFabricDeliveryOrderDetail.HLReference = oItem.HLReference;
                        oFabricDeliveryOrderDetail.BCPID = oItem.ContactPersonnelID;
                        oFabricDeliveryOrderDetail.BuyerCPName = oItem.BuyerCPName;
                        oFabricDeliveryOrderDetail.ExeNo = oItem.ExeNo;
                        oFabricDeliveryOrderDetail.ContractorAddress = oItem.ContractorAddress;
                        if (oFabricDeliveryOrderDetail.Qty > 0)
                        {
                            oFabricDeliveryOrderDetails.Add(oFabricDeliveryOrderDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
                oFabricDeliveryOrderDetails=new List<FabricDeliveryOrderDetail>();
                //oFabricDeliveryOrderDetails.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchByExeNo(FabricDeliveryOrderDetail oFabricDeliveryOrderDetail)
        {
            _oFDOrders = new List<FabricDeliveryOrder>();
            _oFabricDeliveryOrder = new FabricDeliveryOrder();
            try
            {
                string sSQL = "SELECT TOP 200 * FROM View_FabricDeliveryOrder WHERE  FDOID IN (SELECT FDOID FROM View_FabricDeliveryOrderDetail WHERE ExeNo LIKE '%" + oFabricDeliveryOrderDetail.ExeNo + "%')";
                _oFDOrders = FabricDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFDOrders = new List<FabricDeliveryOrder>();
                _oFabricDeliveryOrder = new FabricDeliveryOrder();
                _oFabricDeliveryOrder.ErrorMessage = ex.Message;
                _oFDOrders.Add(_oFabricDeliveryOrder);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFDOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetsFormExcess(FabricDeliveryOrderDetail oFabricDeliveryOrderDetail)
        {
            List<FNOrderUpdateStatus> oFNOrderUpdateStatus = new List<FNOrderUpdateStatus>();
            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(oFabricDeliveryOrderDetail.BUID, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "AND Exeno like '%" +oFabricDeliveryOrderDetail.ExeNo+ "%' and FabricSalesContractDetailID in (Select FSCDID from FNFSCToDC where (isnull(ExcessQty,0)-isnull(ExcessDCQty,0))>0)";
                oFNOrderUpdateStatus = FNOrderUpdateStatus.Gets(sSQL, 0, DateTime.Now, DateTime.Now, (int)Session[SessionInfo.currentUserID]);

                foreach (FNOrderUpdateStatus oItem in oFNOrderUpdateStatus)
                {
                    oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                    oFabricDeliveryOrderDetail.FabricID = oItem.FabricID;
                    oFabricDeliveryOrderDetail.FabricNo = oItem.FabricNo;
                    oFabricDeliveryOrderDetail.ExportPIID = oFabricDeliveryOrderDetail.ExportPIID;
                    ; oFabricDeliveryOrderDetail.PINo = oFabricDeliveryOrderDetail.PINo;
                    oFabricDeliveryOrderDetail.LCNo = oFabricDeliveryOrderDetail.LCNo;
                    oFabricDeliveryOrderDetail.FEONo = oItem.SCNoFull;
                    oFabricDeliveryOrderDetail.FEOID = oItem.FSCDetailID;
                    //if (oFDODDetails.Count > 0)
                    //{
                    //    oFabricDeliveryOrderDetail.Qty_DO = Math.Round(oFDODDetails.Where(c => c.FEOID == oItem.FabricSalesContractDetailID && c.FabricID == oItem.FabricID && c.FDOID != oFabricDeliveryOrder.FDOID).Sum(x => x.Qty), 2);
                    //    oFabricDeliveryOrderDetail.Qty_DC = Math.Round(oFDODDetails.Where(c => c.FEOID == oItem.FabricSalesContractDetailID && c.FabricID == oItem.FabricID).Sum(x => x.Qty_DC), 2);
                    //}
                    oFabricDeliveryOrderDetail.Qty_PI = oItem.ExcessQty;
                    oFabricDeliveryOrderDetail.Qty = oItem.ExcessQty - oItem.ExcessDCQty;
                    oFabricDeliveryOrderDetail.Qty_Meter = oFabricDeliveryOrderDetail.Qty * (oMeasurementUnitCon.Value == 0 ? 1 : oMeasurementUnitCon.Value);
                    //oFabricDeliveryOrderDetail.FEOID = oItem.FSCDetailID;
                    oFabricDeliveryOrderDetail.ContractorID = oItem.ContractorID;
                    oFabricDeliveryOrderDetail.BuyerID = oItem.BuyerID;
                    oFabricDeliveryOrderDetail.ContractorName = oItem.ContractorName;
                    oFabricDeliveryOrderDetail.BuyerName = oItem.BuyerName;
                    oFabricDeliveryOrderDetail.Construction = oItem.Construction;
                    oFabricDeliveryOrderDetail.OrderType = (EnumFabricRequestType)oItem.OrderType;
                    oFabricDeliveryOrderDetail.MUID = oMeasurementUnitCon.FromMUnitID;
                    oFabricDeliveryOrderDetail.MUName = oMeasurementUnitCon.FromMUnit;
                    oFabricDeliveryOrderDetail.Construction = oItem.Construction;
                    oFabricDeliveryOrderDetail.ColorInfo = oItem.ColorInfo;
                    oFabricDeliveryOrderDetail.StyleNo = "";
                    oFabricDeliveryOrderDetail.BuyerRef = "";
                    oFabricDeliveryOrderDetail.HLReference = "";
                    oFabricDeliveryOrderDetail.BCPID =0;
                    oFabricDeliveryOrderDetail.BuyerCPName ="";
                    oFabricDeliveryOrderDetail.ExeNo = oItem.ExeNo;
                    oFabricDeliveryOrderDetail.ContractorAddress = "";
                    if (oFabricDeliveryOrderDetail.Qty > 0)
                    {
                        oFabricDeliveryOrderDetails.Add(oFabricDeliveryOrderDetail);
                    }
                }

            }
            catch (Exception ex)
            {

                oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                oFabricDeliveryOrderDetail.ErrorMessage = ex.Message;
                oFabricDeliveryOrderDetails.Add(oFabricDeliveryOrderDetail);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
        public JsonResult SearchByDONo(FabricDeliveryOrder oFabricDeliveryOrder)
        {
            _oFDOrders = new List<FabricDeliveryOrder>();

            string sReturn = "";
            _sSQL = "";
            _sSQL = "SELECT * FROM View_FabricDeliveryOrder";

            #region DO
            if (!String.IsNullOrEmpty(oFabricDeliveryOrder.DONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DONo LIKE '%" + oFabricDeliveryOrder.DONo + "%'";
            }
            #endregion
            #region PO/SC
            if (!String.IsNullOrEmpty(oFabricDeliveryOrder.SCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FDOID in (SELECT FDOID FROM View_FabricDeliveryOrderDetail  where SCNoFull LIKE '%" + oFabricDeliveryOrder.SCNo + "%')";
            }
            #endregion
            _sSQL = _sSQL + "" + sReturn;
            try
            {
                _oFDOrders = FabricDeliveryOrder.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFDOrders = new List<FabricDeliveryOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFDOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetsApprovedFDO(FabricDeliveryOrder oFDOrder)
        {
            _oFDOrders = new List<FabricDeliveryOrder>();
            try
            {
                string sDONo = (oFDOrder.Params != null) ? oFDOrder.Params.Split('~')[0].Trim() : "";
                string sSQL = "Select * from View_FabricDeliveryOrder Where FDOID <> 0 AND DOStatus IN (" + (int)EnumDOStatus.Approved + ")";
                if (sDONo != "") { sSQL = sSQL + " and DONo Like '%" + sDONo + "%'"; }
                sSQL = sSQL + " order by FDOID";
                _oFDOrders = FabricDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFDOrders = new List<FabricDeliveryOrder>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFDOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult UpdateFinish(FabricDeliveryOrder oFDO)
        {
            try
            {
                bool bIsFinish = (oFDO.IsFinish == true ? false : true);
                if (oFDO.FDOID > 0)
                {
                    oFDO = oFDO.UpdateFinish(oFDO.FDOID, bIsFinish, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFDO.ErrorMessage = "Invalid Fabric Delivery Order.";
                }
            }
            catch (Exception ex)
            {
                oFDO = new FabricDeliveryOrder();
                oFDO.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDO);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region FDODetail
        [HttpPost]
        public JsonResult SaveFDOD(FabricDeliveryOrderDetail oFDODetail)
        {
            try
            {
                _oFDODetail = oFDODetail.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (oFDODetail.ExportLCID > 0)
                //{
                //    string sSQL = "SELECT * FROM View_FabricDeliveryOrder WHERE FDOID IN (SELECT FDOID FROM FabricDeliveryOrderDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE LCID = " + oFDODetail.ExportLCID + "))";
                //    _oFDOrder.FDOs = FabricDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
            }
            catch (Exception ex)
            {
                _oFDODetail = new FabricDeliveryOrderDetail();
                _oFDODetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFDODetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      

        [HttpPost]
        public JsonResult DeleteFDOD(FabricDeliveryOrderDetail oFabricDeliveryOrderDetail)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricDeliveryOrderDetail.Delete(oFabricDeliveryOrderDetail.FDODID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public ActionResult GetsFDOD(FabricDeliveryOrderDetail oFDOrderDetail)
        {
            _oFDODetails = new List<FabricDeliveryOrderDetail>();
            try
            {
                if (oFDOrderDetail.FDOID <= 0) { throw new Exception("No fabric delivery order found to search."); }
                string sSQL = "SELECT * FROM View_FabricDeliveryOrderDetail WHERE FDOID = " + oFDOrderDetail.FDOID + " AND YetToDeliveryQty>0";
                _oFDODetails = FabricDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFDOrderDetail = new FabricDeliveryOrderDetail();
                oFDOrderDetail.ErrorMessage = ex.Message;
                _oFDODetails = new List<FabricDeliveryOrderDetail>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFDODetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public ActionResult PrintabricDeliveryOrderPreview(int nFDOID, int nBUID, bool bIsMeter)
        {
            bool bPrintFormat = true;
            string sExportPIIDs = "";
            #region Note
            /*
             *  bPrintFormat == true means in yard
             *  bPrintFormat == false measn in meter
             *  
             *  bIsInText == true means nomral pdf header
             *  bIsInText == false means pdf header in Image Format
             *  
             */

            #endregion

            string sSQL = "";
            _oFabricDeliveryOrder = new FabricDeliveryOrder();
            _oFabricDeliveryOrder = _oFabricDeliveryOrder.Get(nFDOID, (int)Session[SessionInfo.currentUserID]);
            _oFabricDeliveryOrder.FDODetails = FabricDeliveryOrderDetail.Gets(nFDOID, (int)Session[SessionInfo.currentUserID]);
            _oFabricDeliveryOrder.FDONotes = FDONote.GetByOrderID(nFDOID, (int)Session[SessionInfo.currentUserID]);
            if(_oFabricDeliveryOrder.FDODetails.Count>0)
            {
                _oFabricDeliveryOrder.ContractorName = _oFabricDeliveryOrder.FDODetails[0].ContractorName;
                _oFabricDeliveryOrder.BuyerName = _oFabricDeliveryOrder.FDODetails[0].BuyerName;
                _oFabricDeliveryOrder.MKTPerson = _oFabricDeliveryOrder.FDODetails[0].MKTPerson;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(oBusinessUnit.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            oFDOrderSetup = oFDOrderSetup.GetByType(_oFabricDeliveryOrder.FDOTypeInInt, (int)Session[SessionInfo.currentUserID]);
            
            #region Export PI
            List<ExportPI> oExportPIs = new List<ExportPI>();
            sExportPIIDs = string.Join(",", _oFabricDeliveryOrder.FDODetails.Select(x => x.ExportPIID).ToList());
            if(!string.IsNullOrEmpty(sExportPIIDs))
            {
            sSQL = "SELECT * FROM View_ExportPI WHERE ExportPIID IN (" + sExportPIIDs + ")";
            oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            #endregion

            if (bIsMeter)
            {
               // _oFabricDeliveryOrder.FDODetails.ForEach(o => o.Qty = (o.Qty * oMeasurementUnitCon.Value));

                _oFabricDeliveryOrder.FDODetails.ForEach(o => o.Qty = (o.Qty * (oMeasurementUnitCon.Value == 0 ?1: oMeasurementUnitCon.Value )));
                _oFabricDeliveryOrder.FDODetails.ForEach(o => o.MUName = "Meter");
            }
            else
            {
                _oFabricDeliveryOrder.FDODetails.ForEach(o => o.MUName = "Yards");
            }

            if (oFDOrderSetup.PrintNo == (int)EnumExcellColumn.B)
            {
                rptFabricDeliveryOrder_B oReport = new rptFabricDeliveryOrder_B();
                byte[] abytes = oReport.PrepareReport(_oFabricDeliveryOrder, oCompany, oBusinessUnit, bPrintFormat, oExportPIs);
                return File(abytes, "application/pdf");
            }
            else 
            {
                rptFabricDeliveryOrder oReport = new rptFabricDeliveryOrder();
                byte[] abytes = oReport.PrepareReport(_oFabricDeliveryOrder, oCompany, oBusinessUnit, bPrintFormat, oExportPIs);
                return File(abytes, "application/pdf");
            }
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
  

        public Image GetSignature(UserImage oUserImage)
        {
            if (oUserImage.ImageFile != null)
            {
                MemoryStream m = new MemoryStream(oUserImage.ImageFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "SignatureImage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "CompanyImageTitle.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

      
        #endregion

        #region Adv Search
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult AdvSearch(string sTemp)
        {
            List<FabricDeliveryOrder> oFabricDeliveryOrders = new List<FabricDeliveryOrder>();
            try
            {
                string sSQL = MakeSQL(sTemp);
                oFabricDeliveryOrders = FabricDeliveryOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFabricDeliveryOrders = new List<FabricDeliveryOrder>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(string sTemp)
        {

            int ncboDODate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime txtDODateFrom = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime txtDODateTo = Convert.ToDateTime(sTemp.Split('~')[2]);
            //
            string sFDONo = Convert.ToString(sTemp.Split('~')[3]);
            string sSCNo = Convert.ToString(sTemp.Split('~')[4]);
            string sApplicantIDs = Convert.ToString(sTemp.Split('~')[5]);
            string sPINo = Convert.ToString(sTemp.Split('~')[6]);
            string sLCNo = Convert.ToString(sTemp.Split('~')[7]);
            int nDOType = Convert.ToInt32(sTemp.Split('~')[8]);
            string MKTPersonIDs = Convert.ToString(sTemp.Split('~')[9]);
            string MKTGroupIDs = Convert.ToString(sTemp.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_FabricDeliveryOrder ";
            string sReturn = "";

            #region DO Type
            if (nDOType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOType =" + nDOType;
            }
            #endregion

            #region DO No
            if (!string.IsNullOrEmpty(sFDONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DONo LIKE '%" + sFDONo + "%' ";
            }
            #endregion

            #region SC No
            if (!string.IsNullOrEmpty(sSCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOID IN (SELECT FDOD1.FDOID FROM View_FabricDeliveryOrderDetail AS FDOD1 WHERE FDOD1.SCNoFull LIKE '%" + sSCNo + "%')";
            }
            #endregion

            #region PI No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOID IN (SELECT FF.FDOID FROM View_FabricDeliveryOrderDetail AS FF WHERE FF.PINo LIKE '%" + sPINo + "%')";
            }
            #endregion

            #region LC No
            if (!string.IsNullOrEmpty(sLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOID IN (SELECT FF.FDOID FROM View_FabricDeliveryOrderDetail AS FF WHERE FF.ExportPIID IN ( SELECT DD.ExportPIID  FROM View_ExportPI AS DD WHERE DD.ExportLCNo LIKE '%" + sLCNo + "%'))";
            }
            #endregion

            #region Delivery To
            if (!string.IsNullOrEmpty(sApplicantIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID in( " + sApplicantIDs + ")";
            }
            #endregion

            #region Issue Date Wise
            if (ncboDODate > 0)
            {
                if (ncboDODate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDODateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDODate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDODateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDODate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDODateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDODate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDODateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDODate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDODateFrom.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDODateTo.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboDODate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDODateFrom.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtDODateTo.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region MKTPerson ID
            if (!string.IsNullOrEmpty(MKTPersonIDs))
            {
                Global.TagSQL(ref sReturn);
                //sReturn = sReturn + " FDOID IN (SELECT FDOID FROM View_FabricDeliveryOrderDetail WHERE MKTPersonID IN (" + MKTPersonIDs + ")) ";
                sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricDeliveryOrderDetail WHERE ExportPIID>0 and ExportPIID in (Select ExportPIID from  ExportPI where MKTEMPID in  (" + MKTPersonIDs + ")) OR (isnull(ExportPIID,0)<=0 and FEOID in (Select FabricSalesContractDetailID from  FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where MktAccountID in  (" + MKTPersonIDs + ")))))";
            }
            #endregion

            #region MKTGroup ID
            if (!string.IsNullOrEmpty(MKTGroupIDs))
            {
                Global.TagSQL(ref sReturn);
                //sReturn = sReturn + " FDOID IN (SELECT FDOID FROM View_FabricDeliveryOrderDetail WHERE MKTPersonID IN (Select MarketingAccountID  from View_MarketingAccount where GroupID >0 and GroupID in (" + MKTGroupIDs + "))) ";
                sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricDeliveryOrderDetail WHERE ExportPIID>0 and ExportPIID in (Select ExportPIID from  ExportPI where MKTEMPID in  (Select MarketingAccountID from MarketingAccount where GroupID in (" + MKTGroupIDs + "))) OR (isnull(ExportPIID,0)<=0 and FEOID in (Select FabricSalesContractDetailID from  FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where MktAccountID in  (Select MarketingAccountID from MarketingAccount where GroupID in (" + MKTGroupIDs + "))))))";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " ORDER BY FDOID";
            return sSQL;
        }

        #endregion

        #region Fabric DO Report
        public ActionResult View_FDOReport(int buid, int menuid)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PIPaymentTypes = EnumObject.jGets(typeof(EnumPIPaymentType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); 
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIStatusObjs = EnumObject.jGets(typeof(EnumPIStatus));
            ViewBag.DepthOfShades = EnumObject.jGets(typeof(EnumDepthOfShade));
            ViewBag.BankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BU = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oFDODetails);
        }

        [HttpPost]
        public JsonResult AdvSearch_Report(FabricDeliveryOrderDetail oFabricDeliveryOrderDetail)
        {
            _oFDODetails = new List<FabricDeliveryOrderDetail>();
            try
            {
                string sSQL = MakeSQL(oFabricDeliveryOrderDetail);
                _oFDODetails = FabricDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFDODetails = new List<FabricDeliveryOrderDetail>();
                _oFDODetail.ErrorMessage = ex.Message;
                _oFDODetails.Add(_oFDODetail);
            }
            var jsonResult = Json(_oFDODetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FabricDeliveryOrderDetail oFabricDeliveryOrderDetail)
        {
            _oFDODetail = oFabricDeliveryOrderDetail;
            string sParams = oFabricDeliveryOrderDetail.ErrorMessage;

            //int nCount = 6;
            int nCboIssueDate = 0;
            DateTime dFromIssueDate = DateTime.Today;
            DateTime dToIssueDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                nCboIssueDate = Convert.ToInt32(sParams.Split('~')[9]);
                dFromIssueDate = Convert.ToDateTime(sParams.Split('~')[10]);
                dToIssueDate = Convert.ToDateTime(sParams.Split('~')[11]);
            }


            string sReturn1 = "SELECT * FROM View_FDODetailReport AS FDOD ";
            string sReturn = "";
    
            #region DONo
            if (!String.IsNullOrEmpty(_oFDODetail.DONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.DONo LIKE '%" + _oFDODetail.DONo + "%'";
            }
            #endregion

            #region PINo
            if (!String.IsNullOrEmpty(_oFDODetail.PINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.PINo LIKE '%" + _oFDODetail.PINo + "%'";
            }
            #endregion

            #region StyleNo
            if (!String.IsNullOrEmpty(_oFDODetail.StyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.StyleNo LIKE '%" + _oFDODetail.StyleNo + "%'";
            }
            #endregion

            #region FabricNo
            if (!String.IsNullOrEmpty(_oFDODetail.FabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.FabricNo LIKE '%" + _oFDODetail.FabricNo + "%'";
            }
            #endregion

            #region ExeNo
            if (!String.IsNullOrEmpty(_oFDODetail.ExeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.SCNoFull LIKE '%" + _oFDODetail.ExeNo + "%'";
            }
            #endregion

            #region ProductName
            if (!String.IsNullOrEmpty(_oFDODetail.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.ProductName in(" + _oFDODetail.ProductName + ")";
            }
            #endregion

            #region Contractor
            if (!String.IsNullOrEmpty(_oFDODetail.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.ContractorID in(" + _oFDODetail.ContractorName + ")";
            }
            #endregion

            #region Buyer Id
            if (!String.IsNullOrEmpty(_oFDODetail.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.BuyerID in(" + _oFDODetail.BuyerName + ")";
            }
            #endregion

            #region Issue Date
            ////////////akram added
            DateObject.CompareDateQuery(ref sReturn, "DODate", nCboIssueDate, dFromIssueDate, dToIssueDate);
            ///////////////////
           
            #endregion

            #region Mkt. Person
            if (!String.IsNullOrEmpty(_oFDODetail.MKTPerson))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.MktAccountID IN ( " + _oFDODetail.MKTPerson + ")";
            }
            #endregion

            #region Business Unit
            if (_oFDODetail.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOD.BUID = " + _oFDODetail.BUID;
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn;// +" ORDER BY IssueDate DESC";
            return sSQL;
        }

        #region Print Excel

        public List<TableHeader> GetHeader()
        {
            #region Header
            List<TableHeader> table_headers = new List<TableHeader>();
            table_headers.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "DO Date", Width = 25f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "DO No", Width = 25f, IsRotate = false });

            table_headers.Add(new TableHeader { Header = "PI No", Width = 25f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "DisPo No", Width = 25f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Style No", Width = 25f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Fabric No", Width = 25f, IsRotate = false });

            table_headers.Add(new TableHeader { Header = "Customer Name", Width = 45f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Buyer Name", Width = 45f, IsRotate = false });

            table_headers.Add(new TableHeader { Header = "Product Code", Width = 20f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Compossion", Width = 45f, IsRotate = false });

          
            table_headers.Add(new TableHeader { Header = "M Unit", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Qty", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Production Qty", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Yet To Challan", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Yet To PO", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Challan Qty", Width = 20, IsRotate = false });

            #endregion

            return table_headers;
        }
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(FabricDeliveryOrderDetail oFabricDeliveryOrderDetail)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricDeliveryOrderDetail);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ExportToExcel_FDO(double ts)
        {
            int ProductionSheetID = -999;
            string Header = "", HeaderColumn = "", _sErrorMesage = "";

            #region Get Data From DB
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            FabricDeliveryOrderDetail oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
            try
            {
                //oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDODetails = new List<FabricDeliveryOrderDetail>();
                oFabricDeliveryOrderDetail = (FabricDeliveryOrderDetail)Session[SessionInfo.ParamObj];
                string sSQL = this.MakeSQL(oFabricDeliveryOrderDetail);
                _oFDODetails = FabricDeliveryOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFDODetails.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFDODetails = new List<FabricDeliveryOrderDetail>();
                _sErrorMesage = ex.Message;
            }
            #endregion

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header = GetHeader();
                #endregion

                #region Layout Wise Header
                //if (oFabricDeliveryOrderDetail.ReportLayout == EnumReportLayout.ProductWise)
                //{
                //    Header = "Product Wise"; HeaderColumn = "Product Name : ";
                //}
               
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Delivery Report");
                    sheet.Name = "Fabric Delivery Report";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    //cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Fabric Delivery Report (" + Header + ") "; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    //cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Group By Layout Wises
                    //List<FabricDeliveryOrderDetail> GroupWiseData = new List<FabricDeliveryOrderDetail>();

                    //if (oFabricDeliveryOrderDetail.ReportLayout == EnumReportLayout.DateWise)
                    //{
                     //var  GroupWiseData = _oFDODetails.GroupBy(x => new { x.TransactionDateSt }, (key, grp) =>
                     //   {
                     //       RowHeader = key.TransactionDateSt,
                     //       MoldingProduction = grp.Sum(x => x.MoldingProduction),
                     //       Results = grp.ToList()
                     //   }).ToList();
                    //}
                   
                    #endregion

                    ExcelTool.Formatter = " #,##0.00;(#,##0.00)";

                    #region Data
                    //foreach (var oItem in _oFDODetails)
                    //{
                        nRowIndex++;

                        nStartCol = 2;
                        //ExcelTool.FillCellMerge(ref sheet, HeaderColumn + oItem.RowHeader, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        nRowIndex++;
                        foreach (TableHeader listItem in table_header)
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        ProductionSheetID = 0;
                        nRowIndex++; int nCount = 0, nRowSpan = 0;
                        foreach (var obj in _oFDODetails)
                        {
                            #region Sheet Wise Merge
                            //if (ProductionSheetID != obj.ProductionSheetID)
                            //{
                                //if (nCount > 0)
                                //{
                                //    nStartCol = _nSubTotalColumn;
                                //    ExcelTool.FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                                //    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, false);
                                //    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.MoldingProduction).ToString(), true, true);
                                //    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                                //    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                                //    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                                //    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                                //    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);

                                //    nRowIndex++;
                                //}

                                nStartCol = 2;
                                nRowSpan = 0; //oItem.Results.Where(Sheet => Sheet.ProductionSheetID == obj.ProductionSheetID).ToList().Count;

                                ExcelTool.FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.DODateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.DONo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                ExcelTool.FillCellMerge(ref sheet, obj.PINo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.ExeNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.StyleNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.FabricNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                //if (oFabricDeliveryOrderDetail.ReportLayout != EnumReportLayout.ProductWise)
                                //{
                                ExcelTool.FillCellMerge(ref sheet, obj.ContractorName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.BuyerName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.ProductCode, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.ProductName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.MUName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                //}

                                //ExcelTool.FillCellMerge(ref sheet, obj.UnitSymbol, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            //}
                            #endregion


                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_P0.ToString(), true);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (obj.Qty_P0 - obj.Qty_DC).ToString(), true);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (obj.Qty - obj.Qty_P0).ToString(), true);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_DC.ToString(), true);

                            //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ActualMoldingProduction.ToString(), true);
                            //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ActualFinishGoods.ToString(), true);
                            //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ActualRejectGoods.ToString(), true);
                            //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.YetToProduction.ToString(), true);

                           
                            nRowIndex++;

                            //ProductionSheetID = obj.ProductionSheetID;
                        }
                        #region SubTotal
                        //ExcelTool.FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, false);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.MoldingProduction).ToString(), true, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //nRowIndex++;
                        #endregion

                        #region SubTotal (layout Wise)
                        //nStartCol = 2;
                        //ExcelTool.Formatter = " #,##0.00;(#,##0.00)";

                        //ExcelTool.FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += _nSubTotalColumn, true, ExcelHorizontalAlignment.Right);

                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, false);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, oItem.MoldingProduction.ToString(), true, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        //nRowIndex++;
                        #endregion
                    //}
                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                    ExcelTool.FillCellMerge(ref sheet, Header + " Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 11, true, ExcelHorizontalAlignment.Right);
                  
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, _oFDODetails.Sum(x => x.Qty).ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, _oFDODetails.Sum(x => x.Qty_P0).ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, _oFDODetails.Sum(x => x.Qty_DC).ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, _oFDODetails.Sum(x => x.Qty - x.Qty_P0).ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, _oFDODetails.Sum(x => x.Qty_P0 - x.Qty_DC).ToString(), true, true);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Fabric Delivery Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        #endregion

        /*
          JavaScriptSerializer json_serializer = new JavaScriptSerializer();
          Test routes_list = 
          (Test)json_serializer.DeserializeObject("{ \"test\":\"some data\" }");
         */
        #endregion
    }
}