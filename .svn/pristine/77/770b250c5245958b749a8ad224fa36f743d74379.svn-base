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
    public class RouteSheetApproveController : Controller
    {
        #region Declaration
        RouteSheetApprove _oRouteSheetApprove = new RouteSheetApprove();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<RouteSheetApprove> _oRouteSheetApproves = new List<RouteSheetApprove>();
        string _sDateRange = "";
        int _nReportType = 0;
        #endregion

        #region RouteSheetApprove
        public ActionResult View_RouteSheetApprove(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            string sSQL = "SELECT * FROM View_RouteSheetApprove   WHERE (StockQty+RSQty)>0";
            _oRouteSheetApproves = RouteSheetApprove.Gets(1, sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
         
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oRouteSheetApproves);
        }
        public ActionResult View_RouteSheetApprove_Lot( int nID)
        {
            _oRouteSheetApproves = new List<RouteSheetApprove>();
            _oRouteSheetApproves = RouteSheetApprove.Gets_LotWise(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oRouteSheetApproves);
        }
      
    
        [HttpPost]
        public JsonResult AdvSearch(RouteSheetApprove oRouteSheetApprove)
        {
            _oRouteSheetApproves = new List<RouteSheetApprove>();
            try
            {
                string sSQL = MakeSQL(oRouteSheetApprove);
                _oRouteSheetApproves = RouteSheetApprove.Gets(_nReportType, sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteSheetApproves = new List<RouteSheetApprove>();
                _oRouteSheetApprove.ErrorMessage = ex.Message;
                _oRouteSheetApproves.Add(_oRouteSheetApprove);
            }
            var jsonResult = Json(_oRouteSheetApproves, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

    
        [HttpPost]
        public JsonResult ChangeRSStatus(RouteSheetHistory oRSH)
        {
            _oRouteSheetApprove = new RouteSheetApprove();
            try
            {
                if (oRSH.RouteSheetID <= 0) throw new Exception("Please select a valid routesheet.");

                //string ErrorMessage = RSStateValidation(oRSH.PreviousState, oRSH.CurrentStatus);
                //if (ErrorMessage != "") { throw new Exception(ErrorMessage); }

                oRSH = oRSH.ChangeRSStatus(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRSH.RouteSheetID > 0)
                {
                    _oRouteSheetApprove = RouteSheetApprove.Get(oRSH.RouteSheetID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    throw new Exception(oRSH.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _oRouteSheetApprove = new RouteSheetApprove();
                _oRouteSheetApprove.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetApprove);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(RouteSheetApprove oRouteSheetApprove)
        {
            string sParams = oRouteSheetApprove.ErrorMessage;

            string sProductName = "";
            string sProductID = "";
            string sBaseName = "";
            string sRSNo = "";
            bool bIsGeneral = false;
            bool bWatingForApprove = false;
            _nReportType = 0;
            int nBUID = 0;

            if (sParams == null || sParams == "")
            {
                _oRouteSheetApprove.BaseName = "";
                _oRouteSheetApprove.ProductName = "";
                sProductName = "";
                sBaseName = "";
                bIsGeneral = true;
            }
            else
            {
                _oRouteSheetApprove.BaseName = Convert.ToString(sParams.Split('~')[0]);
                _oRouteSheetApprove.ProductName = Convert.ToString(sParams.Split('~')[1]);
                sProductName = Convert.ToString(sParams.Split('~')[2]);
                sBaseName = Convert.ToString(sParams.Split('~')[3]);
                sRSNo = Convert.ToString(sParams.Split('~')[4]);
                bIsGeneral = Convert.ToBoolean(sParams.Split('~')[5]);
                bWatingForApprove = Convert.ToBoolean(sParams.Split('~')[6]);
                _nReportType = Convert.ToInt16(sParams.Split('~')[7]);
            }


            string sReturn1 = "";
            string sReturn = "";
            if (_nReportType == 1)// Product Wise
            {
                sReturn1 = "SELECT * FROM View_RouteSheetApprove ";
            }
            else if (_nReportType == 2)// Lot Wise
            {
                sReturn1 = "SELECT * FROM View_RouteSheetApprove_Lot ";
            }
            else
            {
                sReturn1 = "SELECT * FROM View_RouteSheetApprove_RS ";
            }



            #region Product
            if (!String.IsNullOrEmpty(_oRouteSheetApprove.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + _oRouteSheetApprove.ProductName + ")";
            }
            #endregion
            #region Product
            if (!String.IsNullOrEmpty(_oRouteSheetApprove.BaseName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductBaseID in(" + _oRouteSheetApprove.BaseName + ")";
            }
            #endregion
            #region ProductName
            if (!string.IsNullOrEmpty(sProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductName LIKE '%" + sProductName + "%' ";
            }
            #endregion
            #region ProductName
            if (!string.IsNullOrEmpty(sBaseName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BaseName LIKE '%" + sBaseName + "%' ";
            }
            #endregion
            #region Product
            if (!String.IsNullOrEmpty(sRSNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(Select ProductID_Raw from RouteSheet where  RouteSheet.RSState <" + (int)EnumRSState.YarnOut + " and RouteSheet.RouteSheetNo LIKE '%" + sRSNo + "%')";
            }
            #endregion
            #region Business Unit
            if (bIsGeneral)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "(StockQty+RSQty)>0";
            }
            #endregion
            #region Business Unit
            if (bWatingForApprove)
            {
                Global.TagSQL(ref sReturn);
                if (_nReportType == 0)// Product Wise
                {
                    sReturn = sReturn + "Isnull(RSState,0)<3";
                }
                else
                {
                    sReturn = sReturn + "Isnull(RSQty,0)>0";
                }
            }
            #endregion
            //#region Business Unit
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " BUID = " + nBUID;
            //}
            //#endregion
            string sSQL = sReturn1 + " " + sReturn;
            return sSQL;
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
        #region Detail info
        [HttpPost]
        public JsonResult GetsLots(RouteSheetApprove oRouteSheetApprove)
        {
            List<RouteSheetApprove> oRouteSheetApproves = new List<RouteSheetApprove>();
            try
            {
                oRouteSheetApproves = RouteSheetApprove.Gets_LotWise( oRouteSheetApprove.ProductID , ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheetApproves = new List<RouteSheetApprove>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheetApproves);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsRSs(RouteSheetApprove oRouteSheetApprove)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                string sSQL = "Select * from View_RouteSheet Where RSState <" + (int)EnumRSState.YarnOut + " and ProductID_Raw=" + oRouteSheetApprove.ProductID;
                oRouteSheets = RouteSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRouteSheets = new List<RouteSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRouteSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Print XlX
        public void PrintRpt_Excel(string sTempString)
        {
          
            string sMunit = "";

            _oRouteSheetApproves = new List<RouteSheetApprove>();
            RouteSheetApprove oRouteSheetApprove = new RouteSheetApprove();
            oRouteSheetApprove.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oRouteSheetApprove);
            _oRouteSheetApproves = RouteSheetApprove.Gets(_nReportType, sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oRouteSheetApproves.Count > 0)
            {
                sMunit = _oRouteSheetApproves[0].MUName;
            }
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            int nSL = 0;
            double nStockQty = 0;
            double nRSQty = 0;
            double nQty_Approve = 0;
            double nBalance = 0;
          

            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 5;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("StockReport(For Production)");
                sheet.Name = "Stock Report(For Production)";
                sheet.Column(2).Width = 15;
                sheet.Column(3).Width = 40;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
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
                cell.Value = "Stock Report   Date: " +DateTime.Today.ToString("dd MMM yyyy hh:mm:tt")+""; cell.Style.Font.Bold = true;
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

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Stock Qty(" + sMunit+")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Issue  Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Booking Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Usable Qty(" + sMunit + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                foreach (RouteSheetApprove oItem in _oRouteSheetApproves)
                {


                    //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.SaleContractNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    //border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.ProductCode; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ProductName.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.StockQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.RSQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Qty_Booking; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.StockQty - oItem.RSQty - oItem.Qty_Booking); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nStockQty =nStockQty+ oItem.StockQty;
                     nRSQty = nRSQty+ oItem.RSQty;
                     nBalance = nBalance + oItem.StockQty - oItem.RSQty - oItem.Qty_Booking;
                     nQty_Approve = nQty_Approve + oItem.Qty_Booking;

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

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "" + ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nStockQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nRSQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nQty_Approve; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nBalance; cell.Style.Font.Bold = true;
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
                Response.AddHeader("content-disposition", "attachment; filename=StockReport(Production).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
          
        #endregion
    }
}
