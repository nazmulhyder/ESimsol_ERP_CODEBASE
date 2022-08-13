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
    public class ImportSummaryRegisterController : Controller
    {

        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<ImportSummaryRegister> _oImportSummaryRegisters = new List<ImportSummaryRegister>();

        #region Actions

        public ActionResult ViewImportSummaryRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ImportSummaryRegister oImportSummaryRegister = new ImportSummaryRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM ImportPI AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.PIWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            List<EnumObject> oGRNTypes = new List<EnumObject>();
            List<EnumObject> oTempGRNTypes = new List<EnumObject>();
            oTempGRNTypes = EnumObject.jGets(typeof(EnumGRNType));
            foreach (EnumObject oItem in oTempGRNTypes)
            {
                if ((EnumGRNType)oItem.id != EnumGRNType.FancyYarn && (EnumGRNType)oItem.id != EnumGRNType.FloorReturn)
                {
                    oGRNTypes.Add(oItem);
                }
            }

            ViewBag.BUID = buid;
            ViewBag.GRNType = oGRNTypes;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ImportPITypes = EnumObject.jGets(typeof(EnumImportPIType));
            ViewBag.ImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            return View(oImportSummaryRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ImportSummaryRegister oImportSummaryRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oImportSummaryRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Excel
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
            return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
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
        public void ExportToExcelImportSummaryRegister(double ts)
        {
            int nPID = -999;
            int nPIDetailID = -999;
            int nLCID = -999;
            int nINVID = -999;
            int nINVDetailID = -999;
            string Header = "", HeaderColumn = "";

            Company oCompany = new Company();
            ImportSummaryRegister oImportSummaryRegister = new ImportSummaryRegister();
            try
            {
                _sErrorMesage = "";
                _oImportSummaryRegisters = new List<ImportSummaryRegister>();
                oImportSummaryRegister = (ImportSummaryRegister)Session[SessionInfo.ParamObj];
                string[] sSQL = this.GetSQL(oImportSummaryRegister);
                _oImportSummaryRegisters = ImportSummaryRegister.Gets(sSQL[0], sSQL[1], sSQL[2], (int)Session[SessionInfo.currentUserID]);
                if (_oImportSummaryRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oImportSummaryRegisters = new List<ImportSummaryRegister>();
                _sErrorMesage = ex.Message;
            }

            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
            //if (oImportSummaryRegister.BUID > 0)
            //{
            //    BusinessUnit oBU = new BusinessUnit();
            //    oBU = oBU.Get(oImportSummaryRegister.BUID, (int)Session[SessionInfo.currentUserID]);
            //    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            //}

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Commodity", Width = 45f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Rate", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Amt", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Party", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC No", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "L/C Date", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Sales Cont. No", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Sales Cont. Date", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Advising Bank", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC Amt", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Due Value", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Inv. No", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Inv. Date", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Inv. value", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Acceptance Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Maturity Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Payment Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Payment Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "CRate", Width = 11f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Goods Rcv. Date", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Goods Rcv. Qty", Width = 25f, IsRotate = false });

                #endregion

                Header = "PI Wise"; HeaderColumn = "PI Name : ";

                #region Layout Wise Header
                //if (oImportSummaryRegister.ReportLayout == EnumReportLayout.ProductWise)
                //{
                //    Header = "Product Wise"; HeaderColumn = "Product Name : ";
                //}

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("GRN Register");
                    sheet.Name = "GRN Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 5, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Import Summary Registerr (" + Header + ") "; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 5, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    nRowIndex++;
                    string sCurrencySymbol = "";

                    #region Data

                    nStartCol = 2;
                    int nCount = 0, nPIRowSpan = 0, nPIDetailRowSpan = 0, nLCRowSpan = 0, nInvRowSpan = 0, nInvoiceDetailRowSpan = 0;
                    double totalPIQty = 0.0, totalPIAmt = 0.0, totalLCAmt = 0.0, totalINValue = 0.0, totalGoodsRcvQty = 0.0;  
                    int StartRowIndex = 0, EndRowRowIndex = 0;
                    List<ImportSummaryRegister> _oImportSummaryLCTemp = new List<ImportSummaryRegister>();
                    List<ImportSummaryRegister> _oImportSummaryInvoiceTemp = new List<ImportSummaryRegister>();

                    foreach (ImportSummaryRegister obj in _oImportSummaryRegisters)
                    {

                        nStartCol = 2;
                        if (nLCID != obj.LCID)
                        {
                            nLCRowSpan = _oImportSummaryRegisters.Where(LCID => LCID.LCID == obj.LCID).ToList().Count - 1;
                            FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "NUMBER");
                        }

                        nStartCol = 3;
                        if (nPID != obj.ImportPIID)
                        {
                            nPIRowSpan = _oImportSummaryRegisters.Where(PIID => PIID.ImportPIID == obj.ImportPIID).ToList().Count - 1;

                            if (nPIRowSpan < 0)
                                nPIRowSpan = 0;

                            FillCellMerge(ref sheet, obj.ImportPINo, nRowIndex, nRowIndex + nPIRowSpan, nStartCol, nStartCol, "STRING");
                            nStartCol++;
                            if (obj.ImportPIDateInString == "01 Jan 0001")
                            {
                                FillCellMerge(ref sheet, "-", nRowIndex, nRowIndex + nPIRowSpan, nStartCol, nStartCol, "DATE");
                            }
                            else
                            {
                                FillCellMerge(ref sheet, obj.ImportPIDateInString, nRowIndex, nRowIndex + nPIRowSpan, nStartCol, nStartCol, "DATE");
                            }

                        }

                        if (nPIDetailID != obj.ImportPIDetailID)
                        {
                            nPIDetailRowSpan = _oImportSummaryRegisters.Where(x => x.ImportPIID == obj.ImportPIID && x.ImportPIDetailID == obj.ImportPIDetailID).ToList().Count - 1;
                            if (nPIDetailRowSpan < 0)
                                nPIDetailRowSpan = 0;
                            nStartCol = 5;
                            FillCellMerge(ref sheet, obj.ProductName, nRowIndex, nRowIndex + nPIDetailRowSpan, nStartCol, nStartCol, "STRING");
                            nStartCol++;
                            FillCellMerge(ref sheet, obj.PIQty.ToString("#,##0;(#,##0)"), nRowIndex, nRowIndex + nPIDetailRowSpan, nStartCol, nStartCol, "NUMBER");
                            if (obj.PIQty != null)
                            {
                                totalPIQty = totalPIQty + obj.PIQty;
                            }
                            nStartCol++;
                            FillCellMerge(ref sheet, obj.UnitPrice.ToString("#,##0;(#,##0)"), nRowIndex, nRowIndex + nPIDetailRowSpan, nStartCol, nStartCol, "NUMBER");
                            nStartCol++;
                            FillCellMerge(ref sheet, obj.PIAmount.ToString("#,##0;(#,##0)"), nRowIndex, nRowIndex + nPIDetailRowSpan, nStartCol, nStartCol, "NUMBER");
                            if (obj.PIAmount != null)
                            {
                                totalPIAmt = totalPIAmt + obj.PIAmount;
                            }
                            nStartCol++;
                        }

                        if (nPID != obj.ImportPIID)
                        {
                            nStartCol = 9;
                            FillCellMerge(ref sheet, obj.PartyName, nRowIndex, nRowIndex + nPIRowSpan, nStartCol, nStartCol, "STRING");
                        }
                        nStartCol = 10;
                        if (nLCID != obj.LCID)
                        {
                            _oImportSummaryLCTemp.AddRange(_oImportSummaryRegisters.Where(LCID => LCID.LCID == obj.LCID).OrderBy(x => x.InvoiceID).ToList());
                            nLCRowSpan = _oImportSummaryRegisters.Where(LCID => LCID.LCID == obj.LCID).ToList().Count - 1;

                            if (nLCRowSpan < 0)
                                nLCRowSpan = 0;

                            FillCellMerge(ref sheet, obj.LCNo, nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "STRING");
                            nStartCol++;
                            if (obj.LCDateInString == "01 Jan 0001")
                            {
                                FillCellMerge(ref sheet, "-", nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "DATE");
                            }
                            else
                            {
                                FillCellMerge(ref sheet, obj.LCDateInString, nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "DATE");
                            }
                            nStartCol++;
                            FillCellMerge(ref sheet, obj.SalesContractNo, nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "STRING");
                            nStartCol++;
                            if (obj.SalesContractDateInString == "01 Jan 0001")
                            {
                                FillCellMerge(ref sheet, "-", nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "DATE");
                            }
                            else
                            {
                                FillCellMerge(ref sheet, obj.SalesContractDateInString, nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "DATE");
                            }
                            nStartCol++;
                            FillCellMerge(ref sheet, obj.LCAdviseBankName, nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "STRING");
                            nStartCol++;
                            FillCellMerge(ref sheet, obj.LCAmount.ToString("#,##0;(#,##0)"), nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "NUMBER");
                            if (obj.LCAmount != null)
                            {
                                totalLCAmt = totalLCAmt + obj.LCAmount;
                            }
                            nStartCol++;
                            FillCellMerge(ref sheet, obj.InvoiceDue.ToString("#,##0;(#,##0)"), nRowIndex, nRowIndex + nLCRowSpan, nStartCol, nStartCol, "NUMBER");
                        }

                        nStartCol = 17;

                        if (_oImportSummaryLCTemp.Count > 0)
                        {
                            int tempnLCRowSpan = nLCRowSpan;
                            StartRowIndex = nRowIndex;

                            foreach (ImportSummaryRegister oItem in _oImportSummaryLCTemp)
                            {
                                if (nINVID != oItem.InvoiceID)
                                {
                                    _oImportSummaryInvoiceTemp.AddRange(_oImportSummaryLCTemp.Where(x => x.InvoiceID == oItem.InvoiceID).OrderBy(x => x.InvoiceDetailID).ToList());
                                    nInvRowSpan = _oImportSummaryRegisters.Where(x => x.LCID == obj.LCID && x.InvoiceID == oItem.InvoiceID).ToList().Count - 1;
                                    if (nInvRowSpan < 0)
                                        nInvRowSpan = 0;
                                    nStartCol = 17;
                                    FillCellMerge(ref sheet, oItem.InvoiceNo, StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "STRING");
                                    nStartCol = 18;
                                    if (oItem.InvoiceDateInString == "01 Jan 0001")
                                    {
                                        FillCellMerge(ref sheet, "-", StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "STRING");
                                    }
                                    else
                                    {
                                        FillCellMerge(ref sheet, oItem.InvoiceDateInString, StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "STRING");
                                    }

                                    if (_oImportSummaryInvoiceTemp.Count > 0)
                                    {
                                        int StartInvRowIndex = StartRowIndex;
                                        int rowIndexBeforeRowSpan = 0;
                                        foreach (ImportSummaryRegister oInvItem in _oImportSummaryInvoiceTemp)
                                        {
                                            if (nINVDetailID != oInvItem.InvoiceDetailID)
                                            {
                                                nInvoiceDetailRowSpan = _oImportSummaryRegisters.Where(x => x.LCID == obj.LCID && x.InvoiceID == oItem.InvoiceID && x.InvoiceDetailID == oInvItem.InvoiceDetailID).ToList().Count - 1;
                                                if (nInvoiceDetailRowSpan < 0)
                                                    nInvoiceDetailRowSpan = 0;
                                                nStartCol = 19;
                                                FillCellMerge(ref sheet, oInvItem.InvoiceValue.ToString("#,##0;(#,##0)"), StartInvRowIndex, StartInvRowIndex + nInvoiceDetailRowSpan, nStartCol, nStartCol, "NUMBER");
                                                if (oInvItem.InvoiceValue != null)
                                                {
                                                    totalINValue = totalINValue + oInvItem.InvoiceValue;
                                                }
                                                rowIndexBeforeRowSpan = StartInvRowIndex;
                                                StartInvRowIndex = StartInvRowIndex + nInvoiceDetailRowSpan + 1;
                                            }
                                            nStartCol = 25;
                                            if (oInvItem.GoodReceivedDateInString == "01 Jan 0001")
                                            {
                                                FillCellMerge(ref sheet, "-", rowIndexBeforeRowSpan, rowIndexBeforeRowSpan, nStartCol, nStartCol, "DATE");
                                            }
                                            else
                                            {
                                                FillCellMerge(ref sheet, oInvItem.GoodReceivedDateInString, rowIndexBeforeRowSpan, rowIndexBeforeRowSpan, nStartCol, nStartCol, "DATE");
                                            }
                                            nStartCol = 26;
                                            FillCellMerge(ref sheet, oInvItem.GRNReceivedQty.ToString("#,##0;(#,##0)"), rowIndexBeforeRowSpan, rowIndexBeforeRowSpan, nStartCol, nStartCol, "NUMBER");
                                            if (oInvItem.GRNReceivedQty != null)
                                            {
                                                totalGoodsRcvQty = totalGoodsRcvQty + oInvItem.GRNReceivedQty;
                                            }
                                            nINVDetailID = oInvItem.InvoiceDetailID;
                                            rowIndexBeforeRowSpan++;
                                        }
                                        _oImportSummaryInvoiceTemp = new List<ImportSummaryRegister>();
                                    }
                                    nStartCol = 20;
                                    if (oItem.AcceptanceDateInString == "01 Jan 0001")
                                    {
                                        FillCellMerge(ref sheet, "-", StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "DATE");
                                    }
                                    else
                                    {
                                        FillCellMerge(ref sheet, oItem.AcceptanceDateInString, StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "DATE");
                                    }
                                    nStartCol = 21;
                                    if (oItem.MaturityDateInString == "01 Jan 0001")
                                    {
                                        FillCellMerge(ref sheet, "-", StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "DATE");
                                    }
                                    else
                                    {
                                        FillCellMerge(ref sheet, oItem.MaturityDateInString, StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "DATE");
                                    }
                                    nStartCol = 22;
                                    if (oItem.PaymentTypeSt == "None")
                                    {
                                        FillCellMerge(ref sheet, oItem.PaymentTypeSt, StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "NUMBER");
                                    }
                                    else
                                    {
                                        FillCellMerge(ref sheet, oItem.PaymentTypeSt, StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "NUMBER");
                                    }
                                    
                                    nStartCol = 23;
                                    if (oItem.PaymentDateInString == "01 Jan 0001")
                                    {
                                        FillCellMerge(ref sheet, "-", StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "DATE");
                                    }
                                    else
                                    {
                                        FillCellMerge(ref sheet, oItem.PaymentDateInString, StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "DATE");
                                    }
                                    nStartCol = 24;
                                    FillCellMerge(ref sheet, oItem.CRate.ToString("#,##0;(#,##0)"), StartRowIndex, StartRowIndex + nInvRowSpan, nStartCol, nStartCol, "NUMBER");
                                    StartRowIndex = StartRowIndex + nInvRowSpan + 1;
                                }

                                nINVID = oItem.InvoiceID;

                            }

                            _oImportSummaryLCTemp = new List<ImportSummaryRegister>();

                        }

                        nPID = obj.ImportPIID;
                        nPIDetailID = obj.ImportPIDetailID;
                        nLCID = obj.LCID;

                        if (nLCID != obj.LCID)
                        {
                            nRowIndex = nRowIndex + nPIRowSpan + 1;
                        }

                        else
                            nRowIndex++;
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right);
                    nStartCol++; _sFormatter = " #,##0;(#,##0)";
                    FillCell(sheet, nRowIndex, nStartCol, totalPIQty.ToString(), true, true);
                    nStartCol++;
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right);
                    nStartCol = 8; _sFormatter = " #,##0;(#,##0)";
                    FillCell(sheet, nRowIndex, nStartCol, totalPIAmt.ToString(), true, true);
                    nStartCol++;
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);

                    nStartCol = 15; _sFormatter = " #,##0;(#,##0)"; 
                    FillCell(sheet, nRowIndex, nStartCol, totalLCAmt.ToString(), true, true);
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                    nStartCol = 19; _sFormatter = " #,##0;(#,##0)";
                    FillCell(sheet, nRowIndex, nStartCol, totalINValue.ToString(), true, true);
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);
                    
                    nStartCol = 26; _sFormatter = " #,##0;(#,##0)";
                    FillCell(sheet, nRowIndex, nStartCol, totalGoodsRcvQty.ToString(), true, true);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 24];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ImportSummaryRegister(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal, string type)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            if (type.ToUpper() == "STRING")
            {
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }
            if (type.ToUpper() == "NUMBER")
            {
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            if (type.ToUpper() == "DATE")
            {
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string type)
        {
            if (type.ToUpper() == "STRING")
            {
                FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
            }
            if (type.ToUpper() == "NUMBER")
            {
                FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Right);
            }
            if (type.ToUpper() == "DATE")
            {
                FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Center);
            }
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];

            if (cell.Merge == false)
                cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = isBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = HoriAlign;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        #endregion

        #endregion

        #region Support Functions
        private string[] GetSQL(ImportSummaryRegister oImportSummaryRegister)
        {
            _sDateRange = "";
            string sSearchingData = oImportSummaryRegister.SearchingData;
            EnumCompareOperator eIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eDateofInvoiceDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dDateofInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dDateofInvoiceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eLCImportLCDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dLCIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dLCIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator eDateofMaturity = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            DateTime dDateofMaturityStartDate = Convert.ToDateTime(sSearchingData.Split('~')[10]);
            DateTime dDateofMaturityEndDate = Convert.ToDateTime(sSearchingData.Split('~')[11]);

            int nProductType = Convert.ToInt32(sSearchingData.Split('~')[12]);

            //string sImportLCNo = sSearchingData.Split('~')[12];
            //string sImportInvoiceNo = sSearchingData.Split('~')[13];

            string sSQLQuery = "", sGRNDSql = "", sInvoiceDSql = "", sPIDSql = "", sGRNWhereCluse = "", sInvoiceWhereCluse = "", sPIWhereCluse = "", sGroupBy = "", sOrderBy = "";
            sGRNDSql = "SELECT GRNDetailID, RefType, RefObjectID  FROM GRNDetail WHERE  RefType IN (2,3) AND GRNID IN (SELECT GRNID FROM GRN WHERE  ISNULL(ReceivedBy,0)!= 0) ";
            sInvoiceDSql = "SELECT ImportInvoiceDetailID, ImportPIDetailID  FROM ImportInvoiceDetail WHERE ImportInvoiceDetailID NOT IN (SELECT TP.InvoiceDetailID FROM #TempTable AS TP)  ";
            sPIDSql = "SELECT  ImportPIDetailID  FROM ImportPIDetail WHERE ImportPIID IN (SELECT MLC.ImportPIID FROM ImportLCDetail AS MLC )  AND ImportPIDetailID NOT IN (SELECT TP.ImportPIDetailID FROM #TempTable AS TP) ";

            string[] Sqls = new string[3];

            #region BusinessUnit
            if (oImportSummaryRegister.BUID > 0)
            {
                Global.TagSQL(ref sGRNDSql);
                sGRNDSql = sGRNDSql + " GRNID IN (SELECT GRNID FROM GRN WHERE BUID = " + oImportSummaryRegister.BUID.ToString() + ")";
                Global.TagSQL(ref sInvoiceDSql);
                sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE BUID =" + oImportSummaryRegister.BUID.ToString() + ")";
                Global.TagSQL(ref sPIDSql);
                sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE BUID = " + oImportSummaryRegister.BUID.ToString() + ")";
            }
            #endregion

            #region ImportPINo
            if (oImportSummaryRegister.ImportPINo != null && oImportSummaryRegister.ImportPINo != "")
            {
                Global.TagSQL(ref sGRNDSql);
                sGRNDSql = sGRNDSql + " GRNID IN (SELECT GRNID FROM View_GRNDetail WHERE (RefType = 6 AND RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE ImportInvoiceNo ='" + oImportSummaryRegister.ImportPINo + "') )) OR (RefType = 2 AND RefObjectID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE ImportPINo ='" + oImportSummaryRegister.ImportPINo + "') ))";
                Global.TagSQL(ref sInvoiceDSql);
                sInvoiceDSql = sInvoiceDSql + " ImportPIDetailID IN(SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE ImportPINo ='" + oImportSummaryRegister.ImportPINo + "'))";
                Global.TagSQL(ref sPIDSql);
                sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE ImportPINo LIKE '%" + oImportSummaryRegister.ImportPINo + "%')";
            }
            #endregion

            #region ImportPIType
            if (oImportSummaryRegister.ImportPIType != EnumImportPIType.None)
            {
                Global.TagSQL(ref sGRNDSql);
                sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE ImportPIType =" + (int)oImportSummaryRegister.ImportPIType + "))) ";
                Global.TagSQL(ref sInvoiceDSql);
                sInvoiceDSql = sInvoiceDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE ImportPIType = " + (int)oImportSummaryRegister.ImportPIType + ")) ";
                Global.TagSQL(ref sPIDSql);
                sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE ImportPIType =" + (int)oImportSummaryRegister.ImportPIType + ") ";
            }
            #endregion

            #region Product Type
            if (nProductType > 0)
            {
                Global.TagSQL(ref sGRNDSql);
                //sGRNDSql = sGRNDSql + " ProductID IN (SELECT ProductID FROM Product WHERE ProductType = " + nProductType + ")";
                sGRNDSql = sGRNDSql + " ProductID IN (SELECT ProductID FROM Product AS HH WHERE HH.Activity = 1 and  HH.ProductCategoryID IN (SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategoryBU](" + oImportSummaryRegister.BUID + ", " + nProductType + ")))";
                Global.TagSQL(ref sInvoiceDSql);
                //sInvoiceDSql = sInvoiceDSql + " ProductID IN (SELECT ProductID FROM Product WHERE ProductType = " + nProductType + ")";
                sInvoiceDSql = sInvoiceDSql + " ProductID IN (SELECT ProductID FROM Product AS HH WHERE HH.Activity = 1 and  HH.ProductCategoryID IN (SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategoryBU](" + oImportSummaryRegister.BUID + ", " + nProductType + ")))";
                Global.TagSQL(ref sPIDSql);
                //sPIDSql = sPIDSql + " ProductID IN (SELECT ProductID FROM Product WHERE ProductType = " + nProductType + ")";
                sPIDSql = sPIDSql + " ProductID IN (SELECT ProductID FROM Product AS HH WHERE HH.Activity = 1 and  HH.ProductCategoryID IN (SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategoryBU](" + oImportSummaryRegister.BUID + ", " + nProductType + ")))";
            }
            #endregion

            //#region RefType
            //if (oImportSummaryRegister.RefType != EnumGRNType.None)
            //{
            //    Global.TagSQL(ref sGRNDSql);
            //    sGRNDSql = sGRNDSql + " RefType = " + (int)oImportSummaryRegister.RefType;
            //    Global.TagSQL(ref sInvoiceDSql);
            //    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceDetailID IN (SELECT RefObjectID FROM GRNDetail WHERE RefType = " + (int)oImportSummaryRegister.RefType + ")";
            //    Global.TagSQL(ref sPIDSql);
            //    sPIDSql = sPIDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceDetailID IN (SELECT RefObjectID FROM GRNDetail WHERE RefType = " + (int)oImportSummaryRegister.RefType+ "))";
            //}
            //#endregion

            #region Supplier
            if (oImportSummaryRegister.SupplierIDs != null && oImportSummaryRegister.SupplierIDs != "")
            {
                Global.TagSQL(ref sGRNDSql);
                sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE SupplierID IN (" + oImportSummaryRegister.SupplierIDs + ")))) ";
                Global.TagSQL(ref sInvoiceDSql);
                sInvoiceDSql = sInvoiceDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE SupplierID IN (" + oImportSummaryRegister.SupplierIDs + ")))";
                Global.TagSQL(ref sPIDSql);
                sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE SupplierID IN (" + oImportSummaryRegister.SupplierIDs + ") ) ";

            }
            #endregion

            #region Product
            if (oImportSummaryRegister.ProductIDs != null && oImportSummaryRegister.ProductIDs != "")
            {
                Global.TagSQL(ref sGRNDSql);
                sGRNDSql = sGRNDSql + " ProductID IN(" + oImportSummaryRegister.ProductIDs + ") ";
                Global.TagSQL(ref sInvoiceDSql);
                sInvoiceDSql = sInvoiceDSql + " ProductID IN(" + oImportSummaryRegister.ProductIDs + ") ";
                Global.TagSQL(ref sPIDSql);
                sPIDSql = sPIDSql + " ProductID IN(" + oImportSummaryRegister.ProductIDs + ") ";
            }
            #endregion

            #region PI Issue Date
            if (eIssueDate != EnumCompareOperator.None)
            {
                if (eIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    _sDateRange = "PI Date @ " + dIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    //_sDateRange = "PI Date Not Equal @ " + dIssueStartDate.ToString("dd MMM yyyy");

                }
                else if (eIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    _sDateRange = "PI Date Greater Then @ " + dIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    _sDateRange = "PI Date Smaller Then @ " + dIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106)) )";
                    _sDateRange = "PI Date Between " + dIssueStartDate.ToString("dd MMM yyyy") + " To " + dIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (eIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dIssueEndDate.ToString("dd MMM yyyy") + "', 106)) )";
                    _sDateRange = "PI Date NOT Between " + dIssueStartDate.ToString("dd MMM yyyy") + " To " + dIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Invoice Date
            if (eDateofInvoiceDate != EnumCompareOperator.None)
            {
                if (eDateofInvoiceDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Invoice Date @ " + dDateofInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Invoice Date Not Equal @ " + dDateofInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Invoice Date Greater Then @ " + dDateofInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Invoice Date Smaller Then @ " + dDateofInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceEndDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Invoice Date Between " + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + " To " + dDateofInvoiceEndDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceEndDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Invoice Date NOT Between " + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + " To " + dDateofInvoiceEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region DateofMaturity
            if (eDateofMaturity != EnumCompareOperator.None)
            {
                if (eDateofMaturity == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                }
                else if (eDateofMaturity == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                }
                else if (eDateofMaturity == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                }
                else if (eDateofMaturity == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                }
                else if (eDateofMaturity == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityEndDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                }
                else if (eDateofMaturity == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityEndDate.ToString("dd MMM yyyy") + "', 106)) )";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                }
            }
            #endregion

            #region LC Issue Date
            if (eLCImportLCDate != EnumCompareOperator.None)
            {
                if (eLCImportLCDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM View_ImportInvoiceDetail WHERE ImportInvoiceID IN  (SELECT ImportInvoiceID FROM View_ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Import LC Date @ " + dLCIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM View_ImportInvoiceDetail WHERE ImportInvoiceID IN  (SELECT ImportInvoiceID FROM View_ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Import LC Date Not Equal @ " + dLCIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM View_ImportInvoiceDetail WHERE ImportInvoiceID IN  (SELECT ImportInvoiceID FROM View_ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Import LC Date Greater Then @ " + dLCIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM View_ImportInvoiceDetail WHERE ImportInvoiceID IN  (SELECT ImportInvoiceID FROM View_ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Import LC Date Smaller Then @ " + dLCIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM View_ImportInvoiceDetail WHERE ImportInvoiceID IN  (SELECT ImportInvoiceID FROM View_ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Import LC Date Between " + dLCIssueStartDate.ToString("dd MMM yyyy") + " To " + dLCIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sGRNDSql);
                    sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM View_ImportInvoiceDetail WHERE ImportInvoiceID IN  (SELECT ImportInvoiceID FROM View_ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106)) )))";
                    Global.TagSQL(ref sInvoiceDSql);
                    sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ImportLCDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    Global.TagSQL(ref sPIDSql);
                    sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106)) ))";
                    _sDateRange = "Import LC Date NOT Between " + dLCIssueStartDate.ToString("dd MMM yyyy") + " To " + dLCIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region ImportLCNo
            if (oImportSummaryRegister.LCNo != null && oImportSummaryRegister.LCNo != "")
            {
                Global.TagSQL(ref sGRNDSql);
                sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM View_ImportInvoiceDetail WHERE ImportInvoiceID IN  (SELECT ImportInvoiceID FROM View_ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE ImportLCID IN (" + oImportSummaryRegister.LCNo + " ) ))) ";
                Global.TagSQL(ref sInvoiceDSql);
                sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE ImportLCID IN (" + oImportSummaryRegister.LCNo + " ) )) ";
                Global.TagSQL(ref sPIDSql);
                sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportLC WHERE ImportLCID IN (" + oImportSummaryRegister.LCNo + " ) )) ";
            }
            #endregion

            #region ImportInvoiceNo
            if (oImportSummaryRegister.InvoiceNo != null && oImportSummaryRegister.InvoiceNo != "")
            {
                Global.TagSQL(ref sGRNDSql);
                sGRNDSql = sGRNDSql + " RefObjectID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID IN (" + oImportSummaryRegister.InvoiceNo + ") )";
                Global.TagSQL(ref sInvoiceDSql);
                sInvoiceDSql = sInvoiceDSql + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE ImportInvoiceID IN (" + oImportSummaryRegister.InvoiceNo + ") ) ";
                Global.TagSQL(ref sPIDSql);
                sPIDSql = sPIDSql + " ImportPIID IN (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID IN (SELECT ImportLCID FROM ImportInvoice WHERE ImportInvoiceID IN (" + oImportSummaryRegister.InvoiceNo + ") )) ";
            }
            #endregion

            Sqls[0] = sGRNDSql;
            Sqls[1] = sInvoiceDSql;
            Sqls[2] = sPIDSql;

            #region Report Layout
            //if (oImportSummaryRegister.ReportLayout == EnumReportLayout.PIWise)
            //{
            //    Sqls[0] = sGRNDSql;
            //    Sqls[1] = sInvoiceDSql;
            //    Sqls[2] = sPIDSql;

            //    //sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
            //    //sSQLQuery = "SELECT * FROM View_ImportSummary ";
            //    //sOrderBy = " ORDER BY  IssueDate, ImportPIID, ImportPIDetailID ASC";
            //}
            #endregion

            //sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return Sqls;
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