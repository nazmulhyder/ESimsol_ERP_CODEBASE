using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using System.Diagnostics;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Timers;
using System.Net.Mail;

namespace ESimSolFinancial.Controllers
{
    public class SaleInvoiceController : Controller
    {
        #region Declaration
        string _sDateRange = "";
        SaleInvoice _oSaleInvoice = new SaleInvoice();
        List<SaleInvoice> _oSaleInvoices = new List<SaleInvoice>();
        #endregion

        public ActionResult ViewSaleInvoices(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.SaleInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUID = buid;

            _oSaleInvoices = new List<SaleInvoice>();
            string sSQL = "SELECT * FROM View_SaleInvoice AS HH WHERE ISNULL(HH.ApprovedBy,0) = 0 ORDER BY HH.SaleInvoiceID ASC";
            _oSaleInvoices = SaleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oSaleInvoices);
        }
        public ActionResult ViewSaleInvoice(int id)
        {
            _oSaleInvoice = new SaleInvoice();
            if (id > 0)
            {
                _oSaleInvoice = _oSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            }

            ViewBag.BaseCurrencyID = (int)Session[SessionInfo.BaseCurrencyID];
            ViewBag.BUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.VehicleLocations = EnumObject.jGets(typeof(EnumVehicleLocation));
            ViewBag.PaymentMethods = EnumObject.jGets(typeof(EnumPaymentMethod));
            return View(_oSaleInvoice);
        }
        public ActionResult ViewSaleInvoiceRegister(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.SaleInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUID = buid;

            ViewBag.MarketingAccounts = MarketingAccount.GetsByBU(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ReportLayouts = EnumObject.jGets(typeof(EnumReportLayout)).Where(x => x.id == (int)EnumReportLayout.Month_Wise || x.id == (int)EnumReportLayout.Marteking_Person_Wise || x.id == (int)EnumReportLayout.Vechile_Model_Wise);
            ViewBag.ReportTypes = EnumObject.jGets(typeof(EnumReportType)).Where(x => x.id == (int)EnumReportType.Sale_Invoice_Report || x.id == (int)EnumReportType.Sales_Quotation_Report);
            return View(_oSaleInvoice);
        }
        public ActionResult ViewSaleInvoice_Update(int id)
        {
            _oSaleInvoice = new SaleInvoice();
            if (id > 0)
            {
                _oSaleInvoice = _oSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oSaleInvoice);
        }

        [HttpPost]
        public JsonResult Save(SaleInvoice oSaleInvoice)
        {
            Debug.WriteLine(oSaleInvoice.Remarks);
            _oSaleInvoice = new SaleInvoice();
            try
            {
                _oSaleInvoice = oSaleInvoice;
                _oSaleInvoice = _oSaleInvoice.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSaleInvoice.ErrorMessage = ex.Message;
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSaleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStatus(SaleInvoice oSaleInvoice)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oSaleInvoice.UpdateStatus(oSaleInvoice, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Delete(SaleInvoice oSaleInvoice)
        {
            string sErrorMease = "";
            SaleInvoice _oSaleInvoice = new SaleInvoice();
            try
            {
                sErrorMease = _oSaleInvoice.Delete(oSaleInvoice.SaleInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region ADV SEARCH

        [HttpPost]
        public JsonResult AdvSearch(SaleInvoice oSaleInvoice)
        {
            List<SaleInvoice> oSaleInvoices = new List<SaleInvoice>();
            SaleInvoice _oSaleInvoice = new SaleInvoice();
            string sSQL = MakeSQL(oSaleInvoice);
            if (sSQL == "Error")
            {
                _oSaleInvoice = new SaleInvoice();
                _oSaleInvoice.ErrorMessage = "Please select a searching critaria.";
                oSaleInvoices = new List<SaleInvoice>();
            }
            else
            {
                oSaleInvoices = new List<SaleInvoice>();
                oSaleInvoices = SaleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oSaleInvoices.Count == 0)
                {
                    oSaleInvoices = new List<SaleInvoice>();
                }
            }
            var jsonResult = Json(oSaleInvoices, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(SaleInvoice oSaleInvoice)
        {
            string sParams = oSaleInvoice.Params;

            int nDateCriteria_Issue = 0;

            string sModelIDs = "",
                   sBuyerIDs = "";

            DateTime dStart_Issue = DateTime.Today,
                     dEnd_Issue = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 1;
                nDateCriteria_Issue = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                sModelIDs = sParams.Split('~')[nCount++];
                sBuyerIDs = sParams.Split('~')[nCount++];
            }

            string sReturn1 = "SELECT * FROM View_SaleInvoice AS EB";
            string sReturn = "";


            #region SQNo
            if (!string.IsNullOrEmpty(oSaleInvoice.SQNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.SQNo LIKE '%" + oSaleInvoice.SQNo + "%'";
            }
            #endregion


            #region KommNo
            if (!string.IsNullOrEmpty(oSaleInvoice.KommNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.KommNo LIKE '%" + oSaleInvoice.KommNo + "%'";
            }
            #endregion

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.InvoiceDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);
            #endregion

            #region Model IDs
            if (!string.IsNullOrEmpty(sModelIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.SalesQuotationID IN (SELECT SalesQuotationID FROM SalesQuotation WHERE VehicleModelID IN (" + sModelIDs + ")) ";
            }
            #endregion

            #region Customer IDs
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ContractorID IN (" + sBuyerIDs + ") ";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Performance Report
        private string MakeSQL_Register(string sParams, int nReportType)
        {
            int nDateCriteria_Issue = 0;

            string sModelIDs = "",
                   sMktAccountIDs = "",
                   sBuyerIDs = "";
            int nBUID = 0;

            DateTime dStart_Issue = DateTime.Today,
                     dEnd_Issue = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                nDateCriteria_Issue = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                sMktAccountIDs = sParams.Split('~')[nCount++];
                sBuyerIDs = sParams.Split('~')[nCount++];
                sModelIDs = sParams.Split('~')[nCount++];
                nBUID = Convert.ToInt32(sParams.Split('~')[nCount + 2]);
            }

            /*
             SELECT InvoiceDate, ContractorID, MarketingPerson, VehicleModelID,  FROM View_SaleInvoice
             SELECT QuotationDate, MarketingPerson,BuyerID,VehicleModelID FROM View_SalesQuotation
             */
            string sReturn1 = "SELECT * FROM View_SaleInvoice AS EB";

            if (nReportType == (int)EnumReportType.Sales_Quotation_Report)
                sReturn1 = "SELECT * FROM View_SalesQuotation AS EB";

            string sReturn = "";

            #region DATE SEARCH
            if (nReportType == (int)EnumReportType.Sale_Invoice_Report)
                DateObject.CompareDateQuery(ref sReturn, " EB.InvoiceDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);
            else DateObject.CompareDateQuery(ref sReturn, " EB.QuotationDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);
            #endregion

            #region Mkt AccountIDs
            if (!string.IsNullOrEmpty(sMktAccountIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.MarketingPerson IN (" + sMktAccountIDs + ")";
            }
            #endregion

            #region sBuyerIDs
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                if (nReportType == (int)EnumReportType.Sale_Invoice_Report)
                    sReturn = sReturn + " EB.ContractorID IN (" + sBuyerIDs + ")";
                else sReturn = sReturn + " EB.BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region sModelIDs
            if (!string.IsNullOrEmpty(sModelIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.VehicleModelID IN (" + sModelIDs + ")";
            }
            #endregion
            #region nBUID
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.BUID =" + nBUID;
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        public ActionResult PreviewSaleInvoice(int id)
        {
            _oSaleInvoice = _oSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

            rptSaleInvoice orptSaleInvoices = new rptSaleInvoice();
            byte[] abytes = orptSaleInvoices.PrepareReport(oCompany, _oSaleInvoice);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintPerformanceReport(string sTempString, int nReportType, int nReportLayout)
        {
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            SaleInvoice oSaleInvoice = new SaleInvoice();
            List<SaleInvoice> oSaleInvoices = new List<SaleInvoice>();
            List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();

            string sSQL = MakeSQL_Register(sTempString, nReportType);
            if (sSQL != "Error")
            {
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                if (nReportType == (int)EnumReportType.Sale_Invoice_Report)
                    oSaleInvoices = SaleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                else if (nReportType == (int)EnumReportType.Sales_Quotation_Report)
                    oSalesQuotations = SalesQuotation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oSaleInvoices.Count == 0 && oSalesQuotations.Count == 0)
            {
                rptErrorMessage orptErrorMessage = new rptErrorMessage();
                byte[] abytes = orptErrorMessage.PrepareReport("No Data Found!");
                return File(abytes, "application/pdf");
            }
            else
            {
                rptSalePerformaceReport orptSaleInvoices = new rptSalePerformaceReport();
                byte[] abytes = orptSaleInvoices.PrepareReport(oSaleInvoices, oSalesQuotations, oCompany, nReportLayout, nReportType, _sDateRange);
                return File(abytes, "application/pdf");
            }
        }
        #endregion

        public ActionResult SaleInvoiceChallan(int id)
        {
            _oSaleInvoice = _oSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

            List<VehicleChallan> oVCs = new List<VehicleChallan>();
            oVCs = VehicleChallan.Gets("SELECT TOP(1)* FROM View_VehicleChallan WHERE SaleInvoiceID=" + id, (int)Session[SessionInfo.currentUserID]);

            if (oVCs.Any())
            {
                rptSaleInvoice_Challan orptSaleInvoices = new rptSaleInvoice_Challan();
                byte[] abytes = orptSaleInvoices.PrepareReport(_oSaleInvoice, oVCs[0], oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage orptErrorMessage = new rptErrorMessage();
                byte[] abytes = orptErrorMessage.PrepareReport("NO VEHICLE CHALLAN FOUND!");
                return File(abytes, "application/pdf");
            }
        }

        public Image GetImage(byte[] oImage)
        {
            if (oImage != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #region Image
        public Image GetLargeImage(int SQImageid)//id is VehicleOrderImageID
        {
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            if (SQImageid > 0)
            {
                oSalesQuotationImage = oSalesQuotationImage.GetFrontImage(SQImageid, (int)Session[SessionInfo.currentUserID]);
                if (oSalesQuotationImage.LargeImage != null)
                {
                    MemoryStream m = new MemoryStream(oSalesQuotationImage.LargeImage);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                    img.Save(Response.OutputStream, ImageFormat.Jpeg);
                    return img;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Searching
        [HttpPost]
        public JsonResult Get(SaleInvoice oSaleInvoice)
        {
            _oSaleInvoice = new SaleInvoice();
            try
            {
                if (oSaleInvoice.SaleInvoiceID <= 0) { throw new Exception("Please select a valid SaleInvoice."); }
                _oSaleInvoice = _oSaleInvoice.Get(oSaleInvoice.SaleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSaleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSaleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.SaleInvoice, EnumProductUsages.Regular, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.SaleInvoice, EnumProductUsages.Regular, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jSonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        #endregion








        #region Excel
        public void PrintPerformanceReportExcelMonthWise(string sTempString, int nReportType, int nReportLayout)
        {
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            SaleInvoice oSaleInvoice = new SaleInvoice();
            List<SaleInvoice> oSaleInvoices = new List<SaleInvoice>();
            List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();

            string sSQL = MakeSQL_Register(sTempString, nReportType);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            if (nReportType == (int)EnumReportType.Sale_Invoice_Report)
            {
                oSaleInvoices = SaleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else if (nReportType == (int)EnumReportType.Sales_Quotation_Report)
            {
                oSalesQuotations = SalesQuotation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oSaleInvoices.Count > 0)
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Invoice No", Width = 10f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Invoice Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Marketing Person", Width = 30f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Komm No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 30f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Model No", Width = 40f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Chassis No / VIN", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Engine No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Status", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Amount", Width = 15f, IsRotate = false, Align = TextAlign.Right });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Performance Report");
                    sheet.Name = "Performance Report (Month Wise)";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = 12;
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Performance Report (Month Wise)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    #endregion

                    #region Data
                    nRowIndex++;
                    nStartCol = 2;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                    int nCount = 1; nEndCol = table_header.Count() + nStartCol;

                    int nPreviousMonthName = 0;
                    double dMonthWiseSubTotal = 0.0;
                    foreach (var obj in oSaleInvoices)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "";

                        if (nPreviousMonthName != obj.InvoiceDate.Month && nCount > 1)
                        {
                            nStartCol = 2;
                            ExcelTool.FillCellMerge(ref sheet, "Month Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 11, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                            ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 12, 12, true, ExcelHorizontalAlignment.Right, true);
                            nStartCol = 2;
                            nCount = 1;
                        }
                        if (nPreviousMonthName != obj.InvoiceDate.Month)
                        {
                            ExcelTool.FillCellMerge(ref sheet, "Month : " + obj.InvoiceDate.ToString("MMMM") + ", " + obj.InvoiceDate.Year.ToString(), nRowIndex, nRowIndex++, nStartCol, 12, true, ExcelHorizontalAlignment.Left, true);
                            nStartCol = 2;
                        }
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount++.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InvoiceNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InvoiceDateST, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MarketingAccountName, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.KommNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.CustomerName.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ModelNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ChassisNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.EngineNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InvoiceStatusST.ToString(), false);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit(obj.NetAmount).ToString(), true, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit((obj.OTRAmount - (obj.VatAmount + obj.RegistrationFee))).ToString(), true, false);

                        nRowIndex++;
                        nPreviousMonthName = obj.InvoiceDate.Month;
                        //dMonthWiseSubTotal = oSaleInvoices.Where(x => x.InvoiceDate.Month.Equals(obj.InvoiceDate.Month)).Sum(x => x.NetAmount);
                        dMonthWiseSubTotal = oSaleInvoices.Where(x => x.InvoiceDate.Month.Equals(obj.InvoiceDate.Month)).Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    }
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Month Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 11, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 12, 12, true, ExcelHorizontalAlignment.Right, true);

                    #region Grand Total
                    //double dGrandTotal = oSaleInvoices.Sum(x => x.NetAmount);
                    double dGrandTotal = oSaleInvoices.Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 11, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 12, 12, true, ExcelHorizontalAlignment.Right, true);
                    #endregion

                    #endregion
                    nRowIndex++;

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Performance Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            oSalesQuotations = oSalesQuotations.OrderBy(x => x.QuotationDate).ToList();
            if (oSalesQuotations.Count > 0)
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "SQ No", Width = 10f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "SQ Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Marketing Person", Width = 30f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Komm No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 30f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Model No", Width = 40f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Chassis No / VIN", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Engine No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Status", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Amount", Width = 15f, IsRotate = false, Align = TextAlign.Right });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Performance Report");
                    sheet.Name = "Performance Report (Month Wise)";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = 12;
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Performance Report (Month Wise)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    #endregion

                    #region Data
                    nRowIndex++;
                    nStartCol = 2;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                    int nCount = 1; nEndCol = table_header.Count() + nStartCol;

                    int nPreviousMonthName = 0;
                    double dMonthWiseSubTotal = 0.0;
                    foreach (var obj in oSalesQuotations)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "";

                        if (nPreviousMonthName != obj.QuotationDate.Month && nCount > 1)
                        {
                            nStartCol = 2;
                            ExcelTool.FillCellMerge(ref sheet, "Month Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 11, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                            ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 12, 12, true, ExcelHorizontalAlignment.Right, true);
                            nStartCol = 2;
                            nCount = 1;
                        }
                        if (nPreviousMonthName != obj.QuotationDate.Month)
                        {
                            ExcelTool.FillCellMerge(ref sheet, "Month : " + obj.QuotationDate.ToString("MMMM") + ", " + obj.QuotationDate.Year.ToString(), nRowIndex, nRowIndex++, nStartCol, 12, true, ExcelHorizontalAlignment.Left, true);
                            nStartCol = 2;
                        }
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount++.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FileNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.QuotationDateInString, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MarketingAccountName, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.KommNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BuyerName.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ModelNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ChassisNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.EngineNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SalesStatusInString.ToString(), false);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit(obj.OfferPrice).ToString(), true, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit((obj.OTRAmount - (obj.VatAmount + obj.RegistrationFee))).ToString(), true, false);

                        nRowIndex++;
                        nPreviousMonthName = obj.QuotationDate.Month;
                        dMonthWiseSubTotal = oSalesQuotations.Where(x => x.QuotationDate.Month.Equals(obj.QuotationDate.Month)).Sum(x => x.OfferPrice);
                        dMonthWiseSubTotal = oSalesQuotations.Where(x => x.QuotationDate.Month.Equals(obj.QuotationDate.Month)).Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    }
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Month Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 11, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 12, 12, true, ExcelHorizontalAlignment.Right, true);

                    #region Grand Total
                    //double dGrandTotal = oSalesQuotations.Sum(x => x.OfferPrice);
                    double dGrandTotal = oSalesQuotations.Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 11, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 12, 12, true, ExcelHorizontalAlignment.Right, true);
                    #endregion

                    #endregion
                    nRowIndex++;

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Performance Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }





        }
        public void PrintPerformanceReportExcelMKTPersonWise(string sTempString, int nReportType, int nReportLayout)
        {
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            SaleInvoice oSaleInvoice = new SaleInvoice();
            List<SaleInvoice> oSaleInvoices = new List<SaleInvoice>();
            List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();

            string sSQL = MakeSQL_Register(sTempString, nReportType);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            if (nReportType == (int)EnumReportType.Sale_Invoice_Report)
            {
                oSaleInvoices = SaleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else if (nReportType == (int)EnumReportType.Sales_Quotation_Report)
            {
                oSalesQuotations = SalesQuotation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oSaleInvoices.Count > 0)
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Invoice No", Width = 10f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Invoice Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Komm No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 30f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Model No", Width = 40f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Chassis No / VIN", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Engine No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Status", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Amount", Width = 15f, IsRotate = false, Align = TextAlign.Right });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Performance Report");
                    sheet.Name = "Performance Report (Marketing Person Wise)";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = 11;
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Performance Report (Marketing Person Wise)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    #endregion

                    #region Data
                    nRowIndex++;
                    nStartCol = 2;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                    int nCount = 1; nEndCol = table_header.Count() + nStartCol;

                    string sPreviousMKTPName = "";
                    double dMonthWiseSubTotal = 0.0;
                    oSaleInvoices = oSaleInvoices.OrderBy(x => x.MarketingAccountName).ToList();
                    foreach (var obj in oSaleInvoices)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "";

                        if (sPreviousMKTPName != obj.MarketingAccountName && nCount > 1)
                        {
                            nStartCol = 2;
                            ExcelTool.FillCellMerge(ref sheet, "Marketing Person Wise Sub-Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                            ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);
                            nStartCol = 2;
                            nCount = 1;
                        }
                        if (sPreviousMKTPName != obj.MarketingAccountName)
                        {
                            ExcelTool.FillCellMerge(ref sheet, "Marketing person : " + obj.MarketingAccountName, nRowIndex, nRowIndex++, nStartCol, 11, true, ExcelHorizontalAlignment.Left, true);
                            nStartCol = 2;
                        }
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount++.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InvoiceNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InvoiceDateST, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.KommNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.CustomerName.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ModelNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ChassisNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.EngineNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InvoiceStatusST.ToString(), false);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit(obj.NetAmount).ToString(), true, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit((obj.OTRAmount - (obj.VatAmount + obj.RegistrationFee))).ToString(), true, false);

                        nRowIndex++;
                        sPreviousMKTPName = obj.MarketingAccountName;
                        //dMonthWiseSubTotal = oSaleInvoices.Where(x => x.MarketingAccountName.Equals(obj.MarketingAccountName)).Sum(x => x.NetAmount);
                        dMonthWiseSubTotal = oSaleInvoices.Where(x => x.MarketingAccountName.Equals(obj.MarketingAccountName)).Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    }
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Marketing Person Wise Sub-Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);

                    #region Grand Total
                    //double dGrandTotal = oSaleInvoices.Sum(x => x.NetAmount);
                    double dGrandTotal = oSaleInvoices.Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);
                    #endregion

                    #endregion
                    nRowIndex++;

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Performance Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            oSalesQuotations = oSalesQuotations.OrderBy(x => x.MarketingAccountName).ToList();
            if (oSalesQuotations.Count > 0)
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "SQ No", Width = 10f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "SQ Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Komm No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 30f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Model No", Width = 40f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Chassis No / VIN", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Engine No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Status", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Amount", Width = 15f, IsRotate = false, Align = TextAlign.Right });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Performance Report");
                    sheet.Name = "Performance Report (Marketing Person Wise)";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = 11;
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Performance Report (Marketing Person Wise)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    #endregion

                    #region Data
                    nRowIndex++;
                    nStartCol = 2;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                    int nCount = 1; nEndCol = table_header.Count() + nStartCol;

                    string sPreviousMKTPName = "";
                    double dMonthWiseSubTotal = 0.0;
                    foreach (var obj in oSalesQuotations)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "";

                        if (sPreviousMKTPName != obj.MarketingAccountName && nCount > 1)
                        {
                            nStartCol = 2;
                            ExcelTool.FillCellMerge(ref sheet, "Marketing Person Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                            ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);
                            nStartCol = 2;
                            nCount = 1;
                        }
                        if (sPreviousMKTPName != obj.MarketingAccountName)
                        {
                            ExcelTool.FillCellMerge(ref sheet, "Marketing Person : " + obj.MarketingAccountName, nRowIndex, nRowIndex++, nStartCol, 11, true, ExcelHorizontalAlignment.Left, true);
                            nStartCol = 2;
                        }
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount++.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FileNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.QuotationDateInString, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.KommNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BuyerName.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ModelNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ChassisNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.EngineNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SalesStatusInString.ToString(), false);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit(obj.OfferPrice).ToString(), true, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit((obj.OTRAmount - (obj.VatAmount + obj.RegistrationFee))).ToString(), true, false);

                        nRowIndex++;
                        sPreviousMKTPName = obj.MarketingAccountName;
                        //dMonthWiseSubTotal = oSalesQuotations.Where(x => x.MarketingAccountName.Equals(obj.MarketingAccountName)).Sum(x => x.OfferPrice);
                        dMonthWiseSubTotal = oSalesQuotations.Where(x => x.MarketingAccountName.Equals(obj.MarketingAccountName)).Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    }
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Marketing person Sub Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);

                    #region Grand Total
                    //double dGrandTotal = oSalesQuotations.Sum(x => x.OfferPrice);
                    double dGrandTotal = oSalesQuotations.Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);
                    #endregion

                    #endregion
                    nRowIndex++;

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Performance Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }

        }
        public void PrintPerformanceReportExcelModelWise(string sTempString, int nReportType, int nReportLayout)
        {
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            SaleInvoice oSaleInvoice = new SaleInvoice();
            List<SaleInvoice> oSaleInvoices = new List<SaleInvoice>();
            List<SalesQuotation> oSalesQuotations = new List<SalesQuotation>();

            string sSQL = MakeSQL_Register(sTempString, nReportType);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            if (nReportType == (int)EnumReportType.Sale_Invoice_Report)
            {
                oSaleInvoices = SaleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else if (nReportType == (int)EnumReportType.Sales_Quotation_Report)
            {
                oSalesQuotations = SalesQuotation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oSaleInvoices.Count > 0)
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Invoice No", Width = 10f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Invoice Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Marketing Person", Width = 40f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Komm No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 30f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Chassis No / VIN", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Engine No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Status", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Amount", Width = 15f, IsRotate = false, Align = TextAlign.Right });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Performance Report");
                    sheet.Name = "Performance Report (Model Wise)";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = 11;
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Performance Report (Model Wise)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    #endregion

                    #region Data
                    nRowIndex++;
                    nStartCol = 2;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                    int nCount = 1; nEndCol = table_header.Count() + nStartCol;

                    string sPreviousModelNo = "";
                    double dMonthWiseSubTotal = 0.0;
                    oSaleInvoices = oSaleInvoices.OrderBy(x => x.ModelNo).ToList();
                    foreach (var obj in oSaleInvoices)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "";

                        if (sPreviousModelNo != obj.ModelNo && nCount > 1)
                        {
                            nStartCol = 2;
                            ExcelTool.FillCellMerge(ref sheet, "Model Wise Sub-Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                            ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);
                            nStartCol = 2;
                            nCount = 1;
                        }
                        if (sPreviousModelNo != obj.ModelNo)
                        {
                            ExcelTool.FillCellMerge(ref sheet, "Model No : " + obj.ModelNo, nRowIndex, nRowIndex++, nStartCol, 11, true, ExcelHorizontalAlignment.Left, true);
                            nStartCol = 2;
                        }
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount++.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InvoiceNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InvoiceDateST, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MarketingAccountName.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.KommNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.CustomerName.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ChassisNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.EngineNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InvoiceStatusST.ToString(), false);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit(obj.NetAmount).ToString(), true, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit((obj.OTRAmount - (obj.VatAmount + obj.RegistrationFee))).ToString(), true, false);

                        nRowIndex++;
                        sPreviousModelNo = obj.ModelNo;
                        //dMonthWiseSubTotal = oSaleInvoices.Where(x => x.ModelNo.Equals(obj.ModelNo)).Sum(x => x.NetAmount);
                        dMonthWiseSubTotal = oSaleInvoices.Where(x => x.ModelNo.Equals(obj.ModelNo)).Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    }
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Model Wise Sub-Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);

