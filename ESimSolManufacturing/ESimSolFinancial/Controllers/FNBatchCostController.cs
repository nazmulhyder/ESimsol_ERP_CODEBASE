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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSolFinancial.Controllers
{
    public class FNBatchCostController : Controller
    {
        string _sDateRange = "", _sDateText = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        FNBatchCost _oFNBatchCost = new FNBatchCost();
        List<FNBatchCost> _oFNBatchCosts = new List<FNBatchCost>();        

        #region Actions
        public ActionResult ViewFNBatchCosts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            FNBatchCost oFNBatchCost = new FNBatchCost();

            ViewBag.BUID = (int)EnumTextileUnit.Dyeing;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType)).Where(x => x.id>0);
          
            return View(_oFNBatchCosts);
        }
        public ActionResult ViewFNBatchCostDetails( int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            FNBatchCost oFNBatchCost = new FNBatchCost();

            ViewBag.BUID = (int)EnumTextileUnit.Dyeing; 
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType)).Where(x => x.id > 0);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
          
            return View(_oFNBatchCosts);
        }
        public ActionResult ViewFNBatchCost(int menuid)
        {
            FNBatchCost oFNBatchCost = new FNBatchCost();
            _oFNBatchCosts = new List<FNBatchCost>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment)).Where(x => x.id != (int)EnumFNTreatment.None);
            return View(_oFNBatchCosts);
        }
        [HttpPost]
        public JsonResult AdvSearch(FNBatchCost oFNBatchCost)
        {
            _oFNBatchCost = new FNBatchCost();
            List<FNBatchCost> oFNBatchCosts = new List<FNBatchCost>();
            //string sSQL = GetSQL(oFNBatchCost);

            string sSQL = "";
            int nCount = 0;
            _oFNBatchCost.DateType = Convert.ToInt16(oFNBatchCost.Params.Split('~')[nCount++]);
            _oFNBatchCost.StartDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);
            _oFNBatchCost.EndDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);

            if (_oFNBatchCost.DateType<=1 ||  _oFNBatchCost.StartDate == _oFNBatchCost.EndDate) { _oFNBatchCost.EndDate = _oFNBatchCost.StartDate.AddDays(1); }

            if (sSQL == "Error")
            {
                _oFNBatchCost = new FNBatchCost();
                _oFNBatchCost.ErrorMessage = "Please select a searching critaria.";
                oFNBatchCosts = new List<FNBatchCost>();
            }
            else
            {
                oFNBatchCosts = new List<FNBatchCost>();
                oFNBatchCosts = FNBatchCost.Gets(_oFNBatchCost.StartDate, _oFNBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFNBatchCosts.Count == 0)
                {
                    oFNBatchCosts = new List<FNBatchCost>();
                }
            }
            var jsonResult = Json(oFNBatchCosts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult AdvSearch_Detail(FNBatchCost oFNBatchCost)
        {
            _oFNBatchCost = new FNBatchCost();
            List<FNBatchCost> oFNBatchCosts = new List<FNBatchCost>();
            //string sSQL = GetSQL_Detail(oFNBatchCost,0);

            string sSQL = "";
            int nCount = 0;
            _oFNBatchCost.DateType = Convert.ToInt16(oFNBatchCost.Params.Split('~')[nCount++]);
            _oFNBatchCost.StartDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);
            _oFNBatchCost.EndDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);

            if (_oFNBatchCost.DateType <= 1 || _oFNBatchCost.StartDate == _oFNBatchCost.EndDate) { _oFNBatchCost.EndDate = _oFNBatchCost.StartDate.AddDays(1); }

            if (sSQL == "Error")
            {
                _oFNBatchCost = new FNBatchCost();
                _oFNBatchCost.ErrorMessage = "Please select a searching critaria.";
                oFNBatchCosts = new List<FNBatchCost>();
            }
            else
            {
                oFNBatchCosts = new List<FNBatchCost>();
                //oFNBatchCosts = FNBatchCost.GetsDetail(_oFNBatchCost.StartDate, _oFNBatchCost.EndDate, sSQL, 0, 2, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFNBatchCosts.Count == 0)
                {
                    oFNBatchCosts = new List<FNBatchCost>();
                }
            }
            var jsonResult = Json(oFNBatchCosts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
    
        #region Excel Support
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
           return FillCell( sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol++];
            if (IsNumber && !string.IsNullOrEmpty(sVal))
                cell.Value = Convert.ToDouble(sVal);
            else
                cell.Value = sVal;
            cell.Style.Font.Bold = IsBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            if (IsNumber)
            {
                cell.Style.Numberformat.Format = _sFormatter;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }
        
        private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        }
        private void FillCellMerge(ref ExcelWorksheet sheet,string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
        }
        private void FillCellMergeForNum(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Right);
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = isBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = HoriAlign;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }
        #endregion

        //#region Export To Excel
        //public void ExportToExcelFNBatchCost(string sParams, double ts) //YD Production Summary
        //{
        //    string Header = "";

        //    Company oCompany = new Company();
        //    FNBatchCost oFNBatchCost = new FNBatchCost();
        //    try
        //    {
        //        _sErrorMesage = "";

        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        _oFNBatchCosts = new List<FNBatchCost>();

        //        //if (!String.IsNullOrEmpty(sParams))
        //        //{
        //        //    oFNBatchCost.RSNo = sParams.Split('~')[0];
        //        //    oFNBatchCost.Params = sParams;
        //        //}
        //        string sSQL = this.GetSQL(oFNBatchCost);
        //        _oFNBatchCosts = FNBatchCost.Gets(_oFNBatchCost.StartDate, _oFNBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //        if (_oFNBatchCosts.Count <= 0)
        //        {
        //            _sErrorMesage = "There is no data for print!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFNBatchCosts = new List<FNBatchCost>();
        //        _sErrorMesage = ex.Message;
        //    }

        //    if (_sErrorMesage == "")
        //    {
        //        #region Header
        //        List<TableHeader> table_header = new List<TableHeader>();
        //        table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "M/C No.", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Buyer", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Customer Name", Width = 25f, IsRotate = false });

        //        table_header.Add(new TableHeader { Header = "Order No/Dispo", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });

        //        table_header.Add(new TableHeader { Header = "Quality", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Color Depth %", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Name of Shade", Width = 25f, IsRotate = false });

        //        table_header.Add(new TableHeader { Header = "Loading Time", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Unloading Time", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Dyeing Duration", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Batch Qty", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Re-production Qty", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Addition Qty Qty", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "No. of Addition", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Loading Capacity Qty", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Programe Short", Width = 20f, IsRotate = false });
        //        #endregion

        //        Header = "YD Production Summary";

        //        #region Export Excel
        //        int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
        //        ExcelRange cell; ExcelFill fill;
        //        OfficeOpenXml.Style.Border border;

        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            excelPackage.Workbook.Properties.Author = "ESimSol";
        //            excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //            var sheet = excelPackage.Workbook.Worksheets.Add("FNBatchCost");
        //            sheet.Name = "FNBatchCost_Summary";

        //            _sFormatter = " #,##0.0000;(#,##0.0000)";
        //            foreach (TableHeader listItem in table_header)
        //            {
        //                sheet.Column(nStartCol++).Width = listItem.Width;
        //            }

        //            #region Report Header
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
        //            cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
        //            cell.Value = Header; cell.Style.Font.Bold = true;
        //            cell.Style.WrapText = true;
        //            cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            nRowIndex = nRowIndex + 1;
        //            #endregion

        //            #region Address & Date
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
        //            cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
        //            cell.Value = ""; cell.Style.Font.Bold = false;
        //            cell.Style.WrapText = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            nRowIndex = nRowIndex + 1;
        //            #endregion

        //            #region Column Header
        //            nStartCol = 2;
        //            foreach (TableHeader listItem in table_header)
        //            {
        //                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            }
        //            #endregion

        //            int nCount = 0;
        //            string sCurrencySymbol = "";

        //            #region Data

        //            nRowIndex++;
        //            foreach (var oItem in _oFNBatchCosts)
        //            {
        //                nStartCol = 2;
        //                #region DATA
        //                FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                FillCellMerge(ref sheet, oItem.ProDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                FillCellMerge(ref sheet, oItem.Name, nRowIndex, nRowIndex, nStartCol, nStartCol++);

        //                //FillCellMerge(ref sheet, oItem.Buyer, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                //FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                //FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                //FillCellMerge(ref sheet, oItem.RSNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                //FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                //FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                //FillCell(sheet, nRowIndex, nStartCol++, oItem.ShadePertage.ToString(), true);

        //                //FillCellMerge(ref sheet, oItem.ShadeName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                //FillCellMerge(ref sheet, oItem.MLoadTimeSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                //FillCellMerge(ref sheet, oItem.MUnLoadTimeSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //                //FillCellMerge(ref sheet, oItem.DyeingTimeDuration, nRowIndex, nRowIndex, nStartCol, nStartCol++);

        //                ////	Loading Capacity	
        //                //FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Yarn.ToString(), true);
        //                //FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Finishid.ToString(), true);
        //                //FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty_Yarn - oItem.Qty_Finishid).ToString(), true);
        //                //FillCell(sheet, nRowIndex, nStartCol++, oItem.NumberOfAddition.ToString(), false); //No Of Addition
        //                FillCell(sheet, nRowIndex, nStartCol++, "", false); //Loading Capacity
        //                FillCell(sheet, nRowIndex, nStartCol++, "", false); //Programe Short
        //                #endregion
        //                nRowIndex++;
        //            }
        //            #region Grand Total
        //            nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
        //            FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.ShadePertage).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Qty_Yarn).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Qty_Finishid).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Qty_Yarn - x.Qty_Finishid).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
        //            nRowIndex++;
        //            #endregion

        //            cell = sheet.Cells[1, 1, nRowIndex, 12];
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.White);
        //            #endregion

        //            Response.ClearContent();
        //            Response.BinaryWrite(excelPackage.GetAsByteArray());
        //            Response.AddHeader("content-disposition", "attachment; filename=FNBatchCost(" + Header + ").xlsx");
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.Flush();
        //            Response.End();
        //        }
        //        #endregion
        //    }
        //}
        //public void Excel_BatchCostReport(string sParams, double ts) //Batch Costing Report
        //{
        //    string Header = "";

        //    Company oCompany = new Company();
        //    FNBatchCost oFNBatchCost = new FNBatchCost();
        //    try
        //    {
        //        _sErrorMesage = "";

        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        _oFNBatchCosts = new List<FNBatchCost>();

        //        //if (!String.IsNullOrEmpty(sParams))
        //        //{
        //        //    oFNBatchCost.RSNo = sParams.Split('~')[0];
        //        //    oFNBatchCost.Params = sParams;
        //        //}
        //        string sSQL = this.GetSQL(oFNBatchCost);
        //        _oFNBatchCosts = FNBatchCost.Gets(_oFNBatchCost.StartDate, _oFNBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //        if (_oFNBatchCosts.Count <= 0)
        //        {
        //            _sErrorMesage = "There is no data for print!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFNBatchCosts = new List<FNBatchCost>();
        //        _sErrorMesage = ex.Message;
        //    }

        //    if (_sErrorMesage == "")
        //    {
        //        #region Header
        //        List<TableHeader> table_header = new List<TableHeader>();
        //        table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "D", Width = 8f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "M", Width = 8f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Y", Width = 10f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "M/C No.", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Buyer", Width = 25f, IsRotate = false });

        //        table_header.Add(new TableHeader { Header = "Order No/Dispo", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Quality", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Color Depth %", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Name of Shade", Width = 25f, IsRotate = false });

        //        table_header.Add(new TableHeader { Header = "Dyeing Duration", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Batch Qty", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Re-production Qty", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Addition Qty Qty", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "No. of Addition", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Loading Capacity Qty", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Programe Short", Width = 20f, IsRotate = false });

        //        //Che Value 	 Dyes Value 	 Dyeing cost 	 Ch/Kg 	 Dy/Kg 	 Dyeing Cost/Kg 
        //        table_header.Add(new TableHeader { Header = "Che Value", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Dyes Value", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Dyeing cost", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = " Ch/Kg", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = " Dy/Kg", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = " Dyeing Cost/Kg", Width = 20f, IsRotate = false });

        //        #endregion

        //        Header = "Batch Costing Report";

        //        #region Export Excel
        //        int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
        //        ExcelRange cell; ExcelFill fill;
        //        OfficeOpenXml.Style.Border border;

        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            excelPackage.Workbook.Properties.Author = "ESimSol";
        //            excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //            var sheet = excelPackage.Workbook.Worksheets.Add("FNBatchCost");
        //            sheet.Name = "FNBatchCost_Report";

        //            foreach (TableHeader listItem in table_header)
        //            {
        //                sheet.Column(nStartCol++).Width = listItem.Width;
        //            }

        //            #region Report Header
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
        //            cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
        //            cell.Value = Header; cell.Style.Font.Bold = true;
        //            cell.Style.WrapText = true;
        //            cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            nRowIndex = nRowIndex + 1;
        //            #endregion

        //            #region Address & Date
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
        //            cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
        //            cell.Value = ""; cell.Style.Font.Bold = false;
        //            cell.Style.WrapText = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            nRowIndex = nRowIndex + 1;
        //            #endregion

        //            #region Column Header
        //            nStartCol = 2;
        //            foreach (TableHeader listItem in table_header)
        //            {
        //                cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            }
        //            #endregion

        //            int nCount = 0;
        //            string sCurrencySymbol = "";

        //            //#region Data
        //            //_sFormatter = " #,##0.0000;(#,##0.0000)";
        //            //nRowIndex++;
        //            //foreach (var oItem in _oFNBatchCosts)
        //            //{
        //            //    nStartCol = 2;
        //            //    #region DATA
        //            //    FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.RSDate.ToString("dd"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.RSDate.ToString("MM"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.RSDate.ToString("yy"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.RSDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex, nStartCol, nStartCol++);

        //            //    FillCellMerge(ref sheet, oItem.Buyer, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.RSNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCell(sheet, nRowIndex, nStartCol++, oItem.ShadePertage.ToString(), true);

        //            //    FillCellMerge(ref sheet, oItem.ShadeName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
        //            //    FillCellMerge(ref sheet, oItem.DyeingTimeDuration, nRowIndex, nRowIndex, nStartCol, nStartCol++);

        //            //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Yarn.ToString(), true);
        //            //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Finishid.ToString(), true);
        //            //    FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty_Yarn - oItem.Qty_Finishid).ToString(), true);
        //            //    FillCell(sheet, nRowIndex, nStartCol++, oItem.NumberOfAddition.ToString(), true); //No Of Addition
        //            //    FillCell(sheet, nRowIndex, nStartCol++, "", false); //Loading Capacity
        //            //    FillCell(sheet, nRowIndex, nStartCol++, "", false); //Programe Short

        //            //    //Che Value 	 Dyes Value 	 Dyeing cost 	 Ch/Kg 	 Dy/Kg 	 Dyeing Cost/Kg 
        //            //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Value_Chemical.ToString(), true);
        //            //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Value_Dyes.ToString(), true);
        //            //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Dyeing_Cost.ToString(), true);
        //            //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Chem_CostPerKG.ToString(), true);
        //            //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Dyes_CostPerKG.ToString(), true);
        //            //    FillCell(sheet, nRowIndex, nStartCol++, (oItem.Dyeing_CostPerKg +""), true);
        //            //    #endregion
        //            //    nRowIndex++;
        //            //}
        //            //#region Grand Total
        //            //nStartCol = 2; 
        //            //FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 10, true, ExcelHorizontalAlignment.Right);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.ShadePertage).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Qty_Yarn).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Qty_Finishid).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Qty_Yarn - x.Qty_Finishid).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Value_Chemical).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Value_Dyes).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Dyeing_Cost).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Chem_CostPerKG).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Dyes_CostPerKG).ToString(), true, true);
        //            //FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Dyeing_CostPerKg).ToString(), true, true);
        //            ////_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
        //            //nRowIndex++;
        //            //#endregion

        //            //cell = sheet.Cells[1, 1, nRowIndex, 12];
        //            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //            //fill.BackgroundColor.SetColor(Color.White);
        //            //#endregion

        //            Response.ClearContent();
        //            Response.BinaryWrite(excelPackage.GetAsByteArray());
        //            Response.AddHeader("content-disposition", "attachment; filename=FNBatchCost(" + Header + ").xlsx");
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.Flush();
        //            Response.End();
        //        }
        //        #endregion
        //    }
        //}

        //#endregion

        #region Export To Excel (Consumption)
        public void ExportToExcel_Details(string sParams, double ts)
        {
            string Header = "";
            int nReportType = 0;
            _oFNBatchCost = new FNBatchCost();
            Company oCompany = new Company();
            FNBatchCost oFNBatchCost = new FNBatchCost();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFNBatchCosts = new List<FNBatchCost>();

                _oFNBatchCost = GetSQL(sParams);
                nReportType = _oFNBatchCost.ReportType;
                if (_oFNBatchCost.DateType == 1) { _oFNBatchCost.EndDate = _oFNBatchCost.StartDate.AddDays(1); }

                _oFNBatchCosts = FNBatchCost.GetsDetail(_oFNBatchCost.Params, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oFNBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFNBatchCosts = new List<FNBatchCost>();
                _sErrorMesage = ex.Message;
            }
            if (nReportType == 1)
                Print_MachineWise(oCompany);
            else if (nReportType == 2)
                Print_BuyerWise(oCompany);
            else if (nReportType == 3)
                Print_MKTPWise(oCompany);
            else if (nReportType == 4)
                Print_ProcessWise(oCompany);
            else if (nReportType == 5)
                Print_PIWise(oCompany);
        }

        public ActionResult ExportToPdf_Details(string sParams, double ts)
        {
            string Header = "";
            int nReportType = 0;
            _oFNBatchCost = new FNBatchCost();
            Company oCompany = new Company();
            FNBatchCost oFNBatchCost = new FNBatchCost();
            _sErrorMesage = "";
            try
            {
                _oFNBatchCosts = new List<FNBatchCost>();
                _oFNBatchCost = GetSQL(sParams);
                nReportType = _oFNBatchCost.ReportType;
                if (_oFNBatchCost.DateType == 1) { _oFNBatchCost.EndDate = _oFNBatchCost.StartDate.AddDays(1); }
                _oFNBatchCosts = FNBatchCost.GetsDetail(_oFNBatchCost.Params, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFNBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFNBatchCosts = new List<FNBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_oFNBatchCosts.Count > 0)
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptFNBatchCostDetails oReport = new rptFNBatchCostDetails();
                byte[] abytes = oReport.PrepareReport(_oFNBatchCosts, oCompany, nReportType, _sDateText);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult CostingRptPdf(string sParams, double ts)
        {
            _oFNBatchCost = new FNBatchCost();
            Company oCompany = new Company();
            FNBatchCost oFNBatchCost = new FNBatchCost();
            try
            {
                _sErrorMesage = "";
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFNBatchCosts = new List<FNBatchCost>();
                _oFNBatchCost = GetSQL(sParams);
                if (_oFNBatchCost.DateType == 1) { _oFNBatchCost.EndDate = _oFNBatchCost.StartDate.AddDays(1); }
                _oFNBatchCosts = FNBatchCost.GetsDetail(_oFNBatchCost.Params, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFNBatchCosts = new List<FNBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_oFNBatchCosts.Count > 0)
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptCostingRpt oReport = new rptCostingRpt();
                byte[] abytes = oReport.PrepareReport(_oFNBatchCosts, oCompany, _sDateText);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }

        public void CostingRptExcel(string sParams, double ts)
        {
            _oFNBatchCost = new FNBatchCost();
            Company oCompany = new Company();
            FNBatchCost oFNBatchCost = new FNBatchCost();
            try
            {
                _sErrorMesage = "";
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFNBatchCosts = new List<FNBatchCost>();
                _oFNBatchCost = GetSQL(sParams);
                if (_oFNBatchCost.DateType == 1) { _oFNBatchCost.EndDate = _oFNBatchCost.StartDate.AddDays(1); }
                _oFNBatchCosts = FNBatchCost.GetsDetail(_oFNBatchCost.Params, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFNBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFNBatchCosts = new List<FNBatchCost>();
                _sErrorMesage = ex.Message;
            }
            PrintCostingRptExcel(oCompany);
        }


        private FNBatchCost GetSQL(string sParams)
        {
            string sPINo = "", sDispoNo = "", sProcessIDs = "", sMachineIDs = "", sBuyerIDs = "", sPONo = "";
            int nTreatment = 0, nReprocess = -1;
            FNBatchCost oFNBatchCost = new FNBatchCost();
            oFNBatchCost.Params = sParams;
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                oFNBatchCost.DateType = Convert.ToInt16(sParams.Split('~')[nCount++]);
                oFNBatchCost.StartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                oFNBatchCost.EndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);

                sPINo = sParams.Split('~')[nCount++];
                sDispoNo = sParams.Split('~')[nCount++];
                nTreatment = Convert.ToInt16(sParams.Split('~')[nCount++]);
                sProcessIDs = sParams.Split('~')[nCount++];
                sMachineIDs = sParams.Split('~')[nCount++];

                oFNBatchCost.ReportType = Convert.ToInt16(sParams.Split('~')[nCount++]);
                sBuyerIDs = sParams.Split('~')[nCount++];
                sPONo = sParams.Split('~')[nCount++];
                if(sParams.Split('~').Length > nCount)
                    nReprocess = Convert.ToInt16(sParams.Split('~')[nCount++]);
            }

            string sReturn1 = "SELECT	FNPC.FNPBatchID,	FNPC.ProductID,	AVG(Lot.UnitPrice),  SUM(FNPC.Qty),  SUM(FNPC.Qty * Lot.UnitPrice),   MAX(FNPC.MUID), MAX(FNPC.LotID) FROM FNProductionConsumption AS FNPC INNER JOIN   Lot ON Lot.LotID = FNPC.LotID ";
            string sReturn = "";

            #region PINo
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FNPC.FNPBatchID IN (SELECT FNPBatchID FROM View_FNProductionBatch where FNExOID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractDetailID in (Select ExportPIDetail.OrderSheetDetailID from ExportPIDetail where ExportPIID in (Select ExportPIID from ExportPI where PINo like  '%" + sPINo + "%'))))";
            }
            #endregion
            #region DispoNo
            if (!string.IsNullOrEmpty(sDispoNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM View_FNProductionBatch WHERE BatchNo like  '%" + sDispoNo + "%' )";
    
            }
            #endregion
            #region PONo
            if (!string.IsNullOrEmpty(sPONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FNPC.FNPBatchID IN (SELECT FNPBatchID FROM View_FNProductionBatch where FNExOID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractID in (SELECT FabricSalesContractID FROM FabricSalesContract WHERE SCNo LIKE '%" + sPONo + "%')))";
            }
            #endregion

            #region Treatment
            //if (nTreatment > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " PINo LIKE '%" + sPINo + "%' ";
            //}
            #endregion
            #region Process
            if (!string.IsNullOrEmpty(sProcessIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE FNProductionID IN (SELECT FNProductionID FROM FNProduction WHERE FNTPID IN (" + sProcessIDs + ")))";
            }
            #endregion
            #region MachineIDs
            if (!string.IsNullOrEmpty(sMachineIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE FNProductionID IN (SELECT FNProductionID FROM FNProduction WHERE FNMachineID IN (" + sMachineIDs + ")))";
            }
            #endregion

            #region Buyers
            //if (!string.IsNullOrEmpty(sBuyerIDs))
            //{
            //    Global.TagSQL(ref sReturn);
            //    //sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE FNProductionID IN (SELECT FNProductionID FROM FNProduction WHERE FNMachineID IN (" + sMachineIDs + ")))";
            //}
            #endregion

            #region  Date
            if (oFNBatchCost.DateType != (int)EnumCompareOperator.None)
            {
                if (oFNBatchCost.DateType == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDateTime,106)) =CONVERT(DATE, CONVERT(VARCHAR(12), '" + oFNBatchCost.StartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    _sDateText = "Date: " + oFNBatchCost.StartDate.ToString("dd MMM yyyy");
                }
                else if (oFNBatchCost.DateType == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDateTime,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + oFNBatchCost.StartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + oFNBatchCost.EndDate.ToString("dd MMM yyyy") + "', 106))  )";
                    _sDateText = "Date: " + oFNBatchCost.StartDate.ToString("dd MMM yyyy") + " - To - " + oFNBatchCost.EndDate.ToString("dd MMM yyyy");
                }
                
            }
            #endregion

            #region Reprocess
            if (nReprocess > -1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE ISNULL(IsProduction,0) = "+nReprocess+")";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            sReturn += " GROUP BY FNPBatchID, FNPC.ProductID";
            oFNBatchCost.Params = sReturn;

            return oFNBatchCost;
        }

        private void Print_MachineWise(Company oCompany) 
        {
            string Header="";
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 15f, IsRotate = false });                
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Production Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw Material", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount Cost", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Avg Cost(Yds)", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oFNBatchCosts.GroupBy(x => new { x.FNTreatment, x.MachineID, x.MachineName, x.FNCode }, (key, grp) => new
                {
                    FNTreatment = key.FNTreatment, //unique dt
                    HeaderName = key.MachineName, //unique dt
                    FNCode = key.FNCode, //unique dt
                    Results = grp.ToList() //All data
                });
                dataGrpList = dataGrpList.OrderBy(x => x.FNTreatment).ThenBy(x => x.FNCode).ToList();
                Header = "Machine Wise Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FNBatchCost");
                    sheet.Name = "FNBatchCost_MachineWise";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = _sDateText; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    string sCurrencySymbol = "";

                    #region Data

                    nRowIndex++;

                    double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
                    double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;
                    double nGrandTtlQty_Production = 0;
                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                        double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                        double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                        nSubTotal_Qty_Order = 0;
                        nSubTotal_Qty_Production = 0;
                        string sPreviousPINo = "~~", sPreviousSCNo = "~~~", sFNProcess = "~~~";
                        double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                        {
                            #region Batch Wise Total
                            if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                            {
                                #region Total
                                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                                nTtlRate = (nTtlAmount / nTtlQty);
                                nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount/nTtlQty_Production).ToString(), true, true);

                                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                                //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                                sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                                nRowIndex++;
                                #endregion
                            }
                            #endregion

                            #region DATA
                            nStartCol = 2;
                            if (sPreviousPINo != oItem.PINo)
                            {
                                int rowCountForPI = (oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID)-1);
                                if (rowCountForPI < 0) rowCountForPI = 0;
                                FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.PINo, nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);
                            }
                            else
                            {
                                nStartCol += 2;
                            }

                            if (sPreviousSCNo != oItem.SCNo)
                            {
                                int rowCountForSCNo = (oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCountForSCNo < 0) rowCountForSCNo = 0;
                                FillCellMerge(ref sheet, oItem.SCNo, nRowIndex, nRowIndex + rowCountForSCNo, nStartCol, nStartCol++);

                                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex + rowCountForSCNo, nStartCol++]; cell.Merge = true; cell.Value = oItem.SCNo; cell.Style.Font.Bold = false;
                                //cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            else
                            {
                                nStartCol += 1;
                            }

                            if (nPreviousBatchID != oItem.FNBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID)-1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.FNBatchNo, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Order.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Color, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                sFNProcess = "~~~~~~";
                            }
                            else
                            {
                                nStartCol += 5;
                            }

                            if (nPreviousFNPBatchID != oItem.FNPBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID)-1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.ProDate.ToString("dd MMM yy"), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Production.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);

                                nTtlQty_Production += oItem.Qty_Production;
                                nSubTtlQty_Production += oItem.Qty_Production;
                                nGrandTtlQty_Production += oItem.Qty_Production;
                            }
                            else
                            {
                                nStartCol += 2;
                            }

                            if (sFNProcess != oItem.FNProcess && nPreviousBatchID != oItem.FNBatchID)
                            {
                                int rowCountForProcess = (oDataGrp.Results.Count(x => x.FNProcess == oItem.FNProcess && x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCountForProcess < 0) rowCountForProcess = 0;
                                FillCellMerge(ref sheet, oItem.FNProcess, nRowIndex, nRowIndex + rowCountForProcess, nStartCol, nStartCol++);
                            }
                            else nStartCol += 1;
                            //FillCellMerge(ref sheet, oItem.FNTreatment_Process, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                            _sFormatter = " #,##0.00;(#,##0.00)";
                            
                            FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.IsProductionSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value).ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value / oItem.Qty_Production).ToString(), true);
                            
                            #endregion
                            nRowIndex++;
                            nPreviousBatchID = oItem.FNBatchID;
                            sPreviousPINo = oItem.PINo;
                            sPreviousSCNo = oItem.SCNo;
                            nPreviousFNPBatchID = oItem.FNPBatchID;
                            sFNProcess = oItem.FNProcess;
                        }
                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount/nTtlQty_Production).ToString(), true, true);

                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion

                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                        nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                        nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                        #region Sub Total
                        nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                        nSubTtlAmount=oDataGrp.Results.Sum(x => x.Value);
                        nSubTtlRate = (nSubTtlAmount/nSubTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Order.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nSubTtlAmount/nSubTtlQty_Production).ToString(), true, true);

                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        nSubTtlQty_Production = 0;
                        #endregion
                    }

                    #region Grand Total
                    nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
                    nGrandTtlAmount=_oFNBatchCosts.Sum(x => x.Value);
                    nGrandTtlRate=(nGrandTtlAmount/nGrandTtlQty);
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Order.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlRate.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlAmount.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, (nGrandTtlAmount/nGrandTtlQty_Production).ToString(), true, true);

                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FNBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void Print_BuyerWise(Company oCompany)
        {
            string Header = "";
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 15f, IsRotate = false });                
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Machine Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Production Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw Material", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount Cost", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Avg Cost(Yds)", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oFNBatchCosts.GroupBy(x => new { x.BuyerID, x.BuyerName }, (key, grp) => new
                {
                    HeaderName = key.BuyerName, //unique dt
                    Results = grp.ToList().OrderBy(z=>z.FNCode)
                });
                Header = "Buyer Wise Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FNBatchCost");
                    sheet.Name = "FNBatchCost_MachineWise";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = _sDateText; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    string sCurrencySymbol = "";

                    #region Data

                    nRowIndex++;

                    double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
                    double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;
                    double nGrandTtlQty_Production = 0;
                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                        double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                        double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                        nSubTotal_Qty_Order = 0;
                        nSubTotal_Qty_Production = 0;
                        string sPreviousPINo = "~~", sPreviousSCNo = "~~~";
                        double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);
                        //foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNCode).ThenBy(y => y.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                        {
                            #region Batch Wise Total
                            if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                            {
                                #region Total
                                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                                nTtlRate = (nTtlAmount / nTtlQty);
                                nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount/nTtlQty_Production).ToString(), true, true);

                                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                                //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                                sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                                nRowIndex++;
                                #endregion
                            }
                            #endregion

                            #region DATA
                            nStartCol = 2;
                            if (sPreviousPINo != oItem.PINo)
                            {
                                int rowCountForPI = (oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCountForPI < 0) rowCountForPI = 0;
                                FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.PINo, nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);

                            }
                            else
                            {
                                nStartCol += 2;
                            }

                            if (sPreviousSCNo != oItem.SCNo)
                            {
                                int rowCountForSCNo = (oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCountForSCNo < 0) rowCountForSCNo = 0;
                                FillCellMerge(ref sheet, oItem.SCNo, nRowIndex, nRowIndex + rowCountForSCNo, nStartCol, nStartCol++);

                                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex + rowCountForSCNo, nStartCol++]; cell.Merge = true; cell.Value = oItem.SCNo; cell.Style.Font.Bold = false;
                                //cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            }
                            else
                            {
                                nStartCol += 1;
                            }

                            if (nPreviousBatchID != oItem.FNBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.FNBatchNo, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Order.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Color, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);

                            }
                            else
                            {
                                nStartCol += 5;
                            }

                            if (nPreviousFNPBatchID != oItem.FNPBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID) - 1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.ProDate.ToString("dd MMM yy"), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Production.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);

                                nTtlQty_Production += oItem.Qty_Production;
                                nSubTtlQty_Production += oItem.Qty_Production;
                                nGrandTtlQty_Production += oItem.Qty_Production;
                            }
                            else
                            {
                                nStartCol += 2;
                            }
                            _sFormatter = " #,##0.00;(#,##0.00)";
                            FillCellMerge(ref sheet, oItem.FNProcess, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.IsProductionSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value).ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value / oItem.Qty_Production).ToString(), true);
                            
                            #endregion
                            nRowIndex++;
                            nPreviousBatchID = oItem.FNBatchID;
                            sPreviousPINo = oItem.PINo;
                            sPreviousSCNo = oItem.SCNo;
                            nPreviousFNPBatchID = oItem.FNPBatchID;
                        }
                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount/nTtlQty_Production).ToString(), true, true);


                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion

                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                        nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                        nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                        #region Sub Total
                        nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                        nSubTtlAmount = oDataGrp.Results.Sum(x => x.Value);
                        nSubTtlRate = (nSubTtlAmount / nSubTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Order.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nSubTtlAmount/nSubTtlQty_Production).ToString(), true, true);

                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        nSubTtlQty_Production = 0;
                        #endregion
                    }

                    #region Grand Total
                    nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
                    nGrandTtlAmount = _oFNBatchCosts.Sum(x => x.Value);
                    nGrandTtlRate = (nGrandTtlAmount / nGrandTtlQty);
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Order.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlRate.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlAmount.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, (nGrandTtlAmount/nGrandTtlQty_Production).ToString(), true, true);

                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FNBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void Print_MKTPWise(Company oCompany)
        {
            string Header = "";
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 15f, IsRotate = false });                
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Supplier Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Production Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw Material", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount Cost", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Avg Cost(Yds)", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oFNBatchCosts.GroupBy(x => new { x.MktAccountID, x.MktName }, (key, grp) => new
                {
                    HeaderName = key.MktName, //unique dt
                    Results = grp.ToList().OrderBy(z => z.FNCode) //All data
                });
                Header = "Supplier Wise Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FNBatchCost");
                    sheet.Name = "FNBatchCost_MachineWise";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = _sDateText; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    string sCurrencySymbol = "";

                    #region Data

                    nRowIndex++;

                    double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
                    double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;
                    double nGrandTtlQty_Production = 0;
                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                        double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                        double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                        nSubTotal_Qty_Order = 0;
                        nSubTotal_Qty_Production = 0;
                        string sPreviousPINo = "~~", sPreviousSCNo = "~~~";
                        double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                        {
                            #region Batch Wise Total
                            if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                            {
                                #region Total
                                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                                nTtlRate = (nTtlAmount / nTtlQty);
                                nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount/nTtlQty_Production).ToString(), true, true);

                                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                                //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                                sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                                nRowIndex++;
                                #endregion
                            }
                            #endregion

                            #region DATA
                            nStartCol = 2;
                            if (sPreviousPINo != oItem.PINo)
                            {
                                int rowCountForPI = (oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCountForPI < 0) rowCountForPI = 0;
                                FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.PINo, nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);

                            }
                            else
                            {
                                nStartCol += 2;
                            }

                            if (sPreviousSCNo != oItem.SCNo)
                            {
                                int rowCountForSCNo = (oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCountForSCNo < 0) rowCountForSCNo = 0;
                                FillCellMerge(ref sheet, oItem.SCNo, nRowIndex, nRowIndex + rowCountForSCNo, nStartCol, nStartCol++);

                                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex + rowCountForSCNo, nStartCol++]; cell.Merge = true; cell.Value = oItem.SCNo; cell.Style.Font.Bold = false;
                                //cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            }
                            else
                            {
                                nStartCol += 1;
                            }

                            if (nPreviousBatchID != oItem.FNBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.FNBatchNo, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Order.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Color, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);

                            }
                            else
                            {
                                nStartCol += 5;
                            }

                            if (nPreviousFNPBatchID != oItem.FNPBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID) - 1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.ProDate.ToString("dd MMM yy"), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Production.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);

                                nTtlQty_Production += oItem.Qty_Production;
                                nSubTtlQty_Production += oItem.Qty_Production;
                                nGrandTtlQty_Production += oItem.Qty_Production;
                            }
                            else
                            {
                                nStartCol += 2;
                            }
                            _sFormatter = " #,##0.00;(#,##0.00)";
                            FillCellMerge(ref sheet, oItem.FNTreatment_Process, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.IsProductionSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value).ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value / oItem.Qty_Production).ToString(), true);

                            #endregion
                            nRowIndex++;
                            nPreviousBatchID = oItem.FNBatchID;
                            sPreviousPINo = oItem.PINo;
                            sPreviousSCNo = oItem.SCNo;
                            nPreviousFNPBatchID = oItem.FNPBatchID;
                        }
                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount/nTtlQty_Production).ToString(), true, true);

                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion

                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                        nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                        nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                        #region Sub Total
                        nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                        nSubTtlAmount = oDataGrp.Results.Sum(x => x.Value);
                        nSubTtlRate = (nSubTtlAmount / nSubTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Order.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nSubTtlAmount/nSubTtlQty_Production).ToString(), true, true);

                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        nSubTtlQty_Production = 0;
                        #endregion
                    }

                    #region Grand Total
                    nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
                    nGrandTtlAmount = _oFNBatchCosts.Sum(x => x.Value);
                    nGrandTtlRate = (nGrandTtlAmount / nGrandTtlQty);
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Order.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlRate.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlAmount.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, (nGrandTtlAmount/nGrandTtlQty_Production).ToString(), true, true);

                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FNBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void Print_ProcessWise(Company oCompany)
        {
            string Header = "";
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });                
                table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 15f, IsRotate = false });                
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Production Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Machine Name", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw Material", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount Cost", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Avg Cost(Yds)", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oFNBatchCosts.OrderBy(z=>z.FNTreatment).GroupBy(x => new { x.FNTreatment, x.FNTreatmentSt }, (key, grp) => new
                {
                    HeaderName = key.FNTreatmentSt, //unique dt
                    Results = grp.ToList().OrderBy(z => z.FNCode) //All data
                });
                Header = "Process Wise Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FNBatchCost");
                    sheet.Name = "FNBatchCost_MachineWise";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = _sDateText; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    string sCurrencySymbol = "";

                    #region Data

                    nRowIndex++;

                    double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
                    double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;
                    double nGrandTtlQty_Production = 0;
                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                        double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                        double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                        nSubTotal_Qty_Order = 0;
                        nSubTotal_Qty_Production = 0;
                        string sPreviousPINo = "~~", sPreviousSCNo = "~~~";
                        double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                        {
                            #region Batch Wise Total
                            if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                            {
                                #region Total
                                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                                nTtlRate = (nTtlAmount / nTtlQty);
                                nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount/nTtlQty_Production).ToString(), true, true);

                                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                                //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                                sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                                nRowIndex++;
                                #endregion
                            }
                            #endregion

                            #region DATA
                            nStartCol = 2;
                            if (sPreviousPINo != oItem.PINo)
                            {
                                int rowCountForPI = (oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCountForPI < 0) rowCountForPI = 0;
                                FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.PINo, nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);

                            }
                            else
                            {
                                nStartCol += 2;
                            }

                            if (sPreviousSCNo != oItem.SCNo)
                            {
                                int rowCountForSCNo = (oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCountForSCNo < 0) rowCountForSCNo = 0;
                                FillCellMerge(ref sheet, oItem.SCNo, nRowIndex, nRowIndex + rowCountForSCNo, nStartCol, nStartCol++);

                                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex + rowCountForSCNo, nStartCol++]; cell.Merge = true; cell.Value = oItem.SCNo; cell.Style.Font.Bold = false;
                                //cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            }
                            else
                            {
                                nStartCol += 1;
                            }

                            if (nPreviousBatchID != oItem.FNBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.FNBatchNo, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Order.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Color, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);

                            }
                            else
                            {
                                nStartCol += 5;
                            }

                            if (nPreviousFNPBatchID != oItem.FNPBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID) - 1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.ProDate.ToString("dd MMM yy"), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Production.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);

                                nTtlQty_Production += oItem.Qty_Production;
                                nSubTtlQty_Production += oItem.Qty_Production;
                                nGrandTtlQty_Production += oItem.Qty_Production;
                            }
                            else
                            {
                                nStartCol += 2;
                            }
                            _sFormatter = " #,##0.00;(#,##0.00)";
                            FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.IsProductionSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value).ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value / oItem.Qty_Production).ToString(), true);

                            #endregion
                            nRowIndex++;
                            nPreviousBatchID = oItem.FNBatchID;
                            sPreviousPINo = oItem.PINo;
                            sPreviousSCNo = oItem.SCNo;
                            nPreviousFNPBatchID = oItem.FNPBatchID;
                        }
                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount/nTtlQty_Production).ToString(), true, true);

                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion

                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                        nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                        nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                        #region Sub Total
                        nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                        nSubTtlAmount = oDataGrp.Results.Sum(x => x.Value);
                        nSubTtlRate = (nSubTtlAmount / nSubTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Order.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nSubTtlAmount/nSubTtlQty_Production).ToString(), true, true);

                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        nSubTtlQty_Production = 0;
                        #endregion
                    }

                    #region Grand Total
                    nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
                    nGrandTtlAmount = _oFNBatchCosts.Sum(x => x.Value);
                    nGrandTtlRate = (nGrandTtlAmount / nGrandTtlQty);
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Order.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlRate.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlAmount.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, (nGrandTtlAmount/nGrandTtlQty_Production).ToString(), true, true);

                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FNBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        private void Print_PIWise(Company oCompany)
        {
            string Header = "";
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });                
                table_header.Add(new TableHeader { Header = "PO No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Supplier Name", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "MKT Name", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Production Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw Material", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount Cost", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Avg Cost(Yds)", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oFNBatchCosts.GroupBy(x => new { x.PIID, x.PINo }, (key, grp) => new
                {
                    HeaderName = key.PINo, //unique dt
                    Results = grp.ToList().OrderBy(z => z.FNCode) //All data
                });
                Header = "PI Wise Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FNBatchCost");
                    sheet.Name = "FNBatchCost_MachineWise";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = _sDateText; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    string sCurrencySymbol = "";

                    #region Data

                    nRowIndex++;

                    double nSubTotal_Qty_Order = 0,
                           nSubTotal_Qty_Production = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Production = 0;
                    double nGrandTtlQty = 0, nGrandTtlAmount = 0, nGrandTtlRate = 0;
                    double nGrandTtlQty_Production = 0;
                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0, nPreviousBatchID = 0, nPreviousFNPBatchID = 0;
                        double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0;
                        double nSubTtlQty = 0, nSubTtlAmount = 0, nSubTtlRate = 0;
                        nSubTotal_Qty_Order = 0;
                        nSubTotal_Qty_Production = 0;
                        string sPreviousPINo = "~~", sPreviousSCNo = "~~~";
                        double nTtlQty_Production = 0, nSubTtlQty_Production = 0;
                        FillCellMerge(ref sheet, "PI No: " + oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNBatchID).ThenBy(y => y.PINo).ThenBy(z => z.SCNo))
                        {
                            #region Batch Wise Total
                            if (nPreviousBatchID != oItem.FNBatchID && nCount > 0)
                            {
                                #region Total
                                nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                                nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                                nTtlRate = (nTtlAmount / nTtlQty);
                                nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount / nTtlQty_Production).ToString(), true, true);

                                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                                nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();
                                //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                                sPreviousPINo = "~~"; sPreviousSCNo = "~~~"; nTtlQty_Production = 0;
                                nRowIndex++;
                                #endregion
                            }
                            #endregion

                            #region DATA
                            nStartCol = 2;
                            //if (sPreviousPINo != oItem.PINo)
                            //{
                            //    int rowCountForPI = (oDataGrp.Results.Count(x => x.PINo == oItem.PINo && x.FNBatchID == oItem.FNBatchID) - 1);
                            //    if (rowCountForPI < 0) rowCountForPI = 0;
                            //    FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);
                            //    FillCellMerge(ref sheet, oItem.PINo, nRowIndex, nRowIndex + rowCountForPI, nStartCol, nStartCol++);

                            //}
                            //else
                            //{
                            //    nStartCol += 2;
                            //}

                            if (sPreviousSCNo != oItem.SCNo)
                            {
                                int rowCountForSCNo = (oDataGrp.Results.Count(x => x.SCNo == oItem.SCNo && x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCountForSCNo < 0) rowCountForSCNo = 0;
                                FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + rowCountForSCNo, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.SCNo, nRowIndex, nRowIndex + rowCountForSCNo, nStartCol, nStartCol++);

                            }
                            else
                            {
                                nStartCol += 2;
                            }

                            if (nPreviousBatchID != oItem.FNBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNBatchID == oItem.FNBatchID) - 1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.FNBatchNo, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Order.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.MktName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, oItem.Color, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);

                            }
                            else
                            {
                                nStartCol += 6;
                            }

                            if (nPreviousFNPBatchID != oItem.FNPBatchID)
                            {
                                int rowCount = (oDataGrp.Results.Count(x => x.FNPBatchID == oItem.FNPBatchID) - 1);
                                if (rowCount < 0) rowCount = 0;
                                FillCellMerge(ref sheet, oItem.ProDate.ToString("dd MMM yy"), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);
                                FillCellMergeForNum(ref sheet, oItem.Qty_Production.ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++);

                                nTtlQty_Production += oItem.Qty_Production;
                                nSubTtlQty_Production += oItem.Qty_Production;
                                nGrandTtlQty_Production += oItem.Qty_Production;
                            }
                            else
                            {
                                nStartCol += 2;
                            }
                            _sFormatter = " #,##0.00;(#,##0.00)";
                            FillCellMerge(ref sheet, oItem.FNTreatment_Process, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.IsProductionSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value).ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Value / oItem.Qty_Production).ToString(), true);

                            #endregion
                            nRowIndex++;
                            nPreviousBatchID = oItem.FNBatchID;
                            sPreviousPINo = oItem.PINo;
                            sPreviousSCNo = oItem.SCNo;
                            nPreviousFNPBatchID = oItem.FNPBatchID;
                        }
                        #region Total
                        nTtlQty = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Qty);
                        nTtlAmount = oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Sum(x => x.Value);
                        nTtlRate = (nTtlAmount / nTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount / nTtlQty_Production).ToString(), true, true);

                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion

                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Production += oDataGrp.Results.Where(x => x.FNBatchID == nPreviousBatchID).Select(x => x.Qty_Production).FirstOrDefault();

                        nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                        nGrndTotal_Qty_Production += nSubTotal_Qty_Production;

                        #region Sub Total
                        nSubTtlQty = oDataGrp.Results.Sum(x => x.Qty);
                        nSubTtlAmount = oDataGrp.Results.Sum(x => x.Value);
                        nSubTtlRate = (nSubTtlAmount / nSubTtlQty);
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Order.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        //FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty_Production.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlRate.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTtlAmount.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nSubTtlAmount / nSubTtlQty_Production).ToString(), true, true);

                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        nSubTtlQty_Production = 0;
                        #endregion
                    }

                    #region Grand Total
                    nGrandTtlQty = _oFNBatchCosts.Sum(x => x.Qty);
                    nGrandTtlAmount = _oFNBatchCosts.Sum(x => x.Value);
                    nGrandTtlRate = (nGrandTtlAmount / nGrandTtlQty);
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Order.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty_Production.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlRate.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTtlAmount.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, (nGrandTtlAmount / nGrandTtlQty_Production).ToString(), true, true);

                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FNBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        private void PrintCostingRptExcel(Company oCompany)
        {
            string Header = "";
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order Qty", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 22f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 18f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo Qty", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Machine Name", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Production Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw material", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount Cost", Width = 15f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "Avg Cost(Yds)", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oFNBatchCosts.OrderBy(z => z.FNTreatment).ThenBy(y=>y.FNCode).ThenBy(y=>y.FNBatchID).GroupBy(x => new { x.FNTreatment, x.FNTreatmentSt }, (key, grp) => new
                {
                    HeaderName = key.FNTreatmentSt, //unique dt
                    Results = grp.ToList() //All data
                });
                Header = "Process Wise Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FNBatchCost");
                    sheet.Name = "FNBatchCost_MachineWise";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = _sDateText; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

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
                    double nGrandTotalDispoQty = 0;
                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        double nSubTotalDispoQty = 0;
                        double nTtlQty = 0, nTtlAmount = 0, nTtlRate = 0, nTotalDispoQty = 0;
                        string sDispoNo = "~~~";
                        int nCount = 0;

                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FNCode).ThenBy(x => x.FNBatchNo))
                        {
                            #region Batch Wise Total
                            if (sDispoNo != oItem.FNBatchNo && nCount > 0)
                            {
                                #region Total
                                nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTotalDispoQty.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount/nTtlQty).ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);

                                nSubTotalDispoQty += nTotalDispoQty;
                                nGrandTotalDispoQty += nTotalDispoQty;
                                nTtlQty = 0; nTtlAmount = 0; nTtlRate = 0; nTotalDispoQty = 0;
                                nRowIndex++;
                                #endregion
                            }
                            #endregion

                            #region DATA
                            nStartCol = 1; _sFormatter = " #,##0.00;(#,##0.00)";
                            FillCell(sheet, nRowIndex, ++nStartCol, (++nCount).ToString(), false, false);
                            FillCellMerge(ref sheet, oItem.ProDate.ToString("dd MMM yy"), nRowIndex, nRowIndex, ++nStartCol, nStartCol);
                            FillCellMerge(ref sheet, oItem.SCNo, nRowIndex, nRowIndex, ++nStartCol, nStartCol);
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Qty_Order.ToString(), true, false);
                            FillCellMerge(ref sheet, oItem.PINo, nRowIndex, nRowIndex, ++nStartCol, nStartCol);
                            FillCellMerge(ref sheet, oItem.FNBatchNo, nRowIndex, nRowIndex, ++nStartCol, nStartCol);
                            FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex, ++nStartCol, nStartCol);
                            FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex, ++nStartCol, nStartCol);
                            FillCellMerge(ref sheet, oItem.Color, nRowIndex, nRowIndex, ++nStartCol, nStartCol);
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Qty_Batch.ToString(), true, false);
                            if (sDispoNo != oItem.FNBatchNo)
                                nTotalDispoQty += oItem.Qty_Batch;
                            FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex, ++nStartCol, nStartCol);
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Qty_Production.ToString(), true, false);
                            FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, ++nStartCol, nStartCol);
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Qty.ToString(), true, false);
                            nTtlQty += oItem.Qty;
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.UnitPrice.ToString(), true, false);
                            nTtlRate += oItem.UnitPrice;
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Value.ToString(), true, false);
                            nTtlAmount += oItem.Value;
                            #endregion

                            nRowIndex++;
                            sDispoNo = oItem.FNBatchNo;
                        }
                        #region Total
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTotalDispoQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (nTtlAmount / nTtlQty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTtlAmount.ToString(), true, true);

                        nSubTotalDispoQty += nTotalDispoQty;
                        nGrandTotalDispoQty += nTotalDispoQty;
                        nTtlQty = 0; nTtlAmount = 0; nTtlRate = 0; nTotalDispoQty = 0;
                        nRowIndex++;
                        #endregion


                        #region Sub Total
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotalDispoQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, (oDataGrp.Results.Sum(x => x.Value)/oDataGrp.Results.Sum(x => x.Qty)).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Sum(x => x.Value).ToString(), true, true);

                        nRowIndex++;
                        #endregion
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrandTotalDispoQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Qty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, (_oFNBatchCosts.Sum(x => x.Value)/_oFNBatchCosts.Sum(x => x.Qty)).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFNBatchCosts.Sum(x => x.Value).ToString(), true, true);

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FNBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        #region Export To Excel (MonthWise)
        public void ExportToExcel_MonthWise(string sParams, double ts)
        {
            string Header = ""; int nReportType = 0;
            _oFNBatchCost = new FNBatchCost();
            Company oCompany = new Company();
            FNBatchCost oFNBatchCost = new FNBatchCost();
            try
            {
                _sErrorMesage = "";
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFNBatchCosts = new List<FNBatchCost>();
                oFNBatchCost.Params = sParams;
                if (!String.IsNullOrEmpty(sParams))
                {
                    int nCount = 0;
                    _oFNBatchCost.DateType = Convert.ToInt16(oFNBatchCost.Params.Split('~')[nCount++]);
                    _oFNBatchCost.StartDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);
                    _oFNBatchCost.EndDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);
                    nReportType = Convert.ToInt16(oFNBatchCost.Params.Split('~')[nCount++]);
                }
                if (_oFNBatchCost.DateType == 1) { _oFNBatchCost.EndDate = _oFNBatchCost.StartDate.AddDays(1); }
                //_oFNBatchCosts = FNBatchCost.GetsDetail(_oFNBatchCost.StartDate, _oFNBatchCost.EndDate, "", 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFNBatchCosts = FNBatchCost.Gets(_oFNBatchCost.StartDate, _oFNBatchCost.EndDate, "", 0, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFNBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFNBatchCosts = new List<FNBatchCost>();
                _sErrorMesage = ex.Message;
            }
            if (_sErrorMesage == "")
            {
                var dataGrpList = _oFNBatchCosts.GroupBy(x => new { ProDateSt = x.ProDateSt }, (key, grp) => new
                {
                    HeaderName = grp.Select(x => x.ProDateSt).FirstOrDefault(), //unique dt
                    Results = grp.ToList().OrderBy(x => x.ID) //All data
                });
                var oMachineID_List = _oFNBatchCosts.OrderBy(x => x.ID).Select(x => x.ID).Distinct();
                //var oMachineList = _oFNBatchCosts.Select(x => new FNBatchCost() { ID = x.ID, Name = x.Name }).Distinct();
                Header = "Month Wise Costing Report";
                if (nReportType == 2)
                    Header = "Buyer Wise Costing Report";
                if (nReportType == 3)
                    Header = "MKT Person Wise Costing Report";
                if (nReportType == 4)
                    Header = "Process Wise Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = 0;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FNBatchCost");
                    sheet.Name = "FNBatchCost_Summary";


                    int nCount_Column = 2;
                    int nTotalMonths = dataGrpList.Count();
                    sheet.Column(nCount_Column++).Width = 15;
                    sheet.Column(nCount_Column++).Width = 30;
                    //sheet.Column(nCount++).Width = 50;

                    for (int i = 0; i <nTotalMonths; i++)
                    {
                        sheet.Column(nCount_Column++).Width = 20;
                        sheet.Column(nCount_Column++).Width = 20;
                        sheet.Column(nCount_Column++).Width = 20;
                    }

                    //sheet.Column(nCount_Column++).Width = 30;
                    sheet.Column(nCount_Column++).Width = 30;
                    sheet.Column(nCount_Column++).Width = 30;
                    sheet.Column(nCount_Column++).Width = 30;
                    nEndCol = nCount_Column;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol/2]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 16; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol/2 + 1, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol / 2]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol / 2 + 1, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nRowIndex++;

                    #region Column Header
                    nStartCol = 3;
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex + 1, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    string sVal = "Machine Name";
                    if (nReportType == 2)
                        sVal = "Buyer Name";
                    if (nReportType == 3)
                        sVal = "MKT Person";
                    if (nReportType == 3)
                        sVal = "Process Name";

                    cell = sheet.Cells[nRowIndex, 3, nRowIndex + 1, 3]; cell.Value = sVal; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    int nCol_Header = 4;
                    foreach(var oDataGrp in dataGrpList)
                    {
                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex, nCol_Header, nCol_Header + 2, true, ExcelHorizontalAlignment.Center);
                        FillCell(sheet, nRowIndex + 1, nCol_Header, "Prod. Qty", false);
                        FillCell(sheet, nRowIndex + 1, nCol_Header + 1, "Value", false);
                        FillCell(sheet, nRowIndex + 1, nCol_Header + 2, "P. YDS", false);
                        nCol_Header += 3;
                    }

                    FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, nCol_Header, nCol_Header + 2, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nRowIndex + 1, nCol_Header, "Prod. Qty", false);
                    FillCell(sheet, nRowIndex + 1, nCol_Header + 1, "Value", false);
                    FillCell(sheet, nRowIndex + 1, nCol_Header + 2, "P. YDS", false);

                    //cell = sheet.Cells[nRowIndex, nCol_Header, nRowIndex + 1, nCol_Header]; cell.Value = "Total"; cell.Style.Font.Bold = true; cell.Merge = true;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion

                    int nCount = 0; nRowIndex+=2;

                    #region Machine Wise Loop
                    foreach (var oMcahineID in oMachineID_List)
                    {
                        nStartCol = 2;
                        FillCell(sheet, nRowIndex, 2, (++nCount).ToString(), false);
                        FillCell(sheet, nRowIndex, 3, _oFNBatchCosts.Where(x => x.ID == oMcahineID).Select(x => x.Name).FirstOrDefault(), false);

                        #region Data List Wise Loop
                        int nCol_Data = 4;
                        foreach (var oDataGrp in dataGrpList)
                        {
                            #region DATA
                            var oItem = new FNBatchCost();
                            var oList = oDataGrp.Results.Where(x => x.ID == oMcahineID);

                            if(oList != null) oItem= new FNBatchCost(){ Qty_Batch=oList.Sum(x=>x.Qty_Batch), Value=oList.Sum(x=>x.Value)};

                            FillCell(sheet, nRowIndex, nCol_Data, oItem.Qty_Batch.ToString(), true);
                            FillCell(sheet, nRowIndex, nCol_Data + 1, oItem.Value.ToString(), true);
                            FillCell(sheet, nRowIndex, nCol_Data + 2, (oItem.Qty_Batch > 0 ? (oItem.Value / oItem.Qty_Batch).ToString() : ""), true);
                            nCol_Data += 3;
                            #endregion
                        }
                        FillCell(sheet, nRowIndex, nCol_Data, _oFNBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.Qty_Batch).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Data + 1, _oFNBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.Value).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Data + 2, _oFNBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
                        //FillCell(sheet, nRowIndex, nCol_Data + 1, _oFNBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.Qty * x.UnitPrice).ToString(), true);
                        //FillCell(sheet, nRowIndex, nCol_Data + 2, _oFNBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.UnitPrice).ToString(), true);
                        #endregion

                        nRowIndex++; 
                    }

                    int nCol_Total = 4;
                    FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, 2, 3, true, ExcelHorizontalAlignment.Center);
                    foreach (var oDataGrp in dataGrpList)
                    {
                        FillCell(sheet, nRowIndex, nCol_Total, oDataGrp.Results.Sum(x => x.Qty_Batch).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Total + 1, oDataGrp.Results.Sum(x => x.Value).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Total + 2, oDataGrp.Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
                        nCol_Total += 3;
                    }
                    FillCell(sheet, nRowIndex, nCol_Total, _oFNBatchCosts.Sum(x => x.Qty_Batch).ToString(), true, true);
                    FillCell(sheet, nRowIndex, nCol_Total + 1, _oFNBatchCosts.Sum(x => x.Value).ToString(), true, true);
                    FillCell(sheet, nRowIndex, nCol_Total + 2, _oFNBatchCosts.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol+1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                  
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FNBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

            }
        }

        public void ExportToExcel_DateWise(string sParams, double ts)
        {
            string Header = ""; int nReportType = 0;
            _oFNBatchCost = new FNBatchCost();
            Company oCompany = new Company();
            FNBatchCost oFNBatchCost = new FNBatchCost();
            try
            {
                _sErrorMesage = "";
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFNBatchCosts = new List<FNBatchCost>();
                oFNBatchCost.Params = sParams;
                if (!String.IsNullOrEmpty(sParams))
                {
                    int nCount = 0;
                    _oFNBatchCost.DateType = Convert.ToInt16(oFNBatchCost.Params.Split('~')[nCount++]);
                    _oFNBatchCost.StartDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);
                    _oFNBatchCost.EndDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);
                    nReportType = Convert.ToInt16(oFNBatchCost.Params.Split('~')[nCount++]);
                }
                if (_oFNBatchCost.DateType == 1) { _oFNBatchCost.EndDate = _oFNBatchCost.StartDate.AddDays(1); }
                //_oFNBatchCosts = FNBatchCost.GetsDetail(_oFNBatchCost.StartDate, _oFNBatchCost.EndDate, "", 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFNBatchCosts = FNBatchCost.Gets(_oFNBatchCost.StartDate, _oFNBatchCost.EndDate, "", 0, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFNBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFNBatchCosts = new List<FNBatchCost>();
                _sErrorMesage = ex.Message;
            }
            if (_sErrorMesage == "")
            {
                _sFormatter = "###,0.00;(###,0.00)";
                _oFNBatchCosts = _oFNBatchCosts.Where(x => x.ID > 0).ToList();
                var dataGrpList = _oFNBatchCosts.OrderBy(x => x.ID).GroupBy(x => new { ID = x.ID }, (key, grp) => new
                {
                    HeaderName = grp.Select(x => x.Name).FirstOrDefault(),
                    Results = grp.ToList().OrderBy(z => z.ID)
                }).ToList();
                var oProDate_List = _oFNBatchCosts.OrderBy(x => x.ID).ThenBy(y=>y.ProDate).Select(x => x.ProDateSt).Distinct();
                //var oMachineList = _oFNBatchCosts.Select(x => new FNBatchCost() { ID = x.ID, Name = x.Name }).Distinct();
                Header = "Machine Wise Costing Report";
                
                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = 0;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Machine_Wise_Costing_Report");
                    sheet.Name = "Machine_Wise_Costing_Report";

                    int nCount_Column = 2;
                    int nTotalMonths = dataGrpList.Count();
                    sheet.Column(nCount_Column++).Width = 15;
                    sheet.Column(nCount_Column++).Width = 30;
                    //sheet.Column(nCount++).Width = 50;

                    for (int i = 0; i < nTotalMonths; i++)
                    {
                        sheet.Column(nCount_Column++).Width = 20;
                        sheet.Column(nCount_Column++).Width = 20;
                        sheet.Column(nCount_Column++).Width = 20;
                    }

                    //sheet.Column(nCount_Column++).Width = 30;
                    sheet.Column(nCount_Column++).Width = 30;
                    sheet.Column(nCount_Column++).Width = 30;
                    sheet.Column(nCount_Column++).Width = 30;
                    nEndCol = nCount_Column;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol / 2]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 16; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol / 2 + 1, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol / 2]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol / 2 + 1, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nRowIndex++;

                    #region Column Header
                    nStartCol = 3;
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex + 1, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3, nRowIndex + 1, 3]; cell.Value = "Production Date"; cell.Style.Font.Bold = true; cell.Merge = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    int nCol_Header = 4;
                    foreach (var oItem in dataGrpList)
                    {
                        FillCellMerge(ref sheet, oItem.HeaderName, nRowIndex, nRowIndex, nCol_Header, nCol_Header + 2, true, ExcelHorizontalAlignment.Center);
                        FillCell(sheet, nRowIndex + 1, nCol_Header, "Prod. Qty", false);
                        FillCell(sheet, nRowIndex + 1, nCol_Header + 1, "Value", false);
                        FillCell(sheet, nRowIndex + 1, nCol_Header + 2, "P. YDS", false);
                        nCol_Header += 3;
                    }

                    FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, nCol_Header, nCol_Header + 2, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nRowIndex + 1, nCol_Header, "Prod. Qty", false);
                    FillCell(sheet, nRowIndex + 1, nCol_Header + 1, "Value", false);
                    FillCell(sheet, nRowIndex + 1, nCol_Header + 2, "P. YDS", false);

                    //cell = sheet.Cells[nRowIndex, nCol_Header, nRowIndex + 1, nCol_Header]; cell.Value = "Total"; cell.Style.Font.Bold = true; cell.Merge = true;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion

                    int nCount = 0; nRowIndex += 2;

                    #region Machine Wise Loop
                    foreach (var oDt in oProDate_List)
                    {
                        nStartCol = 2;
                        FillCell(sheet, nRowIndex, 2, (++nCount).ToString(), false);
                        FillCell(sheet, nRowIndex, 3, oDt, false);

                        #region Data List Wise Loop
                        int nCol_Data = 4;
                        foreach (var oDataGrp in dataGrpList)
                        {
                            #region DATA
                            var oItem = new FNBatchCost();
                            var oList = oDataGrp.Results.Where(x => x.ProDateSt == oDt);

                            if (oList != null) oItem = new FNBatchCost() { Qty_Batch = oList.Sum(x => x.Qty_Batch), Value = oList.Sum(x => x.Value) };

                            FillCell(sheet, nRowIndex, nCol_Data, oItem.Qty_Batch.ToString(), true);
                            FillCell(sheet, nRowIndex, nCol_Data + 1, oItem.Value.ToString(), true);
                            FillCell(sheet, nRowIndex, nCol_Data + 2, (oItem.Qty_Batch > 0 ? (oItem.Value / oItem.Qty_Batch).ToString() : ""), true);
                            nCol_Data += 3;
                            #endregion
                        }
                        FillCell(sheet, nRowIndex, nCol_Data, _oFNBatchCosts.Where(x => x.ProDateSt == oDt).Sum(x => x.Qty_Batch).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Data + 1, _oFNBatchCosts.Where(x => x.ProDateSt == oDt).Sum(x => x.Value).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Data + 2, _oFNBatchCosts.Where(x => x.ProDateSt == oDt).Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
                        //FillCell(sheet, nRowIndex, nCol_Data + 1, _oFNBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.Qty * x.UnitPrice).ToString(), true);
                        //FillCell(sheet, nRowIndex, nCol_Data + 2, _oFNBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.UnitPrice).ToString(), true);
                        #endregion

                        nRowIndex++;
                    }

                    int nCol_Total = 4;
                    FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, 2, 3, true, ExcelHorizontalAlignment.Center);
                    foreach (var oDataGrp in dataGrpList)
                    {
                        FillCell(sheet, nRowIndex, nCol_Total, oDataGrp.Results.Sum(x => x.Qty_Batch).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Total + 1, oDataGrp.Results.Sum(x => x.Value).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Total + 2, oDataGrp.Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
                        nCol_Total += 3;
                    }
                    FillCell(sheet, nRowIndex, nCol_Total, _oFNBatchCosts.Sum(x => x.Qty_Batch).ToString(), true, true);
                    FillCell(sheet, nRowIndex, nCol_Total + 1, _oFNBatchCosts.Sum(x => x.Value).ToString(), true, true);
                    FillCell(sheet, nRowIndex, nCol_Total + 2, _oFNBatchCosts.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
                    #endregion

                    int nIndx = nRowIndex + 3;

                    #region Summary Report
                    FillCellMerge(ref sheet, "Summary Report", nIndx, nIndx, 4, 8, true, ExcelHorizontalAlignment.Center);
                    nIndx++;
                    FillCellMerge(ref sheet, "Total,Dyes & Che, Consumption :Taka", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, _oFNBatchCosts.Sum(x => x.Value).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Average cost per Yds. Sin:,Ble:,Mer:", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, (((dataGrpList.Count() > 0) ? dataGrpList[0].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0) + ((dataGrpList.Count() > 1) ? dataGrpList[1].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0) + ((dataGrpList.Count() > 2) ? dataGrpList[2].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0) ).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Average cost per Yds. Wash/Jigger", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, ((dataGrpList.Count() > 3) ? (dataGrpList[3].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0))) : 0).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Average cost per Yds. (Only:Dyeing):", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, ((dataGrpList.Count() > 5) ? dataGrpList[5].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0).ToString(), true, true);
                    nIndx++;

                    double nDying = (((dataGrpList.Count() > 0) ? dataGrpList[0].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0) + ((dataGrpList.Count() > 1) ? dataGrpList[1].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0) + ((dataGrpList.Count() > 2) ? dataGrpList[2].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0) + ((dataGrpList.Count() > 3) ? dataGrpList[3].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0) + ((dataGrpList.Count() > 5) ? dataGrpList[5].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0) );
                    FillCellMerge(ref sheet, "Average cost per Yds. All Over:Dyeing):", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, nDying.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Average cost per Yds. (Only:Finising):", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, ((dataGrpList.Count() > 7) ? dataGrpList[7].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Average cost per Yds. (Only,Print):", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Average cost per Yds. (Dyeing + Print):", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, (nDying + ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)) : 0)).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Average cost per Yds. All Over:Costing):", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, (_oFNBatchCosts.Sum(x => x.Value) / ((dataGrpList.Count() > 4) ? dataGrpList[4].Results.Sum(x => x.Qty_Batch) : 0)).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Total Production:", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    double nTotalProd = (((dataGrpList.Count() > 4) ? dataGrpList[4].Results.Sum(x => x.Qty_Batch) : 0) + ((dataGrpList.Count() > 5) ? dataGrpList[5].Results.Sum(x => x.Qty_Batch) : 0) + ((dataGrpList.Count() > 6) ? dataGrpList[6].Results.Sum(x => x.Qty_Batch) : 0) + ((dataGrpList.Count() > 7) ? dataGrpList[7].Results.Sum(x => x.Qty_Batch) : 0) + ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Qty_Batch) : 0));
                    FillCell(sheet, nIndx, 8, nTotalProd.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Total Dyieng+Print Production:", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    double nDyiengPrintProd = (((dataGrpList.Count() > 4) ? dataGrpList[4].Results.Sum(x => x.Qty_Batch) : 0) + ((dataGrpList.Count() > 5) ? dataGrpList[5].Results.Sum(x => x.Qty_Batch) : 0) + ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Qty_Batch) : 0));
                    FillCell(sheet, nIndx, 8, (nTotalProd - nDyiengPrintProd).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Total:", nIndx, nIndx, 4, 6, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, "X", nIndx, nIndx, 7, 7, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 8, (((dataGrpList.Count() > 4) ? dataGrpList[4].Results.Sum(x => x.Qty_Batch) : 0) + ((dataGrpList.Count() > 5) ? dataGrpList[5].Results.Sum(x => x.Qty_Batch) : 0) + ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Qty_Batch) : 0)).ToString(), true, true);
                    nIndx++;
                    #endregion

                    nIndx = nRowIndex + 3;

                    #region Machine Wise
                    FillCellMerge(ref sheet, "Machine Wise Taka", nIndx, nIndx, 10, 12, true, ExcelHorizontalAlignment.Center);
                    nIndx++;
                    FillCellMerge(ref sheet, "Dyes:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, " ", false, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Chemicals:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, " ", false, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Re-Proses:Taka:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, " ", false, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Singing:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, ((dataGrpList.Count() > 1) ? dataGrpList[1].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Bleaching:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, ((dataGrpList.Count() > 2) ? dataGrpList[2].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Mercerizing:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, ((dataGrpList.Count() > 3) ? dataGrpList[3].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Thermosol :C P B", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, (((dataGrpList.Count() > 5) ? dataGrpList[5].Results.Sum(x => x.Value) : 0) + ((dataGrpList.Count() > 6) ? dataGrpList[6].Results.Sum(x => x.Value) : 0)).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Pad Steam:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Stenter:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, _oFNBatchCosts.Sum(x => x.Value).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Printing:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, " ", false, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Jigger:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, ((dataGrpList.Count() > 4) ? dataGrpList[4].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Re-Dying::Taka:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, " ", false, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Total,Dyes&Chemicals:", nIndx, nIndx, 10, 11, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 12, " ", false, true);
                    nIndx++;
                    #endregion

                    nIndx = nRowIndex + 3;

                    #region Production Report
                    FillCellMerge(ref sheet, "Machine Wise Costing & Production Report:", nIndx, nIndx, 14, 18, true, ExcelHorizontalAlignment.Center);
                    nIndx++;
                    FillCellMerge(ref sheet, "Machine Name", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCellMerge(ref sheet, "Production", nIndx, nIndx, 16, 16, true, ExcelHorizontalAlignment.Center);
                    FillCellMerge(ref sheet, "Value", nIndx, nIndx, 17, 17, true, ExcelHorizontalAlignment.Center);
                    FillCellMerge(ref sheet, "Avg.", nIndx, nIndx, 18, 18, true, ExcelHorizontalAlignment.Center);
                    nIndx++;
                    FillCellMerge(ref sheet, "Singing/De-Sizing:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, ((dataGrpList.Count() > 0) ? dataGrpList[0].Results.Sum(x => x.Qty_Batch) : 0).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, ((dataGrpList.Count() > 0) ? dataGrpList[0].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    double nAvgSing = ((dataGrpList.Count() > 0) ? (dataGrpList[0].Results.Sum(x => x.Value) / dataGrpList[0].Results.Sum(x => x.Qty_Batch)) : 0);
                    FillCell(sheet, nIndx, 18, nAvgSing.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Scouring/Bleaching:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, ((dataGrpList.Count() > 1) ? dataGrpList[1].Results.Sum(x => x.Qty_Batch) : 0).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, ((dataGrpList.Count() > 1) ? dataGrpList[1].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    double nAvgSour = ((dataGrpList.Count() > 1) ? (dataGrpList[1].Results.Sum(x => x.Value) / dataGrpList[1].Results.Sum(x => x.Qty_Batch)) : 0);
                    FillCell(sheet, nIndx, 18, nAvgSour.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Mercerizing:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, ((dataGrpList.Count() > 2) ? dataGrpList[2].Results.Sum(x => x.Qty_Batch) : 0).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, ((dataGrpList.Count() > 2) ? dataGrpList[2].Results.Sum(x => x.Value): 0).ToString(), true, true);
                    double nAvgMerc = ((dataGrpList.Count() > 2) ? (dataGrpList[2].Results.Sum(x => x.Value) / dataGrpList[2].Results.Sum(x => x.Qty_Batch)) : 0);
                    FillCell(sheet, nIndx, 18, nAvgMerc.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Thermosol:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, ((dataGrpList.Count() > 4) ? dataGrpList[4].Results.Sum(x => x.Qty_Batch) : 0).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, ((dataGrpList.Count() > 4) ? dataGrpList[4].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    double nAvgTher = ((dataGrpList.Count() > 4) ? (dataGrpList[4].Results.Sum(x => x.Value) / dataGrpList[4].Results.Sum(x => x.Qty_Batch)) : 0);
                    FillCell(sheet, nIndx, 18, nAvgTher.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "C P B:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, ((dataGrpList.Count() > 5) ? dataGrpList[5].Results.Sum(x => x.Qty_Batch) : 0).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, ((dataGrpList.Count() > 5) ? dataGrpList[5].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    double nAvgCpb = ((dataGrpList.Count() > 5) ? (dataGrpList[5].Results.Sum(x => x.Value) / dataGrpList[5].Results.Sum(x => x.Qty_Batch)) : 0);
                    FillCell(sheet, nIndx, 18, nAvgCpb.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Pad Steam:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, ((dataGrpList.Count() > 6) ? dataGrpList[6].Results.Sum(x => x.Qty_Batch) : 0).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, ((dataGrpList.Count() > 6) ? dataGrpList[6].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    double nAvgPad = ((dataGrpList.Count() > 6) ? (dataGrpList[6].Results.Sum(x => x.Value) / dataGrpList[6].Results.Sum(x => x.Qty_Batch)) : 0);
                    FillCell(sheet, nIndx, 18, nAvgPad.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Stenter:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, ((dataGrpList.Count() > 7) ? dataGrpList[7].Results.Sum(x => x.Qty_Batch) : 0).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, ((dataGrpList.Count() > 7) ? dataGrpList[7].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    double nAvgStent = ((dataGrpList.Count() > 7) ? (dataGrpList[7].Results.Sum(x => x.Value) / dataGrpList[7].Results.Sum(x => x.Qty_Batch)) : 0);
                    FillCell(sheet, nIndx, 18, nAvgStent.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Printing:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Qty_Batch) : 0).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    double nAvgPrinting = ((dataGrpList.Count() > 8) ? (dataGrpList[8].Results.Sum(x => x.Value) / dataGrpList[8].Results.Sum(x => x.Qty_Batch)) : 0);
                    FillCell(sheet, nIndx, 18, nAvgPrinting.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Jigger:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, ((dataGrpList.Count() > 3) ? dataGrpList[3].Results.Sum(x => x.Qty_Batch) : 0).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, ((dataGrpList.Count() > 3) ? dataGrpList[3].Results.Sum(x => x.Value) : 0).ToString(), true, true);
                    double nAvgJigger = ((dataGrpList.Count() > 3) ? (dataGrpList[3].Results.Sum(x => x.Value) / dataGrpList[3].Results.Sum(x => x.Qty_Batch)) : 0);
                    FillCell(sheet, nIndx, 18, nAvgJigger.ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "All Over Dying:", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, _oFNBatchCosts.Sum(x => x.Qty_Batch).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, _oFNBatchCosts.Sum(x => x.Value).ToString(), true, true);
                    FillCell(sheet, nIndx, 18, (nAvgSing + nAvgSour + nAvgMerc + nAvgTher + nAvgCpb + nAvgPad + nAvgStent + nAvgPrinting + nAvgJigger).ToString(), true, true);
                    nIndx++;
                    FillCellMerge(ref sheet, "Grand Total(Dying+Print):", nIndx, nIndx, 14, 15, true, ExcelHorizontalAlignment.Center);
                    FillCell(sheet, nIndx, 16, (_oFNBatchCosts.Sum(x => x.Qty_Batch) + ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Qty_Batch) : 0)).ToString(), true, true);
                    FillCell(sheet, nIndx, 17, (_oFNBatchCosts.Sum(x => x.Value) + ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Value) : 0)).ToString(), true, true);
                    FillCell(sheet, nIndx, 18, ((_oFNBatchCosts.Sum(x => x.Value) + ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Value) : 0)) / (_oFNBatchCosts.Sum(x => x.Qty_Batch) + ((dataGrpList.Count() > 8) ? dataGrpList[8].Results.Sum(x => x.Qty_Batch) : 0))).ToString(), true, true);
                    nIndx++;

                    #endregion


                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Date_Wise_Costing_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

            }
        }

        #endregion

        #region Support Functions
        
        private string GetSQL_Detail(FNBatchCost oFNBatchCost, int nProductType)
        {
             string sOrderNo = "";
        //    int nOrderType = 0;
        //    _sDateRange = "";
        //    _oFNBatchCost = new FNBatchCost();

        //    _oFNBatchCost.RSNo = oFNBatchCost.RSNo;
        //    _oFNBatchCost.StartDate = DateTime.Now;
        //    _oFNBatchCost.EndDate = DateTime.Now;
        //    if (!String.IsNullOrEmpty(oFNBatchCost.Params))
        //    {
        //        int nCount = 0;
        //        _oFNBatchCost.RSNo = oFNBatchCost.Params.Split('~')[nCount++];
        //        sOrderNo = oFNBatchCost.Params.Split('~')[nCount++];
        //        nOrderType = Convert.ToInt16(oFNBatchCost.Params.Split('~')[nCount++]);
        //        _oFNBatchCost.RSState = Convert.ToInt16(oFNBatchCost.Params.Split('~')[nCount++]);
        //        _oFNBatchCost.DateType = Convert.ToInt16(oFNBatchCost.Params.Split('~')[nCount++]);
        //        _oFNBatchCost.StartDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);
        //        _oFNBatchCost.EndDate = Convert.ToDateTime(oFNBatchCost.Params.Split('~')[nCount++]);
        //    }

        //    // AND  RouteSheet.RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH where RSH.[CurrentStatus]=12 and RSH.EventTime>= ''10 JAN 2016'' and RSH.EventTime< ''10 JAN 2018'') '
             string sSQLQuery = "", sWhereCluse = "  IsDyesChemical=1  ";

        //    #region SheetNo
        //    if (_oFNBatchCost.RSNo != null && _oFNBatchCost.RSNo != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE RouteSheet.RouteSheetNo LIKE'%" + _oFNBatchCost.RSNo + "%' )";
        //    }
        //    #endregion

        //    #region OrderType
        //    if (nOrderType >0 )
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + "  RouteSheetID In (SELECT RouteSheetID FROM RouteSheet WHERE PTUID IN (SELECT ProductionTracingUnitID FROM ProductionTracingUnit WHERE OrderID IN (SELECT DEOID FROM DyeingExecutionOrder WHERE OrderType =" + sOrderNo + ")))";
        //    }
        //    #endregion

        //    #region Order No
        //    if (!string.IsNullOrEmpty(sOrderNo))
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " RouteSheetID In (SELECT RouteSheetID FROM RouteSheet WHERE PTUID IN (SELECT ProductionTracingUnitID FROM ProductionTracingUnit WHERE OrderID IN (SELECT DEOID FROM DyeingExecutionOrder WHERE [Code] LIKE '%" + sOrderNo + "%')))";
        //    }
        //    #endregion

        //    #region RSState & Date
        //    if (_oFNBatchCost.RSState > 0 && _oFNBatchCost.DateType > 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        string sSearchPart = "WHERE RSH.[CurrentState]=" + _oFNBatchCost.RSState;
        //        DateObject.CompareDateQuery(ref sSearchPart, "RSH.EventTime", _oFNBatchCost.DateType, _oFNBatchCost.StartDate, _oFNBatchCost.EndDate);
        //        sWhereCluse = sWhereCluse + " RouteSheetID In (Select RouteSheetID from RouteSheetHistoryEnhance as RSH " + sSearchPart + ")";
        //    }
        //    else if (_oFNBatchCost.RSState > 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " RouteSheetID In (Select RouteSheetID from RouteSheetHistoryEnhance as RSH WHERE RSH.[CurrentState]=" + _oFNBatchCost.RSState + ")";
        //    }
        //    #endregion

        //    #region Dyes / Chemical
        //    if (nProductType == 1) //Dyes
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + "ProcessID IN (SELECT Product.ProductID FROM Product WHERE Product.ProductCategoryID IN (SELECT ProductCategoryID FROM ProductCategory WHERE ProductCategoryName='Dyes'))";
        //    }
        //    else if (nProductType == 2) //Chemical
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + "ProcessID IN (SELECT Product.ProductID FROM Product WHERE Product.ProductCategoryID IN (SELECT ProductCategoryID FROM ProductCategory WHERE ProductCategoryName='Chemicals'))";
        //    }
        //    #endregion


        //    sSQLQuery = sSQLQuery + sWhereCluse;
            return sSQLQuery;
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
        private FNBatchCost GetSQLSummary(string sParams)
        {
            string sPINo = "", sDispoNo = "", sProcessIDs = "", sMachineIDs = "", sBuyerIDs = "", sPONo = "";
            int nTreatment = 0;
            FNBatchCost oFNBatchCost = new FNBatchCost();
            oFNBatchCost.Params = sParams;
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                oFNBatchCost.DateType = Convert.ToInt16(sParams.Split('~')[nCount++]);
                oFNBatchCost.StartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                oFNBatchCost.EndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);

                sPINo = sParams.Split('~')[nCount++];
                sDispoNo = sParams.Split('~')[nCount++];
                nTreatment = Convert.ToInt16(sParams.Split('~')[nCount++]);
                sProcessIDs = sParams.Split('~')[nCount++];
                sMachineIDs = sParams.Split('~')[nCount++];

                oFNBatchCost.ReportType = Convert.ToInt16(sParams.Split('~')[nCount++]);
                sBuyerIDs = sParams.Split('~')[nCount++];
                sPONo = sParams.Split('~')[nCount++];
            }

            string sReturn1 = "SELECT ProductCategory.ProductCategoryName as ProductCategoryName,	Product.ProductName, MeasurementUnit.Symbol as MUnit,	FNPC.ProductID,	AVG(Lot.UnitPrice) as UnitPrice,  SUM(FNPC.Qty) as Qty,  SUM(FNPC.Qty * Lot.UnitPrice) as Value, Currency.CurrencyName as Currency FROM FNProductionConsumption AS FNPC 	INNER JOIN   Lot ON Lot.LotID = FNPC.LotID	INNER JOIN   Product ON Product.ProductID = Lot.ProductID	INNER JOIN   ProductCategory ON Product.ProductCategoryID = ProductCategory.ProductCategoryID	INNER JOIN   MeasurementUnit ON MeasurementUnit.MeasurementUnitID = Lot.MUnitID	INNER JOIN   Currency ON Currency.CurrencyID = Lot.CurrencyID";
            string sReturn = "";

            #region PINo
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FNPC.FNPBatchID IN (SELECT FNPBatchID FROM View_FNProductionBatch where FNExOID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractDetailID in (Select ExportPIDetail.OrderSheetDetailID from ExportPIDetail where ExportPIID in (Select ExportPIID from ExportPI where PINo like  '%" + sPINo + "%'))))";
            }
            #endregion
            #region DispoNo
            if (!string.IsNullOrEmpty(sDispoNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM View_FNProductionBatch WHERE BatchNo like  '%" + sDispoNo + "%' )";

            }
            #endregion
            #region PONo
            if (!string.IsNullOrEmpty(sPONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FNPC.FNPBatchID IN (SELECT FNPBatchID FROM View_FNProductionBatch where FNExOID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractID in (SELECT FabricSalesContractID FROM FabricSalesContract WHERE SCNo LIKE '%" + sPONo + "%')))";
            }
            #endregion

            #region Treatment
            //if (nTreatment > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " PINo LIKE '%" + sPINo + "%' ";
            //}
            #endregion
            #region Process
            if (!string.IsNullOrEmpty(sProcessIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE FNProductionID IN (SELECT FNProductionID FROM FNProduction WHERE FNTPID IN (" + sProcessIDs + ")))";
            }
            #endregion
            #region MachineIDs
            if (!string.IsNullOrEmpty(sMachineIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE FNProductionID IN (SELECT FNProductionID FROM FNProduction WHERE FNMachineID IN (" + sMachineIDs + ")))";
            }
            #endregion

            #region Buyers
            //if (!string.IsNullOrEmpty(sBuyerIDs))
            //{
            //    Global.TagSQL(ref sReturn);
            //    //sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE FNProductionID IN (SELECT FNProductionID FROM FNProduction WHERE FNMachineID IN (" + sMachineIDs + ")))";
            //}
            #endregion

            #region  Date
            if (oFNBatchCost.DateType != (int)EnumCompareOperator.None)
            {
                if (oFNBatchCost.DateType == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDateTime,106)) =CONVERT(DATE, CONVERT(VARCHAR(12), '" + oFNBatchCost.StartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    _sDateText = "Date: " + oFNBatchCost.StartDate.ToString("dd MMM yyyy");
                }
                else if (oFNBatchCost.DateType == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FNPC.FNPBatchID IN (SELECT FNPBatchID FROM FNProductionBatch WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDateTime,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + oFNBatchCost.StartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + oFNBatchCost.EndDate.ToString("dd MMM yyyy") + "', 106))  )";
                    _sDateText = "Date: " + oFNBatchCost.StartDate.ToString("dd MMM yyyy") + " - To - " + oFNBatchCost.EndDate.ToString("dd MMM yyyy");
                }

            }
            #endregion

            sReturn = sReturn1 + sReturn;
            sReturn += "  GROUP BY FNPC.ProductID,ProductName,MeasurementUnit.Symbol,ProductCategory.ProductCategoryName,Currency.CurrencyName order by ProductCategory.ProductCategoryName";
            oFNBatchCost.Params = sReturn;

            return oFNBatchCost;
        }
        public ActionResult PrintSummary(string sParams, double ts)
        {
            string Header = "";
            int nReportType = 0;
            _oFNBatchCost = new FNBatchCost();
            Company oCompany = new Company();
            FNBatchCost oFNBatchCost = new FNBatchCost();
            _sErrorMesage = "";
            try
            {
                _oFNBatchCosts = new List<FNBatchCost>();
                _oFNBatchCost = GetSQLSummary(sParams);
                nReportType = _oFNBatchCost.ReportType;
                if (_oFNBatchCost.DateType == 1) { _oFNBatchCost.EndDate = _oFNBatchCost.StartDate.AddDays(1); }
                _oFNBatchCosts = FNBatchCost.GetsSQL(_oFNBatchCost.Params, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFNBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFNBatchCosts = new List<FNBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_oFNBatchCosts.Count > 0)
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptFNBatchCostDetails oReport = new rptFNBatchCostDetails();
                byte[] abytes = oReport.PrepareReportSUmmary(_oFNBatchCosts, oCompany, nReportType, _sDateText);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }

        }
    }
}

