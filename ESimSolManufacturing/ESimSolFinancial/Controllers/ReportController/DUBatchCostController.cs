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

namespace ESimSolFinancial.ControllersPrintDUBatchCost
{
    public class DUBatchCostController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        DUBatchCost _oDUBatchCost = new DUBatchCost();
        List<DUBatchCost> _oDUBatchCosts = new List<DUBatchCost>();        

        #region Actions
        public ActionResult ViewDUBatchCosts(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            DUBatchCost oDUBatchCost = new DUBatchCost();

            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            //ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType)).Where(x => x.id>0);
            ViewBag.OrderTypes = DUOrderSetup.GetsActive(buid,((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.RSShifts = RSShift.GetsByModule(buid, "" + (int)EnumModuleName.RouteSheet, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUDyeingTypes = DUDyeingType.Gets("SELECT * FROM DUDyeingType", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.RSStates = EnumObject.jGets(typeof(EnumRSState));
            //ViewBag.RSStates = EnumObject.jGets(typeof(EnumRSState)).Where(x => x.id == (int)EnumRSState.QC_Done
            //                                                            || x.id == (int)EnumRSState.InPackageing
            //                                                              || x.id == (int)EnumRSState.InFloor
            //                                                            || x.id == (int)EnumRSState.LoadedInDrier
            //                                                            || x.id == (int)EnumRSState.LoadedInDyeMachine
            //                                                            || x.id == (int)EnumRSState.LoadedInHydro
            //                                                            || x.id == (int)EnumRSState.UnLoadedFromDrier
            //                                                            || x.id == (int)EnumRSState.UnloadedFromDyeMachine
            //                                                            || x.id == (int)EnumRSState.UnloadedFromHydro
            //                                                            );
            return View(_oDUBatchCosts);
        }
        public ActionResult ViewDUBatchCostDetails(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            DUBatchCost oDUBatchCost = new DUBatchCost();

            ViewBag.BUID = buid;
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumOrderType)).Where(x => x.id > 0);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.RSStates = EnumObject.jGets(typeof(EnumRSState)).Where(x =>  x.id == (int)EnumRSState.QC_Done
                                                                            || x.id == (int)EnumRSState.InPackageing
                                                                            || x.id == (int)EnumRSState.LoadedInDrier
                                                                            || x.id == (int)EnumRSState.LoadedInDyeMachine
                                                                            || x.id == (int)EnumRSState.LoadedInHydro
                                                                            || x.id == (int)EnumRSState.UnLoadedFromDrier
                                                                            || x.id == (int)EnumRSState.UnloadedFromDyeMachine
                                                                            || x.id == (int)EnumRSState.UnloadedFromHydro
                                                                            );
            return View(_oDUBatchCosts);
        }
        public ActionResult ViewDUBatchCost(int buid, int id)
        {
            DUBatchCost oDUBatchCost = new DUBatchCost();
            _oDUBatchCosts = new List<DUBatchCost>();
            try 
            {
                _oDUBatchCosts = DUBatchCost.GetsDetail(" RouteSheetID="+id, id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUBatchCosts = new List<DUBatchCost>(); _oDUBatchCosts.Add(new DUBatchCost() {ErrorMessage=ex.Message});
            }
            ViewBag.BUID = buid;

            return View(_oDUBatchCosts);
        }
        [HttpPost]
        public JsonResult AdvSearch(DUBatchCost oDUBatchCost)
        {
            List<DUBatchCost> oDUBatchCosts = new List<DUBatchCost>();
            string sSQL = GetSQL(oDUBatchCost);
            if (sSQL == "Error")
            {
                _oDUBatchCost = new DUBatchCost();
                _oDUBatchCost.ErrorMessage = "Please select a searching critaria.";
                oDUBatchCosts = new List<DUBatchCost>();
            }
            else
            {
                oDUBatchCosts = new List<DUBatchCost>();
                oDUBatchCosts = DUBatchCost.Gets(_oDUBatchCost.StartDate, _oDUBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUBatchCosts.Count == 0)
                {
                    oDUBatchCosts = new List<DUBatchCost>();
                }
            }
            var jsonResult = Json(oDUBatchCosts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult AdvSearch_Detail(DUBatchCost oDUBatchCost)
        {
            List<DUBatchCost> oDUBatchCosts = new List<DUBatchCost>();
            string sSQL = GetSQL_Detail(oDUBatchCost,0);
            if (sSQL == "Error")
            {
                _oDUBatchCost = new DUBatchCost();
                _oDUBatchCost.ErrorMessage = "Please select a searching critaria.";
                oDUBatchCosts = new List<DUBatchCost>();
            }
            else
            {
                oDUBatchCosts = new List<DUBatchCost>();
                oDUBatchCosts = DUBatchCost.GetsDetail(sSQL, 0,((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUBatchCosts.Count == 0)
                {
                    oDUBatchCosts = new List<DUBatchCost>();
                }
            }
            var jsonResult = Json(oDUBatchCosts, JsonRequestBehavior.AllowGet);
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
        public void ExportToExcelDUBatchCost(string sParams, int buid, double ts) //YD Production Summary
        {
            string Header = "";
            BusinessUnit oBusinessUnit = new BusinessUnit();
            Company oCompany = new Company();
            DUBatchCost oDUBatchCost = new DUBatchCost();
         
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUBatchCosts = new List<DUBatchCost>();

                if (!String.IsNullOrEmpty(sParams))
                {
                    oDUBatchCost.RSNo = sParams.Split('~')[0];
                    oDUBatchCost.Params = sParams;
                }
                string sSQL = this.GetSQL(oDUBatchCost);
                _oDUBatchCost.BUID = buid;
                _oDUBatchCosts = DUBatchCost.Gets(_oDUBatchCost.StartDate, _oDUBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnit = oBusinessUnit.Get(_oDUBatchCost.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUBatchCosts = new List<DUBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                string sMUNIT = _oDUBatchCosts.Where(x => !string.IsNullOrEmpty(x.MUnit)).Select(x => x.MUnit).FirstOrDefault();
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M/C No.", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Yarn Type", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color Depth %", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Name of Shade", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Loading Time", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Unloading Time", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyeing Duration", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Qty ("+sMUNIT+")", Width = 20f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "Re-production Qty("+sMUNIT+")", Width = 20f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "Addition Qty ("+sMUNIT+")", Width = 20f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "No. of Addition", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loading Capacity", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Programe Short", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = " Is ReDyeing", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = " InHouse/OutSide", Width = 20f, IsRotate = false });
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("DUBatchCost");
                    sheet.Name = "DUBatchCost_Summary";

                    _sFormatter = "#,##0.0000;(#,##0.0000)";
                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
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
                    
                    #region Data

                    nRowIndex++;
                    foreach (var oItem in _oDUBatchCosts)
                    {
                        nStartCol = 2;
                        #region DATA

                        _sFormatter = "#,##0.0000;(#,##0.0000)";

                        FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                        FillCellMerge(ref sheet, oItem.Buyer, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ShadePertage.ToString(), true);

                        FillCellMerge(ref sheet, oItem.ShadeName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.MLoadTimeSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.MUnLoadTimeSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.DyeingTimeDuration, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        //	Loading Capacity	
                        _sFormatter = "#,##0.0000" ;
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Yarn.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormat(oItem.MCapacity), false); //Loading Capacity
                        FillCell(sheet, nRowIndex, nStartCol++, Global.MillionFormat(oItem.PShort), false); //Programe Short
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.IsReDyeingST, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.IsInHouseSt, false);
                   
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Finishid.ToString(), true);
                        //FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty_Yarn - oItem.Qty_Finishid).ToString(), true);

                        //_sFormatter = "#,##0;(#,##0)";
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.NumberOfAddition.ToString(), false); //No Of Addition
                       
                        #endregion
                        nRowIndex++;
                    }
                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCellMerge(ref sheet, "Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.ShadePertage).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    _sFormatter = " #,##0.0000";
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty_Yarn).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty_Finishid).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty_Yarn - x.Qty_Finishid).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                  
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 12];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DUBatchCost(" + Header.Replace(" ","-") + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void Excel_BatchCostReport(string sParams, int buid, double ts) //Batch Costing Report
        {
            string Header = "";

            Company oCompany = new Company();
            DUBatchCost oDUBatchCost = new DUBatchCost();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            try
            {
                _sErrorMesage = "";
              
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUBatchCosts = new List<DUBatchCost>();

                if (!String.IsNullOrEmpty(sParams))
                {
                    oDUBatchCost.RSNo = sParams.Split('~')[0];
                    oDUBatchCost.Params = sParams;
                }
                string sSQL = this.GetSQL(oDUBatchCost);
                _oDUBatchCosts = DUBatchCost.Gets(_oDUBatchCost.StartDate, _oDUBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUBatchCost.BUID = buid;
                if (_oDUBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUBatchCosts = new List<DUBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                string sMUNIT = _oDUBatchCosts.Where(x => !string.IsNullOrEmpty(x.MUnit)).Select(x => x.MUnit).FirstOrDefault();
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "D", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Y", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M/C No.", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false }); ///8
                table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });//9
                table_header.Add(new TableHeader { Header = "Yarn Type", Width = 25f, IsRotate = false });//10
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color Depth %", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Name of Shade", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyeing Duration", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Qty (" + sMUNIT + ")", Width = 20f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "Addition Qty (" + sMUNIT + ")", Width = 20f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "No. of Addition", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loading Capacity", Width = 20f, IsRotate = false });//18
                table_header.Add(new TableHeader { Header = "Programe Short", Width = 20f, IsRotate = false });//19
                //table_header.Add(new TableHeader { Header = "Yarn Qty", Width = 20f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "Che Qty", Width = 20f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "Dyes Qty", Width = 20f, IsRotate = false });
                //Che Value 	 Dyes Value 	 Dyeing cost 	 Ch/Kg 	 Dy/Kg 	 Dyeing Cost/Kg 
                table_header.Add(new TableHeader { Header = "Yarn Value", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Che Value", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyes Value", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyeing cost", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Ch/"+ sMUNIT , Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyes/" + sMUNIT , Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyeing Cost/" + sMUNIT , Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "RecycleQty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "WastageQty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Packing Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Packing Value", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = " Is Re Dyeing", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = " InHouse/OutSide", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Store Name", Width = 20f, IsRotate = false });

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
                    var sheet = excelPackage.Workbook.Worksheets.Add("DUBatchCost");
                    sheet.Name = "BatchCost Report";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = Header; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
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

                    #region Data
                    _sFormatter = " #,##0.0000;(#,##0.0000)";
                    nRowIndex++;
                    foreach (var oItem in _oDUBatchCosts)
                    {
                        nStartCol = 2;
                        #region DATA
                        FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSDate.ToString("dd"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSDate.ToString("MM"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSDate.ToString("yy"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                        FillCellMerge(ref sheet, oItem.Buyer, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ShadePertage.ToString(), false);

                        FillCellMerge(ref sheet, oItem.ShadeName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.DyeingTimeDuration, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                        _sFormatter =" #,##0.0000;(#,##0.0000)";

                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Yarn.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.MCapacity.ToString(), true);//Loading Capacity
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.PShort.ToString(), true);//P Shart
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Chemical.ToString(), true);
                        //FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty_Dyes).ToString(), true);

                        //_sFormatter = "#,##0;(#,##0)";
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.NumberOfAddition.ToString(), true); //No Of Addition
                     
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.MCapacity, false); //Loading Capacity
                        //FillCell(sheet, nRowIndex, nStartCol++, "", false); //Programe Short
                        //Che Value 	 Dyes Value 	 Dyeing cost 	 Ch/Kg 	 Dy/Kg 	 Dyeing Cost/Kg 
                        _sFormatter = oItem.CurrencySymbol + " #,##0.0000;(#,##0.0000)";
                      
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Value_Yarn.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Value_Chemical.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Value_Dyes.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Dyeing_Cost.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Chem_CostPerKG.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Dyes_CostPerKG.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, (oItem.Dyeing_CostPerKg.ToString()), true);
                        _sFormatter = " #,##0.0000;(#,##0.0000)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.RecycleQty.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.WastageQty.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Finishid.ToString(), true);
                        _sFormatter = oItem.CurrencySymbol + " #,##0.0000;(#,##0.0000)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Finish_Cost.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.IsReDyeingST, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.IsInHouseSt, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.WUName, false);
                        #endregion
                        nRowIndex++;
                    }
                    #region Grand Total
                    nStartCol = 2;
                    _sFormatter = "#,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 10, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.ShadePertage).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    _sFormatter = " #,##0.0000;(#,##0.0000)";

                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty_Yarn).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty_Finishid).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty_Yarn - x.Qty_Finishid).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);

                    _sFormatter = _oDUBatchCosts.Select(x => x.CurrencySymbol).FirstOrDefault() + " #,##0.0000;(#,##0.0000)";
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Value_Yarn).ToString(), true, true);//20
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Value_Chemical).ToString(), true, true);//21
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Value_Dyes).ToString(), true, true);//22
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Dyeing_Cost).ToString(), true, true);//23
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Chem_CostPerKG).ToString(), true, true);//24
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Dyes_CostPerKG).ToString(), true, true);//25
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Dyeing_CostPerKg).ToString(), true, true); //26
                    _sFormatter = " #,##0.0000;(#,##0.0000)";
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.RecycleQty).ToString(), true, true); //26
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.WastageQty).ToString(), true, true); //26
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty_Finishid).ToString(), true, true); //26
                    _sFormatter = _oDUBatchCosts.Select(x => x.CurrencySymbol).FirstOrDefault() + " #,##0.0000;(#,##0.0000)";
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Finish_Cost).ToString(), true, true); //26
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
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
                    Response.AddHeader("content-disposition", "attachment; filename=DUBatchCost(" + Header.Replace(" ", "-") + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        #endregion

        #region Export To Excel (Consumption)
        public void ExportToExcel_DyesConsumption(string sParams, double ts) //Dyes Consumption
        {
            string Header = "";

            Company oCompany = new Company();
            DUBatchCost oDUBatchCost = new DUBatchCost();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUBatchCosts = new List<DUBatchCost>();

                if (!String.IsNullOrEmpty(sParams))
                {
                    oDUBatchCost.RSNo = sParams.Split('~')[0];
                    oDUBatchCost.Params = sParams;
                }
                string sSQL = this.GetSQL_Detail(oDUBatchCost, 1);
                _oDUBatchCosts = DUBatchCost.GetsDetail(sSQL, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oDUBatchCosts = DUBatchCost.Gets(_oDUBatchCost.StartDate, _oDUBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUBatchCosts = new List<DUBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                string sMUNIT = _oDUBatchCosts.Where(x => !string.IsNullOrEmpty(x.MUnit)).Select(x => x.MUnit).FirstOrDefault();
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                //sl2	Date Dispo#	Batch No#	Batch Qty#	Shade#	Che. Name	Rate	Qty	Value		

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No/Dispo", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Qty ("+sMUNIT+")", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Shade", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "CategoryName", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dyes/Chemical Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty (" + sMUNIT + ")", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Additional Qty (" + sMUNIT + ")", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Return Qty (" + sMUNIT + ")", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total Qty (" + sMUNIT + ")", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "Qty (" + sMUNIT + ")", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Value", Width = 20f, IsRotate = false });
                #endregion

                Header = "Dyes Consumption";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("DUBatchCost");
                    sheet.Name = "DUBatchCost_Summary";

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

                    int nCount = 0;
                    
                    #region Data

                    nRowIndex++;
                    foreach (var oItem in _oDUBatchCosts)
                    {
                        nStartCol = 2;
                        #region DATA
                        FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                        _sFormatter = " #,##0.0000;(#,##0.0000)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                        FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ProductCategoryName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        _sFormatter = oItem.CurrencySymbol + " #,##0.0000;(#,##0.0000)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.AdditionalQty.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ReturnQty.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.TotalQty.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.TotalRate.ToString(), true);
                        //_sFormatter = " #,##0.0000;(#,##0.0000)";
                        //FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Dyes.ToString(), true);
                        _sFormatter = oItem.CurrencySymbol + " #,##0.0000;(#,##0.0000)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Value.ToString(), true);
                        #endregion
                        nRowIndex++;
                    }
                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty_Dyes).ToString(), true, true);
                    _sFormatter = _oDUBatchCosts.Select(x => x.CurrencySymbol).FirstOrDefault() + " #,##0.00;(#,##0.00)";
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Value_Dyes).ToString(), true, true);
                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 12];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DUBatchCost(" + Header.Replace(" ", "-") + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void ExportToExcel_CheConsumption(string sParams, double ts) //Chemical Consumption
        {
            string Header = "";

            Company oCompany = new Company();
            DUBatchCost oDUBatchCost = new DUBatchCost();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUBatchCosts = new List<DUBatchCost>();

                if (!String.IsNullOrEmpty(sParams))
                {
                    oDUBatchCost.RSNo = sParams.Split('~')[0];
                    oDUBatchCost.Params = sParams;
                }
                string sSQL = this.GetSQL_Detail(oDUBatchCost, 2);

                _oDUBatchCosts = DUBatchCost.GetsDetail(sSQL, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oDUBatchCosts = DUBatchCost.Gets(_oDUBatchCost.StartDate, _oDUBatchCost.EndDate, sSQL, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUBatchCosts.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUBatchCosts = new List<DUBatchCost>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                string sMUNIT = _oDUBatchCosts.Where(x => !string.IsNullOrEmpty(x.MUnit)).Select(x => x.MUnit).FirstOrDefault();
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                //sl2	Date Dispo#	Batch No#	Batch Qty#	Shade#	Che. Name	Rate	Qty	Value		

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No/Dispo", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Qty (" + sMUNIT + ")", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Shade", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Che. Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty (" + sMUNIT + ")", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Value", Width = 20f, IsRotate = false });
                #endregion

                Header = "Chemical Consumption";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("DUBatchCost");
                    sheet.Name = "DUBatchCost_Summary";

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

                    int nCount = 0;
                    string sCurrencySymbol = "";

                    #region Data

                    nRowIndex++;
                    foreach (var oItem in _oDUBatchCosts)
                    {
                        nStartCol = 2;
                        #region DATA
                        FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.OrderNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.RSNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);


                        _sFormatter = "#,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                        FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                        _sFormatter = oItem.CurrencySymbol + " #,##0.0000;(#,##0.0000)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.TotalRate.ToString(), true);

                        _sFormatter = " #,##0.0000;(#,##0.0000)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Chemical.ToString(), true);

                        _sFormatter = oItem.CurrencySymbol + " #,##0.0000;(#,##0.0000)"; 
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Value_Chemical.ToString(), true);
                        #endregion
                        nRowIndex++;
                    }
                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    _sFormatter = " #,##0.0000;(#,##0.0000)";

                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Qty_Chemical).ToString(), true, true);

                    _sFormatter = _oDUBatchCosts.Select(x => x.CurrencySymbol).FirstOrDefault() + " #,##0.0000;(#,##0.0000)";
                    FillCell(sheet, nRowIndex, ++nStartCol, _oDUBatchCosts.Sum(x => x.Value_Chemical).ToString(), true, true);
                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 12];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DUBatchCost(" + Header.Replace(" ", "-") + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        #region Support Functions
        private string GetSQL(DUBatchCost oDUBatchCost)
        {
            _sDateRange = "";
            _oDUBatchCost = new DUBatchCost();

            string sOrderNo="";
            int nOrderType = 0;
            int nRSShiftID = 0;
            int nDyeingType = 0;
            string sProductIDs = "";
            string sMachineIDs = "";
            bool bIsRedyeing = false;
            _oDUBatchCost.RSNo = oDUBatchCost.RSNo;
            _oDUBatchCost.StartDate = DateTime.Now;
            _oDUBatchCost.EndDate = DateTime.Now;
            if (!String.IsNullOrEmpty(oDUBatchCost.Params))
            {
                int nCount = 0;
                _oDUBatchCost.RSNo = oDUBatchCost.Params.Split('~')[nCount++];
                sOrderNo = oDUBatchCost.Params.Split('~')[nCount++];
                nOrderType = Convert.ToInt16(oDUBatchCost.Params.Split('~')[nCount++]);
                nRSShiftID = Convert.ToInt16(oDUBatchCost.Params.Split('~')[nCount++]);
                nDyeingType = Convert.ToInt16(oDUBatchCost.Params.Split('~')[nCount++]);
                _oDUBatchCost.RSState = (EnumRSState)Convert.ToInt16(oDUBatchCost.Params.Split('~')[nCount++]);
                _oDUBatchCost.DateType = Convert.ToInt16(oDUBatchCost.Params.Split('~')[nCount++]);
                _oDUBatchCost.StartDate = Convert.ToDateTime(oDUBatchCost.Params.Split('~')[nCount++]);
                _oDUBatchCost.EndDate = Convert.ToDateTime(oDUBatchCost.Params.Split('~')[nCount++]);
                sProductIDs = Convert.ToString(oDUBatchCost.Params.Split('~')[nCount++]);
                sMachineIDs = Convert.ToString(oDUBatchCost.Params.Split('~')[nCount++]);
                bIsRedyeing = Convert.ToBoolean(oDUBatchCost.Params.Split('~')[nCount++]);
               // _oDUBatchCost.BUID = Convert.ToInt32(oDUBatchCost.Params.Split('~')[nCount++]);

                //int nBUID = 0;
                //if (oDUBatchCost.Params.Split('~').Length > nCount)
                //    Int32.TryParse(oDUBatchCost.Params.Split('~')[nCount++], out nBUID);
                //_oDUBatchCost.BUID = nBUID;
            }

            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
           // oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            // AND  RouteSheet.RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH where RSH.[CurrentStatus]=12 and RSH.EventTime>= ''10 JAN 2016'' and RSH.EventTime< ''10 JAN 2018'') '
            string sSQLQuery = "", sWhereCluse = "RouteSheet.RouteSheetID <> 0 ";

            #region SheetNo
            if (_oDUBatchCost.RSNo != null && _oDUBatchCost.RSNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RouteSheetNo LIKE'%" + _oDUBatchCost.RSNo + "%'";
            }
            #endregion
            #region nOrderType
            if (nOrderType>0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " OrderType =" + nOrderType;
            }
            #endregion
            #region nRSShiftID
            if (nRSShiftID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RSShiftID =" + nRSShiftID;
            }
            #endregion
            #region nDyeingType
            if (nDyeingType > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HanksCone =" + nDyeingType;
            }
            #endregion
            #region bIsRedyeing
            if (bIsRedyeing ==true)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " IsReDyeing=1 ";
                //sWhereCluse = sWhereCluse + "(IsReDyeing=1 or RouteSheetID in (select RouteSheetID from RSRawLot where LotID in (Select lotID from lot where ParentType=106)))";
            }
            #endregion
            #region Order No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RouteSheet.RouteSheetID In (SELECT RouteSheetID FROM RouteSheetDO WHERE DyeingOrderDetailID IN  (SELECT DyeingOrderDetailID FROM DyeingOrderDetail WHERE DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE OrderNo LIKE '%" + sOrderNo + "%')))";
            }
            #endregion
            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RouteSheet.RouteSheetID In (SELECT RouteSheetID FROM View_RouteSheetDO WHERE ProductID IN  (" + sProductIDs + "))";
            }
            #endregion
            #region Machine IDs
            if (!string.IsNullOrEmpty(sMachineIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RouteSheet.MachineID In (" + sMachineIDs + ")";
            }
            #endregion
            #region RSState & Date
            if (_oDUBatchCost.RSState > 0 && _oDUBatchCost.DateType > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                string sSearchPart = " WHERE RSH.[CurrentStatus]=" + (int)_oDUBatchCost.RSState;
              //  DateObject.CompareDateQuery(ref sSearchPart, "RSH.EventTime", _oDUBatchCost.DateType, _oDUBatchCost.StartDate, _oDUBatchCost.EndDate);
                if (_oDUBatchCost.DateType == (int)EnumCompareOperator.EqualTo)
                {
                    sSearchPart = sSearchPart + "  and RSH.EventTime>='" + _oDUBatchCost.StartDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and RSH.EventTime<'" + _oDUBatchCost.StartDate.AddDays(1).ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'";
                    //sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtRouteSheetFrom.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (_oDUBatchCost.DateType == (int)EnumCompareOperator.Between)
                    {
                        sSearchPart = sSearchPart + " and RSH.EventTime>='" + _oDUBatchCost.StartDate.ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "' and RSH.EventTime<'" + _oDUBatchCost.EndDate.AddDays(1).ToString("dd MMM yyyy " + oRouteSheetSetup.BatchTime.ToString("HH:MM")) + "'";
                    }

                sWhereCluse = sWhereCluse + " RouteSheet.RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH " + sSearchPart + ")";
            }
            else if (_oDUBatchCost.RSState > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RouteSheet.RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH WHERE RSH.[CurrentStatus]=" +(int)_oDUBatchCost.RSState + ")";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse;
            return sSQLQuery;
        }
        private string GetSQL_Detail(DUBatchCost oDUBatchCost, int nProductType)
        {
            string sOrderNo = "";
            int nOrderType = 0;
            _sDateRange = "";
            _oDUBatchCost = new DUBatchCost();

            _oDUBatchCost.RSNo = oDUBatchCost.RSNo;
            _oDUBatchCost.StartDate = DateTime.Now;
            _oDUBatchCost.EndDate = DateTime.Now;
            if (!String.IsNullOrEmpty(oDUBatchCost.Params))
            {
                int nCount = 0;
                _oDUBatchCost.RSNo = oDUBatchCost.Params.Split('~')[nCount++];
                sOrderNo = oDUBatchCost.Params.Split('~')[nCount++];
                nOrderType = Convert.ToInt16(oDUBatchCost.Params.Split('~')[nCount++]);
                _oDUBatchCost.RSState = (EnumRSState)Convert.ToInt16(oDUBatchCost.Params.Split('~')[nCount++]);
                _oDUBatchCost.DateType = Convert.ToInt16(oDUBatchCost.Params.Split('~')[nCount++]);
                _oDUBatchCost.StartDate = Convert.ToDateTime(oDUBatchCost.Params.Split('~')[nCount++]);
                _oDUBatchCost.EndDate = Convert.ToDateTime(oDUBatchCost.Params.Split('~')[nCount++]);
            }

            // AND  RouteSheet.RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH where RSH.[CurrentStatus]=12 and RSH.EventTime>= ''10 JAN 2016'' and RSH.EventTime< ''10 JAN 2018'') '
            string sSQLQuery = "", sWhereCluse = " ";

            #region SheetNo
            if (_oDUBatchCost.RSNo != null && _oDUBatchCost.RSNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE RouteSheet.RouteSheetNo LIKE'%" + _oDUBatchCost.RSNo + "%' )";
            }
            #endregion

            #region OrderType
            if (nOrderType >0 )
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RouteSheetID IN (SELECT RouteSheetID FROM RouteSheet WHERE RouteSheet.OrderType =" + nOrderType + " )";
            }
            #endregion

            #region Order No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RouteSheetID In (SELECT RouteSheetID FROM RouteSheetDO WHERE DyeingOrderDetailID IN (SELECT DyeingOrderDetailID FROM DyeingOrderDetail WHERE DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE OrderNo LIKE '%" + sOrderNo + "%')))";
            }
            #endregion

            #region RSState & Date
            if (_oDUBatchCost.RSState > 0 && _oDUBatchCost.DateType > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                string sSearchPart = "WHERE RSH.[CurrentStatus]=" +(int) _oDUBatchCost.RSState;
                DateObject.CompareDateQuery(ref sSearchPart, "RSH.EventTime", _oDUBatchCost.DateType, _oDUBatchCost.StartDate, _oDUBatchCost.EndDate);
                sWhereCluse = sWhereCluse + " RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH " + sSearchPart + ")";
            }
            else if ((int)_oDUBatchCost.RSState > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                string sSearchPart = "WHERE RSH.[CurrentStatus]=" + (int)EnumRSState.UnloadedFromDyeMachine ;
                DateObject.CompareDateQuery(ref sSearchPart, "RSH.EventTime", _oDUBatchCost.DateType, _oDUBatchCost.StartDate, _oDUBatchCost.EndDate);
                sWhereCluse = sWhereCluse + " RouteSheetID In (Select RouteSheetID from RouteSheetHistory as RSH " + sSearchPart + ")";
            }
            #endregion

            //#region Dyes / Chemical
            //if (nProductType == 1) //Dyes
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + "ProcessID IN (SELECT Product.ProductID FROM Product WHERE Product.ProductCategoryID IN (SELECT ProductCategoryID FROM ProductCategory WHERE ProductCategoryName='Dyes'))";
            //}
            //else if (nProductType == 2) //Chemical
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + "ProductID IN (SELECT Product.ProductID FROM Product WHERE Product.ProductCategoryID IN (SELECT ProductCategoryID FROM ProductCategory WHERE ProductCategoryName='Chemicals'))";
            //}
            //#endregion


            sSQLQuery = sSQLQuery + sWhereCluse;
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

        public ActionResult PrintDUBatchCost(int nId, double nts)
        {
          
            _oDUBatchCost = new DUBatchCost();
            List<DUBatchCost>  _oDUBatchCostDetails = new  List<DUBatchCost>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            List<RSInQCDetail> oRSInQCDetails = new List<RSInQCDetail>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Lot> _oLots = new List<Lot>();

            string sSQL = "";
            try
            {
                if (nId > 0)
                {
                   _oDUBatchCost.StartDate = DateTime.Now;
                   _oDUBatchCost.EndDate = DateTime.Now;
                   _oDUBatchCosts = DUBatchCost.Gets(_oDUBatchCost.StartDate, _oDUBatchCost.EndDate, "RouteSheetID=" + nId, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   _oDUBatchCostDetails = DUBatchCost.GetsDetail(" and RouteSheetID=" + nId, nId, (int)Session[SessionInfo.currentUserID]);
                    oRSRawLots = RSRawLot.GetsByRSID(nId, (int)Session[SessionInfo.currentUserID]);
                    sSQL = "Select * from View_RSInQCDetail Where RouteSheetID=" +nId;
                    oRSInQCDetails = RSInQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUBatchCosts.Count > 0)
                    {
                        _oDUBatchCost = _oDUBatchCosts[0];
                    }

                    //_oLots = Lot.Gets("Select * from view_Lot as LT where LT.ParentType=106 and Lt.ParentID=" + nId, (int)Session[SessionInfo.currentUserID]);
                    RouteSheetSetup oRSS = new RouteSheetSetup();
                    oRSS = oRSS.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                  
                  
                    if (oRSS.IsRateOnUSD)
                    {
                        oRSRawLots.ForEach(o => o.UnitPrice = o.FCUnitPrice);
                        //_oLots.ForEach(o => o.UnitPrice = o.FCUnitPrice);
                        //_oLots.ForEach(o => o.Currency ="$");
                    }
                    else
                    {
                        _oLots = Lot.Gets("Select * from view_Lot as LT where LT.ParentType=106 and Lt.ParentID=" + nId + " OR LotID IN (SELECT LotID FROM ITransaction WHERE TriggerParentType=106 AND TriggerParentID = " + nId + " AND WorkingUnitID = " + oRSS.WorkingUnitIDWIP + ")", (int)Session[SessionInfo.currentUserID]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptDUBatchCost oReport = new rptDUBatchCost();
            byte[] abytes = oReport.PrepareReport(_oDUBatchCost, _oDUBatchCostDetails, oRSRawLots, oRSInQCDetails, oCompany, oBusinessUnit, _oLots);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintDUBatchCostFromDyesChemical(int nId, double nts)
        {
            string sOrderNo = "";
            _oDUBatchCost = new DUBatchCost();
            List<DUBatchCost> _oDUBatchCostDetails = new List<DUBatchCost>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            List<RSInQCDetail> oRSInQCDetails = new List<RSInQCDetail>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Lot> _oLots = new List<Lot>();

            string sSQL = "";
            try
            {
                if (nId > 0)
                {
                    _oDUBatchCost.StartDate = DateTime.Now;
                    _oDUBatchCost.EndDate = DateTime.Now;
                    _oDUBatchCosts = DUBatchCost.Gets(_oDUBatchCost.StartDate, _oDUBatchCost.EndDate, "RouteSheetID=" + nId, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDUBatchCostDetails = DUBatchCost.GetsDetail(" and RouteSheetID=" + nId, nId, (int)Session[SessionInfo.currentUserID]);
                    oRSRawLots = RSRawLot.GetsByRSID(nId, (int)Session[SessionInfo.currentUserID]);
                    //sSQL = "Select * from View_RSInQCDetail Where RouteSheetID=" + nId;
                    //oRSInQCDetails = RSInQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oDUBatchCosts.Count > 0)
                    {
                        _oDUBatchCost = _oDUBatchCosts[0];
                    }

                    //_oLots = Lot.Gets("Select * from view_Lot as LT where LT.ParentType=106 and Lt.ParentID=" + nId, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptPrintDUBatchCostFromDyesChemical oReport = new rptPrintDUBatchCostFromDyesChemical();
            byte[] abytes = oReport.PrepareReport(_oDUBatchCost, _oDUBatchCostDetails, oRSRawLots, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");

        }

        #endregion
    }
}

