using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;


using System.Web.Script.Serialization;
using ReportManagement;
using ESimSol.Reports;

namespace ESimSolFinancial.Controllers
{
    public class MarketingScheduleController : PdfViewController
    {
        #region Declaration
        MarketingSchedule _oMarketingSchedule = new MarketingSchedule();
        List<MarketingSchedule> _oMarketingSchedules = new List<MarketingSchedule>();
        string _sErrorMessage = "";
        string _sSQL = "";
        byte[] _abytes = null;
        #endregion
        #region 
        public ActionResult ViewMarketingSchedules(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);


            _oMarketingSchedules = new List<MarketingSchedule>();
            _oMarketingSchedules = MarketingSchedule.GetsByCurrentMonth(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Employees = MarketingAccount.GetsByBUAndGroup(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oMarketingSchedules);
        }

        public ActionResult ViewMSManagerCalendar(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);


            _oMarketingSchedules = new List<MarketingSchedule>();
            _oMarketingSchedules = MarketingSchedule.GetsByCurrentMonth(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.RefTypes = EnumObject.jGets(typeof(EnumMKTRef));
            //ViewBag.RefTypes = Enum.GetValues(typeof(EnumMKTRef)).Cast<EnumMKTRef>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
            ViewBag.RefTypes = EnumObject.jGets(typeof(EnumMKTRef)).Where(x => x.id != (int)EnumMKTRef.None);
            return View(_oMarketingSchedules);
        }
        public ActionResult ViewMSCalendar(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            MarketingAccount oMarketingAccount = new MarketingAccount();
            oMarketingAccount = oMarketingAccount.GetByUser(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oMarketingSchedules = new List<MarketingSchedule>();
            _oMarketingSchedules = MarketingSchedule.GetsByCurrentMonth(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), (int)(((User)Session[SessionInfo.CurrentUser]).EmployeeID), ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Employee = oMarketingAccount;
            ViewBag.BUID = buid;
            return View(_oMarketingSchedules);
        }
        #endregion

        #region Functions


        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_MarketingSchedule";

            DateTime dScheduleDateTime = _oMarketingSchedule.IsForBaseCollection ? 
                (Arguments.Split(';')[1].Split('~')[0] == null) ? DateTime.MinValue : Convert.ToDateTime(Arguments.Split(';')[1].Split('~')[0]) : DateTime.MinValue;

            string sScheduleDateTime = _oMarketingSchedule.IsForBaseCollection ? 
                "" : (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            
            int nMKTPersonID = _oMarketingSchedule.IsForBaseCollection ?
                _oMarketingSchedule.IsForBaseCalendar ?
                (Arguments.Split(';')[1].Split('~')[1] == null) ? 0 : (Arguments.Split(';')[1].Split('~')[1] == "") ? 0 : Convert.ToInt32(Arguments.Split(';')[1].Split('~')[1])
                : 0
                : (Arguments.Split(';')[1].Split('~')[1] == null) ? 0 : (Arguments.Split(';')[1].Split('~')[1] == "") ? 0 : Convert.ToInt32(Arguments.Split(';')[1].Split('~')[1]);
            
            int nBuyerID = _oMarketingSchedule.IsForBaseCollection ?
                _oMarketingSchedule.IsForBaseCalendar ?
                (Arguments.Split(';')[1].Split('~')[2] == null) ? 0 : (Arguments.Split(';')[1].Split('~')[2] == "") ? 0 : Convert.ToInt32(Arguments.Split(';')[1].Split('~')[2])
                : 0
                : (Arguments.Split(';')[1].Split('~')[2] == null) ? 0 : (Arguments.Split(';')[1].Split('~')[2] == "") ? 0 : Convert.ToInt32(Arguments.Split(';')[1].Split('~')[2]);
            


            string sSQL = "";


            #region Schedule DateTime
            if (dScheduleDateTime != null)
            {
                if (dScheduleDateTime != DateTime.MinValue)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + "CONVERT(DATE,CONVERT(VARCHAR(12),ScheduleDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dScheduleDateTime.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dScheduleDateTime.AddMonths(1).ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            if (sScheduleDateTime != null)
            {
                if (sScheduleDateTime != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ScheduleDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + sScheduleDateTime + "',106))";
                }
            }
            #endregion
            #region MKT Person
            if (nMKTPersonID != null)
            {
                if (nMKTPersonID > 0)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " MKTPersonID IN  (" + nMKTPersonID + ") ";
                }
            }
            #endregion
            #region Buyer
            if (nBuyerID != null)
            {
                if (nBuyerID > 0)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " BuyerID IN (" + nBuyerID + ") ";
                }
            }
            #endregion


            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
            //_sSQL = _sSQL + " ORDER BY ChequeBookID DESC";
        }





