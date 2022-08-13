using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;



namespace ESimSolFinancial.Controllers
{
    public class DURequisitionSetupController : PdfViewController
    {
        #region Declaration
        DURequisitionSetup _oDURequisitionSetup = new DURequisitionSetup();
        List<DURequisitionSetup> _oDURequisitionSetups = new List<DURequisitionSetup>();
        #endregion

        public ActionResult ViewDURequisitionSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<DURequisitionSetup> oDURequisitionSetups = new List<DURequisitionSetup>();
            oDURequisitionSetups = DURequisitionSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oDURequisitionSetups);
        }
        public ActionResult ViewDURequisitionSetup(int nId, int buid, double ts)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
           
            DURequisitionSetup oDURequisitionSetup = new DURequisitionSetup();
            if (nId > 0)
            {
                oDURequisitionSetup = oDURequisitionSetup.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            #region Stores
            oIssueStores = new List<WorkingUnit>();
            oIssueStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DURequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
         
            oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DURequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
          
            ViewBag.IssueStores = oIssueStores;
            ViewBag.ReceivedStores = oReceivedStores;
            #endregion

            ViewBag.InOutTypes = EnumObject.jGets(typeof(EnumInOutType));
            return View(oDURequisitionSetup);
        }

        [HttpPost]
        public JsonResult Save(DURequisitionSetup oDURequisitionSetup)
        {
            oDURequisitionSetup.RemoveNulls();
            _oDURequisitionSetup = new DURequisitionSetup();
            try
            {
                _oDURequisitionSetup = oDURequisitionSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDURequisitionSetup = new DURequisitionSetup();
                _oDURequisitionSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDURequisitionSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(DURequisitionSetup oDURequisitionSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDURequisitionSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ActivateDURequisitionSetup(DURequisitionSetup oDURequisitionSetup)
        {
            _oDURequisitionSetup = new DURequisitionSetup();
            _oDURequisitionSetup = oDURequisitionSetup.Activate(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDURequisitionSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<DURequisitionSetup> oDURequisitionSetups = new List<DURequisitionSetup>();
            oDURequisitionSetups = DURequisitionSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDURequisitionSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
