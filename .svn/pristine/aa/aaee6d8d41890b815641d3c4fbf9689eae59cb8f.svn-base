using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Diagnostics;


namespace ESimSolFinancial.Controllers
{

    public class ProductionRegisterController : Controller
    {
        #region Declaration
        ProductionRegister _oProductionRegister = new ProductionRegister();
        List<ProductionRegister> _oProductionRegisters = new List<ProductionRegister>();
        int _nTotalExcelColumn = 0;
        int _nSubTotalColumn = 0;
        #endregion

        #region Functions
        public string MakeQuery(ProductionRegister oProductionRegister)
        {
            string sSQL = " BUID = "+oProductionRegister.BUID;
            
            #region Shift
            if (!string.IsNullOrEmpty(oProductionRegister.ShiftName))
            {
                sSQL += " AND ShiftID IN (" + oProductionRegister.ShiftName+")";
            }
            #endregion

            #region Machine
            if (!string.IsNullOrEmpty(oProductionRegister.MachineName))
            {
                sSQL += " AND MachineID IN (" + oProductionRegister.MachineName + ")";
            }
            #endregion

            #region Product
            if (!string.IsNullOrEmpty(oProductionRegister.ProductName))
            {
                sSQL += " AND ProductionExecutionID IN (SELECT ProductionExecutionID FROM View_ProductionExecution WHERE ProductID IN (" + oProductionRegister.ProductName + "))";
            }
            #endregion

            #region SheetNo
            if (!string.IsNullOrEmpty(oProductionRegister.SheetNo))
            {
                sSQL += " AND ProductionSheetID IN (SELECT ProductionSheetID FROM ProductionSheet WHERE SheetNo Like '%" + oProductionRegister.SheetNo + "%')";
            }
            #endregion

            #region Customer
            if (!string.IsNullOrEmpty(oProductionRegister.CustomerName))
            {
                sSQL += " AND ProductionSheetID IN (SELECT ProductionSheetID FROM ProductionSheet AS PS WHERE PS.PTUUnit2ID IN (SELECT PTUUnit2ID FROM View_PTUUnit2 WHERE ContractorID IN (" + oProductionRegister.CustomerName + ")))";
            }
            #endregion
            
            #region Buyer
            if (!string.IsNullOrEmpty(oProductionRegister.BuyerName))
            {
                sSQL += " AND ProductionSheetID IN (SELECT ProductionSheetID FROM ProductionSheet AS PS WHERE PS.PTUUnit2ID IN (SELECT PTUUnit2ID FROM View_PTUUnit2 WHERE BuyerID IN (" + oProductionRegister.BuyerName + ")))";
            }
            #endregion

            #region ExportPINo
            if (!string.IsNullOrEmpty(oProductionRegister.ExportPINo))
            {
                sSQL += " AND ProductionSheetID IN (SELECT ProductionSheetID FROM ProductionSheet AS PS WHERE PS.PTUUnit2ID IN (SELECT PTUUnit2ID FROM View_PTUUnit2 WHERE ExportPINo LIKE '%" + oProductionRegister.ExportPINo + "%'))";
            }
            #endregion

            #region Transaction Date
            DateObject.CompareDateQuery(ref sSQL, "TransactionDate", oProductionRegister.DateCriteria, oProductionRegister.TransactionStartDate, oProductionRegister.TransactionEndDate);             
            #endregion
            
            return sSQL;
        }
        #endregion

