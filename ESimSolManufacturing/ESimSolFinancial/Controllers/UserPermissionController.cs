using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class UserPermissionController : Controller
    {
        #region Declaration
        BUPermission _oBUPermission = new BUPermission();
        List<BUPermission> _oBUPermissions = new List<BUPermission>();
        StorePermission _oStorePermission = new StorePermission();
        List<StorePermission> _oStorePermissions = new List<StorePermission>();
        ProductPermission _oProductPermission = new ProductPermission();
        VPPermission _oVPPermission = new VPPermission();
        List<ProductPermission> _oProductPermissions = new List<ProductPermission>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        ContainingProduct _oContainingProduct = new ContainingProduct();
        List<ContainingProduct> _oContainingProducts = new List<ContainingProduct>();
        DBPermission _oDBPermission = new DBPermission();
        List<DBPermission> _oDBPermissions = new List<DBPermission>();
        #endregion

        #region StorePermission Actions
        public ActionResult ViewStorePermission(int id, double ts)
        {       
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.StorePermission).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            User oUser = new ESimSol.BusinessObjects.User();            
            WorkingUnit oWorkingUnit =new WorkingUnit();
            oWorkingUnit.LocationName = "--Select store--";
            oWorkingUnit.OperationUnitName = "";
            


            _oStorePermission = new StorePermission();
            oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oStorePermission.StorePermissions = StorePermission.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
            _oStorePermission.WorkingUnits = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);            
            _oStorePermission.WorkingUnits.Add(oWorkingUnit);
            _oStorePermission.UserName = oUser.UserName;
            _oStorePermission.UserID = id;
            return View(_oStorePermission);
        }

        [HttpPost]
        public JsonResult SearchByModuleName(StorePermission oStorePermission)
        {
            EnumObject oModuleNameObj = new EnumObject();
            List<EnumObject> oModuleNameObjs = new List<EnumObject>();
            List<EnumObject> oTempModuleNameObjs = EnumObject.jGets(typeof(EnumModuleName));
             string ModuleName = oStorePermission.UserName != null ? oStorePermission.UserName : "";
            try
            {
                #region Get ModuleName
                
                foreach (EnumObject oItem in oTempModuleNameObjs)
                {
                    EnumModuleName eModuleName = (EnumModuleName)oItem.id;
                    switch (eModuleName)
                    {
                        case EnumModuleName.DeliveryChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.TransferRequisitionSlip:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.TransferRequisitionSlipDyed:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.GRN:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.RawMaterialOut:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FinishGoodsReceived:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.QC:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.Lot:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.Adjustment:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.ConsumptionRequisition:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.RecycleProcess:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.ReturnChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.DURequisition:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.DUProGuideLine:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.DULotDistribution:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.DUDeliveryChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricDeliveryChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricTransferNote:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricDeliveryOrder:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.PartsRequisition:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.PurchaseReturn:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.RouteSheet:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.RouteSheetDetail:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.LotMixing:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.VehicleChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.DUPSchedule:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricSalesContract:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FNRequisition:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FNProduction:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricBatch:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.VehicleReturnChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.GUQC:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.Shipment:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.DyeingOrder:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricBatchQC:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.WYRequisition:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.KnittingOrder:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.KnittingYarnChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.KnittingFabricReceive:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricBatch_PRO_Sizing:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.RouteSheetCancel:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricReturnChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.CapitalResourceSpareParts:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.SparePartsRequisition:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.SparePartsChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricRequisition:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.TradingSaleInvoice:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.TradingDeliveryChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.SampleRequest:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.DUReturnChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.LoanProductRate:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.Twisting:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.KnitDyeingPTU:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.KnitDyeingBatch:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.WUSubContractYarnChallan:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.FabricAvailableStock:
                            oModuleNameObjs.Add(oItem);
                            break;
                        case EnumModuleName.WUSubContractFabricReceive:
                            oModuleNameObjs.Add(oItem);
                            break;
                    }
                }
                #endregion


                if (ModuleName != "")
                {
                    oModuleNameObjs = oModuleNameObjs.Where(c => c.Value.ToUpper().Contains(ModuleName.ToUpper())).ToList();
                }

                //if (oModuleNameObjs.Count() <= 0) throw new Exception("No Module found.");
            }
            catch (Exception ex)
            {
                oModuleNameObjs = new List<EnumObject>();
                //oModuleNameObj = new EnumObject();
                //oModuleNameObj.Value = ex.Message;
                oModuleNameObjs.Add(oModuleNameObj);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oModuleNameObjs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByStoreName(StorePermission oStorePermission)
        {
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            string StoreName = oStorePermission.UserName != null ? oStorePermission.UserName : "";
            try
            {

                if (StoreName != "")
                {
                    oWorkingUnits = oWorkingUnits.Where(c => c.WorkingUnitName.ToUpper().Contains(StoreName.ToUpper())).ToList();
                }

                if (oWorkingUnits.Count() <= 0) throw new Exception("No Store found.");

            }
            catch (Exception ex)
            {
                oWorkingUnits = new List<WorkingUnit>();
                oWorkingUnit = new WorkingUnit();
                oWorkingUnit.ErrorMessage = ex.Message;
                oWorkingUnits.Add(oWorkingUnit);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkingUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveStorePermission(StorePermission oStorePermission)
        {
            _oStorePermission = new StorePermission();
            _oStorePermissions = new List<StorePermission>();
            try
            {
                _oStorePermission = oStorePermission;
                if (_oStorePermission.Remarks == null) { _oStorePermission.Remarks = ""; }
                _oStorePermissions = _oStorePermission.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStorePermissions = new List<StorePermission>();
                _oStorePermission = new StorePermission();
                _oStorePermission.ErrorMessage = ex.Message;
                _oStorePermissions.Add(_oStorePermission);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStorePermissions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteStorePermission(StorePermission oStorePermission)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oStorePermission.Delete(oStorePermission.StorePermissionID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ProductPermission Actions
        public ActionResult ViewProductPermission(int id, double ts)
        {
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductPermission).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            #region Get ModuleName
            List<EnumObject> oModuleNameObjs = new List<EnumObject>();
            List<EnumObject> oTempModuleNameObjs = EnumObject.jGets(typeof(EnumModuleName));
            foreach (EnumObject oItem in oTempModuleNameObjs)
            {
                EnumModuleName eModuleName = (EnumModuleName)oItem.id;
                switch (eModuleName)
                {
                    case EnumModuleName.DeliveryChallan:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.TransferRequisitionSlip:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.GRN:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.RawMaterialOut:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.FinishGoodsReceived:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.ExportPI:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.ImportPI:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.Recipe:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.PurchaseRequisition:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.PurchaseQuotation:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.NOA:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.PurchaseOrder:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.PurchaseInvoice:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.ProductionSheet:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.Lot:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.Adjustment:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.TechnicalSheet:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.ConsumptionRequisition:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.OrderRecap:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.DevelopmentRecap:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.CostSheet:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.RecycleProcess:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.PackageTemplate:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.Fabric:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.FabricPattern:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.FabricPO:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.DURequisition:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.DUProGuideLine:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.Labdip:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.FNLabdip:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.ServiceInvoice:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.Twisting:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.PartsRequisition:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.PurchaseReturn:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.SaleInvoice:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.FNRequisition:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.KnittingOrder:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.KnittingYarnChallan:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.KnittingFabricReceive:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.YarnRequisition:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.CapitalResourceSpareParts:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.LoanProductRate:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.KnitDyeingBatch:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.KnitDyeingProgram:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.KnitDyeingRecipe:
                        oModuleNameObjs.Add(oItem);
                        break;
                    case EnumModuleName.WUSubContract:
                        oModuleNameObjs.Add(oItem);
                        break;

                }
            }
            #endregion

            User oUser = new ESimSol.BusinessObjects.User();
            EnumObject oModuleNameObj = new EnumObject();
            oModuleNameObj.Value = "--Select Module--";
            oModuleNameObjs.Add(oModuleNameObj);
                       

            _oProductPermission = new ProductPermission();
            oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oProductPermission.ProductPermissions = ProductPermission.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);            
            _oProductPermission.ModuleNameObjs = oModuleNameObjs;
            _oProductPermission.ProductUsagesObjs = EnumObject.jGets(typeof(EnumProductUsages));
            _oProductPermission.UserName = oUser.UserName;
            _oProductPermission.UserID = id;
            return View(_oProductPermission);
        }

        [HttpPost]
        public JsonResult SaveProductPermission(ProductPermission oProductPermission)
        {
            _oProductPermission = new ProductPermission();
            try
            {
                _oProductPermission = oProductPermission;
                if (_oProductPermission.Remarks == null) { _oProductPermission.Remarks = ""; }
                _oProductPermission = _oProductPermission.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductPermission = new ProductPermission();
                _oProductPermission.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductPermission);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProductPermission(ProductPermission oProductPermission)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oProductPermission.Delete(oProductPermission.ProductPermissionID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ContainingProduct Actions
        public ActionResult ViewContaingProduct(int id, double ts)
        {
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ContainingProduct).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            WorkingUnit oWorkingUnit = new WorkingUnit();
            _oContainingProduct = new ContainingProduct();
            oWorkingUnit = oWorkingUnit.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oContainingProduct.ContainingProducts = ContainingProduct.GetsByWU(id, (int)Session[SessionInfo.currentUserID]);
            _oContainingProduct.WorkingUnits = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            _oContainingProduct.LocationName = oWorkingUnit.LocationName;
            _oContainingProduct.OperationUnitName = oWorkingUnit.OperationUnitName;
            _oContainingProduct.WorkingUnitID = id;

            oWorkingUnit = new WorkingUnit();
            oWorkingUnit.LocationName = "--Select store--";
            oWorkingUnit.OperationUnitName = "";
            _oContainingProduct.WorkingUnits.Add(oWorkingUnit);
            return View(_oContainingProduct);
        }

        [HttpPost]
        public JsonResult SaveContainingProduct(ContainingProduct oContainingProduct)
        {
            _oContainingProduct = new ContainingProduct();
            try
            {
                _oContainingProduct = oContainingProduct;
                if (_oContainingProduct.Remarks == null) { _oContainingProduct.Remarks = ""; }
                _oContainingProduct = _oContainingProduct.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oContainingProduct = new ContainingProduct();
                _oContainingProduct.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContainingProduct);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteContainingProduct(ContainingProduct oContainingProduct)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oContainingProduct.Delete(oContainingProduct.ContainingProductID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BUPermission Actions
        public ActionResult ViewBUPermission(int id, double ts)
        {
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.BUPermission).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            User oUser = new ESimSol.BusinessObjects.User();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit.Name = "--Select Business Unit--";

            _oBUPermission = new BUPermission();
            oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oBUPermission.BUPermissions = BUPermission.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
            _oBUPermission.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            _oBUPermission.BusinessUnits.Add(oBusinessUnit);
            _oBUPermission.UserName = oUser.UserName;
            _oBUPermission.UserID = id;
            return View(_oBUPermission);
        }

        [HttpPost]
        public JsonResult SaveBUPermission(BUPermission oBUPermission)
        {
            _oBUPermission = new BUPermission();
            try
            {
                _oBUPermission = oBUPermission;
                if (_oBUPermission.Remarks == null) { _oBUPermission.Remarks = ""; }
                _oBUPermission = _oBUPermission.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBUPermission = new BUPermission();
                _oBUPermission.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBUPermission);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBUPermission(BUPermission oBUPermission)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oBUPermission.Delete(oBUPermission.BUPermissionID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AutoVoucherPermission
        public ActionResult ViewAutoVoucherPermission(int id, double ts)
        {
            
            User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, id);

            _oVPPermission = new VPPermission();
            oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oVPPermission.VPPermissions = VPPermission.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
            
            _oVPPermission.UserName = oUser.UserName;
            _oVPPermission.UserID = id;
            return View(_oVPPermission);
        }

        [HttpPost]
        public JsonResult SaveVPPermission(VPPermission oVPPermission)
        {
            _oVPPermission = new VPPermission();
            try
            {
                _oVPPermission = oVPPermission;
                _oVPPermission = _oVPPermission.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oVPPermission = new VPPermission();
                _oVPPermission.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVPPermission);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult DeleteVPPermission(VPPermission oVPPermission)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oVPPermission.Delete(oVPPermission.VPPermissionID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult RefreshProcesses(IntegrationSetup oIntegrationSetup)
        {
            List<VPPermission> oVPPermissions = new List<VPPermission>();
            List<IntegrationSetup>  oIntegrationSetups = new List<IntegrationSetup>();
            if (oIntegrationSetup.BUID > 0)
            {
                oIntegrationSetups = IntegrationSetup.GetsByBU(oIntegrationSetup.BUID,(int) Session[SessionInfo.currentUserID]);
                oVPPermissions = VPPermission.Gets("SELECT * FROM VIEW_VPPermission WHERE BusinessUnitID = " + oIntegrationSetup.BUID + " AND UserID = " + oIntegrationSetup.Sequence + " ORDER BY Sequence ASC", (int)Session[SessionInfo.currentUserID]); //Sequence property = UserID
            }
            else
            {
                oVPPermissions = VPPermission.Gets("SELECT * FROM VIEW_VPPermission WHERE UserID = " + oIntegrationSetup.Sequence + " ORDER BY Sequence ASC", (int)Session[SessionInfo.currentUserID]);
            }
            

            VPPermission oVPPermission = new VPPermission();
            oVPPermission.IntegrationSetups = oIntegrationSetups;
            oVPPermission.VPPermissions = oVPPermissions;
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVPPermission);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DashBoardPermission Actions
        public ActionResult ViewDashBoardPermission(int id, double ts)
        {
            User oUser = new ESimSol.BusinessObjects.User();
            _oDBPermission = new DBPermission();
            oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oDBPermission.DBPermissions = DBPermission.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
            _oDBPermission.DashBoardTypeObjs = EnumObject.jGets(typeof(EnumDashBoardType));
            _oDBPermission.UserName = oUser.UserName;
            _oDBPermission.UserID = id;
            return View(_oDBPermission);
        }

        [HttpPost]
        public JsonResult SaveDBPermission(DBPermission oDBPermission)
        {
            _oDBPermission = new DBPermission();
            try
            {
                _oDBPermission = oDBPermission;
                if (_oDBPermission.Remarks == null) { _oDBPermission.Remarks = ""; }
                _oDBPermission = _oDBPermission.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDBPermission = new DBPermission();
                _oDBPermission.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDBPermission);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDBPermission(DBPermission oDBPermission)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDBPermission.Delete(oDBPermission.DBPermissionID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
