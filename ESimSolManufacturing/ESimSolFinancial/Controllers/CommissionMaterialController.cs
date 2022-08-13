using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class CommissionMaterialController : Controller
    {
        #region Declaration
        CommissionMaterial _oCommissionMaterial;
        List<CommissionMaterial> _oCommissionMaterials;
        #endregion

        #region Views
        public ActionResult View_CommissionMaterials(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oCommissionMaterials = new List<CommissionMaterial>();
            string sSql = "SELECT * FROM CommissionMaterial";
            _oCommissionMaterials = CommissionMaterial.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oCommissionMaterials);
        }
        #endregion

        #region IUD
        [HttpPost]
        public JsonResult CommissionMaterialName_IU(CommissionMaterial oCommissionMaterial)
        {
            try
            {
                if (oCommissionMaterial.CMID > 0)
                {
                    oCommissionMaterial = oCommissionMaterial.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oCommissionMaterial = oCommissionMaterial.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oCommissionMaterial = new CommissionMaterial();
                oCommissionMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCommissionMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetCommissionMaterialDescription(CommissionMaterialDescription oCommissionMaterialDescription)
        //{
        //    if (oCommissionMaterialDescription.CRDID > 0)
        //    {
        //        oCommissionMaterialDescription = CommissionMaterialDescription.Get("SELECT * FROM View_CommissionMaterialDescription WHERE CRDID=" + oCommissionMaterialDescription.CRDID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oCommissionMaterialDescription);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult CommissionMaterial_Delete(CommissionMaterial oCommissionMaterial)
        {
            try
            {
                oCommissionMaterial = oCommissionMaterial.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oCommissionMaterial = new CommissionMaterial();
                oCommissionMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCommissionMaterial.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion oCommissionMaterial_IUD

        #region  Activity
        [HttpPost]
        public JsonResult CommissionMaterial_Activity(CommissionMaterial oCommissionMaterial)
        {
            _oCommissionMaterial = new CommissionMaterial();
            try
            {
                _oCommissionMaterial = oCommissionMaterial;
                _oCommissionMaterial = CommissionMaterial.Activite(_oCommissionMaterial.CMID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oCommissionMaterial = new CommissionMaterial();
                _oCommissionMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCommissionMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //[HttpPost]
        //public JsonResult GetsCommissionMaterialName(CommissionMaterialName oCommissionMaterialName)
        //{
        //    List<CommissionMaterialName> oCommissionMaterialNames = new List<CommissionMaterialName>();
        //    oCommissionMaterialNames = CommissionMaterialName.Gets("SELECT * FROM CommissionMaterialName WHERE IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oCommissionMaterialNames);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
    }
}
