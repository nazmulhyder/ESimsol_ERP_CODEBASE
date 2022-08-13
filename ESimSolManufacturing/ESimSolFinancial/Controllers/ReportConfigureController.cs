using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class ReportConfigureController : Controller
    {
        #region Declaration
        ReportConfigure _oReportConfigure = new ReportConfigure();
        List<ReportConfigure> _oReportConfigures = new List<ReportConfigure>();
        string _sErrorMessage = "";
        #endregion

          
        [HttpPost]
        public JsonResult Save(ReportConfigure oReportConfigure)
        {
            _oReportConfigure = new ReportConfigure();
            try
            {
                _oReportConfigure = oReportConfigure;
                _oReportConfigure.ReportType = (EnumReportType)oReportConfigure.ReportTypeInInt;
                _oReportConfigure.UserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
                _oReportConfigure = _oReportConfigure.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oReportConfigure = new ReportConfigure();
                _oReportConfigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReportConfigure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //GetReportConfigure

        [HttpPost]
        public JsonResult GetReportConfigure(ReportConfigure oReportConfigure)
        {
            _oReportConfigure = new ReportConfigure();
            try
            {
                #region common Get
                _oReportConfigure = _oReportConfigure.Get(oReportConfigure.ReportTypeInInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #endregion
            }
            catch (Exception ex)
            {
                _oReportConfigure = new ReportConfigure();
                _oReportConfigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReportConfigure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ReportConfigure oReportConfigure = new ReportConfigure();
                sFeedBackMessage = oReportConfigure.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ViewReportConfigure(ReportConfigure oReportConfigure)
        //{
        //    _oReportConfigure = new ReportConfigure();
        //    try
        //    {
        //        #region common Get
        //        _oReportConfigure = _oReportConfigure.Get(oReportConfigure.ReportTypeInInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        _oReportConfigure = new ReportConfigure();
        //        _oReportConfigure.ErrorMessage = ex.Message;
        //    }
        //    return View(_oReportConfigure);
        //}
        public ActionResult ViewReportConfigure(int reportconfiguretype)
        {
            List<ReportConfigure> oReportConfigures = new List<ReportConfigure>();
            oReportConfigures = ReportConfigure.Gets((EnumReportType)reportconfiguretype);
            ReportConfigure oReportConfigure = new ReportConfigure();
            oReportConfigure = oReportConfigure.Get(reportconfiguretype, (int)Session[SessionInfo.currentUserID]);
            string[] sSelectedFields = new string[oReportConfigure.FieldNames.Length];
            if(oReportConfigure.FieldNames.Length > 0) sSelectedFields = oReportConfigure.FieldNames.Split(',');

            List<ReportConfigure> oFinalReportConfigures = new List<ReportConfigure>();
            ReportConfigure oRC = new ReportConfigure();

            foreach (string str in sSelectedFields)
            {
                oRC = new ReportConfigure();
                oRC.FieldNames = str;
                oRC.CaptionNames = oReportConfigures.Where(x => x.FieldNames == str).Select(y => y.CaptionNames).FirstOrDefault();
                oRC.Selected = true;
                oFinalReportConfigures.Add(oRC);
            }
            foreach (ReportConfigure oItem in oReportConfigures)
            {
                bool IsExist = false;
                foreach (ReportConfigure oFRC in oFinalReportConfigures)
                {
                    if (oItem.FieldNames == oFRC.FieldNames)
                    {
                        IsExist = true;
                        break;
                    }
                }
                if (IsExist == false)
                {
                    oItem.Selected = false;
                    oFinalReportConfigures.Add(oItem);
                }
            }

            oReportConfigure.ReportConfigures = oFinalReportConfigures;
            oReportConfigure.ReportTypeInInt = reportconfiguretype;
            return View(oReportConfigure);
        }
  
  
    }
}
