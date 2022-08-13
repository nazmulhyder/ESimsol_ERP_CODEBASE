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
using iTextSharp.text.pdf;
using iTextSharp;
using ESimSol.Reports;
using ESimSolFinancial.Controllers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class StyleWiseStockController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        List<StyleWiseStock> _oStyleWiseStocks = new List<StyleWiseStock>();        

        #region Actions
        public ActionResult ViewStyleWiseStocks(int buid,int menuid )
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            StyleWiseStock oStyleWiseStock = new StyleWiseStock();

            ViewBag.BusinessSessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(oStyleWiseStock);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(StyleWiseStock oStyleWiseStock)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oStyleWiseStock);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintStyleWiseStock(double ts)
        {
            StyleWiseStock oStyleWiseStock = new StyleWiseStock();
            try
            {
                _sErrorMesage = "";
                _oStyleWiseStocks = new List<StyleWiseStock>();
                oStyleWiseStock = (StyleWiseStock)Session[SessionInfo.ParamObj];
                int nSelectedItemQty = Convert.ToInt32(oStyleWiseStock.SearchingData.Split('~')[7]);
                string sSQL = this.GetSQL(oStyleWiseStock);
                _oStyleWiseStocks = StyleWiseStock.Gets(sSQL,nSelectedItemQty, (int)Session[SessionInfo.currentUserID]);
                if (_oStyleWiseStocks.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oStyleWiseStocks = new List<StyleWiseStock>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oStyleWiseStock.BUID!= 0)
                {
                    oBusinessUnit = oBusinessUnit.Get(oStyleWiseStock.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.Name = oBusinessUnit.Name; oCompany.Address = oBusinessUnit.Address; oCompany.Email= oBusinessUnit.Email;
                    oCompany.Phone = oBusinessUnit.Phone; oCompany.WebAddress= oBusinessUnit.WebAddress; oCompany.Name = oBusinessUnit.Name;
                }
                rptStyleWiseStock oReport = new rptStyleWiseStock();
                byte[] abytes = oReport.PrepareReport(_oStyleWiseStocks, oCompany,  _sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public void ExportToExcelStyleWiseStock(double ts)
        {
            StyleWiseStock oStyleWiseStock = new StyleWiseStock();
            _sErrorMesage = "";
            _oStyleWiseStocks = new List<StyleWiseStock>();
            oStyleWiseStock = (StyleWiseStock)Session[SessionInfo.ParamObj];
            int nSelectedItemQty = Convert.ToInt32(oStyleWiseStock.SearchingData.Split('~')[7]);
            string sSQL = this.GetSQL(oStyleWiseStock);
            _oStyleWiseStocks = StyleWiseStock.Gets(sSQL,nSelectedItemQty, (int)Session[SessionInfo.currentUserID]);
          
            byte[] abytes = null;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(_oStyleWiseStocks[0].BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
          

            #region Buyer Wise
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Style Wise Storck Report");
                sheet.Name = "Style Wise Storck Report";

                #region SHEET COLUMN WIDTH
                int nColumn = 1;
                int nWidth = 5;
                sheet.Column(++nColumn).Width = 6;   // SL NO    
                sheet.Column(++nColumn).Width = 20 + nWidth;  // Code
                sheet.Column(++nColumn).Width = (20 + nWidth);  // Product Name  
                sheet.Column(++nColumn).Width = 20;  //ItemDescription                  
                sheet.Column(++nColumn).Width = 40;  //store                  
                sheet.Column(++nColumn).Width = 20;  // Color Name
                sheet.Column(++nColumn).Width = 20;  //Size Name
                sheet.Column(++nColumn).Width = 15;  // M. Unit
                sheet.Column(++nColumn).Width = 15; //Req Qty
                sheet.Column(++nColumn).Width = 15; //Cutting
                sheet.Column(++nColumn).Width = 15; // Consumption                 
                sheet.Column(++nColumn).Width = 15; // Booking Qty
                sheet.Column(++nColumn).Width = 15; // Received Qty 
                sheet.Column(++nColumn).Width = 15; // Issue Qty
                sheet.Column(++nColumn).Width = 15; // Stock Balance
                sheet.Column(++nColumn).Width = 15 + nWidth; //Supplier Name
                sheet.Column(++nColumn).Width = 15; //Remarks

                nEndCol = nColumn;

                #endregion

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Style Wise Stock Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                nRowIndex = nRowIndex + 1;
                #endregion

                int nStyleID = 0; int nCount = 0;
                foreach (StyleWiseStock oItem in _oStyleWiseStocks)
                {
                    nCount++;
                    if (nStyleID != oItem.StyleID)
                    {
                        nCount = 1;

                        #region Style Information
                        nRowIndex = nRowIndex + 2;
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Style No : " + oItem.StyleNo + " || Buyer Name : " + oItem.BuyerName + " || Business Session : " + oItem.SessionName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion

                        #region Column Header
                        nRowIndex = nRowIndex + 1;
                        nStartRow = nRowIndex;
                        int nHeaderIndex = 1;
                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Item Description"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Color Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Size Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "M. Unit"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Req Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Cutting"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Consumption"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Booking Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Received Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Issue Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Stock Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Supplier Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                        nStyleID = oItem.StyleID;
                        #endregion
                    }

                    #region Data

                    int nDataIndex = 1;
                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.Numberformat.Format = "###0;(###0)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.StoreWithQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.SizeName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ItemDescription; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.MUnitSymbol; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ReqQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.CuttingQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ConsumptionQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  
                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ReceivedQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value =  oItem.IssueQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.StockBalance; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.SupplierName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.BillNote; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion

                    nEndRow = nRowIndex;
                    nRowIndex++;
                }

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Support Functions
        private string GetSQL(StyleWiseStock oStyleWiseStock)
        {
            _sDateRange = "";
            string sSearchingData = oStyleWiseStock.SearchingData;
            EnumCompareOperator eReceivedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dReceivedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dReceivedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
            int nBusinessSessionID = Convert.ToInt32(sSearchingData.Split('~')[6]);
            
            string sWhereCluse = "";

            #region BusinessUnit
            //if (oStyleWiseStock.BUID > 0)
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + " HH.BUID =" + oStyleWiseStock.BUID.ToString();
            //}
            #endregion

            #region LotNo
            //if (oStyleWiseStock.LotNo != null && oStyleWiseStock.LotNo != "")
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + " HH.LotNo LIKE'%" + oStyleWiseStock.LotNo + "%'";
            //}
            #endregion

            #region StoreID
            //if (oStyleWiseStock.StoreID != 0)
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + " HH.WorkingUnitID =" + oStyleWiseStock.StoreID.ToString();
            //}
            #endregion

            #region Style No
            if (oStyleWiseStock.StyleNo != null && oStyleWiseStock.StyleNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.TechnicalSheetID IN(" + oStyleWiseStock.StyleNo + ")";
            }
            #endregion

            #region BuyerName
            if (oStyleWiseStock.BuyerName != null && oStyleWiseStock.BuyerName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.TechnicalSheetID IN (SELECT MM.TechnicalSheetID FROM TechnicalSheet AS MM WHERE MM.BuyerID IN(" + oStyleWiseStock.BuyerName + "))";
            }
            #endregion
            
            #region Product
            if (oStyleWiseStock.ProductName != null && oStyleWiseStock.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.ProductID IN(" + oStyleWiseStock.ProductName + ")";
            }
            #endregion

            #region Supplier
            //if (oStyleWiseStock.SupplierName != null && oStyleWiseStock.SupplierName != "")
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + " HH.ParentType=103 AND HH.ParentID IN (SELECT NN.GRNDetailID FROM GRNDetail AS NN WHERE NN.GRNID IN(SELECT BB.GRNID FROM GRN AS BB WHERE BB.ContractorID IN(" + oStyleWiseStock.SupplierName + ")))";
            //}
            #endregion
            
            #region Received Date
            //if (eReceivedDate != EnumCompareOperator.None)
            //{
            //    if (eReceivedDate == EnumCompareOperator.EqualTo)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dReceivedStartDate.ToString("dd MMM yyyy") + "', 106))";
            //        _sDateRange = "PI Date @ " + dReceivedStartDate.ToString("dd MMM yyyy");
            //    }
            //    else if (eReceivedDate == EnumCompareOperator.NotEqualTo)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dReceivedStartDate.ToString("dd MMM yyyy") + "', 106))";
            //        _sDateRange = "PI Date Not Equal @ " + dReceivedStartDate.ToString("dd MMM yyyy");
            //    }
            //    else if (eReceivedDate == EnumCompareOperator.GreaterThen)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dReceivedStartDate.ToString("dd MMM yyyy") + "', 106))";
            //        _sDateRange = "PI Date Greater Then @ " + dReceivedStartDate.ToString("dd MMM yyyy");
            //    }
            //    else if (eReceivedDate == EnumCompareOperator.SmallerThen)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dReceivedStartDate.ToString("dd MMM yyyy") + "', 106))";
            //        _sDateRange = "PI Date Smaller Then @ " + dReceivedStartDate.ToString("dd MMM yyyy");
            //    }
            //    else if (eReceivedDate == EnumCompareOperator.Between)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dReceivedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dReceivedEndDate.ToString("dd MMM yyyy") + "', 106))";
            //        _sDateRange = "PI Date Between " + dReceivedStartDate.ToString("dd MMM yyyy") + " To " + dReceivedEndDate.ToString("dd MMM yyyy");
            //    }
            //    else if (eReceivedDate == EnumCompareOperator.NotBetween)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dReceivedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dReceivedEndDate.ToString("dd MMM yyyy") + "', 106))";
            //        _sDateRange = "PI Date NOT Between " + dReceivedStartDate.ToString("dd MMM yyyy") + " To " + dReceivedEndDate.ToString("dd MMM yyyy");
            //    }
            //}
            #endregion

            #region Consumption Date
            //if (eIssueDate != EnumCompareOperator.None)
            //{
            //    if (eIssueDate == EnumCompareOperator.EqualTo)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
            //    }
            //    else if (eIssueDate == EnumCompareOperator.NotEqualTo)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
            //    }
            //    else if (eIssueDate == EnumCompareOperator.GreaterThen)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
            //    }
            //    else if (eIssueDate == EnumCompareOperator.SmallerThen)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
            //    }
            //    else if (eIssueDate == EnumCompareOperator.Between)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
            //    }
            //    else if (eIssueDate == EnumCompareOperator.NotBetween)
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
            //    }
            //}
            #endregion

            #region Business Session
            if (nBusinessSessionID>0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.TechnicalSheetID IN (SELECT MM.TechnicalSheetID FROM TechnicalSheet AS MM WHERE MM.BusinessSessionID =" + nBusinessSessionID.ToString() + ")";
            }
            #endregion

            return sWhereCluse;
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
        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyImageTitle.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
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
    }
}

