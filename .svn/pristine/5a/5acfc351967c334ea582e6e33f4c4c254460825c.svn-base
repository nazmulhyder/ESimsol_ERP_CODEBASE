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
	public class FNQCParameterController : Controller
	{
		#region Declaration
		FNQCParameter _oFNQCParameter = new FNQCParameter();
		List<FNQCParameter> _oFNQCParameters = new  List<FNQCParameter>();
		#endregion
		#region Functions

		#endregion
		#region Actions
		public ActionResult ViewFNQCParameters(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			_oFNQCParameters = FNQCParameter.Gets((int)Session[SessionInfo.currentUserID]);

            //ViewBag.LastCode = _oFNQCParameters.Max(x => x.Code);
            ViewBag.FNQCParameter = new FNQCParameter();
            ViewBag.FnQCTestGroups = FnQCTestGroup.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oFNQCParameters);
		}

		[HttpPost]
		public JsonResult Save(FNQCParameter oFNQCParameter)
		{
			_oFNQCParameter = new FNQCParameter();
			try
			{
				_oFNQCParameter = oFNQCParameter;
				_oFNQCParameter = _oFNQCParameter.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFNQCParameter = new FNQCParameter();
				_oFNQCParameter.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFNQCParameter);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 

		[HttpPost]
        public JsonResult Delete(FNQCParameter oFNQCParameter)
		{
			string sFeedBackMessage = "";
			try
			{
                if (oFNQCParameter.FNQCParameterID>0)
                {
                    sFeedBackMessage = oFNQCParameter.Delete(oFNQCParameter.FNQCParameterID, (int)Session[SessionInfo.currentUserID]);
                }
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
        public JsonResult Search(FNQCParameter oFNQCParameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(oFNQCParameter.Name))
                {
                    _oFNQCParameters = FNQCParameter.Gets("SELECT * FROM View_FNQCParameter WHERE Name LIKE '%" + oFNQCParameter.Name + "%' ORDER BY Name", (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oFNQCParameters = FNQCParameter.Gets("SELECT * FROM View_FNQCParameter ORDER BY Name", (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oFNQCParameter = new FNQCParameter();
                _oFNQCParameter.ErrorMessage = ex.Message;
                _oFNQCParameters.Add(_oFNQCParameter);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNQCParameters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

       
	}

}
