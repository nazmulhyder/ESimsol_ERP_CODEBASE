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

namespace ESimSolFinancial.Controllers
{
	public class SpecHeadController : Controller
	{
		#region Declaration

		SpecHead _oSpecHead = new SpecHead();
		List<SpecHead> _oSpecHeads = new  List<SpecHead>();
		#endregion

		#region Functions

		#endregion

		#region Actions
        public ActionResult View_SpecHeads(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<SpecHead> oSpecHeads = new List<SpecHead>();
            string sSQL = "Select * from SpecHead";
            oSpecHeads = SpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oSpecHeads);

        }

        [HttpPost]
        public JsonResult Save(SpecHead oSpecHead)
        {
            try
            {
                if (oSpecHead.SpecHeadID <= 0)
                {
                    oSpecHead = oSpecHead.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oSpecHead = oSpecHead.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oSpecHead = new SpecHead();
                oSpecHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSpecHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(SpecHead oSpecHead)
        {
            try
            {
                if (oSpecHead.SpecHeadID <= 0) { throw new Exception("Please select an valid item."); }
                oSpecHead = oSpecHead.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSpecHead = new SpecHead();
                oSpecHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSpecHead.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(SpecHead oSpecHead)
        {
            try
            {
                if (oSpecHead.SpecHeadID <= 0)
                    throw new Exception("Select a valid item from list");

                oSpecHead = oSpecHead.Get(oSpecHead.SpecHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSpecHead = new SpecHead();
                oSpecHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSpecHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


		#endregion

	}

}