        //#region Old Code
        //public ActionResult ViewMarketingSchedule(int id)
        //{
        //    _oMarketingSchedule = new MarketingSchedule();
        //    if (id > 0)
        //    {
        //        _oMarketingSchedule = _oMarketingSchedule.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    return PartialView(_oMarketingSchedule);
        //}
        //#endregion

        #endregion


        #region New Code
     

        [HttpPost]
        public JsonResult Save(MarketingSchedule oMarketingSchedule)
        {
            _oMarketingSchedule = new MarketingSchedule();
            try
            {
                _oMarketingSchedule = oMarketingSchedule;
                _oMarketingSchedule.Remarks = (_oMarketingSchedule.Remarks == null) ? "" : _oMarketingSchedule.Remarks;
                _oMarketingSchedule = _oMarketingSchedule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMarketingSchedule = new MarketingSchedule();
                _oMarketingSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(MarketingSchedule oMarketingSchedule)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oMarketingSchedule.Delete(oMarketingSchedule.MarketingScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Get(MarketingSchedule oMarketingSchedule)
        {
            _oMarketingSchedule = new MarketingSchedule();
            try
            {
                if (oMarketingSchedule.MarketingScheduleID > 0)
                {
                    _oMarketingSchedule = _oMarketingSchedule.Get(oMarketingSchedule.MarketingScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    _oMarketingSchedule.MKTPersonName = new Employee().Get(_oMarketingSchedule.MKTPersonID, ((User)Session[SessionInfo.CurrentUser]).UserID).Name;
                    _oMarketingSchedule.BuyerName =new Contractor().Get(_oMarketingSchedule.BuyerID, ((User)Session[SessionInfo.CurrentUser]).UserID).Name;
                    _oMarketingSchedule.MeetingSummarys = MeetingSummary.Gets(_oMarketingSchedule.MarketingScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (oMarketingSchedule.MKTPersonID > 0)
                {
                    _oMarketingSchedule.MKTPersonID = oMarketingSchedule.MKTPersonID;
                    _oMarketingSchedule.ScheduleDateTime = oMarketingSchedule.ScheduleDateTime;
                    _oMarketingSchedule.MKTPersonName = new Employee().Get(oMarketingSchedule.MKTPersonID, ((User)Session[SessionInfo.CurrentUser]).UserID).Name;
                    _oMarketingSchedule.MarketingSchedules = oMarketingSchedule.IsForBaseCollection ? MarketingSchedule.Gets(oMarketingSchedule.MKTPersonID, _oMarketingSchedule.ScheduleDateTime, ((User)Session[SessionInfo.CurrentUser]).UserID) : null;
                }
                else if(oMarketingSchedule.ScheduleDateTime !=DateTime.MinValue)
                {
                    _oMarketingSchedule.ScheduleDateTime = oMarketingSchedule.ScheduleDateTime;
                    _oMarketingSchedule.MarketingSchedules = oMarketingSchedule.IsForMKTPerson ? MarketingSchedule.Gets((int)((User)Session[SessionInfo.CurrentUser]).EmployeeID, _oMarketingSchedule.ScheduleDateTime, ((User)Session[SessionInfo.CurrentUser]).UserID) : MarketingSchedule.Gets(_oMarketingSchedule.ScheduleDateTime, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oMarketingSchedule = new MarketingSchedule();
                _oMarketingSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetForCalendar(MarketingSchedule oMarketingSchedule)
        {
            _oMarketingSchedule = new MarketingSchedule();
            try
            {
                if (oMarketingSchedule.ScheduleDateTime != DateTime.MinValue)
                {
                    _oMarketingSchedule = oMarketingSchedule;

                    this.MakeSQL(_oMarketingSchedule.ErrorMessage);
                    _oMarketingSchedule.MarketingSchedules =MarketingSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oMarketingSchedule = new MarketingSchedule();
                _oMarketingSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(MarketingSchedule oMarketingSchedule)
        {
            _oMarketingSchedule = oMarketingSchedule;
            this.MakeSQL(_oMarketingSchedule.ErrorMessage);
            List<MarketingSchedule> oMarketingSchedules = new List<MarketingSchedule>();
            oMarketingSchedules = MarketingSchedule.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMarketingSchedules);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PrintMarketingSchedules(FormCollection DataCollection)
        {
            //_bForPrint = true;
            //this.MakeSQL(arguments);

            _oMarketingSchedules = new List<MarketingSchedule>();
            _oMarketingSchedules = new JavaScriptSerializer().Deserialize<List<MarketingSchedule>>(DataCollection["txtCollectionPrintText"]);
            

            Company oCompany = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);


            string Messge = "Schedule List";
            rptMarketingSchedules oReport = new rptMarketingSchedules();
            _abytes = oReport.PrepareReport(_oMarketingSchedules, oCompany, Messge);
            //byte[] abytes = oReport.PrepareReport(oMarketingSchedules, oGroupInfo, Messge);
            return File(_abytes, "application/pdf");
            //return RedirectToAction("PrintPreview");

        }
        #endregion

    }

}
