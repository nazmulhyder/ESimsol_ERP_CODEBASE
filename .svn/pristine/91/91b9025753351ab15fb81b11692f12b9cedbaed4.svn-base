using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class EmployeeTypeController : Controller
    {
        #region Declaration

        private EmployeeType _oEmployeeType = new EmployeeType();
        private List<EmployeeType> _oEmployeeTypes = new List<EmployeeType>();
        private string _sErrorMessage = "";

        #endregion

        public ActionResult ViewEmployeeTypes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployeeTypes = new List<EmployeeType>();
            _oEmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            
            return View(_oEmployeeTypes);
        }

        public ActionResult ViewEmployeeType(string sid, string sMsg)
        {
            int nEmpTypeID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");  
            _oEmployeeType = new EmployeeType();
            if (nEmpTypeID > 0)
            {
                _oEmployeeType = _oEmployeeType.Get(nEmpTypeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            if (sMsg != "N/A")
            {
                _oEmployeeType.ErrorMessage = sMsg;
            }
            ViewBag.EmpGroupings = Enum.GetValues(typeof(EnumEmployeeGrouping)).Cast<EnumEmployeeGrouping>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oEmployeeType);
        }

        [HttpPost]
        public JsonResult Save(EmployeeType oEmployeeType)
        {
            _oEmployeeType = new EmployeeType();
            try
            {
                _oEmployeeType = oEmployeeType;
                _oEmployeeType = _oEmployeeType.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oEmployeeType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                EmployeeType oEmployeeType = new EmployeeType();
                sFeedBackMessage = oEmployeeType.Delete(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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
        public JsonResult ChangeActiveStatus(EmployeeType oEmployeeType)
        {
            _oEmployeeType = new EmployeeType();
            string sMsg;

            sMsg = _oEmployeeType.ChangeActiveStatus(oEmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeType = _oEmployeeType.Get(oEmployeeType.EmployeeTypeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsEmployeeType(EmployeeType oEmployeeType)
        {
            List<EmployeeType> oEmployeeTypes = new List<EmployeeType>();
            oEmployeeTypes = EmployeeType.Gets((int)(Session[SessionInfo.currentUserID]));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}