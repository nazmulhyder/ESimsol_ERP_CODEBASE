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
using System.Drawing.Printing;
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class FabricQtyAllowController : Controller
    {
        #region Declaration
        FabricQtyAllow _oFabricQtyAllow = new FabricQtyAllow();
        List<FabricQtyAllow> _oFabricQtyAllows = new List<FabricQtyAllow>();
        string sFeedBackMessage = "";

        #endregion

        #region Functions
        public ActionResult ViewFabricQtyAllows(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oFabricQtyAllows = new List<FabricQtyAllow>();
            _oFabricQtyAllows = FabricQtyAllow.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricQtyAllowType = EnumObject.jGets(typeof(EnumFabricQtyAllowType)).Where(x => x.id != 0);
            return View(_oFabricQtyAllows);
        }
        public ActionResult ViewFabricQtyAllow(int nId)
        {
            _oFabricQtyAllow = new FabricQtyAllow();
            if (nId > 0)
            {
                _oFabricQtyAllow = FabricQtyAllow.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.FabricQtyAllowType = EnumObject.jGets(typeof(EnumFabricQtyAllowType));
            return View(_oFabricQtyAllow);
        }
        //public ActionResult ViewFabricQtyAllows(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);

        //    _oFabricQtyAllows = new List<FabricQtyAllow>();
        //    _oFabricQtyAllows = FabricQtyAllow.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.FabricQtyAllowType = EnumObject.jGets(typeof(EnumFabricQtyAllowType)).Where(x => x.id != 0);
        //    return View(_oFabricQtyAllows);
        //}
        public ActionResult ViewFabricQtyAllowsV2(int menuid)
        {
            _oFabricQtyAllows = new List<FabricQtyAllow>();
            _oFabricQtyAllow = new FabricQtyAllow();
            List<MeasurementUnit> oMeasurementUnits = new List<MeasurementUnit>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);

            string sSQL = "SELECT * FROM View_FabricQtyAllow WHERE FabricQtyAllowID >0 ORDER BY AllowType";
            _oFabricQtyAllows = FabricQtyAllow.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FabricQtyAllowType = EnumObject.jGets(typeof(EnumFabricQtyAllowType)).Where(x => x.id != 0);
            sSQL = "SELECT * FROM MeasurementUnit";
            oMeasurementUnits = MeasurementUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.OrderType = EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.WarpWeftType = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.MeasurementUnits = oMeasurementUnits;
            return View(_oFabricQtyAllows);
        }
        [HttpPost]
        public JsonResult SearchByAllowType(FabricQtyAllow oFabricQtyAllow)
        {
            _oFabricQtyAllows = new List<FabricQtyAllow>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricQtyAllow ";
                string sSReturn = "";
                if (oFabricQtyAllow.AllowTypeInInt > 0)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " AllowType =" + oFabricQtyAllow.AllowTypeInInt;
                }

                sSQL = sSQL+sSReturn;
                _oFabricQtyAllows = FabricQtyAllow.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricQtyAllow = new FabricQtyAllow();
                _oFabricQtyAllow.ErrorMessage = ex.Message;
                _oFabricQtyAllows.Add(_oFabricQtyAllow);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricQtyAllows);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(FabricQtyAllow oFabricQtyAllow)
        {
            oFabricQtyAllow.RemoveNulls();
            _oFabricQtyAllow = new FabricQtyAllow();
            try
            {
                _oFabricQtyAllow = _oFabricQtyAllow.Save(oFabricQtyAllow, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricQtyAllow = new FabricQtyAllow();
                _oFabricQtyAllow.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricQtyAllow);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //private bool IsValid(FabricQtyAllow oFabricQtyAllow)
        //{
        //    _oFabricQtyAllows = FabricQtyAllow.Gets("SELECT * FROM FabricQtyAllow WHERE AllowType = '" + Convert.ToInt32(oFabricQtyAllow.AllowType) + "'  ORDER BY AllowType, Qty_From", ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    if (_oFabricQtyAllows.Count==0)
        //    {
        //        return true;
        //    }
        //    if (oFabricQtyAllow.Qty_From <= oFabricQtyAllow.Qty_To)
        //    {
        //        foreach (FabricQtyAllow oItem in _oFabricQtyAllows)
        //        {
        //            if (oItem.Qty_From > oFabricQtyAllow.Qty_From && oFabricQtyAllow.Qty_To < oItem.Qty_From)
        //            {
        //                return true;
        //            }
        //            if (oItem.Qty_To < oFabricQtyAllow.Qty_To && oFabricQtyAllow.Qty_From > oItem.Qty_To)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        [HttpGet]
        public JsonResult Delete(int id)
        {
            _oFabricQtyAllow = new FabricQtyAllow();
            try
            {
                sFeedBackMessage = _oFabricQtyAllow.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SearchByType(int nType)
        {
            List<FabricQtyAllow> _oFabricQtyAllows = new List<FabricQtyAllow>();
            try
            {
                if(nType==0)
                {
                    _oFabricQtyAllows = FabricQtyAllow.Gets("SELECT * FROM FabricQtyAllow ORDER BY Qty_From", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oFabricQtyAllows = FabricQtyAllow.Gets("SELECT * FROM FabricQtyAllow WHERE AllowType = '" + nType + "'", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricQtyAllows);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}