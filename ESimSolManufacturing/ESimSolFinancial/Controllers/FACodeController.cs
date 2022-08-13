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
	public class FACodeController : Controller
	{
		#region Declaration

		FACode _oFACode = new FACode();
		List<FACode> _oFACodes = new  List<FACode>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewFACodes(int menuid, int buid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			_oFACodes = new List<FACode>(); 
			_oFACodes = FACode.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oFACodes);
		}
		public ActionResult ViewFACode(int id, int buid)
		{
			_oFACode = new FACode();
            Product oProduct = new Product();
			if (id > 0)
			{
                oProduct = oProduct.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFACode = new FACode()
                {
                    ProductID = oProduct.ProductID,
                    ProductName = oProduct.ProductName,
                    ProductCode = oProduct.ProductCode
                };
                _oFACode.FACodes = FACode.GetsByProduct(id, (int)Session[SessionInfo.currentUserID]);
			}
            ViewBag.FACodingPartType = EnumObject.jGets(typeof(EnumFACodingPartType)).Where(x=>x.id!=0);
			return View(_oFACode);
		}

		[HttpPost]
		public JsonResult Save(FACode oFACode)
		{
			_oFACode = new FACode();
			try
			{
				_oFACode = oFACode;
				_oFACode = _oFACode.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFACode = new FACode();
				_oFACode.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFACode);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult Update(List<FACode> oFACodes)
        {
            _oFACode = new FACode();
            try
            {
                _oFACode.FACodes = oFACodes;
                _oFACode = _oFACode.Update((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFACode = new FACode();
                _oFACode.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFACode);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
		[HttpPost]
        public JsonResult Delete(FACode oFACode)
		{
			string sFeedBackMessage = "";
			try
			{
				sFeedBackMessage = oFACode.Delete(oFACode.FACodeID, (int)Session[SessionInfo.currentUserID]);
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
