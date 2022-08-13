using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using ReportManagement;
using ESimSol.Reports;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ImportOutstandingController : PdfViewController
    {
        #region Declaration 
        ImportOutstanding _oImportOutstanding = new ImportOutstanding();
        List<ImportOutstanding> _oImportOutstandings = new List<ImportOutstanding>();
        string _sDateRange = "";
        #endregion

        public ActionResult ViewImportOutstanding(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oImportOutstanding.StartDate = DateTime.Now;
            _oImportOutstanding.EndDate = DateTime.Now;
            _oImportOutstanding.StartDate = _oImportOutstanding.EndDate.AddYears(-1);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            List<ImportSetup> oImportSetups = new List<ImportSetup>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
                oImportSetups = ImportSetup.Gets(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportSetups = ImportSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.ImportSetups = oImportSetups;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            _oImportOutstandings = new List<ImportOutstanding>();
            try
            {
              //  _oImportOutstandings = ImportOutstanding.Gets(_oImportOutstanding.StartDate, _oImportOutstanding.EndDate.AddDays(1), buid, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

                return View(_oImportOutstandings);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oImportOutstandings);
            }

        }

        public ActionResult ViewImportOutstandingDetail(int buid)
        {
            //ImportOutstanding oImportOutstanding = Newtonsoft.Json.JsonConvert.DeserializeObject<ImportOutstanding>(Jsonstring);
            List<ImportOutStandingDetail> oImportOutStandingDetails = new List<ImportOutStandingDetail>();

            _oImportOutstanding = new ImportOutstanding();
             try
            {
                oImportOutStandingDetails = new List<ImportOutStandingDetail>(); //  ImportOutStandingDetail.Gets(oImportOutstanding.BUID, (int)oImportOutstanding.LCPaymentType, oImportOutstanding.BankBranchID, oImportOutstanding.CurrencyID, oImportOutstanding.StartDate, oImportOutstanding.EndDate, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //ViewBag.ShipmenmentInTransit =oImportOutStandingDetails;// = ImportOutStandingDetail.Gets(oImportOutstanding.BUID, (int)oImportOutstanding.LCPaymentType, oImportOutstanding.BankBranchID, oImportOutstanding.CurrencyID, oImportOutstanding.StartDate, oImportOutstanding.EndDate, 2, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //ViewBag.ShipmenDone  = oImportOutStandingDetails;//ImportOutStandingDetail.Gets(oImportOutstanding.BUID, (int)oImportOutstanding.LCPaymentType, oImportOutstanding.BankBranchID, oImportOutstanding.CurrencyID, oImportOutstanding.StartDate, oImportOutstanding.EndDate, 3, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //ViewBag.DocInBank = oImportOutStandingDetails;// = ImportOutStandingDetail.Gets(oImportOutstanding.BUID, (int)oImportOutstanding.LCPaymentType, oImportOutstanding.BankBranchID, oImportOutstanding.CurrencyID, oImportOutstanding.StartDate, oImportOutstanding.EndDate, 4, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //ViewBag.DocInCnF = oImportOutStandingDetails;// = ImportOutStandingDetail.Gets(oImportOutstanding.BUID, (int)oImportOutstanding.LCPaymentType, oImportOutstanding.BankBranchID, oImportOutstanding.CurrencyID, oImportOutstanding.StartDate, oImportOutstanding.EndDate, 5, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //ViewBag.GoodsInTransit= oImportOutStandingDetails;// = ImportOutStandingDetail.Gets(oImportOutstanding.BUID, (int)oImportOutstanding.LCPaymentType, oImportOutstanding.BankBranchID, oImportOutstanding.CurrencyID, oImportOutstanding.StartDate, oImportOutstanding.EndDate, 6, ((User)Session[SessionInfo.CurrentUser]).UserID);
                return View(oImportOutStandingDetails);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(oImportOutStandingDetails);
            }

        }
        [HttpPost]
        public JsonResult GetsIOSDetail(ImportOutstanding oImportOutstanding)
        {
            List<ImportOutStandingDetail> oImportOutStandingDetails = new List<ImportOutStandingDetail>();
            ImportOutStandingDetail oIOSD = new ImportOutStandingDetail();

            _oImportOutstanding = new ImportOutstanding();
            try
            {
                oImportOutStandingDetails = ImportOutStandingDetail.Gets(oImportOutstanding.BUID, (int)oImportOutstanding.LCPaymentType, oImportOutstanding.BankBranchID, oImportOutstanding.CurrencyID, oImportOutstanding.StartDate, oImportOutstanding.EndDate.AddDays(1), oImportOutstanding.nReportType,Convert.ToInt32(oImportOutstanding.ErrorMessage),((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportOutStandingDetails = new List<ImportOutStandingDetail>();
                 oIOSD = new ImportOutStandingDetail();
                oIOSD.ErrorMessage = ex.Message;
                oImportOutStandingDetails.Add(oIOSD);
            }
            oImportOutStandingDetails=oImportOutStandingDetails.OrderBy(x=>x.LCID).ToList();

            #region Total

            //List<ImportOutStandingDetail> oDetails = new List<ImportOutStandingDetail>();
            //oDetails = oImportOutStandingDetails.GroupBy(x => x.LCID).Select(grp => new ImportOutStandingDetail
            //{
            //    value = grp.Select(p => p.value).FirstOrDefault()
            //}).ToList();

            ImportOutStandingDetail oDetail = new ImportOutStandingDetail();
            //oDetail.InvoiceType = oDetails.Select(x => x.InvoiceType).ToList();
            //oDetail.LCNo = oDetails.Select(x => x.LCNo).ToList();
            //oDetail.InvoiceNo = oDetails.Select(x => x.InvoiceNo).ToList();
            //oDetail.ProductName = oDetails.Select(x => x.ProductName).ToList();
            //oDetail.MUnit = oDetails.Select(x => x.MUnit).ToList();
            oDetail.ContractorName = "Total: ";
            oDetail.Qty = oImportOutStandingDetails.Sum(x => x.Qty) ;
            oDetail.UnitPrice = oImportOutStandingDetails.Sum(x => x.UnitPrice) ;
            oDetail.Qty_Invoice = oImportOutStandingDetails.Sum(x => x.Qty_Invoice) ;
            oDetail.Qty_PI = oImportOutStandingDetails.Sum(x => x.Qty_PI) ;
            //oDetail.PINo = oImportOutStandingDetails.Sum(x => x.PINo) ;
            oDetail.Qty_TR = oImportOutStandingDetails.Sum(x => x.Qty_TR) ;
            //oDetail.GRNNo = oImportOutStandingDetails.Sum(x => x.GRNNo) ;
            oDetail.Qty_StockIn = oImportOutStandingDetails.Sum(x => x.Qty_StockIn) ;
            oDetail.Qty_Short = oImportOutStandingDetails.Sum(x => x.Qty_Short) ;
            //oDetail.value = 0;
            //oDetail.value = oImportOutStandingDetails.Sum(x => x.Qty*x.UnitPrice);
            oDetail.BLDate = DateTime.MinValue;
            oImportOutStandingDetails.Add(oDetail);
            #endregion       

            var jsonResult = Json(oImportOutStandingDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oPRDs);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewProduct_Reconciliation_DetaiI_mport(int nProductID)
        {
            _oImportOutstanding = new ImportOutstanding();

            Product oProduct = new Product();
            oProduct = oProduct.Get(nProductID, (int)Session[SessionInfo.currentUserID]);

         
            try
            {

                return View(_oImportOutstanding);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oImportOutstanding);
            }

        }

        [HttpPost]
        public JsonResult GetsReport(ImportOutstanding oImportOutstanding)
        {
            _oImportOutstandings = new List<ImportOutstanding>();
        
            List<ImportOutstanding> oImportOutstandings_ViewList = new List<ImportOutstanding>();
            try
            {
                _oImportOutstandings = ImportOutstanding.Gets(oImportOutstanding.StartDate, oImportOutstanding.EndDate.AddDays(1), oImportOutstanding.BUID, oImportOutstanding.CurrencyID,oImportOutstanding.nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);

                int nBankID = 0;
                foreach(var oItem in _oImportOutstandings)
                {
                    if (nBankID != oItem.BankBranchID) 
                    {
                        int nCurrencyID=999;
                        foreach (var oIMO in _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID)) 
                        {
                            if (nCurrencyID != oIMO.CurrencyID) 
                            {
                                foreach (var oIMO_C in _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID==oIMO.CurrencyID))
                                    oImportOutstandings_ViewList.Add(oIMO_C);

                                if (_oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).ToList().Count>1)
                                {
                                    AddIO(oIMO, oItem, ref oImportOutstandings_ViewList, "SUB TOTAL: ");
                                }
                            }
                            nCurrencyID = oIMO.CurrencyID;
                        }
                    }
                    nBankID = oItem.BankBranchID;
                }

                if (oImportOutstanding.CurrencyID == 2) 
                {
                    AddIO(oImportOutstanding, oImportOutstanding, ref oImportOutstandings_ViewList, "GRAND TOTAL: ");
                }

            }
            catch (Exception ex)
            {
                _oImportOutstandings = new List<ImportOutstanding>();
                _oImportOutstanding.ErrorMessage = ex.Message;
                _oImportOutstandings.Add(_oImportOutstanding);
            }
            var jsonResult = Json(oImportOutstandings_ViewList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public void AddIO(ImportOutstanding oIMO, ImportOutstanding oItem, ref List<ImportOutstanding> oImportOutstandings_ViewList, string sTitle) 
        {
            ImportOutstanding oImportOutstanding_Temp = new ImportOutstanding();
            oImportOutstanding_Temp.BUName = "";
            oImportOutstanding_Temp.BankBranchID = oIMO.BankBranchID;
            oImportOutstanding_Temp.CurrencyID = oIMO.CurrencyID;
            oImportOutstanding_Temp.BUID = oIMO.BUID;
            oImportOutstanding_Temp.BankShortName = oIMO.BankShortName;
            oImportOutstanding_Temp.BankName = sTitle;
            oImportOutstanding_Temp.LCPaymentType = 0;
            oImportOutstanding_Temp.CurrencyName = oIMO.CurrencyName;
            oImportOutstanding_Temp.LCOpen = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.LCOpen);
            oImportOutstanding_Temp.ShipmenmentInTransit = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.ShipmenmentInTransit);
            oImportOutstanding_Temp.ShipmenDone = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.ShipmenDone);
            oImportOutstanding_Temp.DocInBank = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.DocInBank);
            oImportOutstanding_Temp.DocInHand = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.DocInHand);
            oImportOutstanding_Temp.DocInCnF = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.DocInCnF);
            oImportOutstanding_Temp.GoodsInTransit = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.GoodsInTransit);
            oImportOutstanding_Temp.Accpt_WithoutStockIn = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.Accpt_WithoutStockIn);
            oImportOutstanding_Temp.Accpt_WithStockIn = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.Accpt_WithStockIn);
            oImportOutstanding_Temp.ABP_WithStockIn = _oImportOutstandings.Where(x => x.BankBranchID == oItem.BankBranchID && x.CurrencyID == oIMO.CurrencyID).Sum(x => x.ABP_WithStockIn);

            if (sTitle.Contains("GRAND TOTAL: "))
            {
                oImportOutstanding_Temp.LCOpen = _oImportOutstandings.Sum(x => x.LCOpen);
                oImportOutstanding_Temp.ShipmenmentInTransit = _oImportOutstandings.Sum(x => x.ShipmenmentInTransit);
                oImportOutstanding_Temp.ShipmenDone = _oImportOutstandings.Sum(x => x.ShipmenDone);
                oImportOutstanding_Temp.DocInBank = _oImportOutstandings.Sum(x => x.DocInBank);
                oImportOutstanding_Temp.DocInHand = _oImportOutstandings.Sum(x => x.DocInHand);
                oImportOutstanding_Temp.DocInCnF = _oImportOutstandings.Sum(x => x.DocInCnF);
                oImportOutstanding_Temp.GoodsInTransit = _oImportOutstandings.Sum(x => x.GoodsInTransit);
                oImportOutstanding_Temp.Accpt_WithoutStockIn = _oImportOutstandings.Sum(x => x.Accpt_WithoutStockIn);
                oImportOutstanding_Temp.Accpt_WithStockIn = _oImportOutstandings.Sum(x => x.Accpt_WithStockIn);
                oImportOutstanding_Temp.ABP_WithStockIn = _oImportOutstandings.Sum(x => x.ABP_WithStockIn);
            }

            oImportOutstandings_ViewList.Add(oImportOutstanding_Temp);
        }

        #region Print Report 
        public ActionResult PrintPriview(string sTempString)
        {
            _oImportOutstandings = new List<ImportOutstanding>();
            _oImportOutstanding = new ImportOutstanding();
            List<ImportOutstanding> oImportOutstandings = new List<ImportOutstanding>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string _sHeaderName = "";
            string _sDateRange = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                _oImportOutstanding.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                //_oImportOutstanding.DateType = Convert.ToInt32(sTempString.Split('~')[1]);
                _oImportOutstanding.StartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                _oImportOutstanding.EndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                _oImportOutstanding.CurrencyID = 0;//Convert.ToInt32(sTempString.Split('~')[4]);
                _oImportOutstanding.nReportType = Convert.ToInt32(sTempString.Split('~')[5]);
                oBusinessUnit = oBusinessUnit.Get(_oImportOutstanding.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sDateRange = "Date: " + _oImportOutstanding.StartDate.ToString("dd MMM yyyy") + " To" + _oImportOutstanding.EndDate.ToString("dd MMM yyyy");
                _sHeaderName = "Unit: " + oBusinessUnit.Name ;
                _oImportOutstandings = ImportOutstanding.Gets(_oImportOutstanding.StartDate, _oImportOutstanding.EndDate.AddDays(1), _oImportOutstanding.BUID, _oImportOutstanding.CurrencyID, _oImportOutstanding.nReportType,((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "select BankBranchID_Nego,BankName_Nego,LCPaymenttype,  SUM(Amount*CCRate) as Amount,YEAR(DateofMaturity) as Year_Maturity,Month(DateofMaturity) as Month_Maturity,LEFT(DATENAME(month, DateofMaturity),3) AS [MonthName] from View_ImportInvoice where BankStatus=3  and DateofMaturity>'" + _oImportOutstanding.StartDate+ "' and DateofMaturity is not null Group by BankBranchID_Nego,BankName_Nego, LCPaymenttype,YEAR(DateofMaturity),Month(DateofMaturity),DATENAME(month, DateofMaturity)";
                oImportOutstandings = ImportOutstanding.GetsImportOutstandingReport(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptImportOutstanding oReport = new rptImportOutstanding();
            byte[] abytes = null;
            abytes = oReport.PrepareReportProduct(_oImportOutstandings, oImportOutstandings, oCompany, oBusinessUnit, _sHeaderName, _sDateRange);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintAll(string sTempString)
        {
            ImportOutStandingDetail oIOSD = new ImportOutStandingDetail();
            int BUID = Convert.ToInt32(sTempString.Split('~')[0]);
            int CurrencyID = Convert.ToInt32(sTempString.Split('~')[1]);
            int nUserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
            
            

            #region Total
            //ImportOutStandingDetail oDetail = new ImportOutStandingDetail();
            //oDetail.ContractorName = "Total: ";
            //oDetail.Qty = oImportOutStandingDetails.Sum(x => x.Qty);
            //oDetail.UnitPrice = oImportOutStandingDetails.Sum(x => x.UnitPrice);
            //oDetail.Qty_Invoice = oImportOutStandingDetails.Sum(x => x.Qty_Invoice);
            //oDetail.Qty_PI = oImportOutStandingDetails.Sum(x => x.Qty_PI);
            //oDetail.Qty_TR = oImportOutStandingDetails.Sum(x => x.Qty_TR);
            //oDetail.Qty_StockIn = oImportOutStandingDetails.Sum(x => x.Qty_StockIn);
            //oDetail.Qty_Short = oImportOutStandingDetails.Sum(x => x.Qty_Short);
            //oDetail.BLDate = DateTime.MinValue;
            //oImportOutStandingDetails.Add(oDetail);
            #endregion




            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oCompany.CompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptImportOutstandingAll oReport = new rptImportOutstandingAll();
            byte[] abytes = null;
            abytes = oReport.PrepareReportProduct(oCompany, oBusinessUnit, BUID, CurrencyID, nUserID);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintDetailsPreview(string sTempString)
        {
            _oImportOutstandings = new List<ImportOutstanding>();
            _oImportOutstanding = new ImportOutstanding();
            List<ImportOutstanding> oImportOutstandings = new List<ImportOutstanding>();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            List<ImportOutStandingDetail> oImportOutStandingDetails = new List<ImportOutStandingDetail>();
            ImportOutStandingDetail oIOSD = new ImportOutStandingDetail();

            string _sHeaderName = "";
            string _sDateRange = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                _oImportOutstanding.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                _oImportOutstanding.CurrencyID = Convert.ToInt32(sTempString.Split('~')[1]);
                _oImportOutstanding.StartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                _oImportOutstanding.EndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                _oImportOutstanding.BankBranchID = Convert.ToInt32(sTempString.Split('~')[4]);
                _oImportOutstanding.LCPaymentType = (EnumLCPaymentType)Convert.ToInt32(sTempString.Split('~')[5]);
                _oImportOutstanding.nReportType = Convert.ToInt32(sTempString.Split('~')[6]);
                _oImportOutstanding.ErrorMessage = Convert.ToString(sTempString.Split('~')[7]);
                oBusinessUnit = oBusinessUnit.Get(_oImportOutstanding.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sDateRange = "Date: " + _oImportOutstanding.StartDate.ToString("dd MMM yyyy") + " To " + _oImportOutstanding.EndDate.ToString("dd MMM yyyy");
                _sHeaderName = "Unit: " + oBusinessUnit.Name;
                
                try
                {
                    _oImportOutstandings = ImportOutstanding.Gets(_oImportOutstanding.StartDate, _oImportOutstanding.EndDate.AddDays(1), _oImportOutstanding.BUID, _oImportOutstanding.CurrencyID,Convert.ToInt32(_oImportOutstanding.ErrorMessage), ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oImportOutStandingDetails = ImportOutStandingDetail.Gets(_oImportOutstanding.BUID, (int)_oImportOutstanding.LCPaymentType, _oImportOutstanding.BankBranchID, _oImportOutstanding.CurrencyID, _oImportOutstanding.StartDate, _oImportOutstanding.EndDate.AddDays(1), _oImportOutstanding.nReportType, Convert.ToInt32(_oImportOutstanding.ErrorMessage),((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                catch (Exception ex)
                {
                    oImportOutStandingDetails = new List<ImportOutStandingDetail>();
                    oIOSD = new ImportOutStandingDetail();
                    oIOSD.ErrorMessage = ex.Message;
                    oImportOutStandingDetails = new List<ImportOutStandingDetail>();
                }
            }
            string sDetail = " (Bank-" + (_oImportOutstanding.BankBranchID == 0 ? " All " : _oImportOutstandings[0].BankShortName) + "   Payment-" + ((int)_oImportOutstanding.LCPaymentType == 0 ? " All " : _oImportOutstanding.LCPaymentTypeSt) + (_oImportOutstanding.CurrencyID == 0 ? " All " : "  Currency-" + _oImportOutstandings[0].CurrencyName) + ")";
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptImportOutstandingDetails oReport = new rptImportOutstandingDetails();
            byte[] abytes = null;
            abytes = oReport.PrepareReport(oImportOutStandingDetails, oCompany, oBusinessUnit, _sHeaderName,sDetail, _sDateRange,_oImportOutstanding.nReportType);
            return File(abytes, "application/pdf");
        }
        public void PrintDetailsPreviewXL(string sTempString)
        {
            _oImportOutstandings = new List<ImportOutstanding>();
            _oImportOutstanding = new ImportOutstanding();
            List<ImportOutstanding> oImportOutstandings = new List<ImportOutstanding>();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            List<ImportOutStandingDetail> oImportOutStandingDetails = new List<ImportOutStandingDetail>();
            ImportOutStandingDetail oIOSD = new ImportOutStandingDetail();

            string _sHeaderName = "";
            string _sDateRange = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                _oImportOutstanding.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                _oImportOutstanding.CurrencyID = Convert.ToInt32(sTempString.Split('~')[1]);
                _oImportOutstanding.StartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                _oImportOutstanding.EndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                _oImportOutstanding.BankBranchID = Convert.ToInt32(sTempString.Split('~')[4]);
                _oImportOutstanding.LCPaymentType = (EnumLCPaymentType)Convert.ToInt32(sTempString.Split('~')[5]);
                _oImportOutstanding.nReportType = Convert.ToInt32(sTempString.Split('~')[6]);
                _oImportOutstanding.ErrorMessage = Convert.ToString(sTempString.Split('~')[7]);
                oBusinessUnit = oBusinessUnit.Get(_oImportOutstanding.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sDateRange = "Date: " + _oImportOutstanding.StartDate.ToString("dd MMM yyyy") + " To " + _oImportOutstanding.EndDate.ToString("dd MMM yyyy");
                _sHeaderName = "Unit: " + oBusinessUnit.Name;
                
                try
                {
                    oImportOutStandingDetails = ImportOutStandingDetail.Gets(_oImportOutstanding.BUID, (int)_oImportOutstanding.LCPaymentType, _oImportOutstanding.BankBranchID, _oImportOutstanding.CurrencyID, _oImportOutstanding.StartDate, _oImportOutstanding.EndDate.AddDays(1), _oImportOutstanding.nReportType, Convert.ToInt32(_oImportOutstanding.ErrorMessage), ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                catch (Exception ex)
                {
                    oImportOutStandingDetails = new List<ImportOutStandingDetail>();
                    oIOSD = new ImportOutStandingDetail();
                    oIOSD.ErrorMessage = ex.Message;
                    oImportOutStandingDetails = new List<ImportOutStandingDetail>();
                }
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_oImportOutstanding.nReportType == 1) //LC OPEN
            {
                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 15, nColumn = 1, nCount = 0, nImportLCID = 0;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ImportOutstnding");
                    sheet.Name = "Import OutStanding ";

                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 20; //LC No
                    sheet.Column(++nColumn).Width = 25; //LC Date
                    sheet.Column(++nColumn).Width = 20; //PI No
                    sheet.Column(++nColumn).Width = 40; //Supplier
                    sheet.Column(++nColumn).Width = 35; //Commodity
                    sheet.Column(++nColumn).Width = 15; //YTIQ
                    sheet.Column(++nColumn).Width = 15; //Value
                    sheet.Column(++nColumn).Width = 25; //ShipmentDate
                    sheet.Column(++nColumn).Width = 25; //ExpireDate
                    sheet.Column(++nColumn).Width = 15; //LC Qty
                    sheet.Column(++nColumn).Width = 15; //UP
                    sheet.Column(++nColumn).Width = 15; //LC Val
                    sheet.Column(++nColumn).Width = 15; //InvoiceQty

                    //nCol = 14;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 8]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Import Outstanding (L/C Open)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion


                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Due Inv "; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Expire Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "PI Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Data

                    foreach (ImportOutStandingDetail oItem in oImportOutStandingDetails)
                    {
                        nColumn = 1;

                        nCount++;
                        //int nRowSpan = oImportOutStandingDetails.Where(PIR => PIR.ImportLCID == oItem.ImportLCID).ToList().Count - 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.LCNo.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ImportLCDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "" + oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "" + oItem.value; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ShipmentDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ExpireDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.value; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_PI; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = oImportOutStandingDetails.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = oImportOutStandingDetails.Select(c => c.value).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = oImportOutStandingDetails.Select(c => c.Qty_PI).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ImportOutstnding_LC_Open.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (_oImportOutstanding.nReportType == 2) //Doc Recd
            {
                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 13, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ImportOutstnding_DocRecd");
                    sheet.Name = "Import OutStanding ";

                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 15; //LC No
                    sheet.Column(++nColumn).Width = 15; //LC Date
                    sheet.Column(++nColumn).Width = 15; //Invoice No
                    sheet.Column(++nColumn).Width = 15; //Invoice Date
                    sheet.Column(++nColumn).Width = 30; //Supplier
                    sheet.Column(++nColumn).Width = 30; //Commodity
                    sheet.Column(++nColumn).Width = 15; //ShipmentDate
                    sheet.Column(++nColumn).Width = 15; //ExpireDate
                    sheet.Column(++nColumn).Width = 15; //Qty
                    sheet.Column(++nColumn).Width = 15; //UP
                    sheet.Column(++nColumn).Width = 15; //LC Val
                    //nCol = 14;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Import Outstanding (Doc Recd)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Header
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Expire Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Data

                    foreach (ImportOutStandingDetail oItem in oImportOutStandingDetails)
                    {
                        nColumn = 1;
                        nCount++;
                        //int nRowSpan = oImportOutStandingDetails.Where(PIR => PIR.ImportLCID == oItem.ImportLCID).ToList().Count - 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.LCNo.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ImportLCDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.DateofInvoiceStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ShipmentDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ExpireDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.value; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 3]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = oImportOutStandingDetails.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = oImportOutStandingDetails.Select(c => c.value).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ImportOutstndingDocRecd.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (_oImportOutstanding.nReportType == 3)//Shipment
            {
                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 13, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ImportOutstnding_Shipment");
                    sheet.Name = "Import OutStanding ";

                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 20; //LC No
                    sheet.Column(++nColumn).Width = 15; //LC Date
                    sheet.Column(++nColumn).Width = 20; //Invoice No
                    sheet.Column(++nColumn).Width = 15; //Invoice Date
                    sheet.Column(++nColumn).Width = 45; //Supplier
                    sheet.Column(++nColumn).Width = 35; //Commodity
                    sheet.Column(++nColumn).Width = 20; //BLNo
                    sheet.Column(++nColumn).Width = 15; //BLDate
                    sheet.Column(++nColumn).Width = 15; //Qty
                    sheet.Column(++nColumn).Width = 15; //UP
                    sheet.Column(++nColumn).Width = 15; //LC Val
                    //nCol = 14;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Import Outstanding (Shipment)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Header
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "BL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "BL Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Data

                    foreach (ImportOutStandingDetail oItem in oImportOutStandingDetails)
                    {
                        nColumn = 1;
                        nCount++;
                        //int nRowSpan = oImportOutStandingDetails.Where(PIR => PIR.ImportLCID == oItem.ImportLCID).ToList().Count - 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.LCNo.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ImportLCDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.DateofInvoiceStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BLNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BLDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.value; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 3]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = oImportOutStandingDetails.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = oImportOutStandingDetails.Select(c => c.value).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ImportOutstnding_Shipment.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (_oImportOutstanding.nReportType == 4)//Doc In Bank
            {
                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 13, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ImportOutstnding_DocInBank");
                    sheet.Name = "Import OutStanding ";

                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 15; //LC No
                    sheet.Column(++nColumn).Width = 15; //LC Date
                    sheet.Column(++nColumn).Width = 15; //Invoice No
                    sheet.Column(++nColumn).Width = 15; //Invoice Date
                    sheet.Column(++nColumn).Width = 30; //Supplier
                    sheet.Column(++nColumn).Width = 30; //Commodity
                    sheet.Column(++nColumn).Width = 15; //ShipmentDate
                    sheet.Column(++nColumn).Width = 15; //ExpireDate
                    sheet.Column(++nColumn).Width = 15; //Qty
                    sheet.Column(++nColumn).Width = 15; //UP
                    sheet.Column(++nColumn).Width = 15; //LC Val
                    //nCol = 12;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Import Outstanding (Doc In Bank)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Header
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Expire Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Data

                    foreach (ImportOutStandingDetail oItem in oImportOutStandingDetails)
                    {
                        nColumn = 1;
                        nCount++;
                        //int nRowSpan = oImportOutStandingDetails.Where(PIR => PIR.ImportLCID == oItem.ImportLCID).ToList().Count - 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.LCNo.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ImportLCDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.DateofInvoiceStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ShipmentDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ExpireDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.value; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 3]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = oImportOutStandingDetails.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = oImportOutStandingDetails.Select(c => c.value).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ImportOutstnding_DocInBank.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (_oImportOutstanding.nReportType == 5)//Doc In CnF
            {
                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 15, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ImportOutstnding_DocInCnF");
                    sheet.Name = "Import OutStanding ";

                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 15; //FILE No
                    sheet.Column(++nColumn).Width = 15; //LC No
                    sheet.Column(++nColumn).Width = 15; //LC Date
                    sheet.Column(++nColumn).Width = 15; //Invoice No
                    sheet.Column(++nColumn).Width = 15; //Invoice Date
                    sheet.Column(++nColumn).Width = 30; //Supplier
                    sheet.Column(++nColumn).Width = 30; //Commodity
                    sheet.Column(++nColumn).Width = 15; //ShipmentDate
                    sheet.Column(++nColumn).Width = 15; //ExpireDate
                    sheet.Column(++nColumn).Width = 15; //Qty
                    sheet.Column(++nColumn).Width = 15; //UP
                    sheet.Column(++nColumn).Width = 15; //LC Val
                    sheet.Column(++nColumn).Width = 20; //LC Val
                    //nCol = 14;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Import Outstanding (Doc In CnF)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Header
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "File No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Doc No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Send Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Data

                    foreach (ImportOutStandingDetail oItem in oImportOutStandingDetails)
                    {
                        nColumn = 1;
                        nCount++;
                        //int nRowSpan = oImportOutStandingDetails.Where(PIR => PIR.ImportLCID == oItem.ImportLCID).ToList().Count - 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.FileNo.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.LCNo.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ImportLCDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.DateofInvoiceStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceStatusSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DocNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.CnFSendDate; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.value; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 3]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = oImportOutStandingDetails.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = oImportOutStandingDetails.Select(c => c.value).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ImportOutstnding_DocInCnF.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (_oImportOutstanding.nReportType == 6)
            {
                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 9, nColumn = 1, nCount = 0, nImportLCID = 0;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ImportOutstnding_GoodsInTransit");
                    sheet.Name = "Import OutStanding ";

                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 25; //Invoice No
                    sheet.Column(++nColumn).Width = 20; //Invoice Date
                    sheet.Column(++nColumn).Width = 45; //Supplier
                    sheet.Column(++nColumn).Width = 35; //Commodity
                    sheet.Column(++nColumn).Width = 20; //Qty
                    sheet.Column(++nColumn).Width = 20; //UP
                    sheet.Column(++nColumn).Width = 20; //Val

                    //nCol = 14;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 5, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Import Outstanding (Goods In Transit)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 5, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion


                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Data

                    foreach (ImportOutStandingDetail oItem in oImportOutStandingDetails)
                    {
                        nColumn = 1;

                        nCount++;
                        //int nRowSpan = oImportOutStandingDetails.Where(PIR => PIR.ImportLCID == oItem.ImportLCID).ToList().Count - 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.InvoiceNo.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.DateofInvoiceStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.value; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 3]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = oImportOutStandingDetails.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = oImportOutStandingDetails.Select(c => c.value).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    
                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ImportOutstnding_GoodsInTransit.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion
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
    }
}
