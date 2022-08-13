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
    public class RSFreshDyedYarnController : Controller
    {
        #region Declaration
        RSFreshDyedYarn _oRSFreshDyedYarn = new RSFreshDyedYarn();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<RSFreshDyedYarn> _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();

        RSInQCSubStatus _oRSInQCSubStatus = new RSInQCSubStatus();
        List<RSInQCSubStatus> _oRSInQCSubStatuss = new List<RSInQCSubStatus>();

        DateTime _dtQCdateFrom = DateTime.Now;
        DateTime _dtQCdateTo = DateTime.Now;
        int _ncboQCdate = 0;
        string _sDateRange = "";
        int rowIndex = 0;
        #endregion

        #region RSFreshDyedYarn
        public ActionResult View_RSFreshDyedYarn(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            //ViewBag.PaymentTypes = OrderPaymentTypeObj.Gets();

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();
            //foreach (DUOrderSetup oItem in oDUOrderSetups)
            //{
            //    if (oItem.OrderType != (int)EnumOrderType.LabDipOrder)
            //    {
            //        oDUOrderSetupsTemp.Add(oItem);
            //    }
            //}
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUDyeingTypes = DUDyeingType.Gets("SELECT * FROM DUDyeingType", ((User)Session[SessionInfo.CurrentUser]).UserID);
           // ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
 
            ViewBag.DyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID); ;

            //ViewBag.EnumRSStatuss = Enum.GetValues(typeof(EnumRSState)).Cast<EnumRSState>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.RSStatuss = EnumObject.jGets(typeof(EnumRSState));

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
          
            return View(_oRSFreshDyedYarns);
        }
        [HttpPost]
        public JsonResult AdvSearch(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            try
            {
                string sParams = oRSFreshDyedYarn.ErrorMessage;

                int ncboDateSearch = 0;
                DateTime dStartDate = DateTime.Today;
                DateTime dEndDate = DateTime.Today;
                int nOrderType = 0;
                int nReportType = 0;
                int nBUID = 0;

                if (!string.IsNullOrEmpty(sParams))
                {
                    ncboDateSearch = Convert.ToInt32(sParams.Split('~')[0]);
                    dStartDate = Convert.ToDateTime(sParams.Split('~')[1]);
                    dEndDate = Convert.ToDateTime(sParams.Split('~')[2]);
                    nOrderType = Convert.ToInt32(sParams.Split('~')[3]);
                    nReportType = Convert.ToInt32(sParams.Split('~')[4]);
                    if(ncboDateSearch==(int)EnumCompareOperator.EqualTo)
                    {
                        dEndDate = dStartDate;
                    }
                }

                oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

                dStartDate= dStartDate.AddHours(oRouteSheetSetup.BatchTime.Hour);
                dEndDate= dEndDate.AddHours(oRouteSheetSetup.BatchTime.Hour);
                dStartDate= dStartDate.AddMinutes(oRouteSheetSetup.BatchTime.Minute);
                dEndDate= dEndDate.AddMinutes(oRouteSheetSetup.BatchTime.Minute);

                string sSQL = "";
                #region ORDER TYPE
                if(nOrderType > 0)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = " OrderType = " + nOrderType;
                }
                #endregion

                #region DATE
               
                Global.TagSQL(ref sSQL);
                sSQL += "  RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where RSH.CurrentStatus=13 and EventTime>= '" + dStartDate.ToString("dd MMM yyyy HH:00") + "' and EventTime< '" + dEndDate.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
                
                #endregion
                
                _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, nReportType,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                _oRSFreshDyedYarn.ErrorMessage = ex.Message;
                _oRSFreshDyedYarns.Add(_oRSFreshDyedYarn);
            }
            var jsonResult = Json(_oRSFreshDyedYarns, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
        #region PRINT EXCEL 
        public void PrintRpt_Excel(string sTempString)
        {
            string sMunit = "";
          
            //string sParams = sTempString;
            //int ncboDateSearch = 0;
            //DateTime dStartDate = DateTime.Today;
            //DateTime dEndDate = DateTime.Today;
            //int nOrderType = 0;
            //int nReportType = 0;
            //int nBUID = 0;

            //if (!string.IsNullOrEmpty(sTempString))
            //{
            //    _ncboQCdate = Convert.ToInt32(sTempString.Split('~')[0]);
            //    _dtQCdateFrom = Convert.ToDateTime(sTempString.Split('~')[1]);
            //    _dtQCdateTo = Convert.ToDateTime(sTempString.Split('~')[2]);
            //    nOrderType = Convert.ToInt32(sTempString.Split('~')[3]);
            //    nReportType = Convert.ToInt32(sTempString.Split('~')[4]);
            //    if (_ncboQCdate == (int)EnumCompareOperator.EqualTo)
            //    {
            //        _dtQCdateTo = _dtQCdateFrom;
            //        _sDateRange = "Date " + _dtQCdateTo.ToString("dd MMM yyyy");
            //    }
            //    else
            //    {
            //        _sDateRange = "Date " + _dtQCdateFrom.ToString("dd MMM yyyy") + " to " + _dtQCdateTo.ToString("dd MMM yyyy");
            //    }
            //}

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            sMunit = oRouteSheetSetup.MUnit;
            //dStartDate = dStartDate.AddHours(oRouteSheetSetup.BatchTime.Hour);
            //dEndDate = dEndDate.AddHours(oRouteSheetSetup.BatchTime.Hour);
            //dStartDate = dStartDate.AddMinutes(oRouteSheetSetup.BatchTime.Minute);
            //dEndDate = dEndDate.AddMinutes(oRouteSheetSetup.BatchTime.Minute);

            //string sSQL = "";
            //#region ORDER TYPE
            //if (nOrderType > 0)
            //{
            //    Global.TagSQL(ref sSQL);
            //    sSQL = " OrderType = " + nOrderType;
            //}
            //#endregion

            //#region DATE

            //Global.TagSQL(ref sSQL);
            //sSQL += "  RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where RSH.CurrentStatus=13 and EventTime>= '" + dStartDate.ToString("dd MMM yyyy HH:00") + "' and EventTime< '" + dEndDate.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";

            //#endregion

            string sSQL = "";
            if (!string.IsNullOrEmpty(sTempString))
            {
                _oRSFreshDyedYarn.ErrorMessage = sTempString;
                sSQL = MakeSQL_RSPackingQC(_oRSFreshDyedYarn);
                _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, _ncboQCdate, _dtQCdateFrom, _dtQCdateTo.AddDays(1), 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                sSQL = sSQL + " where RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdate>='" + _dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "' and QCdate<'" + _dtQCdateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
                _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, _ncboQCdate, _dtQCdateFrom, _dtQCdateTo.AddDays(1), 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
          
            if (_oRSFreshDyedYarns.Count <= 0)
            {
                throw new Exception("There is no data to Export Excel!");
            }
            

            //_oRSFreshDyedYarns = RSFreshDyedYarn.Gets(dStartDate, dEndDate.AddDays(1), nOrderType, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nQty_RS = 0;
            double nFreshDyedYarnQty = 0;
            double nManagedQty = 0;
            double nQtyTemp = 0;
         
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 13;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Daily Fresh Dyed Yarn Receive");
                sheet.Name = "Daily Fresh Dyed Yarn Receive";
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 18;
                sheet.Column(6).Width = 18;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 15;
                sheet.Column(10).Width = 15;
                sheet.Column(11).Width = 15;
                sheet.Column(12).Width = 15;
                sheet.Column(13).Width = 15;
                sheet.Column(14).Width = 15;
                sheet.Column(15).Width = 15;
                sheet.Column(16).Width = 15;
                sheet.Column(17).Width = 15;
                sheet.Column(18).Width = 15;
                sheet.Column(19).Width = 20;
                sheet.Column(20).Width = 20;
                sheet.Column(21).Width = 23;
              
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
                cell.Value = "Packing Book"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                //#region Blank
                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                //cell.Value = ""; cell.Style.Font.Bold = true;
                //border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                //cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //nRowIndex = nRowIndex + 1;
                //#endregion

                var oRSShits = _oRSFreshDyedYarns.Select(x => x.RSShiftID).Distinct();

                foreach(var oRSShit in oRSShits)
                {
                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Row Header
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 21]; cell.Merge = true;
                    cell.Value = _oRSFreshDyedYarns.Where(x=>x.RSShiftID==oRSShit).Select(x=>x.RSShiftName).FirstOrDefault(); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Header

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Yarn"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Dye Lot No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex,7]; cell.Value = "Dyeing Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Adding D/C"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Re-Dyeing"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Total Packing Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Packing Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "WIP Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "B/Q"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = "RecycleQty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = "WastageQty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = "ManagedQty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Actual Short Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Total Short Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = "Gain(%)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = "Loss(%)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 21]; cell.Value = "D.Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 22]; cell.Value = "QC By Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 23]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (RSFreshDyedYarn oItem in _oRSFreshDyedYarns.Where(x=>x.RSShiftID==oRSShit))
                    {
                        //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ProductName.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.RouteSheetNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Qty_RS; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     
                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.DCAddCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = (oItem.IsReDyeing)?"Re-Dye":""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.FreshDyedYarnQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.PackingQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.WIPQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.BagCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.RecycleQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.WastageQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.ManagedQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.Loss; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 18]; cell.Value = oItem.TotalShort; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 19]; cell.Value = oItem.GainPer; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 20]; cell.Value = oItem.LossPer; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 21]; cell.Value = oItem.DyeingType; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 22]; cell.Value = oItem.RequestByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 23]; cell.Value = oItem.WUName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nQty_RS = nQty_RS + oItem.Qty_RS;
                        nFreshDyedYarnQty = nFreshDyedYarnQty + oItem.FreshDyedYarnQty;
                        nManagedQty = nManagedQty + oItem.ManagedQty;
                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    #region Sub Total
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + ""; cell.Style.Font.Bold = true;
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

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x=>x.Qty_RS); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.DCAddCount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.FreshDyedYarnQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.PackingQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                
                    
                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.WIPQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.BagCount); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.RecycleQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.WastageQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.ManagedQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                
                    cell = sheet.Cells[nRowIndex,17]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Loss); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.TotalShort); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.GainPer); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.LossPer); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 21]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 22]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 23]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion


                #region Grand Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + ""; cell.Style.Font.Bold = true;
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

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nQty_RS; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.DCAddCount).Sum();
                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

               
                cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nFreshDyedYarnQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.PackingQty).Sum();

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.WIPQty).Sum();

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.BagCount).Sum();
                cell = sheet.Cells[nRowIndex, 13]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.RecycleQty).Sum();
                cell = sheet.Cells[nRowIndex, 14]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.WastageQty).Sum();
                cell = sheet.Cells[nRowIndex, 15]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.ManagedQty).Sum();
                cell = sheet.Cells[nRowIndex, 16]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.Loss).Sum();
                cell = sheet.Cells[nRowIndex, 17]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.TotalShort).Sum();
                cell = sheet.Cells[nRowIndex, 18]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.GainPer).Sum();
                cell = sheet.Cells[nRowIndex, 19]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQtyTemp = _oRSFreshDyedYarns.Select(c => c.LossPer).Sum();
                cell = sheet.Cells[nRowIndex, 20]; cell.Value = nQtyTemp; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 21]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 22]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell = sheet.Cells[nRowIndex, 23]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Summary(Order Type)"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Header Summary(Order Type)

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Dyeing Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Packing Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "WIP Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Adding D/C Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Adding D/C%"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Recycle Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Wastage Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Short Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Gain%"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Loss%"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                nRowIndex++;
                #endregion

                #region Data
                var oRSFreshDyedYarnsOrder =  _oRSFreshDyedYarns.GroupBy(x => new { x.OrderType, x.OrderTypeSt, x.IsReDyeing }, (key, grp) =>
                                        new 
                                        {
                                            OrderType = key.OrderType,
                                            OrderTypeSt = key.OrderTypeSt,
                                            IsReDyeing = key.IsReDyeing,
                                            Qty_RS = grp.Sum(p => p.Qty_RS),
                                            QtyDCAdd = grp.Sum(p => p.QtyDCAdd),
                                            FreshDyedYarnQty = grp.Sum(p => p.FreshDyedYarnQty),
                                            RecycleQty = grp.Sum(p => p.RecycleQty),
                                            WastageQty = grp.Sum(p => p.WastageQty),
                                            WIPQty = grp.Sum(p => p.WIPQty),
                                            Gain = grp.Sum(p => p.Gain),
                                            Loss = grp.Sum(p => p.Loss),
                                            ManagedQty = grp.Sum(p => p.ManagedQty),
                                        }).ToList();
                oRSFreshDyedYarnsOrder = oRSFreshDyedYarnsOrder.OrderBy(x => x.OrderType).ToList();

                nSL = 0;
                foreach (var oItem in oRSFreshDyedYarnsOrder)
                {
                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.OrderTypeSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = ((oItem.IsReDyeing) ? "Re-Dye" : ""); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.Qty_RS; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.FreshDyedYarnQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.WIPQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.QtyDCAdd; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value =Math.Round((oItem.QtyDCAdd/oItem.Qty_RS)*100,2); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.RecycleQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.WastageQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.Loss; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = Math.Round((oItem.Gain * 100 / oItem.Qty_RS),2); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = Math.Round((oItem.Loss * 100 / oItem.Qty_RS), 2); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nQty_RS = nQty_RS + oItem.Qty_RS;
                    nFreshDyedYarnQty = nFreshDyedYarnQty + oItem.FreshDyedYarnQty;
                    nManagedQty = nManagedQty + oItem.ManagedQty;
                    nEndRow = nRowIndex;
                    nRowIndex++;
                }
                #endregion

                 #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Summary(Store wise)"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Header

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Dyeing Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Packing Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "WIP Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Recycle Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Wastage Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Short Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex,10]; cell.Value = "Gain%"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Loss%"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                nRowIndex++;
                #endregion

                #region Data
                nSL = 0;
               var oRSFreshDyedYarns = _oRSFreshDyedYarns.GroupBy(x => new { x.WorkingUnitID, x.WUName, x.IsReDyeing }, (key, grp) =>
                                        new 
                                        {
                                            WorkingUnitID = key.WorkingUnitID,
                                            WUName = key.WUName,
                                            IsReDyeing = key.IsReDyeing,
                                            Qty_RS = grp.Sum(p => p.Qty_RS),
                                            FreshDyedYarnQty = grp.Sum(p => p.FreshDyedYarnQty),
                                            RecycleQty = grp.Sum(p => p.RecycleQty),
                                            WastageQty = grp.Sum(p => p.WastageQty),
                                            WIPQty = grp.Sum(p => p.WIPQty),
                                            Gain = grp.Sum(p => p.Gain),
                                            Loss = grp.Sum(p => p.Loss),
                                            ManagedQty = grp.Sum(p => p.ManagedQty),
                                        }).ToList();


                foreach (var oItem in oRSFreshDyedYarns)
                {
                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.WUName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = ((oItem.IsReDyeing) ? "Re-Dye" : ""); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.Qty_RS; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.FreshDyedYarnQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.WIPQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.RecycleQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.WastageQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.Loss; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = Math.Round((oItem.Gain * 100 / oItem.Qty_RS),2); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = Math.Round((oItem.Loss * 100 / oItem.Qty_RS), 2); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    nQty_RS = nQty_RS + oItem.Qty_RS;
                    nFreshDyedYarnQty = nFreshDyedYarnQty + oItem.FreshDyedYarnQty;
                    nManagedQty = nManagedQty + oItem.ManagedQty;
                    nEndRow = nRowIndex;
                    nRowIndex++;
                }
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PackingBook.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region PDF
        public ActionResult PrintRpt_Preview(string sTempString)
        {
            string sMunit = "";
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            try
            {
                string sParams = sTempString;

                int ncboDateSearch = 0;
                DateTime dStartDate = DateTime.Today;
                DateTime dEndDate = DateTime.Today;
                int nOrderType = 0;
                int nReportType = 0;
                int nBUID = 0;

                //if (!string.IsNullOrEmpty(sParams))
                //{
                //    _ncboQCdate = Convert.ToInt32(sParams.Split('~')[0]);
                //    _dtQCdateFrom = Convert.ToDateTime(sParams.Split('~')[1]);
                //    _dtQCdateTo = Convert.ToDateTime(sParams.Split('~')[2]);
                //    nOrderType = Convert.ToInt32(sParams.Split('~')[3]);
                //    nReportType = Convert.ToInt32(sParams.Split('~')[4]);
                //    if (ncboDateSearch == (int)EnumCompareOperator.EqualTo)
                //    {
                //        dEndDate = dStartDate;
                //        _sDateRange = "Date " + _dtQCdateFrom.ToString("dd MMM yyyy");
                //    }
                //    else
                //    {
                //        _sDateRange = "Date " + _dtQCdateFrom.ToString("dd MMM yyyy") + " to " + _dtQCdateTo.ToString("dd MMM yyyy");
                //    }
                //}
             
                oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
                sMunit = oRouteSheetSetup.MUnit;
           
                //dStartDate = dStartDate.AddHours(oRouteSheetSetup.BatchTime.Hour);
                //dEndDate = dEndDate.AddHours(oRouteSheetSetup.BatchTime.Hour);
                //dStartDate = dStartDate.AddMinutes(oRouteSheetSetup.BatchTime.Minute);
                //dEndDate = dEndDate.AddMinutes(oRouteSheetSetup.BatchTime.Minute);


                string sSQL = "";
                if (!string.IsNullOrEmpty(sTempString))
                {
                    _oRSFreshDyedYarn.ErrorMessage = sTempString;
                    sSQL = MakeSQL_RSPackingQC(_oRSFreshDyedYarn);
                    _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, _ncboQCdate, _dtQCdateFrom, _dtQCdateTo.AddDays(1), 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdate>='" + _dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "' and QCdate<'" + _dtQCdateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
                    _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, _ncboQCdate, _dtQCdateFrom, _dtQCdateTo.AddDays(1), 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }


                if (_oRSFreshDyedYarns.Count <= 0)
                {
                    throw new Exception("There is no data to Export Excel!");
                }

               // string sSQL = "";
               // #region ORDER TYPE
               // if (nOrderType > 0)
               // {
               //     Global.TagSQL(ref sSQL);
               //     sSQL = " OrderType = " + nOrderType;
               // }
               // #endregion

               // #region DATE

               // Global.TagSQL(ref sSQL);
               //// sSQL += "  RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where RSH.CurrentStatus=13 and EventTime>= '" + _dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "' and EventTime< '" + _dtQCdateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
               // sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdate>='" + _dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "' and QCdate<'" + _dtQCdateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
               // #endregion
               // _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, _ncboQCdate, _dtQCdateFrom, _dtQCdateTo, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
              //  _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oRSFreshDyedYarns = RSFreshDyedYarn.Gets(dStartDate, dEndDate.AddDays(1), nOrderType, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                _oRSFreshDyedYarn.ErrorMessage = ex.Message;
                _oRSFreshDyedYarns.Add(_oRSFreshDyedYarn);
            } 
            
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptRSFreshDyedYarn oReport = new rptRSFreshDyedYarn();
            byte[] abytes = oReport.PrepareReport(_oRSFreshDyedYarns, oCompany, oBusinessUnit, _sDateRange, sMunit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintRpt_PreviewProductDetail(string sTempString, int ProductID)
        {
            string sMunit = "";
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            try
            {
                string sParams = sTempString;

                int ncboDateSearch = 0;
           
                int nOrderType = 0;
                int nReportType = 0;
                int nBUID = 0;

                if (!string.IsNullOrEmpty(sParams))
                {
                    ncboDateSearch = Convert.ToInt32(sParams.Split('~')[0]);
                    _dtQCdateFrom = Convert.ToDateTime(sParams.Split('~')[1]);
                    _dtQCdateTo = Convert.ToDateTime(sParams.Split('~')[2]);
                    nOrderType = Convert.ToInt32(sParams.Split('~')[3]);
                    _ncboQCdate = Convert.ToInt32(sParams.Split('~')[4]);
                    if (ncboDateSearch == (int)EnumCompareOperator.EqualTo)
                    {
                        _dtQCdateTo = _dtQCdateFrom;
                        _sDateRange = "Date " + _dtQCdateFrom.ToString("dd MMM yyyy");
                    }
                    else
                    {
                        _sDateRange = "Date " + _dtQCdateFrom.ToString("dd MMM yyyy") + " to " + _dtQCdateTo.ToString("dd MMM yyyy");
                    }
                }

                oRouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
                sMunit = oRouteSheetSetup.MUnit;

                _dtQCdateFrom = _dtQCdateFrom.AddHours(oRouteSheetSetup.BatchTime.Hour);
                _dtQCdateTo = _dtQCdateTo.AddHours(oRouteSheetSetup.BatchTime.Hour);
                _dtQCdateFrom = _dtQCdateFrom.AddMinutes(oRouteSheetSetup.BatchTime.Minute);
                _dtQCdateTo = _dtQCdateTo.AddMinutes(oRouteSheetSetup.BatchTime.Minute);

                string sSQL = "";
                //#region ORDER TYPE
                //if (nOrderType > 0)
                //{
                //    Global.TagSQL(ref sSQL);
                //    sSQL = " OrderType = " + nOrderType;
                //}
                //#endregion

                //#region DATE

                //Global.TagSQL(ref sSQL);
                //sSQL += "  RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where RSH.CurrentStatus=13 and EventTime>= '" + dStartDate.ToString("dd MMM yyyy HH:00") + "' and EventTime< '" + dEndDate.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";

                //#endregion
                //Global.TagSQL(ref sSQL);
                sSQL = sSQL + " where RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdate>='" + _dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "' and QCdate<'" + _dtQCdateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
                _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, _ncboQCdate, _dtQCdateFrom, _dtQCdateTo.AddDays(1), 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
  

                //_oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(p => p.ProductID == ProductID).ToList();
            }
            catch (Exception ex)
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                _oRSFreshDyedYarn.ErrorMessage = ex.Message;
                _oRSFreshDyedYarns.Add(_oRSFreshDyedYarn);
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptRSFreshDyedYarn oReport = new rptRSFreshDyedYarn();
            byte[] abytes = oReport.PrepareReport(_oRSFreshDyedYarns, oCompany, oBusinessUnit, _sDateRange, sMunit);
            return File(abytes, "application/pdf");
        }
        #endregion

        [HttpPost]
        public JsonResult AdvSearch_RSFDYarn(RouteSheet oRouteSheet)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                DateTime dtRouteSheetFrom_His = DateTime.Now;
                DateTime dtRouteSheetTo_His = DateTime.Now;
                string sRouteSheetNo = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[0])) ? "" : oRouteSheet.Params.Split('~')[0].Trim();
                int ncboRSDateSearch = Convert.ToInt16(oRouteSheet.Params.Split('~')[1]);
                DateTime dtRouteSheetFrom = Convert.ToDateTime(oRouteSheet.Params.Split('~')[2]);
                DateTime dtRouteSheetTo = Convert.ToDateTime(oRouteSheet.Params.Split('~')[3]);
                string sDOID = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[4])) ? "" : oRouteSheet.Params.Split('~')[4].Trim();
                string sContractorIDs = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[5])) ? "" : oRouteSheet.Params.Split('~')[5].Trim();
                string sMachineIDs = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[6])) ? "" : oRouteSheet.Params.Split('~')[6].Trim();
                string sRSState = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[7])) ? "" : oRouteSheet.Params.Split('~')[7].Trim();
                string sOrderNo = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[8])) ? "" : oRouteSheet.Params.Split('~')[8].Trim();
                string sProductIDs = (string.IsNullOrEmpty(oRouteSheet.Params.Split('~')[9])) ? "" : oRouteSheet.Params.Split('~')[9].Trim();
                int nOrderType = Convert.ToInt16(oRouteSheet.Params.Split('~')[10]);

                int ncboRSDateSearch_His = 0;
                if (oRouteSheet.Params.Split('~').Length > 11)
                    Int32.TryParse(oRouteSheet.Params.Split('~')[11], out ncboRSDateSearch_His);

                if (ncboRSDateSearch_His > 0)
                {
                    dtRouteSheetFrom_His = Convert.ToDateTime(oRouteSheet.Params.Split('~')[12]);
                    dtRouteSheetTo_His = Convert.ToDateTime(oRouteSheet.Params.Split('~')[13]);
                }

                int nRSShiftID = 0;
                int nDUDyeingType = 0;
                bool IsReDyeing = false;
                if (oRouteSheet.Params.Split('~').Length > 14)
                {
                    Int32.TryParse(oRouteSheet.Params.Split('~')[14], out nRSShiftID);
                    Int32.TryParse(oRouteSheet.Params.Split('~')[15], out nDUDyeingType);
                    Boolean.TryParse(oRouteSheet.Params.Split('~')[16], out IsReDyeing);
                }

                string sSQL = "WHERE RouteSheetID > 0";
                //if (sRouteSheetNo != "") sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE RouteSheetNo Like '%" + sRouteSheetNo + "%')";

                #region OrderDate Date
                if (ncboRSDateSearch != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sSQL);
                    if (ncboRSDateSearch == (int)EnumCompareOperator.EqualTo)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.GreaterThan)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.SmallerThan)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.Between)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetTo.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboRSDateSearch == (int)EnumCompareOperator.NotBetween)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetTo.ToString("dd MMM yyyy") + "',106)) )";
                    }

                }
                #endregion

                #region History Date
                if (ncboRSDateSearch_His != (int)EnumCompareOperator.None)
                {
                    if (string.IsNullOrEmpty(sRSState)) { sRSState = ((int)EnumRSState.InFloor).ToString(); }

                    Global.TagSQL(ref sSQL);
                    if (ncboRSDateSearch_His == (int)EnumCompareOperator.EqualTo)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                        //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.GreaterThan)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.SmallerThan)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.Between)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotBetween)
                    {
                        sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    sRSState = "";
                    
                }
                #endregion
                // if (IsdtRouteSheetSearch) sSQL = sSQL + " And RouteSheetDate Between '" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "' And '" + dtRouteSheetTo.ToString("dd MMM yyyy") + "'";

                if (sDOID != "") sSQL = sSQL          + " And RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (" + sDOID + "))";
                if (sContractorIDs != "") sSQL = sSQL + " And RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ContractorID In (" + sContractorIDs + "))";
                if (sMachineIDs != "") sSQL = sSQL    + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE MachineID In (" + sMachineIDs + ") )";
                if (sRSState != "") sSQL = sSQL       + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE RSState In (" + sRSState + ") )";
                //if (sProductIDs != "") sSQL = sSQL    + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE ProductID_Raw In (" + sProductIDs + ") )";
                if (sProductIDs != "") sSQL = sSQL + " And RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ProductID In (" + sProductIDs + "))";
                if (nOrderType > 0) sSQL = sSQL       + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE OrderType In (" + nOrderType + ") )";
                if (nRSShiftID > 0) sSQL = sSQL       + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE RSShiftID In (" + nRSShiftID + ") )";
                if (nDUDyeingType > 0) sSQL = sSQL    + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE HanksCone In (" + nDUDyeingType + ") )";
                if (IsReDyeing) sSQL = sSQL + " And  RouteSheetID in (Select RouteSheetID from RouteSheet where isnull(IsReDyeing,0)=1)";
                if (sOrderNo != "") sSQL = sSQL       + " And RouteSheetID in (select RouteSheetID from [View_RouteSheetDO] where  OrderNo like '%" + sOrderNo + "%')";
                if (sRouteSheetNo != "") sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE RouteSheetNo Like '%" + sRouteSheetNo + "%')";
                //sSQL = sSQL + " ORDER BY RouteSheetDate DESC";
                //oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheets = new List<RouteSheet>();
            }
            var jsonResult = Json(_oRSFreshDyedYarns, JsonRequestBehavior.AllowGet);
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

        #endregion

        #region RSQC
        public ActionResult View_RSQC(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OrderTypes = DUOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRSFreshDyedYarn = new RSFreshDyedYarn();

            _oRSFreshDyedYarns = RSFreshDyedYarn.Gets("where  routesheetdate>''"+DateTime.Today.AddMonths(-3).ToString("dd MMM yyyy")+"'' and RSState in (" + (int)EnumRSState.UnLoadedFromDrier + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            //string sSQL = "Select * from View_WorkingUnit Where IsActive=1 And IsStore=1 And BUID=" + (short)EnumBusinessUnitType.Dyeing;
            //oWorkingUnits = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RouteSheetCancel, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = oWorkingUnits;

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            RSShift oRSShift = new RSShift();
            ViewBag.RSShifts = RSShift.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RSStatuss = EnumObject.jGets(typeof(EnumRSState)).Where(p => p.id > (int)EnumRSState.LoadedInDrier).ToList();
           // ViewBag.RSStatuss = Enum.GetValues(typeof(EnumRSState)).Cast<EnumRSState>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            //ViewBag.RSSubStatuss = Enum.GetValues(typeof(EnumRSSubStates)).Cast<EnumRSSubStates>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.RSSubStatuss = EnumObject.jGets(typeof(EnumRSSubStates));
            ViewBag.UserName = (string)Session[SessionInfo.currentUserName];
            return View(_oRSFreshDyedYarns);
        }
        
        [HttpPost]
        public JsonResult AdvSearchRSQC(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            try
            {
                string sSQL = MakeSQL(oRSFreshDyedYarn,false);
                _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                _oRSFreshDyedYarn.ErrorMessage = ex.Message;
                _oRSFreshDyedYarns.Add(_oRSFreshDyedYarn);
            }
            var jsonResult = Json(_oRSFreshDyedYarns, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
     
        private string MakeSQL(RSFreshDyedYarn oRSFreshDyedYarn, bool isLoadUnload)
        {
            string sParams = oRSFreshDyedYarn.ErrorMessage;

            string sRouteSheetNo = "";
            int ncboRSDateSearch = 0;
            DateTime dtRouteSheetFrom = DateTime.Now;
            DateTime dtRouteSheetTo = DateTime.Now;
            string sDOID = "";
            string sContractorIDs = "";
            string sMachineIDs = "";
            string sRSState = "";
            string sOrderNo = "";
            string sProductIDs = "";
            int nOrderType = 0;
            int ncboRSDateSearch_His = 0;
            DateTime dtRouteSheetFrom_His = DateTime.Now;
            DateTime dtRouteSheetTo_His = DateTime.Now;
            int ncboQCState = 0;
            DateTime dtQCStateFrom= DateTime.Now;
            DateTime dtQCStateTo = DateTime.Now;
            int nQCState = 0;

            string sSQL = "";
            string sReturn = "";
            if (!string.IsNullOrEmpty(sParams))
            {
                ncboRSDateSearch = Convert.ToInt16(sParams.Split('~')[0]);
                if (ncboRSDateSearch > 0)
                {
                    dtRouteSheetFrom = Convert.ToDateTime(sParams.Split('~')[1]);
                    dtRouteSheetTo = Convert.ToDateTime(sParams.Split('~')[2]);
                }
                sMachineIDs = (string.IsNullOrEmpty(sParams.Split('~')[3])) ? "" : sParams.Split('~')[3].Trim();
                sContractorIDs = (string.IsNullOrEmpty(sParams.Split('~')[4])) ? "" : sParams.Split('~')[4].Trim();
                sProductIDs = (string.IsNullOrEmpty(sParams.Split('~')[5])) ? "" : sParams.Split('~')[5].Trim();
                nOrderType = Convert.ToInt16(sParams.Split('~')[6]);
                sDOID = (string.IsNullOrEmpty(sParams.Split('~')[7])) ? "" : sParams.Split('~')[7].Trim();
                sRSState = (string.IsNullOrEmpty(sParams.Split('~')[8])) ? "" : sParams.Split('~')[8].Trim();
                sRouteSheetNo = (string.IsNullOrEmpty(sParams.Split('~')[9])) ? "" : sParams.Split('~')[9].Trim();
                sOrderNo = (string.IsNullOrEmpty(sParams.Split('~')[10])) ? "" : sParams.Split('~')[10].Trim();

                if (sParams.Split('~').Length > 11)
                    Int32.TryParse(sParams.Split('~')[11], out ncboRSDateSearch_His);
                
                if (ncboRSDateSearch_His > 0)
                {
                    dtRouteSheetFrom_His = Convert.ToDateTime(sParams.Split('~')[12]);
                    dtRouteSheetTo_His = Convert.ToDateTime(sParams.Split('~')[13]);
                }
                if (sParams.Split('~').Length > 14)
                    Int32.TryParse(sParams.Split('~')[14], out nQCState);

                if (sParams.Split('~').Length > 15)
                    Int32.TryParse(sParams.Split('~')[15], out ncboQCState);
                
                if (ncboQCState > 0)
                {
                    dtQCStateFrom = Convert.ToDateTime(sParams.Split('~')[16]);
                    dtQCStateTo = Convert.ToDateTime(sParams.Split('~')[17]);
                }
            }
            else
            { sSQL = "where RSState in (" + (int)EnumRSState.UnLoadedFromDrier + "," + (int)EnumRSState.InDelivery + "," + (int)EnumRSState.InHW_Sub_Store + "," + (int)EnumRSState.InSubFinishingstore_Partially + ")"; }

            if (!string.IsNullOrEmpty(sRouteSheetNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RouteSheetNo Like ''%" + sRouteSheetNo + "%''";
            }
            #region RS Date
            if (ncboRSDateSearch != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboRSDateSearch == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),''" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "'',106)) ";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),''" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "'',106)) ";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),''" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "'',106)) ";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),''" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "'',106)) ";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dtRouteSheetTo.ToString("dd MMM yyyy") + "'',106)) ";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dtRouteSheetTo.ToString("dd MMM yyyy") + "'',106)) ";
                }
            }
            #endregion
            #region History Date
            if (ncboRSDateSearch_His != (int)EnumCompareOperator.None)
            {
                if (string.IsNullOrEmpty(sRSState)) { sRSState = ((int)EnumRSState.InHW_Sub_Store).ToString(); }

                Global.TagSQL(ref sReturn);
                if (ncboRSDateSearch_His == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>=''" + dtRouteSheetFrom_His.ToString("dd MMM yyyy HH:00") + "'' and EventTime<''" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>=''" + dtRouteSheetFrom_His.ToString("dd MMM yyyy HH:00") + "'' and EventTime<''" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>=''" + dtRouteSheetFrom_His.ToString("dd MMM yyyy HH:00") + "'' and EventTime<''" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>=''" + dtRouteSheetFrom_His.ToString("dd MMM yyyy HH:00") + "'' and EventTime<''" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>=''" + dtRouteSheetFrom_His.ToString("dd MMM yyyy HH:00") + "'' and EventTime<''" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>=''" + dtRouteSheetFrom_His.ToString("dd MMM yyyy HH:00") + "'' and EventTime<''" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                sRSState = "";

            }
            #endregion
            #region Shade QC Status  Date
            if (ncboQCState != (int)EnumCompareOperator.None)
            {
              

                Global.TagSQL(ref sReturn);
                if (ncboQCState == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where "+ (nQCState > 0 ?" RSSubStatus In (" + nQCState + ") and ":"")+" LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtQCStateFrom.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where "+ (nQCState > 0 ?" RSSubStatus In (" + nQCState + ") and ":"")+" LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtQCStateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where "+ (nQCState > 0 ?" RSSubStatus In (" + nQCState + ") and ":"")+"  LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtQCStateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where "+ (nQCState > 0 ?" RSSubStatus In (" + nQCState + ") and ":"")+"  LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where "+ (nQCState > 0 ?" RSSubStatus In (" + nQCState + ") and ":"")+"  LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where "+ (nQCState > 0 ?" RSSubStatus In (" + nQCState + ") and ":"")+"  LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                nQCState = 0;

            }
            #endregion
            if (sDOID != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (" + sDOID + "))"; }
            if (sContractorIDs != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ContractorID In (" + sContractorIDs + "))"; }
            if (sMachineIDs != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " MachineID In (" + sMachineIDs + ")"; }
            if (sRSState != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RSState In (" + sRSState + ")"; }
            if (sProductIDs != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ProductID In (" + sProductIDs + "))"; }
            if (nOrderType > 0) { Global.TagSQL(ref sReturn); sReturn = sReturn + " OrderType In (" + nOrderType + ")"; }
            //SELECT RouteSheetID, RSSubStatus FROM RSInQCSubStatus
            //if (nQCState > 0) { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID IN (SELECT RouteSheetID FROM RSInQCSubStatus WHERE  RSSubStatus IN (" + nQCState + "))"; }
            if (nQCState > 0) { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID IN ( Select HH.RouteSheetID from ( SELECT *,ROW_NUMBER() OVER(Partition by RouteSheetID ORDER BY RouteSheetID Asc,DBserverdatetime Desc) as ID FROM RSInQCSubStatus  ) as HH where ID=1 and RSSubStatus in ("+ nQCState +"))"; }
            
            ///if (nQCState > 0) { Global.TagSQL(ref sReturn); sReturn = sReturn + " OrderType In (" + nQCState + ")"; }
            if (sOrderNo != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID in (select RouteSheetID from [View_RouteSheetDO] where  OrderNo like ''%" + sOrderNo + "%'')"; }
            if (!isLoadUnload)
            {
                sReturn = sReturn + " and RSState>=" + (int)EnumRSState.UnLoadedFromDrier;
            }

            sSQL = sSQL + sReturn;
                   return sSQL;
        }

        #region PRINT TO EXCEL
        public void ExportToExcel(string sParams)
        {
            string _sErrorMesage="";
            RSFreshDyedYarn oRSFreshDyedYarn = new RSFreshDyedYarn();
            try
            {
                string sSQL="";
                if(!string.IsNullOrEmpty(sParams))
                {
                    oRSFreshDyedYarn.ErrorMessage = sParams;
                   
                    sSQL = MakeSQL(oRSFreshDyedYarn, false);
                    _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                  
                }else 
                {
                    sSQL="where RSState in (" + (int)EnumRSState.UnLoadedFromDrier + "," + (int)EnumRSState.InPackageing + "," + (int)EnumRSState.InSubFinishingstore_Partially + ")";
                }
                _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oRSFreshDyedYarns.Count <= 0)
                {
                    throw new Exception("There is no data to Export Excel!");
                }
            }
            catch (Exception ex)
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);


                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nStartCol = 2, nEndCol = 12, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("RSFreshDyedYarn");
                    sheet.Name = "Batch QC Report";

                    sheet.Column(++nColumn).Width = 10; //SL
                    //sheet.Column(++nColumn).Width = 10; //TextileUnitST
                    sheet.Column(++nColumn).Width = 15; //Batch No
                    sheet.Column(++nColumn).Width = 15; //OrderNo
                    sheet.Column(++nColumn).Width = 30; //Customer
                    sheet.Column(++nColumn).Width = 30; //Yarn Count
                    sheet.Column(++nColumn).Width = 20; //Color/Shade
                    sheet.Column(++nColumn).Width = 15; //Qty
                    sheet.Column(++nColumn).Width = 10; //Shade Status
                    sheet.Column(++nColumn).Width = 15; //Status
                    sheet.Column(++nColumn).Width = 20; //Unload Time
                    sheet.Column(++nColumn).Width = 20; //QC Date
                    sheet.Column(++nColumn).Width = 10; //Delay Days
                    sheet.Column(++nColumn).Width = 15; //Yarn Lot
                    sheet.Column(++nColumn).Width = 15; //M/C No
                    sheet.Column(++nColumn).Width = 25; //Unload By
                    sheet.Column(++nColumn).Width = 15; //Shift
                    sheet.Column(++nColumn).Width = 15; //Pcs
                    sheet.Column(++nColumn).Width = 20; //Remarks
                    nEndCol = nColumn;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 6]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol - 5, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Batch QC Report"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 6]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
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

                    string[] sHeader = new string[] {
                    "SLNo",
                    "Batch No."
                    ,"Order No"
                    ,"Customer"
                    ,"Yarn Count"
                    ,"Color/Shade"
                    ,"Qty"
                    ,"Shade Status"
                    ,"Status"
                    ,"Unload Time"
                    ,"QC Date"
                    ,"Delay Days"
                    ,"Yarn Lot"
                    ,"M/C No"
                    ,"Unload By"
                    ,"Shift"
                    ,"Pcs"
                    ,"Remarks"
                    };

                    foreach (string Header in sHeader)
                    {
                        this.AddExcelHeader(ref cell, sheet, Header, nRowIndex, ++nColumn);
                    }

                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (RSFreshDyedYarn oItem in _oRSFreshDyedYarns)
                    {
                        nCount++;
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.RouteSheetNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        // "OrderNo" "ContractorName" "ProductName" "RawLotNo"  "MachineName" 
                        //"RouteSheetNo" "ColorName" "BagCount"  "Qty_RS" "QCDateStr"  "RSShiftName" "RSStateStr" 

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_RS; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = " ##,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.RSSubStatesSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.RSStateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnloadTimeStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.QCDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;                        
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.TimeLag; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.RawLotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MachineName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnloadByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.RSShiftName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BagCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = " ##,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.RSSubNote; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 7]; cell.Value = "" + "Total  "; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = _oRSFreshDyedYarns.Select(c => c.Qty_RS).Sum();
                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = " ##,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 19]; cell.Value = " "; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=BatchQCReport.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void AddExcelHeader(ref ExcelRange cell, ExcelWorksheet sheet, string sHeader, int nRowIndex, int nColumn)
        {
            OfficeOpenXml.Style.Border border;
            cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = sHeader; cell.Style.Font.Bold = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }
        #endregion
        
        #endregion

        #region RS Sub Status
        public ActionResult ViewRSInQCSubStatuss(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oRSInQCSubStatuss = new List<RSInQCSubStatus>();
            _oRSInQCSubStatuss = RSInQCSubStatus.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oRSInQCSubStatuss);
        }
        public ActionResult ViewRSInQCSubStatus(int id)
        {
            _oRSInQCSubStatus = new RSInQCSubStatus();
            if (id > 0)
            {
                _oRSInQCSubStatus = _oRSInQCSubStatus.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oRSInQCSubStatus);
        }
        [HttpPost]
        public JsonResult GetRSSubState(RSInQCSubStatus oRSInQCSubStatus)
        {
            _oRSInQCSubStatus = new RSInQCSubStatus();
            try
            {
                _oRSInQCSubStatus = oRSInQCSubStatus;
                _oRSInQCSubStatus = _oRSInQCSubStatus.Get(oRSInQCSubStatus.RouteSheetID,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRSInQCSubStatus = new RSInQCSubStatus();
                _oRSInQCSubStatus.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRSInQCSubStatus);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Gets_RSSubState(RSInQCSubStatus oRSInQCSubStatus)
        {
            _oRSInQCSubStatus = new RSInQCSubStatus();
            _oRSInQCSubStatuss = new List<RSInQCSubStatus>();

            try
            {
                _oRSInQCSubStatuss = RSInQCSubStatus.Gets("SELECT * FROM View_RSInQCSubStatus WHERE RouteSheetID IN (" + oRSInQCSubStatus.RouteSheetID + ") ORDER BY LastUpdateDateTime", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRSInQCSubStatus = new RSInQCSubStatus();
                _oRSInQCSubStatus.ErrorMessage = ex.Message;
                _oRSInQCSubStatuss.Add(_oRSInQCSubStatus);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRSInQCSubStatuss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_RSSubState(RSInQCSubStatus oRSInQCSubStatus)
        {
            _oRSInQCSubStatus = new RSInQCSubStatus();
            try
            {
                _oRSInQCSubStatus = oRSInQCSubStatus;
                _oRSInQCSubStatus = _oRSInQCSubStatus.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRSInQCSubStatus = new RSInQCSubStatus();
                _oRSInQCSubStatus.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRSInQCSubStatus);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete_RSSubState(RSInQCSubStatus oRSInQCSubStatus)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oRSInQCSubStatus.Delete((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult SendToRequsitionToDelivery(RouteSheet oRouteSheet)
        {
            string sReturn = "";
            DUDeliveryStock oDUDeliveryStock = new DUDeliveryStock();
             List<Lot> oLots = new List<Lot>();
             oLots = Lot.Gets("SELECT * FROM View_Lot WHERE ParentType=106 and ParentID=" + oRouteSheet.RouteSheetID + " and WorkingUnitID=10  ORDER BY LotID ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            try
            {
                if (oLots.Count <=0)
                {
                    sReturn = "First Hard winding Lot Not found";
                }
                else
                {
                    oDUDeliveryStock.LotID = oLots[0].LotID;
                    oDUDeliveryStock.WorkingUnitID = oLots[0].WorkingUnitID;
                    sReturn = oDUDeliveryStock.SendToRequsitionToDelivery(oDUDeliveryStock, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDUDeliveryStock = new DUDeliveryStock();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sReturn);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SendToRequsitionToDeliveryHW(DUHardWinding oDUHardWinding)
        {
            string sReturn = "";
            DUDeliveryStock oDUDeliveryStock = new DUDeliveryStock();
            List<Lot> oLots = new List<Lot>();
            oLots = Lot.Gets("SELECT * FROM View_Lot WHERE ParentType=106 and ParentID=" + oDUHardWinding.RouteSheetID + " and WorkingUnitID=10  ORDER BY LotID ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            try
            {
                if (oLots.Count <= 0)
                {
                    sReturn = "First Hard winding Lot Not found";
                }
                else
                {
                    oDUDeliveryStock.LotID = oLots[0].LotID;
                    oDUDeliveryStock.WorkingUnitID = oLots[0].WorkingUnitID;
                    sReturn = oDUDeliveryStock.SendToRequsitionToDelivery(oDUDeliveryStock, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDUDeliveryStock = new DUDeliveryStock();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sReturn);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SendToSubStore(DUHardWinding oDUHardWinding)
        {
           
           try
            {
                    //oRSFreshDyedYarn.RouteSheetID = oRouteSheet.RouteSheetID;
                    //oRSFreshDyedYarn.Qty_RS = oRouteSheet.Qty;
                    //oRSFreshDyedYarn.RSState = EnumRSState.InHW_Sub_Store;

                    //DUHardWinding oDUHardWinding = new DUHardWinding();

                oDUHardWinding.RSState = EnumRSState.InHW_Sub_Store;
                    oDUHardWinding = oDUHardWinding.SendToHWStore(oDUHardWinding, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
            }
            catch (Exception ex)
            {
                oDUHardWinding = new DUHardWinding();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUHardWinding);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RSQCDOneByForce(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            oRouteSheet.RouteSheetID = oRSFreshDyedYarn.RouteSheetID;
            oRouteSheet.Note = oRSFreshDyedYarn.Note;
            oRouteSheet = oRouteSheet.RSQCDOneByForce(((User)Session[SessionInfo.CurrentUser]).UserID);
           
            oRSFreshDyedYarn.ErrorMessage = oRouteSheet.ErrorMessage;
            //_oDyeingOrder.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRSFreshDyedYarn);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsITransactionBYRS(RouteSheet oRouteSheet)
        {
            List<ITransaction> oITransactions = new List<ITransaction>();
            List<DUHardWinding> oDUHardWindings = new List<DUHardWinding>();
            try
            {
                oDUHardWindings = DUHardWinding.Gets("Select * from View_DUHardWinding Where RouteSheetID=" + oRouteSheet.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              //  oITransactions = ITransaction.Gets("Select * from View_ITransaction where TriggerParentID=" + oRouteSheet.RouteSheetID + " and TriggerParentType=" + (int)EnumTriggerParentsType.RouteSheet + " and WorkingUnitID in (Select WorkingUnitID from RSInQCSetup where activity=1 and YarnType=" + (int)EnumYarnType.FreshDyedYarn + ") and WorkingUnitID in (Select WorkingUnitID from WorkingUnit where LocationID=" + oRouteSheet.LocationID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUHardWindings = new List<DUHardWinding>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUHardWindings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region RSDyeingLoadUnLoad
        public ActionResult View_RSDyeingLoadUnLoad(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RouteSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();
           
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUDyeingTypes = DUDyeingType.Gets("SELECT * FROM DUDyeingType", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.DyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID); ;
          
            ViewBag.RSStatuss = EnumObject.jGets(typeof(EnumRSState)); 
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OrderTypes = oDUOrderSetups;


            //_oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" WHERE  RSState In (" + (int)EnumRSState.YarnOut + ") and RouteSheetDate>''" + DateTime.Today.AddYears(-1).ToString("dd MMM yyyy"), ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" WHERE RSState In (" + (int)EnumRSState.YarnOut + ") and  RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + (int)EnumRSState.YarnOut + ") and EventTime>=''" + DateTime.Now.AddDays(-20).ToString("dd MMM yyyy") + "'' and EventTime<''" + DateTime.Now.ToString("dd MMM yyyy") + "'')", ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(_oRSFreshDyedYarns);
        }
        [HttpPost]
        public JsonResult AdvSearch_RSLoadUnLoad(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            try
            {
                string sSQL = MakeSQL(oRSFreshDyedYarn, true);
                _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                _oRSFreshDyedYarn.ErrorMessage = ex.Message;
                _oRSFreshDyedYarns.Add(_oRSFreshDyedYarn);
            }
            var jsonResult = Json(_oRSFreshDyedYarns, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region View_RSPackingQC
        public ActionResult View_RSPackingQC(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RouteSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUDyeingTypes = DUDyeingType.Gets("SELECT * FROM DUDyeingType", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.DyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID); ;

            ViewBag.RSStatuss = EnumObject.jGets(typeof(EnumRSState));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.WarpWeftType = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.RSBagType = EnumObject.jGets(typeof(EnumRSBagType));
            ViewBag.RSInQCSetup = RSInQCSetup.Gets("Select * from View_RSInQCSetup Where Activity=1 order by YarnType ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(" WHERE RSState In (" + (int)EnumRSState.UnLoadedFromDrier + ") and  RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + (int)EnumRSState.UnLoadedFromDrier + ") and EventTime>='" + DateTime.Now.AddDays(-20).ToString("dd MMM yyyy") + "' and EventTime<'" + DateTime.Now.ToString("dd MMM yyyy") + "')",0,DateTime.Now,DateTime.Now, 1,((User)Session[SessionInfo.CurrentUser]).UserID);
          
            return View(_oRSFreshDyedYarns);
        }
        [HttpPost]
        public JsonResult AdvSearch_RSPackingQC(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            if (oRSFreshDyedYarn.ReportType <= 0) { oRSFreshDyedYarn.ReportType = 1; }
          
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            try
            {
                string sSQL = MakeSQL_RSPackingQC(oRSFreshDyedYarn);
                _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, _ncboQCdate, _dtQCdateFrom, _dtQCdateTo.AddDays(1), oRSFreshDyedYarn.ReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                _oRSFreshDyedYarn.ErrorMessage = ex.Message;
                _oRSFreshDyedYarns.Add(_oRSFreshDyedYarn);
            }
            var jsonResult = Json(_oRSFreshDyedYarns, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //NAZMUL
        [HttpGet]
        public ActionResult ProductListPreview(string sTempString, int nRpoertType)
        {
            _oRSFreshDyedYarn = new RSFreshDyedYarn();
            _oRSFreshDyedYarn.ErrorMessage = sTempString;
            _oRSFreshDyedYarn.ReportType = nRpoertType;
            DateTime StartTime = Convert.ToDateTime(_oRSFreshDyedYarn.ErrorMessage.Split('~')[15]);
            DateTime EndTime = Convert.ToDateTime(_oRSFreshDyedYarn.ErrorMessage.Split('~')[16]);
           
            try
            {
                string sSQL = MakeSQL_RSPackingQC(_oRSFreshDyedYarn);
                _oRSFreshDyedYarns = RSFreshDyedYarn.Gets(sSQL, _ncboQCdate, _dtQCdateFrom, _dtQCdateTo.AddDays(1), _oRSFreshDyedYarn.ReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                _oRSFreshDyedYarn.ErrorMessage = ex.Message;
                _oRSFreshDyedYarns.Add(_oRSFreshDyedYarn);
            }
            string sDateRange = StartTime.ToString("dd MMM yyyy") + " To " + EndTime.ToString("dd MMM yyyy");
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptFreshYarnProductView oReport = new rptFreshYarnProductView();
            byte[] abytes = oReport.PrepareReport(_oRSFreshDyedYarns, oCompany,sDateRange,"");
            return File(abytes, "application/pdf");


        }        
      
        private string MakeSQL_RSPackingQC(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            string sParams = oRSFreshDyedYarn.ErrorMessage;
            string sPIIDs = "";

            DateTime dtQCdateFrom = DateTime.Now;
            DateTime dtQCdateTo = DateTime.Now;

            DateTime dtRouteSheetFrom_His = DateTime.Now;
            DateTime dtRouteSheetTo_His = DateTime.Now;

            DateTime dtRouteSheetFrom = DateTime.Now;
            DateTime dtRouteSheetTo = DateTime.Now;
            
            int ncboRSDateSearch = Convert.ToInt16(oRSFreshDyedYarn.ErrorMessage.Split('~')[0]);
            if (ncboRSDateSearch > 0)
            {
                 dtRouteSheetFrom = Convert.ToDateTime(oRSFreshDyedYarn.ErrorMessage.Split('~')[1]);
                 dtRouteSheetTo = Convert.ToDateTime(oRSFreshDyedYarn.ErrorMessage.Split('~')[2]);
            }
            string sMachineIDs = (string.IsNullOrEmpty(oRSFreshDyedYarn.ErrorMessage.Split('~')[3])) ? "" : oRSFreshDyedYarn.ErrorMessage.Split('~')[3].Trim();
            string sContractorIDs = (string.IsNullOrEmpty(oRSFreshDyedYarn.ErrorMessage.Split('~')[4])) ? "" : oRSFreshDyedYarn.ErrorMessage.Split('~')[4].Trim();
            string sProductIDs = (string.IsNullOrEmpty(oRSFreshDyedYarn.ErrorMessage.Split('~')[5])) ? "" : oRSFreshDyedYarn.ErrorMessage.Split('~')[5].Trim();
            int nOrderType = Convert.ToInt16(oRSFreshDyedYarn.ErrorMessage.Split('~')[6]);
            string sDOID = (string.IsNullOrEmpty(oRSFreshDyedYarn.ErrorMessage.Split('~')[7])) ? "" : oRSFreshDyedYarn.ErrorMessage.Split('~')[7].Trim();
            string sRSState = (string.IsNullOrEmpty(oRSFreshDyedYarn.ErrorMessage.Split('~')[8])) ? "" : oRSFreshDyedYarn.ErrorMessage.Split('~')[8].Trim();
            string sRouteSheetNo = (string.IsNullOrEmpty(oRSFreshDyedYarn.ErrorMessage.Split('~')[9])) ? "" : oRSFreshDyedYarn.ErrorMessage.Split('~')[9].Trim();
            string sOrderNo = (string.IsNullOrEmpty(oRSFreshDyedYarn.ErrorMessage.Split('~')[10])) ? "" : oRSFreshDyedYarn.ErrorMessage.Split('~')[10].Trim();
         
            int ncboRSDateSearch_His = 0;
            if (oRSFreshDyedYarn.ErrorMessage.Split('~').Length > 11)
                Int32.TryParse(oRSFreshDyedYarn.ErrorMessage.Split('~')[11], out ncboRSDateSearch_His);

            if (ncboRSDateSearch_His > 0)
            {
                dtRouteSheetFrom_His = Convert.ToDateTime(oRSFreshDyedYarn.ErrorMessage.Split('~')[12]);
                dtRouteSheetTo_His = Convert.ToDateTime(oRSFreshDyedYarn.ErrorMessage.Split('~')[13]);
            }
            int ncboQCdate = 0;
            if (oRSFreshDyedYarn.ErrorMessage.Split('~').Length > 14)
                Int32.TryParse(oRSFreshDyedYarn.ErrorMessage.Split('~')[14], out ncboQCdate);
            if (ncboQCdate > 0)
            {
                dtQCdateFrom = Convert.ToDateTime(oRSFreshDyedYarn.ErrorMessage.Split('~')[15]);
                dtQCdateTo = Convert.ToDateTime(oRSFreshDyedYarn.ErrorMessage.Split('~')[16]);
            }

            int nQCStatus = 0;
            if (oRSFreshDyedYarn.ErrorMessage.Split('~').Length > 17)
                Int32.TryParse(oRSFreshDyedYarn.ErrorMessage.Split('~')[17], out nQCStatus);
            int nDUDyeingType = 0;
            if (oRSFreshDyedYarn.ErrorMessage.Split('~').Length > 18)
                Int32.TryParse(oRSFreshDyedYarn.ErrorMessage.Split('~')[18], out nDUDyeingType);

            int nRSShiftID = 0;
            if (oRSFreshDyedYarn.ErrorMessage.Split('~').Length > 19)
                Int32.TryParse(oRSFreshDyedYarn.ErrorMessage.Split('~')[19], out nRSShiftID);

            bool bIsReDyeing = false;
            if (oRSFreshDyedYarn.ErrorMessage.Split('~').Length > 20)
                Boolean.TryParse(oRSFreshDyedYarn.ErrorMessage.Split('~')[20], out bIsReDyeing);

            if (oRSFreshDyedYarn.ErrorMessage.Split('~').Length > 21)
                sPIIDs = oRSFreshDyedYarn.ErrorMessage.Split('~')[21];

            string sSQL = "WHERE RouteSheetID > 0";
            //if (sRouteSheetNo != "") sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE RouteSheetNo Like '%" + sRouteSheetNo + "%')";

            #region OrderDate Date
            if (ncboRSDateSearch != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sSQL);
                if (ncboRSDateSearch == (int)EnumCompareOperator.EqualTo)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.NotEqualTo)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.GreaterThan)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.SmallerThan)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.Between)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetTo.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (ncboRSDateSearch == (int)EnumCompareOperator.NotBetween)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheet WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetTo.ToString("dd MMM yyyy") + "',106)) )";
                }

            }
            #endregion

            #region History Date
            if (ncboRSDateSearch_His != (int)EnumCompareOperator.None)
            {
                if (string.IsNullOrEmpty(sRSState)) { sRSState = ((int)EnumRSState.InFloor).ToString(); }

                Global.TagSQL(ref sSQL);
                if (ncboRSDateSearch_His == (int)EnumCompareOperator.EqualTo)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotEqualTo)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.GreaterThan)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.SmallerThan)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.Between)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                else if (ncboRSDateSearch_His == (int)EnumCompareOperator.NotBetween)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetTo_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                sRSState = "";

            }
            #endregion
            #region qc Date
            if (ncboQCdate != (int)EnumCompareOperator.None)
            {
                _dtQCdateFrom = dtQCdateFrom;
                _dtQCdateTo = dtQCdateTo;
                _ncboQCdate = ncboQCdate;

                Global.TagSQL(ref sSQL);
                if (ncboQCdate == (int)EnumCompareOperator.EqualTo)
                {
                    _dtQCdateTo = _dtQCdateFrom;
                   // sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdate>='" + dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "' and QCdate<'" + dtQCdateFrom.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
                    _sDateRange = "Date " + _dtQCdateFrom.ToString("dd MMM yyyy");
                }
                else if (ncboQCdate == (int)EnumCompareOperator.NotEqualTo)
                {
                   // sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdate<'" + dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "' and QCdate>='" + dtQCdateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
                }
                else if (ncboQCdate == (int)EnumCompareOperator.GreaterThan)
                {
                    //sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + sRSState + ") and EventTime>='" + dtRouteSheetFrom_His.ToString("dd MMM yyyy") + "' and EventTime<'" + dtRouteSheetFrom_His.AddDays(1).ToString("dd MMM yyyy") + "')";
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdate>'" + dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "')";
                }
                else if (ncboQCdate == (int)EnumCompareOperator.SmallerThan)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdat<'" + dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "')";
                }
                else if (ncboQCdate == (int)EnumCompareOperator.Between)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdate>='" + dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "' and QCdate<'" + dtQCdateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
                    _sDateRange = "Date " + _dtQCdateFrom.ToString("dd MMM yyyy") + " to " + _dtQCdateTo.ToString("dd MMM yyyy");
                }
                else if (ncboQCdate == (int)EnumCompareOperator.NotBetween)
                {
                    sSQL = sSQL + " RouteSheetID in (Select RouteSheetID from RouteSheetPacking where QCdate<'" + dtQCdateFrom.ToString("dd MMM yyyy HH:00") + "' and QCdate>='" + dtQCdateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "')";
                }
                sRSState = "";

            }
            #endregion
            // if (IsdtRouteSheetSearch) sSQL = sSQL + " And RouteSheetDate Between '" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "' And '" + dtRouteSheetTo.ToString("dd MMM yyyy") + "'";

            if (sDOID != "") sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (" + sDOID + "))";
            if (sContractorIDs != "") sSQL = sSQL + " And RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ContractorID In (" + sContractorIDs + "))";
            if (sMachineIDs != "") sSQL = sSQL + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE MachineID In (" + sMachineIDs + ") )";
            if (sRSState != "") sSQL = sSQL + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE RSState In (" + sRSState + ") )";
            //if (sProductIDs != "") sSQL = sSQL + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE ProductID_Raw In (" + sProductIDs + ") )";
            if (sProductIDs != "") sSQL = sSQL + " And RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ProductID In (" + sProductIDs + "))";
            if (nOrderType > 0) sSQL = sSQL + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE OrderType In (" + nOrderType + ") )";
          
            if (sOrderNo != "") sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from [View_RouteSheetDO] where  OrderNo like '%" + sOrderNo + "%')";
            if (sRouteSheetNo != "") sSQL = sSQL + "And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE RouteSheetNo Like '%" + sRouteSheetNo + "%')";

            if (nQCStatus > 0)
            {
                if (nQCStatus == 1) ///// WIP
                {
                    sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from [RouteSheetDO] as RSD where  IsOut=1 and isnull(RSD.RSState,0) not in (" + (int)EnumRSState.QC_Done + "," + (int)EnumRSState.LotReturn + "))";
                }
                if (nQCStatus == 2) ///// WIP
                {
                    sSQL = sSQL + " And RouteSheetID in (select RouteSheetID from [RouteSheetDO] as RSD where isnull(Qty_QC,0)>0 and IsOut=1 and isnull(RSD.RSState,0) not in (" + (int)EnumRSState.QC_Done + "," + (int)EnumRSState.LotReturn + "))";
                }
            }
            if (nRSShiftID > 0) sSQL = sSQL + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE RSShiftID In (" + nRSShiftID + ") )";
            if (nDUDyeingType > 0) sSQL = sSQL + " And RouteSheetID in (Select RouteSheetID from RouteSheet WHERE HanksCone In (" + nDUDyeingType + ") )";
            if (bIsReDyeing) sSQL = sSQL + " And  RouteSheetID in (Select RouteSheetID from RouteSheet where isnull(IsReDyeing,0)=1)";
            if (!string.IsNullOrEmpty(sPIIDs)) sSQL = sSQL + " AND DyeingOrderDetailID IN ( SELECT XX.DyeingOrderDetailID FROM DyeingOrderDetail AS XX WHERE XX.DyeingOrderID IN (SELECT YY.DyeingOrderID FROM DyeingOrder AS YY WHERE YY.ExportPIID IN (" + sPIIDs + ")))";
          
            return sSQL;
        }
        #endregion

        #region Excel
        [HttpPost]
        public ActionResult SetRSFreshDyedYarnData(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oRSFreshDyedYarn);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void RSFreshDyedYarnExcel()
        {
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            RSFreshDyedYarn objRSFreshDyedYarn = new RSFreshDyedYarn();
            try
            {
                objRSFreshDyedYarn = (RSFreshDyedYarn)Session[SessionInfo.ParamObj];
                if(string.IsNullOrEmpty(objRSFreshDyedYarn.ErrorMessage))
                {
                    _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" WHERE RSState In (" + (int)EnumRSState.YarnOut + ") and  RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + (int)EnumRSState.YarnOut + ") and EventTime>=''" + DateTime.Now.AddDays(-20).ToString("dd MMM yyyy") + "'' and EventTime<''" + DateTime.Now.ToString("dd MMM yyyy") + "'')", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    string sSQL = MakeSQL(objRSFreshDyedYarn, true);
                    _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                
            }
            catch
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            }

            if (_oRSFreshDyedYarns.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "#SL", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Date", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn Count", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M/C No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color/Shade", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Status", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Load Time", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Load By", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Unload Time", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Unload By", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Shift", Width = 15f, IsRotate = false });
                
                #endregion


                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Dying Card List");
                    sheet.Name = "Dying_Card_List";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #endregion
                    nRowIndex++;
                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = "Dying Card List"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 2;
                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    #region Data

                    nRowIndex++;

                    int nCount = 0;
                    foreach (var oItem in _oRSFreshDyedYarns)
                    {
                        nStartCol = 2;

                        #region DATA   
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (++nCount).ToString("00"), false, ExcelHorizontalAlignment.Right, false,false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RSDateStr, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RouteSheetNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.OrderNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.MachineName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ColorName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RSStateStr, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Qty_RS.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LoadTimeStr, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LoadByName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.UnloadTimeStr, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.UnloadByName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RSShiftName, false, ExcelHorizontalAlignment.Left, false, false);
                        #endregion
                        nRowIndex++;

                    }

                    #region Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Total : ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right, false);
                    nStartCol++;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oRSFreshDyedYarns.Sum(x => x.Qty_RS).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Right, false);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Dying_Card_List.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }

        }

        #endregion

        #region Floor stock Dyeing
        public ActionResult ViewRSFloorStockReport(int buid,int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RouteSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oDUOrderSetups = oDUOrderSetups.Where(o => o.OrderType != (int)EnumOrderType.LabDipOrder && o.OrderType != (int)EnumOrderType.TwistOrder && o.OrderType != (int)EnumOrderType.ReConing && o.OrderType != (int)EnumOrderType.LoanOrder).ToList();

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUDyeingTypes = DUDyeingType.Gets("SELECT * FROM DUDyeingType", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RouteSheetSetup = oRouteSheetSetup.GetBy(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.DyeingTypes = DUDyeingType.GetsActivity(true, ((User)Session[SessionInfo.CurrentUser]).UserID); ;

            ViewBag.RSStatuss = EnumObject.jGets(typeof(EnumRSState));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OrderTypes = oDUOrderSetups;
            ViewBag.RSSubStatuss = EnumObject.jGets(typeof(EnumRSSubStates));
            //_oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" WHERE  RSState In (" + (int)EnumRSState.YarnOut + ") and RouteSheetDate>''" + DateTime.Today.AddYears(-1).ToString("dd MMM yyyy"), ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" WHERE RSState In (" + (int)EnumRSState.YarnOut + ") and  RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + (int)EnumRSState.YarnOut + ") and EventTime>=''" + DateTime.Now.AddDays(-20).ToString("dd MMM yyyy") + "'' and EventTime<''" + DateTime.Now.ToString("dd MMM yyyy") + "'')", ((User)Session[SessionInfo.CurrentUser]).UserID);

            //_oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" WHERE RSState In (" + (int)EnumRSState.YarnOut + ") and  RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus In (" + (int)EnumRSState.YarnOut + ") and EventTime>=''" + DateTime.Now.AddDays(-20).ToString("dd MMM yyyy") + "'' and EventTime<''" + DateTime.Now.ToString("dd MMM yyyy") + "'') and RouteSheetID  not in ( Select DUH.RouteSheetID from DUHardWinding as DUH  where ReceiveDate is not null)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" WHERE RSState In (8,9,10,11,12,14) and RouteSheetID  not in ( Select DUH.RouteSheetID from DUHardWinding as DUH  where ReceiveDate is not null)", ((User)Session[SessionInfo.CurrentUser]).UserID);          

            return View(_oRSFreshDyedYarns);
        }
        [HttpPost]
        public JsonResult AdvSearchRSFloorStock(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            try
            {
                string sSQL = MakeSQL_FloorStock(oRSFreshDyedYarn);
                _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                _oRSFreshDyedYarn.ErrorMessage = ex.Message;
                _oRSFreshDyedYarns.Add(_oRSFreshDyedYarn);
            }
            var jsonResult = Json(_oRSFreshDyedYarns, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
     
        private string MakeSQL_FloorStock(RSFreshDyedYarn oRSFreshDyedYarn)
        {
            string sParams = oRSFreshDyedYarn.ErrorMessage;

            string sRouteSheetNo = "";
            int ncboFloorStatus = 0;
            DateTime dtRouteSheetFrom = DateTime.Now;
            DateTime dtRouteSheetTo = DateTime.Now;
            string sDOID = "";
            string sContractorIDs = "";
            string sMachineIDs = "";
            string sRSState = "";
            string sOrderNo = "";
            string sProductIDs = "";
            int nOrderType = 0;
        
            int ncboQCState = 0;
            DateTime dtQCStateFrom = DateTime.Now;
            DateTime dtQCStateTo = DateTime.Now;
            int nQCState = 0;

            string sSQL = "";
            string sReturn = "";
            if (!string.IsNullOrEmpty(sParams))
            {
                ncboFloorStatus = Convert.ToInt16(sParams.Split('~')[0]);
                sMachineIDs = (string.IsNullOrEmpty(sParams.Split('~')[1])) ? "" : sParams.Split('~')[1].Trim();
                sContractorIDs = (string.IsNullOrEmpty(sParams.Split('~')[2])) ? "" : sParams.Split('~')[2].Trim();
                sProductIDs = (string.IsNullOrEmpty(sParams.Split('~')[3])) ? "" : sParams.Split('~')[3].Trim();
                nOrderType = Convert.ToInt16(sParams.Split('~')[4]);
                sDOID = (string.IsNullOrEmpty(sParams.Split('~')[5])) ? "" : sParams.Split('~')[5].Trim();
                sRouteSheetNo = (string.IsNullOrEmpty(sParams.Split('~')[6])) ? "" : sParams.Split('~')[6].Trim();
                sOrderNo = (string.IsNullOrEmpty(sParams.Split('~')[7])) ? "" : sParams.Split('~')[7].Trim();
            
                if (sParams.Split('~').Length > 8)
                    Int32.TryParse(sParams.Split('~')[8], out nQCState);

                if (sParams.Split('~').Length > 9)
                    Int32.TryParse(sParams.Split('~')[9], out ncboQCState);
                if (ncboQCState > 0)
                {
                    dtQCStateFrom = Convert.ToDateTime(sParams.Split('~')[10]);
                    dtQCStateTo = Convert.ToDateTime(sParams.Split('~')[11]);
                }
              
            }

            sReturn = " where RouteSheet.RouteSheetDate>''" + DateTime.Now.AddMonths(-15).ToString("dd MMM yy") + "'' and RSState in (" +  (int)EnumRSState.UnloadedFromDyeMachine + "," + (int)EnumRSState.LoadedInHydro + "," + (int)EnumRSState.UnloadedFromHydro + "," + (int)EnumRSState.LoadedInDrier + "," + (int)EnumRSState.UnLoadedFromDrier + "," + (int)EnumRSState.InPackageing + "," + (int)EnumRSState.InSubFinishingstore_Partially + "," + (int)EnumRSState.InHW_Sub_Store + ") ";

            if (!string.IsNullOrEmpty(sRouteSheetNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RouteSheetNo Like ''%" + sRouteSheetNo + "%''";
            }
            
            #region Shade QC Status  Date
            if (ncboQCState != (int)EnumCompareOperator.None)
            {


                Global.TagSQL(ref sReturn);
                if (ncboQCState == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where " + (nQCState > 0 ? " RSSubStatus In (" + nQCState + ") and " : "") + " LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtQCStateFrom.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where " + (nQCState > 0 ? " RSSubStatus In (" + nQCState + ") and " : "") + " LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtQCStateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where " + (nQCState > 0 ? " RSSubStatus In (" + nQCState + ") and " : "") + "  LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtQCStateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where " + (nQCState > 0 ? " RSSubStatus In (" + nQCState + ") and " : "") + "  LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtQCStateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where " + (nQCState > 0 ? " RSSubStatus In (" + nQCState + ") and " : "") + "  LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtQCStateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                else if (ncboQCState == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " RouteSheetID in (Select RouteSheetID from RSInQCSubStatus where " + (nQCState > 0 ? " RSSubStatus In (" + nQCState + ") and " : "") + "  LastUpdateDateTime>=''" + dtQCStateFrom.ToString("dd MMM yyyy HH:00") + "'' and LastUpdateDateTime<''" + dtQCStateTo.AddDays(1).ToString("dd MMM yyyy HH:00") + "'')";
                }
                nQCState = 0;

            }
            #endregion
            if (sDOID != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID in (select RouteSheetID from RouteSheetDO where  DyeingOrderDetailID in (" + sDOID + "))"; }
            if (sContractorIDs != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ContractorID In (" + sContractorIDs + "))"; }
            if (sMachineIDs != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " MachineID In (" + sMachineIDs + ")"; }
            if (sProductIDs != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID In ( Select RouteSheetID from [View_RouteSheetDO] Where ProductID In (" + sProductIDs + "))"; }
            if (nOrderType > 0) { Global.TagSQL(ref sReturn); sReturn = sReturn + " OrderType In (" + nOrderType + ")"; }
            //SELECT RouteSheetID, RSSubStatus FROM RSInQCSubStatus
            //if (nQCState > 0) { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID IN (SELECT RouteSheetID FROM RSInQCSubStatus WHERE  RSSubStatus IN (" + nQCState + "))"; }
            if (nQCState > 0) { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID IN ( Select HH.RouteSheetID from ( SELECT *,ROW_NUMBER() OVER(Partition by RouteSheetID ORDER BY RouteSheetID Asc,DBserverdatetime Desc) as ID FROM RSInQCSubStatus  ) as HH where ID=1 and RSSubStatus in (" + nQCState + "))"; }
            if (sOrderNo != "") { Global.TagSQL(ref sReturn); sReturn = sReturn + " RouteSheetID in (select RouteSheetID from [View_RouteSheetDO] where  OrderNo like ''%" + sOrderNo + "%'')"; }

            if (ncboFloorStatus > 0)
            {

                Global.TagSQL(ref sReturn);
                if (ncboFloorStatus == 1)////In Dyeing Machine
                {
                    sReturn = sReturn + " RSState In (" + (int)EnumRSState.UnloadedFromDyeMachine + ")";
                }
                if (ncboFloorStatus == 2)////Loaded In Hydro
                {
                    sReturn = sReturn + " RSState In (" + (int)EnumRSState.LoadedInHydro +")";
                }
                if (ncboFloorStatus == 3)////unloaded From Hydro
                {
                    sReturn = sReturn + " RSState In (" + (int)EnumRSState.UnloadedFromHydro + ")";
                }
                if (ncboFloorStatus == 4)////Loaded In Drier
                {
                    sReturn = sReturn + " RSState In (" + (int)EnumRSState.LoadedInDrier + ")";
                }
                if (ncboFloorStatus == 5)////Un Loaded From Drier
                {
                    sReturn = sReturn + " RSState In (" + (int)EnumRSState.UnLoadedFromDrier + ")";
                }
                if (ncboFloorStatus == 6)////In QC
                {
                    sReturn = sReturn + " RouteSheetID not in ( Select isnull(DUH.RouteSheetID,0) from DUHardWinding as DUH  where isnull(DUH.RouteSheetID,0)>0 ) and RouteSheetID  in (select RouteSheetID FROM RSInQCSubStatus )";
                }
                if (ncboFloorStatus == 7)////Wating For Receive in HW
                {
                    sReturn = sReturn + " RouteSheetID  in ( Select isnull(DUH.RouteSheetID,0) from DUHardWinding as DUH  where isnull(DUH.RouteSheetID,0)>0 and ReceiveDate is null)";
                }
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  RouteSheetID not in ( Select isnull(DUH.RouteSheetID,0) from DUHardWinding as DUH  where isnull(DUH.RouteSheetID,0)>0 and ReceiveDate is not null)";
            }


            sReturn = sReturn + " ";//RSState>=" + (int)EnumRSState.UnLoadedFromDrier;          
            sSQL = sSQL + sReturn;
            return sSQL;
        }
        #endregion

        #region Floor Stock Report
        [HttpGet]
        public ActionResult RSFloorStockReportPrintList()
        {
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            RSFreshDyedYarn objRSFreshDyedYarn = new RSFreshDyedYarn();
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            try
            {
                objRSFreshDyedYarn = (RSFreshDyedYarn)Session[SessionInfo.ParamObj];
                if (string.IsNullOrEmpty(objRSFreshDyedYarn.ErrorMessage))
                {
                    _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" where RSState in (" + (int)EnumRSState.UnloadedFromDyeMachine + "," + (int)EnumRSState.LoadedInHydro + "," + (int)EnumRSState.UnloadedFromHydro + "," + (int)EnumRSState.LoadedInDrier + "," + (int)EnumRSState.UnLoadedFromDrier + "," + (int)EnumRSState.InPackageing + "," + (int)EnumRSState.InSubFinishingstore_Partially + "," + (int)EnumRSState.InHW_Sub_Store + ") and RouteSheetID not in ( Select  isnull(DUH.RouteSheetID,0)  from DUHardWinding as DUH  where  isnull(DUH.RouteSheetID,0) >0 and ReceiveDate is not null)", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    string sSQL = MakeSQL_FloorStock(objRSFreshDyedYarn);
                    _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            }

            if (_oRSFreshDyedYarns.Count > 0)
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptRSFloorStockReportList oReport = new rptRSFloorStockReportList();
                byte[] abytes = oReport.PrepareReport(_oRSFreshDyedYarns, oCompany);
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
        public void RSFloorStockReportExcel()
        {
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            RSFreshDyedYarn objRSFreshDyedYarn = new RSFreshDyedYarn();
            _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            try
            {
                objRSFreshDyedYarn = (RSFreshDyedYarn)Session[SessionInfo.ParamObj];
                if (string.IsNullOrEmpty(objRSFreshDyedYarn.ErrorMessage))
                {
                    _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" where RSState in (" + (int)EnumRSState.UnloadedFromDyeMachine + "," + (int)EnumRSState.LoadedInHydro + "," + (int)EnumRSState.UnloadedFromHydro + "," + (int)EnumRSState.LoadedInDrier + "," + (int)EnumRSState.UnLoadedFromDrier + "," + (int)EnumRSState.InPackageing + "," + (int)EnumRSState.InSubFinishingstore_Partially + "," + (int)EnumRSState.InHW_Sub_Store + ") and RouteSheetID not in ( Select  isnull(DUH.RouteSheetID,0)  from DUHardWinding as DUH  where  isnull(DUH.RouteSheetID,0) >0 and ReceiveDate is not null)", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    string sSQL = MakeSQL_FloorStock(objRSFreshDyedYarn);
                    _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                #region EXCEL
                List<RSFreshDyedYarn> oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                rowIndex = 2;
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
                    OfficeOpenXml.ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add("Floor Stock Report");
                    sheet.Name = "Floor Stock Report";
                    sheet.Column(colIndex++).Width = 10; //SL
                    sheet.Column(colIndex++).Width = 20; //Batch Date
                    sheet.Column(colIndex++).Width = 20; //Batch No
                    sheet.Column(colIndex++).Width = 20; //OrderNo
                    sheet.Column(colIndex++).Width = 20; //Customer
                    sheet.Column(colIndex++).Width = 25; //Count
                    sheet.Column(colIndex++).Width = 20; //Shade
                    sheet.Column(colIndex++).Width = 20; //Qty(kg)
                    sheet.Column(colIndex++).Width = 35; //Pcs
                    sheet.Column(colIndex++).Width = 20; //remarks

                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 14].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Floor Stock Report"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region STATUS Print

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; cell.Value = "In House: " + _oRSFreshDyedYarns.Where(x => x.IsInHouse == true).Sum(x => x.Qty_RS).ToString("#,##0.00;(#,##0.00)"); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 5, rowIndex, 7]; cell.Merge = true; cell.Value = "Commission: " + _oRSFreshDyedYarns.Where(x => x.IsInHouse == false).Sum(x => x.Qty_RS); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 8, rowIndex, 11]; cell.Merge = true; cell.Value = "Total: " + _oRSFreshDyedYarns.Sum(x => x.Qty_RS).ToString("#,##0.00;(#,##0.00)") + " kg"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rowIndex++;
                    #endregion


                    #region Print Header 1

                    oRSFreshDyedYarns = _oRSFreshDyedYarns;
                    oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.UnloadedFromDyeMachine).ToList();
                    if (oRSFreshDyedYarns.Count > 0)
                    {
                        this.RSFloorStockPrintBody(oRSFreshDyedYarns, "Unloaded From DyeMachine", sheet, cell, border);
                    }

                    oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.LoadedInHydro && x.RSSubStates == EnumRSSubStates.None).ToList();
                    if (oRSFreshDyedYarns.Count > 0)
                    {
                        this.RSFloorStockPrintBody(oRSFreshDyedYarns, "Loaded In Hydro", sheet, cell, border);
                    }

                    oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.UnloadedFromHydro && x.RSSubStates == EnumRSSubStates.None).ToList();
                    if (oRSFreshDyedYarns.Count > 0)
                    {
                        //int row = rowIndex;
                        this.RSFloorStockPrintBody(oRSFreshDyedYarns, "Unloaded From Hydro", sheet, cell, border);
                    }

                    oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.LoadedInDrier && x.RSSubStates == EnumRSSubStates.None).ToList();
                    if (oRSFreshDyedYarns.Count > 0)
                    {
                        this.RSFloorStockPrintBody(oRSFreshDyedYarns, "Loaded In Drier", sheet, cell, border);
                    }

                    oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.UnLoadedFromDrier && x.RSSubStates == EnumRSSubStates.None).ToList();
                    if (oRSFreshDyedYarns.Count > 0)
                    {
                        this.RSFloorStockPrintBody(oRSFreshDyedYarns, "UnLoaded In Drier", sheet, cell, border);
                    }

                    oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSSubStates != EnumRSSubStates.None && x.RSState != EnumRSState.InHW_Sub_Store).ToList();
                    if (oRSFreshDyedYarns.Count > 0)
                    {
                        this.RSFloorStockPrintBody(oRSFreshDyedYarns, "In Shade QC", sheet, cell, border);
                    }

                    oRSFreshDyedYarns = _oRSFreshDyedYarns.Where(x => x.RSState == EnumRSState.InHW_Sub_Store).ToList();
                    if (oRSFreshDyedYarns.Count > 0)
                    {
                        this.RSFloorStockPrintBody(oRSFreshDyedYarns, "Wating For Receive in Hard winding", sheet, cell, border);
                    }

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=RPT_RSFloorStockReport.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();

                }
                #endregion

            }
            catch
            {
                _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
            }
          
        }
        private void RSFloorStockPrintBody(List<RSFreshDyedYarn> oRSFreshDyedYarns, string sHeaderStatus, OfficeOpenXml.ExcelWorksheet _sheet, ExcelRange _cell, OfficeOpenXml.Style.Border _border)
        {
            //int rowIndex = _rowIndex;
            ExcelRange cell;
            cell = _cell;
            ExcelFill fill;
            OfficeOpenXml.ExcelWorksheet sheet = _sheet;
            OfficeOpenXml.Style.Border border = _border;
            int nTotalCol = 0;
            int nCount = 0;
            int colIndex = 2;

            using (var excelPackage = new ExcelPackage())
            {
                colIndex = 2;
                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = sHeaderStatus; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                var oOrderType = oRSFreshDyedYarns.GroupBy(x => new { x.IsInHouse }, (key, grp) =>
                                 new
                                 {
                                     IsInHouse = key.IsInHouse,
                                 }).ToList();
                foreach (var oItem in oOrderType)
                {
                    var oRSFloorStocks = oRSFreshDyedYarns.Where(x => x.IsInHouse == oItem.IsInHouse).ToList();
                    #region Fabric Info
                    if (oOrderType.Count > 0)
                    {             
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = (oItem.IsInHouse) ? "In House" : "Out Side"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Font.Size = 12; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        rowIndex++;
                    }
                    #region Header 2
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Batch No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Customer"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Count"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Shade"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Qty(kg)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Pcs"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    #endregion
                    #endregion

                    #region DATA
                    int nSL = 1;
                    foreach (var oItem1 in oRSFloorStocks)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.RSDateStr; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.RouteSheetNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.OrderNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.ContractorName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.ProductName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (string.IsNullOrEmpty(oItem1.ColorName) ? "" : oItem1.ColorName); cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.Qty_RS; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.BagCount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem1.RSSubStatesSt; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        nSL++;
                        rowIndex++;

                    }

                    #region TOTAL

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Total:";cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = oRSFloorStocks.Sum(x => x.Qty_RS); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = oRSFloorStocks.Sum(x => x.BagCount); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    rowIndex++;
                    #endregion

                    #endregion

                }
               
            }

        }


        //[HttpGet]
        //public void RSFloorStockReportExcel()
        //{
        //    _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
        //    RSFreshDyedYarn objRSFreshDyedYarn = new RSFreshDyedYarn();
        //    _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
        //    try
        //    {
        //        objRSFreshDyedYarn = (RSFreshDyedYarn)Session[SessionInfo.ParamObj];
        //        if (string.IsNullOrEmpty(objRSFreshDyedYarn.ErrorMessage))
        //        {
        //            _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(" where RSState in (" + (int)EnumRSState.UnloadedFromDyeMachine + "," + (int)EnumRSState.LoadedInHydro + "," + (int)EnumRSState.UnloadedFromHydro + "," + (int)EnumRSState.LoadedInDrier + "," + (int)EnumRSState.UnLoadedFromDrier + "," + (int)EnumRSState.InPackageing + "," + (int)EnumRSState.InSubFinishingstore_Partially + "," + (int)EnumRSState.InHW_Sub_Store + ") and RouteSheetID not in ( Select  isnull(DUH.RouteSheetID,0)  from DUHardWinding as DUH  where  isnull(DUH.RouteSheetID,0) >0 and ReceiveDate is not null)", ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //        else
        //        {
        //            string sSQL = MakeSQL_FloorStock(objRSFreshDyedYarn);
        //            _oRSFreshDyedYarns = RSFreshDyedYarn.GetsLoadUnload(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }

        //    }
        //    catch
        //    {
        //        _oRSFreshDyedYarns = new List<RSFreshDyedYarn>();
        //    }

        //    if (_oRSFreshDyedYarns.Count > 0)
        //    {
        //        Company oCompany = new Company();
        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        #region Header
        //        List<TableHeader> table_header = new List<TableHeader>();

        //        table_header.Add(new TableHeader { Header = "#SL", Width = 8f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Batch Date", Width = 13f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Customer", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Yarn Count", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Color/Shade", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Status", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Qty", Width = 13f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "PCS", Width = 13f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Remarks", Width = 20f, IsRotate = false });

        //        #endregion
        //        #region Export Excel
        //        int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
        //        ExcelRange cell; ExcelFill fill;
        //        OfficeOpenXml.Style.Border border;

        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            excelPackage.Workbook.Properties.Author = "ESimSol";
        //            excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //            var sheet = excelPackage.Workbook.Worksheets.Add("Stock Report List");
        //            sheet.Name = "  StockReport_List";

        //            foreach (TableHeader listItem in table_header)
        //            {
        //                sheet.Column(nStartCol++).Width = listItem.Width;
        //            }

        //            #region Report Header
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
        //            cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            #endregion
        //            nRowIndex++;
        //            #region Address & Date
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
        //            cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            #endregion
        //            nRowIndex++;
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
        //            cell.Value = "Stock Report List"; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            nRowIndex += 2;
        //            #region Column Header
        //            nStartCol = 2;
        //            foreach (TableHeader listItem in table_header)
        //            {
        //                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            }
        //            #endregion

        //            #region Data

        //            nRowIndex++;

        //            int nCount = 0;
        //            foreach (var oItem in _oRSFreshDyedYarns)
        //            {
        //                nStartCol = 2;

        //                #region DATA
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), false, ExcelHorizontalAlignment.Center, false, false);
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RSDateStr, false, ExcelHorizontalAlignment.Center, false, false);
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RouteSheetNo, false, ExcelHorizontalAlignment.Left, false, false);
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.OrderNo, false, ExcelHorizontalAlignment.Left, false, false);
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ColorName, false, ExcelHorizontalAlignment.Left, false, false);
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RSStateStr, false, ExcelHorizontalAlignment.Left, false, false);
        //                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Qty_RS.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
        //                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BagCount.ToString(), false, ExcelHorizontalAlignment.Center, false, false);
        //                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RSSubStatesSt, false, ExcelHorizontalAlignment.Left, false, false);
                
        //                #endregion
        //                nRowIndex++;

        //            }

        //            #region Total
        //            nStartCol = 2;
        //            ExcelTool.FillCellMerge(ref sheet, "Total : ", nRowIndex, nRowIndex, nStartCol, nStartCol += 7, true, ExcelHorizontalAlignment.Right, false);
        //            nStartCol++;
        //            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oRSFreshDyedYarns.Sum(x => x.Qty_RS).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
        //            ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 1, true, ExcelHorizontalAlignment.Right, false);
        //            nRowIndex++;
        //            #endregion

        //            cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.White);
        //            #endregion

        //            Response.ClearContent();
        //            Response.BinaryWrite(excelPackage.GetAsByteArray());
        //            Response.AddHeader("content-disposition", "attachment; filename=FloorStock_List.xlsx");
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.Flush();
        //            Response.End();
        //        }
        //        #endregion
        //    }

        //}
        #endregion

    }
}
