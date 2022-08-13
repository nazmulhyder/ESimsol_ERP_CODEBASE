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
    public class FNRequisitionReportController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        FNRequisitionReport _oFNRequisitionReport = new FNRequisitionReport();
        List<FNRequisitionReport> _oFNRequisitionReports = new List<FNRequisitionReport>();        

        #region Actions
        public ActionResult ViewFNRequisitionReport(int menuid, int buid)
        {
            FNRequisitionReport oFNRequisitionReport = new FNRequisitionReport();
            _oFNRequisitionReports = new List<FNRequisitionReport>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FNRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);// WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFNRequisitionReports);
        }
        [HttpPost]
        public JsonResult AdvSearch(FNRequisitionReport oFNRequisitionReport)
        {
            _oFNRequisitionReport = new FNRequisitionReport();
            List<FNRequisitionReport> oFNRequisitionReports = new List<FNRequisitionReport>();

            string sSQL = "";
            int nCount = 0;
            int nDateType = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);
            DateTime dStartDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);
            DateTime dEndDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);

            if (nDateType<=1 ||  dStartDate == dEndDate) { dEndDate = dStartDate.AddDays(1); }

            if (sSQL == "Error")
            {
                _oFNRequisitionReport = new FNRequisitionReport();
                _oFNRequisitionReport.ErrorMessage = "Please select a searching critaria.";
                oFNRequisitionReports = new List<FNRequisitionReport>();
            }
            else
            {
                oFNRequisitionReports = new List<FNRequisitionReport>();
                oFNRequisitionReports = FNRequisitionReport.Gets("", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFNRequisitionReports.Count == 0)
                {
                    oFNRequisitionReports = new List<FNRequisitionReport>();
                }
            }
            var jsonResult = Json(oFNRequisitionReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GetProducts_All(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            EnumProductUsages eEnumProductUsages = new EnumProductUsages();
            try
            {
                eEnumProductUsages = EnumProductUsages.Dyes_Chemical;
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.FNRequisition, eEnumProductUsages, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.FNRequisition, eEnumProductUsages, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
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

        #region Export To Excel (Consumption)
        public void ExportToExcel(string sParams, double ts)
        {
            string Header = "";
            int nReportType = 0;
            _oFNRequisitionReport = new FNRequisitionReport();
            _oFNRequisitionReports = new List<FNRequisitionReport>();

            Company oCompany = new Company();
            FNRequisitionReport oFNRequisitionReport = new FNRequisitionReport();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oFNRequisitionReport.Params = sParams;
                if (!String.IsNullOrEmpty(sParams))
                {
                    int nCount = 0;
                    int nDateType = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);
                    DateTime dStartDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);
                    DateTime dEndDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);
                    nReportType = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);

                    if (nDateType == 1) { dEndDate = dStartDate.AddDays(1); }

                    //string sSQL = this.GetSQL(oFNRequisitionReport);
                    string sSQL = this.GetSQL2(oFNRequisitionReport);
                    _oFNRequisitionReports = FNRequisitionReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                if (_oFNRequisitionReports.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFNRequisitionReports = new List<FNRequisitionReport>();
                _sErrorMesage = ex.Message;
            }
            if (nReportType == 1)
                Print_LotWise(oCompany);
            if (nReportType == 2)
                Print_ProductWise(oCompany);
        }

        public ActionResult ExportToPdf(string sParams, double ts)
        {
            string Header = "";
            int nReportType = 0;
            _oFNRequisitionReport = new FNRequisitionReport();
            _oFNRequisitionReports = new List<FNRequisitionReport>();

            Company oCompany = new Company();
            FNRequisitionReport oFNRequisitionReport = new FNRequisitionReport();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                oFNRequisitionReport.Params = sParams;
                if (!String.IsNullOrEmpty(sParams))
                {
                    int nCount = 0;
                    int nDateType = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);
                    DateTime dStartDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);
                    DateTime dEndDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);
                    nReportType = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);

                    if (nDateType == 1) { dEndDate = dStartDate.AddDays(1); }

                    //string sSQL = this.GetSQL(oFNRequisitionReport);
                    string sSQL = this.GetSQL2(oFNRequisitionReport);
                    _oFNRequisitionReports = FNRequisitionReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                _oFNRequisitionReports = new List<FNRequisitionReport>();
            }
            //if (nReportType == 1)
            //    Print_LotWise(oCompany);
            //if (nReportType == 2)
            //    Print_ProductWise(oCompany);

            if (_oFNRequisitionReports.Count > 0)
            {
                rptFNRequisitionReport oReport = new rptFNRequisitionReport();
                byte[] abytes = oReport.PrepareReport(_oFNRequisitionReports, oCompany, nReportType);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
            
        }

        private string GetSQL2(FNRequisitionReport oFNRequisitionReport)
        {
            _sDateRange = "";
            _oFNRequisitionReport = new FNRequisitionReport();

            // SELECT RequestDate,  FNRNo,      TreatmentID FROM FNRequisition
            // SELECT FNRID,        ProductID,  LotID       FROM FNRequisitionDetail

            int nCount = 0;
            int nDateType = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);
            DateTime dStartDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);
            DateTime dEndDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);

            int nReportType = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);

            string sFNRNo = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            int nTreatmentID = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);
            string sMachineIDs = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            string sLotIDs = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            string sProductIDs = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            string sExeNo = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            int nWorkingUnitID = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);

            string sSQLQuery = "", sWhereCluse = "";

            #region sFNRNoNo
            if (!string.IsNullOrEmpty(sFNRNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition WHERE ISNULL(ReceiveBy,0) != 0 AND FNRNo LIKE'%" + sFNRNo + "%')";
            }
            #endregion

            #region sExeNo
            if (!string.IsNullOrEmpty(sExeNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM View_FNRequisition WHERE ISNULL(ReceiveBy,0) != 0 AND ExeNoFull LIKE'%" + sExeNo + "%')";
            }
            #endregion


            #region nTreatmentID
            if (nTreatmentID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition WHERE ISNULL(ReceiveBy,0) != 0 AND TreatmentID =" + nTreatmentID + ")";
            }
            #endregion

            #region sMachineIDs
            if (!string.IsNullOrEmpty(sMachineIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition WHERE ISNULL(ReceiveBy,0) != 0 AND MachineID IN (" + sMachineIDs + "))";
            }
            #endregion

            #region sLotIDs
            if (!string.IsNullOrEmpty(sLotIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.LotID IN (" + sLotIDs + ")";
            }
            #endregion

            #region sProductIDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.ProductID IN (" + sProductIDs + ")";
            }
            #endregion

            #region RequestDate
            if (nDateType > 0)
            {
                string sSearchPart = " ";
                DateObject.CompareDateQuery(ref sSearchPart, "RequestDate", nDateType, dStartDate, dEndDate);

                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition WHERE ISNULL(ReceiveBy,0) != 0  " + sSearchPart + ")";
            }
            #endregion

            #region Store
            if (nWorkingUnitID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition WHERE ISNULL(ReceiveBy,0) != 0 AND WorkingUnitID = " + nWorkingUnitID + ")";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse;
            return sSQLQuery;
        }

        private void Print_LotWise(Company oCompany) 
        {
            string Header="";
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Machine Name", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Treatment", Width = 15f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "Dyes & Chemical", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Used Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Balance", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oFNRequisitionReports.GroupBy(x => new { x.LotID, x.LotNo, x.ProductID, x.ProductName }, (key, grp) => new
                {
                    HeaderName = "Lot No : " + key.LotNo + " [Dyes/Chemical: " + key.ProductName + "]", //unique dt
                    Results = grp.ToList() //All data
                });
                Header = "Lot Wise Requisition Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FNRequisitionReport");
                    sheet.Name = "FNRequisitionReport_LotWise";

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

                    double nSubTotal_Qty_Requisition = 0,
                           nSubTotal_Qty_Consume = 0,
                           nSubTotal_Qty_Balance = 0,
                           nTotal_Qty_Requisition = 0,
                           nTotal_Qty_Consume = 0,
                           nTotal_Qty_Balance = 0,
                           nGrndTotal_Qty_Requisition = 0,
                           nGrndTotal_Qty_Consume = 0,
                           nGrndTotal_Qty_Balance = 0;

                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0;
                        DateTime dFNR_Date = DateTime.MinValue;

                        nSubTotal_Qty_Requisition = 0;
                        nSubTotal_Qty_Consume = 0;

                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.RequestDate))
                        {
                            #region Batch Wise Total
                            if (dFNR_Date != oItem.RequestDate && nCount > 0)
                            {
                                #region Total
                                nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";

                                nSubTotal_Qty_Requisition += oDataGrp.Results.Where(x => x.RequestDate == dFNR_Date).Sum(x => x.Qty_Requisition);
                                nSubTotal_Qty_Consume += oDataGrp.Results.Where(x => x.RequestDate == dFNR_Date).Sum(x => x.Qty_Consume);
                                nSubTotal_Qty_Balance = nSubTotal_Qty_Requisition - nSubTotal_Qty_Consume;

                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Requisition.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Consume.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Balance.ToString(), true, true);

                                nRowIndex++;
                                #endregion
                                nTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                                nTotal_Qty_Consume += nSubTotal_Qty_Consume;
                                nTotal_Qty_Balance += nSubTotal_Qty_Balance;

                                nGrndTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                                nGrndTotal_Qty_Consume += nSubTotal_Qty_Consume;
                                nGrndTotal_Qty_Balance += nSubTotal_Qty_Balance;

                                nSubTotal_Qty_Requisition = 0; nSubTotal_Qty_Consume = 0; nSubTotal_Qty_Balance = 0;
                            }
                            dFNR_Date = oItem.RequestDate;
                            #endregion


                            //REQ Date	FNR NO	Order No	Buyer Name	Construction	Color	Machine Name	Process Name    Dyes & Chemical 	Req. Qty	Used Qty	Balance				

                            #region DATA
                            _sFormatter = " #,##0.00;(#,##0.00)";
                            nStartCol = 2;
                            FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.RequestDateInString, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.FNRNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.DispoNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                            FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            
                            FillCellMerge(ref sheet, oItem.TreatmentSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            //FillCellMerge(ref sheet, oItem.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Requisition.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Consume.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty_Requisition - oItem.Qty_Consume) .ToString(), true);
                            #endregion
                            nRowIndex++;
                        }
                        #region Total
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";

                        nSubTotal_Qty_Requisition += oDataGrp.Results.Where(x => x.RequestDate == dFNR_Date).Sum(x => x.Qty_Requisition);
                        nSubTotal_Qty_Consume += oDataGrp.Results.Where(x => x.RequestDate == dFNR_Date).Sum(x => x.Qty_Consume);
                        nSubTotal_Qty_Balance = nSubTotal_Qty_Requisition - nSubTotal_Qty_Consume;

                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Requisition.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Consume.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Balance.ToString(), true, true);

                        nRowIndex++;
                        #endregion

                        nTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                        nTotal_Qty_Consume += nSubTotal_Qty_Consume;
                        nTotal_Qty_Balance += nSubTotal_Qty_Balance;

                        nGrndTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                        nGrndTotal_Qty_Consume += nSubTotal_Qty_Consume;
                        nGrndTotal_Qty_Balance += nSubTotal_Qty_Balance;

                        nSubTotal_Qty_Requisition = 0; nSubTotal_Qty_Consume = 0; nSubTotal_Qty_Balance = 0;

                        #region Sub Total
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);

                        FillCell(sheet, nRowIndex, ++nStartCol, nTotal_Qty_Requisition.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTotal_Qty_Consume.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTotal_Qty_Balance.ToString(), true, true);
                        nRowIndex++;
                        nTotal_Qty_Requisition = 0;
                        nTotal_Qty_Consume = 0;
                        nTotal_Qty_Balance = 0;
                        #endregion
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);

                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Requisition.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Consume.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Balance.ToString(), true, true);
                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FNRequisitionReport(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void Print_ProductWise(Company oCompany)
        {
            string Header = "";
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Machine Name", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Treatment", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Lot No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. Qty (" + _oFNRequisitionReports[0].MUName + ")", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Used Qty (" + _oFNRequisitionReports[0].MUName + ")", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Balance (" + _oFNRequisitionReports[0].MUName + ")", Width = 20f, IsRotate = false });
                #endregion

                var dataGrpList = _oFNRequisitionReports.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new
                {
                    HeaderName = "Dyes/Chemical : " + key.ProductName, //unique dt
                    Results = grp.ToList() //All data
                });
                Header = "Product Wise Requisition Report";

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FNRequisitionReport");
                    sheet.Name = "FNRequisitionReport_ProductWise";

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

                    double nSubTotal_Qty_Requisition = 0,
                           nSubTotal_Qty_Consume = 0,
                           nSubTotal_Qty_Balance = 0,
                           nTotal_Qty_Requisition = 0,
                           nTotal_Qty_Consume = 0,
                           nTotal_Qty_Balance = 0,
                           nGrndTotal_Qty_Requisition = 0,
                           nGrndTotal_Qty_Consume = 0,
                           nGrndTotal_Qty_Balance = 0;

                    foreach (var oDataGrp in dataGrpList)
                    {
                        nStartCol = 2;
                        int nCount = 0;
                        int nLotID = 0;

                        nSubTotal_Qty_Requisition = 0;
                        nSubTotal_Qty_Consume = 0;

                        FillCellMerge(ref sheet, oDataGrp.HeaderName, nRowIndex, nRowIndex++, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        foreach (var oItem in oDataGrp.Results.OrderBy(x => x.LotID))
                        {
                            #region Batch Wise Total
                            if (nLotID != oItem.LotID && nCount > 0)
                            {
                                #region Total
                                nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";

                                nSubTotal_Qty_Requisition += oDataGrp.Results.Where(x => x.LotID == nLotID).Sum(x => x.Qty_Requisition);
                                nSubTotal_Qty_Consume += oDataGrp.Results.Where(x => x.LotID == nLotID).Sum(x => x.Qty_Consume);
                                nSubTotal_Qty_Balance = nSubTotal_Qty_Requisition - nSubTotal_Qty_Consume;

                                FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 9, true, ExcelHorizontalAlignment.Right);
                                FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Requisition.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Consume.ToString(), true, true);
                                FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Balance.ToString(), true, true);

                                nRowIndex++;
                                #endregion

                                nTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                                nTotal_Qty_Consume += nSubTotal_Qty_Consume;
                                nTotal_Qty_Balance += nSubTotal_Qty_Balance;

                                nGrndTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                                nGrndTotal_Qty_Consume += nSubTotal_Qty_Consume;
                                nGrndTotal_Qty_Balance += nSubTotal_Qty_Balance;

                                nSubTotal_Qty_Requisition = 0; nSubTotal_Qty_Consume = 0; nSubTotal_Qty_Balance = 0;
                            }
                            nLotID = oItem.LotID;
                            #endregion


                            //REQ Date	FNR NO	Order No	Buyer Name	Construction	Color	Machine Name	Process Name    Dyes & Chemical 	Req. Qty	Used Qty	Balance				
                            _sFormatter = " #,##0.00;(#,##0.00)";
                            #region DATA
                            nStartCol = 2;
                            FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.RequestDateInString, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.FNRNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.DispoNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                            FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.Construction, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.ColorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.MachineName, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                            FillCellMerge(ref sheet, oItem.TreatmentSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.LotNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);

                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Requisition.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_Consume.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, (oItem.Qty_Requisition - oItem.Qty_Consume).ToString(), true);
                            #endregion
                            nRowIndex++;
                        }
                        #region Total
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";

                        nSubTotal_Qty_Requisition += oDataGrp.Results.Where(x => x.LotID == nLotID).Sum(x => x.Qty_Requisition);
                        nSubTotal_Qty_Consume += oDataGrp.Results.Where(x => x.LotID == nLotID).Sum(x => x.Qty_Consume);
                        nSubTotal_Qty_Balance = nSubTotal_Qty_Requisition - nSubTotal_Qty_Consume;

                        FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 9, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Requisition.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Consume.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Balance.ToString(), true, true);

                        nRowIndex++;
                        #endregion

                        nTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                        nTotal_Qty_Consume += nSubTotal_Qty_Consume;
                        nTotal_Qty_Balance += nSubTotal_Qty_Balance;

                        nGrndTotal_Qty_Requisition += nSubTotal_Qty_Requisition;
                        nGrndTotal_Qty_Consume += nSubTotal_Qty_Consume;
                        nGrndTotal_Qty_Balance += nSubTotal_Qty_Balance;

                        nSubTotal_Qty_Requisition = 0; nSubTotal_Qty_Consume = 0; nSubTotal_Qty_Balance = 0;

                        #region Sub Total
                        nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 9, true, ExcelHorizontalAlignment.Right);

                        FillCell(sheet, nRowIndex, ++nStartCol, nTotal_Qty_Requisition.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTotal_Qty_Consume.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, nTotal_Qty_Balance.ToString(), true, true);
                        nRowIndex++;
                        nTotal_Qty_Requisition = 0;
                        nTotal_Qty_Consume = 0;
                        nTotal_Qty_Balance = 0;
                        #endregion
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 9, true, ExcelHorizontalAlignment.Right);

                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Requisition.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Consume.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, nGrndTotal_Qty_Balance.ToString(), true, true);
                    //_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FNRequisitionReport(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        #region Support Functions
        private string GetSQL(FNRequisitionReport oFNRequisitionReport)
        {
            _sDateRange = "";
            _oFNRequisitionReport = new FNRequisitionReport();

            // SELECT RequestDate,  FNRNo,      TreatmentID FROM FNRequisition
            // SELECT FNRID,        ProductID,  LotID       FROM FNRequisitionDetail

            int nCount = 0;
            int nDateType = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);
            DateTime dStartDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);
            DateTime dEndDate = Convert.ToDateTime(oFNRequisitionReport.Params.Split('~')[nCount++]);

            int nReportType = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);

            string sFNRNo = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            int nTreatmentID = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);
            string sMachineIDs = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            string sLotIDs = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            string sProductIDs = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            string sExeNo = (oFNRequisitionReport.Params.Split('~')[nCount++]);
            int nWorkingUnitID = Convert.ToInt16(oFNRequisitionReport.Params.Split('~')[nCount++]);

            string sSQLQuery = "", sWhereCluse = "";

            #region sFNRNoNo
            if (!string.IsNullOrEmpty(sFNRNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition WHERE FNRNo LIKE'%" + sFNRNo + "%')";
            }
            #endregion

            #region sExeNo
            if (!string.IsNullOrEmpty(sExeNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM View_FNRequisition WHERE ExeNoFull LIKE'%" + sExeNo + "%')";
            }
            #endregion


            #region nTreatmentID
            if (nTreatmentID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition WHERE TreatmentID =" + nTreatmentID + ")";
            }
            #endregion

            #region sMachineIDs
            if (!string.IsNullOrEmpty(sMachineIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition WHERE MachineID IN (" + sMachineIDs + "))";
            }
            #endregion

            #region sLotIDs
            if (!string.IsNullOrEmpty(sLotIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.LotID IN (" + sLotIDs + ")";
            }
            #endregion

            #region sProductIDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.ProductID IN (" + sProductIDs + ")";
            }
            #endregion

            #region RequestDate
            if (nDateType > 0)
            {
                string sSearchPart = "";
                DateObject.CompareDateQuery(ref sSearchPart, "RequestDate", nDateType, dStartDate, dEndDate);

                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition " + sSearchPart + ")";
            }
            #endregion

            #region Store
            if (nWorkingUnitID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FNRD.FNRID IN (SELECT FNRID FROM FNRequisition WHERE WorkingUnitID = " + nWorkingUnitID + ")";
            }
            #endregion

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
        #endregion
    }
}

