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
using System.Text;

namespace ESimSolFinancial.Controllers
{
	public class KnitDyeingPTUController : Controller
	{
		#region Declaration

		KnitDyeingPTU _oKnitDyeingPTU = new KnitDyeingPTU();
		List<KnitDyeingPTU> _oKnitDyeingPTUs = new  List<KnitDyeingPTU>();
        List<KnitDyeingPTULog> _oKnitDyeingPTULogs = new List<KnitDyeingPTULog>();
		#endregion

		#region Functions

		#endregion

		#region Actions

        public ActionResult ViewKnitDyeingPTUs(int nId, int menuid)
        {
            _oKnitDyeingPTUs = new List<KnitDyeingPTU>();
            KnitDyeingProgram oKnitDyeingProgram = new KnitDyeingProgram();
            
            List<Product> oProducts = new List<Product>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            if (nId > 0)
            {
                StringBuilder sSQL = new StringBuilder("SELECT HH.KnitDyeingProgramID, HH.IssueDate,HH.MerchandiserName, HH.ShipmentDate, HH.MUSymbol, HH.FabricName,HH.ApprovedByName , HH.SessionName,HH.TechnicalSheetID,  HH.RefNo , HH.StyleNo, HH.BuyerName, HH.RecapOrPAMNos, (SELECT SUM(MM.GarmentsQty) FROM (SELECT DISTINCT TT.RefObjectNo, TT.ColorID, TT.GarmentsQty FROM View_KnitDyeingProgramDetail AS TT WHERE TT.KnitDyeingProgramID = HH.KnitDyeingProgramID ) AS MM) AS OrderQty  FROM View_KnitDyeingProgram AS HH WHERE HH.KnitDyeingProgramID = " + nId);
                oKnitDyeingProgram = KnitDyeingProgram.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID])[0];

                 sSQL = new StringBuilder("SELECT * FROM View_KnitDyeingPTU AS HH WHERE HH.KnitDyeingProgramID = " + nId.ToString() + " ORDER BY HH.ColorID, HH.ReFinishingQty ASC ");
                 _oKnitDyeingPTUs = KnitDyeingPTU.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);

