using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;


namespace ESimSolFinancial.Controllers
{
    public class HangerStickerController : Controller
    {
        #region Declaration
        
        HangerSticker _oHangerSticker = new HangerSticker();
        List<HangerSticker> _oHangerStickers = new List<HangerSticker>();
        #endregion

        #region Fabric Sticker
        public ActionResult View_HangerStickerTemplates(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oHangerStickers = HangerSticker.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
         
            Company oCompany = new Company();
            ViewBag.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oHangerStickers);
        }

        [HttpPost]
        public JsonResult SaveHangerSticker(HangerSticker oHangerSticker)
        {
            try
            {
                oHangerSticker = oHangerSticker.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oHangerSticker = new HangerSticker();
                oHangerSticker.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oHangerSticker);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHangerSticker(HangerSticker oHangerSticker)
        {
            _oHangerSticker = new HangerSticker();
            try
            {
                if (oHangerSticker.HangerStickerID > 0)
                {
                    _oHangerSticker = _oHangerSticker.Get(oHangerSticker.HangerStickerID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oHangerSticker = new HangerSticker();
                _oHangerSticker.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHangerSticker);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteHangerSticker(HangerSticker oHangerSticker)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oHangerSticker.Delete(oHangerSticker.HangerStickerID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public ActionResult PrintHangerSticker(FormCollection DataCollection)
        {

            string HangerStickerIDs = DataCollection["HangerStickerIDs"];
           
            _oHangerStickers = HangerSticker.Gets("Select * FROM HangerSticker WHERE HangerStickerID IN  (" + HangerStickerIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptHangerSticker oReport = new rptHangerSticker();
            byte[] abytes = oReport.PrepareReport(_oHangerStickers);
            return File(abytes, "application/pdf");
        }

        [HttpPost]
        public ActionResult SetSessionData(List<HangerSticker> oHangerStickers)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oHangerStickers);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
                
        public ActionResult PrintHangerStickerNinePerPage(double ts)
        {
            HangerSticker oHangerSticker = new HangerSticker();
            List<HangerSticker> oHangerStickers = new List<HangerSticker>();
            try
            {
                oHangerStickers = (List<HangerSticker>)Session[SessionInfo.ParamObj];
                this.Session.Remove(SessionInfo.ParamObj);
            }
            catch (Exception ex)
            {
                oHangerStickers = new List<HangerSticker>();
            }

            _oHangerStickers = new List<HangerSticker>();
            foreach (HangerSticker oItem in oHangerStickers)
            {
                _oHangerStickers.Add(oItem);
                for (int i = 1; i < oItem.PrintCopy; i++)
                {
                    oHangerSticker = new HangerSticker();
                    oHangerSticker.HangerStickerID = oItem.HangerStickerID;
                    oHangerSticker.ART = oItem.ART;
                    oHangerSticker.Supplier = oItem.Supplier;
                    oHangerSticker.Composition = oItem.Composition;
                    oHangerSticker.Construction = oItem.Construction;
                    oHangerSticker.Finishing = oItem.Finishing;
                    oHangerSticker.MOQ = oItem.MOQ;
                    oHangerSticker.Remarks = oItem.Remarks;
                    oHangerSticker.Price = oItem.Price;
                    oHangerSticker.Date = oItem.Date;
                    oHangerSticker.Width = oItem.Width;
                    oHangerSticker.ErrorMessage = oItem.ErrorMessage;
                    oHangerSticker.Params = oItem.Params;
                    oHangerSticker.PrintCount = oItem.PrintCount;
                    oHangerSticker.PrintCopy = oItem.PrintCopy;
                    _oHangerStickers.Add(oHangerSticker);
                }
            }
            //_oHangerStickers = HangerSticker.Gets("Select * FROM HangerSticker WHERE HangerStickerID IN  (" + HangerStickerIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptHangerSticker oReport = new rptHangerSticker();
            byte[] abytes = oReport.PrepareReport(_oHangerStickers);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintDynamicSticker(double ts, int ndoc)
        {
            HangerSticker oHangerSticker = new HangerSticker();
            DocPrintEngine oDocPrintEngine = new DocPrintEngine();

            List<HangerSticker> oHangerStickers = new List<HangerSticker>();
            try
            {
                oHangerStickers = (List<HangerSticker>)Session[SessionInfo.ParamObj];
                this.Session.Remove(SessionInfo.ParamObj);

                if (ndoc <= 0)
                {
                    oDocPrintEngine = oDocPrintEngine.GetActiveByTypenModule((int)EnumDocumentPrintType.STICKER_PRINT, (int)EnumModuleName.HangerSticker, (int)Session[SessionInfo.currentUserID]);
                }
                else
                    oDocPrintEngine = oDocPrintEngine.Get(ndoc, (long)Session[SessionInfo.currentUserID]);

                oDocPrintEngine.DocPrintEngineDetails = DocPrintEngineDetail.Gets("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineID =" + oDocPrintEngine.DocPrintEngineID + "  ORDER BY CONVERT(INT, SLNo)", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oHangerStickers = new List<HangerSticker>();
            }

            _oHangerStickers = new List<HangerSticker>();
            foreach (HangerSticker oItem in oHangerStickers)
            {
                _oHangerStickers.Add(oItem);
                for (int i = 1; i < oItem.PrintCopy; i++)
                {
                    oHangerSticker = new HangerSticker();
                    oHangerSticker.HangerStickerID = oItem.HangerStickerID;
                    oHangerSticker.ART = oItem.ART;
                    oHangerSticker.Supplier = oItem.Supplier;
                    oHangerSticker.Composition = oItem.Composition;
                    oHangerSticker.Construction = oItem.Construction;
                    oHangerSticker.Finishing = oItem.Finishing;
                    oHangerSticker.MOQ = oItem.MOQ;
                    oHangerSticker.Remarks = oItem.Remarks;
                    oHangerSticker.Price = oItem.Price;
                    oHangerSticker.Date = oItem.Date;
                    oHangerSticker.Width = oItem.Width;
                    oHangerSticker.ErrorMessage = oItem.ErrorMessage;
                    oHangerSticker.Params = oItem.Params;
                    oHangerSticker.PrintCount = oItem.PrintCount;
                    oHangerSticker.PrintCopy = oItem.PrintCopy;
                    _oHangerStickers.Add(oHangerSticker);
                }
            }
            rptHangerSticker oReport = new rptHangerSticker();
            byte[] abytes = oReport.PrepareReport_Dynamic_Sticker(_oHangerStickers, new Company(), oDocPrintEngine);
            return File(abytes, "application/pdf");
        }

        [HttpPost]
        public JsonResult GetsByArticleNo(HangerSticker oHangerSticker)
        {
            List<HangerSticker> oHangerStickers = new List<HangerSticker>();
            try
            {
                string sSQL = "SELECT * FROM HangerSticker AS HH WHERE HH.ART LIKE '%" + oHangerSticker.ART + "%' ORDER BY HH.ART ASC";
                oHangerStickers =  HangerSticker.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oHangerStickers = new List<HangerSticker>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oHangerStickers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}