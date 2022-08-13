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
using System.Drawing.Imaging;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class PNWiseAccountHeadController : Controller
    {
        #region Declaration
        BUPermission _oBUPermission = new BUPermission();
        List<BUPermission> _oBUPermissions = new List<BUPermission>();
        StorePermission _oStorePermission = new StorePermission();
        List<StorePermission> _oStorePermissions = new List<StorePermission>();
        PNWiseAccountHead _oPNWiseAccountHead = new PNWiseAccountHead();
        List<PNWiseAccountHead> _oPNWiseAccountHeads = new List<PNWiseAccountHead>();
        ContainingProduct _oContainingProduct = new ContainingProduct();
        List<ContainingProduct> _oContainingProducts = new List<ContainingProduct>();
        #endregion

        #region PNWiseAccountHead Actions
        public ActionResult ViewPNWiseAccountHead(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PNWiseAccountHead).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            #region Get ModuleName
            //List<EnumObject> oModuleNameObjs = new List<EnumObject>();
            //List<EnumObject> oTempModuleNameObjs = EnumObject.jGets(typeof(EnumModuleName));
            //foreach (EnumObject oItem in oTempModuleNameObjs)
            //{
            //    EnumModuleName eModuleName = (EnumModuleName)oItem.id;
            //    switch (eModuleName)
            //    {
            //        case EnumModuleName.DeliveryChallan:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.TransferRequisitionSlip:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.GRN:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.RawMaterialOut:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.FinishGoodsReceived:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.ExportPI:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.ImportPI:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.Recipe:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.PurchaseRequisition:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.PurchaseQuotation:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.NOA:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.PurchaseOrder:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.PurchaseInvoice:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.ProductionSheet:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.Lot:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.Adjustment:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.TechnicalSheet:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.ConsumptionRequisition:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.OrderRecap:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.DevelopmentRecap:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.CostSheet:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.RecycleProcess:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //        case EnumModuleName.PackageTemplate:
            //            oModuleNameObjs.Add(oItem);
            //            break;
            //    }
            //}
            #endregion

            //User oUser = new ESimSol.BusinessObjects.User();
            //EnumObject oModuleNameObj = new EnumObject();
            //oModuleNameObj.Value = "--Select Module--";
            //oModuleNameObjs.Add(oModuleNameObj);

            _oPNWiseAccountHead = new PNWiseAccountHead();
           // oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oPNWiseAccountHead.PNWiseAccountHeads = PNWiseAccountHead.Gets((int)Session[SessionInfo.currentUserID]);
            //_oPNWiseAccountHead.ModuleNameObjs = oModuleNameObjs;
            //_oPNWiseAccountHead.ProductUsagesObjs = EnumObject.jGets(typeof(EnumProductUsages));
            //_oPNWiseAccountHead.UserName = oUser.UserName;
            //_oPNWiseAccountHead.UserID = id;
            ViewBag.ProductNatures = EnumObject.jGets(typeof(EnumProductNature));
            ViewBag.Accountnatures = EnumObject.jGets(typeof(EnumAccountHeadNature));
            string sSQL = "SELECT * FROM ChartsOfAccount WHERE AccountType=5";
            ViewBag.AccountHeads = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oPNWiseAccountHead);
        }

        [HttpPost]
        public JsonResult SavePNWiseAccountHead(PNWiseAccountHead oPNWiseAccountHead)
        {
            _oPNWiseAccountHead = new PNWiseAccountHead();
            try
            {
                _oPNWiseAccountHead = oPNWiseAccountHead;
                if (_oPNWiseAccountHead.ErrorMessage == null) { _oPNWiseAccountHead.ErrorMessage = ""; }
                _oPNWiseAccountHead = _oPNWiseAccountHead.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPNWiseAccountHead = new PNWiseAccountHead();
                _oPNWiseAccountHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPNWiseAccountHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePNWiseAccountHead(PNWiseAccountHead oPNWiseAccountHead)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oPNWiseAccountHead.Delete(oPNWiseAccountHead.PNWiseAccountHeadID, (int)Session[SessionInfo.currentUserID]);
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