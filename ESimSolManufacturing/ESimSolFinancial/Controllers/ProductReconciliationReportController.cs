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
    public class ProductReconciliationReportController : PdfViewController
    {
        #region Declaration 
        ProductReconciliationReport _oProductReconciliationReport = new ProductReconciliationReport();
        List<ProductReconciliationReport> _oProductReconciliationReports = new List<ProductReconciliationReport>();
        DataSet _oDataSet = new DataSet();
        string _sDateRange = "";
        #endregion

        public ActionResult ViewProductReconciliationDetail(int nProductID, int buid)
        {
            _oProductReconciliationReport = new ProductReconciliationReport();
            Product oProduct = new Product();
            oProduct = oProduct.Get(nProductID, (int)Session[SessionInfo.currentUserID]);
            _oProductReconciliationReport.ProductCode = oProduct.ProductCode;
            _oProductReconciliationReport.ProductName = '[' + oProduct.ProductCode + ']' + oProduct.ProductName;
            _oProductReconciliationReport.ProductID = oProduct.ProductID;
            try
            {
                ViewBag.BUID = buid;
                return View(_oProductReconciliationReport);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oProductReconciliationReport);
            }

        }
     
        public ActionResult ViewProductReconciliationReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oProductReconciliationReports = new List<ProductReconciliationReport>();
            try
            {
                ViewBag.BUID = buid;
                return View(_oProductReconciliationReports);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oProductReconciliationReports);
            }

        }
       
        [HttpPost]
        public JsonResult GetsPRReport(ProductReconciliationReport oTB)
        {
            List<ProductReconciliationReport> oPRRs_Category = new List<ProductReconciliationReport>();
            if (oTB.ProductName == null)
            {
                oTB.ProductName = "";
            }
            try
            {
                _oProductReconciliationReports = ProductReconciliationReport.GetsPR(oTB.BUID, oTB.ProductName, oTB.StartDate, oTB.EndDate, oTB.ReportType, oTB.SortType, (int)Session[SessionInfo.currentUserID]);
                oPRRs_Category = _oProductReconciliationReports.GroupBy(x => new { x.ProductCategoryID, x.CategoryName }, (key, grp) =>
                                    new ProductReconciliationReport
                                    {
                                        ProductCategoryID = key.ProductCategoryID,
                                        ProductID=0,
                                        ProductName = "Category",
                                        CategoryName= key.CategoryName,
                                        StockInHand =0,
                                        ProYetToWithLC = 0,
                                        ProYetToWithoutLC = 0,
                                        LCReceiveDONoReceive = 0,
                                        PIIssueLCnDONotReceive = 0,
                                        ProductionYetToSample = 0,
                                        ShipmentDone = 0,
                                        DocInCnF = 0,
                                        DocReceive = 0,
                                        InvoiceWithoutBL = 0,
                                        LCOpen = 0,
                                        GoodsRelease = 0,
                                        GoodsinTrasit = 0,
                                        POReceive = 0,
                                        MinimumStock = 0,
                                        ProductionYetTo = 0,
                                    }).ToList();

                oPRRs_Category.ForEach(x => _oProductReconciliationReports.Add(x));
                _oProductReconciliationReports = _oProductReconciliationReports.OrderBy(o => o.CategoryName).ThenBy(a => a.ProductID).ToList();
                

            }
            catch (Exception ex)
            {
                _oProductReconciliationReport.ErrorMessage = ex.Message;
                _oProductReconciliationReports = new List<ProductReconciliationReport>();
            }

            var jsonResult = Json(_oProductReconciliationReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #region Print 
        public ActionResult PrintPriview(string sTempString)
        {
            ProductReconciliationReport oTB = new ProductReconciliationReport();
            List<ProductReconciliationReport> oPRRs_Category = new List<ProductReconciliationReport>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string _sHeaderName = "";
            string _sDateRange = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                oTB.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                oTB.ProductName = sTempString.Split('~')[1];
                oTB.ReportType = Convert.ToInt32(sTempString.Split('~')[2]);
                oTB.StartDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                oTB.EndDate = Convert.ToDateTime(sTempString.Split('~')[4]);
                oTB.SortType= Convert.ToInt32(sTempString.Split('~')[5]);
                oBusinessUnit = oBusinessUnit.Get(oTB.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductReconciliationReports = ProductReconciliationReport.GetsPR(oTB.BUID, oTB.ProductName,oTB.StartDate, oTB.EndDate, oTB.ReportType,oTB.SortType, (int)Session[SessionInfo.currentUserID]);

                _sDateRange = "Date: " + oTB.StartDate.ToString("dd MMM yyyy") + " to " + oTB.EndDate.ToString("dd MMM yyyy");

                oPRRs_Category = _oProductReconciliationReports.GroupBy(x => new { x.ProductCategoryID, x.CategoryName }, (key, grp) =>
                                  new ProductReconciliationReport
                                  {
                                      ProductCategoryID = key.ProductCategoryID,
                                      ProductID = 0,
                                      ProductName = "Category",
                                      CategoryName = key.CategoryName,
                                      StockInHand = grp.Sum(x => x.StockInHand),
                                      ProYetToWithLC = grp.Sum(x => x.ProYetToWithLC),
                                      ProYetToWithoutLC = grp.Sum(x => x.ProYetToWithoutLC),
                                      LCReceiveDONoReceive = grp.Sum(x => x.LCReceiveDONoReceive),
                                      PIIssueLCnDONotReceive = grp.Sum(x => x.PIIssueLCnDONotReceive),
                                      ProductionYetToSample = grp.Sum(x => x.ProductionYetToSample),
                                      ShipmentDone = grp.Sum(x => x.ShipmentDone),
                                      DocInCnF = grp.Sum(x => x.DocInCnF),
                                      DocReceive = grp.Sum(x => x.DocReceive),
                                      InvoiceWithoutBL = grp.Sum(x => x.InvoiceWithoutBL),
                                      LCOpen = grp.Sum(x => x.LCOpen),
                                      GoodsRelease = grp.Sum(x => x.GoodsRelease),
                                      GoodsinTrasit = grp.Sum(x => x.GoodsinTrasit),
                                      POReceive = grp.Sum(x => x.POReceive),
                                      MinimumStock = grp.Sum(x => x.MinimumStock),
                                      ProductionYetTo = grp.Sum(x => x.ProductionYetTo),
                                  }).ToList();

                oPRRs_Category.ForEach(x => _oProductReconciliationReports.Add(x));
                _oProductReconciliationReports = _oProductReconciliationReports.OrderBy(o => o.CategoryName).ThenBy(a => a.ProductID).ToList();
            }
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptProductReconciliationReport oReport = new rptProductReconciliationReport();
            byte[] abytes = null;
            abytes = oReport.PrepareReportPR(_oProductReconciliationReports, oCompany, oBusinessUnit, _sDateRange);
            return File(abytes, "application/pdf");


        }

        public ActionResult PrintPriviewPRDetail(string sTempString)
        {
            ProductReconciliationReport oTB = new ProductReconciliationReport();
            ProductReconciliationReport oPRR = new ProductReconciliationReport();
            List<ProductReconciliationReportDetail> oPRDs_StockInHand = new List<ProductReconciliationReportDetail>();//8
            List<ProductReconciliationReportDetail> oPRDs_ProYetToWithoutLC = new List<ProductReconciliationReportDetail>();//10
            List<ProductReconciliationReportDetail> oPRDs_ProYetToWithLC = new List<ProductReconciliationReportDetail>();//11
            List<ProductReconciliationReportDetail> oPRDs_YetToOrderLC = new List<ProductReconciliationReportDetail>();//13
            List<ProductReconciliationReportDetail> oPRDs_YetToOrderPI = new List<ProductReconciliationReportDetail>();//14
            List<ProductReconciliationReportDetail> oPRDs_DocInCnF = new List<ProductReconciliationReportDetail>();//7
            List<ProductReconciliationReportDetail> oPRDs_DocReceived = new List<ProductReconciliationReportDetail>();//6
            List<ProductReconciliationReportDetail> oPRDs_ShipmentDone = new List<ProductReconciliationReportDetail>();//5
            List<ProductReconciliationReportDetail> oPRDs_InvoiceWithOutShipment = new List<ProductReconciliationReportDetail>();//4
            List<ProductReconciliationReportDetail> oPRDs_LCOpen = new List<ProductReconciliationReportDetail>();//3
            List<ProductReconciliationReportDetail> oPRDs_PIReceived = new List<ProductReconciliationReportDetail>();//1
            List<ProductReconciliationReportDetail> oPRDs_GoodsRelease = new List<ProductReconciliationReportDetail>();//20
            List<ProductReconciliationReportDetail> oPRDs_GoodsinTrasit = new List<ProductReconciliationReportDetail>();//21
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string _sHeaderName = "";
            string _sDateRange = "";
          
            if (!string.IsNullOrEmpty(sTempString))
            {
                oTB.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                oTB.ProductName = sTempString.Split('~')[1];
                oTB.ReportType = Convert.ToInt32(sTempString.Split('~')[2]);
                oTB.StartDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                oTB.EndDate = Convert.ToDateTime(sTempString.Split('~')[4]);
                oTB.SortType = Convert.ToInt32(sTempString.Split('~')[5]);
                oBusinessUnit = oBusinessUnit.Get(oTB.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductReconciliationReports = ProductReconciliationReport.GetsPR(oTB.BUID, oTB.ProductName, oTB.StartDate, oTB.EndDate, 1, oTB.SortType, (int)Session[SessionInfo.currentUserID]);
                if (_oProductReconciliationReports.Count > 0)
                {
                    oPRR = _oProductReconciliationReports.First();
                }
            }
            oPRR.StartDate = oTB.StartDate;
            oPRR.EndDate = oTB.EndDate;
            oTB.ReportType = 8;
            oPRDs_StockInHand = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 10;
            oPRDs_ProYetToWithoutLC = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 11;
            oPRDs_ProYetToWithLC = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 13;
            oPRDs_YetToOrderLC = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 14;
            oPRDs_YetToOrderPI = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 7;
            oPRDs_DocInCnF = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 6;
            oPRDs_DocReceived = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 5;
            oPRDs_ShipmentDone = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 4;
            oPRDs_InvoiceWithOutShipment = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 3;
            oPRDs_LCOpen = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 1;
            oPRDs_PIReceived = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);

            oTB.ReportType = 20;
            oPRDs_GoodsRelease = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 21;
            oPRDs_GoodsinTrasit = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptProductReconciliationReport oReport = new rptProductReconciliationReport();
            byte[] abytes = null;
            abytes = oReport.PrepareReportPRDetail(oPRR, oPRDs_StockInHand, oPRDs_ProYetToWithoutLC, oPRDs_ProYetToWithLC, oPRDs_YetToOrderLC, oPRDs_YetToOrderPI, oPRDs_DocInCnF, oPRDs_DocReceived, oPRDs_ShipmentDone, oPRDs_InvoiceWithOutShipment, oPRDs_LCOpen, oPRDs_PIReceived, oPRDs_GoodsRelease, oPRDs_GoodsinTrasit, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintPriviewPRDetailTwo(string sTempString)
        {
            ProductReconciliationReport oTB = new ProductReconciliationReport();
            ProductReconciliationReport oPRR = new ProductReconciliationReport();
            List<ProductReconciliationReportDetail> oPRDs_StockInHand = new List<ProductReconciliationReportDetail>();//8
            List<ProductReconciliationReportDetail> oPRDs_ProYetToWithoutLC = new List<ProductReconciliationReportDetail>();//10
            List<ProductReconciliationReportDetail> oPRDs_ProYetToWithLC = new List<ProductReconciliationReportDetail>();//11
            List<ProductReconciliationReportDetail> oPRDs_YetToOrderLC = new List<ProductReconciliationReportDetail>();//13
            List<ProductReconciliationReportDetail> oPRDs_YetToOrderPI = new List<ProductReconciliationReportDetail>();//14
            List<ProductReconciliationReportDetail> oPRDs_DocInCnF = new List<ProductReconciliationReportDetail>();//7
            List<ProductReconciliationReportDetail> oPRDs_DocReceived = new List<ProductReconciliationReportDetail>();//6
            List<ProductReconciliationReportDetail> oPRDs_ShipmentDone = new List<ProductReconciliationReportDetail>();//5
            List<ProductReconciliationReportDetail> oPRDs_InvoiceWithOutShipment = new List<ProductReconciliationReportDetail>();//4
            List<ProductReconciliationReportDetail> oPRDs_LCOpen = new List<ProductReconciliationReportDetail>();//3
            List<ProductReconciliationReportDetail> oPRDs_PIReceived = new List<ProductReconciliationReportDetail>();//1
            List<ProductReconciliationReportDetail> oPRDs_GoodsRelease = new List<ProductReconciliationReportDetail>();//20
            List<ProductReconciliationReportDetail> oPRDs_GoodsinTrasit = new List<ProductReconciliationReportDetail>();//21
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string _sHeaderName = "";
            string _sDateRange = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                oTB.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                oTB.ProductName = sTempString.Split('~')[1];
                oTB.ReportType = Convert.ToInt32(sTempString.Split('~')[2]);
                oTB.StartDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                oTB.EndDate = Convert.ToDateTime(sTempString.Split('~')[4]);
                oTB.SortType = Convert.ToInt32(sTempString.Split('~')[5]);
                oBusinessUnit = oBusinessUnit.Get(oTB.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductReconciliationReports = ProductReconciliationReport.GetsPR(oTB.BUID, oTB.ProductName, oTB.StartDate, oTB.EndDate, 1, oTB.SortType, (int)Session[SessionInfo.currentUserID]);
                if (_oProductReconciliationReports.Count > 0)
                {
                    oPRR = _oProductReconciliationReports.First();
                }
            }
            oPRR.StartDate = oTB.StartDate;
            oPRR.EndDate = oTB.EndDate;
            oTB.ReportType = 8;
            oPRDs_StockInHand = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 10;
          //  oPRDs_ProYetToWithoutLC = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 11;
           // oPRDs_ProYetToWithLC = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 13;
           // oPRDs_YetToOrderLC = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 14;
           // oPRDs_YetToOrderPI = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 7;
            oPRDs_DocInCnF = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 6;
            oPRDs_DocReceived = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 5;
            oPRDs_ShipmentDone = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 4;
            oPRDs_InvoiceWithOutShipment = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 3;
            oPRDs_LCOpen = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 1;
            oPRDs_PIReceived = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);

            oTB.ReportType = 20;
            oPRDs_GoodsRelease = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
            oTB.ReportType = 21;
            oPRDs_GoodsinTrasit = ProductReconciliationReportDetail.Gets_PRDetail(oTB.BUID, oPRR.ProductID, oTB.StartDate, oTB.EndDate, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptProductReconciliationReport oReport = new rptProductReconciliationReport();
            byte[] abytes = null;
            abytes = oReport.PrepareReportPRDetail(oPRR, oPRDs_StockInHand, oPRDs_ProYetToWithoutLC, oPRDs_ProYetToWithLC, oPRDs_YetToOrderLC, oPRDs_YetToOrderPI, oPRDs_DocInCnF, oPRDs_DocReceived, oPRDs_ShipmentDone, oPRDs_InvoiceWithOutShipment, oPRDs_LCOpen, oPRDs_PIReceived, oPRDs_GoodsRelease, oPRDs_GoodsinTrasit, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
       
        public void PrintPRRInXL(string sTempString)
        {
            //ProductReconciliationReport 
            List<ProductReconciliationReport> oPRRs_Category = new List<ProductReconciliationReport>();

            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            int nBUID = 0;
            int nReportType = 0;
            int nSortType = 0;
            string sProductIDs = "";

            if (!String.IsNullOrEmpty(sTempString))
            {
                nBUID = Convert.ToInt32(sTempString.Split('~')[0]);
                sProductIDs = sTempString.Split('~')[1];
                nReportType = Convert.ToInt32(sTempString.Split('~')[2]);
                dStartDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                dEndDate = Convert.ToDateTime(sTempString.Split('~')[4]);
                nSortType = Convert.ToInt32(sTempString.Split('~')[5]);
            }

            _oProductReconciliationReports = ProductReconciliationReport.GetsPR(nBUID, sProductIDs, dStartDate, dEndDate, nReportType,nSortType, (int)Session[SessionInfo.currentUserID]);

            oPRRs_Category = _oProductReconciliationReports.GroupBy(x => new { x.ProductCategoryID, x.CategoryName }, (key, grp) =>
                                   new ProductReconciliationReport
                                   {
                                       ProductCategoryID = key.ProductCategoryID,
                                       ProductID = 0,
                                       ProductName = "Category",
                                       CategoryName  = key.CategoryName,
                                       StockInHand = grp.Sum(x => x.StockInHand),
                                       ProYetToWithLC = grp.Sum(x => x.ProYetToWithLC),
                                       ProYetToWithoutLC = grp.Sum(x => x.ProYetToWithoutLC),
                                       LCReceiveDONoReceive = grp.Sum(x => x.LCReceiveDONoReceive),
                                       PIIssueLCnDONotReceive = grp.Sum(x => x.PIIssueLCnDONotReceive),
                                       ProductionYetToSample = grp.Sum(x => x.ProductionYetToSample),
                                       ShipmentDone = grp.Sum(x => x.ShipmentDone),
                                       DocInCnF = grp.Sum(x => x.DocInCnF),
                                       DocReceive = grp.Sum(x => x.DocReceive),
                                       InvoiceWithoutBL = grp.Sum(x => x.InvoiceWithoutBL),
                                       LCOpen = grp.Sum(x => x.LCOpen),
                                       GoodsRelease = grp.Sum(x => x.GoodsRelease),
                                       GoodsinTrasit = grp.Sum(x => x.GoodsinTrasit),
                                       POReceive = grp.Sum(x => x.POReceive),
                                       MinimumStock = grp.Sum(x => x.MinimumStock),
                                       ProductionYetTo = grp.Sum(x => x.ProductionYetTo),
                                   }).ToList();

            oPRRs_Category.ForEach(x => _oProductReconciliationReports.Add(x));
            _oProductReconciliationReports = _oProductReconciliationReports.OrderBy(o => o.CategoryName).ThenBy(a => a.ProductID).ToList();

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nStockInHand = 0;
            double nProductionYetTo = 0;
            double nLCReceiveDONoReceive = 0;
            double nPIIssueLCnDONotReceive = 0;
            double nCurrentSalable = 0;
            double nYetToDelivery = 0;
            double nDocInCnF = 0;
            double nDocReceive = 0;
            double nShipmentDone = 0;
            double nInTran = 0;



            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 14;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Reconciliation Report");
                sheet.Name = "Reconciliation Report";
                sheet.Column(2).Width = 40;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                sheet.Column(12).Width = 20;
                sheet.Column(13).Width = 20;

                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Reconciliation Report  Date: " + DateTime.Today.ToString("dd MMM yyyy:HH:mm:tt") + ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Stock In Hand"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Production Pending"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Pending Order(L/C Received)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Pending Order(PI Issue)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Current Salable"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Goods In Trasit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Goods Release"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "DOC IN CnF "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

             

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "DOC Received"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Shipment Done"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "L/C Open"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "P/I Recd"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Net Salable "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                nRowIndex++;
                #endregion

                #region Data
                foreach (ProductReconciliationReport oItem in _oProductReconciliationReports)
                {


                    if (oItem.ProductID <= 0)
                    {
                        cell = sheet.Cells[nRowIndex, 1]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 2]; cell.Merge = true;
                        cell.Value = "Category : " + oItem.ProductName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5];  cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value =""; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    else
                    {
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "[" + oItem.ProductCode + "]" + oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ProductionYetTo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.LCReceiveDONoReceive; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.PIIssueLCnDONotReceive; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.CurrentSalable); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.GoodsinTrasit; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.GoodsRelease; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.DocInCnF; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.DocReceive; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.ShipmentDone; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.InvoiceWithoutBL + oItem.LCOpen; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.POReceive; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.NetSalable; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nStockInHand = nStockInHand + oItem.StockInHand;
                        nProductionYetTo = nProductionYetTo + oItem.ProductionYetTo;
                        nLCReceiveDONoReceive = nLCReceiveDONoReceive + oItem.LCReceiveDONoReceive;
                        nPIIssueLCnDONotReceive = nPIIssueLCnDONotReceive + oItem.PIIssueLCnDONotReceive;
                        nCurrentSalable = nCurrentSalable + oItem.CurrentSalable;
                        nDocInCnF = nDocInCnF + oItem.DocInCnF;
                        nDocReceive = nDocReceive + oItem.DocReceive;
                        nShipmentDone = nShipmentDone + oItem.ShipmentDone;
                        nInTran = nInTran + (oItem.InvoiceWithoutBL + oItem.LCOpen);
                    }

                 

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nProductionYetTo; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nLCReceiveDONoReceive; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nPIIssueLCnDONotReceive; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nCurrentSalable; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nStockInHand = _oProductReconciliationReports.Select(c => c.GoodsinTrasit).Sum();

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nStockInHand = _oProductReconciliationReports.Select(c => c.GoodsRelease).Sum();
                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nDocInCnF; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nDocReceive; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nShipmentDone; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = nInTran; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                
                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ReconciliationReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


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

   

        #region Yet To Send Production Prder
        public ActionResult ViewProductReconciliationReportYetToPO(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ProductReconciliationReportDetail> _oProductReconciliationReportDetails = new List<ProductReconciliationReportDetail>();
            try
            {
                ViewBag.BUID = buid;
                return View(_oProductReconciliationReportDetails);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oProductReconciliationReportDetails);
            }

        }
        [HttpPost]
        public JsonResult AdvSearch(ProductReconciliationReportDetail oProductReconciliationReportDetail)
        {
            List<ProductReconciliationReportDetail> _oPRDetails = new List<ProductReconciliationReportDetail>();
            try
            {
                string sSQL = MakeSQL(oProductReconciliationReportDetail);
                _oPRDetails = ProductReconciliationReportDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPRDetails = new List<ProductReconciliationReportDetail>();
                 oProductReconciliationReportDetail = new ProductReconciliationReportDetail();
                oProductReconciliationReportDetail.ErrorMessage = ex.Message;
                _oPRDetails.Add(oProductReconciliationReportDetail);
            }
            var jsonResult = Json(_oPRDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(ProductReconciliationReportDetail oProductReconciliationReportDetail)
        {
            string sParams = oProductReconciliationReportDetail.ErrorMessage;

            string sContractorIDs = "";
            string sProductIDs = "";
            int nCboIssueDate = 0;
            DateTime dFromIssueDate = DateTime.Today;
            DateTime dToIssueDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                sProductIDs = Convert.ToString(sParams.Split('~')[0]);
                sContractorIDs = Convert.ToString(sParams.Split('~')[1]);
                nCboIssueDate = Convert.ToInt32(sParams.Split('~')[2]);
                dFromIssueDate = Convert.ToDateTime(sParams.Split('~')[3]);
                dToIssueDate = Convert.ToDateTime(sParams.Split('~')[4]);
            }


            string sReturn1 = "SELECT * FROM View_ProductReconciliation_YetTOPO AS EPI ";
            string sReturn = "";

            #region Contractor
            if (!String.IsNullOrEmpty(sContractorIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.ContractorID in(" + sContractorIDs + ")";
            }
            #endregion
            #region Product Name
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.ProductID IN (" + sProductIDs + ")";
            }
            #endregion

            #region Issue Date
            if (nCboIssueDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboIssueDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dFromIssueDate.ToString("dd MMM yyyy") + " To " + dToIssueDate.ToString("dd MMM yyyy");
                }
                else if (nCboIssueDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),EPI.IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromIssueDate.ToString("dd MMM yyyy") + " To " + dToIssueDate.ToString("dd MMM yyyy");
                }
            }
            #endregion


            #region Business Unit
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " EPI.BUID = " + nBUID;
            //}
            #endregion
            string sSQL = sReturn1 + " " + sReturn + " ORDER BY ContractorID, EPI.ExportPIID,ProductID ASC";
            return sSQL;
        }
        public void PrintProductReconciliationReportYetToPOXL(string sTempString)
        {
             List<ProductReconciliationReportDetail> _oPRDetails = new List<ProductReconciliationReportDetail>();
             ProductReconciliationReportDetail oProductReconciliationReportDetail = new ProductReconciliationReportDetail();
             oProductReconciliationReportDetail.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oProductReconciliationReportDetail);
            _oPRDetails = ProductReconciliationReportDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nPIQty = 0;
            double nBPOQty = 0;
            double nQty = 0;
            double nQty_Prod = 0;
            double nYetTo_Prod = 0;

            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 14;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Reconciliation Report");
                sheet.Name = "Reconciliation Report";
                sheet.Column(3).Width = 40;
                sheet.Column(4).Width = 40;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
              

                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Reconciliation Report(Wating for Send production Order) " + _sDateRange; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "P/I Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Production Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Production Pending"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nRowIndex++;
                #endregion

                #region Data
                foreach (ProductReconciliationReportDetail oItem in _oPRDetails)
                {


                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.PIQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Qty_Prod); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = (oItem.YetToProduction); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                     nPIQty =nPIQty+oItem.PIQty;
                     nBPOQty = nBPOQty + oItem.OrderQty;
                     nQty = nQty + oItem.Qty;
                     nQty_Prod = nQty_Prod + oItem.Qty_Prod;
                     if (oItem.YetToProduction > 0)
                     {
                         nYetTo_Prod = nYetTo_Prod + oItem.YetToProduction;
                     }

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nPIQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nBPOQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nQty_Prod; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nYetTo_Prod; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ReconciliationReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        #endregion

        #region Import
        public ActionResult ViewProductReconciliation_Import(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oProductReconciliationReports = new List<ProductReconciliationReport>();
            ViewBag.BUID = buid;
            return View(_oProductReconciliationReports);

        }
        [HttpPost]
        public JsonResult GetsPRImport(ProductReconciliationReport oTB)
        {
            List<ProductReconciliationReport> oPRRs_Category = new List<ProductReconciliationReport>();
            if (oTB.ProductName == null)
            {
                oTB.ProductName = "";
            }

            try
            {
                _oProductReconciliationReports = ProductReconciliationReport.GetsImport(oTB.BUID, oTB.ProductName, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);
                if (oTB.ReportType != 2)
                {
                    oPRRs_Category = _oProductReconciliationReports.GroupBy(x => new { x.ProductCategoryID, x.CategoryName }, (key, grp) =>
                                     new ProductReconciliationReport
                                     {
                                         ProductCategoryID = key.ProductCategoryID,
                                         ProductID = 0,
                                         ProductName = "Category",
                                         CategoryName = key.CategoryName,
                                         StockInHand = grp.Sum(x => x.StockInHand),
                                         ProYetToWithLC = grp.Sum(x => x.ProYetToWithLC),
                                         ProYetToWithoutLC = grp.Sum(x => x.ProYetToWithoutLC),
                                         LCReceiveDONoReceive = grp.Sum(x => x.LCReceiveDONoReceive),
                                         PIIssueLCnDONotReceive = grp.Sum(x => x.PIIssueLCnDONotReceive),
                                         ProductionYetToSample = grp.Sum(x => x.ProductionYetToSample),
                                         ShipmentDone = grp.Sum(x => x.ShipmentDone),
                                         DocInCnF = grp.Sum(x => x.DocInCnF),
                                         DocReceive = grp.Sum(x => x.DocReceive),
                                         InvoiceWithoutBL = grp.Sum(x => x.InvoiceWithoutBL),
                                         LCOpen = grp.Sum(x => x.LCOpen),
                                         GoodsRelease = grp.Sum(x => x.GoodsRelease),
                                         GoodsinTrasit = grp.Sum(x => x.GoodsinTrasit),
                                         POReceive = grp.Sum(x => x.POReceive),
                                         MinimumStock = grp.Sum(x => x.MinimumStock),
                                         ProductionYetTo = grp.Sum(x => x.ProductionYetTo),
                                     }).ToList();

                    oPRRs_Category.ForEach(x => _oProductReconciliationReports.Add(x));
                    _oProductReconciliationReports = _oProductReconciliationReports.OrderBy(o => o.CategoryName).ThenBy(a => a.ProductID).ToList();
                }
            }
            catch (Exception ex)
            {
                _oProductReconciliationReport.ErrorMessage = ex.Message;
                _oProductReconciliationReports = new List<ProductReconciliationReport>();
            }

            var jsonResult = Json(_oProductReconciliationReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult ViewProductReconciliation_ImportDetail(int nProductID, int buid)
        {
            _oProductReconciliationReport = new ProductReconciliationReport();
            Product oProduct = new Product();
            oProduct = oProduct.Get(nProductID, (int)Session[SessionInfo.currentUserID]);
            _oProductReconciliationReport.ProductCode = oProduct.ProductCode;
            _oProductReconciliationReport.ProductName = '[' + oProduct.ProductCode + ']' + oProduct.ProductName;
            _oProductReconciliationReport.ProductID = oProduct.ProductID;
            try
            {
                ViewBag.BUID = buid;
                return View(_oProductReconciliationReport);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oProductReconciliationReport);
            }

        }
        [HttpPost]
        public JsonResult GetsPRDetail(ProductReconciliationReport oPRR)
        {
            ProductReconciliationReportDetail oProductReconciliationReportDetail = new ProductReconciliationReportDetail();
            List<ProductReconciliationReportDetail> oPRDs = new List<ProductReconciliationReportDetail>();
            try
            {
                oPRDs = ProductReconciliationReportDetail.Gets_PRDetail(oPRR.BUID, oPRR.ProductID, oPRR.StartDate, oPRR.EndDate, oPRR.ReportType, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPRDs = new List<ProductReconciliationReportDetail>();
                oProductReconciliationReportDetail = new ProductReconciliationReportDetail();
                oProductReconciliationReportDetail.ErrorMessage = ex.Message;
                oPRDs = new List<ProductReconciliationReportDetail>();
            }
            var jsonResult = Json(oPRDs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oPRDs);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPriview_Import(string sTempString)
        {
            ProductReconciliationReport oTB=new ProductReconciliationReport();
         
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string _sHeaderName = "";
            string _sDateRange = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                 oTB.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                 oTB.ProductName = sTempString.Split('~')[1];
                 oTB.ReportType = Convert.ToInt32(sTempString.Split('~')[2]);
                 oBusinessUnit = oBusinessUnit.Get(oTB.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 _oProductReconciliationReports = ProductReconciliationReport.GetsImport(oTB.BUID, oTB.ProductName, oTB.ReportType, (int)Session[SessionInfo.currentUserID]);

            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptProductReconciliationReport oReport = new rptProductReconciliationReport();
            byte[] abytes = null;

            abytes = oReport.PrepareReport_Import(_oProductReconciliationReports, oCompany, oBusinessUnit);
          
            return File(abytes, "application/pdf");


        }

        #region Print XL
        public void PrintPRR_ImportXL(string sTempString)
        {
            //ProductReconciliationReport 

            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            int nBUID = 0;
            int nReportType = 0;
            string sProductIDs = "";
            List<ProductReconciliationReport> oPRRs_Category = new List<ProductReconciliationReport>();

            if (!String.IsNullOrEmpty(sTempString))
            {
                nBUID = Convert.ToInt32(sTempString.Split('~')[0]);
                sProductIDs = sTempString.Split('~')[1];
                nReportType = Convert.ToInt32(sTempString.Split('~')[2]);
               
            }

            _oProductReconciliationReports = ProductReconciliationReport.GetsImport(nBUID, sProductIDs, nReportType, (int)Session[SessionInfo.currentUserID]);

            oPRRs_Category = _oProductReconciliationReports.GroupBy(x => new { x.ProductCategoryID, x.CategoryName }, (key, grp) =>
                               new ProductReconciliationReport
                               {
                                   ProductCategoryID = key.ProductCategoryID,
                                   ProductID = 0,
                                   ProductName = "Category",
                                   CategoryName = key.CategoryName,
                                   StockInHand = grp.Sum(x => x.StockInHand),
                                   ProYetToWithLC = grp.Sum(x => x.ProYetToWithLC),
                                   ProYetToWithoutLC = grp.Sum(x => x.ProYetToWithoutLC),
                                   LCReceiveDONoReceive = grp.Sum(x => x.LCReceiveDONoReceive),
                                   PIIssueLCnDONotReceive = grp.Sum(x => x.PIIssueLCnDONotReceive),
                                   ProductionYetToSample = grp.Sum(x => x.ProductionYetToSample),
                                   ShipmentDone = grp.Sum(x => x.ShipmentDone),
                                   DocInCnF = grp.Sum(x => x.DocInCnF),
                                   DocReceive = grp.Sum(x => x.DocReceive),
                                   InvoiceWithoutBL = grp.Sum(x => x.InvoiceWithoutBL),
                                   LCOpen = grp.Sum(x => x.LCOpen),
                                   GoodsRelease = grp.Sum(x => x.GoodsRelease),
                                   GoodsinTrasit = grp.Sum(x => x.GoodsinTrasit),
                                   POReceive = grp.Sum(x => x.POReceive),
                                   MinimumStock = grp.Sum(x => x.MinimumStock),
                                   ProductionYetTo = grp.Sum(x => x.ProductionYetTo),
                               }).ToList();

            oPRRs_Category.ForEach(x => _oProductReconciliationReports.Add(x));
            _oProductReconciliationReports = _oProductReconciliationReports.OrderBy(o => o.CategoryName).ThenBy(a => a.ProductID).ToList();

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nStockInHand = 0;
            double nProductionYetTo = 0;
            double nLCReceiveDONoReceive = 0;
            double nPIIssueLCnDONotReceive = 0;
           
          
            double nDocInCnF = 0;
            double nDocReceive = 0;
            double nShipmentDone = 0;
            double nShipmentInTransit = 0;
            double nPOReceive = 0;
            double nTotal_Import = 0;


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 14;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Reconciliation Report");
                sheet.Name = "Reconciliation Report";
                sheet.Column(2).Width = 40;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                sheet.Column(12).Width = 20;
             

                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Reconciliation Report  Date: " + DateTime.Today.ToString("dd MMM yyyy:HH:mm:tt") + ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Stock In Hand"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Goods in Trasit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Goods Release"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "DOC IN CnF "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "DOC Received"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Shipment Done"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Shipment In Transit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "P/I Recd"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Total "; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                nRowIndex++;
                #endregion

                #region Data
                foreach (ProductReconciliationReport oItem in _oProductReconciliationReports)
                {


                    if (oItem.ProductID <= 0)
                    {
                        cell = sheet.Cells[nRowIndex, 1]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 2]; cell.Merge = true;
                        cell.Value = "Category : " + oItem.ProductName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.GoodsinTrasit; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.GoodsRelease; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.DocInCnF; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DocReceive; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ShipmentDone; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.InvoiceWithoutBL + oItem.LCOpen; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.POReceive; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.Total_Import; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

 
                    }
                    else
                    {

                        //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.GoodsinTrasit; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.GoodsRelease; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.DocInCnF; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DocReceive; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ShipmentDone; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.InvoiceWithoutBL + oItem.LCOpen; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.POReceive; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.Total_Import; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nStockInHand = nStockInHand + oItem.StockInHand;
                        nProductionYetTo = nProductionYetTo + oItem.ProductionYetTo;
                        nLCReceiveDONoReceive = nLCReceiveDONoReceive + oItem.LCReceiveDONoReceive;
                        nPIIssueLCnDONotReceive = nPIIssueLCnDONotReceive + oItem.PIIssueLCnDONotReceive;

                        nDocInCnF = nDocInCnF + oItem.DocInCnF;
                        nDocReceive = nDocReceive + oItem.DocReceive;
                        nShipmentDone = nShipmentDone + oItem.ShipmentDone;
                        nShipmentInTransit = nShipmentInTransit + oItem.ShipmentInTransit;
                        nPOReceive = nPOReceive + oItem.POReceive;
                        nTotal_Import = nTotal_Import + oItem.Total_Import;
                    }

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nStockInHand = _oProductReconciliationReports.Select(c => c.GoodsinTrasit).Sum();

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nStockInHand = _oProductReconciliationReports.Select(c => c.GoodsRelease).Sum();
                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nDocInCnF; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nDocReceive; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nShipmentDone; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nShipmentInTransit; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nPOReceive; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nTotal_Import; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            


                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ReconciliationReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        #endregion

        #endregion

        #region
        [HttpPost]
        public JsonResult Save_PSort(ProductSort oProductSort)
        {
            oProductSort.RemoveNulls();
            try
            {
                oProductSort = oProductSort.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oProductSort = new ProductSort();
                oProductSort.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductSort);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductSort(ProductSort oProductSort)
        {
            
            try
            {
                if (oProductSort.ProductID <= 0) { throw new Exception("Please select a valid contractor."); }
                oProductSort = oProductSort.GetBy(oProductSort.ProductID, (int)Session[SessionInfo.currentUserID]);
              
            }
            catch (Exception ex)
            {
                oProductSort.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductSort);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