                    #region Grand Total
                    //double dGrandTotal = oSaleInvoices.Sum(x => x.NetAmount);
                    double dGrandTotal = oSaleInvoices.Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);
                    #endregion

                    #endregion
                    nRowIndex++;

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Performance Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            oSalesQuotations = oSalesQuotations.OrderBy(x => x.ModelNo).ToList();
            if (oSalesQuotations.Count > 0)
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "SQ No", Width = 10f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "SQ Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Marketing Person", Width = 40f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Komm No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 30f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Chassis No / VIN", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Engine No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Status", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Amount", Width = 15f, IsRotate = false, Align = TextAlign.Right });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Performance Report");
                    sheet.Name = "Performance Report (Model Wise)";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = 11;
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Performance Report (Model Wise)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    #endregion

                    #region Data
                    nRowIndex++;
                    nStartCol = 2;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                    int nCount = 1; nEndCol = table_header.Count() + nStartCol;

                    string sPreviousModelNo = "";
                    double dMonthWiseSubTotal = 0.0;
                    foreach (var obj in oSalesQuotations)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "";

                        if (sPreviousModelNo != obj.ModelNo && nCount > 1)
                        {
                            nStartCol = 2;
                            ExcelTool.FillCellMerge(ref sheet, "Model Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                            ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);
                            nStartCol = 2;
                            nCount = 1;
                        }
                        if (sPreviousModelNo != obj.ModelNo)
                        {
                            ExcelTool.FillCellMerge(ref sheet, "Model No : " + obj.ModelNo, nRowIndex, nRowIndex++, nStartCol, 11, true, ExcelHorizontalAlignment.Left, true);
                            nStartCol = 2;
                        }
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount++.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FileNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.QuotationDateInString, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MarketingAccountName.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.KommNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BuyerName.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ChassisNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.EngineNo.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SalesStatusInString.ToString(), false);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit(obj.OfferPrice).ToString(), true, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormatActualDigit((obj.OTRAmount - (obj.VatAmount + obj.RegistrationFee))).ToString(), true, false);

                        nRowIndex++;
                        sPreviousModelNo = obj.ModelNo;
                        //dMonthWiseSubTotal = oSalesQuotations.Where(x => x.ModelNo.Equals(obj.ModelNo)).Sum(x => x.OfferPrice);
                        dMonthWiseSubTotal = oSalesQuotations.Where(x => x.ModelNo.Equals(obj.ModelNo)).Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    }
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Model Sub Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, true);
                    ExcelTool.FillCellMerge(ref sheet, Global.MillionFormatActualDigit(dMonthWiseSubTotal).ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);

                    #region Grand Total
                    //double dGrandTotal = oSalesQuotations.Sum(x => x.OfferPrice);
                    double dGrandTotal = oSalesQuotations.Sum(x => (x.OTRAmount - (x.VatAmount + x.RegistrationFee)));
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 11, 11, true, ExcelHorizontalAlignment.Right, true);
                    #endregion

                    #endregion
                    nRowIndex++;

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Performance Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }





        }



        #endregion

        #region Auto-Generate Mail
        [HttpPost]
        public JsonResult GetServiceSchedule()
        {
            string sMsg = "";
            List<ServiceSchedule> oServiceSchedules = new List<ServiceSchedule>();
            oServiceSchedules = ServiceSchedule.Gets("SELECT * FROM View_ServiceSchedule WHERE PreInvoiceID>0 AND IsDone = 'false' AND ServiceDate BETWEEN GETDATE() AND DATEADD(day, 2, GETDATE())", -9);
            //string sReturn1 = "UPDATE FabricSalesContractDetail ";
            //bool bIsWithMail = false;
            //bIsWithMail = oFabricSalesContractDetail.IsWithMail;
            try
            {
                this.SendMail();
            }
            catch (Exception ex) { sMsg = ex.Message; }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMsg);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string SendMail()
         {
            try
            {
                string sSubject = "For Testing";
                string sBodyInformation = "<h2>Mr. " + "Ali Akram" + ",</h2>";

                List<string> sMailTos = new List<string>();
                sMailTos.Add("aliakramtushar@gmail.com");
                List<string> sMailCCs = new List<string>();

                #region Email Credential
                EmailConfig oEmailConfig = new EmailConfig();
                oEmailConfig = oEmailConfig.GetByBU(1, -9);
                #endregion

                Global.MailSend(sSubject, sBodyInformation, sMailTos, sMailCCs, new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
            }
            catch (Exception ex)
            {
                return "Status is updated but failed to send mail. Because of " + ex.Message + "!";
            }
            return Global.SuccessMessage;
        }
        #endregion
    }
}
