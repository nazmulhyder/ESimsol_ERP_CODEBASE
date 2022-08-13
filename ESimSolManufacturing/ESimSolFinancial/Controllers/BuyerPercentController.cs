using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ESimSolFinancial.Controllers
{
    public class BuyerPercentController : Controller
    {
        #region Declaration
        BuyerPercent _oBuyerPercent = new BuyerPercent();
        List<BuyerPercent> _oBuyerPercents = new List<BuyerPercent>();
        #endregion
        [HttpGet]
        public ActionResult ViewBuyerPercent(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string ssSQL = "SELECT * FROM View_BuyerPercent  AS HH ORDER BY HH.BuyerPercentID ASC";
            _oBuyerPercents = BuyerPercent.Gets(ssSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.QCGradeType = EnumObject.jGets(typeof(EnumFBQCGrade));
            return View(_oBuyerPercents);
        }

        [HttpPost]
        public JsonResult Save(BuyerPercent oBuyerPercent)
        {
            _oBuyerPercent = new BuyerPercent();
            try
            {
                _oBuyerPercent = oBuyerPercent;
                _oBuyerPercent = _oBuyerPercent.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBuyerPercent = new BuyerPercent();
                _oBuyerPercent.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBuyerPercent);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                BuyerPercent oBuyerPercent = new BuyerPercent();
                sFeedBackMessage = oBuyerPercent.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsBuyerPosition(BuyerPercent oBuyerPercent)
        {
            List<BuyerPercent> oBuyerPercents = new List<BuyerPercent>();
            try
            {
                string sSQL = "";
                if (!String.IsNullOrEmpty(oBuyerPercent.BPosition))
                {
                    sSQL = "SELECT * FROM View_BuyerPercent WHERE BPosition Like '%"+oBuyerPercent.BPosition+"%'";
                }
                else{
                    sSQL = "SELECT * FROM View_BuyerPercent WHERE BuyerPercentID>0 Order By BuyerPercentID ASC";
                }

                oBuyerPercents = BuyerPercent.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oBuyerPercents = new List<BuyerPercent>();
                oBuyerPercents.Add(new BuyerPercent() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBuyerPercents);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
       
}