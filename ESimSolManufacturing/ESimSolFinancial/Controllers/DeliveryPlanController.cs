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
    public class DeliveryPlanController :Controller
    {
        #region Declaration
        DeliveryPlan _oDeliveryPlan = new DeliveryPlan();
        List<DeliveryPlan> _oDeliveryPlans = new List<DeliveryPlan>();
      
        #endregion

        #region Actions
        public ActionResult ViewDeliveryPlans(int ProductNature, int buid, int menuid)
        {
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DeliveryPlan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oDeliveryPlans = new List<DeliveryPlan>();
            //there is no need contractor id so send 0 as param
            string sSQL = "SELECT * FROM View_DeliveryPlan as DP WHERE DP.BUID = " + buid + " AND DP.ProductNature = " + ProductNature + " AND DP.YetToDeliveryChallanQty>0  Order By Sequence";
            _oDeliveryPlans = DeliveryPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            sSQL = "SELECT  * FROM View_DeliveryOrder WHERE BUID = " + buid + " AND ProductNature = "+ProductNature+" AND DeliveryOrderID NOT IN (SELECT DeliveryOrderID FROM DeliveryPlan) AND ISNULL(YetToDeliveryChallanQty,0)>0 AND CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106))>= CONVERT(DATE,CONVERT(VARCHAR(12),'01 June 2018',106)) ORDER BY DODate";//for date fixed user requirement
            ViewBag.DeliveryOrders = DeliveryOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oDeliveryPlans);
        }

        [HttpPost]
        public JsonResult Save(DeliveryPlan oDeliveryPlan)
        {
            _oDeliveryPlans = new List<DeliveryPlan>();
            try
            {
                oDeliveryPlan.ProductNature = (EnumProductNature)oDeliveryPlan.ProductNatureInInt;
                _oDeliveryPlans = oDeliveryPlan.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDeliveryPlan = new DeliveryPlan();
                _oDeliveryPlan.ErrorMessage = ex.Message;
                _oDeliveryPlans.Add(_oDeliveryPlan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliveryPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(DeliveryPlan oDeliveryPlan)
        {
            string sFeedBackMessage = "";
            try
            {
                if (oDeliveryPlan.DeliveryPlanID <= 0)
                throw new Exception("Invalid delivery challan.");
                sFeedBackMessage = oDeliveryPlan.Delete(oDeliveryPlan.DeliveryPlanID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetDeliveryOrders(DeliveryOrder oDeliveryOrder)
        {
            List<DeliveryOrder> oDeliveryOrders = new List<DeliveryOrder>();
            try
            {
                string sSQL = "SELECT  * FROM View_DeliveryOrder WHERE BUID = " + oDeliveryOrder.BUID + " AND ProductNature = " +oDeliveryOrder.ProductNatureInInt + " AND DeliveryOrderID NOT IN (SELECT DeliveryOrderID FROM DeliveryPlan) AND ISNULL(YetToDeliveryChallanQty,0)>0 AND CONVERT(DATE,CONVERT(VARCHAR(12),DODate,106))>= CONVERT(DATE,CONVERT(VARCHAR(12),'01 June 2018',106)) ORDER BY DODate";//for date fixed user requirement
                oDeliveryOrders = DeliveryOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDeliveryOrder = new DeliveryOrder();
                oDeliveryOrder.ErrorMessage = ex.Message;
                oDeliveryOrders.Add(oDeliveryOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDeliveryOrderBUWise(DeliveryPlan oDeliveryPlan)
        {
            List<DeliveryOrder> oDeliveryOrders = new List<DeliveryOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_DeliveryOrder WHERE BUID = " + oDeliveryPlan.BUID + " AND ProductNature = " + oDeliveryPlan.ProductNatureInInt + "  AND ISNULL(ApproveBy,0)!=0 AND ISNULL(YetToDeliveryChallanQty,0)>0 AND DeliveryOrderID NOT IN (SELECT DeliveryOrderID FROM DeliveryPlan WHERE  DeliveryPlanDate >='"+oDeliveryPlan.DeliveryPlanDate.ToString("dd MMM yyyy")+"' )";
                if (oDeliveryPlan.ContractorID > 0)
                {
                    sSQL += " And ContractorID =" + oDeliveryPlan.ContractorID + "";
                }
                if (!string.IsNullOrEmpty(oDeliveryPlan.DONo))
                {
                    sSQL += " And DONo Like '%" + oDeliveryPlan.DONo+ "%'";
                }
                oDeliveryOrders = DeliveryOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                DeliveryOrder oDeliveryOrder = new DeliveryOrder();
                oDeliveryOrder.ErrorMessage = ex.Message;
                oDeliveryOrders.Add(oDeliveryOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Print
        public ActionResult PrintDeliveryPlan(int BUID, int ProductNatureInInt, DateTime PlanDate)
        {
            _oDeliveryPlan = new DeliveryPlan();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (BUID > 0)
            {
             //there is no need contractor id so send 0 as param
                _oDeliveryPlan.DeliveryPlans = DeliveryPlan.GetsByBUwithDate(BUID, ProductNatureInInt, PlanDate,0, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryPlan.BusinessUnit = oBusinessUnit.Get(BUID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oDeliveryPlan.BusinessUnit = new BusinessUnit();
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oDeliveryPlan.Company = oCompany;
            if(_oDeliveryPlan.DeliveryPlans.Count>0)
            {
                rptDeliveryPlans oReport = new rptDeliveryPlans();
                byte[] abytes = oReport.PrepareReport(_oDeliveryPlan);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("There is no data for print.");
                return File(abytes, "application/pdf");
            }
            
        }
        #endregion
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