        #region Actions
        public ActionResult ViewProductionRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oProductionRegisters = new List<ProductionRegister>();

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise|| (EnumReportLayout)oItem.id == EnumReportLayout.Machine_Wise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.HRMShifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oProductionRegisters);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ProductionRegister oProductionRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oProductionRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReports(ProductionRegister oProductionRegister)
        {
            _oProductionRegisters = new List<ProductionRegister>();
            string sSQL = MakeQuery(oProductionRegister);
            try
            {
                _oProductionRegisters = ProductionRegister.Gets(sSQL, oProductionRegister.ReportLayout, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionRegister = new ProductionRegister();
                _oProductionRegister.ErrorMessage = ex.Message;
                _oProductionRegisters.Add(_oProductionRegister);
            }
            var jSonResult = Json(_oProductionRegisters, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        public ActionResult PrintProductionRegister(string sIDs)
        {
            _oProductionRegister = new ProductionRegister();
            _oProductionRegister = (ProductionRegister)Session[SessionInfo.ParamObj];
            BusinessUnit oBusinessUnit = new BusinessUnit();
            //int nCount = 0;
            //_oProductionRegister.BUID = Convert.ToInt32(sIDs.Split('~')[nCount++]);
            //_oProductionRegister.ShiftName = sIDs.Split('~')[nCount++];
            //_oProductionRegister.MachineName = sIDs.Split('~')[nCount++];
            //_oProductionRegister.SheetNo = sIDs.Split('~')[nCount++];
            //_oProductionRegister.DateCriteria = Convert.ToInt32(sIDs.Split('~')[nCount++]);
            //_oProductionRegister.TransactionStartDate = Convert.ToDateTime(sIDs.Split('~')[nCount++]);
            //_oProductionRegister.TransactionEndDate = Convert.ToDateTime(sIDs.Split('~')[nCount++]);
            //_oProductionRegister.ProductName = sIDs.Split('~')[nCount++];
            //_oProductionRegister.CustomerName = sIDs.Split('~')[nCount++];
            //_oProductionRegister.BuyerName = sIDs.Split('~')[nCount++];
            //_oProductionRegister.ExportPINo = sIDs.Split('~')[nCount++];
            //_oProductionRegister.ReportLayout = (EnumReportLayout)Convert.ToInt32(sIDs.Split('~')[nCount++]);
            
            string sReportHeading = "";
            if (_oProductionRegister.TransactionStartDate != _oProductionRegister.TransactionEndDate)
            {
                sReportHeading = "Date:( "+_oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy")+" To "+_oProductionRegister.TransactionEndDate.ToString("dd MMM yyyy")+" )";  
            }
            else
            {
                sReportHeading = "Date : "+_oProductionRegister.TransactionStartDate.ToString("dd MMM yyyy");
            }

            string sSql = MakeQuery(_oProductionRegister);
            _oProductionRegisters = ProductionRegister.Gets(sSql,_oProductionRegister.ReportLayout, (int)Session[SessionInfo.currentUserID]);
            oBusinessUnit = oBusinessUnit.Get(_oProductionRegister.BUID, (int)Session[SessionInfo.currentUserID]);
      
            if (_oProductionRegisters.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                rptProductionRegister oReport = new rptProductionRegister();
                byte[] abytes = oReport.PrepareReport(_oProductionRegisters, oCompany, oBusinessUnit, _oProductionRegister.ReportLayout, sReportHeading);
                return File(abytes, "application/pdf");
            }
            else
            {
                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
        }

        public List<TableHeader> GetHeader(EnumReportLayout eReportLayout) 
        {
            #region Header
            _nTotalExcelColumn = 16; _nSubTotalColumn = 8;
            List<TableHeader> table_headers = new List<TableHeader>();
            table_headers.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Sheet No", Width = 15f, IsRotate = false });

            table_headers.Add(new TableHeader { Header = "PI No", Width = 25f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Customer Name", Width = 45f, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Buyer Name", Width = 45f, IsRotate = false });

            if (eReportLayout != EnumReportLayout.ProductWise)
            {
                _nTotalExcelColumn = 17; _nSubTotalColumn = 9;
                table_headers.Add(new TableHeader { Header = "Product Code", Width = 20f, IsRotate = false });
                table_headers.Add(new TableHeader { Header = "Product Name", Width = 45f, IsRotate = false });
            }

            if (eReportLayout == EnumReportLayout.DateWise)
            {
                table_headers.Add(new TableHeader { Header = "Shift Name", Width = 45f, IsRotate = false });
                table_headers.Add(new TableHeader { Header = "Machine Name", Width = 45f, IsRotate = false });
            }
            else if (eReportLayout == EnumReportLayout.Machine_Wise)
            {
                table_headers.Add(new TableHeader { Header = "Transaction Date", Width = 25f, IsRotate = false });
                table_headers.Add(new TableHeader { Header = "Shift Name", Width = 45f, IsRotate = false });
            }
            else
            {
                table_headers.Add(new TableHeader { Header = "Transaction Date", Width = 25f, IsRotate = false });
                table_headers.Add(new TableHeader { Header = "Shift Name", Width = 45f, IsRotate = false });
                table_headers.Add(new TableHeader { Header = "Machine Name", Width = 45f, IsRotate = false });
            }

            table_headers.Add(new TableHeader { Header = "M Unit", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Sheet Qty", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Production Qty", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Total Production", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Yet To Production", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "QC Pass", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "QC Reject", Width = 20, IsRotate = false });
            table_headers.Add(new TableHeader { Header = "Yet To QC", Width = 20, IsRotate = false });
            #endregion

            return table_headers;
        }
        public void ExportToExcelProductionRegister(double ts)
        {
            int ProductionSheetID=-999;
            string Header = "", HeaderColumn = "", _sErrorMesage = "";

            #region Get Data From DB
            Company oCompany = new Company();
            ProductionRegister oProductionRegister = new ProductionRegister();
            try
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oProductionRegisters = new List<ProductionRegister>();
                oProductionRegister = (ProductionRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.MakeQuery(oProductionRegister);
                _oProductionRegisters = ProductionRegister.Gets(sSQL, oProductionRegister.ReportLayout, (int)Session[SessionInfo.currentUserID]);
                if (_oProductionRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oProductionRegisters = new List<ProductionRegister>();
                _sErrorMesage = ex.Message;
            }
            #endregion
            
            if (_sErrorMesage == "") 
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header = GetHeader(oProductionRegister.ReportLayout);
                #endregion

                #region Layout Wise Header
                if (oProductionRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    Header = "Product Wise"; HeaderColumn = "Product Name : ";
                }
                else if (oProductionRegister.ReportLayout == EnumReportLayout.Machine_Wise)
                {
                    Header = "Machine Wise"; HeaderColumn = "Machine Name : ";
                }
                else if (oProductionRegister.ReportLayout == EnumReportLayout.DateWise)
                {
                    Header = "Date Wise"; HeaderColumn = "Transaction Date : ";
                }
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Production Register");
                    sheet.Name = "Production Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Production Register (" + Header+") "; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Group By Layout Wises
                    List<ProductionRegister> GroupWiseData=new List<ProductionRegister>();

                    if (oProductionRegister.ReportLayout == EnumReportLayout.DateWise)
                    {
                        GroupWiseData = _oProductionRegisters.GroupBy(x => new { x.TransactionDateSt }, (key, grp) => new ProductionRegister
                        {
                            RowHeader = key.TransactionDateSt,
                            MoldingProduction = grp.Sum(x => x.MoldingProduction),
                            Results = grp.ToList()
                        }).ToList();
                    }
                    else if (oProductionRegister.ReportLayout == EnumReportLayout.ProductWise)
                    {
                        GroupWiseData = _oProductionRegisters.GroupBy(x => new { x.ProductID,x.ProductName }, (key, grp) => new ProductionRegister
                        {
                            RowHeader = key.ProductName,
                            MoldingProduction = grp.Sum(x => x.MoldingProduction),
                            Results = grp.ToList()
                        }).ToList();
                    }
                    else if (oProductionRegister.ReportLayout == EnumReportLayout.Machine_Wise)
                    {
                        GroupWiseData = _oProductionRegisters.GroupBy(x => new { x.MachineID, x.MachineName, }, (key, grp) => new ProductionRegister
                        {
                            RowHeader = key.MachineName,
                            MoldingProduction = grp.Sum(x => x.MoldingProduction),
                            Results = grp.ToList()
                        }).ToList();
                    }
                    #endregion

                    ExcelTool.Formatter = " #,##0.00;(#,##0.00)";

                    #region Data
                    foreach (var oItem in GroupWiseData)
                    {
                        nRowIndex++;

                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, HeaderColumn + oItem.RowHeader, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        nRowIndex++;
                        foreach (TableHeader listItem in table_header)
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        ProductionSheetID = 0;
                        nRowIndex++; int nCount = 0, nRowSpan = 0;
                        foreach (var obj in oItem.Results)
                        {
                            #region Sheet Wise Merge
                            if (ProductionSheetID != obj.ProductionSheetID)
                            {
                                if (nCount > 0)
                                {
                                    nStartCol = _nSubTotalColumn;
                                    ExcelTool.FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, false);
                                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.MoldingProduction).ToString(), true, true);
                                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                                  
                                    nRowIndex++;
                                }

                                nStartCol = 2;
                                nRowSpan = oItem.Results.Where(Sheet => Sheet.ProductionSheetID == obj.ProductionSheetID).ToList().Count;

                                ExcelTool.FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.SheetNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                ExcelTool.FillCellMerge(ref sheet, obj.ExportPINo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.CustomerName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.BuyerName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                if (oProductionRegister.ReportLayout != EnumReportLayout.ProductWise)
                                {
                                    ExcelTool.FillCellMerge(ref sheet, obj.ProductCode, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                    ExcelTool.FillCellMerge(ref sheet, obj.ProductName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                }

                                //ExcelTool.FillCellMerge(ref sheet, obj.UnitSymbol, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            }
                            #endregion
                            nStartCol = _nSubTotalColumn;
                            if (oProductionRegister.ReportLayout == EnumReportLayout.ProductWise)
                                nStartCol = 7;

                            if (oProductionRegister.ReportLayout == EnumReportLayout.DateWise)
                            {
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ShiftName, false);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MachineName, false);
                            }
                            else if (oProductionRegister.ReportLayout == EnumReportLayout.Machine_Wise)
                            {
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.TransactionDateSt, false);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ShiftName, false);
                            }
                            else 
                            {
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.TransactionDateSt, false);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ShiftName, false);
                                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MachineName, false);
                            }

                            if (ProductionSheetID != obj.ProductionSheetID)
                            {
                                nRowSpan = oItem.Results.Where(OrderR => OrderR.ProductionSheetID == obj.ProductionSheetID).ToList().Count-1;
                                ExcelTool.FillCellMerge(ref sheet, obj.UnitSymbol, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                ExcelTool.FillCellMerge(ref sheet, obj.SheetQty, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false);
                            }
                            nStartCol = _nTotalExcelColumn-4;

                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MoldingProduction.ToString(), true);
                            //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ActualMoldingProduction.ToString(), true);
                            //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ActualFinishGoods.ToString(), true);
                            //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ActualRejectGoods.ToString(), true);
                            //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.YetToProduction.ToString(), true);

                            if (ProductionSheetID != obj.ProductionSheetID)
                            {
                                nRowSpan = oItem.Results.Where(OrderR => OrderR.ProductionSheetID == obj.ProductionSheetID).ToList().Count - 1;
                                ExcelTool.FillCellMerge(ref sheet, obj.ActualMoldingProduction, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false);
                                ExcelTool.FillCellMerge(ref sheet, obj.YetToModling, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false);
                                ExcelTool.FillCellMerge(ref sheet, obj.ActualFinishGoods, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false);
                                ExcelTool.FillCellMerge(ref sheet, obj.ActualRejectGoods, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false);
                                ExcelTool.FillCellMerge(ref sheet, obj.YetToProduction, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false);
                            }

                            nRowIndex++;

                            ProductionSheetID = obj.ProductionSheetID;
                        }
                        #region SubTotal
                        nStartCol = _nSubTotalColumn;
                        ExcelTool.FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, false);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.MoldingProduction).ToString(), true, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        nRowIndex++;
                        #endregion

                        #region SubTotal (layout Wise)
                        nStartCol = 2; 
                        ExcelTool.Formatter = " #,##0.00;(#,##0.00)";

                        ExcelTool.FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += _nSubTotalColumn, true, ExcelHorizontalAlignment.Right);

                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, false);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, oItem.MoldingProduction.ToString(), true, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        nRowIndex++;
                        #endregion
                    }
                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                    ExcelTool.FillCellMerge(ref sheet, Header + " Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += _nSubTotalColumn, true, ExcelHorizontalAlignment.Right);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, _oProductionRegisters.Sum(x => x.MoldingProduction).ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                    ExcelTool.FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, _nTotalExcelColumn+2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ProductionRegister(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
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
    }
}