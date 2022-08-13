using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class SampleInvoiceApproveController : Controller
    {
        #region Declaration
        SampleInvoiceApprove _oSampleInvoiceApprove = new SampleInvoiceApprove();
        List<SampleInvoiceApprove> _oSampleInvoiceApproves = new List<SampleInvoiceApprove>();
        string _sErrorMessage = "";
        string sdate = "";
        DateTime _dStartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;
        #endregion
        #region Functions

        #endregion



        public ActionResult ViewSampleInvoiceApprove(int buid,int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.CurrentStatus= Enum.GetValues(typeof(EnumSampleInvoiceStatus)).Cast<EnumSampleInvoiceStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeDesignationType.MarketPerson, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeDesignationType.MarketPerson, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<EnumObject> oOrderTypeObjs = new List<EnumObject>();
            List<EnumObject> oOrderTypes = new List<EnumObject>();
            oOrderTypeObjs = EnumObject.jGets(typeof(EnumOrderType));

            foreach (EnumObject oItem in oOrderTypeObjs)
            {
                if (oItem.id == (int)EnumOrderType.SampleOrder )
                {
                    oOrderTypes.Add(oItem);
                }
            }
            ViewBag.OrderTypes = oOrderTypes;
            return View(_oSampleInvoiceApproves);
        }
        public ActionResult ViewSampleInvoiceApproves(string sTemp)
        {
            _oSampleInvoiceApproves = new List<SampleInvoiceApprove>();
            _oSampleInvoiceApprove.SampleInvoiceNo = sTemp;
            try
            {
                string sSQL = GetSQL(_oSampleInvoiceApprove);

                _oSampleInvoiceApproves = SampleInvoiceApprove.Gets(sSQL, 2, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oSampleInvoiceApproves.Count > 0)
                {
                    //_oSampleInvoiceApproves[0].StartDate = _dStartDate.ToString("dd MMM yyyy");
                    //_oSampleInvoiceApproves[0].EndDate = _dEndDate.ToString("dd MMM yyyy");
                }

            }
            catch (Exception ex)
            {
                SampleInvoiceApprove oSampleInvoiceApprove1 = new SampleInvoiceApprove();
                oSampleInvoiceApprove1.ErrorMessage = ex.Message;
            }
            // for Enum show payable
            return View(_oSampleInvoiceApproves);
        }

        public ActionResult ViewRPT_PartyOutStanding(int nConID)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();

            try
            {
                oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder as DO where  DO.DyeingOrderType=2 and SampleInvoiceID>0 and DO.ContractorID=" + nConID + " and  SampleInvoiceID in (Select SampleInvoice.SampleInvoiceID from SampleInvoice where  Isnull(PaymentSettlementStatus,0)<=3)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrders = new List<DyeingOrder>();
            }
            // for Enum show payable
            return View(oDyeingOrders);
        }

     
   

        [HttpPost]
        public JsonResult GetsByDate(SampleInvoiceApprove oSampleInvoiceApprove)
        {
            List<SampleInvoiceApprove> oSampleInvoiceApproves = new List<SampleInvoiceApprove>();
            oSampleInvoiceApproves = new List<SampleInvoiceApprove>();
           
            try
            {
                string sSQL = GetSQL(oSampleInvoiceApprove);
                oSampleInvoiceApproves = SampleInvoiceApprove.Gets(sSQL, oSampleInvoiceApprove.ReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oSampleInvoiceApproves.Count > 0)
                {
                    //_oSampleInvoiceApproves[0].StartDate = _dStartDate.ToString("dd MMM yyyy");
                    //_oSampleInvoiceApproves[0].EndDate = _dEndDate.ToString("dd MMM yyyy");
                }

            }
            catch (Exception ex)
            {
                SampleInvoiceApprove oSampleInvoiceApprove1 = new SampleInvoiceApprove();
                oSampleInvoiceApprove1.ErrorMessage = ex.Message;

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSampleInvoiceApproves);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetsPartyLedger(SampleInvoiceApprove oSampleInvoiceApprove)
        //{
        //    List<PartyLog> oPartyLogs = new List<PartyLog>();

        //    try
        //    {
        //        oPartyLogs = PartyLog.GetsbyInvoice((((int)EnumPartyTriggerType._NonLCPayment).ToString() + "," + (int)EnumPartyTriggerType._SampleInvoice).ToString() + "," + ((int)EnumPartyTriggerType._SampleInvoiceAdjustment).ToString(), oSampleInvoiceApprove.SampleInvoiceID.ToString(), oSampleInvoiceApprove.ContractorID.ToString(), (Guid)Session[SessionInfo.wcfSessionID]);
        //        double nColosingValue = 0;
        //        foreach (PartyLog oPartyLog in oPartyLogs)
        //        {
        //            nColosingValue = nColosingValue + oPartyLog.Debit_TotalValue;
        //            nColosingValue = nColosingValue - oPartyLog.Credit_TotalValue;
        //            oPartyLog.ClosingValue = nColosingValue;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        SampleInvoiceApprove oSampleInvoiceApprove1 = new SampleInvoiceApprove();
        //        oSampleInvoiceApprove1.ErrorMessage = ex.Message;


        //    }


        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oPartyLogs);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        public JsonResult Gets(SampleInvoiceApprove oSampleInvoiceApprove)
        {
            _oSampleInvoiceApproves = new List<SampleInvoiceApprove>();
            try
            {
                string sSQL = GetSQL(oSampleInvoiceApprove);
                _oSampleInvoiceApproves = SampleInvoiceApprove.Gets(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSampleInvoiceApproves = new List<SampleInvoiceApprove>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoiceApproves);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        private string GetSQL(SampleInvoiceApprove oSampleInvoiceApprove)
        {

            //string sInvoiceNo = (string.IsNullOrEmpty(oSampleInvoiceApprove.Param.Split('~')[0])) ? "" : oSampleInvoiceApprove.Param.Split('~')[0].Trim();
            //string sContractorIDs = (string.IsNullOrEmpty(oSampleInvoiceApprove.Param.Split('~')[1])) ? "" : oSampleInvoiceApprove.Param.Split('~')[1].Trim();
            //string sMktPersonIDs = (string.IsNullOrEmpty(oSampleInvoiceApprove.Param.Split('~')[2])) ? "" : oSampleInvoiceApprove.Param.Split('~')[2].Trim();
            //int OrderSearch = Convert.ToInt16(oSampleInvoiceApprove.Param.Split('~')[3]);
            //_dStartDate = Convert.ToDateTime(oSampleInvoiceApprove.Param.Split('~')[4]);
            //string SEndDate = oSampleInvoiceApprove.Param.Split('~')[5];
            //_dEndDate = DateTime.Now;
            //if (SEndDate.Length > 0)
            //{
            //    _dEndDate = Convert.ToDateTime(oSampleInvoiceApprove.Param.Split('~')[5]);
            //}
            //else
            //{
            //    _dEndDate = _dStartDate;
            //}
            //string sOrderStatus = (string.IsNullOrEmpty(oSampleInvoiceApprove.Param.Split('~')[6])) ? "" : oSampleInvoiceApprove.Param.Split('~')[6].Trim();

            string sSQL = "";
            string sReturn = "";
            #region String Making


            if (!String.IsNullOrEmpty(oSampleInvoiceApprove.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + oSampleInvoiceApprove.ContractorName + ")";
            }

            if (!String.IsNullOrEmpty(oSampleInvoiceApprove.MKTPName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MKTEmpID IN (" + oSampleInvoiceApprove.MKTPName + ")";
            }
            if (!String.IsNullOrEmpty(oSampleInvoiceApprove.SampleInvoiceNo))
            {
              
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SampleInvoiceNo IN (" + oSampleInvoiceApprove.SampleInvoiceNo + ")";
                
            }
            //if (!String.IsNullOrEmpty(sOrderStatus))
            //{
                
            //        Global.TagSQL(ref sReturn);
            //        sReturn = sReturn + " CurrentStatus IN (" + sOrderStatus + ")";
                
            //}

       
                if (oSampleInvoiceApprove.DateType > 0)
                {
                    if (oSampleInvoiceApprove.DateType != (int)EnumCompareOperator.None)
                    {

                        if (oSampleInvoiceApprove.DateType == (int)EnumCompareOperator.Between)
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " SampleInvoiceDate >= '" + oSampleInvoiceApprove.SampleInvoiceDate.ToString("dd MMM yyyy") + "' AND SampleInvoiceDate < '" + oSampleInvoiceApprove.EndDate.AddDays(1).ToString("dd MMM yyyy") + "'";

                        }

                        else if (oSampleInvoiceApprove.DateType == (int)EnumCompareOperator.EqualTo)
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " SampleInvoiceDate >= '" + oSampleInvoiceApprove.SampleInvoiceDate.ToString("dd MMM yyyy") + "' AND SampleInvoiceDate < '" + oSampleInvoiceApprove.SampleInvoiceDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                        }


                    }

                }
            
            #endregion


            sSQL = sSQL + "" + sReturn;


            return sSQL;
        }


        public void PrintSampleInvoiceApproveExcel(string Params)
        {

            _oSampleInvoiceApproves = new List<SampleInvoiceApprove>();

            _oSampleInvoiceApprove.Param = Params;

            string sSQL = GetSQL(_oSampleInvoiceApprove);

            _oSampleInvoiceApproves = SampleInvoiceApprove.Gets(sSQL, 2, ((User)Session[SessionInfo.CurrentUser]).UserID);


            int nSL = 0;

            double nTotalQty = 0;
            double nTotalValue = 0;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);


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
                var sheet = excelPackage.Workbook.Worksheets.Add("DSSReportPartyWise");
                sheet.Name = "DSS Report Party Wise";
                sheet.Column(2).Width = 35;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 20;
                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Sample Invoice Issue"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;


                #endregion

                #region DSSReport


                #region Report Data


                #region Date


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Date : " + _dStartDate.ToString("dd MMM yyyy") + " to " + _dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
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
                nSL++;
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Qty(LBS)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "UnitPrice($)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "MKT Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Party Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "InvoiceNo"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Outstanding"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Approve By"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nRowIndex++;
                #endregion

                #region Data
                foreach (SampleInvoiceApprove oItem in _oSampleInvoiceApproves)
                {


                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.SampleInvoiceDateSt; cell.Style.Font.Bold = false;
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
                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Color; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.PartyConcernPerson; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.SampleInvoiceNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.Outstanding; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.ApproveByName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nTotalQty = nTotalQty + oItem.Qty;
                    nTotalValue = nTotalValue + oItem.Amount;


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

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nTotalQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nTotalValue; cell.Style.Font.Bold = true;
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

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
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

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SampleInvoice.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
    }
}
