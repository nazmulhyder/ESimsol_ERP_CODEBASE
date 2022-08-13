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
    public class PurchaseInvoiceRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<PurchaseInvoiceRegister> _oPurchaseInvoiceRegisters = new List<PurchaseInvoiceRegister>();        

        #region Actions
        public ActionResult ViewPurchaseInvoiceRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            PurchaseInvoiceRegister oPurchaseInvoiceRegister = new PurchaseInvoiceRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM PurchaseInvoice AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
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
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PurchaseInvoiceStates = EnumObject.jGets(typeof(EnumPInvoiceStatus));
            
            return View(oPurchaseInvoiceRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(PurchaseInvoiceRegister oPurchaseInvoiceRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oPurchaseInvoiceRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPurchaseInvoiceRegister(double ts)
        {
            PurchaseInvoiceRegister oPurchaseInvoiceRegister = new PurchaseInvoiceRegister();
            try
            {
                _sErrorMesage = "";
                _oPurchaseInvoiceRegisters = new List<PurchaseInvoiceRegister>();                
                oPurchaseInvoiceRegister = (PurchaseInvoiceRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oPurchaseInvoiceRegister);
                _oPurchaseInvoiceRegisters = PurchaseInvoiceRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oPurchaseInvoiceRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oPurchaseInvoiceRegisters = new List<PurchaseInvoiceRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oPurchaseInvoiceRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptPurchaseInvoiceRegisters oReport = new rptPurchaseInvoiceRegisters();
                byte[] abytes = oReport.PrepareReport(_oPurchaseInvoiceRegisters, oCompany, oPurchaseInvoiceRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
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
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
           return FillCell( sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
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
        public void ExportToExcelPurchaseInvoiceRegister(double ts)
        {
            int InvoiceID=-999;
            string Header = "", HeaderColumn = "";

            Company oCompany = new Company();
            PurchaseInvoiceRegister oPurchaseInvoiceRegister = new PurchaseInvoiceRegister();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oPurchaseInvoiceRegisters = new List<PurchaseInvoiceRegister>();
                oPurchaseInvoiceRegister = (PurchaseInvoiceRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oPurchaseInvoiceRegister);
                _oPurchaseInvoiceRegisters = PurchaseInvoiceRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oPurchaseInvoiceRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oPurchaseInvoiceRegisters = new List<PurchaseInvoiceRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "") 
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Invoice No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Invoice Status", Width = 15f, IsRotate = false });
                
                if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                    table_header.Add(new TableHeader { Header = "Supplier Name", Width = 45f, IsRotate = false });
                else
                    table_header.Add(new TableHeader { Header = "Invoice Date", Width = 25f, IsRotate = false });
               
                table_header.Add(new TableHeader { Header = "Date Of Bill", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Approved By Name", Width = 25f, IsRotate = false });

                if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    table_header.Add(new TableHeader { Header = "Supplier Name", Width = 45f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Payment Mode", Width = 20f, IsRotate = false });
                }
                else
                {
                    table_header.Add(new TableHeader { Header = "Product Code", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Product Name", Width = 45f, IsRotate = false });
                }

                table_header.Add(new TableHeader { Header = "M Unit", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Rate", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount", Width = 25f, IsRotate = false });
                #endregion

                #region Layout Wise Header
                if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.ProductWise) 
                {
                    Header = "Product Wise"; HeaderColumn="Product Name : "; 
                }
                else if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
                {
                    Header = "Party Wise"; HeaderColumn="Party Name : "; 
                }
                else if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                {
                    Header = "Date Wise"; HeaderColumn="Invoice Date : ";
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("Purchase Invoice Register");
                    sheet.Name = "Purchase Invoice Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Purchase Invoice Register (" + Header+") "; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Group By Layout Wise
                    var data = _oPurchaseInvoiceRegisters.GroupBy(x => new { x.SupplierName, x.ContractorID}, (key, grp) => new
                    {
                        HeaderName = key.SupplierName,
                        TotalQty = grp.Sum(x => x.Qty),
                        TotalAmount= grp.Sum(x => x.Amount),
                        Results = grp.ToList()
                    });

                    if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.ProductWise)
                    {
                        data = _oPurchaseInvoiceRegisters.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
                        {
                            HeaderName = key.ProductName,
                            TotalQty = grp.Sum(x => x.Qty),
                            TotalAmount = grp.Sum(x => x.Amount),
                            Results = grp.ToList()
                        });
                    }
                    else if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                    {
                        data = _oPurchaseInvoiceRegisters.GroupBy(x => new { x.DateofInvoiceSt }, (key, grp) => new
                        {
                            HeaderName = key.DateofInvoiceSt,
                            TotalQty = grp.Sum(x => x.Qty),
                            TotalAmount = grp.Sum(x => x.Amount),
                            Results = grp.ToList()
                        });
                    }
                    #endregion

                    string sCurrencySymbol = "";
                    #region Data
                    foreach (var oItem in data)
                    {
                        nRowIndex++;

                        nStartCol = 2;
                        FillCellMerge(ref sheet, HeaderColumn + oItem.HeaderName, nRowIndex, nRowIndex, nStartCol, nEndCol+1, true, ExcelHorizontalAlignment.Left);

                        nRowIndex++;
                        foreach (TableHeader listItem in table_header)
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false; 
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        InvoiceID = 0; 
                        nRowIndex++; int nCount = 0, nRowSpan=0;
                        foreach (var obj in oItem.Results)
                        {
                            #region Invoice Wise Merge
                            if (InvoiceID != obj.PurchaseInvoiceID) 
                            {
                                if (nCount > 0) 
                                {
                                    nStartCol = 8;
                                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                                    _sFormatter = " #,##0;(#,##0)";
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.PurchaseInvoiceID == InvoiceID).Sum(x => x.Qty).ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);

                                    _sFormatter = obj.CurrencySymbol + " #,##0.00;(#,##0.00)";
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.PurchaseInvoiceID == InvoiceID).Sum(x => x.Amount).ToString(), true, true);
                                    nRowIndex++;
                                }

                                nStartCol = 2;
                                nRowSpan = oItem.Results.Where(InvoiceR => InvoiceR.PurchaseInvoiceID == obj.PurchaseInvoiceID).ToList().Count;

                                FillCellMerge(ref sheet,(++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.PurchaseInvoiceNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.InvoiceStatus.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                
                                if (oPurchaseInvoiceRegister.ReportLayout != EnumReportLayout.DateWiseDetails)
                                    FillCellMerge(ref sheet, obj.SupplierName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                else
                                    FillCellMerge(ref sheet, obj.DateofInvoiceSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                FillCellMerge(ref sheet, obj.DateofBillSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.ApprovedByName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            }
                            #endregion

                            nStartCol = 8;

                            if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.ProductWise) 
                            { 
                                FillCell( sheet, nRowIndex, nStartCol++, obj.SupplierName, false);
                                FillCell(sheet, nRowIndex, nStartCol++, obj.InvoicePaymentMode.ToString(), false);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ProductCode, false);
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                            }

                            FillCell(sheet, nRowIndex, nStartCol++, obj.MUName, false);
                            _sFormatter = " #,##0;(#,##0)";
                            FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true);
                            _sFormatter = obj.CurrencySymbol + " #,##0.00;(#,##0.00)";
                            FillCell(sheet, nRowIndex, nStartCol++, obj.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.Amount.ToString(), true);
                            nRowIndex++;

                            InvoiceID = obj.PurchaseInvoiceID;
                            sCurrencySymbol = obj.CurrencySymbol;
                        }

                        nStartCol = 8;
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.PurchaseInvoiceID == InvoiceID).Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.PurchaseInvoiceID == InvoiceID).Sum(x => x.Amount).ToString(), true, true);
                        nRowIndex++;

                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false, true); _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalAmount.ToString(), true, true);
                        nRowIndex++;
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalQty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalAmount).ToString(), true, true);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 13];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion
                    
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=PurchaseInvoiceRegister(" + Header + ").xlsx");
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

        #region Support Functions
        private string GetSQL(PurchaseInvoiceRegister oPurchaseInvoiceRegister)
        {
            _sDateRange = "";
            string sSearchingData = oPurchaseInvoiceRegister.SearchingData;
            EnumCompareOperator eDateofInvoice = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPurchaseInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPurchaseInvoiceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eDateofBill = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dDateofBillStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dDateofBillEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eApprovedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator ePIAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            double nPIAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
            double nPIAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oPurchaseInvoiceRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oPurchaseInvoiceRegister.BUID.ToString();
            }
            #endregion

            #region PurchaseInvoiceNo
            if (oPurchaseInvoiceRegister.PurchaseInvoiceNo != null && oPurchaseInvoiceRegister.PurchaseInvoiceNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PurchaseInvoiceNo LIKE'%" + oPurchaseInvoiceRegister.PurchaseInvoiceNo + "%'";
            }
            #endregion

            #region BillNo
            if (oPurchaseInvoiceRegister.BillNo != null && oPurchaseInvoiceRegister.BillNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BillNo LIKE'%" + oPurchaseInvoiceRegister.BillNo + "%'";
            }
            #endregion

            #region ApprovedBy
            if (oPurchaseInvoiceRegister.ApprovedBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApprovedBy =" + oPurchaseInvoiceRegister.ApprovedBy.ToString();
            }
            #endregion

            #region PurchaseInvoiceStatus
            if ((int)oPurchaseInvoiceRegister.InvoiceStatus>=0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceStatus =" + ((int)oPurchaseInvoiceRegister.InvoiceStatus).ToString();
            }
            #endregion

            #region Remarks
            if (oPurchaseInvoiceRegister.Remarks != null && oPurchaseInvoiceRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Remarks LIKE'%" + oPurchaseInvoiceRegister.Remarks + "%'";
            }
            #endregion

            #region Supplier
            if (oPurchaseInvoiceRegister.SupplierName != null && oPurchaseInvoiceRegister.SupplierName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ContractorID IN(" + oPurchaseInvoiceRegister.SupplierName + ")";
            }
            #endregion

            #region Product
            if (oPurchaseInvoiceRegister.ProductName != null && oPurchaseInvoiceRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oPurchaseInvoiceRegister.ProductName + ")";
            }
            #endregion

            #region Issue Date
            if (eDateofInvoice != EnumCompareOperator.None)
            {
                if (eDateofInvoice == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date @ " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Not Equal @ " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Greater Then @ " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Smaller Then @ " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Between " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseInvoiceEndDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoice == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date NOT Between " + dPurchaseInvoiceStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseInvoiceEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region DateofBill Date
            if (eDateofBill != EnumCompareOperator.None)
            {
                if (eDateofBill == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfBill,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofBillStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofBill == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfBill,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofBillStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofBill == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfBill,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofBillStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofBill == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfBill,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofBillStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofBill == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfBill,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofBillStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofBillEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofBill == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfBill,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofBillStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofBillEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Approved Date
            if (eApprovedDate != EnumCompareOperator.None)
            {
                if (eApprovedDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApprovedDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Amount
            if (ePIAmount != EnumCompareOperator.None)
            {
                if (ePIAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount = " + nPIAmountStsrt.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount != " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount > " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount < " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount NOT BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
            }
            #endregion

            #region Report Layout
           if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PurchaseInvoiceRegister ";
                sOrderBy = " ORDER BY  DateofInvoice, PurchaseInvoiceID, PurchaseInvoiceDetailID ASC";
            }
            else if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PurchaseInvoiceRegister ";
                sOrderBy = " ORDER BY  SupplierName, PurchaseInvoiceID, PurchaseInvoiceDetailID ASC";
            }
            else if (oPurchaseInvoiceRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
               sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
               sSQLQuery = "SELECT * FROM View_PurchaseInvoiceRegister ";
               sOrderBy = " ORDER BY  ProductName,ProductID, PurchaseInvoiceID, PurchaseInvoiceDetailID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PurchaseInvoiceRegister ";
                sOrderBy = " ORDER BY DateofInvoice, PurchaseInvoiceID, PurchaseInvoiceDetailID ASC";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
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

