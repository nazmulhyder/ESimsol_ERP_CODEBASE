using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
 
namespace ESimSolFinancial.Controllers
{
    public class SampleTypeController : Controller
    {

        #region Declaration
        SampleType _oSampleType = new SampleType();
        List<SampleType> _oSampleTypes = new List<SampleType>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewSampleTypes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(EnumModuleName.SampleType.ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oSampleTypes = new List<SampleType>();
            _oSampleTypes = SampleType.Gets((int)Session[SessionInfo.currentUserID]);            
            return View(_oSampleTypes);
        }

        public ActionResult ViewSampleType(int id, double ts)
        {
            _oSampleType = new SampleType();
            if (id > 0)
            {
                _oSampleType = _oSampleType.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return PartialView(_oSampleType);
        }

        [HttpPost]
        public JsonResult Save(SampleType oSampleType)
        {
            _oSampleType = new SampleType();
            try
            {
                _oSampleType = oSampleType.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleType = new SampleType();
                _oSampleType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                SampleType oSampleType = new SampleType();
                sFeedBackMessage = oSampleType.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}
