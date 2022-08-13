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

namespace ESimSolFinancial.Controllers
{
	public class WorkOrderController : Controller
	{
		#region Declaration

		WorkOrder _oWorkOrder = new WorkOrder();
		List<WorkOrder> _oWorkOrders = new  List<WorkOrder>();
        WorkOrderDetail _oWorkOrderDetail = new WorkOrderDetail();
        List<WorkOrderDetail> _oWorkOrderDetails = new List<WorkOrderDetail>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        ReviseRequest _oReviseRequest = new ReviseRequest();

		#endregion

		#region Actions
		public ActionResult ViewWorkOrders(int buid, int menuid)
		{            
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.WorkOrder).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM Users";
            if (buid>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " WHERE UserID  IN (SELECT DBUserID FROM WorkOrder WHERE BUID = " + buid + ")";
            }            
            _oWorkOrders = new List<WorkOrder>();
            ViewBag.Merchandisers = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Users = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
			return View(_oWorkOrders);
		}
        public ActionResult ViewWorkOrder(int id, int buid)
		{
			_oWorkOrder = new WorkOrder();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (id > 0)
            {
                _oWorkOrder = _oWorkOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oWorkOrder.WorkOrderDetails = WorkOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oWorkOrder.WOTermsAndConditions= WOTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oWorkOrder.CurrencyID = oCompany.BaseCurrencyID;
                _oWorkOrder.CRate = 1;
            }
            ViewBag.Merchandisers = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany;
			return View(_oWorkOrder);
		}
        public ActionResult ViewWorkOrderRevise(int id, int buid)
        {
            _oWorkOrder = new WorkOrder();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (id > 0)
            {
                _oWorkOrder = _oWorkOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oWorkOrder.WorkOrderDetails = WorkOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oWorkOrder.WOTermsAndConditions = WOTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oWorkOrder.CurrencyID = oCompany.BaseCurrencyID;
                _oWorkOrder.CRate = 1;
            }
            ViewBag.Merchandisers = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany;
            return View(_oWorkOrder);
        }

