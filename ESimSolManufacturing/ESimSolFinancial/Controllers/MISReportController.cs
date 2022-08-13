using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
 
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ESimSolFinancial.Controllers;

namespace ESimSolFinancial.Controllers
{
    public class MISReportController : Controller
    {

        #region Declaration 
        SessionWiseRecapSummary _oSessionWiseRecapSummary = new SessionWiseRecapSummary();
        List<SessionWiseRecapSummary> _oSessionWiseRecapSummaries = new List<SessionWiseRecapSummary>();
        //CommissionSummary _oCommissionSummary = new CommissionSummary();
        //List<CommissionSummary> _oCommissionSummaries = new List<CommissionSummary>();
        //List<PackAccessoriesStatement> _oPackAccessoriesStatements = new List<PackAccessoriesStatement>();
        //PackAccessoriesStatement _oPackAccessoriesStatement = new PackAccessoriesStatement();
        //EndrosmentCommissionStatement _oEndrosmentCommissionStatement = new EndrosmentCommissionStatement();
        //List<EndrosmentCommissionStatement> _oEndrosmentCommissionStatements = new List<EndrosmentCommissionStatement>();
        //OrderStatement _oOrderStatement = new OrderStatement();
        //List<OrderStatement> _oOrderStatements = new List<OrderStatement>();
        //CommissionStatement _oCommissionStatement = new CommissionStatement();
        //List<CommissionStatement> _oCommissionStatements = new List<CommissionStatement>();
        //BuyeingSummaryFollowUp _oBuyeingSummaryFollowUp = new BuyeingSummaryFollowUp();
        //List<BuyeingSummaryFollowUp> _oBuyeingSummaryFollowUps = new List<BuyeingSummaryFollowUp>();
        //YarnB2BLCDueStatus _oYarnB2BLCDueStatus = new YarnB2BLCDueStatus();
        //List<YarnB2BLCDueStatus> _oYarnB2BLCDueStatuss = new List<YarnB2BLCDueStatus>();
        #endregion

        #region function
        //#region GetBuyerwithDatesummaries
        //private List<CommissionSummary> GetBuyerwithDatesummaries(List<CommissionSummary> oCommissionSummaries)
        //{
        //    List<CommissionSummary> oNewCommissionSummaries = new List<CommissionSummary>();
        //    foreach (CommissionSummary oItem in oCommissionSummaries)
        //    {
        //        if (oItem.DataViewType == 1 || oItem.DataViewType == 2)
        //        {
        //            oNewCommissionSummaries.Add(oItem);
        //        }
        //    }
        //    return oNewCommissionSummaries;
        //}
        //#endregion
        #endregion

        #region Session Wise Recap Summary        
        public ActionResult ViewSessionWiseRecapSummary(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oSessionWiseRecapSummary = new SessionWiseRecapSummary();
            _oSessionWiseRecapSummary.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(_oSessionWiseRecapSummary);
        }
                        
