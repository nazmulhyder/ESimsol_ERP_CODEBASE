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
	public class FPDataController : Controller
	{
		#region Declaration

		FPData _oFPData = new FPData();
		List<FPData> _oFPDatas = new  List<FPData>();
		#endregion

		#region Functions

		#endregion

		#region Actions

        public ActionResult ViewFPData(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
			_oFPData = new FPData();
			_oFPData = _oFPData.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FPDataLogs = FPData.Gets("SELECT * FROM FPDataHistory WHERe YEAR(FPDate) =  Year('" + DateTime.Today.ToString("dd MMM yyyy") + "')", (int)Session[SessionInfo.currentUserID]);
			return View(_oFPData);
		}

		[HttpPost]
		public JsonResult Save(FPData oFPData)
		{
			_oFPData = new FPData();
			try
			{
				_oFPData = oFPData;
				_oFPData = _oFPData.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFPData = new FPData();
				_oFPData.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFPData);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}


        [HttpPost]
        public JsonResult GetsFPDataHistory(FPData oFPData)
        {
            _oFPDatas = new List<FPData>();
            try
            {
                _oFPDatas = FPData.Gets("SELECT * FROM FPDataHistory WHERe YEAR(FPDate) =  Year('" + oFPData.FPDate.ToString("dd MMM yyyy") + "')", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFPData = new FPData();
                _oFPData.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFPDatas);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
		#endregion

	}

}
