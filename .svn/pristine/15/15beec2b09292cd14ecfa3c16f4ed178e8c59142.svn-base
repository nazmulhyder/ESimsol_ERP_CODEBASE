using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ImportInvoiceController : Controller
    {
        ImportInvoice _oImportInvoice = new ImportInvoice();
        List<ImportInvoice> _oImportInvoices = new List<ImportInvoice>();
        ImportPack _oImportPack = new ImportPack();
        List<ImportPackDetail> _oImportPackDetails = new List<ImportPackDetail>();
        ImportPackDetail _oImportPackDetail = new ImportPackDetail();
        ImportLC _oImportLC = new ImportLC();
        private string sReturn = "";

        List<Currency> _oCurrencys = new List<Currency>();

        public ActionResult ViewImportInvoices(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ImportInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oImportInvoices = new List<ImportInvoice>();
            string sSQL = "SELECT * FROM View_ImportInvoice WHERE BUID = " + buid + " AND InvoiceStatus = " + (int)EnumImportInvoiceEvent.Initialized + " ORDER BY DateofReceive, ImportInvoiceNo ASC";
            _oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
          
            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            oImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.InvoiceStatusObjs = EnumObject.jGets(typeof(EnumInvoiceEvent));
            ViewBag.InvoiceBankStatusObjs = EnumObject.jGets(typeof(EnumInvoiceBankStatus));
            ViewBag.BUID = buid;
            ViewBag.ImportPITypeObj = EnumObject.jGets(typeof(EnumImportPIType)).Where(x => x.id == (int)EnumImportPIType.Foreign || x.id == (int)EnumImportPIType.Local || x.id == (int)EnumImportPIType.NonLC || x.id == (int)EnumImportPIType.TTForeign || x.id == (int)EnumImportPIType.TTLocal);
            return View(_oImportInvoices);
        }
        public ActionResult ViewImportInvoice(int id, int nLCID)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ImportInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oImportInvoice = new ImportInvoice();
            ImportInvoiceHistory oImportInvoiceHistory = new ImportInvoiceHistory();
            ImportBL oImportBL = new ImportBL();
            Company oCompany = new Company();
            if (id > 0)
            {
                _oImportInvoice = _oImportInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oImportInvoice.ImportBL = oImportBL.GetByInvoice(id, (int)Session[SessionInfo.currentUserID]);
                oImportInvoiceHistory = oImportInvoiceHistory.Get((int)_oImportInvoice.InvoiceStatus, (int)_oImportInvoice.BankStatus, _oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportInvoice.ImportInvoiceHistory = oImportInvoiceHistory;
                _oImportInvoice.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            }
            if (nLCID > 0)
            {
                _oImportLC = new ImportLC();
                _oImportLC = _oImportLC.Get(nLCID, (int)Session[SessionInfo.currentUserID]);
                _oImportInvoice.ImportLCID = _oImportLC.ImportLCID;
                _oImportInvoice.ShipmentDate = _oImportLC.ShipmentDate;
                _oImportInvoice.ImportLCNo = _oImportLC.ImportLCNo;
                _oImportInvoice.ContractorName = _oImportLC.ContractorName;
                _oImportInvoice.ContractorID = _oImportLC.ContractorID;
                _oImportInvoice.Currency = _oImportLC.Currency;
                _oImportInvoice.CurrencyID = _oImportLC.CurrencyID;
            }

            List<RouteLocation> oRouteLocations = new List<RouteLocation>();
            ViewBag.RouteLocations = RouteLocation.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oImportInvoice);
        }
        public ActionResult ViewLCWiseInvoices(int nLCID)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ImportInvoice).ToString() + "," + ((int)EnumModuleName.ImportLC).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oImportLC = new ImportLC();
            List<ImportPIDetail> oImportPIDetails = new List<ImportPIDetail>();
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            string sSQL = "";
            if (nLCID > 0)
            {
                _oImportLC = _oImportLC.Get(nLCID, (int)Session[SessionInfo.currentUserID]);
                _oImportLC.ImportInvoices = ImportInvoice.Gets(nLCID, (int)Session[SessionInfo.currentUserID]);
                sSQL = "Select * from View_ImportPIDetail where ImportPIID in (Select ImportPIID from ImportLCDetail where ImportLCID=" + _oImportLC .ImportLCID+ "and Activity=1 )";
                oImportPIDetails = ImportPIDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "Select * from View_ImportInvoiceDetail where ImportInvoiceID  in (Select ImportInvoiceID from ImportInvoice where ImportLCID>0 and ImportLCID in (Select ImportLCID from ImportLC where ImportLCID=" + _oImportLC.ImportLCID + "))";
                oImportInvoiceDetails = ImportInvoiceDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oImportInvoiceDetails.Any() && oImportInvoiceDetails.FirstOrDefault().ImportPIDetailID > 0)
                {
                    oImportPIDetails.ForEach(x => { x.Qty = x.Qty - oImportInvoiceDetails.Where(p => p.ImportPIDetailID == x.ImportPIDetailID).Sum(o => o.Qty); });
                    oImportPIDetails.RemoveAll(x => x.Qty <= 0);
                }
                oImportPIDetails = oImportPIDetails.GroupBy(x => new { x.MUnitID, x.MUName}, (key, grp) =>
                                         new ImportPIDetail
                                         {
                                             MUnitID = key.MUnitID,
                                             MUName = key.MUName,
                                             Qty = grp.Sum(x => x.Qty)
                                         }).ToList();

                foreach (ImportInvoice oItem in _oImportLC.ImportInvoices)
                    oItem.ImportInvoiceDetails = oImportInvoiceDetails.Where(x => x.ImportInvoiceID == oItem.ImportInvoiceID).ToList();
            }
            else
            {
                _oImportLC = new ImportLC();
                _oImportLC.ImportInvoices = new List<ImportInvoice>();
            }
            ViewBag.ImportPIDetails = oImportPIDetails;
            return View(_oImportLC);
        }
        public ActionResult ViewSendToCnf(int id, decimal ts)
        {
            _oImportInvoice = new ImportInvoice();
            ImportCnf oImportCnf = new ImportCnf();
            if (id > 0)
            {
                _oImportInvoice = _oImportInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                oImportCnf = oImportCnf.GetBy(id, (int)Session[SessionInfo.currentUserID]);
              
            }
            ViewBag.ImportCnf = oImportCnf;
            return View(_oImportInvoice);
        }
        public ActionResult ViewDocRelease(int id, decimal ts)
        {
            _oImportInvoice = new ImportInvoice();
            List<ImportInvoiceHistory> oImportInvoiceHistorys = new List<ImportInvoiceHistory>();
            if (id > 0)
            {
                _oImportInvoice = _oImportInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                oImportInvoiceHistorys = ImportInvoiceHistory.GetsByInvoiceEvent(id, ((int)EnumInvoiceEvent.DocReceive_By_Bank).ToString() + "," + ((int)EnumInvoiceEvent.DocIn_Hand).ToString(), (int)Session[SessionInfo.currentUserID]);

            }
            ViewBag.ImportInvoiceHistorys = oImportInvoiceHistorys;
            return View(_oImportInvoice);
        }
        public ActionResult ViewImportBL(int id, decimal ts)
        {
            _oImportInvoice = new ImportInvoice();
            ImportBL oImportBL = new ImportBL();
            List<ImportInvoiceHistory> oImportInvoiceHistorys = new List<ImportInvoiceHistory>();
            if (id > 0)
            {
                _oImportInvoice = _oImportInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                oImportBL = oImportBL.GetByInvoice(id, (int)Session[SessionInfo.currentUserID]);
            }
            List<RouteLocation> oRouteLocations = new List<RouteLocation>();
            ViewBag.RouteLocations = RouteLocation.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ImportBL = oImportBL;
            ViewBag.LocationTypes = EnumObject.jGets(typeof(EnumRouteLocation));
            return View(_oImportInvoice);
        }
        public ActionResult ViewUpdateCustomInfo(int id)
        {
            _oImportInvoice = new ImportInvoice();
            ImportCnf oImportCnf = new ImportCnf(); 
            List<ImportInvoiceHistory> oImportInvoiceHistorys = new List<ImportInvoiceHistory>();

            if (id > 0)
            {
                _oImportInvoice = _oImportInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                oImportInvoiceHistorys = ImportInvoiceHistory.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oImportCnf = oImportCnf.GetBy(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.ImportInvoiceHistorys = oImportInvoiceHistorys;
            ViewBag.ImportCnf = oImportCnf;
            return View(_oImportInvoice);
        }
        public ActionResult ViewImportInvoiceAcceptance(int id, bool bIsApprove, decimal ts)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();
            oImportInvoice.ImportInvoiceHistory = new ImportInvoiceHistory();
            ImportInvoiceHistory oImportInvoiceHistory = new ImportInvoiceHistory();
            if (id > 0)
            {
                oImportInvoice = oImportInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportInvoice.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportInvoice.LCTermsName = oImportInvoice.LCTermsName + " From the Date of " + ((EnumPaymentInstruction)oImportInvoice.PaymentInstructionType).ToString();

                if (oImportInvoice.BankStatus != EnumInvoiceBankStatus.ABP)
                {
                    if (oImportInvoice.PaymentInstructionType == (int)EnumPaymentInstruction.BL)
                    {
                        if (oImportInvoice.DateofMaturity == DateTime.MinValue)
                        {
                            if (oImportInvoice.BLDate == DateTime.MinValue) { oImportInvoice.BLDate = DateTime.Now; }

                            oImportInvoice.DateofMaturity = oImportInvoice.BLDate.AddDays(oImportInvoice.Tenor);
                        }
                        oImportInvoice.LCTermsName = oImportInvoice.LCTermsName + ",  BL Date:" + oImportInvoice.BLDateSt;
                    }
                    else if (oImportInvoice.PaymentInstructionType == (int)EnumPaymentInstruction.Negotiation)
                    {
                        if (oImportInvoice.DateofMaturity == DateTime.MinValue)
                        {
                            if (oImportInvoice.DateofNegotiation == DateTime.MinValue)
                            {
                                oImportInvoice.DateofNegotiation = DateTime.Now;
                            }
                            if (oImportInvoice.DateofMaturity == DateTime.MinValue)
                            {
                                oImportInvoice.DateofMaturity = oImportInvoice.DateofNegotiation.AddDays(oImportInvoice.Tenor);
                            }
                        }
                        oImportInvoice.LCTermsName = oImportInvoice.LCTermsName + ", Negotiation Date:" + oImportInvoice.BLDate.ToString("dd MMM yyyy");
                    }
                    else if (oImportInvoice.PaymentInstructionType == (int)EnumPaymentInstruction.Acceptence)
                    {
                        if (oImportInvoice.DateofAcceptance == DateTime.MinValue)
                        {
                            oImportInvoice.DateofAcceptance = DateTime.Now;
                        }
                        if (oImportInvoice.DateofMaturity == DateTime.MinValue)
                        {
                            oImportInvoice.DateofMaturity = oImportInvoice.DateofAcceptance.AddDays(oImportInvoice.Tenor);
                        }
                        oImportInvoice.LCTermsName = oImportInvoice.LCTermsName + ", Acceptence Date:" + oImportInvoice.DateofAcceptance.ToString("dd MMM yyyy");
                    }
                    else if (oImportInvoice.PaymentInstructionType == (int)EnumPaymentInstruction.LCOpen)
                    {
                        if (oImportInvoice.DateofMaturity == DateTime.MinValue)
                        {
                            if (oImportInvoice.DateofNegotiation == DateTime.MinValue)
                            {
                                oImportInvoice.DateofNegotiation = DateTime.Now;
                            }
                            if (oImportInvoice.DateofMaturity == DateTime.MinValue)
                            {
                                oImportInvoice.DateofMaturity = oImportInvoice.ImportLCDate.AddDays(oImportInvoice.Tenor);
                            }
                        }
                        oImportInvoice.LCTermsName = oImportInvoice.LCTermsName + ", LC Date:" + oImportInvoice.ImportLCDate.ToString("dd MMM yyyy");
                    }
                }
                
                 //oImportInvoice.BankStatus = EnumInvoiceBankStatus.ABP;
                Company oCompany = new Company();
                oImportInvoice.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oImportInvoiceHistory = oImportInvoiceHistory.Get((int)oImportInvoice.InvoiceStatus, (int)oImportInvoice.BankStatus, oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportInvoice.ImportInvoiceHistory = oImportInvoiceHistory;
            }
            return View(oImportInvoice);
        }

        #region Search By Lc no and Invoice No
        public ActionResult SearchByLcNo(string sUserInput)
        {
            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            string sSQL = "SELECT * FROM View_ImportInvoice WHERE LCNo Like '%" + sUserInput + "%'";
            oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oImportInvoices);
        }

        public JsonResult GetsByInvoiceNo(ImportInvoice oImportInvoice)
        {
            string sSQL = "";
            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
          
            sSQL = "SELECT top(100)* FROM View_ImportInvoice WHERE ImportInvoiceNo Like '%" + oImportInvoice.ImportInvoiceNo + "%'";
            if (oImportInvoice.BUID > 0) { sSQL = sSQL + " and BUID=" + oImportInvoice.BUID; }
            sSQL = sSQL + " Order by ImportInvoiceID Desc";
            oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchbyInvoiceNo(string sUserInput)
        {
            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            string sSQL = "SELECT * FROM View_ImportInvoice WHERE PurchaseInvoiceNo Like '%" + sUserInput + "%'";
            oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oImportInvoices);
        }
        #endregion

        #region Operation
        [HttpPost]
        public JsonResult Save_UpdateStatus(ImportInvoice oImportInvoice)
        {
            oImportInvoice.RemoveNulls();
            try
            {

            _oImportInvoice = oImportInvoice;
            _oImportInvoice.BankStatus=(EnumInvoiceBankStatus)_oImportInvoice.BankStatusInt;
            _oImportInvoice.InvoiceStatus = (EnumInvoiceEvent)_oImportInvoice.InvoiceStatusInt;
            _oImportInvoice = _oImportInvoice.Save_UpdateStatus(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(ImportInvoice oImportInvoice)
        {
            try
            {
                oImportInvoice.InvoiceType = (EnumImportPIType)oImportInvoice.InvoiceTypeInt;
                oImportInvoice.InvoiceStatus = (EnumInvoiceEvent)oImportInvoice.InvoiceStatusInt;
                oImportInvoice.BankStatus = (EnumInvoiceBankStatus)oImportInvoice.BankStatusInt;
                _oImportInvoice = oImportInvoice.Save((int)Session[SessionInfo.currentUserID]);
                if (_oImportInvoice.ImportInvoiceID > 0)
                {
                    _oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(_oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ImportInvoice oImportInvoice)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oImportInvoice.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsImportPIProduct(ImportInvoice oImportInvoice)
        {
            List<ImportPIDetail> oImportPIDetails = new List<ImportPIDetail>();
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            ImportInvoiceDetail oImportInvoiceDetail = new ImportInvoiceDetail();
            if (oImportInvoice.ImportLCID > 0)
            {
                oImportPIDetails = ImportPIDetail.Gets("Select * from View_ImportPIDetail WHERE ImportPIID in (Select ImportPIID from ImportLCDetail where  activity=1 and ImportLCID =" + oImportInvoice.ImportLCID + ")", (int)Session[SessionInfo.currentUserID]);
            }
            foreach (ImportPIDetail oItem in oImportPIDetails)
            {
                oImportInvoiceDetail = new ImportInvoiceDetail();
                oImportInvoiceDetail.ProductCode = oItem.ProductCode;
                oImportInvoiceDetail.ProductName = oItem.ProductName;
                oImportInvoiceDetail.ProductID = oItem.ProductID;
                oImportInvoiceDetail.MUnitID = oItem.MUnitID;
                oImportInvoiceDetail.MUName = oItem.MUName;
                oImportInvoiceDetail.MUSymbol = oItem.MUName;
                oImportInvoiceDetail.Qty = oItem.Qty - oItem.InvoiceQty;
                oImportInvoiceDetail.UnitPrice = oItem.UnitPrice + oItem.FreightRate;
                oImportInvoiceDetail.RateUnit = oItem.RateUnit;
                oImportInvoiceDetail.Amount = (oImportInvoiceDetail.Qty / oItem.RateUnit) * (oItem.UnitPrice+oItem.FreightRate);
                oImportInvoiceDetail.ImportPIDetailID = oItem.ImportPIDetailID;                
                if (oImportInvoiceDetail.Qty > 0)
                {
                    oImportInvoiceDetails.Add(oImportInvoiceDetail);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oImportInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult BankInfoCollect(ImportInvoice oImportInvoice)
        {
            try
            {
                _oImportInvoice = oImportInvoice;
                _oImportInvoice.InvoiceType = (EnumImportPIType)_oImportInvoice.InvoiceTypeInt;
                _oImportInvoice.BankStatus = EnumInvoiceBankStatus.Document_In_HandBank;
                _oImportInvoice.InvoiceStatus = EnumInvoiceEvent.DocReceive_By_Bank;
                _oImportInvoice.ProductionApprovalStatus = EnumApprovalStatus.None;
                _oImportInvoice = _oImportInvoice.SavePIHistory( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TakeOutOrginalDoc(ImportInvoice oImportInvoice)
        {
            try
            {
                _oImportInvoice = oImportInvoice;
                _oImportInvoice.InvoiceType = (EnumImportPIType)oImportInvoice.InvoiceTypeInt;
                _oImportInvoice.BankStatus = EnumInvoiceBankStatus.WaitFor_Acceptance;
                _oImportInvoice.InvoiceStatus = EnumInvoiceEvent.DocIn_Hand;
                _oImportInvoice.ProductionApprovalStatus = EnumApprovalStatus.None;
                _oImportInvoice = _oImportInvoice.SavePIHistory( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ImportInvoiceAcceptance(ImportInvoice oImportInvoice)
        {
            try
            {
                _oImportInvoice = oImportInvoice;
                _oImportInvoice.InvoiceType = (EnumImportPIType)oImportInvoice.InvoiceTypeInt;
                _oImportInvoice.BankStatus = EnumInvoiceBankStatus.ABP;
                _oImportInvoice.InvoiceStatus = EnumInvoiceEvent.DocIn_Hand;
                _oImportInvoice = _oImportInvoice.SavePIHistory( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveAccptanceVoucher(ImportInvoice oImportInvoice)
        {
            try
            {
                _oImportInvoice = oImportInvoice;
                _oImportInvoice.InvoiceType = (EnumImportPIType)oImportInvoice.InvoiceTypeInt;
                _oImportInvoice.BankStatus = EnumInvoiceBankStatus.ABP;
                _oImportInvoice.InvoiceStatus = EnumInvoiceEvent.Shipment_Done;
                _oImportInvoice.ProductionApprovalStatus = EnumApprovalStatus.None;
                _oImportInvoice = _oImportInvoice.SaveAccptanceVoucher((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSendToCnF(ImportInvoice oImportInvoice)
        {
            ImportCnf oImportCnf = new ImportCnf();
            oImportCnf = oImportInvoice.ImportCnf;
            try
            {
                _oImportInvoice = oImportInvoice;
                _oImportInvoice.InvoiceType = (EnumImportPIType)_oImportInvoice.InvoiceTypeInt;
                _oImportInvoice.InvoiceStatus = EnumInvoiceEvent.Doc_In_CnF;
                _oImportInvoice.ProductionApprovalStatus = EnumApprovalStatus.None;
                oImportCnf = oImportCnf.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportInvoice.ImportCnf = oImportCnf;
            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveCustomInfo(ImportInvoice oImportInvoice)
        {
            try
            {
                _oImportInvoice = oImportInvoice;
                _oImportInvoice.InvoiceType = (EnumImportPIType)_oImportInvoice.InvoiceTypeInt;
                _oImportInvoice.InvoiceStatus = (EnumInvoiceEvent)oImportInvoice.InvoiceStatusInt;
                _oImportInvoice.BankStatus = EnumInvoiceBankStatus.None;
                _oImportInvoice.ProductionApprovalStatus = EnumApprovalStatus.None;
                _oImportInvoice = _oImportInvoice.SavePIHistory(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveDeliveryNotice(ImportInvoice oImportInvoice)
        {
            bool result = _oImportInvoice.SaveDeliveryNotice(oImportInvoice,(int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(result);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsPInvoiceProduct(int nPLCID)
        {
            List<ImportLCDetailProduct> oImportPIDetails = new List<ImportLCDetailProduct>();
            List<ImportInvoiceDetail> oPInvoiceDetails = new List<ImportInvoiceDetail>();
            ImportInvoiceDetail oPInvoiceDetail = new ImportInvoiceDetail();
            if (nPLCID > 0)
            {
                oImportPIDetails = ImportLCDetailProduct.GetsByLCID(nPLCID, (int)Session[SessionInfo.currentUserID]);
            }
            foreach (ImportLCDetailProduct oItem in oImportPIDetails)
            {
                oPInvoiceDetail = new ImportInvoiceDetail();
                oPInvoiceDetail.ProductCode = oItem.ProductCode;
                oPInvoiceDetail.ProductName = oItem.ProductName;
                oPInvoiceDetail.ProductID = oItem.ProductID;
                oPInvoiceDetail.MUnitID = oItem.MeasurementUnitID;
                oPInvoiceDetail.Qty = oItem.Quantity - oItem.InvoiceQty;
                oPInvoiceDetail.UnitPrice = oItem.UnitPrice;
                oPInvoiceDetail.ImportInvoiceID = oItem.PCDetailID;
                if (oPInvoiceDetail.Qty > 0)
                {
                    oPInvoiceDetails.Add(oPInvoiceDetail);
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateAmount(ImportInvoice oImportInvoice)
        {
            _oImportInvoice = new ImportInvoice();
            try
            {
                _oImportInvoice = oImportInvoice.UpdateAmount((int)Session[SessionInfo.currentUserID]);
                _oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(_oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDetail(ImportInvoiceDetail oImportInvoiceDetail)
        {
            string sErrorMease = "";
            try
            {

                sErrorMease = oImportInvoiceDetail.Delete((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Load Import Invoices

        [HttpPost]
        public JsonResult RefreshPurchaseInvoice(ImportInvoice oImportInvoice)
        {
            _oImportInvoices = new List<ImportInvoice>();
            try
            {

                _oImportInvoices = ImportInvoice.Gets(oImportInvoice.ImportLCID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oImportInvoices = new List<ImportInvoice>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        
        [HttpPost]
        public JsonResult UpdateCommission(ImportInvoice oImportInvoice)
        {
            try
            {
                _oImportInvoice = oImportInvoice;
                _oImportInvoice = _oImportInvoice.UpdateCommission(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportInvoice = new ImportInvoice();
                _oImportInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View Advance Search
        [HttpPost]
        public JsonResult GetsSearchedData(ImportInvoice oPurchaseInvoice)
        {
            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            try
            {
                string sSQL = GetSQL(oPurchaseInvoice);
                oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportInvoices = new List<ImportInvoice>();
            }
            var jsonResult = Json(oImportInvoices, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQL(ImportInvoice oImportInvoice)
        {
            int nDateofInvoice = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[2]);
            int nDateofReceive = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[3]);
            DateTime dReceiveStartDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[4]);
            DateTime dReceiveEndDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[5]);
            int nDateofMaturity = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[6]);
            DateTime dMaturityStartDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[7]);
            DateTime dMaturityEndDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[8]);
            int nDateofNegotiation = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[9]);
            DateTime dNegotiationStartDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[10]);
            DateTime dNegotiationEndDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[11]);
            int nDateofAcceptance = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[12]);
            DateTime dAcceptanceStartDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[13]);
            DateTime dAcceptanceEndDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[14]);

            int nComboAmount = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[15]);
            double nAmountStart = Convert.ToDouble(oImportInvoice.ErrorMessage.Split('~')[16]);
            double nAmountEnd = Convert.ToDouble(oImportInvoice.ErrorMessage.Split('~')[17]);

            string sImportInvoiceNo = oImportInvoice.ErrorMessage.Split('~')[18];
            string sImportLCNo = oImportInvoice.ErrorMessage.Split('~')[19];
            string sContractorIDs = oImportInvoice.ErrorMessage.Split('~')[20];
            string sInvoiceStatus = oImportInvoice.ErrorMessage.Split('~')[21];
            int nBUID = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[22]);
            int nProductType = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[23]);

            string sImportDateString = oImportInvoice.ErrorMessage.Split('~')[24];
            int nImportDate = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[25]);
            DateTime dImportStartDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[26]);
            DateTime dImportEndDate = Convert.ToDateTime(oImportInvoice.ErrorMessage.Split('~')[27]);
            int nImportPIType = Convert.ToInt32(oImportInvoice.ErrorMessage.Split('~')[28]);
            bool bIsWaiting = Convert.ToBoolean(oImportInvoice.ErrorMessage.Split('~')[29]);
            string sBankStatusIDs = "";
            if(oImportInvoice.ErrorMessage.Split('~').Length > 30)
                sBankStatusIDs = oImportInvoice.ErrorMessage.Split('~')[30];

            string sReturn1 = "SELECT * FROM View_ImportInvoice ";

            #region Import Date String

            if (!string.IsNullOrEmpty(sImportDateString)) 
            {
                DateObject.CompareDateQuery(ref sReturn,sImportDateString, nImportDate, dImportStartDate, dImportEndDate);
            }

            #endregion

            #region ImportInvoiceNo
            if (sImportInvoiceNo != null && sImportInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportInvoiceNo LIKE " + "'%" + sImportInvoiceNo + "%'";
            }
            #endregion

            #region ImportLCNo
            if (sImportLCNo != null && sImportLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportLCNo LIKE " + "'%" + sImportLCNo + "%'";
            }
            #endregion

            #region Supplier id
            if (sContractorIDs != null && sContractorIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN(" + sContractorIDs + ")";
            }
            #endregion

            #region status
            if (sInvoiceStatus != null && sInvoiceStatus != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " InvoiceStatus IN(" + sInvoiceStatus + ")";
            }
            #endregion

            #region  Date of invoice Wise
            if (nDateofInvoice > 0)
            {
                if (nDateofInvoice == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofInvoice>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND DateofInvoice < '" + dStartDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nDateofInvoice == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofInvoice != '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofInvoice == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofInvoice > '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofInvoice == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofInvoice < '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofInvoice == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofInvoice>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND DateofInvoice < '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nDateofInvoice == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofInvoice< '" + dStartDate + "' OR DateofInvoice > '" + dEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Date Of Receive
            if (nDateofReceive > 0)
            {
                if (nDateofReceive == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofReceive>= '" + dReceiveStartDate.ToString("dd MMM yyyy") + "' AND DateofReceive < '" + dReceiveStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateofReceive == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofReceive != '" + dReceiveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofReceive == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofReceive > '" + dReceiveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofReceive == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofReceive < '" + dReceiveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofReceive == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofReceive>= '" + dReceiveStartDate.ToString("dd MMM yyyy") + "' AND DateofReceive < '" + dReceiveEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateofReceive == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofReceive< '" + dReceiveStartDate.ToString("dd MMM yyyy") + "' OR DateofReceive > '" + dReceiveEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Dateof Maturity Date Wise
            if (nDateofMaturity > 0)
            {
                if (nDateofMaturity == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofMaturity>= '" + dMaturityStartDate.ToString("dd MMM yyyy") + "' AND DateofMaturity < '" + dMaturityStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateofMaturity == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofMaturity != '" + dMaturityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofMaturity == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofMaturity > '" + dMaturityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofMaturity == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofMaturity < '" + dMaturityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofMaturity == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofMaturity>= '" + dMaturityStartDate.ToString("dd MMM yyyy") + "' AND DateofMaturity < '" + dMaturityEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateofMaturity == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofMaturity< '" + dMaturityStartDate.ToString("dd MMM yyyy") + "' OR DateofMaturity > '" + dMaturityEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Dateof Nego Date Wise
            if (nDateofNegotiation > 0)
            {
                if (nDateofNegotiation == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofNegotiation>= '" + dMaturityStartDate.ToString("dd MMM yyyy") + "' AND DateofNegotiation < '" + dMaturityStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateofNegotiation == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofNegotiation != '" + dMaturityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofNegotiation == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofNegotiation > '" + dMaturityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofNegotiation == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofNegotiation < '" + dMaturityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofNegotiation == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofNegotiation>= '" + dMaturityStartDate.ToString("dd MMM yyyy") + "' AND DateofNegotiation < '" + dMaturityEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateofNegotiation == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofNegotiation< '" + dMaturityStartDate.ToString("dd MMM yyyy") + "' OR DateofNegotiation > '" + dMaturityEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Dateof Acceptance Date Wise
            if (nDateofAcceptance > 0)
            {
                if (nDateofAcceptance == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofAcceptance>= '" + dMaturityStartDate.ToString("dd MMM yyyy") + "' AND DateofAcceptance < '" + dMaturityStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateofAcceptance == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofAcceptance != '" + dMaturityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofAcceptance == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofAcceptance > '" + dMaturityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofAcceptance == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DateofAcceptance < '" + dMaturityStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nDateofAcceptance == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofAcceptance>= '" + dMaturityStartDate.ToString("dd MMM yyyy") + "' AND DateofAcceptance < '" + dMaturityEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDateofAcceptance == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (DateofAcceptance< '" + dMaturityStartDate.ToString("dd MMM yyyy") + "' OR DateofAcceptance > '" + dMaturityEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Amount  Wise
            if (nComboAmount > 0)
            {
                if (nComboAmount == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount = " + nAmountStart;
                }
                if (nComboAmount == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount != " + nAmountStart;
                }
                if (nComboAmount == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount > " + nAmountStart;
                }
                if (nComboAmount == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount < " + nAmountStart;
                }
                if (nComboAmount == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (Amount>= " + nAmountStart + " AND Amount < " + nAmountEnd + ")";
                }
                if (nComboAmount == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (Amount <" + nAmountStart + " OR Amount > " + nAmountEnd + ")";
                }
            }
            #endregion

            #region Bu
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion
            #region ProductType
            if (nProductType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductType = " + nProductType;
            }
            #endregion

            #region Import PI Type
            if (nImportPIType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " InvoiceType = " + nImportPIType;
            }
            #endregion

            #region Waiting for maturity date
            if (bIsWaiting)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(DateOfMaturity,'') = '' AND BankStatus = " + (int)EnumInvoiceBankStatus.ABP;
            }
            #endregion

            #region Bank status
            if (!string.IsNullOrEmpty(sBankStatusIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BankStatus IN(" + sBankStatusIDs + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY DateofReceive, ImportInvoiceNo ASC";

            return sReturn;
        }

        [HttpGet]
        public JsonResult GetbyLCNo(string PLCNo, int BUID, double ts)
        {
            PLCNo = PLCNo.Trim();
            _oImportInvoices = new List<ImportInvoice>();
            string sSQL = "SELECT * FROM View_ImportInvoice where BUID = " + BUID + " AND ImportLCNo like '%" + PLCNo + "%' ";
            _oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetbyPInvoiceNo(string PInvoiceNo, int BUID, double ts)
        {
            PInvoiceNo = PInvoiceNo.Trim();
            _oImportInvoices = new List<ImportInvoice>();
            string sSQL = "SELECT * FROM View_ImportInvoice where  BUID = "+BUID+" AND ImportInvoiceNo like '%" + PInvoiceNo + "%' ";
            _oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Pakcing List
        public ActionResult ViewImportInvoicePackingListPI(int id)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            ImportPI oImportPI = new ImportPI();
            ImportInvoice oImportInvoice = new ImportInvoice();
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            _oImportPackDetails = new List<ImportPackDetail>();
            List<ImportPack> oImportPacks = new List<ImportPack>();
            ImportPack oImportPack = new ImportPack();
            Contractor oContractor = new Contractor();
            int nCount = 0;
           
            if (id > 0)
            {
                oImportPI = oImportPI.Get(id, (int)Session[SessionInfo.currentUserID]);
                oContractor = oContractor.Get(oImportPI.SupplierID, (int)Session[SessionInfo.currentUserID]);
                oImportInvoice = oImportInvoice.Get(oImportPI.ImportPITypeInt, id, (int)Session[SessionInfo.currentUserID]);
                _oImportPack.ImportPacks = ImportPack.Gets(oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
                if (_oImportPack.ImportPacks.Count > 0)
                {
                    _oImportPack.ImportPackDetails = ImportPackDetail.GetsByInvioce(oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);

                }
                oImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
                oImportInvoice.ImportInvoiceDetails = oImportInvoiceDetails;
                oImportInvoiceDetails = oImportInvoiceDetails.GroupBy(x => new { x.ImportInvoiceID, x.ProductID, x.ProductCode, x.ProductName, x.MUnitID, x.MUName, x.MUSymbol }, (key, grp) =>
                                       new ImportInvoiceDetail
                                       {
                                           ImportInvoiceID = key.ImportInvoiceID,
                                           ProductID = key.ProductID,
                                           ProductCode = key.ProductCode,
                                           ProductName = key.ProductName,
                                           MUnitID = key.MUnitID,
                                           MUName = key.MUName,
                                           MUSymbol = key.MUSymbol,
                                           Qty = grp.Sum(p => p.Qty)
                                       }).ToList();

                foreach (ImportInvoiceDetail oItem in oImportInvoiceDetails)
                {
                    oImportPack = new ImportPack();
                    oImportPack.ImportInvoiceID = oImportInvoice.ImportInvoiceID;
                    oImportPack.ProductID = oItem.ProductID;
                    oImportPack.ProductCode = oItem.ProductCode;
                    oImportPack.ProductName = oItem.ProductName;
                    oImportPack.MUnitID = oItem.MUnitID;
                    oImportPack.MUName = oItem.MUName;
                    oImportPack.Qty = oItem.Qty;
                    oImportPack.InvoiceQty = oItem.Qty;
                    nCount = _oImportPack.ImportPacks.Where(x => x.ProductID == oItem.ProductID).ToList().Count;
                    if (nCount <= 0)
                    {
                        oImportPacks.Add(oImportPack);
                    }
                }
                if (oImportPacks.Any() && oImportPacks.FirstOrDefault().ProductID > 0)
                {
                    oImportPacks.ForEach(x => _oImportPack.ImportPacks.Add(x));
                }
                //  oImportPIDetails.ForEach(x => { x.Qty = x.Qty - oImportInvoiceDetails.Where(p => p.ImportPIDetailID == x.ImportPIDetailID).Sum(o => o.Qty); });
                //_oImportPack.ImportPacks.RemoveAll(x => x.ImportPackID <= 0 && x.ProductID==1);

                _oImportPack.ImportInvoiceID = oImportInvoice.ImportInvoiceID;
                _oImportPack.ImportInvoiceNo = oImportInvoice.ImportInvoiceNo;
                _oImportPack.ImportLCNo = oImportInvoice.ImportLCNo;
                _oImportPack.Origin = oContractor.Origin;
            }
            ViewBag.ImportInvoice = oImportInvoice;
            ViewBag.ImportInvoiceDetails = oImportInvoice.ImportInvoiceDetails;
            ViewBag.PacksCountBy = EnumObject.jGets(typeof(EnumPackCountBy));
            return View(_oImportPack);
        }
        public ActionResult ViewImportInvoicePackingList(int id)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            

            ImportInvoice oImportInvoice = new ImportInvoice();
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            _oImportPackDetails = new List<ImportPackDetail>();
            List<ImportPack> oImportPacks = new List<ImportPack>();
            ImportPack oImportPack = new ImportPack();
            Contractor oContractor = new Contractor();
            int nCount = 0;
            oImportInvoice = oImportInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (id > 0)
            {
                _oImportPack.ImportPacks = ImportPack.Gets(id, (int)Session[SessionInfo.currentUserID]);
                if (_oImportPack.ImportPacks.Count > 0)
                {
                    _oImportPack.ImportPackDetails = ImportPackDetail.GetsByInvioce(id, (int)Session[SessionInfo.currentUserID]);
                   
                }
                oContractor = oContractor.Get(oImportInvoice.ContractorID, (int)Session[SessionInfo.currentUserID]);
                 oImportInvoiceDetails = ImportInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                 oImportInvoice.ImportInvoiceDetails = oImportInvoiceDetails;

                 if (oImportInvoiceDetails.Count > 0)
                 {
                     oMeasurementUnitCon = oMeasurementUnitCon.GetByMUnit(oImportInvoiceDetails[0].MUnitID, (int)Session[SessionInfo.currentUserID]);
                 }

                 

                 oImportInvoiceDetails = oImportInvoiceDetails.GroupBy(x => new { x.ImportInvoiceID, x.ProductID, x.ProductCode, x.ProductName, x.MUnitID, x.MUName, x.MUSymbol}, (key, grp) =>
                                        new ImportInvoiceDetail
                                        {
                                            ImportInvoiceID = key.ImportInvoiceID,
                                            ProductID = key.ProductID,
                                            ProductCode = key.ProductCode,
                                            ProductName = key.ProductName,
                                            MUnitID = key.MUnitID,
                                            MUName = key.MUName,
                                            MUSymbol = key.MUSymbol,
                                            Qty = grp.Sum(p => p.Qty)
                                        }).ToList();

                  foreach (ImportInvoiceDetail oItem in oImportInvoiceDetails)
                  {
                      oImportPack = new ImportPack();
                      oImportPack.ImportInvoiceID = oImportInvoice.ImportInvoiceID;
                      oImportPack.ProductID = oItem.ProductID;
                      oImportPack.ProductCode = oItem.ProductCode;
                      oImportPack.ProductName = oItem.ProductName;
                      oImportPack.MUnitID = oItem.MUnitID;
                      oImportPack.MUName = oItem.MUName;
                      oImportPack.Qty = oItem.Qty;
                      oImportPack.InvoiceQty = oItem.Qty;
                      oImportPack.MUNameTwo = oMeasurementUnitCon.ToMUnit;
                      oImportPack.UnitConValue = oMeasurementUnitCon.Value;
                      _oImportPack.Origin = oContractor.Origin;

                      nCount = _oImportPack.ImportPacks.Where(x => x.ProductID == oItem.ProductID).ToList().Count;
                      if (nCount <= 0)
                      {
                          oImportPacks.Add(oImportPack);
                      }
                  }
                  if (oImportPacks.Any() && oImportPacks.FirstOrDefault().ProductID > 0)
                  {
                      oImportPacks.ForEach(x => _oImportPack.ImportPacks.Add(x));
                  }
                  //  oImportPIDetails.ForEach(x => { x.Qty = x.Qty - oImportInvoiceDetails.Where(p => p.ImportPIDetailID == x.ImportPIDetailID).Sum(o => o.Qty); });
                  //_oImportPack.ImportPacks.RemoveAll(x => x.ImportPackID <= 0 && x.ProductID==1);

                _oImportPack.ImportInvoiceID = oImportInvoice.ImportInvoiceID;
                _oImportPack.ImportInvoiceNo = oImportInvoice.ImportInvoiceNo;
                _oImportPack.ImportLCNo = oImportInvoice.ImportLCNo;
                _oImportPack.MUNameTwo = oMeasurementUnitCon.ToMUnit;
                _oImportPack.UnitConValue =Math.Round(oMeasurementUnitCon.Value,5);
                _oImportPack.Origin = oContractor.Origin;
                _oImportPack.ImportPacks.ForEach(o => o.UnitConValue = oMeasurementUnitCon.Value);
            }
            ViewBag.ImportInvoice = oImportInvoice;
            ViewBag.ImportInvoiceDetails = oImportInvoice.ImportInvoiceDetails;
            ViewBag.MeasurementUnitCon = oMeasurementUnitCon;
            ViewBag.PacksCountBy = EnumObject.jGets(typeof(EnumPackCountBy));
            return View(_oImportPack);
        }

        [HttpPost]
        public JsonResult ResetPackingList(ImportInvoice oImportInvoice)
        {   
            ImportBL oImportBL = new ImportBL();
            _oImportPackDetails = new List<ImportPackDetail>();
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();

            oImportInvoice = oImportInvoice.Get(oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            oImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            //oImportBL = oImportBL.GetByInvoice(oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            _oImportPack = new ImportPack();
            _oImportPack.ImportInvoiceID = oImportInvoice.ImportInvoiceID;
            _oImportPack.ImportInvoiceNo = oImportInvoice.ImportInvoiceNo;
            _oImportPack.ImportLCNo = oImportInvoice.ImportLCNo;
            //_oImportPack.LoadingPortID = oImportBL.LandingPort;
            //_oImportPack.DischargePortID = oImportBL.DestinationPort;
            foreach (ImportInvoiceDetail oItem in oImportInvoiceDetails)
            {
                _oImportPackDetail = new ImportPackDetail();
                _oImportPackDetail.ImportInvoiceDetailID = oItem.ImportInvoiceDetailID;
                _oImportPackDetail.ProductID = oItem.ProductID;
                _oImportPackDetail.ProductCode = oItem.ProductCode;
                _oImportPackDetail.ProductName = oItem.ProductName;
                _oImportPackDetail.MUnitID = oItem.MUnitID;
                _oImportPackDetail.MUName = oItem.MUName;
                //_oImportPackDetail.MUSymbol = oItem.MUSymbol;
                _oImportPackDetail.Qty = oItem.Qty;
                _oImportPackDetail.InvoiceQty = oItem.Qty;
                _oImportPackDetails.Add(_oImportPackDetail);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oImportPackDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPackingList(ImportPack oImportPack)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            oImportInvoice = oImportInvoice.Get(oImportPack.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            oImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportPack.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            oImportInvoiceDetails = oImportInvoiceDetails.Where(o => o.ProductID == oImportPack.ProductID).ToList();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            if (oImportPack.MUnitID > 0)
            {
                if(oImportPack.MUnitID==7)
                {
                    oMeasurementUnitCon = oMeasurementUnitCon.GetBy(21,oImportPack.MUnitID, (int)Session[SessionInfo.currentUserID]);
                }
                else{ oMeasurementUnitCon = oMeasurementUnitCon.GetByMUnit(oImportPack.MUnitID, (int)Session[SessionInfo.currentUserID]);}
               
            }
            _oImportPack = new ImportPack();
            ImportPackDetail oImportPackDetail = new ImportPackDetail();
            if (oImportPack.ImportPackID > 0)
            {
                _oImportPack = _oImportPack.Get(oImportPack.ImportPackID, (int)Session[SessionInfo.currentUserID]);
                _oImportPack.ImportPackDetails = ImportPackDetail.Gets(_oImportPack.ImportPackID, (int)Session[SessionInfo.currentUserID]);
            }
            if (_oImportPack.ImportPackDetails.Count <= 0)
            {
                foreach (ImportInvoiceDetail oItem in oImportInvoiceDetails)
                {
                    oImportPackDetail = new ImportPackDetail();
                    oImportPackDetail.ImportInvoiceDetailID = oItem.ImportInvoiceDetailID;
                    oImportPackDetail.ProductID = oItem.ProductID;
                    oImportPackDetail.ProductCode = oItem.ProductCode;
                    oImportPackDetail.ProductName = oItem.ProductName;
                    oImportPackDetail.MUnitID = oItem.MUnitID;
                    oImportPackDetail.MUName = oItem.MUName;
                    if (oImportPack.MUnitID == 7)
                    {
                        oImportPackDetail.MUNameTwo = oMeasurementUnitCon.FromMUnit;
                    }
                    else
                    {
                        oImportPackDetail.MUNameTwo = oMeasurementUnitCon.ToMUnit;
                    }
                    oImportPackDetail.UnitConValue = Math.Round(oMeasurementUnitCon.Value,5);
                    oImportPackDetail.MURate = ((oMeasurementUnitCon.Value<=0)?1: Math.Round(oMeasurementUnitCon.Value, 5));
                    oImportPackDetail.MUName = oItem.MUSymbol;
                    oImportPackDetail.Qty = oItem.Qty;
                    oImportPackDetail.InvoiceQty = oItem.Qty;
                    _oImportPack.MUName = oItem.MUSymbol;
                    _oImportPack.ImportPackDetails.Add(oImportPackDetail);
                }
            }
            foreach (ImportPackDetail oItem in _oImportPack.ImportPackDetails)
            {
                if (oItem.MURate <= 0)
                { oItem.MURate =  Math.Round(oMeasurementUnitCon.Value, 5); }
            }
            _oImportPack.ImportPackDetails.ForEach(o => o.MUNameTwo = oMeasurementUnitCon.ToMUnit);
            //_oImportPack.ImportPackDetails.ForEach(o => o.UnitConValue = Math.Round(oMeasurementUnitCon.Value, 5));
            if(String.IsNullOrEmpty(_oImportPack.MUName)){ _oImportPack.MUName=oMeasurementUnitCon.FromMUnit;}

            _oImportPack.ImportInvoiceID = oImportInvoice.ImportInvoiceID;
            _oImportPack.ImportInvoiceNo = oImportInvoice.ImportInvoiceNo;
            _oImportPack.ImportLCNo = oImportInvoice.ImportLCNo;
            if (oImportPack.MUnitID == 7)
            {
                _oImportPack.MUNameTwo = oMeasurementUnitCon.FromMUnit;
            }
            else
            {
                _oImportPack.MUNameTwo = oMeasurementUnitCon.ToMUnit;
            }
            _oImportPack.UnitConValue = Math.Round(oMeasurementUnitCon.Value,5);
           

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oImportPack);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePackingList(ImportPack oImportPack)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            try
            {
                oImportPack.PackCountBy = (EnumPackCountBy)oImportPack.PackCountByInInt;
                _oImportPack = oImportPack.Save((int)Session[SessionInfo.currentUserID]);

                if (_oImportPack.MUnitID > 0)
                {
                    oMeasurementUnitCon = oMeasurementUnitCon.GetByMUnit(_oImportPack.MUnitID, (int)Session[SessionInfo.currentUserID]);
                }
                _oImportPack.MUNameTwo = oMeasurementUnitCon.ToMUnit;
                _oImportPack.UnitConValue = Math.Round(oMeasurementUnitCon.Value, 5);
                
            }
            catch (Exception ex)
            {
                _oImportPack = new ImportPack();
                _oImportPack.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPack);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePL(ImportPack oImportPack)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oImportPack.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_FromDO(ImportPack oImportPack)
        {
            try
            {
                oImportPack.PackCountBy = (EnumPackCountBy)oImportPack.PackCountByInInt;
                oImportPack.ErrorMessage = oImportPack.Save_FromDO((int)Session[SessionInfo.currentUserID]);
                _oImportPack = oImportPack;
                _oImportPack.ImportPacks = ImportPack.Gets(oImportPack.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
                if (_oImportPack.ImportPacks.Count > 0)
                {
                    _oImportPack.ImportPackDetails = ImportPackDetail.GetsByInvioce(oImportPack.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oImportPack = new ImportPack();
                _oImportPack.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPack);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         #endregion

        #region Print
      
        public ActionResult PrintTakeOutOriginalDoc(int id)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oImportInvoice = new ImportInvoice();
            ImportPack oImportPack = new ImportPack();
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            BankBranch oBankBranch = new BankBranch();
            _oImportInvoice = _oImportInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(_oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            oBankBranch = BankBranch.Get(_oImportInvoice.BankBranchID_Nego, (int)Session[SessionInfo.currentUserID]);
            _oImportInvoice.BBranchName_Nego = oBankBranch.BranchName;
            _oImportInvoice.BankAddress_Nego = oBankBranch.Address;
            oImportPack = oImportPack.GetByInvoice(_oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            oImportPack.ImportPackDetails = ImportPackDetail.Gets(oImportPack.ImportPackID, (int)Session[SessionInfo.currentUserID]);
            string sLCAppType = "";
            if (_oImportInvoice.ImportLCID > 0)
            {
                _oImportLC = _oImportLC.Get(_oImportInvoice.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);


             
                string sSQL = "";
                if (_oImportLC.LCAppTypeInt > 0) sSQL = sSQL + " And LCAppType in (" + (_oImportLC.LCAppTypeInt).ToString() + ")";
                sSQL = sSQL + " And LCPaymentType in (0," + _oImportLC.LCPaymentTypeInt + ")";
                sSQL = sSQL + " Order By LCPaymentType DESC";
                oImportLetterSetup = oImportLetterSetup.Get((int)EnumImportLetterType.DOC_Release, (int)EnumImportLetterIssueTo.Bank, _oImportLC.BUID, _oImportInvoice.ImportLCID, sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(_oImportLC.BUID, (int)Session[SessionInfo.currentUserID]);

            rptPurchaseTakeOutOrginalDoc oReport = new rptPurchaseTakeOutOrginalDoc();
            byte[] abytes = oReport.PreparePrintLetter(oImportLetterSetup, _oImportInvoice, oCompany, oBusinessUnit,  _oImportLC);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintAcceptanceLetter(int id)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            BankBranch oBankBranch = new BankBranch();
            _oImportInvoice = new ImportInvoice();
            ImportPack oImportPack = new ImportPack();
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            _oImportInvoice = _oImportInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(_oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            oBankBranch = BankBranch.Get(_oImportInvoice.BankBranchID_Nego, (int)Session[SessionInfo.currentUserID]);
            _oImportInvoice.BBranchName_Nego = oBankBranch.BranchName;
            _oImportInvoice.BankAddress_Nego = oBankBranch.Address;
            string sLCAppType = "";
            if (_oImportInvoice.ImportLCID > 0)
            {
                _oImportLC = _oImportLC.Get(_oImportInvoice.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);


                string sSQL = "";
                if (_oImportLC.LCAppTypeInt > 0) sSQL = sSQL + " And LCAppType in (" + (_oImportLC.LCAppTypeInt).ToString() + ")";
                sSQL = sSQL + " And LCPaymentType in (0," + _oImportLC.LCPaymentTypeInt + ")";
                //if (oImportPIs.Count > 0)
                //{
                //    if (oImportPIs[0].ProductType > 0) sSQL = sSQL + " And ProductType in (0," + oImportPIs[0].ProductType + ")";
                //}
                sSQL = sSQL + " Order By LCPaymentType DESC";
                oImportLetterSetup = oImportLetterSetup.Get((int)EnumImportLetterType.Invoice_Acceptance, (int)EnumImportLetterIssueTo.Bank, _oImportLC.BUID, _oImportInvoice.ImportLCID, sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(_oImportLC.BUID, (int)Session[SessionInfo.currentUserID]);

            rptPurchaseTakeOutOrginalDoc oReport = new rptPurchaseTakeOutOrginalDoc();
            byte[] abytes = oReport.PreparePrintLetter(oImportLetterSetup, _oImportInvoice, oCompany, oBusinessUnit,  _oImportLC);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintCnFLetter(string sParams)
        {
            int nInvoiceID = Convert.ToInt32(sParams.Split('~')[0]);
            int nContractorID = Convert.ToInt32(sParams.Split('~')[1]);
            bool bIsConfirm = Convert.ToBoolean(sParams.Split('~')[2]);

          
            ImportPack oImportPack = new ImportPack();
            ImportCnf oImportCnf = new ImportCnf();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oImportInvoice = new ImportInvoice();
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            _oImportInvoice = _oImportInvoice.Get(nInvoiceID, (int)Session[SessionInfo.currentUserID]);
            _oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(_oImportInvoice.ImportInvoiceID, (int)Session[SessionInfo.currentUserID]);
            
            Contractor oContractor = new Contractor();
            Contractor oContractorInv = new Contractor();
            oContractor = oContractor.Get(nContractorID, (int)Session[SessionInfo.currentUserID]);
            oContractorInv = oContractorInv.Get(_oImportInvoice.ContractorID, (int)Session[SessionInfo.currentUserID]);
            List<ImportPack> oImportPacks = new List<ImportPack>();
            List<ImportPackDetail> oImportPackDetails = new List<ImportPackDetail>();
            if (_oImportInvoice.ImportLCID > 0)
            {
                oImportCnf = oImportCnf.GetBy(_oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(_oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportPacks = ImportPack.Gets("select * from View_ImportPack where ImportInvoiceID=" + _oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportPackDetails = ImportPackDetail.Gets("select * from View_ImportPackDetail where ImportPackID in (select ImportPackID from ImportPack where ImportInvoiceID=" + _oImportInvoice.ImportInvoiceID + ") order by ProductID, LotNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ImportPack oitem in oImportPacks)
                {
                    oitem.ImportPackDetails = oImportPackDetails.Where(x => x.ImportPackID == oitem.ImportPackID).ToList();
                }
                _oImportLC = _oImportLC.Get(_oImportInvoice.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "";
                if (_oImportLC.LCAppTypeInt > 0) sSQL = sSQL + " And LCAppType in (" + (_oImportLC.LCAppTypeInt).ToString() + ")";
                sSQL = sSQL + " And LCPaymentType in (0," + _oImportLC.LCPaymentTypeInt + ")";
                //if (oImportPIs.Count > 0)
                //{
                //    if (oImportPIs[0].ProductType > 0) sSQL = sSQL + " And ProductType in (0," + oImportPIs[0].ProductType + ")";
                //}
                sSQL = sSQL + " Order By LCPaymentType DESC";
                oImportLetterSetup = oImportLetterSetup.Get((int)EnumImportLetterType.CnfLetter, (int)EnumImportLetterIssueTo.Supplier, _oImportLC.BUID, _oImportInvoice.ImportLCID, sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportLetterSetup.ContractorName = oContractor.Name;
                oImportLetterSetup.Address_Supplier = oContractorInv.Address;
                oImportLetterSetup.ContractorAddress = oContractor.Address;
                oImportLetterSetup.Origin = oContractorInv.Origin;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(_oImportLC.BUID, (int)Session[SessionInfo.currentUserID]);

            rptPurchaseTakeOutOrginalDoc oReport = new rptPurchaseTakeOutOrginalDoc();
            byte[] abytes = oReport.PrepareReport_SendToCnFLetter(oImportCnf,oImportLetterSetup, _oImportInvoice, oCompany, oBusinessUnit, oContractor, _oImportLC, oImportPacks, oImportPackDetails);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintInvoice_GRN(int id)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();
            List<GRN> oGRNs = new List<GRN>();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            oImportInvoice = oImportInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oImportInvoice.ImportInvoiceID > 0)
            {
                oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGRNs = GRN.Gets("Select * from View_GRN where  ReceivedBy<>0 and GRNType in (" + (int)EnumGRNType.ImportInvoice + ","+ (int)EnumGRNType.FancyYarn + ") and RefObjectID=" + oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGRNDetails = GRNDetail.Gets("Select * from View_GRNDetail where  GRNID in (Select GRN.GRNID from GRN where  ReceivedBy<>0 and GRNType in (" + (int)EnumGRNType.ImportInvoice + "," + (int)EnumGRNType.FancyYarn + ") and RefObjectID=" + oImportInvoice.ImportInvoiceID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
          

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(oImportInvoice.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oImportInvoice.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptImportGRN oReport = new rptImportGRN();//for plastic and integrated
            byte[] abytes = oReport.PrepareReport(oGRNs, oGRNDetails, oImportInvoice, oContractor, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintPackingList(int id)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();
            List<ImportPack> oImportPacks = new List<ImportPack>();
            List<ImportPackDetail> oImportPackDetails = new List<ImportPackDetail>();
            ImportCnf oImportCnf = new ImportCnf();
            string sIsCnf = "";
            oImportInvoice = oImportInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oImportInvoice.ImportInvoiceID > 0)
            {
               
                oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportPacks = ImportPack.Gets("select * from View_ImportPack where ImportInvoiceID=" + oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportPackDetails = ImportPackDetail.Gets("select * from View_ImportPackDetail where ImportPackID in (select ImportPackID from ImportPack where ImportInvoiceID=" + oImportInvoice.ImportInvoiceID + ") order by ProductID, LotNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ImportPack oitem in oImportPacks)
                {
                    oitem.ImportPackDetails = oImportPackDetails.Where(x => x.ImportPackID == oitem.ImportPackID).ToList();
                }
                
            }


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(oImportInvoice.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oImportInvoice.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptImportGRN oReport = new rptImportGRN();//for plastic and integrated
            byte[] abytes = oReport.Prepare_PackingList(oImportCnf, oImportPacks, oImportPackDetails, oImportInvoice, oContractor, oCompany, oBusinessUnit, sIsCnf);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintPackingList_CNF(int id, string sIsCnf)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();
            List<ImportPack> oImportPacks = new List<ImportPack>();
            List<ImportPackDetail> oImportPackDetails = new List<ImportPackDetail>();
            ImportCnf oImportCnf = new ImportCnf();
            oImportInvoice = oImportInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oImportInvoice.ImportInvoiceID > 0)
            {
                oImportCnf = oImportCnf.GetBy(oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportInvoice.ImportInvoiceDetails = ImportInvoiceDetail.Gets(oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportPacks = ImportPack.Gets("select * from View_ImportPack where ImportInvoiceID=" + oImportInvoice.ImportInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportPackDetails = ImportPackDetail.Gets("select * from View_ImportPackDetail where ImportPackID in (select ImportPackID from ImportPack where ImportInvoiceID=" + oImportInvoice.ImportInvoiceID + ") order by ProductID, LotNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ImportPack oitem in oImportPacks)
                {
                    oitem.ImportPackDetails = oImportPackDetails.Where(x => x.ImportPackID == oitem.ImportPackID).ToList();
                }

            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(oImportInvoice.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oImportInvoice.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptImportGRN oReport = new rptImportGRN();//for plastic and integrated
            byte[] abytes = oReport.Prepare_PackingList(oImportCnf, oImportPacks, oImportPackDetails, oImportInvoice, oContractor, oCompany, oBusinessUnit, sIsCnf);
            return File(abytes, "application/pdf");
        }
      
        #endregion

        #region Get Company Logo
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
        #endregion

        [HttpPost]
        public JsonResult GetsLCSupllier(Contractor oContractor)
        {
            List<Contractor> oContractors = new List<Contractor>();
         if(string.IsNullOrEmpty(oContractor.Name))
         {
             oContractors = Contractor.Gets("Select * from  Contractor where ContractorID in (Select ImportLC.ContractorID from ImportLC where ImportLC.LCCurrentStatus in (1,2,3,45,7,8,9,10))", (int)Session[SessionInfo.currentUserID]);
         }
         else
         {
             oContractors = Contractor.Gets("Select * from  Contractor where name like '%" + oContractor.Name + "%' and ContractorID in (Select ContractorID from ImportLC where ImportLC.LCCurrentStatus in (1,2,3,45,7,8,9,10))", (int)Session[SessionInfo.currentUserID]);
         } 
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsImportLC(ImportLC oImportLC)
        {
            List<ImportLC> oImportLCs = new List<ImportLC>();
            if (string.IsNullOrEmpty(oImportLC.LCNo))
            {
                oImportLCs = ImportLC.Gets("SELECT * FROM [View_ImportLC] where LCCurrentStatus in (1,2,3,45,7,8,9,10) Order By [ImportLCID]", (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oImportLCs = ImportLC.Gets("SELECT * FROM [View_ImportLC] where LCCurrentStatus in (1,2,3,45,7,8,9,10) Order By [ImportLCID]", (int)Session[SessionInfo.currentUserID]);
            } 

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oImportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Autocomplete Gets
        [HttpGet]
        public JsonResult GetsRouteLocations(int Type, string SearchText)
        {   
            List<RouteLocation> oRouteLocations = new List<RouteLocation>();
            SearchText = SearchText == null ? "" : SearchText;
            string sSql = "SELECT * FROM RouteLocation WHERE LocationType = " + Type + " AND Name LIKE '%" + SearchText + "%' ORDER BY Name ASC";
            oRouteLocations = RouteLocation.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            
            var jsonResult = Json(oRouteLocations, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion


        #region Excel

        public void ImportExcel(string sTempString)
        {
            ImportInvoice oImportInvoice = new ImportInvoice();
            oImportInvoice.ErrorMessage = sTempString;

            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            
            string sSQL = GetSQL(oImportInvoice);
            oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            int nMaxColumn = 21;
            int colIndex = 1;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Import Invoice List");
                sheet.Name = "Import Invoice List";

                sheet.Column(1).Width = 6; 
                sheet.Column(2).Width = 20; 
                sheet.Column(3).Width = 20; 
                sheet.Column(4).Width = 20; 
                sheet.Column(5).Width = 20; 
                sheet.Column(6).Width = 20; 
                sheet.Column(7).Width = 20; 
                sheet.Column(8).Width = 20; 
                sheet.Column(9).Width = 20; 
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                sheet.Column(12).Width = 30;
                sheet.Column(13).Width = 30;
                sheet.Column(14).Width = 20;
                sheet.Column(15).Width = 20;
                sheet.Column(16).Width = 20;
                sheet.Column(17).Width = 20;
                sheet.Column(18).Width = 20;
                sheet.Column(19).Width = 20;
                sheet.Column(20).Width = 20;
                sheet.Column(21).Width = 20;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Value = oCompany.Name; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Value = oCompany.Address; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Value = "Import Invoice List"; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;
                #endregion


                if (oImportInvoices.Count <= 0)
                {
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Value = "No data to print."; cell.Merge = true; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Red); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                else
                {
                    #region Column
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Invoice Type"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Current Status"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Status"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payment Type"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "L/C Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Applicant Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Negotiate Bank"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BL No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BL Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ETA Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Negotiation Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Acceptance Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Accepted By"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bill Payment Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex += 1;
                    #endregion
                    #region Body
                    int nSl = 0;
                    foreach (ImportInvoice oItem in oImportInvoices)
                    {
                        colIndex = 1;
                        nSl += 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSl; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ImportInvoiceNo; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InvoiceDateInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InvoiceTypeSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CurrentStatusInSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankStatusInSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LCPaymentType; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ImportLCNo; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "$#,##0.####;($#,##0.####)";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "$#,##0.####;($#,##0.####)";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ImportLCDateInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankName_Nego; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BLNo; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BLDateSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ETADateSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateofNegotiationInST; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateofAcceptanceSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AcceptedByName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateofMaturityST; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateofPaymentSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex += 1;
                    }
                    #endregion
                    #region Total
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 7]; cell.Merge = true; cell.Value = "Total : "; cell.Style.Font.Bold = true; 
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oImportInvoices.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "$#,##0.####;($#,##0.####)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oImportInvoices.Sum(x => x.Amount); cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "$#,##0.####;($#,##0.####)";
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                }


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ImportInvoiceExcel.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion

    }


   
  
}