		[HttpPost]
		public JsonResult Save(WorkOrder oWorkOrder)
		{
			_oWorkOrder = new WorkOrder();
			try
			{
				_oWorkOrder = oWorkOrder;
				_oWorkOrder = _oWorkOrder.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oWorkOrder = new WorkOrder();
				_oWorkOrder.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oWorkOrder);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult AcceptRevise(WorkOrder oWorkOrder)
        {
            _oWorkOrder = new WorkOrder();
            try
            {
                _oWorkOrder = oWorkOrder;
                _oWorkOrder = _oWorkOrder.AcceptRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWorkOrder = new WorkOrder();
                _oWorkOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWorkOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				WorkOrder oWorkOrder = new WorkOrder();
				sFeedBackMessage = oWorkOrder.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetReviseHistory(WorkOrder oWorkOrder)
        {
            _oWorkOrders = new List<WorkOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_WorkOrderLog WHERE WorkOrderID = "+oWorkOrder.WorkOrderID;
                _oWorkOrders = WorkOrder.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWorkOrder = new WorkOrder();
                _oWorkOrder.ErrorMessage = ex.Message;
                _oWorkOrders.Add(_oWorkOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWorkOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(WorkOrder oWorkOrder)
        {
            _oWorkOrder = new WorkOrder();
            _oWorkOrder = oWorkOrder;
            try
            {
                if (oWorkOrder.ActionTypeExtra == "RequestForApproval")
                {
                    _oWorkOrder.WorkOrderActionType = EnumWorkOrderActionType.RequestForApproval;
                    _oWorkOrder.ApprovalRequest.RequestBy = ((int)Session[SessionInfo.currentUserID]);
                    _oWorkOrder.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.WorkOrder;

                }
                else if (oWorkOrder.ActionTypeExtra == "UndoRequest")
                {
                    _oWorkOrder.WorkOrderActionType = EnumWorkOrderActionType.UndoRequest;
                }
                else if (oWorkOrder.ActionTypeExtra == "Approve")
                {

                    _oWorkOrder.WorkOrderActionType = EnumWorkOrderActionType.Approve;

                }
                else if (oWorkOrder.ActionTypeExtra == "UndoApprove")
                {

                    _oWorkOrder.WorkOrderActionType = EnumWorkOrderActionType.UndoApprove;
                }
                else if (oWorkOrder.ActionTypeExtra == "InReceived")
                {

                    _oWorkOrder.WorkOrderActionType = EnumWorkOrderActionType.InReceived;
                }
                else if (oWorkOrder.ActionTypeExtra == "Received_Done")
                {

                    _oWorkOrder.WorkOrderActionType = EnumWorkOrderActionType.Received_Done;
                }
                else if (oWorkOrder.ActionTypeExtra == "RequestRevise")
                {
                    _oWorkOrder.WorkOrderActionType = EnumWorkOrderActionType.Request_For_Revise;
                    _oWorkOrder.ReviseRequest.RequestBy = ((int)Session[SessionInfo.currentUserID]);
                    _oWorkOrder.ReviseRequest.OperationType = EnumReviseRequestOperationType.WorkOrder;
                }
                else if (oWorkOrder.ActionTypeExtra == "Cancel")
                {
                    _oWorkOrder.WorkOrderActionType = EnumWorkOrderActionType.Cancel;
                }
                _oWorkOrder = SetORSStatus(_oWorkOrder);
                _oWorkOrder = _oWorkOrder.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWorkOrder = new WorkOrder();
                _oWorkOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWorkOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private WorkOrder SetORSStatus(WorkOrder oWorkOrder)//Set EnumOrderStatus Value
        {
            switch (oWorkOrder.WorkOrderStatusInt)
            {
                case 0:
                    {
                        oWorkOrder.WorkOrderStatus = EnumWorkOrderStatus.Intialize;
                        break;
                    }
                case 1:
                    {
                        oWorkOrder.WorkOrderStatus = EnumWorkOrderStatus.Request_For_Approval;
                        break;
                    }
                case 2:
                    {
                        oWorkOrder.WorkOrderStatus = EnumWorkOrderStatus.Approved;
                        break;
                    }

                case 3:
                    {
                        oWorkOrder.WorkOrderStatus = EnumWorkOrderStatus.InReceived;
                        break;
                    }
                case 4:
                    {
                        oWorkOrder.WorkOrderStatus = EnumWorkOrderStatus.Received_Done;
                        break;
                    }
                case 5:
                    {
                        oWorkOrder.WorkOrderStatus = EnumWorkOrderStatus.Request_For_Revise;
                        break;
                    }
                case 6:
                    {
                        oWorkOrder.WorkOrderStatus = EnumWorkOrderStatus.Cancel;
                        break;
                    }
            }

            return oWorkOrder;
        }
        #endregion
        #endregion


		#endregion


        #region SEARCHING
        private string GetSQL(string sTemp)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            int nWorkOrderDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dWOStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dWOEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sFileNo = sTemp.Split('~')[3];
            string sWorkOrderNo = sTemp.Split('~')[4];
            string sSupplierIDs = sTemp.Split('~')[5];            
            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[6]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[7]);
            int nMerchandiserID = Convert.ToInt32(sTemp.Split('~')[8]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[9]);
            int nOperationBy = Convert.ToInt32(sTemp.Split('~')[10]);            
            string sReturn1 = "SELECT * FROM View_WorkOrder";
            string sReturn = "";

            #region File No
            if (sFileNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FileNo LIKE '%" + sFileNo + "%'";
            }
            #endregion

            #region WorkOrderNo
            if (sWorkOrderNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkOrderNo LIKE '%" + sWorkOrderNo + "%'";
            }
            #endregion

            #region Supplier Name
            if (sSupplierIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SupplierID IN (" + sSupplierIDs + ")";
            }
            #endregion

            #region Merchandiser
            if (nMerchandiserID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID =" + nMerchandiserID.ToString();

            }
            #endregion

            #region WorkOrderDate
            if (nWorkOrderDate > 0)
            {
                if (nWorkOrderDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WorkOrderDate = '" + dWOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nWorkOrderDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WorkOrderDate != '" + dWOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nWorkOrderDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WorkOrderDate > '" + dWOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nWorkOrderDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WorkOrderDate < '" + dWOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nWorkOrderDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WorkOrderDate>= '" + dWOStartDate.ToString("dd MMM yyyy") + "' AND WorkOrderDate < '" + dWOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nWorkOrderDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WorkOrderDate< '" + dWOStartDate.ToString("dd MMM yyyy") + "' OR WorkOrderDate > '" + dWOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
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
            if (nBUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region OperationBy
            if (nOperationBy > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DBUserID =" + nOperationBy.ToString();
            }
            #endregion
                        
            sReturn = sReturn1 + sReturn + " ORDER BY WorkOrderID";
            return sReturn;
        }
        [HttpGet]
        public JsonResult WorkOrdersAdvSearch(string Temp)
        {
            List<WorkOrder> oWorkOrders = new List<WorkOrder>();
            try
            {
                string sSQL = GetSQL(Temp);
                oWorkOrders = WorkOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWorkOrders = new List<WorkOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByWONo(WorkOrder oWorkOrder)
        {
            List<WorkOrder> oWorkOrders = new List<WorkOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_WorkOrder AS HH WHERE HH.BUID=" + oWorkOrder.BUID.ToString() + " AND (HH.FileNo+HH.WorkOrderNo+HH.SupplierName) LIKE '%" + oWorkOrder.WorkOrderNo + "%' ORDER BY WorkOrderID ASC";
                oWorkOrders = WorkOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWorkOrders = new List<WorkOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WaitingSearch(WorkOrder oWorkOrder)
        {
            List<WorkOrder> oWorkOrders = new List<WorkOrder>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_WorkOrder AS HH WHERE ISNULL(HH.ApproveBy,0) = 0 ";
                if (oWorkOrder.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = " + oWorkOrder.BUID ;
                }
                sSQL += " ORDER BY WorkOrderID ASC";
                oWorkOrders = WorkOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWorkOrders = new List<WorkOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PickWorkOrderDetails(BillOfMaterial oBillOfMaterial)
        {
            List<BillOfMaterial> oBillOfMaterials = new List<BillOfMaterial>();
            _oWorkOrderDetails = new List<WorkOrderDetail>();
            try
            {
                oBillOfMaterials = BillOfMaterial.Gets(oBillOfMaterial.TechnicalSheetID,  (int)Session[SessionInfo.currentUserID]);
                foreach(BillOfMaterial oItem in oBillOfMaterials)
                {
                    _oWorkOrderDetail = new WorkOrderDetail();
                    _oWorkOrderDetail.ProductID = oItem.ProductID;
                    _oWorkOrderDetail.StyleID = oItem.TechnicalSheetID;
                    _oWorkOrderDetail.ColorID = oItem.ColorID;
                    _oWorkOrderDetail.SizeID = oItem.SizeID;
                    _oWorkOrderDetail.UnitID = oItem.MUnitID;
                    _oWorkOrderDetail.Qty = oItem.ReqQty;
                    _oWorkOrderDetail.ProductCode = oItem.ProductCode;
                    _oWorkOrderDetail.ProductName = oItem.ProductName;
                    _oWorkOrderDetail.IsApplyColor = oItem.IsApplyColor;
                    _oWorkOrderDetail.IsApplySize = oItem.IsApplySize;
                    _oWorkOrderDetail.ColorName = oItem.ColorName;
                    _oWorkOrderDetail.SizeName = oItem.SizeName;
                    _oWorkOrderDetail.StyleNo = oItem.StyleNo;
                    _oWorkOrderDetail.UnitName = oItem.UnitName;
                    _oWorkOrderDetail.UnitSymbol = oItem.Symbol;
                    _oWorkOrderDetail.Reference = oItem.Reference;
                    _oWorkOrderDetails.Add(_oWorkOrderDetail);
                }
            }
            catch (Exception ex)
            {
                _oWorkOrderDetail = new WorkOrderDetail();
                _oWorkOrderDetail.ErrorMessage = ex.Message;
                _oWorkOrderDetails.Add(_oWorkOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWorkOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets from different Module
        [HttpPost]
        public JsonResult GetsORSWithDetails(WorkOrder oWorkOrder)
        {
            List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
            try
            {
                oWorkOrderDetails = WorkOrderDetail.Gets(oWorkOrder.WorkOrderID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {                
                WorkOrderDetail oWorkOrderDetail = new WorkOrderDetail();
                oWorkOrderDetails = new List<WorkOrderDetail>();
                oWorkOrderDetail.ErrorMessage = ex.Message;
                oWorkOrderDetails.Add(oWorkOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         public JsonResult GetsWODWithGRN(WorkOrder oWorkOrder)
        {
            List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
            try
            {                
                string sSQL = " ";
                if(oWorkOrder.WorkOrderID>0)
                {
                    sSQL += "SELECT * FROM View_WorkOrderDetail WHERE WorkOrderID = " + oWorkOrder.WorkOrderID + " AND WorkOrderStatus Between " + (int)EnumWorkOrderStatus.Approved + " AND " + (int)EnumWorkOrderStatus.Request_For_Revise;
                    sSQL += "  AND ProductID IN (SELECT ProductID FROM View_GRNDetail WHERE GRNID IN (SELECT GRNID FROM GRN WHERE RefObjectID=" + oWorkOrder.WorkOrderID + " AND GRNType=3))";
                }
                else
                {
                    sSQL += "SELECT * FROM View_WorkOrderDetail WHERE SupplierID = " + oWorkOrder.SupplierID + " AND ISNULL(YetToInvoiceQty,0)>0   AND WorkOrderStatus Between " + (int)EnumWorkOrderStatus.Approved + " AND " + (int)EnumWorkOrderStatus.Request_For_Revise;
                    if(!string.IsNullOrWhiteSpace(oWorkOrder.WorkOrderNo))
                    {
                        sSQL+=" AND WorkOrderNo LIKE '%"+oWorkOrder.WorkOrderNo+"%'";
                    }
                }
                 
                oWorkOrderDetails = WorkOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {                
                WorkOrderDetail oWorkOrderDetail = new WorkOrderDetail();
                oWorkOrderDetails = new List<WorkOrderDetail>();
                oWorkOrderDetail.ErrorMessage = ex.Message;
                oWorkOrderDetails.Add(oWorkOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
         public JsonResult BillDone(WorkOrder oWorkOrder)
         {
             try
             {
                 oWorkOrder = oWorkOrder.BillDone((int)Session[SessionInfo.currentUserID]);
             }
             catch (Exception ex)
             {
                 oWorkOrder = new WorkOrder();
                 oWorkOrder.ErrorMessage = ex.Message;   
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(oWorkOrder);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
        
        #endregion

        #region Print
        public ActionResult WorkOrderPrintList(string sIDs, double ts)
        {
            _oWorkOrder = new WorkOrder();
            _oWorkOrders = new List<WorkOrder>();
            string sSql = "SELECT * FROM View_WorkOrder WHERE WorkOrderID IN (" + sIDs + ")";
            _oWorkOrders = WorkOrder.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            int nUserID = Convert.ToInt32(Session[SessionInfo.currentUserID]);
            _oWorkOrder.WorkOrderList = _oWorkOrders;
            if (_oWorkOrder.WorkOrderList.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(_oWorkOrders[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany.Name = oBusinessUnit.Name;
                oCompany.Address = oBusinessUnit.Address;
                oCompany.Phone = oBusinessUnit.Phone;
                oCompany.Email = oBusinessUnit.Email;
                oCompany.WebAddress = oBusinessUnit.WebAddress;
                _oWorkOrder.Company = oCompany;

                rptWorkOrderList oReport = new rptWorkOrderList();
                byte[] abytes = oReport.PrepareReport(_oWorkOrder);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }

        }

        public ActionResult WorkOrderPrintPreview(int id)
        {
            _oWorkOrder = new WorkOrder();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oWorkOrder = _oWorkOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oWorkOrder.BusinessUnit = oBusinessUnit.Get(_oWorkOrder.BUID, (int)Session[SessionInfo.currentUserID]);                
                _oWorkOrder.WorkOrderDetails = WorkOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oWorkOrder.WOTermsAndConditions= WOTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oWorkOrder.Company = oCompany;
            //_oWorkOrder.SignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.WorkOrderPreview, (int)Session[SessionInfo.currentUserID]);
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.WorkOrder + " AND BUID = " + _oWorkOrder.BUID + "  Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);


            byte[] abytes;
            rptWorkOrderPreview oReport = new rptWorkOrderPreview();
            abytes = oReport.PrepareReport(_oWorkOrder, oApprovalHeads);
            return File(abytes, "application/pdf");
        }

        public ActionResult WorkOrderPrintPreview2(int id)
        {
            _oWorkOrder = new WorkOrder();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oWorkOrder = _oWorkOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oWorkOrder.BusinessUnit = oBusinessUnit.Get(_oWorkOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                _oWorkOrder.WorkOrderDetails = WorkOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oWorkOrder.WOTermsAndConditions = WOTermsAndCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oWorkOrder.Company = oCompany;
            _oWorkOrder.SignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.WorkOrderPreview, (int)Session[SessionInfo.currentUserID]);

            byte[] abytes;
            rptWorkOrderPreview2 oReport = new rptWorkOrderPreview2();
            abytes = oReport.PrepareReport(_oWorkOrder);
            return File(abytes, "application/pdf");
        }

        public ActionResult WorkOrderPrintPreviewLog(int nLogid)
        {
            _oWorkOrder = new WorkOrder();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (nLogid > 0)
            {
                _oWorkOrder = _oWorkOrder.GetByLog(nLogid, (int)Session[SessionInfo.currentUserID]);                
                _oWorkOrder.BusinessUnit = oBusinessUnit.Get(_oWorkOrder.BUID, (int)Session[SessionInfo.currentUserID]);                
                _oWorkOrder.WorkOrderDetails = WorkOrderDetail.GetsByLog(nLogid, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oWorkOrder.Company = oCompany;

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            //string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.WorkOrder + " AND BUID = " + _oWorkOrder.BUID + "  Order By Sequence";
            //oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            byte[] abytes;
            rptWorkOrderPreview oReport = new rptWorkOrderPreview();
            abytes = oReport.PrepareReport(_oWorkOrder, oApprovalHeads);
            return File(abytes, "application/pdf");
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

    
    }

}
