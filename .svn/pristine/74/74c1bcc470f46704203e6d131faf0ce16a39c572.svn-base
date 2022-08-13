using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace ESimSolFinancial.Controllers
{
	public class ProductionOrderController : Controller
	{
		#region Declaration

		ProductionOrder _oProductionOrder = new ProductionOrder();
		List<ProductionOrder> _oProductionOrders = new  List<ProductionOrder>();
        ProductionOrderDetail _oProductionOrderDetail = new ProductionOrderDetail();
        List<ProductionOrderDetail> _oProductionOrderDetails = new List<ProductionOrderDetail>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        List<POSizerBreakDown> _oPOSizerBreakDowns = new List<POSizerBreakDown>();
        POSizerBreakDown _oPOSizerBreakDown = new POSizerBreakDown();
        List<PTUUnit2> _oPTUUnit2s = new List<PTUUnit2>();
        PTUUnit2 _oPTUUnit2 = new PTUUnit2();
		#endregion

        #region Function
          private List<SizeCategory> GetDistictSizes(List<POSizerBreakDown> oPOSizerBreakDowns)
        {
            List<SizeCategory> oSizeCategoryies = new List<SizeCategory>();
            SizeCategory oSizeCategory = new SizeCategory();
            List<POSizerBreakDown> oNewPOSizerBreakDowns = oPOSizerBreakDowns;
            foreach (POSizerBreakDown oItem in oNewPOSizerBreakDowns)
            {
                if (!IsExist(oSizeCategoryies, oItem))
                {
                    oSizeCategory = new SizeCategory();
                    oSizeCategory.SizeCategoryID = oItem.SizeID;
                    oSizeCategory.SizeCategoryName = oItem.SizeName;
                    oSizeCategoryies.Add(oSizeCategory);
                }
            }

            return oSizeCategoryies;
        }

        private bool IsExist(List<SizeCategory> oSizeCategories, POSizerBreakDown oPOSizerBreakDown)
        {
            foreach (SizeCategory oITem in oSizeCategories)
            {
                if (oITem.SizeCategoryID == oPOSizerBreakDown.SizeID)
                {
                    return true;
                }
            }
            return false;
        }

        private List<ColorCategory> GetDistictColorWithProducts(List<POSizerBreakDown> oPOSizerBreakDowns)
        {
            List<ColorCategory> oColorCategories = new List<ColorCategory>();
            ColorCategory oColorCategory = new ColorCategory();
            foreach (POSizerBreakDown oItem in oPOSizerBreakDowns)
            {
                if (!IsExist(oColorCategories, oItem))
                {
                    oColorCategory = new ColorCategory();
                    oColorCategory.ColorCategoryID = oItem.ColorID;
                    oColorCategory.ColorName = oItem.ColorName;
                    oColorCategory.ObjectID = oItem.ProductID;
                    oColorCategory.ObjectName = oItem.ProductName;
                    oColorCategories.Add(oColorCategory);
                }
            }
            return oColorCategories;
        }

        private bool IsExist(List<ColorCategory> oColorCategories, POSizerBreakDown oPOSizerBreakDown)
        {
            foreach (ColorCategory oITem in oColorCategories)
            {
                if (oITem.ColorCategoryID == oPOSizerBreakDown.ColorID && oITem.ObjectID == oPOSizerBreakDown.ProductID)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

		#region Actions

        public ActionResult ViewProductionOrders(int buid, int ProductNature, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductionOrder).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            string sSQL = "SELECT * FROM View_ProductionOrder WHERE BUID = "+buid+" AND ProductNature = "+ProductNature+" AND ProductionOrderStatus = "+(int)EnumProductionOrderStatus.Intialize;
			_oProductionOrders = new List<ProductionOrder>();
            _oProductionOrders = ProductionOrder.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
			return View(_oProductionOrders);
		}

        public ActionResult ViewProductionOrder(int id, int buid)
		{
			_oProductionOrder = new ProductionOrder();
            List<POSizerBreakDown> oPOSizerBreakDowns = new List<POSizerBreakDown>();
			if (id > 0)
			{
				_oProductionOrder = _oProductionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.ProductionOrderDetails = ProductionOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.POSizerBreakDowns = POSizerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
			}
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, (int)Session[SessionInfo.currentUserID]);
			return View(_oProductionOrder);
		}

        public ActionResult ViewProductionOrderRevise(int id, int buid)
        {
            _oProductionOrder = new ProductionOrder();
            List<POSizerBreakDown> oPOSizerBreakDowns = new List<POSizerBreakDown>();
            if (id > 0)
            {
                _oProductionOrder = _oProductionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.ProductionOrderDetails = ProductionOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.POSizerBreakDowns = POSizerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, (int)Session[SessionInfo.currentUserID]);
            return View(_oProductionOrder);
        }

        #region View PO Approval Request
        public ActionResult ViewPOApprovalRequest()
        {
            _oApprovalRequest = new ApprovalRequest();
            string sSql = "SELECT * FROM View_User WHERE EmployeeID IN (SELECT EmployeeID FROM Employee WHERE EmployeeDesignationType IN(" + ((int)EnumEmployeeDesignationType.Management).ToString() + "," + ((int)EnumEmployeeDesignationType.MarketPerson).ToString() + "))";
            _oApprovalRequest.UserList = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oApprovalRequest);
        }
        public ActionResult ViewPODirApprovalRequest()
        {
            _oApprovalRequest = new ApprovalRequest();
            string sSql = "SELECT * FROM View_User WHERE EmployeeID IN (SELECT EmployeeID FROM Employee WHERE EmployeeDesignationType IN(" + ((int)EnumEmployeeDesignationType.Director).ToString() + "))";
            _oApprovalRequest.UserList = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oApprovalRequest);
        }
        #endregion

		[HttpPost]
		public JsonResult Save(ProductionOrder oProductionOrder)
		{
			_oProductionOrder = new ProductionOrder();
			try
			{
                _oProductionOrder = oProductionOrder.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oProductionOrder = new ProductionOrder();
				_oProductionOrder.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oProductionOrder);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult AcceptRevise(ProductionOrder oProductionOrder)
        {
            _oProductionOrder = new ProductionOrder();
            try
            {
                _oProductionOrder = oProductionOrder.AcceptRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionOrder = new ProductionOrder();
                _oProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				ProductionOrder oProductionOrder = new ProductionOrder();
				sFeedBackMessage = oProductionOrder.Delete(id, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(ProductionOrder oProductionOrder)
        {
            _oProductionOrder = new ProductionOrder();
            _oProductionOrder = oProductionOrder;
            try
            {
                if (oProductionOrder.ActionTypeExtra == "RequestForApproval")
                {
                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.RequestForApproval;
                    _oProductionOrder.ApprovalRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                    _oProductionOrder.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.ProductionOrder;

                }
                else if (oProductionOrder.ActionTypeExtra == "UndoRequest")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.UndoRequest;

                }
                else if (oProductionOrder.ActionTypeExtra == "Approve")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.Approve;

                }
                else if (oProductionOrder.ActionTypeExtra == "UndoApprove")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.UndoApprove;
                }
                else if (oProductionOrder.ActionTypeExtra == "Req_For_DIR_App")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.Req_For_DIR_App;
                }
                else if (oProductionOrder.ActionTypeExtra == "Dir_App")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.Dir_App;
                }
                else if (oProductionOrder.ActionTypeExtra == "Unod_DIR_App")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.Unod_DIR_App;
                }
                else if (oProductionOrder.ActionTypeExtra == "InProduction")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.InProduction;
                   
                }
                else if (oProductionOrder.ActionTypeExtra == "Production_Process")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.Production_Process;
                }
                else if (oProductionOrder.ActionTypeExtra == "Production_Done")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.Production_Done;
                }
                else if (oProductionOrder.ActionTypeExtra == "Request_For_Revise")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.Request_Revise;
                }
                else if (oProductionOrder.ActionTypeExtra == "Cancel")
                {

                    _oProductionOrder.ProductionOrderActionType = EnumProductionOrderActionType.Cancel;
                }

                _oProductionOrder = SetORSStatus(_oProductionOrder);
                _oProductionOrder = _oProductionOrder.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionOrder = new ProductionOrder();
                _oProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private ProductionOrder SetORSStatus(ProductionOrder oProductionOrder)//Set EnumOrderStatus Value
        {
            switch (oProductionOrder.ProductionOrderStatusInInt)
            {
                case 0:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.Intialize;
                        break;
                    }
                case 1:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.Request_For_Approval;
                        break;
                    }
                case 2:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.Approved;
                        break;
                    }
                case 3:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.Req_For_Dir_App;
                        break;
                    }
                case 4:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.DIR_Approved;
                        break;
                    }
                case 5:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.InProduction;
                        break;
                    }

                case 6:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.Production_In_Progress;
                        break;
                    }
                case 7:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.Production_Done;
                        break;
                    }
                case 8:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.Request_For_Revise;
                        break;
                    }
                case 9:
                    {
                        oProductionOrder.ProductionOrderStatus = EnumProductionOrderStatus.Cancel;
                        break;
                    }
            }

            return oProductionOrder;
        }
        #endregion
        #endregion


        [HttpPost]
        public JsonResult GetProductionOrderDetails(ExportSCDetail oExportSCDetail)
        {
            List<ProductionOrderDetail> oProductionOrderDetails = new List<ProductionOrderDetail>();
            List<ExportSCDetail> oExportSCDetails = new List<ExportSCDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportSCDetail WHERE ExportSCID = " + oExportSCDetail.ExportSCID + " AND ISNULL(YetToProductionOrderQty,0)>0";
                oExportSCDetails = ExportSCDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (ExportSCDetail oItem in oExportSCDetails)
                {
                    _oProductionOrderDetail = new ProductionOrderDetail();
                    _oProductionOrderDetail.ProductID = oItem.ProductID;
                    _oProductionOrderDetail.IsApplySizer = oItem.IsApplySizer;
                    _oProductionOrderDetail.ColorID = oItem.ColorID;
                    _oProductionOrderDetail.ProductDescription = oItem.ProductDescription;
                    _oProductionOrderDetail.StyleDescription = oItem.StyleNo;
                    _oProductionOrderDetail.Measurement = oItem.Measurement;
                    _oProductionOrderDetail.Qty = oItem.YetToProductionOrderQty;
                    _oProductionOrderDetail.BuyerRef = oItem.BuyerRef;
                    _oProductionOrderDetail.ModelReferenceID = oItem.ModelReferenceID;
                    _oProductionOrderDetail.ProductCode = oItem.ProductCode;
                    _oProductionOrderDetail.ProductName = oItem.ProductName;
                    _oProductionOrderDetail.ModelReferenceName = oItem.ModelReferenceName;
                    _oProductionOrderDetail.ColorName = oItem.ColorName;
                    _oProductionOrderDetail.ColorQty = oItem.ColorQty;
                    _oProductionOrderDetail.SizeName = oItem.SizeName;
                    _oProductionOrderDetail.UnitSymbol = oItem.MUName;
                    _oProductionOrderDetail.UnitID = oItem.MUnitID;
                    _oProductionOrderDetail.ExportSCDetailID = oItem.ExportSCDetailID;
                    oProductionOrderDetails.Add(_oProductionOrderDetail);
                 }
            }
            catch (Exception ex)
            {
                _oProductionOrderDetail = new ProductionOrderDetail();
                _oProductionOrderDetail.ErrorMessage = ex.Message;
                oProductionOrderDetails.Add(_oProductionOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExportSCDetails(ExportSC oExportSC)
        {
            List<ProductionOrderDetail> oProductionOrderDetails = new List<ProductionOrderDetail>();
            List<ExportSCDetail> oExportSCDetails = new List<ExportSCDetail>();
            try
            {
                //string sSQL = "SELECT * FROM View_ExportPIDetail WHERE ExportPIID = " + oExportPI.ExportPIID + " AND ISNULL(YetToProductionOrderQty,0)>0";
                string sSQL = "SELECT * FROM View_ExportSCDetail WHERE ExportSCID = " + oExportSC.ExportSCID;
                oExportSCDetails = ExportSCDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                ExportSCDetail oExportSCDetail = new ExportSCDetail();
                oExportSCDetail.ErrorMessage = ex.Message;
                oExportSCDetails.Add(oExportSCDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportSCDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

        #region SEARCHING
        private string GetSQL(string sTemp)
        {
            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dSOStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dSOEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sPONo = sTemp.Split('~')[3];
            string sPINo = sTemp.Split('~')[4];
            string sBuyerIDs = sTemp.Split('~')[5];
            string sDeliveryToIDs = sTemp.Split('~')[6];
            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[7]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[8]);
            int nMKTPersonID = Convert.ToInt32(sTemp.Split('~')[9]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[10]);
            int nProductNature = Convert.ToInt32(sTemp.Split('~')[11]);
            string sReturn1 = "SELECT * FROM View_ProductionOrder";
            string sReturn = "";

            #region PO No

            if (sPONo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PONo LIKE '%" + sPONo + "%'";
            }
            #endregion

            #region Export PI No

            if (sPINo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportPINo LIKE '%" + sPINo + "%'";
            }
            #endregion

            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Delivery Name

            if (sDeliveryToIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DeliveryTo IN (" + sDeliveryToIDs + ")";
            }
            #endregion

            #region MKT Person
            if (nMKTPersonID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MKTEmpID =" + nMKTPersonID;

            }
            #endregion

            #region Order Date Wise
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dSOStartDate.ToString("dd MMM yyyy") + "',106))";
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

            #region IsApproved
            if (IsCheckedApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApproveBy,0) != 0";
            }
            #endregion

            #region IsNotApproved
            if (IsCheckedNotApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  ISNULL(ApproveBy,0) = 0";
            }
            #endregion

            #region BU 
            if (nBUID!= 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = "+nBUID;
            }
            #endregion

            #region Product Nature
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " ISNULL(ProductNature,0) = " + nProductNature;
            #endregion

            //
            sReturn = sReturn1 + sReturn + " ORDER BY ContractorID, ProductionOrderID";
            return sReturn;
        }
        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<ProductionOrder> oProductionOrders = new List<ProductionOrder>();
            try
            {
                string sSQL = GetSQL(Temp);
                oProductionOrders = ProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProductionOrders = new List<ProductionOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByPONo(ProductionOrder oProductionOrder)
        {
            List<ProductionOrder> oProductionOrders = new List<ProductionOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_ProductionOrder AS HH WHERE HH.BUID=" + oProductionOrder.BUID.ToString() + " AND HH.PONo LIKE '%" + oProductionOrder.PONo + "%' AND ProductNature=" + oProductionOrder.ProductNatureInt.ToString() + "  ORDER BY ProductionOrderID ASC";
                oProductionOrders = ProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProductionOrders = new List<ProductionOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets from different Module
        [HttpPost]
        public JsonResult SearchByExportSCBUWise(ExportSC oExportSC)
        {
            List<ExportSC> oExportSCs = new List<ExportSC>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportSC WHERE BUID = " + oExportSC.BUID + " AND ISNULL(ProductNature,0) = " + (int)oExportSC.ProductNature + "  AND ISNULL(ApprovedBy,0)!=0 AND ISNULL(YetToProductionOrderQty,0)>0";
                if(!string.IsNullOrEmpty(oExportSC.PINo))
                {
                    sSQL += " AND PINo LIKE '%"+oExportSC.PINo+"%'";
                }
                oExportSCs = ExportSC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportSCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      
        #endregion

        #region GetProductionOrders
        [HttpPost]
        public JsonResult GetProductionOrders(ProductionOrder oProductionOrder)
        {
            _oProductionOrders = new List<ProductionOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_ProductionOrder WHERE BUID = "+oProductionOrder.BUID+" AND ISNULL(ApproveBy,0)!=0 AND ISNULL(YetToProductionScheduleQty,0)>0";
                if (oProductionOrder.PONo != "" && oProductionOrder.PONo != null)
                {
                    sSQL += " AND PONo LIKE '%"+oProductionOrder.PONo+"%'";
                }
                _oProductionOrders = ProductionOrder.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionOrder = new ProductionOrder();
                _oProductionOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPODetails(ProductionOrder oProductionOrder)
        {
            _oProductionOrderDetails = new List<ProductionOrderDetail>();
            try
            {
                _oProductionOrderDetails = ProductionOrderDetail.Gets(oProductionOrder.ProductionOrderID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionOrderDetail = new ProductionOrderDetail();
                _oProductionOrderDetail.ErrorMessage = ex.Message;
                _oProductionOrderDetails.Add(_oProductionOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReviseHistory(ProductionOrder oProductionOrder)
        {
            _oProductionOrders = new List<ProductionOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_ProductionOrderLog WHERE ProductionOrderID = " + oProductionOrder.ProductionOrderID;
                _oProductionOrders = ProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionOrder = new ProductionOrder();
                _oProductionOrder.ErrorMessage = ex.Message;
                _oProductionOrders.Add(_oProductionOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Print
        public ActionResult ProductionOrderPrintList(string sIDs, double ts)
        {
            _oProductionOrder = new ProductionOrder();
            _oProductionOrders = new List<ProductionOrder>();
            string sSql = "SELECT * FROM View_ProductionOrder WHERE ProductionOrderID IN (" + sIDs + ")";
            _oProductionOrders = ProductionOrder.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            int nUserID = Convert.ToInt32(Session[SessionInfo.currentUserID]);
            _oProductionOrder.ProductionOrderList = _oProductionOrders;
            if (_oProductionOrder.ProductionOrderList.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                _oProductionOrder.Company = oCompany;
                rptProductionOrderList oReport = new rptProductionOrderList();
                byte[] abytes = oReport.PrepareReport(_oProductionOrder);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }

        }

        public ActionResult ProductionOrderPrintPreview(int id)
        {
            _oProductionOrder = new ProductionOrder();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
          List<ESimSol.BusinessObjects.User> oSalesExecutebeUsers = new List<User>();
          ESimSol.BusinessObjects.User oDirectorUser = new User();
          string sApprovalCaption = "Managing Director";
            if (id > 0)
            {
                _oProductionOrder = _oProductionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.Buyer = oContractor.Get(_oProductionOrder.ContractorID, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.BusinessUnit = oBusinessUnit.Get(_oProductionOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.DeliveryToPerson = oContractor.Get(_oProductionOrder.DeliveryTo, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.ProductionOrderDetails = ProductionOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.POSizerBreakDowns = POSizerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductionOrder.SizeCategories = GetDistictSizes(_oProductionOrder.POSizerBreakDowns);
                _oProductionOrder.ColorCategories = GetDistictColorWithProducts(_oProductionOrder.POSizerBreakDowns);
                oSalesExecutebeUsers = ESimSol.BusinessObjects.User.GetsBySql("SELECT top 1 * FROM View_User WHERE EmployeeID = " + _oProductionOrder.MKTEmpID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDirectorUser = oDirectorUser.Get(_oProductionOrder.DirApprovedBy, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oProductionOrder.Company = oCompany;
            if (_oProductionOrder.ProductionOrderDetails.Where(x => x.IsApplySizer == false).Count() > 0)
            {
                AttachDocument oAttachDocument = new AttachDocument();                
                oAttachDocument = new  AttachDocument();
                oAttachDocument= oAttachDocument.GetUserSignature(_oProductionOrder.PrepareBy, (int)Session[SessionInfo.currentUserID]);
                Image oPrepareBySignature = this.GetSignature(oAttachDocument);

                oAttachDocument = new AttachDocument();
                if (oSalesExecutebeUsers.Count > 0)
                {
                    oAttachDocument = oAttachDocument.GetUserSignature(oSalesExecutebeUsers[0].UserID, (int)Session[SessionInfo.currentUserID]);
                }
                Image oSESignature = this.GetSignature(oAttachDocument);

                oAttachDocument = new AttachDocument();
                oAttachDocument = oAttachDocument.GetUserSignature(_oProductionOrder.DirApprovedBy, (int)Session[SessionInfo.currentUserID]);
                Image oDirApprovedSignature = this.GetSignature(oAttachDocument);
                if (oDirectorUser.UserID != 0 && oDirectorUser.EmployeeType == EnumEmployeeDesignationType.Director)
                {
                    sApprovalCaption = "Director";
                }

                byte[] abytes;
                rptProductionOrderPreview oReport = new rptProductionOrderPreview();
                abytes = oReport.PrepareReport(_oProductionOrder, oPrepareBySignature, oSESignature, oDirApprovedSignature, sApprovalCaption);
                return File(abytes, "application/pdf");
            }
            else
            {
                return RedirectToAction("MessageHelper", "User", new { message = "Sorry there is no Hanger Reference" });
            }
        }

        public ActionResult ProductionOrderPrintPreviewLog(int nLogid)
        {
            _oProductionOrder = new ProductionOrder();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<ESimSol.BusinessObjects.User> oSalesExecutebeUsers = new List<User>();
            ESimSol.BusinessObjects.User oDirectorUser = new User();
            string sApprovalCaption = "Managing Director";
            if (nLogid > 0)
            {
                _oProductionOrder = _oProductionOrder.GetByLog(nLogid, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.Buyer = oContractor.Get(_oProductionOrder.ContractorID, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.BusinessUnit = oBusinessUnit.Get(_oProductionOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.DeliveryToPerson = oContractor.Get(_oProductionOrder.DeliveryTo, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.ProductionOrderDetails = ProductionOrderDetail.GetsByLog(nLogid, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.POSizerBreakDowns = POSizerBreakDown.GetsByLog(nLogid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductionOrder.SizeCategories = GetDistictSizes(_oProductionOrder.POSizerBreakDowns);
                _oProductionOrder.ColorCategories = GetDistictColorWithProducts(_oProductionOrder.POSizerBreakDowns);
                oSalesExecutebeUsers = ESimSol.BusinessObjects.User.GetsBySql("SELECT top 1 * FROM View_User WHERE EmployeeID = " + _oProductionOrder.MKTEmpID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDirectorUser = oDirectorUser.Get(_oProductionOrder.DirApprovedBy, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oProductionOrder.Company = oCompany;
            if (_oProductionOrder.ProductionOrderDetails.Where(x => x.IsApplySizer == false).Count() > 0)
            {
                AttachDocument oAttachDocument = new AttachDocument();
                oAttachDocument = new AttachDocument();
                oAttachDocument = oAttachDocument.GetUserSignature(_oProductionOrder.PrepareBy, (int)Session[SessionInfo.currentUserID]);
                Image oPrepareBySignature = this.GetSignature(oAttachDocument);

                oAttachDocument = new AttachDocument();
                if (oSalesExecutebeUsers.Count > 0)
                {
                    oAttachDocument = oAttachDocument.GetUserSignature(oSalesExecutebeUsers[0].UserID, (int)Session[SessionInfo.currentUserID]);
                }
                Image oSESignature = this.GetSignature(oAttachDocument);
                if (oDirectorUser.UserID != 0 && oDirectorUser.EmployeeType == EnumEmployeeDesignationType.Director)
                {
                    sApprovalCaption = "Director";
                }

                oAttachDocument = new AttachDocument();
                oAttachDocument = oAttachDocument.GetUserSignature(_oProductionOrder.DirApprovedBy, (int)Session[SessionInfo.currentUserID]);
                Image oDirApprovedSignature = this.GetSignature(oAttachDocument);

                byte[] abytes;
                rptProductionOrderPreview oReport = new rptProductionOrderPreview();
                abytes = oReport.PrepareReport(_oProductionOrder, oPrepareBySignature, oSESignature, oDirApprovedSignature, sApprovalCaption);
                return File(abytes, "application/pdf");
            }
            else
            {
                return RedirectToAction("MessageHelper", "User", new { message = "Sorry there is no Hanger Reference" });
            }
        }

        public ActionResult POSizerPrint(int id)
        {
            _oProductionOrder = new ProductionOrder();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oProductionOrder = _oProductionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.Buyer = oContractor.Get(_oProductionOrder.ContractorID, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.BusinessUnit = oBusinessUnit.Get(_oProductionOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.DeliveryToPerson = oContractor.Get(_oProductionOrder.DeliveryTo, (int)Session[SessionInfo.currentUserID]);
                _oProductionOrder.ProductionOrderDetails = ProductionOrderDetail.Gets("SELECT * FROM View_ProductionOrderDetail WHERE ProductionOrderID = " + id + " AND ISNULL(IsApplySizer,0)=1 Order By ProductID", (int)Session[SessionInfo.currentUserID]);
                //_oProductionOrder.POSizerBreakDowns = POSizerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oProductionOrder.Company = oCompany;
            if (_oProductionOrder.ProductionOrderDetails.Count > 0)
            {
                AttachDocument oAttachDocument = new AttachDocument();
                oAttachDocument = new AttachDocument();
                oAttachDocument = oAttachDocument.GetUserSignature(_oProductionOrder.PrepareBy, (int)Session[SessionInfo.currentUserID]);
                Image oPrepareBySignature = this.GetSignature(oAttachDocument);

                oAttachDocument = new AttachDocument();
                oAttachDocument = oAttachDocument.GetUserSignature(_oProductionOrder.ApproveBy, (int)Session[SessionInfo.currentUserID]);
                Image oSESignature = this.GetSignature(oAttachDocument);

                oAttachDocument = new AttachDocument();
                oAttachDocument = oAttachDocument.GetUserSignature(_oProductionOrder.DirApprovedBy, (int)Session[SessionInfo.currentUserID]);
                Image oDirApprovedSignature = this.GetSignature(oAttachDocument);

                byte[] abytes;
                rptPOSizer oReport = new rptPOSizer();
                abytes = oReport.PrepareReport(_oProductionOrder, oPrepareBySignature, oSESignature, oDirApprovedSignature);
                return File(abytes, "application/pdf");
            }
            else
            {
                return RedirectToAction("MessageHelper", "User", new { message = "Sorry there is no Sizer Break Downs" });
            }
           
        }
        #endregion Print

        public Image GetCompanyLogo(Company oCompany)
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

        public Image GetSignature(AttachDocument oAttachDocument)
        {
            if (oAttachDocument.AttachFile != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oAttachDocument.AttachFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

    
    }

}
