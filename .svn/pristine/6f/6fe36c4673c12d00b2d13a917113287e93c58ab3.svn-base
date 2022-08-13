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
	public class TermsAndConditionController : Controller
	{
		#region Declaration

		TermsAndCondition _oTermsAndCondition = new TermsAndCondition();
		List<TermsAndCondition> _oTermsAndConditions = new  List<TermsAndCondition>();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewTermsAndConditions(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			
			_oTermsAndConditions = new List<TermsAndCondition>(); 
			_oTermsAndConditions = TermsAndCondition.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oTermsAndConditions);
		}

		public ActionResult ViewTermsAndCondition(int id)
		{
			_oTermsAndCondition = new TermsAndCondition();
			if (id > 0)
			{
				_oTermsAndCondition = _oTermsAndCondition.Get(id, (int)Session[SessionInfo.currentUserID]);
			}
            ViewBag.EnumModules = EnumObject.jGets(typeof(EnumModuleName)).OrderBy(x=>x.Value);
			return View(_oTermsAndCondition);
		}

		[HttpPost]
		public JsonResult Save(TermsAndCondition oTermsAndCondition)
		{
			_oTermsAndCondition = new TermsAndCondition();
			try
			{
				_oTermsAndCondition = oTermsAndCondition;
				_oTermsAndCondition = _oTermsAndCondition.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oTermsAndCondition = new TermsAndCondition();
				_oTermsAndCondition.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oTermsAndCondition);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 

		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				TermsAndCondition oTermsAndCondition = new TermsAndCondition();
				sFeedBackMessage = oTermsAndCondition.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsByModule(TermsAndCondition oTermsAndCondition)
        {
            _oTermsAndConditions = new List<TermsAndCondition>();
            try
            {
                _oTermsAndConditions =TermsAndCondition.GetsByModule(oTermsAndCondition.ModuleID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTermsAndCondition = new TermsAndCondition();
                _oTermsAndCondition.ErrorMessage = ex.Message;
                _oTermsAndConditions.Add(_oTermsAndCondition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTermsAndConditions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

		#endregion

	}

}
