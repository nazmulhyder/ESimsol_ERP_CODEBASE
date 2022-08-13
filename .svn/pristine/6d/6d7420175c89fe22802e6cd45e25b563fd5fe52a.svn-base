using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ReportManagement;
using ICS.Core.Utility;
using ESimSol.Reports;

namespace ESimSolFinancial.Controllers
{
    public class MeetingSummaryController : PdfViewController
    {
        #region Declaration
        MeetingSummary _oMeetingSummary = new MeetingSummary();
        List<MeetingSummary> _oMeetingSummarys = new List<MeetingSummary>();
        string _sErrorMessage = "";
        string _sSQL = "";
        byte[] _abytes = null;
        #endregion

        #region Functions
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_MeetingSummary";

            int nMarketingScheduleID = (Arguments.Split(';')[1].Split('~')[0] == null) ? 0 : (Arguments.Split(';')[1].Split('~')[0] == "") ? 0 : Convert.ToInt32(Arguments.Split(';')[1].Split('~')[0]);
            int nBuyerID = (Arguments.Split(';')[1].Split('~')[1] == null) ? 0 : (Arguments.Split(';')[1].Split('~')[1] == "") ? 0 : Convert.ToInt32(Arguments.Split(';')[1].Split('~')[1]);
            string sDateFrom = (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[2];
            string sDateTo = (Arguments.Split(';')[1].Split('~')[3] == null) ? "" : Arguments.Split(';')[1].Split('~')[3];
            
            string sSQL = "";

            #region MarketingSchedule
            if (nMarketingScheduleID != null)
            {
                if (nMarketingScheduleID > 0)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " MarketingScheduleID =  " + nMarketingScheduleID + " ";
                }
            }
            #endregion
            #region Buyer
            if (nBuyerID != null)
            {
                if (nBuyerID > 0)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " BuyerID =  " + nBuyerID + " ";
                }
            }
            #endregion
            #region DBServerDateTime
            if (sDateFrom != null&&sDateTo!=null)
            {
                if (sDateFrom != "" && sDateTo != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + sDateFrom + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + sDateTo + "',106))" + " ";
                }
            }
            #endregion
            
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
            _sSQL = _sSQL + " ORDER BY MeetingSummaryID ASC";
        }
        #endregion
        #region 
        [HttpPost]
        public JsonResult Save(MeetingSummary oMeetingSummary)
        {
            _oMeetingSummary = new MeetingSummary();
            try
            {
                _oMeetingSummary = oMeetingSummary;
                _oMeetingSummary = _oMeetingSummary.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMeetingSummary = new MeetingSummary();
                _oMeetingSummary.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMeetingSummary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveFromFabric(MeetingSummary oMeetingSummary)
        {
            _oMeetingSummary = new MeetingSummary();
            string sTemp = oMeetingSummary.ErrorMessage;
            int nCount = 0;
            int nMKTPID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            int nBuyerID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            try
            {
                _oMeetingSummary = oMeetingSummary;
                _oMeetingSummary = oMeetingSummary.SaveFromFabric(oMeetingSummary, nMKTPID, nBuyerID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMeetingSummary = new MeetingSummary();
                _oMeetingSummary.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMeetingSummary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(MeetingSummary oMeetingSummary)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oMeetingSummary.Delete(oMeetingSummary.MeetingSummaryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Get(MeetingSummary oMeetingSummary)
        {
            _oMeetingSummary = new MeetingSummary();
            try
            {
                if (oMeetingSummary.MeetingSummaryID > 0)
                {
                    _oMeetingSummary = _oMeetingSummary.Get(oMeetingSummary.MeetingSummaryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oMeetingSummary = new MeetingSummary();
                _oMeetingSummary.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMeetingSummary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Gets(MeetingSummary oMeetingSummary)
        {
            _oMeetingSummary = oMeetingSummary;
            this.MakeSQL(_oMeetingSummary.ErrorMessage);
            List<MeetingSummary> oMeetingSummarys = new List<MeetingSummary>();
            oMeetingSummarys = MeetingSummary.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMeetingSummarys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsMeetingSummaryByFabricID(MeetingSummary oMeetingSummary)
        {
            _oMeetingSummary = oMeetingSummary;
            List<MeetingSummary> oMeetingSummarys = new List<MeetingSummary>();
            oMeetingSummarys = MeetingSummary.Gets("SELECT * FROM View_MeetingSummary WHERE RefID = " + oMeetingSummary.RefID + " ORDER BY MeetingSummaryID DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMeetingSummarys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetScheduleByFabricID(MeetingSummary oMeetingSummary)
        //{
        //    string sTemp = oMeetingSummary.ErrorMessage;
        //    int nCount = 0;
        //    int nFabricID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
        //    int nMKTPID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
        //    int nBuyerID = Convert.ToInt32(sTemp.Split('~')[nCount++]);

        //    oMeetingSummary = oMeetingSummary.GetScheduleByFabricID(nFabricID, nMKTPID, nBuyerID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    SELECT top(1)* FROM View_MeetingSummary WHERE RefID = " + nFabricID + " AND RefType = " + (int)EnumMKTRef.FabricID;

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize((object)oMeetingSummary);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult GetScheduleByFabricID(MeetingSummary oMeetingSummary)
        {
            string sTemp = oMeetingSummary.ErrorMessage;
            int nCount = 0;
            int nFabricID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            int nMKTPID = Convert.ToInt32(sTemp.Split('~')[nCount++]);
            int nBuyerID = Convert.ToInt32(sTemp.Split('~')[nCount++]);

            _oMeetingSummarys = MeetingSummary.Gets("SELECT top(1)* FROM View_MeetingSummary WHERE RefID = " + nFabricID + " AND RefType = " + (int)EnumMKTRef.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if(_oMeetingSummarys.Count>0)
            {
                oMeetingSummary = _oMeetingSummarys[0];
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMeetingSummary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteMeetingSummaryByID(MeetingSummary oMeetingSummary)
        {
            MeetingSummary objMeetingSummary = new MeetingSummary();
            try
            {
                if(oMeetingSummary.MeetingSummaryID>0)
                {
                    objMeetingSummary.ErrorMessage = oMeetingSummary.Delete(oMeetingSummary.MeetingSummaryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                objMeetingSummary = new MeetingSummary();
                objMeetingSummary.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(objMeetingSummary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewMeetingSummarys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oMeetingSummarys = new List<MeetingSummary>();
            //_oMeetingSummarys = MeetingSummary.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oMeetingSummarys);
        }

        [HttpPost]
        public ActionResult PrintMeetingSummarys(FormCollection DataCollection)
        {
            //_bForPrint = true;
            //this.MakeSQL(arguments);

            _oMeetingSummarys = new List<MeetingSummary>();
            _oMeetingSummarys = new JavaScriptSerializer().Deserialize<List<MeetingSummary>>(DataCollection["txtCollectionPrintText"]);
            string sBuyerName = DataCollection["txtBuyerNamePrint"];
            string sDatefrom = DataCollection["txtDateFromPrint"];
            string sDateTo = DataCollection["txtDateToPrint"];
            Company oCompany = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string Messge = "Buyer Interaction Log for " + sBuyerName + " from " + sDatefrom + " to " + sDateTo;
            rptMeetingSummarys oReport = new rptMeetingSummarys();
            _abytes = oReport.PrepareReport(_oMeetingSummarys, oCompany, Messge);
            //byte[] abytes = oReport.PrepareReport(oMeetingSummarys, oGroupInfo, Messge);
            return File(_abytes, "application/pdf");
            //return RedirectToAction("PrintPreview");

        }
        #endregion

    }

}
       

      

       
       
       
