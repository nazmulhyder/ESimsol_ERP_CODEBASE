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
using System.Web;
using ICS.Core.Utility;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ExportGraphController : Controller
    {
        #region Declaration
        List<ExportGraph> _oExportGraphs = new List<ExportGraph>();
        ExportGraph _oExportGraph = new ExportGraph();
        string _sErrorMessage = "";
        #endregion

        #region View
        public ActionResult View_ExportGraph(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportGraphs = new List<ExportGraph>();
            _oExportGraphs = ExportGraph.Gets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oExportGraphs.Count > 0)
            {
                _oExportGraphs[0].BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            return View(_oExportGraphs);
        }
        #endregion
        #region HTTPGet
        [HttpPost]
        public JsonResult GetsSearchedData(DateTime ExportStartDate, DateTime ExportEndDate, string BankName, string SelectedOption, string DateType, int BUID)
        {
            List<ExportGraph> oExportGraphs = new List<ExportGraph>();
            ExportGraph oExportGraph = new ExportGraph();
            oExportGraph.ExportStartDate = ExportStartDate;
            oExportGraph.ExportEndDate = ExportEndDate;
            oExportGraph.BankName = BankName;
            oExportGraph.SelectedOption = SelectedOption;
            oExportGraph.Tenor = DateType; // use this for passing DateType
            oExportGraph.BUID = BUID;
            try
            {
                string sSQL = GetSQL(oExportGraph);
                oExportGraphs = ExportGraph.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportGraphs = new List<ExportGraph>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportGraphs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(ExportGraph oExportGraph)
        {
            string sReturn1 = "SELECT * FROM View_ExportBillReport ";
            //string sReturn = " where State =3 ";
            string sReturn = "";
            #region String BankName
            if (oExportGraph.BankName != "0")
            {
                if (oExportGraph.BankName != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BankBranchID_Negotiation IN (" + oExportGraph.BankName + ")";
                }
            }
            if (oExportGraph.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID IN (" + oExportGraph.BUID + ")";
            }
            if (oExportGraph.SelectedOption != null)
            {
                if (oExportGraph.SelectedOption != "")
                {
                    if (oExportGraph.SelectedOption != EnumCompareOperator.None.ToString())
                    {
                        if (oExportGraph.SelectedOption == EnumCompareOperator.Between.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " CONVERT(date, " + oExportGraph.Tenor + ") between '" + oExportGraph.ExportStartDate.ToString("dd MMM yyyy") + "' AND '" + oExportGraph.ExportEndDate.ToString("dd MMM yyyy") + "'";
                        }
                        else if (oExportGraph.SelectedOption == EnumCompareOperator.NotBetween.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " CONVERT(date, " + oExportGraph.Tenor + ") between '" + oExportGraph.ExportStartDate.ToString("dd MMM yyyy") + "' OR '" + oExportGraph.ExportEndDate.ToString("dd MMM yyyy") + "'";
                        }

                        //if (oExportGraph.SelectedOption == EnumCompareOperator.Between.ToString())
                        //{
                        //    Global.TagSQL(ref sReturn);
                        //    sReturn = sReturn + " '" + oExportGraph.ExportStartDate.ToString("dd MMM yyyy") + "'>= " + oExportGraph.Tenor + " AND " + oExportGraph.Tenor + " < '" + oExportGraph.ExportEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                        //}
                        //else if (oExportGraph.SelectedOption == EnumCompareOperator.NotBetween.ToString())
                        //{
                        //    Global.TagSQL(ref sReturn);
                        //    sReturn = sReturn + " '" + oExportGraph.MaturityDate.ToString("dd MMM yyyy") + "'>= " + oExportGraph.Tenor + " OR " + oExportGraph.Tenor + " > '" + oExportGraph.ExportEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                        //}
                        else if (oExportGraph.SelectedOption == EnumCompareOperator.EqualTo.ToString())
                        {
                            Global.TagSQL(ref sReturn);
                            sReturn = sReturn + " '" + oExportGraph.ExportStartDate.ToString("dd MMM yyyy") + "'= " + oExportGraph.Tenor;
                        }
                    }
                }
            }
            #endregion
            sReturn = sReturn1 + sReturn + "  order by BankBranchID_Negotiation,MaturityDate,CurrencyID";
            return sReturn;
        }
        public ActionResult Print_Report_Month(string sTempString, int BUID)
        {

            _oExportGraphs = new List<ExportGraph>();
            ExportGraph oExportGraph = new ExportGraph();
            oExportGraph.SelectedOption = sTempString.Split('~')[0];
            oExportGraph.MaturityDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            oExportGraph.MaturityDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            oExportGraph.BankName = sTempString.Split('~')[3];
            oExportGraph.Tenor = sTempString.Split('~')[4];
            oExportGraph.BUID = BUID;
            DateTime reportHeaderDate = oExportGraph.MaturityDate;

            oExportGraph.ExportStartDate = new DateTime(oExportGraph.MaturityDate.Year, oExportGraph.MaturityDate.Month, 1);
            oExportGraph.ExportEndDate = new DateTime(oExportGraph.MaturityDate.Year, oExportGraph.MaturityDate.Month, DateTime.DaysInMonth(oExportGraph.MaturityDate.Year, oExportGraph.MaturityDate.Month));

            try
            {
                string sSQL = GetSQL(oExportGraph);
                _oExportGraphs = ExportGraph.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportGraphs = new List<ExportGraph>();
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oExportGraph.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Currency> oCurrencys = new List<Currency>();
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<BankBranch> oBankBranchs = new List<BankBranch>();
            oBankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptExportGraph oReport = new rptExportGraph();
            byte[] abytes = oReport.PrepareReport(_oExportGraphs, oCompany, oCurrencys, oBankBranchs, oBusinessUnit, reportHeaderDate);
            return File(abytes, "application/pdf");
        }
        public ActionResult Print_ReportXL_Month(string sTempString, int BUID)
        {

            _oExportGraphs = new List<ExportGraph>();
            ExportGraph oExportGraph = new ExportGraph();
            oExportGraph.SelectedOption = sTempString.Split('~')[0];
            oExportGraph.MaturityDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            oExportGraph.MaturityDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            oExportGraph.BankName = sTempString.Split('~')[3];
            oExportGraph.Tenor = sTempString.Split('~')[4];
            oExportGraph.BUID = BUID;
            DateTime reportHeaderDate = oExportGraph.MaturityDate;

            oExportGraph.ExportStartDate = new DateTime(oExportGraph.MaturityDate.Year, oExportGraph.MaturityDate.Month, 1);
            oExportGraph.ExportEndDate = new DateTime(oExportGraph.MaturityDate.Year, oExportGraph.MaturityDate.Month, DateTime.DaysInMonth(oExportGraph.MaturityDate.Year, oExportGraph.MaturityDate.Month));

            try
            {
                string sSQL = GetSQL(oExportGraph);
                _oExportGraphs = ExportGraph.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportGraphs = new List<ExportGraph>();
            }
            List<Currency> oCurrencys = new List<Currency>();
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<BankBranch> oBankBranchs = new List<BankBranch>();
            oBankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);




            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptExportGraph oReport = new rptExportGraph();
            ExelReport(_oExportGraphs, BUID, reportHeaderDate, oCurrencys);
            byte[] abytes = oReport.PrepareReport(_oExportGraphs, oCompany, oCurrencys, oBankBranchs, oBusinessUnit, reportHeaderDate);
            return File(abytes, "application/pdf");
        }
        public ActionResult Print_Report_All(string sTempString, int BUID)
        {

            _oExportGraphs = new List<ExportGraph>();
            ExportGraph oExportGraph = new ExportGraph();
            oExportGraph.SelectedOption = sTempString.Split('~')[0];
            oExportGraph.ExportStartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            oExportGraph.ExportEndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            oExportGraph.BankName = Convert.ToString(sTempString.Split('~')[3]);
            oExportGraph.Tenor = sTempString.Split('~')[4];
            oExportGraph.BUID = BUID;
            DateTime reportHeaderDate = DateTime.MinValue;
            try
            {
                string sSQL = GetSQL(oExportGraph);
                _oExportGraphs = ExportGraph.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportGraphs = new List<ExportGraph>();
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oExportGraph.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Currency> oCurrencys = new List<Currency>();
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<BankBranch> oBankBranchs = new List<BankBranch>();
            oBankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptExportGraph oReport = new rptExportGraph();
            _oExportGraphs[0].ExportStartDate = Convert.ToDateTime(sTempString.Split('~')[1]); // using for pass the searching start date
            _oExportGraphs[0].ExportEndDate = Convert.ToDateTime(sTempString.Split('~')[2]); // using for pass the searching end date
            byte[] abytes = oReport.PrepareReport(_oExportGraphs, oCompany, oCurrencys, oBankBranchs, oBusinessUnit, reportHeaderDate);
            return File(abytes, "application/pdf");
        }
        public ActionResult Print_ReportXL_All(string sTempString, int BUID)
        {
            _oExportGraphs = new List<ExportGraph>();
            ExportGraph oExportGraph = new ExportGraph();
            oExportGraph.SelectedOption = sTempString.Split('~')[0];
            oExportGraph.ExportStartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            oExportGraph.ExportEndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
            oExportGraph.BankName = Convert.ToString(sTempString.Split('~')[3]);
            oExportGraph.Tenor = sTempString.Split('~')[4];
            oExportGraph.BUID = BUID;
            DateTime reportHeaderDate = DateTime.MinValue;
            try
            {
                string sSQL = GetSQL(oExportGraph);
                _oExportGraphs = ExportGraph.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportGraphs = new List<ExportGraph>();
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oExportGraph.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Currency> oCurrencys = new List<Currency>();
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<BankBranch> oBankBranchs = new List<BankBranch>();
            oBankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptExportGraph oReport = new rptExportGraph();
            _oExportGraphs[0].ExportStartDate = Convert.ToDateTime(sTempString.Split('~')[1]); // using for pass the searching start date
            _oExportGraphs[0].ExportEndDate = Convert.ToDateTime(sTempString.Split('~')[2]); // using for pass the searching end date
            ExelReportAll(_oExportGraphs, BUID, reportHeaderDate, oCurrencys);
            byte[] abytes = oReport.PrepareReport(_oExportGraphs, oCompany, oCurrencys, oBankBranchs, oBusinessUnit, reportHeaderDate);
            return File(abytes, "application/pdf");
        }
       
        public void ExelReport(List<ExportGraph> oExportGraphs, int BUID, DateTime reportHeaderDate, List<Currency> oCurrencys)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<Currency> _oCurrencys = oCurrencys;
            string CurrencyName = "";
            DateTime _reportHeaderDate = DateTime.MinValue;
            double amountMonth = 0;
            double amountYear = 0;

            _reportHeaderDate = reportHeaderDate;

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 5f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "L/C No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Bank Ref No.", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Issue Bank Branch", Width = 50f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI No", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Party Name", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Value", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Maturity Receive Date", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Maturity Date", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Realization Date", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "LDBP No.", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "State", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "C/P", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell;
            //ExcelFill fill;
            //OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export Graph Report");
                sheet.Name = "Export Graph Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 12;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Export Graph Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                #region Bank Name
                ExcelTool.FillCellMerge(ref sheet, oExportGraphs[0].BankName, nRowIndex, nRowIndex, 2, nEndCol + 2, false, ExcelHorizontalAlignment.Left, true);
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 25;
                #endregion
                #region Data
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                foreach (var obj in oExportGraphs)
                {
                    nStartCol = 2;
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExportLCNo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LDBCNo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.IssueBankNameAndBrunch, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PINo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ApplicantName.ToString(), false);
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.AmountST.ToString(), false);
                    ExcelTool.Formatter = "$ #,##0.00";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Amount.ToString("00.00"), true);
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MaturityReceivedDateSt, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MaturityDateST, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RelizationDateSt.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LDBPNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.State.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MKTPName.ToString(), false);

                    amountMonth = amountMonth + obj.Amount;
                    CurrencyName = obj.CurrencyName;
                    nRowIndex++;
                }
                amountYear = amountYear + amountMonth;
                #endregion
                #region Total
                nStartCol = 2;
                ExcelTool.Formatter = "";
                ExcelTool.FillCellMerge(ref sheet, _reportHeaderDate.ToString("MMM yy"), nRowIndex, nRowIndex, nStartCol, 6, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.FillCellMerge(ref sheet, CurrencyName, nRowIndex, nRowIndex, 7, 7, false, ExcelHorizontalAlignment.Right);
                ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(amountMonth).ToString(), nRowIndex, nRowIndex, 8, 9, false, ExcelHorizontalAlignment.Left, true);
                ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, 10, 14, false, ExcelHorizontalAlignment.Right);
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.Formatter = "";
                ExcelTool.FillCellMerge(ref sheet, "Total :", nRowIndex, nRowIndex, nStartCol, 8, false, ExcelHorizontalAlignment.Right, false);
                ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(amountYear).ToString(), nRowIndex, nRowIndex, 9, 14, false, ExcelHorizontalAlignment.Left, false);
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.Formatter = "";
                ExcelTool.FillCellMerge(ref sheet, oExportGraphs[0].BankName, nRowIndex, nRowIndex, nStartCol, 6, false, ExcelHorizontalAlignment.Left);
                ExcelTool.FillCellMerge(ref sheet, CurrencyName, nRowIndex, nRowIndex, 7, 7, false, ExcelHorizontalAlignment.Right);
                ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(amountYear).ToString(), nRowIndex, nRowIndex, 8, 9, false, ExcelHorizontalAlignment.Left, true);
                ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, 10, 14, false, ExcelHorizontalAlignment.Right);
                nRowIndex++;
                #endregion
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Export Graph Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        [HttpPost]
        public JsonResult GetsForGraph(string sYear, int nBankBranchID, int BUID, string sDateCriteria)
        {
            _oExportGraph = new ExportGraph();
            _oExportGraphs = new List<ExportGraph>();
            ExportGraph oExportGraph_Chart = new ExportGraph();
            List<ExportGraph> oExportGraphs_Chart = new List<ExportGraph>();

            try
            {
                _oExportGraphs = ExportGraph.GetsForGraph(sYear, nBankBranchID, BUID, sDateCriteria, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportGraphs.Count <= 0 || _oExportGraphs[0].ErrorMessage != "")
                {
                    throw new Exception("Data not found!");
                }
                else
                {
                    //_oExportGraph.ExportGraphs = _oExportGraphs;

                    while (_oExportGraphs.Count > 0)
                    {
                        double nAmount = 0;
                        List<ExportGraph> oPIIBPs = new List<ExportGraph>();
                        //oPIIBPs = _oExportGraphs;
                        oPIIBPs = _oExportGraphs.Where(x => x.MaturityDate.Month == _oExportGraphs[0].MaturityDate.Month).ToList();
                        foreach (ExportGraph oPIIBP in oPIIBPs)
                        {
                            nAmount = nAmount + oPIIBP.Amount * oPIIBP.AcceptanceRate;
                        }
                        oExportGraph_Chart = new ExportGraph();
                        oExportGraph_Chart.Amount = nAmount;
                        oExportGraph_Chart.Currency = oPIIBPs[0].Currency;
                        oExportGraph_Chart.MaturityDate = oPIIBPs[0].MaturityDate;
                        oExportGraph_Chart.ExportGraphs = oPIIBPs;
                        oExportGraphs_Chart.Add(oExportGraph_Chart);
                        _oExportGraphs.RemoveAll(x => x.MaturityDate.Month == oPIIBPs[0].MaturityDate.Month);
                    }
                    _oExportGraph.PIIBPs_ChartList = oExportGraphs_Chart;

                }

            }
            catch (Exception ex)
            {

                _oExportGraph = new ExportGraph();
                _oExportGraph.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportGraph);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ExelReportAll(List<ExportGraph> oExportGraphs, int BUID, DateTime reportHeaderDate, List<Currency> oCurrencys)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<Currency> _oCurrencys = oCurrencys;
            string CurrencyName = "";
            DateTime _reportHeaderDate = DateTime.MinValue;
            double amountMonth = 0;
            double amountBankTotal = 0;
            double amountTotal = 0;

            _reportHeaderDate = reportHeaderDate;

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 5f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "L/C No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Bank Ref No.", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Issue Bank Branch", Width = 50f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI No", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Party Name", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Value", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Maturity Received Date", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Maturity Date", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Realization Date", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "LDBP No.", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "State", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "C/P", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell;


            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export Graph Report");
                sheet.Name = "Export Graph Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 12;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Export Graph Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                ///////////////////////////////////
                int i = -1;
                int BBrachID=-999;
                foreach(ExportGraph oItem_EG in oExportGraphs)
                {
                    i++;
                    if (BBrachID != oItem_EG.BankBranchID)
                    {
                        #region Bank Name
                        ExcelTool.FillCellMerge(ref sheet, oItem_EG.BankName, nRowIndex, nRowIndex, 2, nEndCol + 2, false, ExcelHorizontalAlignment.Left, true);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 25;
                        #endregion
                        #region Data
                        nRowIndex++;
                        nStartCol = 2;
                        ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                        int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                        foreach (var obj in oExportGraphs.Where(x=>x.BankBranchID==oItem_EG.BankBranchID))
                        {
                            nStartCol = 2;
                            ExcelTool.Formatter = "";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExportLCNo, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LDBCNo, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.IssueBankNameAndBrunch, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PINo, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ApplicantName, false);
                            ExcelTool.Formatter = "$ #,##0.00";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Amount.ToString("00.00"), true);
                            ExcelTool.Formatter = "";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MaturityReceivedDateSt, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MaturityDateST, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.RelizationDateSt, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LDBPNo, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.State.ToString(), false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MKTPName, false);

                            amountMonth = amountMonth + obj.Amount;
                            CurrencyName = obj.CurrencyName;
                            nRowIndex++;
                        }
                        amountBankTotal = amountBankTotal + amountMonth;
                        #endregion
                        #region Total
                        //nStartCol = 2;
                        //ExcelTool.Formatter = "";
                        //ExcelTool.FillCellMerge(ref sheet, _reportHeaderDate.ToString("MMM yy"), nRowIndex, nRowIndex, nStartCol, 6, false, ExcelHorizontalAlignment.Right, false);
                        //ExcelTool.FillCellMerge(ref sheet, CurrencyName, nRowIndex, nRowIndex, 7, 7, false, ExcelHorizontalAlignment.Right);
                        //ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(amountBankTotal).ToString(), nRowIndex, nRowIndex, 8, 9, false, ExcelHorizontalAlignment.Left, true);
                        //ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, 10, 14, false, ExcelHorizontalAlignment.Right);
                        //nRowIndex++;

                        //nStartCol = 2;
                        //ExcelTool.Formatter = "";
                        //ExcelTool.FillCellMerge(ref sheet, "Total :", nRowIndex, nRowIndex, nStartCol, 8, false, ExcelHorizontalAlignment.Right, false);
                        //ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(amountBankTotal).ToString(), nRowIndex, nRowIndex, 9, 14, false, ExcelHorizontalAlignment.Left, false);
                        //nRowIndex++;
                        //BBrachID = oItem_EG.BankBranchID;
                        //amountMonth = 0;
                        nStartCol = 2;
                        ExcelTool.Formatter = "";
                        ExcelTool.FillCellMerge(ref sheet, "Total :", nRowIndex, nRowIndex, nStartCol, 7, false, ExcelHorizontalAlignment.Right, false);
                        ExcelTool.Formatter = "$ #,##0.00";
                        ExcelTool.FillCellMerge(ref sheet, amountBankTotal.ToString("00.00"), nRowIndex, nRowIndex, 8, 14, true, ExcelHorizontalAlignment.Left, false);
                        nRowIndex++;
                        BBrachID = oItem_EG.BankBranchID;
                        amountMonth = 0;
                    }
                    amountTotal = amountBankTotal + amountTotal;
                    amountBankTotal = 0;
                }
            
                
                //nStartCol = 2;
                //ExcelTool.Formatter = "";
                //ExcelTool.FillCellMerge(ref sheet, oExportGraphs[0].BankName, nRowIndex, nRowIndex, nStartCol, 6, false, ExcelHorizontalAlignment.Left);
                //ExcelTool.FillCellMerge(ref sheet, CurrencyName, nRowIndex, nRowIndex, 7, 7, false, ExcelHorizontalAlignment.Right);
                //ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(amountTotal).ToString(), nRowIndex, nRowIndex, 8, 9, false, ExcelHorizontalAlignment.Left, true);
                //ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, 10, 14, false, ExcelHorizontalAlignment.Right);
                //nRowIndex++;
                nStartCol = 2;
                ExcelTool.Formatter = "";
                ExcelTool.FillCellMerge(ref sheet, "Total :", nRowIndex, nRowIndex, nStartCol, 6, false, ExcelHorizontalAlignment.Left);
                ExcelTool.FillCellMerge(ref sheet, CurrencyName, nRowIndex, nRowIndex, 7, 7, false, ExcelHorizontalAlignment.Right);
                ExcelTool.FillCellMerge(ref sheet, Global.MillionFormat(amountTotal).ToString(), nRowIndex, nRowIndex, 8, 9, false, ExcelHorizontalAlignment.Left, true);
                ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, 10, 14, false, ExcelHorizontalAlignment.Right);
                nRowIndex++;
                #endregion
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Export Graph Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion

        }
    }

}
            #endregion