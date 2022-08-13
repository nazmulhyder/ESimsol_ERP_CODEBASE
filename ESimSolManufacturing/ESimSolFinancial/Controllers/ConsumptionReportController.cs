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

namespace ESimSolFinancial.Controllers
{
    public class ConsumptionReportController : Controller
    {
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<ConsumptionReport> _oConsumptionReports = new List<ConsumptionReport>();
        ConsumptionUnit _oConsumptionUnit = new ConsumptionUnit();
        List<ConsumptionUnit> _oConsumptionUnits = new List<ConsumptionUnit>();
        TConsumptionUnit _oTConsumptionUnit = new TConsumptionUnit();
        List<TConsumptionUnit> _oTConsumptionUnits = new List<TConsumptionUnit>();

        #region Actions
        public ActionResult ViewConsumptionReports(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ConsumptionReport oConsumptionReport = new ConsumptionReport();

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.ProductSummary || (EnumReportLayout)oItem.id == EnumReportLayout.Consumption_Group_Summary)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.BusinessSessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            //ViewBag.WorkingUnits = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.Shifts = EnumObject.jGets(typeof(EnumShift));
            ViewBag.ReportLayouts = oReportLayouts;
            return View(oConsumptionReport);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ConsumptionReport oConsumptionReport)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oConsumptionReport);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintConsumptionReport(double ts)
        {
            ConsumptionReport oConsumptionReport = new ConsumptionReport();
            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ConsumptionRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            try
            {
                _sErrorMesage = "";
                _oConsumptionReports = new List<ConsumptionReport>();
                oConsumptionReport = (ConsumptionReport)Session[SessionInfo.ParamObj];


                if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.Consumption_Group_Summary)
                {
                    string sSQL = this.GetSQLForSummary(oConsumptionReport);
                    _oConsumptionReports = ConsumptionReport.GetsConsumptionSummary(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    string sSQL = this.GetSQL(oConsumptionReport);
                    _oConsumptionReports = ConsumptionReport.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }

                if (_oConsumptionReports.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oConsumptionReports = new List<ConsumptionReport>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.Consumption_Group_Summary)
                {
                    rptConsumptionGroupSummary oReport = new rptConsumptionGroupSummary();
                    byte[] abytes = oReport.PrepareReport(_oConsumptionReports, oCompany, _sDateRange, oConsumptionReport.ShiftName, oConsumptionReport.ReportLayout, bIsRateView);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptConsumptionReport oReport = new rptConsumptionReport();
                    byte[] abytes = oReport.PrepareReport(_oConsumptionReports, oCompany, _sDateRange, oConsumptionReport.ShiftName, oConsumptionReport.ReportLayout, bIsRateView);
                    return File(abytes, "application/pdf");
                }

            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        #endregion

        #region Excel
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, ExcelHorizontalAlignment HoriAlign)
        {
            return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false, HoriAlign);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold, ExcelHorizontalAlignment HoriAlign)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol++];
            if (IsNumber)
                cell.Value = Convert.ToDouble(sVal);
            else
                cell.Value = sVal;
            cell.Style.Font.Bold = IsBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = HoriAlign;

            if (IsNumber)
            {
                cell.Style.Numberformat.Format = _sFormatter;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }
        public void ExportToExcelConsumptionReport(double ts)
        {
            ConsumptionReport oConsumptionReport = new ConsumptionReport();
            string sShiftName = "";
            Company oCompany = new Company();

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ConsumptionRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            try
            {
                _sErrorMesage = "";
                _oConsumptionReports = new List<ConsumptionReport>();
                oConsumptionReport = (ConsumptionReport)Session[SessionInfo.ParamObj];


                if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.Consumption_Group_Summary)
                {
                    string sSQL = this.GetSQLForSummary(oConsumptionReport);
                    _oConsumptionReports = ConsumptionReport.GetsConsumptionSummary(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    string sSQL = this.GetSQL(oConsumptionReport);
                    _oConsumptionReports = ConsumptionReport.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }

                if (_oConsumptionReports.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
                else
                {
                    if (_sErrorMesage == "")
                    {
                        sShiftName = oConsumptionReport.ShiftName;
                        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                        if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.Consumption_Group_Summary)
                        {
                            ConsumptionGroupSummary(_oConsumptionReports, oCompany, _sDateRange, oConsumptionReport.ShiftName, oConsumptionReport.ReportLayout, bIsRateView);
                        }
                        else
                        {
                            string Header = "", HeaderColumn = "";

                            #region Header
                            List<TableHeader> table_header = new List<TableHeader>();
                            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false });
                            if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.DateWiseDetails)
                            {
                                table_header.Add(new TableHeader { Header = "RefNo", Width = 25f, IsRotate = false });
                            }
                            table_header.Add(new TableHeader { Header = "Code", Width = 25f, IsRotate = false });
                            table_header.Add(new TableHeader { Header = "Product Name", Width = 15f, IsRotate = false });
                            table_header.Add(new TableHeader { Header = "Product Category", Width = 45f, IsRotate = false });
                            table_header.Add(new TableHeader { Header = "Color Name", Width = 19f, IsRotate = false });
                            table_header.Add(new TableHeader { Header = "Size Name", Width = 15f, IsRotate = false });
                            table_header.Add(new TableHeader { Header = "M. Unit", Width = 17f, IsRotate = false });
                            table_header.Add(new TableHeader { Header = "Issue Qty", Width = 17f, IsRotate = false });
                            table_header.Add(new TableHeader { Header = "Date", Width = 17f, IsRotate = false });
                            if (bIsRateView)
                            {
                                table_header.Add(new TableHeader { Header = "Unit Price", Width = 17f, IsRotate = false });
                                table_header.Add(new TableHeader { Header = "Value", Width = 17f, IsRotate = false });
                            }
                            table_header.Add(new TableHeader { Header = "BU Name", Width = 17f, IsRotate = false });
                            table_header.Add(new TableHeader { Header = "Store Name", Width = 17f, IsRotate = false });

                            #endregion

                            #region Layout Wise Header
                            if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.ProductSummary)
                            {
                                Header = "Product Wise";
                            }
                            if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.DateWiseDetails)
                            {
                                Header = "Date Wise";
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
                                var sheet = excelPackage.Workbook.Worksheets.Add("Loan Register");
                                sheet.Name = "Consumption Report";

                                foreach (TableHeader listItem in table_header)
                                {
                                    sheet.Column(nStartCol++).Width = listItem.Width;
                                }

                                #region Report Header
                                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = "Consumption Report (" + Header + ") "; cell.Style.Font.Bold = true;
                                cell.Style.WrapText = true;
                                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                nRowIndex = nRowIndex + 1;
                                #endregion

                                #region Address & Date
                                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.WrapText = true;
                                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                nRowIndex = nRowIndex + 1;
                                #endregion

                                string sCurrencySymbol = "";
                                #region Data

                                nRowIndex++;
                                nStartCol = 2;

                                foreach (TableHeader listItem in table_header)
                                {
                                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                }

                                if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.ProductSummary)
                                {
                                    List<ConsumptionReport> oProductSummary = new List<ConsumptionReport>();
                                    oProductSummary = _oConsumptionReports.OrderBy(x => x.ConsumptionUnitID).ThenBy(x => x.TransactionTime).ToList();
                                    _oConsumptionReports = oProductSummary;

                                    oProductSummary = _oConsumptionReports.GroupBy(x => new { x.BUID, x.ConsumptionUnitID, x.StoreID, x.ProductID }).Select(group => new ConsumptionReport
                                    {
                                        BUID = group.First().BUID,
                                        ParentSequence = group.First().ParentSequence,
                                        CUSequence = group.First().CUSequence,
                                        BUName = group.First().BUName,
                                        StoreID = group.First().StoreID,
                                        StoreName = group.First().StoreName,
                                        ProductID = group.First().ProductID,
                                        ProductCode = group.First().ProductCode,
                                        ProductName = group.First().ProductName,
                                        ProductCategoryID = group.First().ProductCategoryID,
                                        ProductCategoryName = group.First().ProductCategoryName,
                                        ColorName = group.First().ColorName,
                                        SizeName = group.First().SizeName,
                                        MUnitSymbol = group.First().MUnitSymbol,
                                        TransactionTimeInString = group.First().TransactionTime.ToString("dd MMM yyyy") + "-" + group.Last().TransactionTime.ToString("dd MMM yyyy"),
                                        IssueQty = group.Sum(y => y.IssueQty),
                                        UnitPrice = group.Sum(y => y.IssueQty * y.UnitPrice) / group.Sum(y => y.IssueQty),
                                        ConsumptionValue = group.Sum(y => y.ConsumptionValue),
                                        ConsumptionUnitID = group.First().ConsumptionUnitID,
                                        ConsumptionUnitName = group.First().ConsumptionUnitName
                                    }).OrderBy(y => y.ConsumptionUnitID).ToList();

                                    _oConsumptionReports = oProductSummary.OrderBy(y => y.ParentSequence).ThenBy(x => x.CUSequence).ToList();

                                }

                                int nConsumptionUnitID = -1; int nCount = 0; double nSubTotal = 0; double nGrandTotal = 0; double nSubTotalQty = 0; double nGrandTotalQty = 0;
                                int rowspan = _oConsumptionReports.Select(i => i.ConsumptionUnitID).Count();

                                foreach (var oItem in _oConsumptionReports)
                                {
                                    nRowIndex++;
                                    nStartCol = 2;

                                    #region Product Wise Merge
                                    if (nConsumptionUnitID != oItem.ConsumptionUnitID)
                                    {
                                        if (nSubTotal > 0)
                                        {
                                            nStartCol = 2;

                                            if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.DateWiseDetails)
                                            {
                                                FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 7, true, ExcelHorizontalAlignment.Right);
                                            }
                                            else
                                            {
                                                FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                                            }

                                            nStartCol++;
                                            _sFormatter = " #,##0;(#,##0)";
                                            FillCell(sheet, nRowIndex, nStartCol, nSubTotalQty.ToString(), true, ExcelHorizontalAlignment.Right);
                                            nStartCol++;
                                            if (bIsRateView)
                                            {
                                                FillCell(sheet, nRowIndex, nStartCol, "", false, ExcelHorizontalAlignment.Right);
                                                nStartCol++;
                                                _sFormatter = " #,##0;(#,##0)";
                                                FillCell(sheet, nRowIndex, nStartCol, nSubTotal.ToString(), true, ExcelHorizontalAlignment.Right);
                                                nStartCol++;
                                            }
                                            FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nEndCol + 1, false, ExcelHorizontalAlignment.Right);
                                            nRowIndex++;
                                        }

                                        nStartCol = 2;
                                        #region Consumption Unit Heading
                                        FillCellMerge(ref sheet, sShiftName != "" && sShiftName != null ? "Consumption Unit : " + oItem.ConsumptionUnitName + " Shift :" + sShiftName : "Consumption Unit : " + oItem.ConsumptionUnitName, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);
                                        nRowIndex++;
                                        #endregion

                                        nCount = 0;
                                        nSubTotal = 0;
                                        nSubTotalQty = 0;

                                    }
                                    #endregion

                                    nStartCol = 2;
                                    FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true, ExcelHorizontalAlignment.Right);

                                    if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.DateWiseDetails)
                                    {
                                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FileNo.ToString(), false, ExcelHorizontalAlignment.Left);
                                    }

                                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductCode.ToString(), false, ExcelHorizontalAlignment.Left);
                                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductName.ToString(), false, ExcelHorizontalAlignment.Left);
                                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductCategoryName.ToString(), false, ExcelHorizontalAlignment.Left);
                                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorName.ToString(), false, ExcelHorizontalAlignment.Left);
                                    FillCell(sheet, nRowIndex, nStartCol++, oItem.SizeName.ToString(), false, ExcelHorizontalAlignment.Left);
                                    FillCell(sheet, nRowIndex, nStartCol++, oItem.MUnitSymbol.ToString(), false, ExcelHorizontalAlignment.Left);
                                    _sFormatter = " #,##0;(#,##0)";
                                    FillCell(sheet, nRowIndex, nStartCol++, oItem.IssueQty.ToString(), true, ExcelHorizontalAlignment.Right);
                                    FillCell(sheet, nRowIndex, nStartCol++, oItem.TransactionTime.ToString("dd MMM yyyy"), false, ExcelHorizontalAlignment.Center);
                                    if (bIsRateView)
                                    {
                                        _sFormatter = " #,##0;(#,##0)";
                                        FillCell(sheet, nRowIndex, nStartCol++, " " + oItem.UnitPrice.ToString(), true, ExcelHorizontalAlignment.Right);
                                        _sFormatter = " #,##0;(#,##0)";
                                        FillCell(sheet, nRowIndex, nStartCol++, " " + oItem.ConsumptionValue.ToString(), true, ExcelHorizontalAlignment.Right);
                                    }
                                    FillCell(sheet, nRowIndex, nStartCol++, " " + oItem.BUName, false, ExcelHorizontalAlignment.Left);
                                    FillCell(sheet, nRowIndex, nStartCol++, " " + oItem.StoreName, false, ExcelHorizontalAlignment.Left);

                                    nConsumptionUnitID = oItem.ConsumptionUnitID;
                                    nSubTotal = nSubTotal + oItem.ConsumptionValue;
                                    nGrandTotal = nGrandTotal + oItem.ConsumptionValue;

                                    nSubTotalQty = nSubTotalQty + oItem.IssueQty;
                                    nGrandTotalQty = nGrandTotalQty + oItem.IssueQty;

                                }

                                nStartCol = 2;

                                if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.DateWiseDetails)
                                {
                                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 7, true, ExcelHorizontalAlignment.Right);
                                }
                                else
                                {
                                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                                }
                                nStartCol++;
                                _sFormatter = " #,##0;(#,##0)";
                                FillCell(sheet, nRowIndex, nStartCol, nSubTotalQty.ToString(), true, ExcelHorizontalAlignment.Right);
                                nStartCol++;
                                if (bIsRateView)
                                {
                                    FillCell(sheet, nRowIndex, nStartCol, "", false, ExcelHorizontalAlignment.Right);
                                    nStartCol++;
                                    _sFormatter = " #,##0;(#,##0)";
                                    FillCell(sheet, nRowIndex, nStartCol, nSubTotal.ToString(), true, ExcelHorizontalAlignment.Right);
                                    nStartCol++;
                                }
                                FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Right);
                                nRowIndex++;

                                #region Grand Total
                                nStartCol = 2;

                                if (oConsumptionReport.ReportLayout == (int)EnumReportLayout.DateWiseDetails)
                                {
                                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 7, true, ExcelHorizontalAlignment.Right);
                                }
                                else
                                {
                                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                                }

                                nStartCol++;
                                _sFormatter = " #,##0;(#,##0)";
                                FillCell(sheet, nRowIndex, nStartCol, nGrandTotalQty.ToString(), true, ExcelHorizontalAlignment.Right);
                                nStartCol++;
                                if (bIsRateView)
                                {
                                    FillCell(sheet, nRowIndex, nStartCol, "", false, ExcelHorizontalAlignment.Right);
                                    nStartCol++;
                                    _sFormatter = " #,##0;(#,##0)";
                                    FillCell(sheet, nRowIndex, nStartCol, nGrandTotal.ToString(), true, ExcelHorizontalAlignment.Right);
                                    nStartCol++;
                                }
                                FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nEndCol + 1, false, ExcelHorizontalAlignment.Right);

                                nRowIndex++;
                                #endregion

                                cell = sheet.Cells[1, 1, nRowIndex, 13];
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(Color.White);
                                #endregion

                                Response.ClearContent();
                                Response.BinaryWrite(excelPackage.GetAsByteArray());
                                Response.AddHeader("content-disposition", "attachment; filename=ConsumptionReport(" + Header + ").xlsx");
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.Flush();
                                Response.End();
                            }
                            #endregion
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _oConsumptionReports = new List<ConsumptionReport>();
                _sErrorMesage = ex.Message;
            }

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
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
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

        #region Support Functions
        private void ConsumptionGroupSummary(List<ConsumptionReport> oConsumptionReports, Company oCompany, string sDateRange, string ShiftName, int ReportLayout, bool bIsRateView)
        {
            List<ConsumptionReport> _oConsumptionReports = oConsumptionReports;
            Company _oCompany = oCompany;
            EnumReportLayout _eEnumReportLayout = (EnumReportLayout)ReportLayout;
            string _sShiftName = ShiftName;
            bool _bIsRateView = bIsRateView;

            string Header = "", HeaderColumn = "";

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Code", Width = 25f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 25f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Unit", Width = 17f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Unit Price", Width = 17f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty", Width = 17f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amount", Width = 17f, IsRotate = false });
            Header = "Consumption Group Summary";
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Loan Register");
                sheet.Name = "Consumption Report";

                foreach (TableHeader listItem in table_header)
                {
                    sheet.Column(nStartCol++).Width = listItem.Width;
                }

                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[nRowIndex, 5, nRowIndex, nEndCol + 1]; cell.Merge = true;
                cell.Value = "Consumption Report (" + Header + ") "; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 5]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                
                cell = sheet.Cells[nRowIndex, 6, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                string sCurrencySymbol = "";
                #region Data

                nRowIndex++;
                nStartCol = 2;

                foreach (TableHeader listItem in table_header)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                #region group by
                if (_oConsumptionReports.Count > 0)
                {
                    var data = _oConsumptionReports.GroupBy(x => new { x.CUGroupName }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                    {
                        CUGroupName = key.CUGroupName,
                        Results = grp.ToList() //All data
                    });

                #endregion

                    int nConsumptionUnitID = -1; int nCount = 0; double nSubTotal = 0; double nGrandTotal = 0; double nSubTotalQty = 0; double nGrandTotalQty = 0;
                    int rowspan = _oConsumptionReports.Select(i => i.ConsumptionUnitID).Count();

                    foreach (var oItem in data)
                    {
                        nStartCol = 2;
                        nRowIndex++;
                        #region Consumption Group Wise Merge

                        if (nSubTotal > 0)
                        {
                            nStartCol = 2;
                            nRowIndex--;
                            FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Right);
                            nStartCol++;
                            FillCell(sheet, nRowIndex, nStartCol, nSubTotalQty.ToString("###,0.00"), true, ExcelHorizontalAlignment.Right);
                            nStartCol++;
                            FillCell(sheet, nRowIndex, nStartCol, nSubTotal.ToString("###,0.00"), true, ExcelHorizontalAlignment.Right);
                            nRowIndex++;

                            nSubTotal = 0;
                            nSubTotalQty = 0;
                        }

                        nStartCol = 2;
                        #region Consumption Group Heading
                        FillCellMerge(ref sheet, oItem.CUGroupName, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);
                        nRowIndex++;
                        #endregion

                        nCount = 0; int num = 0;

                        foreach (var obj in oItem.Results)
                        {
                            nStartCol = 2;
                            FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true, ExcelHorizontalAlignment.Right);

                            FillCell(sheet, nRowIndex, nStartCol++, obj.ProductCode.ToString(), false, ExcelHorizontalAlignment.Left);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName.ToString(), false, ExcelHorizontalAlignment.Left);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.MUnitSymbol.ToString(), false, ExcelHorizontalAlignment.Left);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.UnitPrice.ToString("###,0.00"), false, ExcelHorizontalAlignment.Right);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.IssueQty.ToString(), false, ExcelHorizontalAlignment.Right);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.ConsumptionValue.ToString("###,0.00"), false, ExcelHorizontalAlignment.Right);

                            nSubTotal = nSubTotal + obj.ConsumptionValue;
                            nSubTotalQty = nSubTotalQty + obj.IssueQty;
                            nGrandTotal = nGrandTotal + obj.ConsumptionValue;
                            nGrandTotalQty = nGrandTotalQty + obj.IssueQty;
                            nRowIndex++;
                        }

                        #endregion

                    }

                    nStartCol = 2;
                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 4, false, ExcelHorizontalAlignment.Right);
                    nStartCol++;
                    FillCell(sheet, nRowIndex, nStartCol, nSubTotalQty.ToString("###,0.00"), false, ExcelHorizontalAlignment.Right);
                    nStartCol++;
                    FillCell(sheet, nRowIndex, nStartCol, nSubTotal.ToString("###,0.00"), false, ExcelHorizontalAlignment.Right);
                    nRowIndex++;

                    #region Grand Total
                    nStartCol = 2;
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 4, false, ExcelHorizontalAlignment.Right);
                    nStartCol++;
                    FillCell(sheet, nRowIndex, nStartCol, nGrandTotalQty.ToString("###,0.00"), false, ExcelHorizontalAlignment.Right);
                    nStartCol++;
                    FillCell(sheet, nRowIndex, nStartCol, nGrandTotal.ToString("###,0.00"), false, ExcelHorizontalAlignment.Right);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 13];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ConsumptionReport(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
            #endregion

        }

        private string GetSQLForSummary(ConsumptionReport oConsumptionReport)
        {
            _sDateRange = "";
            string sSearchingData = oConsumptionReport.SearchingData;
            EnumCompareOperator eIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sAndCluse = "";

            #region BusinessUnit
            if (oConsumptionReport.BUID > 0)
            {
                sAndCluse = sAndCluse + " AND CR.BUID =" + oConsumptionReport.BUID.ToString();
            }
            #endregion
            #region BusinessUnit
            if (!string.IsNullOrEmpty(oConsumptionReport.StyleNo))
            {

                sAndCluse = sAndCluse + " AND CRD.LotID IN (SELECT LotID FROM Lot WHERE StyleID IN (" + oConsumptionReport.StyleNo + "))";
            }
            #endregion

            #region LotNo
            if (oConsumptionReport.LotNo != null && oConsumptionReport.LotNo != "")
            {
                sAndCluse = sAndCluse + " AND CRD.LotID IN (SELECT LotID FROM Lot WHERE  LotNo LIKE'%" + oConsumptionReport.LotNo + "%')";
            }
            #endregion

            #region StoreID
            if (!string.IsNullOrEmpty(oConsumptionReport.StoreName))
            {
                sAndCluse = sAndCluse + " AND CR.StoreID IN (" + oConsumptionReport.StoreName + ")";
            }
            #endregion

            #region Consumption Unit
            if (oConsumptionReport.ConsumptionUnitName != null && oConsumptionReport.ConsumptionUnitName != "")
            {
                sAndCluse = sAndCluse + " AND CR.RequisitionFor IN (" + oConsumptionReport.ConsumptionUnitName + ")";
            }

            #endregion

            #region Shift
            if (oConsumptionReport.ShiftInInt != 0)
            {
                sAndCluse = sAndCluse + " AND  ISNULL(CR.Shift,0) = " + oConsumptionReport.ShiftInInt;
            }
            #endregion

            #region Product
            if (oConsumptionReport.ProductName != null && oConsumptionReport.ProductName != "")
            {
                sAndCluse = sAndCluse + " AND  CRD.ProductID IN(" + oConsumptionReport.ProductName + ")";
            }
            #endregion

            #region Product Category
            if (oConsumptionReport.ProductCategoryID > 0)
            {

                sAndCluse = sAndCluse + "AND  CRD.ProductID IN (SELECT NN.ProductID FROM Product AS NN WHERE NN.ProductCategoryID IN (SELECT MM.ProductCategoryID FROM dbo.fn_GetProductCategory(" + oConsumptionReport.ProductCategoryID.ToString() + ") AS MM))";
            }
            #endregion

            #region Consumption Date
            if (eIssueDate != EnumCompareOperator.None)
            {
                if (eIssueDate == EnumCompareOperator.EqualTo)
                {
                    _sDateRange = "Report Date EqualTo : " + dIssueStartDate.ToString("dd MMM yyyy");

                    sAndCluse = sAndCluse + "AND CONVERT(DATE,CONVERT(VARCHAR(12),CR.IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";

                }
                else if (eIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    _sDateRange = "Report Date Not EqualTo : " + dIssueStartDate.ToString("dd MMM yyyy");
                    sAndCluse = sAndCluse + "AND CONVERT(DATE,CONVERT(VARCHAR(12),CR.IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eIssueDate == EnumCompareOperator.GreaterThan)
                {
                    _sDateRange = "Report Date Greater Then : " + dIssueStartDate.ToString("dd MMM yyyy");
                    sAndCluse = sAndCluse + " AND CONVERT(DATE,CONVERT(VARCHAR(12),CR.IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eIssueDate == EnumCompareOperator.SmallerThan)
                {
                    _sDateRange = "Consumption Date Smaller Then : " + dIssueStartDate.ToString("dd MMM yyyy");
                    sAndCluse = sAndCluse + " AND CONVERT(DATE,CONVERT(VARCHAR(12),CR.IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eIssueDate == EnumCompareOperator.Between)
                {
                    _sDateRange = "Report Date : " + dIssueStartDate.ToString("dd MMM yyyy") + " --to-- " + dIssueEndDate.ToString("dd MMM yyyy");
                    sAndCluse = sAndCluse + "AND CONVERT(DATE,CONVERT(VARCHAR(12),CR.IssueDate,106)) Between  CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eIssueDate == EnumCompareOperator.NotBetween)
                {
                    _sDateRange = "Report Date Not Between: " + dIssueStartDate.ToString("dd MMM yyyy") + " --to-- " + dIssueEndDate.ToString("dd MMM yyyy");
                    sAndCluse = sAndCluse + "AND CONVERT(DATE,CONVERT(VARCHAR(12),CR.IssueDate,106)) NOT Between  CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            return sAndCluse;
        }
        private string GetSQL(ConsumptionReport oConsumptionReport)
        {
            _sDateRange = "";
            string sSearchingData = oConsumptionReport.SearchingData;
            EnumCompareOperator eIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sWhereCluse = "";

            #region BusinessUnit
            if (oConsumptionReport.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.LotID IN (SELECT LotID FROM Lot WHERE BUID =" + oConsumptionReport.BUID.ToString() + ")";
            }
            #endregion
            #region BusinessUnit
            if (!string.IsNullOrEmpty(oConsumptionReport.StyleNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.LotID IN (SELECT LotID FROM Lot WHERE StyleID IN (" + oConsumptionReport.StyleNo + "))";
            }
            #endregion

            #region LotNo
            if (oConsumptionReport.LotNo != null && oConsumptionReport.LotNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.LotID IN (SELECT LotID FROM Lot WHERE  LotNo LIKE'%" + oConsumptionReport.LotNo + "%')";
            }
            #endregion

            #region StoreID
            if (!string.IsNullOrEmpty(oConsumptionReport.StoreName))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.WorkingUnitID IN (" + oConsumptionReport.StoreName + ")";
            }
            #endregion

            #region Consumption Unit
            if (oConsumptionReport.ConsumptionUnitName != null && oConsumptionReport.ConsumptionUnitName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.TriggerParentType = 118 AND HH.TriggerParentID IN(SELECT MM.ConsumptionRequisitionDetailID FROM ConsumptionRequisitionDetail AS MM WHERE MM.ConsumptionRequisitionID IN (SELECT NN.ConsumptionRequisitionID FROM ConsumptionRequisition AS NN WHERE NN.CRType = 1 AND NN.RequisitionFor IN (" + oConsumptionReport.ConsumptionUnitName + ")))";
            }
            else
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.TriggerParentType = 118 AND HH.TriggerParentID IN(SELECT MM.ConsumptionRequisitionDetailID FROM ConsumptionRequisitionDetail AS MM WHERE MM.ConsumptionRequisitionID IN (SELECT NN.ConsumptionRequisitionID FROM ConsumptionRequisition AS NN WHERE NN.CRType = 1))";
            }
            #endregion

            #region Shift
            if (oConsumptionReport.ShiftInInt != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.TriggerParentType = 118 AND HH.TriggerParentID IN(SELECT MM.ConsumptionRequisitionDetailID FROM ConsumptionRequisitionDetail AS MM WHERE MM.ConsumptionRequisitionID IN (SELECT NN.ConsumptionRequisitionID FROM ConsumptionRequisition AS NN WHERE NN.CRType = 1 AND NN.Shift = " + oConsumptionReport.ShiftInInt + "))";
            }
            #endregion

            #region Product
            if (oConsumptionReport.ProductName != null && oConsumptionReport.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.ProductID IN(" + oConsumptionReport.ProductName + ")";
            }
            #endregion

            #region Product Category
            if (oConsumptionReport.ProductCategoryID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " HH.ProductID IN (SELECT NN.ProductID FROM View_Product AS NN WHERE NN.ProductCategoryID IN (SELECT MM.ProductCategoryID FROM dbo.fn_GetProductCategory(" + oConsumptionReport.ProductCategoryID.ToString() + ") AS MM))";
            }
            #endregion

            #region Consumption Date
            if (eIssueDate != EnumCompareOperator.None)
            {
                if (eIssueDate == EnumCompareOperator.EqualTo)
                {
                    _sDateRange = "Consumption Date EqualTo : " + dIssueStartDate.ToString("dd MMM yyyy");
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + "HH.TriggerParentID IN (SELECT ConsumptionRequisitionDetailID FROM ConsumptionRequisitionDetail WHERE ConsumptionRequisitionID IN (SELECT ConsumptionRequisitionID FROM ConsumptionRequisition WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))))";
                    //sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.[DateTime],106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    _sDateRange = "Consumption Date Not EqualTo : " + dIssueStartDate.ToString("dd MMM yyyy");
                    Global.TagSQL(ref sWhereCluse);
                    //sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.[DateTime],106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    sWhereCluse = sWhereCluse + "HH.TriggerParentID IN (SELECT ConsumptionRequisitionDetailID FROM ConsumptionRequisitionDetail WHERE ConsumptionRequisitionID IN (SELECT ConsumptionRequisitionID FROM ConsumptionRequisition WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))))";
                }
                else if (eIssueDate == EnumCompareOperator.GreaterThan)
                {
                    _sDateRange = "Consumption Date Greater Then : " + dIssueStartDate.ToString("dd MMM yyyy");
                    Global.TagSQL(ref sWhereCluse);
                    //sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.[DateTime],106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    sWhereCluse = sWhereCluse + "HH.TriggerParentID IN (SELECT ConsumptionRequisitionDetailID FROM ConsumptionRequisitionDetail WHERE ConsumptionRequisitionID IN (SELECT ConsumptionRequisitionID FROM ConsumptionRequisition WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))))";
                }
                else if (eIssueDate == EnumCompareOperator.SmallerThan)
                {
                    _sDateRange = "Consumption Date Smaller Then : " + dIssueStartDate.ToString("dd MMM yyyy");
                    Global.TagSQL(ref sWhereCluse);
                    //sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.[DateTime],106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    sWhereCluse = sWhereCluse + "HH.TriggerParentID IN (SELECT ConsumptionRequisitionDetailID FROM ConsumptionRequisitionDetail WHERE ConsumptionRequisitionID IN (SELECT ConsumptionRequisitionID FROM ConsumptionRequisition WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106))))";
                }
                else if (eIssueDate == EnumCompareOperator.Between)
                {
                    _sDateRange = "Consumption Date Between : " + dIssueStartDate.ToString("dd MMM yyyy") + " --to-- " + dIssueEndDate.ToString("dd MMM yyyy");
                    Global.TagSQL(ref sWhereCluse);
                    // sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.[DateTime],106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    sWhereCluse = sWhereCluse + "HH.TriggerParentID IN (SELECT ConsumptionRequisitionDetailID FROM ConsumptionRequisitionDetail WHERE ConsumptionRequisitionID IN (SELECT ConsumptionRequisitionID FROM ConsumptionRequisition WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) Between  CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106))))";
                }
                else if (eIssueDate == EnumCompareOperator.NotBetween)
                {
                    _sDateRange = "Consumption Date Not Between : " + dIssueStartDate.ToString("dd MMM yyyy") + " --to-- " + dIssueEndDate.ToString("dd MMM yyyy");
                    Global.TagSQL(ref sWhereCluse);
                    //sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.[DateTime],106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    sWhereCluse = sWhereCluse + "HH.TriggerParentID IN (SELECT ConsumptionRequisitionDetailID FROM ConsumptionRequisitionDetail WHERE ConsumptionRequisitionID IN (SELECT ConsumptionRequisitionID FROM ConsumptionRequisition WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT Between  CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106))))";
                }
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



        private TConsumptionUnit GetRoot(int nParentConsumptionUnitID)
        {
            TConsumptionUnit oTConsumptionUnit = new TConsumptionUnit();
            foreach (TConsumptionUnit oItem in _oTConsumptionUnits)
            {
                if (oItem.parentid == nParentConsumptionUnitID)
                {
                    return oItem;
                }
            }
            return _oTConsumptionUnit;
        }



        private IEnumerable<TConsumptionUnit> GetChild(int nParentConsumptionUnit)
        {
            List<TConsumptionUnit> oTConsumptionUnits = new List<TConsumptionUnit>();
            foreach (TConsumptionUnit oItem in _oTConsumptionUnits)
            {
                if (oItem.parentid == nParentConsumptionUnit)
                {
                    oTConsumptionUnits.Add(oItem);
                }
            }
            return oTConsumptionUnits;
        }



        private void AddTreeNodes(ref TConsumptionUnit oTConsumptionUnit)
        {
            IEnumerable<TConsumptionUnit> oChildNodes;
            oChildNodes = GetChild(oTConsumptionUnit.id);
            oTConsumptionUnit.children = oChildNodes;

            foreach (TConsumptionUnit oItem in oChildNodes)
            {
                TConsumptionUnit oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }



        public JsonResult GetsConsumptionUnitTree()
        {

            _oConsumptionUnit = new ConsumptionUnit();
            _oTConsumptionUnit = new TConsumptionUnit();
            _oConsumptionUnits = new List<ConsumptionUnit>();
            _oTConsumptionUnits = new List<TConsumptionUnit>();

            _oConsumptionUnits = ConsumptionUnit.Gets((int)Session[SessionInfo.currentUserID]);

            Menu _oMenu = new Menu();
            foreach (ConsumptionUnit oItem in _oConsumptionUnits)
            {

                _oTConsumptionUnit = new TConsumptionUnit();
                _oTConsumptionUnit.id = oItem.ConsumptionUnitID;
                _oTConsumptionUnit.text = oItem.Name;
                _oTConsumptionUnit.state = "";
                _oTConsumptionUnit.attributes = "";
                _oTConsumptionUnit.parentid = oItem.ParentConsumptionUnitID;
                _oTConsumptionUnit.Description = oItem.Note;
                _oTConsumptionUnit.IsLastLayer = oItem.IsLastLayer;

                //_oTProductCategory.AssetTypeInString = oItem.AssetTypeInString;
                _oTConsumptionUnits.Add(_oTConsumptionUnit);
            }



            _oTConsumptionUnit = new TConsumptionUnit();
            _oTConsumptionUnit = GetRoot(0);
            this.AddTreeNodes(ref _oTConsumptionUnit);



            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTConsumptionUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #endregion
    }
}

