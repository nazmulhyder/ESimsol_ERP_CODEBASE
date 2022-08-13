using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ESimSolFinancial.Controllers
{
    public class RouteSheetHistoryController : Controller
    {
        #region Declaration
        RouteSheet _oRouteSheet = new RouteSheet();
        List<RouteSheetHistory> _oRouteSheetHistorys = new List<RouteSheetHistory>();
        List<RouteSheet> _oRouteSheets = new List<RouteSheet>();
        string _sErrorMessage = "";
        #endregion

        
        #region RS in SubfinishingStore
        public ActionResult ViewRouteSheetHistory(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oRouteSheet = new RouteSheet();
            return View(_oRouteSheet);
        }
        public ActionResult ViewRSHistoryUpdate(int id)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RouteSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oRouteSheet = new RouteSheet();
            if (id > 0) 
            {
                _oRouteSheet = RouteSheet.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oRouteSheet.RSHistorys = RouteSheetHistory.Gets(_oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oRouteSheet);
        }
        [HttpPost]
        public JsonResult UpdateEventTime(RouteSheetHistory oRouteSheetHistory)
        {
            try
            {
                oRouteSheetHistory = oRouteSheetHistory.UpdateEventTime(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetHistory = new RouteSheetHistory();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetHistory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public JsonResult GetsITBYRS(RouteSheet oRouteSheet)
        {
            List<ITransaction> oITransactions = new List<ITransaction>();
            try
            {
                oITransactions = ITransaction.Gets("Select * from View_ITransaction where LotID in (Select LotID from Lot where Lot.ParentType=106 and Lot.ParentID=" + oRouteSheet.RouteSheetID + " ) OR (TriggerParentID=" + oRouteSheet.RouteSheetID + " and  TriggerParentType in (106)) order by [DateTime],WorkingunitID", ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oITransactions = new List<ITransaction>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITransactions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintRouteSheetHistory(int id)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<ITransaction> oITransactions = new List<ITransaction>();
            List<Lot> oLots = new List<Lot>();
            string sTemp = "";
            try
            {
                if (id > 0)
                {
                    oRouteSheet = RouteSheet.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oITransactions = ITransaction.Gets("Select * from View_ITransaction where LotID in (Select LotID from Lot where Lot.ParentType=106 and Lot.ParentID=" + id + " ) or TriggerParentID=" + id + " and  TriggerParentType in (106) order by [DateTime],WorkingunitID", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sTemp=string.Join(",", oITransactions.Select(x => x.LotID).Distinct().ToList());
                    if(!string.IsNullOrEmpty(sTemp)); 
                    {
                        oLots = Lot.Gets("Select * from View_Lot Where LotID<>0 AND  LotID in (" + sTemp + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }

                }
            }
            catch (Exception ex)
            {
                oITransactions = new List<ITransaction>();
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            byte[] abytes;
            rptRouteSheetHistory oReport = new rptRouteSheetHistory();
            abytes = oReport.PrepareReport(oRouteSheet, oITransactions, oCompany, oBusinessUnit, oLots);
            return File(abytes, "application/pdf");
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

        [HttpPost]
        public JsonResult GetsLot(RouteSheet oRouteSheet)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                oLots = Lot.Gets("Select * from View_Lot where ParentType=106 and ParentID=" + oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPTU(RouteSheet oRouteSheet)
        {
            PTU oPTU = new PTU();
            try
            {
                oPTU = PTU.Get(oRouteSheet.PTUID , ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPTU = new PTU();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPTU);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsRouteSheet(RouteSheet oRouteSheet)
        {
            _oRouteSheets = new List<RouteSheet>();
            try
            {
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheet.RouteSheetNo)) ? "" : oRouteSheet.RouteSheetNo.Trim();
                string sSQL = "Select top(100)* from View_RouteSheet Where RouteSheetID<>0 ";// +(int)EnumRSState.InFloor;
                if (sRouteSheetNo != "") sSQL = sSQL + "And RouteSheetNo Like '%" + sRouteSheetNo + "%'";
                sSQL = sSQL + " order by RouteSheetID DESC";
                _oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheet = new RouteSheet();
                _oRouteSheets = new List<RouteSheet>();
                oRouteSheet.ErrorMessage = ex.Message;
                _oRouteSheets.Add(oRouteSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsRSHistory(RouteSheet oRouteSheet)
        {
            _oRouteSheetHistorys = new List<RouteSheetHistory>();
            try
            {
                _oRouteSheetHistorys = RouteSheetHistory.Gets(oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheetHistorys = new List<RouteSheetHistory>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsInvoiceInFo(RouteSheet oRouteSheet)
        {
            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            try
            {
                oImportInvoices = ImportInvoice.Gets("Select * from view_ImportInvoice where ImportInvoiceID in (Select GRN.RefObjectID from GRN where GRNID in (Select GRNID from GRNDetail where GRNDetail.GRNDetailID in (Select IT.TriggerParentID from ITransaction as IT where IT.TriggerParentType =103 and LotID in (Select LotID from RouteSheet where RouteSheetID =" + oRouteSheet.RouteSheetID + " )or LotID in (Select ParentLotID from Lot where ParentLotID>0 and LotID in (Select LotID from RouteSheet where RouteSheetID =" + oRouteSheet.RouteSheetID + ") )) ))", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportInvoices = new List<ImportInvoice>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}