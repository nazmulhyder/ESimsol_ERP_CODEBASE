using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
 
using ESimSol.Reports;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace ESimSolFinancial.Controllers
{

    public class OrderStepController : Controller
    {
        #region Declaration
        OrderStep _oOrderStep = new OrderStep();
        List<OrderStep> _oOrderSteps = new List<OrderStep>();
        List<HIASetup> _oHIASetups = new List<HIASetup>();
        HIASetup _oHIASetup = new HIASetup();
        string _sErrorMessage = "";
        #endregion

        #region Function 
     
        #endregion

        #region Actions

        public ActionResult ViewOrderSteps(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderStep).ToString()+","+((int)EnumModuleName.HIASetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oOrderSteps = new List<OrderStep>();           
            _oOrderSteps = OrderStep.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oOrderSteps);
        }
      

        public ActionResult ViewOrderStep(int id)
        {
            _oOrderStep = new OrderStep();
            if (id > 0)
            {
                _oOrderStep = _oOrderStep.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
           @ViewBag.TSTypes = EnumObject.jGets(typeof (EnumTSType));
           @ViewBag.TnASteps = EnumObject.jGets(typeof(EnumTnAStep));
           @ViewBag.StepTypes = EnumObject.jGets(typeof(EnumStepType));
           return View(_oOrderStep);
        }
              

        public ActionResult ViewNotifications(int id, int buid)//OrderStepID
        {
            _oHIASetups = new List<HIASetup>();
            _oHIASetups = HIASetup.GetsByOrderStepBUWise(id,buid,(int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.HIASetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return View(_oHIASetups);
        }

        [HttpPost]
        public JsonResult Save(OrderStep oOrderStep)
        {
            _oOrderStep = new OrderStep();
            try
            {
                _oOrderStep = oOrderStep;
                _oOrderStep = _oOrderStep.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderStep = new OrderStep();
                _oOrderStep.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderStep);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActiveInActive(OrderStep oOrderStep)
        {
            _oOrderStep = new OrderStep();
            try
            {
                _oOrderStep = oOrderStep;
                _oOrderStep = _oOrderStep.ActiveInActive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderStep = new OrderStep();
                _oOrderStep.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderStep);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                OrderStep oOrderStep = new OrderStep();
                sFeedBackMessage = oOrderStep.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Order Step Picker
        public ActionResult OrderStepPicker(double ts)
        {
            _oOrderSteps = new List<OrderStep>();
            string sSQL = "SELECT * FROM OrderStep WHERE ParentID = 1 Order By OrderStepID";
            _oOrderSteps = OrderStep.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            return PartialView(_oOrderSteps);
        }
        [HttpPost]
        public JsonResult GetOrderSteps(OrderStep oOrderStep)
        {
            _oOrderSteps = new List<OrderStep>();
            string sSQL = "SELECT * FROM OrderStep WHERE IsActive=1 AND StyleType =" + (int)oOrderStep.StyleType + " AND StepType =" + (int)oOrderStep.StepType + "  Order By StyleType,StepType, Sequence";
            _oOrderSteps = OrderStep.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
  
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderSteps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RefreshStepSequence(OrderStep oOrderStep)
        {
            _oOrderStep = new OrderStep();
            try
            {
                _oOrderStep = oOrderStep;
                _oOrderStep = _oOrderStep.RefreshStepSequence((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderStep = new OrderStep();
                _oOrderStep.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderStep);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion


    }
    
 
}