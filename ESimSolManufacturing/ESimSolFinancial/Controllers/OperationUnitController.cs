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
using System.Web;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class OperationUnitController : Controller
    {
        #region Declaration
        OperationUnit _oOperationUnit = new OperationUnit();
        List<OperationUnit> _oOperationUnits = new List<OperationUnit>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private bool ValidateInput(OperationUnit oOperationUnit)
        {
            if (oOperationUnit.OperationUnitName == null || oOperationUnit.OperationUnitName == "")
            {
                _sErrorMessage = "Please enter Operation Unit Name";
                return false;
            }
            return true;
        }
        #endregion

        public ActionResult RefreshList(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oOperationUnits = new List<OperationUnit>();
            _oOperationUnits = OperationUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oOperationUnits);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult ViewOperationUnit(int id)
        {
            if (id > 0)
            {
                _oOperationUnit = _oOperationUnit.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oOperationUnit = new OperationUnit();
            }
            return View(_oOperationUnit);
        }

        [HttpPost]
        public JsonResult Save(OperationUnit oOperationUnit)
        {
            _oOperationUnit = new OperationUnit();
            _oOperationUnit = oOperationUnit;
            try
            {
                _oOperationUnit = _oOperationUnit.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oOperationUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOperationUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                _oOperationUnit = new OperationUnit();
                sErrorMease = _oOperationUnit.Delete(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       
       
    }
}
