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
	public class DocPrintEngineController : Controller
	{
		#region Declaration
		DocPrintEngine _oDocPrintEngine = new DocPrintEngine();
		List<DocPrintEngine> _oDocPrintEngines = new  List<DocPrintEngine>();
        DocPrintEngineDetail _oDocPrintEngineDetail = new DocPrintEngineDetail();
        List<DocPrintEngineDetail> _oDocPrintEngineDetails = new List<DocPrintEngineDetail>();
		#endregion

		#region Actions

		public ActionResult ViewDocPrintEngines(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			_oDocPrintEngines = new List<DocPrintEngine>(); 
			_oDocPrintEngines = DocPrintEngine.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

			return View(_oDocPrintEngines);
		}

        public ActionResult ViewDocPrintEngine(int id, int buid)
		{
			_oDocPrintEngine = new DocPrintEngine();
			if (id > 0)
			{
				_oDocPrintEngine = _oDocPrintEngine.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID=" + id + " Order By  CONVERT(int,SLNo)", ((User)Session[SessionInfo.CurrentUser]).UserID);
                //"SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineDetailID=%n
            }
            ViewBag.SizingFields = EnumObject.jGets(typeof(ESimSol.Reports.rptFabricBatchCard.EnumSizingSection));
            List<BusinessUnit> oBUs = new List<BusinessUnit>();
            BusinessUnit oBU = new BusinessUnit();
            if (buid > 0)
            {
                oBU = oBU.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBUs.Add(oBU);
            }
            else
            {
                oBUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BusinessUnits = oBUs;
            ViewBag.BUID = buid;
            ViewBag.LetterTypes = EnumObject.jGets(typeof(EnumDocumentPrintType)).Where(x => x.id != 0);
            ViewBag.Modules = EnumObject.jGets(typeof(EnumModuleName)).OrderBy(x=>x.Value);
			return View(_oDocPrintEngine);
		}

		[HttpPost]
		public JsonResult Save(DocPrintEngine oDocPrintEngine)
		{
			_oDocPrintEngine = new DocPrintEngine();
			try
			{
				_oDocPrintEngine = oDocPrintEngine;
				_oDocPrintEngine = _oDocPrintEngine.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
			}
			catch (Exception ex)
			{
				_oDocPrintEngine = new DocPrintEngine();
				_oDocPrintEngine.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oDocPrintEngine);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Copy(DocPrintEngine oDocPrintEngine)
        {
            _oDocPrintEngine = new DocPrintEngine();
            try
            {
                _oDocPrintEngine = oDocPrintEngine;
                _oDocPrintEngine = _oDocPrintEngine.Copy(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID=" + _oDocPrintEngine.DocPrintEngineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDocPrintEngine = new DocPrintEngine();
                _oDocPrintEngine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDocPrintEngine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

		[HttpPost]
        public JsonResult Delete(DocPrintEngine oDPE)
		{
			string sFeedBackMessage = "";
			try
			{
				DocPrintEngine oDocPrintEngine = new DocPrintEngine();
                sFeedBackMessage = oDocPrintEngine.Delete(oDPE.DocPrintEngineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Update(DocPrintEngine oDocPrintEngine)
        {
            try
            {
                if (oDocPrintEngine.DocPrintEngineID > 0)
                {
                    oDocPrintEngine = oDocPrintEngine.Update(oDocPrintEngine.DocPrintEngineID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else throw new Exception("Sorry, Invalid DocPrintEngine ID !!");
            }
            catch (Exception ex)
            {
                oDocPrintEngine = new DocPrintEngine();
                oDocPrintEngine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDocPrintEngine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

        #region Actions DETAILS

        [HttpPost]
        public JsonResult SaveDetail(DocPrintEngineDetail oDocPrintEngineDetail)
        {
            _oDocPrintEngineDetail = new DocPrintEngineDetail();
            try
            {
                _oDocPrintEngineDetail = oDocPrintEngineDetail;
                _oDocPrintEngineDetail = _oDocPrintEngineDetail.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDocPrintEngineDetail = new DocPrintEngineDetail();
                _oDocPrintEngineDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDocPrintEngineDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDetail(DocPrintEngineDetail oDocPrintEngineDetail)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDocPrintEngineDetail.Delete(oDocPrintEngineDetail.DocPrintEngineDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetProperties(DocPrintEngine oDocPrintEngine)
        {
            List<string> oProperties = new List<string>();
            try
            {
                switch (oDocPrintEngine.ModuleID)
                {
                    case (int)EnumModuleName.HangerSticker:
                        oProperties = GetPropertiesNameOfClass(new HangerSticker());
                        break;
                    case (int)EnumModuleName.FabricBatchProduction:
                        oProperties = GetPropertiesNameOfClass(new FabricBatchProduction());
                        break;
                    case (int)EnumModuleName.RouteSheetDetail:
                        oProperties = GetPropertiesNameOfClass(new RSInQCDetail());
                        break;
                    case (int)EnumModuleName.FabricBatch:
                        oProperties = GetPropertiesNameOfClass(new FabricBatch());
                        break;
                }
            }
            catch (Exception ex)
            {
                oProperties.Add("Error");
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProperties);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDocumentByModule(DocPrintEngine oDocPrintEngine)
        {
            _oDocPrintEngines = new List<DocPrintEngine>();

            try
            {
                string sSQL = "SELECT * FROM DocPrintEngine WHERE DocPrintEngine.DocPrintEngineID > 0";

                if(oDocPrintEngine.ModuleID > 0) 
                {
                    sSQL = sSQL + " AND DocPrintEngine.ModuleID =" + oDocPrintEngine.ModuleID;
                }
                _oDocPrintEngines = DocPrintEngine.Gets( sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDocPrintEngines.Add(new DocPrintEngine() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDocPrintEngines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStickerDocumentsByModule(DocPrintEngine oDocPrintEngine)
        {
            _oDocPrintEngines = new List<DocPrintEngine>();

            try
            {
                string sSQL = "SELECT * FROM DocPrintEngine WHERE DocPrintEngine.DocPrintEngineID > 0 AND LetterType = " + (int)EnumDocumentPrintType.STICKER_PRINT;

                if (oDocPrintEngine.ModuleID > 0)
                {
                    sSQL = sSQL + " AND DocPrintEngine.ModuleID =" + oDocPrintEngine.ModuleID;
                }
                _oDocPrintEngines = DocPrintEngine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDocPrintEngines.Add(new DocPrintEngine() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDocPrintEngines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCardDocumentsByModule(DocPrintEngine oDocPrintEngine)
        {
            _oDocPrintEngines = new List<DocPrintEngine>();

            try
            {
                string sSQL = "SELECT * FROM DocPrintEngine WHERE DocPrintEngine.DocPrintEngineID > 0 AND LetterType = " + (int)EnumDocumentPrintType.CARD_PRINT;

                if (oDocPrintEngine.ModuleID > 0)
                {
                    sSQL = sSQL + " AND DocPrintEngine.ModuleID =" + oDocPrintEngine.ModuleID;
                }
                _oDocPrintEngines = DocPrintEngine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDocPrintEngines.Add(new DocPrintEngine() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDocPrintEngines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public List<string> GetPropertiesNameOfClass(object pObject)
        {
            List<string> propertyList = new List<string>();
            if (pObject != null)
            {
                foreach (var prop in pObject.GetType().GetProperties())
                {
                    propertyList.Add(prop.Name);
                }
            }
            return propertyList;
        }
        #endregion
    }

}
