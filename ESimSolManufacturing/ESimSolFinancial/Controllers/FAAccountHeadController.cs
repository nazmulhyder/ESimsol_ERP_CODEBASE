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
	public class FAAccountHeadController : Controller
	{
		#region Declaration

		FAAccountHead _oFAAccountHead = new FAAccountHead();
		List<FAAccountHead> _oFAAccountHeads = new  List<FAAccountHead>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewFAAccountHeads(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			_oFAAccountHeads = new List<FAAccountHead>(); 
			_oFAAccountHeads = FAAccountHead.Gets((int)Session[SessionInfo.currentUserID]);

            ViewBag.FAAcountHeadTypes = EnumObject.jGets(typeof(EnumFAAccountHeadType));
			return View(_oFAAccountHeads);
		}

		[HttpPost]
		public JsonResult Save(FAAccountHead oFAAccountHead)
		{
			_oFAAccountHead = new FAAccountHead();
			try
			{
				_oFAAccountHead = oFAAccountHead;
				_oFAAccountHead = _oFAAccountHead.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFAAccountHead = new FAAccountHead();
				_oFAAccountHead.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFAAccountHead);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Delete(FAAccountHead oFAAccountHead)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oFAAccountHead.Delete(oFAAccountHead.FAAccountHeadID, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 

		#endregion

	}

}
