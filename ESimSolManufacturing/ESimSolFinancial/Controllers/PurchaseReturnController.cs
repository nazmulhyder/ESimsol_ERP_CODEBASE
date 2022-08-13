using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
	public class PurchaseReturnController : Controller
	{
		#region Declaration

		PurchaseReturn _oPurchaseReturn = new PurchaseReturn();
		List<PurchaseReturn> _oPurchaseReturns = new  List<PurchaseReturn>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        List<PurchaseReturnRegister> _oPurchaseReturnRegisters = new List<PurchaseReturnRegister>();
        PurchaseReturnRegister _oPurchaseReturnRegister = new PurchaseReturnRegister();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewPurchaseReturns(int buid, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PurchaseReturn).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
			_oPurchaseReturns = new List<PurchaseReturn>();
            string sSQL = "SELECT * FROM View_PurchaseReturn AS HH WHERE ISNULL(HH.DisbursedBy,0)=0 ";
            if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " AND BUID = " + buid;
            }
            _oPurchaseReturns = PurchaseReturn.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseReturns = _oPurchaseReturns.OrderByDescending(x => x.ReturnDate).ToList();
            #region Get User
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.DisbursedBy FROM PurchaseReturn AS MM WHERE MM.BUID =" + buid.ToString() + " AND ISNULL(MM.DisbursedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oDisbursedUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oDisbursedUser = new ESimSol.BusinessObjects.User();
            oDisbursedUser.UserID = 0; oDisbursedUser.UserName = "--Select Received User--";
            oDisbursedUsers.Add(oDisbursedUser);
            oDisbursedUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.DisbursedUsers = oDisbursedUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            ViewBag.PurchaseReturnTypes = EnumObject.jGets(typeof(EnumPurchaseReturnType));

			return View(_oPurchaseReturns);
		}
		public ActionResult ViewPurchaseReturn(int id, int buid)
		{
			_oPurchaseReturn = new PurchaseReturn();
            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.PurchaseReturn, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
			if (id > 0)
			{
				_oPurchaseReturn = _oPurchaseReturn.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseReturn.PurchaseReturnDetails = PurchaseReturnDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                if (!this.ContainStore(_oPurchaseReturn.WorkingUnitID, oWorkingUnits))
                {
                    oWorkingUnit = new WorkingUnit();
                    oWorkingUnit = oWorkingUnit.Get(_oPurchaseReturn.WorkingUnitID, (int)Session[SessionInfo.currentUserID]);
                    oWorkingUnits.Add(oWorkingUnit);
                }
			}
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumPurchaseReturnType));
            ViewBag.Stores = oWorkingUnits;
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
			return View(_oPurchaseReturn);
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
		public JsonResult Save(PurchaseReturn oPurchaseReturn)
		{
			_oPurchaseReturn = new PurchaseReturn();
			try
			{
				_oPurchaseReturn = oPurchaseReturn;
				_oPurchaseReturn = _oPurchaseReturn.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oPurchaseReturn = new PurchaseReturn();
				_oPurchaseReturn.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oPurchaseReturn);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 
		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				PurchaseReturn oPurchaseReturn = new PurchaseReturn();
				sFeedBackMessage = oPurchaseReturn.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approve(PurchaseReturn oPurchaseReturn)
        {
            _oPurchaseReturn = new PurchaseReturn();
            oPurchaseReturn.Remarks = oPurchaseReturn.Remarks == null ? "" : oPurchaseReturn.Remarks;
            try
            {
                _oPurchaseReturn = oPurchaseReturn;
                _oPurchaseReturn = _oPurchaseReturn.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseReturn = new PurchaseReturn();
                _oPurchaseReturn.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseReturn);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Disburse(PurchaseReturn oPurchaseReturn)
        {
            _oPurchaseReturn = new PurchaseReturn();
            try
            {
                _oPurchaseReturn = oPurchaseReturn;
                _oPurchaseReturn = _oPurchaseReturn.Disburse((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseReturn = new PurchaseReturn();
                _oPurchaseReturn.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseReturn);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

        #region Gets
        [HttpPost]
        public JsonResult GetsPurchaseInvocie(PurchaseReturn oPurchaseReturn)
        {
            List<PurchaseInvoice> oPurchaseInvoices = new List<PurchaseInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseInvoice AS TT WHERE  ISNULL(TT.ApprovedBy,0)!=0 AND TT.BUID= " + oPurchaseReturn.BUID.ToString() + " AND TT.YetToPurchaseReturnQty>0 AND TT.LotBalance>0 AND TT.ContractorID=" + oPurchaseReturn.SupplierID.ToString() + " OR TT.PurchaseInvoiceID=" + oPurchaseReturn.RefObjectID.ToString();
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
        public JsonResult GetsPurchaseOrders(PurchaseReturn oPurchaseReturn)
        {
            List<PurchaseOrder> oPurchaseOrders = new List<PurchaseOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseOrder AS TT WHERE  ISNULL(TT.ApproveBy,0)!=0 AND TT.BUID= " + oPurchaseReturn.BUID.ToString() + " AND TT.YetToPurchaseReturnQty>0 AND TT.LotBalance>0  AND TT.ContractorID=" + oPurchaseReturn.SupplierID.ToString();
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
        public JsonResult GetsImportInvocie(PurchaseReturn oPurchaseReturn)
        {
            String sImportPIType = "";

            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            ImportInvoice oImportInvoice = new ImportInvoice();
            try
            {
                string sSQL = "SELECT * FROM View_ImportInvoice";
                string sReturn = " WHERE BUID=" + oPurchaseReturn.BUID.ToString();
                if (!String.IsNullOrEmpty(oPurchaseReturn.RefNo))
                {
                    oPurchaseReturn.RefNo = oPurchaseReturn.RefNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ImportInvoiceNo Like'%" + oPurchaseReturn.RefNo + "%'";
                }
                if (oPurchaseReturn.SupplierID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID  in (" + oPurchaseReturn.SupplierID + " )";
                }
                if (oPurchaseReturn.RefTypeInt > 0)
                {
                    if (oPurchaseReturn.RefTypeInt == (int)EnumPurchaseReturnType.LocalInvoice)
                    {
                        sImportPIType = ((int)EnumImportPIType.NonLC).ToString();
                    }
                    else if (oPurchaseReturn.RefTypeInt == (int)EnumPurchaseReturnType.ImportInvoice)
                    {
                        sImportPIType = ((int)EnumImportPIType.Foreign).ToString() + "," + ((int)EnumImportPIType.Local).ToString() + "," + ((int)EnumImportPIType.TTForeign).ToString();
                    }
                    else
                    {
                        sImportPIType = "";
                    }
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "InvoiceType in (" + sImportPIType + ")";
                }

                sSQL = sSQL + "" + sReturn;
                oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (ImportInvoice oItem in oImportInvoices)
                {
                    _oPurchaseReturn = new PurchaseReturn();
                    _oPurchaseReturn.RefNo = oItem.ImportInvoiceNo;
                    _oPurchaseReturn.RefObjectID = oItem.ImportInvoiceID;
                    //_oPurchaseReturn.RefObjectDate = oItem.DateofInvoice;
                    _oPurchaseReturn.SupplierID = oItem.ContractorID;
                    _oPurchaseReturn.SupplierName = oItem.ContractorName;
                    //_oPurchaseReturn.CurrencyID = oItem.CurrencyID;
                    //if (oItem.CCRate <= 0)
                    //{
                    //    oItem.CCRate = 1;
                    //}
                   // _oPurchaseReturn.ConversionRate = oItem.CCRate;
                    if (oItem.InvoiceType == EnumImportPIType.Foreign)
                    {
                        if (oItem.InvoiceStatus == EnumInvoiceEvent.Goods_In_Transit || oItem.InvoiceStatus == EnumInvoiceEvent.DO_Received_From_Shippingline || oItem.InvoiceStatus == EnumInvoiceEvent.Stock_In_Partial || oItem.InvoiceStatus == EnumInvoiceEvent.Stock_In)
                        {
                            _oPurchaseReturns.Add(_oPurchaseReturn);
                        }
                    }
                    else
                    {
                        _oPurchaseReturns.Add(_oPurchaseReturn);
                    }
                }
            }
            catch (Exception ex)
            {
                _oPurchaseReturns = new List<PurchaseReturn>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseReturns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsWorkOrder(PurchaseReturn oPurchaseReturn)
        {
            List<WorkOrder> oWorkOrders = new List<WorkOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_WorkOrder AS TT WHERE ISNULL(TT.ApproveBy,0)!=0 AND  TT.BUID = " + oPurchaseReturn.BUID.ToString() + " AND TT.YetToPurchaseReturnQty>0 AND TT.LotBalance>0  AND TT.SupplierID=" + oPurchaseReturn.SupplierID.ToString() + " OR TT.WorkOrderID=" + oPurchaseReturn.RefObjectID.ToString();
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
        public JsonResult GetsImportPIs(PurchaseReturn oPurchaseReturn)
        {
            List<ImportPI> oImportPIs = new List<ImportPI>();
            try
            {
                string sSQL = "SELECT * FROM View_ImportPI AS TT WHERE ISNULL(TT.ApprovedBy,0)!=0 AND  TT.BUID = " + oPurchaseReturn.BUID.ToString() + " AND TT.SupplierID=" + oPurchaseReturn.SupplierID.ToString() + " OR TT.ImportPIID =" + oPurchaseReturn.RefObjectID.ToString();
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
        public JsonResult GetsRefItems(PurchaseReturn oPurchaseReturn)
        {
            List<PurchaseReturnDetail> oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
            try
            {
                if ((EnumPurchaseReturnType)oPurchaseReturn.RefTypeInt == EnumPurchaseReturnType.LocalInvoice)
                {
                    oPurchaseReturnDetails = MapPurchaseReturnDetailFromInvoice(oPurchaseReturn.RefObjectID, oPurchaseReturn.WorkingUnitID);
                }
                else if ((EnumPurchaseReturnType)oPurchaseReturn.RefTypeInt == EnumPurchaseReturnType.PurchaseOrder)
                {
                    oPurchaseReturnDetails = MapPurchaseReturnDetailFromPO(oPurchaseReturn.RefObjectID, oPurchaseReturn.WorkingUnitID);
                }
                else if ((EnumPurchaseReturnType)oPurchaseReturn.RefTypeInt == EnumPurchaseReturnType.ImportInvoice)
                {
                    oPurchaseReturnDetails = MapPurchaseReturnDetailFromImportInvoice(oPurchaseReturn.RefObjectID);
                }
                else if ((EnumPurchaseReturnType)oPurchaseReturn.RefTypeInt == EnumPurchaseReturnType.WorkOrder)
                {
                    oPurchaseReturnDetails = MapPurchaseReturnDetailFromWorkOrder(oPurchaseReturn.RefObjectID, oPurchaseReturn.WorkingUnitID);
                }
                else if ((EnumPurchaseReturnType)oPurchaseReturn.RefTypeInt == EnumPurchaseReturnType.ImportPI)
                {
                    oPurchaseReturnDetails = MapPurchaseReturnDetailFromImportPI(oPurchaseReturn.RefObjectID, oPurchaseReturn.WorkingUnitID);
                }
              
            }
            catch (Exception ex)
            {
                oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
                PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
                oPurchaseReturnDetail.ErrorMessage = ex.Message;
                oPurchaseReturnDetails.Add(oPurchaseReturnDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseReturnDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private List<PurchaseReturnDetail> MapPurchaseReturnDetailFromInvoice(int nInvoiceID, int WorkingUnitID)
        {
            PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
            List<PurchaseReturnDetail> oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
            List<PurchaseInvoiceDetail> oPurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();
            string sSQL = "SELECT * FROM View_PurchaseInvoiceDetail AS TT WHERE TT.PurchaseInvoiceID=" + nInvoiceID.ToString() + " AND TT.YetToPurchaseReturnQty>0 AND TT.LotBalance>0 AND TT.WorkingUnitID=" + WorkingUnitID;
            oPurchaseInvoiceDetails = PurchaseInvoiceDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (PurchaseInvoiceDetail oItem in oPurchaseInvoiceDetails)
            {
                oPurchaseReturnDetail = new PurchaseReturnDetail();
                oPurchaseReturnDetail.PurchaseReturnDetailID = 0;
                oPurchaseReturnDetail.PurchaseReturnID = 0;
                oPurchaseReturnDetail.ProductID = oItem.ProductID;
                oPurchaseReturnDetail.MUnitID = oItem.MUnitID;
                oPurchaseReturnDetail.RefDetailQty = oItem.Qty;
                oPurchaseReturnDetail.YetToPurchaseReturnQty = oItem.YetToPurchaseReturnQty;
                oPurchaseReturnDetail.LotBalance = oItem.LotBalance;
                oPurchaseReturnDetail.ReturnQty = oItem.LotBalance;
                oPurchaseReturnDetail.LotID = oItem.LotID;
                oPurchaseReturnDetail.RefType = EnumPurchaseReturnType.LocalInvoice;
                oPurchaseReturnDetail.RefTypeInt = (int)EnumPurchaseReturnType.LocalInvoice;
                oPurchaseReturnDetail.RefObjectDetailID = oItem.PurchaseInvoiceDetailID;
                oPurchaseReturnDetail.MUName = oItem.MUName;
                oPurchaseReturnDetail.MUSymbol = oItem.MUSymbol;
                oPurchaseReturnDetail.ProductName = oItem.ProductName;
                oPurchaseReturnDetail.ProductCode = oItem.ProductCode;
                oPurchaseReturnDetail.LotNo = oItem.LotNo;
                oPurchaseReturnDetails.Add(oPurchaseReturnDetail);
            }
            return oPurchaseReturnDetails;
        }
        private List<PurchaseReturnDetail> MapPurchaseReturnDetailFromPO(int nInvoiceID, int WorkingUnitID)
        {
            PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
            List<PurchaseReturnDetail> oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
            List<PurchaseOrderDetail> oPurchaseOrderDetails = new List<PurchaseOrderDetail>();
            string sSQL = "SELECT * FROM View_PurchaseOrderDetail AS TT WHERE TT.POID=" + nInvoiceID.ToString() + " AND TT.YetToPurchaseReturnQty>0 AND TT.LotBalance>0 AND TT.WorkingUnitID="+WorkingUnitID;
            oPurchaseOrderDetails = PurchaseOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (PurchaseOrderDetail oItem in oPurchaseOrderDetails)
            {
                oPurchaseReturnDetail = new PurchaseReturnDetail();
                oPurchaseReturnDetail.PurchaseReturnDetailID = 0;
                oPurchaseReturnDetail.PurchaseReturnID = 0;
                oPurchaseReturnDetail.ProductID = oItem.ProductID;
                oPurchaseReturnDetail.MUnitID = oItem.MUnitID;
                oPurchaseReturnDetail.RefDetailQty = oItem.Qty;
                oPurchaseReturnDetail.YetToPurchaseReturnQty = oItem.YetToPurchaseReturnQty;
                oPurchaseReturnDetail.LotBalance = oItem.LotBalance;
                oPurchaseReturnDetail.ReturnQty = oItem.LotBalance;
                oPurchaseReturnDetail.LotID = oItem.LotID;
                oPurchaseReturnDetail.RefType = EnumPurchaseReturnType.PurchaseOrder;
                oPurchaseReturnDetail.RefTypeInt = (int)EnumPurchaseReturnType.PurchaseOrder;
                oPurchaseReturnDetail.RefObjectDetailID = oItem.PODetailID;
                oPurchaseReturnDetail.MUName = oItem.UnitName;
                oPurchaseReturnDetail.MUSymbol = oItem.UnitSymbol;
                oPurchaseReturnDetail.ProductName = oItem.ProductName;
                oPurchaseReturnDetail.ProductCode = oItem.ProductCode;
                oPurchaseReturnDetail.LotNo = oItem.LotNo;
                oPurchaseReturnDetails.Add(oPurchaseReturnDetail);
            }
            return oPurchaseReturnDetails;
        }

        private List<PurchaseReturnDetail> MapPurchaseReturnDetailFromWorkOrder(int nWorkOrderID, int WorkingUnitID)
        {
            PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
            List<PurchaseReturnDetail> oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
            List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
            string sSQL = "SELECT * FROM View_WorkOrderDetail AS TT WHERE TT.WorkOrderID=" + nWorkOrderID.ToString() + " AND TT.YetToPurchaseReturnQty>0 AND TT.LotBalance>0 AND TT.WorkingUnitID=" + WorkingUnitID;
            oWorkOrderDetails = WorkOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (WorkOrderDetail oItem in oWorkOrderDetails)
            {
                oPurchaseReturnDetail = new PurchaseReturnDetail();
                oPurchaseReturnDetail.PurchaseReturnDetailID = 0;
                oPurchaseReturnDetail.PurchaseReturnID = 0;
                oPurchaseReturnDetail.ProductID = oItem.ProductID;
                oPurchaseReturnDetail.MUnitID = oItem.UnitID;
                oPurchaseReturnDetail.RefDetailQty = oItem.Qty;
                oPurchaseReturnDetail.YetToPurchaseReturnQty = oItem.YetToPurchaseReturnQty;
                oPurchaseReturnDetail.LotBalance = oItem.LotBalance;
                oPurchaseReturnDetail.ReturnQty = oItem.LotBalance;
                oPurchaseReturnDetail.LotID = oItem.LotID;
                oPurchaseReturnDetail.RefType = EnumPurchaseReturnType.WorkOrder;
                oPurchaseReturnDetail.RefTypeInt = (int)EnumPurchaseReturnType.WorkOrder;
                oPurchaseReturnDetail.RefObjectDetailID = oItem.WorkOrderDetailID;
                oPurchaseReturnDetail.StyleID = oItem.StyleID;
                oPurchaseReturnDetail.ColorID = oItem.ColorID;
                oPurchaseReturnDetail.SizeID = oItem.SizeID;
                oPurchaseReturnDetail.MUName = oItem.UnitName;
                oPurchaseReturnDetail.MUSymbol = oItem.UnitSymbol;
                oPurchaseReturnDetail.ProductName = oItem.ProductName;
                oPurchaseReturnDetail.ProductCode = oItem.ProductCode;
                oPurchaseReturnDetail.StyleNo = oItem.StyleNo;
                oPurchaseReturnDetail.BuyerName = oItem.BuyerName;
                oPurchaseReturnDetail.ColorName = oItem.ColorName;
                oPurchaseReturnDetail.SizeName = oItem.SizeName;
                oPurchaseReturnDetail.LotNo = oItem.LotNo;
                oPurchaseReturnDetail.MCDia = oItem.MCDia;
                oPurchaseReturnDetail.FinishDia = oItem.FinishDia;
                oPurchaseReturnDetail.GSM = oItem.GSM;
                oPurchaseReturnDetails.Add(oPurchaseReturnDetail);
            }
            return oPurchaseReturnDetails;
        }
        private List<PurchaseReturnDetail> MapPurchaseReturnDetailFromImportInvoice(int nInvoiceID)
        {
            PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
            List<PurchaseReturnDetail> oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
            List<ImportPackDetail> oImportPackDetails = new List<ImportPackDetail>();
            string sSQL = "SELECT * FROM View_ImportPackDetail AS TT WHERE TT.ImportInvoiceID=" + nInvoiceID.ToString() + " AND ISNULL(TT.YetToPurchaseReturnQty,0)>0 order by ProductID, LotNo";
            oImportPackDetails = ImportPackDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (ImportPackDetail oItem in oImportPackDetails)
            {
                oPurchaseReturnDetail = new PurchaseReturnDetail();
                oPurchaseReturnDetail.PurchaseReturnDetailID = 0;
                oPurchaseReturnDetail.PurchaseReturnID = 0;
                oPurchaseReturnDetail.ProductID = oItem.ProductID;

                oPurchaseReturnDetail.MUnitID = oItem.MUnitID;
                //oPurchaseReturnDetail.RefDetailQty = oItem.YetToPurchaseReturnQty;
                //oPurchaseReturnDetail.ReceivedQty = oItem.YetToPurchaseReturnQty;
                //oPurchaseReturnDetail.YetToReceiveQty = oItem.YetToPurchaseReturnQty;


                oPurchaseReturnDetail.LotID = 0;
                oPurchaseReturnDetail.RefType = EnumPurchaseReturnType.ImportInvoice;
                oPurchaseReturnDetail.RefTypeInt = (int)EnumPurchaseReturnType.ImportInvoice;
                oPurchaseReturnDetail.RefObjectDetailID = oItem.ImportPackDetailID;

                oPurchaseReturnDetail.MUName = oItem.MUName;
                oPurchaseReturnDetail.MUSymbol = oItem.MUName;
                oPurchaseReturnDetail.ProductName = oItem.ProductName;
                oPurchaseReturnDetail.ProductCode = oItem.ProductCode;
                oPurchaseReturnDetail.LotNo = oItem.LotNo;
              
                oPurchaseReturnDetail.StyleID = oItem.TechnicalSheetID;
                oPurchaseReturnDetail.StyleNo = oItem.StyleNo;
                oPurchaseReturnDetails.Add(oPurchaseReturnDetail);
            }
            return oPurchaseReturnDetails;
        }

        private List<PurchaseReturnDetail> MapPurchaseReturnDetailFromImportPI(int nImportPIID, int WorkingUnitID)
        {
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            PurchaseReturnDetail oPurchaseReturnDetail = new PurchaseReturnDetail();
            List<PurchaseReturnDetail> oPurchaseReturnDetails = new List<PurchaseReturnDetail>();
            string sSQL = "SELECT HH.ProductID, HH.MUnitID, HH.RefObjectID, HH.MUName, HH.MUSymbol, HH.ProductCode, HH.ProductName, HH.StyleID, HH.StyleNo, HH.BuyerName, (SELECT TOP 1 Lot.LotID FROM Lot WHERE Lot.ParentType=103 AND Lot.ParentID=HH.GRNDetailID) AS LotID, (SELECT TOP 1 Lot.LotNo FROM Lot WHERE Lot.ParentType=103 AND Lot.ParentID=HH.GRNDetailID) AS LotNo, ISNULL((SELECT TOP 1 Lot.Balance FROM Lot WHERE Lot.ParentType=103 AND Lot.ParentID=HH.GRNDetailID),0) AS ReceivedQty FROM View_GRNDetail AS HH WHERE HH.RefType = 6 AND HH.RefObjectID IN (SELECT TT.ImportPIDetailID FROM View_ImportPIDetail AS TT WHERE TT.ImportPIID = " + nImportPIID.ToString() + ") AND HH.GRNID IN (SELECT GRN.GRNID FROM GRN WHERE GRN.StoreID=" + WorkingUnitID.ToString() + ")";
            oGRNDetails = GRNDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            foreach (GRNDetail oItem in oGRNDetails)
            {
                if (oItem.ReceivedQty > 0)
                {
                    oPurchaseReturnDetail = new PurchaseReturnDetail();
                    oPurchaseReturnDetail.PurchaseReturnDetailID = 0;
                    oPurchaseReturnDetail.PurchaseReturnID = 0;
                    oPurchaseReturnDetail.ProductID = oItem.ProductID;

                    oPurchaseReturnDetail.MUnitID = oItem.MUnitID;
                    oPurchaseReturnDetail.ReturnQty = oItem.ReceivedQty;
                    oPurchaseReturnDetail.RefDetailQty = oItem.ReceivedQty;
                    oPurchaseReturnDetail.LotBalance = oItem.ReceivedQty;
                    oPurchaseReturnDetail.YetToPurchaseReturnQty = oItem.ReceivedQty;

                    oPurchaseReturnDetail.LotID = oItem.LotID;
                    oPurchaseReturnDetail.RefType = EnumPurchaseReturnType.ImportPI;
                    oPurchaseReturnDetail.RefTypeInt = (int)EnumPurchaseReturnType.ImportPI;
                    oPurchaseReturnDetail.RefObjectDetailID = oItem.RefObjectID;
                    oPurchaseReturnDetail.MUName = oItem.MUName;
                    oPurchaseReturnDetail.MUSymbol = oItem.MUName;
                    oPurchaseReturnDetail.ProductName = oItem.ProductName;
                    oPurchaseReturnDetail.ProductCode = oItem.ProductCode;
                    oPurchaseReturnDetail.LotNo = oItem.LotNo;

                    oPurchaseReturnDetail.StyleID = oItem.StyleID;
                    oPurchaseReturnDetail.StyleNo = oItem.StyleNo;
                    oPurchaseReturnDetail.BuyerName = oItem.BuyerName;
                    oPurchaseReturnDetails.Add(oPurchaseReturnDetail);
                }
            }
            return oPurchaseReturnDetails;
        }

        #endregion
        #region 
        public ActionResult PreviewPurchaseReturn(int id)
        {
            _oPurchaseReturn = new PurchaseReturn();
            _oClientOperationSetting = new ClientOperationSetting();
            _oPurchaseReturn = _oPurchaseReturn.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseReturn.PurchaseReturnDetails = PurchaseReturnDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseReturn.PurchaseReturnDetails = _oPurchaseReturn.PurchaseReturnDetails.OrderBy(x => x.StyleNo).ToList();
            _oClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oPurchaseReturn.BusinessUnit = oBusinessUnit.Get(_oPurchaseReturn.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.PurchaseReturnPreview, (int)Session[SessionInfo.currentUserID]);
                    
            rptPurchaseReturn oReport = new rptPurchaseReturn();
            byte[] abytes = oReport.PrepareReport(_oPurchaseReturn, _oClientOperationSetting, oSignatureSetups, oCompany);
            return File(abytes, "application/pdf");
        }

      
        public ActionResult PrintPurchaseReturns(string Param)
        {
            _oPurchaseReturns = new List<PurchaseReturn>();
            string sSQLQuery = "SELECT *  FROM View_PurchaseReturn WHERE PurchaseReturnID IN (" + Param + ")";
            _oPurchaseReturns = PurchaseReturn.Gets(sSQLQuery, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            string Messge = "PurchaseReturn List";
            rptPurchaseReturns oReport = new rptPurchaseReturns();
            byte[] abytes = oReport.PrepareReport(_oPurchaseReturns, oCompany, Messge);
            return File(abytes, "application/pdf");

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
        public JsonResult GetsByReturnNo(PurchaseReturn oPurchaseReturn)
        {
            List<PurchaseReturn> oPurchaseReturns = new List<PurchaseReturn>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseReturn AS HH WHERE  (HH.ReturnNo+HH.VendorName+HH.RefObjectNo) LIKE '%" + oPurchaseReturn.ReturnNo + "%' ";
                if (oPurchaseReturn.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oPurchaseReturn.BUID;
                }
                sSQL += " ORDER BY PurchaseReturnID ASC";
                oPurchaseReturns = PurchaseReturn.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseReturns = new List<PurchaseReturn>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseReturns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WaitForDisbursed(PurchaseReturn oPurchaseReturn)
        {
            _oPurchaseReturns = new List<PurchaseReturn>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseReturn WHERE ISNULL(DisbursedBy,0)=0 ";
                if (oPurchaseReturn.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oPurchaseReturn.BUID;
                }
                sSQL += " ORDER BY PurchaseReturnID ASC";
                _oPurchaseReturns = PurchaseReturn.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseReturn = new PurchaseReturn();
                _oPurchaseReturns = new List<PurchaseReturn>();
                _oPurchaseReturn.ErrorMessage = ex.Message;
                _oPurchaseReturns.Add(_oPurchaseReturn);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseReturns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(PurchaseReturn oPurchaseReturn)
        {
            _oPurchaseReturns = new List<PurchaseReturn>();
            try
            {
                string sSQL = this.GetSQL(oPurchaseReturn);
                _oPurchaseReturns = PurchaseReturn.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseReturn = new PurchaseReturn();
                _oPurchaseReturns = new List<PurchaseReturn>();
                _oPurchaseReturn.ErrorMessage = ex.Message;
                _oPurchaseReturns.Add(_oPurchaseReturn);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseReturns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(PurchaseReturn oPurchaseReturn)
        {

            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sReturnNo = Convert.ToString(oPurchaseReturn.Remarks.Split('~')[0]);
            EnumCompareOperator eReturnDate = (EnumCompareOperator)Convert.ToInt32(oPurchaseReturn.Remarks.Split('~')[1]);
            DateTime dStartDate = Convert.ToDateTime(oPurchaseReturn.Remarks.Split('~')[2]);
            DateTime dEndDate = Convert.ToDateTime(oPurchaseReturn.Remarks.Split('~')[3]);
            int nDisbursedBy = Convert.ToInt32(oPurchaseReturn.Remarks.Split('~')[4]);
            string sSupplierIDs = Convert.ToString(oPurchaseReturn.Remarks.Split('~')[5]);
            string sProductIDs = Convert.ToString(oPurchaseReturn.Remarks.Split('~')[6]);

            string sReturn1 = "SELECT * FROM View_PurchaseReturn";
            string sReturn = "";

            #region BUID
            if (oPurchaseReturn.BUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oPurchaseReturn.BUID.ToString();
            }
            #endregion

            #region InvoiceSLNo
            if (sReturnNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ReturnNo LIKE '%" + sReturnNo + "%'";
            }
            #endregion


            #region PurchaseReturn Date
            if (eReturnDate != EnumCompareOperator.None)
            {
                if (eReturnDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eReturnDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eReturnDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eReturnDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eReturnDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eReturnDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReturnDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Received By
            if (nDisbursedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(DisbursedBy,0) = " + nDisbursedBy.ToString();
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
                sReturn = sReturn + " PurchaseReturnID IN (SELECT TT.PurchaseReturnID FROM PurchaseReturnDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            #region PurchaseReturn Type
            if (oPurchaseReturn.RefTypeInt != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefType = " + oPurchaseReturn.RefTypeInt;
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Purchase Return Register
        public ActionResult ViewPurchaseReturnRegister(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oPurchaseReturn = new PurchaseReturn();
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumPurchaseReturnType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            //ViewBag.BUID = buid;
            return View(_oPurchaseReturn);
        }

        public ActionResult ViewPurchaseReturnGroupRegister(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oPurchaseReturn = new PurchaseReturn();
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumPurchaseReturnType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            //ViewBag.BUID = buid;
            return View(_oPurchaseReturn);
        }
        private string MakeSQL(string sString)
        {
            int nCount = 0;
            string sPurchaseReturnNumber = (sString.Split('~')[nCount++]).ToString();
            int cboReturnDate = Convert.ToInt32(sString.Split('~')[nCount++]);
            DateTime ReturnStartDate = Convert.ToDateTime(sString.Split('~')[nCount++]);
            DateTime ReturnEndDate = Convert.ToDateTime(sString.Split('~')[nCount++]);
            string sSupplierNameIDs = (sString.Split('~')[nCount++]).ToString();
            int nRefType = Convert.ToInt32(sString.Split('~')[nCount++]);
            int nBUID = Convert.ToInt32(sString.Split('~')[nCount++]);


            string sReturn1 = "select * from View_PurchaseReturnDetail ";
            string sReturn = "";

            #region BUID
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseReturnID IN (SELECT PurchaseReturnID FROM PurchaseReturn WHERE BUID = " + nBUID + ") ";
            }
            #endregion

            #region Purchase Return No
            if (!string.IsNullOrEmpty(sPurchaseReturnNumber))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseReturnID IN (SELECT PurchaseReturnID FROM PurchaseReturn WHERE ReturnNo Like '%" + sPurchaseReturnNumber + "%')";
            }
            #endregion

            #region ORDER DATE SEARCH
            if (cboReturnDate > 0)
            {
                Global.TagSQL(ref sReturn);
                string sOrderDate = "";
                DateObject.CompareDateQuery(ref sOrderDate, "ReturnDate", cboReturnDate, ReturnStartDate, ReturnEndDate);
                sReturn = sReturn + "  PurchaseReturnID IN (SELECT PurchaseReturnID FROM PurchaseReturn " + sOrderDate + ")";
            }
            #endregion

            #region Supplier ID
            if (!string.IsNullOrEmpty(sSupplierNameIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseReturnID IN (SELECT PurchaseReturnID FROM PurchaseReturn WHERE SupplierID IN (" + sSupplierNameIDs + "))";
            }
            #endregion

            #region Ref Type
            if (nRefType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseReturnID IN (SELECT PurchaseReturnID FROM PurchaseReturn WHERE RefType = " + nRefType + ") ";
            }
            #endregion
            //sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        public JsonResult AdvSearch(PurchaseReturnRegister oPurchaseReturnRegister)
        {
            List<PurchaseReturnRegister> oPurchaseReturnRegisters = new List<PurchaseReturnRegister>();
            string sSQL = MakeSQL(oPurchaseReturnRegister.ErrorMessage);

            oPurchaseReturnRegisters = PurchaseReturnRegister.GetsPurchaseReturnRegister(sSQL, (int)Session[SessionInfo.currentUserID]);
            var jsonResult = Json(oPurchaseReturnRegisters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
        #region Excel
        public void ExportToExcelDateWise(string sParam)
        {
            PurchaseReturnRegister oPurchaseReturnRegister = new PurchaseReturnRegister();
            string sSQL = MakeSQL(sParam);
            List<PurchaseReturnRegister> oPurchaseReturnRegisters = new List<PurchaseReturnRegister>();
            oPurchaseReturnRegisters = PurchaseReturnRegister.GetsPurchaseReturnRegister(sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "P. R. No", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 40f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Lot No", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Store Name", Width = 35f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Ref Type", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Ref No", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Ref Date", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Supplier Name", Width = 25f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "LC No", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Style No", Width = 12f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "M.Unit", Width = 12f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Rate", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Amount", Width = 18f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Register");
                sheet.Name = "Purchase Return Register (Date Wise)";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = table_header.Count;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Purchase Return Register(Date Wise)"; cell.Style.Font.Bold = true;
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

                #region Data
                nRowIndex++;
                nStartCol = 2;

                int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                nEndCol = table_header.Count() + nStartCol;
                string sPreviousReturnDate = "";
                double dSubQty = 0;
                double dSubAmount = 0;

                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                oPurchaseReturnRegisters = oPurchaseReturnRegisters.OrderBy(x => x.ReturnDate).ToList();
                foreach (PurchaseReturnRegister obj in oPurchaseReturnRegisters)
                {
                    nStartCol = 2;

                    ExcelTool.Formatter = "";
                    if (sPreviousReturnDate != obj.ReturnDateSt)
                    {
                        ExcelTool.FillCellMerge(ref sheet, "Return Date : " + obj.ReturnDateSt, nRowIndex, nRowIndex++, nStartCol, nEndCol - 1, true, ExcelHorizontalAlignment.Left, true);
                        nStartCol = 2;
                    }
                    if (sPreviousReturnDate != obj.ReturnDateSt && nCount > 0)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Date Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 13, true, ExcelHorizontalAlignment.Right, true);
                        ExcelTool.FillCell(sheet, nRowIndex, 14, dSubQty.ToString(), true, true);
                        ExcelTool.FillCell(sheet, nRowIndex, 15, "", false, false);
                        ExcelTool.FillCell(sheet, nRowIndex++, 16, dSubAmount.ToString(), true, true);
                        nStartCol = 2; dSubQty = 0; dSubAmount = 0;
                    }
                    nStartCol = 2;
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PurchaseReturnNo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LotNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.StoreName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RefTypeSt, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RefNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RefDateSt.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SupplierName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LCNo.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.StyleNo.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MUSymbol.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Rate.ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Amount.ToString(), true, true);
                    nRowIndex++;
                    sPreviousReturnDate = obj.ReturnDateSt;
                    dSubQty = dSubQty + obj.Qty;
                    dSubAmount = dSubAmount + obj.Amount;
                }
                #region Sub Total
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Date Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 13, true, ExcelHorizontalAlignment.Right, true);
                ExcelTool.FillCell(sheet, nRowIndex, 14, dSubQty.ToString(), true, true);
                ExcelTool.FillCell(sheet, nRowIndex, 15, "", false, false);
                ExcelTool.FillCell(sheet, nRowIndex++, 16, dSubAmount.ToString(), true, true);
                #endregion

                #region Grand Total
                nStartCol = 2;
                double dGrandTotalQty = oPurchaseReturnRegisters.Sum(x => x.Qty);
                double dGrandTotalAmount = oPurchaseReturnRegisters.Sum(x => x.Amount);
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 13, true, ExcelHorizontalAlignment.Right, true);
                ExcelTool.FillCell(sheet, nRowIndex, 14, Math.Round(dGrandTotalQty, 2).ToString(), true, true);
                ExcelTool.FillCell(sheet, nRowIndex, 15, "", false, false);
                ExcelTool.FillCell(sheet, nRowIndex++, 16, Math.Round(dGrandTotalAmount, 2).ToString(), true, true);
                #endregion


                #endregion
                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PurchaseReturnRegister Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
            
        }
        public void ExportToExcelSupplierWise(string sParam)
        {
            PurchaseReturnRegister oPurchaseReturnRegister = new PurchaseReturnRegister();
            string sSQL = MakeSQL(sParam);
            List<PurchaseReturnRegister> oPurchaseReturnRegisters = new List<PurchaseReturnRegister>();
            oPurchaseReturnRegisters = PurchaseReturnRegister.GetsPurchaseReturnRegister(sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "P. R. No", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 40f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Lot No", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Store Name", Width = 35f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Ref Type", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Ref No", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Ref Date", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Return Date", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "LC No", Width = 20f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Style No", Width = 12f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "M.Unit", Width = 12f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Rate", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Amount", Width = 18f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Register");
                sheet.Name = "Purchase Return Register (Supplier Wise)";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = table_header.Count;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Purchase Return Register(Supplier Wise)"; cell.Style.Font.Bold = true;
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

                #region Data
                nRowIndex++;
                nStartCol = 2;

                int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                nEndCol = table_header.Count() + nStartCol;
                int sPreviousSupplierID = 0;
                double dSubQty = 0;
                double dSubAmount = 0;

                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                oPurchaseReturnRegisters = oPurchaseReturnRegisters.OrderBy(x => x.SupplierID).ToList();
                foreach (PurchaseReturnRegister obj in oPurchaseReturnRegisters)
                {
                    nStartCol = 2;

                    ExcelTool.Formatter = "";
                    if (sPreviousSupplierID != obj.SupplierID)
                    {
                        ExcelTool.FillCellMerge(ref sheet, "Supplier Name : " + obj.SupplierName, nRowIndex, nRowIndex++, nStartCol, nEndCol - 1, true, ExcelHorizontalAlignment.Left, true);
                        nStartCol = 2;
                    }
                    if (sPreviousSupplierID != obj.SupplierID && nCount > 0)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Supplier Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 13, true, ExcelHorizontalAlignment.Right, true);
                        ExcelTool.FillCell(sheet, nRowIndex, 14, dSubQty.ToString(), true, true);
                        ExcelTool.FillCell(sheet, nRowIndex, 15, "", false, false);
                        ExcelTool.FillCell(sheet, nRowIndex++, 16, dSubAmount.ToString(), true, true);
                        nStartCol = 2; dSubQty = 0; dSubAmount = 0;
                    }
                    nStartCol = 2;
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PurchaseReturnNo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LotNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.StoreName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RefTypeSt.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RefNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RefDateSt.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ReturnDateSt.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LCNo.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.StyleNo.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MUSymbol.ToString(), false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Rate.ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Amount.ToString(), true, true);
                    nRowIndex++;
                    sPreviousSupplierID = obj.SupplierID;
                    dSubQty = dSubQty + obj.Qty;
                    dSubAmount = dSubAmount + obj.Amount;
                }
                #region Sub Total
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Supplier Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 13, true, ExcelHorizontalAlignment.Right, true);
                ExcelTool.FillCell(sheet, nRowIndex, 14, dSubQty.ToString(), true, true);
                ExcelTool.FillCell(sheet, nRowIndex, 15, "", false, false);
                ExcelTool.FillCell(sheet, nRowIndex++, 16, dSubAmount.ToString(), true, true);
                #endregion

                #region Grand Total
                nStartCol = 2;
                double dGrandTotalQty = oPurchaseReturnRegisters.Sum(x => x.Qty);
                double dGrandTotalAmount = oPurchaseReturnRegisters.Sum(x => x.Amount);
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 13, true, ExcelHorizontalAlignment.Right, true);
                ExcelTool.FillCell(sheet, nRowIndex, 14, Math.Round(dGrandTotalQty, 2).ToString(), true, true);
                ExcelTool.FillCell(sheet, nRowIndex, 15, "", false, false);
                ExcelTool.FillCell(sheet, nRowIndex++, 16, Math.Round(dGrandTotalAmount, 2).ToString(), true, true);
                #endregion


                #endregion
                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PurchaseReturnRegister Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
            
        }


        
        #endregion

    }

}
