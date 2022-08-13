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
    public class GratuityController : Controller
    {
        #region Declaration
        GratuityScheme _oGratuityScheme;
        List<GratuityScheme> _oGratuitySchemes;

        #endregion

        public ActionResult View_GratuitySchemes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oGratuitySchemes = new List<GratuityScheme>();
            _oGratuitySchemes = GratuityScheme.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oGratuitySchemes);
        }

        public ActionResult View_GratuityScheme(string sid, string sMsg)
        {
            int nGSID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oGratuityScheme = new GratuityScheme();
            if (nGSID > 0)
            {
                _oGratuityScheme = GratuityScheme.Get(nGSID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                string sSql = "SELECT * FROM GratuitySchemeDetail WHERE GSID=" + nGSID;
                _oGratuityScheme.GratuitySchemeDetails = GratuitySchemeDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            if (sMsg != "N/A")
            {
                _oGratuityScheme.ErrorMessage = sMsg;
            }

            ViewBag.ActivationAfter = Enum.GetValues(typeof(EnumRecruitmentEvent)).Cast<EnumRecruitmentEvent>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x=>x.Value!=0.ToString()).ToList();
            ViewBag.PayrollApplyOn = Enum.GetValues(typeof(EnumPayrollApplyOn)).Cast<EnumPayrollApplyOn>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
            
            return View(_oGratuityScheme);
        }

        #region GratuityScheme_IUD
        [HttpPost]
        public JsonResult GratuityScheme_IU(GratuityScheme oGratuityScheme)
        {
            _oGratuityScheme = new GratuityScheme();
            try
            {
                _oGratuityScheme = oGratuityScheme;
                if (_oGratuityScheme.GSID > 0)
                {
                    _oGratuityScheme = _oGratuityScheme.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oGratuityScheme = _oGratuityScheme.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oGratuityScheme = new GratuityScheme();
                _oGratuityScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGratuityScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GratuityScheme_Delete(GratuityScheme oGratuityScheme)
        {
            string sErrorMease = "";
            try
            {
                oGratuityScheme = oGratuityScheme.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sErrorMease = oGratuityScheme.ErrorMessage;
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GratuitySchemeDetail_IU(GratuitySchemeDetail oGratuitySchemeDetail)
        {
            _oGratuityScheme = new GratuityScheme();
            try
            {
                _oGratuityScheme = oGratuitySchemeDetail.GratuityScheme;
                if (_oGratuityScheme.GSID <= 0)
                {
                    _oGratuityScheme = _oGratuityScheme.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }

                if (_oGratuityScheme.GSID > 0 && string.IsNullOrEmpty(_oGratuityScheme.ErrorMessage))
                {
                    oGratuitySchemeDetail.GSID = _oGratuityScheme.GSID;
                    if (oGratuitySchemeDetail.GSDID > 0)
                    {
                        oGratuitySchemeDetail = oGratuitySchemeDetail.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    else
                    {
                        oGratuitySchemeDetail = oGratuitySchemeDetail.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    oGratuitySchemeDetail.GratuityScheme = _oGratuityScheme;
                }
                else
                {
                    oGratuitySchemeDetail.ErrorMessage = _oGratuityScheme.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                oGratuitySchemeDetail = new GratuitySchemeDetail();
                oGratuitySchemeDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGratuitySchemeDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GratuitySchemeDetail_Delete(GratuitySchemeDetail oGratuitySchemeDetail)
        {
            string sErrorMease = "";
            try
            {
                oGratuitySchemeDetail = oGratuitySchemeDetail.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sErrorMease = oGratuitySchemeDetail.ErrorMessage;
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion GratuityScheme_IUD

        #region Gratuity Scheme Activity
        [HttpPost]
        public JsonResult GratuityScheme_Activity(GratuityScheme oGratuityScheme)
        {
            _oGratuityScheme = new GratuityScheme();
            try
            {
                _oGratuityScheme = oGratuityScheme;
                _oGratuityScheme = _oGratuityScheme.Activity(((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oGratuityScheme = new GratuityScheme();
                _oGratuityScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGratuityScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gratuity Scheme Approve
        [HttpPost]
        public JsonResult GratuityScheme_Approve(GratuityScheme oGratuityScheme)
        {
            _oGratuityScheme = new GratuityScheme();
            try
            {
                _oGratuityScheme = oGratuityScheme;
                _oGratuityScheme = _oGratuityScheme.Approve(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oGratuityScheme = new GratuityScheme();
                _oGratuityScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGratuityScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //[HttpPost]
        //public JsonResult GetGratuityScheme(GratuityScheme oGratuityScheme)
        //{
        //    try
        //    {
        //        oGratuityScheme = GratuityScheme.Get(oGratuityScheme.GratuitySchemeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        oGratuityScheme = new GratuityScheme();
        //        oGratuityScheme.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oGratuityScheme);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

    }
}
