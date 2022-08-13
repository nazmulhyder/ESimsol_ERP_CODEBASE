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
	public class OrderSheetController : Controller
	{
		#region Declaration

		OrderSheet _oOrderSheet = new OrderSheet();
		List<OrderSheet> _oOrderSheets = new  List<OrderSheet>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        ReviseRequest _oReviseRequest = new ReviseRequest();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewOrderSheets(int ORSType, int ProductNature,  int buid, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderSheet).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oOrderSheets = new List<OrderSheet>();
            //_oOrderSheets = OrderSheet.BUWiseWithProductNatureGets(buid,ProductNature,(int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.ORSType = ORSType;
            ViewBag.ProductNature = ProductNature;//EnumProductNature{Dyeing = 0,Hanger = 1,Poly = 2,Cone = 3}
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
			return View(_oOrderSheets);
		}

        public ActionResult ViewOrderSheet(int id, int buid, int nature)
		{
			_oOrderSheet = new OrderSheet();
			
			if (id > 0)
			{
				_oOrderSheet = _oOrderSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.OrderSheetDetails = OrderSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}
            ViewBag.PaymentTypes = EnumObject.jGets(typeof(EnumPaymentType));
            ViewBag.PriorityLebels = EnumObject.jGets(typeof(EnumPriorityLevel));
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Recipes = Recipe.GetsByTypeWithBUAndNature((int)EnumRecipeType.Consumtion, buid, nature, (int)Session[SessionInfo.currentUserID]);
            ViewBag.PolyMeasurementTypes = EnumObject.jGets(typeof(EnumPolyMeasurementType));
			return View(_oOrderSheet);
		}
        public ActionResult ViewOrderSheetRevise(int id, int buid, int nature)
        {
            _oOrderSheet = new OrderSheet();
            if (id > 0)
            {
                _oOrderSheet = _oOrderSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.OrderSheetDetails = OrderSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.PaymentTypes = EnumObject.jGets(typeof(EnumPaymentType));
            ViewBag.PriorityLebels = EnumObject.jGets(typeof(EnumPriorityLevel));
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Recipes = Recipe.GetsByTypeWithBUAndNature((int)EnumRecipeType.Consumtion, buid, nature, (int)Session[SessionInfo.currentUserID]);
            ViewBag.PolyMeasurementTypes = EnumObject.jGets(typeof(EnumPolyMeasurementType));
            return View(_oOrderSheet);
        }
        public ActionResult ViewOrderSheetMgt(int id)
        {
            _oOrderSheet = new OrderSheet();
            if (id > 0)
            {
                _oOrderSheet = _oOrderSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            string sSQL = "SELECT * FROM View_ExportPI WHERE OrderSheetID = " + id;
            ViewBag.ExportPIs = ExportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //sSQL = "SELECT * FROM View_ProductionOrder WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE OrderSheetID = " + id + ")";
            ViewBag.ProductionOrders = new List<ProductionOrder>();  //ProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oOrderSheet);
        }

		[HttpPost]
		public JsonResult Save(OrderSheet oOrderSheet)
		{
			_oOrderSheet = new OrderSheet();
			try
			{
				_oOrderSheet = oOrderSheet;
				_oOrderSheet = _oOrderSheet.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oOrderSheet = new OrderSheet();
				_oOrderSheet.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oOrderSheet);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}


        [HttpPost]
        public JsonResult AcceptRevise(OrderSheet oOrderSheet)
        {
            _oOrderSheet = new OrderSheet();
            try
            {
                _oOrderSheet = oOrderSheet;
                _oOrderSheet = _oOrderSheet.AcceptRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderSheet = new OrderSheet();
                _oOrderSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				OrderSheet oOrderSheet = new OrderSheet();
				sFeedBackMessage = oOrderSheet.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetReviseHistory(OrderSheet oOrderSheet)
        {
            _oOrderSheets = new List<OrderSheet>();
            try
            {
                string sSQL = "SELECT * FROM View_OrderSheetLog WHERE OrderSheetID = "+oOrderSheet.OrderSheetID;
                _oOrderSheets = OrderSheet.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderSheet = new OrderSheet();
                _oOrderSheet.ErrorMessage = ex.Message;
                _oOrderSheets.Add(_oOrderSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(OrderSheet oOrderSheet)
        {
            _oOrderSheet = new OrderSheet();
            _oOrderSheet = oOrderSheet;
            try
            {
                if (oOrderSheet.ActionTypeExtra == "RequestForApproval")
                {
                    _oOrderSheet.OrderSheetActionType = EnumOrderSheetActionType.RequestForApproval;
                    _oOrderSheet.ApprovalRequest.RequestBy = ((int)Session[SessionInfo.currentUserID]);
                    _oOrderSheet.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.OrderSheet;

                }
                else if (oOrderSheet.ActionTypeExtra == "UndoRequest")
                {
                    _oOrderSheet.OrderSheetActionType = EnumOrderSheetActionType.UndoRequest;
                }
                else if (oOrderSheet.ActionTypeExtra == "Approve")
                {

                    _oOrderSheet.OrderSheetActionType = EnumOrderSheetActionType.Approve;

                }
                else if (oOrderSheet.ActionTypeExtra == "UndoApprove")
                {

                    _oOrderSheet.OrderSheetActionType = EnumOrderSheetActionType.UndoApprove;
                }
                else if (oOrderSheet.ActionTypeExtra == "InProduction")
                {

                    _oOrderSheet.OrderSheetActionType = EnumOrderSheetActionType.InProduction;
                }
                else if (oOrderSheet.ActionTypeExtra == "Production_Done")
                {

                    _oOrderSheet.OrderSheetActionType = EnumOrderSheetActionType.Production_Done;
                }
                else if (oOrderSheet.ActionTypeExtra == "RequestRevise")
                {
                    _oOrderSheet.OrderSheetActionType = EnumOrderSheetActionType.Request_For_Revise;
                    _oOrderSheet.ReviseRequest.RequestBy = ((int)Session[SessionInfo.currentUserID]);
                    _oOrderSheet.ReviseRequest.OperationType = EnumReviseRequestOperationType.OrderSheet;
                }
                else if (oOrderSheet.ActionTypeExtra == "Cancel")
                {
                    _oOrderSheet.OrderSheetActionType = EnumOrderSheetActionType.Cancel;
                }
                _oOrderSheet = SetORSStatus(_oOrderSheet);
                _oOrderSheet = _oOrderSheet.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderSheet = new OrderSheet();
                _oOrderSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private OrderSheet SetORSStatus(OrderSheet oOrderSheet)//Set EnumOrderStatus Value
        {
            switch (oOrderSheet.OrderSheetStatusInInt)
            {
                case 0:
                    {
                        oOrderSheet.OrderSheetStatus = EnumOrderSheetStatus.Intialize;
                        break;
                    }
                case 1:
                    {
                        oOrderSheet.OrderSheetStatus = EnumOrderSheetStatus.Request_For_Approval;
                        break;
                    }
                case 2:
                    {
                        oOrderSheet.OrderSheetStatus = EnumOrderSheetStatus.Approved;
                        break;
                    }

                case 3:
                    {
                        oOrderSheet.OrderSheetStatus = EnumOrderSheetStatus.InProduction;
                        break;
                    }
                case 4:
                    {
                        oOrderSheet.OrderSheetStatus = EnumOrderSheetStatus.Production_Done;
                        break;
                    }
                case 5:
                    {
                        oOrderSheet.OrderSheetStatus = EnumOrderSheetStatus.Request_For_Revise;
                        break;
                    }
                case 6:
                    {
                        oOrderSheet.OrderSheetStatus = EnumOrderSheetStatus.Cancel;
                        break;
                    }
            }

            return oOrderSheet;
        }
        #endregion
        #endregion


		#endregion


        #region SEARCHING
        private string GetSQL(string sTemp)
        {
            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dSOStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dSOEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sPONo = sTemp.Split('~')[3];
            string sPartyPONo = sTemp.Split('~')[4];
            string sBuyerIDs = sTemp.Split('~')[5];
            string sDeliveryToIDs = sTemp.Split('~')[6];
            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[7]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[8]);
            int nMKTPersonID = Convert.ToInt32(sTemp.Split('~')[9]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[10]);
            int nORSType = Convert.ToInt32(sTemp.Split('~')[11]);
            int nProductNature = Convert.ToInt32(sTemp.Split('~')[12]);
            string sReturn1 = "SELECT * FROM View_OrderSheet";
            string sReturn = "";

            #region PO No

            if (sPONo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PONo = '" + sPONo + "'";
            }
            #endregion

            #region Party PO No

            if (sPartyPONo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PartyPONo = '" + sPartyPONo + "'";
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

            #region order Sheet Type
            if (nORSType != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderSheetType = " + nORSType;
            }
            #endregion

            #region Product Nature
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " ISNULL(ProductNature,0) = " + nProductNature;
            #endregion
            

            sReturn = sReturn1 + sReturn + " ORDER BY ContractorID, OrderSheetID";
            return sReturn;
        }
        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<OrderSheet> oOrderSheets = new List<OrderSheet>();
            try
            {
                string sSQL = GetSQL(Temp);
                oOrderSheets = OrderSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oOrderSheets = new List<OrderSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByPONo(OrderSheet oOrderSheet)
        {
            List<OrderSheet> oOrderSheets = new List<OrderSheet>();
            try
            {
                string sSQL = "SELECT * FROM View_OrderSheet AS HH WHERE HH.BUID=" + oOrderSheet.BUID.ToString() + " AND HH.PONo LIKE '%" + oOrderSheet.PONo + "%' AND ProductNature=" + oOrderSheet.ProductNatureInt.ToString() + "  ORDER BY OrderSheetID ASC";
                oOrderSheets = OrderSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oOrderSheets = new List<OrderSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets from different Module
        [HttpPost]
        public JsonResult GetsORSWithDetails(OrderSheet oOrderSheet)
        {
            List<OrderSheetDetail> oOrderSheetDetails = new List<OrderSheetDetail>();
            try
            {
                oOrderSheetDetails = OrderSheetDetail.Gets(oOrderSheet.OrderSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {                
                OrderSheetDetail oOrderSheetDetail = new OrderSheetDetail();
                oOrderSheetDetails = new List<OrderSheetDetail>();
                oOrderSheetDetail.ErrorMessage = ex.Message;
                oOrderSheetDetails.Add(oOrderSheetDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderSheetDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsORSForExportPI(ExportPI oExportPI)
        {
            List<OrderSheet> oOrderSheets = new List<OrderSheet>();
            try
            {
                string sSQL = "SELECT * FROM View_OrderSheet WHERE BUID = " + oExportPI.BUID.ToString() + " AND ProductNature=" + oExportPI.ProductNatureInt.ToString() + " AND  ISNULL(ApproveBy,0)!=0 AND YetToPIQty >0 AND OrderSheetID NOT IN (SELECT ISNULL(VEP.OrderSheetID,0) FROM ExportPI AS VEP) ";
                if (!String.IsNullOrEmpty(oExportPI.OrderSheetNo))
                {
                    sSQL += " AND PONo like '%" + oExportPI.OrderSheetNo + "%'";
                }
                if ( oExportPI.ExportPIID > 0)
                {
                    sSQL += "  OR OrderSheetID IN (SELECT OrderSheetID FROM ExportPI WHERE ExportPIID = " + oExportPI.ExportPIID.ToString() + ")";
                }
                sSQL += "Order BY OrderSheetID ASC";
                oOrderSheets = OrderSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);                
            }
            catch (Exception ex)
            {
                oOrderSheets = new List<OrderSheet>();
                OrderSheet oOrderSheet = new OrderSheet();
                oOrderSheet.ErrorMessage = ex.Message;
                oOrderSheets.Add(oOrderSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Print
        public ActionResult OrderSheetPrintList(string sIDs, double ts)
        {
            _oOrderSheet = new OrderSheet();
            _oOrderSheets = new List<OrderSheet>();
            string sSql = "SELECT * FROM View_OrderSheet WHERE OrderSheetID IN (" + sIDs + ")";
            _oOrderSheets = OrderSheet.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            int nUserID = Convert.ToInt32(Session[SessionInfo.currentUserID]);
            _oOrderSheet.OrderSheetList = _oOrderSheets;
            if (_oOrderSheet.OrderSheetList.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                _oOrderSheet.Company = oCompany;
                rptOrderSheetList oReport = new rptOrderSheetList();
                byte[] abytes = oReport.PrepareReport(_oOrderSheet);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }

        }

        public ActionResult OrderSheetPrintPreview(int id)
        {
            _oOrderSheet = new OrderSheet();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oOrderSheet = _oOrderSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.Buyer = oContractor.Get(_oOrderSheet.ContractorID, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.BusinessUnit = oBusinessUnit.Get(_oOrderSheet.BUID, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.DeliveryToPerson = oContractor.Get(_oOrderSheet.DeliveryTo, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.OrderSheetDetails = OrderSheetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oOrderSheet.Company = oCompany;

            byte[] abytes;
            rptOrderSheetPreview oReport = new rptOrderSheetPreview();
            abytes = oReport.PrepareReport(_oOrderSheet);
            return File(abytes, "application/pdf");
        }

        public ActionResult OrderSheetPrintPreviewLog(int nLogid)
        {
            _oOrderSheet = new OrderSheet();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (nLogid > 0)
            {
                _oOrderSheet = _oOrderSheet.GetByLog(nLogid, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.Buyer = oContractor.Get(_oOrderSheet.ContractorID, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.BusinessUnit = oBusinessUnit.Get(_oOrderSheet.BUID, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.DeliveryToPerson = oContractor.Get(_oOrderSheet.DeliveryTo, (int)Session[SessionInfo.currentUserID]);
                _oOrderSheet.OrderSheetDetails = OrderSheetDetail.GetsByLog(nLogid, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oOrderSheet.Company = oCompany;

            byte[] abytes;
            rptOrderSheetPreview oReport = new rptOrderSheetPreview();
            abytes = oReport.PrepareReport(_oOrderSheet);
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
