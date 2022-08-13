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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class SampleOutStandingController : Controller
    {
        #region Declaration
        SampleOutStanding _oSampleOutStanding = new SampleOutStanding();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<SampleOutStanding> _oSampleOutStandings = new List<SampleOutStanding>();
        bool _bIsDr = true;
        string _sDateRange = "";
        #endregion

        #region SampleOutStanding 
        public ActionResult View_SampleOutStandings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            //ViewBag.PaymentTypes = OrderPaymentTypeObj.Gets();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
          
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oSampleOutStandings);
        }
        public ActionResult View_SampleOutStandings_Mkt(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            //ViewBag.PaymentTypes = OrderPaymentTypeObj.Gets();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oSampleOutStandings);
        }
        [HttpPost]
        public JsonResult AdvSearch_Party(SampleOutStanding oSampleOutStanding)
        {
            _oSampleOutStandings = new List<SampleOutStanding>();
            string sParams = oSampleOutStanding.ErrorMessage;
            int ncboOrderDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                ncboOrderDate = Convert.ToInt32(sParams.Split('~')[1]);
                dStartDate = Convert.ToDateTime(sParams.Split('~')[2]);
                dEndDate = Convert.ToDateTime(sParams.Split('~')[3]);

                _oSampleOutStanding.MKTPName = string.Empty;
                if (sParams.Split('~').Length > 4)
                    _oSampleOutStanding.MKTPName = sParams.Split('~')[4];
               

            }
            try
            {
                if (!string.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
                {
                    _oSampleOutStandings = SampleOutStanding.GetsWithMkt(dStartDate, dEndDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oSampleOutStandings = SampleOutStanding.Gets(dStartDate, dEndDate, _oSampleOutStanding.ContractorName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
               
            }
            catch (Exception ex)
            {
                _oSampleOutStandings = new List<SampleOutStanding>();
            }
            var jsonResult = Json(_oSampleOutStandings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult AdvSearch_PartyMkt(SampleOutStanding oSampleOutStanding)
        {
            _oSampleOutStandings = new List<SampleOutStanding>();
            string sParams = oSampleOutStanding.ErrorMessage;
            int ncboOrderDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                ncboOrderDate = Convert.ToInt32(sParams.Split('~')[1]);
                dStartDate = Convert.ToDateTime(sParams.Split('~')[2]);
                dEndDate = Convert.ToDateTime(sParams.Split('~')[3]);
                _oSampleOutStanding.MKTPName = Convert.ToString(sParams.Split('~')[4]);
               
            }
            try
            {
                _oSampleOutStandings = SampleOutStanding.GetsWithMkt(dStartDate, dEndDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSampleOutStandings = new List<SampleOutStanding>();
            }
            var jsonResult = Json(_oSampleOutStandings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        
        public void Print_SampleOutStandingSummartXL(string sTempString)
        {
           
            int ncboOrderDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            if (!string.IsNullOrEmpty(sTempString))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sTempString.Split('~')[0]);
                ncboOrderDate = Convert.ToInt32(sTempString.Split('~')[1]);
                dStartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                dEndDate = Convert.ToDateTime(sTempString.Split('~')[3]);

                _oSampleOutStanding.MKTPName = string.Empty;
                if (sTempString.Split('~').Length > 4)
                    _oSampleOutStanding.MKTPName = sTempString.Split('~')[4];
                //_oSampleOutStanding.MKTPName = Convert.ToString(sTempString.Split('~')[4]);

                if (!string.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
                {
                    _oSampleOutStandings = SampleOutStanding.GetsWithMkt(dStartDate, dEndDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oSampleOutStandings = SampleOutStanding.Gets(dStartDate, dEndDate, _oSampleOutStanding.ContractorName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nValue = 0;
           


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 7;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SampleOutStanding");
                sheet.Name = "Sample OutStanding ";
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                
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
                cell.Value = "Sample OutStanding report"  ; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Date: " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Opening"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex,7]; cell.Value = "Closing"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

              
                nRowIndex++;
                #endregion

                #region Data
                foreach (SampleOutStanding oItem in _oSampleOutStandings)
                {
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.ContractorID.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.Opening; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Debit; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Credit; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Closing; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                 

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oSampleOutStandings.Select(c => c.Opening).Sum();
                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oSampleOutStandings.Select(c => c.Debit).Sum();
                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oSampleOutStandings.Select(c => c.Credit).Sum();
                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oSampleOutStandings.Select(c => c.Closing).Sum();
                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nValue; cell.Style.Font.Bold = true;
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
                Response.AddHeader("content-disposition", "attachment; filename=SampleOutStanding.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        private string MakeSQL_Dr(SampleOutStanding oSampleOutStanding)
        {
            string sParams = oSampleOutStanding.ErrorMessage;
            int ncboOrderDate = 0;
            DateTime dFromOrderDate = DateTime.Today;
            DateTime dToOrderDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                ncboOrderDate = Convert.ToInt32(sParams.Split('~')[1]);
                dFromOrderDate = Convert.ToDateTime(sParams.Split('~')[2]);
                dToOrderDate = Convert.ToDateTime(sParams.Split('~')[3]);

                _oSampleOutStanding.MKTPName = string.Empty;
                if (sParams.Split('~').Length > 4)
                    _oSampleOutStanding.MKTPName = sParams.Split('~')[4];
               
            }
            if (!string.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
            {
                _oSampleOutStandings = SampleOutStanding.GetsWithMkt(dFromOrderDate, dToOrderDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oSampleOutStandings = SampleOutStanding.Gets(dFromOrderDate, dToOrderDate, _oSampleOutStanding.ContractorName, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            string sReturn1 = "";
            string sReturn = "";

            sReturn1 = "SELECT * FROM View_SampleOutStanding_Debit";
           
            #region Contractor
            if (!String.IsNullOrEmpty(_oSampleOutStanding.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + _oSampleOutStanding.ContractorName + ")";
            }
            #endregion
            #region MKTPName
            if (!String.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MKTEmpID in(" + _oSampleOutStanding.MKTPName + ")";
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

       
            string sSQL = sReturn1 + " " + sReturn + " Order BY OrderDate";
            return sSQL;
        }
        private string MakeSQL_Cr(SampleOutStanding oSampleOutStanding)
        {
            string sParams = oSampleOutStanding.ErrorMessage;
            int ncboInvoiceDate = 0;
            DateTime dFromInvoiceDate = DateTime.Today;
            DateTime dToInvoiceDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                ncboInvoiceDate = Convert.ToInt32(sParams.Split('~')[1]);
                dFromInvoiceDate = Convert.ToDateTime(sParams.Split('~')[2]);
                dToInvoiceDate = Convert.ToDateTime(sParams.Split('~')[3]);
                _oSampleOutStanding.MKTPName = string.Empty;
                if (sParams.Split('~').Length > 4)
                    _oSampleOutStanding.MKTPName = sParams.Split('~')[4];
               
            }

            string sReturn1 = "";
            string sReturn = "";

           sReturn1 = "SELECT * FROM View_SampleOutStanding_Credit";
         
            #region Contractor
            if (!String.IsNullOrEmpty(_oSampleOutStanding.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + _oSampleOutStanding.ContractorName + ")";
            }
            #endregion
            #region MKTPName
            if (!String.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MKTEmpID in(" + _oSampleOutStanding.MKTPName + ")";
            }
            #endregion

            #region SampleInvoiceDate
            if (ncboInvoiceDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

               

                if (ncboInvoiceDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion


            string sSQL = sReturn1 + " " + sReturn + " Order BY SampleInvoiceDate DESC";
            return sSQL;
        }
        private string MakeSQL_SampleOrderAdj_CR(SampleOutStanding oSampleOutStanding)
        {
            string sParams = oSampleOutStanding.ErrorMessage;
            int ncboOrderDate = 0;
            DateTime dFromOrderDate = DateTime.Today;
            DateTime dToOrderDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                ncboOrderDate = Convert.ToInt32(sParams.Split('~')[1]);
                dFromOrderDate = Convert.ToDateTime(sParams.Split('~')[2]);
                dToOrderDate = Convert.ToDateTime(sParams.Split('~')[3]);


                _oSampleOutStanding.MKTPName = string.Empty;
                if (sParams.Split('~').Length > 4)
                    _oSampleOutStanding.MKTPName = sParams.Split('~')[4];
              

            }
          
            string sReturn1 = "";
            string sReturn = "";

            sReturn1 = "SELECT * FROM View_SampleOutStanding_Debit";

            #region Contractor
            if (!String.IsNullOrEmpty(_oSampleOutStanding.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + _oSampleOutStanding.ContractorName + ")";
            }
            #endregion

            #region MKTPName
            if (!String.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MKTEmpID in(" + _oSampleOutStanding.MKTPName + ")";
            }
            #endregion

            #region Issue Date
            if (ncboOrderDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboOrderDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " ExportPIID in (Select EPLM.ExportPIID from ExportPILCMapping as EPLM where EPLM.Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))) ";
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
                  
                    sReturn = sReturn + " ExportPIID in (Select EPLM.ExportPIID from ExportPILCMapping as EPLM where EPLM.Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)))";
                    _sDateRange = "Date: From " + dFromOrderDate.ToString("dd MMM yyyy") + " To " + dToOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromOrderDate.ToString("dd MMM yyyy") + " To " + dToOrderDate.ToString("dd MMM yyyy");
                }
            }
            #endregion


            string sSQL = sReturn1 + " " + sReturn + " Order BY OrderDate DESC";
            return sSQL;
        }
        private string MakeSQL_Payment_Cr(SampleOutStanding oSampleOutStanding)
        {
            string sParams = oSampleOutStanding.ErrorMessage;
            int ncboInvoiceDate = 0;
            DateTime dFromInvoiceDate = DateTime.Today;
            DateTime dToInvoiceDate = DateTime.Today;

            if (!string.IsNullOrEmpty(sParams))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                ncboInvoiceDate = Convert.ToInt32(sParams.Split('~')[1]);
                dFromInvoiceDate = Convert.ToDateTime(sParams.Split('~')[2]);
                dToInvoiceDate = Convert.ToDateTime(sParams.Split('~')[3]);

                _oSampleOutStanding.MKTPName = string.Empty;
                if (sParams.Split('~').Length > 4)
                    _oSampleOutStanding.MKTPName = sParams.Split('~')[4];
               
            }

            string sReturn1 = "";
            string sReturn = "";

            sReturn1 = "SELECT * FROM View_PaymentDetail_Report";

            #region Contractor
            if (!String.IsNullOrEmpty(_oSampleOutStanding.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + _oSampleOutStanding.ContractorName + ")";
            }
            #endregion

            #region Contractor
            if (!String.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MKTEmpID in(" + _oSampleOutStanding.MKTPName + ")";
            }
            #endregion

            #region MRDate
            if (ncboInvoiceDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);


                if (ncboInvoiceDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),MRDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion


            string sSQL = sReturn1 + " " + sReturn + " Order BY MRDate DESC";
            return sSQL;
        }
        //#region Print Production Order
        public ActionResult Print_SampleOutStanding(string sTempString)
        {
            List<SampleOutStanding> oSampleOutStandings_Dr = new List<SampleOutStanding>();
            List<SampleOutStanding> oSampleOutStandings_Cr = new List<SampleOutStanding>();
            List<SampleOutStanding> oSampleOutStandings_Cr_SampleOrder = new List<SampleOutStanding>();
            //List<Payment> oPayments = new List<Payment>();
            List<PaymentDetail> oPaymentDetails = new List<PaymentDetail>();
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding.ErrorMessage = sTempString;
            string sSQL = MakeSQL_Dr(oSampleOutStanding);
            oSampleOutStandings_Dr = SampleOutStanding.Gets(sSQL, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = MakeSQL_Cr(oSampleOutStanding);
            oSampleOutStandings_Cr = SampleOutStanding.Gets(sSQL, false, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = MakeSQL_SampleOrderAdj_CR(oSampleOutStanding);
            oSampleOutStandings_Cr_SampleOrder = SampleOutStanding.Gets(sSQL, true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = MakeSQL_Payment_Cr(oSampleOutStanding);
            oPaymentDetails = PaymentDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            rptSampleOutStanding oReport = new rptSampleOutStanding();
            byte[] abytes = oReport.PrepareReport(_oSampleOutStandings, oSampleOutStandings_Dr, oSampleOutStandings_Cr, oSampleOutStandings_Cr_SampleOrder, oPaymentDetails, oCompany, _sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult Print_SampleOutStandingSummary(string sTempString)
        {
            int ncboOrderDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            if (!string.IsNullOrEmpty(sTempString))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sTempString.Split('~')[0]);
                ncboOrderDate = Convert.ToInt32(sTempString.Split('~')[1]);
                dStartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                dEndDate = Convert.ToDateTime(sTempString.Split('~')[3]);

                _oSampleOutStanding.MKTPName = string.Empty;
                if (sTempString.Split('~').Length > 4)
                {
                    _oSampleOutStanding.MKTPName = sTempString.Split('~')[4];
                }
               

                if (!string.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
                {
                    _oSampleOutStandings = SampleOutStanding.GetsWithMkt(dStartDate, dEndDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oSampleOutStandings = SampleOutStanding.Gets(dStartDate, dEndDate, _oSampleOutStanding.ContractorName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string StartEndDate = "Date: " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            rptSampleOrderOutstanding oReport = new rptSampleOrderOutstanding();
            byte[] abytes = oReport.PrepareReport(_oSampleOutStandings, oBusinessUnit, StartEndDate);
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
        public void Print_SampleOutStandingXL(string sTempString)
        {
            List<SampleOutStanding> oSampleOutStandings_Dr = new List<SampleOutStanding>();
            List<SampleOutStanding> oSampleOutStandings_Cr = new List<SampleOutStanding>();
            List<SampleOutStanding> oSampleOutStandings_Cr_SampleOrder = new List<SampleOutStanding>();
            List<PaymentDetail> oPaymentDetails = new List<PaymentDetail>();
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding.ErrorMessage = sTempString;
            string sSQL = MakeSQL_Dr(oSampleOutStanding);
            oSampleOutStandings_Dr = SampleOutStanding.Gets(sSQL, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = MakeSQL_Cr(oSampleOutStanding);
            oSampleOutStandings_Cr = SampleOutStanding.Gets(sSQL, false, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = MakeSQL_SampleOrderAdj_CR(oSampleOutStanding);
            oSampleOutStandings_Cr_SampleOrder = SampleOutStanding.Gets(sSQL, true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = MakeSQL_Payment_Cr(oSampleOutStanding);
            oPaymentDetails = PaymentDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nQty = 0;
            double nAmount = 0;



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
                var sheet = excelPackage.Workbook.Worksheets.Add("Sample Outstanding Report");
                sheet.Name = "Sample Outstanding Report";
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
                cell.Value = "Sample Outstanding Report" + _sDateRange; cell.Style.Font.Bold = true;
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

                #region Debit Part
                if (oSampleOutStandings_Dr.Count > 0)
                {
                    nQty =0;
                    nAmount =0;
                    #region 

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "SAMPLE ORDER DETAILS(Dr Part):" ; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Sample No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value ="Qty(LBS)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "U/P($)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = _oBusinessUnit.ShortName+" Concern Persone"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Payment Mode"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Comments"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;
                    #endregion

                    #region 
                    foreach (SampleOutStanding oItem in oSampleOutStandings_Dr)
                    {


                        //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.OrderDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     
                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.Amount / oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.PaymentTypeSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.Note; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nQty = nQty + oItem.Qty;
                        nAmount = nAmount + oItem.Qty * oItem.UnitPrice;


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

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = nQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion

                #region Sample Adjustment Note Cr Part
                if (oSampleOutStandings_Cr.Count > 0)
                {
                    nQty = 0;
                    nAmount = 0;

                    #region
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "SAMPLE ADJUSTMENT NOTE (CR Part):"; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "SAN No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Qty(LBS)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "U/P($)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = _oBusinessUnit.ShortName + " Concern Persone"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Payment Mood"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Comments"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;
                    #endregion

                    #region
                    foreach (SampleOutStanding oItem in oSampleOutStandings_Cr)
                    {


                        //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.SampleInvoiceDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.SampleInvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.UnitPrice); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.InvoiceTypeSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.Note; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nQty = nQty + oItem.Qty;
                        nAmount = nAmount + oItem.Qty * oItem.UnitPrice;


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

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = nQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion

                #region Sample Order Adjustment With next LC or PI Cr Part
                if (oSampleOutStandings_Cr_SampleOrder.Count > 0)
                {
                    nQty = 0;
                    nAmount = 0;
                    #region

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "SAMPLE ORDER ADJUSTED With L/C(Cr Part):"; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Sample No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Qty(LBS)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "U/P($)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = _oBusinessUnit.ShortName + " Concern Persone"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    
                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Payment Mode"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Comments"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;
                    #endregion

                    #region
                    foreach (SampleOutStanding oItem in oSampleOutStandings_Cr_SampleOrder)
                    {


                        //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.OrderDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNoFull; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.Amount / oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.PaymentType; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.Note; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nQty = nQty + oItem.Qty;
                        nAmount = nAmount + oItem.Qty * oItem.UnitPrice;


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

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = nQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion

                #region Payment Received
                if (oPaymentDetails.Count > 0)
                {
                    nQty = 0;
                    nAmount = 0;
                    #region

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Payment Received(Cr Part):"; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "MR Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "MR No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Value($)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Value(BDT)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Comments"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;
                    #endregion

                    #region
                    foreach (PaymentDetail oItem in oPaymentDetails)
                    {
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.MRDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.MRNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = (oItem.PaymentAmount + oItem.DisCount) / oItem.CRate; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.PaymentAmount + oItem.DisCount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.SampleInvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.SampleInvoiceDateInString); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.Note; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nAmount = nAmount + ((oItem.PaymentAmount + oItem.DisCount) / oItem.CRate);


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

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                #region Balance Part Print
                nQty = _oSampleOutStandings.Select(c => c.Opening).Sum();
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Previous Due: " + nQty; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                nQty = _oSampleOutStandings.Select(c => c.Debit).Sum();
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Total Due: " + nQty; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                nQty = _oSampleOutStandings.Select(c => c.Credit).Sum();
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Total Adjustment: " + nQty; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                nQty = _oSampleOutStandings.Select(c => c.Closing).Sum();
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Balance: " + nQty; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;
                #endregion

                #endregion
            
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SampleOutStandingReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }



        #region Marketing Person Wise
        public ActionResult Print_SampleOutStandingMkt(string sTempString)
        {
            List<SampleOutStanding> oSampleOutStandings_Dr = new List<SampleOutStanding>();
            List<SampleOutStanding> oSampleOutStandings_Cr = new List<SampleOutStanding>();
            List<SampleOutStanding> oSampleOutStandings_Cr_SampleOrder = new List<SampleOutStanding>();
            //List<Payment> oPayments = new List<Payment>();
            List<PaymentDetail> oPaymentDetails = new List<PaymentDetail>();
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding.ErrorMessage = sTempString;
            string sSQL = MakeSQL_Dr(oSampleOutStanding);
            oSampleOutStandings_Dr = SampleOutStanding.Gets(sSQL, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = MakeSQL_Cr(oSampleOutStanding);
            oSampleOutStandings_Cr = SampleOutStanding.Gets(sSQL, false, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = MakeSQL_SampleOrderAdj_CR(oSampleOutStanding);
            oSampleOutStandings_Cr_SampleOrder = SampleOutStanding.Gets(sSQL, true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = MakeSQL_Payment_Cr(oSampleOutStanding);
            oPaymentDetails = PaymentDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            rptSampleOutStandingMkt oReport = new rptSampleOutStandingMkt();
            byte[] abytes = oReport.PrepareReport(_oSampleOutStandings, oSampleOutStandings_Dr, oSampleOutStandings_Cr, oSampleOutStandings_Cr_SampleOrder, oPaymentDetails, oCompany, _sDateRange);
            return File(abytes, "application/pdf");
        }
        public void Print_SampleOutStandingSummartXLMkt(string sTempString)
        {

            int ncboOrderDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            if (!string.IsNullOrEmpty(sTempString))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sTempString.Split('~')[0]);
                ncboOrderDate = Convert.ToInt32(sTempString.Split('~')[1]);
                dStartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                dEndDate = Convert.ToDateTime(sTempString.Split('~')[3]);

                _oSampleOutStanding.MKTPName = string.Empty;
                if (sTempString.Split('~').Length > 4)
                {
                    _oSampleOutStanding.MKTPName = sTempString.Split('~')[4];
                }
                //_oSampleOutStanding.MKTPName = Convert.ToString(sTempString.Split('~')[4]);

                if (!string.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
                {
                    _oSampleOutStandings = SampleOutStanding.GetsWithMkt(dStartDate, dEndDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oSampleOutStandings = SampleOutStanding.Gets(dStartDate, dEndDate, _oSampleOutStanding.ContractorName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

               // _oSampleOutStandings = SampleOutStanding.Gets(dStartDate, dEndDate, _oSampleOutStanding.ContractorName, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nValue = 0;



            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 7;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SampleOutStanding");
                sheet.Name = "Sample OutStanding ";
                sheet.Column(3).Width = 40;
                sheet.Column(4).Width = 40;
                sheet.Column(5).Width = 30;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;

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
                cell.Value = "Sample OutStanding report"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Date: " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "MKTP Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Opening"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Closing"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nRowIndex++;
                #endregion

                #region Data
                foreach (SampleOutStanding oItem in _oSampleOutStandings)
                {
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.ContractorID.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "" + oItem.MKTPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Opening; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Debit; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Credit; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.Closing; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" ; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oSampleOutStandings.Select(c => c.Opening).Sum();
                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oSampleOutStandings.Select(c => c.Debit).Sum();
                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oSampleOutStandings.Select(c => c.Credit).Sum();
                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oSampleOutStandings.Select(c => c.Closing).Sum();
                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nValue; cell.Style.Font.Bold = true;
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
                Response.AddHeader("content-disposition", "attachment; filename=SampleOutStanding.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public ActionResult Print_SampleOutStandingSummaryMkt(string sTempString)
        {
            int ncboOrderDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            if (!string.IsNullOrEmpty(sTempString))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sTempString.Split('~')[0]);
                ncboOrderDate = Convert.ToInt32(sTempString.Split('~')[1]);
                dStartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                dEndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                  _oSampleOutStanding.MKTPName = string.Empty;
                if (sTempString.Split('~').Length > 4)
                    _oSampleOutStanding.MKTPName = sTempString.Split('~')[4];
                _oSampleOutStanding.MKTPName = Convert.ToString(sTempString.Split('~')[4]);

                if (!string.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
                {
                    _oSampleOutStandings = SampleOutStanding.GetsWithMkt(dStartDate, dEndDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oSampleOutStandings = SampleOutStanding.Gets(dStartDate, dEndDate, _oSampleOutStanding.ContractorName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string StartEndDate = "Date: " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            rptSampleOrderOutstandingMkt oReport = new rptSampleOrderOutstandingMkt();
            byte[] abytes = oReport.PrepareReport(_oSampleOutStandings, oBusinessUnit, StartEndDate);
            return File(abytes, "application/pdf");
        }
        public ActionResult Print_SampleOutStandingSummaryMktDetail(string sTempString)
        {
            int ncboOrderDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            if (!string.IsNullOrEmpty(sTempString))
            {
                _oSampleOutStanding.ContractorName = Convert.ToString(sTempString.Split('~')[0]);
                ncboOrderDate = Convert.ToInt32(sTempString.Split('~')[1]);
                dStartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                dEndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                _oSampleOutStanding.MKTPName = string.Empty;
                if (sTempString.Split('~').Length > 4)
                {
                    _oSampleOutStanding.MKTPName = Convert.ToString(sTempString.Split('~')[4]);
                }

                if (!string.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
                {
                    _oSampleOutStandings = SampleOutStanding.GetsMktDetail(dStartDate, dEndDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
               
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            rptSampleOutStandingMkt oReport = new rptSampleOutStandingMkt();
            byte[] abytes = oReport.PrepareReportMktDetail(_oSampleOutStandings,  oCompany, _sDateRange);
            return File(abytes, "application/pdf");
        }

        public void Print_SampleOutStandingSummaryMktDetail_XL(string sTempString)
        {
            int ncboOrderDate = 0;
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;

            _oSampleOutStanding.ContractorName = Convert.ToString(sTempString.Split('~')[0]);
            ncboOrderDate = Convert.ToInt32(sTempString.Split('~')[1]);
            dStartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            dEndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
            _oSampleOutStanding.MKTPName = string.Empty;
            if (sTempString.Split('~').Length > 4)
            {
                _oSampleOutStanding.MKTPName = Convert.ToString(sTempString.Split('~')[4]);
            }

            if (!string.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
            {
                _oSampleOutStandings = SampleOutStanding.GetsMktDetail(dStartDate, dEndDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }


            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nTotal_Debit = 0;
            double nTotal_Credit = 0;
            double nClosing = 0;
            
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 3, nEndCol = 8;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SAMPLE ORDER vs ADJUSTMENT");
                sheet.Name = "SAMPLE ORDER vs ADJUSTMENT";

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
                cell.Value = "SAMPLE ORDER vs ADJUSTMENT" + _sDateRange; cell.Style.Font.Bold = true;
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


                var oContractors = _oSampleOutStandings.GroupBy(x => new { x.ContractorName, x.ContractorID, x.Opening }, (key, grp) =>
                                      new
                                      {
                                          ContractorName = key.ContractorName,
                                          Opening = key.Opening,
                                          ContractorID = key.ContractorID,
                                          Results = grp.ToList()
                                      }).ToList();


                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Party Wise Summary"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion


                #region Debit Part
                if (oContractors.Count > 0)
                {
                    foreach (var oItem1 in oContractors)
                    {
                        #region Buyer & Openning

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = oItem1.ContractorName; cell.Style.Font.Bold = true;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Openning"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 8]; cell.Merge = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Value = "$ " + Global.MillionFormat(oItem1.Opening); cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Header

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Ref No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Data
                        foreach (var oItem in oItem1.Results)
                        {
                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.RefNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Numberformat.Format = "#####";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.RefDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Numberformat.Format = "#####";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Numberformat.Format = "#####";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Numberformat.Format = "#####";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.Debit); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Credit); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nTotal_Credit += oItem.Credit;
                            nTotal_Debit += oItem.Debit;

                            nRowIndex++;
                        }
                        #endregion

                        #region Total
                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = nTotal_Debit; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00;($ #,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = nTotal_Credit; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;

                        nEndRow = nRowIndex;
                        #endregion

                        #region Closing
                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = "" + "Closing"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nClosing = nTotal_Debit - nTotal_Credit;
                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = nClosing; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00;($ #,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;

                        nEndRow = nRowIndex;
                        #endregion
                    }

                #endregion

                }
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SAMPLEORDERvsADJUSTMENT.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }

        //public void Print_SampleOutStandingSummaryMktDetailXL(string sTempString)
        //{
        //       int ncboOrderDate = 0;
        //    DateTime dStartDate = DateTime.Today;
        //    DateTime dEndDate = DateTime.Today;
        //    if (!string.IsNullOrEmpty(sTempString))
        //    {
        //        _oSampleOutStanding.ContractorName = Convert.ToString(sTempString.Split('~')[0]);
        //        ncboOrderDate = Convert.ToInt32(sTempString.Split('~')[1]);
        //        dStartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
        //        dEndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
        //        _oSampleOutStanding.MKTPName = string.Empty;
        //        if (sTempString.Split('~').Length > 4)
        //        {
        //            _oSampleOutStanding.MKTPName = Convert.ToString(sTempString.Split('~')[4]);
        //        }

        //        if (!string.IsNullOrEmpty(_oSampleOutStanding.MKTPName))
        //        {
        //            _oSampleOutStandings = SampleOutStanding.GetsMktDetail(dStartDate, dEndDate, _oSampleOutStanding.MKTPName, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
               
        //    }
        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    int nSL = 0;
        //    double nValue = 0;



        //    #region Export Excel
        //    int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 7;
        //    ExcelRange cell;
        //    ExcelRange HeaderCell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    using (var excelPackage = new ExcelPackage())
        //    {
        //        excelPackage.Workbook.Properties.Author = "ESimSol";
        //        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //        var sheet = excelPackage.Workbook.Worksheets.Add("SampleOutStanding");
        //        sheet.Name = "Sample OutStanding ";
        //        sheet.Column(3).Width = 40;
        //        sheet.Column(4).Width = 40;
        //        sheet.Column(5).Width = 30;
        //        sheet.Column(5).Width = 30;
        //        sheet.Column(6).Width = 20;
        //        sheet.Column(7).Width = 20;
        //        sheet.Column(8).Width = 20;
        //        sheet.Column(9).Width = 20;

        //        //   nEndCol = 10;

        //        #region Report Header
        //        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
        //        cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        nRowIndex = nRowIndex + 1;

        //        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
        //        cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
        //        cell.Style.WrapText = true;
        //        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        nRowIndex = nRowIndex + 1;

        //        #endregion

        //        #region Report Data

        //        #region Date Print

        //        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
        //        cell.Value = "Sample OutStanding report"; cell.Style.Font.Bold = true;
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        //        cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
        //        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        nRowIndex = nRowIndex + 1;
        //        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
        //        cell.Value = "Date: " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        //        cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        nRowIndex = nRowIndex + 1;
        //        #endregion

        //        #region Blank
        //        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
        //        cell.Value = ""; cell.Style.Font.Bold = true;
        //        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        //        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        nRowIndex = nRowIndex + 1;
        //        #endregion

        //        #region

        //        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Code"; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //        cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[nRowIndex, 4]; cell.Value = "MKTP Name"; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Opening"; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Closing"; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //        nRowIndex++;
        //        #endregion

        //        #region Data
        //        foreach (SampleOutStanding oItem in _oSampleOutStandings)
        //        {
        //            nSL++;
        //            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.ContractorID.ToString(); cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + oItem.ContractorName; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "" + oItem.MKTPName; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Opening; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Style.Numberformat.Format = "$ #,##0.00";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Debit; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Style.Numberformat.Format = "$ #,##0.00";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Credit; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Style.Numberformat.Format = "$ #,##0.00";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.Closing; cell.Style.Font.Bold = false;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            cell.Style.Numberformat.Format = "$ #,##0.00";
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




        //            nEndRow = nRowIndex;
        //            nRowIndex++;

        //        }
        //        #endregion

        //        #region Total
        //        cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[nRowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[nRowIndex, 4]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        nValue = _oSampleOutStandings.Select(c => c.Opening).Sum();
        //        cell = sheet.Cells[nRowIndex, 5]; cell.Value = nValue; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        cell.Style.Numberformat.Format = "$ #,##0.00";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        nValue = _oSampleOutStandings.Select(c => c.Debit).Sum();
        //        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nValue; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        cell.Style.Numberformat.Format = "$ #,##0.00";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        nValue = _oSampleOutStandings.Select(c => c.Credit).Sum();
        //        cell = sheet.Cells[nRowIndex, 7]; cell.Value = nValue; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        cell.Style.Numberformat.Format = "$ #,##0.00";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        nValue = _oSampleOutStandings.Select(c => c.Closing).Sum();
        //        cell = sheet.Cells[nRowIndex, 8]; cell.Value = nValue; cell.Style.Font.Bold = true;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        cell.Style.Numberformat.Format = "$ #,##0.00";
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



        //        nRowIndex = nRowIndex + 1;
        //        #endregion
        //        #endregion

        //        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //        fill.BackgroundColor.SetColor(Color.White);

        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=SampleOutStanding.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();
        //    }
        //    #endregion
        //}

        public void Print_SampleOutStandingXLMkt(string sTempString)
        {
            List<SampleOutStanding> oSampleOutStandings_Dr = new List<SampleOutStanding>();
            List<SampleOutStanding> oSampleOutStandings_Cr = new List<SampleOutStanding>();
            List<SampleOutStanding> oSampleOutStandings_Cr_SampleOrder = new List<SampleOutStanding>();
            List<PaymentDetail> oPaymentDetails = new List<PaymentDetail>();
            SampleOutStanding oSampleOutStanding = new SampleOutStanding();
            oSampleOutStanding.ErrorMessage = sTempString;
            string sSQL = MakeSQL_Dr(oSampleOutStanding);
            oSampleOutStandings_Dr = SampleOutStanding.Gets(sSQL, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = MakeSQL_Cr(oSampleOutStanding);
            oSampleOutStandings_Cr = SampleOutStanding.Gets(sSQL, false, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = MakeSQL_SampleOrderAdj_CR(oSampleOutStanding);
            oSampleOutStandings_Cr_SampleOrder = SampleOutStanding.Gets(sSQL, true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = MakeSQL_Payment_Cr(oSampleOutStanding);
            oPaymentDetails = PaymentDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nQty = 0;
            double nAmount = 0;



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
                var sheet = excelPackage.Workbook.Worksheets.Add("Sample Outstanding Report");
                sheet.Name = "Sample Outstanding Report";
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
                cell.Value = "Sample Outstanding Report" + _sDateRange; cell.Style.Font.Bold = true;
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

                #region Debit Part
                if (oSampleOutStandings_Dr.Count > 0)
                {
                    nQty = 0;
                    nAmount = 0;
                    #region

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "SAMPLE ORDER DETAILS(Dr Part):"; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Sample No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "MKTP Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Qty(LBS)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "U/P($)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = _oBusinessUnit.ShortName + " Party Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Payment Mode"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Comments"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;
                    #endregion

                    #region
                    foreach (SampleOutStanding oItem in oSampleOutStandings_Dr)
                    {


                        //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.OrderDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.Amount / oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.PaymentTypeSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.Note; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nQty = nQty + oItem.Qty;
                        nAmount = nAmount + oItem.Qty * oItem.UnitPrice;


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

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = nQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion

                #region Sample Adjustment Note Cr Part
                if (oSampleOutStandings_Cr.Count > 0)
                {
                    nQty = 0;
                    nAmount = 0;

                    #region
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "SAMPLE ADJUSTMENT NOTE (CR Part):"; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "SAN No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "MKTP Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Qty(LBS)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "U/P($)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = _oBusinessUnit.ShortName + " Party Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Payment Mood"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Comments"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;
                    #endregion

                    #region
                    foreach (SampleOutStanding oItem in oSampleOutStandings_Cr)
                    {


                        //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.SampleInvoiceDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.SampleInvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.UnitPrice); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.InvoiceTypeSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.Note; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nQty = nQty + oItem.Qty;
                        nAmount = nAmount + oItem.Qty * oItem.UnitPrice;


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

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = nQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion

                #region Sample Order Adjustment With next LC or PI Cr Part
                if (oSampleOutStandings_Cr_SampleOrder.Count > 0)
                {
                    nQty = 0;
                    nAmount = 0;
                    #region

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "SAMPLE ORDER ADJUSTED With L/C(Cr Part):"; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Sample No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "MKTP Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Qty(LBS)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "U/P($)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = _oBusinessUnit.ShortName + " party Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Payment Mode"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Comments"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;
                    #endregion

                    #region
                    foreach (SampleOutStanding oItem in oSampleOutStandings_Cr_SampleOrder)
                    {


                        //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.OrderDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNoFull; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.Amount / oItem.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Amount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.PaymentType; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.Note; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nQty = nQty + oItem.Qty;
                        nAmount = nAmount + oItem.Qty * oItem.UnitPrice;


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

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = nQty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion

                #region Payment Received
                if (oPaymentDetails.Count > 0)
                {
                    nQty = 0;
                    nAmount = 0;
                    #region

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Payment Received(Cr Part):"; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "MR Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "MR No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "MKTP Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Value($)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Value(BDT)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Comments"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;
                    #endregion

                    #region
                    foreach (PaymentDetail oItem in oPaymentDetails)
                    {
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.MRDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.MRNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.MKTEmpName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = (oItem.PaymentAmount + oItem.DisCount) / oItem.CRate; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.PaymentAmount + oItem.DisCount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.SampleInvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.SampleInvoiceDateInString); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.Note; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nAmount = nAmount + ((oItem.PaymentAmount + oItem.DisCount) / oItem.CRate);


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

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                #region Balance Part Print
                nQty = _oSampleOutStandings.Select(c => c.Opening).Sum();
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Previous Due: " + nQty; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                nQty = _oSampleOutStandings.Select(c => c.Debit).Sum();
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Total Due: " + nQty; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                nQty = _oSampleOutStandings.Select(c => c.Credit).Sum();
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Total Adjustment: " + nQty; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                nQty = _oSampleOutStandings.Select(c => c.Closing).Sum();
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Balance: " + nQty; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SampleOutStandingReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        #endregion

        #endregion




    }
}
