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
    public class ServiceOrderController : Controller
    {
        #region Declaration
        ServiceOrder _oServiceOrder = new ServiceOrder();
        List<ServiceOrder> _oServiceOrders = new List<ServiceOrder>();
       // ServiceOrderImage _oServiceOrderImage = new ServiceOrderImage();
        #endregion

        #region Actions
        public ActionResult ViewServiceOrderList(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ServiceOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oServiceOrders = new List<ServiceOrder>();
            string sSQL = "SELECT * FROM View_SaleOrder AS HH WHERE ISNULL(HH.ApprovedBy,0) = 0 ORDER BY HH.SaleOrderID ASC";
            _oServiceOrders = ServiceOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ServiceOrderTypes = EnumObject.jGets(typeof(EnumServiceOrderType)); ;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(_oServiceOrders);
        }
        public ActionResult ViewServiceOrder(int id)
        {
            _oServiceOrder = new ServiceOrder();
            if (id > 0)
            {
                _oServiceOrder = _oServiceOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oServiceOrder.ServiceOrderDetails = ServiceOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oServiceOrder.RegularServiceOrderDetails = _oServiceOrder.ServiceOrderDetails.Where(x => x.ServiceWorkType == EnumServiceType.RegularService).ToList();
                _oServiceOrder.ExtraServiceOrderDetails = _oServiceOrder.ServiceOrderDetails.Where(x => x.ServiceWorkType == EnumServiceType.ExtraService).ToList();
            }
            string sSQL = "SELECT * FROM Employee WHERE ISActive =1 AND EmployeeDesignationType IN(" + ((int)EnumEmployeeDesignationType.Service).ToString() + "," + ((int)EnumEmployeeDesignationType.Management).ToString() + ")";


            ViewBag.VehicleTypes = VehicleType.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Sessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FuelTypes = EnumObject.jGets(typeof(EnumFuelType));
            ViewBag.Advisors = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ServiceOrderTypes = EnumObject.jGets(typeof(EnumServiceOrderType));
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FuelStatusList = EnumObject.jGets(typeof(EnumFuelStatus));
            ViewBag.PaymentMethods = EnumObject.jGets(typeof(EnumPaymentMethod)).Where(x => x.Value == "None" || x.Value == "Cash" || x.Value == "DemandDraft" || x.Value == "Credit").ToList();
            ViewBag.VehicleRegistrationTypes = EnumObject.jGets(typeof(EnumVehicleRegistrationType)).Where(x => x.id == (int)EnumVehicleRegistrationType.Inhouse_Client || x.id == (int)EnumVehicleRegistrationType.Out_Client).ToList();
            return View(_oServiceOrder);
        }
        public ActionResult ViewServiceOrder_Update(int id)
        {
            _oServiceOrder = new ServiceOrder();
            if (id > 0)
            {
                _oServiceOrder = _oServiceOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oServiceOrder);
        }
        public ActionResult ViewServiceSchedulings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ServiceOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oServiceOrders = new List<ServiceOrder>();
            string sSQL = "SELECT * FROM View_ServiceOrder WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ServiceOrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + DateTime.Now.ToString("dd MMM yyyy") + "',106))";
            _oServiceOrders = ServiceOrder.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.ServiceOrderTypes = EnumObject.jGets(typeof(EnumServiceOrderType)); ;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oServiceOrders);
        }
        public ActionResult AdvSearchServiceOrder()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult Save(ServiceOrder oServiceOrder)
        {
            _oServiceOrder = new ServiceOrder();
            try
            {
                _oServiceOrder = oServiceOrder;
                _oServiceOrder = _oServiceOrder.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oServiceOrder = new ServiceOrder();
                _oServiceOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ServiceOrder oServiceOrder)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oServiceOrder.Delete(oServiceOrder.ServiceOrderID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult UpdateStatus(ServiceOrder oServiceOrder)
        {
            string sFeedBackMessage = "";
            try
            {
               sFeedBackMessage = oServiceOrder.UpdateStatus(oServiceOrder, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region GET/GETS FUNCTION
        [HttpPost] 
        public JsonResult GetsByStatus(ServiceOrder oServiceOrder)
        {
            List<ServiceOrder> oServiceOrders = new List<ServiceOrder>();
            try
            {
                oServiceOrders = ServiceOrder.Gets("SELECT * FROM View_ServiceOrder Where OrderStatus =" + oServiceOrder.OrderStatusInt, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oServiceOrder = new ServiceOrder();
                oServiceOrder.ErrorMessage = ex.Message;
                oServiceOrders.Add(oServiceOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oServiceOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ServiceOrderSearchByNo(ServiceOrder oServiceOrder)
        {
            _oServiceOrders = new List<ServiceOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_ServiceOrder WHERE ServiceOrderNo LIKE '%" + oServiceOrder.ServiceOrderNo + "%'";
                _oServiceOrders = ServiceOrder.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oServiceOrder = new ServiceOrder();
                _oServiceOrder.ErrorMessage = ex.Message;
                _oServiceOrders.Add(_oServiceOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByRegOrOrderNo(ServiceOrder oServiceOrder)
        {
            List<ServiceOrder> oServiceOrders = new List<ServiceOrder>();
            try
            {
                //string sSQL = "SELECT * FROM View_ServiceOrder WHERE ServiceOrderID NOT IN (SELECT ServiceOrderID FROM ServiceInvoice) AND OrderStatus !=4 AND OrderStatus>0 AND (ServiceOrderNo+VehicleRegNo) LIKE ('%" + oServiceOrder.ServiceOrderNo + "%')";
                string sSQL = "SELECT * FROM View_ServiceOrder WHERE OrderStatus !=4 AND OrderStatus>0 AND (ServiceOrderNo+VehicleRegNo) LIKE ('%" + oServiceOrder.ServiceOrderNo + "%')";
                oServiceOrders = ServiceOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oServiceOrder = new ServiceOrder();
                oServiceOrder.ErrorMessage = ex.Message;
                oServiceOrders.Add(oServiceOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oServiceOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByRegOrVIN(ServiceOrder oServiceOrder)
        {
            List<ServiceOrder> oServiceOrders = new List<ServiceOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_ServiceOrder Where  (ISNULL(ChassisNo,'')+ISNULL(VehicleRegNo,'')) LIKE ('%" + oServiceOrder.ServiceOrderNo + "%')";
                oServiceOrders = ServiceOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oServiceOrder = new ServiceOrder();
                oServiceOrder.ErrorMessage = ex.Message;
                oServiceOrders.Add(oServiceOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oServiceOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetServiceOrderDetailsByOrderID(ServiceOrderDetail oServiceOrderDetail)
        {
            List<ServiceOrderDetail> oServiceOrderDetails = new List<ServiceOrderDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_ServiceOrderDetail WHERE ServiceOrderID=" + oServiceOrderDetail.ServiceOrderID; //VechileOrder
                oServiceOrderDetails = ServiceOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oServiceOrderDetail = new ServiceOrderDetail();
                oServiceOrderDetail.ErrorMessage = ex.Message;
                oServiceOrderDetails.Add(oServiceOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oServiceOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        #endregion

        #endregion

        #region Advance Search
        [HttpPost]
        public JsonResult AdvSearch(ServiceOrder oServiceOrder)
        {
            List<ServiceOrder> oServiceOrders = new List<ServiceOrder>();
            try
            {
                string sSQL = GetSQL(oServiceOrder.Params);
                oServiceOrders = ServiceOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oServiceOrder.ErrorMessage = ex.Message;
                oServiceOrders.Add(oServiceOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oServiceOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(string sTemp)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();

            int nCount = 0;
            string sOrderNo = Convert.ToString(sTemp.Split('~')[nCount++]);
            int nDateCriteria_Issue = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dStart_Issue = Convert.ToDateTime(sTemp.Split('~')[nCount++]),
                    dEnd_Issue = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            
            int nServiceOrderDateCompare = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            DateTime dtServiceOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[nCount++]);
            DateTime dtServiceOrderEndDate = Convert.ToDateTime(sTemp.Split('~')[nCount++]);

            string sCustomerIDs = sTemp.Split('~')[nCount++];
           
            string sReturn1 = "SELECT * FROM View_ServiceOrder ";
            string sReturn = "";

            #region Order No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ServiceOrderNo LIKE '%" + sOrderNo + "%'";
            }
            #endregion

            #region Date Wise
            DateObject.CompareDateQuery(ref sReturn, " IssueDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);
            DateObject.CompareDateQuery(ref sReturn, " ServiceOrderDate", nServiceOrderDateCompare, dtServiceOrderDateStart, dtServiceOrderEndDate);
            #endregion


            #region Customer IDs
            if (!string.IsNullOrEmpty(sCustomerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CustomerID IN (" + sCustomerIDs + ") ";
            }
            #endregion
          
            //#region SL No
            //if (!string.IsNullOrEmpty(sRegNo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " VehicleRegNo LIKE '%" + sRegNo + "%'";
            //}
            //#endregion
           
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        #endregion



        #region Actions
        public ActionResult ViewServiceOrderReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ServiceOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oServiceOrders = new List<ServiceOrder>();
            //_oServiceOrders = ServiceOrder.Gets(""(int)Session[SessionInfo.currentUserID]);
            ViewBag.ServiceOrderTypes = EnumObject.jGets(typeof(EnumServiceOrderType)); ;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(_oServiceOrders);
        }
        #endregion


        #region PrintList
        public ActionResult PrintList(string ids, int buid)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                _oServiceOrders = ServiceOrder.Gets("SELECT * FROM View_ServiceOrder WHERE ServiceOrderID IN (" + ids + ")", (int)Session[SessionInfo.currentUserID]);
            } 
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptServiceOrderList oReport = new rptServiceOrderList();
            byte[] abytes = oReport.PrepareReport(_oServiceOrders, oCompany, oBusinessUnit, "Service Order");
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintListService(string ids, int buid)
        {
            if (!string.IsNullOrEmpty(ids)) 
            {
                _oServiceOrders = ServiceOrder.Gets("SELECT * FROM View_ServiceOrder WHERE ServiceOrderID IN ("+ids+")", (int)Session[SessionInfo.currentUserID]);
            } 
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptServiceOrderList oReport = new rptServiceOrderList();
            byte[] abytes = oReport.PrepareReport_Service(_oServiceOrders, oCompany, oBusinessUnit, "Service Report");
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintSchedules(string ids, int buid)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                _oServiceOrders = ServiceOrder.Gets("SELECT * FROM View_ServiceOrder WHERE ServiceOrderID IN (" + ids + ")", (int)Session[SessionInfo.currentUserID]);
            } 
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptServiceOrderList oReport = new rptServiceOrderList();
            byte[] abytes = oReport.PrepareReport_Schedules(_oServiceOrders, oCompany, oBusinessUnit, "Service Scheduling");
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintServiceOrderPreview(int id)
        {
            _oServiceOrder = new ServiceOrder();
            _oServiceOrder = _oServiceOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oServiceOrder.ServiceOrderDetails = ServiceOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<AttachDocument> oAttachDocuments = new List<AttachDocument>();
            oAttachDocuments = AttachDocument.Gets_WithAttachFile(_oServiceOrder.ServiceOrderID, (int)EnumAttachRefType.ServiceOrder, (int)Session[SessionInfo.currentUserID]);
            if (oAttachDocuments.Count > 0)
            {
                _oServiceOrder.Img_Damages = GetAttachmentDOC(oAttachDocuments[0]);
            }

            rptServiceOrder oReport = new rptServiceOrder();
            byte[] abytes = oReport.PrepareReport(_oServiceOrder, oCompany);
            return File(abytes, "application/pdf");
        }

        public Image GetAttachmentDOC(AttachDocument oAttachDocument)
        {
            if (oAttachDocument.AttachFile != null)
            {

                string fileDirectory = Server.MapPath("~/Content/SignatureImage.jpg");
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

        #region Get Company Logo
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

        #endregion
    }
}
