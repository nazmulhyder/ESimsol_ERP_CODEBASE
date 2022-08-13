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
    public class WUBatchCostController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        WUBatchCost _oWUBatchCost = new WUBatchCost();
        List<WUBatchCost> _oWUBatchCosts = new List<WUBatchCost>();        

        #region Actions
        public ActionResult ViewWUBatchCosts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            WUBatchCost oWUBatchCost = new WUBatchCost();

            ViewBag.BUID = (int)EnumTextileUnit.Dyeing;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType)).Where(x => x.id>0);
            ViewBag.RSStates = EnumObject.jGets(typeof(EnumRSState)).Where(x => x.id == (int)EnumRSState.Finished
                                                                        || x.id == (int)EnumRSState.InPackageing
                                                                        || x.id == (int)EnumRSState.InFloor
                                                                        || x.id == (int)EnumRSState.LoadedInDrier
                                                                        || x.id == (int)EnumRSState.LoadedInDyeMachine
                                                                        || x.id == (int)EnumRSState.LoadedInHydro
                                                                        || x.id == (int)EnumRSState.UnLoadedFromDrier
                                                                        || x.id == (int)EnumRSState.UnloadedFromDyeMachine
                                                                        || x.id == (int)EnumRSState.UnloadedFromHydro
                                                                    );
            return View(_oWUBatchCosts);
        }
        public ActionResult ViewWUBatchCostDetails( int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            WUBatchCost oWUBatchCost = new WUBatchCost();

            ViewBag.BUID = (int)EnumTextileUnit.Dyeing; 
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType)).Where(x => x.id > 0);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.RSStates = EnumObject.jGets(typeof(EnumRSState)).Where(x => x.id == (int)EnumRSState.Finished
                                                                            || x.id == (int)EnumRSState.InPackageing
                                                                            || x.id == (int)EnumRSState.LoadedInDrier
                                                                            || x.id == (int)EnumRSState.LoadedInDyeMachine
                                                                            || x.id == (int)EnumRSState.LoadedInHydro
                                                                            || x.id == (int)EnumRSState.UnLoadedFromDrier
                                                                            || x.id == (int)EnumRSState.UnloadedFromDyeMachine
                                                                            || x.id == (int)EnumRSState.UnloadedFromHydro
                                                                            );
            return View(_oWUBatchCosts);
        }
        public ActionResult ViewWUBatchCost(int menuid)
        {
            WUBatchCost oWUBatchCost = new WUBatchCost();
            _oWUBatchCosts = new List<WUBatchCost>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oWUBatchCosts);
        }
        [HttpPost]
        public JsonResult AdvSearch(WUBatchCost oWUBatchCost)
        {
            _oWUBatchCost = new WUBatchCost();
            List<WUBatchCost> oWUBatchCosts = new List<WUBatchCost>();
            //string sSQL = GetSQL(oWUBatchCost);

            string sSQL = "";
            int nCount = 0;
            _oWUBatchCost.DateType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
            _oWUBatchCost.StartDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
            _oWUBatchCost.EndDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);

            if (_oWUBatchCost.DateType<=1 ||  _oWUBatchCost.StartDate == _oWUBatchCost.EndDate) { _oWUBatchCost.EndDate = _oWUBatchCost.StartDate.AddDays(1); }

            if (sSQL == "Error")
            {
                _oWUBatchCost = new WUBatchCost();
                _oWUBatchCost.ErrorMessage = "Please select a searching critaria.";
                oWUBatchCosts = new List<WUBatchCost>();
            }
            else
            {
                oWUBatchCosts = new List<WUBatchCost>();
                oWUBatchCosts = WUBatchCost.Gets(_oWUBatchCost.StartDate, _oWUBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oWUBatchCosts.Count == 0)
                {
                    oWUBatchCosts = new List<WUBatchCost>();
                }
            }
            var jsonResult = Json(oWUBatchCosts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult AdvSearch_Detail(WUBatchCost oWUBatchCost)
        {
            _oWUBatchCost = new WUBatchCost();
            List<WUBatchCost> oWUBatchCosts = new List<WUBatchCost>();
            //string sSQL = GetSQL_Detail(oWUBatchCost,0);

            string sSQL = "";
            int nCount = 0;
            _oWUBatchCost.DateType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
            _oWUBatchCost.StartDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
            _oWUBatchCost.EndDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);

            if (_oWUBatchCost.DateType <= 1 || _oWUBatchCost.StartDate == _oWUBatchCost.EndDate) { _oWUBatchCost.EndDate = _oWUBatchCost.StartDate.AddDays(1); }

            if (sSQL == "Error")
            {
                _oWUBatchCost = new WUBatchCost();
                _oWUBatchCost.ErrorMessage = "Please select a searching critaria.";
                oWUBatchCosts = new List<WUBatchCost>();
            }
            else
            {
                oWUBatchCosts = new List<WUBatchCost>();
                oWUBatchCosts = WUBatchCost.GetsDetail(_oWUBatchCost.StartDate, _oWUBatchCost.EndDate, sSQL, 0, 2, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oWUBatchCosts.Count == 0)
                {
                    oWUBatchCosts = new List<WUBatchCost>();
                }
            }
            var jsonResult = Json(oWUBatchCosts, JsonRequestBehavior.AllowGet);
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

        #region Export To Excel
        public void ExportToExcelWUBatchCost(string sParams, double ts) //YD Production Summary
        {
            string Header = "";

            Company oCompany = new Company();
            WUBatchCost oWUBatchCost = new WUBatchCost();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oWUBatchCosts = new List<WUBatchCost>();

                //if (!String.IsNullOrEmpty(sParams))
                //{
                //    oWUBatchCost.RSNo = sParams.Split('~')[0];
                //    oWUBatchCost.Params = sParams;
                //}
                string sSQL = this.GetSQL(oWUBatchCost);
                _oWUBatchCosts = WUBatchCost.Gets(_oWUBatchCost.StartDate, _oWUBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oWUBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oWUBatchCosts = new List<WUBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M/C No.", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Order No/Dispo", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Quality", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color Depth %", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Name of Shade", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Loading Time", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Unloading Time", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyeing Duration", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Re-production Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Addition Qty Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "No. of Addition", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loading Capacity Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Programe Short", Width = 20f, IsRotate = false });
                #endregion

                Header = "YD Production Summary";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("WUBatchCost");
                    sheet.Name = "WUBatchCost_Summary";

                    _sFormatter = " #,##0.0000;(#,##0.0000)";
                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
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

                    int nCount = 0;
                    string sCurrencySymbol = "";

                    #region Data

                    nRowIndex++;
                    foreach (var oItem in _oWUBatchCosts)
                    {
                        nStartCol = 2;
                        #region DATA
                        FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ProDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.Name, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                        //FillCellMerge(ref sheet, oItem.Buyer, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //FillCellMerge(ref sheet, oItem.RSNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.ShadePertage.ToString(), true);

                        //FillCellMerge(ref sheet, oItem.ShadeName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //FillCellMerge(ref sheet, oItem.MLoadTimeSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //FillCellMerge(ref sheet, oItem.MUnLoadTimeSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //FillCellMerge(ref sheet, oItem.DyeingTimeDuration, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                        ////	Loading Capacity	
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Yarn.ToString(), true);
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Finishid.ToString(), true);
                        //FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty_Yarn - oItem.Qty_Finishid).ToString(), true);
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.NumberOfAddition.ToString(), false); //No Of Addition
                        FillCell(sheet, nRowIndex, nStartCol++, "", false); //Loading Capacity
                        FillCell(sheet, nRowIndex, nStartCol++, "", false); //Programe Short
                        #endregion
                        nRowIndex++;
                    }
                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.ShadePertage).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty_Yarn).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty_Finishid).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty_Yarn - x.Qty_Finishid).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 12];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=WUBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void Excel_BatchCostReport(string sParams, double ts) //Batch Costing Report
        {
            string Header = "";

            Company oCompany = new Company();
            WUBatchCost oWUBatchCost = new WUBatchCost();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oWUBatchCosts = new List<WUBatchCost>();

                //if (!String.IsNullOrEmpty(sParams))
                //{
                //    oWUBatchCost.RSNo = sParams.Split('~')[0];
                //    oWUBatchCost.Params = sParams;
                //}
                string sSQL = this.GetSQL(oWUBatchCost);
                _oWUBatchCosts = WUBatchCost.Gets(_oWUBatchCost.StartDate, _oWUBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oWUBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oWUBatchCosts = new List<WUBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "D", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Y", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M/C No.", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Order No/Dispo", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Quality", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color Depth %", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Name of Shade", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Dyeing Duration", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Re-production Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Addition Qty Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "No. of Addition", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loading Capacity Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Programe Short", Width = 20f, IsRotate = false });

                //Che Value 	 Dyes Value 	 Dyeing cost 	 Ch/Kg 	 Dy/Kg 	 Dyeing Cost/Kg 
                table_header.Add(new TableHeader { Header = "Che Value", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyes Value", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyeing cost", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = " Ch/Kg", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = " Dy/Kg", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = " Dyeing Cost/Kg", Width = 20f, IsRotate = false });

                #endregion

                Header = "Batch Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("WUBatchCost");
                    sheet.Name = "WUBatchCost_Report";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
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

                    int nCount = 0;
                    string sCurrencySymbol = "";

                    //#region Data
                    //_sFormatter = " #,##0.0000;(#,##0.0000)";
                    //nRowIndex++;
                    //foreach (var oItem in _oWUBatchCosts)
                    //{
                    //    nStartCol = 2;
                    //    #region DATA
                    //    FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.RSDate.ToString("dd"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.RSDate.ToString("MM"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.RSDate.ToString("yy"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.RSDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                    //    FillCellMerge(ref sheet, oItem.Buyer, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.RSNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCell(sheet, nRowIndex, nStartCol++, oItem.ShadePertage.ToString(), true);

                    //    FillCellMerge(ref sheet, oItem.ShadeName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    //    FillCellMerge(ref sheet, oItem.DyeingTimeDuration, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                    //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Yarn.ToString(), true);
                    //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Finishid.ToString(), true);
                    //    FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty_Yarn - oItem.Qty_Finishid).ToString(), true);
                    //    FillCell(sheet, nRowIndex, nStartCol++, oItem.NumberOfAddition.ToString(), true); //No Of Addition
                    //    FillCell(sheet, nRowIndex, nStartCol++, "", false); //Loading Capacity
                    //    FillCell(sheet, nRowIndex, nStartCol++, "", false); //Programe Short

                    //    //Che Value 	 Dyes Value 	 Dyeing cost 	 Ch/Kg 	 Dy/Kg 	 Dyeing Cost/Kg 
                    //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Value_Chemical.ToString(), true);
                    //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Value_Dyes.ToString(), true);
                    //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Dyeing_Cost.ToString(), true);
                    //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Chem_CostPerKG.ToString(), true);
                    //    FillCell(sheet, nRowIndex, nStartCol++, oItem.Dyes_CostPerKG.ToString(), true);
                    //    FillCell(sheet, nRowIndex, nStartCol++, (oItem.Dyeing_CostPerKg +""), true);
                    //    #endregion
                    //    nRowIndex++;
                    //}
                    //#region Grand Total
                    //nStartCol = 2; 
                    //FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 10, true, ExcelHorizontalAlignment.Right);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.ShadePertage).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty_Yarn).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty_Finishid).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty_Yarn - x.Qty_Finishid).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Value_Chemical).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Value_Dyes).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Dyeing_Cost).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Chem_CostPerKG).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Dyes_CostPerKG).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Dyeing_CostPerKg).ToString(), true, true);
                    ////_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    //nRowIndex++;
                    //#endregion

                    //cell = sheet.Cells[1, 1, nRowIndex, 12];
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    //fill.BackgroundColor.SetColor(Color.White);
                    //#endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=WUBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        #endregion

        #region Export To Excel (Consumption)
        public void ExportToExcel_Details(string sParams, double ts)
        {
            string Header = "";
            int nReportType = 0;
            _oWUBatchCost = new WUBatchCost();
            Company oCompany = new Company();
            WUBatchCost oWUBatchCost = new WUBatchCost();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oWUBatchCosts = new List<WUBatchCost>();

                oWUBatchCost.Params = sParams;
                if (!String.IsNullOrEmpty(sParams))
                {
                    int nCount = 0;
                    _oWUBatchCost.DateType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
                    _oWUBatchCost.StartDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
                    _oWUBatchCost.EndDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
                    nReportType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
                }

                if (_oWUBatchCost.DateType == 1) { _oWUBatchCost.EndDate = _oWUBatchCost.StartDate.AddDays(1); }

                _oWUBatchCosts = WUBatchCost.GetsDetail(_oWUBatchCost.StartDate, _oWUBatchCost.EndDate, "", 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oWUBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oWUBatchCosts = new List<WUBatchCost>();
                _sErrorMesage = ex.Message;
            }
            if (nReportType == 1)
                Print_MachineWise(oCompany);
            if (nReportType == 2)
                Print_BuyerWise(oCompany);
            if (nReportType == 3)
                Print_MKTPWise(oCompany);
        }
        private void Print_MachineWise(Company oCompany) 
        {
            string Header="";
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw Material", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount Cost", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oWUBatchCosts.GroupBy(x => new { x.MachineID, x.MachineName }, (key, grp) => new
                {
                    HeaderName = key.MachineName, //unique dt
                    Results = grp.ToList() //All data
                });
                Header = "Machine Wise Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("WUBatchCost");
                    sheet.Name = "WUBatchCost_MachineWise";

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
                    cell.Value = ""; cell.Style.Font.Bold = false;
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
                           nSubTotal_Qty_Batch = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Batch = 0;

                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0, nPreviousBatchID = 0;

                        nSubTotal_Qty_Order = 0;
                        nSubTotal_Qty_Batch = 0;

                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FBID))
                        {
                            #region Batch Wise Total
                            if (nPreviousBatchID != oItem.FBID && nCount > 0)
                            {
                                #region Total
                                nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty).ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);

                                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                                nSubTotal_Qty_Batch += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault();
                                //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                                nRowIndex++;
                                #endregion
                            }
                            nPreviousBatchID = oItem.FBID;
                            #endregion

                            #region DATA
                            nStartCol = 2;
                            FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ExcNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Order.ToString(), true);
                            FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.Color, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Batch.ToString(), true);
                            FillCellMerge(ref sheet, oItem.WeavingProcess.ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty * oItem.UnitPrice).ToString(), true);
                            #endregion
                            nRowIndex++;
                        }
                        #region Total
                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);
                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion

                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Batch += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault();

                        nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                        nGrndTotal_Qty_Batch += nSubTotal_Qty_Batch;

                        #region Sub Total
                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Order.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Batch.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);
                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Order.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Batch.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);
                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=WUBatchCost(" + Header + ").xlsx");
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
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Machine Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw Material", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount Cost", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oWUBatchCosts.GroupBy(x => new { x.BuyerID, x.BuyerName }, (key, grp) => new
                {
                    HeaderName = key.BuyerName, //unique dt
                    Results = grp.ToList() //All data
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("WUBatchCost");
                    sheet.Name = "WUBatchCost_BuyerWise";

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
                    cell.Value = ""; cell.Style.Font.Bold = false;
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
                           nSubTotal_Qty_Batch = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Batch = 0;

                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0, nPreviousBatchID = 0;

                        nSubTotal_Qty_Order = 0;
                        nSubTotal_Qty_Batch = 0;

                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FBID))
                        {
                            #region Batch Wise Total
                            if (nPreviousBatchID != oItem.FBID && nCount > 0)
                            {
                                #region Total
                                nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty).ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);

                                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                                nSubTotal_Qty_Batch += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault();
                                //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                                nRowIndex++;
                                #endregion
                            }
                            nPreviousBatchID = oItem.FBID;
                            #endregion

                            #region DATA
                            nStartCol = 2;
                            FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ExcNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Order.ToString(), true);
                            FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.Color, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Batch.ToString(), true);
                            FillCellMerge(ref sheet, oItem.WeavingProcess.ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty * oItem.UnitPrice).ToString(), true);
                            #endregion
                            nRowIndex++;
                        }
                        #region Total
                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);
                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion

                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Batch += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault();

                        nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                        nGrndTotal_Qty_Batch += nSubTotal_Qty_Batch;

                        #region Sub Total
                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Order.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Batch.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);
                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Order.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Batch.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);
                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=WUBatchCost(" + Header + ").xlsx");
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
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Supplier Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Raw Material", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount Cost", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oWUBatchCosts.GroupBy(x => new { x.ContractorID, x.ContractorName }, (key, grp) => new
                {
                    HeaderName = key.ContractorName, //unique dt
                    Results = grp.ToList() //All data
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("WUBatchCost");
                    sheet.Name = "WUBatchCost_SupplierWise";

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
                    cell.Value = ""; cell.Style.Font.Bold = false;
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
                           nSubTotal_Qty_Batch = 0,
                           nGrndTotal_Qty_Order = 0,
                           nGrndTotal_Qty_Batch = 0;

                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0, nPreviousBatchID = 0;

                        nSubTotal_Qty_Order = 0;
                        nSubTotal_Qty_Batch = 0;

                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.FBID))
                        {
                            #region Batch Wise Total
                            if (nPreviousBatchID != oItem.FBID && nCount > 0)
                            {
                                #region Total
                                nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault().ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty).ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);

                                nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                                nSubTotal_Qty_Batch += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault();
                                //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                                nRowIndex++;
                                #endregion
                            }
                            nPreviousBatchID = oItem.FBID;
                            #endregion

                            #region DATA
                            nStartCol = 2;
                            FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ExcNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Order.ToString(), true);
                            FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.Color, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Batch.ToString(), true);
                            FillCellMerge(ref sheet, oItem.WeavingProcess.ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty * oItem.UnitPrice).ToString(), true);
                            #endregion
                            nRowIndex++;
                        }
                        #region Total
                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault().ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);
                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion

                        nSubTotal_Qty_Order += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Order).FirstOrDefault();
                        nSubTotal_Qty_Batch += oDataGrp.Results.Where(x => x.FBID == nPreviousBatchID).Select(x => x.Qty_Batch).FirstOrDefault();

                        nGrndTotal_Qty_Order += nSubTotal_Qty_Order;
                        nGrndTotal_Qty_Batch += nSubTotal_Qty_Batch;

                        #region Sub Total
                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Order.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Batch.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oDataGrp.Results.Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);
                        //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        nRowIndex++;
                        #endregion
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Order.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Batch.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oWUBatchCosts.Sum(x => x.Qty * x.UnitPrice).ToString(), true, true);
                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=WUBatchCost(" + Header + ").xlsx");
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

            _oWUBatchCost = new WUBatchCost();
            Company oCompany = new Company();
            WUBatchCost oWUBatchCost = new WUBatchCost();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oWUBatchCosts = new List<WUBatchCost>();

                oWUBatchCost.Params = sParams;
                if (!String.IsNullOrEmpty(sParams))
                {
                    int nCount = 0;
                    _oWUBatchCost.DateType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
                    _oWUBatchCost.StartDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
                    _oWUBatchCost.EndDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
                    nReportType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
                }

                if (_oWUBatchCost.DateType == 1) { _oWUBatchCost.EndDate = _oWUBatchCost.StartDate.AddDays(1); }

                //_oWUBatchCosts = WUBatchCost.GetsDetail(_oWUBatchCost.StartDate, _oWUBatchCost.EndDate, "", 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oWUBatchCosts = WUBatchCost.Gets(_oWUBatchCost.StartDate, _oWUBatchCost.EndDate, "", 0, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oWUBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oWUBatchCosts = new List<WUBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                var dataGrpList = _oWUBatchCosts.GroupBy(x => new { x.ProDateSt}, (key, grp) => new
                {
                    HeaderName = grp.Select(x => x.ProDateSt).FirstOrDefault(), //unique dt
                    Results = grp.ToList().OrderBy(x => x.ID) //All data
                });

                var oMachineID_List = _oWUBatchCosts.OrderBy(x => x.ID).Select(x => x.ID).Distinct();
                //var oMachineList = _oWUBatchCosts.Select(x => new WUBatchCost() { ID = x.ID, Name = x.Name }).Distinct();

                Header = "Month Wise Costing Report";
                if (nReportType == 2)
                    Header = "Buyer Wise Costing Report";
                if (nReportType == 3)
                    Header = "MKT Person Wise Costing Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = 0;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("WUBatchCost");
                    sheet.Name = "WUBatchCost_Summary";


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
                        FillCell(sheet, nRowIndex, 3, _oWUBatchCosts.Where(x => x.ID == oMcahineID).Select(x => x.Name).FirstOrDefault(), false);

                        #region Data List Wise Loop
                        int nCol_Data = 4;
                        foreach (var oDataGrp in dataGrpList)
                        {
                            #region DATA
                            var oItem = new WUBatchCost();
                            var oList = oDataGrp.Results.Where(x => x.ID == oMcahineID);

                            if(oList != null) oItem= new WUBatchCost(){ Qty_Batch=oList.Sum(x=>x.Qty_Batch), Value=oList.Sum(x=>x.Value)};

                            FillCell(sheet, nRowIndex, nCol_Data, oItem.Qty_Batch.ToString(), true);
                            FillCell(sheet, nRowIndex, nCol_Data + 1, oItem.Value.ToString(), true);
                            FillCell(sheet, nRowIndex, nCol_Data + 2, (oItem.Qty_Batch > 0 ? (oItem.Value / oItem.Qty_Batch).ToString() : ""), true);
                            nCol_Data += 3;
                            #endregion
                        }
                        FillCell(sheet, nRowIndex, nCol_Data, _oWUBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.Qty_Batch).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Data + 1, _oWUBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.Value).ToString(), true, true);
                        FillCell(sheet, nRowIndex, nCol_Data + 2, _oWUBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
                        //FillCell(sheet, nRowIndex, nCol_Data + 1, _oWUBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.Qty * x.UnitPrice).ToString(), true);
                        //FillCell(sheet, nRowIndex, nCol_Data + 2, _oWUBatchCosts.Where(x => x.ID == oMcahineID).Sum(x => x.UnitPrice).ToString(), true);
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
                    FillCell(sheet, nRowIndex, nCol_Total, _oWUBatchCosts.Sum(x => x.Qty_Batch).ToString(), true, true);
                    FillCell(sheet, nRowIndex, nCol_Total + 1, _oWUBatchCosts.Sum(x => x.Value).ToString(), true, true);
                    FillCell(sheet, nRowIndex, nCol_Total + 2, _oWUBatchCosts.Sum(x => (x.Qty_Batch > 0 ? (x.Value / x.Qty_Batch) : 0)).ToString(), true, true);
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol+1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                  
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=WUBatchCost(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        #region Support Functions
        private string GetSQL(WUBatchCost oWUBatchCost)
        {
            _sDateRange = "";
            _oWUBatchCost = new WUBatchCost();

            string sOrderNo="";
            int nOrderType = 0;

            //_oWUBatchCost.RSNo = oWUBatchCost.RSNo;
            //_oWUBatchCost.StartDate = DateTime.Now;
            //_oWUBatchCost.EndDate = DateTime.Now;
            //if (!String.IsNullOrEmpty(oWUBatchCost.Params))
            //{
            //    int nCount = 0;
            //    _oWUBatchCost.RSNo = oWUBatchCost.Params.Split('~')[nCount++];
            //    sOrderNo = oWUBatchCost.Params.Split('~')[nCount++];
            //    nOrderType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
            //    _oWUBatchCost.RSState = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
            //    _oWUBatchCost.DateType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
            //    _oWUBatchCost.StartDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
            //    _oWUBatchCost.EndDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
            //}

            // AND  RouteSheet.RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH where RSH.[CurrentStatus]=12 and RSH.EventTime>= ''10 JAN 2016'' and RSH.EventTime< ''10 JAN 2018'') '
            string sSQLQuery = "", sWhereCluse = "RouteSheet.RouteSheetID <> 0 ";

            //#region SheetNo
            //if (_oWUBatchCost.RSNo != null && _oWUBatchCost.RSNo != "")
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + " RouteSheetNo LIKE'%" + _oWUBatchCost.RSNo + "%'";
            //}
            //#endregion
            //#region nOrderType
            //if (nOrderType>0)
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + " RouteSheet.PTUID IN (SELECT ProductionTracingUnitID FROM ProductionTracingUnit WHERE OrderID IN (SELECT DEOID FROM DyeingExecutionOrder WHERE OrderType =" + nOrderType +"))";
            //}
            //#endregion
            //#region Order No
            //if (!string.IsNullOrEmpty(sOrderNo))
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + " RouteSheet.PTUID IN (SELECT ProductionTracingUnitID FROM ProductionTracingUnit WHERE OrderID IN (SELECT DEOID FROM DyeingExecutionOrder WHERE [Code] LIKE '%" + sOrderNo + "%'))";
            //}
            //#endregion
            //#region RSState & Date
            //if (_oWUBatchCost.RSState > 0 && _oWUBatchCost.DateType > 0)
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    string sSearchPart = "WHERE RSH.[CurrentState]=" + _oWUBatchCost.RSState;
            //    DateObject.CompareDateQuery(ref sSearchPart, "RSH.EventTime", _oWUBatchCost.DateType, _oWUBatchCost.StartDate, _oWUBatchCost.EndDate);
            //    sWhereCluse = sWhereCluse + " RouteSheet.RouteSheetID In (Select RouteSheetID from RouteSheetHistoryEnhance as RSH " + sSearchPart + ")";
            //}
            //else if (_oWUBatchCost.RSState > 0)
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + " RouteSheet.RouteSheetID In ( RouteSheetHistoryEnhance as RSH WHERE RSH.[CurrentState]=" + _oWUBatchCost.RSState + ")";
            //}
            //#endregion

            sSQLQuery = sSQLQuery + sWhereCluse;
            return sSQLQuery;
        }
        private string GetSQL_Detail(WUBatchCost oWUBatchCost, int nProductType)
        {
             string sOrderNo = "";
        //    int nOrderType = 0;
        //    _sDateRange = "";
        //    _oWUBatchCost = new WUBatchCost();

        //    _oWUBatchCost.RSNo = oWUBatchCost.RSNo;
        //    _oWUBatchCost.StartDate = DateTime.Now;
        //    _oWUBatchCost.EndDate = DateTime.Now;
        //    if (!String.IsNullOrEmpty(oWUBatchCost.Params))
        //    {
        //        int nCount = 0;
        //        _oWUBatchCost.RSNo = oWUBatchCost.Params.Split('~')[nCount++];
        //        sOrderNo = oWUBatchCost.Params.Split('~')[nCount++];
        //        nOrderType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
        //        _oWUBatchCost.RSState = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
        //        _oWUBatchCost.DateType = Convert.ToInt16(oWUBatchCost.Params.Split('~')[nCount++]);
        //        _oWUBatchCost.StartDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
        //        _oWUBatchCost.EndDate = Convert.ToDateTime(oWUBatchCost.Params.Split('~')[nCount++]);
        //    }

        //    // AND  RouteSheet.RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH where RSH.[CurrentStatus]=12 and RSH.EventTime>= ''10 JAN 2016'' and RSH.EventTime< ''10 JAN 2018'') '
             string sSQLQuery = "", sWhereCluse = "  IsDyesChemical=1  ";

        //    #region SheetNo
        //    if (_oWUBatchCost.RSNo != null && _oWUBatchCost.RSNo != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE RouteSheet.RouteSheetNo LIKE'%" + _oWUBatchCost.RSNo + "%' )";
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
        //    if (_oWUBatchCost.RSState > 0 && _oWUBatchCost.DateType > 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        string sSearchPart = "WHERE RSH.[CurrentState]=" + _oWUBatchCost.RSState;
        //        DateObject.CompareDateQuery(ref sSearchPart, "RSH.EventTime", _oWUBatchCost.DateType, _oWUBatchCost.StartDate, _oWUBatchCost.EndDate);
        //        sWhereCluse = sWhereCluse + " RouteSheetID In (Select RouteSheetID from RouteSheetHistoryEnhance as RSH " + sSearchPart + ")";
        //    }
        //    else if (_oWUBatchCost.RSState > 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " RouteSheetID In (Select RouteSheetID from RouteSheetHistoryEnhance as RSH WHERE RSH.[CurrentState]=" + _oWUBatchCost.RSState + ")";
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
    }
}