                 sSQL = new StringBuilder("SELECT * FROM View_Product AS HH WHERE HH.ProductID IN (SELECT KDYR.YarnCountID FROM KnitDyeingYarnRequisition AS KDYR WHERE KDYR.KnitDyeingProgramID =" + nId.ToString() + ") ORDER BY HH.ProductID ASC");
                oProducts = Product.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
            }

            ViewBag.menuid = menuid;
            ViewBag.Products = oProducts;
            ViewBag.KnitDyeingProgram = oKnitDyeingProgram;
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.KnitDyeingPTU, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            return View(_oKnitDyeingPTUs);
        }

		public ActionResult ViewKnitDyeingPTU(int id)
		{
			_oKnitDyeingPTU = new KnitDyeingPTU();
			if (id > 0)
			{
				_oKnitDyeingPTU = _oKnitDyeingPTU.Get(id, (int)Session[SessionInfo.currentUserID]);
			}
			return View(_oKnitDyeingPTU);
		}
        
        [HttpPost]
        public JsonResult GetKnitDyeingPTUs(KnitDyeingPTU oKnitDyeingPTU)
        {
            _oKnitDyeingPTU = new KnitDyeingPTU();
            _oKnitDyeingPTUs = new List<KnitDyeingPTU>();
            try
            {
                _oKnitDyeingPTUs = KnitDyeingPTU.Gets("SELECT * FROM View_KnitDyeingPTU WHERE KnitDyeingProgramID = " + oKnitDyeingPTU.KnitDyeingProgramID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingPTU = new KnitDyeingPTU();
                _oKnitDyeingPTU.ErrorMessage = ex.Message;
                _oKnitDyeingPTUs.Add(_oKnitDyeingPTU);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingPTUs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetKnitDyeingYarnBooking(int id)
        {
            KnitDyeingYarnBooking _oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
           List<KnitDyeingYarnBooking> _oKnitDyeingYarnBookings = new List<KnitDyeingYarnBooking>();
            try
            {
                _oKnitDyeingYarnBookings = KnitDyeingYarnBooking.Gets("SELECT * FROM View_KnitDyeingYarnBooking WHERE KnitDyeingPTUID = " + id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
                _oKnitDyeingYarnBooking.ErrorMessage = ex.Message;
                _oKnitDyeingYarnBookings.Add(_oKnitDyeingYarnBooking);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingYarnBookings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByColorAndOrderRecap(KnitDyeingPTU oKnitDyeingPTU)
        {
            _oKnitDyeingPTU = new KnitDyeingPTU();
            _oKnitDyeingPTUs = new List<KnitDyeingPTU>();
            try
            {
                _oKnitDyeingPTUs = KnitDyeingPTU.Gets("SELECT * FROM View_KnitDyeingPTU WHERE KnitDyeingProgramID = " + oKnitDyeingPTU.KnitDyeingProgramID + " AND ColorID = " + oKnitDyeingPTU.ColorID + " AND RefObjectNo = '" + oKnitDyeingPTU.RefObjectNo+"'", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingPTU = new KnitDyeingPTU();
                _oKnitDyeingPTU.ErrorMessage = ex.Message;
                _oKnitDyeingPTUs.Add(_oKnitDyeingPTU);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingPTUs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		[HttpPost]
		public JsonResult Save(KnitDyeingPTU oKnitDyeingPTU)
		{
			_oKnitDyeingPTU = new KnitDyeingPTU();
			try
			{
				_oKnitDyeingPTU = oKnitDyeingPTU;
				_oKnitDyeingPTU = _oKnitDyeingPTU.Save((int)Session[SessionInfo.currentUserID]);

			}
			catch (Exception ex)
			{
				_oKnitDyeingPTU = new KnitDyeingPTU();
				_oKnitDyeingPTU.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oKnitDyeingPTU);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult Approved(KnitDyeingYarnBooking oKnitDyeingYarnBooking)
        {
            KnitDyeingYarnBooking _oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
            List<KnitDyeingYarnBooking> _oKnitDyeingYarnBookings = new List<KnitDyeingYarnBooking>();
            try
            {
                _oKnitDyeingYarnBooking = oKnitDyeingYarnBooking;
                _oKnitDyeingYarnBooking = _oKnitDyeingYarnBooking.Approve((int)Session[SessionInfo.currentUserID]);
                _oKnitDyeingYarnBookings = KnitDyeingYarnBooking.Gets("SELECT * FROM View_KnitDyeingYarnBooking WHERE KnitDyeingPTUID = " + oKnitDyeingYarnBooking.KnitDyeingPTUID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
                _oKnitDyeingYarnBooking.ErrorMessage = ex.Message;
                _oKnitDyeingYarnBookings.Add(_oKnitDyeingYarnBooking);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingYarnBookings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveYarnBooking(KnitDyeingYarnBooking oKnitDyeingYarnBooking)
        {
            KnitDyeingYarnBooking _oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
            try
            {
                _oKnitDyeingYarnBooking = oKnitDyeingYarnBooking;
                _oKnitDyeingYarnBooking = _oKnitDyeingYarnBooking.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
                _oKnitDyeingYarnBooking.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingYarnBooking);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				KnitDyeingPTU oKnitDyeingPTU = new KnitDyeingPTU();
				sFeedBackMessage = oKnitDyeingPTU.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        [HttpGet]
        public JsonResult DeleteYarnBooking(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                KnitDyeingYarnBooking oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
                sFeedBackMessage = oKnitDyeingYarnBooking.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public ActionResult SetKnitDyeingPTUListData(KnitDyeingPTU oKnitDyeingPTU)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oKnitDyeingPTU);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByPTUwithType(KnitDyeingPTULog oKnitDyeingPTULog)
        {
            _oKnitDyeingPTULogs = new List<KnitDyeingPTULog>();
            try
            {
                _oKnitDyeingPTULogs = KnitDyeingPTULog.Gets(oKnitDyeingPTULog.KnitDyeingPTUID, oKnitDyeingPTULog.KnitDyeingPTURefTypeInt, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oKnitDyeingPTULog = new KnitDyeingPTULog();
                oKnitDyeingPTULog.ErrorMessage = ex.Message;
                _oKnitDyeingPTULogs.Add(oKnitDyeingPTULog);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingPTULogs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintKnitDyeingPTUs(int nKnitDyeingProgramID, double ts)
        {
            KnitDyeingProgram oKnitDyeingProgram = new KnitDyeingProgram();
            _oKnitDyeingPTUs = new List<KnitDyeingPTU>();
            try
            {

                StringBuilder sSQL = new StringBuilder("SELECT HH.KnitDyeingProgramID, HH.IssueDate,HH.MerchandiserName, HH.ShipmentDate, HH.MUSymbol, HH.FabricName,HH.ApprovedByName , HH.SessionName,HH.TechnicalSheetID,  HH.RefNo , HH.StyleNo, HH.BuyerName, HH.RecapOrPAMNos, (SELECT SUM(MM.GarmentsQty) FROM (SELECT DISTINCT TT.RefObjectNo, TT.ColorID, TT.GarmentsQty FROM View_KnitDyeingProgramDetail AS TT WHERE TT.KnitDyeingProgramID = HH.KnitDyeingProgramID ) AS MM) AS OrderQty  FROM View_KnitDyeingProgram AS HH WHERE HH.KnitDyeingProgramID = " + nKnitDyeingProgramID);
                oKnitDyeingProgram = KnitDyeingProgram.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID])[0];
                _oKnitDyeingPTUs = KnitDyeingPTU.Gets("SELECT * FROM View_KnitDyeingPTU WHERE KnitDyeingProgramID = " + nKnitDyeingProgramID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oKnitDyeingPTU = new KnitDyeingPTU();
                _oKnitDyeingPTUs = new List<KnitDyeingPTU>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptKnitDyeingPTUs oReport = new rptKnitDyeingPTUs();
            byte[] abytes = oReport.PrepareReport(oKnitDyeingProgram, _oKnitDyeingPTUs, oCompany);
            return File(abytes, "application/pdf");
        }
	}

}
