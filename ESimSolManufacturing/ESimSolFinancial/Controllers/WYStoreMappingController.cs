using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class WYStoreMappingController : Controller
    {
        #region Declaration
        WYStoreMapping _oWYStoreMapping = new WYStoreMapping();
        List<WYStoreMapping> _oWYStoreMappings = new List<WYStoreMapping>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
       
        #endregion
        public ActionResult ViewWYStoreMappings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oWYStoreMappings = new List<WYStoreMapping>();
            _oWYStoreMappings = WYStoreMapping.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.BUID = buid;
            return View(_oWYStoreMappings);
        }

        public ActionResult ViewWYStoreMapping(int id, int buid)
        {
            _oWYStoreMapping = new WYStoreMapping();

            if (id > 0)
                _oWYStoreMapping = _oWYStoreMapping.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            _oWYStoreMapping.BUID = buid;
            BusinessUnit oDyeingBusinessUnit = new BusinessUnit();
            oDyeingBusinessUnit = oDyeingBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, (int)Session[SessionInfo.currentUserID]);

            ViewBag.IssueStores = WorkingUnit.GetsPermittedStore(oDyeingBusinessUnit.BusinessUnitID, EnumModuleName.WYRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ReceiveStores = WorkingUnit.GetsPermittedStore(buid,  EnumModuleName.WYRequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);

            ViewBag.WYarnTypes = EnumObject.jGets(typeof(EnumWYarnType)).OrderBy(x => x.id).ToList();
            ViewBag.WYStoreTypes = EnumObject.jGets(typeof(EnumStoreType)).OrderBy(x => x.id).ToList(); 

            return View(_oWYStoreMapping);
        }

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(WYStoreMapping oWYStoreMapping)
        {
            _oWYStoreMapping = new WYStoreMapping();
            try
            {
                _oWYStoreMapping = oWYStoreMapping;
                _oWYStoreMapping = _oWYStoreMapping.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oWYStoreMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWYStoreMapping);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ToggleActivity(WYStoreMapping oWYStoreMapping)
        {
            _oWYStoreMapping = new WYStoreMapping();
            try
            {
                _oWYStoreMapping = oWYStoreMapping;
                _oWYStoreMapping = _oWYStoreMapping.ToggleActivity(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oWYStoreMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWYStoreMapping);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string smessage = "";
            try
            {
                WYStoreMapping oWYStoreMapping = new WYStoreMapping();
                smessage = oWYStoreMapping.Delete(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

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
    }
}
