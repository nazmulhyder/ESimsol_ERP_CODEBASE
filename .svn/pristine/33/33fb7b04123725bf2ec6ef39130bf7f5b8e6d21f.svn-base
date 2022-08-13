using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class LabDipReportController : Controller
    {
        #region Declaration
        LabDipReport _oLabDipReport = new LabDipReport();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<LabDipReport> _oLabDipReports = new List<LabDipReport>();
        string _sDateRange = "";
        #endregion

        #region LabDipReport
        public ActionResult View_LabDipReports(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            //ViewBag.PaymentTypes = OrderPaymentTypeObj.Gets();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat)); //LabdipFormatObj.Gets();
            ViewBag.LabdipStatus = EnumObject.jGets(typeof(EnumLabdipOrderStatus)); //Enum.GetValues(typeof(EnumLabdipOrderStatus)).Cast<EnumLabdipOrderStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.LabDipSetup = oLabDipSetup;
            return View(_oLabDipReports);
        }
        public ActionResult View_LabDipReports_Product(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LabdipFormats = EnumObject.jGets(typeof(EnumLabdipFormat)); //LabdipFormatObj.Gets();
            ViewBag.LabdipStatus = EnumObject.jGets(typeof(EnumLabdipOrderStatus)); //Enum.GetValues(typeof(EnumLabdipOrderStatus)).Cast<EnumLabdipOrderStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.LabDipSetup = oLabDipSetup;
            return View(_oLabDipReports);
        }
    
        [HttpPost]
        public JsonResult AdvSearch(LabDipReport oLabDipReport)
        {
            _oLabDipReports = new List<LabDipReport>();
            try
            {
                string sSQL = MakeSQL(oLabDipReport,false);
                _oLabDipReports = LabDipReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLabDipReports = new List<LabDipReport>();
                _oLabDipReport.ErrorMessage = ex.Message;
                _oLabDipReports.Add(_oLabDipReport);
            }
            var jsonResult = Json(_oLabDipReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AdvSearch_Product(LabDipReport oLabDipReport)
        {
            _oLabDipReports = new List<LabDipReport>();
            try
            {
                string sSQL = MakeSQL(oLabDipReport,false);
                _oLabDipReports = LabDipReport.Gets_Product(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLabDipReports = new List<LabDipReport>();
                _oLabDipReport.ErrorMessage = ex.Message;
                _oLabDipReports.Add(_oLabDipReport);
            }
            var jsonResult = Json(_oLabDipReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(LabDipReport oLabDipReport,bool IsSummary)
        {
            string sParams = oLabDipReport.ErrorMessage;

       
            int ncboOrderDate = 0;
            DateTime dFromOrderDate = DateTime.Today;
            DateTime dToOrderDate = DateTime.Today;
            int ncboSeekingDate = 0;
            DateTime dFromSeekingDate = DateTime.Today;
            DateTime dToSeekingDate = DateTime.Today;
            int ncboStatusDate = 0;
            DateTime dFromStatusDate = DateTime.Today;
            DateTime dToStatusDate = DateTime.Today;
            int nCboMkPerson = 0;
            string sCurrentStatus = "";
         
            string sColorNo = "";
            string sLabdipNo = "";
            string sColorName = "";
            string sPantonNo = "";
            string sNote = "";
            string sRefNo = "";
            string sTemp = "";
            int nBUID = 0;

        

            if (!string.IsNullOrEmpty(sParams))
            {
               
                _oLabDipReport.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                _oLabDipReport.DeliveryToName = Convert.ToString(sParams.Split('~')[1]);
                _oLabDipReport.ProductName = Convert.ToString(sParams.Split('~')[2]);
                ncboOrderDate = Convert.ToInt32(sParams.Split('~')[3]);
                dFromOrderDate = Convert.ToDateTime(sParams.Split('~')[4]);
                dToOrderDate = Convert.ToDateTime(sParams.Split('~')[5]);

                ncboSeekingDate = Convert.ToInt32(sParams.Split('~')[6]);
                dFromSeekingDate = Convert.ToDateTime(sParams.Split('~')[7]);
                dToSeekingDate = Convert.ToDateTime(sParams.Split('~')[8]);

                ncboStatusDate = Convert.ToInt32(sParams.Split('~')[9]);
                dFromStatusDate = Convert.ToDateTime(sParams.Split('~')[10]);
                dToStatusDate = Convert.ToDateTime(sParams.Split('~')[11]);
                
                nCboMkPerson = Convert.ToInt32(sParams.Split('~')[12]);
                sCurrentStatus = Convert.ToString(sParams.Split('~')[13]);
               
                sLabdipNo = Convert.ToString(sParams.Split('~')[14]);
                sColorNo = Convert.ToString(sParams.Split('~')[15]);
                sColorName = Convert.ToString(sParams.Split('~')[16]);
                sPantonNo = Convert.ToString(sParams.Split('~')[17]);
                _oLabDipReport.ContractorCPName = Convert.ToString(sParams.Split('~')[18]);
            }


            string sReturn1 = "";
            string sReturn = "";
           
            sReturn1 = "";
          
            #region Contractor
            if (!String.IsNullOrEmpty(_oLabDipReport.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + _oLabDipReport.ContractorName + ")";
            }
            #endregion

            #region Buyer Id
            if (!String.IsNullOrEmpty(_oLabDipReport.DeliveryToName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DeliveryToID in(" + _oLabDipReport.DeliveryToName + ")";
            }
            #endregion

            #region Product
            if (!String.IsNullOrEmpty(_oLabDipReport.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + _oLabDipReport.ProductName + ")";
            }
            #endregion

            #region Issue Date
            if (ncboOrderDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboOrderDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dFromOrderDate.ToString("dd MMM yyyy") + " To " + dToOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromOrderDate.ToString("dd MMM yyyy") + " To " + dToOrderDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region SeekingDate
            if (ncboSeekingDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (ncboSeekingDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SeekingDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSeekingDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSeekingDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SeekingDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSeekingDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSeekingDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SeekingDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSeekingDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSeekingDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SeekingDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSeekingDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSeekingDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SeekingDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSeekingDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSeekingDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSeekingDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SeekingDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSeekingDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSeekingDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion


            #region Status With Date
            if (ncboStatusDate != (int)EnumCompareOperator.None)
            {
                if (!String.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboStatusDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " LabDipID in (Select LabDipID from LabdipHistory where CurrentStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    if (ncboStatusDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " LabDipID in (Select LabDipID from LabdipHistory where CurrentStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToStatusDate.ToString("dd MMM yyyy") + "',106)) ) ";
                    }
                }

            }
            #endregion

            #region Mkt. Person
            if (nCboMkPerson > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MktPersonID = " + nCboMkPerson;
            }
            #endregion

            #region Current Status
            if (!string.IsNullOrEmpty(sCurrentStatus))
            {
                if (ncboStatusDate == (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " [OrderStatus] IN (" + sCurrentStatus + ")";
                }
            }
            #endregion

            #region sPantonNo
            if (!string.IsNullOrEmpty(sPantonNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PantonNo LIKE '%" + sPantonNo + "%'";
            }
            #endregion

            #region sColorName
            if (!string.IsNullOrEmpty(sColorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ColorName LIKE '%" + sColorName + "%'";
            }
            #endregion

            #region LC No
            if (!string.IsNullOrEmpty(sLabdipNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LabdipNo LIKE '%" + sLabdipNo + "%' ";
            }
            #endregion
            #region PINo No
            if (!string.IsNullOrEmpty(sColorNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ColorNo LIKE '%" + sColorNo + "%' ";
            }
            #endregion

            #region Note 
            if (!string.IsNullOrEmpty(sNote))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Note LIKE '%" + sNote + "%'";
            }
            #endregion
            #region RefNo
            if (!string.IsNullOrEmpty(sRefNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo LIKE '%" + sRefNo + "%'";
            }
            #endregion

            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region Contractor
            if (!String.IsNullOrEmpty(_oLabDipReport.ContractorCPName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContactPersonnelID in(" + _oLabDipReport.ContractorCPName + ") or DeliveryToContactPersonnelID in(" + _oLabDipReport.ContractorCPName + ")";
            }
            #endregion
              string sSQL ="";
              if (!IsSummary)
              {
                  sSQL = sReturn1 + " " + sReturn + " Order by OrderDate DESC";
              }
              else
              {
                  sSQL = sReturn1 + " " + sReturn ;
              }
            return sSQL;
        }

        #region Print Labdid Order
        public ActionResult Print_LabdipReportReport(string sTempString)
        {
            _oLabDipReports = new List<LabDipReport>();
            LabDipReport oLabDipReport = new LabDipReport();
            oLabDipReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oLabDipReport,false);
            _oLabDipReports = LabDipReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptLabdipReportAll oReport = new rptLabdipReportAll();
            byte[] abytes = oReport.PrepareReport(_oLabDipReports, oCompanys.First(), oBusinessUnit, _sDateRange, oLabDipSetup);
            return File(abytes, "application/pdf");
        }
        public void PrintExcel_LabdipReportReport(string sTempString)
        {
            _oLabDipReports = new List<LabDipReport>();
            LabDipReport oLabDipReport = new LabDipReport();
            oLabDipReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oLabDipReport,false);
            _oLabDipReports = LabDipReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oLabDipReports = _oLabDipReports.OrderBy(x => x.OrderDate).ToList();
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 11;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nSL = 0;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Lab Dip Order Register");
                sheet.Name = "Lab Dip Order Register";
                sheet.Column(2).Width = 22;
                sheet.Column(3).Width = 12;
                sheet.Column(4).Width = 28;
                sheet.Column(5).Width = 28;
                sheet.Column(6).Width = 17;
                sheet.Column(7).Width = 22;
                sheet.Column(8).Width = 25;
                sheet.Column(9).Width = 25;
                sheet.Column(10).Width = 25;
                sheet.Column(11).Width = 18;
                sheet.Column(12).Width = 20;

                sheet.Column(10).Width = 18;

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

                #region Header Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "LabDip Order Register  "; cell.Style.Font.Bold = true;
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

                #region Head Column

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Lab Dip No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Master Buyer"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Buyer Concern person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "No Of Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Color NO"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Color Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Panton No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Delivery Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               

                nRowIndex++;
                #endregion

                #region Data
                foreach (LabDipReport oItem in _oLabDipReports)
                {

                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.OrderDateStr; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + oItem.LabdipNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.EndBuyer; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex,6]; cell.Value = oItem.ContractorCPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.NoOfColor; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ColorNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.PantonNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.DeliveyDateStr; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.ChallanNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LabdipOderRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
            
        }
        public ActionResult Print_LabdipReportReport_Product(string sTempString)
        {
            _oLabDipReports = new List<LabDipReport>();
            LabDipReport oLabDipReport = new LabDipReport();
            oLabDipReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oLabDipReport,false);
            _oLabDipReports = LabDipReport.Gets_Product(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptLabdipReportAll oReport = new rptLabdipReportAll();
            byte[] abytes = oReport.PrepareReport(_oLabDipReports, oCompanys.First(), oBusinessUnit, _sDateRange, oLabDipSetup);
            return File(abytes, "application/pdf");
        }

        public ActionResult Print_LabdipReportReport_Summary(string sTempString)
        {
            _oLabDipReports = new List<LabDipReport>();
            LabDipReport oLabDipReport = new LabDipReport();
            oLabDipReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oLabDipReport, true);

            sSQL = "SELECT  ContractorID,ContractorName,Count(Distinct(LabDipID)) as LabDipID,Count(Distinct(LabdipDetailID)) as ColorSet   FROM View_LabDipReport " + sSQL + " Group by ContractorID,ContractorName  order by ColorSet desc";

            _oLabDipReports = LabDipReport.GetsSql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptLabdipReportAll oReport = new rptLabdipReportAll();
            byte[] abytes = oReport.PrepareReportSUmmary(_oLabDipReports, oCompanys.First(), oBusinessUnit, _sDateRange);
            return File(abytes, "application/pdf");
        }
       
        public void PrintExcel_LabdipReportReport_Product(string sTempString)
        {
            _oLabDipReports = new List<LabDipReport>();
            LabDipReport oLabDipReport = new LabDipReport();
            oLabDipReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oLabDipReport,false);
            _oLabDipReports = LabDipReport.Gets_Product(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = oLabDipSetup.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oLabDipReports = _oLabDipReports.OrderBy(x => x.OrderDate).ToList();
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 8;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nSL = 0;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Lab Dip Order Register");
                sheet.Name = "Lab Dip Order Register";
                sheet.Column(2).Width = 22;
                sheet.Column(3).Width = 12;
                sheet.Column(4).Width = 28;
                sheet.Column(5).Width = 28;
                sheet.Column(6).Width = 17;
                sheet.Column(7).Width = 22;
                sheet.Column(8).Width = 25;
                sheet.Column(9).Width = 25;

                sheet.Column(10).Width = 18;
                
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

                #region Header Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "LabDip Order Register  "; cell.Style.Font.Bold = true;
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

                #region Head Column

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Lab Dip Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Master Buyer"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Buyer Concern person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "No Of Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Color NO"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Color Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Panton No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion
              
                #region Data
                foreach (LabDipReport oItem in _oLabDipReports)
                {

                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.LabdipNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderDateStr; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex,6]; cell.Value = oItem.EndBuyer; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.NoOfColor; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ColorNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.PantonNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion
                
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LabdipOderRegister.xlsx");
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


        public ActionResult PrintShowDetail(int nId, double nts)
        {
            LabDip oLabDip = new LabDip();
            List<LabdipChallan> oLabdipChallans = new List<LabdipChallan>();
            List<LabdipRecipe> oLabdipRecipes = new List<LabdipRecipe>();
            List<LabdipShade> oLabdipShades = new List<LabdipShade>();
            List<LabdipHistory> oLabdipHistorys = new List<LabdipHistory>();
            try
            {
                if (nId > 0)
                {
                    oLabDip = LabDip.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oLabDip.LabDipID > 0)
                    {
                        string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + oLabDip.LabDipID;
                        oLabDip.LabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oLabdipChallans = LabdipChallan.Gets("SELECT * FROM View_LabdipChallan WHERE LabdipChallanID IN ("+ string.Join(",",oLabDip.LabDipDetails.Select(x=>x.LabdipChallanID)) +")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                        //sSQL = "SELECT * FROM View_LabDipReport Where LabDipID=" + oLabDip.LabDipID;
                        //_oLabDipReports = LabDipReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "Select * from View_LabdipShade Where LabdipDetailID in (Select LabdipDetailID from LabdipDetail where LabdipID=" + oLabDip.LabDipID + ")";
                        oLabdipShades = LabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        sSQL = "Select * from View_LabdipRecipe Where LabdipShadeID in (Select LabdipShadeID from LabdipShade Where LabdipDetailID in (Select LabdipDetailID from LabdipDetail where LabdipID=" + oLabDip.LabDipID + "))";
                        oLabdipRecipes = LabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oLabdipHistorys = LabdipHistory.Gets(oLabDip.LabDipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLabDipInfo oReport = new rptLabDipInfo();
            byte[] abytes = oReport.PrepareReport(oLabDip, oCompany, oBusinessUnit, oLabdipShades, oLabdipRecipes, oLabdipHistorys, oLabdipChallans);
            return File(abytes, "application/pdf");

        }

        #endregion

      

     
    }
}
