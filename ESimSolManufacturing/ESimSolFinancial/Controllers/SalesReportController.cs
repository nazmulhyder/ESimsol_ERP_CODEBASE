using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ESimSolFinancial.Controllers
{
    public class SalesReportController : Controller
    {
        #region Declaration
        public int IsDouble = 0;
        SalesReport _oSalesReport = new SalesReport();
        List<SalesReport> _oSalesReports = new List<SalesReport>();
        List<MarketingAccount> _oMarketingAccounts = new List<MarketingAccount>();
        MarketingAccount _oMarketingAccount = new MarketingAccount();
        string MktPersonName = "";
        #endregion
        [HttpGet]
        public ActionResult ViewSalesReports(int menuid,int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string ssSQL = "SELECT * FROM View_SalesReport  AS HH ORDER BY HH.SalesReportID ASC";
            _oSalesReports = SalesReport.Gets(ssSQL, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            string SQL = "SELECT * FROM View_SalesReport AS HH WHERE HH.SalesReportID>0";
            ViewBag.SalesReports = SalesReport.Gets(SQL,(int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.BUID = buid;
            return View(_oSalesReports);
        }
        public ActionResult ViewMktProjection(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.MktProjectionReport).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<MarketingAccount> _oMarketingAccounts = new List<MarketingAccount>();
            int uID = ((User)Session[SessionInfo.CurrentUser]).UserID;
            string sSQL = "";
            _oMarketingAccounts = MarketingAccount.GetsGroup("",buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.OrderType = EnumObject.jGets(typeof(EnumFabricRequestType));
            return View(_oMarketingAccounts);
        }
        [HttpPost]
        public JsonResult LoadMktHeads(MarketingAccount oMarketingAccount)
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            string sParams = oMarketingAccount.ErrorMessage;
            oMarketingAccount.GroupID = Convert.ToInt32(sParams.Split('~')[0]);
            int BUID = Convert.ToInt32(sParams.Split('~')[2]);
            List<MarketingAccount> _oMktAccounts = new List<MarketingAccount>();
           
            string sSQL = "";
            try
            {

                if (oMarketingAccount.GroupID == 1)/// 1 Group Gets
                {
                    _oMarketingAccounts = MarketingAccount.GetsGroup("", BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (oMarketingAccount.GroupID == 2) // MktPersonName persion
                {
                    _oMarketingAccounts = MarketingAccount.GetsByUser(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
              
            }
            catch (Exception ex)
            {
                _oMarketingAccount = new MarketingAccount();
                _oMarketingAccount.ErrorMessage = ex.Message;
                _oMarketingAccounts.Add(_oMarketingAccount);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Activate(SalesReport oSalesReport)
        {
            try
            {            
                if (oSalesReport.Activity == true)
                {
                    throw new Exception("Already Active.");
                }
                oSalesReport = oSalesReport.Activate((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oSalesReport = new SalesReport();
                oSalesReport.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InActivate(SalesReport oSalesReport)
        {
            try
            {
             
                if (oSalesReport.Activity == false)
                {
                    throw new Exception("Already InActive.");
                }
                oSalesReport = oSalesReport.InActivate((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oSalesReport = new SalesReport();
                oSalesReport.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approve(MktSaleTarget oMktSaleTarget)
        {
            MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            List<MktSaleTarget> _TempList = new List<MktSaleTarget>();
            string sSQL = "";
            try
            {
                _TempList = oMktSaleTarget.oMktSaleTargets;
                _oMktSaleTargets = _oMktSaleTarget.Approve(_TempList, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_MktSaleTarget Where MktSaleTargetID IN (" + oMktSaleTarget.ContractorName + ") Order By Date";
                _oMktSaleTargets = MktSaleTarget.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oMktSaleTargets = new List<MktSaleTarget>();
                _oMktSaleTarget = new MktSaleTarget();
                _oMktSaleTarget.ErrorMessage = ex.Message;
                _oMktSaleTargets.Add(_oMktSaleTarget);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMktSaleTargets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApprove(MktSaleTarget oMktSaleTarget)
        {
            MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            List<MktSaleTarget> _TempList = new List<MktSaleTarget>();

            string sSQL = "";
            try
            {
                _TempList = oMktSaleTarget.oMktSaleTargets;
                _oMktSaleTargets = _oMktSaleTarget.UndoApprove(_TempList, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_MktSaleTarget Where MktSaleTargetID IN (" + oMktSaleTarget.ContractorName + ") Order By Date";
                _oMktSaleTargets = MktSaleTarget.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMktSaleTarget = new MktSaleTarget();
                _oMktSaleTarget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMktSaleTargets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewSalesReportPrint(int menuid,int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oSalesReports = new List<SalesReport>();
            ViewBag.BUID = buid;
            return View(_oSalesReports);
        }
        [HttpGet]
        public ActionResult ViewSalesReportDetails(string sValue)
        {
            _oSalesReports = new List<SalesReport>();
            _oSalesReport = new SalesReport();
            int SalesReportID = Convert.ToInt32(sValue.Split('~')[3]);
            _oSalesReport = _oSalesReport.Get(SalesReportID, (int)Session[SessionInfo.currentUserID]);
            _oSalesReport.ErrorMessage = Convert.ToString(sValue.Split('~')[0]);
            _oSalesReport.ViewType = Convert.ToInt32(sValue.Split('~')[1]);
            int ViewType = _oSalesReport.ViewType;
            _oSalesReport.Year = Convert.ToInt32(sValue.Split('~')[2]);

            try
            {
                string Sql = LayerTwoMakeSQL(_oSalesReport);
                _oSalesReports = SalesReport.Gets(Sql, (int)Session[SessionInfo.currentUserID]);
                _oSalesReports = _oSalesReports.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList();
            }
            catch (Exception ex)
            {
                _oSalesReport = new SalesReport();
                _oSalesReport.ErrorMessage = ex.Message;
                _oSalesReports.Add(_oSalesReport);
            }
            MktPersonName = Convert.ToString(sValue.Split('~')[4]);
            ViewBag.MktPersonName_Year = MktPersonName + " | " + Convert.ToInt32(sValue.Split('~')[2]);
            ViewBag.MktPersonName = MktPersonName;
            ViewBag.SalesReportID = SalesReportID;
            ViewBag.ViewType = ViewType;
            return View(_oSalesReports);
        }
        [HttpGet]
        public ActionResult ViewSalesReportLayer3(string sValue)
        {
            _oSalesReports = new List<SalesReport>();
            _oSalesReport = new SalesReport();
            int SalesReportID = Convert.ToInt32(sValue.Split('~')[3]);
            _oSalesReport = _oSalesReport.Get(SalesReportID, (int)Session[SessionInfo.currentUserID]);
            _oSalesReport.Year = Convert.ToInt32(sValue.Split('~')[0]);
            _oSalesReport.Month = Convert.ToInt32(sValue.Split('~')[1]);
            _oSalesReport.ErrorMessage = Convert.ToInt32(sValue.Split('~')[2]).ToString(); 
            _oSalesReport.ViewType = Convert.ToInt32(sValue.Split('~')[4]);
            try
            {
                string Sql = LayerThreeMakeSQL(_oSalesReport);
                _oSalesReports = SalesReport.Gets(Sql, (int)Session[SessionInfo.currentUserID]);
            }
            catch(Exception  ex)
            {
                _oSalesReport = new SalesReport();
                _oSalesReport.ErrorMessage = ex.Message;
                _oSalesReports.Add(_oSalesReport);
            }

            ViewBag.MktPersonName = Convert.ToString(sValue.Split('~')[5]) + " | " + GetMonthWithYear(Convert.ToInt32(sValue.Split('~')[1]), Convert.ToInt32(sValue.Split('~')[0]));
            ViewBag.Parameters = sValue;
            _oSalesReports = _oSalesReports.OrderBy(x => x.RefDate).ToList();
            return View(_oSalesReports);
        }  
        public string GetMonthWithYear(int MonthInInt,int Year)
        {
            if (MonthInInt == 1)
                return "Jan, " + Year;
            if (MonthInInt == 2)
                return "Feb, " + Year;
            if (MonthInInt == 3)
                return "Mar, " + Year;
            if (MonthInInt == 4)
                return "Apr, " + Year;
            if (MonthInInt == 5)
                return "May, " + Year;
            if (MonthInInt == 6)
                return "Jun, " + Year;
            if (MonthInInt == 7)
                return "Jul, " + Year;
            if (MonthInInt == 8)
                return "Aug, " + Year;
            if (MonthInInt == 9)
                return "Sep, " + Year;
            if (MonthInInt == 10)
                return "Oct, " + Year;
            if (MonthInInt == 11)
                return "Nov, " + Year;
            if (MonthInInt == 12)
                return "Dec, " + Year;
            return "";
        }

        [HttpPost]
        public JsonResult SearchSalesReport(SalesReport oSalesReport)
        {
            _oSalesReports = new List<SalesReport>();
            _oSalesReport = new SalesReport();
            try
            {
                string Sql = MakeSQL(oSalesReport);
                _oSalesReports = SalesReport.Gets(Sql, (int)Session[SessionInfo.currentUserID]);
                _oSalesReport = _oSalesReport.Get(oSalesReport.SalesReportID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSalesReport = new SalesReport();
                _oSalesReport.ErrorMessage = ex.Message;
                _oSalesReports.Add(_oSalesReport);
            }
            List<SalesReport> oSalesReport_Distinct = new List<SalesReport>();
            oSalesReport_Distinct = _oSalesReports.GroupBy(x => x.RefID).Select(g => g.First()).ToList();
            oSalesReport_Distinct = oSalesReport_Distinct.OrderBy(x => x.GroupName).ThenBy(x=>x.RefName).ToList();

            if (oSalesReport_Distinct.Count>0)
            {
                oSalesReport_Distinct[0].Note = _oSalesReport.Note;
            }          

            var tuple = new Tuple<List<SalesReport>, List<SalesReport>>(new List<SalesReport>(), new List<SalesReport>());
            tuple = new Tuple<List<SalesReport>, List<SalesReport>>(_oSalesReports, oSalesReport_Distinct);
            var jsonResult = Json(tuple, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsForGraph(SalesReport oSalesReport)
        {
            _oSalesReports = new List<SalesReport>();
            SalesReport oSalesReportChart = new SalesReport();
            List<SalesReport> oSalesReprotCharts = new List<SalesReport>();
            try
            {
                string Sql = MakeSQL(oSalesReport);
                _oSalesReports = SalesReport.Gets(Sql, (int)Session[SessionInfo.currentUserID]);
                if (_oSalesReports.Count <= 0 || _oSalesReports[0].ErrorMessage != "")
                {
                    throw new Exception("Data not found!");
                }

            }
            catch (Exception ex)
            {
                _oSalesReport = new SalesReport();
                _oSalesReport.ErrorMessage = ex.Message;
                _oSalesReports.Add(_oSalesReport);
            }

            while (_oSalesReports.Count > 0)
            {
                double nAmount = 0;
                List<SalesReport> oSalesReports = new List<SalesReport>();
                oSalesReports = _oSalesReports.Where(x => x.Month == _oSalesReports[0].Month).ToList();
                foreach (SalesReport oItem in oSalesReports)
                {
                    nAmount += oItem.Value;
                }
                _oSalesReport = new SalesReport();
                _oSalesReport.Value = nAmount;
                _oSalesReport.Month = oSalesReports[0].Month;
                _oSalesReport.oSalesReports = oSalesReports;
                oSalesReprotCharts.Add(_oSalesReport);
                _oSalesReports.RemoveAll(x => x.Month == oSalesReports[0].Month);

            }

            oSalesReportChart._oSalesReports = oSalesReprotCharts;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesReportChart);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public string LayerThreeMakeSQL(SalesReport oSalesReport)
        {
            _oSalesReport = new SalesReport();
            _oSalesReport = oSalesReport;
            _oSalesReport.Year = oSalesReport.Year;

            #region MktPerson~SearchingDate~BUID
            string sMktPersonIDsParam = "", SearchingDate = "", BUIDSt = "";
            if (_oSalesReport.IDs != "")
            {
                sMktPersonIDsParam = Convert.ToString(_oSalesReport.IDs.Split('~')[0]);
                SearchingDate = Convert.ToString(_oSalesReport.IDs.Split('~')[1]);
                BUIDSt = Convert.ToString(_oSalesReport.IDs.Split('~')[2]);
            }
            else
            {
                sMktPersonIDsParam = SearchingDate = BUIDSt = "";
            }
            #endregion

            #region Group By
            string GroupBy = "";
            if (_oSalesReport.GrpByQ != "")
            {
                GroupBy = Convert.ToString(_oSalesReport.GrpByQ);
            }
            else
            {
                GroupBy = "";
            }
            #endregion

            string sReturn = " ";
            string sReturn1 = "";


            if (_oSalesReport.QueryLayerThree.Contains("@DATEGROUP"))
            {
                if (oSalesReport.ViewType == 2)
                {
                    _oSalesReport.QueryLayerThree = _oSalesReport.QueryLayerTwo.Replace("@DATEGROUP", "Day(" + SearchingDate + ") as Day" + ",Month(" + SearchingDate + ") as Month" + ",Year(" + SearchingDate + ") as Year");
                    _oSalesReport.GrpByQ = _oSalesReport.GrpByQ.Replace("@DATEGROUP", "Day(" + SearchingDate + ")"+",Month(" + SearchingDate + ") " + ",Year(" + SearchingDate + ")");
                }
                else
                {
                    _oSalesReport.QueryLayerThree = _oSalesReport.QueryLayerTwo.Replace("@DATEGROUP", "Year(" + SearchingDate + ") as Year");
                    _oSalesReport.GrpByQ = _oSalesReport.GrpByQ.Replace("@DATEGROUP", "Year(" + SearchingDate + ")");
                }
            }

            sReturn1 = _oSalesReport.QueryLayerThree;

            if (oSalesReport.ErrorMessage != null && oSalesReport.ErrorMessage != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + sMktPersonIDsParam + " IN (" + oSalesReport.ErrorMessage + ") ";
            }

            //if (_oSalesReport.BUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + BUIDSt + " = " + _oSalesReport.BUID;
            //}

            if (_oSalesReport.Year > 0)
            {
                Global.TagSQL(ref sReturn);
                if (oSalesReport.ViewType == 2)
                {
                    sReturn = sReturn + "Year(" + SearchingDate + ") = " + _oSalesReport.Year + " ";
                }
                else
                {
                    sReturn = sReturn + "Year(" + SearchingDate + ") > " + (_oSalesReport.Year - 11) + " AND Year(" + SearchingDate + ") <=" + _oSalesReport.Year;
                }
            }
            if (_oSalesReport.Month > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Month(" + SearchingDate + ") = " + _oSalesReport.Month;
            }

            sReturn = sReturn1 + " " + sReturn + " ";

            //sReturn = sReturn1 + " " + sReturn + " " + _oSalesReport.GrpByQ + ",ESCDetail.MUnitID,FSC.CurrencyID,Currency.CurrencyName,MU.UnitName";
            return sReturn;

        }
        public ActionResult PrintLists(string sValue)
        {
            _oSalesReports = new List<SalesReport>();
            _oSalesReport = new SalesReport();
            int SalesReportID = Convert.ToInt32(sValue.Split('~')[3]);
            _oSalesReport = _oSalesReport.Get(SalesReportID, (int)Session[SessionInfo.currentUserID]);
            _oSalesReport.Year = Convert.ToInt32(sValue.Split('~')[0]);
            _oSalesReport.Month = Convert.ToInt32(sValue.Split('~')[1]);
            _oSalesReport.ErrorMessage = Convert.ToInt32(sValue.Split('~')[2]).ToString();
            _oSalesReport.ViewType = Convert.ToInt32(sValue.Split('~')[4]);
            string sMsg = Convert.ToString(sValue.Split('~')[6]);
            string Sql = LayerThreeMakeSQL(_oSalesReport);
            _oSalesReports = SalesReport.Gets(Sql, (int)Session[SessionInfo.currentUserID]);
            if (_oSalesReports.Count > 0)
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptSalesReportLayerThree oReport = new rptSalesReportLayerThree();               
                byte[] abytes = oReport.PrepareReport(_oSalesReports, oCompany, sMsg);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }
            
        }
        public string LayerTwoMakeSQL(SalesReport oSalesReport)
        {
            _oSalesReport = new SalesReport();
            _oSalesReport = _oSalesReport.Get(oSalesReport.SalesReportID, (int)Session[SessionInfo.currentUserID]);
            _oSalesReport.Year = oSalesReport.Year;

            #region MktPerson~SearchingDate~BUID
            string sMktPersonIDsParam = "", SearchingDate = "", BUIDSt = "";
            if (_oSalesReport.IDs != "")
            {
                sMktPersonIDsParam = Convert.ToString(_oSalesReport.IDs.Split('~')[0]);
                SearchingDate = Convert.ToString(_oSalesReport.IDs.Split('~')[1]);
                BUIDSt = Convert.ToString(_oSalesReport.IDs.Split('~')[2]);
            }
            else
            {
                sMktPersonIDsParam = SearchingDate = BUIDSt= "";
            }
            #endregion

            #region Group By
            string GroupBy = "";
            if (_oSalesReport.GrpByQ != "")
            {
                GroupBy = Convert.ToString(_oSalesReport.GrpByQ);
            }
            else
            {
                GroupBy = "";
            }
            #endregion

            string sReturn = " ";
            string sReturn1 = "";


            if (_oSalesReport.QueryLayerTwo.Contains("@DATEGROUP"))
            {
                if (oSalesReport.ViewType == 2)
                {
                    _oSalesReport.QueryLayerTwo = _oSalesReport.QueryLayerTwo.Replace("@DATEGROUP", "Month(" + SearchingDate + ") as Month" + ",Year(" + SearchingDate + ") as Year");
                    _oSalesReport.GrpByQ = _oSalesReport.GrpByQ.Replace("@DATEGROUP", "Month(" + SearchingDate + ") " + ",Year(" + SearchingDate + ")");
                }
                else
                {
                    _oSalesReport.QueryLayerTwo = _oSalesReport.QueryLayerTwo.Replace("@DATEGROUP", "Year(" + SearchingDate + ") as Year");
                    _oSalesReport.GrpByQ = _oSalesReport.GrpByQ.Replace("@DATEGROUP", "Year(" + SearchingDate + ")");
                }
            }

            sReturn1 = _oSalesReport.QueryLayerTwo;

            if (oSalesReport.ErrorMessage != null && oSalesReport.ErrorMessage != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + sMktPersonIDsParam + " IN (" + oSalesReport.ErrorMessage + ") ";
            }

            if (_oSalesReport.BUID > 0 && !string.IsNullOrEmpty(BUIDSt))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + BUIDSt + " = " + _oSalesReport.BUID;
            }

            if (_oSalesReport.Year > 0)
            {
                Global.TagSQL(ref sReturn);
                if (oSalesReport.ViewType == 2)
                {
                    sReturn = sReturn + "Year(" + SearchingDate + ") = " + _oSalesReport.Year + " ";
                }
                else
                {
                    sReturn = sReturn + "Year(" + SearchingDate + ") > " + (_oSalesReport.Year - 11) + " AND Year(" + SearchingDate + ") <=" + _oSalesReport.Year;
                }
            }
            //if (_oSalesReport.BUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " BUID = " + _oSalesReport.BUID;
            //}

            sReturn = sReturn1 + " " + sReturn + " " + _oSalesReport.GrpByQ + "";
            return sReturn;

        }
        public string MakeSQL(SalesReport oSalesReport)
        {
            DateTime sStartDate = new DateTime();
            DateTime sEndDate = DateTime.MaxValue;
            _oSalesReport = new SalesReport();
            _oSalesReport = _oSalesReport.Get(oSalesReport.SalesReportID, (int)Session[SessionInfo.currentUserID]);
            _oSalesReport.Year = oSalesReport.Year;
            _oSalesReport.Month = oSalesReport.Month;
            #region Make start & end date
            sStartDate = new DateTime(_oSalesReport.Year, _oSalesReport.Month, 1);
            sEndDate = sStartDate.AddMonths(11);
            int nGetMonth = sEndDate.Month;
            int nGetDays = DateTime.DaysInMonth(_oSalesReport.Year, nGetMonth);
            sEndDate = sEndDate.AddDays(nGetDays).AddDays(-1);
            #endregion
            #region MktPerson~SearchingDate~BUID
            string sMktPersonIDsParam = "", SearchingDate = "", BUIDSt = "";
            if (_oSalesReport.IDs != "")
            {
                sMktPersonIDsParam = Convert.ToString(_oSalesReport.IDs.Split('~')[0]);
                SearchingDate = Convert.ToString(_oSalesReport.IDs.Split('~')[1]);
                BUIDSt = Convert.ToString(_oSalesReport.IDs.Split('~')[2]);
            }
            else
            {
                sMktPersonIDsParam = SearchingDate = BUIDSt = "";
            }
            #endregion

            #region Group By
            string GroupBy = "";
            if (_oSalesReport.GrpByQ != "")
            {
                GroupBy = Convert.ToString(_oSalesReport.GrpByQ);
            }
            else
            {
                GroupBy = "";
            }
            #endregion
            string sReturn = " ";
            string sReturn1 = "";


            if ( _oSalesReport.Query.Contains("@DATEGROUP"))
            {
                if (oSalesReport.ViewType == 2)
                {
                    _oSalesReport.Query = _oSalesReport.Query.Replace("@DATEGROUP", "Month(" + SearchingDate + ") as Month" + ",Year(" + SearchingDate + ") as Year");
                    _oSalesReport.GrpByQ = _oSalesReport.GrpByQ.Replace("@DATEGROUP", "Month(" + SearchingDate + ") " + ",Year(" + SearchingDate + ")");
                }
                else
                {
                    _oSalesReport.Query = _oSalesReport.Query.Replace("@DATEGROUP", "Year(" + SearchingDate + ") as Year");
                    _oSalesReport.GrpByQ = _oSalesReport.GrpByQ.Replace("@DATEGROUP", "Year(" + SearchingDate + ")");
                }
            }

            sReturn1 = _oSalesReport.Query;

            if (oSalesReport.ErrorMessage != null && oSalesReport.ErrorMessage != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + sMktPersonIDsParam + " IN (SELECT MarketingAccountID FROM MarketingAccount WHERE GroupID IN(" + oSalesReport.ErrorMessage + "))";
            }

            if (_oSalesReport.BUID > 0)
            {
                if(!string.IsNullOrEmpty(BUIDSt))
                {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + BUIDSt + " = " + _oSalesReport.BUID;
                }
            }

            if (_oSalesReport.Year>0)
            {
                Global.TagSQL(ref sReturn);
                if (oSalesReport.ViewType == 2)
                {
                    //sReturn = sReturn + "Year(" + SearchingDate + ") = " + _oSalesReport.Year + " ";
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12)," + SearchingDate + ",106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + sStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + sEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else
                {
                    sReturn = sReturn + "Year(" + SearchingDate + ") > " + (_oSalesReport.Year - 11) + " AND Year(" + SearchingDate + ") <=" + _oSalesReport.Year;
                }
            }


            sReturn = sReturn1 + " " + sReturn + " " + _oSalesReport.GrpByQ;
            return sReturn;

        }
        [HttpPost]
        public JsonResult GetsAllMktPerson(MarketingAccount_BU oMarketingAccount)
        {
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            try
            {                    
                string sSQL = "SELECT * FROM View_MarketingAccount WHERE MarketingAccountID >0";
                if (string.IsNullOrEmpty(oMarketingAccount.Name))
                {
                    sSQL += " AND Name LIKE'%" + oMarketingAccount.Name + "%'";
                }
                if (oMarketingAccount.BUID > 0)
                {
                    sSQL += " AND MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + oMarketingAccount.BUID + "))";
                }
                sSQL += " and IsGroup=1 Order by GroupName";
                oMarketingAccounts = MarketingAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMarketingAccounts = new List<MarketingAccount>();
                oMarketingAccounts.Add(new MarketingAccount() { ErrorMessage = ex.Message });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllReportTypeQuery(SalesReport oSalesReport)
        {
            List<SalesReport> oSalesReports = new List<SalesReport>();
            string sSQL = "";
            try
            {
                string sParamName = "";
                if (oSalesReport.Name != "")
                {
                    sParamName = oSalesReport.Name;
                }
                if (oSalesReport.BUID > 0)
                {
                    sSQL = "SELECT * FROM View_SalesReport WHERE Activity = 1 AND  SalesReportID >0 AND BUID =" + oSalesReport.BUID;
                }
                else
                {
                    sSQL = "SELECT * FROM View_SalesReport WHERE Activity = 1 AND SalesReportID>0";
                }
                
                if (sParamName != null && sParamName != "")
                {
                    sSQL += " AND Name LIKE'%" + sParamName + "%'";
                }

                oSalesReports = SalesReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSalesReports = new List<SalesReport>();
                oSalesReports.Add(new SalesReport() { ErrorMessage = ex.Message });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalesReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllMktSaleTarget(MktSaleTarget oMktSaleTarget)
        {
            MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            String sSQL = ""; int nMonth = 0; int nYear = 0;
            if (oMktSaleTarget.ErrorMessage != "")
            {
                 nMonth = GetMonthNumber(Convert.ToString(oMktSaleTarget.ErrorMessage.Split(' ')[0]));
                 nYear = Convert.ToInt32(oMktSaleTarget.ErrorMessage.Split(' ')[1]);
            }                   
            try
            {
                if (oMktSaleTarget.IsMonth == false)
                {
                    if (oMktSaleTarget.ContractorID > 0)
                    {
                        sSQL = "SELECT * FROM View_MktSaleTarget AS HH WHERE HH.MarketingAccountID = " + oMktSaleTarget.MarketingAccountID + " AND ContractorID=" + oMktSaleTarget.ContractorID + " AND Year(HH.Date)=" + nYear + " Order By Date";
                    }
                    else
                    {
                        sSQL = "SELECT * FROM View_MktSaleTarget AS HH WHERE HH.MarketingAccountID = " + oMktSaleTarget.MarketingAccountID + " AND Year(HH.Date)=" + nYear + " Order By Date";
                    }

                }

                if (oMktSaleTarget.IsMonth == true)
                {
                    if (oMktSaleTarget.ContractorID > 0)
                    {
                        sSQL = "SELECT * FROM View_MktSaleTarget AS HH WHERE HH.MarketingAccountID = " + oMktSaleTarget.MarketingAccountID + " AND ContractorID=" + oMktSaleTarget.ContractorID + " AND Month(HH.Date) = " + nMonth + " AND Year(HH.Date)=" + nYear + " Order By Date";
                    }
                    else
                    {
                        sSQL = "SELECT * FROM View_MktSaleTarget AS HH WHERE HH.MarketingAccountID = " + oMktSaleTarget.MarketingAccountID + " AND Month(HH.Date) = " + nMonth + " AND Year(HH.Date)=" + nYear + " Order By Date";
                    }
                }

                _oMktSaleTargets = MktSaleTarget.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMktSaleTarget = new MktSaleTarget();
                _oMktSaleTarget.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMktSaleTargets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult MktSaleTargetSave(MktSaleTarget oMktSaleTarget)
        {
            MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            try
            {
                _oMktSaleTarget = oMktSaleTarget;
                _oMktSaleTarget = _oMktSaleTarget.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMktSaleTarget = new MktSaleTarget();
                _oMktSaleTarget.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMktSaleTarget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult MktSaleTargetDelete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                MktSaleTarget _MktSaleTarget = new MktSaleTarget();
                sFeedBackMessage = _MktSaleTarget.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public int GetMonthNumber(string sMonth)
        {
            if (sMonth == "Jan") return 1; if (sMonth == "Feb") return 2; if (sMonth == "Mar") return 3;
            if (sMonth == "Apr") return 4; if (sMonth == "May") return 5; if (sMonth == "Jun") return 6;
            if (sMonth == "Jul") return 7; if (sMonth == "Aug") return 8; if (sMonth == "Sep") return 9;
            if (sMonth == "Oct") return 10; if (sMonth == "Nov") return 11; if (sMonth == "Dec") return 12;
            return 0;
        }

        [HttpPost]
        public JsonResult Save(SalesReport oSalesReport)
        {
            _oSalesReport = new SalesReport();
            try
            {
                _oSalesReport = oSalesReport;
                _oSalesReport = _oSalesReport.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSalesReport = new SalesReport();
                _oSalesReport.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalesReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                SalesReport oSalesReport = new SalesReport();
                sFeedBackMessage = oSalesReport.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Excel
        public void MarketingProjectionExcel(string sValue,double nts)
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> oOrderReceives = new List<MktSaleTarget>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
            oMktSaleTarget.MarketingAccountID = Convert.ToInt32(sValue.Split('~')[0]);
            oMktSaleTarget.Year = Convert.ToInt32(sValue.Split('~')[1]);
            oMktSaleTarget.ViewType = Convert.ToInt32(sValue.Split('~')[2]);
            oMktSaleTarget.BUID = Convert.ToInt32(sValue.Split('~')[3]);
            oMktSaleTarget.GroupHeadName = Convert.ToString(sValue.Split('~')[4]);
            oMktSaleTarget.MKTViewType = Convert.ToInt32(sValue.Split('~')[5]);
            try
            {
                string Sql = "SELECT * FROM View_MarketingAccount Where GroupID =" + oMktSaleTarget.MarketingAccountID + " AND ISNULL(Activity,0)=1 Order By IsGroupHead DESC";
                _oMarketingAccounts = MarketingAccount.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                Sql = MakeSQL(oMktSaleTarget, 1);
                _oMktSaleTargets = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                Sql = MakeSQL(oMktSaleTarget, 2);
                oOrderReceives = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                oOrderReceives.ForEach(x =>
                {
                    if (_oMktSaleTargets.FirstOrDefault() != null && _oMktSaleTargets.FirstOrDefault().ContractorID > 0 && _oMktSaleTargets.Where(b => b.ContractorID == x.ContractorID && b.MarketingAccountID == x.MarketingAccountID).Count() > 0)
                    {
                        x.ErrorMessage = "";
                    }
                    else
                    {
                        _oMktSaleTargets.Add(x);
                    }
                });


                List<MktSaleTarget> _oTempList = new List<MktSaleTarget>();
                if (_oMktSaleTargets.Count > 0)
                {
                    foreach (MktSaleTarget oItem in _oMktSaleTargets)
                    {
                        _oTempList = new List<MktSaleTarget>();
                        _oTempList = oOrderReceives.Where(x => x.MarketingAccountID == oItem.MarketingAccountID && x.ContractorID == oItem.ContractorID && x.Month == oItem.Month).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.ReceiveQty = _oTempList.Sum(x => x.ReceiveQty);
                            oItem.Amount = _oTempList.Sum(x => x.Amount);
                        }

                    }
                }

                oExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI WHERE LCID IN (SELECT ExportLCID FROM ExportLC WHERE YEAR(OpeningDate)='" + oMktSaleTarget.Year + "')", (int)Session[SessionInfo.currentUserID]);

                #region Excel Start
                if (_oMktSaleTargets.Count > 0)
                {
                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int _nViewType = Convert.ToInt32(sValue.Split('~')[2]);
                    int nTotalCol = 0;
                    int nCount = 0;
                    double AvgDz = 0;
                    int colIndex = 2;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Marketing Projection Report");
                        sheet.Name = "Marketing Projection Report";
                        sheet.Column(colIndex++).Width = 5; //1
                        sheet.Column(colIndex++).Width = 20; //2
                        sheet.Column(colIndex++).Width = 20; //3
                        sheet.Column(colIndex++).Width = 13; //4
                        sheet.Column(colIndex++).Width = 13; //5
                        sheet.Column(colIndex++).Width = 13; //6
                        sheet.Column(colIndex++).Width = 13; //7
                        sheet.Column(colIndex++).Width = 13; //8
                        sheet.Column(colIndex++).Width = 13; //9
                        sheet.Column(colIndex++).Width = 13; //10
                        sheet.Column(colIndex++).Width = 13; //11
                        sheet.Column(colIndex++).Width = 13; //12
                        sheet.Column(colIndex++).Width = 13; //13
                        sheet.Column(colIndex++).Width = 13; //14
                        sheet.Column(colIndex++).Width = 13; //15
                        sheet.Column(colIndex++).Width = 13; //16
                        sheet.Column(colIndex++).Width = 13; //17
                        sheet.Column(colIndex++).Width = 13; //18
                        sheet.Column(colIndex++).Width = 13; //19
                        sheet.Column(colIndex++).Width = 13; //20
                        sheet.Column(colIndex++).Width = 13; //21
                        sheet.Column(colIndex++).Width = 13; //22
                        sheet.Column(colIndex++).Width = 13; //23
                        sheet.Column(colIndex++).Width = 13; //24
                        sheet.Column(colIndex++).Width = 13; //25
                        sheet.Column(colIndex++).Width = 13; //26
                        sheet.Column(colIndex++).Width = 13; //27
                        sheet.Column(colIndex++).Width = 13; //28
                        sheet.Column(colIndex++).Width = 13; //29
                        sheet.Column(colIndex++).Width = 13; //30
                        sheet.Column(colIndex++).Width = 13; //31
                        sheet.Column(colIndex++).Width = 13; //32
                        sheet.Column(colIndex++).Width = 13; //33
                        sheet.Column(colIndex++).Width = 13; //34
                        sheet.Column(colIndex++).Width = 13; //35
                        sheet.Column(colIndex++).Width = 13; //36
                        sheet.Column(colIndex++).Width = 13; //37
                        sheet.Column(colIndex++).Width = 13; //38
                        sheet.Column(colIndex++).Width = 13; //39
                        sheet.Column(colIndex++).Width = 13; //40

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 40].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 40].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Marketing Projection Report-" + oMktSaleTarget.Year; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;
                        if (_nViewType == 1)
                        {
                            sheet.Cells[rowIndex, 2, rowIndex, 40].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = "Buyer Wise Report"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        if (_nViewType == 2)
                        {
                            sheet.Cells[rowIndex, 2, rowIndex, 40].Merge = true;
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = "Vendor Wise Report"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        rowIndex = rowIndex + 2;
                        #endregion
                        #region Header 1
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3 , rowIndex, 7]; cell.Merge = true;  cell.Value = "Team Leader"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8, rowIndex, 11]; cell.Merge = true; cell.Value = "Team Member"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = oMktSaleTarget.GroupHeadName; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        rowIndex++;
                        #endregion
                        int nSL = 1;
                        foreach (MarketingAccount oItem in _oMarketingAccounts)
                        {
                            colIndex = 2;                       
                            int rowSpan = _oMarketingAccounts.Count - 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            if (oItem.IsGroupHead == true)
                            {
                               

                                cell = sheet.Cells[rowIndex, 3, rowIndex+rowSpan, 7]; cell.Merge = true; cell.Value = oItem.Name; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                               
                            }
                           
                            cell = sheet.Cells[rowIndex, 8, rowIndex, 11]; cell.Merge = true; cell.Value = oItem.Name; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            rowIndex++;
                            nSL++;

                        }
                      
                        //blank row
                        sheet.Cells[rowIndex, 2, rowIndex, 40].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                        rowIndex = rowIndex + 2;
                        colIndex = 2;
                        #region Marketing Projection Table
                        cell = sheet.Cells[rowIndex, colIndex,rowIndex+1,colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Buyer Position"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Est. Order Qty"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        string[] MonthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                        foreach (string month in MonthNames)
                        {
                            if (!string.IsNullOrEmpty(month))
                            {
                                string Month = month.Substring(0, 3).ToUpper();
                                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = Month + " " + oMktSaleTarget.Year.ToString().Substring(2, 2); cell.Style.Font.Bold = true;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                colIndex++;
                            }
                        }

                        rowIndex++;
                        colIndex = 6;

                        for (int i = 0; i < 12; i++)
                        {
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Projection Qty"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Achive Qty"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        }
                        rowIndex++;

                        List<MktSaleTarget> _distMktSaleTarget = new List<MktSaleTarget>();
                        _distMktSaleTarget = _oMktSaleTargets.GroupBy(x => x.ContractorID).Select(g => g.First()).ToList();
                        _distMktSaleTarget = _distMktSaleTarget.OrderBy(x => x.BuyerName).ToList();

                        #region DATA
                        string tempGroupName = "";
                        int allocationValue = 0;

                        nSL = 1;
                        //cell = sheet.Cells[rowIndex, 2, rowIndex, 41]; cell.Merge = true; cell.Value = "Buyer"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //rowIndex++;

                        foreach (MktSaleTarget oItem in _distMktSaleTarget)
                        {
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerPosition; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderQty; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            for (int i = 1; i <= 12; i++)
                            {
                                double nValue = 0;
                                double nReceiveQty = 0;
                                double nAmount = 0;
                                string BPercent = "";
                                nValue = _oMktSaleTargets.Where(x => x.Month == i && x.ContractorID == oItem.ContractorID).Sum(x => x.Value);
                                nReceiveQty = _oMktSaleTargets.Where(x => x.Month == i && x.ContractorID == oItem.ContractorID).Sum(x => x.ReceiveQty);
                                nAmount = _oMktSaleTargets.Where(x => x.Month == i && x.ContractorID == oItem.ContractorID).Sum(x => x.Amount);
     
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nValue; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nReceiveQty; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }

                            nSL++;
                            rowIndex++;
                        }

                        #region TOTAL
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        colIndex = 5;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = _distMktSaleTarget.Sum(x => x.OrderQty); cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        for (int i = 1; i <= 12; i++)
                        {
                            double nValue = _oMktSaleTargets.Where(x => x.Month == i).Sum(x => x.Value);
                            double nReceiveQty = _oMktSaleTargets.Where(x => x.Month == i).Sum(x => x.ReceiveQty);
                            double nAmount = _oMktSaleTargets.Where(x => x.Month == i).Sum(x => x.Amount);

                            if (nValue > 0)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = nValue; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            else
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "-"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }

                            if (nReceiveQty > 0)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = nReceiveQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            else
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "-"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            if (nAmount > 0)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = nAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            else
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "-"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                        
                        }
                        rowIndex++;
                        #endregion

                        #endregion


                        #endregion

                        #region Summery
                        rowIndex = rowIndex + 2;
                        colIndex = 3;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex+=8]; cell.Merge = true; cell.Value = "Summery"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; 
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;

                        colIndex = 3;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Month"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "No Of PI"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 4]; cell.Merge = true; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex++;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;

                        var data = oExportPIs.GroupBy(x => new { x.OpeningDate.Month, x.OpeningDate.Year }, (key, grp) => new
                        {
                            Month = key.Month,
                            Year = key.Year,
                            NoOfPI = grp.ToList().Count(),
                            PIsNo = string.Join(", ", grp.ToList().Select(y => y.PINo)),
                            Qty = grp.ToList().Sum(y => y.Qty),
                            Amount = grp.ToList().Sum(y => y.Amount)
                        }).OrderBy(c => c.Year).ThenBy(c => c.Month);

                        foreach (var oItem in data)
                        {
                            colIndex = 3;
                            DateTime dDate = new DateTime(oItem.Year, oItem.Month, 1);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dDate.ToString("MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NoOfPI; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 4]; cell.Merge = true; cell.Value = oItem.PIsNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Row(rowIndex).Height = MeasureTextHeight(oItem.PIsNo, cell.Style.Font, 70);
                            colIndex++;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                        }

                        colIndex = 3;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.Sum(x => x.NoOfPI); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 4]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex++;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.Sum(x => x.Amount); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;

                        #endregion


                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=RPT_MarketingProjection.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                oMktSaleTarget = new MktSaleTarget();
                oMktSaleTarget.ErrorMessage = ex.Message;
                _oMktSaleTargets.Add(oMktSaleTarget);
            }
        }

        public double MeasureTextHeight(string text, ExcelFont font, int width)
        {
            if (string.IsNullOrEmpty(text)) return 0.0;
            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            var pixelWidth = Convert.ToInt32(width * 6);
            var drawingFont = new Font(font.Name, font.Size);
            var size = graphics.MeasureString(text, drawingFont, pixelWidth);

            return Math.Min(Convert.ToDouble(size.Height) * 50 / 96, 409);
        }
        #endregion

        [HttpGet]
        public ActionResult MKTProjectionReport(string sValue, double nts)
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> oOrderReceives = new List<MktSaleTarget>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
            oMktSaleTarget.MarketingAccountID = Convert.ToInt32(sValue.Split('~')[0]);
            oMktSaleTarget.Year = Convert.ToInt32(sValue.Split('~')[1]);
            oMktSaleTarget.ViewType = Convert.ToInt32(sValue.Split('~')[2]);
            oMktSaleTarget.BUID = Convert.ToInt32(sValue.Split('~')[3]);
            oMktSaleTarget.GroupHeadName = Convert.ToString(sValue.Split('~')[4]);
            oMktSaleTarget.MKTViewType = Convert.ToInt32(sValue.Split('~')[5]);

            try
            {
                string Sql = "SELECT * FROM View_MarketingAccount Where GroupID =" + oMktSaleTarget.MarketingAccountID + " AND ISNULL(Activity,0)=1 Order By IsGroupHead DESC";
                _oMarketingAccounts = MarketingAccount.Gets(Sql, (int)Session[SessionInfo.currentUserID]);
             
                 Sql = MakeSQL(oMktSaleTarget, 1);
                _oMktSaleTargets = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                Sql = MakeSQL(oMktSaleTarget, 2);
                oOrderReceives = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                oOrderReceives.ForEach(x =>
                {
                    if (_oMktSaleTargets.FirstOrDefault() != null && _oMktSaleTargets.FirstOrDefault().ContractorID > 0 && _oMktSaleTargets.Where(b => b.ContractorID == x.ContractorID && b.MarketingAccountID == x.MarketingAccountID).Count() > 0)
                    {
                        x.ErrorMessage = "";
                    }
                    else
                    {
                        _oMktSaleTargets.Add(x);
                    }
                });
             
                List<MktSaleTarget> _oTempList = new List<MktSaleTarget>();
                if (_oMktSaleTargets.Count > 0)
                {
                    foreach (MktSaleTarget oItem in _oMktSaleTargets)
                    {
                        _oTempList = new List<MktSaleTarget>();
                        _oTempList = oOrderReceives.Where(x => x.MarketingAccountID == oItem.MarketingAccountID && x.ContractorID == oItem.ContractorID && x.Month == oItem.Month).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.ReceiveQty = _oTempList.Sum(x => x.ReceiveQty);
                            oItem.Amount = _oTempList.Sum(x => x.Amount);
                        }
                  
                    }
                }
                oExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI WHERE LCID IN (SELECT ExportLCID FROM ExportLC WHERE YEAR(OpeningDate)='" + oMktSaleTarget.Year + "')", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMktSaleTarget = new MktSaleTarget();
                oMktSaleTarget.ErrorMessage = ex.Message;
                _oMktSaleTargets.Add(oMktSaleTarget);
            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oMktSaleTarget.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptMktProjectionsReport oReport = new rptMktProjectionsReport();
            int ViewType = Convert.ToInt32(sValue.Split('~')[2]);
            string printStatus = "";
            if (ViewType == 1)
            {
                printStatus = "Buyer Wise Report";
            }

            if(ViewType == 2){
                printStatus = "Vendor Wise Report";
            }
            if (ViewType==0)
            {
                printStatus = "";
            }
            byte[] abytes = oReport.PrepareReport(_oMarketingAccounts, _oMktSaleTargets, oMktSaleTarget.GroupHeadName, oCompany, oMktSaleTarget.ViewType, oMktSaleTarget.Year, printStatus, oExportPIs);
            return File(abytes, "application/pdf");
        }
        public ActionResult MarketingProjectionSummary(string sValue, double nts)
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> oOrderReceives = new List<MktSaleTarget>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
            oMktSaleTarget.ErrorMessage = Convert.ToString(sValue.Split('~')[0]);
            oMktSaleTarget.Year = Convert.ToInt32(sValue.Split('~')[1]);
            oMktSaleTarget.BUID = Convert.ToInt32(sValue.Split('~')[2]);
            string Sql = "";
            try
            {
                //string Sql = "SELECT * FROM View_MarketingAccount Where GroupID =" + oMktSaleTarget.MarketingAccountID + " Order By IsGroupHead DESC";
                //_oMarketingAccounts = MarketingAccount.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                Sql = MakeSQLSummary(oMktSaleTarget, 1);
                _oMktSaleTargets = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                Sql = MakeSQLSummary(oMktSaleTarget, 2);
                oOrderReceives = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                oOrderReceives.ForEach(x =>
                {
                    if (_oMktSaleTargets.FirstOrDefault() != null && _oMktSaleTargets.FirstOrDefault().ContractorID > 0 && _oMktSaleTargets.Where(b => b.ContractorID == x.ContractorID && b.MarketingAccountID == x.MarketingAccountID).Count() > 0)
                    {
                        x.ErrorMessage = "";
                    }
                    else
                    {
                        _oMktSaleTargets.Add(x);
                    }
                });

                List<MktSaleTarget> _oTempList = new List<MktSaleTarget>();
                if (_oMktSaleTargets.Count > 0)
                {
                    foreach (MktSaleTarget oItem in _oMktSaleTargets)
                    {
                        _oTempList = new List<MktSaleTarget>();
                        _oTempList = oOrderReceives.Where(x => x.MarketingAccountID == oItem.MarketingAccountID && x.ContractorID == oItem.ContractorID && x.Month == oItem.Month).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.ReceiveQty = _oTempList.Sum(x => x.ReceiveQty);
                            oItem.Amount = _oTempList.Sum(x => x.Amount);
                        }

                    }
                }
                
                oExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI WHERE LCID IN (SELECT ExportLCID FROM ExportLC WHERE YEAR(OpeningDate)='" + oMktSaleTarget.Year + "')", (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oMktSaleTarget = new MktSaleTarget();
                oMktSaleTarget.ErrorMessage = ex.Message;
                _oMktSaleTargets.Add(oMktSaleTarget);
            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oMktSaleTarget.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptMktProjectionSummary oReport = new rptMktProjectionSummary();
            int ViewType = Convert.ToInt32(sValue.Split('~')[2]);
            string printStatus = "";
            if (ViewType == 1)
            {
                printStatus = "Buyer Wise Report";
            }

            if (ViewType == 2)
            {
                printStatus = "Vendor Wise Report";
            }
            if (ViewType == 0)
            {
                printStatus = "";
            }
            byte[] abytes = oReport.PrepareReport(_oMktSaleTargets, oMktSaleTarget.GroupHeadName, oCompany, oMktSaleTarget.ViewType, oMktSaleTarget.Year, printStatus, oExportPIs);
            return File(abytes, "application/pdf");
        }
        public string MakeSQL(MktSaleTarget oMktSaleTarget, int Value)
        {

            MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            _oMktSaleTarget.Year = oMktSaleTarget.Year;
            _oMktSaleTarget.MarketingAccountID = oMktSaleTarget.MarketingAccountID;
            _oMktSaleTarget.BUID = oMktSaleTarget.BUID;
            int nValue = Value;
            _oMktSaleTarget.MKTViewType = oMktSaleTarget.MKTViewType;


            string sReturn = "";

            if (nValue == 1)
            {
              
                sReturn = "SELECT HH.ContractorID, HH.MarketingAccountID,CC.Name AS BuyerName,MAX(HH.Date) AS Date,HH.BuyerPosition,HH.BPercent,max(HH.Value) AS Value,max(HH.OrderQty) AS OrderQty,Month(HH.Date) AS Month,Year(HH.Date) AS Year FROM MktSaleTarget  AS HH  "
                + " LEFT JOIN Contractor AS CC ON CC.ContractorID = HH.ContractorID   "
                + " LEFT JOIN MarketingAccount AS MA ON MA.MarketingAccountID = HH.MarketingAccountID "
                + " WHERE HH.OrderType IN("+  ((int)EnumFabricRequestType.Bulk).ToString() +") and isnull(HH.MarketingAccountID,0)>0  ";
                
                #region Marketing Head
                if (_oMktSaleTarget.MarketingAccountID != null)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.MarketingAccountID IN(" + _oMktSaleTarget.MarketingAccountID + ")";
                }
                #endregion
                if (oMktSaleTarget.ViewType >0)
                {
                    #region ContractorType   
                    Global.TagSQL(ref sReturn);             
                    if (oMktSaleTarget.ViewType == 2)
                    {                      
                        sReturn = sReturn + " HH.ContractorID in (Select ContractorID from ContractorType where ContractorType in (" + (int)EnumContractorType.Agent + "))";                    
                    }
                    else
                    {
                        sReturn = sReturn + " HH.ContractorID in (Select ContractorID from ContractorType where ContractorType not in (" + (int)EnumContractorType.Agent + "))";
                    }

                    #endregion
                }
              
                #region YEAR
                if (_oMktSaleTarget.Year != null)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Year(Date)=" + _oMktSaleTarget.Year;
                }
                #endregion

            }

            if (nValue == 2)
            {
              //  sReturn = "SELECT FCD.BuyerID as ContractorID,MktGroup.GroupID as MarketingAccountID,CC.Name AS BuyerName,Sum(FSCD.Qty) AS ReceiveQty,SUM(FSCD.Qty*FSCD.UnitPrice) AS Amount,MONTH(FCD.SCDate) AS Month,Year(FCD.SCDate) AS Year FROM FabricSalesContractDetail  AS FSCD   "
              //+ " LEFT JOIN FabricSalesContract AS FCD ON FCD.FabricSalesContractID = FSCD.FabricSalesContractID  "
              //+ " LEFT JOIN Contractor AS CC ON CC.ContractorID = FCD.BuyerID  "
              //+ " LEFT JOIN MarketingAccount AS MktGroup ON MktGroup.MarketingAccountID = FCD.MktAccountID   "
              //+ " WHERE FCD.OrderType IN(" + ((int)EnumFabricRequestType.Bulk).ToString() + ") and isnull(FCD.MktAccountID,0)>0  ";

                sReturn = "SELECT EE.BuyerID AS ContractorID,CC.Name AS BuyerName, MktGroup.GroupID AS MarketingAccountID, Sum(EE.Qty) AS ReceiveQty, SUM(EE.Amount) AS Amount, MONTH(ELCM.[Date]) AS [Month],Year(ELCM.[Date]) AS [Year] FROM ExportPI AS EE "
             + "LEFT JOIN MarketingAccount AS MktGroup ON MktGroup.MarketingAccountID = EE.MKTEmpID "
             + "LEFT JOIN Contractor AS CC ON CC.ContractorID = EE.BuyerID "
             + "LEFT JOIN ExportPILCMapping AS ELCM ON ELCM.ExportPIID = EE.ExportPIID "
             + "WHERE ISNULL(MktGroup.MarketingAccountID,0)>0 AND ISNULL(ELCM.Activity,0) = 1  ";

                #region Marketing Head
                if (_oMktSaleTarget.MarketingAccountID >0)
                {
                    Global.TagSQL(ref sReturn);
                    if (_oMktSaleTarget.MKTViewType == 1)
                    {
                        sReturn = sReturn + " MktGroup.GroupID=" + _oMktSaleTarget.MarketingAccountID;
                    }

                    else if (_oMktSaleTarget.MKTViewType == 2)
                    {
                        sReturn = sReturn + " MktGroup.MarketingAccountID=" + _oMktSaleTarget.MarketingAccountID;
                    }                                     
                }
                #endregion

                if (oMktSaleTarget.ViewType >0)
                {
                    #region ContractorType
                    Global.TagSQL(ref sReturn);
                    if (oMktSaleTarget.ViewType == 2)
                    {                       
                        sReturn = sReturn + " CC.ContractorID IN (Select ContractorID from ContractorType where ContractorType in (" + (int)EnumContractorType.Agent + "))";
                    }
                    else
                    {                    
                        sReturn = sReturn + " CC.ContractorID IN (Select ContractorID from ContractorType where ContractorType not in (" + (int)EnumContractorType.Agent + "))";
                    }
                    #endregion
                }

                #region YEAR
                if (_oMktSaleTarget.Year>0)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " Year(SCDate)=" + _oMktSaleTarget.Year;
                    sReturn = sReturn + " Year(ELCM.[Date])=" + _oMktSaleTarget.Year;
                }
                #endregion
            }

            if (nValue == 1)
            {
                return sReturn + " group by HH.ContractorID,HH.MarketingAccountID,CC.Name,HH.BuyerPosition,HH.BPercent,Month(HH.Date),Year(HH.Date),HH.Date,MA.GroupID  ";
            }
            else
            {
                //return sReturn + " group by FCD.BuyerID,MktGroup.GroupID,CC.Name,MONTH(FCD.SCDate),Year(FCD.SCDate)";
                return sReturn + " GROUP BY EE.BuyerID,MktGroup.GroupID,CC.Name,MONTH(ELCM.[Date]),Year(ELCM.[Date])";
            }
        }
        public string MakeSQLSummary(MktSaleTarget oMktSaleTarget, int Value)
        {

            MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            _oMktSaleTarget.Year = oMktSaleTarget.Year;
            _oMktSaleTarget.BUID = oMktSaleTarget.BUID;
            _oMktSaleTarget.ErrorMessage = oMktSaleTarget.ErrorMessage;
            int nValue = Value;


            string sReturn = "";

            if (nValue == 1)
            {

                sReturn = "SELECT HH.ContractorID,MA.Name,MA.GroupName AS GroupHeadName, HH.MarketingAccountID,CC.Name AS BuyerName,MAX(HH.Date) AS Date,HH.BuyerPosition,HH.BPercent,max(HH.Value) AS Value,max(HH.OrderQty) AS OrderQty,Month(HH.Date) AS Month,Year(HH.Date) AS Year FROM View_MktSaleTarget  AS HH  "
                + " LEFT JOIN Contractor AS CC ON CC.ContractorID = HH.ContractorID   "
                + " LEFT JOIN View_MarketingAccount AS MA ON MA.MarketingAccountID = HH.MarketingAccountID "
                + " WHERE HH.OrderType IN(2,3) and isnull(HH.MarketingAccountID,0)>0  ";

                #region Marketing Head
                if (_oMktSaleTarget.ErrorMessage != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.MarketingAccountID IN(" + _oMktSaleTarget.ErrorMessage + ")";
                }
                #endregion
                if (oMktSaleTarget.ViewType > 0)
                {
                    #region ContractorType
                    Global.TagSQL(ref sReturn);
                    if (oMktSaleTarget.ViewType == 2)
                    {
                        sReturn = sReturn + " HH.ContractorID in (Select ContractorID from ContractorType where ContractorType in (" + (int)EnumContractorType.Agent + "))";
                    }
                    else
                    {
                        sReturn = sReturn + " HH.ContractorID in (Select ContractorID from ContractorType where ContractorType not in (" + (int)EnumContractorType.Agent + "))";
                    }

                    #endregion
                }

                #region YEAR
                if (_oMktSaleTarget.Year != null)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Year(Date)=" + _oMktSaleTarget.Year;
                }
                #endregion

            }

            if (nValue == 2)
            {
              //  sReturn = "SELECT FCD.BuyerID as ContractorID,MktGroup.GroupID as MarketingAccountID,CC.Name AS BuyerName,Sum(FSCD.Qty) AS ReceiveQty,MONTH(FCD.SCDate) AS Month,Year(FCD.SCDate) AS Year FROM FabricSalesContractDetail  AS FSCD   "
              //+ " LEFT JOIN FabricSalesContract AS FCD ON FCD.FabricSalesContractID = FSCD.FabricSalesContractID  "
              //+ " LEFT JOIN Contractor AS CC ON CC.ContractorID = FCD.BuyerID  "
              //+ " LEFT JOIN MarketingAccount AS MktGroup ON MktGroup.MarketingAccountID = FCD.MktAccountID   "
              //+ " WHERE FCD.OrderType IN(" + ((int)EnumFabricRequestType.Sample).ToString() + "," + ((int)EnumFabricRequestType.Bulk).ToString() + "," + ((int)EnumFabricRequestType.SampleFOC).ToString() + ") and isnull(FCD.MktAccountID,0)>0  ";

                sReturn = "SELECT EE.BuyerID AS ContractorID,CC.Name AS BuyerName, MktGroup.GroupID AS MarketingAccountID,MktGroup.GroupName AS GroupHeadName, Sum(EE.Qty) AS ReceiveQty, SUM(EE.Amount) AS Amount, MONTH(ELCM.[Date]) AS [Month],Year(ELCM.[Date]) AS [Year] FROM ExportPI AS EE "
              + "LEFT JOIN View_MarketingAccount AS MktGroup ON MktGroup.MarketingAccountID = EE.MKTEmpID "
              + "LEFT JOIN Contractor AS CC ON CC.ContractorID = EE.BuyerID "
              + "LEFT JOIN ExportPILCMapping AS ELCM ON ELCM.ExportPIID = EE.ExportPIID "
              + " WHERE ISNULL(MktGroup.MarketingAccountID,0)>0 AND ISNULL(ELCM.Activity,0) = 1 ";

                #region Marketing Head
                if (_oMktSaleTarget.ErrorMessage!="")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktGroup.GroupID IN(" + _oMktSaleTarget.ErrorMessage+") ";
                }
                #endregion
                if (oMktSaleTarget.ViewType > 0)
                {
                    #region ContractorType

                    Global.TagSQL(ref sReturn);
                    if (oMktSaleTarget.ViewType == 2)
                    {
                        sReturn = sReturn + " CC.ContractorID in (Select ContractorID from ContractorType where ContractorType in (" + (int)EnumContractorType.Agent + "))";
                    }
                    else
                    {
                        sReturn = sReturn + " CC.ContractorID in (Select ContractorID from ContractorType where ContractorType not in (" + (int)EnumContractorType.Agent + "))";
                    }


                    #endregion
                }
                #region YEAR
                if (_oMktSaleTarget.Year > 0)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " Year(SCDate)=" + _oMktSaleTarget.Year;
                    sReturn = sReturn + " Year(ELCM.[Date])=" + _oMktSaleTarget.Year;
                }
                #endregion

            }

            if (nValue == 1)
            {
                return sReturn + " group by HH.ContractorID,HH.MarketingAccountID,CC.Name,HH.BuyerPosition,HH.BPercent,Month(HH.Date),Year(HH.Date),HH.Date,MA.GroupID,MA.GroupName,MA.Name ";
            }

            else
            {
                //return sReturn + " group by FCD.BuyerID,MktGroup.GroupID,CC.Name,MONTH(FCD.SCDate),Year(FCD.SCDate) ";
                return sReturn + " group by EE.BuyerID,MktGroup.GroupID,MktGroup.GroupName,CC.Name,MONTH(ELCM.[Date]),Year(ELCM.[Date])";
            }

        }
        public string MakeSQLMktPersonWiseReport(MktSaleTarget oMktSaleTarget, int Value)
        {

            MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            _oMktSaleTarget.Year = oMktSaleTarget.Year;
            _oMktSaleTarget.Month = oMktSaleTarget.Month;
            _oMktSaleTarget.BUID = oMktSaleTarget.BUID;
            _oMktSaleTarget.ErrorMessage = oMktSaleTarget.ErrorMessage;
            _oMktSaleTarget.MKTViewType = oMktSaleTarget.MKTViewType;
            int nValue = Value;


            string sReturn = "";

            if (nValue == 1)
            {

                sReturn = "SELECT HH.ContractorID,MA.Name, HH.MarketingAccountID,HH.ProductID,Product.ProductName,FabricProcess.Name AS WeaveTypeName,FP.Name AS FinishTypeName,HH.WeaveType,HH.FinishType,HH.Construction,CC.Name AS BuyerName,MAX(HH.Date) AS Date,HH.BuyerPosition,HH.BPercent,max(HH.Value) AS Value,max(HH.OrderQty) AS OrderQty,Month(HH.Date) AS Month,Year(HH.Date) AS Year FROM View_MktSaleTarget  AS HH  "
                + " LEFT JOIN Contractor AS CC ON CC.ContractorID = HH.ContractorID   "
                + " LEFT JOIN MarketingAccount AS MA ON MA.MarketingAccountID = HH.MarketingAccountID "
                + " LEFT JOIN Product ON Product.ProductID = HH.ProductID "
                + " LEFT JOIN FabricProcess ON FabricProcess.FabricProcessID = HH.WeaveType "
                + " LEFT JOIN FabricProcess AS FP ON FP.FabricProcessID = HH.FinishType "
                + " WHERE HH.OrderType IN(2,3) and isnull(HH.MarketingAccountID,0)>0  ";

                #region Marketing Head
                if (_oMktSaleTarget.ErrorMessage != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.MarketingAccountID IN(" + _oMktSaleTarget.ErrorMessage + ")";
                }
                #endregion
                if (oMktSaleTarget.ViewType > 0)
                {
                    #region ContractorType
                    Global.TagSQL(ref sReturn);
                    if (oMktSaleTarget.ViewType == 2)
                    {
                        sReturn = sReturn + " HH.ContractorID in (Select ContractorID from ContractorType where ContractorType in (" + (int)EnumContractorType.Agent + "))";
                    }
                    else
                    {
                        sReturn = sReturn + " HH.ContractorID in (Select ContractorID from ContractorType where ContractorType not in (" + (int)EnumContractorType.Agent + "))";
                    }

                    #endregion
                }

                #region YEAR
                if (_oMktSaleTarget.Year != null)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Year(Date)=" + _oMktSaleTarget.Year;
                }
                #endregion
                #region Month
                if (_oMktSaleTarget.Month >0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Month(Date)=" + _oMktSaleTarget.Month;
                }
                #endregion

            }

            if (nValue == 2)
            {
              //  sReturn = "SELECT FCD.BuyerID as ContractorID,MktGroup.GroupID as MarketingAccountID,CC.Name AS BuyerName,Sum(FSCD.Qty) AS ReceiveQty,MONTH(FCD.SCDate) AS Month,Year(FCD.SCDate) AS Year FROM FabricSalesContractDetail  AS FSCD   "
              //+ " LEFT JOIN FabricSalesContract AS FCD ON FCD.FabricSalesContractID = FSCD.FabricSalesContractID  "
              //+ " LEFT JOIN Contractor AS CC ON CC.ContractorID = FCD.BuyerID  "
              //+ " LEFT JOIN MarketingAccount AS MktGroup ON MktGroup.MarketingAccountID = FCD.MktAccountID   "
              //+ " WHERE FCD.OrderType IN(" + ((int)EnumFabricRequestType.Sample).ToString() + "," + ((int)EnumFabricRequestType.Bulk).ToString() + "," + ((int)EnumFabricRequestType.SampleFOC).ToString() + ") and isnull(FCD.MktAccountID,0)>0  ";

                sReturn = "SELECT EE.BuyerID AS ContractorID,CC.Name AS BuyerName, MktGroup.GroupID AS MarketingAccountID, Sum(EE.Qty) AS ReceiveQty, MONTH(ELCM.[Date]) AS [Month],Year(ELCM.[Date]) AS [Year] FROM ExportPI AS EE "
              + "LEFT JOIN MarketingAccount AS MktGroup ON MktGroup.MarketingAccountID = EE.MKTEmpID "
              + "LEFT JOIN Contractor AS CC ON CC.ContractorID = EE.BuyerID "
              + "LEFT JOIN ExportPILCMapping AS ELCM ON ELCM.ExportPIID = EE.ExportPIID "
              + "WHERE ISNULL(MktGroup.MarketingAccountID,0)>0 AND ISNULL(ELCM.Activity,0) = 1  ";

                #region Marketing Head
                if (_oMktSaleTarget.ErrorMessage != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktGroup.MarketingAccountID IN(" + _oMktSaleTarget.ErrorMessage + ") ";
                    //if (_oMktSaleTarget.MKTViewType == 1)
                    //{
                    //    sReturn = sReturn + " MktGroup.GroupID IN(" + _oMktSaleTarget.ErrorMessage + ") ";
                      
                    //}
                    //else if (_oMktSaleTarget.MKTViewType == 2)
                    //{
                    //      sReturn = sReturn + " MktGroup.MarketingAccountID IN(" + _oMktSaleTarget.ErrorMessage + ") ";
                    //}
                }
                #endregion

                if (oMktSaleTarget.ViewType > 0)
                {
                    #region ContractorType

                    Global.TagSQL(ref sReturn);
                    if (oMktSaleTarget.ViewType == 2)
                    {
                        sReturn = sReturn + " CC.ContractorID in (Select ContractorID from ContractorType where ContractorType in (" + (int)EnumContractorType.Agent + "))";
                    }
                    else
                    {
                        sReturn = sReturn + " CC.ContractorID in (Select ContractorID from ContractorType where ContractorType not in (" + (int)EnumContractorType.Agent + "))";
                    }


                    #endregion
                }
                #region YEAR
                if (_oMktSaleTarget.Year > 0)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " Year(SCDate)=" + _oMktSaleTarget.Year;
                    sReturn = sReturn + " Year(ELCM.[Date])=" + _oMktSaleTarget.Year;
                }
                #endregion
                #region MONTH
                if (_oMktSaleTarget.Month > 0)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " Month(SCDate)=" + _oMktSaleTarget.Month;
                    sReturn = sReturn + " Month(ELCM.[Date])=" + _oMktSaleTarget.Month;

                }
                #endregion

            }

            if (nValue == 1)
            {
                return sReturn + " group by HH.ContractorID,HH.MarketingAccountID,CC.Name,HH.BuyerPosition,HH.BPercent,Month(HH.Date),Year(HH.Date),HH.Date,MA.GroupID,MA.Name,HH.ProductID,HH.WeaveType,HH.FinishType,HH.Construction,Product.ProductName,FabricProcess.Name,FP.Name";
            }
            else
            {
                //return sReturn + " group by FCD.BuyerID,MktGroup.GroupID,CC.Name,MONTH(FCD.SCDate),Year(FCD.SCDate) ";
                return sReturn + " GROUP BY EE.BuyerID,MktGroup.GroupID,CC.Name,MONTH(ELCM.[Date]),Year(ELCM.[Date])";
            }

        }
        [HttpPost]
        public JsonResult SearchMarketingProjection(MktSaleTarget oMktSaleTarget)
        {
            MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            List<MktSaleTarget> _oOrderReceives = new List<MktSaleTarget>();
            try
            {
                string Sql = MakeSQL(oMktSaleTarget, 1);
                _oMktSaleTargets = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                Sql = MakeSQL(oMktSaleTarget, 2);
                _oOrderReceives = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                _oOrderReceives.ForEach(x =>
                {
                    if (_oMktSaleTargets.FirstOrDefault() != null && _oMktSaleTargets.FirstOrDefault().ContractorID > 0 && _oMktSaleTargets.Where(b => b.ContractorID == x.ContractorID && b.MarketingAccountID == x.MarketingAccountID).Count() > 0)
                    {
                        x.ErrorMessage = "";
                    }
                    else
                    {
                        _oMktSaleTargets.Add(x);
                    }
                });
              
            }
            catch (Exception ex)
            {
                _oMktSaleTarget = new MktSaleTarget();
                _oMktSaleTarget.ErrorMessage = ex.Message;
                _oMktSaleTargets.Add(oMktSaleTarget);
            }

            List<MktSaleTarget> _oTempList = new List<MktSaleTarget>();
            if (_oMktSaleTargets.Count > 0)
            {
                foreach (MktSaleTarget oItem in _oMktSaleTargets)
                {
                    _oTempList = new List<MktSaleTarget>();
                    _oTempList = _oOrderReceives.Where(x =>  x.ContractorID == oItem.ContractorID && x.Month == oItem.Month).ToList();
                    if (_oTempList.Count > 0)
                    {
                        oItem.ReceiveQty = _oTempList.Sum(x => x.ReceiveQty);
                        oItem.Amount = _oTempList.Sum(x => x.Amount);
                    }
                    //_oMktSaleTargets.Add(oItem);
                }
            }

            List<MktSaleTarget> oMktSaleTarget_Distinct = new List<MktSaleTarget>();
            oMktSaleTarget_Distinct = _oMktSaleTargets.GroupBy(x => x.ContractorID).Select(g => g.First()).ToList();
            oMktSaleTarget_Distinct = oMktSaleTarget_Distinct.OrderBy(x => x.BuyerName).ToList();

            var tuple = new Tuple<List<MktSaleTarget>, List<MktSaleTarget>>(new List<MktSaleTarget>(), new List<MktSaleTarget>());
            tuple = new Tuple<List<MktSaleTarget>, List<MktSaleTarget>>(_oMktSaleTargets, oMktSaleTarget_Distinct);
            var jsonResult = Json(tuple, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


      
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public ActionResult SalesReportPreview(string sValue,double nts)
        {
            _oSalesReports = new List<SalesReport>();
            _oSalesReport = new SalesReport();
            int SalesReportID = Convert.ToInt32(sValue.Split('~')[1]);
            SalesReport oSalesReportPrint = new SalesReport();
            oSalesReportPrint = oSalesReportPrint.Get(SalesReportID, (int)Session[SessionInfo.currentUserID]);
            oSalesReportPrint.ViewType = Convert.ToInt32(sValue.Split('~')[3]);
            oSalesReportPrint.Year = Convert.ToInt32(sValue.Split('~')[0]);
            List<SalesReport> RevSalesReports  = new List<SalesReport>();
            SalesReport TempSalesReport1 = new SalesReport();
            SalesReport RcvSalesReportParam = new SalesReport();
            List<MktSaleTarget> MktSaleTargetList = new List<MktSaleTarget>();           
            IsDouble = Convert.ToInt32(sValue.Split('~')[4]);
            _oSalesReport.ErrorMessage = ""; _oSalesReport.ViewType = 0;
             
            if (string.IsNullOrEmpty(sValue))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {
                _oSalesReport.Year = Convert.ToInt32(sValue.Split('~')[0]);
                _oSalesReport.SalesReportID = Convert.ToInt32(sValue.Split('~')[1]);
                _oSalesReport.ErrorMessage = Convert.ToString(sValue.Split('~')[2]);
                _oSalesReport.ViewType = Convert.ToInt32(sValue.Split('~')[3]);
                _oSalesReport.Month = Convert.ToInt32(sValue.Split('~')[5]);   
                string Sql = MakeSQL(_oSalesReport);               
                _oSalesReports = SalesReport.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                #region Get Receive Data     
                TempSalesReport1 = _oSalesReport.Get(_oSalesReport.SalesReportID, (int)Session[SessionInfo.currentUserID]);
                RcvSalesReportParam = RcvSalesReportParam.Get(TempSalesReport1.ParentID, (int)Session[SessionInfo.currentUserID]);
                RcvSalesReportParam.ErrorMessage = Convert.ToString(sValue.Split('~')[2]);
                RcvSalesReportParam.ViewType = Convert.ToInt32(sValue.Split('~')[3]);
                RcvSalesReportParam.Year = Convert.ToInt32(sValue.Split('~')[0]);
                string MarketingAccountIDs = Convert.ToString(sValue.Split('~')[2]);
               
                if (IsDouble == 2)
                {
                    string rcvSQL = MakeSQL(RcvSalesReportParam);
                    RevSalesReports = SalesReport.Gets(rcvSQL, (int)Session[SessionInfo.currentUserID]);
                    
                    #region No Of Dispo
                    string dispoSQL = "";
                    if(MarketingAccountIDs  != null && MarketingAccountIDs != "")
                    {
                         string Part1mktTarget = "", Part2mktTarget = "", Part3mktTarget = "";
                         Part1mktTarget = Convert.ToString(TempSalesReport1.DispoTargetQuery.Split('~')[0]);
                         Part2mktTarget = Convert.ToString(TempSalesReport1.DispoTargetQuery.Split('~')[1]);
                         Part3mktTarget = Convert.ToString(TempSalesReport1.DispoTargetQuery.Split('~')[2]);
                         //dispoSQL = "SELECT * FROM View_MktSaleTarget AS HH WHERE HH.MarketingAccountID IN (" + RcvSalesReportParam.ErrorMessage + ")";

                         dispoSQL = Part1mktTarget + " = " + RcvSalesReportParam.Year + " AND  "+Part2mktTarget+" IN(" + MarketingAccountIDs + ")" +Part3mktTarget;
                        
                    }
                    else
                    {
                        dispoSQL = "SELECT HH.MarketingAccountID, MAX(hh.value) AS Value, Year(HH.Date) AS YEAR  from MktSaleTarget AS HH WHERE Year(HH.Date) = " + RcvSalesReportParam.Year + " GROUP BY  Year(HH.Date),MarketingAccountID";
                    }
                    MktSaleTargetList = MktSaleTarget.Gets(dispoSQL, (int)Session[SessionInfo.currentUserID]);
                    #endregion
                }
                #endregion

            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (oSalesReportPrint.BUID > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(oSalesReportPrint.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(oSalesReportPrint.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oSalesReports.ForEach(o => o.GroupName = "Group Name:" + o.GroupName);

            string sHeader = "Year Wise LC Receive Report";
            sHeader = _oSalesReport.PrintName;
            _oSalesReport.ViewType = Convert.ToInt32(sValue.Split('~')[3]);   
            rptSalesReportPreview oReport = new rptSalesReportPreview();
            rptSalesOrderMultipleType oReport1 = new rptSalesOrderMultipleType();
            byte[] abytes; ;
            if (IsDouble == 1)
            {
               abytes = oReport.PrepareReport(_oSalesReports, _oSalesReport, oCompany,  oBusinessUnit, sHeader);
            }
            else
            {
                sHeader = oSalesReportPrint.PrintName;
                abytes = oReport1.PrepareReport(_oSalesReports, RevSalesReports, MktSaleTargetList, oSalesReportPrint, oCompany,oBusinessUnit, sHeader);
            }            
            return File(abytes, "application/pdf");
        }

        [HttpPost]
        public ActionResult SetMktReportParam(MktSaleTarget oMktSaleTarget)
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            string sMarketingIDs = ""; int nCount = 0; string _SQL = "";       
            if (oMktSaleTarget.MKTViewType == 2)
            {
                sMarketingIDs = oMktSaleTarget.MarketingAccountID + "";
            }
            else
            {
                 _SQL = "SELECT * FROM MarketingAccount Where MarketingAccountID>0 AND GroupID = " + oMktSaleTarget.MarketingAccountID;
                _oMarketingAccounts = MarketingAccount.Gets(_SQL, (int)Session[SessionInfo.currentUserID]);       
            }
             
            foreach (MarketingAccount oItem in _oMarketingAccounts)
            {
                nCount++;
                if (_oMarketingAccounts.Count == nCount)
                {
                    sMarketingIDs += oItem.MarketingAccountID + "";

                }
                else
                {
                    sMarketingIDs += oItem.MarketingAccountID + ",";
                }
            }
            string sParam = sMarketingIDs + '~' + oMktSaleTarget.Year + '~' + oMktSaleTarget.ViewType + '~' + oMktSaleTarget.MKTViewType;
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, sParam);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MktPersonWiseReport()
        {
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            List<MktSaleTarget> oOrderReceives = new List<MktSaleTarget>();
            _oMarketingAccounts = new List<MarketingAccount>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
            string MktParams = (string)Session[SessionInfo.ParamObj];
            string Sql = "";
            oMktSaleTarget.ErrorMessage = Convert.ToString(MktParams.Split('~')[0]);
            oMktSaleTarget.Year = Convert.ToInt32(MktParams.Split('~')[1]);
            oMktSaleTarget.ViewType = Convert.ToInt32(MktParams.Split('~')[2]);
            oMktSaleTarget.MKTViewType = Convert.ToInt32(MktParams.Split('~')[3]);
            

            try
            {
                Sql = MakeSQLMktPersonWiseReport(oMktSaleTarget, 1);
                _oMktSaleTargets = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                Sql = MakeSQLMktPersonWiseReport(oMktSaleTarget, 2);
                oOrderReceives = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                oOrderReceives.ForEach(x =>
                {
                    if (_oMktSaleTargets.FirstOrDefault() != null && _oMktSaleTargets.FirstOrDefault().ContractorID > 0 && _oMktSaleTargets.Where(b => b.ContractorID == x.ContractorID && b.MarketingAccountID == x.MarketingAccountID).Count() > 0)
                    {
                        x.ErrorMessage = "";
                    }
                    else
                    {
                        _oMktSaleTargets.Add(x);
                    }
                });

                List<MktSaleTarget> _oTempList = new List<MktSaleTarget>();
                if (_oMktSaleTargets.Count > 0)
                {
                    foreach (MktSaleTarget oItem in _oMktSaleTargets)
                    {
                        _oTempList = new List<MktSaleTarget>();
                        _oTempList = oOrderReceives.Where(x => x.MarketingAccountID == oItem.MarketingAccountID && x.ContractorID == oItem.ContractorID && x.Month == oItem.Month).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.ReceiveQty = _oTempList.Sum(x => x.ReceiveQty);
                        }

                    }
                }
                oExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI WHERE LCID IN (SELECT ExportLCID FROM ExportLC WHERE YEAR(OpeningDate)='" + oMktSaleTarget.Year + "')", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMktSaleTarget = new MktSaleTarget();
                oMktSaleTarget.ErrorMessage = ex.Message;
                _oMktSaleTargets.Add(oMktSaleTarget);
            }

            if (_oMktSaleTargets.Count > 0)
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oMktSaleTarget.BUID = Convert.ToInt32(MktParams.Split('~')[3]);
                oBusinessUnit = oBusinessUnit.Get(oMktSaleTarget.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptMktPersonDetails oReport = new rptMktPersonDetails();
                int ViewType = Convert.ToInt32(MktParams.Split('~')[2]);
                string printStatus = "";

                byte[] abytes = oReport.PrepareReport(_oMktSaleTargets, oMktSaleTarget.GroupHeadName, oCompany, oMktSaleTarget.ViewType, oMktSaleTarget.Year, printStatus, oExportPIs);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[]  abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }
     
        }

        [HttpGet]
        public ActionResult MktSalesTargetPrintDetail(string sParams, double nts)
        {
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            List<MktSaleTarget> oOrderReceives = new List<MktSaleTarget>();
            _oMarketingAccounts = new List<MarketingAccount>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
            string MktParams = (string)Session[SessionInfo.ParamObj];
            string Sql = "";
            oMktSaleTarget.ErrorMessage = Convert.ToString(sParams.Split('~')[0]);
            oMktSaleTarget.Year = Convert.ToInt32(sParams.Split('~')[1]);
            oMktSaleTarget.ViewType = Convert.ToInt32(sParams.Split('~')[2]);
            oMktSaleTarget.MKTViewType = Convert.ToInt32(sParams.Split('~')[3]);
            string sString = Convert.ToString(sParams.Split('~')[4]);
            oMktSaleTarget.Month = GetMonthNumber(Convert.ToString(sString.Split(' ')[0]));
            oMktSaleTarget.Year = Convert.ToInt32(sString.Split(' ')[1]);

            try
            {
                Sql = MakeSQLMktPersonWiseReport(oMktSaleTarget, 1);
                _oMktSaleTargets = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                Sql = MakeSQLMktPersonWiseReport(oMktSaleTarget, 2);
                oOrderReceives = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                oOrderReceives.ForEach(x =>
                {
                    if (_oMktSaleTargets.FirstOrDefault() != null && _oMktSaleTargets.FirstOrDefault().ContractorID > 0 && _oMktSaleTargets.Where(b => b.ContractorID == x.ContractorID && b.MarketingAccountID == x.MarketingAccountID).Count() > 0)
                    {
                        x.ErrorMessage = "";
                    }
                    else
                    {
                        _oMktSaleTargets.Add(x);
                    }
                });

                List<MktSaleTarget> _oTempList = new List<MktSaleTarget>();
                if (_oMktSaleTargets.Count > 0)
                {
                    foreach (MktSaleTarget oItem in _oMktSaleTargets)
                    {
                        _oTempList = new List<MktSaleTarget>();
                        _oTempList = oOrderReceives.Where(x => x.MarketingAccountID == oItem.MarketingAccountID && x.ContractorID == oItem.ContractorID && x.Month == oItem.Month).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.ReceiveQty = _oTempList.Sum(x => x.ReceiveQty);
                        }

                    }
                }
                oExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI WHERE LCID IN (SELECT ExportLCID FROM ExportLC WHERE YEAR(OpeningDate)='" + oMktSaleTarget.Year + "')", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMktSaleTarget = new MktSaleTarget();
                oMktSaleTarget.ErrorMessage = ex.Message;
                _oMktSaleTargets.Add(oMktSaleTarget);
            }

            if (_oMktSaleTargets.Count > 0)
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oMktSaleTarget.BUID = Convert.ToInt32(sParams.Split('~')[3]);
                oBusinessUnit = oBusinessUnit.Get(oMktSaleTarget.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptMktPersonDetails oReport = new rptMktPersonDetails();
                int ViewType = Convert.ToInt32(sParams.Split('~')[2]);
                string printStatus = "";

                byte[] abytes = oReport.PrepareReport(_oMktSaleTargets, oMktSaleTarget.GroupHeadName, oCompany, oMktSaleTarget.ViewType, oMktSaleTarget.Year, printStatus, oExportPIs);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }

        }

        [HttpGet]
        public void MktPersonWiseXL()
        {
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            List<MktSaleTarget> oOrderReceives = new List<MktSaleTarget>();
            _oMarketingAccounts = new List<MarketingAccount>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
            string MktParams = (string)Session[SessionInfo.ParamObj];
            string Sql = "";
            oMktSaleTarget.ErrorMessage = Convert.ToString(MktParams.Split('~')[0]);
            oMktSaleTarget.Year = Convert.ToInt32(MktParams.Split('~')[1]);
            oMktSaleTarget.ViewType = Convert.ToInt32(MktParams.Split('~')[2]);
            oMktSaleTarget.MKTViewType = Convert.ToInt32(MktParams.Split('~')[3]);

            try
            {
                Sql = MakeSQLMktPersonWiseReport(oMktSaleTarget, 1);
                _oMktSaleTargets = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                Sql = MakeSQLMktPersonWiseReport(oMktSaleTarget, 2);
                oOrderReceives = MktSaleTarget.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                oOrderReceives.ForEach(x =>
                {
                    if (_oMktSaleTargets.FirstOrDefault() != null && _oMktSaleTargets.FirstOrDefault().ContractorID > 0 && _oMktSaleTargets.Where(b => b.ContractorID == x.ContractorID && b.MarketingAccountID == x.MarketingAccountID).Count() > 0)
                    {
                        x.ErrorMessage = "";
                    }
                    else
                    {
                        _oMktSaleTargets.Add(x);
                    }
                });

                List<MktSaleTarget> _oTempList = new List<MktSaleTarget>();
                if (_oMktSaleTargets.Count > 0)
                {
                    foreach (MktSaleTarget oItem in _oMktSaleTargets)
                    {
                        _oTempList = new List<MktSaleTarget>();
                        _oTempList = oOrderReceives.Where(x => x.MarketingAccountID == oItem.MarketingAccountID && x.ContractorID == oItem.ContractorID && x.Month == oItem.Month).ToList();
                        if (_oTempList.Count > 0)
                        {
                            oItem.ReceiveQty = _oTempList.Sum(x => x.ReceiveQty);
                        }

                    }
                }
                oExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI WHERE LCID IN (SELECT ExportLCID FROM ExportLC WHERE YEAR(OpeningDate)='" + oMktSaleTarget.Year + "')", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMktSaleTarget = new MktSaleTarget();
                oMktSaleTarget.ErrorMessage = ex.Message;
                _oMktSaleTargets.Add(oMktSaleTarget);
            }

            #region EXCEL
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nTotalCol = 0;
            int nCount = 0;
            int colIndex = 2;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Projection Report");
                sheet.Name = "Projection Report";
                sheet.Column(colIndex++).Width = 10; //SL
                sheet.Column(colIndex++).Width = 20; //composition
                sheet.Column(colIndex++).Width = 20; //buyer 
                sheet.Column(colIndex++).Width = 20; //construction
                sheet.Column(colIndex++).Width = 20; //weave
                sheet.Column(colIndex++).Width = 25; //finish
                sheet.Column(colIndex++).Width = 20; //approx order qty
                sheet.Column(colIndex++).Width = 20; //delivery qty
                sheet.Column(colIndex++).Width = 20; //tntv sending month

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Projection Report (markeing person wise) for year " + oMktSaleTarget.Year; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion              
                #region Report Body
                int nSL = 1;
                colIndex = 2;
                var MktSaleTargets = _oMktSaleTargets.GroupBy(x => new { x.Month,x.Name })
                                            .OrderBy(x => x.Key.Month)
                                            .Select(x => new
                                            {
                                                Month = x.Key.Month,
                                                MktPerson = x.Key.Name,
                                                _MktSaleTargetsList = x.OrderBy(c => c.MarketingAccountID),
                                                ApproxOrderQty = x.Sum(y => y.OrderQty),
                                                DeliveryQty = x.Sum(y => y.ReceiveQty)
                                            });
                foreach(var oData in MktSaleTargets)
                {
                    nSL = 1;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Projection for month of " + GetMonthWithYear(oData.Month, oMktSaleTarget.Year); cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = oData.MktPerson; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex++;
                    #region Header 1
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Composition"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Weave"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Finish"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Approx. Order Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Delivery Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Tntv Sending Month"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    #endregion
                    foreach (var oItem in oData._MktSaleTargetsList)
                    {
                        colIndex = 2;                             
                        cell = sheet.Cells[rowIndex,colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;  cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WeaveTypeName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FinishTypeName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EstOrderQtyST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceiveQtyST; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;                                  
                        nSL++;
                        rowIndex++;
                    }
                        #region SUB TOTAL
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oData.ApproxOrderQty; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oData.DeliveryQty; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;                             
                        rowIndex++;
                        #endregion

                }
                #region GRAND TOTAL
                cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Grand Total:"; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = _oMktSaleTargets.Sum(x => x.OrderQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = _oMktSaleTargets.Sum(x => x.ReceiveQty); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;                             
                
                rowIndex++;	
                #endregion

                #endregion

                #region Summery
                rowIndex = rowIndex + 2;
                colIndex = 3;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 6]; cell.Merge = true; cell.Value = "Summery"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                colIndex = 3;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Month"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "No Of PI"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex++;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                var data = oExportPIs.GroupBy(x => new { x.OpeningDate.Month, x.OpeningDate.Year }, (key, grp) => new
                {
                    Month = key.Month,
                    Year = key.Year,
                    NoOfPI = grp.ToList().Count(),
                    PIsNo = string.Join(", ", grp.ToList().Select(y => y.PINo)),
                    Qty = grp.ToList().Sum(y => y.Qty),
                    Amount = grp.ToList().Sum(y => y.Amount)
                }).OrderBy(c => c.Year).ThenBy(c => c.Month);

                foreach (var oItem in data)
                {
                    colIndex = 3;
                    DateTime dDate = new DateTime(oItem.Year, oItem.Month, 1);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dDate.ToString("MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NoOfPI; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = oItem.PIsNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Row(rowIndex).Height = MeasureTextHeight(oItem.PIsNo, cell.Style.Font, 70);
                    colIndex++;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                }

                colIndex = 3;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.Sum(x => x.NoOfPI); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0;(#,##0)"; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex++;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = data.Sum(x => x.Amount); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=RPT_MKTPersonDetailReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();

            }

            #endregion
        }

    }
                
}