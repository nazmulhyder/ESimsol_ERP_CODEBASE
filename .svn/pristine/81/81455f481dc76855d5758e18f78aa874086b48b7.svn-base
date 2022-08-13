using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class CharitySlabController : Controller
    {
        public ActionResult ViewCharitySlabs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<CharitySlab> _oCharitySlabs = new List<CharitySlab>();
            string sSQL = "SELECT * FROM View_CharitySlab  AS HH ORDER BY HH.SalaryHeadID,HH.StartSalaryRange ASC";
            _oCharitySlabs = CharitySlab.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
             sSQL = "SELECT * FROM SalaryHead AS SH WHERE SH.SalaryHeadType=" + Convert.ToInt16(EnumSalaryHeadType.Deduction);
             ViewBag.SalaryHeads = SalaryHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oCharitySlabs);

        }
      
        [HttpPost]
        public JsonResult Save(CharitySlab oCharitySlab)
        {
            CharitySlab _oCharitySlab = new CharitySlab();
            try
            {

                _oCharitySlab = oCharitySlab.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCharitySlab = new CharitySlab();
                _oCharitySlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCharitySlab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public JsonResult Delete(CharitySlab oCharitySlab)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oCharitySlab.Delete(oCharitySlab.CharitySlabID, (int)Session[SessionInfo.currentUserID]);
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