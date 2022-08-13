using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
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
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;

namespace ESimSolFinancial.Controllers
{
    public class HumanInteractionAgentController : Controller
    {
        #region Declaration
        HumanInteractionAgent _oHumanInteractionAgent = new HumanInteractionAgent();
        List<HumanInteractionAgent> _oHumanInteractionAgents = new List<HumanInteractionAgent>();

        HIASetup _oHIASetup = new HIASetup();
        List<HIASetup> _oHIASetups = new List<HIASetup>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private bool ValidateInput_HIASetup(HIASetup oHIASetup)
        {
            if (oHIASetup.SetupName == null || oHIASetup.SetupName == "")
            {
                _sErrorMessage = "Please enter setup Name";
                return false;
            }

            return true;
        }
        #endregion

        #region HIASetup
        public ActionResult ViewHIASetups(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oHIASetups = new List<HIASetup>();
            _oHIASetups = HIASetup.Gets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oHIASetups);
        }

        public ActionResult ViewHIASetup(int nHIASetupID, double ts)
        {
            _oHIASetup = new HIASetup();

            if (nHIASetupID > 0)
            {
                _oHIASetup = _oHIASetup.Get(nHIASetupID, (int)Session[SessionInfo.currentUserID]);
                _oHIASetup.HIAUserAssigns = HIAUserAssign.Gets(nHIASetupID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oHIASetup = new HIASetup();
            }
            return View(_oHIASetup);
        }


        public ActionResult ViewSystemGeneratedHIASetup(int nHIASetupID, int nOrderStepID, double ts)
        {
            _oHIASetup = new HIASetup();
            OrderStep oOrderStep = new OrderStep();
            OrderStep oParentOrderStep = new OrderStep();

            if (nHIASetupID > 0)
            {
                _oHIASetup = _oHIASetup.Get(nHIASetupID, (int)Session[SessionInfo.currentUserID]);
                _oHIASetup.HIAUserAssigns = HIAUserAssign.Gets(nHIASetupID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oOrderStep = oOrderStep.Get(nOrderStepID, (int)Session[SessionInfo.currentUserID]);//Get Order STep
                _oHIASetup = new HIASetup();
                //Name configure
               _oHIASetup.SetupName = oOrderStep.OrderStepName;
                _oHIASetup.OrderStepID = nOrderStepID;
                _oHIASetup.DBTable = "View_TAPDetail";
                _oHIASetup.KeyColumn = "TAPDetailID";
                _oHIASetup.FileNumberColumn = "OrderRecapNo";
                _oHIASetup.SenderColumn = "System";
                _oHIASetup.ReceiverColumn = "Assigned User with Merchandiser";
                _oHIASetup.LinkReference = "TAP/ViewTAP";
                _oHIASetup.OperationName = "TAPHeader";
                _oHIASetup.OperationValue = "Approved Time Action Plan";
                _oHIASetup.WhereClause = "N/A";
                _oHIASetup.MessageBodyText = "Depend of HIASetup Type(Auto Generate)";
            }
            return View(_oHIASetup);
        }


        [HttpPost]
        public JsonResult Save(HIASetup oHIASetup)
        {
            _oHIASetup = new HIASetup();
            try
            {
                _oHIASetup = oHIASetup;
                _oHIASetup.HIASetupType = (EnumHIASetupType)_oHIASetup.HIASetupTypeInInt;
                _oHIASetup.TimeEventType = (EnumTimeEventType)_oHIASetup.TimeEventTypeInInt;
                if (this.ValidateInput_HIASetup(_oHIASetup))
                {
                    _oHIASetup = _oHIASetup.Save((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oHIASetup.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oHIASetup = new HIASetup();
                _oHIASetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHIASetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string smessage = "";
            try
            {
                HIASetup oHIASetup = new HIASetup();
                smessage = oHIASetup.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult ViewNotifications()
        {
            _oHumanInteractionAgents = new List<HumanInteractionAgent>();
            _oHumanInteractionAgents = HumanInteractionAgent.Gets(true, (int)Session[SessionInfo.currentUserID]);
            return View(_oHumanInteractionAgents);
        }

        [HttpPost]
        public JsonResult SetNotification()
        {
            int nNotificationCount = 0;
            try
            {
                HumanInteractionAgent oHumanInteractionAgent = new HumanInteractionAgent();
                nNotificationCount = oHumanInteractionAgent.GetHIA_NotificationCount((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                nNotificationCount = 0;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(nNotificationCount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsNotifications()
        {
            _oHumanInteractionAgents = new List<HumanInteractionAgent>();
            try
            {
                HumanInteractionAgent oHumanInteractionAgent = new HumanInteractionAgent();
                _oHumanInteractionAgents = HumanInteractionAgent.Gets(false, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oHumanInteractionAgent = new HumanInteractionAgent();
                _oHumanInteractionAgent.ErrorMessage = ex.Message;
                _oHumanInteractionAgents.Add(_oHumanInteractionAgent);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHumanInteractionAgents);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateRead(HumanInteractionAgent oHIA)
        {
            HumanInteractionAgent oHumanInteractionAgent = new HumanInteractionAgent();
            try
            {
                oHumanInteractionAgent = oHIA.UpdateRead((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oHumanInteractionAgent = new HumanInteractionAgent();
                oHumanInteractionAgent.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oHumanInteractionAgent);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