        [HttpPost]
        public JsonResult GetSessionWiseRecapSummary(Contractor oContractor)
        {
            _oSessionWiseRecapSummaries = new List<SessionWiseRecapSummary>();
            int nSessionID = oContractor.ContractorID; //for this function Contractor ID hold sessionid 
            string sBuyerIDs = oContractor.Name;//for this function Contractor Name hold buyer ids
            try
            {
                _oSessionWiseRecapSummaries = SessionWiseRecapSummary.Gets(nSessionID, sBuyerIDs, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSessionWiseRecapSummary = new SessionWiseRecapSummary();
                _oSessionWiseRecapSummary.ErrorMessage = ex.Message;
                _oSessionWiseRecapSummaries.Add(_oSessionWiseRecapSummary);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSessionWiseRecapSummaries);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOrderRecapList(OrderRecap oOrderRecap)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            string sSQL = "";
            try
            {
                if (oOrderRecap.BuyerID > 0)
                {
                    sSQL = "SELECT * FROM View_OrderRecap WHERE BuyerID = " + oOrderRecap.BuyerID + " AND BusinessSessionID = " + oOrderRecap.BusinessSessionID+" AND BUID = "+oOrderRecap.BUID;
                    oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = ex.Message;
                oOrderRecaps.Add(oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }       
        #endregion

        //#region Commission Summary

        //public ActionResult ViewCommissionSummery(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
        //    List<CommissionSummary> oCommissionSummaries = new List<CommissionSummary>();
        //    return View(oCommissionSummaries);
        //}

        //public ActionResult ViewMonthWiseSummary(bool Isrealize, double ts)
        //{
        //    CommissionSummary oCommissionSummary = new CommissionSummary();
        //    return PartialView(oCommissionSummary);
        //}


        //[HttpGet]
        //public JsonResult GetCommissionSummery(string sYear)
        //{
        //    List<CommissionSummary> oCommissionSummaries = new List<CommissionSummary>();
        //    List<CommissionSummary> oMonthWiseSummaries = new List<CommissionSummary>();
        //    CommissionSummary oCommissionSummary = new CommissionSummary();

        //    try
        //    {
        //        oCommissionSummaries = CommissionSummary.Gets(sYear, (int)Session[SessionInfo.UserType], (int)Session[SessionInfo.currentUserID]);
        //        foreach (CommissionSummary oItem in oCommissionSummaries)
        //        {
        //            if (oItem.DataViewType == 4)
        //            {
        //                oMonthWiseSummaries.Add(oItem);
        //            }
        //        }

        //        foreach (CommissionSummary oItem in oMonthWiseSummaries)
        //        {
        //            oItem.BuyerWithMonthWiseSummaries = GetBuyerWithMonthWiseList(oItem.ShipmentMonth, oCommissionSummaries);
        //        }
        //        oCommissionSummary.MonthWiseSummaries = FillupMonthCycle(oMonthWiseSummaries, sYear);
        //        oCommissionSummary.BuyerWithDateWiseSummaries = GetBuyerwithDatesummaries(oCommissionSummaries);
        //        oCommissionSummary.ForcastingCommissionSummarys = _oForcastingCommissionSummarys;
        //        oCommissionSummary.RealizeationCommissionSummarys = _oCommissionRealizeCommissionSummarys;
        //    }
        //    catch (Exception ex)
        //    {
        //        oCommissionSummary = new CommissionSummary();
        //        oCommissionSummary.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oCommissionSummary);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //public List<CommissionSummary> _oForcastingCommissionSummarys = new List<CommissionSummary>();
        //public List<CommissionSummary> _oCommissionRealizeCommissionSummarys = new List<CommissionSummary>();

        //private List<CommissionSummary> GetBuyerWithMonthWiseList(int nMonth, List<CommissionSummary> oCommissionSummaries)
        //{
        //    List<CommissionSummary> oForCastingCommissionSummaries = new List<CommissionSummary>();
        //    List<CommissionSummary> oCommisionRealizeCommissionRecaps = new List<CommissionSummary>();
        //    List<CommissionSummary> oForcastingCommissionRecaps = new List<CommissionSummary>();
        //    CommissionSummary oTempCommissionSummary = new CommissionSummary();
        //    int nTempID = 0;
        //    int nRealizeTempID = 0;
        //    foreach (CommissionSummary oItem in oCommissionSummaries)
        //    {
        //        if (oItem.ShipmentMonth == nMonth && oItem.DataViewType == 3)
        //        {
                    
        //            nTempID = 0; nTempID = _oForcastingCommissionSummarys.Count + 1;
        //            nRealizeTempID = 0; nRealizeTempID = _oCommissionRealizeCommissionSummarys.Count + 1;
        //            oForcastingCommissionRecaps = new List<CommissionSummary>();
        //            oForcastingCommissionRecaps = GetMonthWiseList(nMonth, oItem.BuyerID, oCommissionSummaries);
        //            oCommisionRealizeCommissionRecaps = new List<CommissionSummary>();
        //            oCommisionRealizeCommissionRecaps = GetMonthWiseListForRealize(nMonth, oItem.BuyerID, oCommissionSummaries);
        //            oTempCommissionSummary = new CommissionSummary();
        //            oTempCommissionSummary.BuyerID = oItem.BuyerID;
        //            oTempCommissionSummary.BuyerName = oItem.BuyerName;
        //            oTempCommissionSummary.ShipmentMonthInString = oItem.ShipmentMonthInString;
        //            oTempCommissionSummary.TempID = nTempID;
        //            oTempCommissionSummary.RealizeTempID = nRealizeTempID;
        //            oTempCommissionSummary.ForcastingCommissionSummarys = oForcastingCommissionRecaps;
        //            _oForcastingCommissionSummarys.Add(oTempCommissionSummary);
        //            oTempCommissionSummary.RealizeationCommissionSummarys = oCommisionRealizeCommissionRecaps;
        //            _oCommissionRealizeCommissionSummarys.Add(oTempCommissionSummary);

        //            oItem.TempID = nTempID;
        //            oItem.RealizeTempID = nRealizeTempID;
        //            oItem.ShipmentCount = oForcastingCommissionRecaps.Count;
        //            oItem.RealizeCount = oCommisionRealizeCommissionRecaps.Count;
        //            oForCastingCommissionSummaries.Add(oItem);
        //        }
        //    }
        //    return oForCastingCommissionSummaries;
        //}

        //private List<CommissionSummary> GetMonthWiseList(int nMonth, int nBuyerID, List<CommissionSummary> oCommissionSummaries)
        //{
        //    List<CommissionSummary> oTempCommissionSummaries = new List<CommissionSummary>();
        //    foreach (CommissionSummary oItem in oCommissionSummaries)
        //    {
        //        if (oItem.ShipmentMonth == nMonth && oItem.BuyerID == nBuyerID && oItem.DataViewType == 1)
        //        {
        //            oTempCommissionSummaries.Add(oItem);
        //        }
        //    }
        //    return oTempCommissionSummaries;
        //}

        //private List<CommissionSummary> GetMonthWiseListForRealize(int nMonth, int nBuyerID, List<CommissionSummary> oCommissionSummaries)
        //{
        //    List<CommissionSummary> oTempCommissionSummaries = new List<CommissionSummary>();
        //    foreach (CommissionSummary oItem in oCommissionSummaries)
        //    {
        //        if (oItem.ShipmentMonth == nMonth && oItem.BuyerID == nBuyerID && oItem.DataViewType == 2)
        //        {
        //            oTempCommissionSummaries.Add(oItem);
        //        }
        //    }
        //    return oTempCommissionSummaries;
        //}

        //private List<CommissionSummary> FillupMonthCycle(List<CommissionSummary> oMonthWiseSummaries, string sYear)
        //{
        //    List<CommissionSummary> oTempMonthWiseSummaries = new List<CommissionSummary>();
        //    CommissionSummary oCommissionSummary = new CommissionSummary();
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        oCommissionSummary = GetMonth(oMonthWiseSummaries, i);
        //        if (oCommissionSummary.ForcastingAmount > 0 || oCommissionSummary.CommisionRealizeAmount>0)
        //        {
        //            oTempMonthWiseSummaries.Add(oCommissionSummary);
        //        }
        //        else
        //        {
        //            oCommissionSummary = new CommissionSummary();
        //            oCommissionSummary.ShipmentMonth = i;
        //            oCommissionSummary.ShipmentMonthInString = ((EnumMonthName)i).ToString() + "-" + sYear;
        //            oCommissionSummary.ForcastingAmount = 0;
        //            oCommissionSummary.CommisionRealizeAmount= 0;
        //            oTempMonthWiseSummaries.Add(oCommissionSummary);
        //        }
        //    }
        //    return oTempMonthWiseSummaries;
        //}

        //private CommissionSummary GetMonth(List<CommissionSummary> oMonthWiseSummaries, int nMonth)
        //{
        //    CommissionSummary oCommissionSummary = new CommissionSummary();
        //    foreach (CommissionSummary oItem in oMonthWiseSummaries)
        //    {
        //        if (oItem.ShipmentMonth == nMonth)
        //        {
        //            return oItem;
        //        }
        //    }
        //    return oCommissionSummary;
        //}       
        //#endregion

        #region Package Summary
    
        //public ActionResult ViewPackAccessoriesStatementReport(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
        //    _oPackAccessoriesStatement = new PackAccessoriesStatement();
        //    _oPackAccessoriesStatement.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
        //    return View(_oPackAccessoriesStatement);
        //}
        
        //[HttpGet]
        //public JsonResult GetPackAccessoriesStatement(string sControl)
        //{
        //    _oPackAccessoriesStatements = new List<PackAccessoriesStatement>();
        //    int nSessionID = Convert.ToInt32(sControl.Split('~')[0]);
        //    string sSupplierIDs = sControl.Split('~')[1];
        //    int nPrintingCriteria = Convert.ToInt32(sControl.Split('~')[2]);//0:Supplier Wise; 1:Buyer wise;2:LC Wise
        //    int nAccessorisType = Convert.ToInt32(sControl.Split('~')[3]);
        //    string sAccessoriesType = "";
        //    try
        //    {
        //        if (nAccessorisType == 0)
        //        {
        //            sAccessoriesType = "1,2";
        //        }
        //        else
        //        {
        //            sAccessoriesType = nAccessorisType.ToString();
        //        }
        //        _oPackAccessoriesStatements = PackAccessoriesStatement.Gets(nSessionID, sSupplierIDs,nPrintingCriteria,sAccessoriesType,(int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oPackAccessoriesStatement = new PackAccessoriesStatement();
        //        _oPackAccessoriesStatement.ErrorMessage = ex.Message;
        //        _oPackAccessoriesStatements.Add(_oPackAccessoriesStatement);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oPackAccessoriesStatements);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //#region Print
        //public ActionResult PrintPakageAccessoriesStatement(string sControl)
        //{
        //    _oPackAccessoriesStatements = new List<PackAccessoriesStatement>();
        //    Company oCompany = new Company();
        //    int nSessionID = Convert.ToInt32(sControl.Split('~')[0]);
        //    string sSupplierIDs = sControl.Split('~')[1];
        //    string sSessionName = sControl.Split('~')[2];
        //    int nPrintingCriteria = Convert.ToInt32(sControl.Split('~')[3]);//0:Supplier Wise; 1:Buyer wise;2:LC Wise
        //    int nAccessorisType = Convert.ToInt32(sControl.Split('~')[4]);
        //    string sAccessoriesType = ""; string sPrintHeader = "";
        //    if (nAccessorisType == 0)
        //    {
        //        sAccessoriesType = "1,2";
        //    }
        //    else
        //    {
        //        sAccessoriesType = nAccessorisType.ToString();
        //    }
        //    _oPackAccessoriesStatements = PackAccessoriesStatement.Gets(nSessionID, sSupplierIDs,nPrintingCriteria,sAccessoriesType, (int)Session[SessionInfo.currentUserID]);
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    if(nAccessorisType==0)
        //    {
        //        sPrintHeader = "Accessories (All)";
        //    }else if(nAccessorisType==1)
        //    {
        //        sPrintHeader = "Accessories ";
        //    }
        //    else
        //    {
        //        sPrintHeader = "Package Accessories ";
        //    }
        //    if (_oPackAccessoriesStatements.Count > 0)
        //    {
        //        rptPackAccessoriesStatementReport oReport = new rptPackAccessoriesStatementReport();
        //        byte[] abytes = oReport.PrepareReport(_oPackAccessoriesStatements, oCompany, sSessionName, nPrintingCriteria, sPrintHeader);
        //        return File(abytes, "application/pdf");
        //    }
        //    else
        //    {

        //        string sMessage = "There is no data for print";
        //        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
        //    }
        //}

        //public Image GetCompanyLogo(Company oCompany)
        //{
        //    if (oCompany.OrganizationLogo != null)
        //    {
        //        MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        //img.Save(Response.OutputStream, ImageFormat.Jpeg);
        //        img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
        //        return img;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //#endregion
        #endregion

        #region Endrosement Statement
        #region  View Endrosment Statement
        //public ActionResult ViewEndrosementCommissionStatement(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
        //    string sSQL = "SELECT * FROM VIEW_ProductCategory WHERE ParentCategoryID = 3";
        //    _oEndrosmentCommissionStatement = new EndrosmentCommissionStatement();
        //    _oEndrosmentCommissionStatement.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
        //    _oEndrosmentCommissionStatement.ProductCategorys = ProductCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);// 3 parent id for Finish Goods
        //    return View(_oEndrosmentCommissionStatement);
        //}
        #endregion

        //[HttpGet]
        //public JsonResult GetEndrosmentCommissionStatement(string sControl)
        //{
        //    _oEndrosmentCommissionStatements = new List<EndrosmentCommissionStatement>();
        //    int nSessionID = Convert.ToInt32(sControl.Split('~')[0]);
        //    int nProductCategoryID = Convert.ToInt32(sControl.Split('~')[1]);
        //    string sFactoryIDs = sControl.Split('~')[2];
        //    string sBuyerIDs = sControl.Split('~')[3];
        //    bool bIsDueOnly = Convert.ToBoolean(sControl.Split('~')[4]);
        //    DateTime dSelectedDate = Convert.ToDateTime(sControl.Split('~')[5]);
        //    int nPrintingCriteria = Convert.ToInt32(sControl.Split('~')[6]);//0:Fatory Wise; 1:Buyer wise;
        //    try
        //    {
        //        _oEndrosmentCommissionStatements = EndrosmentCommissionStatement.Gets(nProductCategoryID,nSessionID,sFactoryIDs,sBuyerIDs,bIsDueOnly,dSelectedDate,nPrintingCriteria,(int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oEndrosmentCommissionStatement = new EndrosmentCommissionStatement();
        //        _oEndrosmentCommissionStatement.ErrorMessage = ex.Message;
        //        _oEndrosmentCommissionStatements.Add(_oEndrosmentCommissionStatement);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oEndrosmentCommissionStatements);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        #region Print
        //public ActionResult PrintEndorsmentCommissionStatement(string sControl)
        //{
        //    _oEndrosmentCommissionStatement = new EndrosmentCommissionStatement();
        //    Company oCompany = new Company();
        //    int nSessionID = Convert.ToInt32(sControl.Split('~')[0]);
        //    int nProductCategoryID = Convert.ToInt32(sControl.Split('~')[1]);
        //    string sFactoryIDs = sControl.Split('~')[2];
        //    string sBuyerIDs = sControl.Split('~')[3];
        //    bool bIsDueOnly = Convert.ToBoolean(sControl.Split('~')[4]);
        //    DateTime dSelectedDate = Convert.ToDateTime(sControl.Split('~')[5]);
        //    int nPrintingCriteria = Convert.ToInt32(sControl.Split('~')[6]);//0:Fatory Wise; 1:Buyer wise;
        //    string sSessionName = sControl.Split('~')[7];
        //    _oEndrosmentCommissionStatements = EndrosmentCommissionStatement.Gets(nProductCategoryID, nSessionID, sFactoryIDs, sBuyerIDs, bIsDueOnly, dSelectedDate, nPrintingCriteria, (int)Session[SessionInfo.currentUserID]);
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    if (_oEndrosmentCommissionStatements.Count > 0)
        //    {
        //        rptEndrosmentCommissionStatement oReport = new rptEndrosmentCommissionStatement();
        //        byte[] abytes = oReport.PrepareReport(_oEndrosmentCommissionStatements, oCompany, sSessionName, nPrintingCriteria);
        //        return File(abytes, "application/pdf");
        //    }
        //    else
        //    {

        //        string sMessage = "There is no data for print";
        //        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
        //    }
        //}

        
        #endregion
        #endregion

        //#region Bill With Allocate 
        //#region  View Endrosment Statement
        //public ActionResult ViewBillWithAllocate (int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
        //    BillWithAllocateReport oBillWithAllocateReport = new BillWithAllocateReport();
        //    return View(oBillWithAllocateReport);
        //}
        //#endregion

        //#region Print Bill with Allocate Report
        //public ActionResult PrintBillWithAllocateReport(string sIDS)
        //{
        //    List<BillWithAllocateReport> oBillWithAllocateReports = new List<BillWithAllocateReport>();
        //    Company oCompany = new Company();

        //    oBillWithAllocateReports = BillWithAllocateReport.Gets(sIDS, (int)Session[SessionInfo.currentUserID]);
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    if (oBillWithAllocateReports.Count > 0)
        //    {
        //        rptBillWithAllocateReport oReport = new rptBillWithAllocateReport();
        //        byte[] abytes = oReport.PrepareReport(oBillWithAllocateReports, oCompany);
        //        return File(abytes, "application/pdf");
        //    }
        //    else
        //    {

        //        string sMessage = "There is no data for print";
        //        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
        //    }
        //}
        //#endregion
        //#endregion

        //#region Order Statement
        //public ActionResult ViewOrderStatement(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
        //    _oOrderStatement = new OrderStatement();
        //    _oOrderStatement.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
        //    return View(_oOrderStatement);
        //}
        //[HttpGet]
        //public JsonResult GetOrderStatement(int nSessionID, string sBuyerIDs, double ts)
        //{
        //    _oOrderStatements = new List<OrderStatement>();
        //    try
        //    {
        //        _oOrderStatements = OrderStatement.Gets(nSessionID, sBuyerIDs, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oOrderStatement = new OrderStatement();
        //        _oOrderStatement.ErrorMessage = ex.Message;
        //        _oOrderStatements.Add(_oOrderStatement);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oOrderStatements);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult PrintOrderSheet(FormCollection DataCollection)
        //{
        //    _oOrderStatements = new List<OrderStatement>();
        //    _oOrderStatements = new JavaScriptSerializer().Deserialize<List<OrderStatement>>(DataCollection["txtCollectionPrintText"]);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    if (_oOrderStatements.Count > 0)
        //    {
        //        rptOrderStatement oReport = new rptOrderStatement();
        //        byte[] _abytes = oReport.PrepareReport(_oOrderStatements, oCompany);
        //        return File(_abytes, "application/pdf");
        //    }
        //    else
        //    {
        //        string sMessage = "There is no Data for Print";
        //        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
        //    }
        //}
        //#endregion

        //#region Commission Statement
        //public ActionResult ViewCommissionStatement(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
        //    _oCommissionStatement = new CommissionStatement();
        //    _oCommissionStatement.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
        //    return View(_oCommissionStatement);
        //}
        //[HttpGet]
        //public JsonResult GetCommissionStatement(int nSessionID,  double ts)
        //{
        //    _oCommissionStatements = new List<CommissionStatement>();
        //    try
        //    {
        //        _oCommissionStatements = CommissionStatement.Gets(nSessionID,  (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oCommissionStatement = new CommissionStatement();
        //        _oCommissionStatement.ErrorMessage = ex.Message;
        //        _oCommissionStatements.Add(_oCommissionStatement);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oCommissionStatements);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult PrintCommissinStatement(FormCollection DataCollection)
        //{
        //    _oCommissionStatements = new List<CommissionStatement>();
        //    _oCommissionStatements = new JavaScriptSerializer().Deserialize<List<CommissionStatement>>(DataCollection["txtCollectionPrintText"]);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    if (_oCommissionStatements.Count > 0)
        //    {
        //        rptCommissionStatement oReport = new rptCommissionStatement();
        //        byte[] _abytes = oReport.PrepareReport(_oCommissionStatements, _oCommissionStatements[0].SessionName, oCompany);
        //        return File(_abytes, "application/pdf");
        //    }
        //    else
        //    {
        //        string sMessage = "There is no Data for Print";
        //        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
        //    }
        //}
        //#endregion

        #region Bueing Summary Follow up
        //public ActionResult ViewOperationFollowUpSummary(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
        //    _oBuyeingSummaryFollowUp = new BuyeingSummaryFollowUp();
        //    if ((int)Session[SessionInfo.UserType] == (int)EnumUserType.Admin_User)
        //    {
        //        ViewBag.Users = ESimSol.BusinessObjects.User.Gets((int)Session[SessionInfo.currentUserID]);
        //        //ViewBag.Employees = Employee.Gets((int)Session[SessionInfo.currentUserID]);//Merchandiser Get
        //    }
        //    else
        //    {
        //        User oUser = new ESimSol.BusinessObjects.User();
        //        oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
        //        ViewBag.Users = oUser;
        //        //string sSQL = "SELECT * FROM View_Employee WHERE EmployeeID = (SELECT EmployeeID FROM Users WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
        //        //ViewBag.Employees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);//Merchandiser Get
        //    }
        //    ViewBag.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
        //    ViewBag.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]); 

        //    return View(_oBuyeingSummaryFollowUp);
        //}
        //private string GetSQL(string sTemp)
        //{
          
        //    int nIssueDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
        //    DateTime dIssueStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    DateTime dIssueEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

        //    string sStNo = sTemp.Split('~')[3];
        //    string sBuyerIDs = sTemp.Split('~')[4];
        //    int nEntryByID = Convert.ToInt32(sTemp.Split('~')[5]);

        //    int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[6]);
        //    int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[7]);
        //    int nSessionID = Convert.ToInt32(sTemp.Split('~')[8]);
        //    int nBrandID = Convert.ToInt32(sTemp.Split('~')[9]);
        //    string sFactoryIDs = sTemp.Split('~')[10];
        //    string nMerchandiserIDs = sTemp.Split('~')[11];
        //    int nStyleType = Convert.ToInt32(sTemp.Split('~')[12]);
        //    int nIsRunning = Convert.ToInt32(sTemp.Split('~')[13]);

        //    string sReturn1 = "SELECT * FROM View_TechnicalSheet";
        //    string sReturn = "";

        //    #region Style No

        //    if (sStNo != "")
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " StyleNo = '" + sStNo + "'";
        //    }
        //    #endregion

        //    #region Buyer Name

        //    if (sBuyerIDs != "")
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
        //    }
        //    #endregion

        //    #region Entry By Name

        //    if (nEntryByID != 0)
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " DBUserID = " + nEntryByID;
        //    }
        //    #endregion

        //    #region Session
        //    if (nSessionID > 0)
        //    {

        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " BusinessSessionID =" + nSessionID;

        //    }
        //    #endregion

        //    #region Brand
        //    if (nBrandID > 0)
        //    {

        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " ISNULL(BrandID,0) =" + nBrandID;

        //    }
        //    #endregion

        //    #region Style Type
        //    if (nStyleType!= -1)//-1: None
        //    {

        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " ISNULL(TSType,0) =" + nStyleType;

        //    }
        //    #endregion

        //    #region Issue Date Wise
        //    if (nIssueDateCom > 0)
        //    {
        //        if (nIssueDateCom == 1)
        //        {
        //            Global.TagSQL(ref sReturn);
        //            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
        //        }
        //        if (nIssueDateCom == 2)
        //        {
        //            Global.TagSQL(ref sReturn);
        //            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
        //        }
        //        if (nIssueDateCom == 3)
        //        {
        //            Global.TagSQL(ref sReturn);
        //            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
        //        }
        //        if (nIssueDateCom == 4)
        //        {
        //            Global.TagSQL(ref sReturn);
        //            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
        //        }
        //        if (nIssueDateCom == 5)
        //        {
        //            Global.TagSQL(ref sReturn);
        //            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
        //        }
        //        if (nIssueDateCom == 6)
        //        {
        //            Global.TagSQL(ref sReturn);
        //            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
        //        }
        //    }

        //    #endregion

        //    #region IsApproved
        //    if (IsCheckedApproved == 1)
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " DevelopmentStatus = " + (int)EnumDevelopmentStatus.ApprovedDone;
        //    }
        //    #endregion

        //    #region IsNotApproved
        //    if (IsCheckedNotApproved == 1)
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " DevelopmentStatus <"+ (int)EnumDevelopmentStatus.ApprovedDone;;
        //    }
        //    #endregion

        //    #region Merchandiser
        //    if (nMerchandiserIDs != "")
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " TechnicalSheetID IN (SELECT TechnicalSheetID FROM OrderRecap WHERE MerchandiserID IN( " + nMerchandiserIDs+"))";
        //    }
        //    #endregion

        //    #region Fatory
        //    if (sFactoryIDs != "")
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " TechnicalSheetID IN (SELECT TechnicalSheetID FROM OrderRecap WHERE ProductionFactoryID IN ( " + sFactoryIDs + "))";
        //    }
        //    #endregion

        //    #region IsRunning
        //    if (nIsRunning==1)
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " TechnicalSheetID IN (SELECT HH.TechnicalSheetID FROM OrderRecap AS HH  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),HH.ShipmentDate,106)) >=CONVERT(DATE,CONVERT(VARCHAR(12),GETDATE(),106)))";
        //    }
        //    #endregion

            

        //    #region User Set
        //    if ((int)Session[SessionInfo.UserType] == (int)EnumUserType.Normal_User)
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn += " TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
        //    }
        //    #endregion

        //    #region Avoid Hidden Style
        //    Global.TagSQL(ref sReturn);
        //    sReturn += " TechnicalSheetID NOT IN (SELECT TT.TechnicalSheetID FROM UserWiseStyleConfigure AS TT  WHERE TT.UserID=" + ((int)Session[SessionInfo.currentUserID]).ToString() + ")";
        //    #endregion

        //    sReturn = sReturn1 + sReturn + " ORDER BY TechnicalSheetID, BuyerID";
        //    return sReturn;
        //}
        //[HttpGet]
        //public JsonResult Gets(string Temp)
        //{
        //    List<TechnicalSheet> oTechnicalSheets = new List<TechnicalSheet>();
        //    try
        //    {
        //        string sSQL = GetSQL(Temp);
        //        oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        oTechnicalSheets = new List<TechnicalSheet>();
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oTechnicalSheets);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult GetsOperationSummaryFollowUp(string sTSIDs)
        //{
        //    _oBuyeingSummaryFollowUps = new List<BuyeingSummaryFollowUp>();
        //    try
        //    {                
        //        _oBuyeingSummaryFollowUps = BuyeingSummaryFollowUp.Gets(sTSIDs, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oBuyeingSummaryFollowUps = new List<BuyeingSummaryFollowUp>();
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oBuyeingSummaryFollowUps);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult PrintOperationSummaryFollowUp(string sParam)
        //{
        //    _oBuyeingSummaryFollowUp = new BuyeingSummaryFollowUp();
        //    Company oCompany = new Company();
        //    int nPriviousTSID = 0;
        //    _oBuyeingSummaryFollowUp.BuyeingSummaryFollowUps = BuyeingSummaryFollowUp.Gets(sParam, (int)Session[SessionInfo.currentUserID]);
        //    foreach (BuyeingSummaryFollowUp oItem in _oBuyeingSummaryFollowUp.BuyeingSummaryFollowUps)
        //    {
        //        if (oItem.TechnicalSheetID != nPriviousTSID)//get one time for one Technicalsheet
        //        {
        //            oItem.StyleCoverImage = GetThumImage(oItem.TechnicalSheetID);
        //            nPriviousTSID = oItem.TechnicalSheetID;
        //        }
        //    }
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    _oBuyeingSummaryFollowUp.Company = oCompany;

        //    if (_oBuyeingSummaryFollowUp.BuyeingSummaryFollowUps.Count > 0)
        //    {
        //        rptBuyeingOperationSummaryFollowUps oReport = new rptBuyeingOperationSummaryFollowUps();
        //        byte[] abytes = oReport.PrepareReport(_oBuyeingSummaryFollowUp);
        //        return File(abytes, "application/pdf");
        //    }
        //    else
        //    {
        //        string sMessage = "There is no data for print";
        //        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
        //    }

        //}

        public Image GetThumImage(int id)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = oTechnicalSheetThumbnail.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetThumbnail.ThumbnailImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetThumbnail.ThumbnailImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Yarn B2B LC Due Status
        //public ActionResult ViewYarnB2BLCDueStatus(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
        //    _oYarnB2BLCDueStatus = new YarnB2BLCDueStatus();
        //    ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
        //    return View(_oYarnB2BLCDueStatus);
        //}
        //[HttpGet]
        //public JsonResult GetsForYarnB2BLCDueStatus(int nBSID)
        //{
        //    _oYarnB2BLCDueStatuss = new List<YarnB2BLCDueStatus>();
        //    try
        //    {
        //        if (nBSID > 0)
        //        {
        //            _oYarnB2BLCDueStatuss = YarnB2BLCDueStatus.Gets(nBSID, (int)Session[SessionInfo.currentUserID]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _oYarnB2BLCDueStatus = new YarnB2BLCDueStatus();
        //        _oYarnB2BLCDueStatus.ErorMessage = ex.Message.Split('!')[0];
        //        _oYarnB2BLCDueStatuss.Add(_oYarnB2BLCDueStatus);

        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oYarnB2BLCDueStatuss);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        ////
        //public ActionResult PrintYarnB2BLCDueStatus(int nBSID, string sSessionName)
        //{
        //    _oYarnB2BLCDueStatuss = new List<YarnB2BLCDueStatus>();
        //    Company oCompany = new Company();
        //    if (nBSID>0)
        //    {
        //        _oYarnB2BLCDueStatuss = YarnB2BLCDueStatus.Gets(nBSID, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    if (_oYarnB2BLCDueStatuss.Count > 0)
        //    {
        //        rptYarnB2BLCDueStatus oReport = new rptYarnB2BLCDueStatus();
        //        byte[] abytes = oReport.PrepareReport(_oYarnB2BLCDueStatuss, oCompany, sSessionName);
        //        return File(abytes, "application/pdf");
        //    }
        //    else
        //    {
        //        string sMessage = "There is no data for print";
        //        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
        //    }

        //}

        #endregion
    }
}
