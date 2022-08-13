using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class LotBuyerController : PdfViewController
    {
        #region Declaration
        LotBuyer _oLotBuyer = new LotBuyer();
        List<LotBuyer> _LotBuyers = new List<LotBuyer>();
        #endregion
        public ActionResult ViewLotBuyers(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<LotBuyer> oLotBuyers = new List<LotBuyer>();
          
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
           List<WorkingUnit> WorkingUnits= WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
           ViewBag.Stores = WorkingUnits;
            return View(oLotBuyers);
        }
        #region AdvSearch
        public ActionResult AdvSearchLotBuyerView()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult AdvSearchLotBuyer(string sTemp)
        {
            List<LotBuyer> oLotBuyers = new List<LotBuyer>();
            int nBUID = Convert.ToInt32(sTemp.Split('~')[0]);
            string searchStr = sTemp.Split('~')[1];
            LotBuyer oLotBuyer = new LotBuyer();
            try
            {
                string sSQL = GetSQL(sTemp);
                oLotBuyers = LotBuyer.Gets(sSQL, nBUID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {

                _oLotBuyer.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotBuyers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
             int nBUID = Convert.ToInt32(sTemp.Split('~')[0]);
            int ncboStore = Convert.ToInt32(sTemp.Split('~')[1]);
            string sLotNo = sTemp.Split('~')[2];
            string sProductIDs = sTemp.Split('~')[3];
            string sContractorIDs = sTemp.Split('~')[4];

            string sReturn1 = "AND  Lot.BUID  IN (" + nBUID + ")";
            string sReturn = "";
          
            #region WorkingUnit
            if (ncboStore > 0)
            {
               // Global.TagSQL(ref sReturn);
                sReturn = sReturn + "AND Lot.WorkingUnitID IN (" + ncboStore + ")";

            }
            #endregion

            #region Lot No

            if (sLotNo != "")
            {
                //Global.TagSQL(ref sReturn);
                sReturn = sReturn + "AND LotNo LIKE '%" + sLotNo + "%'";

            }
            #endregion


            #region Product wise

            if (sProductIDs != "")
            {
                //Global.TagSQL(ref sReturn);
                sReturn = sReturn + "AND Lot.ProductID IN (" + sProductIDs + ")";
            }
            #endregion
            #region Contractor wise

            if (sContractorIDs != "")
            {
                //Global.TagSQL(ref sReturn);
                sReturn = sReturn + "AND ContractorID IN (" + sContractorIDs + ")";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion
        #region ReportXL
        public void Print_ReportXL(string sTempString, int BUID)
        {

            List<LotBuyer> oLotBuyers = new List<LotBuyer>();
            int nBUID = Convert.ToInt32(sTempString.Split('~')[0]);
            string searchStr = sTempString.Split('~')[1];
            LotBuyer oLotBuyer = new LotBuyer();
            string sSQL = GetSQL(sTempString);
            oLotBuyers = LotBuyer.Gets(sSQL, nBUID, (int)Session[SessionInfo.currentUserID]);


            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nBalance= 0;
            


           #region Export Excel
                int nRowIndex = 2, nStartRow = 1, nEndRow = 0, nStartCol = 1, nEndCol = 8;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Buyer Lot");
                    sheet.Name = "Buyer Lot";

                    sheet.Column(2).Width = 28;
                    sheet.Column(3).Width = 25;
                    sheet.Column(4).Width = 28;
                    sheet.Column(5).Width = 20;
                    sheet.Column(6).Width = 18;
                    sheet.Column(7).Width = 20;
                    sheet.Column(8).Width = 20;




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
                    cell.Value = "Buyer Lot"; cell.Style.Font.Bold = true;
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
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Party Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "product Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Location Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;


                    #endregion
                    string sTemp = "";
                    #region Data
                    foreach (LotBuyer oItem in oLotBuyers)
                    {

                        nSL++;

                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Balance; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.OperationUnitName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nBalance = nBalance + oItem.Balance;
                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }
                    #endregion
                    #region Total
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + "Total"; cell.Style.Font.Bold = false;
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
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value =nBalance; cell.Style.Font.Bold = true;
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




                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=BuyerLot.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

         
         





        }
        #endregion
    }
}