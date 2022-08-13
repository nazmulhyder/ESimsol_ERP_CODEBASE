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
	public class FNQCResultSetupController : Controller
	{
		#region Declaration
		FNQCResultSetup _oFNQCResultSetup = new FNQCResultSetup();
		List<FNQCResultSetup> _oFNQCResultSetups = new  List<FNQCResultSetup>();
		#endregion
		#region Functions

		#endregion
		#region Actions
		public ActionResult ViewFNQCResultSetups(int nFNTPID)
		{
            FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
            oFNTreatmentProcess = oFNTreatmentProcess.Get(nFNTPID, (int)Session[SessionInfo.currentUserID]);

            _oFNQCResultSetups = FNQCResultSetup.Gets("SELECT * FROM View_FNQCResultSetup WHERE FNTPID = " + nFNTPID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FNQCResultSetups = _oFNQCResultSetups;
            return View(oFNTreatmentProcess);
		}

		[HttpPost]
		public JsonResult SaveAll(List<FNQCResultSetup> oFNQCResultSetups)
		{
			try
			{
                _oFNQCResultSetups = _oFNQCResultSetup.SaveAll(oFNQCResultSetups, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFNQCResultSetup = new FNQCResultSetup();
				_oFNQCResultSetup.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNQCResultSetups);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult Save(FNQCResultSetup oFNQCResultSetup)
        {
            try
            {
                _oFNQCResultSetup = _oFNQCResultSetup.Save(oFNQCResultSetup, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNQCResultSetup = new FNQCResultSetup();
                _oFNQCResultSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNQCResultSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

		[HttpPost]
        public JsonResult Delete(FNQCResultSetup oFNQCResultSetup)
		{
			string sFeedBackMessage = "";
			try
			{
                if (oFNQCResultSetup.FNQCResultSetupID>0)
                {
                    sFeedBackMessage = oFNQCResultSetup.Delete(oFNQCResultSetup.FNQCResultSetupID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Search(FNQCResultSetup oFNQCResultSetup)
        {
            try
            {
                if (!string.IsNullOrEmpty(oFNQCResultSetup.Name))
                {
                    _oFNQCResultSetups = FNQCResultSetup.Gets("SELECT * FROM View_FNQCResultSetup WHERE Name LIKE '%" + oFNQCResultSetup.Name + "%' AND FNTPID = " + oFNQCResultSetup.FNTPID + " ORDER BY Name", (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oFNQCResultSetups = FNQCResultSetup.Gets("SELECT * FROM View_FNQCResultSetup WHERE FNTPID = " + oFNQCResultSetup.FNTPID + " ORDER BY Name", (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oFNQCResultSetup = new FNQCResultSetup();
                _oFNQCResultSetup.ErrorMessage = ex.Message;
                _oFNQCResultSetups.Add(_oFNQCResultSetup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNQCResultSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

       
	}

}
