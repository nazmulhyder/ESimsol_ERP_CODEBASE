using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class PartsReceivedController : Controller
    {
        #region Declaration
        GRN _oGRN = new GRN();
        List<GRN> _oGRNs = new List<GRN>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        #endregion
        #region Actions
        public ActionResult ViewPartsReceiveds(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID])); 
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            _oGRNs = new List<GRN>();
            string sSQL = "SELECT * FROM View_GRN AS HH WHERE ISNULL(HH.ApproveBy,0)=0 ";
            if (buid>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " AND BUID = " + buid ;
            }
            _oGRNs = GRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oGRNs = _oGRNs.OrderByDescending(x => x.GRNDate).ToList();
            #region Get User
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ReceivedBy FROM GRN AS MM WHERE MM.BUID =" + buid.ToString() + " AND ISNULL(MM.ReceivedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oReceivedUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oReceivedUser = new ESimSol.BusinessObjects.User();
            oReceivedUser.UserID = 0; oReceivedUser.UserName = "--Select Received User--";
            oReceivedUsers.Add(oReceivedUser);
            oReceivedUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.ReceivedUsers = oReceivedUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;

            #region GRN Types
            var oGRNTypes=new List<EnumObject>();
            foreach (var item in EnumObject.jGets(typeof(EnumGRNType)))
            {
                if (item.id == (int)EnumGRNType.LocalInvoice)
                    oGRNTypes.Add(item);
                else if (item.id == (int)EnumGRNType.PurchaseOrder) {
                    item.Value = "Supplier Order"; oGRNTypes.Add(item);
                }
            }
            #endregion

            ViewBag.GRNTypes = oGRNTypes;
            ViewBag.GRNStatusList = EnumObject.jGets(typeof(EnumGRNStatus));
            return View(_oGRNs);
        }

        public ActionResult ViewPartsReceived(int id, int buid)
        {
            _oGRN = new GRN();
            Company oCompany = new Company();

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.GRN, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            if (id > 0)
            {
                _oGRN = _oGRN.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oGRN.GRNDetails = GRNDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                if (!this.ContainStore(_oGRN.StoreID, oWorkingUnits))
                {
                    oWorkingUnit = new WorkingUnit();
                    oWorkingUnit = oWorkingUnit.Get(_oGRN.StoreID, (int)Session[SessionInfo.currentUserID]);
                    oWorkingUnits.Add(oWorkingUnit);
                }
            }

            #region GRN Types
            var oGRNTypes = new List<EnumObject>();
            foreach (var item in EnumObject.jGets(typeof(EnumGRNType)))
            {
                if (item.id == (int)EnumGRNType.LocalInvoice)
                    oGRNTypes.Add(item);
                else if (item.id == (int)EnumGRNType.PurchaseOrder)
                {
                    item.Value = "Supplier Order"; oGRNTypes.Add(item);
                }
            }
            #endregion

            ViewBag.GRNTypes = oGRNTypes;
            ViewBag.Stores = oWorkingUnits;
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oGRN);
        }

        private bool ContainStore(int nStoreID, List<WorkingUnit> oWorkingUnits)
        {
            foreach (WorkingUnit oItem in oWorkingUnits)
            {
                if (oItem.WorkingUnitID == nStoreID)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public JsonResult Save(GRN oGRN)
        {
            _oGRN = new GRN();
            oGRN.Remarks = oGRN.Remarks == null ? "" : oGRN.Remarks;
            try
            {
                _oGRN = oGRN;
                _oGRN = _oGRN.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approve(GRN oGRN)
        {
            _oGRN = new GRN();
            oGRN.Remarks = oGRN.Remarks == null ? "" : oGRN.Remarks;
            oGRN.ApproveBy = (int)Session[SessionInfo.currentUserID];
            oGRN.ApproveDate = DateTime.Now;
            oGRN.GRNStatusInt = (int)EnumGRNStatus.Approved;
            try
            {
                _oGRN = oGRN;
                _oGRN = _oGRN.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Receive(GRN oGRN)
        {
            _oGRN = new GRN();
            try
            {
                _oGRN = oGRN;
                _oGRN = _oGRN.Receive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(GRN oGRN)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oGRN.Delete(oGRN.GRNID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsPurchaseInvocie(GRN oGRN)
        {
            List<PurchaseInvoice> oPurchaseInvoices = new List<PurchaseInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseInvoice AS TT WHERE  ISNULL(TT.ApprovedBy,0)!=0 AND TT.BUID= " + oGRN.BUID.ToString() + " AND TT.YetToGRNQty>0  AND TT.ContractorID=" + oGRN.ContractorID.ToString() + " OR TT.PurchaseInvoiceID=" + oGRN.RefObjectID.ToString();
                oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseInvoices = new List<PurchaseInvoice>();
                PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = ex.Message;
                oPurchaseInvoices.Add(oPurchaseInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsPurchaseOrders(GRN oGRN)
        {
            List<PurchaseOrder> oPurchaseOrders = new List<PurchaseOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseOrder AS TT WHERE  ISNULL(TT.ApproveBy,0)!=0 AND TT.BUID= " + oGRN.BUID.ToString() + " AND TT.YetToGRNQty>0  AND TT.ContractorID=" + oGRN.ContractorID.ToString();
                oPurchaseOrders = PurchaseOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseOrders = new List<PurchaseOrder>();
                PurchaseOrder oPurchaseOrder = new PurchaseOrder();
                oPurchaseOrder.ErrorMessage = ex.Message;
                oPurchaseOrders.Add(oPurchaseOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsImportInvocie(GRN oGRN)
        {
            String sImportPIType = "";

            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            ImportInvoice oImportInvoice = new ImportInvoice();
            try
            {
                string sSQL = "SELECT * FROM View_ImportInvoice";
                string sReturn = " WHERE BUID=" + oGRN.BUID.ToString();
                if (!String.IsNullOrEmpty(oGRN.RefObjectNo))
                {
                    oGRN.RefObjectNo = oGRN.RefObjectNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ImportInvoiceNo Like'%" + oGRN.RefObjectNo + "%'";
                }
                if (oGRN.ContractorID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID  in (" + oGRN.ContractorID + " )";
                }
                if (oGRN.GRNTypeInt > 0)
                {
                    if(oGRN.GRNTypeInt==(int)EnumGRNType.LocalInvoice)
                    {
                        sImportPIType = ((int)EnumImportPIType.NonLC).ToString();
                    }
                    else if (oGRN.GRNTypeInt == (int)EnumGRNType.ImportInvoice)
                    {
                        sImportPIType = ((int)EnumImportPIType.Foreign).ToString() + ","+((int)EnumImportPIType.Local).ToString() + "," + ((int)EnumImportPIType.TTForeign).ToString();
                    }
                    //else if (oGRN.GRNTypeInt == (int)EnumGRNType.Service)
                    //{
                    //    sImportPIType = ((int)EnumImportPIType.Servise).ToString();

                    //}
                    else if (oGRN.GRNTypeInt == (int)EnumGRNType.FancyYarn)
                    {
                        sImportPIType = ((int)EnumImportPIType.FancyYarn).ToString();

                    }
                    else
                    {
                        sImportPIType = "";
                    }
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "InvoiceType in (" + sImportPIType+")";
                }

                sSQL = sSQL + "" + sReturn;
                oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (ImportInvoice oItem in oImportInvoices)
                {
                    _oGRN = new  GRN();
                    _oGRN.RefObjectNo = oItem.ImportInvoiceNo;                    
                    _oGRN.RefObjectID = oItem.ImportInvoiceID;
                    _oGRN.RefObjectDate = oItem.DateofInvoice;
                    _oGRN.ContractorID = oItem.ContractorID;
                    _oGRN.ContractorName = oItem.ContractorName;
                    _oGRN.CurrencyID = oItem.CurrencyID;
                    if (oItem.CCRate<=0)
                    {
                        oItem.CCRate = 1;
                    }
                    _oGRN.ConversionRate = oItem.CCRate;
                    if (oItem.InvoiceType == EnumImportPIType.Foreign)
                    {
                        if (oItem.InvoiceStatus == EnumInvoiceEvent.Goods_In_Transit ||  oItem.InvoiceStatus == EnumInvoiceEvent.DO_Received_From_Shippingline || oItem.InvoiceStatus == EnumInvoiceEvent.Stock_In_Partial || oItem.InvoiceStatus == EnumInvoiceEvent.Stock_In)
                        {
                            _oGRNs.Add(_oGRN);
                        }
                    }
                    else
                    {
                        _oGRNs.Add(_oGRN);
                    }
                }
            }
            catch (Exception ex)
            {
                _oGRNs = new List<GRN>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsWorkOrder(GRN oGRN)
        {
            List<WorkOrder> oWorkOrders = new List<WorkOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_WorkOrder AS TT WHERE ISNULL(TT.ApproveBy,0)!=0 AND  TT.BUID = " + oGRN.BUID.ToString() + " AND  TT.YetToGRNQty>0  AND TT.SupplierID=" + oGRN.ContractorID.ToString() + " OR TT.WorkOrderID=" + oGRN.RefObjectID.ToString();
                oWorkOrders = WorkOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWorkOrders = new List<WorkOrder>();
                WorkOrder oWorkOrder = new WorkOrder();
                oWorkOrder.ErrorMessage = ex.Message;
                oWorkOrders.Add(oWorkOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsImportPIs(GRN oGRN)
        {
            List<ImportPI> oImportPIs = new List<ImportPI>();
            try
            {
                string sSQL = "SELECT * FROM View_ImportPI AS TT WHERE ISNULL(TT.ApprovedBy,0)!=0 AND  TT.BUID = " + oGRN.BUID.ToString() + " AND  TT.YetToGRNQty>0  AND TT.SupplierID=" + oGRN.ContractorID.ToString() + " OR TT.ImportPIID =" + oGRN.RefObjectID.ToString();
                oImportPIs = ImportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportPIs = new List<ImportPI>();
                ImportPI oImportPI = new ImportPI();
                oImportPI.ErrorMessage = ex.Message;
                oImportPIs.Add(oImportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsConsumptions(GRN oGRN)
        {
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();
            try
            {
                if (oGRN.RefObjectNo == null) { oGRN.RefObjectNo = ""; }
                string sSQL = "SELECT * FROM View_ConsumptionRequisition AS TT WHERE TT.CRType=2 AND ISNULL(TT.ApprovedBy,0)!=0 AND TT.RequisitionForName LIKE '%" + oGRN.RefObjectNo + "%' AND TT.BUID = " + oGRN.BUID.ToString() + " AND ISNULL(TT.DeliveryBy,0) !=0 OR TT.ConsumptionRequisitionID =" + oGRN.RefObjectID.ToString();
                oConsumptionRequisitions = ConsumptionRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oConsumptionRequisitions = new List<ConsumptionRequisition>();
                ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.ErrorMessage = ex.Message;
                oConsumptionRequisitions.Add(oConsumptionRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }                
        [HttpPost]
        public JsonResult GetsRefItems(GRN oGRN)
        {
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();            
            try
            {
                if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.LocalInvoice)
                {
                    oGRNDetails = MapGRNDetailFromInvoice(oGRN.RefObjectID);
                }else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.PurchaseOrder)
                {
                    oGRNDetails = MapGRNDetailFromPO(oGRN.RefObjectID);
                }
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.ImportInvoice)
                {
                    oGRNDetails = MapGRNDetailFromImportInvoice(oGRN.RefObjectID);
                }
                //else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.Service)
                //{
                //    oGRNDetails = MapGRNDetailFromImportInvoice(oGRN.RefObjectID);
                //}
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.FancyYarn)
                {
                    oGRNDetails = MapGRNDetailFromImportInvoice(oGRN.RefObjectID);
                }
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.WorkOrder)
                {
                    oGRNDetails = MapGRNDetailFromWorkOrder(oGRN.RefObjectID);
                }
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.ImportPI)
                {
                    oGRNDetails = MapGRNDetailFromImportPI(oGRN.RefObjectID);
                }
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.FloorReturn)
                {
                    oGRNDetails = MapGRNDetailFromConsumtion(oGRN.RefObjectID);
                }
            }
            catch (Exception ex)
            {
                oGRNDetails = new List<GRNDetail>();
                GRNDetail oGRNDetail = new GRNDetail();
                oGRNDetail.ErrorMessage = ex.Message;
                oGRNDetails.Add(oGRNDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGRNDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private List<GRNDetail> MapGRNDetailFromInvoice(int nInvoiceID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            List<PurchaseInvoiceDetail> oPurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();
            string sSQL = "SELECT * FROM View_PurchaseInvoiceDetail AS TT WHERE TT.PurchaseInvoiceID=" + nInvoiceID.ToString() + " AND TT.YetToGRNQty>0";
            oPurchaseInvoiceDetails = PurchaseInvoiceDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (PurchaseInvoiceDetail oItem in oPurchaseInvoiceDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = oItem.ProductSpec;
                oGRNDetail.MUnitID = oItem.MUnitID;
                oGRNDetail.RefQty = oItem.YetToGRNQty;
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                oGRNDetail.UnitPrice = oItem.UnitPrice;
                oGRNDetail.VehicleModelID = oItem.VehicleModelID;
                oGRNDetail.ModelShortName = oItem.ModelShortName;
                oGRNDetail.Amount = (oItem.YetToGRNQty * oItem.UnitPrice);               
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.LocalInvoice;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.LocalInvoice;
                oGRNDetail.RefObjectID = oItem.PurchaseInvoiceDetailID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.MUName;
                oGRNDetail.MUSymbol = oItem.MUSymbol;                
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotNo = "";
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromPO(int nInvoiceID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            List<PurchaseOrderDetail> oPurchaseOrderDetails = new List<PurchaseOrderDetail>();
            string sSQL = "SELECT * FROM View_PurchaseOrderDetail AS TT WHERE TT.POID=" + nInvoiceID.ToString() + " AND TT.YetToGRNQty>0";
            oPurchaseOrderDetails = PurchaseOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (PurchaseOrderDetail oItem in oPurchaseOrderDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = oItem.ProductSpec;
                oGRNDetail.MUnitID = oItem.MUnitID;
                oGRNDetail.RefQty = oItem.YetToGRNQty;
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                oGRNDetail.UnitPrice = oItem.UnitPrice;
                oGRNDetail.Amount = (oItem.YetToGRNQty * oItem.UnitPrice);
                oGRNDetail.LotID = 0;
                oGRNDetail.VehicleModelID = oItem.VehicleModelID;
                oGRNDetail.ModelShortName = oItem.ModelShortName;
                oGRNDetail.RefType = EnumGRNType.PurchaseOrder;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.PurchaseOrder;
                oGRNDetail.RefObjectID = oItem.PODetailID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.UnitName;
                oGRNDetail.MUSymbol = oItem.UnitSymbol;                
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotNo = "";
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromImportInvoice(int nInvoiceID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();            
            List<ImportPackDetail> oImportPackDetails = new List<ImportPackDetail>();
            string sSQL = "SELECT * FROM View_ImportPackDetail AS TT WHERE TT.ImportInvoiceID=" + nInvoiceID.ToString() + " AND ISNULL(TT.YetToGRNQty,0)>0 order by ProductID, LotNo";            
            oImportPackDetails = ImportPackDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);           
            foreach (ImportPackDetail oItem in oImportPackDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = oItem.ProductSpec;
                oGRNDetail.MUnitID = oItem.MUnitID;
                oGRNDetail.RefQty = oItem.YetToGRNQty;
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                oGRNDetail.UnitPrice = (oItem.UnitPriceBC + oItem.InvoiceLandingCost + oItem.LCLandingCost);
                oGRNDetail.Amount = (oItem.YetToGRNQty * (oItem.UnitPriceBC + oItem.InvoiceLandingCost + oItem.LCLandingCost));
                oGRNDetail.InvoiceLandingCost = oItem.InvoiceLandingCost;
                oGRNDetail.LCLandingCost = oItem.LCLandingCost;
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.ImportInvoice;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.ImportInvoice;
                oGRNDetail.RefObjectID = oItem.ImportPackDetailID;                
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.MUName;
                oGRNDetail.MUSymbol = oItem.MUName;
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotNo = oItem.LotNo;
                oGRNDetail.NumberOfPack = oItem.NumberOfPack;
                oGRNDetail.QtyPerPack = oItem.QtyPerPack;
                oGRNDetail.Brand = oItem.Brand;
                oGRNDetail.Origin = oItem.Origin;
                oGRNDetail.StyleID = oItem.TechnicalSheetID;
                oGRNDetail.StyleNo = oItem.StyleNo;
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromWorkOrder(int nWorkOrderID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
            string sSQL = "SELECT * FROM View_WorkOrderDetail AS TT WHERE TT.WorkOrderID=" + nWorkOrderID.ToString() + " AND TT.YetToGRNQty>0";
            oWorkOrderDetails = WorkOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (WorkOrderDetail oItem in oWorkOrderDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;                
                oGRNDetail.TechnicalSpecification = oItem.ProductDescription;
                oGRNDetail.MUnitID = oItem.UnitID;
                oGRNDetail.RefQty = oItem.YetToGRNQty;
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                oGRNDetail.UnitPrice = (oItem.UnitPrice / oItem.RateUnit);
                oGRNDetail.Amount = (oItem.YetToGRNQty * (oItem.UnitPrice/oItem.RateUnit));
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.WorkOrder;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.WorkOrder;
                oGRNDetail.RefObjectID = oItem.WorkOrderDetailID;
                oGRNDetail.StyleID = oItem.StyleID;
                oGRNDetail.ColorID = oItem.ColorID;
                oGRNDetail.SizeID = oItem.SizeID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.UnitName;
                oGRNDetail.MUSymbol = oItem.UnitSymbol;
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.StyleNo = oItem.StyleNo;
                oGRNDetail.BuyerName = oItem.BuyerName;
                oGRNDetail.ColorName = oItem.ColorName;
                oGRNDetail.SizeName = oItem.SizeName;
                oGRNDetail.LotNo = "";
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromImportPI(int nImportPIID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            List<ImportPIDetail> oImportPIDetails = new List<ImportPIDetail>();
            string sSQL = "SELECT * FROM View_ImportPIDetail AS TT WHERE TT.ImportPIID =" + nImportPIID.ToString() + " AND ISNULL(TT.YetToGRNQty,0)>0";
            oImportPIDetails = ImportPIDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (ImportPIDetail oItem in oImportPIDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = "";
                oGRNDetail.MUnitID = oItem.MUnitID;
                oGRNDetail.RefQty = oItem.YetToGRNQty;
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                oGRNDetail.UnitPrice = (oItem.UnitPrice * oItem.CRate);
                oGRNDetail.Amount = (oItem.YetToGRNQty * (oItem.UnitPrice * oItem.CRate));
                oGRNDetail.InvoiceLandingCost = 0.00;
                oGRNDetail.LCLandingCost = 0.00;
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.ImportPI;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.ImportPI;
                oGRNDetail.RefObjectID = oItem.ImportPIDetailID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.MUName;
                oGRNDetail.MUSymbol = oItem.MUName;
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotNo = "";
                oGRNDetail.NumberOfPack = 0.00;
                oGRNDetail.QtyPerPack = 0.00;
                oGRNDetail.Brand = "";
                oGRNDetail.Origin = "";
                oGRNDetail.StyleID = oItem.TechnicalSheetID;
                oGRNDetail.StyleNo = oItem.StyleNo;
                oGRNDetail.BuyerName = oItem.BuyerName;
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromConsumtion(int nConsumtionID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
            string sSQL = "SELECT * FROM View_ConsumptionRequisitionDetail AS TT WHERE TT.YetToReturnQty>0 AND TT.ConsumptionRequisitionID =" + nConsumtionID.ToString();
            oConsumptionRequisitionDetails = ConsumptionRequisitionDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (ConsumptionRequisitionDetail oItem in oConsumptionRequisitionDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = "";
                oGRNDetail.MUnitID = oItem.UnitID;
                oGRNDetail.RefQty = oItem.YetToReturnQty;
                oGRNDetail.ReceivedQty = oItem.YetToReturnQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToReturnQty;
                oGRNDetail.UnitPrice = oItem.UnitPrice;
                oGRNDetail.Amount = (oItem.YetToReturnQty * oItem.UnitPrice);
                oGRNDetail.InvoiceLandingCost = 0.00;
                oGRNDetail.LCLandingCost = 0.00;
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.FloorReturn;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.FloorReturn;
                oGRNDetail.RefObjectID = oItem.ConsumptionRequisitionDetailID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.UnitName;
                oGRNDetail.MUSymbol = oItem.Symbol;
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotID = oItem.LotID;
                oGRNDetail.LotNo = oItem.LotNo;
                oGRNDetail.StyleID = oItem.StyleID;
                oGRNDetail.ColorID = oItem.ColorID;
                oGRNDetail.SizeID = oItem.SizeID;
                oGRNDetail.NumberOfPack = 0.00;
                oGRNDetail.QtyPerPack = 0.00;
                oGRNDetail.Brand = "";
                oGRNDetail.Origin = "";
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }

        public ActionResult PreviewPartsReceived(int id)
        {
            _oGRN = new GRN();
            _oClientOperationSetting = new ClientOperationSetting();
            _oGRN = _oGRN.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oGRN.GRNDetails = GRNDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oGRN.GRNDetails = _oGRN.GRNDetails.OrderBy(x => x.StyleNo).ToList();
            _oClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oGRN.BusinessUnit = oBusinessUnit.Get(_oGRN.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.GRN + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);

            rptPartsReceived oReport = new rptPartsReceived();
            byte[] abytes = oReport.PrepareReport(_oGRN,_oClientOperationSetting, oCompany, oApprovalHeads);
            return File(abytes, "application/pdf");
        }

        public ActionResult PreviewPartsReceivedWithRate(int id)
        {
            _oGRN = new GRN();
            _oGRN = _oGRN.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oGRN.GRNDetails = GRNDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oGRN.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptPartsReceviesRate oReport = new rptPartsReceviesRate();
            byte[] abytes = oReport.PrepareReport(_oGRN, oCompany);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintPartsReceiveds(string Param)
        {
            _oGRNs = new List<GRN>();
            string sSQLQuery = "SELECT *  FROM View_GRN WHERE GRNID IN ("+Param+")";
            _oGRNs = GRN.Gets(sSQLQuery, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oGRNs[0].BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sHeader = "Part's Receive List";
            if (_oGRNs.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();
                SelectedField oSelectedField = new SelectedField("GRNNo", "Date", 45, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("GRNDateSt", "Issue Date", 50, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("GLDateSt", "GL Date", 50, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ContractorName", "Vendor Name", 90, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("StoreName", "Store Name", 95, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ChallanNo", "Challan No/Ref No", 50, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ChallanDateSt", "Challan Date", 50, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ApproveBy", "Approve By", 74, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Amount", "Amount", 45, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);

                rptDynamicReport oReport = new rptDynamicReport(595, 842);
                oReport.SpanTotal = 9;//ColSpanForTotal
                byte[] abytes = oReport.PrepareReport(_oGRNs.Cast<object>().ToList(), oBusinessUnit, oCompany, sHeader, oSelectedFields);
                return File(abytes, "application/pdf");
            }
        }

        public Image GetDBCompanyLogo(int id)
        {
            #region From DB
            Company oCompany = new Company();
            oCompany = oCompany.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
            #endregion
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
        #region Advance Search
        [HttpPost]
        public JsonResult GetsByGRNNo(GRN oGRN)
        {
            List<GRN> oGRNs = new List<GRN>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_GRN AS HH WHERE  (HH.GRNNo+HH.VendorName+HH.RefObjectNo) LIKE '%" + oGRN.GRNNo + "%' ";
                if (oGRN.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oGRN.BUID ;
                }
                sSQL += " ORDER BY GRNID ASC";
                oGRNs = GRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oGRNs = new List<GRN>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGRNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WaitForReceived(GRN oGRN)
        {
            _oGRNs = new List<GRN>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_GRN WHERE ISNULL(ReceivedBy,0)=0 ";
                if (oGRN.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oGRN.BUID;
                }
                sSQL += " ORDER BY GRNID ASC";
                _oGRNs = GRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRNs = new List<GRN>();
                _oGRN.ErrorMessage = ex.Message;
                _oGRNs.Add(_oGRN);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(GRN oGRN)
        {
            _oGRNs = new List<GRN>();
            try
            {
                string sSQL = this.GetSQL(oGRN);
                _oGRNs = GRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRNs = new List<GRN>();
                _oGRN.ErrorMessage = ex.Message;
                _oGRNs.Add(_oGRN);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(GRN oGRN )
        {
            
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sGRNNo = Convert.ToString(oGRN.Remarks.Split('~')[0]);
            string sInvoiceNo = Convert.ToString(oGRN.Remarks.Split('~')[1]);
            EnumCompareOperator eGRNDate = (EnumCompareOperator)Convert.ToInt32(oGRN.Remarks.Split('~')[2]);
            DateTime dStartDate = Convert.ToDateTime(oGRN.Remarks.Split('~')[3]);
            DateTime dEndDate = Convert.ToDateTime(oGRN.Remarks.Split('~')[4]);
            EnumCompareOperator eInvoiceAmount = (EnumCompareOperator)Convert.ToInt32(oGRN.Remarks.Split('~')[5]);
            double nStartAmount = Convert.ToDouble(oGRN.Remarks.Split('~')[6]);
            double nEndAmount = Convert.ToDouble(oGRN.Remarks.Split('~')[7]);
            int nReceivedBy = Convert.ToInt32(oGRN.Remarks.Split('~')[8]);
            string sSupplierIDs = Convert.ToString(oGRN.Remarks.Split('~')[9]);
            string sProductIDs = Convert.ToString(oGRN.Remarks.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_GRN";
            string sReturn = "";

            #region BUID
            if (oGRN.BUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oGRN.BUID.ToString();
            }
            #endregion

            #region InvoiceSLNo
            if (sGRNNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNNo LIKE '%" + sGRNNo + "%'";
            }
            #endregion

            #region InvoiceNo
            if (sInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefObjectNo LIKE '%" + sInvoiceNo + "%'";
            }
            #endregion

            #region GRN Date
            if (eGRNDate != EnumCompareOperator.None)
            {
                if (eGRNDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region GRNAmount
            if (eInvoiceAmount != EnumCompareOperator.None)
            {
                if (eInvoiceAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount = " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount != " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount < " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount > " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount NOT BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
            }
            #endregion

            #region Received By
            if (nReceivedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ReceivedBy,0) = " + nReceivedBy.ToString();
            }
            #endregion

            #region SupplierIDs
            if (sSupplierIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sSupplierIDs + ")";
            }
            #endregion

            #region Products
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNID IN (SELECT TT.GRNID FROM GRNDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            #region GRN Type
            if (oGRN.GRNTypeInt != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNType = " + oGRN.GRNTypeInt;
            }
            #endregion

            #region GRNStatus
            if (oGRN.GRNStatusInt>-1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNStatus = " + oGRN.GRNStatusInt;
            }
            #endregion
            
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion
    }
}
