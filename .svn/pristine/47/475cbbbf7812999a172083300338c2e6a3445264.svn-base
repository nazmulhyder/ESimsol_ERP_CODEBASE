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
    public class ITransactionGRNController : Controller
    {
        ITransactionGRN _oITransactionGRN = new ITransactionGRN();
        List<ITransactionGRN> _oITransactionGRNs = new List<ITransactionGRN>();
        public ActionResult ViewITransactionGRN(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oITransactionGRNs = new List<ITransactionGRN>();
            ViewBag.GRNTypes = EnumObject.jGets(typeof(EnumGRNType));
            ViewBag.ImportProducts = ImportProduct.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oITransactionGRNs);
        }
        [HttpPost]
        public JsonResult SearchITransactionGRN(ITransactionGRN oITransactionGRN)
        {
            try
            {
                _oITransactionGRNs = new List<ITransactionGRN>();
                _oITransactionGRNs = ITransactionGRN.Gets(oITransactionGRN.StartDate, oITransactionGRN.EndDate, oITransactionGRN.BUID, oITransactionGRN.GRNTypeint, oITransactionGRN.ProductUsageInt, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oITransactionGRNs = new List<ITransactionGRN>();
            }
            var jsonResult = Json(_oITransactionGRNs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region PrintXL
        public void Print_ReportXL(string sSTemp)
        {
            _oITransactionGRNs = new List<ITransactionGRN>();

            ITransactionGRN oITransactionGRN = new ITransactionGRN();
            oITransactionGRN.BUID = Convert.ToInt32(sSTemp.Split('~')[0]);
            oITransactionGRN.StartDate = Convert.ToDateTime(sSTemp.Split('~')[1]);
            oITransactionGRN.EndDate = Convert.ToDateTime(sSTemp.Split('~')[2]);
            oITransactionGRN.GRNTypeint = Convert.ToInt32(sSTemp.Split('~')[3]);
            oITransactionGRN.ProductUsageInt = Convert.ToInt32(sSTemp.Split('~')[3]);
            _oITransactionGRNs = ITransactionGRN.Gets(oITransactionGRN.StartDate, oITransactionGRN.EndDate, oITransactionGRN.BUID, oITransactionGRN.GRNTypeint, oITransactionGRN.ProductUsageInt, (int)Session[SessionInfo.currentUserID]);
            
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (oITransactionGRN.BUID > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(oITransactionGRN.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                if (oCompanys.Count > 0)
                {
                    oBusinessUnit.Name = oCompanys[0].Name;
                    oBusinessUnit.Address = oCompanys[0].Address;
                }
            }

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nStartRow = 1, nEndRow = 0, nStartCol = 1, nEndCol = 12;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("GRN Receive");
                sheet.Name = "GRN Receive Sheet";

                sheet.Column(2).Width = 25;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 22;
                sheet.Column(5).Width = 15;
                sheet.Column(6).Width = 22;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 12;
                sheet.Column(12).Width = 20;
                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;

                if (oITransactionGRN.BUID > 0)
                {
                    cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                }
                else
                {
                    if (oCompanys.Count > 0)
                    {
                        cell.Value = oCompanys[0].PringReportHead; cell.Style.Font.Bold = false;
                    }
                }
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "GRN Received "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Date:" + oITransactionGRN.StartDate.ToString("dd MMM yyyy") + " to " + oITransactionGRN.EndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Invoice Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Contractor Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "GRN No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Stock In Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nRowIndex = nRowIndex + 1;

                double nQty =0;
                #endregion

                            
                #region Data
                foreach (ITransactionGRN oItem in _oITransactionGRNs)
                {

                    nSL++;

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.StartDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.GRNTypeSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.GRNNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                           
                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.UnitName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nQty = nQty + oItem.Qty;
                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion
                #region Total
                //_oITransactionGRNs=_oITransactionGRNs
                _oITransactionGRNs = _oITransactionGRNs.GroupBy(x => new { x.UnitName }, (key, grp) =>
                           new ITransactionGRN
                           {
                               UnitName = key.UnitName,
                               Qty = grp.Sum(p => p.Qty)
                           }).ToList();
                nSL = 0;
                foreach (ITransactionGRN oItem in _oITransactionGRNs)
                {
                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + ""; cell.Style.Font.Bold = false;
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

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
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
                    if (nSL == 1)
                    {
                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    }
                    else
                    {
                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    }
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.Qty; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.UnitName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = nRowIndex;
                    nRowIndex++;
                    nRowIndex = nRowIndex + 1;
                }
                #endregion
                #endregion
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=GRN Receive.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();


            }
            #endregion
  



          

           





        }
        #endregion
     
    }
}
