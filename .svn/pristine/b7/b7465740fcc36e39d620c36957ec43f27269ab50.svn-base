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
    public class LoanRegisterController : Controller
    {
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<LoanRegister> _oLoanRegisters = new List<LoanRegister>();

        #region Actions
        public ActionResult ViewLoanRegister(int menuid)
        {
            Loan oLoan = new Loan();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            GRNRegister oLoanRegister = new GRNRegister();

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.LoanWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.LoanTypes = EnumObject.jGets(typeof(EnumFinanceLoanType));
            ViewBag.LoanRefTypes = EnumObject.jGets(typeof(EnumLoanRefType));
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]);
            //ViewBag.BUID = buid;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));

            return View(oLoan);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(LoanRegister oLoanRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oLoanRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

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
        public void ExportToExcelLoanRegister(double ts)
        {
            LoanRegister oLoanRegister = new LoanRegister();
            Company oCompany = new Company();
            try
            {
                _sErrorMesage = "";
                _oLoanRegisters = new List<LoanRegister>();
                oLoanRegister = (LoanRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oLoanRegister);
                _oLoanRegisters = LoanRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oLoanRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oLoanRegisters = new List<LoanRegister>();
                _sErrorMesage = ex.Message;
            }

            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
            if (oLoanRegister.BUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oLoanRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

            if (_sErrorMesage == "")
            {
                string Header = "", HeaderColumn = "";

                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loan No", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Ref Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Ref No", Width = 45f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Principle Amount", Width = 19f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loan Start Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Settlement No", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Settlement Date", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Settlement By", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Effect Amount", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Transfer", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Transfer IR(%)", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Transfer Days", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Trnsfer Interest Amount", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "IR(%)", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Interest Days", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Interest Amount", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Libor Rate(%)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Libor Days", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Libor Amount", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total Interest", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Bank Charge", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total Payble", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Paid Amount", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Remain Amount", Width = 25f, IsRotate = false });

                #endregion

                #region Layout Wise Header
                if (oLoanRegister.ReportLayout == EnumReportLayout.LoanWise)
                {
                    Header = "Loan Wise";
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
                    sheet.Name = "Loan Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Loan Register (" + Header + ") "; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion



                    string sCurrencySymbol = "";
                    #region Data

                    var rowspan = _oLoanRegisters.Select(i => i.LoanID).Count();
                    int nLoanID = 0; int nCount = 0; int nRowSpan = 0;

                    nRowIndex++;
                    nStartCol = 2; 

                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    foreach (var oItem in _oLoanRegisters)
                    {
                        nRowIndex++;
                        nStartCol = 2; 

                        #region Loan Wise Merge
                        if (nLoanID != oItem.LoanID)
                        {
                            //if (nCount > 0)
                            //{
                            //    nStartCol = 8;
                            //    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                            //    _sFormatter = " #,##0;(#,##0)";
                            //    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Where(x => x.LoanID == nLoanID).Sum(x => x.ReceivedQty).ToString(), true, true);
                            //    FillCell(sheet, nRowIndex, ++nStartCol, "", false);

                            //    nRowIndex++;
                            //}

                            nStartCol = 2;
                            nRowSpan = _oLoanRegisters.Where(i => i.LoanID == oItem.LoanID).Count() - 1;

                            FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.LoanNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                            if (oItem.LoanRefType == EnumLoanRefType.None)
                                FillCellMerge(ref sheet, "-", nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            else
                                FillCellMerge(ref sheet, oItem.LoanRefTypeSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                            FillCellMerge(ref sheet, oItem.LoanRefNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            _sFormatter = " #,##0;(#,##0)";
                            FillCellMerge(ref sheet, oItem.PrincipalAmount.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            FillCellMerge(ref sheet, oItem.LoanStartDateInString, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center);

                        }
                        #endregion

                        nStartCol = 8;

                        FillCell(sheet, nRowIndex, nStartCol++, oItem.InstallmentNo, false, ExcelHorizontalAlignment.Left);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.SettlementDateInString, false, ExcelHorizontalAlignment.Center);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.SettleByName, false, ExcelHorizontalAlignment.Left);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.InstallmentPrincipal.ToString(), true, ExcelHorizontalAlignment.Right);

                        if (oItem.LoanTransferType == EnumLoanTransfer.None)
                            FillCell(sheet, nRowIndex, nStartCol++, "-", false,  ExcelHorizontalAlignment.Left);
                        else
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.LoanTransferTypeSt, false, ExcelHorizontalAlignment.Left);

                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.TransferInterestRate.ToString(), true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.TransferDays.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.TransferInterestAmount.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.InterestRate.ToString(), true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.InterestDays.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.InstallmentInterestAmount.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.LiborRate.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.InterestDays.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.InstallmentLiborInterestAmount.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, (oItem.TransferInterestAmount + oItem.InstallmentInterestAmount + oItem.InstallmentLiborInterestAmount).ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ChargeAmount.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.TotalPayableAmount.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.PaidAmount.ToString(), true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.PrincipalBalance.ToString(), true, ExcelHorizontalAlignment.Right);


                        //nRowIndex++;

                        nLoanID = oItem.LoanID;
                        //sCurrencySymbol = obj.CurrencySymbol;


                        //nStartCol = 8;
                        //FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                        //_sFormatter = " #,##0;(#,##0)";
                        //FillCell(sheet, nRowIndex, ++nStartCol, oItem.Where(x => x.LoanID == nLoanID).Sum(x => x.ReceivedQty).ToString(), true, true);
                        //FillCell(sheet, nRowIndex, ++nStartCol, "", false);

                        //nRowIndex++;

                        //nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        //FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                        //FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalQty.ToString(), true, true);
                        //FillCell(sheet, nRowIndex, ++nStartCol, "", false, true); _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";

                        //nRowIndex++;
                    }

                    //#region Grand Total
                    //nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    //FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    //FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalQty).ToString(), true, true);
                    //FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    ////_sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    ////FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalAmount).ToString(), true, true);
                    //nRowIndex++;
                    //#endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 13];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=LoanRegister(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion



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

        private string GetSQL(LoanRegister oLoanRegister)
        {
            _sDateRange = "";
            string sSearchingData = oLoanRegister.SearchingData;
            EnumCompareOperator eLoanIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dLoanIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dLoanIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eLoanStartDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dLoanStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dLoanEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eLoanRcvDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dLoanRcvStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dLoanRcvEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oLoanRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oLoanRegister.BUID.ToString();
            }
            #endregion

            #region LoanNo
            if (oLoanRegister.LoanNo != null && oLoanRegister.LoanNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LoanNo LIKE'%" + oLoanRegister.LoanNo + "%'";
            }
            #endregion

            #region RcvBankAccount
            if (oLoanRegister.RcvBankAccountID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RcvBankAccountID =" + oLoanRegister.RcvBankAccountID;
            }
            #endregion

            #region Loan Type
            if (oLoanRegister.LoanType != EnumFinanceLoanType.None)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LoanType =" + ((int)oLoanRegister.LoanType).ToString();
            }
            #endregion

            #region Issue Date
            if (eLoanIssueDate != EnumCompareOperator.None)
            {
                if (eLoanIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    //_sDateRange = "PI Date @ " + dLoanIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLoanIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    //_sDateRange = "PI Date Not Equal @ " + dLoanIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLoanIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    //_sDateRange = "PI Date Greater Then @ " + dLoanIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLoanIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    //_sDateRange = "PI Date Smaller Then @ " + dLoanIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLoanIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    //_sDateRange = "PI Date Between " + dLoanIssueStartDate.ToString("dd MMM yyyy") + " To " + dLoanIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (eLoanIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    //_sDateRange = "PI Date NOT Between " + dLoanIssueStartDate.ToString("dd MMM yyyy") + " To " + dLoanIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region LoanStartDate Date
            if (eLoanStartDate != EnumCompareOperator.None)
            {
                if (eLoanStartDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LoanStartDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanStartDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LoanStartDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanStartDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LoanStartDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanStartDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LoanStartDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanStartDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LoanStartDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanStartDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LoanStartDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Received Date
            if (eLoanRcvDate != EnumCompareOperator.None)
            {
                if (eLoanRcvDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RcvDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanRcvStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanRcvDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RcvDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanRcvStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanRcvDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RcvDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanRcvStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanRcvDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RcvDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanRcvStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanRcvDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RcvDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanRcvStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanRcvEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLoanRcvDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RcvDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanRcvStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLoanRcvEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region RefType
            if (oLoanRegister.LoanRefType != EnumLoanRefType.None)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LoanRefType = " + ((int)oLoanRegister.LoanRefType).ToString() ;
            }
            #endregion

            #region RefNo
            if (oLoanRegister.LoanRefNo != null && oLoanRegister.LoanRefNo != "")
            {
                if (oLoanRegister.LoanRefType != EnumLoanRefType.None)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " LoanRefID IN (" + oLoanRegister.LoanRefNo + ")";
                }
            }
            #endregion

            #region Report Layout
            if (oLoanRegister.ReportLayout == EnumReportLayout.LoanWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_LoanRegister ";
                sOrderBy = " ORDER BY LoanID ASC";
            }

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;

            #endregion
        }

        #endregion



        

    }
}